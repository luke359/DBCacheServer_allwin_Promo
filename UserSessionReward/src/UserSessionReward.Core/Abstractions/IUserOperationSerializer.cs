namespace UserSessionReward.Core.Abstractions;

/// <summary>
/// 以玩家 UID 為鍵，序列化同一玩家資料操作的同步機制。
/// </summary>
/// <remarks>
/// 實作至少需確保單一執行程序內，同一玩家的新增、結束與更新 Session 依序完成；跨執行程序路由責任由外部宿主承擔。
/// </remarks>
public interface IUserOperationSerializer
{
    /// <summary>
    /// 在指定玩家的序列化範圍內執行沒有回傳值的操作。
    /// </summary>
    /// <param name="userUid">用作序列化鍵的玩家 UID。</param>
    /// <param name="action">必須在取得玩家序列化權後完整執行的操作。</param>
    /// <exception cref="ArgumentNullException">當 <paramref name="action"/> 為 <see langword="null"/> 時擲回。</exception>
    void Execute(int userUid, Action action);

    /// <summary>
    /// 在指定玩家的序列化範圍內執行並回傳結果。
    /// </summary>
    /// <typeparam name="TResult">操作成功完成後回傳值的型別。</typeparam>
    /// <param name="userUid">用作序列化鍵的玩家 UID。</param>
    /// <param name="action">必須在取得玩家序列化權後完整執行的操作。</param>
    /// <returns><paramref name="action"/> 執行完成後的結果。</returns>
    /// <exception cref="ArgumentNullException">當 <paramref name="action"/> 為 <see langword="null"/> 時擲回。</exception>
    TResult Execute<TResult>(int userUid, Func<TResult> action);
}
