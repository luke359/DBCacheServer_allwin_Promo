using Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class ReportGameData
    {
        /// <summary>唯一碼</summary>
        public int UserGameUID { get; set; }
        /// <summary>玩家唯一碼</summary>
        public int UserUID { get; set; }
        /// <summary>遊戲名稱</summary>
        public string GameName { get; set; }
        /// <summary>遊戲類型</summary>
        public string GameType { get; set; }
        /// <summary>總押</summary>
        public decimal TotalBet { get; set; }
        /// <summary>總贏</summary>
        public decimal TotalWin { get; set; }
        /// <summary>JP總贏</summary>
        public decimal JpWin { get; set; }
        /// <summary>遊戲局數</summary>
        public int Rounds { get; set; }
        /// <summary>遊戲紅包</summary>
        public double GameRedEnvelope { get; set; }
        /// <summary>任務紅包</summary>
        public double MissionRedEnvelope { get; set; }
        /// <summary>彩券得幣</summary>
        public double LottoTicketBonus { get; set; }
        /// <summary>大廳彩金得幣</summary>
        public double LobbyBonus { get; set; }
        /// <summary>FreeRound贏金</summary>
        public double FreeRoundWin { get; set; }
        /// <summary>創建時間</summary>
        public DateTime Buildtime { get; set; }
        /// <summary>更新旗號(資料已變更)</summary>
        public bool update;


        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            UserGameUID = Convert.ToInt32(datalist["UserGameUID"]);
            UserUID = Convert.ToInt32(datalist["UserUID"]);
            GameName = datalist["GameName"];
            GameType = datalist["GameType"];
            TotalBet = Convert.ToDecimal(datalist["TotalBet"]);
            TotalWin = Convert.ToDecimal(datalist["TotalWin"]);
            JpWin = Convert.ToDecimal(datalist["JpWin"]);
            Rounds = Convert.ToInt32(datalist["Rounds"]);
            GameRedEnvelope = Convert.ToDouble(datalist["GameRedEnvelope"]);
            MissionRedEnvelope = Convert.ToDouble(datalist["MissionRedEnvelope"]);
            LottoTicketBonus = Convert.ToDouble(datalist["LottoTicketBonus"]);
            LobbyBonus = Convert.ToDouble(datalist["LobbyBonus"]);
            FreeRoundWin = Convert.ToDouble(datalist["FreeRoundWin"]);
            Buildtime = Convert.ToDateTime(datalist["Buildtime"]);
            update = false;
        }

        /// <summary>依據內存製作更新字典</summary>
        public Dictionary<string, string> GetUpdateData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();
            
            updata.Add("UserUID", UserUID.ToString());
            updata.Add("GameName", "'" + GameName + "'");
            updata.Add("GameType", "'" + GameType + "'");
            updata.Add("TotalBet", TotalBet.ToString());
            updata.Add("TotalWin", TotalWin.ToString());
            updata.Add("JpWin", JpWin.ToString());
            updata.Add("Rounds", Rounds.ToString("f2"));
            updata.Add("GameRedEnvelope", GameRedEnvelope.ToString("f2"));
            updata.Add("MissionRedEnvelope", MissionRedEnvelope.ToString("f2"));
            updata.Add("LottoTicketBonus", LottoTicketBonus.ToString("f2"));
            updata.Add("LobbyBonus", LobbyBonus.ToString("f2"));
            updata.Add("FreeRoundWin", FreeRoundWin.ToString("f2"));

            return updata;
        }
    }

    public class ReportGameDataRec
    {
        /// <summary>報表記錄唯一碼</summary>
        public int UserGameUID { get; set; }
        /// <summary>遊戲名稱</summary>
        //public string GameName { get; set; }
        /// <summary>遊戲名稱</summary>
        public GameServerCode GameServerCode { get; set; }
        /// <summary>創建時間</summary>
        public DateTime Buildtime { get; set; }
    }
}
