using System;

namespace DBCacheServer
{
    public partial class UserData
    {
        internal void CreditPromotionBonusWallet(double amount)
        {
            Balance2 = Math.Round(Balance2 + amount, Program.AccuracyDigitBal);
        }

        internal void ClearPromotionBonusWallet()
        {
            Balance2 = 0;
        }
    }
}
