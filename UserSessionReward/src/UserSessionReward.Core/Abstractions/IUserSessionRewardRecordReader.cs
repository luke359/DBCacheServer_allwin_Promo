using UserSessionReward.Core.Models;

namespace UserSessionReward.Core.Abstractions;

/// <summary>
/// 讀取玩家上升額度 Session 紀錄的外部資料存取介面。
/// </summary>
public interface IUserSessionRewardRecordReader
{
    /// <summary>
    /// 讀取指定玩家所有進行中的 Session。
    /// </summary>
    /// <param name="userUid">欲查詢的玩家 UID。</param>
    /// <returns>符合玩家 UID 且 <c>RecordStatus=0</c> 的紀錄清單；沒有紀錄時回傳空清單。</returns>
    IReadOnlyList<UserSessionRewardRecord> GetInProgressByUserUid(int userUid);

    /// <summary>
    /// 以玩家 UID 與紀錄流水號讀取單一 Session。
    /// </summary>
    /// <param name="userUid">紀錄必須相符的玩家 UID。</param>
    /// <param name="rewardRecordId">欲讀取的資料庫紀錄流水號。</param>
    /// <returns>兩個識別值均相符時的紀錄；找不到或不屬於此玩家時為 <see langword="null"/>。</returns>
    UserSessionRewardRecord? GetByUserUidAndRewardRecordId(int userUid, long rewardRecordId);
}
