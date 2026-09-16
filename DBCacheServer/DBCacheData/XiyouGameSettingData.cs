using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public  class XiyouGameSettingData : ArcadesCommonGameSetting
    {
        /////// <summary>倍率表</summary>
        ////public bool OddsTable { get; set; }
        /////// <summary>保留時間</summary>
        ////public int KeepTime { get; set; }
        /////// <summary>開放台數</summary>
        ////public int NumberOfOpen { get; set; }
        /////// <summary>人機數量</summary>
        ////public int NumberOfBots { get; set; }
        /////// <summary>人機入賬旗標</summary>
        ////public bool IsBotBill { get; set; }
        /////// <summary>抽水</summary>
        ////public int PumpIn { get; set; }
        /////// <summary>抽水分次</summary>
        ////public int PumpInTimes { get; set; }
        /////// <summary>放水</summary>
        ////public int DrainOut { get; set; }
        /////// <summary>放水分次</summary>
        ////public int DrainOutTimes { get; set; }
        

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
