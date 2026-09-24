using Promotion.Core.Contracts;

namespace Promotion.Core.Domain;

public static class DomainInvariantValidator
{
    public static void ValidateActivity(PromotionActivity value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.ActivityUID <= 0 || value.ActivityInfo?.Length > 100 ||
            !Enum.IsDefined(value.BonusType) || !Enum.IsDefined(value.ConvertType) ||
            !Enum.IsDefined(value.WagerCalculationType) ||
            !Enum.IsDefined(value.ActivityStatus) ||
            !Enum.IsDefined(value.TriggerType) ||
            !Between(value.FixedBonusAmount, 1, 100) || !Between(value.MaxBonusAmount, 1, 100) ||
            !Between(value.DepositPercentage, 1, 1000) ||
            !Between(value.WagerMultiplier, 1, 100) ||
            value.MaxBetAmount is { } maxBet && !Between(maxBet, 1, 100) ||
            value.GameServerList?.Length > 500 ||
            value.WagerContributionRate is < 1m or > 100m ||
            !Between(value.FixedConvertedAmount, 1, 1000) ||
            value.MaxBalanceConvertedAmount is { } maxBalance && !Between(maxBalance, 1, 1000) ||
            !Between(value.DailyClaimLimit, 1, 99) ||
            value.WagerContributionRate != PromotionAmountCalculator.Truncate4(value.WagerContributionRate) ||
            value.WeekdayMask is null || value.WeekdayMask.Length != 7 ||
            value.WeekdayMask.Any(c => c is not '0' and not '1') ||
            value.ExclusiveGroup?.Length > 64 ||
            value.StartDate is { } start && value.EndDate is { } end && start >= end ||
            value.CreatedAt.Kind == DateTimeKind.Utc || value.UpdatedAt.Kind == DateTimeKind.Utc ||
            value.MinimumDepositAmount is { } minimum && minimum <= 0 ||
            value.TriggerType is TriggerType.Deposit or TriggerType.FirstDepositOfBusinessDay && value.MinimumDepositAmount is null ||
            value.TriggerType == TriggerType.Free && value.MinimumDepositAmount is not null)
            throw new DomainInvariantException("Invalid PromotionActivity.");
    }

    public static void ValidateEligibility(EligibilityEntry value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.EligibilityEntryId <= 0 || value.UserUID <= 0 || value.ActivityUID <= 0 ||
            string.IsNullOrEmpty(value.EventId) || !Enum.IsDefined(value.Status) ||
            !Enum.IsDefined(value.TriggerType) ||
            value.Status == EligibilityStatus.Claimed != (value.ClaimedBonusTaskId is not null) ||
            value.Status != EligibilityStatus.Excluded && value.ExcludedByBonusTaskId is not null ||
            value.ClaimedBonusTaskId is not null && !IsGuid(value.ClaimedBonusTaskId) ||
            value.ExcludedByBonusTaskId is not null && !IsGuid(value.ExcludedByBonusTaskId))
            throw new DomainInvariantException("Invalid EligibilityEntry.");
    }

    public static void ValidateTriggerEvent(PromotionTriggerEvent value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var requiresDeposit = value.TriggerType is TriggerType.Deposit or TriggerType.FirstDepositOfBusinessDay;
        if (value.EventId is null || value.EventId.Length is < 1 or > 128 || value.EventId != value.EventId.Trim() ||
            value.UserUID <= 0 || !Enum.IsDefined(value.TriggerType) ||
            value.EventTime.Kind == DateTimeKind.Utc || value.CreatedAt.Kind == DateTimeKind.Utc ||
            requiresDeposit && value.EligibleDepositAmount is null or <= 0 ||
            !requiresDeposit && value.EligibleDepositAmount is not null)
            throw new DomainInvariantException("Invalid PromotionTriggerEvent.");
    }

    public static EligibilityEntry TransitionEligibility(EligibilityEntry value, EligibilityStatus target,
        DateTime operationTime, string? bonusTaskId = null)
    {
        ValidateEligibility(value);
        if (value.Status != EligibilityStatus.Available ||
            target is not (EligibilityStatus.Claimed or EligibilityStatus.Excluded or EligibilityStatus.Expired) ||
            target == EligibilityStatus.Claimed && !IsGuid(bonusTaskId) ||
            target == EligibilityStatus.Expired && bonusTaskId is not null ||
            target == EligibilityStatus.Excluded && bonusTaskId is not null && !IsGuid(bonusTaskId))
            throw new DomainInvariantException("Illegal eligibility transition.");
        var changed = value with
        {
            Status = target, StatusChangedAt = operationTime,
            ClaimedBonusTaskId = target == EligibilityStatus.Claimed ? bonusTaskId : null,
            ExcludedByBonusTaskId = target == EligibilityStatus.Excluded ? bonusTaskId : null
        };
        ValidateEligibility(changed);
        return changed;
    }

    public static void ValidateBonusStatus(BonusStatus value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.UserUID <= 0 || value.BonusAmount < 0 || value.RequiredWagerAmount < 0 ||
            value.CurrentWagerAmount < 0 || !Enum.IsDefined(value.TaskState) ||
            value.TaskState == BonusTaskState.Closing) throw new DomainInvariantException("Invalid BonusStatus state.");
        if (value.TaskState == BonusTaskState.None)
        {
            if (value.BonusTaskId is not null || value.EligibilityEntryId is not null ||
                value.ActivityUID is not null || value.BusinessDay is not null ||
                value.ActivitySnapshotJson is not null || value.ClaimedAt is not null ||
                value.BonusAmount != 0 || value.RequiredWagerAmount != 0 || value.CurrentWagerAmount != 0)
                throw new DomainInvariantException("Invalid empty BonusStatus.");
        }
        else if (!IsGuid(value.BonusTaskId) || value.EligibilityEntryId is not > 0 ||
            value.ActivityUID is not > 0 || value.BusinessDay is null ||
            string.IsNullOrWhiteSpace(value.ActivitySnapshotJson) || value.ClaimedAt is null)
            throw new DomainInvariantException("Invalid active BonusStatus.");
        else
        {
            var snapshot = ActivitySnapshotSerializer.Deserialize(value.ActivitySnapshotJson);
            if (snapshot.ActivityUID != value.ActivityUID)
                throw new DomainInvariantException("BonusStatus snapshot mismatch.");
        }
    }

    public static BonusStatus ResetBonusStatus(BonusStatus value, DateTime closedAt)
    {
        ValidateBonusStatus(value);
        if (value.TaskState != BonusTaskState.InProgress) throw new DomainInvariantException("No active task.");
        return value with
        {
            TaskState = BonusTaskState.None, BonusTaskId = null, EligibilityEntryId = null,
            ActivityUID = null, BusinessDay = null, ActivitySnapshotJson = null,
            BonusAmount = 0, RequiredWagerAmount = 0, CurrentWagerAmount = 0,
            ClaimedAt = null, UpdatedAt = closedAt
        };
    }

    public static void ValidateHistory(BonusHistory value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.BonusHistoryId <= 0 || !IsGuid(value.BonusTaskId) || value.UserUID <= 0 ||
            value.EligibilityEntryId <= 0 || value.ActivityUID <= 0 ||
            string.IsNullOrWhiteSpace(value.ActivitySnapshotJson) ||
            value.BonusAmount < 0 || value.RequiredWagerAmount < 0 ||
            value.CurrentWagerAmount < 0 || value.ConvertedAmount < 0 ||
            !Enum.IsDefined(value.CloseReason) ||
            value.CloseReason != CloseReason.WagerCompleted && value.ConvertedAmount != 0)
            throw new DomainInvariantException("Invalid BonusHistory.");
        var snapshot = ActivitySnapshotSerializer.Deserialize(value.ActivitySnapshotJson);
        if (snapshot.ActivityUID != value.ActivityUID)
            throw new DomainInvariantException("BonusHistory snapshot mismatch.");
    }

    private static bool Between(int value, int min, int max) => value >= min && value <= max;
    public static bool IsGuid(string? value) => value is { Length: 36 } &&
        Guid.TryParseExact(value, "D", out _);
}

public sealed class DomainInvariantException : Exception
{
    public DomainInvariantException(string message) : base(message) { }
}
