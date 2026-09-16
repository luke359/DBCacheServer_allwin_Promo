using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class UserSessionAccount
    {
        /// <summary>唯一碼</summary>
        public int AccountUID { get; set; }
        /// <summary>ID</summary>
        public int UserUID { get; set; }
        /// <summary></summary>
        public int SessionID { get; set; }
        /// <summary></summary>
        public int TotalRounds { get; set; }
        /// <summary></summary>
        public decimal TotalBet { get; set; }
        /// <summary></summary>
        public decimal TotalWin { get; set; }
        /// <summary></summary>
        public decimal JpWin { get; set; }
        /// <summary></summary>
        public string IP { get; set; }
        /// <summary></summary>
        public string Buildtime { get; set; }
        /// <summary>更新旗號(資料已變更)</summary>
        public bool update { get; set; }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            AccountUID = Convert.ToInt32(datalist["AccountUID"]);
            UserUID = Convert.ToInt32(datalist["UserUID"]);
            SessionID = Convert.ToInt32(datalist["SessionID"]);
            TotalRounds = Convert.ToInt32(datalist["TotalRounds"]);
            TotalBet = Convert.ToDecimal(datalist["TotalBet"]);
            TotalWin = Convert.ToDecimal(datalist["TotalWin"]);
            JpWin = Convert.ToDecimal(datalist["JpWin"]);
            IP = datalist["IP"];
            Buildtime = datalist["Buildtime"];
            update = false;
        }

        /// <summary>依據內存製作更新字典</summary>
        public Dictionary<string, string> GetUpdateData()
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();

            Data.Add("UserUID", UserUID.ToString());
            Data.Add("SessionID", "'" + SessionID.ToString() + "'");
            Data.Add("TotalRounds", "'" + TotalRounds.ToString() + "'");
            Data.Add("TotalBet", TotalBet.ToString());
            Data.Add("TotalWin", TotalWin.ToString());
            Data.Add("JpWin", JpWin.ToString());
            Data.Add("IP", "'" + IP.ToString() + "'");

            return Data;
        }
    }
}
