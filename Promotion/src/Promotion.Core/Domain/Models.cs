using Promotion.Core.Contracts;

namespace Promotion.Core.Domain;

public sealed record PromotionActivity(
    long ActivityUID, string? ActivityInfo, BonusType BonusType, int FixedBonusAmount,
    int MaxBonusAmount, int DepositPercentage, int? MinimumDepositAmount,
    int WagerMultiplier, WagerCalculationType WagerCalculationType, int? MaxBetAmount,
    string? GameServerList, decimal WagerContributionRate, ConvertType ConvertType,
    int FixedConvertedAmount, int? MaxBalanceConvertedAmount, int DailyClaimLimit,
    DateOnly? StartDate, DateOnly? EndDate, string WeekdayMask, ActivityStatus ActivityStatus, TriggerType TriggerType,
    bool IsNonStackable, string? ExclusiveGroup, DateTime CreatedAt, DateTime UpdatedAt);

public sealed record ActivitySnapshot(ActivitySnapshotDto Value);

public sealed record PromotionTriggerEvent(string EventId, long UserUID, TriggerType TriggerType,
    DateTime EventTime, DateOnly BusinessDay, decimal? EligibleDepositAmount, DateTime CreatedAt);

public sealed record EligibilityEntry(long EligibilityEntryId, string EventId, long UserUID,
    long ActivityUID, DateOnly BusinessDay, TriggerType TriggerType,
    decimal? EligibleDepositAmount, EligibilityStatus Status, string? ClaimedBonusTaskId,
    string? ExcludedByBonusTaskId, DateTime StatusChangedAt, DateTime CreatedAt);

public sealed record NewEligibilityEntry(string EventId, long UserUID, long ActivityUID,
    DateOnly BusinessDay, TriggerType TriggerType, decimal? EligibleDepositAmount,
    EligibilityStatus Status, string? ClaimedBonusTaskId, string? ExcludedByBonusTaskId,
    DateTime StatusChangedAt, DateTime CreatedAt);

public sealed record BonusStatus(long UserUID, BonusTaskState TaskState, string? BonusTaskId,
    long? EligibilityEntryId, long? ActivityUID, DateOnly? BusinessDay,
    DateOnly? LastFirstLoginBusinessDay, DateOnly? LastFirstDepositBusinessDay,
    string? ActivitySnapshotJson, decimal BonusAmount, decimal RequiredWagerAmount,
    decimal CurrentWagerAmount, DateTime? ClaimedAt, DateTime CreatedAt, DateTime UpdatedAt);

public sealed record BonusHistory(long BonusHistoryId, string BonusTaskId, long UserUID,
    long EligibilityEntryId, long ActivityUID, DateOnly BusinessDay, string ActivitySnapshotJson,
    decimal BonusAmount, decimal RequiredWagerAmount, decimal CurrentWagerAmount,
    decimal ConvertedAmount, CloseReason CloseReason, DateTime ClaimedAt,
    DateTime ClosedAt, DateTime CreatedAt);

public sealed record NewBonusHistory(string BonusTaskId, long UserUID,
    long EligibilityEntryId, long ActivityUID, DateOnly BusinessDay, string ActivitySnapshotJson,
    decimal BonusAmount, decimal RequiredWagerAmount, decimal CurrentWagerAmount,
    decimal ConvertedAmount, CloseReason CloseReason, DateTime ClaimedAt,
    DateTime ClosedAt, DateTime CreatedAt);
