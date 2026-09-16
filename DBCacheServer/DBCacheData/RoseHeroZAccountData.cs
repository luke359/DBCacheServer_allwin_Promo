using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class RoseHeroZAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int FreeFree_Times;
        private int FreeGrand_Times;
        private int FreeMega_Times;
        private int FreeMajor_Times;
        private int FreeMinor_Times;
        private int FreeMini_Times;
        private int Rose_Times;
        private double Rose_Bet;
        private double Rose_Win;
        private int RoseGolden_Times;
        private int RoseOrange_Times;
        private int RoseRed_Times;
        private int RoseBlue_Times;
        private int RoseGrand_Times;
        private int RoseMega_Times;
        private int RoseMajor_Times;
        private int RoseMinor_Times;
        private int RoseMini_Times;
        private int Logo5_Times;
        private double Logo5_Bet;
        private double Logo5_Win;
        private int Logo4_Times;
        private double Logo4_Bet;
        private double Logo4_Win;
        private int Logo3_Times;
        private double Logo3_Bet;
        private double Logo3_Win;
        private int Heroine5_Times;
        private double Heroine5_Bet;
        private double Heroine5_Win;
        private int Heroine4_Times;
        private double Heroine4_Bet;
        private double Heroine4_Win;
        private int Heroine3_Times;
        private double Heroine3_Bet;
        private double Heroine3_Win;
        private int Lady5_Times;
        private double Lady5_Bet;
        private double Lady5_Win;
        private int Lady4_Times;
        private double Lady4_Bet;
        private double Lady4_Win;
        private int Lady3_Times;
        private double Lady3_Bet;
        private double Lady3_Win;
        private int Horse5_Times;
        private double Horse5_Bet;
        private double Horse5_Win;
        private int Horse4_Times;
        private double Horse4_Bet;
        private double Horse4_Win;
        private int Horse3_Times;
        private double Horse3_Bet;
        private double Horse3_Win;
        private int Mask5_Times;
        private double Mask5_Bet;
        private double Mask5_Win;
        private int Mask4_Times;
        private double Mask4_Bet;
        private double Mask4_Win;
        private int Mask3_Times;
        private double Mask3_Bet;
        private double Mask3_Win;
        private int Fan5_Times;
        private double Fan5_Bet;
        private double Fan5_Win;
        private int Fan4_Times;
        private double Fan4_Bet;
        private double Fan4_Win;
        private int Fan3_Times;
        private double Fan3_Bet;
        private double Fan3_Win;
        private int A5_Times;
        private double A5_Bet;
        private double A5_Win;
        private int A4_Times;
        private double A4_Bet;
        private double A4_Win;
        private int A3_Times;
        private double A3_Bet;
        private double A3_Win;
        private int K5_Times;
        private double K5_Bet;
        private double K5_Win;
        private int K4_Times;
        private double K4_Bet;
        private double K4_Win;
        private int K3_Times;
        private double K3_Bet;
        private double K3_Win;
        private int Q5_Times;
        private double Q5_Bet;
        private double Q5_Win;
        private int Q4_Times;
        private double Q4_Bet;
        private double Q4_Win;
        private int Q3_Times;
        private double Q3_Bet;
        private double Q3_Win;
        private int J5_Times;
        private double J5_Bet;
        private double J5_Win;
        private int J4_Times;
        private double J4_Bet;
        private double J4_Win;
        private int J3_Times;
        private double J3_Bet;
        private double J3_Win;
        private int IndepPlayTimes;
        private double IndepTotalBet;
        private double IndepTotalWin;
        private int IndepFreeFree_Times;
        private int IndepFreeGrand_Times;
        private int IndepFreeMega_Times;
        private int IndepFreeMajor_Times;
        private int IndepFreeMinor_Times;
        private int IndepFreeMini_Times;
        //250714追加
        private int FreeRose_Times;
        private double FreeRose_Win;
        private int RoseHit_Times;
        private int IndepTotalRoundTimes;
        private int IndepTotalWinTimes;
        private int IndepFreeRose_Times;
        private double IndepFreeRose_Win;
        #endregion

        /// <summary>清除額外押注</summary>
        public override void ClearCacheGameExPlay()
        {
            //update = true;
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
                FreeMega_Times = 0;
                FreeMajor_Times = 0;
                FreeMinor_Times = 0;
                FreeMini_Times = 0;
                Rose_Times = 0;
                Rose_Bet = 0;
                Rose_Win = 0;
                RoseGolden_Times = 0;
                RoseOrange_Times = 0;
                RoseRed_Times = 0;
                RoseBlue_Times = 0;
                RoseGrand_Times = 0;
                RoseMega_Times = 0;
                RoseMajor_Times = 0;
                RoseMinor_Times = 0;
                RoseMini_Times = 0;
                Logo5_Times = 0;
                Logo5_Bet = 0;
                Logo5_Win = 0;
                Logo4_Times = 0;
                Logo4_Bet = 0;
                Logo4_Win = 0;
                Logo3_Times = 0;
                Logo3_Bet = 0;
                Logo3_Win = 0;
                Heroine5_Times = 0;
                Heroine5_Bet = 0;
                Heroine5_Win = 0;
                Heroine4_Times = 0;
                Heroine4_Bet = 0;
                Heroine4_Win = 0;
                Heroine3_Times = 0;
                Heroine3_Bet = 0;
                Heroine3_Win = 0;
                Lady5_Times = 0;
                Lady5_Bet = 0;
                Lady5_Win = 0;
                Lady4_Times = 0;
                Lady4_Bet = 0;
                Lady4_Win = 0;
                Lady3_Times = 0;
                Lady3_Bet = 0;
                Lady3_Win = 0;
                Horse5_Times = 0;
                Horse5_Bet = 0;
                Horse5_Win = 0;
                Horse4_Times = 0;
                Horse4_Bet = 0;
                Horse4_Win = 0;
                Horse3_Times = 0;
                Horse3_Bet = 0;
                Horse3_Win = 0;
                Mask5_Times = 0;
                Mask5_Bet = 0;
                Mask5_Win = 0;
                Mask4_Times = 0;
                Mask4_Bet = 0;
                Mask4_Win = 0;
                Mask3_Times = 0;
                Mask3_Bet = 0;
                Mask3_Win = 0;
                Fan5_Times = 0;
                Fan5_Bet = 0;
                Fan5_Win = 0;
                Fan4_Times = 0;
                Fan4_Bet = 0;
                Fan4_Win = 0;
                Fan3_Times = 0;
                Fan3_Bet = 0;
                Fan3_Win = 0;
                A5_Times = 0;
                A5_Bet = 0;
                A5_Win = 0;
                A4_Times = 0;
                A4_Bet = 0;
                A4_Win = 0;
                A3_Times = 0;
                A3_Bet = 0;
                A3_Win = 0;
                K5_Times = 0;
                K5_Bet = 0;
                K5_Win = 0;
                K4_Times = 0;
                K4_Bet = 0;
                K4_Win = 0;
                K3_Times = 0;
                K3_Bet = 0;
                K3_Win = 0;
                Q5_Times = 0;
                Q5_Bet = 0;
                Q5_Win = 0;
                Q4_Times = 0;
                Q4_Bet = 0;
                Q4_Win = 0;
                Q3_Times = 0;
                Q3_Bet = 0;
                Q3_Win = 0;
                J5_Times = 0;
                J5_Bet = 0;
                J5_Win = 0;
                J4_Times = 0;
                J4_Bet = 0;
                J4_Win = 0;
                J3_Times = 0;
                J3_Bet = 0;
                J3_Win = 0;

                FreeRose_Times = 0;
                FreeRose_Win = 0;
                RoseHit_Times = 0;
            }
            if (mode == 1 || mode == 2)
            {
                IndepPlayTimes = 0;
                IndepTotalBet = 0;
                IndepTotalWin = 0;
                IndepFreeFree_Times = 0;
                IndepFreeGrand_Times = 0;
                IndepFreeMega_Times = 0;
                IndepFreeMajor_Times = 0;
                IndepFreeMinor_Times = 0;
                IndepFreeMini_Times = 0;

                IndepTotalRoundTimes = 0;
                IndepTotalWinTimes = 0;
                IndepFreeRose_Times = 0;
                IndepFreeRose_Win = 0;
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
            if (field == nameof(FreeMega_Times)) { FreeMega_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMajor_Times)) { FreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMinor_Times)) { FreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMini_Times)) { FreeMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Rose_Times)) { Rose_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Rose_Bet)) { Rose_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Rose_Win)) { Rose_Win += Convert.ToDouble(value); return; }
            if (field == nameof(RoseGolden_Times)) { RoseGolden_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseOrange_Times)) { RoseOrange_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseRed_Times)) { RoseRed_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseBlue_Times)) { RoseBlue_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseGrand_Times)) { RoseGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseMega_Times)) { RoseMega_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseMajor_Times)) { RoseMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseMinor_Times)) { RoseMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RoseMini_Times)) { RoseMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo5_Times)) { Logo5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo5_Bet)) { Logo5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Win)) { Logo5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Times)) { Logo4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo4_Bet)) { Logo4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Win)) { Logo4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Times)) { Logo3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo3_Bet)) { Logo3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Win)) { Logo3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Heroine5_Times)) { Heroine5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Heroine5_Bet)) { Heroine5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Heroine5_Win)) { Heroine5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Heroine4_Times)) { Heroine4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Heroine4_Bet)) { Heroine4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Heroine4_Win)) { Heroine4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Heroine3_Times)) { Heroine3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Heroine3_Bet)) { Heroine3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Heroine3_Win)) { Heroine3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Lady5_Times)) { Lady5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Lady5_Bet)) { Lady5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Lady5_Win)) { Lady5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Lady4_Times)) { Lady4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Lady4_Bet)) { Lady4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Lady4_Win)) { Lady4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Lady3_Times)) { Lady3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Lady3_Bet)) { Lady3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Lady3_Win)) { Lady3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Horse5_Times)) { Horse5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Horse5_Bet)) { Horse5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Horse5_Win)) { Horse5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Horse4_Times)) { Horse4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Horse4_Bet)) { Horse4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Horse4_Win)) { Horse4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Horse3_Times)) { Horse3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Horse3_Bet)) { Horse3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Horse3_Win)) { Horse3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Mask5_Times)) { Mask5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Mask5_Bet)) { Mask5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Mask5_Win)) { Mask5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Mask4_Times)) { Mask4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Mask4_Bet)) { Mask4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Mask4_Win)) { Mask4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Mask3_Times)) { Mask3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Mask3_Bet)) { Mask3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Mask3_Win)) { Mask3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan5_Times)) { Fan5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan5_Bet)) { Fan5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan5_Win)) { Fan5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan4_Times)) { Fan4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan4_Bet)) { Fan4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan4_Win)) { Fan4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan3_Times)) { Fan3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan3_Bet)) { Fan3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan3_Win)) { Fan3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A5_Times)) { A5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A5_Bet)) { A5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A5_Win)) { A5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A4_Times)) { A4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A4_Bet)) { A4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A4_Win)) { A4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A3_Times)) { A3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A3_Bet)) { A3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A3_Win)) { A3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K5_Times)) { K5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K5_Bet)) { K5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K5_Win)) { K5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K4_Times)) { K4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K4_Bet)) { K4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K4_Win)) { K4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K3_Times)) { K3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K3_Bet)) { K3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K3_Win)) { K3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q5_Times)) { Q5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q5_Bet)) { Q5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q5_Win)) { Q5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q4_Times)) { Q4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q4_Bet)) { Q4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q4_Win)) { Q4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q3_Times)) { Q3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q3_Bet)) { Q3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q3_Win)) { Q3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J5_Times)) { J5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J5_Bet)) { J5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J5_Win)) { J5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J4_Times)) { J4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J4_Bet)) { J4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J4_Win)) { J4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J3_Times)) { J3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J3_Bet)) { J3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J3_Win)) { J3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepPlayTimes)) { IndepPlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalBet)) { IndepTotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepTotalWin)) { IndepTotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeFree_Times)) { IndepFreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeGrand_Times)) { IndepFreeGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMega_Times)) { IndepFreeMega_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMajor_Times)) { IndepFreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMinor_Times)) { IndepFreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMini_Times)) { IndepFreeMini_Times += Convert.ToInt32(value); return; }

            if (field == nameof(FreeRose_Times)) { FreeRose_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRose_Win)) { FreeRose_Win += Convert.ToDouble(value); return; }
            if (field == nameof(RoseHit_Times)) { RoseHit_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalRoundTimes)) { IndepTotalRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalWinTimes)) { IndepTotalWinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeRose_Times)) { IndepFreeRose_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeRose_Win)) { IndepFreeRose_Win += Convert.ToDouble(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(Free_Times), Free_Times.ToString());
            updata.Add(nameof(Free_Bet), Free_Bet.ToString());
            updata.Add(nameof(Free_Win), Free_Win.ToString());
            updata.Add(nameof(FreeFree_Times), FreeFree_Times.ToString());
            updata.Add(nameof(FreeGrand_Times), FreeGrand_Times.ToString());
            updata.Add(nameof(FreeMega_Times), FreeMega_Times.ToString());
            updata.Add(nameof(FreeMajor_Times), FreeMajor_Times.ToString());
            updata.Add(nameof(FreeMinor_Times), FreeMinor_Times.ToString());
            updata.Add(nameof(FreeMini_Times), FreeMini_Times.ToString());
            updata.Add(nameof(Rose_Times), Rose_Times.ToString());
            updata.Add(nameof(Rose_Bet), Rose_Bet.ToString());
            updata.Add(nameof(Rose_Win), Rose_Win.ToString());
            updata.Add(nameof(RoseGolden_Times), RoseGolden_Times.ToString());
            updata.Add(nameof(RoseOrange_Times), RoseOrange_Times.ToString());
            updata.Add(nameof(RoseRed_Times), RoseRed_Times.ToString());
            updata.Add(nameof(RoseBlue_Times), RoseBlue_Times.ToString());
            updata.Add(nameof(RoseGrand_Times), RoseGrand_Times.ToString());
            updata.Add(nameof(RoseMega_Times), RoseMega_Times.ToString());
            updata.Add(nameof(RoseMajor_Times), RoseMajor_Times.ToString());
            updata.Add(nameof(RoseMinor_Times), RoseMinor_Times.ToString());
            updata.Add(nameof(RoseMini_Times), RoseMini_Times.ToString());
            updata.Add(nameof(Logo5_Times), Logo5_Times.ToString());
            updata.Add(nameof(Logo5_Bet), Logo5_Bet.ToString());
            updata.Add(nameof(Logo5_Win), Logo5_Win.ToString());
            updata.Add(nameof(Logo4_Times), Logo4_Times.ToString());
            updata.Add(nameof(Logo4_Bet), Logo4_Bet.ToString());
            updata.Add(nameof(Logo4_Win), Logo4_Win.ToString());
            updata.Add(nameof(Logo3_Times), Logo3_Times.ToString());
            updata.Add(nameof(Logo3_Bet), Logo3_Bet.ToString());
            updata.Add(nameof(Logo3_Win), Logo3_Win.ToString());
            updata.Add(nameof(Heroine5_Times), Heroine5_Times.ToString());
            updata.Add(nameof(Heroine5_Bet), Heroine5_Bet.ToString());
            updata.Add(nameof(Heroine5_Win), Heroine5_Win.ToString());
            updata.Add(nameof(Heroine4_Times), Heroine4_Times.ToString());
            updata.Add(nameof(Heroine4_Bet), Heroine4_Bet.ToString());
            updata.Add(nameof(Heroine4_Win), Heroine4_Win.ToString());
            updata.Add(nameof(Heroine3_Times), Heroine3_Times.ToString());
            updata.Add(nameof(Heroine3_Bet), Heroine3_Bet.ToString());
            updata.Add(nameof(Heroine3_Win), Heroine3_Win.ToString());
            updata.Add(nameof(Lady5_Times), Lady5_Times.ToString());
            updata.Add(nameof(Lady5_Bet), Lady5_Bet.ToString());
            updata.Add(nameof(Lady5_Win), Lady5_Win.ToString());
            updata.Add(nameof(Lady4_Times), Lady4_Times.ToString());
            updata.Add(nameof(Lady4_Bet), Lady4_Bet.ToString());
            updata.Add(nameof(Lady4_Win), Lady4_Win.ToString());
            updata.Add(nameof(Lady3_Times), Lady3_Times.ToString());
            updata.Add(nameof(Lady3_Bet), Lady3_Bet.ToString());
            updata.Add(nameof(Lady3_Win), Lady3_Win.ToString());
            updata.Add(nameof(Horse5_Times), Horse5_Times.ToString());
            updata.Add(nameof(Horse5_Bet), Horse5_Bet.ToString());
            updata.Add(nameof(Horse5_Win), Horse5_Win.ToString());
            updata.Add(nameof(Horse4_Times), Horse4_Times.ToString());
            updata.Add(nameof(Horse4_Bet), Horse4_Bet.ToString());
            updata.Add(nameof(Horse4_Win), Horse4_Win.ToString());
            updata.Add(nameof(Horse3_Times), Horse3_Times.ToString());
            updata.Add(nameof(Horse3_Bet), Horse3_Bet.ToString());
            updata.Add(nameof(Horse3_Win), Horse3_Win.ToString());
            updata.Add(nameof(Mask5_Times), Mask5_Times.ToString());
            updata.Add(nameof(Mask5_Bet), Mask5_Bet.ToString());
            updata.Add(nameof(Mask5_Win), Mask5_Win.ToString());
            updata.Add(nameof(Mask4_Times), Mask4_Times.ToString());
            updata.Add(nameof(Mask4_Bet), Mask4_Bet.ToString());
            updata.Add(nameof(Mask4_Win), Mask4_Win.ToString());
            updata.Add(nameof(Mask3_Times), Mask3_Times.ToString());
            updata.Add(nameof(Mask3_Bet), Mask3_Bet.ToString());
            updata.Add(nameof(Mask3_Win), Mask3_Win.ToString());
            updata.Add(nameof(Fan5_Times), Fan5_Times.ToString());
            updata.Add(nameof(Fan5_Bet), Fan5_Bet.ToString());
            updata.Add(nameof(Fan5_Win), Fan5_Win.ToString());
            updata.Add(nameof(Fan4_Times), Fan4_Times.ToString());
            updata.Add(nameof(Fan4_Bet), Fan4_Bet.ToString());
            updata.Add(nameof(Fan4_Win), Fan4_Win.ToString());
            updata.Add(nameof(Fan3_Times), Fan3_Times.ToString());
            updata.Add(nameof(Fan3_Bet), Fan3_Bet.ToString());
            updata.Add(nameof(Fan3_Win), Fan3_Win.ToString());
            updata.Add(nameof(A5_Times), A5_Times.ToString());
            updata.Add(nameof(A5_Bet), A5_Bet.ToString());
            updata.Add(nameof(A5_Win), A5_Win.ToString());
            updata.Add(nameof(A4_Times), A4_Times.ToString());
            updata.Add(nameof(A4_Bet), A4_Bet.ToString());
            updata.Add(nameof(A4_Win), A4_Win.ToString());
            updata.Add(nameof(A3_Times), A3_Times.ToString());
            updata.Add(nameof(A3_Bet), A3_Bet.ToString());
            updata.Add(nameof(A3_Win), A3_Win.ToString());
            updata.Add(nameof(K5_Times), K5_Times.ToString());
            updata.Add(nameof(K5_Bet), K5_Bet.ToString());
            updata.Add(nameof(K5_Win), K5_Win.ToString());
            updata.Add(nameof(K4_Times), K4_Times.ToString());
            updata.Add(nameof(K4_Bet), K4_Bet.ToString());
            updata.Add(nameof(K4_Win), K4_Win.ToString());
            updata.Add(nameof(K3_Times), K3_Times.ToString());
            updata.Add(nameof(K3_Bet), K3_Bet.ToString());
            updata.Add(nameof(K3_Win), K3_Win.ToString());
            updata.Add(nameof(Q5_Times), Q5_Times.ToString());
            updata.Add(nameof(Q5_Bet), Q5_Bet.ToString());
            updata.Add(nameof(Q5_Win), Q5_Win.ToString());
            updata.Add(nameof(Q4_Times), Q4_Times.ToString());
            updata.Add(nameof(Q4_Bet), Q4_Bet.ToString());
            updata.Add(nameof(Q4_Win), Q4_Win.ToString());
            updata.Add(nameof(Q3_Times), Q3_Times.ToString());
            updata.Add(nameof(Q3_Bet), Q3_Bet.ToString());
            updata.Add(nameof(Q3_Win), Q3_Win.ToString());
            updata.Add(nameof(J5_Times), J5_Times.ToString());
            updata.Add(nameof(J5_Bet), J5_Bet.ToString());
            updata.Add(nameof(J5_Win), J5_Win.ToString());
            updata.Add(nameof(J4_Times), J4_Times.ToString());
            updata.Add(nameof(J4_Bet), J4_Bet.ToString());
            updata.Add(nameof(J4_Win), J4_Win.ToString());
            updata.Add(nameof(J3_Times), J3_Times.ToString());
            updata.Add(nameof(J3_Bet), J3_Bet.ToString());
            updata.Add(nameof(J3_Win), J3_Win.ToString());
            updata.Add(nameof(IndepPlayTimes), IndepPlayTimes.ToString());
            updata.Add(nameof(IndepTotalBet), IndepTotalBet.ToString());
            updata.Add(nameof(IndepTotalWin), IndepTotalWin.ToString());
            updata.Add(nameof(IndepFreeFree_Times), IndepFreeFree_Times.ToString());
            updata.Add(nameof(IndepFreeGrand_Times), IndepFreeGrand_Times.ToString());
            updata.Add(nameof(IndepFreeMega_Times), IndepFreeMega_Times.ToString());
            updata.Add(nameof(IndepFreeMajor_Times), IndepFreeMajor_Times.ToString());
            updata.Add(nameof(IndepFreeMinor_Times), IndepFreeMinor_Times.ToString());
            updata.Add(nameof(IndepFreeMini_Times), IndepFreeMini_Times.ToString());

            updata.Add(nameof(FreeRose_Times), FreeRose_Times.ToString());
            updata.Add(nameof(FreeRose_Win), FreeRose_Win.ToString());
            updata.Add(nameof(RoseHit_Times), RoseHit_Times.ToString());
            updata.Add(nameof(IndepTotalRoundTimes), IndepTotalRoundTimes.ToString());
            updata.Add(nameof(IndepTotalWinTimes), IndepTotalWinTimes.ToString());
            updata.Add(nameof(IndepFreeRose_Times), IndepFreeRose_Times.ToString());
            updata.Add(nameof(IndepFreeRose_Win), IndepFreeRose_Win.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            Free_Times = Convert.ToInt32(datalist[nameof(Free_Times)]);
            Free_Bet = Convert.ToDouble(datalist[nameof(Free_Bet)]);
            Free_Win = Convert.ToDouble(datalist[nameof(Free_Win)]);
            FreeFree_Times = Convert.ToInt32(datalist[nameof(FreeFree_Times)]);
            FreeGrand_Times = Convert.ToInt32(datalist[nameof(FreeGrand_Times)]);
            FreeMega_Times = Convert.ToInt32(datalist[nameof(FreeMega_Times)]);
            FreeMajor_Times = Convert.ToInt32(datalist[nameof(FreeMajor_Times)]);
            FreeMinor_Times = Convert.ToInt32(datalist[nameof(FreeMinor_Times)]);
            FreeMini_Times = Convert.ToInt32(datalist[nameof(FreeMini_Times)]);
            Rose_Times = Convert.ToInt32(datalist[nameof(Rose_Times)]);
            Rose_Bet = Convert.ToDouble(datalist[nameof(Rose_Bet)]);
            Rose_Win = Convert.ToDouble(datalist[nameof(Rose_Win)]);
            RoseGolden_Times = Convert.ToInt32(datalist[nameof(RoseGolden_Times)]);
            RoseOrange_Times = Convert.ToInt32(datalist[nameof(RoseOrange_Times)]);
            RoseRed_Times = Convert.ToInt32(datalist[nameof(RoseRed_Times)]);
            RoseBlue_Times = Convert.ToInt32(datalist[nameof(RoseBlue_Times)]);
            RoseGrand_Times = Convert.ToInt32(datalist[nameof(RoseGrand_Times)]);
            RoseMega_Times = Convert.ToInt32(datalist[nameof(RoseMega_Times)]);
            RoseMajor_Times = Convert.ToInt32(datalist[nameof(RoseMajor_Times)]);
            RoseMinor_Times = Convert.ToInt32(datalist[nameof(RoseMinor_Times)]);
            RoseMini_Times = Convert.ToInt32(datalist[nameof(RoseMini_Times)]);
            Logo5_Times = Convert.ToInt32(datalist[nameof(Logo5_Times)]);
            Logo5_Bet = Convert.ToDouble(datalist[nameof(Logo5_Bet)]);
            Logo5_Win = Convert.ToDouble(datalist[nameof(Logo5_Win)]);
            Logo4_Times = Convert.ToInt32(datalist[nameof(Logo4_Times)]);
            Logo4_Bet = Convert.ToDouble(datalist[nameof(Logo4_Bet)]);
            Logo4_Win = Convert.ToDouble(datalist[nameof(Logo4_Win)]);
            Logo3_Times = Convert.ToInt32(datalist[nameof(Logo3_Times)]);
            Logo3_Bet = Convert.ToDouble(datalist[nameof(Logo3_Bet)]);
            Logo3_Win = Convert.ToDouble(datalist[nameof(Logo3_Win)]);
            Heroine5_Times = Convert.ToInt32(datalist[nameof(Heroine5_Times)]);
            Heroine5_Bet = Convert.ToDouble(datalist[nameof(Heroine5_Bet)]);
            Heroine5_Win = Convert.ToDouble(datalist[nameof(Heroine5_Win)]);
            Heroine4_Times = Convert.ToInt32(datalist[nameof(Heroine4_Times)]);
            Heroine4_Bet = Convert.ToDouble(datalist[nameof(Heroine4_Bet)]);
            Heroine4_Win = Convert.ToDouble(datalist[nameof(Heroine4_Win)]);
            Heroine3_Times = Convert.ToInt32(datalist[nameof(Heroine3_Times)]);
            Heroine3_Bet = Convert.ToDouble(datalist[nameof(Heroine3_Bet)]);
            Heroine3_Win = Convert.ToDouble(datalist[nameof(Heroine3_Win)]);
            Lady5_Times = Convert.ToInt32(datalist[nameof(Lady5_Times)]);
            Lady5_Bet = Convert.ToDouble(datalist[nameof(Lady5_Bet)]);
            Lady5_Win = Convert.ToDouble(datalist[nameof(Lady5_Win)]);
            Lady4_Times = Convert.ToInt32(datalist[nameof(Lady4_Times)]);
            Lady4_Bet = Convert.ToDouble(datalist[nameof(Lady4_Bet)]);
            Lady4_Win = Convert.ToDouble(datalist[nameof(Lady4_Win)]);
            Lady3_Times = Convert.ToInt32(datalist[nameof(Lady3_Times)]);
            Lady3_Bet = Convert.ToDouble(datalist[nameof(Lady3_Bet)]);
            Lady3_Win = Convert.ToDouble(datalist[nameof(Lady3_Win)]);
            Horse5_Times = Convert.ToInt32(datalist[nameof(Horse5_Times)]);
            Horse5_Bet = Convert.ToDouble(datalist[nameof(Horse5_Bet)]);
            Horse5_Win = Convert.ToDouble(datalist[nameof(Horse5_Win)]);
            Horse4_Times = Convert.ToInt32(datalist[nameof(Horse4_Times)]);
            Horse4_Bet = Convert.ToDouble(datalist[nameof(Horse4_Bet)]);
            Horse4_Win = Convert.ToDouble(datalist[nameof(Horse4_Win)]);
            Horse3_Times = Convert.ToInt32(datalist[nameof(Horse3_Times)]);
            Horse3_Bet = Convert.ToDouble(datalist[nameof(Horse3_Bet)]);
            Horse3_Win = Convert.ToDouble(datalist[nameof(Horse3_Win)]);
            Mask5_Times = Convert.ToInt32(datalist[nameof(Mask5_Times)]);
            Mask5_Bet = Convert.ToDouble(datalist[nameof(Mask5_Bet)]);
            Mask5_Win = Convert.ToDouble(datalist[nameof(Mask5_Win)]);
            Mask4_Times = Convert.ToInt32(datalist[nameof(Mask4_Times)]);
            Mask4_Bet = Convert.ToDouble(datalist[nameof(Mask4_Bet)]);
            Mask4_Win = Convert.ToDouble(datalist[nameof(Mask4_Win)]);
            Mask3_Times = Convert.ToInt32(datalist[nameof(Mask3_Times)]);
            Mask3_Bet = Convert.ToDouble(datalist[nameof(Mask3_Bet)]);
            Mask3_Win = Convert.ToDouble(datalist[nameof(Mask3_Win)]);
            Fan5_Times = Convert.ToInt32(datalist[nameof(Fan5_Times)]);
            Fan5_Bet = Convert.ToDouble(datalist[nameof(Fan5_Bet)]);
            Fan5_Win = Convert.ToDouble(datalist[nameof(Fan5_Win)]);
            Fan4_Times = Convert.ToInt32(datalist[nameof(Fan4_Times)]);
            Fan4_Bet = Convert.ToDouble(datalist[nameof(Fan4_Bet)]);
            Fan4_Win = Convert.ToDouble(datalist[nameof(Fan4_Win)]);
            Fan3_Times = Convert.ToInt32(datalist[nameof(Fan3_Times)]);
            Fan3_Bet = Convert.ToDouble(datalist[nameof(Fan3_Bet)]);
            Fan3_Win = Convert.ToDouble(datalist[nameof(Fan3_Win)]);
            A5_Times = Convert.ToInt32(datalist[nameof(A5_Times)]);
            A5_Bet = Convert.ToDouble(datalist[nameof(A5_Bet)]);
            A5_Win = Convert.ToDouble(datalist[nameof(A5_Win)]);
            A4_Times = Convert.ToInt32(datalist[nameof(A4_Times)]);
            A4_Bet = Convert.ToDouble(datalist[nameof(A4_Bet)]);
            A4_Win = Convert.ToDouble(datalist[nameof(A4_Win)]);
            A3_Times = Convert.ToInt32(datalist[nameof(A3_Times)]);
            A3_Bet = Convert.ToDouble(datalist[nameof(A3_Bet)]);
            A3_Win = Convert.ToDouble(datalist[nameof(A3_Win)]);
            K5_Times = Convert.ToInt32(datalist[nameof(K5_Times)]);
            K5_Bet = Convert.ToDouble(datalist[nameof(K5_Bet)]);
            K5_Win = Convert.ToDouble(datalist[nameof(K5_Win)]);
            K4_Times = Convert.ToInt32(datalist[nameof(K4_Times)]);
            K4_Bet = Convert.ToDouble(datalist[nameof(K4_Bet)]);
            K4_Win = Convert.ToDouble(datalist[nameof(K4_Win)]);
            K3_Times = Convert.ToInt32(datalist[nameof(K3_Times)]);
            K3_Bet = Convert.ToDouble(datalist[nameof(K3_Bet)]);
            K3_Win = Convert.ToDouble(datalist[nameof(K3_Win)]);
            Q5_Times = Convert.ToInt32(datalist[nameof(Q5_Times)]);
            Q5_Bet = Convert.ToDouble(datalist[nameof(Q5_Bet)]);
            Q5_Win = Convert.ToDouble(datalist[nameof(Q5_Win)]);
            Q4_Times = Convert.ToInt32(datalist[nameof(Q4_Times)]);
            Q4_Bet = Convert.ToDouble(datalist[nameof(Q4_Bet)]);
            Q4_Win = Convert.ToDouble(datalist[nameof(Q4_Win)]);
            Q3_Times = Convert.ToInt32(datalist[nameof(Q3_Times)]);
            Q3_Bet = Convert.ToDouble(datalist[nameof(Q3_Bet)]);
            Q3_Win = Convert.ToDouble(datalist[nameof(Q3_Win)]);
            J5_Times = Convert.ToInt32(datalist[nameof(J5_Times)]);
            J5_Bet = Convert.ToDouble(datalist[nameof(J5_Bet)]);
            J5_Win = Convert.ToDouble(datalist[nameof(J5_Win)]);
            J4_Times = Convert.ToInt32(datalist[nameof(J4_Times)]);
            J4_Bet = Convert.ToDouble(datalist[nameof(J4_Bet)]);
            J4_Win = Convert.ToDouble(datalist[nameof(J4_Win)]);
            J3_Times = Convert.ToInt32(datalist[nameof(J3_Times)]);
            J3_Bet = Convert.ToDouble(datalist[nameof(J3_Bet)]);
            J3_Win = Convert.ToDouble(datalist[nameof(J3_Win)]);
            IndepPlayTimes = Convert.ToInt32(datalist[nameof(IndepPlayTimes)]);
            IndepTotalBet = Convert.ToDouble(datalist[nameof(IndepTotalBet)]);
            IndepTotalWin = Convert.ToDouble(datalist[nameof(IndepTotalWin)]);
            IndepFreeFree_Times = Convert.ToInt32(datalist[nameof(IndepFreeFree_Times)]);
            IndepFreeGrand_Times = Convert.ToInt32(datalist[nameof(IndepFreeGrand_Times)]);
            IndepFreeMega_Times = Convert.ToInt32(datalist[nameof(IndepFreeMega_Times)]);
            IndepFreeMajor_Times = Convert.ToInt32(datalist[nameof(IndepFreeMajor_Times)]);
            IndepFreeMinor_Times = Convert.ToInt32(datalist[nameof(IndepFreeMinor_Times)]);
            IndepFreeMini_Times = Convert.ToInt32(datalist[nameof(IndepFreeMini_Times)]);

            FreeRose_Times = Convert.ToInt32(datalist[nameof(FreeRose_Times)]);
            FreeRose_Win = Convert.ToDouble(datalist[nameof(FreeRose_Win)]);
            RoseHit_Times = Convert.ToInt32(datalist[nameof(RoseHit_Times)]);
            IndepTotalRoundTimes = Convert.ToInt32(datalist[nameof(IndepTotalRoundTimes)]);
            IndepTotalWinTimes = Convert.ToInt32(datalist[nameof(IndepTotalWinTimes)]);
            IndepFreeRose_Times = Convert.ToInt32(datalist[nameof(IndepFreeRose_Times)]);
            IndepFreeRose_Win = Convert.ToDouble(datalist[nameof(IndepFreeRose_Win)]);
        }
    }
}
