using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class MythofNezhaAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int FreeFree_Times;
        private int FreeGrand_Times;
        private double FreeGrand_Bet;
        private double FreeGrand_Win;
        private int FreeMajor_Times;
        private double FreeMajor_Bet;
        private double FreeMajor_Win;
        private int FreeMinor_Times;
        private double FreeMinor_Bet;
        private double FreeMinor_Win;
        private int FreeMini_Times;
        private double FreeMini_Bet;
        private double FreeMini_Win;
        private int Wild5_Times;
        private double Wild5_Bet;
        private double Wild5_Win;
        private int ErlangShen5_Times;
        private double ErlangShen5_Bet;
        private double ErlangShen5_Win;
        private int Nezha5_Times;
        private double Nezha5_Bet;
        private double Nezha5_Win;
        private int Reel5_Times;
        private double Reel5_Bet;
        private double Reel5_Win;
        private int Gourd5_Times;
        private double Gourd5_Bet;
        private double Gourd5_Win;
        private int A5_Times;
        private double A5_Bet;
        private double A5_Win;
        private int K5_Times;
        private double K5_Bet;
        private double K5_Win;
        private int Q5_Times;
        private double Q5_Bet;
        private double Q5_Win;
        private int J5_Times;
        private double J5_Bet;
        private double J5_Win;
        private int Ten5_Times;
        private double Ten5_Bet;
        private double Ten5_Win;

        private int IndepPlayTimes;
        private double IndepTotalBet;
        private double IndepTotalWin;
        private int IndepFreeFree_Times;
        private int IndepTotalRoundTimes;
        private int IndepTotalWinTimes;
        private int IndepFreeGrand_Times;
        private double IndepFreeGrand_Bet;
        private double IndepFreeGrand_Win;
        private int IndepFreeMajor_Times;
        private double IndepFreeMajor_Bet;
        private double IndepFreeMajor_Win;
        private int IndepFreeMinor_Times;
        private double IndepFreeMinor_Bet;
        private double IndepFreeMinor_Win;
        private int IndepFreeMini_Times;
        private double IndepFreeMini_Bet;
        private double IndepFreeMini_Win;
        private int IndepS1PlayTimes;
        private double IndepS1TotalBet;
        private double IndepS1TotalWin;
        private int IndepS1FreeFree_Times;
        private int IndepS1TotalRoundTimes;
        private int IndepS1TotalWinTimes;
        private int IndepS1FreeGrand_Times;
        private double IndepS1FreeGrand_Bet;
        private double IndepS1FreeGrand_Win;
        private int IndepS1FreeMajor_Times;
        private double IndepS1FreeMajor_Bet;
        private double IndepS1FreeMajor_Win;
        private int IndepS1FreeMinor_Times;
        private double IndepS1FreeMinor_Bet;
        private double IndepS1FreeMinor_Win;
        private int IndepS1FreeMini_Times;
        private double IndepS1FreeMini_Bet;
        private double IndepS1FreeMini_Win;
        private int IndepS2PlayTimes;
        private double IndepS2TotalBet;
        private double IndepS2TotalWin;
        private int IndepS2FreeFree_Times;
        private int IndepS2TotalRoundTimes;
        private int IndepS2TotalWinTimes;
        private int IndepS2FreeGrand_Times;
        private double IndepS2FreeGrand_Bet;
        private double IndepS2FreeGrand_Win;
        private int IndepS2FreeMajor_Times;
        private double IndepS2FreeMajor_Bet;
        private double IndepS2FreeMajor_Win;
        private int IndepS2FreeMinor_Times;
        private double IndepS2FreeMinor_Bet;
        private double IndepS2FreeMinor_Win;
        private int IndepS2FreeMini_Times;
        private double IndepS2FreeMini_Bet;
        private double IndepS2FreeMini_Win;

        private int ExPlay_Times;
        private double ExPlay_Bet;
        private double ExPlay_Win;
        private int ExPlay_WinTimes;
        private int ExPlayS1_Times;
        private double ExPlayS1_Bet;
        private double ExPlayS1_Win;
        private int ExPlayS1_WinTimes;
        private int ExPlayS2_Times;
        private double ExPlayS2_Bet;
        private double ExPlayS2_Win;
        private int ExPlayS2_WinTimes;
        private int ExPlayS3_Times;
        private double ExPlayS3_Bet;
        private double ExPlayS3_Win;
        private int ExPlayS3_WinTimes;
        #endregion


        /// <summary>清除額外押注內存</summary>
        public override void ClearCacheGameExPlay()
        {
            ExPlay_Times = 0;
            ExPlay_Bet = 0;
            ExPlay_Win = 0;
            ExPlay_WinTimes = 0;
            ExPlayS1_Times = 0;
            ExPlayS1_Bet = 0;
            ExPlayS1_Win = 0;
            ExPlayS1_WinTimes = 0;
            ExPlayS2_Times = 0;
            ExPlayS2_Bet = 0;
            ExPlayS2_Win = 0;
            ExPlayS2_WinTimes = 0;
            ExPlayS3_Times = 0;
            ExPlayS3_Bet = 0;
            ExPlayS3_Win = 0;
            ExPlayS3_WinTimes = 0;
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
                FreeGrand_Times = 0;
                FreeGrand_Bet = 0;
                FreeGrand_Win = 0;
                FreeMajor_Times = 0;
                FreeMajor_Bet = 0;
                FreeMajor_Win = 0;
                FreeMinor_Times = 0;
                FreeMinor_Bet = 0;
                FreeMinor_Win = 0;
                FreeMini_Times = 0;
                FreeMini_Bet = 0;
                FreeMini_Win = 0;
                Wild5_Times = 0;
                Wild5_Bet = 0;
                Wild5_Win = 0;
                ErlangShen5_Times = 0;
                ErlangShen5_Bet = 0;
                ErlangShen5_Win = 0;
                Nezha5_Times = 0;
                Nezha5_Bet = 0;
                Nezha5_Win = 0;
                Reel5_Times = 0;
                Reel5_Bet = 0;
                Reel5_Win = 0;
                Gourd5_Times = 0;
                Gourd5_Bet = 0;
                Gourd5_Win = 0;
                A5_Times = 0;
                A5_Bet = 0;
                A5_Win = 0;
                K5_Times = 0;
                K5_Bet = 0;
                K5_Win = 0;
                Q5_Times = 0;
                Q5_Bet = 0;
                Q5_Win = 0;
                J5_Times = 0;
                J5_Bet = 0;
                J5_Win = 0;
                Ten5_Times = 0;
                Ten5_Bet = 0;
                Ten5_Win = 0;
            }
            if (mode == 1 || mode == 2)
            {
                IndepPlayTimes = 0;
                IndepTotalBet = 0;
                IndepTotalWin = 0;
                IndepFreeFree_Times = 0;
                IndepTotalRoundTimes = 0;
                IndepTotalWinTimes = 0;
                IndepFreeGrand_Times = 0;
                IndepFreeGrand_Bet = 0;
                IndepFreeGrand_Win = 0;
                IndepFreeMajor_Times = 0;
                IndepFreeMajor_Bet = 0;
                IndepFreeMajor_Win = 0;
                IndepFreeMinor_Times = 0;
                IndepFreeMinor_Bet = 0;
                IndepFreeMinor_Win = 0;
                IndepFreeMini_Times = 0;
                IndepFreeMini_Bet = 0;
                IndepFreeMini_Win = 0;
                IndepS1PlayTimes = 0;
                IndepS1TotalBet = 0;
                IndepS1TotalWin = 0;
                IndepS1FreeFree_Times = 0;
                IndepS1TotalRoundTimes = 0;
                IndepS1TotalWinTimes = 0;
                IndepS1FreeGrand_Times = 0;
                IndepS1FreeGrand_Bet = 0;
                IndepS1FreeGrand_Win = 0;
                IndepS1FreeMajor_Times = 0;
                IndepS1FreeMajor_Bet = 0;
                IndepS1FreeMajor_Win = 0;
                IndepS1FreeMinor_Times = 0;
                IndepS1FreeMinor_Bet = 0;
                IndepS1FreeMinor_Win = 0;
                IndepS1FreeMini_Times = 0;
                IndepS1FreeMini_Bet = 0;
                IndepS1FreeMini_Win = 0;
                IndepS2PlayTimes = 0;
                IndepS2TotalBet = 0;
                IndepS2TotalWin = 0;
                IndepS2FreeFree_Times = 0;
                IndepS2TotalRoundTimes = 0;
                IndepS2TotalWinTimes = 0;
                IndepS2FreeGrand_Times = 0;
                IndepS2FreeGrand_Bet = 0;
                IndepS2FreeGrand_Win = 0;
                IndepS2FreeMajor_Times = 0;
                IndepS2FreeMajor_Bet = 0;
                IndepS2FreeMajor_Win = 0;
                IndepS2FreeMinor_Times = 0;
                IndepS2FreeMinor_Bet = 0;
                IndepS2FreeMinor_Win = 0;
                IndepS2FreeMini_Times = 0;
                IndepS2FreeMini_Bet = 0;
                IndepS2FreeMini_Win = 0;
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(Free_Times)) { Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Free_Bet)) { Free_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Free_Win)) { Free_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeFree_Times)) { FreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeGrand_Times)) { FreeGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeGrand_Bet)) { FreeGrand_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FreeGrand_Win)) { FreeGrand_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMajor_Times)) { FreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMajor_Bet)) { FreeMajor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMajor_Win)) { FreeMajor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMinor_Times)) { FreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMinor_Bet)) { FreeMinor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMinor_Win)) { FreeMinor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMini_Times)) { FreeMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMini_Bet)) { FreeMini_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMini_Win)) { FreeMini_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Wild5_Times)) { Wild5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Wild5_Bet)) { Wild5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Wild5_Win)) { Wild5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ErlangShen5_Times)) { ErlangShen5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ErlangShen5_Bet)) { ErlangShen5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ErlangShen5_Win)) { ErlangShen5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Nezha5_Times)) { Nezha5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Nezha5_Bet)) { Nezha5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Nezha5_Win)) { Nezha5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Reel5_Times)) { Reel5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Reel5_Bet)) { Reel5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Reel5_Win)) { Reel5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd5_Times)) { Gourd5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gourd5_Bet)) { Gourd5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd5_Win)) { Gourd5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A5_Times)) { A5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A5_Bet)) { A5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A5_Win)) { A5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K5_Times)) { K5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K5_Bet)) { K5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K5_Win)) { K5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q5_Times)) { Q5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q5_Bet)) { Q5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q5_Win)) { Q5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J5_Times)) { J5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J5_Bet)) { J5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J5_Win)) { J5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ten5_Times)) { Ten5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ten5_Bet)) { Ten5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ten5_Win)) { Ten5_Win += Convert.ToDouble(value); return; }

            if (field == nameof(IndepPlayTimes)) { IndepPlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalBet)) { IndepTotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepTotalWin)) { IndepTotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeFree_Times)) { IndepFreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalRoundTimes)) { IndepTotalRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalWinTimes)) { IndepTotalWinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeGrand_Times)) { IndepFreeGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeGrand_Bet)) { IndepFreeGrand_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeGrand_Win)) { IndepFreeGrand_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMajor_Times)) { IndepFreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMajor_Bet)) { IndepFreeMajor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMajor_Win)) { IndepFreeMajor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMinor_Times)) { IndepFreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMinor_Bet)) { IndepFreeMinor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMinor_Win)) { IndepFreeMinor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMini_Times)) { IndepFreeMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMini_Bet)) { IndepFreeMini_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMini_Win)) { IndepFreeMini_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1PlayTimes)) { IndepS1PlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1TotalBet)) { IndepS1TotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1TotalWin)) { IndepS1TotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeFree_Times)) { IndepS1FreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1TotalRoundTimes)) { IndepS1TotalRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1TotalWinTimes)) { IndepS1TotalWinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1FreeGrand_Times)) { IndepS1FreeGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1FreeGrand_Bet)) { IndepS1FreeGrand_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeGrand_Win)) { IndepS1FreeGrand_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeMajor_Times)) { IndepS1FreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1FreeMajor_Bet)) { IndepS1FreeMajor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeMajor_Win)) { IndepS1FreeMajor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeMinor_Times)) { IndepS1FreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1FreeMinor_Bet)) { IndepS1FreeMinor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeMinor_Win)) { IndepS1FreeMinor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeMini_Times)) { IndepS1FreeMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1FreeMini_Bet)) { IndepS1FreeMini_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1FreeMini_Win)) { IndepS1FreeMini_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2PlayTimes)) { IndepS2PlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2TotalBet)) { IndepS2TotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2TotalWin)) { IndepS2TotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeFree_Times)) { IndepS2FreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2TotalRoundTimes)) { IndepS2TotalRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2TotalWinTimes)) { IndepS2TotalWinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2FreeGrand_Times)) { IndepS2FreeGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2FreeGrand_Bet)) { IndepS2FreeGrand_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeGrand_Win)) { IndepS2FreeGrand_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeMajor_Times)) { IndepS2FreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2FreeMajor_Bet)) { IndepS2FreeMajor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeMajor_Win)) { IndepS2FreeMajor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeMinor_Times)) { IndepS2FreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2FreeMinor_Bet)) { IndepS2FreeMinor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeMinor_Win)) { IndepS2FreeMinor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeMini_Times)) { IndepS2FreeMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2FreeMini_Bet)) { IndepS2FreeMini_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2FreeMini_Win)) { IndepS2FreeMini_Win += Convert.ToDouble(value); return; }

            if (field == nameof(ExPlay_Times)) { ExPlay_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlay_Bet)) { ExPlay_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay_Win)) { ExPlay_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay_WinTimes)) { ExPlay_WinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS1_Times)) { ExPlayS1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS1_Bet)) { ExPlayS1_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS1_Win)) { ExPlayS1_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS1_WinTimes)) { ExPlayS1_WinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS2_Times)) { ExPlayS2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS2_Bet)) { ExPlayS2_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS2_Win)) { ExPlayS2_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS2_WinTimes)) { ExPlayS2_WinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS3_Times)) { ExPlayS3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlayS3_Bet)) { ExPlayS3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS3_Win)) { ExPlayS3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlayS3_WinTimes)) { ExPlayS3_WinTimes += Convert.ToInt32(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(Free_Times), Free_Times.ToString());
            updata.Add(nameof(Free_Bet), Free_Bet.ToString());
            updata.Add(nameof(Free_Win), Free_Win.ToString());
            updata.Add(nameof(FreeFree_Times), FreeFree_Times.ToString());
            updata.Add(nameof(FreeGrand_Times), FreeGrand_Times.ToString());
            updata.Add(nameof(FreeGrand_Bet), FreeGrand_Bet.ToString());
            updata.Add(nameof(FreeGrand_Win), FreeGrand_Win.ToString());
            updata.Add(nameof(FreeMajor_Times), FreeMajor_Times.ToString());
            updata.Add(nameof(FreeMajor_Bet), FreeMajor_Bet.ToString());
            updata.Add(nameof(FreeMajor_Win), FreeMajor_Win.ToString());
            updata.Add(nameof(FreeMinor_Times), FreeMinor_Times.ToString());
            updata.Add(nameof(FreeMinor_Bet), FreeMinor_Bet.ToString());
            updata.Add(nameof(FreeMinor_Win), FreeMinor_Win.ToString());
            updata.Add(nameof(FreeMini_Times), FreeMini_Times.ToString());
            updata.Add(nameof(FreeMini_Bet), FreeMini_Bet.ToString());
            updata.Add(nameof(FreeMini_Win), FreeMini_Win.ToString());
            updata.Add(nameof(Wild5_Times), Wild5_Times.ToString());
            updata.Add(nameof(Wild5_Bet), Wild5_Bet.ToString());
            updata.Add(nameof(Wild5_Win), Wild5_Win.ToString());
            updata.Add(nameof(ErlangShen5_Times), ErlangShen5_Times.ToString());
            updata.Add(nameof(ErlangShen5_Bet), ErlangShen5_Bet.ToString());
            updata.Add(nameof(ErlangShen5_Win), ErlangShen5_Win.ToString());
            updata.Add(nameof(Nezha5_Times), Nezha5_Times.ToString());
            updata.Add(nameof(Nezha5_Bet), Nezha5_Bet.ToString());
            updata.Add(nameof(Nezha5_Win), Nezha5_Win.ToString());
            updata.Add(nameof(Reel5_Times), Reel5_Times.ToString());
            updata.Add(nameof(Reel5_Bet), Reel5_Bet.ToString());
            updata.Add(nameof(Reel5_Win), Reel5_Win.ToString());
            updata.Add(nameof(Gourd5_Times), Gourd5_Times.ToString());
            updata.Add(nameof(Gourd5_Bet), Gourd5_Bet.ToString());
            updata.Add(nameof(Gourd5_Win), Gourd5_Win.ToString());
            updata.Add(nameof(A5_Times), A5_Times.ToString());
            updata.Add(nameof(A5_Bet), A5_Bet.ToString());
            updata.Add(nameof(A5_Win), A5_Win.ToString());
            updata.Add(nameof(K5_Times), K5_Times.ToString());
            updata.Add(nameof(K5_Bet), K5_Bet.ToString());
            updata.Add(nameof(K5_Win), K5_Win.ToString());
            updata.Add(nameof(Q5_Times), Q5_Times.ToString());
            updata.Add(nameof(Q5_Bet), Q5_Bet.ToString());
            updata.Add(nameof(Q5_Win), Q5_Win.ToString());
            updata.Add(nameof(J5_Times), J5_Times.ToString());
            updata.Add(nameof(J5_Bet), J5_Bet.ToString());
            updata.Add(nameof(J5_Win), J5_Win.ToString());
            updata.Add(nameof(Ten5_Times), Ten5_Times.ToString());
            updata.Add(nameof(Ten5_Bet), Ten5_Bet.ToString());
            updata.Add(nameof(Ten5_Win), Ten5_Win.ToString());

            updata.Add(nameof(IndepPlayTimes), IndepPlayTimes.ToString());
            updata.Add(nameof(IndepTotalBet), IndepTotalBet.ToString());
            updata.Add(nameof(IndepTotalWin), IndepTotalWin.ToString());
            updata.Add(nameof(IndepFreeFree_Times), IndepFreeFree_Times.ToString());
            updata.Add(nameof(IndepTotalRoundTimes), IndepTotalRoundTimes.ToString());
            updata.Add(nameof(IndepTotalWinTimes), IndepTotalWinTimes.ToString());
            updata.Add(nameof(IndepFreeGrand_Times), IndepFreeGrand_Times.ToString());
            updata.Add(nameof(IndepFreeGrand_Bet), IndepFreeGrand_Bet.ToString());
            updata.Add(nameof(IndepFreeGrand_Win), IndepFreeGrand_Win.ToString());
            updata.Add(nameof(IndepFreeMajor_Times), IndepFreeMajor_Times.ToString());
            updata.Add(nameof(IndepFreeMajor_Bet), IndepFreeMajor_Bet.ToString());
            updata.Add(nameof(IndepFreeMajor_Win), IndepFreeMajor_Win.ToString());
            updata.Add(nameof(IndepFreeMinor_Times), IndepFreeMinor_Times.ToString());
            updata.Add(nameof(IndepFreeMinor_Bet), IndepFreeMinor_Bet.ToString());
            updata.Add(nameof(IndepFreeMinor_Win), IndepFreeMinor_Win.ToString());
            updata.Add(nameof(IndepFreeMini_Times), IndepFreeMini_Times.ToString());
            updata.Add(nameof(IndepFreeMini_Bet), IndepFreeMini_Bet.ToString());
            updata.Add(nameof(IndepFreeMini_Win), IndepFreeMini_Win.ToString());
            updata.Add(nameof(IndepS1PlayTimes), IndepS1PlayTimes.ToString());
            updata.Add(nameof(IndepS1TotalBet), IndepS1TotalBet.ToString());
            updata.Add(nameof(IndepS1TotalWin), IndepS1TotalWin.ToString());
            updata.Add(nameof(IndepS1FreeFree_Times), IndepS1FreeFree_Times.ToString());
            updata.Add(nameof(IndepS1TotalRoundTimes), IndepS1TotalRoundTimes.ToString());
            updata.Add(nameof(IndepS1TotalWinTimes), IndepS1TotalWinTimes.ToString());
            updata.Add(nameof(IndepS1FreeGrand_Times), IndepS1FreeGrand_Times.ToString());
            updata.Add(nameof(IndepS1FreeGrand_Bet), IndepS1FreeGrand_Bet.ToString());
            updata.Add(nameof(IndepS1FreeGrand_Win), IndepS1FreeGrand_Win.ToString());
            updata.Add(nameof(IndepS1FreeMajor_Times), IndepS1FreeMajor_Times.ToString());
            updata.Add(nameof(IndepS1FreeMajor_Bet), IndepS1FreeMajor_Bet.ToString());
            updata.Add(nameof(IndepS1FreeMajor_Win), IndepS1FreeMajor_Win.ToString());
            updata.Add(nameof(IndepS1FreeMinor_Times), IndepS1FreeMinor_Times.ToString());
            updata.Add(nameof(IndepS1FreeMinor_Bet), IndepS1FreeMinor_Bet.ToString());
            updata.Add(nameof(IndepS1FreeMinor_Win), IndepS1FreeMinor_Win.ToString());
            updata.Add(nameof(IndepS1FreeMini_Times), IndepS1FreeMini_Times.ToString());
            updata.Add(nameof(IndepS1FreeMini_Bet), IndepS1FreeMini_Bet.ToString());
            updata.Add(nameof(IndepS1FreeMini_Win), IndepS1FreeMini_Win.ToString());
            updata.Add(nameof(IndepS2PlayTimes), IndepS2PlayTimes.ToString());
            updata.Add(nameof(IndepS2TotalBet), IndepS2TotalBet.ToString());
            updata.Add(nameof(IndepS2TotalWin), IndepS2TotalWin.ToString());
            updata.Add(nameof(IndepS2FreeFree_Times), IndepS2FreeFree_Times.ToString());
            updata.Add(nameof(IndepS2TotalRoundTimes), IndepS2TotalRoundTimes.ToString());
            updata.Add(nameof(IndepS2TotalWinTimes), IndepS2TotalWinTimes.ToString());
            updata.Add(nameof(IndepS2FreeGrand_Times), IndepS2FreeGrand_Times.ToString());
            updata.Add(nameof(IndepS2FreeGrand_Bet), IndepS2FreeGrand_Bet.ToString());
            updata.Add(nameof(IndepS2FreeGrand_Win), IndepS2FreeGrand_Win.ToString());
            updata.Add(nameof(IndepS2FreeMajor_Times), IndepS2FreeMajor_Times.ToString());
            updata.Add(nameof(IndepS2FreeMajor_Bet), IndepS2FreeMajor_Bet.ToString());
            updata.Add(nameof(IndepS2FreeMajor_Win), IndepS2FreeMajor_Win.ToString());
            updata.Add(nameof(IndepS2FreeMinor_Times), IndepS2FreeMinor_Times.ToString());
            updata.Add(nameof(IndepS2FreeMinor_Bet), IndepS2FreeMinor_Bet.ToString());
            updata.Add(nameof(IndepS2FreeMinor_Win), IndepS2FreeMinor_Win.ToString());
            updata.Add(nameof(IndepS2FreeMini_Times), IndepS2FreeMini_Times.ToString());
            updata.Add(nameof(IndepS2FreeMini_Bet), IndepS2FreeMini_Bet.ToString());
            updata.Add(nameof(IndepS2FreeMini_Win), IndepS2FreeMini_Win.ToString());

            updata.Add(nameof(ExPlay_Times), ExPlay_Times.ToString());
            updata.Add(nameof(ExPlay_Bet), ExPlay_Bet.ToString());
            updata.Add(nameof(ExPlay_Win), ExPlay_Win.ToString());
            updata.Add(nameof(ExPlay_WinTimes), ExPlay_WinTimes.ToString());
            updata.Add(nameof(ExPlayS1_Times), ExPlayS1_Times.ToString());
            updata.Add(nameof(ExPlayS1_Bet), ExPlayS1_Bet.ToString());
            updata.Add(nameof(ExPlayS1_Win), ExPlayS1_Win.ToString());
            updata.Add(nameof(ExPlayS1_WinTimes), ExPlayS1_WinTimes.ToString());
            updata.Add(nameof(ExPlayS2_Times), ExPlayS2_Times.ToString());
            updata.Add(nameof(ExPlayS2_Bet), ExPlayS2_Bet.ToString());
            updata.Add(nameof(ExPlayS2_Win), ExPlayS2_Win.ToString());
            updata.Add(nameof(ExPlayS2_WinTimes), ExPlayS2_WinTimes.ToString());
            updata.Add(nameof(ExPlayS3_Times), ExPlayS3_Times.ToString());
            updata.Add(nameof(ExPlayS3_Bet), ExPlayS3_Bet.ToString());
            updata.Add(nameof(ExPlayS3_Win), ExPlayS3_Win.ToString());
            updata.Add(nameof(ExPlayS3_WinTimes), ExPlayS3_WinTimes.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            Free_Times = Convert.ToInt32(datalist[nameof(Free_Times)]);
            Free_Bet = Convert.ToDouble(datalist[nameof(Free_Bet)]);
            Free_Win = Convert.ToDouble(datalist[nameof(Free_Win)]);
            FreeFree_Times = Convert.ToInt32(datalist[nameof(FreeFree_Times)]);
            FreeGrand_Times = Convert.ToInt32(datalist[nameof(FreeGrand_Times)]);
            FreeGrand_Bet = Convert.ToDouble(datalist[nameof(FreeGrand_Bet)]);
            FreeGrand_Win = Convert.ToDouble(datalist[nameof(FreeGrand_Win)]);
            FreeMajor_Times = Convert.ToInt32(datalist[nameof(FreeMajor_Times)]);
            FreeMajor_Bet = Convert.ToDouble(datalist[nameof(FreeMajor_Bet)]);
            FreeMajor_Win = Convert.ToDouble(datalist[nameof(FreeMajor_Win)]);
            FreeMinor_Times = Convert.ToInt32(datalist[nameof(FreeMinor_Times)]);
            FreeMinor_Bet = Convert.ToDouble(datalist[nameof(FreeMinor_Bet)]);
            FreeMinor_Win = Convert.ToDouble(datalist[nameof(FreeMinor_Win)]);
            FreeMini_Times = Convert.ToInt32(datalist[nameof(FreeMini_Times)]);
            FreeMini_Bet = Convert.ToDouble(datalist[nameof(FreeMini_Bet)]);
            FreeMini_Win = Convert.ToDouble(datalist[nameof(FreeMini_Win)]);
            Wild5_Times = Convert.ToInt32(datalist[nameof(Wild5_Times)]);
            Wild5_Bet = Convert.ToDouble(datalist[nameof(Wild5_Bet)]);
            Wild5_Win = Convert.ToDouble(datalist[nameof(Wild5_Win)]);
            ErlangShen5_Times = Convert.ToInt32(datalist[nameof(ErlangShen5_Times)]);
            ErlangShen5_Bet = Convert.ToDouble(datalist[nameof(ErlangShen5_Bet)]);
            ErlangShen5_Win = Convert.ToDouble(datalist[nameof(ErlangShen5_Win)]);
            Nezha5_Times = Convert.ToInt32(datalist[nameof(Nezha5_Times)]);
            Nezha5_Bet = Convert.ToDouble(datalist[nameof(Nezha5_Bet)]);
            Nezha5_Win = Convert.ToDouble(datalist[nameof(Nezha5_Win)]);
            Reel5_Times = Convert.ToInt32(datalist[nameof(Reel5_Times)]);
            Reel5_Bet = Convert.ToDouble(datalist[nameof(Reel5_Bet)]);
            Reel5_Win = Convert.ToDouble(datalist[nameof(Reel5_Win)]);
            Gourd5_Times = Convert.ToInt32(datalist[nameof(Gourd5_Times)]);
            Gourd5_Bet = Convert.ToDouble(datalist[nameof(Gourd5_Bet)]);
            Gourd5_Win = Convert.ToDouble(datalist[nameof(Gourd5_Win)]);
            A5_Times = Convert.ToInt32(datalist[nameof(A5_Times)]);
            A5_Bet = Convert.ToDouble(datalist[nameof(A5_Bet)]);
            A5_Win = Convert.ToDouble(datalist[nameof(A5_Win)]);
            K5_Times = Convert.ToInt32(datalist[nameof(K5_Times)]);
            K5_Bet = Convert.ToDouble(datalist[nameof(K5_Bet)]);
            K5_Win = Convert.ToDouble(datalist[nameof(K5_Win)]);
            Q5_Times = Convert.ToInt32(datalist[nameof(Q5_Times)]);
            Q5_Bet = Convert.ToDouble(datalist[nameof(Q5_Bet)]);
            Q5_Win = Convert.ToDouble(datalist[nameof(Q5_Win)]);
            J5_Times = Convert.ToInt32(datalist[nameof(J5_Times)]);
            J5_Bet = Convert.ToDouble(datalist[nameof(J5_Bet)]);
            J5_Win = Convert.ToDouble(datalist[nameof(J5_Win)]);
            Ten5_Times = Convert.ToInt32(datalist[nameof(Ten5_Times)]);
            Ten5_Bet = Convert.ToDouble(datalist[nameof(Ten5_Bet)]);
            Ten5_Win = Convert.ToDouble(datalist[nameof(Ten5_Win)]);

            IndepPlayTimes = Convert.ToInt32(datalist[nameof(IndepPlayTimes)]);
            IndepTotalBet = Convert.ToDouble(datalist[nameof(IndepTotalBet)]);
            IndepTotalWin = Convert.ToDouble(datalist[nameof(IndepTotalWin)]);
            IndepFreeFree_Times = Convert.ToInt32(datalist[nameof(IndepFreeFree_Times)]);
            IndepTotalRoundTimes = Convert.ToInt32(datalist[nameof(IndepTotalRoundTimes)]);
            IndepTotalWinTimes = Convert.ToInt32(datalist[nameof(IndepTotalWinTimes)]);
            IndepFreeGrand_Times = Convert.ToInt32(datalist[nameof(IndepFreeGrand_Times)]);
            IndepFreeGrand_Bet = Convert.ToDouble(datalist[nameof(IndepFreeGrand_Bet)]);
            IndepFreeGrand_Win = Convert.ToDouble(datalist[nameof(IndepFreeGrand_Win)]);
            IndepFreeMajor_Times = Convert.ToInt32(datalist[nameof(IndepFreeMajor_Times)]);
            IndepFreeMajor_Bet = Convert.ToDouble(datalist[nameof(IndepFreeMajor_Bet)]);
            IndepFreeMajor_Win = Convert.ToDouble(datalist[nameof(IndepFreeMajor_Win)]);
            IndepFreeMinor_Times = Convert.ToInt32(datalist[nameof(IndepFreeMinor_Times)]);
            IndepFreeMinor_Bet = Convert.ToDouble(datalist[nameof(IndepFreeMinor_Bet)]);
            IndepFreeMinor_Win = Convert.ToDouble(datalist[nameof(IndepFreeMinor_Win)]);
            IndepFreeMini_Times = Convert.ToInt32(datalist[nameof(IndepFreeMini_Times)]);
            IndepFreeMini_Bet = Convert.ToDouble(datalist[nameof(IndepFreeMini_Bet)]);
            IndepFreeMini_Win = Convert.ToDouble(datalist[nameof(IndepFreeMini_Win)]);
            IndepS1PlayTimes = Convert.ToInt32(datalist[nameof(IndepS1PlayTimes)]);
            IndepS1TotalBet = Convert.ToDouble(datalist[nameof(IndepS1TotalBet)]);
            IndepS1TotalWin = Convert.ToDouble(datalist[nameof(IndepS1TotalWin)]);
            IndepS1FreeFree_Times = Convert.ToInt32(datalist[nameof(IndepS1FreeFree_Times)]);
            IndepS1TotalRoundTimes = Convert.ToInt32(datalist[nameof(IndepS1TotalRoundTimes)]);
            IndepS1TotalWinTimes = Convert.ToInt32(datalist[nameof(IndepS1TotalWinTimes)]);
            IndepS1FreeGrand_Times = Convert.ToInt32(datalist[nameof(IndepS1FreeGrand_Times)]);
            IndepS1FreeGrand_Bet = Convert.ToDouble(datalist[nameof(IndepS1FreeGrand_Bet)]);
            IndepS1FreeGrand_Win = Convert.ToDouble(datalist[nameof(IndepS1FreeGrand_Win)]);
            IndepS1FreeMajor_Times = Convert.ToInt32(datalist[nameof(IndepS1FreeMajor_Times)]);
            IndepS1FreeMajor_Bet = Convert.ToDouble(datalist[nameof(IndepS1FreeMajor_Bet)]);
            IndepS1FreeMajor_Win = Convert.ToDouble(datalist[nameof(IndepS1FreeMajor_Win)]);
            IndepS1FreeMinor_Times = Convert.ToInt32(datalist[nameof(IndepS1FreeMinor_Times)]);
            IndepS1FreeMinor_Bet = Convert.ToDouble(datalist[nameof(IndepS1FreeMinor_Bet)]);
            IndepS1FreeMinor_Win = Convert.ToDouble(datalist[nameof(IndepS1FreeMinor_Win)]);
            IndepS1FreeMini_Times = Convert.ToInt32(datalist[nameof(IndepS1FreeMini_Times)]);
            IndepS1FreeMini_Bet = Convert.ToDouble(datalist[nameof(IndepS1FreeMini_Bet)]);
            IndepS1FreeMini_Win = Convert.ToDouble(datalist[nameof(IndepS1FreeMini_Win)]);
            IndepS2PlayTimes = Convert.ToInt32(datalist[nameof(IndepS2PlayTimes)]);
            IndepS2TotalBet = Convert.ToDouble(datalist[nameof(IndepS2TotalBet)]);
            IndepS2TotalWin = Convert.ToDouble(datalist[nameof(IndepS2TotalWin)]);
            IndepS2FreeFree_Times = Convert.ToInt32(datalist[nameof(IndepS2FreeFree_Times)]);
            IndepS2TotalRoundTimes = Convert.ToInt32(datalist[nameof(IndepS2TotalRoundTimes)]);
            IndepS2TotalWinTimes = Convert.ToInt32(datalist[nameof(IndepS2TotalWinTimes)]);
            IndepS2FreeGrand_Times = Convert.ToInt32(datalist[nameof(IndepS2FreeGrand_Times)]);
            IndepS2FreeGrand_Bet = Convert.ToDouble(datalist[nameof(IndepS2FreeGrand_Bet)]);
            IndepS2FreeGrand_Win = Convert.ToDouble(datalist[nameof(IndepS2FreeGrand_Win)]);
            IndepS2FreeMajor_Times = Convert.ToInt32(datalist[nameof(IndepS2FreeMajor_Times)]);
            IndepS2FreeMajor_Bet = Convert.ToDouble(datalist[nameof(IndepS2FreeMajor_Bet)]);
            IndepS2FreeMajor_Win = Convert.ToDouble(datalist[nameof(IndepS2FreeMajor_Win)]);
            IndepS2FreeMinor_Times = Convert.ToInt32(datalist[nameof(IndepS2FreeMinor_Times)]);
            IndepS2FreeMinor_Bet = Convert.ToDouble(datalist[nameof(IndepS2FreeMinor_Bet)]);
            IndepS2FreeMinor_Win = Convert.ToDouble(datalist[nameof(IndepS2FreeMinor_Win)]);
            IndepS2FreeMini_Times = Convert.ToInt32(datalist[nameof(IndepS2FreeMini_Times)]);
            IndepS2FreeMini_Bet = Convert.ToDouble(datalist[nameof(IndepS2FreeMini_Bet)]);
            IndepS2FreeMini_Win = Convert.ToDouble(datalist[nameof(IndepS2FreeMini_Win)]);

            ExPlay_Times = Convert.ToInt32(datalist[nameof(ExPlay_Times)]);
            ExPlay_Bet = Convert.ToDouble(datalist[nameof(ExPlay_Bet)]);
            ExPlay_Win = Convert.ToDouble(datalist[nameof(ExPlay_Win)]);
            ExPlay_WinTimes = Convert.ToInt32(datalist[nameof(ExPlay_WinTimes)]);
            ExPlayS1_Times = Convert.ToInt32(datalist[nameof(ExPlayS1_Times)]);
            ExPlayS1_Bet = Convert.ToDouble(datalist[nameof(ExPlayS1_Bet)]);
            ExPlayS1_Win = Convert.ToDouble(datalist[nameof(ExPlayS1_Win)]);
            ExPlayS1_WinTimes = Convert.ToInt32(datalist[nameof(ExPlayS1_WinTimes)]);
            ExPlayS2_Times = Convert.ToInt32(datalist[nameof(ExPlayS2_Times)]);
            ExPlayS2_Bet = Convert.ToDouble(datalist[nameof(ExPlayS2_Bet)]);
            ExPlayS2_Win = Convert.ToDouble(datalist[nameof(ExPlayS2_Win)]);
            ExPlayS2_WinTimes = Convert.ToInt32(datalist[nameof(ExPlayS2_WinTimes)]);
            ExPlayS3_Times = Convert.ToInt32(datalist[nameof(ExPlayS3_Times)]);
            ExPlayS3_Bet = Convert.ToDouble(datalist[nameof(ExPlayS3_Bet)]);
            ExPlayS3_Win = Convert.ToDouble(datalist[nameof(ExPlayS3_Win)]);
            ExPlayS3_WinTimes = Convert.ToInt32(datalist[nameof(ExPlayS3_WinTimes)]);
        }
    }
}
