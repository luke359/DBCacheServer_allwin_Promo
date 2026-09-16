using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class DayMissionBaseData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>群組碼</summary>
        public int GroupID { get; set; }
        /// <summary>任務號碼</summary>
        public int MissionID { get; set; }
        /// <summary>狀態 0未完成 1已完成 2已兌獎</summary>
        public int Status { get; set; }
        /// <summary>任務UID對應到TaskEdit</summary>
        public int Task_UID { get; set; }
        /// <summary>任務編碼</summary>
        public string TaskDetail { get; set; }
        /// <summary>備註</summary>
        public string TaskRemark { get; set; }
        /// <summary>獎勵UID對應到TaskAward</summary>
        public int Award_UID { get; set; }
        /// <summary>獎勵編碼</summary>
        public string AwardDetail { get; set; }
        /// <summary>備註</summary>
        public string AwardRemark { get; set; }
    }
}
