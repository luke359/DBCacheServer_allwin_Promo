using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class MonkeyKing2AccountData
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

        public int GrandSlam { get; set; }
        public int HalfCountry { get; set; }
        public int AllColor { get; set; }
        public int SameColor { get; set; }
        public int SameAnimal { get; set; }
        public int Quartet { get; set; }
        public int GiveLamp { get; set; }
        public int Bonus { get; set; }
        public int Gold { get; set; }
        public int FourPeat { get; set; }
        public int ThreePeat { get; set; }
        public int TwoPeat { get; set; }
        public int TwinAnimal { get; set; }
        public int SeventyTwo { get; set; }
        public int RA { get; set; }
        public int GA { get; set; }
        public int YA { get; set; }
        public int RB { get; set; }
        public int GB { get; set; }
        public int YB { get; set; }
        public int RC { get; set; }
        public int GC { get; set; }
        public int YC { get; set; }
        public int RD { get; set; }
        public int GD { get; set; }
        public int YD { get; set; }

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

            else if (field == "GrandSlam") GrandSlam += Convert.ToInt32(value);
            else if (field == "HalfCountry") HalfCountry += Convert.ToInt32(value);
            else if (field == "AllColor") AllColor += Convert.ToInt32(value);
            else if (field == "SameColor") SameColor += Convert.ToInt32(value);
            else if (field == "SameAnimal") SameAnimal += Convert.ToInt32(value);
            else if (field == "Quartet") Quartet += Convert.ToInt32(value);
            else if (field == "GiveLamp") GiveLamp += Convert.ToInt32(value);
            else if (field == "Bonus") Bonus += Convert.ToInt32(value);
            else if (field == "Gold") Gold += Convert.ToInt32(value);
            else if (field == "FourPeat") FourPeat += Convert.ToInt32(value);
            else if (field == "ThreePeat") ThreePeat += Convert.ToInt32(value);
            else if (field == "TwoPeat") TwoPeat += Convert.ToInt32(value);
            else if (field == "TwinAnimal") TwinAnimal += Convert.ToInt32(value);
            else if (field == "SeventyTwo") SeventyTwo += Convert.ToInt32(value);
            else if (field == "RA") RA += Convert.ToInt32(value);
            else if (field == "GA") GA += Convert.ToInt32(value);
            else if (field == "YA") YA += Convert.ToInt32(value);
            else if (field == "RB") RB += Convert.ToInt32(value);
            else if (field == "GB") GB += Convert.ToInt32(value);
            else if (field == "YB") YB += Convert.ToInt32(value);
            else if (field == "RC") RC += Convert.ToInt32(value);
            else if (field == "GC") GC += Convert.ToInt32(value);
            else if (field == "YC") YC += Convert.ToInt32(value);
            else if (field == "RD") RD += Convert.ToInt32(value);
            else if (field == "GD") GD += Convert.ToInt32(value);
            else if (field == "YD") YD += Convert.ToInt32(value);
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

            updata.Add("GrandSlam", GrandSlam.ToString());
            updata.Add("HalfCountry", HalfCountry.ToString());
            updata.Add("AllColor", AllColor.ToString());
            updata.Add("SameColor", SameColor.ToString());
            updata.Add("SameAnimal", SameAnimal.ToString());
            updata.Add("Quartet", Quartet.ToString());
            updata.Add("GiveLamp", GiveLamp.ToString());
            updata.Add("Bonus", Bonus.ToString());
            updata.Add("Gold", Gold.ToString());
            updata.Add("FourPeat", FourPeat.ToString());
            updata.Add("ThreePeat", ThreePeat.ToString());
            updata.Add("TwoPeat", TwoPeat.ToString());
            updata.Add("TwinAnimal", TwinAnimal.ToString());
            updata.Add("SeventyTwo", SeventyTwo.ToString());
            updata.Add("RA", RA.ToString());
            updata.Add("GA", GA.ToString());
            updata.Add("YA", YA.ToString());
            updata.Add("RB", RB.ToString());
            updata.Add("GB", GB.ToString());
            updata.Add("YB", YB.ToString());
            updata.Add("RC", RC.ToString());
            updata.Add("GC", GC.ToString());
            updata.Add("YC", YC.ToString());
            updata.Add("RD", RD.ToString());
            updata.Add("GD", GD.ToString());
            updata.Add("YD", YD.ToString());

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

            GrandSlam = 0;
            HalfCountry = 0;
            AllColor = 0;
            SameColor = 0;
            SameAnimal = 0;
            Quartet = 0;
            GiveLamp = 0;
            Bonus = 0;
            Gold = 0;
            FourPeat = 0;
            ThreePeat = 0;
            TwoPeat = 0;
            TwinAnimal = 0;
            SeventyTwo = 0;
            RA = 0;
            GA = 0;
            YA = 0;
            RB = 0;
            GB = 0;
            YB = 0;
            RC = 0;
            GC = 0;
            YC = 0;
            RD = 0;
            GD = 0;
            YD = 0;
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

            GrandSlam = Convert.ToInt32(datalist["GrandSlam"]);
            HalfCountry = Convert.ToInt32(datalist["HalfCountry"]);
            AllColor = Convert.ToInt32(datalist["AllColor"]);
            SameColor = Convert.ToInt32(datalist["SameColor"]);
            SameAnimal = Convert.ToInt32(datalist["SameAnimal"]);
            Quartet = Convert.ToInt32(datalist["Quartet"]);
            GiveLamp = Convert.ToInt32(datalist["GiveLamp"]);
            Bonus = Convert.ToInt32(datalist["Bonus"]);
            Gold = Convert.ToInt32(datalist["Gold"]);
            FourPeat = Convert.ToInt32(datalist["FourPeat"]);
            ThreePeat = Convert.ToInt32(datalist["ThreePeat"]);
            TwoPeat = Convert.ToInt32(datalist["TwoPeat"]);
            TwinAnimal = Convert.ToInt32(datalist["TwinAnimal"]);
            SeventyTwo = Convert.ToInt32(datalist["SeventyTwo"]);
            RA = Convert.ToInt32(datalist["RA"]);
            GA = Convert.ToInt32(datalist["GA"]);
            YA = Convert.ToInt32(datalist["YA"]);
            RB = Convert.ToInt32(datalist["RB"]);
            GB = Convert.ToInt32(datalist["GB"]);
            YB = Convert.ToInt32(datalist["YB"]);
            RC = Convert.ToInt32(datalist["RC"]);
            GC = Convert.ToInt32(datalist["GC"]);
            YC = Convert.ToInt32(datalist["YC"]);
            RD = Convert.ToInt32(datalist["RD"]);
            GD = Convert.ToInt32(datalist["GD"]);
            YD = Convert.ToInt32(datalist["YD"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);
            update = false;
        }
    }
}
