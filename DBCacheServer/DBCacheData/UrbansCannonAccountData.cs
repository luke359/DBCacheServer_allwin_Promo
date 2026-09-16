using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class UrbansCannonAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int Urban5_Times;
        private double Urban5_Bet;
        private double Urban5_Win;
        private int Muhammad5_Times;
        private double Muhammad5_Bet;
        private double Muhammad5_Win;
        private int Constantine5_Times;
        private double Constantine5_Bet;
        private double Constantine5_Win;
        private int Mercenaries5_Times;
        private double Mercenaries5_Bet;
        private double Mercenaries5_Win;
        private int Mason5_Times;
        private double Mason5_Bet;
        private double Mason5_Win;
        private int Cannon5_Times;
        private double Cannon5_Bet;
        private double Cannon5_Win;
        private int Blueprint5_Times;
        private double Blueprint5_Bet;
        private double Blueprint5_Win;
        private int StoneBullet5_Times;
        private double StoneBullet5_Bet;
        private double StoneBullet5_Win;
        private int CityWall5_Times;
        private double CityWall5_Bet;
        private double CityWall5_Win;
        private int OliveOil5_Times;
        private double OliveOil5_Bet;
        private double OliveOil5_Win;

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
                Urban5_Times = 0;
                Urban5_Bet = 0;
                Urban5_Win = 0;
                Muhammad5_Times = 0;
                Muhammad5_Bet = 0;
                Muhammad5_Win = 0;
                Constantine5_Times = 0;
                Constantine5_Bet = 0;
                Constantine5_Win = 0;
                Mercenaries5_Times = 0;
                Mercenaries5_Bet = 0;
                Mercenaries5_Win = 0;
                Mason5_Times = 0;
                Mason5_Bet = 0;
                Mason5_Win = 0;
                Cannon5_Times = 0;
                Cannon5_Bet = 0;
                Cannon5_Win = 0;
                Blueprint5_Times = 0;
                Blueprint5_Bet = 0;
                Blueprint5_Win = 0;
                StoneBullet5_Times = 0;
                StoneBullet5_Bet = 0;
                StoneBullet5_Win = 0;
                CityWall5_Times = 0;
                CityWall5_Bet = 0;
                CityWall5_Win = 0;
                OliveOil5_Times = 0;
                OliveOil5_Bet = 0;
                OliveOil5_Win = 0;
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
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(Free_Times)) { Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Free_Bet)) { Free_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Free_Win)) { Free_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Urban5_Times)) { Urban5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Urban5_Bet)) { Urban5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Urban5_Win)) { Urban5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Muhammad5_Times)) { Muhammad5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Muhammad5_Bet)) { Muhammad5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Muhammad5_Win)) { Muhammad5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Constantine5_Times)) { Constantine5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Constantine5_Bet)) { Constantine5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Constantine5_Win)) { Constantine5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Mercenaries5_Times)) { Mercenaries5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Mercenaries5_Bet)) { Mercenaries5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Mercenaries5_Win)) { Mercenaries5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Mason5_Times)) { Mason5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Mason5_Bet)) { Mason5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Mason5_Win)) { Mason5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Cannon5_Times)) { Cannon5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Cannon5_Bet)) { Cannon5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Cannon5_Win)) { Cannon5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Blueprint5_Times)) { Blueprint5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Blueprint5_Bet)) { Blueprint5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Blueprint5_Win)) { Blueprint5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(StoneBullet5_Times)) { StoneBullet5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StoneBullet5_Bet)) { StoneBullet5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(StoneBullet5_Win)) { StoneBullet5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(CityWall5_Times)) { CityWall5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(CityWall5_Bet)) { CityWall5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(CityWall5_Win)) { CityWall5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(OliveOil5_Times)) { OliveOil5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(OliveOil5_Bet)) { OliveOil5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(OliveOil5_Win)) { OliveOil5_Win += Convert.ToDouble(value); return; }

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
            updata.Add(nameof(Urban5_Times), Urban5_Times.ToString());
            updata.Add(nameof(Urban5_Bet), Urban5_Bet.ToString());
            updata.Add(nameof(Urban5_Win), Urban5_Win.ToString());
            updata.Add(nameof(Muhammad5_Times), Muhammad5_Times.ToString());
            updata.Add(nameof(Muhammad5_Bet), Muhammad5_Bet.ToString());
            updata.Add(nameof(Muhammad5_Win), Muhammad5_Win.ToString());
            updata.Add(nameof(Constantine5_Times), Constantine5_Times.ToString());
            updata.Add(nameof(Constantine5_Bet), Constantine5_Bet.ToString());
            updata.Add(nameof(Constantine5_Win), Constantine5_Win.ToString());
            updata.Add(nameof(Mercenaries5_Times), Mercenaries5_Times.ToString());
            updata.Add(nameof(Mercenaries5_Bet), Mercenaries5_Bet.ToString());
            updata.Add(nameof(Mercenaries5_Win), Mercenaries5_Win.ToString());
            updata.Add(nameof(Mason5_Times), Mason5_Times.ToString());
            updata.Add(nameof(Mason5_Bet), Mason5_Bet.ToString());
            updata.Add(nameof(Mason5_Win), Mason5_Win.ToString());
            updata.Add(nameof(Cannon5_Times), Cannon5_Times.ToString());
            updata.Add(nameof(Cannon5_Bet), Cannon5_Bet.ToString());
            updata.Add(nameof(Cannon5_Win), Cannon5_Win.ToString());
            updata.Add(nameof(Blueprint5_Times), Blueprint5_Times.ToString());
            updata.Add(nameof(Blueprint5_Bet), Blueprint5_Bet.ToString());
            updata.Add(nameof(Blueprint5_Win), Blueprint5_Win.ToString());
            updata.Add(nameof(StoneBullet5_Times), StoneBullet5_Times.ToString());
            updata.Add(nameof(StoneBullet5_Bet), StoneBullet5_Bet.ToString());
            updata.Add(nameof(StoneBullet5_Win), StoneBullet5_Win.ToString());
            updata.Add(nameof(CityWall5_Times), CityWall5_Times.ToString());
            updata.Add(nameof(CityWall5_Bet), CityWall5_Bet.ToString());
            updata.Add(nameof(CityWall5_Win), CityWall5_Win.ToString());
            updata.Add(nameof(OliveOil5_Times), OliveOil5_Times.ToString());
            updata.Add(nameof(OliveOil5_Bet), OliveOil5_Bet.ToString());
            updata.Add(nameof(OliveOil5_Win), OliveOil5_Win.ToString());

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
            Urban5_Times = Convert.ToInt32(datalist[nameof(Urban5_Times)]);
            Urban5_Bet = Convert.ToDouble(datalist[nameof(Urban5_Bet)]);
            Urban5_Win = Convert.ToDouble(datalist[nameof(Urban5_Win)]);
            Muhammad5_Times = Convert.ToInt32(datalist[nameof(Muhammad5_Times)]);
            Muhammad5_Bet = Convert.ToDouble(datalist[nameof(Muhammad5_Bet)]);
            Muhammad5_Win = Convert.ToDouble(datalist[nameof(Muhammad5_Win)]);
            Constantine5_Times = Convert.ToInt32(datalist[nameof(Constantine5_Times)]);
            Constantine5_Bet = Convert.ToDouble(datalist[nameof(Constantine5_Bet)]);
            Constantine5_Win = Convert.ToDouble(datalist[nameof(Constantine5_Win)]);
            Mercenaries5_Times = Convert.ToInt32(datalist[nameof(Mercenaries5_Times)]);
            Mercenaries5_Bet = Convert.ToDouble(datalist[nameof(Mercenaries5_Bet)]);
            Mercenaries5_Win = Convert.ToDouble(datalist[nameof(Mercenaries5_Win)]);
            Mason5_Times = Convert.ToInt32(datalist[nameof(Mason5_Times)]);
            Mason5_Bet = Convert.ToDouble(datalist[nameof(Mason5_Bet)]);
            Mason5_Win = Convert.ToDouble(datalist[nameof(Mason5_Win)]);
            Cannon5_Times = Convert.ToInt32(datalist[nameof(Cannon5_Times)]);
            Cannon5_Bet = Convert.ToDouble(datalist[nameof(Cannon5_Bet)]);
            Cannon5_Win = Convert.ToDouble(datalist[nameof(Cannon5_Win)]);
            Blueprint5_Times = Convert.ToInt32(datalist[nameof(Blueprint5_Times)]);
            Blueprint5_Bet = Convert.ToDouble(datalist[nameof(Blueprint5_Bet)]);
            Blueprint5_Win = Convert.ToDouble(datalist[nameof(Blueprint5_Win)]);
            StoneBullet5_Times = Convert.ToInt32(datalist[nameof(StoneBullet5_Times)]);
            StoneBullet5_Bet = Convert.ToDouble(datalist[nameof(StoneBullet5_Bet)]);
            StoneBullet5_Win = Convert.ToDouble(datalist[nameof(StoneBullet5_Win)]);
            CityWall5_Times = Convert.ToInt32(datalist[nameof(CityWall5_Times)]);
            CityWall5_Bet = Convert.ToDouble(datalist[nameof(CityWall5_Bet)]);
            CityWall5_Win = Convert.ToDouble(datalist[nameof(CityWall5_Win)]);
            OliveOil5_Times = Convert.ToInt32(datalist[nameof(OliveOil5_Times)]);
            OliveOil5_Bet = Convert.ToDouble(datalist[nameof(OliveOil5_Bet)]);
            OliveOil5_Win = Convert.ToDouble(datalist[nameof(OliveOil5_Win)]);

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
