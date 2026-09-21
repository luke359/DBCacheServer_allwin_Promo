using UserSessionReward.Core.Models;

namespace UserSessionReward.Core.Abstractions;

/// <summary>
/// 記錄玩家上升額度流程錯誤與警告的外部介面。
/// </summary>
public interface IUserSessionRewardLogger
{
    /// <summary>
    /// 記錄會使目前操作失敗或拒絕執行的錯誤。
    /// </summary>
    /// <param name="entry">含操作、玩家、狀態、原因及本機時間的結構化紀錄。</param>
    void LogError(SessionRewardLogEntry entry);

    /// <summary>
    /// 記錄不寫入資料庫但可供追蹤的警告。
    /// </summary>
    /// <param name="entry">含操作、玩家、狀態、原因及本機時間的結構化紀錄。</param>
    void LogWarning(SessionRewardLogEntry entry);
}
