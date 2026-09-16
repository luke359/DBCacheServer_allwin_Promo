using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class ScroogesWinterTreasureAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Respin_Times;
        private double Respin_Bet;
        private double Respin_Win;
        private int Respin_RoundTimes;
        private int Balls_Times;
        private double Balls_Bet;
        private double Balls_Win;
        private int Balls1015_Times;
        private double Balls1015_Bet;
        private double Balls1015_Win;
        private int Balls0709_Times;
        private double Balls0709_Bet;
        private double Balls0709_Win;
        private int Balls0306_Times;
        private double Balls0306_Bet;
        private double Balls0306_Win;
        private int Balls0102_Times;
        private double Balls0102_Bet;
        private double Balls0102_Win;
        private int Star1_Times;
        private double Star1_Bet;
        private double Star1_Win;
        private int Star2_Times;
        private double Star2_Bet;
        private double Star2_Win;
        private int Star3_Times;
        private double Star3_Bet;
        private double Star3_Win;
        private int Star4_Times;
        private double Star4_Bet;
        private double Star4_Win;
        private int Star5_Times;
        private double Star5_Bet;
        private double Star5_Win;
        private int Logo5_Times;
        private double Logo5_Bet;
        private double Logo5_Win;
        private int Logo4_Times;
        private double Logo4_Bet;
        private double Logo4_Win;
        private int Logo3_Times;
        private double Logo3_Bet;
        private double Logo3_Win;
        private int Scrooge5_Times;
        private double Scrooge5_Bet;
        private double Scrooge5_Win;
        private int Scrooge4_Times;
        private double Scrooge4_Bet;
        private double Scrooge4_Win;
        private int Scrooge3_Times;
        private double Scrooge3_Bet;
        private double Scrooge3_Win;
        private int Bar3x5_Times;
        private double Bar3x5_Bet;
        private double Bar3x5_Win;
        private int Bar3x4_Times;
        private double Bar3x4_Bet;
        private double Bar3x4_Win;
        private int Bar3x3_Times;
        private double Bar3x3_Bet;
        private double Bar3x3_Win;
        private int Bar2x5_Times;
        private double Bar2x5_Bet;
        private double Bar2x5_Win;
        private int Bar2x4_Times;
        private double Bar2x4_Bet;
        private double Bar2x4_Win;
        private int Bar2x3_Times;
        private double Bar2x3_Bet;
        private double Bar2x3_Win;
        private int Bar1x5_Times;
        private double Bar1x5_Bet;
        private double Bar1x5_Win;
        private int Bar1x4_Times;
        private double Bar1x4_Bet;
        private double Bar1x4_Win;
        private int Bar1x3_Times;
        private double Bar1x3_Bet;
        private double Bar1x3_Win;
        private int Book5_Times;
        private double Book5_Bet;
        private double Book5_Win;
        private int Book4_Times;
        private double Book4_Bet;
        private double Book4_Win;
        private int Book3_Times;
        private double Book3_Bet;
        private double Book3_Win;
        private int Purse5_Times;
        private double Purse5_Bet;
        private double Purse5_Win;
        private int Purse4_Times;
        private double Purse4_Bet;
        private double Purse4_Win;
        private int Purse3_Times;
        private double Purse3_Bet;
        private double Purse3_Win;
        private int Clock5_Times;
        private double Clock5_Bet;
        private double Clock5_Win;
        private int Clock4_Times;
        private double Clock4_Bet;
        private double Clock4_Win;
        private int Clock3_Times;
        private double Clock3_Bet;
        private double Clock3_Win;
        private int Key5_Times;
        private double Key5_Bet;
        private double Key5_Win;
        private int Key4_Times;
        private double Key4_Bet;
        private double Key4_Win;
        private int Key3_Times;
        private double Key3_Bet;
        private double Key3_Win;
        private int Scales5_Times;
        private double Scales5_Bet;
        private double Scales5_Win;
        private int Scales4_Times;
        private double Scales4_Bet;
        private double Scales4_Win;
        private int Scales3_Times;
        private double Scales3_Bet;
        private double Scales3_Win;
        private int IndepPlayTimes;
        private double IndepTotalBet;
        private double IndepTotalWin;
        private int IndepRoundTimes;
        private int IndepRespinRoundTimes;
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
                Respin_Times = 0;
                Respin_Bet = 0;
                Respin_Win = 0;
                Respin_RoundTimes = 0;
                Balls_Times = 0;
                Balls_Bet = 0;
                Balls_Win = 0;
                Balls1015_Times = 0;
                Balls1015_Bet = 0;
                Balls1015_Win = 0;
                Balls0709_Times = 0;
                Balls0709_Bet = 0;
                Balls0709_Win = 0;
                Balls0306_Times = 0;
                Balls0306_Bet = 0;
                Balls0306_Win = 0;
                Balls0102_Times = 0;
                Balls0102_Bet = 0;
                Balls0102_Win = 0;
                Star1_Times = 0;
                Star1_Bet = 0;
                Star1_Win = 0;
                Star2_Times = 0;
                Star2_Bet = 0;
                Star2_Win = 0;
                Star3_Times = 0;
                Star3_Bet = 0;
                Star3_Win = 0;
                Star4_Times = 0;
                Star4_Bet = 0;
                Star4_Win = 0;
                Star5_Times = 0;
                Star5_Bet = 0;
                Star5_Win = 0;
                Logo5_Times = 0;
                Logo5_Bet = 0;
                Logo5_Win = 0;
                Logo4_Times = 0;
                Logo4_Bet = 0;
                Logo4_Win = 0;
                Logo3_Times = 0;
                Logo3_Bet = 0;
                Logo3_Win = 0;
                Scrooge5_Times = 0;
                Scrooge5_Bet = 0;
                Scrooge5_Win = 0;
                Scrooge4_Times = 0;
                Scrooge4_Bet = 0;
                Scrooge4_Win = 0;
                Scrooge3_Times = 0;
                Scrooge3_Bet = 0;
                Scrooge3_Win = 0;
                Bar3x5_Times = 0;
                Bar3x5_Bet = 0;
                Bar3x5_Win = 0;
                Bar3x4_Times = 0;
                Bar3x4_Bet = 0;
                Bar3x4_Win = 0;
                Bar3x3_Times = 0;
                Bar3x3_Bet = 0;
                Bar3x3_Win = 0;
                Bar2x5_Times = 0;
                Bar2x5_Bet = 0;
                Bar2x5_Win = 0;
                Bar2x4_Times = 0;
                Bar2x4_Bet = 0;
                Bar2x4_Win = 0;
                Bar2x3_Times = 0;
                Bar2x3_Bet = 0;
                Bar2x3_Win = 0;
                Bar1x5_Times = 0;
                Bar1x5_Bet = 0;
                Bar1x5_Win = 0;
                Bar1x4_Times = 0;
                Bar1x4_Bet = 0;
                Bar1x4_Win = 0;
                Bar1x3_Times = 0;
                Bar1x3_Bet = 0;
                Bar1x3_Win = 0;
                Book5_Times = 0;
                Book5_Bet = 0;
                Book5_Win = 0;
                Book4_Times = 0;
                Book4_Bet = 0;
                Book4_Win = 0;
                Book3_Times = 0;
                Book3_Bet = 0;
                Book3_Win = 0;
                Purse5_Times = 0;
                Purse5_Bet = 0;
                Purse5_Win = 0;
                Purse4_Times = 0;
                Purse4_Bet = 0;
                Purse4_Win = 0;
                Purse3_Times = 0;
                Purse3_Bet = 0;
                Purse3_Win = 0;
                Clock5_Times = 0;
                Clock5_Bet = 0;
                Clock5_Win = 0;
                Clock4_Times = 0;
                Clock4_Bet = 0;
                Clock4_Win = 0;
                Clock3_Times = 0;
                Clock3_Bet = 0;
                Clock3_Win = 0;
                Key5_Times = 0;
                Key5_Bet = 0;
                Key5_Win = 0;
                Key4_Times = 0;
                Key4_Bet = 0;
                Key4_Win = 0;
                Key3_Times = 0;
                Key3_Bet = 0;
                Key3_Win = 0;
                Scales5_Times = 0;
                Scales5_Bet = 0;
                Scales5_Win = 0;
                Scales4_Times = 0;
                Scales4_Bet = 0;
                Scales4_Win = 0;
                Scales3_Times = 0;
                Scales3_Bet = 0;
                Scales3_Win = 0;
            }
            if (mode == 1 || mode == 2)
            {
                IndepPlayTimes = 0;
                IndepTotalBet = 0;
                IndepTotalWin = 0;
                IndepRoundTimes = 0;
                IndepRespinRoundTimes = 0;
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(Respin_Times)) { Respin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin_Bet)) { Respin_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Respin_Win)) { Respin_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Respin_RoundTimes)) { Respin_RoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(Balls_Times)) { Balls_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Balls_Bet)) { Balls_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Balls_Win)) { Balls_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Balls1015_Times)) { Balls1015_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Balls1015_Bet)) { Balls1015_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Balls1015_Win)) { Balls1015_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Balls0709_Times)) { Balls0709_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Balls0709_Bet)) { Balls0709_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Balls0709_Win)) { Balls0709_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Balls0306_Times)) { Balls0306_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Balls0306_Bet)) { Balls0306_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Balls0306_Win)) { Balls0306_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Balls0102_Times)) { Balls0102_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Balls0102_Bet)) { Balls0102_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Balls0102_Win)) { Balls0102_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Star1_Times)) { Star1_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Star1_Bet)) { Star1_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Star1_Win)) { Star1_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Star2_Times)) { Star2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Star2_Bet)) { Star2_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Star2_Win)) { Star2_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Star3_Times)) { Star3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Star3_Bet)) { Star3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Star3_Win)) { Star3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Star4_Times)) { Star4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Star4_Bet)) { Star4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Star4_Win)) { Star4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Star5_Times)) { Star5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Star5_Bet)) { Star5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Star5_Win)) { Star5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Times)) { Logo5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo5_Bet)) { Logo5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Win)) { Logo5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Times)) { Logo4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo4_Bet)) { Logo4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Win)) { Logo4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Times)) { Logo3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo3_Bet)) { Logo3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Win)) { Logo3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scrooge5_Times)) { Scrooge5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scrooge5_Bet)) { Scrooge5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scrooge5_Win)) { Scrooge5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scrooge4_Times)) { Scrooge4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scrooge4_Bet)) { Scrooge4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scrooge4_Win)) { Scrooge4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scrooge3_Times)) { Scrooge3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scrooge3_Bet)) { Scrooge3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scrooge3_Win)) { Scrooge3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar3x5_Times)) { Bar3x5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar3x5_Bet)) { Bar3x5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar3x5_Win)) { Bar3x5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar3x4_Times)) { Bar3x4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar3x4_Bet)) { Bar3x4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar3x4_Win)) { Bar3x4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar3x3_Times)) { Bar3x3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar3x3_Bet)) { Bar3x3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar3x3_Win)) { Bar3x3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar2x5_Times)) { Bar2x5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar2x5_Bet)) { Bar2x5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar2x5_Win)) { Bar2x5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar2x4_Times)) { Bar2x4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar2x4_Bet)) { Bar2x4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar2x4_Win)) { Bar2x4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar2x3_Times)) { Bar2x3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar2x3_Bet)) { Bar2x3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar2x3_Win)) { Bar2x3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar1x5_Times)) { Bar1x5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar1x5_Bet)) { Bar1x5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar1x5_Win)) { Bar1x5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar1x4_Times)) { Bar1x4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar1x4_Bet)) { Bar1x4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar1x4_Win)) { Bar1x4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bar1x3_Times)) { Bar1x3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bar1x3_Bet)) { Bar1x3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bar1x3_Win)) { Bar1x3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Book5_Times)) { Book5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Book5_Bet)) { Book5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Book5_Win)) { Book5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Book4_Times)) { Book4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Book4_Bet)) { Book4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Book4_Win)) { Book4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Book3_Times)) { Book3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Book3_Bet)) { Book3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Book3_Win)) { Book3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Purse5_Times)) { Purse5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Purse5_Bet)) { Purse5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Purse5_Win)) { Purse5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Purse4_Times)) { Purse4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Purse4_Bet)) { Purse4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Purse4_Win)) { Purse4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Purse3_Times)) { Purse3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Purse3_Bet)) { Purse3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Purse3_Win)) { Purse3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Clock5_Times)) { Clock5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Clock5_Bet)) { Clock5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Clock5_Win)) { Clock5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Clock4_Times)) { Clock4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Clock4_Bet)) { Clock4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Clock4_Win)) { Clock4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Clock3_Times)) { Clock3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Clock3_Bet)) { Clock3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Clock3_Win)) { Clock3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Key5_Times)) { Key5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Key5_Bet)) { Key5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Key5_Win)) { Key5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Key4_Times)) { Key4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Key4_Bet)) { Key4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Key4_Win)) { Key4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Key3_Times)) { Key3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Key3_Bet)) { Key3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Key3_Win)) { Key3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scales5_Times)) { Scales5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scales5_Bet)) { Scales5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scales5_Win)) { Scales5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scales4_Times)) { Scales4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scales4_Bet)) { Scales4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scales4_Win)) { Scales4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scales3_Times)) { Scales3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scales3_Bet)) { Scales3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scales3_Win)) { Scales3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepPlayTimes)) { IndepPlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalBet)) { IndepTotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepTotalWin)) { IndepTotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepRoundTimes)) { IndepRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepRespinRoundTimes)) { IndepRespinRoundTimes += Convert.ToInt32(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(Respin_Times), Respin_Times.ToString());
            updata.Add(nameof(Respin_Bet), Respin_Bet.ToString());
            updata.Add(nameof(Respin_Win), Respin_Win.ToString());
            updata.Add(nameof(Respin_RoundTimes), Respin_RoundTimes.ToString());
            updata.Add(nameof(Balls_Times), Balls_Times.ToString());
            updata.Add(nameof(Balls_Bet), Balls_Bet.ToString());
            updata.Add(nameof(Balls_Win), Balls_Win.ToString());
            updata.Add(nameof(Balls1015_Times), Balls1015_Times.ToString());
            updata.Add(nameof(Balls1015_Bet), Balls1015_Bet.ToString());
            updata.Add(nameof(Balls1015_Win), Balls1015_Win.ToString());
            updata.Add(nameof(Balls0709_Times), Balls0709_Times.ToString());
            updata.Add(nameof(Balls0709_Bet), Balls0709_Bet.ToString());
            updata.Add(nameof(Balls0709_Win), Balls0709_Win.ToString());
            updata.Add(nameof(Balls0306_Times), Balls0306_Times.ToString());
            updata.Add(nameof(Balls0306_Bet), Balls0306_Bet.ToString());
            updata.Add(nameof(Balls0306_Win), Balls0306_Win.ToString());
            updata.Add(nameof(Balls0102_Times), Balls0102_Times.ToString());
            updata.Add(nameof(Balls0102_Bet), Balls0102_Bet.ToString());
            updata.Add(nameof(Balls0102_Win), Balls0102_Win.ToString());
            updata.Add(nameof(Star1_Times), Star1_Times.ToString());
            updata.Add(nameof(Star1_Bet), Star1_Bet.ToString());
            updata.Add(nameof(Star1_Win), Star1_Win.ToString());
            updata.Add(nameof(Star2_Times), Star2_Times.ToString());
            updata.Add(nameof(Star2_Bet), Star2_Bet.ToString());
            updata.Add(nameof(Star2_Win), Star2_Win.ToString());
            updata.Add(nameof(Star3_Times), Star3_Times.ToString());
            updata.Add(nameof(Star3_Bet), Star3_Bet.ToString());
            updata.Add(nameof(Star3_Win), Star3_Win.ToString());
            updata.Add(nameof(Star4_Times), Star4_Times.ToString());
            updata.Add(nameof(Star4_Bet), Star4_Bet.ToString());
            updata.Add(nameof(Star4_Win), Star4_Win.ToString());
            updata.Add(nameof(Star5_Times), Star5_Times.ToString());
            updata.Add(nameof(Star5_Bet), Star5_Bet.ToString());
            updata.Add(nameof(Star5_Win), Star5_Win.ToString());
            updata.Add(nameof(Logo5_Times), Logo5_Times.ToString());
            updata.Add(nameof(Logo5_Bet), Logo5_Bet.ToString());
            updata.Add(nameof(Logo5_Win), Logo5_Win.ToString());
            updata.Add(nameof(Logo4_Times), Logo4_Times.ToString());
            updata.Add(nameof(Logo4_Bet), Logo4_Bet.ToString());
            updata.Add(nameof(Logo4_Win), Logo4_Win.ToString());
            updata.Add(nameof(Logo3_Times), Logo3_Times.ToString());
            updata.Add(nameof(Logo3_Bet), Logo3_Bet.ToString());
            updata.Add(nameof(Logo3_Win), Logo3_Win.ToString());
            updata.Add(nameof(Scrooge5_Times), Scrooge5_Times.ToString());
            updata.Add(nameof(Scrooge5_Bet), Scrooge5_Bet.ToString());
            updata.Add(nameof(Scrooge5_Win), Scrooge5_Win.ToString());
            updata.Add(nameof(Scrooge4_Times), Scrooge4_Times.ToString());
            updata.Add(nameof(Scrooge4_Bet), Scrooge4_Bet.ToString());
            updata.Add(nameof(Scrooge4_Win), Scrooge4_Win.ToString());
            updata.Add(nameof(Scrooge3_Times), Scrooge3_Times.ToString());
            updata.Add(nameof(Scrooge3_Bet), Scrooge3_Bet.ToString());
            updata.Add(nameof(Scrooge3_Win), Scrooge3_Win.ToString());
            updata.Add(nameof(Bar3x5_Times), Bar3x5_Times.ToString());
            updata.Add(nameof(Bar3x5_Bet), Bar3x5_Bet.ToString());
            updata.Add(nameof(Bar3x5_Win), Bar3x5_Win.ToString());
            updata.Add(nameof(Bar3x4_Times), Bar3x4_Times.ToString());
            updata.Add(nameof(Bar3x4_Bet), Bar3x4_Bet.ToString());
            updata.Add(nameof(Bar3x4_Win), Bar3x4_Win.ToString());
            updata.Add(nameof(Bar3x3_Times), Bar3x3_Times.ToString());
            updata.Add(nameof(Bar3x3_Bet), Bar3x3_Bet.ToString());
            updata.Add(nameof(Bar3x3_Win), Bar3x3_Win.ToString());
            updata.Add(nameof(Bar2x5_Times), Bar2x5_Times.ToString());
            updata.Add(nameof(Bar2x5_Bet), Bar2x5_Bet.ToString());
            updata.Add(nameof(Bar2x5_Win), Bar2x5_Win.ToString());
            updata.Add(nameof(Bar2x4_Times), Bar2x4_Times.ToString());
            updata.Add(nameof(Bar2x4_Bet), Bar2x4_Bet.ToString());
            updata.Add(nameof(Bar2x4_Win), Bar2x4_Win.ToString());
            updata.Add(nameof(Bar2x3_Times), Bar2x3_Times.ToString());
            updata.Add(nameof(Bar2x3_Bet), Bar2x3_Bet.ToString());
            updata.Add(nameof(Bar2x3_Win), Bar2x3_Win.ToString());
            updata.Add(nameof(Bar1x5_Times), Bar1x5_Times.ToString());
            updata.Add(nameof(Bar1x5_Bet), Bar1x5_Bet.ToString());
            updata.Add(nameof(Bar1x5_Win), Bar1x5_Win.ToString());
            updata.Add(nameof(Bar1x4_Times), Bar1x4_Times.ToString());
            updata.Add(nameof(Bar1x4_Bet), Bar1x4_Bet.ToString());
            updata.Add(nameof(Bar1x4_Win), Bar1x4_Win.ToString());
            updata.Add(nameof(Bar1x3_Times), Bar1x3_Times.ToString());
            updata.Add(nameof(Bar1x3_Bet), Bar1x3_Bet.ToString());
            updata.Add(nameof(Bar1x3_Win), Bar1x3_Win.ToString());
            updata.Add(nameof(Book5_Times), Book5_Times.ToString());
            updata.Add(nameof(Book5_Bet), Book5_Bet.ToString());
            updata.Add(nameof(Book5_Win), Book5_Win.ToString());
            updata.Add(nameof(Book4_Times), Book4_Times.ToString());
            updata.Add(nameof(Book4_Bet), Book4_Bet.ToString());
            updata.Add(nameof(Book4_Win), Book4_Win.ToString());
            updata.Add(nameof(Book3_Times), Book3_Times.ToString());
            updata.Add(nameof(Book3_Bet), Book3_Bet.ToString());
            updata.Add(nameof(Book3_Win), Book3_Win.ToString());
            updata.Add(nameof(Purse5_Times), Purse5_Times.ToString());
            updata.Add(nameof(Purse5_Bet), Purse5_Bet.ToString());
            updata.Add(nameof(Purse5_Win), Purse5_Win.ToString());
            updata.Add(nameof(Purse4_Times), Purse4_Times.ToString());
            updata.Add(nameof(Purse4_Bet), Purse4_Bet.ToString());
            updata.Add(nameof(Purse4_Win), Purse4_Win.ToString());
            updata.Add(nameof(Purse3_Times), Purse3_Times.ToString());
            updata.Add(nameof(Purse3_Bet), Purse3_Bet.ToString());
            updata.Add(nameof(Purse3_Win), Purse3_Win.ToString());
            updata.Add(nameof(Clock5_Times), Clock5_Times.ToString());
            updata.Add(nameof(Clock5_Bet), Clock5_Bet.ToString());
            updata.Add(nameof(Clock5_Win), Clock5_Win.ToString());
            updata.Add(nameof(Clock4_Times), Clock4_Times.ToString());
            updata.Add(nameof(Clock4_Bet), Clock4_Bet.ToString());
            updata.Add(nameof(Clock4_Win), Clock4_Win.ToString());
            updata.Add(nameof(Clock3_Times), Clock3_Times.ToString());
            updata.Add(nameof(Clock3_Bet), Clock3_Bet.ToString());
            updata.Add(nameof(Clock3_Win), Clock3_Win.ToString());
            updata.Add(nameof(Key5_Times), Key5_Times.ToString());
            updata.Add(nameof(Key5_Bet), Key5_Bet.ToString());
            updata.Add(nameof(Key5_Win), Key5_Win.ToString());
            updata.Add(nameof(Key4_Times), Key4_Times.ToString());
            updata.Add(nameof(Key4_Bet), Key4_Bet.ToString());
            updata.Add(nameof(Key4_Win), Key4_Win.ToString());
            updata.Add(nameof(Key3_Times), Key3_Times.ToString());
            updata.Add(nameof(Key3_Bet), Key3_Bet.ToString());
            updata.Add(nameof(Key3_Win), Key3_Win.ToString());
            updata.Add(nameof(Scales5_Times), Scales5_Times.ToString());
            updata.Add(nameof(Scales5_Bet), Scales5_Bet.ToString());
            updata.Add(nameof(Scales5_Win), Scales5_Win.ToString());
            updata.Add(nameof(Scales4_Times), Scales4_Times.ToString());
            updata.Add(nameof(Scales4_Bet), Scales4_Bet.ToString());
            updata.Add(nameof(Scales4_Win), Scales4_Win.ToString());
            updata.Add(nameof(Scales3_Times), Scales3_Times.ToString());
            updata.Add(nameof(Scales3_Bet), Scales3_Bet.ToString());
            updata.Add(nameof(Scales3_Win), Scales3_Win.ToString());
            updata.Add(nameof(IndepPlayTimes), IndepPlayTimes.ToString());
            updata.Add(nameof(IndepTotalBet), IndepTotalBet.ToString());
            updata.Add(nameof(IndepTotalWin), IndepTotalWin.ToString());
            updata.Add(nameof(IndepRoundTimes), IndepRoundTimes.ToString());
            updata.Add(nameof(IndepRespinRoundTimes), IndepRespinRoundTimes.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            Respin_Times = Convert.ToInt32(datalist[nameof(Respin_Times)]);
            Respin_Bet = Convert.ToDouble(datalist[nameof(Respin_Bet)]);
            Respin_Win = Convert.ToDouble(datalist[nameof(Respin_Win)]);
            Respin_RoundTimes = Convert.ToInt32(datalist[nameof(Respin_RoundTimes)]);
            Balls_Times = Convert.ToInt32(datalist[nameof(Balls_Times)]);
            Balls_Bet = Convert.ToDouble(datalist[nameof(Balls_Bet)]);
            Balls_Win = Convert.ToDouble(datalist[nameof(Balls_Win)]);
            Balls1015_Times = Convert.ToInt32(datalist[nameof(Balls1015_Times)]);
            Balls1015_Bet = Convert.ToDouble(datalist[nameof(Balls1015_Bet)]);
            Balls1015_Win = Convert.ToDouble(datalist[nameof(Balls1015_Win)]);
            Balls0709_Times = Convert.ToInt32(datalist[nameof(Balls0709_Times)]);
            Balls0709_Bet = Convert.ToDouble(datalist[nameof(Balls0709_Bet)]);
            Balls0709_Win = Convert.ToDouble(datalist[nameof(Balls0709_Win)]);
            Balls0306_Times = Convert.ToInt32(datalist[nameof(Balls0306_Times)]);
            Balls0306_Bet = Convert.ToDouble(datalist[nameof(Balls0306_Bet)]);
            Balls0306_Win = Convert.ToDouble(datalist[nameof(Balls0306_Win)]);
            Balls0102_Times = Convert.ToInt32(datalist[nameof(Balls0102_Times)]);
            Balls0102_Bet = Convert.ToDouble(datalist[nameof(Balls0102_Bet)]);
            Balls0102_Win = Convert.ToDouble(datalist[nameof(Balls0102_Win)]);
            Star1_Times = Convert.ToInt32(datalist[nameof(Star1_Times)]);
            Star1_Bet = Convert.ToDouble(datalist[nameof(Star1_Bet)]);
            Star1_Win = Convert.ToDouble(datalist[nameof(Star1_Win)]);
            Star2_Times = Convert.ToInt32(datalist[nameof(Star2_Times)]);
            Star2_Bet = Convert.ToDouble(datalist[nameof(Star2_Bet)]);
            Star2_Win = Convert.ToDouble(datalist[nameof(Star2_Win)]);
            Star3_Times = Convert.ToInt32(datalist[nameof(Star3_Times)]);
            Star3_Bet = Convert.ToDouble(datalist[nameof(Star3_Bet)]);
            Star3_Win = Convert.ToDouble(datalist[nameof(Star3_Win)]);
            Star4_Times = Convert.ToInt32(datalist[nameof(Star4_Times)]);
            Star4_Bet = Convert.ToDouble(datalist[nameof(Star4_Bet)]);
            Star4_Win = Convert.ToDouble(datalist[nameof(Star4_Win)]);
            Star5_Times = Convert.ToInt32(datalist[nameof(Star5_Times)]);
            Star5_Bet = Convert.ToDouble(datalist[nameof(Star5_Bet)]);
            Star5_Win = Convert.ToDouble(datalist[nameof(Star5_Win)]);
            Logo5_Times = Convert.ToInt32(datalist[nameof(Logo5_Times)]);
            Logo5_Bet = Convert.ToDouble(datalist[nameof(Logo5_Bet)]);
            Logo5_Win = Convert.ToDouble(datalist[nameof(Logo5_Win)]);
            Logo4_Times = Convert.ToInt32(datalist[nameof(Logo4_Times)]);
            Logo4_Bet = Convert.ToDouble(datalist[nameof(Logo4_Bet)]);
            Logo4_Win = Convert.ToDouble(datalist[nameof(Logo4_Win)]);
            Logo3_Times = Convert.ToInt32(datalist[nameof(Logo3_Times)]);
            Logo3_Bet = Convert.ToDouble(datalist[nameof(Logo3_Bet)]);
            Logo3_Win = Convert.ToDouble(datalist[nameof(Logo3_Win)]);
            Scrooge5_Times = Convert.ToInt32(datalist[nameof(Scrooge5_Times)]);
            Scrooge5_Bet = Convert.ToDouble(datalist[nameof(Scrooge5_Bet)]);
            Scrooge5_Win = Convert.ToDouble(datalist[nameof(Scrooge5_Win)]);
            Scrooge4_Times = Convert.ToInt32(datalist[nameof(Scrooge4_Times)]);
            Scrooge4_Bet = Convert.ToDouble(datalist[nameof(Scrooge4_Bet)]);
            Scrooge4_Win = Convert.ToDouble(datalist[nameof(Scrooge4_Win)]);
            Scrooge3_Times = Convert.ToInt32(datalist[nameof(Scrooge3_Times)]);
            Scrooge3_Bet = Convert.ToDouble(datalist[nameof(Scrooge3_Bet)]);
            Scrooge3_Win = Convert.ToDouble(datalist[nameof(Scrooge3_Win)]);
            Bar3x5_Times = Convert.ToInt32(datalist[nameof(Bar3x5_Times)]);
            Bar3x5_Bet = Convert.ToDouble(datalist[nameof(Bar3x5_Bet)]);
            Bar3x5_Win = Convert.ToDouble(datalist[nameof(Bar3x5_Win)]);
            Bar3x4_Times = Convert.ToInt32(datalist[nameof(Bar3x4_Times)]);
            Bar3x4_Bet = Convert.ToDouble(datalist[nameof(Bar3x4_Bet)]);
            Bar3x4_Win = Convert.ToDouble(datalist[nameof(Bar3x4_Win)]);
            Bar3x3_Times = Convert.ToInt32(datalist[nameof(Bar3x3_Times)]);
            Bar3x3_Bet = Convert.ToDouble(datalist[nameof(Bar3x3_Bet)]);
            Bar3x3_Win = Convert.ToDouble(datalist[nameof(Bar3x3_Win)]);
            Bar2x5_Times = Convert.ToInt32(datalist[nameof(Bar2x5_Times)]);
            Bar2x5_Bet = Convert.ToDouble(datalist[nameof(Bar2x5_Bet)]);
            Bar2x5_Win = Convert.ToDouble(datalist[nameof(Bar2x5_Win)]);
            Bar2x4_Times = Convert.ToInt32(datalist[nameof(Bar2x4_Times)]);
            Bar2x4_Bet = Convert.ToDouble(datalist[nameof(Bar2x4_Bet)]);
            Bar2x4_Win = Convert.ToDouble(datalist[nameof(Bar2x4_Win)]);
            Bar2x3_Times = Convert.ToInt32(datalist[nameof(Bar2x3_Times)]);
            Bar2x3_Bet = Convert.ToDouble(datalist[nameof(Bar2x3_Bet)]);
            Bar2x3_Win = Convert.ToDouble(datalist[nameof(Bar2x3_Win)]);
            Bar1x5_Times = Convert.ToInt32(datalist[nameof(Bar1x5_Times)]);
            Bar1x5_Bet = Convert.ToDouble(datalist[nameof(Bar1x5_Bet)]);
            Bar1x5_Win = Convert.ToDouble(datalist[nameof(Bar1x5_Win)]);
            Bar1x4_Times = Convert.ToInt32(datalist[nameof(Bar1x4_Times)]);
            Bar1x4_Bet = Convert.ToDouble(datalist[nameof(Bar1x4_Bet)]);
            Bar1x4_Win = Convert.ToDouble(datalist[nameof(Bar1x4_Win)]);
            Bar1x3_Times = Convert.ToInt32(datalist[nameof(Bar1x3_Times)]);
            Bar1x3_Bet = Convert.ToDouble(datalist[nameof(Bar1x3_Bet)]);
            Bar1x3_Win = Convert.ToDouble(datalist[nameof(Bar1x3_Win)]);
            Book5_Times = Convert.ToInt32(datalist[nameof(Book5_Times)]);
            Book5_Bet = Convert.ToDouble(datalist[nameof(Book5_Bet)]);
            Book5_Win = Convert.ToDouble(datalist[nameof(Book5_Win)]);
            Book4_Times = Convert.ToInt32(datalist[nameof(Book4_Times)]);
            Book4_Bet = Convert.ToDouble(datalist[nameof(Book4_Bet)]);
            Book4_Win = Convert.ToDouble(datalist[nameof(Book4_Win)]);
            Book3_Times = Convert.ToInt32(datalist[nameof(Book3_Times)]);
            Book3_Bet = Convert.ToDouble(datalist[nameof(Book3_Bet)]);
            Book3_Win = Convert.ToDouble(datalist[nameof(Book3_Win)]);
            Purse5_Times = Convert.ToInt32(datalist[nameof(Purse5_Times)]);
            Purse5_Bet = Convert.ToDouble(datalist[nameof(Purse5_Bet)]);
            Purse5_Win = Convert.ToDouble(datalist[nameof(Purse5_Win)]);
            Purse4_Times = Convert.ToInt32(datalist[nameof(Purse4_Times)]);
            Purse4_Bet = Convert.ToDouble(datalist[nameof(Purse4_Bet)]);
            Purse4_Win = Convert.ToDouble(datalist[nameof(Purse4_Win)]);
            Purse3_Times = Convert.ToInt32(datalist[nameof(Purse3_Times)]);
            Purse3_Bet = Convert.ToDouble(datalist[nameof(Purse3_Bet)]);
            Purse3_Win = Convert.ToDouble(datalist[nameof(Purse3_Win)]);
            Clock5_Times = Convert.ToInt32(datalist[nameof(Clock5_Times)]);
            Clock5_Bet = Convert.ToDouble(datalist[nameof(Clock5_Bet)]);
            Clock5_Win = Convert.ToDouble(datalist[nameof(Clock5_Win)]);
            Clock4_Times = Convert.ToInt32(datalist[nameof(Clock4_Times)]);
            Clock4_Bet = Convert.ToDouble(datalist[nameof(Clock4_Bet)]);
            Clock4_Win = Convert.ToDouble(datalist[nameof(Clock4_Win)]);
            Clock3_Times = Convert.ToInt32(datalist[nameof(Clock3_Times)]);
            Clock3_Bet = Convert.ToDouble(datalist[nameof(Clock3_Bet)]);
            Clock3_Win = Convert.ToDouble(datalist[nameof(Clock3_Win)]);
            Key5_Times = Convert.ToInt32(datalist[nameof(Key5_Times)]);
            Key5_Bet = Convert.ToDouble(datalist[nameof(Key5_Bet)]);
            Key5_Win = Convert.ToDouble(datalist[nameof(Key5_Win)]);
            Key4_Times = Convert.ToInt32(datalist[nameof(Key4_Times)]);
            Key4_Bet = Convert.ToDouble(datalist[nameof(Key4_Bet)]);
            Key4_Win = Convert.ToDouble(datalist[nameof(Key4_Win)]);
            Key3_Times = Convert.ToInt32(datalist[nameof(Key3_Times)]);
            Key3_Bet = Convert.ToDouble(datalist[nameof(Key3_Bet)]);
            Key3_Win = Convert.ToDouble(datalist[nameof(Key3_Win)]);
            Scales5_Times = Convert.ToInt32(datalist[nameof(Scales5_Times)]);
            Scales5_Bet = Convert.ToDouble(datalist[nameof(Scales5_Bet)]);
            Scales5_Win = Convert.ToDouble(datalist[nameof(Scales5_Win)]);
            Scales4_Times = Convert.ToInt32(datalist[nameof(Scales4_Times)]);
            Scales4_Bet = Convert.ToDouble(datalist[nameof(Scales4_Bet)]);
            Scales4_Win = Convert.ToDouble(datalist[nameof(Scales4_Win)]);
            Scales3_Times = Convert.ToInt32(datalist[nameof(Scales3_Times)]);
            Scales3_Bet = Convert.ToDouble(datalist[nameof(Scales3_Bet)]);
            Scales3_Win = Convert.ToDouble(datalist[nameof(Scales3_Win)]);
            IndepPlayTimes = Convert.ToInt32(datalist[nameof(IndepPlayTimes)]);
            IndepTotalBet = Convert.ToDouble(datalist[nameof(IndepTotalBet)]);
            IndepTotalWin = Convert.ToDouble(datalist[nameof(IndepTotalWin)]);
            IndepRoundTimes = Convert.ToInt32(datalist[nameof(IndepRoundTimes)]);
            IndepRespinRoundTimes = Convert.ToInt32(datalist[nameof(IndepRespinRoundTimes)]);
        }
    }
}
