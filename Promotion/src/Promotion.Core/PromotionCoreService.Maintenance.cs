using Promotion.Core.Contracts;
using Promotion.Core.Domain;
using Promotion.Core.Validation;

namespace Promotion.Core;

public sealed partial class PromotionCoreService
{
    private sealed record MaintenanceCloseData(BonusHistory? History,
        IReadOnlyList<WalletInstructionDto> Instructions);

    public PromotionResult<DailyMaintenanceData> RunDailyMaintenance(DailyMaintenanceRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<DailyMaintenanceData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        var day = BusinessDay(request.ExecutionTime);
        var cutoff = request.ExecutionTime.AddDays(-90);
        var failures = new List<MaintenanceFailureDto>();
        var instructions = new List<WalletInstructionDto>();
        var expired = 0;
        var closed = 0;
        var deleted = 0;
        var cursor = 0L;
        while (true)
        {
            IReadOnlyList<long> users;
            try { users = data.GetUsersWithExpiredEligibility(day, cursor, request.BatchSize); }
            catch (Exception ex)
            {
                failures.Add(MaintenanceFailure(MaintenanceStage.ExpireEligibility, null, null, ex, correlation));
                break;
            }
            if (users.Count == 0) break;
            foreach (var userUid in users)
            {
                try
                {
                    expired += data.ExecuteInTransaction(tx =>
                    {
                        tx.GetOrCreateBonusStatusForUpdate(userUid, request.ExecutionTime);
                        return tx.ExpireAvailableEligibilityEntries(userUid, day, request.ExecutionTime);
                    });
                }
                catch (Exception ex)
                { failures.Add(MaintenanceFailure(MaintenanceStage.ExpireEligibility, userUid, null, ex, correlation)); }
                cursor = userUid;
            }
            if (users.Count < request.BatchSize) break;
        }
        cursor = 0;
        while (true)
        {
            IReadOnlyList<long> users;
            try { users = data.GetUsersWithExpiredTasks(day, cursor, request.BatchSize); }
            catch (Exception ex)
            {
                failures.Add(MaintenanceFailure(MaintenanceStage.CloseExpiredTask, null, null, ex, correlation));
                break;
            }
            if (users.Count == 0) break;
            foreach (var userUid in users)
            {
                try
                {
                    string? expectedTaskId = null;
                    var result = Transactional(correlation, tx =>
                    {
                        var status = tx.GetOrCreateBonusStatusForUpdate(userUid, request.ExecutionTime);
                        if (status.TaskState != BonusTaskState.InProgress || status.BusinessDay >= day)
                            return PromotionResultFactory.Success(correlation,
                                new MaintenanceCloseData(null, Array.Empty<WalletInstructionDto>()));
                        expectedTaskId = status.BonusTaskId;
                        var outcome = Close(tx, status, CloseReason.BusinessDayExpired,
                            request.ExecutionTime, 0, 0, 0, request.ExecutionTime);
                        return PromotionResultFactory.Success(correlation,
                            new MaintenanceCloseData(outcome.History, outcome.Instructions));
                    }, () => VerifyMaintenanceClose(userUid, expectedTaskId, correlation));
                    if (result.Kind != PromotionResultKind.Succeeded)
                    {
                        failures.Add(new MaintenanceFailureDto(MaintenanceStage.CloseExpiredTask,
                            userUid, expectedTaskId, result.ErrorCode, result.IsRetryable));
                    }
                    else if (result.Data!.History is not null)
                    {
                        closed++;
                        instructions.AddRange(result.Data.Instructions);
                    }
                }
                catch (Exception ex)
                { failures.Add(MaintenanceFailure(MaintenanceStage.CloseExpiredTask, userUid, null, ex, correlation)); }
                cursor = userUid;
            }
            if (users.Count < request.BatchSize) break;
        }
        while (true)
        {
            try
            {
                var count = data.DeleteExpiredBonusHistory(cutoff, request.BatchSize);
                deleted += count;
                if (count < request.BatchSize) break;
            }
            catch (Exception ex)
            {
                failures.Add(MaintenanceFailure(MaintenanceStage.DeleteHistory, null, null, ex, correlation));
                break;
            }
        }
        var ordered = instructions.OrderBy(x => x.UserUID).ThenBy(x => x.BonusTaskId, StringComparer.Ordinal)
            .ThenBy(x => x.Action).DistinctBy(x => x.OperationKey).ToArray();
        var resultData = new DailyMaintenanceData(day, cutoff, expired, closed, deleted,
            Freeze(ordered), Freeze(failures));
        if (failures.Count == 0) return PromotionResultFactory.Success(correlation, resultData);
        if (expired + closed + deleted > 0) return PromotionResultFactory.Partial(correlation, resultData);
        var first = PromotionResultFactory.Failure<DailyMaintenanceData>(correlation, failures[0].ErrorCode);
        return first with { Data = resultData };
    }

    private PromotionResult<MaintenanceCloseData>? VerifyMaintenanceClose(long userUid,
        string? taskId, string correlation)
    {
        if (taskId is null) throw new InvalidOperationException("Maintenance task identity is missing.");
        var history = data.ExecuteInTransaction(tx => tx.GetBonusHistoryByTaskId(taskId));
        if (history is not null)
        {
            if (history.UserUID != userUid || history.CloseReason != CloseReason.BusinessDayExpired)
                throw new InvalidOperationException("Maintenance history does not match the requested close.");
            return PromotionResultFactory.Success(correlation,
                new MaintenanceCloseData(history, CloseInstructions(history)));
        }
        var status = data.GetBonusStatus(userUid);
        if (status?.TaskState == BonusTaskState.InProgress && status.BonusTaskId == taskId) return null;
        throw new InvalidOperationException("Maintenance close outcome cannot be determined.");
    }

    private static MaintenanceFailureDto MaintenanceFailure(MaintenanceStage stage, long? userUid,
        string? taskId, Exception ex, string correlation)
    {
        var result = FailureFrom<object>(correlation, ex);
        return new MaintenanceFailureDto(stage, userUid, taskId, result.ErrorCode, result.IsRetryable);
    }
}
