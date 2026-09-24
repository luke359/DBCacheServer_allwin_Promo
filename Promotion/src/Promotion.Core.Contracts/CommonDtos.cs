namespace Promotion.Core.Contracts;

public sealed record PromotionResult<T>(
    PromotionResultKind Kind, PromotionErrorCode ErrorCode, bool IsRetryable,
    string CorrelationId, T? Data, PromotionErrorDetailsDto? ErrorDetails);

public sealed record PromotionErrorDetailsDto(
    string? ExistingEventId, long? ExistingUserUID, TriggerType? ExistingTriggerType,
    DateTime? ExistingEventTime, DateOnly? ExistingBusinessDay, string? ExistingBonusTaskId);

public sealed record WalletInstructionDto(
    string OperationKey, WalletAction Action, long UserUID, string BonusTaskId, decimal Amount);

public sealed record EligibilityEntryDto(
    long EligibilityEntryId, string EventId, long UserUID, long ActivityUID,
    DateOnly BusinessDay, TriggerType TriggerType, decimal? EligibleDepositAmount,
    EligibilityStatus Status, string? ClaimedBonusTaskId, string? ExcludedByBonusTaskId,
    DateTime StatusChangedAt, DateTime CreatedAt);

public sealed record ActivitySnapshotDto(
    int SnapshotVersion, long ActivityUID, string? ActivityInfo, BonusType BonusType,
    int FixedBonusAmount, int MaxBonusAmount, int DepositPercentage, int? MinimumDepositAmount,
    int WagerMultiplier, WagerCalculationType WagerCalculationType, int? MaxBetAmount,
    decimal WagerContributionRate, ConvertType ConvertType,
    int FixedConvertedAmount, int? MaxBalanceConvertedAmount, int DailyClaimLimit,
    DateOnly? StartDate, DateOnly? EndDate, string WeekdayMask, TriggerType TriggerType,
    bool IsNonStackable, string? ExclusiveGroup, DateTime CapturedAt);

public sealed record BonusHistoryDto(
    long BonusHistoryId, string BonusTaskId, long UserUID, long EligibilityEntryId,
    long ActivityUID, DateOnly BusinessDay, ActivitySnapshotDto ActivitySnapshot,
    decimal BonusAmount, decimal RequiredWagerAmount, decimal CurrentWagerAmount,
    decimal ConvertedAmount, CloseReason CloseReason, DateTime ClaimedAt, DateTime ClosedAt);
