using UserSessionReward.Core.Enums;

namespace UserSessionReward.Core.Models;

/// <summary>
/// 表示外部宿主保存與回傳的玩家上升額度最小快取資料契約。
/// </summary>
/// <remarks>
/// Core 僅建立、接收及回傳此資料；不負責玩家資料表、GameServer 快取或其他外部儲存體的保存與生命週期。
/// </remarks>
public sealed class RewardCacheDataModel
{
    /// <summary>
    /// 初始化可由外部宿主填入的空白快取資料。
    /// </summary>
    public RewardCacheDataModel()
    {
    }

    /// <summary>
    /// 取得或設定資料庫 Session 紀錄的流水號。
    /// </summary>
    /// <value>對應 <c>UserSessionRewardRecord.RewardRecordId</c> 的正整數流水號。</value>
    public long RewardRecordId { get; set; }

    /// <summary>
    /// 取得或設定功能的首次終止狀態。
    /// </summary>
    /// <value>對應資料庫 <c>RewardEndStatus</c> 的列舉值；首次成為非進行中後不得由後續更新覆寫。</value>
    public RewardEndStatus RewardEndStatus { get; set; }

    /// <summary>
    /// 取得或設定由外部產生器計算的目標餘額。
    /// </summary>
    public double TargetBalance { get; set; }

    /// <summary>
    /// 取得或設定目前 Session 的累積遊戲場數。
    /// </summary>
    /// <value>新 Session 的初始值為零。</value>
    public int TotalGameCount { get; set; }

    /// <summary>
    /// 取得或設定目前 Session 的累積總押分。
    /// </summary>
    /// <value>新 Session 的初始值為零。</value>
    public double TotalBet { get; set; }

    /// <summary>
    /// 取得或設定目前 Session 的最大單局押分。
    /// </summary>
    /// <value>尚無遊戲資料時為 <see langword="null"/>，寫入時保留資料庫 <c>NULL</c>。</value>
    public double? MaxBet { get; set; }

    /// <summary>
    /// 取得或設定目前 Session 的最小單局押分。
    /// </summary>
    /// <value>尚無遊戲資料時為 <see langword="null"/>，寫入時保留資料庫 <c>NULL</c>。</value>
    public double? MinBet { get; set; }

    /// <summary>
    /// 取得或設定目前 Session 已觀察到的最高玩家餘額。
    /// </summary>
    /// <value>新 Session 的初始值為開分交易後餘額。</value>
    public double MaxBalance { get; set; }

    /// <summary>
    /// 取得或設定目前 Session 已觀察到的最低玩家餘額。
    /// </summary>
    /// <value>新 Session 的初始值為開分交易後餘額。</value>
    public double MinBalance { get; set; }

    /// <summary>
    /// 取得或設定由宿主保存、Core 不解析的額外資訊。
    /// </summary>
    /// <value>只有非空白內容會在一般 Session 更新時覆寫資料庫欄位。</value>
    public string? ExtraInfo { get; set; }
}
