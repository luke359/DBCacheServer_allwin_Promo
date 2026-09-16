using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    /// <summary>老虎機 各分機設定</summary>
    public abstract class SlotCommonGameSetting : BaseGameSetting
    {
        /// <summary>獨立遊戲押分設定</summary>
        public VerifyBetInfo IndepBetInfo;

        //public CommonGameSetting(){}

        /// <summary>從DB資料更新內存</summary>
        public override void GetDBData(int machineUID, Dictionary<string, string> datalist, CommonGame commGame)
        {
            MachineUID = machineUID;
            KiosMode = machineUID;

            if(commGame.NewTypeServer && machineUID == 0) Console.WriteLine($"    {commGame.GameNameCd} 載入Kios設定");

            double minTotalBet = Convert.ToDouble(datalist["MinTotalBet"]);
            double maxTotalBet = Convert.ToDouble(datalist["MaxTotalBet"]);
            BetInfo = new VerifyBetInfo(minTotalBet, maxTotalBet, 0);

            if (commGame.IndepPlay)
            {
                if (datalist.ContainsKey("IndepGameBetMulti"))
                {
                    int indepGameBetMulti = Convert.ToInt32(datalist["IndepGameBetMulti"]);
                    if (indepGameBetMulti > 0)
                    {
                        commGame.SetIndepGameBetMulti(indepGameBetMulti); //若資料庫有設定則覆寫
                    }
                }

                double indepGameBetMin = Convert.ToDouble(datalist["IndepGameBetMin"]);
                double indepGameBetMax = Convert.ToDouble(datalist["IndepGameBetMax"]);
                double indepGameBetMinMu = Math.Round(indepGameBetMin * (double)commGame.IndepGameBetMulti, 4);
                double indepGameBetMaxMu = Math.Round(indepGameBetMax * (double)commGame.IndepGameBetMulti, 4);
                IndepBetInfo = new VerifyBetInfo(indepGameBetMinMu, indepGameBetMaxMu, 0);
            }
        }
    }
}
