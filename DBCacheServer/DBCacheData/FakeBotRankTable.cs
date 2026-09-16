using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class FakeBotRankTable
    {
        /// <summary>唯一碼</summary>
        public int RecordUid { get; set; }
        /// <summary>玩家唯一碼</summary>
        //public int UserUID { get; set; }
        /// <summary>遊戲機台</summary>
        public int GameMachine { get; set; }
        /// <summary>遊戲名稱</summary>
        public string GameName { get; set; }
        /// <summary>大獎種類</summary>
        public int RankType { get; set; }
        /// <summary>大獎資訊</summary>
        public string RankData { get; set; }
        /// <summary>創建時間</summary>
        public string BuildTime { get; set; }
        /// <summary>排序用時間</summary>
        public int BuildSecond { get; set; }
    }
}
