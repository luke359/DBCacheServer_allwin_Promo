using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public  class AstonMartinSettingData : ArcadesCommonGameSetting
    {
        

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(int machineUID, Dictionary<string, string> datalist)
        {
            //datalist 為各遊戲都有的共同設定
            GetDBDataCommon(machineUID, datalist);
        
            //datalist2 為各遊戲特有的設定

        }
        /// <summary>獲取遊戲設定值</summary>
        public Dictionary<string,string> GetSetting()
        {            
            Dictionary<string, string> data = GetSettingCommon();
            return data;
        }
    }
}
