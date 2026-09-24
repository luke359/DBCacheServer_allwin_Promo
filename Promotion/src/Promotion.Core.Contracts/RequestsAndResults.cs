namespace Promotion.Core.Contracts;

public delegate double BalanceConvertFormula(BalanceConvertContext context);
public sealed record InitializeRequest(TimeSpan BusinessDayCutover, BalanceConvertFormula BalanceConvertFormula, string? CorrelationId = null);
public sealed record InitializeData(TimeSpan BusinessDayCutover, int SnapshotVersion, bool WasAlreadyInitialized);
public sealed record BalanceConvertContext(
    long UserUID, string BonusTaskId, long EligibilityEntryId, long ActivityUID, DateOnly BusinessDay,
    decimal BonusAmount, decimal RequiredWagerAmount, decimal CurrentWagerAmount,
    decimal RemainingWagerAmount, decimal SettledBonusWalletBalance, decimal GameBetAmount,
    decimal GameWinAmount, DateTime GameTime, ActivitySnapshotDto ActivitySnapshot);

public sealed record CreateEligibilityRequest(string EventId, long UserUID, TriggerType TriggerType,
    DateTime EventTime, decimal? EligibleDepositAmount, IReadOnlyCollection<long> AllowedActivityUIDs,
    string? CorrelationId = null);
public sealed record CreateEligibilityData(string EventId, long UserUID, DateOnly BusinessDay,
    bool IsReplay, IReadOnlyList<EligibilityEntryDto> Entries);

public sealed record GetAvailablePromotionsRequest(long UserUID, DateTime QueryTime, string? CorrelationId = null);
public sealed record AvailablePromotionListData(long UserUID, DateOnly BusinessDay,
    bool HasActiveBonusTask, string? ActiveBonusTaskId, IReadOnlyList<AvailablePromotionDto> Items);
public sealed record AvailablePromotionDto(long EligibilityEntryId, string EventId, long ActivityUID,
    DateOnly BusinessDay, TriggerType TriggerType, decimal? EligibleDepositAmount, string? ActivityInfo,
    decimal EstimatedBonusAmount, decimal EstimatedRequiredWagerAmount, int? MaxBetAmount,
    bool IsNonStackable, string? ExclusiveGroup);

public sealed record GetGameServerListRequest(long ActivityUID, string? CorrelationId = null);
public sealed record GameServerListData(long ActivityUID, string? GameServerList);

public sealed record ClaimPromotionRequest(long UserUID, long EligibilityEntryId, string? CorrelationId = null);
public sealed record ClaimPromotionData(long UserUID, long EligibilityEntryId, string BonusTaskId,
    long ActivityUID, DateOnly BusinessDay, decimal BonusAmount, decimal RequiredWagerAmount,
    decimal CurrentWagerAmount, decimal RemainingWagerAmount, int? MaxBetAmount, DateTime ClaimedAt,
    int SnapshotVersion, bool IsReplay, IReadOnlyList<WalletInstructionDto> WalletInstructions);

public sealed record GetBonusTaskStatusRequest(long UserUID, string? CorrelationId = null);
public sealed record BonusTaskStatusData(long UserUID, bool HasActiveTask, ActiveBonusTaskDto? Task);
public sealed record ActiveBonusTaskDto(string BonusTaskId, long EligibilityEntryId, long ActivityUID,
    DateOnly BusinessDay, decimal BonusAmount, decimal RequiredWagerAmount, decimal CurrentWagerAmount,
    decimal RemainingWagerAmount, int? MaxBetAmount, DateTime ClaimedAt, ActivitySnapshotDto ActivitySnapshot);

public sealed record AccumulateWagerRequest(long UserUID, string ExpectedBonusTaskId,
    decimal GameBetAmount, decimal GameWinAmount, DateTime GameTime,
    decimal SettledBonusWalletBalance, string? CorrelationId = null);
public sealed record AccumulateWagerData(long UserUID, string BonusTaskId, decimal EffectiveWagerAmount,
    decimal CurrentWagerAmount, decimal RemainingWagerAmount, BonusTaskOutcome Outcome,
    CloseReason? CloseReason, decimal ConvertedAmount, DateTime? ClosedAt,
    IReadOnlyList<WalletInstructionDto> WalletInstructions);

public sealed record CloseWagerCompletedBonusTaskRequest(long UserUID, string ExpectedBonusTaskId,
    decimal SettledBonusWalletBalance, decimal GameBetAmount, decimal GameWinAmount,
    DateTime GameTime, string? CorrelationId = null);
public sealed record CloseDepletedBonusTaskRequest(long UserUID, string ExpectedBonusTaskId,
    decimal SettledBonusWalletBalance, DateTime NotificationTime, string? CorrelationId = null);
public sealed record AbandonBonusTaskRequest(long UserUID, string ExpectedBonusTaskId,
    DateTime RequestedAt, string? CorrelationId = null);
public sealed record CloseBonusTaskData(long UserUID, string BonusTaskId, CloseReason CloseReason,
    decimal FinalCurrentWagerAmount, decimal ConvertedAmount, DateTime ClosedAt,
    bool AlreadyClosed, IReadOnlyList<WalletInstructionDto> WalletInstructions);

public sealed record DailyMaintenanceRequest(DateTime ExecutionTime, int BatchSize, string? CorrelationId = null);
public sealed record DailyMaintenanceData(DateOnly CurrentBusinessDay, DateTime HistoryCutoffTime,
    int ExpiredEligibilityCount, int ClosedExpiredTaskCount, int DeletedHistoryCount,
    IReadOnlyList<WalletInstructionDto> WalletInstructions, IReadOnlyList<MaintenanceFailureDto> Failures);
public sealed record MaintenanceFailureDto(MaintenanceStage Stage, long? UserUID, string? BonusTaskId,
    PromotionErrorCode ErrorCode, bool IsRetryable);

public sealed record GetBonusHistoryRequest(long UserUID, DateTime FromInclusive,
    DateTime ToExclusive, int Offset, int Limit, string? CorrelationId = null);
public sealed record BonusHistoryPageData(long UserUID, DateTime FromInclusive, DateTime ToExclusive,
    int Offset, int Limit, IReadOnlyList<BonusHistoryDto> Items);
