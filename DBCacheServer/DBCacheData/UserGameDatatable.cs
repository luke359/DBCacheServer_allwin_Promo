using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class UserGameData
    {
        /// <summary>唯一碼</summary>
        public int UserGameUID { get; set; }
        /// <summary>玩家唯一碼</summary>
        public int UserUID { get; set; }
        /// <summary>玩家流水號</summary>
        public long SerialNumber { get; set; }
        /// <summary>玩家登入號</summary>
        public int SessionID { get; set; }
        /// <summary>遊戲機台</summary>
        public int GameMachine { get; set; }
        /// <summary>遊戲名稱</summary>
        public string GameName { get; set; }
        /// <summary>遊戲類型</summary>
        public string GameType { get; set; }
        /// <summary>獎項資訊</summary>
        public string PrizeData { get; set; }
        /// <summary>JP出牌紀錄UID</summary>
        public int JpAccountUID { get; set; }
        /// <summary>押注資訊</summary>
        public string UserBetData { get; set; }
        /// <summary>押注前分數</summary>
        public double Balancebefore { get; set; }
        /// <summary>總押</summary>
        public double TotalBet { get; set; }
        /// <summary>總贏</summary>
        public double TotalWin { get; set; }
        /// <summary>押注後分數</summary>
        public double Balanceafter { get; set; }
        /// <summary>玩家IP</summary>
        public string UserIP { get; set; }
        /// <summary>大獎種類</summary>
        public int RankType { get; set; }
        /// <summary>大獎資訊</summary>
        public string RankData { get; set; }
        /// <summary>創建時間</summary>
        public string BuildTime { get; set; }
    }
}
