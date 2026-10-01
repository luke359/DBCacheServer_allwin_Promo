namespace Promotion.ResponseDecoder.Contracts;

/// <summary>所有已解包優惠 Payload 的基底型別。</summary>
public abstract class PromoPayloadBase
{
}

/// <summary>
/// 玩家優惠頁 Payload；供 PromoGetActivitiesResponse 與 PromoGetPlayerOffersResponse 共用。
/// 前者的 TodayCompletedItems 固定為空集合。
/// </summary>
public sealed class PromoGetPlayerOffersPayload : PromoPayloadBase
{
    public string BusinessDay { get; init; } = string.Empty;
    public bool HasActiveBonusTask { get; init; }
    public string ActiveBonusTaskId { get; init; } = string.Empty;
    public PromoActiveBonusTaskPayload? ActiveTask { get; init; }
    public IReadOnlyList<PromoPlayerPromotionActivityPayload> Activities { get; init; } = Array.Empty<PromoPlayerPromotionActivityPayload>();
    public IReadOnlyList<PromoTodayCompletedItemPayload> TodayCompletedItems { get; init; } = Array.Empty<PromoTodayCompletedItemPayload>();
}

public sealed class PromoPlayerPromotionActivityPayload
{
    public long ActivityUID { get; init; }
    public PromoActivityDisplayPayload Activity { get; init; } = new();
    public long? EligibilityEntryId { get; init; }
    public bool CanClaim { get; init; }
    public bool HasActiveTask { get; init; }
    public bool ClaimButtonEnabled { get; init; }
    public PromoBonusTaskProgressPayload? ActiveTaskProgress { get; init; }
}

public sealed class PromoBonusTaskProgressPayload
{
    public decimal CurrentWagerAmount { get; init; }
    public decimal RequiredWagerAmount { get; init; }
    public decimal RemainingWagerAmount { get; init; }
}

/// <summary>玩家優惠頁顯示用活動規則；不包含後端快照與結算資料。</summary>
public sealed class PromoActivityDisplayPayload
{
    public string? ActivityInfo { get; init; }
    public int BonusType { get; init; }
    public int FixedBonusAmount { get; init; }
    public int MaxBonusAmount { get; init; }
    public int DepositPercentage { get; init; }
    public int? MinimumDepositAmount { get; init; }
    public int WagerMultiplier { get; init; }
    public int? MaxBetAmount { get; init; }
    public int DailyClaimLimit { get; init; }
}

public sealed class PromoActiveBonusTaskPayload
{
    public string BonusTaskId { get; init; } = string.Empty;
    public long EligibilityEntryId { get; init; }
    public long ActivityUID { get; init; }
    public string BusinessDay { get; init; } = string.Empty;
    public decimal BonusAmount { get; init; }
    public decimal RequiredWagerAmount { get; init; }
    public decimal CurrentWagerAmount { get; init; }
    public decimal RemainingWagerAmount { get; init; }
    public int? MaxBetAmount { get; init; }
    public string ClaimedAt { get; init; } = string.Empty;
}

public sealed class PromoTodayCompletedItemPayload
{
    public long BonusHistoryId { get; init; }
    public string BonusTaskId { get; init; } = string.Empty;
    public long ActivityUID { get; init; }
    public string BusinessDay { get; init; } = string.Empty;
    public string ActivityInfo { get; init; } = string.Empty;
    public decimal BonusAmount { get; init; }
    public decimal RequiredWagerAmount { get; init; }
    public decimal CurrentWagerAmount { get; init; }
    public decimal ConvertedAmount { get; init; }
    public string CloseReason { get; init; } = string.Empty;
    public string ClaimedAt { get; init; } = string.Empty;
    public string ClosedAt { get; init; } = string.Empty;
}

public sealed class PromoGetGamesPayload : PromoPayloadBase
{
    public long ActivityUID { get; init; }
    public string GameServerList { get; init; } = string.Empty;
}

public sealed class PromoClaimPayload : PromoPayloadBase
{
    public long EligibilityEntryId { get; init; }
    public string BonusTaskId { get; init; } = string.Empty;
    public long ActivityUID { get; init; }
    public string BusinessDay { get; init; } = string.Empty;
    public decimal BonusAmount { get; init; }
    public decimal RequiredWagerAmount { get; init; }
    public decimal CurrentWagerAmount { get; init; }
    public decimal RemainingWagerAmount { get; init; }
    public int? MaxBetAmount { get; init; }
    public string ClaimedAt { get; init; } = string.Empty;
    public bool IsReplay { get; init; }
}

public sealed class PromoTaskPayload
{
    public string BonusTaskId { get; init; } = string.Empty;
    public long EligibilityEntryId { get; init; }
    public long ActivityUID { get; init; }
    public decimal BonusAmount { get; init; }
    public decimal RequiredWagerAmount { get; init; }
    public decimal CurrentWagerAmount { get; init; }
    public decimal RemainingWagerAmount { get; init; }
    public string UnlockMode { get; init; } = string.Empty;
    public bool CanClaimUnlock { get; init; }
    public decimal? EstimatedUnlockAmount { get; init; }
    public int? MaxBetAmount { get; init; }
    public string ClaimedAt { get; init; } = string.Empty;
}

public sealed class PromoGetTaskPayload : PromoPayloadBase
{
    public bool HasActiveTask { get; init; }
    public PromoTaskPayload? Task { get; init; }
}

public sealed class PromoAbandonPayload : PromoPayloadBase
{
    public string BonusTaskId { get; init; } = string.Empty;
    public string CloseReason { get; init; } = string.Empty;
    public decimal FinalCurrentWagerAmount { get; init; }
    public decimal ConvertedAmount { get; init; }
    public string ClosedAt { get; init; } = string.Empty;
    public bool AlreadyClosed { get; init; }
}

public sealed class PromoHistoryItemPayload
{
    public long BonusHistoryId { get; init; }
    public string BonusTaskId { get; init; } = string.Empty;
    public long ActivityUID { get; init; }
    public string ActivityInfo { get; init; } = string.Empty;
    public string BusinessDay { get; init; } = string.Empty;
    public decimal BonusAmount { get; init; }
    public decimal RequiredWagerAmount { get; init; }
    public decimal CurrentWagerAmount { get; init; }
    public decimal ConvertedAmount { get; init; }
    public string CloseReason { get; init; } = string.Empty;
    public string ClaimedAt { get; init; } = string.Empty;
    public string ClosedAt { get; init; } = string.Empty;
}

public sealed class PromoGetHistoryPayload : PromoPayloadBase
{
    public string FromInclusive { get; init; } = string.Empty;
    public string ToExclusive { get; init; } = string.Empty;
    public int Offset { get; init; }
    public int Limit { get; init; }
    public IReadOnlyList<PromoHistoryItemPayload> Items { get; init; } = Array.Empty<PromoHistoryItemPayload>();
}
