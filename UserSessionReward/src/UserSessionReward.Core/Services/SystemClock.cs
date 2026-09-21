using UserSessionReward.Core.Abstractions;

namespace UserSessionReward.Core.Services;

/// <summary>
/// 使用系統本機時間的預設時間來源。
/// </summary>
public sealed class SystemClock : IClock
{
    /// <summary>
    /// 初始化使用系統本機時間的時間來源。
    /// </summary>
    public SystemClock()
    {
    }

    /// <summary>
    /// 取得目前的伺服器本機時間。
    /// </summary>
    /// <value>由 <see cref="DateTime.Now"/> 提供的本機時間。</value>
    public DateTime Now => DateTime.Now;
}
