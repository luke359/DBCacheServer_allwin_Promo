using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class JPAcoountData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>類型</summary>
        public int JpAward { get; set; }
        /// <summary>類型</summary>
        //public string JPType { get; set; }
        /// <summary>遊戲名稱</summary>
        public string GameName { get; set; }
        /// <summary>遊戲類型</summary>
        public string GameType { get; set; }
        /// <summary>贏分</summary>
        public double TotalWin { get; set; }
        /// <summary>玩家</summary>
        public string User { get; set; }
        /// <summary>時間</summary>
        public string BuildTime { get; set; }
        /// <summary>玩家IP</summary>
        public string PlayerIP { get; set; }
        /// <summary>出牌類型 (0假出 1真出 2逼牌)</summary>
        public JpLotteryType LotteryType { get; set; }
        /// <summary>出牌處</summary>
        public JpLotteryCast LotteryCast { get; set; }

        //public Dictionary<string,string> GetJPAcoountData()
        //{
        //    Dictionary<string, string> tempdata = new Dictionary<string, string>();
        //    tempdata.Add("UID", UID.ToString());
        //    tempdata.Add("JpAward", JpAward.ToString());
        //    tempdata.Add("JpType", JPType);
        //    tempdata.Add("GameName", GameName);
        //    tempdata.Add("GameType", GameType);
        //    tempdata.Add("TotalWin", TotalWin.ToString());
        //    tempdata.Add("UserID", User);
        //    tempdata.Add("Buildtime", BuildTime);
        //    tempdata.Add("PlayerIP", PlayerIP);
        //    tempdata.Add("LotteryType", LotteryType.ToString());
        //    tempdata.Add("LotteryCast", LotteryCast.ToString());
        //
        //    return tempdata;
        //}
    }
}
