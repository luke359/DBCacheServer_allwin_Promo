using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class FishCommonGlobalGameSetting
    {
        /// <summary>唯一碼</summary>
        public int GlobalSettingUID;
        /// <summary>保留時間</summary>
        public int KeepTime;
        /// <summary>開放台數</summary>
        public int NumberOfOpen;
        /// <summary>試玩網吧機台數量(從最後機台開始倒數取)</summary>
        public int FreeMachCount = 0; //#240509

        /// <summary>大水庫累積百分比</summary>
        public double BigWaterCumulativePercent;
        public double BigWaterCumulativePercent2;
        public double BigWaterCumulativePercent3;
        public double BigWaterCumulativePercent4;

        public bool IsBigWater1_1;
        public int BigWater1_1Time;
        public int BigWater1_1ProFitDays;
        public bool IsBigWater1_2;
        public int BigWater1_2Time;
        public int BigWater1_2ProFitDays;
        public bool IsBigWater1_3;
        public int BigWater1_3Time;
        public int BigWater1_3ProFitDays;
        public bool IsBigWater2_1;
        public int BigWater2_1Time;
        public int BigWater2_1ProFitDays;
        public bool IsBigWater2_2;
        public int BigWater2_2Time;
        public int BigWater2_2ProFitDays;
        public bool IsBigWater2_3;
        public int BigWater2_3Time;
        public int BigWater2_3ProFitDays;
        public bool IsBigWater3_1;
        public int BigWater3_1Time;
        public int BigWater3_1ProFitDays;
        public bool IsBigWater3_2;
        public int BigWater3_2Time;
        public int BigWater3_2ProFitDays;
        public bool IsBigWater3_3;
        public int BigWater3_3Time;
        public int BigWater3_3ProFitDays;
        public bool IsBigWater4_1;
        public int BigWater4_1Time;
        public int BigWater4_1ProFitDays;
        public bool IsBigWater4_2;
        public int BigWater4_2Time;
        public int BigWater4_2ProFitDays;
        /// <summary>節慶獎種類 (空字串=無, 1=龍舟, 2=月兔)</summary>
        public string FestivalPrize;

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            GlobalSettingUID = Convert.ToInt32(datalist["GlobalSettingUID"]);
            KeepTime = Convert.ToInt32(datalist["KeepTime"]);
            NumberOfOpen = Convert.ToInt32(datalist["NumberOfOpen"]);
            BigWaterCumulativePercent = Convert.ToDouble(datalist["BigWaterCumulativePercent"]);
            BigWaterCumulativePercent2 = Convert.ToDouble(datalist["BigWaterCumulativePercent2"]);
            BigWaterCumulativePercent3 = Convert.ToDouble(datalist["BigWaterCumulativePercent3"]);
            BigWaterCumulativePercent4 = Convert.ToDouble(datalist["BigWaterCumulativePercent4"]);

            IsBigWater1_1 = Convert.ToBoolean(datalist["IsBigWater1_1"]);
            BigWater1_1Time = Convert.ToInt32(datalist["BigWater1_1Time"]);
            BigWater1_1ProFitDays = Convert.ToInt32(datalist["BigWater1_1ProFitDays"]);
            IsBigWater1_2 = Convert.ToBoolean(datalist["IsBigWater1_2"]);
            BigWater1_2Time = Convert.ToInt32(datalist["BigWater1_2Time"]);
            BigWater1_2ProFitDays = Convert.ToInt32(datalist["BigWater1_2ProFitDays"]);
            IsBigWater1_3 = Convert.ToBoolean(datalist["IsBigWater1_3"]);
            BigWater1_3Time = Convert.ToInt32(datalist["BigWater1_3Time"]);
            BigWater1_3ProFitDays = Convert.ToInt32(datalist["BigWater1_3ProFitDays"]);
            IsBigWater2_1 = Convert.ToBoolean(datalist["IsBigWater2_1"]);
            BigWater2_1Time = Convert.ToInt32(datalist["BigWater2_1Time"]);
            BigWater2_1ProFitDays = Convert.ToInt32(datalist["BigWater2_1ProFitDays"]);
            IsBigWater2_2 = Convert.ToBoolean(datalist["IsBigWater2_2"]);
            BigWater2_2Time = Convert.ToInt32(datalist["BigWater2_2Time"]);
            BigWater2_2ProFitDays = Convert.ToInt32(datalist["BigWater2_2ProFitDays"]);
            IsBigWater2_3 = Convert.ToBoolean(datalist["IsBigWater2_3"]);
            BigWater2_3Time = Convert.ToInt32(datalist["BigWater2_3Time"]);
            BigWater2_3ProFitDays = Convert.ToInt32(datalist["BigWater2_3ProFitDays"]);
            IsBigWater3_1 = Convert.ToBoolean(datalist["IsBigWater3_1"]);
            BigWater3_1Time = Convert.ToInt32(datalist["BigWater3_1Time"]);
            BigWater3_1ProFitDays = Convert.ToInt32(datalist["BigWater3_1ProFitDays"]);
            IsBigWater3_2 = Convert.ToBoolean(datalist["IsBigWater3_2"]);
            BigWater3_2Time = Convert.ToInt32(datalist["BigWater3_2Time"]);
            BigWater3_2ProFitDays = Convert.ToInt32(datalist["BigWater3_2ProFitDays"]);
            IsBigWater3_3 = Convert.ToBoolean(datalist["IsBigWater3_3"]);
            BigWater3_3Time = Convert.ToInt32(datalist["BigWater3_3Time"]);
            BigWater3_3ProFitDays = Convert.ToInt32(datalist["BigWater3_3ProFitDays"]);
            IsBigWater4_1 = Convert.ToBoolean(datalist["IsBigWater4_1"]);
            BigWater4_1Time = Convert.ToInt32(datalist["BigWater4_1Time"]);
            BigWater4_1ProFitDays = Convert.ToInt32(datalist["BigWater4_1ProFitDays"]);
            IsBigWater4_2 = Convert.ToBoolean(datalist["IsBigWater4_2"]);
            BigWater4_2Time = Convert.ToInt32(datalist["BigWater4_2Time"]);
            BigWater4_2ProFitDays = Convert.ToInt32(datalist["BigWater4_2ProFitDays"]);
            FestivalPrize = datalist["FestivalPrize"];
        }

        /// <summary>獲取遊戲設定值</summary>
        public Dictionary<string, string> GetSetting()
        {
            var tempdata = new Dictionary<string, string>
            {
                { "GlobalSettingUID", GlobalSettingUID.ToString() },
                { "KeepTime", KeepTime.ToString() },
                { "NumberOfOpen", NumberOfOpen.ToString() },
                { "FreeMachCount", FreeMachCount.ToString() },
                { "BigWaterCumulativePercent", BigWaterCumulativePercent.ToString() },
                { "BigWaterCumulativePercent2", BigWaterCumulativePercent2.ToString() },
                { "BigWaterCumulativePercent3", BigWaterCumulativePercent3.ToString() },
                { "BigWaterCumulativePercent4", BigWaterCumulativePercent4.ToString() },
                { "IsBigWater1_1",          IsBigWater1_1.ToString() },
                { "BigWater1_1Time",        BigWater1_1Time.ToString() },
                { "BigWater1_1ProFitDays",  BigWater1_1ProFitDays.ToString() },
                { "IsBigWater1_2",          IsBigWater1_2.ToString() },
                { "BigWater1_2Time",        BigWater1_2Time.ToString() },
                { "BigWater1_2ProFitDays",  BigWater1_2ProFitDays.ToString() },
                { "IsBigWater1_3",          IsBigWater1_3.ToString() },
                { "BigWater1_3Time",        BigWater1_3Time.ToString() },
                { "BigWater1_3ProFitDays",  BigWater1_3ProFitDays.ToString() },
                { "IsBigWater2_1",          IsBigWater2_1.ToString() },
                { "BigWater2_1Time",        BigWater2_1Time.ToString() },
                { "BigWater2_1ProFitDays",  BigWater2_1ProFitDays.ToString() },
                { "IsBigWater2_2",          IsBigWater2_2.ToString() },
                { "BigWater2_2Time",        BigWater2_2Time.ToString() },
                { "BigWater2_2ProFitDays",  BigWater2_2ProFitDays.ToString() },
                { "IsBigWater2_3",          IsBigWater2_3.ToString() },
                { "BigWater2_3Time",        BigWater2_3Time.ToString() },
                { "BigWater2_3ProFitDays",  BigWater2_3ProFitDays.ToString() },
                { "IsBigWater3_1",          IsBigWater3_1.ToString() },
                { "BigWater3_1Time",        BigWater3_1Time.ToString() },
                { "BigWater3_1ProFitDays",  BigWater3_1ProFitDays.ToString() },
                { "IsBigWater3_2",          IsBigWater3_2.ToString() },
                { "BigWater3_2Time",        BigWater3_2Time.ToString() },
                { "BigWater3_2ProFitDays",  BigWater3_2ProFitDays.ToString() },
                { "IsBigWater3_3",          IsBigWater3_3.ToString() },
                { "BigWater3_3Time",        BigWater3_3Time.ToString() },
                { "BigWater3_3ProFitDays",  BigWater3_3ProFitDays.ToString() },
                { "IsBigWater4_1",          IsBigWater4_1.ToString() },
                { "BigWater4_1Time",        BigWater4_1Time.ToString() },
                { "BigWater4_1ProFitDays",  BigWater4_1ProFitDays.ToString() },
                { "IsBigWater4_2",          IsBigWater4_2.ToString() },
                { "BigWater4_2Time",        BigWater4_2Time.ToString() },
                { "BigWater4_2ProFitDays",  BigWater4_2ProFitDays.ToString() },
                { "FestivalPrize", FestivalPrize },
            };

            return tempdata;
        }
    }
}
