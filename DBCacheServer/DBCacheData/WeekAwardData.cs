using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    class WeekAwardData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>獎勵編號</summary>
        public int AwardNum { get; set; }
        /// <summary>獎勵代碼編號</summary>
        public int AwardUID { get; set; }
        /// <summary>啟用狀態</summary>
        public int Status { get; set; }
    }
}
