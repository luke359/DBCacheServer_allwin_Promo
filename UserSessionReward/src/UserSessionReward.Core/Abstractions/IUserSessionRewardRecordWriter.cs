using UserSessionReward.Core.Enums;
using UserSessionReward.Core.Models;

namespace UserSessionReward.Core.Abstractions;

/// <summary>
/// 寫入玩家上升額度 Session 紀錄的 MySQL 邊界介面。
/// </summary>
public interface IUserSessionRewardRecordWriter
{
    /// <summary>
    /// 將已格式化的欄位值交由 MySQL 寫入層執行新增或更新。
    /// </summary>
    /// <param name="sqlTableName">受白名單控制的資料表名稱。</param>
    /// <param name="operation">新增或更新操作種類。</param>
    /// <param name="data">僅包含本次要寫入欄位的字典；所有值必須是不受地區影響的字串，宿主必須將每個值參數化。</param>
    /// <param name="where">更新時的相等條件字典；鍵必須是白名單欄位名稱，值必須是已格式化字串。宿主必須將每個值參數化，Core 不組 SQL。</param>
    /// <returns>包含影響筆數，及新增成功時流水號的寫入結果。</returns>
    /// <exception cref="ArgumentNullException">當必要參數為 <see langword="null"/> 時擲回。</exception>
    /// <exception cref="ArgumentException">當操作與 <paramref name="where"/> 的組合不符合規則時擲回。</exception>
    MysqlWriteResult WriteMysql(
        string sqlTableName,
        MysqlWriteOperation operation,
        Dictionary<string, string> data,
        Dictionary<string, string>? where = null);
}
