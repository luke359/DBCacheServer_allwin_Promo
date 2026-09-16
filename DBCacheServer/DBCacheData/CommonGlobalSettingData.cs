using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class CommonGlobalSettingData
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

        /// <summary>獨立買大水庫累積百分比</summary>
        public double IndepBigWaterCumulativePercent;
        public bool IndepIsBigWater1;
        public int IndepBigWater1Time;
        public int IndepBigWater1ProFitDays;
        public bool IndepIsBigWater2;
        public int IndepBigWater2Time;
        public int IndepBigWater2ProFitDays;
        public bool IndepIsBigWater3;
        public int IndepBigWater3Time;
        public int IndepBigWater3ProFitDays;
        public bool IndepIsBigWater4;
        public int IndepBigWater4Time;
        public int IndepBigWater4ProFitDays;
        public bool IndepIsBigWater5;
        public int IndepBigWater5Time;
        public int IndepBigWater5ProFitDays;

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            GlobalSettingUID = Convert.ToInt32(datalist["GlobalSettingUID"]);
            KeepTime = Convert.ToInt32(datalist["KeepTime"]);
            NumberOfOpen = Convert.ToInt32(datalist["NumberOfOpen"]);

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

            IndepBigWaterCumulativePercent = Convert.ToDouble(datalist["IndepBigWaterCumulativePercent"]);
            IndepIsBigWater1 = Convert.ToBoolean(datalist["IndepIsBigWater1"]);
            IndepBigWater1Time = Convert.ToInt32(datalist["IndepBigWater1Time"]);
            IndepBigWater1ProFitDays = Convert.ToInt32(datalist["IndepBigWater1ProFitDays"]);
            IndepIsBigWater2 = Convert.ToBoolean(datalist["IndepIsBigWater2"]);
            IndepBigWater2Time = Convert.ToInt32(datalist["IndepBigWater2Time"]);
            IndepBigWater2ProFitDays = Convert.ToInt32(datalist["IndepBigWater2ProFitDays"]);
            IndepIsBigWater3 = Convert.ToBoolean(datalist["IndepIsBigWater3"]);
            IndepBigWater3Time = Convert.ToInt32(datalist["IndepBigWater3Time"]);
            IndepBigWater3ProFitDays = Convert.ToInt32(datalist["IndepBigWater3ProFitDays"]);
            IndepIsBigWater4 = Convert.ToBoolean(datalist["IndepIsBigWater4"]);
            IndepBigWater4Time = Convert.ToInt32(datalist["IndepBigWater4Time"]);
            IndepBigWater4ProFitDays = Convert.ToInt32(datalist["IndepBigWater4ProFitDays"]);
            IndepIsBigWater5 = Convert.ToBoolean(datalist["IndepIsBigWater5"]);
            IndepBigWater5Time = Convert.ToInt32(datalist["IndepBigWater5Time"]);
            IndepBigWater5ProFitDays = Convert.ToInt32(datalist["IndepBigWater5ProFitDays"]);
        }

        /// <summary>獲取遊戲設定值</summary>
        public Dictionary<string, string> GetSetting()
        {
            var tempdata = new Dictionary<string, string>();
            tempdata.Add("GlobalSettingUID", GlobalSettingUID.ToString());
            tempdata.Add("KeepTime", KeepTime.ToString());
            tempdata.Add("NumberOfOpen", NumberOfOpen.ToString());
            tempdata.Add("FreeMachCount", FreeMachCount.ToString());

            tempdata.Add("BigWaterCumulativePercent", BigWaterCumulativePercent.ToString());
            tempdata.Add("IsBigWater1", IsBigWater1.ToString());
            tempdata.Add("BigWater1Time", BigWater1Time.ToString());
            tempdata.Add("BigWater1ProFitDays", BigWater1ProFitDays.ToString());
            tempdata.Add("IsBigWater2", IsBigWater2.ToString());
            tempdata.Add("BigWater2Time", BigWater2Time.ToString());
            tempdata.Add("BigWater2ProFitDays", BigWater2ProFitDays.ToString());
            tempdata.Add("IsBigWater3", IsBigWater3.ToString());
            tempdata.Add("BigWater3Time", BigWater3Time.ToString());
            tempdata.Add("BigWater3ProFitDays", BigWater3ProFitDays.ToString());
            tempdata.Add("IsBigWater4", IsBigWater4.ToString());
            tempdata.Add("BigWater4Time", BigWater4Time.ToString());
            tempdata.Add("BigWater4ProFitDays", BigWater4ProFitDays.ToString());
            tempdata.Add("IsBigWater5", IsBigWater5.ToString());
            tempdata.Add("BigWater5Time", BigWater5Time.ToString());
            tempdata.Add("BigWater5ProFitDays", BigWater5ProFitDays.ToString());

            tempdata.Add("IndepBigWaterCumulativePercent", IndepBigWaterCumulativePercent.ToString());
            tempdata.Add("IndepIsBigWater1", IndepIsBigWater1.ToString());
            tempdata.Add("IndepBigWater1Time", IndepBigWater1Time.ToString());
            tempdata.Add("IndepBigWater1ProFitDays", IndepBigWater1ProFitDays.ToString());
            tempdata.Add("IndepIsBigWater2", IndepIsBigWater2.ToString());
            tempdata.Add("IndepBigWater2Time", IndepBigWater2Time.ToString());
            tempdata.Add("IndepBigWater2ProFitDays", IndepBigWater2ProFitDays.ToString());
            tempdata.Add("IndepIsBigWater3", IndepIsBigWater3.ToString());
            tempdata.Add("IndepBigWater3Time", IndepBigWater3Time.ToString());
            tempdata.Add("IndepBigWater3ProFitDays", IndepBigWater3ProFitDays.ToString());
            tempdata.Add("IndepIsBigWater4", IndepIsBigWater4.ToString());
            tempdata.Add("IndepBigWater4Time", IndepBigWater4Time.ToString());
            tempdata.Add("IndepBigWater4ProFitDays", IndepBigWater4ProFitDays.ToString());
            tempdata.Add("IndepIsBigWater5", IndepIsBigWater5.ToString());
            tempdata.Add("IndepBigWater5Time", IndepBigWater5Time.ToString());
            tempdata.Add("IndepBigWater5ProFitDays", IndepBigWater5ProFitDays.ToString());
            return tempdata;
        }

        /// <summary>加入遊戲設定值(不在DB資料庫的設定)</summary>
        public void GetSettingExtra(Dictionary<string, string> datalist)
        {
            datalist.Add("FreeMachCount", FreeMachCount.ToString());
        }
    }
}
