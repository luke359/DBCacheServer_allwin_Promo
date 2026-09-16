using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class FishCommonGameSetting
    {
        /// <summary>唯一碼</summary>
        public int SettingUID { get; set; }
        /// <summary>機台唯一碼</summary>
        public int MachineUID { get; set; }
        /// <summary>換分比例</summary>
        public double ConversionRaito { get; set; }
        /// <summary>機率參數</summary>
        public double Probability { get; set; }

        /// <summary>0=子彈  1=鎖定  2=雷射</summary>
        public int WeaponType;
        /// <summary>押分單位</summary>
        public string BetUnit;
        /// <summary>最低總押注</summary>
        public double MinTotalBet;
        /// <summary>最高總押注</summary>
        public double MaxTotalBet;
        /// <summary>押分內定植</summary>
        public double DefaultBet;
        /// <summary>快速遊戲旗標</summary>
        public bool IsFastGame;
        /// <summary>歸零炒場下限</summary>
        public double ResetPumpIn;
        /// <summary>歸零炒場上限</summary>
        public double ResetPumpInMax;
        /// <summary>歸零炒場分次</summary>
        public int ResetPumpInTimes;
        /// <summary>鎖牌設定旗標</summary>
        public bool IsCardTypeLock;
        /// <summary>鎖牌設定選項</summary>
        public string CardTypeLockString;


        /// <summary>從DB資料更新內存</summary>
        public void GetDBDataCommon(int machineUID, Dictionary<string, string> datalist)
        {
            MachineUID = machineUID;
            SettingUID = Convert.ToInt32(datalist["SettingUID"]);
            ConversionRaito = Convert.ToDouble(datalist["ConversionRaito"]);
            Probability = Convert.ToDouble(datalist["Probability"]);

            WeaponType = Convert.ToInt32(datalist["WeaponType"]);
            BetUnit = datalist["BetUnit"];
            MinTotalBet = Convert.ToDouble(datalist["MinTotalBet"]);
            MaxTotalBet = Convert.ToDouble(datalist["MaxTotalBet"]);
            DefaultBet = Convert.ToDouble(datalist["DefaultBet"]);
            IsFastGame = Convert.ToBoolean(datalist["IsFastGame"]);
            ResetPumpIn = Convert.ToDouble(datalist["ResetPumpIn"]);
            ResetPumpInMax = Convert.ToDouble(datalist["ResetPumpInMax"]);
            ResetPumpInTimes = Convert.ToInt32(datalist["ResetPumpInTimes"]);
            IsCardTypeLock = Convert.ToBoolean(datalist["IsCardTypeLock"]);
            CardTypeLockString = datalist["CardTypeLockString"];
        }

        /// <summary>獲取遊戲設定值</summary>
        public Dictionary<string, string> GetSettingCommon()
        {
            var data = new Dictionary<string, string>
            {
                { "SettingUID", SettingUID.ToString() },
                { "MachineUID", MachineUID.ToString() },
                { "ConversionRaito", (ConversionRaito).ToString() },
                { "Probability", Probability.ToString() },

                { "WeaponType",          WeaponType.ToString() },
                { "BetUnit",             BetUnit },
                { "MinTotalBet",         MinTotalBet.ToString() },
                { "MaxTotalBet",         MaxTotalBet.ToString() },
                { "DefaultBet",          DefaultBet.ToString() },
                { "IsFastGame",          IsFastGame.ToString() },
                { "ResetPumpIn",         ResetPumpIn.ToString() },
                { "ResetPumpInMax",      ResetPumpInMax.ToString() },
                { "ResetPumpInTimes",    ResetPumpInTimes.ToString() },
                { "IsCardTypeLock",      IsCardTypeLock.ToString() },
                { "CardTypeLockString",  CardTypeLockString },
            };

            return data;
        }
    }
}
