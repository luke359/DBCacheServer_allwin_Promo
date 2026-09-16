using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class JPCountData
    {
        /// <summary>唯一碼</summary>
        public int JPCountUID { get; set; }
        /// <summary>Super表演用</summary>
        public double SuperShow { get; set; }
        /// <summary>Mega表演用</summary>
        public double MegaShow { get; set; }
        /// <summary>Major表演用</summary>
        public double MajorShow { get; set; }
        /// <summary>Minor表演用</summary>
        public double MinorShow { get; set; }
    }

    public class JPRealData
    {
        /// <summary>Super真實值</summary>
        public double RealSuper { get; set; }
        /// <summary>Mega真實值</summary>
        public double RealMega { get; set; }
        /// <summary>Major真實值</summary>
        public double RealMajor { get; set; }
        /// <summary>Minor真實值</summary>
        public double RealMinor { get; set; }

        /// <summary>Super計數器(正數)</summary>
        public int SuperTimer { get; set; }
        /// <summary>Mega計數器(正數)</summary>
        public int MegaTimer { get; set; }
        /// <summary>Major計數器(正數)</summary>
        public int MajorTimer { get; set; }
        /// <summary>Minor計數器(正數)</summary>
        public int MinorTimer { get; set; }

        /// <summary>Super出獎時間設定旗號</summary>
        public bool SuperActFg { get; set; }
        /// <summary>Super出獎時間</summary>
        public DateTime SuperOutTime { get; set; }
        /// <summary>Mega出獎時間設定旗號</summary>
        public bool MegaActFg { get; set; }
        /// <summary>Mega出獎時間</summary>
        public DateTime MegaOutTime { get; set; }
        /// <summary>Major出獎時間設定旗號</summary>
        public bool MajorActFg { get; set; }
        /// <summary>Major出獎時間</summary>
        public DateTime MajorOutTime { get; set; }
        /// <summary>Minor出獎時間設定旗號</summary>
        public bool MinorActFg { get; set; }
        /// <summary>Minor出獎時間</summary>
        public DateTime MinorOutTime { get; set; }
        /// <summary>金庫出獎時間設定旗號(0:停止 1:休息 2:出現)</summary>
        public int TreasuryActFg { get; set; }
        /// <summary>金庫休息時間</summary>
        public DateTime TreasuryCountdownTime { get; set; }
        /// <summary>金庫出獎時間</summary>
        public DateTime TreasuryWinTime { get; set; }
        /// <summary>金庫放出預報</summary>
        public int TreasuryStatus { get; set; }
    }
}
