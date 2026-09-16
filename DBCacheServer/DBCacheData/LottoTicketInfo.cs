using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    class LottoTicketInfo
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>期號</summary>
        public string ISSNID { get; set; }
        /// <summary>同號限制張數</summary>
        public int LimitPiece { get; set; }
        /// <summary>頭獎號碼</summary>
        public string FirstPrize { get; set; }
        /// <summary>頭獎彩金</summary>
        public double FirstBonus { get; set; }
        /// <summary>二獎號碼</summary>
        public string SecondPrize { get; set; }
        /// <summary>二獎彩金</summary>
        public double SecondBonus { get; set; }
        /// <summary>三獎號碼</summary>
        public string ThirdPrize { get; set; }
        /// <summary>三獎彩金</summary>
        public double ThirdBonus { get; set; }
        /// <summary>特獎號碼</summary>
        public string SpecialPrize { get; set; }
        /// <summary>特獎彩金</summary>
        public double SpecialBonus { get; set; }
        /// <summary>封牌時間</summary>
        public DateTime ClosureTime { get; set; }
        /// <summary>開獎旗標 0未開 1已開 </summary>
        public int AlreadyDraw { get; set; }
    }
}
