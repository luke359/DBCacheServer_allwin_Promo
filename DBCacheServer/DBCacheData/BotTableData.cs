using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class BotTableData
    {
        /// <summary>唯一碼</summary>
        public int BotUID { get; set; }
        /// <summary>玩家表唯一碼</summary>
        public int UserUID { get; set; }
        /// <summary>遊戲名稱</summary>
        public string GameName { get; set; }
        /// <summary>機台UID</summary>
        public int MachineUID { get; set; }
        /// <summary>座位</summary>
        public int Seat { get; set; }
        /// <summary>機器人開關</summary>
        public bool IsEnabled { get; set; }
        /// <summary>機器人是否入帳</summary>
        public bool IsBotBill { get; set; }

        public Dictionary<string,string> GetBotTableData()
        {
            var tempdata = new Dictionary<string, string>();
            tempdata.Add("BotUID", BotUID.ToString());
            tempdata.Add("UserUID", UserUID.ToString());
            tempdata.Add("GameName", GameName);
            tempdata.Add("MachineUID", MachineUID.ToString());
            tempdata.Add("Seat", Seat.ToString());
            tempdata.Add("IsEnabled", IsEnabled.ToString());
            tempdata.Add("IsBotBill", IsBotBill.ToString());

            return tempdata;
        }
    }
}
