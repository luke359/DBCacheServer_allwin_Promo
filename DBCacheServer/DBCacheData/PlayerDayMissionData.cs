using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class PlayerDayMissionData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>玩家UID</summary>
        public int UserUID { get; set; }
        /// <summary>每日任務UID對應DayMissionBaseTable</summary>
        public int DayMissionUID { get; set; }
        /// <summary>群組碼</summary>
        public int GroupID { get; set; }
        /// <summary>任務號碼</summary>
        public int MissionID { get; set; }
        /// <summary>狀態 0未完成 1已完成 2已兌獎</summary>
        public int Status { get; set; }
        /// <summary>任務編碼</summary>
        public string TaskDetail { get; set; }
        /// <summary>獎勵編碼</summary>
        public string AwardDetail { get; set; }
        /// <summary>時間戳記</summary>
        public DateTime BuildTime { get; set; }
    }
}
