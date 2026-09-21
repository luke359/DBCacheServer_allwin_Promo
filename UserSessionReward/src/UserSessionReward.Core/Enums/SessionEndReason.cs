namespace UserSessionReward.Core.Enums;

/// <summary>
/// 表示結束 Session 的玩家交易原因。
/// </summary>
public enum SessionEndReason : byte
{
    /// <summary>
    /// 表示因下一次開分而結束既有 Session。
    /// </summary>
    Reopen = 1,

    /// <summary>
    /// 表示因洗分而結束既有 Session。
    /// </summary>
    Wash = 2
}
