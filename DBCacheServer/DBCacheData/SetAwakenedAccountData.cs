using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class SetAwakenedAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int FreeFree_Times;
        private int AwakenedFree_Times;
        private double AwakenedFree_Bet;
        private double AwakenedFree_Win;
        private int AwakenedFreeFree_Times;
        private int RainbowCore1000_Times;
        private double RainbowCore1000_Bet;
        private double RainbowCore1000_Win;
        private int MultiCore1000_Times;
        private double MultiCore1000_Bet;
        private double MultiCore1000_Win;
        private int MultiCore100_500_Times;
        private double MultiCore100_500_Bet;
        private double MultiCore100_500_Win;
        private int MultiCore25_50_Times;
        private double MultiCore25_50_Bet;
        private double MultiCore25_50_Win;
        private int MultiCore2_20_Times;
        private double MultiCore2_20_Bet;
        private double MultiCore2_20_Win;
        private int Seth3_Times;
        private double Seth3_Bet;
        private double Seth3_Win;
        private int Nephthys3_Times;
        private double Nephthys3_Bet;
        private double Nephthys3_Win;
        private int Totem12_Times;
        private double Totem12_Bet;
        private double Totem12_Win;
        private int Cross12_Times;
        private double Cross12_Bet;
        private double Cross12_Win;
        private int Bow12_Times;
        private double Bow12_Bet;
        private double Bow12_Win;
        private int Dagger12_Times;
        private double Dagger12_Bet;
        private double Dagger12_Win;
        private int Ruby12_Times;
        private double Ruby12_Bet;
        private double Ruby12_Win;
        private int Amethyst12_Times;
        private double Amethyst12_Bet;
        private double Amethyst12_Win;
        private int YellowSapphire12_Times;
        private double YellowSapphire12_Bet;
        private double YellowSapphire12_Win;
        private int Emerald12_Times;
        private double Emerald12_Bet;
        private double Emerald12_Win;
        private int Sapphire12_Times;
        private double Sapphire12_Bet;
        private double Sapphire12_Win;

        private int IndepPlayTimes;
        private double IndepTotalBet;
        private double IndepTotalWin;
        private int IndepFreeFree_Times;
        private int IndepTotalRoundTimes;
        private int IndepTotalWinTimes;
        private int IndepFree1_Times;
        private double IndepFree1_Bet;
        private double IndepFree1_Win;
        private int IndepFree1Free_Times;
        private int IndepFree1TotalRound_Times;
        private int IndepFree1TotalWin_Times;
        private int IndepFree2_Times;
        private double IndepFree2_Bet;
        private double IndepFree2_Win;
        private int IndepFree2Free_Times;
        private int IndepFree2TotalRound_Times;
        private int IndepFree2TotalWin_Times;
        private int IndepFree3_Times;
        private double IndepFree3_Bet;
        private double IndepFree3_Win;
        private int IndepFree3Free_Times;
        private int IndepFree3TotalRound_Times;
        private int IndepFree3TotalWin_Times;

        private int ExPlay_Times;
        private int ExPlay_WinTimes;
        private double ExPlay_Bet;
        private double ExPlay_Win;
        private int ExPlayS1_Times;
        private int ExPlayS1_WinTimes;
        private double ExPlayS1_Bet;
        private double ExPlayS1_Win;
        private int ExPlayS2_Times;
        private int ExPlayS2_WinTimes;
        private double ExPlayS2_Bet;
        private double ExPlayS2_Win;
        private int ExPlayS3_Times;
        private int ExPlayS3_WinTimes;
        private double ExPlayS3_Bet;
        private double ExPlayS3_Win;
        #endregion


        /// <summary>清除額外押注內存</summary>
        public override void ClearCacheGameExPlay()
        {
            ExPlay_Times = 0;
            ExPlay_WinTimes = 0;
            ExPlay_Bet = 0;
            ExPlay_Win = 0;
            ExPlayS1_Times = 0;
            ExPlayS1_WinTimes = 0;
            ExPlayS1_Bet = 0;
            ExPlayS1_Win = 0;
            ExPlayS2_Times = 0;
            ExPlayS2_WinTimes = 0;
            ExPlayS2_Bet = 0;
            ExPlayS2_Win = 0;
            ExPlayS3_Times = 0;
            ExPlayS3_WinTimes = 0;
            ExPlayS3_Bet = 0;
            ExPlayS3_Win = 0;
            update = true;
        }

        /// <summary>清除內存(mode:0=常規, 1=獨立買, 2=全部)</summary>
        public override void ClearCacheGame(int mode)
        {
            if (mode == 0 || mode == 2)
            {
                Free_Times = 0;
                Free_Bet = 0;
                Free_Win = 0;
                FreeFree_Times = 0;
                AwakenedFree_Times = 0;
                AwakenedFree_Bet = 0;
                AwakenedFree_Win = 0;
                AwakenedFreeFree_Times = 0;
                RainbowCore1000_Times = 0;
                RainbowCore1000_Bet = 0;
                RainbowCore1000_Win = 0;
                MultiCore1000_Times = 0;
                MultiCore1000_Bet = 0;
                MultiCore1000_Win = 0;
                MultiCore100_500_Times = 0;
                MultiCore100_500_Bet = 0;
                MultiCore100_500_Win = 0;
                MultiCore25_50_Times = 0;
                MultiCore25_50_Bet = 0;
                MultiCore25_50_Win = 0;
                MultiCore2_20_Times = 0;
                MultiCore2_20_Bet = 0;
                MultiCore2_20_Win = 0;
                Seth3_Times = 0;
                Seth3_Bet = 0;
                Seth3_Win = 0;
                Nephthys3_Times = 0;
                Nephthys3_Bet = 0;
                Nephthys3_Win = 0;
                Totem12_Times = 0;
                Totem12_Bet = 0;
                Totem12_Win = 0;
                Cross12_Times = 0;
                Cross12_Bet = 0;
                Cross12_Win = 0;
                Bow12_Times = 0;
                Bow12_Bet = 0;
                Bow12_Win = 0;
                Dagger12_Times = 0;
                Dagger12_Bet = 0;
                Dagger12_Win = 0;
                Ruby12_Times = 0;
                Ruby12_Bet = 0;
                Ruby12_Win = 0;
                Amethyst12_Times = 0;
                Amethyst12_Bet = 0;
                Amethyst12_Win = 0;
                YellowSapphire12_Times = 0;
                YellowSapphire12_Bet = 0;
                YellowSapphire12_Win = 0;
                Emerald12_Times = 0;
                Emerald12_Bet = 0;
                Emerald12_Win = 0;
                Sapphire12_Times = 0;
                Sapphire12_Bet = 0;
                Sapphire12_Win = 0;
            }
            if (mode == 1 || mode == 2)
            {
                IndepPlayTimes = 0;
                IndepTotalBet = 0;
                IndepTotalWin = 0;
                IndepFreeFree_Times = 0;
                IndepTotalRoundTimes = 0;
                IndepTotalWinTimes = 0;
                IndepFree1_Times = 0;
                IndepFree1_Bet = 0;
                IndepFree1_Win = 0;
                IndepFree1Free_Times = 0;
                IndepFree1TotalRound_Times = 0;
                IndepFree1TotalWin_Times = 0;
                IndepFree2_Times = 0;
                IndepFree2_Bet = 0;
                IndepFree2_Win = 0;
                IndepFree2Free_Times = 0;
                IndepFree2TotalRound_Times = 0;
                IndepFree2TotalWin_Times = 0;
                IndepFree3_Times = 0;
                IndepFree3_Bet = 0;
                IndepFree3_Win = 0;
                IndepFree3Free_Times = 0;
                IndepFree3TotalRound_Times = 0;
                IndepFree3TotalWin_Times = 0;
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(Free_Times)) { Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Free_Bet)) { Free_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Free_Win)) { Free_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeFree_Times)) { FreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(AwakenedFree_Times)) { AwakenedFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(AwakenedFree_Bet)) { AwakenedFree_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(AwakenedFree_Win)) { AwakenedFree_Win += Convert.ToDouble(value); return; }
            if (field == nameof(AwakenedFreeFree_Times)) { AwakenedFreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RainbowCore1000_Times)) { RainbowCore1000_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RainbowCore1000_Bet)) { RainbowCore1000_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(RainbowCore1000_Win)) { RainbowCore1000_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore1000_Times)) { MultiCore1000_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore1000_Bet)) { MultiCore1000_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore1000_Win)) { MultiCore1000_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore100_500_Times)) { MultiCore100_500_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore100_500_Bet)) { MultiCore100_500_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore100_500_Win)) { MultiCore100_500_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore25_50_Times)) { MultiCore25_50_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore25_50_Bet)) { MultiCore25_50_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore25_50_Win)) { MultiCore25_50_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore2_20_Times)) { MultiCore2_20_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore2_20_Bet)) { MultiCore2_20_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore2_20_Win)) { MultiCore2_20_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Seth3_Times)) { Seth3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Seth3_Bet)) { Seth3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Seth3_Win)) { Seth3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Nephthys3_Times)) { Nephthys3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Nephthys3_Bet)) { Nephthys3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Nephthys3_Win)) { Nephthys3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Totem12_Times)) { Totem12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Totem12_Bet)) { Totem12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Totem12_Win)) { Totem12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Cross12_Times)) { Cross12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Cross12_Bet)) { Cross12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Cross12_Win)) { Cross12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bow12_Times)) { Bow12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bow12_Bet)) { Bow12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bow12_Win)) { Bow12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger12_Times)) { Dagger12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Dagger12_Bet)) { Dagger12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger12_Win)) { Dagger12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby12_Times)) { Ruby12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ruby12_Bet)) { Ruby12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby12_Win)) { Ruby12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst12_Times)) { Amethyst12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Amethyst12_Bet)) { Amethyst12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst12_Win)) { Amethyst12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(YellowSapphire12_Times)) { YellowSapphire12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(YellowSapphire12_Bet)) { YellowSapphire12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(YellowSapphire12_Win)) { YellowSapphire12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald12_Times)) { Emerald12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Emerald12_Bet)) { Emerald12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald12_Win)) { Emerald12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire12_Times)) { Sapphire12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Sapphire12_Bet)) { Sapphire12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire12_Win)) { Sapphire12_Win += Convert.ToDouble(value); return; }

            if (field == nameof(IndepPlayTimes)) { IndepPlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalBet)) { IndepTotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepTotalWin)) { IndepTotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeFree_Times)) { IndepFreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalRoundTimes)) { IndepTotalRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalWinTimes)) { IndepTotalWinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree1_Times)) { IndepFree1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree1_Bet)) { IndepFree1_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFree1_Win)) { IndepFree1_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFree1Free_Times)) { IndepFree1Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree1TotalRound_Times)) { IndepFree1TotalRound_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree1TotalWin_Times)) { IndepFree1TotalWin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree2_Times)) { IndepFree2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree2_Bet)) { IndepFree2_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFree2_Win)) { IndepFree2_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFree2Free_Times)) { IndepFree2Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree2TotalRound_Times)) { IndepFree2TotalRound_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree2TotalWin_Times)) { IndepFree2TotalWin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree3_Times)) { IndepFree3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree3_Bet)) { IndepFree3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFree3_Win)) { IndepFree3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFree3Free_Times)) { IndepFree3Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree3TotalRound_Times)) { IndepFree3TotalRound_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFree3TotalWin_Times)) { IndepFree3TotalWin_Times += Convert.ToInt32(value); return; }

            if (field == nameof(ExPlay_Times)) { ExPlay_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlay_WinTimes)) { ExPlay_WinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlay_Bet)) { ExPlay_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay_Win)) { ExPlay_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS1_Times)) { ExPlayS1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS1_WinTimes)) { ExPlayS1_WinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS1_Bet)) { ExPlayS1_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS1_Win)) { ExPlayS1_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS2_Times)) { ExPlayS2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS2_WinTimes)) { ExPlayS2_WinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS2_Bet)) { ExPlayS2_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS2_Win)) { ExPlayS2_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS3_Times)) { ExPlayS3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS3_WinTimes)) { ExPlayS3_WinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS3_Bet)) { ExPlayS3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS3_Win)) { ExPlayS3_Win += Convert.ToDouble(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(Free_Times), Free_Times.ToString());
            updata.Add(nameof(Free_Bet), Free_Bet.ToString());
            updata.Add(nameof(Free_Win), Free_Win.ToString());
            updata.Add(nameof(FreeFree_Times), FreeFree_Times.ToString());
            updata.Add(nameof(AwakenedFree_Times), AwakenedFree_Times.ToString());
            updata.Add(nameof(AwakenedFree_Bet), AwakenedFree_Bet.ToString());
            updata.Add(nameof(AwakenedFree_Win), AwakenedFree_Win.ToString());
            updata.Add(nameof(AwakenedFreeFree_Times), AwakenedFreeFree_Times.ToString());
            updata.Add(nameof(RainbowCore1000_Times), RainbowCore1000_Times.ToString());
            updata.Add(nameof(RainbowCore1000_Bet), RainbowCore1000_Bet.ToString());
            updata.Add(nameof(RainbowCore1000_Win), RainbowCore1000_Win.ToString());
            updata.Add(nameof(MultiCore1000_Times), MultiCore1000_Times.ToString());
            updata.Add(nameof(MultiCore1000_Bet), MultiCore1000_Bet.ToString());
            updata.Add(nameof(MultiCore1000_Win), MultiCore1000_Win.ToString());
            updata.Add(nameof(MultiCore100_500_Times), MultiCore100_500_Times.ToString());
            updata.Add(nameof(MultiCore100_500_Bet), MultiCore100_500_Bet.ToString());
            updata.Add(nameof(MultiCore100_500_Win), MultiCore100_500_Win.ToString());
            updata.Add(nameof(MultiCore25_50_Times), MultiCore25_50_Times.ToString());
            updata.Add(nameof(MultiCore25_50_Bet), MultiCore25_50_Bet.ToString());
            updata.Add(nameof(MultiCore25_50_Win), MultiCore25_50_Win.ToString());
            updata.Add(nameof(MultiCore2_20_Times), MultiCore2_20_Times.ToString());
            updata.Add(nameof(MultiCore2_20_Bet), MultiCore2_20_Bet.ToString());
            updata.Add(nameof(MultiCore2_20_Win), MultiCore2_20_Win.ToString());
            updata.Add(nameof(Seth3_Times), Seth3_Times.ToString());
            updata.Add(nameof(Seth3_Bet), Seth3_Bet.ToString());
            updata.Add(nameof(Seth3_Win), Seth3_Win.ToString());
            updata.Add(nameof(Nephthys3_Times), Nephthys3_Times.ToString());
            updata.Add(nameof(Nephthys3_Bet), Nephthys3_Bet.ToString());
            updata.Add(nameof(Nephthys3_Win), Nephthys3_Win.ToString());
            updata.Add(nameof(Totem12_Times), Totem12_Times.ToString());
            updata.Add(nameof(Totem12_Bet), Totem12_Bet.ToString());
            updata.Add(nameof(Totem12_Win), Totem12_Win.ToString());
            updata.Add(nameof(Cross12_Times), Cross12_Times.ToString());
            updata.Add(nameof(Cross12_Bet), Cross12_Bet.ToString());
            updata.Add(nameof(Cross12_Win), Cross12_Win.ToString());
            updata.Add(nameof(Bow12_Times), Bow12_Times.ToString());
            updata.Add(nameof(Bow12_Bet), Bow12_Bet.ToString());
            updata.Add(nameof(Bow12_Win), Bow12_Win.ToString());
            updata.Add(nameof(Dagger12_Times), Dagger12_Times.ToString());
            updata.Add(nameof(Dagger12_Bet), Dagger12_Bet.ToString());
            updata.Add(nameof(Dagger12_Win), Dagger12_Win.ToString());
            updata.Add(nameof(Ruby12_Times), Ruby12_Times.ToString());
            updata.Add(nameof(Ruby12_Bet), Ruby12_Bet.ToString());
            updata.Add(nameof(Ruby12_Win), Ruby12_Win.ToString());
            updata.Add(nameof(Amethyst12_Times), Amethyst12_Times.ToString());
            updata.Add(nameof(Amethyst12_Bet), Amethyst12_Bet.ToString());
            updata.Add(nameof(Amethyst12_Win), Amethyst12_Win.ToString());
            updata.Add(nameof(YellowSapphire12_Times), YellowSapphire12_Times.ToString());
            updata.Add(nameof(YellowSapphire12_Bet), YellowSapphire12_Bet.ToString());
            updata.Add(nameof(YellowSapphire12_Win), YellowSapphire12_Win.ToString());
            updata.Add(nameof(Emerald12_Times), Emerald12_Times.ToString());
            updata.Add(nameof(Emerald12_Bet), Emerald12_Bet.ToString());
            updata.Add(nameof(Emerald12_Win), Emerald12_Win.ToString());
            updata.Add(nameof(Sapphire12_Times), Sapphire12_Times.ToString());
            updata.Add(nameof(Sapphire12_Bet), Sapphire12_Bet.ToString());
            updata.Add(nameof(Sapphire12_Win), Sapphire12_Win.ToString());

            updata.Add(nameof(IndepPlayTimes), IndepPlayTimes.ToString());
            updata.Add(nameof(IndepTotalBet), IndepTotalBet.ToString());
            updata.Add(nameof(IndepTotalWin), IndepTotalWin.ToString());
            updata.Add(nameof(IndepFreeFree_Times), IndepFreeFree_Times.ToString());
            updata.Add(nameof(IndepTotalRoundTimes), IndepTotalRoundTimes.ToString());
            updata.Add(nameof(IndepTotalWinTimes), IndepTotalWinTimes.ToString());
            updata.Add(nameof(IndepFree1_Times), IndepFree1_Times.ToString());
            updata.Add(nameof(IndepFree1_Bet), IndepFree1_Bet.ToString());
            updata.Add(nameof(IndepFree1_Win), IndepFree1_Win.ToString());
            updata.Add(nameof(IndepFree1Free_Times), IndepFree1Free_Times.ToString());
            updata.Add(nameof(IndepFree1TotalRound_Times), IndepFree1TotalRound_Times.ToString());
            updata.Add(nameof(IndepFree1TotalWin_Times), IndepFree1TotalWin_Times.ToString());
            updata.Add(nameof(IndepFree2_Times), IndepFree2_Times.ToString());
            updata.Add(nameof(IndepFree2_Bet), IndepFree2_Bet.ToString());
            updata.Add(nameof(IndepFree2_Win), IndepFree2_Win.ToString());
            updata.Add(nameof(IndepFree2Free_Times), IndepFree2Free_Times.ToString());
            updata.Add(nameof(IndepFree2TotalRound_Times), IndepFree2TotalRound_Times.ToString());
            updata.Add(nameof(IndepFree2TotalWin_Times), IndepFree2TotalWin_Times.ToString());
            updata.Add(nameof(IndepFree3_Times), IndepFree3_Times.ToString());
            updata.Add(nameof(IndepFree3_Bet), IndepFree3_Bet.ToString());
            updata.Add(nameof(IndepFree3_Win), IndepFree3_Win.ToString());
            updata.Add(nameof(IndepFree3Free_Times), IndepFree3Free_Times.ToString());
            updata.Add(nameof(IndepFree3TotalRound_Times), IndepFree3TotalRound_Times.ToString());
            updata.Add(nameof(IndepFree3TotalWin_Times), IndepFree3TotalWin_Times.ToString());

            updata.Add(nameof(ExPlay_Times), ExPlay_Times.ToString());
            updata.Add(nameof(ExPlay_WinTimes), ExPlay_WinTimes.ToString());
            updata.Add(nameof(ExPlay_Bet), ExPlay_Bet.ToString());
            updata.Add(nameof(ExPlay_Win), ExPlay_Win.ToString());
            updata.Add(nameof(ExPlayS1_Times), ExPlayS1_Times.ToString());
            updata.Add(nameof(ExPlayS1_WinTimes), ExPlayS1_WinTimes.ToString());
            updata.Add(nameof(ExPlayS1_Bet), ExPlayS1_Bet.ToString());
            updata.Add(nameof(ExPlayS1_Win), ExPlayS1_Win.ToString());
            updata.Add(nameof(ExPlayS2_Times), ExPlayS2_Times.ToString());
            updata.Add(nameof(ExPlayS2_WinTimes), ExPlayS2_WinTimes.ToString());
            updata.Add(nameof(ExPlayS2_Bet), ExPlayS2_Bet.ToString());
            updata.Add(nameof(ExPlayS2_Win), ExPlayS2_Win.ToString());
            updata.Add(nameof(ExPlayS3_Times), ExPlayS3_Times.ToString());
            updata.Add(nameof(ExPlayS3_WinTimes), ExPlayS3_WinTimes.ToString());
            updata.Add(nameof(ExPlayS3_Bet), ExPlayS3_Bet.ToString());
            updata.Add(nameof(ExPlayS3_Win), ExPlayS3_Win.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            Free_Times = Convert.ToInt32(datalist[nameof(Free_Times)]);
            Free_Bet = Convert.ToDouble(datalist[nameof(Free_Bet)]);
            Free_Win = Convert.ToDouble(datalist[nameof(Free_Win)]);
            FreeFree_Times = Convert.ToInt32(datalist[nameof(FreeFree_Times)]);
            AwakenedFree_Times = Convert.ToInt32(datalist[nameof(AwakenedFree_Times)]);
            AwakenedFree_Bet = Convert.ToDouble(datalist[nameof(AwakenedFree_Bet)]);
            AwakenedFree_Win = Convert.ToDouble(datalist[nameof(AwakenedFree_Win)]);
            AwakenedFreeFree_Times = Convert.ToInt32(datalist[nameof(AwakenedFreeFree_Times)]);
            RainbowCore1000_Times = Convert.ToInt32(datalist[nameof(RainbowCore1000_Times)]);
            RainbowCore1000_Bet = Convert.ToDouble(datalist[nameof(RainbowCore1000_Bet)]);
            RainbowCore1000_Win = Convert.ToDouble(datalist[nameof(RainbowCore1000_Win)]);
            MultiCore1000_Times = Convert.ToInt32(datalist[nameof(MultiCore1000_Times)]);
            MultiCore1000_Bet = Convert.ToDouble(datalist[nameof(MultiCore1000_Bet)]);
            MultiCore1000_Win = Convert.ToDouble(datalist[nameof(MultiCore1000_Win)]);
            MultiCore100_500_Times = Convert.ToInt32(datalist[nameof(MultiCore100_500_Times)]);
            MultiCore100_500_Bet = Convert.ToDouble(datalist[nameof(MultiCore100_500_Bet)]);
            MultiCore100_500_Win = Convert.ToDouble(datalist[nameof(MultiCore100_500_Win)]);
            MultiCore25_50_Times = Convert.ToInt32(datalist[nameof(MultiCore25_50_Times)]);
            MultiCore25_50_Bet = Convert.ToDouble(datalist[nameof(MultiCore25_50_Bet)]);
            MultiCore25_50_Win = Convert.ToDouble(datalist[nameof(MultiCore25_50_Win)]);
            MultiCore2_20_Times = Convert.ToInt32(datalist[nameof(MultiCore2_20_Times)]);
            MultiCore2_20_Bet = Convert.ToDouble(datalist[nameof(MultiCore2_20_Bet)]);
            MultiCore2_20_Win = Convert.ToDouble(datalist[nameof(MultiCore2_20_Win)]);
            Seth3_Times = Convert.ToInt32(datalist[nameof(Seth3_Times)]);
            Seth3_Bet = Convert.ToDouble(datalist[nameof(Seth3_Bet)]);
            Seth3_Win = Convert.ToDouble(datalist[nameof(Seth3_Win)]);
            Nephthys3_Times = Convert.ToInt32(datalist[nameof(Nephthys3_Times)]);
            Nephthys3_Bet = Convert.ToDouble(datalist[nameof(Nephthys3_Bet)]);
            Nephthys3_Win = Convert.ToDouble(datalist[nameof(Nephthys3_Win)]);
            Totem12_Times = Convert.ToInt32(datalist[nameof(Totem12_Times)]);
            Totem12_Bet = Convert.ToDouble(datalist[nameof(Totem12_Bet)]);
            Totem12_Win = Convert.ToDouble(datalist[nameof(Totem12_Win)]);
            Cross12_Times = Convert.ToInt32(datalist[nameof(Cross12_Times)]);
            Cross12_Bet = Convert.ToDouble(datalist[nameof(Cross12_Bet)]);
            Cross12_Win = Convert.ToDouble(datalist[nameof(Cross12_Win)]);
            Bow12_Times = Convert.ToInt32(datalist[nameof(Bow12_Times)]);
            Bow12_Bet = Convert.ToDouble(datalist[nameof(Bow12_Bet)]);
            Bow12_Win = Convert.ToDouble(datalist[nameof(Bow12_Win)]);
            Dagger12_Times = Convert.ToInt32(datalist[nameof(Dagger12_Times)]);
            Dagger12_Bet = Convert.ToDouble(datalist[nameof(Dagger12_Bet)]);
            Dagger12_Win = Convert.ToDouble(datalist[nameof(Dagger12_Win)]);
            Ruby12_Times = Convert.ToInt32(datalist[nameof(Ruby12_Times)]);
            Ruby12_Bet = Convert.ToDouble(datalist[nameof(Ruby12_Bet)]);
            Ruby12_Win = Convert.ToDouble(datalist[nameof(Ruby12_Win)]);
            Amethyst12_Times = Convert.ToInt32(datalist[nameof(Amethyst12_Times)]);
            Amethyst12_Bet = Convert.ToDouble(datalist[nameof(Amethyst12_Bet)]);
            Amethyst12_Win = Convert.ToDouble(datalist[nameof(Amethyst12_Win)]);
            YellowSapphire12_Times = Convert.ToInt32(datalist[nameof(YellowSapphire12_Times)]);
            YellowSapphire12_Bet = Convert.ToDouble(datalist[nameof(YellowSapphire12_Bet)]);
            YellowSapphire12_Win = Convert.ToDouble(datalist[nameof(YellowSapphire12_Win)]);
            Emerald12_Times = Convert.ToInt32(datalist[nameof(Emerald12_Times)]);
            Emerald12_Bet = Convert.ToDouble(datalist[nameof(Emerald12_Bet)]);
            Emerald12_Win = Convert.ToDouble(datalist[nameof(Emerald12_Win)]);
            Sapphire12_Times = Convert.ToInt32(datalist[nameof(Sapphire12_Times)]);
            Sapphire12_Bet = Convert.ToDouble(datalist[nameof(Sapphire12_Bet)]);
            Sapphire12_Win = Convert.ToDouble(datalist[nameof(Sapphire12_Win)]);

            IndepPlayTimes = Convert.ToInt32(datalist[nameof(IndepPlayTimes)]);
            IndepTotalBet = Convert.ToDouble(datalist[nameof(IndepTotalBet)]);
            IndepTotalWin = Convert.ToDouble(datalist[nameof(IndepTotalWin)]);
            IndepFreeFree_Times = Convert.ToInt32(datalist[nameof(IndepFreeFree_Times)]);
            IndepTotalRoundTimes = Convert.ToInt32(datalist[nameof(IndepTotalRoundTimes)]);
            IndepTotalWinTimes = Convert.ToInt32(datalist[nameof(IndepTotalWinTimes)]);
            IndepFree1_Times = Convert.ToInt32(datalist[nameof(IndepFree1_Times)]);
            IndepFree1_Bet = Convert.ToDouble(datalist[nameof(IndepFree1_Bet)]);
            IndepFree1_Win = Convert.ToDouble(datalist[nameof(IndepFree1_Win)]);
            IndepFree1Free_Times = Convert.ToInt32(datalist[nameof(IndepFree1Free_Times)]);
            IndepFree1TotalRound_Times = Convert.ToInt32(datalist[nameof(IndepFree1TotalRound_Times)]);
            IndepFree1TotalWin_Times = Convert.ToInt32(datalist[nameof(IndepFree1TotalWin_Times)]);
            IndepFree2_Times = Convert.ToInt32(datalist[nameof(IndepFree2_Times)]);
            IndepFree2_Bet = Convert.ToDouble(datalist[nameof(IndepFree2_Bet)]);
            IndepFree2_Win = Convert.ToDouble(datalist[nameof(IndepFree2_Win)]);
            IndepFree2Free_Times = Convert.ToInt32(datalist[nameof(IndepFree2Free_Times)]);
            IndepFree2TotalRound_Times = Convert.ToInt32(datalist[nameof(IndepFree2TotalRound_Times)]);
            IndepFree2TotalWin_Times = Convert.ToInt32(datalist[nameof(IndepFree2TotalWin_Times)]);
            IndepFree3_Times = Convert.ToInt32(datalist[nameof(IndepFree3_Times)]);
            IndepFree3_Bet = Convert.ToDouble(datalist[nameof(IndepFree3_Bet)]);
            IndepFree3_Win = Convert.ToDouble(datalist[nameof(IndepFree3_Win)]);
            IndepFree3Free_Times = Convert.ToInt32(datalist[nameof(IndepFree3Free_Times)]);
            IndepFree3TotalRound_Times = Convert.ToInt32(datalist[nameof(IndepFree3TotalRound_Times)]);
            IndepFree3TotalWin_Times = Convert.ToInt32(datalist[nameof(IndepFree3TotalWin_Times)]);

            ExPlay_Times = Convert.ToInt32(datalist[nameof(ExPlay_Times)]);
            ExPlay_WinTimes = Convert.ToInt32(datalist[nameof(ExPlay_WinTimes)]);
            ExPlay_Bet = Convert.ToDouble(datalist[nameof(ExPlay_Bet)]);
            ExPlay_Win = Convert.ToDouble(datalist[nameof(ExPlay_Win)]);
            ExPlayS1_Times = Convert.ToInt32(datalist[nameof(ExPlayS1_Times)]);
            ExPlayS1_WinTimes = Convert.ToInt32(datalist[nameof(ExPlayS1_WinTimes)]);
            ExPlayS1_Bet = Convert.ToDouble(datalist[nameof(ExPlayS1_Bet)]);
            ExPlayS1_Win = Convert.ToDouble(datalist[nameof(ExPlayS1_Win)]);
            ExPlayS2_Times = Convert.ToInt32(datalist[nameof(ExPlayS2_Times)]);
            ExPlayS2_WinTimes = Convert.ToInt32(datalist[nameof(ExPlayS2_WinTimes)]);
            ExPlayS2_Bet = Convert.ToDouble(datalist[nameof(ExPlayS2_Bet)]);
            ExPlayS2_Win = Convert.ToDouble(datalist[nameof(ExPlayS2_Win)]);
            ExPlayS3_Times = Convert.ToInt32(datalist[nameof(ExPlayS3_Times)]);
            ExPlayS3_WinTimes = Convert.ToInt32(datalist[nameof(ExPlayS3_WinTimes)]);
            ExPlayS3_Bet = Convert.ToDouble(datalist[nameof(ExPlayS3_Bet)]);
            ExPlayS3_Win = Convert.ToDouble(datalist[nameof(ExPlayS3_Win)]);
        }
    }
}
