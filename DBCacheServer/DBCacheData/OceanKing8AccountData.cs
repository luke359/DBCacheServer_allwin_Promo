using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class OceanKing8AccountData
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
        public int Buddha_Times;
        public double Buddha_Bet;
        public double Buddha_Win;
        public int Poseidon_Times;
        public double Poseidon_Bet;
        public double Poseidon_Win;
        public int IceDragon_Times;
        public double IceDragon_Bet;
        public double IceDragon_Win;
        public int GoldenSpiderCrab_Times;
        public double GoldenSpiderCrab_Bet;
        public double GoldenSpiderCrab_Win;
        public int IcePhoenix_Times;
        public double IcePhoenix_Bet;
        public double IcePhoenix_Win;
        public int Mermaid_Times;
        public double Mermaid_Bet;
        public double Mermaid_Win;
        public int ThunderDragon_Times;
        public double ThunderDragon_Bet;
        public double ThunderDragon_Win;
        public int PurpleDragon_Times;
        public double PurpleDragon_Bet;
        public double PurpleDragon_Win;
        //
        public int GeneralLobster_Times;
        public double GeneralLobster_Bet;
        public double GeneralLobster_Win;
        public int GiantSquid_Times;
        public double GiantSquid_Bet;
        public double GiantSquid_Win;
        //特殊魚種 (武器蟹)
        public int ThunderCrab_Times;
        public double ThunderCrab_Bet;
        public double ThunderCrab_Win;
        public int BombCrab_Times;
        public double BombCrab_Bet;
        public double BombCrab_Win;
        public int DrillCrab_Times;
        public double DrillCrab_Bet;
        public double DrillCrab_Win;
        public int LaserCrab_Times;
        public double LaserCrab_Bet;
        public double LaserCrab_Win;
        public int Lightning_Times;
        public double Lightning_Bet;
        public double Lightning_Win;
        public int Tornato_Times;
        public double Tornato_Bet;
        public double Tornato_Win;
        //一般魚種
        public int HumpbackWhale_Times;
        public double HumpbackWhale_Bet;
        public double HumpbackWhale_Win;
        public int KillerWhale_Times;
        public double KillerWhale_Bet;
        public double KillerWhale_Win;
        public int Shark_Times;
        public double Shark_Bet;
        public double Shark_Win;
        public int GiantPuffer_Times;
        public double GiantPuffer_Bet;
        public double GiantPuffer_Win;
        public int GiantNemo_Times;
        public double GiantNemo_Bet;
        public double GiantNemo_Win;
        public int GiantCoralFish_Times;
        public double GiantCoralFish_Bet;
        public double GiantCoralFish_Win;
        public int Mobula_Times;
        public double Mobula_Bet;
        public double Mobula_Win;
        public int SawtoothShark_Times;
        public double SawtoothShark_Bet;
        public double SawtoothShark_Win;
        public int Turtle_Times;
        public double Turtle_Bet;
        public double Turtle_Win;
        public int AnglerFish_Times;
        public double AnglerFish_Bet;
        public double AnglerFish_Win;
        public int Octopus_Times;
        public double Octopus_Bet;
        public double Octopus_Win;
        public int SwordFish_Times;
        public double SwordFish_Bet;
        public double SwordFish_Win;
        public int Lobster_Times;
        public double Lobster_Bet;
        public double Lobster_Win;
        public int FlatFish_Times;
        public double FlatFish_Bet;
        public double FlatFish_Win;
        public int Pterois_Times;
        public double Pterois_Bet;
        public double Pterois_Win;
        public int Puffer_Times;
        public double Puffer_Bet;
        public double Puffer_Win;
        public int Carp_Times;
        public double Carp_Bet;
        public double Carp_Win;
        public int ClownFish_Times;
        public double ClownFish_Bet;
        public double ClownFish_Win;
        public int FlyingFish_Times;
        public double FlyingFish_Bet;
        public double FlyingFish_Win;
        public int DragonBoat_Times;
        public double DragonBoat_Bet;
        public double DragonBoat_Win;
        public int MoonRabbit_Times;
        public double MoonRabbit_Bet;
        public double MoonRabbit_Win;
        public int LionDance_Times;
        public double LionDance_Bet;
        public double LionDance_Win;

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

            else if (field == "Buddha_Times") Buddha_Times += Convert.ToInt32(value);
            else if (field == "Buddha_Bet") Buddha_Bet += Convert.ToDouble(value);
            else if (field == "Buddha_Win") Buddha_Win += Convert.ToDouble(value);
            else if (field == "Poseidon_Times") Poseidon_Times += Convert.ToInt32(value);
            else if (field == "Poseidon_Bet") Poseidon_Bet += Convert.ToDouble(value);
            else if (field == "Poseidon_Win") Poseidon_Win += Convert.ToDouble(value);
            else if (field == "IceDragon_Times") IceDragon_Times += Convert.ToInt32(value);
            else if (field == "IceDragon_Bet") IceDragon_Bet += Convert.ToDouble(value);
            else if (field == "IceDragon_Win") IceDragon_Win += Convert.ToDouble(value);
            else if (field == "GoldenSpiderCrab_Times") GoldenSpiderCrab_Times += Convert.ToInt32(value);
            else if (field == "GoldenSpiderCrab_Bet") GoldenSpiderCrab_Bet += Convert.ToDouble(value);
            else if (field == "GoldenSpiderCrab_Win") GoldenSpiderCrab_Win += Convert.ToDouble(value);
            else if (field == "IcePhoenix_Times") IcePhoenix_Times += Convert.ToInt32(value);
            else if (field == "IcePhoenix_Bet") IcePhoenix_Bet += Convert.ToDouble(value);
            else if (field == "IcePhoenix_Win") IcePhoenix_Win += Convert.ToDouble(value);
            else if (field == "Mermaid_Times") Mermaid_Times += Convert.ToInt32(value);
            else if (field == "Mermaid_Bet") Mermaid_Bet += Convert.ToDouble(value);
            else if (field == "Mermaid_Win") Mermaid_Win += Convert.ToDouble(value);
            else if (field == "ThunderDragon_Times") ThunderDragon_Times += Convert.ToInt32(value);
            else if (field == "ThunderDragon_Bet") ThunderDragon_Bet += Convert.ToDouble(value);
            else if (field == "ThunderDragon_Win") ThunderDragon_Win += Convert.ToDouble(value);
            else if (field == "PurpleDragon_Times") PurpleDragon_Times += Convert.ToInt32(value);
            else if (field == "PurpleDragon_Bet") PurpleDragon_Bet += Convert.ToDouble(value);
            else if (field == "PurpleDragon_Win") PurpleDragon_Win += Convert.ToDouble(value);
            else if (field == "GeneralLobster_Times") GeneralLobster_Times += Convert.ToInt32(value);
            else if (field == "GeneralLobster_Bet") GeneralLobster_Bet += Convert.ToDouble(value);
            else if (field == "GeneralLobster_Win") GeneralLobster_Win += Convert.ToDouble(value);
            else if (field == "GiantSquid_Times") GiantSquid_Times += Convert.ToInt32(value);
            else if (field == "GiantSquid_Bet") GiantSquid_Bet += Convert.ToDouble(value);
            else if (field == "GiantSquid_Win") GiantSquid_Win += Convert.ToDouble(value);
            else if (field == "ThunderCrab_Times") ThunderCrab_Times += Convert.ToInt32(value);
            else if (field == "ThunderCrab_Bet") ThunderCrab_Bet += Convert.ToDouble(value);
            else if (field == "ThunderCrab_Win") ThunderCrab_Win += Convert.ToDouble(value);
            else if (field == "BombCrab_Times") BombCrab_Times += Convert.ToInt32(value);
            else if (field == "BombCrab_Bet") BombCrab_Bet += Convert.ToDouble(value);
            else if (field == "BombCrab_Win") BombCrab_Win += Convert.ToDouble(value);
            else if (field == "DrillCrab_Times") DrillCrab_Times += Convert.ToInt32(value);
            else if (field == "DrillCrab_Bet") DrillCrab_Bet += Convert.ToDouble(value);
            else if (field == "DrillCrab_Win") DrillCrab_Win += Convert.ToDouble(value);
            else if (field == "LaserCrab_Times") LaserCrab_Times += Convert.ToInt32(value);
            else if (field == "LaserCrab_Bet") LaserCrab_Bet += Convert.ToDouble(value);
            else if (field == "LaserCrab_Win") LaserCrab_Win += Convert.ToDouble(value);
            else if (field == "Lightning_Times") Lightning_Times += Convert.ToInt32(value);
            else if (field == "Lightning_Bet") Lightning_Bet += Convert.ToDouble(value);
            else if (field == "Lightning_Win") Lightning_Win += Convert.ToDouble(value);
            else if (field == "Tornato_Times") Tornato_Times += Convert.ToInt32(value);
            else if (field == "Tornato_Bet") Tornato_Bet += Convert.ToDouble(value);
            else if (field == "Tornato_Win") Tornato_Win += Convert.ToDouble(value);
            else if (field == "HumpbackWhale_Times") HumpbackWhale_Times += Convert.ToInt32(value);
            else if (field == "HumpbackWhale_Bet") HumpbackWhale_Bet += Convert.ToDouble(value);
            else if (field == "HumpbackWhale_Win") HumpbackWhale_Win += Convert.ToDouble(value);
            else if (field == "KillerWhale_Times") KillerWhale_Times += Convert.ToInt32(value);
            else if (field == "KillerWhale_Bet") KillerWhale_Bet += Convert.ToDouble(value);
            else if (field == "KillerWhale_Win") KillerWhale_Win += Convert.ToDouble(value);
            else if (field == "Shark_Times") Shark_Times += Convert.ToInt32(value);
            else if (field == "Shark_Bet") Shark_Bet += Convert.ToDouble(value);
            else if (field == "Shark_Win") Shark_Win += Convert.ToDouble(value);
            else if (field == "GiantPuffer_Times") GiantPuffer_Times += Convert.ToInt32(value);
            else if (field == "GiantPuffer_Bet") GiantPuffer_Bet += Convert.ToDouble(value);
            else if (field == "GiantPuffer_Win") GiantPuffer_Win += Convert.ToDouble(value);
            else if (field == "GiantNemo_Times") GiantNemo_Times += Convert.ToInt32(value);
            else if (field == "GiantNemo_Bet") GiantNemo_Bet += Convert.ToDouble(value);
            else if (field == "GiantNemo_Win") GiantNemo_Win += Convert.ToDouble(value);
            else if (field == "GiantCoralFish_Times") GiantCoralFish_Times += Convert.ToInt32(value);
            else if (field == "GiantCoralFish_Bet") GiantCoralFish_Bet += Convert.ToDouble(value);
            else if (field == "GiantCoralFish_Win") GiantCoralFish_Win += Convert.ToDouble(value);
            else if (field == "Mobula_Times") Mobula_Times += Convert.ToInt32(value);
            else if (field == "Mobula_Bet") Mobula_Bet += Convert.ToDouble(value);
            else if (field == "Mobula_Win") Mobula_Win += Convert.ToDouble(value);
            else if (field == "SawtoothShark_Times") SawtoothShark_Times += Convert.ToInt32(value);
            else if (field == "SawtoothShark_Bet") SawtoothShark_Bet += Convert.ToDouble(value);
            else if (field == "SawtoothShark_Win") SawtoothShark_Win += Convert.ToDouble(value);
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
            else if (field == "FlatFish_Times") FlatFish_Times += Convert.ToInt32(value);
            else if (field == "FlatFish_Bet") FlatFish_Bet += Convert.ToDouble(value);
            else if (field == "FlatFish_Win") FlatFish_Win += Convert.ToDouble(value);
            else if (field == "Pterois_Times") Pterois_Times += Convert.ToInt32(value);
            else if (field == "Pterois_Bet") Pterois_Bet += Convert.ToDouble(value);
            else if (field == "Pterois_Win") Pterois_Win += Convert.ToDouble(value);
            else if (field == "Puffer_Times") Puffer_Times += Convert.ToInt32(value);
            else if (field == "Puffer_Bet") Puffer_Bet += Convert.ToDouble(value);
            else if (field == "Puffer_Win") Puffer_Win += Convert.ToDouble(value);
            else if (field == "Carp_Times") Carp_Times += Convert.ToInt32(value);
            else if (field == "Carp_Bet") Carp_Bet += Convert.ToDouble(value);
            else if (field == "Carp_Win") Carp_Win += Convert.ToDouble(value);
            else if (field == "ClownFish_Times") ClownFish_Times += Convert.ToInt32(value);
            else if (field == "ClownFish_Bet") ClownFish_Bet += Convert.ToDouble(value);
            else if (field == "ClownFish_Win") ClownFish_Win += Convert.ToDouble(value);
            else if (field == "FlyingFish_Times") FlyingFish_Times += Convert.ToInt32(value);
            else if (field == "FlyingFish_Bet") FlyingFish_Bet += Convert.ToDouble(value);
            else if (field == "FlyingFish_Win") FlyingFish_Win += Convert.ToDouble(value);
            else if (field == "DragonBoat_Times") DragonBoat_Times += Convert.ToInt32(value);
            else if (field == "DragonBoat_Bet") DragonBoat_Bet += Convert.ToDouble(value);
            else if (field == "DragonBoat_Win") DragonBoat_Win += Convert.ToDouble(value);
            else if (field == "MoonRabbit_Times") MoonRabbit_Times += Convert.ToInt32(value);
            else if (field == "MoonRabbit_Bet") MoonRabbit_Bet += Convert.ToDouble(value);
            else if (field == "MoonRabbit_Win") MoonRabbit_Win += Convert.ToDouble(value);
            else if (field == "LionDance_Times") LionDance_Times += Convert.ToInt32(value);
            else if (field == "LionDance_Bet") LionDance_Bet += Convert.ToDouble(value);
            else if (field == "LionDance_Win") LionDance_Win += Convert.ToDouble(value);
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

            updata.Add("Buddha_Times", Buddha_Times.ToString());
            updata.Add("Buddha_Bet", Buddha_Bet.ToString());
            updata.Add("Buddha_Win", Buddha_Win.ToString());
            updata.Add("Poseidon_Times", Poseidon_Times.ToString());
            updata.Add("Poseidon_Bet", Poseidon_Bet.ToString());
            updata.Add("Poseidon_Win", Poseidon_Win.ToString());
            updata.Add("IceDragon_Times", IceDragon_Times.ToString());
            updata.Add("IceDragon_Bet", IceDragon_Bet.ToString());
            updata.Add("IceDragon_Win", IceDragon_Win.ToString());
            updata.Add("GoldenSpiderCrab_Times", GoldenSpiderCrab_Times.ToString());
            updata.Add("GoldenSpiderCrab_Bet", GoldenSpiderCrab_Bet.ToString());
            updata.Add("GoldenSpiderCrab_Win", GoldenSpiderCrab_Win.ToString());
            updata.Add("IcePhoenix_Times", IcePhoenix_Times.ToString());
            updata.Add("IcePhoenix_Bet", IcePhoenix_Bet.ToString());
            updata.Add("IcePhoenix_Win", IcePhoenix_Win.ToString());
            updata.Add("Mermaid_Times", Mermaid_Times.ToString());
            updata.Add("Mermaid_Bet", Mermaid_Bet.ToString());
            updata.Add("Mermaid_Win", Mermaid_Win.ToString());
            updata.Add("ThunderDragon_Times", ThunderDragon_Times.ToString());
            updata.Add("ThunderDragon_Bet", ThunderDragon_Bet.ToString());
            updata.Add("ThunderDragon_Win", ThunderDragon_Win.ToString());
            updata.Add("PurpleDragon_Times", PurpleDragon_Times.ToString());
            updata.Add("PurpleDragon_Bet", PurpleDragon_Bet.ToString());
            updata.Add("PurpleDragon_Win", PurpleDragon_Win.ToString());
            updata.Add("GeneralLobster_Times", GeneralLobster_Times.ToString());
            updata.Add("GeneralLobster_Bet", GeneralLobster_Bet.ToString());
            updata.Add("GeneralLobster_Win", GeneralLobster_Win.ToString());
            updata.Add("GiantSquid_Times", GiantSquid_Times.ToString());
            updata.Add("GiantSquid_Bet", GiantSquid_Bet.ToString());
            updata.Add("GiantSquid_Win", GiantSquid_Win.ToString());
            updata.Add("ThunderCrab_Times", ThunderCrab_Times.ToString());
            updata.Add("ThunderCrab_Bet", ThunderCrab_Bet.ToString());
            updata.Add("ThunderCrab_Win", ThunderCrab_Win.ToString());
            updata.Add("BombCrab_Times", BombCrab_Times.ToString());
            updata.Add("BombCrab_Bet", BombCrab_Bet.ToString());
            updata.Add("BombCrab_Win", BombCrab_Win.ToString());
            updata.Add("DrillCrab_Times", DrillCrab_Times.ToString());
            updata.Add("DrillCrab_Bet", DrillCrab_Bet.ToString());
            updata.Add("DrillCrab_Win", DrillCrab_Win.ToString());
            updata.Add("LaserCrab_Times", LaserCrab_Times.ToString());
            updata.Add("LaserCrab_Bet", LaserCrab_Bet.ToString());
            updata.Add("LaserCrab_Win", LaserCrab_Win.ToString());
            updata.Add("Lightning_Times", Lightning_Times.ToString());
            updata.Add("Lightning_Bet", Lightning_Bet.ToString());
            updata.Add("Lightning_Win", Lightning_Win.ToString());
            updata.Add("Tornato_Times", Tornato_Times.ToString());
            updata.Add("Tornato_Bet", Tornato_Bet.ToString());
            updata.Add("Tornato_Win", Tornato_Win.ToString());
            updata.Add("HumpbackWhale_Times", HumpbackWhale_Times.ToString());
            updata.Add("HumpbackWhale_Bet", HumpbackWhale_Bet.ToString());
            updata.Add("HumpbackWhale_Win", HumpbackWhale_Win.ToString());
            updata.Add("KillerWhale_Times", KillerWhale_Times.ToString());
            updata.Add("KillerWhale_Bet", KillerWhale_Bet.ToString());
            updata.Add("KillerWhale_Win", KillerWhale_Win.ToString());
            updata.Add("Shark_Times", Shark_Times.ToString());
            updata.Add("Shark_Bet", Shark_Bet.ToString());
            updata.Add("Shark_Win", Shark_Win.ToString());
            updata.Add("GiantPuffer_Times", GiantPuffer_Times.ToString());
            updata.Add("GiantPuffer_Bet", GiantPuffer_Bet.ToString());
            updata.Add("GiantPuffer_Win", GiantPuffer_Win.ToString());
            updata.Add("GiantNemo_Times", GiantNemo_Times.ToString());
            updata.Add("GiantNemo_Bet", GiantNemo_Bet.ToString());
            updata.Add("GiantNemo_Win", GiantNemo_Win.ToString());
            updata.Add("GiantCoralFish_Times", GiantCoralFish_Times.ToString());
            updata.Add("GiantCoralFish_Bet", GiantCoralFish_Bet.ToString());
            updata.Add("GiantCoralFish_Win", GiantCoralFish_Win.ToString());
            updata.Add("Mobula_Times", Mobula_Times.ToString());
            updata.Add("Mobula_Bet", Mobula_Bet.ToString());
            updata.Add("Mobula_Win", Mobula_Win.ToString());
            updata.Add("SawtoothShark_Times", SawtoothShark_Times.ToString());
            updata.Add("SawtoothShark_Bet", SawtoothShark_Bet.ToString());
            updata.Add("SawtoothShark_Win", SawtoothShark_Win.ToString());
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
            updata.Add("FlatFish_Times", FlatFish_Times.ToString());
            updata.Add("FlatFish_Bet", FlatFish_Bet.ToString());
            updata.Add("FlatFish_Win", FlatFish_Win.ToString());
            updata.Add("Pterois_Times", Pterois_Times.ToString());
            updata.Add("Pterois_Bet", Pterois_Bet.ToString());
            updata.Add("Pterois_Win", Pterois_Win.ToString());
            updata.Add("Puffer_Times", Puffer_Times.ToString());
            updata.Add("Puffer_Bet", Puffer_Bet.ToString());
            updata.Add("Puffer_Win", Puffer_Win.ToString());
            updata.Add("Carp_Times", Carp_Times.ToString());
            updata.Add("Carp_Bet", Carp_Bet.ToString());
            updata.Add("Carp_Win", Carp_Win.ToString());
            updata.Add("ClownFish_Times", ClownFish_Times.ToString());
            updata.Add("ClownFish_Bet", ClownFish_Bet.ToString());
            updata.Add("ClownFish_Win", ClownFish_Win.ToString());
            updata.Add("FlyingFish_Times", FlyingFish_Times.ToString());
            updata.Add("FlyingFish_Bet", FlyingFish_Bet.ToString());
            updata.Add("FlyingFish_Win", FlyingFish_Win.ToString());
            updata.Add("DragonBoat_Times", DragonBoat_Times.ToString());
            updata.Add("DragonBoat_Bet", DragonBoat_Bet.ToString());
            updata.Add("DragonBoat_Win", DragonBoat_Win.ToString());
            updata.Add("MoonRabbit_Times", MoonRabbit_Times.ToString());
            updata.Add("MoonRabbit_Bet", MoonRabbit_Bet.ToString());
            updata.Add("MoonRabbit_Win", MoonRabbit_Win.ToString());
            updata.Add("LionDance_Times", LionDance_Times.ToString());
            updata.Add("LionDance_Bet", LionDance_Bet.ToString());
            updata.Add("LionDance_Win", LionDance_Win.ToString());

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

            Buddha_Times = 0;
            Buddha_Bet = 0;
            Buddha_Win = 0;
            Poseidon_Times = 0;
            Poseidon_Bet = 0;
            Poseidon_Win = 0;
            IceDragon_Times = 0;
            IceDragon_Bet = 0;
            IceDragon_Win = 0;
            GoldenSpiderCrab_Times = 0;
            GoldenSpiderCrab_Bet = 0;
            GoldenSpiderCrab_Win = 0;
            IcePhoenix_Times = 0;
            IcePhoenix_Bet = 0;
            IcePhoenix_Win = 0;
            Mermaid_Times = 0;
            Mermaid_Bet = 0;
            Mermaid_Win = 0;
            ThunderDragon_Times = 0;
            ThunderDragon_Bet = 0;
            ThunderDragon_Win = 0;
            PurpleDragon_Times = 0;
            PurpleDragon_Bet = 0;
            PurpleDragon_Win = 0;
            GeneralLobster_Times = 0;
            GeneralLobster_Bet = 0;
            GeneralLobster_Win = 0;
            GiantSquid_Times = 0;
            GiantSquid_Bet = 0;
            GiantSquid_Win = 0;
            ThunderCrab_Times = 0;
            ThunderCrab_Bet = 0;
            ThunderCrab_Win = 0;
            BombCrab_Times = 0;
            BombCrab_Bet = 0;
            BombCrab_Win = 0;
            DrillCrab_Times = 0;
            DrillCrab_Bet = 0;
            DrillCrab_Win = 0;
            LaserCrab_Times = 0;
            LaserCrab_Bet = 0;
            LaserCrab_Win = 0;
            Lightning_Times = 0;
            Lightning_Bet = 0;
            Lightning_Win = 0;
            Tornato_Times = 0;
            Tornato_Bet = 0;
            Tornato_Win = 0;
            HumpbackWhale_Times = 0;
            HumpbackWhale_Bet = 0;
            HumpbackWhale_Win = 0;
            KillerWhale_Times = 0;
            KillerWhale_Bet = 0;
            KillerWhale_Win = 0;
            Shark_Times = 0;
            Shark_Bet = 0;
            Shark_Win = 0;
            GiantPuffer_Times = 0;
            GiantPuffer_Bet = 0;
            GiantPuffer_Win = 0;
            GiantNemo_Times = 0;
            GiantNemo_Bet = 0;
            GiantNemo_Win = 0;
            GiantCoralFish_Times = 0;
            GiantCoralFish_Bet = 0;
            GiantCoralFish_Win = 0;
            Mobula_Times = 0;
            Mobula_Bet = 0;
            Mobula_Win = 0;
            SawtoothShark_Times = 0;
            SawtoothShark_Bet = 0;
            SawtoothShark_Win = 0;
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
            FlatFish_Times = 0;
            FlatFish_Bet = 0;
            FlatFish_Win = 0;
            Pterois_Times = 0;
            Pterois_Bet = 0;
            Pterois_Win = 0;
            Puffer_Times = 0;
            Puffer_Bet = 0;
            Puffer_Win = 0;
            Carp_Times = 0;
            Carp_Bet = 0;
            Carp_Win = 0;
            ClownFish_Times = 0;
            ClownFish_Bet = 0;
            ClownFish_Win = 0;
            FlyingFish_Times = 0;
            FlyingFish_Bet = 0;
            FlyingFish_Win = 0;
            DragonBoat_Times = 0;
            DragonBoat_Bet = 0;
            DragonBoat_Win = 0;
            MoonRabbit_Times = 0;
            MoonRabbit_Bet = 0;
            MoonRabbit_Win = 0;
            LionDance_Times = 0;
            LionDance_Bet = 0;
            LionDance_Win = 0;
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

            Buddha_Times = Convert.ToInt32(datalist["Buddha_Times"]);
            Buddha_Bet = Convert.ToDouble(datalist["Buddha_Bet"]);
            Buddha_Win = Convert.ToDouble(datalist["Buddha_Win"]);
            Poseidon_Times = Convert.ToInt32(datalist["Poseidon_Times"]);
            Poseidon_Bet = Convert.ToDouble(datalist["Poseidon_Bet"]);
            Poseidon_Win = Convert.ToDouble(datalist["Poseidon_Win"]);
            IceDragon_Times = Convert.ToInt32(datalist["IceDragon_Times"]);
            IceDragon_Bet = Convert.ToDouble(datalist["IceDragon_Bet"]);
            IceDragon_Win = Convert.ToDouble(datalist["IceDragon_Win"]);
            GoldenSpiderCrab_Times = Convert.ToInt32(datalist["GoldenSpiderCrab_Times"]);
            GoldenSpiderCrab_Bet = Convert.ToDouble(datalist["GoldenSpiderCrab_Bet"]);
            GoldenSpiderCrab_Win = Convert.ToDouble(datalist["GoldenSpiderCrab_Win"]);
            IcePhoenix_Times = Convert.ToInt32(datalist["IcePhoenix_Times"]);
            IcePhoenix_Bet = Convert.ToDouble(datalist["IcePhoenix_Bet"]);
            IcePhoenix_Win = Convert.ToDouble(datalist["IcePhoenix_Win"]);
            Mermaid_Times = Convert.ToInt32(datalist["Mermaid_Times"]);
            Mermaid_Bet = Convert.ToDouble(datalist["Mermaid_Bet"]);
            Mermaid_Win = Convert.ToDouble(datalist["Mermaid_Win"]);
            ThunderDragon_Times = Convert.ToInt32(datalist["ThunderDragon_Times"]);
            ThunderDragon_Bet = Convert.ToDouble(datalist["ThunderDragon_Bet"]);
            ThunderDragon_Win = Convert.ToDouble(datalist["ThunderDragon_Win"]);
            PurpleDragon_Times = Convert.ToInt32(datalist["PurpleDragon_Times"]);
            PurpleDragon_Bet = Convert.ToDouble(datalist["PurpleDragon_Bet"]);
            PurpleDragon_Win = Convert.ToDouble(datalist["PurpleDragon_Win"]);
            GeneralLobster_Times = Convert.ToInt32(datalist["GeneralLobster_Times"]);
            GeneralLobster_Bet = Convert.ToDouble(datalist["GeneralLobster_Bet"]);
            GeneralLobster_Win = Convert.ToDouble(datalist["GeneralLobster_Win"]);
            GiantSquid_Times = Convert.ToInt32(datalist["GiantSquid_Times"]);
            GiantSquid_Bet = Convert.ToDouble(datalist["GiantSquid_Bet"]);
            GiantSquid_Win = Convert.ToDouble(datalist["GiantSquid_Win"]);
            ThunderCrab_Times = Convert.ToInt32(datalist["ThunderCrab_Times"]);
            ThunderCrab_Bet = Convert.ToDouble(datalist["ThunderCrab_Bet"]);
            ThunderCrab_Win = Convert.ToDouble(datalist["ThunderCrab_Win"]);
            BombCrab_Times = Convert.ToInt32(datalist["BombCrab_Times"]);
            BombCrab_Bet = Convert.ToDouble(datalist["BombCrab_Bet"]);
            BombCrab_Win = Convert.ToDouble(datalist["BombCrab_Win"]);
            DrillCrab_Times = Convert.ToInt32(datalist["DrillCrab_Times"]);
            DrillCrab_Bet = Convert.ToDouble(datalist["DrillCrab_Bet"]);
            DrillCrab_Win = Convert.ToDouble(datalist["DrillCrab_Win"]);
            LaserCrab_Times = Convert.ToInt32(datalist["LaserCrab_Times"]);
            LaserCrab_Bet = Convert.ToDouble(datalist["LaserCrab_Bet"]);
            LaserCrab_Win = Convert.ToDouble(datalist["LaserCrab_Win"]);
            Lightning_Times = Convert.ToInt32(datalist["Lightning_Times"]);
            Lightning_Bet = Convert.ToDouble(datalist["Lightning_Bet"]);
            Lightning_Win = Convert.ToDouble(datalist["Lightning_Win"]);
            Tornato_Times = Convert.ToInt32(datalist["Tornato_Times"]);
            Tornato_Bet = Convert.ToDouble(datalist["Tornato_Bet"]);
            Tornato_Win = Convert.ToDouble(datalist["Tornato_Win"]);
            HumpbackWhale_Times = Convert.ToInt32(datalist["HumpbackWhale_Times"]);
            HumpbackWhale_Bet = Convert.ToDouble(datalist["HumpbackWhale_Bet"]);
            HumpbackWhale_Win = Convert.ToDouble(datalist["HumpbackWhale_Win"]);
            KillerWhale_Times = Convert.ToInt32(datalist["KillerWhale_Times"]);
            KillerWhale_Bet = Convert.ToDouble(datalist["KillerWhale_Bet"]);
            KillerWhale_Win = Convert.ToDouble(datalist["KillerWhale_Win"]);
            Shark_Times = Convert.ToInt32(datalist["Shark_Times"]);
            Shark_Bet = Convert.ToDouble(datalist["Shark_Bet"]);
            Shark_Win = Convert.ToDouble(datalist["Shark_Win"]);
            GiantPuffer_Times = Convert.ToInt32(datalist["GiantPuffer_Times"]);
            GiantPuffer_Bet = Convert.ToDouble(datalist["GiantPuffer_Bet"]);
            GiantPuffer_Win = Convert.ToDouble(datalist["GiantPuffer_Win"]);
            GiantNemo_Times = Convert.ToInt32(datalist["GiantNemo_Times"]);
            GiantNemo_Bet = Convert.ToDouble(datalist["GiantNemo_Bet"]);
            GiantNemo_Win = Convert.ToDouble(datalist["GiantNemo_Win"]);
            GiantCoralFish_Times = Convert.ToInt32(datalist["GiantCoralFish_Times"]);
            GiantCoralFish_Bet = Convert.ToDouble(datalist["GiantCoralFish_Bet"]);
            GiantCoralFish_Win = Convert.ToDouble(datalist["GiantCoralFish_Win"]);
            Mobula_Times = Convert.ToInt32(datalist["Mobula_Times"]);
            Mobula_Bet = Convert.ToDouble(datalist["Mobula_Bet"]);
            Mobula_Win = Convert.ToDouble(datalist["Mobula_Win"]);
            SawtoothShark_Times = Convert.ToInt32(datalist["SawtoothShark_Times"]);
            SawtoothShark_Bet = Convert.ToDouble(datalist["SawtoothShark_Bet"]);
            SawtoothShark_Win = Convert.ToDouble(datalist["SawtoothShark_Win"]);
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
            FlatFish_Times = Convert.ToInt32(datalist["FlatFish_Times"]);
            FlatFish_Bet = Convert.ToDouble(datalist["FlatFish_Bet"]);
            FlatFish_Win = Convert.ToDouble(datalist["FlatFish_Win"]);
            Pterois_Times = Convert.ToInt32(datalist["Pterois_Times"]);
            Pterois_Bet = Convert.ToDouble(datalist["Pterois_Bet"]);
            Pterois_Win = Convert.ToDouble(datalist["Pterois_Win"]);
            Puffer_Times = Convert.ToInt32(datalist["Puffer_Times"]);
            Puffer_Bet = Convert.ToDouble(datalist["Puffer_Bet"]);
            Puffer_Win = Convert.ToDouble(datalist["Puffer_Win"]);
            Carp_Times = Convert.ToInt32(datalist["Carp_Times"]);
            Carp_Bet = Convert.ToDouble(datalist["Carp_Bet"]);
            Carp_Win = Convert.ToDouble(datalist["Carp_Win"]);
            ClownFish_Times = Convert.ToInt32(datalist["ClownFish_Times"]);
            ClownFish_Bet = Convert.ToDouble(datalist["ClownFish_Bet"]);
            ClownFish_Win = Convert.ToDouble(datalist["ClownFish_Win"]);
            FlyingFish_Times = Convert.ToInt32(datalist["FlyingFish_Times"]);
            FlyingFish_Bet = Convert.ToDouble(datalist["FlyingFish_Bet"]);
            FlyingFish_Win = Convert.ToDouble(datalist["FlyingFish_Win"]);
            DragonBoat_Times = Convert.ToInt32(datalist["DragonBoat_Times"]);
            DragonBoat_Bet = Convert.ToDouble(datalist["DragonBoat_Bet"]);
            DragonBoat_Win = Convert.ToDouble(datalist["DragonBoat_Win"]);
            MoonRabbit_Times = Convert.ToInt32(datalist["MoonRabbit_Times"]);
            MoonRabbit_Bet = Convert.ToDouble(datalist["MoonRabbit_Bet"]);
            MoonRabbit_Win = Convert.ToDouble(datalist["MoonRabbit_Win"]);
            LionDance_Times = Convert.ToInt32(datalist["LionDance_Times"]);
            LionDance_Bet = Convert.ToDouble(datalist["LionDance_Bet"]);
            LionDance_Win = Convert.ToDouble(datalist["LionDance_Win"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);
            update = false;
        }
    }
}
