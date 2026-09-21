using UserSessionReward.Core.Enums;

namespace UserSessionReward.Core.Models;

/// <summary>
/// 表示從 <c>UserSessionRewardRecord</c> 資料表讀取的一筆 Session 完整快照。
/// </summary>
/// <remarks>
/// 此模型只作為資料讀取邊界的契約；實際 MySQL 查詢及欄位轉換由外部實作負責。
/// </remarks>
public sealed class UserSessionRewardRecord
{
    /// <summary>
    /// 初始化可由外部資料讀取器填入的空白 Session 紀錄快照。
    /// </summary>
    public UserSessionRewardRecord()
    {
    }

    /// <summary>
    /// 取得或設定資料庫紀錄流水號。
    /// </summary>
    public long RewardRecordId { get; init; }

    /// <summary>
    /// 取得或設定此紀錄所屬的玩家 UID。
    /// </summary>
    public int UserUid { get; init; }

    /// <summary>
    /// 取得或設定建立 Session 時的開分值。
    /// </summary>
    /// <remarks>此值建立後永久保留，結束 Session 時不得覆寫。</remarks>
    public double KeyInAmount { get; init; }

    /// <summary>
    /// 取得或設定開分交易完成後的玩家餘額。
    /// </summary>
    public double BalanceAfterKeyIn { get; init; }

    /// <summary>
    /// 取得或設定依發生順序以逗號分隔的洗分值。
    /// </summary>
    public string? KeyOutAmount { get; init; }

    /// <summary>
    /// 取得或設定依發生順序以逗號分隔的洗分交易前玩家餘額。
    /// </summary>
    public string? BalanceWhenKeyOut { get; init; }

    /// <summary>
    /// 取得或設定由宿主保存、Core 不解析的額外資訊。
    /// </summary>
    public string? ExtraInfo { get; init; }

    /// <summary>
    /// 取得或設定外部產生器計算的目標餘額。
    /// </summary>
    public double TargetBalance { get; init; }

    /// <summary>
    /// 取得或設定 Session 的生命週期狀態。
    /// </summary>
    public SessionRecordStatus RecordStatus { get; init; }

    /// <summary>
    /// 取得或設定功能的首次終止狀態。
    /// </summary>
    public RewardEndStatus RewardEndStatus { get; init; }

    /// <summary>
    /// 取得或設定 Session 首次結束時、交易套用前的玩家餘額。
    /// </summary>
    public double? EndBalance { get; init; }

    /// <summary>
    /// 取得或設定 Session 累積遊戲場數。
    /// </summary>
    public int TotalGameCount { get; init; }

    /// <summary>
    /// 取得或設定 Session 累積總押分。
    /// </summary>
    public double TotalBet { get; init; }

    /// <summary>
    /// 取得或設定 Session 最大單局押分。
    /// </summary>
    public double? MaxBet { get; init; }

    /// <summary>
    /// 取得或設定 Session 最小單局押分。
    /// </summary>
    public double? MinBet { get; init; }

    /// <summary>
    /// 取得或設定 Session 最高玩家餘額。
    /// </summary>
    public double MaxBalance { get; init; }

    /// <summary>
    /// 取得或設定 Session 最低玩家餘額。
    /// </summary>
    public double MinBalance { get; init; }

    /// <summary>
    /// 取得或設定 Session 開始時間。
    /// </summary>
    /// <remarks>值為伺服器本機時間，不做時區轉換。</remarks>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 取得或設定 Session 首次結束時間。
    /// </summary>
    /// <remarks>值為 <see langword="null"/> 或伺服器本機時間，不做時區轉換。</remarks>
    public DateTime? EndTime { get; init; }
}
