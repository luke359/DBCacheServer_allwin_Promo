namespace UserSessionReward.Core.Enums;

/// <summary>
/// 表示玩家 Session 在資料庫中的生命週期狀態。
/// </summary>
public enum SessionRecordStatus : byte
{
    /// <summary>
    /// 表示 Session 正在進行，允許更新遊戲統計或結束 Session。
    /// </summary>
    InProgress = 0,

    /// <summary>
    /// 表示玩家再次開分而結束前一筆 Session。
    /// </summary>
    EndedByReopen = 1,

}
