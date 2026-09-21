namespace UserSessionReward.Core.Models;

/// <summary>
/// 表示結束或更新一筆 Session 的處理結果。
/// </summary>
public sealed class SessionOperationResult
{
    /// <summary>
    /// 初始化空白 Session 操作結果。
    /// </summary>
    public SessionOperationResult()
    {
    }

    /// <summary>
    /// 取得或設定操作是否實際成功寫入一筆資料庫紀錄。
    /// </summary>
    public bool IsSuccessful { get; init; }

    /// <summary>
    /// 取得或設定操作是否因已結束或無效狀態而安全地未寫入資料庫。
    /// </summary>
    /// <remarks>此值為 <see langword="true"/> 時，<see cref="IsSuccessful"/> 必定為 <see langword="false"/>。</remarks>
    public bool IsNoOperation { get; init; }

    /// <summary>
    /// 取得或設定可供外部紀錄的結果說明。
    /// </summary>
    public string Message { get; init; } = string.Empty;
}
