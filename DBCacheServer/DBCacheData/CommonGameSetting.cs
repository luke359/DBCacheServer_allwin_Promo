using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class CommonGameSetting
    {
        /// <summary>唯一碼</summary>
        public int SettingUID { get; set; }
        /// <summary>機台唯一碼</summary>
        public int MachineUID { get; set; }
        /// <summary>換分比例</summary>
        public double ConversionRaito { get; set; }
        /// <summary>機率參數</summary>
        public double Probability { get; set; }
        /// <summary>押分單位</summary>
        public string BetUnit { get; set; }
        /// <summary>最低總押注</summary>
        public double MinTotalBet { get; set; }
        /// <summary>最高總押注</summary>
        public double MaxTotalBet { get; set; }

        /// <summary>快速遊戲旗標</summary>
        public bool IsFastGame { get; set; }
        /// <summary>歸零炒場</summary>
        public double ResetPumpIn { get; set; }
        /// <summary>歸零炒場</summary>
        public double ResetPumpInMax { get; set; }
        /// <summary>歸零炒場分次</summary>
        public int ResetPumpInTimes { get; set; }
        /// <summary>鎖牌設定旗標</summary>
        public bool IsCardTypeLock { get; set; }
        /// <summary>鎖牌設定選項</summary>
        public string CardTypeLockString { get; set; }

        /// <summary>時控A旗標</summary>
        public bool IsTimeControlA { get; set; }
        /// <summary>時控A次數</summary>
        public int TimeControlTimesA { get; set; }
        /// <summary>時控A選項</summary>
        public string TimeControlStringA { get; set; }
        /// <summary>時控B旗標</summary>
        public bool IsTimeControlB { get; set; }
        /// <summary>時控B次數</summary>
        public int TimeControlTimesB { get; set; }
        /// <summary>時控B選項</summary>
        public string TimeControlStringB { get; set; }
        /// <summary>時控C旗標</summary>
        public bool IsTimeControlC { get; set; }
        /// <summary>時控C次數</summary>
        public int TimeControlTimesC { get; set; }
        /// <summary>時控C選項</summary>
        public string TimeControlStringC { get; set; }
        /// <summary>時控D旗標</summary>
        public bool IsTimeControlD { get; set; }
        /// <summary>時控D次數</summary>
        public int TimeControlTimesD { get; set; }
        /// <summary>時控D選項</summary>
        public string TimeControlStringD { get; set; }
        /// <summary>時控E旗標</summary>
        public bool IsTimeControlE { get; set; }
        /// <summary>時控E次數</summary>
        public int TimeControlTimesE { get; set; }
        /// <summary>時控E選項</summary>
        public string TimeControlStringE { get; set; }
        /// <summary>時控F旗標</summary>
        public bool IsTimeControlF { get; set; }
        /// <summary>時控F次數</summary>
        public int TimeControlTimesF { get; set; }
        /// <summary>時控F選項</summary>
        public string TimeControlStringF { get; set; }
        /// <summary>時控G旗標</summary>
        public bool IsTimeControlG { get; set; }
        /// <summary>時控G次數</summary>
        public int TimeControlTimesG { get; set; }
        /// <summary>時控G選項</summary>
        public string TimeControlStringG { get; set; }
        /// <summary>時控H旗標</summary>
        public bool IsTimeControlH { get; set; }
        /// <summary>時控H次數</summary>
        public int TimeControlTimesH { get; set; }
        /// <summary>時控H選項</summary>
        public string TimeControlStringH { get; set; }
        /// <summary>時控I旗標</summary>
        public bool IsTimeControlI { get; set; }
        /// <summary>時控I次數</summary>
        public int TimeControlTimesI { get; set; }
        /// <summary>時控I選項</summary>
        public string TimeControlStringI { get; set; }
        /// <summary>時控J旗標</summary>
        public bool IsTimeControlJ { get; set; }
        /// <summary>時控J次數</summary>
        public int TimeControlTimesJ { get; set; }
        /// <summary>時控J選項</summary>
        public string TimeControlStringJ { get; set; }

        /// <summary>獨立遊戲機率</summary>
        public double IndepProbability { get; set; }
        /// <summary>獨立遊戲最低總押分</summary>
        public double IndepGameBetMin { get; set; }
        /// <summary>獨立遊戲最高總押分</summary>
        public double IndepGameBetMax { get; set; }
        /// <summary>獨立遊戲機率模式</summary>
        public int IndepProbabilityMode { get; set; }
        /// <summary>獨立遊戲歸零炒場下限</summary>
        public double IndepResetPumpIn { get; set; }
        /// <summary>獨立遊戲歸零炒場上限</summary>
        public double IndepResetPumpInMax { get; set; }
        /// <summary>獨立遊戲歸零炒場分次</summary>
        public int IndepResetPumpInTimes { get; set; }

        /// <summary>Level內定植</summary>
        public int DefaultLevel { get; set; }
        /// <summary>押分內定植</summary>
        public double DefaultBet { get; set; }
        /// <summary>運算總押分門檻(算牌最低總押, 低於此值只出小牌)</summary>
        public double CalBetThreshold { get; set; }


        /// <summary>從DB資料更新內存</summary>
        public void GetDBDataCommon(int machineUID, Dictionary<string, string> datalist)
        {
            MachineUID = machineUID;
            SettingUID = Convert.ToInt32(datalist["SettingUID"]);
            ConversionRaito = Convert.ToDouble(datalist["ConversionRaito"]);
            Probability = Convert.ToDouble(datalist["Probability"]);
            BetUnit = datalist["BetUnit"];
            MinTotalBet = Convert.ToDouble(datalist["MinTotalBet"]);
            MaxTotalBet = Convert.ToDouble(datalist["MaxTotalBet"]);

            IsFastGame = Convert.ToBoolean(datalist["IsFastGame"]);
            ResetPumpIn = Convert.ToDouble(datalist["ResetPumpIn"]);
            ResetPumpInMax = Convert.ToDouble(datalist["ResetPumpInMax"]);
            ResetPumpInTimes = Convert.ToInt32(datalist["ResetPumpInTimes"]);
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

            IndepProbability = Convert.ToDouble(datalist["IndepProbability"]);
            IndepGameBetMin = Convert.ToDouble(datalist["IndepGameBetMin"]);
            IndepGameBetMax = Convert.ToDouble(datalist["IndepGameBetMax"]);
            IndepProbabilityMode = Convert.ToInt32(datalist["IndepProbabilityMode"]);
            IndepResetPumpIn = Convert.ToDouble(datalist["IndepResetPumpIn"]);
            IndepResetPumpInMax = Convert.ToDouble(datalist["IndepResetPumpInMax"]);
            IndepResetPumpInTimes = Convert.ToInt32(datalist["IndepResetPumpInTimes"]);

            DefaultLevel = Convert.ToInt32(datalist["DefaultLevel"]);
            DefaultBet = Convert.ToDouble(datalist["DefaultBet"]);
            CalBetThreshold = Convert.ToDouble(datalist["CalBetThreshold"]);
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
                { "BetUnit", BetUnit },
                { "MinTotalBet", MinTotalBet.ToString() },
                { "MaxTotalBet", MaxTotalBet.ToString() },

                { "IsFastGame", IsFastGame.ToString() },
                { "ResetPumpIn", ResetPumpIn.ToString() },
                { "ResetPumpInMax", ResetPumpInMax.ToString() },
                { "ResetPumpInTimes", ResetPumpInTimes.ToString() },
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
                { "TimeControlStringJ", TimeControlStringJ },

                { "IndepProbability", IndepProbability.ToString() },
                { "IndepGameBetMin", IndepGameBetMin.ToString() },
                { "IndepGameBetMax", IndepGameBetMax.ToString() },
                { "IndepProbabilityMode", IndepProbabilityMode.ToString() },
                { "IndepResetPumpIn", IndepResetPumpIn.ToString() },
                { "IndepResetPumpInMax", IndepResetPumpInMax.ToString() },
                { "IndepResetPumpInTimes", IndepResetPumpInTimes.ToString() },

                { "DefaultLevel", DefaultLevel.ToString() },
                { "DefaultBet", DefaultBet.ToString() },
                { "CalBetThreshold", CalBetThreshold.ToString() },
            };

            return data;
        }
    }
}
