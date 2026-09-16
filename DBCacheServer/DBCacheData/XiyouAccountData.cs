using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class XiyouAccountData
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
        /// <summary>四海歸一次數</summary>
        public int GrandSlam { get; set; }
        /// <summary>四大天王次數</summary>
        public int SameColor { get; set; }
        /// <summary>角色全中次數</summary>
        public int SameAnimal { get; set; }
        /// <summary>縱橫四海次數</summary>
        public int Quartet { get; set; }
        /// <summary>仙女散花次數</summary>
        public int GiveLamp { get; set; }
        /// <summary>連線彩金次數</summary>
        public int Bonus { get; set; }
        /// <summary>四連次數</summary>
        public int FourPeat { get; set; }
        /// <summary>三連次數</summary>
        public int ThreePeat { get; set; }
        /// <summary>二連次數</summary>
        public int TwoPeat { get; set; }
        /// <summary>72變次數</summary>
        public int SeventyTwo { get; set; }
        /// <summary>封神次數</summary>
        public int TwinAnimal { get; set; }
        /// <summary>紅色孫悟空</summary>
        public int RA { get; set; }
        /// <summary>綠色孫悟空</summary>
        public int GA { get; set; }
        /// <summary>黃色孫悟空</summary>
        public int YA { get; set; }
        /// <summary>紅色紅孩兒</summary>
        public int RB { get; set; }
        /// <summary>綠色紅孩兒</summary>
        public int GB { get; set; }
        /// <summary>黃色紅孩兒</summary>
        public int YB { get; set; }
        /// <summary>紅色沙悟淨</summary>
        public int RC { get; set; }
        /// <summary>綠色沙悟淨</summary>
        public int GC { get; set; }
        /// <summary>黃色沙悟淨</summary>
        public int YC { get; set; }
        /// <summary>紅色牛魔王</summary>
        public int RD { get; set; }
        /// <summary>綠色牛魔王</summary>
        public int GD { get; set; }
        /// <summary>黃色牛魔王</summary>
        public int YD { get; set; }
        /// <summary>莊閒總押</summary>
        public double BPTotalBet { get; set; }
        /// <summary>莊閒總贏</summary>
        public double BPTotalWin { get; set; }
        /// <summary>莊閒總盈餘</summary>
        public double BPTotalSurplus { get; set; }
        /// <summary>莊閒遊戲次數</summary>
        public int BPGameTimes { get; set; }
        /// <summary>莊</summary>
        public int BZ { get; set; }
        /// <summary>閒</summary>
        public int BX { get; set; }
        /// <summary>和</summary>
        public int BH { get; set; }
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
            else if (field == "SameColor") SameColor += Convert.ToInt32(value);
            else if (field == "SameAnimal") SameAnimal += Convert.ToInt32(value);
            else if (field == "Quartet") Quartet += Convert.ToInt32(value);
            else if (field == "GiveLamp") GiveLamp += Convert.ToInt32(value);
            else if (field == "Bonus") Bonus += Convert.ToInt32(value);
            else if (field == "FourPeat") FourPeat += Convert.ToInt32(value);
            else if (field == "ThreePeat") ThreePeat += Convert.ToInt32(value);
            else if (field == "TwoPeat") TwoPeat += Convert.ToInt32(value);
            else if (field == "SeventyTwo") SeventyTwo += Convert.ToInt32(value);
            else if (field == "TwinAnimal") TwinAnimal += Convert.ToInt32(value);
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
            else if (field == "BPTotalBet") BPTotalBet += Convert.ToDouble(value);
            else if (field == "BPTotalWin") BPTotalWin += Convert.ToDouble(value);
            else if (field == "BPTotalSurplus") BPTotalSurplus += Convert.ToDouble(value);
            else if (field == "BPGameTimes") BPGameTimes += Convert.ToInt32(value);
            else if (field == "BZ") BZ += Convert.ToInt32(value);
            else if (field == "BX") BX += Convert.ToInt32(value);
            else if (field == "BH") BH += Convert.ToInt32(value);
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

            updata.Add("GrandSlam" , GrandSlam.ToString());
            updata.Add("SameColor" , SameColor.ToString());
            updata.Add("SameAnimal", SameAnimal.ToString());
            updata.Add("Quartet"   , Quartet.ToString());
            updata.Add("GiveLamp"  , GiveLamp.ToString());
            updata.Add("Bonus"     , Bonus.ToString());
            updata.Add("FourPeat"  , FourPeat.ToString());
            updata.Add("ThreePeat" , ThreePeat.ToString());
            updata.Add("TwoPeat"   , TwoPeat.ToString());
            updata.Add("SeventyTwo", SeventyTwo.ToString());
            updata.Add("TwinAnimal", TwinAnimal.ToString());
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
            updata.Add("BPTotalBet"    , BPTotalBet.ToString());
            updata.Add("BPTotalWin"    , BPTotalWin.ToString());
            updata.Add("BPTotalSurplus", BPTotalSurplus.ToString());
            updata.Add("BPGameTimes"   , BPGameTimes.ToString());
            updata.Add("BZ"            , BZ.ToString());
            updata.Add("BX"            , BX.ToString());
            updata.Add("BH"            , BH.ToString());

            if (detail)
            {
                updata.Add("RecDate", "'" + RecDate.ToString("yyyy-MM-dd") + "'");
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
            SameColor = 0;
            SameAnimal = 0;
            Quartet = 0;
            GiveLamp = 0;
            Bonus = 0;
            FourPeat = 0;
            ThreePeat = 0;
            TwoPeat = 0;
            SeventyTwo = 0;
            TwinAnimal = 0;
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
            BPTotalBet = 0;
            BPTotalWin = 0;
            BPTotalSurplus = 0;
            BPGameTimes = 0;
            BZ = 0;
            BX = 0;
            BH = 0;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist, bool detail)
        {
            if (detail)
            {
                AccountUID = Convert.ToInt32(datalist["AccountDatailUID"]);
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
            SameColor = Convert.ToInt32(datalist["SameColor"]);
            SameAnimal = Convert.ToInt32(datalist["SameAnimal"]);
            Quartet = Convert.ToInt32(datalist["Quartet"]);
            GiveLamp = Convert.ToInt32(datalist["GiveLamp"]);
            Bonus = Convert.ToInt32(datalist["Bonus"]);
            FourPeat = Convert.ToInt32(datalist["FourPeat"]);
            ThreePeat = Convert.ToInt32(datalist["ThreePeat"]);
            TwoPeat = Convert.ToInt32(datalist["TwoPeat"]);
            SeventyTwo = Convert.ToInt32(datalist["SeventyTwo"]);
            TwinAnimal = Convert.ToInt32(datalist["TwinAnimal"]);
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
            BPTotalBet = Convert.ToDouble(datalist["BPTotalBet"]);
            BPTotalWin = Convert.ToDouble(datalist["BPTotalWin"]);
            BPTotalSurplus = Convert.ToDouble(datalist["BPTotalSurplus"]);
            BPGameTimes = Convert.ToInt32(datalist["BPGameTimes"]);
            BZ = Convert.ToInt32(datalist["BZ"]);
            BX = Convert.ToInt32(datalist["BX"]);
            BH = Convert.ToInt32(datalist["BH"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);
            update = false;
        }

    }
}
