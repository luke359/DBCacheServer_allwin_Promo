using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class FortuneNeko3AccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int Scatter4_Times;
        private int Scatter5_Times;
        private int Scatter6_Times;
        private int Scatter7_Times;
        private int FreeFree_Times;
        private int FreeRespin1_Times;
        private int FreeRespin2_Times;
        private int FreeRespin3_Times;
        private int FreeRespin5_Times;
        private int Respin1_Times;
        private int Respin2_Times;
        private int Respin3_Times;
        private int Respin5_Times;
        private int FortuneCat6_Times;
        private double FortuneCat6_Bet;
        private double FortuneCat6_Win;
        private int FortuneCat5_Times;
        private double FortuneCat5_Bet;
        private double FortuneCat5_Win;
        private int FortuneCat4_Times;
        private double FortuneCat4_Bet;
        private double FortuneCat4_Win;
        private int FortuneCat3_Times;
        private double FortuneCat3_Bet;
        private double FortuneCat3_Win;
        private int FortuneCat_Times;
        private int FreeFortuneCat_Times;
        private int Drum6_Times;
        private double Drum6_Bet;
        private double Drum6_Win;
        private int Drum5_Times;
        private double Drum5_Bet;
        private double Drum5_Win;
        private int Drum4_Times;
        private double Drum4_Bet;
        private double Drum4_Win;
        private int Drum3_Times;
        private double Drum3_Bet;
        private double Drum3_Win;
        private int Lantern6_Times;
        private double Lantern6_Bet;
        private double Lantern6_Win;
        private int Lantern5_Times;
        private double Lantern5_Bet;
        private double Lantern5_Win;
        private int Lantern4_Times;
        private double Lantern4_Bet;
        private double Lantern4_Win;
        private int Lantern3_Times;
        private double Lantern3_Bet;
        private double Lantern3_Win;
        private int Fan6_Times;
        private double Fan6_Bet;
        private double Fan6_Win;
        private int Fan5_Times;
        private double Fan5_Bet;
        private double Fan5_Win;
        private int Fan4_Times;
        private double Fan4_Bet;
        private double Fan4_Win;
        private int Fan3_Times;
        private double Fan3_Bet;
        private double Fan3_Win;
        private int SashimiSushi6_Times;
        private double SashimiSushi6_Bet;
        private double SashimiSushi6_Win;
        private int SashimiSushi5_Times;
        private double SashimiSushi5_Bet;
        private double SashimiSushi5_Win;
        private int SashimiSushi4_Times;
        private double SashimiSushi4_Bet;
        private double SashimiSushi4_Win;
        private int SashimiSushi3_Times;
        private double SashimiSushi3_Bet;
        private double SashimiSushi3_Win;
        private int SeaweedSushi6_Times;
        private double SeaweedSushi6_Bet;
        private double SeaweedSushi6_Win;
        private int SeaweedSushi5_Times;
        private double SeaweedSushi5_Bet;
        private double SeaweedSushi5_Win;
        private int SeaweedSushi4_Times;
        private double SeaweedSushi4_Bet;
        private double SeaweedSushi4_Win;
        private int SeaweedSushi3_Times;
        private double SeaweedSushi3_Bet;
        private double SeaweedSushi3_Win;
        private int A6_Times;
        private double A6_Bet;
        private double A6_Win;
        private int A5_Times;
        private double A5_Bet;
        private double A5_Win;
        private int A4_Times;
        private double A4_Bet;
        private double A4_Win;
        private int A3_Times;
        private double A3_Bet;
        private double A3_Win;
        private int K6_Times;
        private double K6_Bet;
        private double K6_Win;
        private int K5_Times;
        private double K5_Bet;
        private double K5_Win;
        private int K4_Times;
        private double K4_Bet;
        private double K4_Win;
        private int K3_Times;
        private double K3_Bet;
        private double K3_Win;
        private int Q6_Times;
        private double Q6_Bet;
        private double Q6_Win;
        private int Q5_Times;
        private double Q5_Bet;
        private double Q5_Win;
        private int Q4_Times;
        private double Q4_Bet;
        private double Q4_Win;
        private int Q3_Times;
        private double Q3_Bet;
        private double Q3_Win;
        private int J6_Times;
        private double J6_Bet;
        private double J6_Win;
        private int J5_Times;
        private double J5_Bet;
        private double J5_Win;
        private int J4_Times;
        private double J4_Bet;
        private double J4_Win;
        private int J3_Times;
        private double J3_Bet;
        private double J3_Win;
        private int Ten6_Times;
        private double Ten6_Bet;
        private double Ten6_Win;
        private int Ten5_Times;
        private double Ten5_Bet;
        private double Ten5_Win;
        private int Ten4_Times;
        private double Ten4_Bet;
        private double Ten4_Win;
        private int Ten3_Times;
        private double Ten3_Bet;
        private double Ten3_Win;

        private int ExPlay05_Times;
        private double ExPlay05_Bet;
        private double ExPlay05_Win;
        private double ExPlay05_WinTimes;

        private int IndepPlayTimes;
        private double IndepTotalBet;
        private double IndepTotalWin;
        private int IndepRoundTimes;
        private int IndepWinTimes;
        private int IndepFreeFree_Times;
        private int IndepFreeRespin1_Times;
        private int IndepFreeRespin2_Times;
        private int IndepFreeRespin3_Times;
        private int IndepFreeRespin5_Times;
        #endregion

        /// <summary>清除額外押注</summary>
        public override void ClearCacheGameExPlay()
        {
            ExPlay05_Times = 0;
            ExPlay05_Bet = 0;
            ExPlay05_Win = 0;
            ExPlay05_WinTimes = 0;
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
                Scatter4_Times = 0;
                Scatter5_Times = 0;
                Scatter6_Times = 0;
                Scatter7_Times = 0;
                FreeFree_Times = 0;
                FreeRespin1_Times = 0;
                FreeRespin2_Times = 0;
                FreeRespin3_Times = 0;
                FreeRespin5_Times = 0;
                Respin1_Times = 0;
                Respin2_Times = 0;
                Respin3_Times = 0;
                Respin5_Times = 0;
                FortuneCat6_Times = 0;
                FortuneCat6_Bet = 0;
                FortuneCat6_Win = 0;
                FortuneCat5_Times = 0;
                FortuneCat5_Bet = 0;
                FortuneCat5_Win = 0;
                FortuneCat4_Times = 0;
                FortuneCat4_Bet = 0;
                FortuneCat4_Win = 0;
                FortuneCat3_Times = 0;
                FortuneCat3_Bet = 0;
                FortuneCat3_Win = 0;
                FortuneCat_Times = 0;
                FreeFortuneCat_Times = 0;
                Drum6_Times = 0;
                Drum6_Bet = 0;
                Drum6_Win = 0;
                Drum5_Times = 0;
                Drum5_Bet = 0;
                Drum5_Win = 0;
                Drum4_Times = 0;
                Drum4_Bet = 0;
                Drum4_Win = 0;
                Drum3_Times = 0;
                Drum3_Bet = 0;
                Drum3_Win = 0;
                Lantern6_Times = 0;
                Lantern6_Bet = 0;
                Lantern6_Win = 0;
                Lantern5_Times = 0;
                Lantern5_Bet = 0;
                Lantern5_Win = 0;
                Lantern4_Times = 0;
                Lantern4_Bet = 0;
                Lantern4_Win = 0;
                Lantern3_Times = 0;
                Lantern3_Bet = 0;
                Lantern3_Win = 0;
                Fan6_Times = 0;
                Fan6_Bet = 0;
                Fan6_Win = 0;
                Fan5_Times = 0;
                Fan5_Bet = 0;
                Fan5_Win = 0;
                Fan4_Times = 0;
                Fan4_Bet = 0;
                Fan4_Win = 0;
                Fan3_Times = 0;
                Fan3_Bet = 0;
                Fan3_Win = 0;
                SashimiSushi6_Times = 0;
                SashimiSushi6_Bet = 0;
                SashimiSushi6_Win = 0;
                SashimiSushi5_Times = 0;
                SashimiSushi5_Bet = 0;
                SashimiSushi5_Win = 0;
                SashimiSushi4_Times = 0;
                SashimiSushi4_Bet = 0;
                SashimiSushi4_Win = 0;
                SashimiSushi3_Times = 0;
                SashimiSushi3_Bet = 0;
                SashimiSushi3_Win = 0;
                SeaweedSushi6_Times = 0;
                SeaweedSushi6_Bet = 0;
                SeaweedSushi6_Win = 0;
                SeaweedSushi5_Times = 0;
                SeaweedSushi5_Bet = 0;
                SeaweedSushi5_Win = 0;
                SeaweedSushi4_Times = 0;
                SeaweedSushi4_Bet = 0;
                SeaweedSushi4_Win = 0;
                SeaweedSushi3_Times = 0;
                SeaweedSushi3_Bet = 0;
                SeaweedSushi3_Win = 0;
                A6_Times = 0;
                A6_Bet = 0;
                A6_Win = 0;
                A5_Times = 0;
                A5_Bet = 0;
                A5_Win = 0;
                A4_Times = 0;
                A4_Bet = 0;
                A4_Win = 0;
                A3_Times = 0;
                A3_Bet = 0;
                A3_Win = 0;
                K6_Times = 0;
                K6_Bet = 0;
                K6_Win = 0;
                K5_Times = 0;
                K5_Bet = 0;
                K5_Win = 0;
                K4_Times = 0;
                K4_Bet = 0;
                K4_Win = 0;
                K3_Times = 0;
                K3_Bet = 0;
                K3_Win = 0;
                Q6_Times = 0;
                Q6_Bet = 0;
                Q6_Win = 0;
                Q5_Times = 0;
                Q5_Bet = 0;
                Q5_Win = 0;
                Q4_Times = 0;
                Q4_Bet = 0;
                Q4_Win = 0;
                Q3_Times = 0;
                Q3_Bet = 0;
                Q3_Win = 0;
                J6_Times = 0;
                J6_Bet = 0;
                J6_Win = 0;
                J5_Times = 0;
                J5_Bet = 0;
                J5_Win = 0;
                J4_Times = 0;
                J4_Bet = 0;
                J4_Win = 0;
                J3_Times = 0;
                J3_Bet = 0;
                J3_Win = 0;
                Ten6_Times = 0;
                Ten6_Bet = 0;
                Ten6_Win = 0;
                Ten5_Times = 0;
                Ten5_Bet = 0;
                Ten5_Win = 0;
                Ten4_Times = 0;
                Ten4_Bet = 0;
                Ten4_Win = 0;
                Ten3_Times = 0;
                Ten3_Bet = 0;
                Ten3_Win = 0;
            }
            if (mode == 1 || mode == 2)
            {
                IndepPlayTimes = 0;
                IndepTotalBet = 0;
                IndepTotalWin = 0;
                IndepRoundTimes = 0;
                IndepWinTimes = 0;
                IndepFreeFree_Times = 0;
                IndepFreeRespin1_Times = 0;
                IndepFreeRespin2_Times = 0;
                IndepFreeRespin3_Times = 0;
                IndepFreeRespin5_Times = 0;
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(Free_Times)) { Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Free_Bet)) { Free_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Free_Win)) { Free_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Times)) { Scatter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter5_Times)) { Scatter5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter6_Times)) { Scatter6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter7_Times)) { Scatter7_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeFree_Times)) { FreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin1_Times)) { FreeRespin1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin2_Times)) { FreeRespin2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin3_Times)) { FreeRespin3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin5_Times)) { FreeRespin5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin1_Times)) { Respin1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin2_Times)) { Respin2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin3_Times)) { Respin3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin5_Times)) { Respin5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FortuneCat6_Times)) { FortuneCat6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FortuneCat6_Bet)) { FortuneCat6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat6_Win)) { FortuneCat6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat5_Times)) { FortuneCat5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FortuneCat5_Bet)) { FortuneCat5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat5_Win)) { FortuneCat5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat4_Times)) { FortuneCat4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FortuneCat4_Bet)) { FortuneCat4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat4_Win)) { FortuneCat4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat3_Times)) { FortuneCat3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FortuneCat3_Bet)) { FortuneCat3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat3_Win)) { FortuneCat3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FortuneCat_Times)) { FortuneCat_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeFortuneCat_Times)) { FreeFortuneCat_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum6_Times)) { Drum6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum6_Bet)) { Drum6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Drum6_Win)) { Drum6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Drum5_Times)) { Drum5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum5_Bet)) { Drum5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Drum5_Win)) { Drum5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Drum4_Times)) { Drum4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum4_Bet)) { Drum4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Drum4_Win)) { Drum4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Drum3_Times)) { Drum3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum3_Bet)) { Drum3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Drum3_Win)) { Drum3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern6_Times)) { Lantern6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Lantern6_Bet)) { Lantern6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern6_Win)) { Lantern6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern5_Times)) { Lantern5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Lantern5_Bet)) { Lantern5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern5_Win)) { Lantern5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern4_Times)) { Lantern4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Lantern4_Bet)) { Lantern4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern4_Win)) { Lantern4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern3_Times)) { Lantern3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Lantern3_Bet)) { Lantern3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Lantern3_Win)) { Lantern3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan6_Times)) { Fan6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan6_Bet)) { Fan6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan6_Win)) { Fan6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan5_Times)) { Fan5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan5_Bet)) { Fan5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan5_Win)) { Fan5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan4_Times)) { Fan4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan4_Bet)) { Fan4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan4_Win)) { Fan4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan3_Times)) { Fan3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan3_Bet)) { Fan3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan3_Win)) { Fan3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi6_Times)) { SashimiSushi6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SashimiSushi6_Bet)) { SashimiSushi6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi6_Win)) { SashimiSushi6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi5_Times)) { SashimiSushi5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SashimiSushi5_Bet)) { SashimiSushi5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi5_Win)) { SashimiSushi5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi4_Times)) { SashimiSushi4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SashimiSushi4_Bet)) { SashimiSushi4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi4_Win)) { SashimiSushi4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi3_Times)) { SashimiSushi3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SashimiSushi3_Bet)) { SashimiSushi3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SashimiSushi3_Win)) { SashimiSushi3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi6_Times)) { SeaweedSushi6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SeaweedSushi6_Bet)) { SeaweedSushi6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi6_Win)) { SeaweedSushi6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi5_Times)) { SeaweedSushi5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SeaweedSushi5_Bet)) { SeaweedSushi5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi5_Win)) { SeaweedSushi5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi4_Times)) { SeaweedSushi4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SeaweedSushi4_Bet)) { SeaweedSushi4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi4_Win)) { SeaweedSushi4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi3_Times)) { SeaweedSushi3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SeaweedSushi3_Bet)) { SeaweedSushi3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SeaweedSushi3_Win)) { SeaweedSushi3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A6_Times)) { A6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A6_Bet)) { A6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A6_Win)) { A6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A5_Times)) { A5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A5_Bet)) { A5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A5_Win)) { A5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A4_Times)) { A4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A4_Bet)) { A4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A4_Win)) { A4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A3_Times)) { A3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A3_Bet)) { A3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A3_Win)) { A3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K6_Times)) { K6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K6_Bet)) { K6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K6_Win)) { K6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K5_Times)) { K5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K5_Bet)) { K5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K5_Win)) { K5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K4_Times)) { K4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K4_Bet)) { K4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K4_Win)) { K4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K3_Times)) { K3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K3_Bet)) { K3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K3_Win)) { K3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q6_Times)) { Q6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q6_Bet)) { Q6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q6_Win)) { Q6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q5_Times)) { Q5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q5_Bet)) { Q5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q5_Win)) { Q5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q4_Times)) { Q4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q4_Bet)) { Q4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q4_Win)) { Q4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q3_Times)) { Q3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q3_Bet)) { Q3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q3_Win)) { Q3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J6_Times)) { J6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J6_Bet)) { J6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J6_Win)) { J6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J5_Times)) { J5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J5_Bet)) { J5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J5_Win)) { J5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J4_Times)) { J4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J4_Bet)) { J4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J4_Win)) { J4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J3_Times)) { J3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J3_Bet)) { J3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J3_Win)) { J3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ten6_Times)) { Ten6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ten6_Bet)) { Ten6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ten6_Win)) { Ten6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ten5_Times)) { Ten5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ten5_Bet)) { Ten5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ten5_Win)) { Ten5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ten4_Times)) { Ten4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ten4_Bet)) { Ten4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ten4_Win)) { Ten4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ten3_Times)) { Ten3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ten3_Bet)) { Ten3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ten3_Win)) { Ten3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay05_Times)) { ExPlay05_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlay05_Bet)) { ExPlay05_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay05_Win)) { ExPlay05_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay05_WinTimes)) { ExPlay05_WinTimes += Convert.ToDouble(value); return; }
            if (field == nameof(IndepPlayTimes)) { IndepPlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalBet)) { IndepTotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepTotalWin)) { IndepTotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepRoundTimes)) { IndepRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepWinTimes)) { IndepWinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeFree_Times)) { IndepFreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeRespin1_Times)) { IndepFreeRespin1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeRespin2_Times)) { IndepFreeRespin2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeRespin3_Times)) { IndepFreeRespin3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeRespin5_Times)) { IndepFreeRespin5_Times += Convert.ToInt32(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(Free_Times), Free_Times.ToString());
            updata.Add(nameof(Free_Bet), Free_Bet.ToString());
            updata.Add(nameof(Free_Win), Free_Win.ToString());
            updata.Add(nameof(Scatter4_Times), Scatter4_Times.ToString());
            updata.Add(nameof(Scatter5_Times), Scatter5_Times.ToString());
            updata.Add(nameof(Scatter6_Times), Scatter6_Times.ToString());
            updata.Add(nameof(Scatter7_Times), Scatter7_Times.ToString());
            updata.Add(nameof(FreeFree_Times), FreeFree_Times.ToString());
            updata.Add(nameof(FreeRespin1_Times), FreeRespin1_Times.ToString());
            updata.Add(nameof(FreeRespin2_Times), FreeRespin2_Times.ToString());
            updata.Add(nameof(FreeRespin3_Times), FreeRespin3_Times.ToString());
            updata.Add(nameof(FreeRespin5_Times), FreeRespin5_Times.ToString());
            updata.Add(nameof(Respin1_Times), Respin1_Times.ToString());
            updata.Add(nameof(Respin2_Times), Respin2_Times.ToString());
            updata.Add(nameof(Respin3_Times), Respin3_Times.ToString());
            updata.Add(nameof(Respin5_Times), Respin5_Times.ToString());
            updata.Add(nameof(FortuneCat6_Times), FortuneCat6_Times.ToString());
            updata.Add(nameof(FortuneCat6_Bet), FortuneCat6_Bet.ToString());
            updata.Add(nameof(FortuneCat6_Win), FortuneCat6_Win.ToString());
            updata.Add(nameof(FortuneCat5_Times), FortuneCat5_Times.ToString());
            updata.Add(nameof(FortuneCat5_Bet), FortuneCat5_Bet.ToString());
            updata.Add(nameof(FortuneCat5_Win), FortuneCat5_Win.ToString());
            updata.Add(nameof(FortuneCat4_Times), FortuneCat4_Times.ToString());
            updata.Add(nameof(FortuneCat4_Bet), FortuneCat4_Bet.ToString());
            updata.Add(nameof(FortuneCat4_Win), FortuneCat4_Win.ToString());
            updata.Add(nameof(FortuneCat3_Times), FortuneCat3_Times.ToString());
            updata.Add(nameof(FortuneCat3_Bet), FortuneCat3_Bet.ToString());
            updata.Add(nameof(FortuneCat3_Win), FortuneCat3_Win.ToString());
            updata.Add(nameof(FortuneCat_Times), FortuneCat_Times.ToString());
            updata.Add(nameof(FreeFortuneCat_Times), FreeFortuneCat_Times.ToString());
            updata.Add(nameof(Drum6_Times), Drum6_Times.ToString());
            updata.Add(nameof(Drum6_Bet), Drum6_Bet.ToString());
            updata.Add(nameof(Drum6_Win), Drum6_Win.ToString());
            updata.Add(nameof(Drum5_Times), Drum5_Times.ToString());
            updata.Add(nameof(Drum5_Bet), Drum5_Bet.ToString());
            updata.Add(nameof(Drum5_Win), Drum5_Win.ToString());
            updata.Add(nameof(Drum4_Times), Drum4_Times.ToString());
            updata.Add(nameof(Drum4_Bet), Drum4_Bet.ToString());
            updata.Add(nameof(Drum4_Win), Drum4_Win.ToString());
            updata.Add(nameof(Drum3_Times), Drum3_Times.ToString());
            updata.Add(nameof(Drum3_Bet), Drum3_Bet.ToString());
            updata.Add(nameof(Drum3_Win), Drum3_Win.ToString());
            updata.Add(nameof(Lantern6_Times), Lantern6_Times.ToString());
            updata.Add(nameof(Lantern6_Bet), Lantern6_Bet.ToString());
            updata.Add(nameof(Lantern6_Win), Lantern6_Win.ToString());
            updata.Add(nameof(Lantern5_Times), Lantern5_Times.ToString());
            updata.Add(nameof(Lantern5_Bet), Lantern5_Bet.ToString());
            updata.Add(nameof(Lantern5_Win), Lantern5_Win.ToString());
            updata.Add(nameof(Lantern4_Times), Lantern4_Times.ToString());
            updata.Add(nameof(Lantern4_Bet), Lantern4_Bet.ToString());
            updata.Add(nameof(Lantern4_Win), Lantern4_Win.ToString());
            updata.Add(nameof(Lantern3_Times), Lantern3_Times.ToString());
            updata.Add(nameof(Lantern3_Bet), Lantern3_Bet.ToString());
            updata.Add(nameof(Lantern3_Win), Lantern3_Win.ToString());
            updata.Add(nameof(Fan6_Times), Fan6_Times.ToString());
            updata.Add(nameof(Fan6_Bet), Fan6_Bet.ToString());
            updata.Add(nameof(Fan6_Win), Fan6_Win.ToString());
            updata.Add(nameof(Fan5_Times), Fan5_Times.ToString());
            updata.Add(nameof(Fan5_Bet), Fan5_Bet.ToString());
            updata.Add(nameof(Fan5_Win), Fan5_Win.ToString());
            updata.Add(nameof(Fan4_Times), Fan4_Times.ToString());
            updata.Add(nameof(Fan4_Bet), Fan4_Bet.ToString());
            updata.Add(nameof(Fan4_Win), Fan4_Win.ToString());
            updata.Add(nameof(Fan3_Times), Fan3_Times.ToString());
            updata.Add(nameof(Fan3_Bet), Fan3_Bet.ToString());
            updata.Add(nameof(Fan3_Win), Fan3_Win.ToString());
            updata.Add(nameof(SashimiSushi6_Times), SashimiSushi6_Times.ToString());
            updata.Add(nameof(SashimiSushi6_Bet), SashimiSushi6_Bet.ToString());
            updata.Add(nameof(SashimiSushi6_Win), SashimiSushi6_Win.ToString());
            updata.Add(nameof(SashimiSushi5_Times), SashimiSushi5_Times.ToString());
            updata.Add(nameof(SashimiSushi5_Bet), SashimiSushi5_Bet.ToString());
            updata.Add(nameof(SashimiSushi5_Win), SashimiSushi5_Win.ToString());
            updata.Add(nameof(SashimiSushi4_Times), SashimiSushi4_Times.ToString());
            updata.Add(nameof(SashimiSushi4_Bet), SashimiSushi4_Bet.ToString());
            updata.Add(nameof(SashimiSushi4_Win), SashimiSushi4_Win.ToString());
            updata.Add(nameof(SashimiSushi3_Times), SashimiSushi3_Times.ToString());
            updata.Add(nameof(SashimiSushi3_Bet), SashimiSushi3_Bet.ToString());
            updata.Add(nameof(SashimiSushi3_Win), SashimiSushi3_Win.ToString());
            updata.Add(nameof(SeaweedSushi6_Times), SeaweedSushi6_Times.ToString());
            updata.Add(nameof(SeaweedSushi6_Bet), SeaweedSushi6_Bet.ToString());
            updata.Add(nameof(SeaweedSushi6_Win), SeaweedSushi6_Win.ToString());
            updata.Add(nameof(SeaweedSushi5_Times), SeaweedSushi5_Times.ToString());
            updata.Add(nameof(SeaweedSushi5_Bet), SeaweedSushi5_Bet.ToString());
            updata.Add(nameof(SeaweedSushi5_Win), SeaweedSushi5_Win.ToString());
            updata.Add(nameof(SeaweedSushi4_Times), SeaweedSushi4_Times.ToString());
            updata.Add(nameof(SeaweedSushi4_Bet), SeaweedSushi4_Bet.ToString());
            updata.Add(nameof(SeaweedSushi4_Win), SeaweedSushi4_Win.ToString());
            updata.Add(nameof(SeaweedSushi3_Times), SeaweedSushi3_Times.ToString());
            updata.Add(nameof(SeaweedSushi3_Bet), SeaweedSushi3_Bet.ToString());
            updata.Add(nameof(SeaweedSushi3_Win), SeaweedSushi3_Win.ToString());
            updata.Add(nameof(A6_Times), A6_Times.ToString());
            updata.Add(nameof(A6_Bet), A6_Bet.ToString());
            updata.Add(nameof(A6_Win), A6_Win.ToString());
            updata.Add(nameof(A5_Times), A5_Times.ToString());
            updata.Add(nameof(A5_Bet), A5_Bet.ToString());
            updata.Add(nameof(A5_Win), A5_Win.ToString());
            updata.Add(nameof(A4_Times), A4_Times.ToString());
            updata.Add(nameof(A4_Bet), A4_Bet.ToString());
            updata.Add(nameof(A4_Win), A4_Win.ToString());
            updata.Add(nameof(A3_Times), A3_Times.ToString());
            updata.Add(nameof(A3_Bet), A3_Bet.ToString());
            updata.Add(nameof(A3_Win), A3_Win.ToString());
            updata.Add(nameof(K6_Times), K6_Times.ToString());
            updata.Add(nameof(K6_Bet), K6_Bet.ToString());
            updata.Add(nameof(K6_Win), K6_Win.ToString());
            updata.Add(nameof(K5_Times), K5_Times.ToString());
            updata.Add(nameof(K5_Bet), K5_Bet.ToString());
            updata.Add(nameof(K5_Win), K5_Win.ToString());
            updata.Add(nameof(K4_Times), K4_Times.ToString());
            updata.Add(nameof(K4_Bet), K4_Bet.ToString());
            updata.Add(nameof(K4_Win), K4_Win.ToString());
            updata.Add(nameof(K3_Times), K3_Times.ToString());
            updata.Add(nameof(K3_Bet), K3_Bet.ToString());
            updata.Add(nameof(K3_Win), K3_Win.ToString());
            updata.Add(nameof(Q6_Times), Q6_Times.ToString());
            updata.Add(nameof(Q6_Bet), Q6_Bet.ToString());
            updata.Add(nameof(Q6_Win), Q6_Win.ToString());
            updata.Add(nameof(Q5_Times), Q5_Times.ToString());
            updata.Add(nameof(Q5_Bet), Q5_Bet.ToString());
            updata.Add(nameof(Q5_Win), Q5_Win.ToString());
            updata.Add(nameof(Q4_Times), Q4_Times.ToString());
            updata.Add(nameof(Q4_Bet), Q4_Bet.ToString());
            updata.Add(nameof(Q4_Win), Q4_Win.ToString());
            updata.Add(nameof(Q3_Times), Q3_Times.ToString());
            updata.Add(nameof(Q3_Bet), Q3_Bet.ToString());
            updata.Add(nameof(Q3_Win), Q3_Win.ToString());
            updata.Add(nameof(J6_Times), J6_Times.ToString());
            updata.Add(nameof(J6_Bet), J6_Bet.ToString());
            updata.Add(nameof(J6_Win), J6_Win.ToString());
            updata.Add(nameof(J5_Times), J5_Times.ToString());
            updata.Add(nameof(J5_Bet), J5_Bet.ToString());
            updata.Add(nameof(J5_Win), J5_Win.ToString());
            updata.Add(nameof(J4_Times), J4_Times.ToString());
            updata.Add(nameof(J4_Bet), J4_Bet.ToString());
            updata.Add(nameof(J4_Win), J4_Win.ToString());
            updata.Add(nameof(J3_Times), J3_Times.ToString());
            updata.Add(nameof(J3_Bet), J3_Bet.ToString());
            updata.Add(nameof(J3_Win), J3_Win.ToString());
            updata.Add(nameof(Ten6_Times), Ten6_Times.ToString());
            updata.Add(nameof(Ten6_Bet), Ten6_Bet.ToString());
            updata.Add(nameof(Ten6_Win), Ten6_Win.ToString());
            updata.Add(nameof(Ten5_Times), Ten5_Times.ToString());
            updata.Add(nameof(Ten5_Bet), Ten5_Bet.ToString());
            updata.Add(nameof(Ten5_Win), Ten5_Win.ToString());
            updata.Add(nameof(Ten4_Times), Ten4_Times.ToString());
            updata.Add(nameof(Ten4_Bet), Ten4_Bet.ToString());
            updata.Add(nameof(Ten4_Win), Ten4_Win.ToString());
            updata.Add(nameof(Ten3_Times), Ten3_Times.ToString());
            updata.Add(nameof(Ten3_Bet), Ten3_Bet.ToString());
            updata.Add(nameof(Ten3_Win), Ten3_Win.ToString());
            updata.Add(nameof(ExPlay05_Times), ExPlay05_Times.ToString());
            updata.Add(nameof(ExPlay05_Bet), ExPlay05_Bet.ToString());
            updata.Add(nameof(ExPlay05_Win), ExPlay05_Win.ToString());
            updata.Add(nameof(ExPlay05_WinTimes), ExPlay05_WinTimes.ToString());
            updata.Add(nameof(IndepPlayTimes), IndepPlayTimes.ToString());
            updata.Add(nameof(IndepTotalBet), IndepTotalBet.ToString());
            updata.Add(nameof(IndepTotalWin), IndepTotalWin.ToString());
            updata.Add(nameof(IndepRoundTimes), IndepRoundTimes.ToString());
            updata.Add(nameof(IndepWinTimes), IndepWinTimes.ToString());
            updata.Add(nameof(IndepFreeFree_Times), IndepFreeFree_Times.ToString());
            updata.Add(nameof(IndepFreeRespin1_Times), IndepFreeRespin1_Times.ToString());
            updata.Add(nameof(IndepFreeRespin2_Times), IndepFreeRespin2_Times.ToString());
            updata.Add(nameof(IndepFreeRespin3_Times), IndepFreeRespin3_Times.ToString());
            updata.Add(nameof(IndepFreeRespin5_Times), IndepFreeRespin5_Times.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            Free_Times = Convert.ToInt32(datalist[nameof(Free_Times)]);
            Free_Bet = Convert.ToDouble(datalist[nameof(Free_Bet)]);
            Free_Win = Convert.ToDouble(datalist[nameof(Free_Win)]);
            Scatter4_Times = Convert.ToInt32(datalist[nameof(Scatter4_Times)]);
            Scatter5_Times = Convert.ToInt32(datalist[nameof(Scatter5_Times)]);
            Scatter6_Times = Convert.ToInt32(datalist[nameof(Scatter6_Times)]);
            Scatter7_Times = Convert.ToInt32(datalist[nameof(Scatter7_Times)]);
            FreeFree_Times = Convert.ToInt32(datalist[nameof(FreeFree_Times)]);
            FreeRespin1_Times = Convert.ToInt32(datalist[nameof(FreeRespin1_Times)]);
            FreeRespin2_Times = Convert.ToInt32(datalist[nameof(FreeRespin2_Times)]);
            FreeRespin3_Times = Convert.ToInt32(datalist[nameof(FreeRespin3_Times)]);
            FreeRespin5_Times = Convert.ToInt32(datalist[nameof(FreeRespin5_Times)]);
            Respin1_Times = Convert.ToInt32(datalist[nameof(Respin1_Times)]);
            Respin2_Times = Convert.ToInt32(datalist[nameof(Respin2_Times)]);
            Respin3_Times = Convert.ToInt32(datalist[nameof(Respin3_Times)]);
            Respin5_Times = Convert.ToInt32(datalist[nameof(Respin5_Times)]);
            FortuneCat6_Times = Convert.ToInt32(datalist[nameof(FortuneCat6_Times)]);
            FortuneCat6_Bet = Convert.ToDouble(datalist[nameof(FortuneCat6_Bet)]);
            FortuneCat6_Win = Convert.ToDouble(datalist[nameof(FortuneCat6_Win)]);
            FortuneCat5_Times = Convert.ToInt32(datalist[nameof(FortuneCat5_Times)]);
            FortuneCat5_Bet = Convert.ToDouble(datalist[nameof(FortuneCat5_Bet)]);
            FortuneCat5_Win = Convert.ToDouble(datalist[nameof(FortuneCat5_Win)]);
            FortuneCat4_Times = Convert.ToInt32(datalist[nameof(FortuneCat4_Times)]);
            FortuneCat4_Bet = Convert.ToDouble(datalist[nameof(FortuneCat4_Bet)]);
            FortuneCat4_Win = Convert.ToDouble(datalist[nameof(FortuneCat4_Win)]);
            FortuneCat3_Times = Convert.ToInt32(datalist[nameof(FortuneCat3_Times)]);
            FortuneCat3_Bet = Convert.ToDouble(datalist[nameof(FortuneCat3_Bet)]);
            FortuneCat3_Win = Convert.ToDouble(datalist[nameof(FortuneCat3_Win)]);
            FortuneCat_Times = Convert.ToInt32(datalist[nameof(FortuneCat_Times)]);
            FreeFortuneCat_Times = Convert.ToInt32(datalist[nameof(FreeFortuneCat_Times)]);
            Drum6_Times = Convert.ToInt32(datalist[nameof(Drum6_Times)]);
            Drum6_Bet = Convert.ToDouble(datalist[nameof(Drum6_Bet)]);
            Drum6_Win = Convert.ToDouble(datalist[nameof(Drum6_Win)]);
            Drum5_Times = Convert.ToInt32(datalist[nameof(Drum5_Times)]);
            Drum5_Bet = Convert.ToDouble(datalist[nameof(Drum5_Bet)]);
            Drum5_Win = Convert.ToDouble(datalist[nameof(Drum5_Win)]);
            Drum4_Times = Convert.ToInt32(datalist[nameof(Drum4_Times)]);
            Drum4_Bet = Convert.ToDouble(datalist[nameof(Drum4_Bet)]);
            Drum4_Win = Convert.ToDouble(datalist[nameof(Drum4_Win)]);
            Drum3_Times = Convert.ToInt32(datalist[nameof(Drum3_Times)]);
            Drum3_Bet = Convert.ToDouble(datalist[nameof(Drum3_Bet)]);
            Drum3_Win = Convert.ToDouble(datalist[nameof(Drum3_Win)]);
            Lantern6_Times = Convert.ToInt32(datalist[nameof(Lantern6_Times)]);
            Lantern6_Bet = Convert.ToDouble(datalist[nameof(Lantern6_Bet)]);
            Lantern6_Win = Convert.ToDouble(datalist[nameof(Lantern6_Win)]);
            Lantern5_Times = Convert.ToInt32(datalist[nameof(Lantern5_Times)]);
            Lantern5_Bet = Convert.ToDouble(datalist[nameof(Lantern5_Bet)]);
            Lantern5_Win = Convert.ToDouble(datalist[nameof(Lantern5_Win)]);
            Lantern4_Times = Convert.ToInt32(datalist[nameof(Lantern4_Times)]);
            Lantern4_Bet = Convert.ToDouble(datalist[nameof(Lantern4_Bet)]);
            Lantern4_Win = Convert.ToDouble(datalist[nameof(Lantern4_Win)]);
            Lantern3_Times = Convert.ToInt32(datalist[nameof(Lantern3_Times)]);
            Lantern3_Bet = Convert.ToDouble(datalist[nameof(Lantern3_Bet)]);
            Lantern3_Win = Convert.ToDouble(datalist[nameof(Lantern3_Win)]);
            Fan6_Times = Convert.ToInt32(datalist[nameof(Fan6_Times)]);
            Fan6_Bet = Convert.ToDouble(datalist[nameof(Fan6_Bet)]);
            Fan6_Win = Convert.ToDouble(datalist[nameof(Fan6_Win)]);
            Fan5_Times = Convert.ToInt32(datalist[nameof(Fan5_Times)]);
            Fan5_Bet = Convert.ToDouble(datalist[nameof(Fan5_Bet)]);
            Fan5_Win = Convert.ToDouble(datalist[nameof(Fan5_Win)]);
            Fan4_Times = Convert.ToInt32(datalist[nameof(Fan4_Times)]);
            Fan4_Bet = Convert.ToDouble(datalist[nameof(Fan4_Bet)]);
            Fan4_Win = Convert.ToDouble(datalist[nameof(Fan4_Win)]);
            Fan3_Times = Convert.ToInt32(datalist[nameof(Fan3_Times)]);
            Fan3_Bet = Convert.ToDouble(datalist[nameof(Fan3_Bet)]);
            Fan3_Win = Convert.ToDouble(datalist[nameof(Fan3_Win)]);
            SashimiSushi6_Times = Convert.ToInt32(datalist[nameof(SashimiSushi6_Times)]);
            SashimiSushi6_Bet = Convert.ToDouble(datalist[nameof(SashimiSushi6_Bet)]);
            SashimiSushi6_Win = Convert.ToDouble(datalist[nameof(SashimiSushi6_Win)]);
            SashimiSushi5_Times = Convert.ToInt32(datalist[nameof(SashimiSushi5_Times)]);
            SashimiSushi5_Bet = Convert.ToDouble(datalist[nameof(SashimiSushi5_Bet)]);
            SashimiSushi5_Win = Convert.ToDouble(datalist[nameof(SashimiSushi5_Win)]);
            SashimiSushi4_Times = Convert.ToInt32(datalist[nameof(SashimiSushi4_Times)]);
            SashimiSushi4_Bet = Convert.ToDouble(datalist[nameof(SashimiSushi4_Bet)]);
            SashimiSushi4_Win = Convert.ToDouble(datalist[nameof(SashimiSushi4_Win)]);
            SashimiSushi3_Times = Convert.ToInt32(datalist[nameof(SashimiSushi3_Times)]);
            SashimiSushi3_Bet = Convert.ToDouble(datalist[nameof(SashimiSushi3_Bet)]);
            SashimiSushi3_Win = Convert.ToDouble(datalist[nameof(SashimiSushi3_Win)]);
            SeaweedSushi6_Times = Convert.ToInt32(datalist[nameof(SeaweedSushi6_Times)]);
            SeaweedSushi6_Bet = Convert.ToDouble(datalist[nameof(SeaweedSushi6_Bet)]);
            SeaweedSushi6_Win = Convert.ToDouble(datalist[nameof(SeaweedSushi6_Win)]);
            SeaweedSushi5_Times = Convert.ToInt32(datalist[nameof(SeaweedSushi5_Times)]);
            SeaweedSushi5_Bet = Convert.ToDouble(datalist[nameof(SeaweedSushi5_Bet)]);
            SeaweedSushi5_Win = Convert.ToDouble(datalist[nameof(SeaweedSushi5_Win)]);
            SeaweedSushi4_Times = Convert.ToInt32(datalist[nameof(SeaweedSushi4_Times)]);
            SeaweedSushi4_Bet = Convert.ToDouble(datalist[nameof(SeaweedSushi4_Bet)]);
            SeaweedSushi4_Win = Convert.ToDouble(datalist[nameof(SeaweedSushi4_Win)]);
            SeaweedSushi3_Times = Convert.ToInt32(datalist[nameof(SeaweedSushi3_Times)]);
            SeaweedSushi3_Bet = Convert.ToDouble(datalist[nameof(SeaweedSushi3_Bet)]);
            SeaweedSushi3_Win = Convert.ToDouble(datalist[nameof(SeaweedSushi3_Win)]);
            A6_Times = Convert.ToInt32(datalist[nameof(A6_Times)]);
            A6_Bet = Convert.ToDouble(datalist[nameof(A6_Bet)]);
            A6_Win = Convert.ToDouble(datalist[nameof(A6_Win)]);
            A5_Times = Convert.ToInt32(datalist[nameof(A5_Times)]);
            A5_Bet = Convert.ToDouble(datalist[nameof(A5_Bet)]);
            A5_Win = Convert.ToDouble(datalist[nameof(A5_Win)]);
            A4_Times = Convert.ToInt32(datalist[nameof(A4_Times)]);
            A4_Bet = Convert.ToDouble(datalist[nameof(A4_Bet)]);
            A4_Win = Convert.ToDouble(datalist[nameof(A4_Win)]);
            A3_Times = Convert.ToInt32(datalist[nameof(A3_Times)]);
            A3_Bet = Convert.ToDouble(datalist[nameof(A3_Bet)]);
            A3_Win = Convert.ToDouble(datalist[nameof(A3_Win)]);
            K6_Times = Convert.ToInt32(datalist[nameof(K6_Times)]);
            K6_Bet = Convert.ToDouble(datalist[nameof(K6_Bet)]);
            K6_Win = Convert.ToDouble(datalist[nameof(K6_Win)]);
            K5_Times = Convert.ToInt32(datalist[nameof(K5_Times)]);
            K5_Bet = Convert.ToDouble(datalist[nameof(K5_Bet)]);
            K5_Win = Convert.ToDouble(datalist[nameof(K5_Win)]);
            K4_Times = Convert.ToInt32(datalist[nameof(K4_Times)]);
            K4_Bet = Convert.ToDouble(datalist[nameof(K4_Bet)]);
            K4_Win = Convert.ToDouble(datalist[nameof(K4_Win)]);
            K3_Times = Convert.ToInt32(datalist[nameof(K3_Times)]);
            K3_Bet = Convert.ToDouble(datalist[nameof(K3_Bet)]);
            K3_Win = Convert.ToDouble(datalist[nameof(K3_Win)]);
            Q6_Times = Convert.ToInt32(datalist[nameof(Q6_Times)]);
            Q6_Bet = Convert.ToDouble(datalist[nameof(Q6_Bet)]);
            Q6_Win = Convert.ToDouble(datalist[nameof(Q6_Win)]);
            Q5_Times = Convert.ToInt32(datalist[nameof(Q5_Times)]);
            Q5_Bet = Convert.ToDouble(datalist[nameof(Q5_Bet)]);
            Q5_Win = Convert.ToDouble(datalist[nameof(Q5_Win)]);
            Q4_Times = Convert.ToInt32(datalist[nameof(Q4_Times)]);
            Q4_Bet = Convert.ToDouble(datalist[nameof(Q4_Bet)]);
            Q4_Win = Convert.ToDouble(datalist[nameof(Q4_Win)]);
            Q3_Times = Convert.ToInt32(datalist[nameof(Q3_Times)]);
            Q3_Bet = Convert.ToDouble(datalist[nameof(Q3_Bet)]);
            Q3_Win = Convert.ToDouble(datalist[nameof(Q3_Win)]);
            J6_Times = Convert.ToInt32(datalist[nameof(J6_Times)]);
            J6_Bet = Convert.ToDouble(datalist[nameof(J6_Bet)]);
            J6_Win = Convert.ToDouble(datalist[nameof(J6_Win)]);
            J5_Times = Convert.ToInt32(datalist[nameof(J5_Times)]);
            J5_Bet = Convert.ToDouble(datalist[nameof(J5_Bet)]);
            J5_Win = Convert.ToDouble(datalist[nameof(J5_Win)]);
            J4_Times = Convert.ToInt32(datalist[nameof(J4_Times)]);
            J4_Bet = Convert.ToDouble(datalist[nameof(J4_Bet)]);
            J4_Win = Convert.ToDouble(datalist[nameof(J4_Win)]);
            J3_Times = Convert.ToInt32(datalist[nameof(J3_Times)]);
            J3_Bet = Convert.ToDouble(datalist[nameof(J3_Bet)]);
            J3_Win = Convert.ToDouble(datalist[nameof(J3_Win)]);
            Ten6_Times = Convert.ToInt32(datalist[nameof(Ten6_Times)]);
            Ten6_Bet = Convert.ToDouble(datalist[nameof(Ten6_Bet)]);
            Ten6_Win = Convert.ToDouble(datalist[nameof(Ten6_Win)]);
            Ten5_Times = Convert.ToInt32(datalist[nameof(Ten5_Times)]);
            Ten5_Bet = Convert.ToDouble(datalist[nameof(Ten5_Bet)]);
            Ten5_Win = Convert.ToDouble(datalist[nameof(Ten5_Win)]);
            Ten4_Times = Convert.ToInt32(datalist[nameof(Ten4_Times)]);
            Ten4_Bet = Convert.ToDouble(datalist[nameof(Ten4_Bet)]);
            Ten4_Win = Convert.ToDouble(datalist[nameof(Ten4_Win)]);
            Ten3_Times = Convert.ToInt32(datalist[nameof(Ten3_Times)]);
            Ten3_Bet = Convert.ToDouble(datalist[nameof(Ten3_Bet)]);
            Ten3_Win = Convert.ToDouble(datalist[nameof(Ten3_Win)]);
            ExPlay05_Times = Convert.ToInt32(datalist[nameof(ExPlay05_Times)]);
            ExPlay05_Bet = Convert.ToDouble(datalist[nameof(ExPlay05_Bet)]);
            ExPlay05_Win = Convert.ToDouble(datalist[nameof(ExPlay05_Win)]);
            ExPlay05_WinTimes = Convert.ToDouble(datalist[nameof(ExPlay05_WinTimes)]);
            IndepPlayTimes = Convert.ToInt32(datalist[nameof(IndepPlayTimes)]);
            IndepTotalBet = Convert.ToDouble(datalist[nameof(IndepTotalBet)]);
            IndepTotalWin = Convert.ToDouble(datalist[nameof(IndepTotalWin)]);
            IndepRoundTimes = Convert.ToInt32(datalist[nameof(IndepRoundTimes)]);
            IndepWinTimes = Convert.ToInt32(datalist[nameof(IndepWinTimes)]);
            IndepFreeFree_Times = Convert.ToInt32(datalist[nameof(IndepFreeFree_Times)]);
            IndepFreeRespin1_Times = Convert.ToInt32(datalist[nameof(IndepFreeRespin1_Times)]);
            IndepFreeRespin2_Times = Convert.ToInt32(datalist[nameof(IndepFreeRespin2_Times)]);
            IndepFreeRespin3_Times = Convert.ToInt32(datalist[nameof(IndepFreeRespin3_Times)]);
            IndepFreeRespin5_Times = Convert.ToInt32(datalist[nameof(IndepFreeRespin5_Times)]);
        }
    }
}
