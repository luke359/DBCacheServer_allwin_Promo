namespace UserSessionReward.Core.Abstractions;

/// <summary>
/// 提供可替換的伺服器本機現在時間來源。
/// </summary>
public interface IClock
{
    /// <summary>
    /// 取得目前的伺服器本機時間。
    /// </summary>
    /// <value>時間種類應為 <see cref="DateTimeKind.Local"/>，且不得轉換為世界協調時間（UTC）。</value>
    DateTime Now { get; }
}
