using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class OceanKing3AccountData
    {
        /// <summary>唯一碼</summary>
        public int AccountUID;
        /// <summary>機台唯一碼</summary>
        public int MachineUID;
        /// <summary>總押</summary>
        public decimal TotalBet;
        /// <summary>總贏</summary>
        public decimal TotalWin;
        /// <summary>總盈餘</summary>
        public decimal TotalSurplus;
        /// <summary>遊戲次數</summary>
        public int GameTimes;
        /// <summary>贏的次數</summary>
        public int WinTimes;

        public int Super_Times;
        public double Super_Bet;
        public decimal Super_Win;
        public int Mega_Times;
        public double Mega_Bet;
        public decimal Mega_Win;
        public int Major_Times;
        public double Major_Bet;
        public decimal Major_Win;
        public int Minor_Times;
        public double Minor_Bet;
        public decimal Minor_Win;
        //BOSS魚種
        public int ThunderDragon_Times;  //霸雷轟龍
        public double ThunderDragon_Bet;
        public double ThunderDragon_Win;
        //public int PurpleDragon_Times;   //紫焰冥龍
        //public double PurpleDragon_Bet;
        //public double PurpleDragon_Win;
        //public int Phoenix_Times;       //熾火鳳凰
        //public double Phoenix_Bet;
        //public double Phoenix_Win;
        //public int Mermaid_Times;       //深海人魚
        //public double Mermaid_Bet;
        //public double Mermaid_Win;
        public int Behemoth_Times;      //暗夜巨獸
        public double Behemoth_Bet;
        public double Behemoth_Win;
        //public int GiantCrocodile_Times; //史前巨鱷
        //public double GiantCrocodile_Bet;
        //public double GiantCrocodile_Win;
        public int GiantOctopus_Times;   //遠古巨章
        public double GiantOctopus_Bet;
        public double GiantOctopus_Win;
        //public int GiantCrab_Times;     //霸王巨蟹
        //public double GiantCrab_Bet;
        //public double GiantCrab_Win;
        //public int FireTurtle_Times;    //赤焰龍龜
        //public double FireTurtle_Bet;
        //public double FireTurtle_Win;
        //public int FireDragon_Times;    //烈火暴龍
        //public double FireDragon_Bet;
        //public double FireDragon_Win;
        //特殊魚種 (武器蟹)
        public int JellyFish_Times;    //電水母
        public double JellyFish_Bet;
        public double JellyFish_Win;
        public int BombCrab_Times;     //爆破蟹
        public double BombCrab_Bet;
        public double BombCrab_Win;
        public int DrillCrab_Times;    //鑽頭蟹
        public double DrillCrab_Bet;
        public double DrillCrab_Win;
        public int LaserCrab_Times;    //雷射蟹
        public double LaserCrab_Bet;
        public double LaserCrab_Win;
        public int LungPanCrab_Times;  //龍蟠蟹
        public double LungPanCrab_Bet;
        public double LungPanCrab_Win;
        public int ThunderCrab_Times;  //雷神蟹
        public double ThunderCrab_Bet;
        public double ThunderCrab_Win;
        //一般魚種
        public int UnicornWhale_Times;  //獨角鯨
        public double UnicornWhale_Bet;
        public double UnicornWhale_Win;
        public int KillerWhale_Times;   //虎鯨
        public double KillerWhale_Bet;
        public double KillerWhale_Win;
        public int Shark_Times;        //鯊魚
        public double Shark_Bet;
        public double Shark_Win;
        public int PufferGold_Times;    //刺豚(金)
        public double PufferGold_Bet;
        public double PufferGold_Win;
        public int MoorishIdolGold_Times; //鐮魚(金)
        public double MoorishIdolGold_Bet;
        public double MoorishIdolGold_Win;
        public int ClownFishGold_Times;   //小丑魚(金)
        public double ClownFishGold_Bet;
        public double ClownFishGold_Win;
        public int PufferBig_Times;      //刺豚(大)
        public double PufferBig_Bet;
        public double PufferBig_Win;
        public int MoorishIdolBig_Times;  //鐮魚(大
        public double MoorishIdolBig_Bet;
        public double MoorishIdolBig_Win;
        public int ClownFishBig_Times;    //小丑魚(大)
        public double ClownFishBig_Bet;
        public double ClownFishBig_Win;
        public int Mobula_Times;         //蝠鱝
        public double Mobula_Bet;
        public double Mobula_Win;
        public int Stingray_Times;       //魟魚
        public double Stingray_Bet;
        public double Stingray_Win;
        public int Turtle_Times;         //烏龜
        public double Turtle_Bet;
        public double Turtle_Win;
        public int AnglerFish_Times;     //燈籠魚
        public double AnglerFish_Bet;
        public double AnglerFish_Win;
        public int Octopus_Times;        //章魚
        public double Octopus_Bet;
        public double Octopus_Win;
        public int SwordFish_Times;      //旗魚
        public double SwordFish_Bet;
        public double SwordFish_Win;
        public int Lobster_Times;       //螯蝦
        public double Lobster_Bet;
        public double Lobster_Win;
        public int YellowTang_Times;    //黃金吊
        public double YellowTang_Bet;
        public double YellowTang_Win;
        public int Pterois_Times;      //獅子魚
        public double Pterois_Bet;
        public double Pterois_Win;
        public int Puffer_Times;       //刺豚
        public double Puffer_Bet;
        public double Puffer_Win;
        public int MoorishIdol_Times;  //鐮魚
        public double MoorishIdol_Bet;
        public double MoorishIdol_Win;
        public int ClownFish_Times;    //小丑魚
        public double ClownFish_Bet;
        public double ClownFish_Win;
        public int FlyingFish_Times;   //飛魚
        public double FlyingFish_Bet;
        public double FlyingFish_Win;

        /// <summary>內帳或外帳</summary>
        public int AccountType;
        /// <summary>紀錄時間(日帳使用)</summary>
        public DateTime RecDate;
        /// <summary>更新旗號(資料已變更)</summary>
        public bool update;

        /// <summary>累計更新遊戲紀錄內存</summary>
        public void AccumulatecGameData(Dictionary<string, string> refdata)
        {
            double betAdd = Convert.ToDouble(refdata["TotalBet"]);
            double winAdd = Convert.ToDouble(refdata["TotalWin"]);
            int gameTimes = Convert.ToInt32(refdata["GameTimes"]);
            int winTimes = Convert.ToInt32(refdata["WinTimes"]);

            TotalBet += (decimal)betAdd;
            TotalWin += (decimal)winAdd;
            GameTimes += gameTimes;
            WinTimes += winTimes;

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
            else if (field == "Super_Win") Super_Win += Convert.ToDecimal(value);

            else if (field == "Mega_Times") Mega_Times += Convert.ToInt32(value);
            else if (field == "Mega_Bet") Mega_Bet += Convert.ToDouble(value);
            else if (field == "Mega_Win") Mega_Win += Convert.ToDecimal(value);

            else if (field == "Major_Times") Major_Times += Convert.ToInt32(value);
            else if (field == "Major_Bet") Major_Bet += Convert.ToDouble(value);
            else if (field == "Major_Win") Major_Win += Convert.ToDecimal(value);

            else if (field == "Minor_Times") Minor_Times += Convert.ToInt32(value);
            else if (field == "Minor_Bet") Minor_Bet += Convert.ToDouble(value);
            else if (field == "Minor_Win") Minor_Win += Convert.ToDecimal(value);

            else if (field == "ThunderDragon_Times") ThunderDragon_Times += Convert.ToInt32(value);
            else if (field == "ThunderDragon_Bet") ThunderDragon_Bet += Convert.ToDouble(value);
            else if (field == "ThunderDragon_Win") ThunderDragon_Win += Convert.ToDouble(value);
            //else if (field == "PurpleDragon_Times") PurpleDragon_Times += Convert.ToInt32(value);
            //else if (field == "PurpleDragon_Bet") PurpleDragon_Bet += Convert.ToDouble(value);
            //else if (field == "PurpleDragon_Win") PurpleDragon_Win += Convert.ToDouble(value);
            //else if (field == "Phoenix_Times") Phoenix_Times += Convert.ToInt32(value);
            //else if (field == "Phoenix_Bet") Phoenix_Bet += Convert.ToDouble(value);
            //else if (field == "Phoenix_Win") Phoenix_Win += Convert.ToDouble(value);
            //else if (field == "Mermaid_Times") Mermaid_Times += Convert.ToInt32(value);
            //else if (field == "Mermaid_Bet") Mermaid_Bet += Convert.ToDouble(value);
            //else if (field == "Mermaid_Win") Mermaid_Win += Convert.ToDouble(value);
            else if (field == "Behemoth_Times") Behemoth_Times += Convert.ToInt32(value);
            else if (field == "Behemoth_Bet") Behemoth_Bet += Convert.ToDouble(value);
            else if (field == "Behemoth_Win") Behemoth_Win += Convert.ToDouble(value);
            //else if (field == "GiantCrocodile_Times") GiantCrocodile_Times += Convert.ToInt32(value);
            //else if (field == "GiantCrocodile_Bet") GiantCrocodile_Bet += Convert.ToDouble(value);
            //else if (field == "GiantCrocodile_Win") GiantCrocodile_Win += Convert.ToDouble(value);
            else if (field == "GiantOctopus_Times") GiantOctopus_Times += Convert.ToInt32(value);
            else if (field == "GiantOctopus_Bet") GiantOctopus_Bet += Convert.ToDouble(value);
            else if (field == "GiantOctopus_Win") GiantOctopus_Win += Convert.ToDouble(value);
            //else if (field == "GiantCrab_Times") GiantCrab_Times += Convert.ToInt32(value);
            //else if (field == "GiantCrab_Bet") GiantCrab_Bet += Convert.ToDouble(value);
            //else if (field == "GiantCrab_Win") GiantCrab_Win += Convert.ToDouble(value);
            //else if (field == "FireTurtle_Times") FireTurtle_Times += Convert.ToInt32(value);
            //else if (field == "FireTurtle_Bet") FireTurtle_Bet += Convert.ToDouble(value);
            //else if (field == "FireTurtle_Win") FireTurtle_Win += Convert.ToDouble(value);
            //else if (field == "FireDragon_Times") FireDragon_Times += Convert.ToInt32(value);
            //else if (field == "FireDragon_Bet") FireDragon_Bet += Convert.ToDouble(value);
            //else if (field == "FireDragon_Win") FireDragon_Win += Convert.ToDouble(value);
            else if (field == "JellyFish_Times") JellyFish_Times += Convert.ToInt32(value);
            else if (field == "JellyFish_Bet") JellyFish_Bet += Convert.ToDouble(value);
            else if (field == "JellyFish_Win") JellyFish_Win += Convert.ToDouble(value);
            else if (field == "BombCrab_Times") BombCrab_Times += Convert.ToInt32(value);
            else if (field == "BombCrab_Bet") BombCrab_Bet += Convert.ToDouble(value);
            else if (field == "BombCrab_Win") BombCrab_Win += Convert.ToDouble(value);
            else if (field == "DrillCrab_Times") DrillCrab_Times += Convert.ToInt32(value);
            else if (field == "DrillCrab_Bet") DrillCrab_Bet += Convert.ToDouble(value);
            else if (field == "DrillCrab_Win") DrillCrab_Win += Convert.ToDouble(value);
            else if (field == "LaserCrab_Times") LaserCrab_Times += Convert.ToInt32(value);
            else if (field == "LaserCrab_Bet") LaserCrab_Bet += Convert.ToDouble(value);
            else if (field == "LaserCrab_Win") LaserCrab_Win += Convert.ToDouble(value);
            else if (field == "LungPanCrab_Times") LungPanCrab_Times += Convert.ToInt32(value);
            else if (field == "LungPanCrab_Bet") LungPanCrab_Bet += Convert.ToDouble(value);
            else if (field == "LungPanCrab_Win") LungPanCrab_Win += Convert.ToDouble(value);
            else if (field == "ThunderCrab_Times") ThunderCrab_Times += Convert.ToInt32(value);
            else if (field == "ThunderCrab_Bet") ThunderCrab_Bet += Convert.ToDouble(value);
            else if (field == "ThunderCrab_Win") ThunderCrab_Win += Convert.ToDouble(value);
            else if (field == "UnicornWhale_Times") UnicornWhale_Times += Convert.ToInt32(value);
            else if (field == "UnicornWhale_Bet") UnicornWhale_Bet += Convert.ToDouble(value);
            else if (field == "UnicornWhale_Win") UnicornWhale_Win += Convert.ToDouble(value);
            else if (field == "KillerWhale_Times") KillerWhale_Times += Convert.ToInt32(value);
            else if (field == "KillerWhale_Bet") KillerWhale_Bet += Convert.ToDouble(value);
            else if (field == "KillerWhale_Win") KillerWhale_Win += Convert.ToDouble(value);
            else if (field == "Shark_Times") Shark_Times += Convert.ToInt32(value);
            else if (field == "Shark_Bet") Shark_Bet += Convert.ToDouble(value);
            else if (field == "Shark_Win") Shark_Win += Convert.ToDouble(value);
            else if (field == "PufferGold_Times") PufferGold_Times += Convert.ToInt32(value);
            else if (field == "PufferGold_Bet") PufferGold_Bet += Convert.ToDouble(value);
            else if (field == "PufferGold_Win") PufferGold_Win += Convert.ToDouble(value);
            else if (field == "MoorishIdolGold_Times") MoorishIdolGold_Times += Convert.ToInt32(value);
            else if (field == "MoorishIdolGold_Bet") MoorishIdolGold_Bet += Convert.ToDouble(value);
            else if (field == "MoorishIdolGold_Win") MoorishIdolGold_Win += Convert.ToDouble(value);
            else if (field == "ClownFishGold_Times") ClownFishGold_Times += Convert.ToInt32(value);
            else if (field == "ClownFishGold_Bet") ClownFishGold_Bet += Convert.ToDouble(value);
            else if (field == "ClownFishGold_Win") ClownFishGold_Win += Convert.ToDouble(value);
            else if (field == "PufferBig_Times") PufferBig_Times += Convert.ToInt32(value);
            else if (field == "PufferBig_Bet") PufferBig_Bet += Convert.ToDouble(value);
            else if (field == "PufferBig_Win") PufferBig_Win += Convert.ToDouble(value);
            else if (field == "MoorishIdolBig_Times") MoorishIdolBig_Times += Convert.ToInt32(value);
            else if (field == "MoorishIdolBig_Bet") MoorishIdolBig_Bet += Convert.ToDouble(value);
            else if (field == "MoorishIdolBig_Win") MoorishIdolBig_Win += Convert.ToDouble(value);
            else if (field == "ClownFishBig_Times") ClownFishBig_Times += Convert.ToInt32(value);
            else if (field == "ClownFishBig_Bet") ClownFishBig_Bet += Convert.ToDouble(value);
            else if (field == "ClownFishBig_Win") ClownFishBig_Win += Convert.ToDouble(value);
            else if (field == "Mobula_Times") Mobula_Times += Convert.ToInt32(value);
            else if (field == "Mobula_Bet") Mobula_Bet += Convert.ToDouble(value);
            else if (field == "Mobula_Win") Mobula_Win += Convert.ToDouble(value);
            else if (field == "Stingray_Times") Stingray_Times += Convert.ToInt32(value);
            else if (field == "Stingray_Bet") Stingray_Bet += Convert.ToDouble(value);
            else if (field == "Stingray_Win") Stingray_Win += Convert.ToDouble(value);
            else if (field == "Turtle_Times") Turtle_Times += Convert.ToInt32(value);
            else if (field == "Turtle_Bet") Turtle_Bet += Convert.ToDouble(value);
            else if (field == "Turtle_Win") Turtle_Win += Convert.ToDouble(value);
            else if (field == "AnglerFish_Times") AnglerFish_Times += Convert.ToInt32(value);
            else if (field == "AnglerFish_Bet") AnglerFish_Bet += Convert.ToDouble(value);
            else if (field == "AnglerFish_Win") AnglerFish_Win += Convert.ToDouble(value);
            else if (field == "Octopus_Times") Octopus_Times += Convert.ToInt32(value);
            else if (field == "Octopus_Bet") Octopus_Bet += Convert.ToDouble(value);
            else if (field == "Octopus_Win") Octopus_Win += Convert.ToDouble(value);
            else if (field == "SwordFish_Times") SwordFish_Times += Convert.ToInt32(value);
            else if (field == "SwordFish_Bet") SwordFish_Bet += Convert.ToDouble(value);
            else if (field == "SwordFish_Win") SwordFish_Win += Convert.ToDouble(value);
            else if (field == "Lobster_Times") Lobster_Times += Convert.ToInt32(value);
            else if (field == "Lobster_Bet") Lobster_Bet += Convert.ToDouble(value);
            else if (field == "Lobster_Win") Lobster_Win += Convert.ToDouble(value);
            else if (field == "YellowTang_Times") YellowTang_Times += Convert.ToInt32(value);
            else if (field == "YellowTang_Bet") YellowTang_Bet += Convert.ToDouble(value);
            else if (field == "YellowTang_Win") YellowTang_Win += Convert.ToDouble(value);
            else if (field == "Pterois_Times") Pterois_Times += Convert.ToInt32(value);
            else if (field == "Pterois_Bet") Pterois_Bet += Convert.ToDouble(value);
            else if (field == "Pterois_Win") Pterois_Win += Convert.ToDouble(value);
            else if (field == "Puffer_Times") Puffer_Times += Convert.ToInt32(value);
            else if (field == "Puffer_Bet") Puffer_Bet += Convert.ToDouble(value);
            else if (field == "Puffer_Win") Puffer_Win += Convert.ToDouble(value);
            else if (field == "MoorishIdol_Times") MoorishIdol_Times += Convert.ToInt32(value);
            else if (field == "MoorishIdol_Bet") MoorishIdol_Bet += Convert.ToDouble(value);
            else if (field == "MoorishIdol_Win") MoorishIdol_Win += Convert.ToDouble(value);
            else if (field == "ClownFish_Times") ClownFish_Times += Convert.ToInt32(value);
            else if (field == "ClownFish_Bet") ClownFish_Bet += Convert.ToDouble(value);
            else if (field == "ClownFish_Win") ClownFish_Win += Convert.ToDouble(value);
            else if (field == "FlyingFish_Times") FlyingFish_Times += Convert.ToInt32(value);
            else if (field == "FlyingFish_Bet") FlyingFish_Bet += Convert.ToDouble(value);
            else if (field == "FlyingFish_Win") FlyingFish_Win += Convert.ToDouble(value);
        }
        /// <summary>累計更新JP內存</summary>
        public void AccumulatecJPData(JpAward jPType, double jPBet, double jPWin)
        {
            if (jPWin > 0)
            {
                TotalWin += (decimal)jPWin;
                TotalSurplus = TotalBet - TotalWin;
            }

            if (jPType == JpAward.MINOR)
            {
                Minor_Times += 1;
                Minor_Bet += jPBet;
                Minor_Win += (decimal)jPWin;
            }
            else if (jPType == JpAward.MAJOR)
            {
                Major_Times += 1;
                Major_Bet += jPBet;
                Major_Win += (decimal)jPWin;
            }
            else if (jPType == JpAward.MEGA)
            {
                Mega_Times += 1;
                Mega_Bet += jPBet;
                Mega_Win += (decimal)jPWin;
            }
            else if (jPType == JpAward.SUPER)
            {
                Super_Times += 1;
                Super_Bet += jPBet;
                Super_Win += (decimal)jPWin;
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

            updata.Add("ThunderDragon_Times", ThunderDragon_Times.ToString());
            updata.Add("ThunderDragon_Bet", ThunderDragon_Bet.ToString());
            updata.Add("ThunderDragon_Win", ThunderDragon_Win.ToString());
            //updata.Add("PurpleDragon_Times", PurpleDragon_Times.ToString());
            //updata.Add("PurpleDragon_Bet", PurpleDragon_Bet.ToString());
            //updata.Add("PurpleDragon_Win", PurpleDragon_Win.ToString());
            //updata.Add("Phoenix_Times", Phoenix_Times.ToString());
            //updata.Add("Phoenix_Bet", Phoenix_Bet.ToString());
            //updata.Add("Phoenix_Win", Phoenix_Win.ToString());
            //updata.Add("Mermaid_Times", Mermaid_Times.ToString());
            //updata.Add("Mermaid_Bet", Mermaid_Bet.ToString());
            //updata.Add("Mermaid_Win", Mermaid_Win.ToString());
            updata.Add("Behemoth_Times", Behemoth_Times.ToString());
            updata.Add("Behemoth_Bet", Behemoth_Bet.ToString());
            updata.Add("Behemoth_Win", Behemoth_Win.ToString());
            //updata.Add("GiantCrocodile_Times", GiantCrocodile_Times.ToString());
            //updata.Add("GiantCrocodile_Bet", GiantCrocodile_Bet.ToString());
            //updata.Add("GiantCrocodile_Win", GiantCrocodile_Win.ToString());
            updata.Add("GiantOctopus_Times", GiantOctopus_Times.ToString());
            updata.Add("GiantOctopus_Bet", GiantOctopus_Bet.ToString());
            updata.Add("GiantOctopus_Win", GiantOctopus_Win.ToString());
            //updata.Add("GiantCrab_Times", GiantCrab_Times.ToString());
            //updata.Add("GiantCrab_Bet", GiantCrab_Bet.ToString());
            //updata.Add("GiantCrab_Win", GiantCrab_Win.ToString());
            //updata.Add("FireTurtle_Times", FireTurtle_Times.ToString());
            //updata.Add("FireTurtle_Bet", FireTurtle_Bet.ToString());
            //updata.Add("FireTurtle_Win", FireTurtle_Win.ToString());
            //updata.Add("FireDragon_Times", FireDragon_Times.ToString());
            //updata.Add("FireDragon_Bet", FireDragon_Bet.ToString());
            //updata.Add("FireDragon_Win", FireDragon_Win.ToString());
            updata.Add("JellyFish_Times", JellyFish_Times.ToString());
            updata.Add("JellyFish_Bet", JellyFish_Bet.ToString());
            updata.Add("JellyFish_Win", JellyFish_Win.ToString());
            updata.Add("BombCrab_Times", BombCrab_Times.ToString());
            updata.Add("BombCrab_Bet", BombCrab_Bet.ToString());
            updata.Add("BombCrab_Win", BombCrab_Win.ToString());
            updata.Add("DrillCrab_Times", DrillCrab_Times.ToString());
            updata.Add("DrillCrab_Bet", DrillCrab_Bet.ToString());
            updata.Add("DrillCrab_Win", DrillCrab_Win.ToString());
            updata.Add("LaserCrab_Times", LaserCrab_Times.ToString());
            updata.Add("LaserCrab_Bet", LaserCrab_Bet.ToString());
            updata.Add("LaserCrab_Win", LaserCrab_Win.ToString());
            updata.Add("LungPanCrab_Times", LungPanCrab_Times.ToString());
            updata.Add("LungPanCrab_Bet", LungPanCrab_Bet.ToString());
            updata.Add("LungPanCrab_Win", LungPanCrab_Win.ToString());
            updata.Add("ThunderCrab_Times", ThunderCrab_Times.ToString());
            updata.Add("ThunderCrab_Bet", ThunderCrab_Bet.ToString());
            updata.Add("ThunderCrab_Win", ThunderCrab_Win.ToString());
            updata.Add("UnicornWhale_Times", UnicornWhale_Times.ToString());
            updata.Add("UnicornWhale_Bet", UnicornWhale_Bet.ToString());
            updata.Add("UnicornWhale_Win", UnicornWhale_Win.ToString());
            updata.Add("KillerWhale_Times", KillerWhale_Times.ToString());
            updata.Add("KillerWhale_Bet", KillerWhale_Bet.ToString());
            updata.Add("KillerWhale_Win", KillerWhale_Win.ToString());
            updata.Add("Shark_Times", Shark_Times.ToString());
            updata.Add("Shark_Bet", Shark_Bet.ToString());
            updata.Add("Shark_Win", Shark_Win.ToString());
            updata.Add("PufferGold_Times", PufferGold_Times.ToString());
            updata.Add("PufferGold_Bet", PufferGold_Bet.ToString());
            updata.Add("PufferGold_Win", PufferGold_Win.ToString());
            updata.Add("MoorishIdolGold_Times", MoorishIdolGold_Times.ToString());
            updata.Add("MoorishIdolGold_Bet", MoorishIdolGold_Bet.ToString());
            updata.Add("MoorishIdolGold_Win", MoorishIdolGold_Win.ToString());
            updata.Add("ClownFishGold_Times", ClownFishGold_Times.ToString());
            updata.Add("ClownFishGold_Bet", ClownFishGold_Bet.ToString());
            updata.Add("ClownFishGold_Win", ClownFishGold_Win.ToString());
            updata.Add("PufferBig_Times", PufferBig_Times.ToString());
            updata.Add("PufferBig_Bet", PufferBig_Bet.ToString());
            updata.Add("PufferBig_Win", PufferBig_Win.ToString());
            updata.Add("MoorishIdolBig_Times", MoorishIdolBig_Times.ToString());
            updata.Add("MoorishIdolBig_Bet", MoorishIdolBig_Bet.ToString());
            updata.Add("MoorishIdolBig_Win", MoorishIdolBig_Win.ToString());
            updata.Add("ClownFishBig_Times", ClownFishBig_Times.ToString());
            updata.Add("ClownFishBig_Bet", ClownFishBig_Bet.ToString());
            updata.Add("ClownFishBig_Win", ClownFishBig_Win.ToString());
            updata.Add("Mobula_Times", Mobula_Times.ToString());
            updata.Add("Mobula_Bet", Mobula_Bet.ToString());
            updata.Add("Mobula_Win", Mobula_Win.ToString());
            updata.Add("Stingray_Times", Stingray_Times.ToString());
            updata.Add("Stingray_Bet", Stingray_Bet.ToString());
            updata.Add("Stingray_Win", Stingray_Win.ToString());
            updata.Add("Turtle_Times", Turtle_Times.ToString());
            updata.Add("Turtle_Bet", Turtle_Bet.ToString());
            updata.Add("Turtle_Win", Turtle_Win.ToString());
            updata.Add("AnglerFish_Times", AnglerFish_Times.ToString());
            updata.Add("AnglerFish_Bet", AnglerFish_Bet.ToString());
            updata.Add("AnglerFish_Win", AnglerFish_Win.ToString());
            updata.Add("Octopus_Times", Octopus_Times.ToString());
            updata.Add("Octopus_Bet", Octopus_Bet.ToString());
            updata.Add("Octopus_Win", Octopus_Win.ToString());
            updata.Add("SwordFish_Times", SwordFish_Times.ToString());
            updata.Add("SwordFish_Bet", SwordFish_Bet.ToString());
            updata.Add("SwordFish_Win", SwordFish_Win.ToString());
            updata.Add("Lobster_Times", Lobster_Times.ToString());
            updata.Add("Lobster_Bet", Lobster_Bet.ToString());
            updata.Add("Lobster_Win", Lobster_Win.ToString());
            updata.Add("YellowTang_Times", YellowTang_Times.ToString());
            updata.Add("YellowTang_Bet", YellowTang_Bet.ToString());
            updata.Add("YellowTang_Win", YellowTang_Win.ToString());
            updata.Add("Pterois_Times", Pterois_Times.ToString());
            updata.Add("Pterois_Bet", Pterois_Bet.ToString());
            updata.Add("Pterois_Win", Pterois_Win.ToString());
            updata.Add("Puffer_Times", Puffer_Times.ToString());
            updata.Add("Puffer_Bet", Puffer_Bet.ToString());
            updata.Add("Puffer_Win", Puffer_Win.ToString());
            updata.Add("MoorishIdol_Times", MoorishIdol_Times.ToString());
            updata.Add("MoorishIdol_Bet", MoorishIdol_Bet.ToString());
            updata.Add("MoorishIdol_Win", MoorishIdol_Win.ToString());
            updata.Add("ClownFish_Times", ClownFish_Times.ToString());
            updata.Add("ClownFish_Bet", ClownFish_Bet.ToString());
            updata.Add("ClownFish_Win", ClownFish_Win.ToString());
            updata.Add("FlyingFish_Times", FlyingFish_Times.ToString());
            updata.Add("FlyingFish_Bet", FlyingFish_Bet.ToString());
            updata.Add("FlyingFish_Win", FlyingFish_Win.ToString());

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

            ThunderDragon_Times = 0;
            ThunderDragon_Bet = 0;
            ThunderDragon_Win = 0;
            //PurpleDragon_Times = 0;
            //PurpleDragon_Bet = 0;
            //PurpleDragon_Win = 0;
            //Phoenix_Times = 0;
            //Phoenix_Bet = 0;
            //Phoenix_Win = 0;
            //Mermaid_Times = 0;
            //Mermaid_Bet = 0;
            //Mermaid_Win = 0;
            Behemoth_Times = 0;
            Behemoth_Bet = 0;
            Behemoth_Win = 0;
            //GiantCrocodile_Times = 0;
            //GiantCrocodile_Bet = 0;
            //GiantCrocodile_Win = 0;
            GiantOctopus_Times = 0;
            GiantOctopus_Bet = 0;
            GiantOctopus_Win = 0;
            //GiantCrab_Times = 0;
            //GiantCrab_Bet = 0;
            //GiantCrab_Win = 0;
            //FireTurtle_Times = 0;
            //FireTurtle_Bet = 0;
            //FireTurtle_Win = 0;
            //FireDragon_Times = 0;
            //FireDragon_Bet = 0;
            //FireDragon_Win = 0;
            JellyFish_Times = 0;
            JellyFish_Bet = 0;
            JellyFish_Win = 0;
            BombCrab_Times = 0;
            BombCrab_Bet = 0;
            BombCrab_Win = 0;
            DrillCrab_Times = 0;
            DrillCrab_Bet = 0;
            DrillCrab_Win = 0;
            LaserCrab_Times = 0;
            LaserCrab_Bet = 0;
            LaserCrab_Win = 0;
            LungPanCrab_Times = 0;
            LungPanCrab_Bet = 0;
            LungPanCrab_Win = 0;
            ThunderCrab_Times = 0;
            ThunderCrab_Bet = 0;
            ThunderCrab_Win = 0;
            UnicornWhale_Times = 0;
            UnicornWhale_Bet = 0;
            UnicornWhale_Win = 0;
            KillerWhale_Times = 0;
            KillerWhale_Bet = 0;
            KillerWhale_Win = 0;
            Shark_Times = 0;
            Shark_Bet = 0;
            Shark_Win = 0;
            PufferGold_Times = 0;
            PufferGold_Bet = 0;
            PufferGold_Win = 0;
            MoorishIdolGold_Times = 0;
            MoorishIdolGold_Bet = 0;
            MoorishIdolGold_Win = 0;
            ClownFishGold_Times = 0;
            ClownFishGold_Bet = 0;
            ClownFishGold_Win = 0;
            PufferBig_Times = 0;
            PufferBig_Bet = 0;
            PufferBig_Win = 0;
            MoorishIdolBig_Times = 0;
            MoorishIdolBig_Bet = 0;
            MoorishIdolBig_Win = 0;
            ClownFishBig_Times = 0;
            ClownFishBig_Bet = 0;
            ClownFishBig_Win = 0;
            Mobula_Times = 0;
            Mobula_Bet = 0;
            Mobula_Win = 0;
            Stingray_Times = 0;
            Stingray_Bet = 0;
            Stingray_Win = 0;
            Turtle_Times = 0;
            Turtle_Bet = 0;
            Turtle_Win = 0;
            AnglerFish_Times = 0;
            AnglerFish_Bet = 0;
            AnglerFish_Win = 0;
            Octopus_Times = 0;
            Octopus_Bet = 0;
            Octopus_Win = 0;
            SwordFish_Times = 0;
            SwordFish_Bet = 0;
            SwordFish_Win = 0;
            Lobster_Times = 0;
            Lobster_Bet = 0;
            Lobster_Win = 0;
            YellowTang_Times = 0;
            YellowTang_Bet = 0;
            YellowTang_Win = 0;
            Pterois_Times = 0;
            Pterois_Bet = 0;
            Pterois_Win = 0;
            Puffer_Times = 0;
            Puffer_Bet = 0;
            Puffer_Win = 0;
            MoorishIdol_Times = 0;
            MoorishIdol_Bet = 0;
            MoorishIdol_Win = 0;
            ClownFish_Times = 0;
            ClownFish_Bet = 0;
            ClownFish_Win = 0;
            FlyingFish_Times = 0;
            FlyingFish_Bet = 0;
            FlyingFish_Win = 0;
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
            TotalBet = Convert.ToDecimal(datalist["TotalBet"]);
            TotalWin = Convert.ToDecimal(datalist["TotalWin"]);
            TotalSurplus = Convert.ToDecimal(datalist["TotalSurplus"]);
            GameTimes = Convert.ToInt32(datalist["GameTimes"]);
            WinTimes = Convert.ToInt32(datalist["WinTimes"]);

            Super_Times = Convert.ToInt32(datalist["Super_Times"]);
            Super_Bet = Convert.ToDouble(datalist["Super_Bet"]);
            Super_Win = Convert.ToDecimal(datalist["Super_Win"]);
            Mega_Times = Convert.ToInt32(datalist["Mega_Times"]);
            Mega_Bet = Convert.ToDouble(datalist["Mega_Bet"]);
            Mega_Win = Convert.ToDecimal(datalist["Mega_Win"]);
            Major_Times = Convert.ToInt32(datalist["Major_Times"]);
            Major_Bet = Convert.ToDouble(datalist["Major_Bet"]);
            Major_Win = Convert.ToDecimal(datalist["Major_Win"]);
            Minor_Times = Convert.ToInt32(datalist["Minor_Times"]);
            Minor_Bet = Convert.ToDouble(datalist["Minor_Bet"]);
            Minor_Win = Convert.ToDecimal(datalist["Minor_Win"]);

            ThunderDragon_Times = Convert.ToInt32(datalist["ThunderDragon_Times"]);
            ThunderDragon_Bet = Convert.ToDouble(datalist["ThunderDragon_Bet"]);
            ThunderDragon_Win = Convert.ToDouble(datalist["ThunderDragon_Win"]);
            //PurpleDragon_Times = Convert.ToInt32(datalist["PurpleDragon_Times"]);
            //PurpleDragon_Bet = Convert.ToDouble(datalist["PurpleDragon_Bet"]);
            //PurpleDragon_Win = Convert.ToDouble(datalist["PurpleDragon_Win"]);
            //Phoenix_Times = Convert.ToInt32(datalist["Phoenix_Times"]);
            //Phoenix_Bet = Convert.ToDouble(datalist["Phoenix_Bet"]);
            //Phoenix_Win = Convert.ToDouble(datalist["Phoenix_Win"]);
            //Mermaid_Times = Convert.ToInt32(datalist["Mermaid_Times"]);
            //Mermaid_Bet = Convert.ToDouble(datalist["Mermaid_Bet"]);
            //Mermaid_Win = Convert.ToDouble(datalist["Mermaid_Win"]);
            Behemoth_Times = Convert.ToInt32(datalist["Behemoth_Times"]);
            Behemoth_Bet = Convert.ToDouble(datalist["Behemoth_Bet"]);
            Behemoth_Win = Convert.ToDouble(datalist["Behemoth_Win"]);
            //GiantCrocodile_Times = Convert.ToInt32(datalist["GiantCrocodile_Times"]);
            //GiantCrocodile_Bet = Convert.ToDouble(datalist["GiantCrocodile_Bet"]);
            //GiantCrocodile_Win = Convert.ToDouble(datalist["GiantCrocodile_Win"]);
            GiantOctopus_Times = Convert.ToInt32(datalist["GiantOctopus_Times"]);
            GiantOctopus_Bet = Convert.ToDouble(datalist["GiantOctopus_Bet"]);
            GiantOctopus_Win = Convert.ToDouble(datalist["GiantOctopus_Win"]);
            //GiantCrab_Times = Convert.ToInt32(datalist["GiantCrab_Times"]);
            //GiantCrab_Bet = Convert.ToDouble(datalist["GiantCrab_Bet"]);
            //GiantCrab_Win = Convert.ToDouble(datalist["GiantCrab_Win"]);
            //FireTurtle_Times = Convert.ToInt32(datalist["FireTurtle_Times"]);
            //FireTurtle_Bet = Convert.ToDouble(datalist["FireTurtle_Bet"]);
            //FireTurtle_Win = Convert.ToDouble(datalist["FireTurtle_Win"]);
            //FireDragon_Times = Convert.ToInt32(datalist["FireDragon_Times"]);
            //FireDragon_Bet = Convert.ToDouble(datalist["FireDragon_Bet"]);
            //FireDragon_Win = Convert.ToDouble(datalist["FireDragon_Win"]);
            JellyFish_Times = Convert.ToInt32(datalist["JellyFish_Times"]);
            JellyFish_Bet = Convert.ToDouble(datalist["JellyFish_Bet"]);
            JellyFish_Win = Convert.ToDouble(datalist["JellyFish_Win"]);
            BombCrab_Times = Convert.ToInt32(datalist["BombCrab_Times"]);
            BombCrab_Bet = Convert.ToDouble(datalist["BombCrab_Bet"]);
            BombCrab_Win = Convert.ToDouble(datalist["BombCrab_Win"]);
            DrillCrab_Times = Convert.ToInt32(datalist["DrillCrab_Times"]);
            DrillCrab_Bet = Convert.ToDouble(datalist["DrillCrab_Bet"]);
            DrillCrab_Win = Convert.ToDouble(datalist["DrillCrab_Win"]);
            LaserCrab_Times = Convert.ToInt32(datalist["LaserCrab_Times"]);
            LaserCrab_Bet = Convert.ToDouble(datalist["LaserCrab_Bet"]);
            LaserCrab_Win = Convert.ToDouble(datalist["LaserCrab_Win"]);
            LungPanCrab_Times = Convert.ToInt32(datalist["LungPanCrab_Times"]);
            LungPanCrab_Bet = Convert.ToDouble(datalist["LungPanCrab_Bet"]);
            LungPanCrab_Win = Convert.ToDouble(datalist["LungPanCrab_Win"]);
            ThunderCrab_Times = Convert.ToInt32(datalist["ThunderCrab_Times"]);
            ThunderCrab_Bet = Convert.ToDouble(datalist["ThunderCrab_Bet"]);
            ThunderCrab_Win = Convert.ToDouble(datalist["ThunderCrab_Win"]);
            UnicornWhale_Times = Convert.ToInt32(datalist["UnicornWhale_Times"]);
            UnicornWhale_Bet = Convert.ToDouble(datalist["UnicornWhale_Bet"]);
            UnicornWhale_Win = Convert.ToDouble(datalist["UnicornWhale_Win"]);
            KillerWhale_Times = Convert.ToInt32(datalist["KillerWhale_Times"]);
            KillerWhale_Bet = Convert.ToDouble(datalist["KillerWhale_Bet"]);
            KillerWhale_Win = Convert.ToDouble(datalist["KillerWhale_Win"]);
            Shark_Times = Convert.ToInt32(datalist["Shark_Times"]);
            Shark_Bet = Convert.ToDouble(datalist["Shark_Bet"]);
            Shark_Win = Convert.ToDouble(datalist["Shark_Win"]);
            PufferGold_Times = Convert.ToInt32(datalist["PufferGold_Times"]);
            PufferGold_Bet = Convert.ToDouble(datalist["PufferGold_Bet"]);
            PufferGold_Win = Convert.ToDouble(datalist["PufferGold_Win"]);
            MoorishIdolGold_Times = Convert.ToInt32(datalist["MoorishIdolGold_Times"]);
            MoorishIdolGold_Bet = Convert.ToDouble(datalist["MoorishIdolGold_Bet"]);
            MoorishIdolGold_Win = Convert.ToDouble(datalist["MoorishIdolGold_Win"]);
            ClownFishGold_Times = Convert.ToInt32(datalist["ClownFishGold_Times"]);
            ClownFishGold_Bet = Convert.ToDouble(datalist["ClownFishGold_Bet"]);
            ClownFishGold_Win = Convert.ToDouble(datalist["ClownFishGold_Win"]);
            PufferBig_Times = Convert.ToInt32(datalist["PufferBig_Times"]);
            PufferBig_Bet = Convert.ToDouble(datalist["PufferBig_Bet"]);
            PufferBig_Win = Convert.ToDouble(datalist["PufferBig_Win"]);
            MoorishIdolBig_Times = Convert.ToInt32(datalist["MoorishIdolBig_Times"]);
            MoorishIdolBig_Bet = Convert.ToDouble(datalist["MoorishIdolBig_Bet"]);
            MoorishIdolBig_Win = Convert.ToDouble(datalist["MoorishIdolBig_Win"]);
            ClownFishBig_Times = Convert.ToInt32(datalist["ClownFishBig_Times"]);
            ClownFishBig_Bet = Convert.ToDouble(datalist["ClownFishBig_Bet"]);
            ClownFishBig_Win = Convert.ToDouble(datalist["ClownFishBig_Win"]);
            Mobula_Times = Convert.ToInt32(datalist["Mobula_Times"]);
            Mobula_Bet = Convert.ToDouble(datalist["Mobula_Bet"]);
            Mobula_Win = Convert.ToDouble(datalist["Mobula_Win"]);
            Stingray_Times = Convert.ToInt32(datalist["Stingray_Times"]);
            Stingray_Bet = Convert.ToDouble(datalist["Stingray_Bet"]);
            Stingray_Win = Convert.ToDouble(datalist["Stingray_Win"]);
            Turtle_Times = Convert.ToInt32(datalist["Turtle_Times"]);
            Turtle_Bet = Convert.ToDouble(datalist["Turtle_Bet"]);
            Turtle_Win = Convert.ToDouble(datalist["Turtle_Win"]);
            AnglerFish_Times = Convert.ToInt32(datalist["AnglerFish_Times"]);
            AnglerFish_Bet = Convert.ToDouble(datalist["AnglerFish_Bet"]);
            AnglerFish_Win = Convert.ToDouble(datalist["AnglerFish_Win"]);
            Octopus_Times = Convert.ToInt32(datalist["Octopus_Times"]);
            Octopus_Bet = Convert.ToDouble(datalist["Octopus_Bet"]);
            Octopus_Win = Convert.ToDouble(datalist["Octopus_Win"]);
            SwordFish_Times = Convert.ToInt32(datalist["SwordFish_Times"]);
            SwordFish_Bet = Convert.ToDouble(datalist["SwordFish_Bet"]);
            SwordFish_Win = Convert.ToDouble(datalist["SwordFish_Win"]);
            Lobster_Times = Convert.ToInt32(datalist["Lobster_Times"]);
            Lobster_Bet = Convert.ToDouble(datalist["Lobster_Bet"]);
            Lobster_Win = Convert.ToDouble(datalist["Lobster_Win"]);
            YellowTang_Times = Convert.ToInt32(datalist["YellowTang_Times"]);
            YellowTang_Bet = Convert.ToDouble(datalist["YellowTang_Bet"]);
            YellowTang_Win = Convert.ToDouble(datalist["YellowTang_Win"]);
            Pterois_Times = Convert.ToInt32(datalist["Pterois_Times"]);
            Pterois_Bet = Convert.ToDouble(datalist["Pterois_Bet"]);
            Pterois_Win = Convert.ToDouble(datalist["Pterois_Win"]);
            Puffer_Times = Convert.ToInt32(datalist["Puffer_Times"]);
            Puffer_Bet = Convert.ToDouble(datalist["Puffer_Bet"]);
            Puffer_Win = Convert.ToDouble(datalist["Puffer_Win"]);
            MoorishIdol_Times = Convert.ToInt32(datalist["MoorishIdol_Times"]);
            MoorishIdol_Bet = Convert.ToDouble(datalist["MoorishIdol_Bet"]);
            MoorishIdol_Win = Convert.ToDouble(datalist["MoorishIdol_Win"]);
            ClownFish_Times = Convert.ToInt32(datalist["ClownFish_Times"]);
            ClownFish_Bet = Convert.ToDouble(datalist["ClownFish_Bet"]);
            ClownFish_Win = Convert.ToDouble(datalist["ClownFish_Win"]);
            FlyingFish_Times = Convert.ToInt32(datalist["FlyingFish_Times"]);
            FlyingFish_Bet = Convert.ToDouble(datalist["FlyingFish_Bet"]);
            FlyingFish_Win = Convert.ToDouble(datalist["FlyingFish_Win"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);
            update = false;
        }
    }
}
