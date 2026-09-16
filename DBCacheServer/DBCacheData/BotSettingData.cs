using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class BotSettingData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>0短休 1長休 2不休 3混和</summary>
        public int RestType { get; set; }
        /// <summary>行為模式0~13</summary>
        public int ModeType { get; set; }
        /// <summary>空位數</summary>
        public int EmptSeatNum { get; set; }
    }
}
