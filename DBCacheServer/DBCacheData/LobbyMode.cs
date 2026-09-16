using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    class LobbyMode
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>遊戲名稱</summary>
        public string Mode { get; set; }
        /// <summary>玩家機台資料保留時間10~1440(分鐘)</summary>
        public int PlayerDataReserveTime { get; set; }
    }
}
