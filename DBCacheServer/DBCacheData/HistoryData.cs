using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class HistoryData
    {
        /// <summary>唯一碼</summary>
        public int HistoryUID { get; set; }
        /// <summary>機台唯一碼</summary>
        public int MachineUID { get; set; }
        /// <summary>獎項資訊</summary>
        public string PrizeData { get; set; }
    }
}
