using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;
using Promotion.Core.Validation;

namespace Promotion.Core;

public sealed partial class PromotionCoreService
{
    private static WalletInstructionDto Instruction(string taskId, WalletAction action, long userUid, decimal amount) =>
        new(taskId + ":" + action, action, userUid, taskId, amount);

    public PromotionResult<ClaimPromotionData> ClaimPromotion(ClaimPromotionRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<ClaimPromotionData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        var now = OperationTime();
        var day = BusinessDay(now);
        return Transactional(correlation, tx =>
        {
            var status = tx.GetOrCreateBonusStatusForUpdate(request.UserUID, now);
            var entry = tx.GetEligibilityEntryForUpdate(request.EligibilityEntryId);
            if (entry is null) return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.EligibilityNotFound);
            if (entry.UserUID != request.UserUID)
                return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.EligibilityPlayerMismatch);
            if (entry.Status == EligibilityStatus.Claimed) return ReplayClaim(tx, entry, status, correlation);
            if (entry.Status == EligibilityStatus.Expired || entry.BusinessDay < day)
                return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.EligibilityExpired);
            if (entry.Status == EligibilityStatus.Excluded)
                return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.EligibilityExcluded);
            if (status.TaskState != BonusTaskState.None)
                return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.ActiveBonusTaskExists);
            var activity = tx.GetActivity(entry.ActivityUID);
            if (activity is null) return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.ActivityNotFound);
            if (tx.CountClaimedEligibility(request.UserUID, entry.ActivityUID, day) >= activity.DailyClaimLimit)
                return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.DailyClaimLimitReached);
            var snapshot = ActivitySnapshotFactory.Capture(activity, now);
            var snapshotJson = ActivitySnapshotSerializer.Serialize(snapshot);
            var bonus = PromotionAmountCalculator.Bonus(snapshot, entry.EligibleDepositAmount);
            var required = PromotionAmountCalculator.RequiredWager(snapshot, bonus, entry.EligibleDepositAmount);
            var taskId = guids.NewGuid().ToString("D");
            tx.MarkEligibilityClaimed(entry.EligibilityEntryId, taskId, now);
            tx.SaveBonusStatus(status with
            {
                TaskState = BonusTaskState.InProgress, BonusTaskId = taskId,
                EligibilityEntryId = entry.EligibilityEntryId, ActivityUID = entry.ActivityUID,
                BusinessDay = entry.BusinessDay, ActivitySnapshotJson = snapshotJson,
                BonusAmount = bonus, RequiredWagerAmount = required, CurrentWagerAmount = 0,
                ClaimedAt = now, UpdatedAt = now
            });
            tx.ExcludeEligibilityEntries(request.UserUID, day, entry.EligibilityEntryId, taskId,
                activity.IsNonStackable, activity.ExclusiveGroup, now);
            return PromotionResultFactory.Success(correlation, new ClaimPromotionData(request.UserUID,
                entry.EligibilityEntryId, taskId, entry.ActivityUID, entry.BusinessDay, bonus,
                required, 0, required, snapshot.MaxBetAmount, now, snapshot.SnapshotVersion,
                false, Freeze(new[] { Instruction(taskId, WalletAction.CreditBonusWallet, request.UserUID, bonus) })));
        }, () => VerifyClaim(request, correlation, now),
            retryDuplicateConstraint: "EligibilityEntry.UK_EligibilityEntry_ClaimedBonusTaskId",
            retryDuplicateConstraint2: "PromotionBonusStatus.UK_PromotionBonusStatus_BonusTaskId");
    }

    private PromotionResult<ClaimPromotionData> ReplayClaim(IPromotionDataTransaction tx,
        EligibilityEntry entry, BonusStatus status, string correlation)
    {
        var taskId = entry.ClaimedBonusTaskId!;
        if (status.TaskState == BonusTaskState.InProgress && status.BonusTaskId == taskId)
        {
            var snapshot = ActivitySnapshotSerializer.Deserialize(status.ActivitySnapshotJson!);
            return PromotionResultFactory.Success(correlation, new ClaimPromotionData(entry.UserUID,
                entry.EligibilityEntryId, taskId, entry.ActivityUID, entry.BusinessDay, status.BonusAmount,
                status.RequiredWagerAmount, status.CurrentWagerAmount,
                PromotionAmountCalculator.RemainingWager(status.RequiredWagerAmount, status.CurrentWagerAmount),
                snapshot.MaxBetAmount, status.ClaimedAt!.Value, snapshot.SnapshotVersion, true,
                Freeze(new[] { Instruction(taskId, WalletAction.CreditBonusWallet, entry.UserUID, status.BonusAmount) })));
        }
        var history = tx.GetBonusHistoryByTaskId(taskId);
        if (history is null)
            return PromotionResultFactory.Failure<ClaimPromotionData>(correlation,
                PromotionErrorCode.ClaimReplayDataUnavailable,
                new PromotionErrorDetailsDto(null, null, null, null, null, taskId));
        if (history.UserUID != entry.UserUID || history.EligibilityEntryId != entry.EligibilityEntryId)
            return PromotionResultFactory.Failure<ClaimPromotionData>(correlation, PromotionErrorCode.DataCorruption);
        var captured = ActivitySnapshotSerializer.Deserialize(history.ActivitySnapshotJson);
        return PromotionResultFactory.Success(correlation, new ClaimPromotionData(entry.UserUID,
            entry.EligibilityEntryId, taskId, entry.ActivityUID, entry.BusinessDay, history.BonusAmount,
            history.RequiredWagerAmount, 0, history.RequiredWagerAmount, captured.MaxBetAmount,
            history.ClaimedAt, captured.SnapshotVersion, true,
            Freeze(new[] { Instruction(taskId, WalletAction.CreditBonusWallet, entry.UserUID, history.BonusAmount) })));
    }

    private PromotionResult<ClaimPromotionData>? VerifyClaim(ClaimPromotionRequest request, string correlation, DateTime now)
    {
        return data.ExecuteInTransaction(tx =>
        {
            var status = tx.GetOrCreateBonusStatusForUpdate(request.UserUID, now);
            var entry = tx.GetEligibilityEntryForUpdate(request.EligibilityEntryId);
            if (entry is null || entry.Status == EligibilityStatus.Available)
            {
                if (status.TaskState == BonusTaskState.None) return null;
                throw new InvalidOperationException("Claim outcome cannot be determined.");
            }
            return entry.Status == EligibilityStatus.Claimed ? ReplayClaim(tx, entry, status, correlation)
                : throw new InvalidOperationException("Claim outcome cannot be determined.");
        });
    }

    public PromotionResult<AccumulateWagerData> AccumulateWager(AccumulateWagerRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<AccumulateWagerData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        var now = OperationTime();
        return Transactional(correlation, tx =>
        {
            var status = tx.GetOrCreateBonusStatusForUpdate(request.UserUID, now);
            if (status.TaskState == BonusTaskState.None)
            {
                var old = tx.GetBonusHistoryByTaskId(request.ExpectedBonusTaskId);
                if (old is not null && old.UserUID == request.UserUID)
                    return PromotionResultFactory.Success(correlation, new AccumulateWagerData(request.UserUID,
                        old.BonusTaskId, 0, old.CurrentWagerAmount, 0, BonusTaskOutcome.AlreadyClosed,
                        old.CloseReason, old.ConvertedAmount, old.ClosedAt, Array.Empty<WalletInstructionDto>()));
                return PromotionResultFactory.Failure<AccumulateWagerData>(correlation, PromotionErrorCode.BonusTaskNotFound);
            }
            if (status.BonusTaskId != request.ExpectedBonusTaskId)
            {
                var old = tx.GetBonusHistoryByTaskId(request.ExpectedBonusTaskId);
                if (old is not null && old.UserUID == request.UserUID)
                    return PromotionResultFactory.Success(correlation, new AccumulateWagerData(request.UserUID,
                        old.BonusTaskId, 0, old.CurrentWagerAmount, 0, BonusTaskOutcome.AlreadyClosed,
                        old.CloseReason, old.ConvertedAmount, old.ClosedAt, Array.Empty<WalletInstructionDto>()));
                return PromotionResultFactory.Failure<AccumulateWagerData>(correlation, PromotionErrorCode.BonusTaskMismatch);
            }
            var snapshot = ActivitySnapshotSerializer.Deserialize(status.ActivitySnapshotJson!);
            var effective = PromotionAmountCalculator.EffectiveWager(request.GameBetAmount, snapshot.WagerContributionRate);
            var updated = status with { CurrentWagerAmount = checked(status.CurrentWagerAmount + effective), UpdatedAt = now };
            var reached = updated.CurrentWagerAmount >= updated.RequiredWagerAmount;
            tx.SaveBonusStatus(updated);
            return PromotionResultFactory.Success(correlation, new AccumulateWagerData(request.UserUID,
                status.BonusTaskId!, effective, updated.CurrentWagerAmount,
                PromotionAmountCalculator.RemainingWager(updated.RequiredWagerAmount, updated.CurrentWagerAmount),
                reached ? BonusTaskOutcome.ReadyToClose : BonusTaskOutcome.InProgress,
                null, 0, null, Array.Empty<WalletInstructionDto>()));
        }); // No success verifier: a game round has no database idempotency key.
    }

    public PromotionResult<CloseBonusTaskData> CloseWagerCompletedBonusTask(CloseWagerCompletedBonusTaskRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<CloseBonusTaskData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        var now = OperationTime();
        return Transactional(correlation, tx =>
        {
            var status = tx.GetOrCreateBonusStatusForUpdate(request.UserUID, now);
            if (status.TaskState == BonusTaskState.None)
            {
                var existing = tx.GetBonusHistoryByTaskId(request.ExpectedBonusTaskId);
                return existing is null || existing.UserUID != request.UserUID
                    ? PromotionResultFactory.Failure<CloseBonusTaskData>(correlation, PromotionErrorCode.BonusTaskNotFound)
                    : PromotionResultFactory.Success(correlation, CloseData(existing, true, false));
            }
            if (status.BonusTaskId != request.ExpectedBonusTaskId)
            {
                var old = tx.GetBonusHistoryByTaskId(request.ExpectedBonusTaskId);
                return old is not null && old.UserUID == request.UserUID
                    ? PromotionResultFactory.Success(correlation, CloseData(old, true, false))
                    : PromotionResultFactory.Failure<CloseBonusTaskData>(correlation, PromotionErrorCode.BonusTaskMismatch);
            }
            if (status.CurrentWagerAmount < status.RequiredWagerAmount)
                return PromotionResultFactory.Failure<CloseBonusTaskData>(correlation,
                    PromotionErrorCode.WagerRequirementNotMet);
            var closed = Close(tx, status, CloseReason.WagerCompleted, now,
                request.SettledBonusWalletBalance, request.GameBetAmount, request.GameWinAmount, request.GameTime);
            return PromotionResultFactory.Success(correlation, CloseData(closed.History, false, false));
        }, () => VerifyClosed(request.UserUID, request.ExpectedBonusTaskId, correlation, false));
    }

    public PromotionResult<CloseBonusTaskData> CloseDepletedBonusTask(CloseDepletedBonusTaskRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<CloseBonusTaskData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        if (request.SettledBonusWalletBalance != 0)
            return PromotionResultFactory.Failure<CloseBonusTaskData>(correlation, PromotionErrorCode.BonusWalletNotDepleted);
        return CloseExplicit(request.UserUID, request.ExpectedBonusTaskId, request.NotificationTime, correlation,
            (status, snapshot) => status.CurrentWagerAmount >= status.RequiredWagerAmount &&
                snapshot.ConvertType == ConvertType.Fixed ? CloseReason.WagerCompleted : CloseReason.BonusDepleted,
            omitClearInstruction: true);
    }

    public PromotionResult<CloseBonusTaskData> AbandonBonusTask(AbandonBonusTaskRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<CloseBonusTaskData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        return CloseExplicit(request.UserUID, request.ExpectedBonusTaskId, request.RequestedAt, correlation,
            (_, _) => CloseReason.PlayerAbandoned);
    }

    private PromotionResult<CloseBonusTaskData> CloseExplicit(long userUid, string expectedTaskId,
        DateTime time, string correlation, Func<BonusStatus, ActivitySnapshotDto, CloseReason> chooseReason,
        bool omitClearInstruction = false) =>
        Transactional(correlation, tx =>
        {
            var status = tx.GetOrCreateBonusStatusForUpdate(userUid, time);
            if (status.TaskState == BonusTaskState.None)
            {
                var existing = tx.GetBonusHistoryByTaskId(expectedTaskId);
                return existing is null || existing.UserUID != userUid
                    ? PromotionResultFactory.Failure<CloseBonusTaskData>(correlation, PromotionErrorCode.BonusTaskNotFound)
                    : PromotionResultFactory.Success(correlation, CloseData(existing, true, omitClearInstruction));
            }
            if (status.BonusTaskId != expectedTaskId)
            {
                var old = tx.GetBonusHistoryByTaskId(expectedTaskId);
                return old is not null && old.UserUID == userUid
                    ? PromotionResultFactory.Success(correlation, CloseData(old, true, omitClearInstruction))
                    : PromotionResultFactory.Failure<CloseBonusTaskData>(correlation, PromotionErrorCode.BonusTaskMismatch);
            }
            var snapshot = ActivitySnapshotSerializer.Deserialize(status.ActivitySnapshotJson!);
            var closed = Close(tx, status, chooseReason(status, snapshot), time, 0, 0, 0, time);
            return PromotionResultFactory.Success(correlation, CloseData(closed.History, false, omitClearInstruction));
        }, () => VerifyClosed(userUid, expectedTaskId, correlation, omitClearInstruction));

    private PromotionResult<CloseBonusTaskData>? VerifyClosed(long userUid, string taskId, string correlation,
        bool omitClearInstruction)
    {
        var history = data.ExecuteInTransaction(tx => tx.GetBonusHistoryByTaskId(taskId));
        if (history is not null)
        {
            if (history.UserUID != userUid) throw new InvalidOperationException("History player mismatch.");
            return PromotionResultFactory.Success(correlation, CloseData(history, true, omitClearInstruction));
        }
        var status = data.GetBonusStatus(userUid);
        if (status?.TaskState == BonusTaskState.InProgress && status.BonusTaskId == taskId) return null;
        throw new InvalidOperationException("Close outcome cannot be determined.");
    }

    private static CloseBonusTaskData CloseData(BonusHistory history, bool replay, bool omitClearInstruction) =>
        new(history.UserUID, history.BonusTaskId, history.CloseReason, history.CurrentWagerAmount,
            history.ConvertedAmount, history.ClosedAt, replay, Freeze(CloseInstructions(history)
                .Where(x => !omitClearInstruction || x.Action != WalletAction.ClearBonusWallet)));

    private static IReadOnlyList<WalletInstructionDto> CloseInstructions(BonusHistory history)
    {
        var result = new List<WalletInstructionDto>();
        if (history.CloseReason != CloseReason.BonusDepleted)
            result.Add(Instruction(history.BonusTaskId, WalletAction.ClearBonusWallet, history.UserUID, 0));
        if (history.ConvertedAmount > 0)
            result.Add(Instruction(history.BonusTaskId, WalletAction.CreditMainWallet, history.UserUID, history.ConvertedAmount));
        return Freeze(result);
    }

    private (BonusHistory History, IReadOnlyList<WalletInstructionDto> Instructions) Close(
        IPromotionDataTransaction tx, BonusStatus status, CloseReason reason, DateTime closedAt,
        decimal balance, decimal bet, decimal win, DateTime gameTime)
    {
        if (tx.GetBonusHistoryByTaskId(status.BonusTaskId!) is not null)
            throw new DomainInvariantException("Active bonus task already has closing history.");
        tx.SaveBonusStatus(status with { TaskState = BonusTaskState.Closing, UpdatedAt = closedAt });
        var snapshot = ActivitySnapshotSerializer.Deserialize(status.ActivitySnapshotJson!);
        decimal converted = 0;
        if (reason == CloseReason.WagerCompleted)
        {
            var context = new BalanceConvertContext(status.UserUID, status.BonusTaskId!,
                status.EligibilityEntryId!.Value, status.ActivityUID!.Value, status.BusinessDay!.Value,
                status.BonusAmount, status.RequiredWagerAmount, status.CurrentWagerAmount,
                PromotionAmountCalculator.RemainingWager(status.RequiredWagerAmount, status.CurrentWagerAmount),
                balance, bet, win, gameTime, snapshot);
            converted = ConvertedAmountCalculator.Calculate(snapshot, balanceFormula, context);
        }
        var history = tx.InsertBonusHistory(new NewBonusHistory(status.BonusTaskId!, status.UserUID,
            status.EligibilityEntryId!.Value, status.ActivityUID!.Value, status.BusinessDay!.Value,
            status.ActivitySnapshotJson!, status.BonusAmount, status.RequiredWagerAmount,
            status.CurrentWagerAmount, converted, reason, status.ClaimedAt!.Value, closedAt, closedAt));
        tx.SaveBonusStatus(DomainInvariantValidator.ResetBonusStatus(status, closedAt));
        return (history, CloseInstructions(history));
    }
}
