using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class BigSmallSettingData : ArcadesCommonGameSetting
    {
        /// <summary>JP獎金累積百分比</summary>
        public double JPCollectRaito;
        /// <summary>JP獎金人數</summary>
        public int JPPrizePlayerLimit = 50;
        /// <summary>開放人數</summary>
        public int NumberOfPlayer = 1000;
        /// <summary>押分內定植</summary>
        public double DefaultBet;


        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(int machineUID, Dictionary<string, string> datalist, Dictionary<string, string> datalist2)
        {
            //datalist 為各遊戲都有的共同設定
            GetDBDataCommon(machineUID, datalist);

            //datalist2 為各遊戲特有的設定
            JPCollectRaito = Convert.ToDouble(datalist2["JPCollectRaito"]);
            JPPrizePlayerLimit = Convert.ToInt32(datalist2["JPPrizePlayerLimit"]);
            NumberOfPlayer = Convert.ToInt32(datalist2["NumberOfPlayer"]);
            DefaultBet = Convert.ToDouble(datalist2["DefaultBet"]);
        }

        /// <summary>獲取遊戲設定值</summary>
        public Dictionary<string, string> GetSetting()
        {
            Dictionary<string, string> data = GetSettingCommon();
            data.Add("JPCollectRaito", JPCollectRaito.ToString());
            data.Add("JPPrizePlayerLimit", JPPrizePlayerLimit.ToString());
            data.Add("NumberOfPlayer", NumberOfPlayer.ToString());
            data.Add("DefaultBet", DefaultBet.ToString());
            return data;
        }
    }
}
