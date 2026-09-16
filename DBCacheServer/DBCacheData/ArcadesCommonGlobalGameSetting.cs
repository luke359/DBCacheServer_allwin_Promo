using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class ArcadesCommonGlobalGameSetting
    {
        /// <summary>唯一碼</summary>
        public int GlobalSettingUID;
        /// <summary>保留時間</summary>
        public int KeepTime;
        /// <summary>開放台數</summary>
        public int NumberOfOpen;
        /// <summary>試玩網吧機台數量(從最後機台開始倒數取)</summary>
        public int FreeMachCount = 0; //#240509

        /// <summary>賠率表模式</summary>
        public bool OddsType;
        /// <summary>大水庫累積百分比</summary>
        public double BigWaterCumulativePercent;

        public bool IsBigWater1;
        public int BigWater1Time;
        public int BigWater1ProFitDays;
        public bool IsBigWater2;
        public int BigWater2Time;
        public int BigWater2ProFitDays;
        public bool IsBigWater3;
        public int BigWater3Time;
        public int BigWater3ProFitDays;
        public bool IsBigWater4;
        public int BigWater4Time;
        public int BigWater4ProFitDays;
        public bool IsBigWater5;
        public int BigWater5Time;
        public int BigWater5ProFitDays;

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            GlobalSettingUID = Convert.ToInt32(datalist["GlobalSettingUID"]);
            KeepTime = Convert.ToInt32(datalist["KeepTime"]);
            NumberOfOpen = Convert.ToInt32(datalist["NumberOfOpen"]);
            OddsType = Convert.ToBoolean(datalist["OddsType"]);
            BigWaterCumulativePercent = Convert.ToDouble(datalist["BigWaterCumulativePercent"]);

            IsBigWater1 = Convert.ToBoolean(datalist["IsBigWater1"]);
            BigWater1Time = Convert.ToInt32(datalist["BigWater1Time"]);
            BigWater1ProFitDays = Convert.ToInt32(datalist["BigWater1ProFitDays"]);
            IsBigWater2 = Convert.ToBoolean(datalist["IsBigWater2"]);
            BigWater2Time = Convert.ToInt32(datalist["BigWater2Time"]);
            BigWater2ProFitDays = Convert.ToInt32(datalist["BigWater2ProFitDays"]);
            IsBigWater3 = Convert.ToBoolean(datalist["IsBigWater3"]);
            BigWater3Time = Convert.ToInt32(datalist["BigWater3Time"]);
            BigWater3ProFitDays = Convert.ToInt32(datalist["BigWater3ProFitDays"]);
            IsBigWater4 = Convert.ToBoolean(datalist["IsBigWater4"]);
            BigWater4Time = Convert.ToInt32(datalist["BigWater4Time"]);
            BigWater4ProFitDays = Convert.ToInt32(datalist["BigWater4ProFitDays"]);
            IsBigWater5 = Convert.ToBoolean(datalist["IsBigWater5"]);
            BigWater5Time = Convert.ToInt32(datalist["BigWater5Time"]);
            BigWater5ProFitDays = Convert.ToInt32(datalist["BigWater5ProFitDays"]);
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
                { "OddsType", OddsType.ToString() },
                { "BigWaterCumulativePercent", BigWaterCumulativePercent.ToString() },

                { "IsBigWater1", IsBigWater1.ToString() },
                { "BigWater1Time", BigWater1Time.ToString() },
                { "BigWater1ProFitDays", BigWater1ProFitDays.ToString() },
                { "IsBigWater2", IsBigWater2.ToString() },
                { "BigWater2Time", BigWater2Time.ToString() },
                { "BigWater2ProFitDays", BigWater2ProFitDays.ToString() },
                { "IsBigWater3", IsBigWater3.ToString() },
                { "BigWater3Time", BigWater3Time.ToString() },
                { "BigWater3ProFitDays", BigWater3ProFitDays.ToString() },
                { "IsBigWater4", IsBigWater4.ToString() },
                { "BigWater4Time", BigWater4Time.ToString() },
                { "BigWater4ProFitDays", BigWater4ProFitDays.ToString() },
                { "IsBigWater5", IsBigWater5.ToString() },
                { "BigWater5Time", BigWater5Time.ToString() },
                { "BigWater5ProFitDays", BigWater5ProFitDays.ToString() }
            };

            return tempdata;
        }
    }
}
