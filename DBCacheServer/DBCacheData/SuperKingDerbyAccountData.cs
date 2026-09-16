using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class SuperKingDerbyAccountData
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

        public int JP5_Times { get; set; }
        public int JP4_Times { get; set; }
        public int JP3_Times { get; set; }
        public int JP2_Times { get; set; }
        public int H6_Times { get; set; }
        public int H5_Times { get; set; }
        public int H4_Times { get; set; }
        public int H3_Times { get; set; }
        public int D1000_Times { get; set; }
        public int D500_Times { get; set; }
        public int D200_Times { get; set; }
        public int D175_Times { get; set; }
        public int D125_Times { get; set; }
        public int D100_Times { get; set; }
        public int D80_Times { get; set; }
        public int D60_Times { get; set; }
        public int D30_Times { get; set; }
        public int D20_Times { get; set; }
        public int D10_Times { get; set; }
        public int D8_Times { get; set; }
        public int D5_Times { get; set; }
        public int D4_Times { get; set; }
        public int D3_Times { get; set; }

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

            else if (field == "JP5_Times") JP5_Times += Convert.ToInt32(value);
            else if (field == "JP4_Times") JP4_Times += Convert.ToInt32(value);
            else if (field == "JP3_Times") JP3_Times += Convert.ToInt32(value);
            else if (field == "JP2_Times") JP2_Times += Convert.ToInt32(value);
            else if (field == "H6_Times") H6_Times += Convert.ToInt32(value);
            else if (field == "H5_Times") H5_Times += Convert.ToInt32(value);
            else if (field == "H4_Times") H4_Times += Convert.ToInt32(value);
            else if (field == "H3_Times") H3_Times += Convert.ToInt32(value);
            else if (field == "D1000_Times") D1000_Times += Convert.ToInt32(value);
            else if (field == "D500_Times") D500_Times += Convert.ToInt32(value);
            else if (field == "D200_Times") D200_Times += Convert.ToInt32(value);
            else if (field == "D175_Times") D175_Times += Convert.ToInt32(value);
            else if (field == "D125_Times") D125_Times += Convert.ToInt32(value);
            else if (field == "D100_Times") D100_Times += Convert.ToInt32(value);
            else if (field == "D80_Times") D80_Times += Convert.ToInt32(value);
            else if (field == "D60_Times") D60_Times += Convert.ToInt32(value);
            else if (field == "D30_Times") D30_Times += Convert.ToInt32(value);
            else if (field == "D20_Times") D20_Times += Convert.ToInt32(value);
            else if (field == "D10_Times") D10_Times += Convert.ToInt32(value);
            else if (field == "D8_Times") D8_Times += Convert.ToInt32(value);
            else if (field == "D5_Times") D5_Times += Convert.ToInt32(value);
            else if (field == "D4_Times") D4_Times += Convert.ToInt32(value);
            else if (field == "D3_Times") D3_Times += Convert.ToInt32(value);
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

            updata.Add("JP5_Times", JP5_Times.ToString());
            updata.Add("JP4_Times", JP4_Times.ToString());
            updata.Add("JP3_Times", JP3_Times.ToString());
            updata.Add("JP2_Times", JP2_Times.ToString());
            updata.Add("H6_Times", H6_Times.ToString());
            updata.Add("H5_Times", H5_Times.ToString());
            updata.Add("H4_Times", H4_Times.ToString());
            updata.Add("H3_Times", H3_Times.ToString());
            updata.Add("D1000_Times", D1000_Times.ToString());
            updata.Add("D500_Times", D500_Times.ToString());
            updata.Add("D200_Times", D200_Times.ToString());
            updata.Add("D175_Times", D175_Times.ToString());
            updata.Add("D125_Times", D125_Times.ToString());
            updata.Add("D100_Times", D100_Times.ToString());
            updata.Add("D80_Times", D80_Times.ToString());
            updata.Add("D60_Times", D60_Times.ToString());
            updata.Add("D30_Times", D30_Times.ToString());
            updata.Add("D20_Times", D20_Times.ToString());
            updata.Add("D10_Times", D10_Times.ToString());
            updata.Add("D8_Times", D8_Times.ToString());
            updata.Add("D5_Times", D5_Times.ToString());
            updata.Add("D4_Times", D4_Times.ToString());
            updata.Add("D3_Times", D3_Times.ToString());

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

            JP5_Times = 0;
            JP4_Times = 0;
            JP3_Times = 0;
            JP2_Times = 0;
            H6_Times = 0;
            H5_Times = 0;
            H4_Times = 0;
            H3_Times = 0;
            D1000_Times = 0;
            D500_Times = 0;
            D200_Times = 0;
            D175_Times = 0;
            D125_Times = 0;
            D100_Times = 0;
            D80_Times = 0;
            D60_Times = 0;
            D30_Times = 0;
            D20_Times = 0;
            D10_Times = 0;
            D8_Times = 0;
            D5_Times = 0;
            D4_Times = 0;
            D3_Times = 0;
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

            JP5_Times = Convert.ToInt32(datalist["JP5_Times"]);
            JP4_Times = Convert.ToInt32(datalist["JP4_Times"]);
            JP3_Times = Convert.ToInt32(datalist["JP3_Times"]);
            JP2_Times = Convert.ToInt32(datalist["JP2_Times"]);
            H6_Times = Convert.ToInt32(datalist["H6_Times"]);
            H5_Times = Convert.ToInt32(datalist["H5_Times"]);
            H4_Times = Convert.ToInt32(datalist["H4_Times"]);
            H3_Times = Convert.ToInt32(datalist["H3_Times"]);
            D1000_Times = Convert.ToInt32(datalist["D1000_Times"]);
            D500_Times = Convert.ToInt32(datalist["D500_Times"]);
            D200_Times = Convert.ToInt32(datalist["D200_Times"]);
            D175_Times = Convert.ToInt32(datalist["D175_Times"]);
            D125_Times = Convert.ToInt32(datalist["D125_Times"]);
            D100_Times = Convert.ToInt32(datalist["D100_Times"]);
            D80_Times = Convert.ToInt32(datalist["D80_Times"]);
            D60_Times = Convert.ToInt32(datalist["D60_Times"]);
            D30_Times = Convert.ToInt32(datalist["D30_Times"]);
            D20_Times = Convert.ToInt32(datalist["D20_Times"]);
            D10_Times = Convert.ToInt32(datalist["D10_Times"]);
            D8_Times = Convert.ToInt32(datalist["D8_Times"]);
            D5_Times = Convert.ToInt32(datalist["D5_Times"]);
            D4_Times = Convert.ToInt32(datalist["D4_Times"]);
            D3_Times = Convert.ToInt32(datalist["D3_Times"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);
            update = false;
        }
    }
}
