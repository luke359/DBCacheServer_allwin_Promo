namespace UserSessionReward.Core.Models;

/// <summary>
/// 表示一次 MySQL 寫入操作的最小結果。
/// </summary>
public sealed class MysqlWriteResult
{
    /// <summary>
    /// 初始化空白 MySQL 寫入結果。
    /// </summary>
    public MysqlWriteResult()
    {
    }

    /// <summary>
    /// 取得或設定受本次操作影響的資料列數。
    /// </summary>
    public long AffectedRows { get; init; }

    /// <summary>
    /// 取得或設定新增成功後的自動流水號。
    /// </summary>
    /// <value>僅新增操作成功時應有正整數；更新操作通常為 <see langword="null"/>。</value>
    public long? LastInsertedId { get; init; }
}
