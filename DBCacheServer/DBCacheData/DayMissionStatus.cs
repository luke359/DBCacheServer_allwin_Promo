using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    class DayMissionStatus
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>群組碼</summary>
        public int GroupID { get; set; }
        /// <summary>時間戳記</summary>
        public DateTime TimeStamp { get; set; }
    }
}
