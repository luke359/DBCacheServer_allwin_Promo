namespace UserSessionReward.Core.Abstractions;

/// <summary>
/// 依開分後餘額、本次開分值與宿主提供的設定字串，產生新 Session 所需目標餘額的外部規則介面。
/// </summary>
/// <remarks>
/// Core 只原樣轉入開分後餘額、本次開分值與 <c>rewardWebSetting</c>，不解析、不驗證、不保存該字串；解析與演算法由外部實作負責。該字串不得寫入 <c>RewardCacheDataModel</c> 或資料庫。
/// </remarks>
public interface ITargetBalanceGenerator
{
    /// <summary>
    /// 依開分後餘額、本次開分值與宿主設定字串計算新 Session 的功能終止狀態與目標餘額。
    /// </summary>
    /// <param name="balanceAfterKeyIn">本次建立 Session 的開分交易套用後玩家餘額。</param>
    /// <param name="keyInAmount">本次建立 Session 的開分值。</param>
    /// <param name="rewardWebSetting">宿主提供的目標餘額規則設定字串；Core 不解析、不驗證、不保存，僅原樣轉入。</param>
    /// <returns>外部規則計算完成的功能終止狀態與目標餘額；Core 會忽略 <c>rewardEndStatus</c>。</returns>
    (int rewardEndStatus, double targetBalance) Generate(double balanceAfterKeyIn, double keyInAmount, string rewardWebSetting);
}
