using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class MachineRankData
    {
        /// <summary>唯一碼</summary>
        public int RecordUid { get; set; }
        /// <summary>遊戲機台</summary>
        //public int MachineUID { get; set; }
        /// <summary>遊戲名稱</summary>
        public string RankData { get; set; }
        /// <summary>大獎資訊</summary>
        public string CountList { get; set; }
        /// <summary>創建時間</summary>
        public string Buildtime { get; set; }
    }
}
