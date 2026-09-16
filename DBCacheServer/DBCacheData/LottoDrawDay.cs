using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class LottoDrawDay
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>開獎日</summary>
        public string DrawDay { get; set; }
        /// <summary>同號限制張數</summary>
        public int LimitPiece { get; set; }
        /// <summary>封牌時間</summary>
        public string ClosureTime { get; set; }
    }
}
