using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class PlayerWeekMissionTable
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>玩家UID</summary>
        public int UserUID { get; set; }
        /// <summary>日期紀錄</summary>
        public string DayLog { get; set; }
        /// <summary>任務計數</summary>
        public int MissionCount { get; set; }
        /// <summary>狀態 0未完成 1已完成 2已兌獎</summary>
        public int Status { get; set; }
        /// <summary>時間戳記</summary>
        public DateTime TimeStamp { get; set; }
    }
}
