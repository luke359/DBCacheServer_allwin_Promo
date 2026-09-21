namespace UserSessionReward.Core.Enums;

/// <summary>
/// 表示玩家上升額度功能的首次終止原因。
/// </summary>
public enum RewardEndStatus : byte
{
    /// <summary>
    /// 表示功能仍在進行中，尚未寫入首次終止餘額。
    /// </summary>
    InProgress = 0,

    /// <summary>
    /// 表示 Server 自然關閉功能。
    /// </summary>
    ServerNaturalClose = 1,

    /// <summary>
    /// 表示玩家再次開分時提前關閉功能。
    /// </summary>
    PlayerEarlyClose = 2,

    /// <summary>
    /// 表示 Web 後台強制關閉功能。
    /// </summary>
    WebForceClose = 3
}
