using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class OceanKing8SettingData : FishCommonGameSetting
    {
        /// <summary>加農砲加倍押分最大倍數</summary>
        public int MultiBetMax;
        /// <summary>免費火箭數量</summary>
        public int RocketCount;


        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(int machineUID, Dictionary<string, string> datalist, Dictionary<string, string> datalist2)
        {
            //datalist 為各遊戲都有的共同設定
            GetDBDataCommon(machineUID, datalist);

            //datalist2 為各遊戲特有的設定
            MultiBetMax = Convert.ToInt32(datalist2["MultiBetMax"]);
            RocketCount = Convert.ToInt32(datalist2["RocketCount"]);
        }

        /// <summary>獲取遊戲設定值</summary>
        public Dictionary<string, string> GetSetting()
        {
            Dictionary<string, string> data = GetSettingCommon();
            data.Add("MultiBetMax", MultiBetMax.ToString()); 
            data.Add("RocketCount", RocketCount.ToString()); 
            return data;
        }
    }
}
