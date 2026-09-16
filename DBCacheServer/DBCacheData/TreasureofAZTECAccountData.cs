using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class TreasureofAZTECAccountData : CommonAccountData
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
        private int Chief6_Times;
        private double Chief6_Bet;
        private double Chief6_Win;
        private int Chief_Times;
        private int FreeChief_Times;
        private int Priest6_Times;
        private double Priest6_Bet;
        private double Priest6_Win;
        private int Warrior6_Times;
        private double Warrior6_Bet;
        private double Warrior6_Win;
        private int Slave6_Times;
        private double Slave6_Bet;
        private double Slave6_Win;
        private int TotemA6_Times;
        private double TotemA6_Bet;
        private double TotemA6_Win;
        private int TotemB6_Times;
        private double TotemB6_Bet;
        private double TotemB6_Win;
        private int A6_Times;
        private double A6_Bet;
        private double A6_Win;
        private int K6_Times;
        private double K6_Bet;
        private double K6_Win;
        private int Q6_Times;
        private double Q6_Bet;
        private double Q6_Win;
        private int J6_Times;
        private double J6_Bet;
        private double J6_Win;
        private int Ten6_Times;
        private double Ten6_Bet;
        private double Ten6_Win;
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
                Chief6_Times = 0;
                Chief6_Bet = 0;
                Chief6_Win = 0;
                Chief_Times = 0;
                FreeChief_Times = 0;
                Priest6_Times = 0;
                Priest6_Bet = 0;
                Priest6_Win = 0;
                Warrior6_Times = 0;
                Warrior6_Bet = 0;
                Warrior6_Win = 0;
                Slave6_Times = 0;
                Slave6_Bet = 0;
                Slave6_Win = 0;
                TotemA6_Times = 0;
                TotemA6_Bet = 0;
                TotemA6_Win = 0;
                TotemB6_Times = 0;
                TotemB6_Bet = 0;
                TotemB6_Win = 0;
                A6_Times = 0;
                A6_Bet = 0;
                A6_Win = 0;
                K6_Times = 0;
                K6_Bet = 0;
                K6_Win = 0;
                Q6_Times = 0;
                Q6_Bet = 0;
                Q6_Win = 0;
                J6_Times = 0;
                J6_Bet = 0;
                J6_Win = 0;
                Ten6_Times = 0;
                Ten6_Bet = 0;
                Ten6_Win = 0;
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
            if (field == nameof(Chief6_Times)) { Chief6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Chief6_Bet)) { Chief6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Chief6_Win)) { Chief6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Chief_Times)) { Chief_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeChief_Times)) { FreeChief_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Priest6_Times)) { Priest6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Priest6_Bet)) { Priest6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Priest6_Win)) { Priest6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Warrior6_Times)) { Warrior6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Warrior6_Bet)) { Warrior6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Warrior6_Win)) { Warrior6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Slave6_Times)) { Slave6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Slave6_Bet)) { Slave6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Slave6_Win)) { Slave6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TotemA6_Times)) { TotemA6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TotemA6_Bet)) { TotemA6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TotemA6_Win)) { TotemA6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TotemB6_Times)) { TotemB6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TotemB6_Bet)) { TotemB6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TotemB6_Win)) { TotemB6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(A6_Times)) { A6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(A6_Bet)) { A6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(A6_Win)) { A6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(K6_Times)) { K6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(K6_Bet)) { K6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(K6_Win)) { K6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Q6_Times)) { Q6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Q6_Bet)) { Q6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Q6_Win)) { Q6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(J6_Times)) { J6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(J6_Bet)) { J6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(J6_Win)) { J6_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ten6_Times)) { Ten6_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ten6_Bet)) { Ten6_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ten6_Win)) { Ten6_Win += Convert.ToDouble(value); return; }
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
            updata.Add(nameof(Chief6_Times), Chief6_Times.ToString());
            updata.Add(nameof(Chief6_Bet), Chief6_Bet.ToString());
            updata.Add(nameof(Chief6_Win), Chief6_Win.ToString());
            updata.Add(nameof(Chief_Times), Chief_Times.ToString());
            updata.Add(nameof(FreeChief_Times), FreeChief_Times.ToString());
            updata.Add(nameof(Priest6_Times), Priest6_Times.ToString());
            updata.Add(nameof(Priest6_Bet), Priest6_Bet.ToString());
            updata.Add(nameof(Priest6_Win), Priest6_Win.ToString());
            updata.Add(nameof(Warrior6_Times), Warrior6_Times.ToString());
            updata.Add(nameof(Warrior6_Bet), Warrior6_Bet.ToString());
            updata.Add(nameof(Warrior6_Win), Warrior6_Win.ToString());
            updata.Add(nameof(Slave6_Times), Slave6_Times.ToString());
            updata.Add(nameof(Slave6_Bet), Slave6_Bet.ToString());
            updata.Add(nameof(Slave6_Win), Slave6_Win.ToString());
            updata.Add(nameof(TotemA6_Times), TotemA6_Times.ToString());
            updata.Add(nameof(TotemA6_Bet), TotemA6_Bet.ToString());
            updata.Add(nameof(TotemA6_Win), TotemA6_Win.ToString());
            updata.Add(nameof(TotemB6_Times), TotemB6_Times.ToString());
            updata.Add(nameof(TotemB6_Bet), TotemB6_Bet.ToString());
            updata.Add(nameof(TotemB6_Win), TotemB6_Win.ToString());
            updata.Add(nameof(A6_Times), A6_Times.ToString());
            updata.Add(nameof(A6_Bet), A6_Bet.ToString());
            updata.Add(nameof(A6_Win), A6_Win.ToString());
            updata.Add(nameof(K6_Times), K6_Times.ToString());
            updata.Add(nameof(K6_Bet), K6_Bet.ToString());
            updata.Add(nameof(K6_Win), K6_Win.ToString());
            updata.Add(nameof(Q6_Times), Q6_Times.ToString());
            updata.Add(nameof(Q6_Bet), Q6_Bet.ToString());
            updata.Add(nameof(Q6_Win), Q6_Win.ToString());
            updata.Add(nameof(J6_Times), J6_Times.ToString());
            updata.Add(nameof(J6_Bet), J6_Bet.ToString());
            updata.Add(nameof(J6_Win), J6_Win.ToString());
            updata.Add(nameof(Ten6_Times), Ten6_Times.ToString());
            updata.Add(nameof(Ten6_Bet), Ten6_Bet.ToString());
            updata.Add(nameof(Ten6_Win), Ten6_Win.ToString());
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
            Chief6_Times = Convert.ToInt32(datalist[nameof(Chief6_Times)]);
            Chief6_Bet = Convert.ToDouble(datalist[nameof(Chief6_Bet)]);
            Chief6_Win = Convert.ToDouble(datalist[nameof(Chief6_Win)]);
            Chief_Times = Convert.ToInt32(datalist[nameof(Chief_Times)]);
            FreeChief_Times = Convert.ToInt32(datalist[nameof(FreeChief_Times)]);
            Priest6_Times = Convert.ToInt32(datalist[nameof(Priest6_Times)]);
            Priest6_Bet = Convert.ToDouble(datalist[nameof(Priest6_Bet)]);
            Priest6_Win = Convert.ToDouble(datalist[nameof(Priest6_Win)]);
            Warrior6_Times = Convert.ToInt32(datalist[nameof(Warrior6_Times)]);
            Warrior6_Bet = Convert.ToDouble(datalist[nameof(Warrior6_Bet)]);
            Warrior6_Win = Convert.ToDouble(datalist[nameof(Warrior6_Win)]);
            Slave6_Times = Convert.ToInt32(datalist[nameof(Slave6_Times)]);
            Slave6_Bet = Convert.ToDouble(datalist[nameof(Slave6_Bet)]);
            Slave6_Win = Convert.ToDouble(datalist[nameof(Slave6_Win)]);
            TotemA6_Times = Convert.ToInt32(datalist[nameof(TotemA6_Times)]);
            TotemA6_Bet = Convert.ToDouble(datalist[nameof(TotemA6_Bet)]);
            TotemA6_Win = Convert.ToDouble(datalist[nameof(TotemA6_Win)]);
            TotemB6_Times = Convert.ToInt32(datalist[nameof(TotemB6_Times)]);
            TotemB6_Bet = Convert.ToDouble(datalist[nameof(TotemB6_Bet)]);
            TotemB6_Win = Convert.ToDouble(datalist[nameof(TotemB6_Win)]);
            A6_Times = Convert.ToInt32(datalist[nameof(A6_Times)]);
            A6_Bet = Convert.ToDouble(datalist[nameof(A6_Bet)]);
            A6_Win = Convert.ToDouble(datalist[nameof(A6_Win)]);
            K6_Times = Convert.ToInt32(datalist[nameof(K6_Times)]);
            K6_Bet = Convert.ToDouble(datalist[nameof(K6_Bet)]);
            K6_Win = Convert.ToDouble(datalist[nameof(K6_Win)]);
            Q6_Times = Convert.ToInt32(datalist[nameof(Q6_Times)]);
            Q6_Bet = Convert.ToDouble(datalist[nameof(Q6_Bet)]);
            Q6_Win = Convert.ToDouble(datalist[nameof(Q6_Win)]);
            J6_Times = Convert.ToInt32(datalist[nameof(J6_Times)]);
            J6_Bet = Convert.ToDouble(datalist[nameof(J6_Bet)]);
            J6_Win = Convert.ToDouble(datalist[nameof(J6_Win)]);
            Ten6_Times = Convert.ToInt32(datalist[nameof(Ten6_Times)]);
            Ten6_Bet = Convert.ToDouble(datalist[nameof(Ten6_Bet)]);
            Ten6_Win = Convert.ToDouble(datalist[nameof(Ten6_Win)]);
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
