namespace UserSessionReward.Core.Enums;

/// <summary>
/// 表示 MySQL 寫入介面支援的操作種類。
/// </summary>
public enum MysqlWriteOperation : byte
{
    /// <summary>
    /// 表示新增一筆資料列，且不得提供 <c>where</c> 條件字典。
    /// </summary>
    Insert = 1,

    /// <summary>
    /// 表示更新既有資料列，且必須提供非空的 <c>where</c> 條件字典。
    /// </summary>
    Update = 2
}
