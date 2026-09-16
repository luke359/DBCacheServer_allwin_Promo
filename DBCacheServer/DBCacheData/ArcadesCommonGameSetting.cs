using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class ArcadesCommonGameSetting
    {
        /// <summary>唯一碼</summary>
        public int SettingUID { get; set; }
        /// <summary>機台唯一碼</summary>
        public int MachineUID { get; set; }
        /// <summary>換分比例</summary>
        public double ConversionRaito { get; set; }
        /// <summary>機率參數</summary>
        public double Probability { get; set; }
        /// <summary>押注時間</summary>
        public int CountDownTime { get; set; }

        /// <summary>最低總押注</summary>
        public double MinRateLimit { get; set; }
        /// <summary>最高總押注</summary>
        public double MaxRateLimit { get; set; }
        /// <summary>最低啟動押分</summary>
        public double MinStartRateLimit { get; set; }

        /// <summary>押分單位</summary>
        public string BetUnit { get; set; }
        /// <summary>倍率選擇</summary>
        public int OddsChoice { get; set; }
        /// <summary>快速遊戲旗標</summary>
        public bool IsFastGame { get; set; }

        /// <summary>歸零炒場</summary>
        public double ResetPumpIn { get; set; }
        /// <summary>歸零炒場</summary>
        public double ResetPumpInMax { get; set; }
        /// <summary>歸零炒場分次</summary>
        public int ResetPumpInTimes { get; set; }

        /// <summary>循環炒場次數(低)</summary>
        public double CyclePumpInLow { get; set; }
        /// <summary>循環炒場次數(高)</summary>
        public double CyclePumpInHigh { get; set; }
        /// <summary>循環炒場百分比</summary>
        public double CyclePumpInPercent { get; set; }

        /// <summary>鎖牌設定旗標</summary>
        public bool IsCardTypeLock { get; set; }
        /// <summary>鎖牌設定選項</summary>
        public string CardTypeLockString { get; set; }

        public bool IsTimeControlA { get; set; }
        public int TimeControlTimesA { get; set; }
        public string TimeControlStringA { get; set; }
        public bool IsTimeControlB { get; set; }
        public int TimeControlTimesB { get; set; }
        public string TimeControlStringB { get; set; }
        public bool IsTimeControlC { get; set; }
        public int TimeControlTimesC { get; set; }
        public string TimeControlStringC { get; set; }
        public bool IsTimeControlD { get; set; }
        public int TimeControlTimesD { get; set; }
        public string TimeControlStringD { get; set; }
        public bool IsTimeControlE { get; set; }
        public int TimeControlTimesE { get; set; }
        public string TimeControlStringE { get; set; }
        public bool IsTimeControlF { get; set; }
        public int TimeControlTimesF { get; set; }
        public string TimeControlStringF { get; set; }
        public bool IsTimeControlG { get; set; }
        public int TimeControlTimesG { get; set; }
        public string TimeControlStringG { get; set; }
        public bool IsTimeControlH { get; set; }
        public int TimeControlTimesH { get; set; }
        public string TimeControlStringH { get; set; }
        public bool IsTimeControlI { get; set; }
        public int TimeControlTimesI { get; set; }
        public string TimeControlStringI { get; set; }
        public bool IsTimeControlJ { get; set; }
        public int TimeControlTimesJ { get; set; }
        public string TimeControlStringJ { get; set; }


        /// <summary>從DB資料更新內存</summary>
        public void GetDBDataCommon(int machineUID, Dictionary<string, string> datalist)
        {
            MachineUID = machineUID;
            SettingUID = Convert.ToInt32(datalist["SettingUID"]);
            ConversionRaito = Convert.ToDouble(datalist["ConversionRaito"]);
            Probability = Convert.ToDouble(datalist["Probability"]);
            CountDownTime = Convert.ToInt32(datalist["CountDownTime"]);

            MinRateLimit = Convert.ToDouble(datalist["MinRateLimit"]);
            MaxRateLimit = Convert.ToDouble(datalist["MaxRateLimit"]);
            MinStartRateLimit = Convert.ToDouble(datalist["MinStartRateLimit"]);

            BetUnit = datalist["BetUnit"];
            OddsChoice = Convert.ToInt32(datalist["OddsChoice"]);
            IsFastGame = Convert.ToBoolean(datalist["IsFastGame"]);

            ResetPumpIn = Convert.ToDouble(datalist["ResetPumpIn"]);
            ResetPumpInMax = Convert.ToDouble(datalist["ResetPumpInMax"]);
            ResetPumpInTimes = Convert.ToInt32(datalist["ResetPumpInTimes"]);

            CyclePumpInLow = Convert.ToDouble(datalist["CyclePumpInLow"]);
            CyclePumpInHigh = Convert.ToDouble(datalist["CyclePumpInHigh"]);
            CyclePumpInPercent = Convert.ToDouble(datalist["CyclePumpInPercent"]);

            IsCardTypeLock = Convert.ToBoolean(datalist["IsCardTypeLock"]);
            CardTypeLockString = datalist["CardTypeLockString"];

            IsTimeControlA = Convert.ToBoolean(datalist["IsTimeControlA"]);
            TimeControlTimesA = Convert.ToInt32(datalist["TimeControlTimesA"]);
            TimeControlStringA = datalist["TimeControlStringA"];
            IsTimeControlB = Convert.ToBoolean(datalist["IsTimeControlB"]);
            TimeControlTimesB = Convert.ToInt32(datalist["TimeControlTimesB"]);
            TimeControlStringB = datalist["TimeControlStringB"];
            IsTimeControlC = Convert.ToBoolean(datalist["IsTimeControlC"]);
            TimeControlTimesC = Convert.ToInt32(datalist["TimeControlTimesC"]);
            TimeControlStringC = datalist["TimeControlStringC"];
            IsTimeControlD = Convert.ToBoolean(datalist["IsTimeControlD"]);
            TimeControlTimesD = Convert.ToInt32(datalist["TimeControlTimesD"]);
            TimeControlStringD = datalist["TimeControlStringD"];
            IsTimeControlE = Convert.ToBoolean(datalist["IsTimeControlE"]);
            TimeControlTimesE = Convert.ToInt32(datalist["TimeControlTimesE"]);
            TimeControlStringE = datalist["TimeControlStringE"];
            IsTimeControlF = Convert.ToBoolean(datalist["IsTimeControlF"]);
            TimeControlTimesF = Convert.ToInt32(datalist["TimeControlTimesF"]);
            TimeControlStringF = datalist["TimeControlStringF"];
            IsTimeControlG = Convert.ToBoolean(datalist["IsTimeControlG"]);
            TimeControlTimesG = Convert.ToInt32(datalist["TimeControlTimesG"]);
            TimeControlStringG = datalist["TimeControlStringG"];
            IsTimeControlH = Convert.ToBoolean(datalist["IsTimeControlH"]);
            TimeControlTimesH = Convert.ToInt32(datalist["TimeControlTimesH"]);
            TimeControlStringH = datalist["TimeControlStringH"];
            IsTimeControlI = Convert.ToBoolean(datalist["IsTimeControlI"]);
            TimeControlTimesI = Convert.ToInt32(datalist["TimeControlTimesI"]);
            TimeControlStringI = datalist["TimeControlStringI"];
            IsTimeControlJ = Convert.ToBoolean(datalist["IsTimeControlJ"]);
            TimeControlTimesJ = Convert.ToInt32(datalist["TimeControlTimesJ"]);
            TimeControlStringJ = datalist["TimeControlStringJ"];
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
                { "CountDownTime", CountDownTime.ToString() },

                { "MinRateLimit", MinRateLimit.ToString() },
                { "MaxRateLimit", MaxRateLimit.ToString() },
                { "MinStartRateLimit", MinStartRateLimit.ToString() },

                { "BetUnit", BetUnit },
                { "OddsChoice", OddsChoice.ToString() },
                { "IsFastGame", IsFastGame.ToString() },

                { "ResetPumpIn", ResetPumpIn.ToString() },
                { "ResetPumpInMax", ResetPumpInMax.ToString() },
                { "ResetPumpInTimes", ResetPumpInTimes.ToString() },

                { "CyclePumpInLow", CyclePumpInLow.ToString() },
                { "CyclePumpInHigh", CyclePumpInHigh.ToString() },
                { "CyclePumpInPercent", CyclePumpInPercent.ToString() },

                { "IsCardTypeLock", IsCardTypeLock.ToString() },
                { "CardTypeLockString", CardTypeLockString },

                { "IsTimeControlA", IsTimeControlA.ToString() },
                { "TimeControlTimesA", TimeControlTimesA.ToString() },
                { "TimeControlStringA", TimeControlStringA },
                { "IsTimeControlB", IsTimeControlB.ToString() },
                { "TimeControlTimesB", TimeControlTimesB.ToString() },
                { "TimeControlStringB", TimeControlStringB },
                { "IsTimeControlC", IsTimeControlC.ToString() },
                { "TimeControlTimesC", TimeControlTimesC.ToString() },
                { "TimeControlStringC", TimeControlStringC },
                { "IsTimeControlD", IsTimeControlD.ToString() },
                { "TimeControlTimesD", TimeControlTimesD.ToString() },
                { "TimeControlStringD", TimeControlStringD },
                { "IsTimeControlE", IsTimeControlE.ToString() },
                { "TimeControlTimesE", TimeControlTimesE.ToString() },
                { "TimeControlStringE", TimeControlStringE },
                { "IsTimeControlF", IsTimeControlF.ToString() },
                { "TimeControlTimesF", TimeControlTimesF.ToString() },
                { "TimeControlStringF", TimeControlStringF },
                { "IsTimeControlG", IsTimeControlG.ToString() },
                { "TimeControlTimesG", TimeControlTimesG.ToString() },
                { "TimeControlStringG", TimeControlStringG },
                { "IsTimeControlH", IsTimeControlH.ToString() },
                { "TimeControlTimesH", TimeControlTimesH.ToString() },
                { "TimeControlStringH", TimeControlStringH },
                { "IsTimeControlI", IsTimeControlI.ToString() },
                { "TimeControlTimesI", TimeControlTimesI.ToString() },
                { "TimeControlStringI", TimeControlStringI },
                { "IsTimeControlJ", IsTimeControlJ.ToString() },
                { "TimeControlTimesJ", TimeControlTimesJ.ToString() },
                { "TimeControlStringJ", TimeControlStringJ }
            };

            return data;
        }
    }
}
