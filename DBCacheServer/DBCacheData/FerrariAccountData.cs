using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class FerrariAccountData
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

        /// <summary>Super次數</summary>
        public int Super_Times { get; set; }
        /// <summary>Super總押</summary>
        public double Super_Bet { get; set; }
        /// <summary>Super總贏</summary>
        public double Super_Win { get; set; }
        /// <summary>Mega次數</summary>
        public int Mega_Times { get; set; }
        /// <summary>Mega總押</summary>
        public double Mega_Bet { get; set; }
        /// <summary>Mega總贏</summary>
        public double Mega_Win { get; set; }
        /// <summary>Major次數</summary>
        public int Major_Times { get; set; }
        /// <summary>Major總押</summary>
        public double Major_Bet { get; set; }
        /// <summary>Major總贏</summary>
        public double Major_Win { get; set; }
        /// <summary>Minor次數</summary>
        public int Minor_Times { get; set; }
        /// <summary>Minor總押</summary>
        public double Minor_Bet { get; set; }
        /// <summary>Minor總贏</summary>
        public double Minor_Win { get; set; }

        public int Lotto_Times { get; set; }
        public double Lotto_Win { get; set; }
        public int Bingo_Times { get; set; }
        public double Bingo_Win { get; set; }

        public int Grand_Times { get; set; }      //大满贯
        public int BigFour_Times { get; set; }    //大四喜
        public int BigThree_Times { get; set; }   //大三元
        public int Bonus_Times { get; set; }      //彩金奖
        public int JackPot_Times { get; set; }    //JP奖
        public int ShootLight_Times { get; set; } //射灯奖

        public int RF_Times { get; set; } //Ferrari
        public int GF_Times { get; set; }
        public int YF_Times { get; set; }
        public int RP_Times { get; set; } //Porsche
        public int GP_Times { get; set; }
        public int YP_Times { get; set; }
        public int RT_Times { get; set; } //Toyota
        public int GT_Times { get; set; }
        public int YT_Times { get; set; }
        public int RL_Times { get; set; } //Lexus
        public int GL_Times { get; set; }
        public int YL_Times { get; set; }

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

            else if (field == "Lotto_Times") Lotto_Times += Convert.ToInt32(value);
            else if (field == "Lotto_Win") Lotto_Win += Convert.ToDouble(value);
            else if (field == "Bingo_Times") Bingo_Times += Convert.ToInt32(value);
            else if (field == "Bingo_Win") Bingo_Win += Convert.ToDouble(value);

            else if (field == "Grand_Times") Grand_Times += Convert.ToInt32(value);
            else if (field == "BigFour_Times") BigFour_Times += Convert.ToInt32(value);
            else if (field == "BigThree_Times") BigThree_Times += Convert.ToInt32(value);
            else if (field == "Bonus_Times") Bonus_Times += Convert.ToInt32(value);
            else if (field == "JackPot_Times") JackPot_Times += Convert.ToInt32(value);
            else if (field == "ShootLight_Times") ShootLight_Times += Convert.ToInt32(value);

            else if (field == "RF_Times") RF_Times += Convert.ToInt32(value);
            else if (field == "GF_Times") GF_Times += Convert.ToInt32(value);
            else if (field == "YF_Times") YF_Times += Convert.ToInt32(value);
            else if (field == "RP_Times") RP_Times += Convert.ToInt32(value);
            else if (field == "GP_Times") GP_Times += Convert.ToInt32(value);
            else if (field == "YP_Times") YP_Times += Convert.ToInt32(value);
            else if (field == "RT_Times") RT_Times += Convert.ToInt32(value);
            else if (field == "GT_Times") GT_Times += Convert.ToInt32(value);
            else if (field == "YT_Times") YT_Times += Convert.ToInt32(value);
            else if (field == "RL_Times") RL_Times += Convert.ToInt32(value);
            else if (field == "GL_Times") GL_Times += Convert.ToInt32(value);
            else if (field == "YL_Times") YL_Times += Convert.ToInt32(value);
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

            updata.Add("Lotto_Times", Lotto_Times.ToString());
            updata.Add("Lotto_Win", Lotto_Win.ToString());
            updata.Add("Bingo_Times", Bingo_Times.ToString());
            updata.Add("Bingo_Win", Bingo_Win.ToString());

            updata.Add("Grand_Times", Grand_Times.ToString());
            updata.Add("BigFour_Times", BigFour_Times.ToString());
            updata.Add("BigThree_Times", BigThree_Times.ToString());
            updata.Add("Bonus_Times", Bonus_Times.ToString());
            updata.Add("JackPot_Times", JackPot_Times.ToString());
            updata.Add("ShootLight_Times", ShootLight_Times.ToString());

            updata.Add("RF_Times", RF_Times.ToString());
            updata.Add("GF_Times", GF_Times.ToString());
            updata.Add("YF_Times", YF_Times.ToString());
            updata.Add("RP_Times", RP_Times.ToString());
            updata.Add("GP_Times", GP_Times.ToString());
            updata.Add("YP_Times", YP_Times.ToString());
            updata.Add("RT_Times", RT_Times.ToString());
            updata.Add("GT_Times", GT_Times.ToString());
            updata.Add("YT_Times", YT_Times.ToString());
            updata.Add("RL_Times", RL_Times.ToString());
            updata.Add("GL_Times", GL_Times.ToString());
            updata.Add("YL_Times", YL_Times.ToString());

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

            Lotto_Times = 0;
            Lotto_Win = 0;
            Bingo_Times = 0;
            Bingo_Win = 0;

            Grand_Times = 0;
            BigFour_Times = 0;
            BigThree_Times = 0;
            Bonus_Times = 0;
            JackPot_Times = 0;
            ShootLight_Times = 0;

            RF_Times = 0;
            GF_Times = 0;
            YF_Times = 0;
            RP_Times = 0;
            GP_Times = 0;
            YP_Times = 0;
            RT_Times = 0;
            GT_Times = 0;
            YT_Times = 0;
            RL_Times = 0;
            GL_Times = 0;
            YL_Times = 0;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist, bool detail)
        {
            AccountUID = Convert.ToInt32(datalist["AccountUID"]);
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

            Lotto_Times = Convert.ToInt32(datalist["Lotto_Times"]);
            Lotto_Win = Convert.ToDouble(datalist["Lotto_Win"]);
            Bingo_Times = Convert.ToInt32(datalist["Bingo_Times"]);
            Bingo_Win = Convert.ToDouble(datalist["Bingo_Win"]);

            Grand_Times = Convert.ToInt32(datalist["Grand_Times"]);
            BigFour_Times = Convert.ToInt32(datalist["BigFour_Times"]);
            BigThree_Times = Convert.ToInt32(datalist["BigThree_Times"]);
            Bonus_Times = Convert.ToInt32(datalist["Bonus_Times"]);
            JackPot_Times = Convert.ToInt32(datalist["JackPot_Times"]);
            ShootLight_Times = Convert.ToInt32(datalist["ShootLight_Times"]);

            RF_Times = Convert.ToInt32(datalist["RF_Times"]);
            GF_Times = Convert.ToInt32(datalist["GF_Times"]);
            YF_Times = Convert.ToInt32(datalist["YF_Times"]);
            RP_Times = Convert.ToInt32(datalist["RP_Times"]);
            GP_Times = Convert.ToInt32(datalist["GP_Times"]);
            YP_Times = Convert.ToInt32(datalist["YP_Times"]);
            RT_Times = Convert.ToInt32(datalist["RT_Times"]);
            GT_Times = Convert.ToInt32(datalist["GT_Times"]);
            YT_Times = Convert.ToInt32(datalist["YT_Times"]);
            RL_Times = Convert.ToInt32(datalist["RL_Times"]);
            GL_Times = Convert.ToInt32(datalist["GL_Times"]);
            YL_Times = Convert.ToInt32(datalist["YL_Times"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);

            if (detail)
            {
                RecDate = Convert.ToDateTime(datalist["RecDate"]).Date;
            }
            update = false;
        }
    }
}
