using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class TaskAwardData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>獎勵編碼</summary>
        public string AwardDetail { get; set; }
        /// <summary>備註</summary>
        public string Remark { get; set; }
    }
}
