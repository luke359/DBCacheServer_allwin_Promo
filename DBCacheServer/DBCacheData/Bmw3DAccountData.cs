using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class Bmw3DAccountData
    {
        /// <summary>唯一碼</summary>
        public int AccountUID { get; set; }
        /// <summary>機台唯一碼</summary>
        public int MachineUID { get; set; }
        /// <summary>總押</summary>
        public double TotalBet { get; set; }
        /// <summary>總贏</summary>
        public double TotalWin { get; set; }
        /// <summary>總盈餘</summary>
        public double TotalSurplus { get; set; }
        /// <summary>遊戲次數</summary>
        public int GameTimes { get; set; }
        /// <summary>贏的次數</summary>
        public int WinTimes { get; set; }

        public int Super_Times { get; set; }
        public double Super_Bet { get; set; }
        public double Super_Win { get; set; }
        public int Mega_Times { get; set; }
        public double Mega_Bet { get; set; }
        public double Mega_Win { get; set; }
        public int Major_Times { get; set; }
        public double Major_Bet { get; set; }
        public double Major_Win { get; set; }
        public int Minor_Times { get; set; }
        public double Minor_Bet { get; set; }
        public double Minor_Win { get; set; }

        public int Special_Times { get; set; }
        public int SuperBonus_Times { get; set; }
        public int LuckyShoot_Times { get; set; }
        public int Celebrate_Times { get; set; }
        public int MaxOdds_Times { get; set; }
        public int RBenz_Times { get; set; }
        public int GBenz_Times { get; set; }
        public int YBenz_Times { get; set; }
        public int RBMW_Times { get; set; }
        public int GBMW_Times { get; set; }
        public int YBMW_Times { get; set; }
        public int RAudi_Times { get; set; }
        public int GAudi_Times { get; set; }
        public int YAudi_Times { get; set; }
        public int RJetta_Times { get; set; }
        public int GJetta_Times { get; set; }
        public int YJetta_Times { get; set; }

        /// <summary>內帳或外帳</summary>
        public int AccountType { get; set; }
        /// <summary>紀錄時間(日帳使用)</summary>
        public DateTime RecDate { get; set; }
        /// <summary>更新旗號(資料已變更)</summary>
        public bool update { get; set; }

        /// <summary>累計更新遊戲紀錄內存</summary>
        public void AccumulatecGameData(Dictionary<string, string> refdata)
        {
            double betAdd = Convert.ToDouble(refdata["TotalBet"]);
            double winAdd = Convert.ToDouble(refdata["TotalWin"]);

            if (betAdd > 0)
            {
                TotalBet += betAdd;
                GameTimes++;
            }
            if (winAdd > 0)
            {
                TotalWin += winAdd;
                WinTimes++;
            }
            TotalSurplus = TotalBet - TotalWin;

            foreach (KeyValuePair<string, string> info in refdata)
            {
                AccumulatecCacheData(info.Key, info.Value);
            }

            update = true;
        }
        /// <summary>累計更新出牌紀錄內存</summary>
        public void AccumulatecCacheData(string field, string value)
        {
            if (field == "Super_Times") Super_Times += Convert.ToInt32(value);
            else if (field == "Super_Bet") Super_Bet += Convert.ToDouble(value);
            else if (field == "Super_Win") Super_Win += Convert.ToDouble(value);

            else if (field == "Mega_Times") Mega_Times += Convert.ToInt32(value);
            else if (field == "Mega_Bet") Mega_Bet += Convert.ToDouble(value);
            else if (field == "Mega_Win") Mega_Win += Convert.ToDouble(value);

            else if (field == "Major_Times") Major_Times += Convert.ToInt32(value);
            else if (field == "Major_Bet") Major_Bet += Convert.ToDouble(value);
            else if (field == "Major_Win") Major_Win += Convert.ToDouble(value);

            else if (field == "Minor_Times") Minor_Times += Convert.ToInt32(value);
            else if (field == "Minor_Bet") Minor_Bet += Convert.ToDouble(value);
            else if (field == "Minor_Win") Minor_Win += Convert.ToDouble(value);

            else if (field == "Special_Times") Special_Times += Convert.ToInt32(value);
            else if (field == "SuperBonus_Times") SuperBonus_Times += Convert.ToInt32(value);
            else if (field == "LuckyShoot_Times") LuckyShoot_Times += Convert.ToInt32(value);
            else if (field == "Celebrate_Times") Celebrate_Times += Convert.ToInt32(value);
            else if (field == "MaxOdds_Times") MaxOdds_Times += Convert.ToInt32(value);
            else if (field == "RBenz_Times") RBenz_Times += Convert.ToInt32(value);
            else if (field == "GBenz_Times") GBenz_Times += Convert.ToInt32(value);
            else if (field == "YBenz_Times") YBenz_Times += Convert.ToInt32(value);
            else if (field == "RBMW_Times") RBMW_Times += Convert.ToInt32(value);
            else if (field == "GBMW_Times") GBMW_Times += Convert.ToInt32(value);
            else if (field == "YBMW_Times") YBMW_Times += Convert.ToInt32(value);
            else if (field == "RAudi_Times") RAudi_Times += Convert.ToInt32(value);
            else if (field == "GAudi_Times") GAudi_Times += Convert.ToInt32(value);
            else if (field == "YAudi_Times") YAudi_Times += Convert.ToInt32(value);
            else if (field == "RJetta_Times") RJetta_Times += Convert.ToInt32(value);
            else if (field == "GJetta_Times") GJetta_Times += Convert.ToInt32(value);
            else if (field == "YJetta_Times") YJetta_Times += Convert.ToInt32(value);
        }
        /// <summary>累計更新JP內存</summary>
        public void AccumulatecJPData(JpAward jPType, double jPBet, double jPWin)
        {
            if (jPWin > 0)
            {
                TotalWin += jPWin;
                TotalSurplus = TotalBet - TotalWin;
            }

            if (jPType == JpAward.MINOR)
            {
                Minor_Times += 1;
                Minor_Bet += jPBet;
                Minor_Win += jPWin;
            }
            else if (jPType == JpAward.MAJOR)
            {
                Major_Times += 1;
                Major_Bet += jPBet;
                Major_Win += jPWin;
            }
            else if (jPType == JpAward.MEGA)
            {
                Mega_Times += 1;
                Mega_Bet += jPBet;
                Mega_Win += jPWin;
            }
            else if (jPType == JpAward.SUPER)
            {
                Super_Times += 1;
                Super_Bet += jPBet;
                Super_Win += jPWin;
            }
        }

        /// <summary>依據內存製作更新字典</summary>
        public Dictionary<string, string> GetUpdateData(bool detail)
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();

            updata.Add("TotalBet", TotalBet.ToString());
            updata.Add("TotalWin", TotalWin.ToString());
            updata.Add("TotalSurplus", TotalSurplus.ToString());
            updata.Add("GameTimes", GameTimes.ToString());
            updata.Add("WinTimes", WinTimes.ToString());

            updata.Add("Super_Times", Super_Times.ToString());
            updata.Add("Super_Bet", Super_Bet.ToString());
            updata.Add("Super_Win", Super_Win.ToString());
            updata.Add("Mega_Times", Mega_Times.ToString());
            updata.Add("Mega_Bet", Mega_Bet.ToString());
            updata.Add("Mega_Win", Mega_Win.ToString());
            updata.Add("Major_Times", Major_Times.ToString());
            updata.Add("Major_Bet", Major_Bet.ToString());
            updata.Add("Major_Win", Major_Win.ToString());
            updata.Add("Minor_Times", Minor_Times.ToString());
            updata.Add("Minor_Bet", Minor_Bet.ToString());
            updata.Add("Minor_Win", Minor_Win.ToString());

            updata.Add("Special_Times", Special_Times.ToString());
            updata.Add("SuperBonus_Times", SuperBonus_Times.ToString());
            updata.Add("LuckyShoot_Times", LuckyShoot_Times.ToString());
            updata.Add("Celebrate_Times", Celebrate_Times.ToString());
            updata.Add("MaxOdds_Times", MaxOdds_Times.ToString());
            updata.Add("RBenz_Times", RBenz_Times.ToString());
            updata.Add("GBenz_Times", GBenz_Times.ToString());
            updata.Add("YBenz_Times", YBenz_Times.ToString());
            updata.Add("RBMW_Times", RBMW_Times.ToString());
            updata.Add("GBMW_Times", GBMW_Times.ToString());
            updata.Add("YBMW_Times", YBMW_Times.ToString());
            updata.Add("RAudi_Times", RAudi_Times.ToString());
            updata.Add("GAudi_Times", GAudi_Times.ToString());
            updata.Add("YAudi_Times", YAudi_Times.ToString());
            updata.Add("RJetta_Times", RJetta_Times.ToString());
            updata.Add("GJetta_Times", GJetta_Times.ToString());
            updata.Add("YJetta_Times", YJetta_Times.ToString());

            if (detail)
            {
                updata.Add("RecDate", "'" + RecDate.ToString("yyyy-MM-dd") + "'");
            }
            else
            {
                updata.Add("RecDate", "'2021-01-01'"); //非detail此欄用不到, 增加此欄位是為了共用DB"更新表單"函式.
            }
            return updata;
        }
        /// <summary>清除內存</summary>
        public void ClearCache()
        {
            TotalBet = 0;
            TotalWin = 0;
            TotalSurplus = 0;
            GameTimes = 0;
            WinTimes = 0;

            Super_Times = 0;
            Super_Bet = 0;
            Super_Win = 0;
            Mega_Times = 0;
            Mega_Bet = 0;
            Mega_Win = 0;
            Major_Times = 0;
            Major_Bet = 0;
            Major_Win = 0;
            Minor_Times = 0;
            Minor_Bet = 0;
            Minor_Win = 0;

            Special_Times = 0;
            SuperBonus_Times = 0;
            LuckyShoot_Times = 0;
            Celebrate_Times = 0;
            MaxOdds_Times = 0;
            RBenz_Times = 0;
            GBenz_Times = 0;
            YBenz_Times = 0;
            RBMW_Times = 0;
            GBMW_Times = 0;
            YBMW_Times = 0;
            RAudi_Times = 0;
            GAudi_Times = 0;
            YAudi_Times = 0;
            RJetta_Times = 0;
            GJetta_Times = 0;
            YJetta_Times = 0;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist, bool detail)
        {
            if (detail)
            {
                AccountUID = Convert.ToInt32(datalist["AccountUID"]);
                RecDate = Convert.ToDateTime(datalist["RecDate"]).Date;
            }
            else
            {
                AccountUID = Convert.ToInt32(datalist["AccountUID"]);
            }

            MachineUID = Convert.ToInt32(datalist["MachineUID"]);
            TotalBet = Convert.ToDouble(datalist["TotalBet"]);
            TotalWin = Convert.ToDouble(datalist["TotalWin"]);
            TotalSurplus = Convert.ToDouble(datalist["TotalSurplus"]);
            GameTimes = Convert.ToInt32(datalist["GameTimes"]);
            WinTimes = Convert.ToInt32(datalist["WinTimes"]);

            Super_Times = Convert.ToInt32(datalist["Super_Times"]);
            Super_Bet = Convert.ToDouble(datalist["Super_Bet"]);
            Super_Win = Convert.ToDouble(datalist["Super_Win"]);
            Mega_Times = Convert.ToInt32(datalist["Mega_Times"]);
            Mega_Bet = Convert.ToDouble(datalist["Mega_Bet"]);
            Mega_Win = Convert.ToDouble(datalist["Mega_Win"]);
            Major_Times = Convert.ToInt32(datalist["Major_Times"]);
            Major_Bet = Convert.ToDouble(datalist["Major_Bet"]);
            Major_Win = Convert.ToDouble(datalist["Major_Win"]);
            Minor_Times = Convert.ToInt32(datalist["Minor_Times"]);
            Minor_Bet = Convert.ToDouble(datalist["Minor_Bet"]);
            Minor_Win = Convert.ToDouble(datalist["Minor_Win"]);

            Special_Times = Convert.ToInt32(datalist["Special_Times"]);
            SuperBonus_Times = Convert.ToInt32(datalist["SuperBonus_Times"]);
            LuckyShoot_Times = Convert.ToInt32(datalist["LuckyShoot_Times"]);
            Celebrate_Times = Convert.ToInt32(datalist["Celebrate_Times"]);
            MaxOdds_Times = Convert.ToInt32(datalist["MaxOdds_Times"]);
            RBenz_Times = Convert.ToInt32(datalist["RBenz_Times"]);
            GBenz_Times = Convert.ToInt32(datalist["GBenz_Times"]);
            YBenz_Times = Convert.ToInt32(datalist["YBenz_Times"]);
            RBMW_Times = Convert.ToInt32(datalist["RBMW_Times"]);
            GBMW_Times = Convert.ToInt32(datalist["GBMW_Times"]);
            YBMW_Times = Convert.ToInt32(datalist["YBMW_Times"]);
            RAudi_Times = Convert.ToInt32(datalist["RAudi_Times"]);
            GAudi_Times = Convert.ToInt32(datalist["GAudi_Times"]);
            YAudi_Times = Convert.ToInt32(datalist["YAudi_Times"]);
            RJetta_Times = Convert.ToInt32(datalist["RJetta_Times"]);
            GJetta_Times = Convert.ToInt32(datalist["GJetta_Times"]);
            YJetta_Times = Convert.ToInt32(datalist["YJetta_Times"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);
            update = false;
        }
    }
}
