using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    /// <summary>押分驗證用</summary>
    public struct VerifyBetInfo
    {
        /// <summary>最低總押注</summary>
        public double MinTotalBet { get; init; }
        /// <summary>最高總押注</summary>
        public double MaxTotalBet { get; init; }
        /// <summary>最低啟動押分(押分機)</summary>
        public double MinStartRateLimit { get; init; }

        public VerifyBetInfo(double minTotalBet, double maxTotalBet, double minStartRateLimit)
        {
            MinTotalBet = minTotalBet;
            MaxTotalBet = maxTotalBet;
            MinStartRateLimit = minStartRateLimit;
        }
    }

    /// <summary>全機種 各分機設定</summary>
    public abstract class BaseGameSetting
    {
        /// <summary>機台唯一碼</summary>
        public int MachineUID { get; set; }
        /// <summary>新版 Kios模式</summary>
        public int KiosMode { get; set; }

        /// <summary>押分設定</summary>
        public VerifyBetInfo BetInfo;

        /// <summary>從DB資料更新 遊戲特有的設定</summary>
        public abstract void GetDBDataUnique(Dictionary<string, string> uniqueSettinglist);
        /// <summary>GameServer獲取 遊戲特有的設定</summary>
        public abstract void GetSettingUnique(bool newTypeServer, Dictionary<string, string> datalist, Dictionary<string, string> uniqueSettinglist);

        //public CommonGameSetting(){}

        /// <summary>從DB資料更新內存</summary>
        public virtual void GetDBData(int machineUID, Dictionary<string, string> datalist, CommonGame commGame)
        {
            //override 在 SlotCommonGameSetting
        }
    }
}
