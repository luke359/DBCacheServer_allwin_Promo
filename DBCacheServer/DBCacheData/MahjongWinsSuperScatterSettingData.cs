using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class MahjongWinsSuperScatterSettingData : SlotCommonGameSetting
    {
        /// <summary>從DB資料更新 遊戲特有的設定</summary>
        public override void GetDBDataUnique(Dictionary<string, string> uniqueSettinglist)
        {
            //Console.WriteLine("梅杜莎 從DB資料更新 遊戲特有的設定");
            //DBCache 須用到的資訊, 才須讀入
        }

        /// <summary>GameServer獲取遊戲 遊戲特有的設定值</summary>
        public override void GetSettingUnique(bool newTypeServer, Dictionary<string, string> datalist, Dictionary<string, string> uniqueSettinglist)
        {
            //Console.WriteLine("梅杜莎 GameServer獲取遊戲特有的設定值");
            //將 uniqueSettinglist 資訊加入 datalist
            try
            {
                if (newTypeServer)
                {
                    int smid = Convert.ToInt32(uniqueSettinglist["KiosMode"]);
                    if (KiosMode == smid)
                    {
                        foreach (var item in uniqueSettinglist)
                        {
                            if (item.Key != "SettingUID" && item.Key != "KiosMode")
                            {
                                datalist.Add(item.Key, item.Value);
                                //if(MachineUID == 93001) Console.WriteLine($"{MachineUID}[{uniqueSettinglist["MachineUID"]}]:  Key: {item.Key}, Value: {item.Value}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Get {this.GetType().Name} SettingUnique KiosMode Error: {KiosMode} != {smid}");
                    }
                }
                else
                {
                    int smid = Convert.ToInt32(uniqueSettinglist["MachineUID"]);
                    if (MachineUID == smid)
                    {
                        foreach (var item in uniqueSettinglist)
                        {
                            if (item.Key != "SettingUID" && item.Key != "MachineUID")
                            {
                                datalist.Add(item.Key, item.Value);
                                //if(MachineUID == 93001) Console.WriteLine($"{MachineUID}[{uniqueSettinglist["MachineUID"]}]:  Key: {item.Key}, Value: {item.Value}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Get {this.GetType().Name} SettingUnique MachineUID Error: {MachineUID} != {smid}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get {this.GetType().Name} SettingUnique Error: {ex.Message}");
            }
        }
    }
}
