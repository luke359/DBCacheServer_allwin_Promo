using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class NusantaraAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int FreeFree_Times;
        private int AwakenedFree_Times;
        private double AwakenedFree_Bet;
        private double AwakenedFree_Win;
        /// <summary>覺醒FREE再中FREE次數</summary>
        private int AwakenedFreeFree_Times;
        /// <summary>彩虹倍率徽章</summary>
        private int RainbowCore_Times;
        private double RainbowCore_Bet;
        private double RainbowCore_Win;
        /// <summary>倍率徽章1000</summary>
        private int MultiCore1000_Times;
        private double MultiCore1000_Bet;
        private double MultiCore1000_Win;
        /// <summary>倍率徽章100-500</summary>
        private int MultiCore100_500_Times;
        private double MultiCore100_500_Bet;
        private double MultiCore100_500_Win;
        private int MultiCore50_90_Times;
        private double MultiCore50_90_Bet;
        private double MultiCore50_90_Win;
        private int MultiCore12_40_Times;
        private double MultiCore12_40_Bet;
        private double MultiCore12_40_Win;
        private int MultiCore2_10_Times;
        private double MultiCore2_10_Bet;
        private double MultiCore2_10_Win;
        /// <summary>覺醒加查馬達3相同</summary>
        private int GajahMada3_Times;
        private double GajahMada3_Bet;
        private double GajahMada3_Win;
        /// <summary>覺醒漢麗寶公主3相同</summary>
        private int HangLiPo3_Times;
        private double HangLiPo3_Bet;
        private double HangLiPo3_Win;
        /// <summary>神獸面具12+相同</summary>
        private int MythicalBeastMask12_Times;
        private double MythicalBeastMask12_Bet;
        private double MythicalBeastMask12_Win;
        /// <summary>盾牌12+相同</summary>
        private int Shield12_Times;
        private double Shield12_Bet;
        private double Shield12_Win;
        /// <summary>地圖12+相同</summary>
        private int Map12_Times;
        private double Map12_Bet;
        private double Map12_Win;
        /// <summary>克里斯劍12+相同</summary>
        private int ChrisSword12_Times;
        private double ChrisSword12_Bet;
        private double ChrisSword12_Win;
        /// <summary>黃寶石12+相同</summary>
        private int YellowSapphire12_Times;
        private double YellowSapphire12_Bet;
        private double YellowSapphire12_Win;
        /// <summary>紅寶石12+相同</summary>
        private int Ruby12_Times;
        private double Ruby12_Bet;
        private double Ruby12_Win;
        /// <summary>紫寶石12+相同</summary>
        private int Amethyst12_Times;
        private double Amethyst12_Bet;
        private double Amethyst12_Win;
        /// <summary>藍寶石12+相同</summary>
        private int Sapphire12_Times;
        private double Sapphire12_Bet;
        private double Sapphire12_Win;
        /// <summary>綠寶石12+相同</summary>
        private int Emerald12_Times;
        private double Emerald12_Bet;
        private double Emerald12_Win;

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
                RainbowCore_Times = 0;
                RainbowCore_Bet = 0;
                RainbowCore_Win = 0;
                MultiCore1000_Times = 0;
                MultiCore1000_Bet = 0;
                MultiCore1000_Win = 0;
                MultiCore100_500_Times = 0;
                MultiCore100_500_Bet = 0;
                MultiCore100_500_Win = 0;
                MultiCore50_90_Times = 0;
                MultiCore50_90_Bet = 0;
                MultiCore50_90_Win = 0;
                MultiCore12_40_Times = 0;
                MultiCore12_40_Bet = 0;
                MultiCore12_40_Win = 0;
                MultiCore2_10_Times = 0;
                MultiCore2_10_Bet = 0;
                MultiCore2_10_Win = 0;
                GajahMada3_Times = 0;
                GajahMada3_Bet = 0;
                GajahMada3_Win = 0;
                HangLiPo3_Times = 0;
                HangLiPo3_Bet = 0;
                HangLiPo3_Win = 0;
                MythicalBeastMask12_Times = 0;
                MythicalBeastMask12_Bet = 0;
                MythicalBeastMask12_Win = 0;
                Shield12_Times = 0;
                Shield12_Bet = 0;
                Shield12_Win = 0;
                Map12_Times = 0;
                Map12_Bet = 0;
                Map12_Win = 0;
                ChrisSword12_Times = 0;
                ChrisSword12_Bet = 0;
                ChrisSword12_Win = 0;
                YellowSapphire12_Times = 0;
                YellowSapphire12_Bet = 0;
                YellowSapphire12_Win = 0;
                Ruby12_Times = 0;
                Ruby12_Bet = 0;
                Ruby12_Win = 0;
                Amethyst12_Times = 0;
                Amethyst12_Bet = 0;
                Amethyst12_Win = 0;
                Sapphire12_Times = 0;
                Sapphire12_Bet = 0;
                Sapphire12_Win = 0;
                Emerald12_Times = 0;
                Emerald12_Bet = 0;
                Emerald12_Win = 0;
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
            if (field == nameof(RainbowCore_Times)) { RainbowCore_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RainbowCore_Bet)) { RainbowCore_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(RainbowCore_Win)) { RainbowCore_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore1000_Times)) { MultiCore1000_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore1000_Bet)) { MultiCore1000_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore1000_Win)) { MultiCore1000_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore100_500_Times)) { MultiCore100_500_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore100_500_Bet)) { MultiCore100_500_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore100_500_Win)) { MultiCore100_500_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore50_90_Times)) { MultiCore50_90_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore50_90_Bet)) { MultiCore50_90_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore50_90_Win)) { MultiCore50_90_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore12_40_Times)) { MultiCore12_40_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore12_40_Bet)) { MultiCore12_40_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore12_40_Win)) { MultiCore12_40_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore2_10_Times)) { MultiCore2_10_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MultiCore2_10_Bet)) { MultiCore2_10_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MultiCore2_10_Win)) { MultiCore2_10_Win += Convert.ToDouble(value); return; }
            if (field == nameof(GajahMada3_Times)) { GajahMada3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GajahMada3_Bet)) { GajahMada3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GajahMada3_Win)) { GajahMada3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(HangLiPo3_Times)) { HangLiPo3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(HangLiPo3_Bet)) { HangLiPo3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(HangLiPo3_Win)) { HangLiPo3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MythicalBeastMask12_Times)) { MythicalBeastMask12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MythicalBeastMask12_Bet)) { MythicalBeastMask12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MythicalBeastMask12_Win)) { MythicalBeastMask12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Shield12_Times)) { Shield12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Shield12_Bet)) { Shield12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Shield12_Win)) { Shield12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Map12_Times)) { Map12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Map12_Bet)) { Map12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Map12_Win)) { Map12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ChrisSword12_Times)) { ChrisSword12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ChrisSword12_Bet)) { ChrisSword12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ChrisSword12_Win)) { ChrisSword12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(YellowSapphire12_Times)) { YellowSapphire12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(YellowSapphire12_Bet)) { YellowSapphire12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(YellowSapphire12_Win)) { YellowSapphire12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby12_Times)) { Ruby12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ruby12_Bet)) { Ruby12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby12_Win)) { Ruby12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst12_Times)) { Amethyst12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Amethyst12_Bet)) { Amethyst12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst12_Win)) { Amethyst12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire12_Times)) { Sapphire12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Sapphire12_Bet)) { Sapphire12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire12_Win)) { Sapphire12_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald12_Times)) { Emerald12_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Emerald12_Bet)) { Emerald12_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald12_Win)) { Emerald12_Win += Convert.ToDouble(value); return; }

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
            updata.Add(nameof(RainbowCore_Times), RainbowCore_Times.ToString());
            updata.Add(nameof(RainbowCore_Bet), RainbowCore_Bet.ToString());
            updata.Add(nameof(RainbowCore_Win), RainbowCore_Win.ToString());
            updata.Add(nameof(MultiCore1000_Times), MultiCore1000_Times.ToString());
            updata.Add(nameof(MultiCore1000_Bet), MultiCore1000_Bet.ToString());
            updata.Add(nameof(MultiCore1000_Win), MultiCore1000_Win.ToString());
            updata.Add(nameof(MultiCore100_500_Times), MultiCore100_500_Times.ToString());
            updata.Add(nameof(MultiCore100_500_Bet), MultiCore100_500_Bet.ToString());
            updata.Add(nameof(MultiCore100_500_Win), MultiCore100_500_Win.ToString());
            updata.Add(nameof(MultiCore50_90_Times), MultiCore50_90_Times.ToString());
            updata.Add(nameof(MultiCore50_90_Bet), MultiCore50_90_Bet.ToString());
            updata.Add(nameof(MultiCore50_90_Win), MultiCore50_90_Win.ToString());
            updata.Add(nameof(MultiCore12_40_Times), MultiCore12_40_Times.ToString());
            updata.Add(nameof(MultiCore12_40_Bet), MultiCore12_40_Bet.ToString());
            updata.Add(nameof(MultiCore12_40_Win), MultiCore12_40_Win.ToString());
            updata.Add(nameof(MultiCore2_10_Times), MultiCore2_10_Times.ToString());
            updata.Add(nameof(MultiCore2_10_Bet), MultiCore2_10_Bet.ToString());
            updata.Add(nameof(MultiCore2_10_Win), MultiCore2_10_Win.ToString());
            updata.Add(nameof(GajahMada3_Times), GajahMada3_Times.ToString());
            updata.Add(nameof(GajahMada3_Bet), GajahMada3_Bet.ToString());
            updata.Add(nameof(GajahMada3_Win), GajahMada3_Win.ToString());
            updata.Add(nameof(HangLiPo3_Times), HangLiPo3_Times.ToString());
            updata.Add(nameof(HangLiPo3_Bet), HangLiPo3_Bet.ToString());
            updata.Add(nameof(HangLiPo3_Win), HangLiPo3_Win.ToString());
            updata.Add(nameof(MythicalBeastMask12_Times), MythicalBeastMask12_Times.ToString());
            updata.Add(nameof(MythicalBeastMask12_Bet), MythicalBeastMask12_Bet.ToString());
            updata.Add(nameof(MythicalBeastMask12_Win), MythicalBeastMask12_Win.ToString());
            updata.Add(nameof(Shield12_Times), Shield12_Times.ToString());
            updata.Add(nameof(Shield12_Bet), Shield12_Bet.ToString());
            updata.Add(nameof(Shield12_Win), Shield12_Win.ToString());
            updata.Add(nameof(Map12_Times), Map12_Times.ToString());
            updata.Add(nameof(Map12_Bet), Map12_Bet.ToString());
            updata.Add(nameof(Map12_Win), Map12_Win.ToString());
            updata.Add(nameof(ChrisSword12_Times), ChrisSword12_Times.ToString());
            updata.Add(nameof(ChrisSword12_Bet), ChrisSword12_Bet.ToString());
            updata.Add(nameof(ChrisSword12_Win), ChrisSword12_Win.ToString());
            updata.Add(nameof(YellowSapphire12_Times), YellowSapphire12_Times.ToString());
            updata.Add(nameof(YellowSapphire12_Bet), YellowSapphire12_Bet.ToString());
            updata.Add(nameof(YellowSapphire12_Win), YellowSapphire12_Win.ToString());
            updata.Add(nameof(Ruby12_Times), Ruby12_Times.ToString());
            updata.Add(nameof(Ruby12_Bet), Ruby12_Bet.ToString());
            updata.Add(nameof(Ruby12_Win), Ruby12_Win.ToString());
            updata.Add(nameof(Amethyst12_Times), Amethyst12_Times.ToString());
            updata.Add(nameof(Amethyst12_Bet), Amethyst12_Bet.ToString());
            updata.Add(nameof(Amethyst12_Win), Amethyst12_Win.ToString());
            updata.Add(nameof(Sapphire12_Times), Sapphire12_Times.ToString());
            updata.Add(nameof(Sapphire12_Bet), Sapphire12_Bet.ToString());
            updata.Add(nameof(Sapphire12_Win), Sapphire12_Win.ToString());
            updata.Add(nameof(Emerald12_Times), Emerald12_Times.ToString());
            updata.Add(nameof(Emerald12_Bet), Emerald12_Bet.ToString());
            updata.Add(nameof(Emerald12_Win), Emerald12_Win.ToString());

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
            RainbowCore_Times = Convert.ToInt32(datalist[nameof(RainbowCore_Times)]);
            RainbowCore_Bet = Convert.ToDouble(datalist[nameof(RainbowCore_Bet)]);
            RainbowCore_Win = Convert.ToDouble(datalist[nameof(RainbowCore_Win)]);
            MultiCore1000_Times = Convert.ToInt32(datalist[nameof(MultiCore1000_Times)]);
            MultiCore1000_Bet = Convert.ToDouble(datalist[nameof(MultiCore1000_Bet)]);
            MultiCore1000_Win = Convert.ToDouble(datalist[nameof(MultiCore1000_Win)]);
            MultiCore100_500_Times = Convert.ToInt32(datalist[nameof(MultiCore100_500_Times)]);
            MultiCore100_500_Bet = Convert.ToDouble(datalist[nameof(MultiCore100_500_Bet)]);
            MultiCore100_500_Win = Convert.ToDouble(datalist[nameof(MultiCore100_500_Win)]);
            MultiCore50_90_Times = Convert.ToInt32(datalist[nameof(MultiCore50_90_Times)]);
            MultiCore50_90_Bet = Convert.ToDouble(datalist[nameof(MultiCore50_90_Bet)]);
            MultiCore50_90_Win = Convert.ToDouble(datalist[nameof(MultiCore50_90_Win)]);
            MultiCore12_40_Times = Convert.ToInt32(datalist[nameof(MultiCore12_40_Times)]);
            MultiCore12_40_Bet = Convert.ToDouble(datalist[nameof(MultiCore12_40_Bet)]);
            MultiCore12_40_Win = Convert.ToDouble(datalist[nameof(MultiCore12_40_Win)]);
            MultiCore2_10_Times = Convert.ToInt32(datalist[nameof(MultiCore2_10_Times)]);
            MultiCore2_10_Bet = Convert.ToDouble(datalist[nameof(MultiCore2_10_Bet)]);
            MultiCore2_10_Win = Convert.ToDouble(datalist[nameof(MultiCore2_10_Win)]);
            GajahMada3_Times = Convert.ToInt32(datalist[nameof(GajahMada3_Times)]);
            GajahMada3_Bet = Convert.ToDouble(datalist[nameof(GajahMada3_Bet)]);
            GajahMada3_Win = Convert.ToDouble(datalist[nameof(GajahMada3_Win)]);
            HangLiPo3_Times = Convert.ToInt32(datalist[nameof(HangLiPo3_Times)]);
            HangLiPo3_Bet = Convert.ToDouble(datalist[nameof(HangLiPo3_Bet)]);
            HangLiPo3_Win = Convert.ToDouble(datalist[nameof(HangLiPo3_Win)]);
            MythicalBeastMask12_Times = Convert.ToInt32(datalist[nameof(MythicalBeastMask12_Times)]);
            MythicalBeastMask12_Bet = Convert.ToDouble(datalist[nameof(MythicalBeastMask12_Bet)]);
            MythicalBeastMask12_Win = Convert.ToDouble(datalist[nameof(MythicalBeastMask12_Win)]);
            Shield12_Times = Convert.ToInt32(datalist[nameof(Shield12_Times)]);
            Shield12_Bet = Convert.ToDouble(datalist[nameof(Shield12_Bet)]);
            Shield12_Win = Convert.ToDouble(datalist[nameof(Shield12_Win)]);
            Map12_Times = Convert.ToInt32(datalist[nameof(Map12_Times)]);
            Map12_Bet = Convert.ToDouble(datalist[nameof(Map12_Bet)]);
            Map12_Win = Convert.ToDouble(datalist[nameof(Map12_Win)]);
            ChrisSword12_Times = Convert.ToInt32(datalist[nameof(ChrisSword12_Times)]);
            ChrisSword12_Bet = Convert.ToDouble(datalist[nameof(ChrisSword12_Bet)]);
            ChrisSword12_Win = Convert.ToDouble(datalist[nameof(ChrisSword12_Win)]);
            YellowSapphire12_Times = Convert.ToInt32(datalist[nameof(YellowSapphire12_Times)]);
            YellowSapphire12_Bet = Convert.ToDouble(datalist[nameof(YellowSapphire12_Bet)]);
            YellowSapphire12_Win = Convert.ToDouble(datalist[nameof(YellowSapphire12_Win)]);
            Ruby12_Times = Convert.ToInt32(datalist[nameof(Ruby12_Times)]);
            Ruby12_Bet = Convert.ToDouble(datalist[nameof(Ruby12_Bet)]);
            Ruby12_Win = Convert.ToDouble(datalist[nameof(Ruby12_Win)]);
            Amethyst12_Times = Convert.ToInt32(datalist[nameof(Amethyst12_Times)]);
            Amethyst12_Bet = Convert.ToDouble(datalist[nameof(Amethyst12_Bet)]);
            Amethyst12_Win = Convert.ToDouble(datalist[nameof(Amethyst12_Win)]);
            Sapphire12_Times = Convert.ToInt32(datalist[nameof(Sapphire12_Times)]);
            Sapphire12_Bet = Convert.ToDouble(datalist[nameof(Sapphire12_Bet)]);
            Sapphire12_Win = Convert.ToDouble(datalist[nameof(Sapphire12_Win)]);
            Emerald12_Times = Convert.ToInt32(datalist[nameof(Emerald12_Times)]);
            Emerald12_Bet = Convert.ToDouble(datalist[nameof(Emerald12_Bet)]);
            Emerald12_Win = Convert.ToDouble(datalist[nameof(Emerald12_Win)]);

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
