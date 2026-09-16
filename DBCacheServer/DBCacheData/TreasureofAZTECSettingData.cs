using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class TreasureofAZTECSettingData : SlotCommonGameSetting
    {
        /// <summary>從DB資料更新 遊戲特有的設定</summary>
        public override void GetDBDataUnique(Dictionary<string, string> uniqueSettinglist)
        {
            //DBCache 須用到的資訊, 才須讀入
        }

        /// <summary>GameServer獲取遊戲 遊戲特有的設定值</summary>
        public override void GetSettingUnique(bool newTypeServer, Dictionary<string, string> datalist, Dictionary<string, string> uniqueSettinglist)
        {
            //將 uniqueSettinglist 資訊加入 datalist
        }
    }
}
