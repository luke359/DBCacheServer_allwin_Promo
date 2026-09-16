using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    class TicketBox
    {
        /// <summary>唯一碼</summary>
        public int TicketUID { get; set; }
        /// <summary>期號</summary>
        public string ISSNID { get; set; }
        /// <summary>玩家UID</summary>
        public int UserUID { get; set; }
        /// <summary>彩卷狀態</summary>
        public int Status { get; set; }
        /// <summary>彩卷號碼</summary>
        public string TicketNumber { get; set; }
        /// <summary>時間戳記</summary>
        public DateTime TimeStamp { get; set; }
    }
}
