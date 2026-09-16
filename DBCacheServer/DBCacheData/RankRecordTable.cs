using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class RankRecordTable
    {
        /// <summary>唯一碼</summary>
        public int RankUID;
        /// <summary>玩家UID</summary>
        public int UserUID;
        /// <summary>總押</summary>
        public decimal TotalBet;
        /// <summary>總得</summary>
        public decimal TotalWin;
        /// <summary>累計總押</summary>
        public decimal GrandTotalBet;
        /// <summary>累計總得</summary>
        public decimal GrandTotalWin;
        /// <summary>更新旗號(資料已變更)</summary>
        public bool update;
        /// <summary>創建時間</summary>
        //public string Buildtime;


        /// <summary>Construction</summary>
        public void SetRankRecordTable(int rankUID, int userUID, decimal betAdd, decimal winAdd, decimal grandBetAdd, decimal grandWinAdd)
        {
            RankUID = rankUID;
            UserUID = userUID;
            TotalBet = betAdd;
            TotalWin = winAdd;
            GrandTotalBet = grandBetAdd;
            GrandTotalWin = grandWinAdd;
            //update = false;
        }

        /// <summary>累計更新遊戲紀錄內存</summary>
        public void AccumulatecGameData(double betAdd, double winAdd)
        {
            TotalBet += (decimal)betAdd;
            GrandTotalBet += (decimal)betAdd;
            TotalWin += (decimal)winAdd;
            GrandTotalWin += (decimal)winAdd;
            update = true;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            RankUID = Convert.ToInt32(datalist["RankUID"]);
            UserUID = Convert.ToInt32(datalist["UserUID"]);
            TotalBet = Convert.ToDecimal(datalist["TotalBet"]);
            TotalWin = Convert.ToDecimal(datalist["TotalWin"]);
            GrandTotalBet = Convert.ToDecimal(datalist["GrandTotalBet"]);
            GrandTotalWin = Convert.ToDecimal(datalist["GrandTotalWin"]);
            update = false;
        }

        /// <summary>重置玩家押分排行資訊</summary>
        public void ResetRankRecord()
        {
            TotalBet = 0;
            TotalWin = 0;
        }
        /// <summary>重置玩家押分排行資訊 (玩家中JP彩金時)</summary>
        public void ResetRankRecord(double jpWin)
        {
            GrandTotalWin += (decimal)jpWin;
            TotalBet = 0;
            TotalWin = 0;
            update = true;
        }

        /// <summary>依據內存製作更新字典</summary>
        public Dictionary<string, string> GetUpdateData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();
            updata.Add("UserUID", UserUID.ToString());
            updata.Add("TotalBet", TotalBet.ToString());
            updata.Add("TotalWin", TotalWin.ToString());
            updata.Add("GrandTotalBet", GrandTotalBet.ToString());
            updata.Add("GrandTotalWin", GrandTotalWin.ToString());
            return updata;
        }
    }
}
