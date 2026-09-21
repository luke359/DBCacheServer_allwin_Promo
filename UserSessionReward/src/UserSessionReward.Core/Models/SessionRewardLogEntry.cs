using UserSessionReward.Core.Enums;

namespace UserSessionReward.Core.Models;

/// <summary>
/// 表示玩家上升額度流程所產生的一筆結構化錯誤或警告紀錄。
/// </summary>
public sealed class SessionRewardLogEntry
{
    /// <summary>
    /// 初始化空白結構化紀錄。
    /// </summary>
    public SessionRewardLogEntry()
    {
    }

    /// <summary>
    /// 取得或設定發生問題的操作名稱。
    /// </summary>
    public string OperationName { get; init; } = string.Empty;

    /// <summary>
    /// 取得或設定相關玩家 UID。
    /// </summary>
    public int UserUid { get; init; }

    /// <summary>
    /// 取得或設定相關的 Session 紀錄流水號。
    /// </summary>
    /// <value>操作尚未取得紀錄流水號時為 <see langword="null"/>。</value>
    public long? RewardRecordId { get; init; }

    /// <summary>
    /// 取得或設定已讀取到的 Session 狀態。
    /// </summary>
    /// <value>尚未讀取紀錄時為 <see langword="null"/>。</value>
    public SessionRecordStatus? RecordStatus { get; init; }

    /// <summary>
    /// 取得或設定已讀取到的功能終止狀態。
    /// </summary>
    /// <value>尚未讀取紀錄時為 <see langword="null"/>。</value>
    public RewardEndStatus? RewardEndStatus { get; init; }

    /// <summary>
    /// 取得或設定錯誤或警告原因。
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// 取得或設定紀錄發生時間。
    /// </summary>
    /// <remarks>值為伺服器本機時間，與資料庫寫入時間一致，不做時區轉換。</remarks>
    public DateTime OccurredAt { get; init; }
}
