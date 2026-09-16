using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class MahjongWinsSuperScatterAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int FreeFree_Times;
        private int FreeRespin1_Times;
        private int FreeRespin2_Times;
        private int FreeRespin3_Times;
        private int FreeRespin5_Times;
        private int Respin1_Times;
        private int Respin2_Times;
        private int Respin3_Times;
        private int Respin5_Times;
        private int SuperScatter1_Times;
        private double SuperScatter1_Bet;
        private double SuperScatter1_Win;
        private int SuperScatter2_Times;
        private double SuperScatter2_Bet;
        private double SuperScatter2_Win;
        private int SuperScatter3_Times;
        private double SuperScatter3_Bet;
        private double SuperScatter3_Win;
        private int SuperScatter4_Times;
        private double SuperScatter4_Bet;
        private double SuperScatter4_Win;
        private int RedDragon5_Times;
        private double RedDragon5_Bet;
        private double RedDragon5_Win;
        private int RedDragon4_Times;
        private double RedDragon4_Bet;
        private double RedDragon4_Win;
        private int RedDragon3_Times;
        private double RedDragon3_Bet;
        private double RedDragon3_Win;
        private int GreenDragon5_Times;
        private double GreenDragon5_Bet;
        private double GreenDragon5_Win;
        private int GreenDragon4_Times;
        private double GreenDragon4_Bet;
        private double GreenDragon4_Win;
        private int GreenDragon3_Times;
        private double GreenDragon3_Bet;
        private double GreenDragon3_Win;
        private int WhiteDragon5_Times;
        private double WhiteDragon5_Bet;
        private double WhiteDragon5_Win;
        private int WhiteDragon4_Times;
        private double WhiteDragon4_Bet;
        private double WhiteDragon4_Win;
        private int WhiteDragon3_Times;
        private double WhiteDragon3_Bet;
        private double WhiteDragon3_Win;
        private int EightCharacter5_Times;
        private double EightCharacter5_Bet;
        private double EightCharacter5_Win;
        private int EightCharacter4_Times;
        private double EightCharacter4_Bet;
        private double EightCharacter4_Win;
        private int EightCharacter3_Times;
        private double EightCharacter3_Bet;
        private double EightCharacter3_Win;
        private int FiveDots5_Times;
        private double FiveDots5_Bet;
        private double FiveDots5_Win;
        private int FiveDots4_Times;
        private double FiveDots4_Bet;
        private double FiveDots4_Win;
        private int FiveDots3_Times;
        private double FiveDots3_Bet;
        private double FiveDots3_Win;
        private int ThreeDots5_Times;
        private double ThreeDots5_Bet;
        private double ThreeDots5_Win;
        private int ThreeDots4_Times;
        private double ThreeDots4_Bet;
        private double ThreeDots4_Win;
        private int ThreeDots3_Times;
        private double ThreeDots3_Bet;
        private double ThreeDots3_Win;
        private int TwoDots5_Times;
        private double TwoDots5_Bet;
        private double TwoDots5_Win;
        private int TwoDots4_Times;
        private double TwoDots4_Bet;
        private double TwoDots4_Win;
        private int TwoDots3_Times;
        private double TwoDots3_Bet;
        private double TwoDots3_Win;
        private int FiveSticks5_Times;
        private double FiveSticks5_Bet;
        private double FiveSticks5_Win;
        private int FiveSticks4_Times;
        private double FiveSticks4_Bet;
        private double FiveSticks4_Win;
        private int FiveSticks3_Times;
        private double FiveSticks3_Bet;
        private double FiveSticks3_Win;
        private int TwoSticks5_Times;
        private double TwoSticks5_Bet;
        private double TwoSticks5_Win;
        private int TwoSticks4_Times;
        private double TwoSticks4_Bet;
        private double TwoSticks4_Win;
        private int TwoSticks3_Times;
        private double TwoSticks3_Bet;
        private double TwoSticks3_Win;
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
        private int IndepSuperScatter1_Times;
        private double IndepSuperScatter1_Bet;
        private double IndepSuperScatter1_Win;
        private int IndepSuperScatter2_Times;
        private double IndepSuperScatter2_Bet;
        private double IndepSuperScatter2_Win;
        private int IndepSuperScatter3_Times;
        private double IndepSuperScatter3_Bet;
        private double IndepSuperScatter3_Win;
        private int IndepSuperScatter4_Times;
        private double IndepSuperScatter4_Bet;
        private double IndepSuperScatter4_Win;
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
                FreeFree_Times = 0;
                FreeRespin1_Times = 0;
                FreeRespin2_Times = 0;
                FreeRespin3_Times = 0;
                FreeRespin5_Times = 0;
                Respin1_Times = 0;
                Respin2_Times = 0;
                Respin3_Times = 0;
                Respin5_Times = 0;
                SuperScatter1_Times = 0;
                SuperScatter1_Bet = 0;
                SuperScatter1_Win = 0;
                SuperScatter2_Times = 0;
                SuperScatter2_Bet = 0;
                SuperScatter2_Win = 0;
                SuperScatter3_Times = 0;
                SuperScatter3_Bet = 0;
                SuperScatter3_Win = 0;
                SuperScatter4_Times = 0;
                SuperScatter4_Bet = 0;
                SuperScatter4_Win = 0;
                RedDragon5_Times = 0;
                RedDragon5_Bet = 0;
                RedDragon5_Win = 0;
                RedDragon4_Times = 0;
                RedDragon4_Bet = 0;
                RedDragon4_Win = 0;
                RedDragon3_Times = 0;
                RedDragon3_Bet = 0;
                RedDragon3_Win = 0;
                GreenDragon5_Times = 0;
                GreenDragon5_Bet = 0;
                GreenDragon5_Win = 0;
                GreenDragon4_Times = 0;
                GreenDragon4_Bet = 0;
                GreenDragon4_Win = 0;
                GreenDragon3_Times = 0;
                GreenDragon3_Bet = 0;
                GreenDragon3_Win = 0;
                WhiteDragon5_Times = 0;
                WhiteDragon5_Bet = 0;
                WhiteDragon5_Win = 0;
                WhiteDragon4_Times = 0;
                WhiteDragon4_Bet = 0;
                WhiteDragon4_Win = 0;
                WhiteDragon3_Times = 0;
                WhiteDragon3_Bet = 0;
                WhiteDragon3_Win = 0;
                EightCharacter5_Times = 0;
                EightCharacter5_Bet = 0;
                EightCharacter5_Win = 0;
                EightCharacter4_Times = 0;
                EightCharacter4_Bet = 0;
                EightCharacter4_Win = 0;
                EightCharacter3_Times = 0;
                EightCharacter3_Bet = 0;
                EightCharacter3_Win = 0;
                FiveDots5_Times = 0;
                FiveDots5_Bet = 0;
                FiveDots5_Win = 0;
                FiveDots4_Times = 0;
                FiveDots4_Bet = 0;
                FiveDots4_Win = 0;
                FiveDots3_Times = 0;
                FiveDots3_Bet = 0;
                FiveDots3_Win = 0;
                ThreeDots5_Times = 0;
                ThreeDots5_Bet = 0;
                ThreeDots5_Win = 0;
                ThreeDots4_Times = 0;
                ThreeDots4_Bet = 0;
                ThreeDots4_Win = 0;
                ThreeDots3_Times = 0;
                ThreeDots3_Bet = 0;
                ThreeDots3_Win = 0;
                TwoDots5_Times = 0;
                TwoDots5_Bet = 0;
                TwoDots5_Win = 0;
                TwoDots4_Times = 0;
                TwoDots4_Bet = 0;
                TwoDots4_Win = 0;
                TwoDots3_Times = 0;
                TwoDots3_Bet = 0;
                TwoDots3_Win = 0;
                FiveSticks5_Times = 0;
                FiveSticks5_Bet = 0;
                FiveSticks5_Win = 0;
                FiveSticks4_Times = 0;
                FiveSticks4_Bet = 0;
                FiveSticks4_Win = 0;
                FiveSticks3_Times = 0;
                FiveSticks3_Bet = 0;
                FiveSticks3_Win = 0;
                TwoSticks5_Times = 0;
                TwoSticks5_Bet = 0;
                TwoSticks5_Win = 0;
                TwoSticks4_Times = 0;
                TwoSticks4_Bet = 0;
                TwoSticks4_Win = 0;
                TwoSticks3_Times = 0;
                TwoSticks3_Bet = 0;
                TwoSticks3_Win = 0;
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
                IndepSuperScatter1_Times = 0;
                IndepSuperScatter1_Bet = 0;
                IndepSuperScatter1_Win = 0;
                IndepSuperScatter2_Times = 0;
                IndepSuperScatter2_Bet = 0;
                IndepSuperScatter2_Win = 0;
                IndepSuperScatter3_Times = 0;
                IndepSuperScatter3_Bet = 0;
                IndepSuperScatter3_Win = 0;
                IndepSuperScatter4_Times = 0;
                IndepSuperScatter4_Bet = 0;
                IndepSuperScatter4_Win = 0;
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(Free_Times)) { Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Free_Bet)) { Free_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Free_Win)) { Free_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeFree_Times)) { FreeFree_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin1_Times)) { FreeRespin1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin2_Times)) { FreeRespin2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin3_Times)) { FreeRespin3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeRespin5_Times)) { FreeRespin5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin1_Times)) { Respin1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin2_Times)) { Respin2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin3_Times)) { Respin3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin5_Times)) { Respin5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SuperScatter1_Times)) { SuperScatter1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SuperScatter1_Bet)) { SuperScatter1_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SuperScatter1_Win)) { SuperScatter1_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SuperScatter2_Times)) { SuperScatter2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SuperScatter2_Bet)) { SuperScatter2_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SuperScatter2_Win)) { SuperScatter2_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SuperScatter3_Times)) { SuperScatter3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SuperScatter3_Bet)) { SuperScatter3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SuperScatter3_Win)) { SuperScatter3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(SuperScatter4_Times)) { SuperScatter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(SuperScatter4_Bet)) { SuperScatter4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(SuperScatter4_Win)) { SuperScatter4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(RedDragon5_Times)) { RedDragon5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RedDragon5_Bet)) { RedDragon5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(RedDragon5_Win)) { RedDragon5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(RedDragon4_Times)) { RedDragon4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RedDragon4_Bet)) { RedDragon4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(RedDragon4_Win)) { RedDragon4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(RedDragon3_Times)) { RedDragon3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RedDragon3_Bet)) { RedDragon3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(RedDragon3_Win)) { RedDragon3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(GreenDragon5_Times)) { GreenDragon5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GreenDragon5_Bet)) { GreenDragon5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GreenDragon5_Win)) { GreenDragon5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(GreenDragon4_Times)) { GreenDragon4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GreenDragon4_Bet)) { GreenDragon4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GreenDragon4_Win)) { GreenDragon4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(GreenDragon3_Times)) { GreenDragon3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GreenDragon3_Bet)) { GreenDragon3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GreenDragon3_Win)) { GreenDragon3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WhiteDragon5_Times)) { WhiteDragon5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WhiteDragon5_Bet)) { WhiteDragon5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WhiteDragon5_Win)) { WhiteDragon5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WhiteDragon4_Times)) { WhiteDragon4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WhiteDragon4_Bet)) { WhiteDragon4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WhiteDragon4_Win)) { WhiteDragon4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WhiteDragon3_Times)) { WhiteDragon3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WhiteDragon3_Bet)) { WhiteDragon3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WhiteDragon3_Win)) { WhiteDragon3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(EightCharacter5_Times)) { EightCharacter5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EightCharacter5_Bet)) { EightCharacter5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(EightCharacter5_Win)) { EightCharacter5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(EightCharacter4_Times)) { EightCharacter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EightCharacter4_Bet)) { EightCharacter4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(EightCharacter4_Win)) { EightCharacter4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(EightCharacter3_Times)) { EightCharacter3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EightCharacter3_Bet)) { EightCharacter3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(EightCharacter3_Win)) { EightCharacter3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FiveDots5_Times)) { FiveDots5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FiveDots5_Bet)) { FiveDots5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FiveDots5_Win)) { FiveDots5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FiveDots4_Times)) { FiveDots4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FiveDots4_Bet)) { FiveDots4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FiveDots4_Win)) { FiveDots4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FiveDots3_Times)) { FiveDots3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FiveDots3_Bet)) { FiveDots3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FiveDots3_Win)) { FiveDots3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ThreeDots5_Times)) { ThreeDots5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ThreeDots5_Bet)) { ThreeDots5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ThreeDots5_Win)) { ThreeDots5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ThreeDots4_Times)) { ThreeDots4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ThreeDots4_Bet)) { ThreeDots4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ThreeDots4_Win)) { ThreeDots4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ThreeDots3_Times)) { ThreeDots3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ThreeDots3_Bet)) { ThreeDots3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ThreeDots3_Win)) { ThreeDots3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TwoDots5_Times)) { TwoDots5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TwoDots5_Bet)) { TwoDots5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TwoDots5_Win)) { TwoDots5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TwoDots4_Times)) { TwoDots4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TwoDots4_Bet)) { TwoDots4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TwoDots4_Win)) { TwoDots4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TwoDots3_Times)) { TwoDots3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TwoDots3_Bet)) { TwoDots3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TwoDots3_Win)) { TwoDots3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FiveSticks5_Times)) { FiveSticks5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FiveSticks5_Bet)) { FiveSticks5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FiveSticks5_Win)) { FiveSticks5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FiveSticks4_Times)) { FiveSticks4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FiveSticks4_Bet)) { FiveSticks4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FiveSticks4_Win)) { FiveSticks4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FiveSticks3_Times)) { FiveSticks3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FiveSticks3_Bet)) { FiveSticks3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FiveSticks3_Win)) { FiveSticks3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TwoSticks5_Times)) { TwoSticks5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TwoSticks5_Bet)) { TwoSticks5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TwoSticks5_Win)) { TwoSticks5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TwoSticks4_Times)) { TwoSticks4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TwoSticks4_Bet)) { TwoSticks4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TwoSticks4_Win)) { TwoSticks4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TwoSticks3_Times)) { TwoSticks3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TwoSticks3_Bet)) { TwoSticks3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TwoSticks3_Win)) { TwoSticks3_Win += Convert.ToDouble(value); return; }
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
            if (field == nameof(IndepSuperScatter1_Times)) { IndepSuperScatter1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepSuperScatter1_Bet)) { IndepSuperScatter1_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepSuperScatter1_Win)) { IndepSuperScatter1_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepSuperScatter2_Times)) { IndepSuperScatter2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepSuperScatter2_Bet)) { IndepSuperScatter2_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepSuperScatter2_Win)) { IndepSuperScatter2_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepSuperScatter3_Times)) { IndepSuperScatter3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepSuperScatter3_Bet)) { IndepSuperScatter3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepSuperScatter3_Win)) { IndepSuperScatter3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepSuperScatter4_Times)) { IndepSuperScatter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepSuperScatter4_Bet)) { IndepSuperScatter4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepSuperScatter4_Win)) { IndepSuperScatter4_Win += Convert.ToDouble(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(Free_Times), Free_Times.ToString());
            updata.Add(nameof(Free_Bet), Free_Bet.ToString());
            updata.Add(nameof(Free_Win), Free_Win.ToString());
            updata.Add(nameof(FreeFree_Times), FreeFree_Times.ToString());
            updata.Add(nameof(FreeRespin1_Times), FreeRespin1_Times.ToString());
            updata.Add(nameof(FreeRespin2_Times), FreeRespin2_Times.ToString());
            updata.Add(nameof(FreeRespin3_Times), FreeRespin3_Times.ToString());
            updata.Add(nameof(FreeRespin5_Times), FreeRespin5_Times.ToString());
            updata.Add(nameof(Respin1_Times), Respin1_Times.ToString());
            updata.Add(nameof(Respin2_Times), Respin2_Times.ToString());
            updata.Add(nameof(Respin3_Times), Respin3_Times.ToString());
            updata.Add(nameof(Respin5_Times), Respin5_Times.ToString());
            updata.Add(nameof(SuperScatter1_Times), SuperScatter1_Times.ToString());
            updata.Add(nameof(SuperScatter1_Bet), SuperScatter1_Bet.ToString());
            updata.Add(nameof(SuperScatter1_Win), SuperScatter1_Win.ToString());
            updata.Add(nameof(SuperScatter2_Times), SuperScatter2_Times.ToString());
            updata.Add(nameof(SuperScatter2_Bet), SuperScatter2_Bet.ToString());
            updata.Add(nameof(SuperScatter2_Win), SuperScatter2_Win.ToString());
            updata.Add(nameof(SuperScatter3_Times), SuperScatter3_Times.ToString());
            updata.Add(nameof(SuperScatter3_Bet), SuperScatter3_Bet.ToString());
            updata.Add(nameof(SuperScatter3_Win), SuperScatter3_Win.ToString());
            updata.Add(nameof(SuperScatter4_Times), SuperScatter4_Times.ToString());
            updata.Add(nameof(SuperScatter4_Bet), SuperScatter4_Bet.ToString());
            updata.Add(nameof(SuperScatter4_Win), SuperScatter4_Win.ToString());
            updata.Add(nameof(RedDragon5_Times), RedDragon5_Times.ToString());
            updata.Add(nameof(RedDragon5_Bet), RedDragon5_Bet.ToString());
            updata.Add(nameof(RedDragon5_Win), RedDragon5_Win.ToString());
            updata.Add(nameof(RedDragon4_Times), RedDragon4_Times.ToString());
            updata.Add(nameof(RedDragon4_Bet), RedDragon4_Bet.ToString());
            updata.Add(nameof(RedDragon4_Win), RedDragon4_Win.ToString());
            updata.Add(nameof(RedDragon3_Times), RedDragon3_Times.ToString());
            updata.Add(nameof(RedDragon3_Bet), RedDragon3_Bet.ToString());
            updata.Add(nameof(RedDragon3_Win), RedDragon3_Win.ToString());
            updata.Add(nameof(GreenDragon5_Times), GreenDragon5_Times.ToString());
            updata.Add(nameof(GreenDragon5_Bet), GreenDragon5_Bet.ToString());
            updata.Add(nameof(GreenDragon5_Win), GreenDragon5_Win.ToString());
            updata.Add(nameof(GreenDragon4_Times), GreenDragon4_Times.ToString());
            updata.Add(nameof(GreenDragon4_Bet), GreenDragon4_Bet.ToString());
            updata.Add(nameof(GreenDragon4_Win), GreenDragon4_Win.ToString());
            updata.Add(nameof(GreenDragon3_Times), GreenDragon3_Times.ToString());
            updata.Add(nameof(GreenDragon3_Bet), GreenDragon3_Bet.ToString());
            updata.Add(nameof(GreenDragon3_Win), GreenDragon3_Win.ToString());
            updata.Add(nameof(WhiteDragon5_Times), WhiteDragon5_Times.ToString());
            updata.Add(nameof(WhiteDragon5_Bet), WhiteDragon5_Bet.ToString());
            updata.Add(nameof(WhiteDragon5_Win), WhiteDragon5_Win.ToString());
            updata.Add(nameof(WhiteDragon4_Times), WhiteDragon4_Times.ToString());
            updata.Add(nameof(WhiteDragon4_Bet), WhiteDragon4_Bet.ToString());
            updata.Add(nameof(WhiteDragon4_Win), WhiteDragon4_Win.ToString());
            updata.Add(nameof(WhiteDragon3_Times), WhiteDragon3_Times.ToString());
            updata.Add(nameof(WhiteDragon3_Bet), WhiteDragon3_Bet.ToString());
            updata.Add(nameof(WhiteDragon3_Win), WhiteDragon3_Win.ToString());
            updata.Add(nameof(EightCharacter5_Times), EightCharacter5_Times.ToString());
            updata.Add(nameof(EightCharacter5_Bet), EightCharacter5_Bet.ToString());
            updata.Add(nameof(EightCharacter5_Win), EightCharacter5_Win.ToString());
            updata.Add(nameof(EightCharacter4_Times), EightCharacter4_Times.ToString());
            updata.Add(nameof(EightCharacter4_Bet), EightCharacter4_Bet.ToString());
            updata.Add(nameof(EightCharacter4_Win), EightCharacter4_Win.ToString());
            updata.Add(nameof(EightCharacter3_Times), EightCharacter3_Times.ToString());
            updata.Add(nameof(EightCharacter3_Bet), EightCharacter3_Bet.ToString());
            updata.Add(nameof(EightCharacter3_Win), EightCharacter3_Win.ToString());
            updata.Add(nameof(FiveDots5_Times), FiveDots5_Times.ToString());
            updata.Add(nameof(FiveDots5_Bet), FiveDots5_Bet.ToString());
            updata.Add(nameof(FiveDots5_Win), FiveDots5_Win.ToString());
            updata.Add(nameof(FiveDots4_Times), FiveDots4_Times.ToString());
            updata.Add(nameof(FiveDots4_Bet), FiveDots4_Bet.ToString());
            updata.Add(nameof(FiveDots4_Win), FiveDots4_Win.ToString());
            updata.Add(nameof(FiveDots3_Times), FiveDots3_Times.ToString());
            updata.Add(nameof(FiveDots3_Bet), FiveDots3_Bet.ToString());
            updata.Add(nameof(FiveDots3_Win), FiveDots3_Win.ToString());
            updata.Add(nameof(ThreeDots5_Times), ThreeDots5_Times.ToString());
            updata.Add(nameof(ThreeDots5_Bet), ThreeDots5_Bet.ToString());
            updata.Add(nameof(ThreeDots5_Win), ThreeDots5_Win.ToString());
            updata.Add(nameof(ThreeDots4_Times), ThreeDots4_Times.ToString());
            updata.Add(nameof(ThreeDots4_Bet), ThreeDots4_Bet.ToString());
            updata.Add(nameof(ThreeDots4_Win), ThreeDots4_Win.ToString());
            updata.Add(nameof(ThreeDots3_Times), ThreeDots3_Times.ToString());
            updata.Add(nameof(ThreeDots3_Bet), ThreeDots3_Bet.ToString());
            updata.Add(nameof(ThreeDots3_Win), ThreeDots3_Win.ToString());
            updata.Add(nameof(TwoDots5_Times), TwoDots5_Times.ToString());
            updata.Add(nameof(TwoDots5_Bet), TwoDots5_Bet.ToString());
            updata.Add(nameof(TwoDots5_Win), TwoDots5_Win.ToString());
            updata.Add(nameof(TwoDots4_Times), TwoDots4_Times.ToString());
            updata.Add(nameof(TwoDots4_Bet), TwoDots4_Bet.ToString());
            updata.Add(nameof(TwoDots4_Win), TwoDots4_Win.ToString());
            updata.Add(nameof(TwoDots3_Times), TwoDots3_Times.ToString());
            updata.Add(nameof(TwoDots3_Bet), TwoDots3_Bet.ToString());
            updata.Add(nameof(TwoDots3_Win), TwoDots3_Win.ToString());
            updata.Add(nameof(FiveSticks5_Times), FiveSticks5_Times.ToString());
            updata.Add(nameof(FiveSticks5_Bet), FiveSticks5_Bet.ToString());
            updata.Add(nameof(FiveSticks5_Win), FiveSticks5_Win.ToString());
            updata.Add(nameof(FiveSticks4_Times), FiveSticks4_Times.ToString());
            updata.Add(nameof(FiveSticks4_Bet), FiveSticks4_Bet.ToString());
            updata.Add(nameof(FiveSticks4_Win), FiveSticks4_Win.ToString());
            updata.Add(nameof(FiveSticks3_Times), FiveSticks3_Times.ToString());
            updata.Add(nameof(FiveSticks3_Bet), FiveSticks3_Bet.ToString());
            updata.Add(nameof(FiveSticks3_Win), FiveSticks3_Win.ToString());
            updata.Add(nameof(TwoSticks5_Times), TwoSticks5_Times.ToString());
            updata.Add(nameof(TwoSticks5_Bet), TwoSticks5_Bet.ToString());
            updata.Add(nameof(TwoSticks5_Win), TwoSticks5_Win.ToString());
            updata.Add(nameof(TwoSticks4_Times), TwoSticks4_Times.ToString());
            updata.Add(nameof(TwoSticks4_Bet), TwoSticks4_Bet.ToString());
            updata.Add(nameof(TwoSticks4_Win), TwoSticks4_Win.ToString());
            updata.Add(nameof(TwoSticks3_Times), TwoSticks3_Times.ToString());
            updata.Add(nameof(TwoSticks3_Bet), TwoSticks3_Bet.ToString());
            updata.Add(nameof(TwoSticks3_Win), TwoSticks3_Win.ToString());
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
            updata.Add(nameof(IndepSuperScatter1_Times), IndepSuperScatter1_Times.ToString());
            updata.Add(nameof(IndepSuperScatter1_Bet), IndepSuperScatter1_Bet.ToString());
            updata.Add(nameof(IndepSuperScatter1_Win), IndepSuperScatter1_Win.ToString());
            updata.Add(nameof(IndepSuperScatter2_Times), IndepSuperScatter2_Times.ToString());
            updata.Add(nameof(IndepSuperScatter2_Bet), IndepSuperScatter2_Bet.ToString());
            updata.Add(nameof(IndepSuperScatter2_Win), IndepSuperScatter2_Win.ToString());
            updata.Add(nameof(IndepSuperScatter3_Times), IndepSuperScatter3_Times.ToString());
            updata.Add(nameof(IndepSuperScatter3_Bet), IndepSuperScatter3_Bet.ToString());
            updata.Add(nameof(IndepSuperScatter3_Win), IndepSuperScatter3_Win.ToString());
            updata.Add(nameof(IndepSuperScatter4_Times), IndepSuperScatter4_Times.ToString());
            updata.Add(nameof(IndepSuperScatter4_Bet), IndepSuperScatter4_Bet.ToString());
            updata.Add(nameof(IndepSuperScatter4_Win), IndepSuperScatter4_Win.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            Free_Times = Convert.ToInt32(datalist[nameof(Free_Times)]);
            Free_Bet = Convert.ToDouble(datalist[nameof(Free_Bet)]);
            Free_Win = Convert.ToDouble(datalist[nameof(Free_Win)]);
            FreeFree_Times = Convert.ToInt32(datalist[nameof(FreeFree_Times)]);
            FreeRespin1_Times = Convert.ToInt32(datalist[nameof(FreeRespin1_Times)]);
            FreeRespin2_Times = Convert.ToInt32(datalist[nameof(FreeRespin2_Times)]);
            FreeRespin3_Times = Convert.ToInt32(datalist[nameof(FreeRespin3_Times)]);
            FreeRespin5_Times = Convert.ToInt32(datalist[nameof(FreeRespin5_Times)]);
            Respin1_Times = Convert.ToInt32(datalist[nameof(Respin1_Times)]);
            Respin2_Times = Convert.ToInt32(datalist[nameof(Respin2_Times)]);
            Respin3_Times = Convert.ToInt32(datalist[nameof(Respin3_Times)]);
            Respin5_Times = Convert.ToInt32(datalist[nameof(Respin5_Times)]);
            SuperScatter1_Times = Convert.ToInt32(datalist[nameof(SuperScatter1_Times)]);
            SuperScatter1_Bet = Convert.ToDouble(datalist[nameof(SuperScatter1_Bet)]);
            SuperScatter1_Win = Convert.ToDouble(datalist[nameof(SuperScatter1_Win)]);
            SuperScatter2_Times = Convert.ToInt32(datalist[nameof(SuperScatter2_Times)]);
            SuperScatter2_Bet = Convert.ToDouble(datalist[nameof(SuperScatter2_Bet)]);
            SuperScatter2_Win = Convert.ToDouble(datalist[nameof(SuperScatter2_Win)]);
            SuperScatter3_Times = Convert.ToInt32(datalist[nameof(SuperScatter3_Times)]);
            SuperScatter3_Bet = Convert.ToDouble(datalist[nameof(SuperScatter3_Bet)]);
            SuperScatter3_Win = Convert.ToDouble(datalist[nameof(SuperScatter3_Win)]);
            SuperScatter4_Times = Convert.ToInt32(datalist[nameof(SuperScatter4_Times)]);
            SuperScatter4_Bet = Convert.ToDouble(datalist[nameof(SuperScatter4_Bet)]);
            SuperScatter4_Win = Convert.ToDouble(datalist[nameof(SuperScatter4_Win)]);
            RedDragon5_Times = Convert.ToInt32(datalist[nameof(RedDragon5_Times)]);
            RedDragon5_Bet = Convert.ToDouble(datalist[nameof(RedDragon5_Bet)]);
            RedDragon5_Win = Convert.ToDouble(datalist[nameof(RedDragon5_Win)]);
            RedDragon4_Times = Convert.ToInt32(datalist[nameof(RedDragon4_Times)]);
            RedDragon4_Bet = Convert.ToDouble(datalist[nameof(RedDragon4_Bet)]);
            RedDragon4_Win = Convert.ToDouble(datalist[nameof(RedDragon4_Win)]);
            RedDragon3_Times = Convert.ToInt32(datalist[nameof(RedDragon3_Times)]);
            RedDragon3_Bet = Convert.ToDouble(datalist[nameof(RedDragon3_Bet)]);
            RedDragon3_Win = Convert.ToDouble(datalist[nameof(RedDragon3_Win)]);
            GreenDragon5_Times = Convert.ToInt32(datalist[nameof(GreenDragon5_Times)]);
            GreenDragon5_Bet = Convert.ToDouble(datalist[nameof(GreenDragon5_Bet)]);
            GreenDragon5_Win = Convert.ToDouble(datalist[nameof(GreenDragon5_Win)]);
            GreenDragon4_Times = Convert.ToInt32(datalist[nameof(GreenDragon4_Times)]);
            GreenDragon4_Bet = Convert.ToDouble(datalist[nameof(GreenDragon4_Bet)]);
            GreenDragon4_Win = Convert.ToDouble(datalist[nameof(GreenDragon4_Win)]);
            GreenDragon3_Times = Convert.ToInt32(datalist[nameof(GreenDragon3_Times)]);
            GreenDragon3_Bet = Convert.ToDouble(datalist[nameof(GreenDragon3_Bet)]);
            GreenDragon3_Win = Convert.ToDouble(datalist[nameof(GreenDragon3_Win)]);
            WhiteDragon5_Times = Convert.ToInt32(datalist[nameof(WhiteDragon5_Times)]);
            WhiteDragon5_Bet = Convert.ToDouble(datalist[nameof(WhiteDragon5_Bet)]);
            WhiteDragon5_Win = Convert.ToDouble(datalist[nameof(WhiteDragon5_Win)]);
            WhiteDragon4_Times = Convert.ToInt32(datalist[nameof(WhiteDragon4_Times)]);
            WhiteDragon4_Bet = Convert.ToDouble(datalist[nameof(WhiteDragon4_Bet)]);
            WhiteDragon4_Win = Convert.ToDouble(datalist[nameof(WhiteDragon4_Win)]);
            WhiteDragon3_Times = Convert.ToInt32(datalist[nameof(WhiteDragon3_Times)]);
            WhiteDragon3_Bet = Convert.ToDouble(datalist[nameof(WhiteDragon3_Bet)]);
            WhiteDragon3_Win = Convert.ToDouble(datalist[nameof(WhiteDragon3_Win)]);
            EightCharacter5_Times = Convert.ToInt32(datalist[nameof(EightCharacter5_Times)]);
            EightCharacter5_Bet = Convert.ToDouble(datalist[nameof(EightCharacter5_Bet)]);
            EightCharacter5_Win = Convert.ToDouble(datalist[nameof(EightCharacter5_Win)]);
            EightCharacter4_Times = Convert.ToInt32(datalist[nameof(EightCharacter4_Times)]);
            EightCharacter4_Bet = Convert.ToDouble(datalist[nameof(EightCharacter4_Bet)]);
            EightCharacter4_Win = Convert.ToDouble(datalist[nameof(EightCharacter4_Win)]);
            EightCharacter3_Times = Convert.ToInt32(datalist[nameof(EightCharacter3_Times)]);
            EightCharacter3_Bet = Convert.ToDouble(datalist[nameof(EightCharacter3_Bet)]);
            EightCharacter3_Win = Convert.ToDouble(datalist[nameof(EightCharacter3_Win)]);
            FiveDots5_Times = Convert.ToInt32(datalist[nameof(FiveDots5_Times)]);
            FiveDots5_Bet = Convert.ToDouble(datalist[nameof(FiveDots5_Bet)]);
            FiveDots5_Win = Convert.ToDouble(datalist[nameof(FiveDots5_Win)]);
            FiveDots4_Times = Convert.ToInt32(datalist[nameof(FiveDots4_Times)]);
            FiveDots4_Bet = Convert.ToDouble(datalist[nameof(FiveDots4_Bet)]);
            FiveDots4_Win = Convert.ToDouble(datalist[nameof(FiveDots4_Win)]);
            FiveDots3_Times = Convert.ToInt32(datalist[nameof(FiveDots3_Times)]);
            FiveDots3_Bet = Convert.ToDouble(datalist[nameof(FiveDots3_Bet)]);
            FiveDots3_Win = Convert.ToDouble(datalist[nameof(FiveDots3_Win)]);
            ThreeDots5_Times = Convert.ToInt32(datalist[nameof(ThreeDots5_Times)]);
            ThreeDots5_Bet = Convert.ToDouble(datalist[nameof(ThreeDots5_Bet)]);
            ThreeDots5_Win = Convert.ToDouble(datalist[nameof(ThreeDots5_Win)]);
            ThreeDots4_Times = Convert.ToInt32(datalist[nameof(ThreeDots4_Times)]);
            ThreeDots4_Bet = Convert.ToDouble(datalist[nameof(ThreeDots4_Bet)]);
            ThreeDots4_Win = Convert.ToDouble(datalist[nameof(ThreeDots4_Win)]);
            ThreeDots3_Times = Convert.ToInt32(datalist[nameof(ThreeDots3_Times)]);
            ThreeDots3_Bet = Convert.ToDouble(datalist[nameof(ThreeDots3_Bet)]);
            ThreeDots3_Win = Convert.ToDouble(datalist[nameof(ThreeDots3_Win)]);
            TwoDots5_Times = Convert.ToInt32(datalist[nameof(TwoDots5_Times)]);
            TwoDots5_Bet = Convert.ToDouble(datalist[nameof(TwoDots5_Bet)]);
            TwoDots5_Win = Convert.ToDouble(datalist[nameof(TwoDots5_Win)]);
            TwoDots4_Times = Convert.ToInt32(datalist[nameof(TwoDots4_Times)]);
            TwoDots4_Bet = Convert.ToDouble(datalist[nameof(TwoDots4_Bet)]);
            TwoDots4_Win = Convert.ToDouble(datalist[nameof(TwoDots4_Win)]);
            TwoDots3_Times = Convert.ToInt32(datalist[nameof(TwoDots3_Times)]);
            TwoDots3_Bet = Convert.ToDouble(datalist[nameof(TwoDots3_Bet)]);
            TwoDots3_Win = Convert.ToDouble(datalist[nameof(TwoDots3_Win)]);
            FiveSticks5_Times = Convert.ToInt32(datalist[nameof(FiveSticks5_Times)]);
            FiveSticks5_Bet = Convert.ToDouble(datalist[nameof(FiveSticks5_Bet)]);
            FiveSticks5_Win = Convert.ToDouble(datalist[nameof(FiveSticks5_Win)]);
            FiveSticks4_Times = Convert.ToInt32(datalist[nameof(FiveSticks4_Times)]);
            FiveSticks4_Bet = Convert.ToDouble(datalist[nameof(FiveSticks4_Bet)]);
            FiveSticks4_Win = Convert.ToDouble(datalist[nameof(FiveSticks4_Win)]);
            FiveSticks3_Times = Convert.ToInt32(datalist[nameof(FiveSticks3_Times)]);
            FiveSticks3_Bet = Convert.ToDouble(datalist[nameof(FiveSticks3_Bet)]);
            FiveSticks3_Win = Convert.ToDouble(datalist[nameof(FiveSticks3_Win)]);
            TwoSticks5_Times = Convert.ToInt32(datalist[nameof(TwoSticks5_Times)]);
            TwoSticks5_Bet = Convert.ToDouble(datalist[nameof(TwoSticks5_Bet)]);
            TwoSticks5_Win = Convert.ToDouble(datalist[nameof(TwoSticks5_Win)]);
            TwoSticks4_Times = Convert.ToInt32(datalist[nameof(TwoSticks4_Times)]);
            TwoSticks4_Bet = Convert.ToDouble(datalist[nameof(TwoSticks4_Bet)]);
            TwoSticks4_Win = Convert.ToDouble(datalist[nameof(TwoSticks4_Win)]);
            TwoSticks3_Times = Convert.ToInt32(datalist[nameof(TwoSticks3_Times)]);
            TwoSticks3_Bet = Convert.ToDouble(datalist[nameof(TwoSticks3_Bet)]);
            TwoSticks3_Win = Convert.ToDouble(datalist[nameof(TwoSticks3_Win)]);
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
            IndepSuperScatter1_Times = Convert.ToInt32(datalist[nameof(IndepSuperScatter1_Times)]);
            IndepSuperScatter1_Bet = Convert.ToDouble(datalist[nameof(IndepSuperScatter1_Bet)]);
            IndepSuperScatter1_Win = Convert.ToDouble(datalist[nameof(IndepSuperScatter1_Win)]);
            IndepSuperScatter2_Times = Convert.ToInt32(datalist[nameof(IndepSuperScatter2_Times)]);
            IndepSuperScatter2_Bet = Convert.ToDouble(datalist[nameof(IndepSuperScatter2_Bet)]);
            IndepSuperScatter2_Win = Convert.ToDouble(datalist[nameof(IndepSuperScatter2_Win)]);
            IndepSuperScatter3_Times = Convert.ToInt32(datalist[nameof(IndepSuperScatter3_Times)]);
            IndepSuperScatter3_Bet = Convert.ToDouble(datalist[nameof(IndepSuperScatter3_Bet)]);
            IndepSuperScatter3_Win = Convert.ToDouble(datalist[nameof(IndepSuperScatter3_Win)]);
            IndepSuperScatter4_Times = Convert.ToInt32(datalist[nameof(IndepSuperScatter4_Times)]);
            IndepSuperScatter4_Bet = Convert.ToDouble(datalist[nameof(IndepSuperScatter4_Bet)]);
            IndepSuperScatter4_Win = Convert.ToDouble(datalist[nameof(IndepSuperScatter4_Win)]);
        }
    }
}
