using System.Collections.Concurrent;
using UserSessionReward.Core.Abstractions;

namespace UserSessionReward.Core.Services;

/// <summary>
/// 在單一執行程序內以玩家 UID 鎖定操作的序列化實作。
/// </summary>
/// <remarks>
/// 同一玩家會依序執行，不同玩家可平行執行；此類別無法取代多個 DBCache 執行個體間的穩定路由。
/// </remarks>
public sealed class InProcessUserOperationSerializer : IUserOperationSerializer
{
    /// <summary>
    /// 初始化單一執行程序的玩家操作序列化器。
    /// </summary>
    public InProcessUserOperationSerializer()
    {
    }

    /// <summary>
    /// 保存每個玩家 UID 對應的鎖定物件。
    /// </summary>
    private readonly ConcurrentDictionary<int, object> _userLocks = new();

    /// <summary>
    /// 在指定玩家的序列化範圍內執行沒有回傳值的操作。
    /// </summary>
    /// <param name="userUid">用作序列化鍵的玩家 UID。</param>
    /// <param name="action">必須在取得玩家序列化權後完整執行的操作。</param>
    /// <exception cref="ArgumentNullException">當 <paramref name="action"/> 為 <see langword="null"/> 時擲回。</exception>
    public void Execute(int userUid, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);
        lock (GetUserLock(userUid))
        {
            action();
        }
    }

    /// <summary>
    /// 在指定玩家的序列化範圍內執行並回傳結果。
    /// </summary>
    /// <typeparam name="TResult">操作成功完成後回傳值的型別。</typeparam>
    /// <param name="userUid">用作序列化鍵的玩家 UID。</param>
    /// <param name="action">必須在取得玩家序列化權後完整執行的操作。</param>
    /// <returns><paramref name="action"/> 執行完成後的結果。</returns>
    /// <exception cref="ArgumentNullException">當 <paramref name="action"/> 為 <see langword="null"/> 時擲回。</exception>
    public TResult Execute<TResult>(int userUid, Func<TResult> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        lock (GetUserLock(userUid))
        {
            return action();
        }
    }

    /// <summary>
    /// 取得指定玩家專用的鎖定物件。
    /// </summary>
    /// <param name="userUid">欲取得鎖定物件的玩家 UID。</param>
    /// <returns>同一玩家 UID 永遠對應同一個鎖定物件。</returns>
    private object GetUserLock(int userUid)
    {
        return _userLocks.GetOrAdd(userUid, _ => new object());
    }
}
