using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class TaskEditData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>任務編碼</summary>
        public string TaskDetail { get; set; }
        /// <summary>任務編碼</summary>
        public int Status { get; set; }
        /// <summary>備註</summary>
        public string Remark { get; set; }
    }
}
