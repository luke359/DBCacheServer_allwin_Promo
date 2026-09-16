using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class DoubleFortuneAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int Respin_Times;
        private double Respin_Bet;
        private double Respin_Win;
        private int RespinGrand_Times;
        private int RespinMajor_Times;
        private int RespinMinor_Times;
        private int RespinMini_Times;
        private int Sticky_Times;
        private double Sticky_Bet;
        private double Sticky_Win;
        private int StickyRespin_Times;
        private double StickyRespin_Win;
        private int StickyRespinGrand_Times;
        private int StickyRespinMajor_Times;
        private int StickyRespinMinor_Times;
        private int StickyRespinMini_Times;
        private int Bonus_Times;
        private double Bonus_Bet;
        private double Bonus_Win;
        private int BonusRespin_Times;
        private double BonusRespin_Win;
        private int BonusRespinGrand_Times;
        private int BonusRespinMajor_Times;
        private int BonusRespinMinor_Times;
        private int BonusRespinMini_Times;
        private int StickyBonus_Times;
        private double StickyBonus_Bet;
        private double StickyBonus_Win;
        private int StickyBonusRespin_Times;
        private double StickyBonusRespin_Win;
        private int StickyBonusRespinGrand_Times;
        private int StickyBonusRespinMajor_Times;
        private int StickyBonusRespinMinor_Times;
        private int StickyBonusRespinMini_Times;
        private int EvilSpirit5_Times;
        private double EvilSpirit5_Bet;
        private double EvilSpirit5_Win;
        private int Girl5_Times;
        private double Girl5_Bet;
        private double Girl5_Win;
        private int Motorcycle5_Times;
        private double Motorcycle5_Bet;
        private double Motorcycle5_Win;
        private int Gloves5_Times;
        private double Gloves5_Bet;
        private double Gloves5_Win;
        private int Collar5_Times;
        private double Collar5_Bet;
        private double Collar5_Win;
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
        private int IndepPlayTimes;
        private double IndepTotalBet;
        private double IndepTotalWin;
        private int IndepRespinGrand_Times;
        private int IndepRespinMajor_Times;
        private int IndepRespinMinor_Times;
        private int IndepRespinMini_Times;
        private int IndepS1PlayTimes;
        private double IndepS1TotalBet;
        private double IndepS1TotalWin;
        private int IndepS1RespinGrand_Times;
        private int IndepS1RespinMajor_Times;
        private int IndepS1RespinMinor_Times;
        private int IndepS1RespinMini_Times;
        private int IndepS2PlayTimes;
        private double IndepS2TotalBet;
        private double IndepS2TotalWin;
        private int IndepS2RespinGrand_Times;
        private int IndepS2RespinMajor_Times;
        private int IndepS2RespinMinor_Times;
        private int IndepS2RespinMini_Times;
        #endregion

        /// <summary>清除額外押注</summary>
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
                Respin_Times = 0;
                Respin_Bet = 0;
                Respin_Win = 0;
                RespinGrand_Times = 0;
                RespinMajor_Times = 0;
                RespinMinor_Times = 0;
                RespinMini_Times = 0;
                Sticky_Times = 0;
                Sticky_Bet = 0;
                Sticky_Win = 0;
                StickyRespin_Times = 0;
                StickyRespin_Win = 0;
                StickyRespinGrand_Times = 0;
                StickyRespinMajor_Times = 0;
                StickyRespinMinor_Times = 0;
                StickyRespinMini_Times = 0;
                Bonus_Times = 0;
                Bonus_Bet = 0;
                Bonus_Win = 0;
                BonusRespin_Times = 0;
                BonusRespin_Win = 0;
                BonusRespinGrand_Times = 0;
                BonusRespinMajor_Times = 0;
                BonusRespinMinor_Times = 0;
                BonusRespinMini_Times = 0;
                StickyBonus_Times = 0;
                StickyBonus_Bet = 0;
                StickyBonus_Win = 0;
                StickyBonusRespin_Times = 0;
                StickyBonusRespin_Win = 0;
                StickyBonusRespinGrand_Times = 0;
                StickyBonusRespinMajor_Times = 0;
                StickyBonusRespinMinor_Times = 0;
                StickyBonusRespinMini_Times = 0;
                EvilSpirit5_Times = 0;
                EvilSpirit5_Bet = 0;
                EvilSpirit5_Win = 0;
                Girl5_Times = 0;
                Girl5_Bet = 0;
                Girl5_Win = 0;
                Motorcycle5_Times = 0;
                Motorcycle5_Bet = 0;
                Motorcycle5_Win = 0;
                Gloves5_Times = 0;
                Gloves5_Bet = 0;
                Gloves5_Win = 0;
                Collar5_Times = 0;
                Collar5_Bet = 0;
                Collar5_Win = 0;
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
            }
            if (mode == 1 || mode == 2)
            {
                IndepPlayTimes = 0;
                IndepTotalBet = 0;
                IndepTotalWin = 0;
                IndepRespinGrand_Times = 0;
                IndepRespinMajor_Times = 0;
                IndepRespinMinor_Times = 0;
                IndepRespinMini_Times = 0;
                IndepS1PlayTimes = 0;
                IndepS1TotalBet = 0;
                IndepS1TotalWin = 0;
                IndepS1RespinGrand_Times = 0;
                IndepS1RespinMajor_Times = 0;
                IndepS1RespinMinor_Times = 0;
                IndepS1RespinMini_Times = 0;
                IndepS2PlayTimes = 0;
                IndepS2TotalBet = 0;
                IndepS2TotalWin = 0;
                IndepS2RespinGrand_Times = 0;
                IndepS2RespinMajor_Times = 0;
                IndepS2RespinMinor_Times = 0;
                IndepS2RespinMini_Times = 0;
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(Respin_Times)) { Respin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Respin_Bet)) { Respin_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Respin_Win)) { Respin_Win += Convert.ToDouble(value); return; }
            if (field == nameof(RespinGrand_Times)) { RespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RespinMajor_Times)) { RespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RespinMinor_Times)) { RespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RespinMini_Times)) { RespinMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Sticky_Times)) { Sticky_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Sticky_Bet)) { Sticky_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Sticky_Win)) { Sticky_Win += Convert.ToDouble(value); return; }
            if (field == nameof(StickyRespin_Times)) { StickyRespin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyRespin_Win)) { StickyRespin_Win += Convert.ToDouble(value); return; }
            if (field == nameof(StickyRespinGrand_Times)) { StickyRespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyRespinMajor_Times)) { StickyRespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyRespinMinor_Times)) { StickyRespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyRespinMini_Times)) { StickyRespinMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bonus_Times)) { Bonus_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bonus_Bet)) { Bonus_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bonus_Win)) { Bonus_Win += Convert.ToDouble(value); return; }
            if (field == nameof(BonusRespin_Times)) { BonusRespin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(BonusRespin_Win)) { BonusRespin_Win += Convert.ToDouble(value); return; }
            if (field == nameof(BonusRespinGrand_Times)) { BonusRespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(BonusRespinMajor_Times)) { BonusRespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(BonusRespinMinor_Times)) { BonusRespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(BonusRespinMini_Times)) { BonusRespinMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyBonus_Times)) { StickyBonus_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyBonus_Bet)) { StickyBonus_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(StickyBonus_Win)) { StickyBonus_Win += Convert.ToDouble(value); return; }
            if (field == nameof(StickyBonusRespin_Times)) { StickyBonusRespin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyBonusRespin_Win)) { StickyBonusRespin_Win += Convert.ToDouble(value); return; }
            if (field == nameof(StickyBonusRespinGrand_Times)) { StickyBonusRespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyBonusRespinMajor_Times)) { StickyBonusRespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyBonusRespinMinor_Times)) { StickyBonusRespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(StickyBonusRespinMini_Times)) { StickyBonusRespinMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EvilSpirit5_Times)) { EvilSpirit5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EvilSpirit5_Bet)) { EvilSpirit5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(EvilSpirit5_Win)) { EvilSpirit5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Girl5_Times)) { Girl5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Girl5_Bet)) { Girl5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Girl5_Win)) { Girl5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle5_Times)) { Motorcycle5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Motorcycle5_Bet)) { Motorcycle5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle5_Win)) { Motorcycle5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves5_Times)) { Gloves5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gloves5_Bet)) { Gloves5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves5_Win)) { Gloves5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Collar5_Times)) { Collar5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Collar5_Bet)) { Collar5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Collar5_Win)) { Collar5_Win += Convert.ToDouble(value); return; }
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
            if (field == nameof(IndepPlayTimes)) { IndepPlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalBet)) { IndepTotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepTotalWin)) { IndepTotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepRespinGrand_Times)) { IndepRespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepRespinMajor_Times)) { IndepRespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepRespinMinor_Times)) { IndepRespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepRespinMini_Times)) { IndepRespinMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1PlayTimes)) { IndepS1PlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1TotalBet)) { IndepS1TotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1TotalWin)) { IndepS1TotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS1RespinGrand_Times)) { IndepS1RespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1RespinMajor_Times)) { IndepS1RespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1RespinMinor_Times)) { IndepS1RespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS1RespinMini_Times)) { IndepS1RespinMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2PlayTimes)) { IndepS2PlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2TotalBet)) { IndepS2TotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2TotalWin)) { IndepS2TotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepS2RespinGrand_Times)) { IndepS2RespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2RespinMajor_Times)) { IndepS2RespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2RespinMinor_Times)) { IndepS2RespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepS2RespinMini_Times)) { IndepS2RespinMini_Times += Convert.ToInt32(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(Respin_Times), Respin_Times.ToString());
            updata.Add(nameof(Respin_Bet), Respin_Bet.ToString());
            updata.Add(nameof(Respin_Win), Respin_Win.ToString());
            updata.Add(nameof(RespinGrand_Times), RespinGrand_Times.ToString());
            updata.Add(nameof(RespinMajor_Times), RespinMajor_Times.ToString());
            updata.Add(nameof(RespinMinor_Times), RespinMinor_Times.ToString());
            updata.Add(nameof(RespinMini_Times), RespinMini_Times.ToString());
            updata.Add(nameof(Sticky_Times), Sticky_Times.ToString());
            updata.Add(nameof(Sticky_Bet), Sticky_Bet.ToString());
            updata.Add(nameof(Sticky_Win), Sticky_Win.ToString());
            updata.Add(nameof(StickyRespin_Times), StickyRespin_Times.ToString());
            updata.Add(nameof(StickyRespin_Win), StickyRespin_Win.ToString());
            updata.Add(nameof(StickyRespinGrand_Times), StickyRespinGrand_Times.ToString());
            updata.Add(nameof(StickyRespinMajor_Times), StickyRespinMajor_Times.ToString());
            updata.Add(nameof(StickyRespinMinor_Times), StickyRespinMinor_Times.ToString());
            updata.Add(nameof(StickyRespinMini_Times), StickyRespinMini_Times.ToString());
            updata.Add(nameof(Bonus_Times), Bonus_Times.ToString());
            updata.Add(nameof(Bonus_Bet), Bonus_Bet.ToString());
            updata.Add(nameof(Bonus_Win), Bonus_Win.ToString());
            updata.Add(nameof(BonusRespin_Times), BonusRespin_Times.ToString());
            updata.Add(nameof(BonusRespin_Win), BonusRespin_Win.ToString());
            updata.Add(nameof(BonusRespinGrand_Times), BonusRespinGrand_Times.ToString());
            updata.Add(nameof(BonusRespinMajor_Times), BonusRespinMajor_Times.ToString());
            updata.Add(nameof(BonusRespinMinor_Times), BonusRespinMinor_Times.ToString());
            updata.Add(nameof(BonusRespinMini_Times), BonusRespinMini_Times.ToString());
            updata.Add(nameof(StickyBonus_Times), StickyBonus_Times.ToString());
            updata.Add(nameof(StickyBonus_Bet), StickyBonus_Bet.ToString());
            updata.Add(nameof(StickyBonus_Win), StickyBonus_Win.ToString());
            updata.Add(nameof(StickyBonusRespin_Times), StickyBonusRespin_Times.ToString());
            updata.Add(nameof(StickyBonusRespin_Win), StickyBonusRespin_Win.ToString());
            updata.Add(nameof(StickyBonusRespinGrand_Times), StickyBonusRespinGrand_Times.ToString());
            updata.Add(nameof(StickyBonusRespinMajor_Times), StickyBonusRespinMajor_Times.ToString());
            updata.Add(nameof(StickyBonusRespinMinor_Times), StickyBonusRespinMinor_Times.ToString());
            updata.Add(nameof(StickyBonusRespinMini_Times), StickyBonusRespinMini_Times.ToString());
            updata.Add(nameof(EvilSpirit5_Times), EvilSpirit5_Times.ToString());
            updata.Add(nameof(EvilSpirit5_Bet), EvilSpirit5_Bet.ToString());
            updata.Add(nameof(EvilSpirit5_Win), EvilSpirit5_Win.ToString());
            updata.Add(nameof(Girl5_Times), Girl5_Times.ToString());
            updata.Add(nameof(Girl5_Bet), Girl5_Bet.ToString());
            updata.Add(nameof(Girl5_Win), Girl5_Win.ToString());
            updata.Add(nameof(Motorcycle5_Times), Motorcycle5_Times.ToString());
            updata.Add(nameof(Motorcycle5_Bet), Motorcycle5_Bet.ToString());
            updata.Add(nameof(Motorcycle5_Win), Motorcycle5_Win.ToString());
            updata.Add(nameof(Gloves5_Times), Gloves5_Times.ToString());
            updata.Add(nameof(Gloves5_Bet), Gloves5_Bet.ToString());
            updata.Add(nameof(Gloves5_Win), Gloves5_Win.ToString());
            updata.Add(nameof(Collar5_Times), Collar5_Times.ToString());
            updata.Add(nameof(Collar5_Bet), Collar5_Bet.ToString());
            updata.Add(nameof(Collar5_Win), Collar5_Win.ToString());
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
            updata.Add(nameof(IndepPlayTimes), IndepPlayTimes.ToString());
            updata.Add(nameof(IndepTotalBet), IndepTotalBet.ToString());
            updata.Add(nameof(IndepTotalWin), IndepTotalWin.ToString());
            updata.Add(nameof(IndepRespinGrand_Times), IndepRespinGrand_Times.ToString());
            updata.Add(nameof(IndepRespinMajor_Times), IndepRespinMajor_Times.ToString());
            updata.Add(nameof(IndepRespinMinor_Times), IndepRespinMinor_Times.ToString());
            updata.Add(nameof(IndepRespinMini_Times), IndepRespinMini_Times.ToString());
            updata.Add(nameof(IndepS1PlayTimes), IndepS1PlayTimes.ToString());
            updata.Add(nameof(IndepS1TotalBet), IndepS1TotalBet.ToString());
            updata.Add(nameof(IndepS1TotalWin), IndepS1TotalWin.ToString());
            updata.Add(nameof(IndepS1RespinGrand_Times), IndepS1RespinGrand_Times.ToString());
            updata.Add(nameof(IndepS1RespinMajor_Times), IndepS1RespinMajor_Times.ToString());
            updata.Add(nameof(IndepS1RespinMinor_Times), IndepS1RespinMinor_Times.ToString());
            updata.Add(nameof(IndepS1RespinMini_Times), IndepS1RespinMini_Times.ToString());
            updata.Add(nameof(IndepS2PlayTimes), IndepS2PlayTimes.ToString());
            updata.Add(nameof(IndepS2TotalBet), IndepS2TotalBet.ToString());
            updata.Add(nameof(IndepS2TotalWin), IndepS2TotalWin.ToString());
            updata.Add(nameof(IndepS2RespinGrand_Times), IndepS2RespinGrand_Times.ToString());
            updata.Add(nameof(IndepS2RespinMajor_Times), IndepS2RespinMajor_Times.ToString());
            updata.Add(nameof(IndepS2RespinMinor_Times), IndepS2RespinMinor_Times.ToString());
            updata.Add(nameof(IndepS2RespinMini_Times), IndepS2RespinMini_Times.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            Respin_Times = Convert.ToInt32(datalist[nameof(Respin_Times)]);
            Respin_Bet = Convert.ToDouble(datalist[nameof(Respin_Bet)]);
            Respin_Win = Convert.ToDouble(datalist[nameof(Respin_Win)]);
            RespinGrand_Times = Convert.ToInt32(datalist[nameof(RespinGrand_Times)]);
            RespinMajor_Times = Convert.ToInt32(datalist[nameof(RespinMajor_Times)]);
            RespinMinor_Times = Convert.ToInt32(datalist[nameof(RespinMinor_Times)]);
            RespinMini_Times = Convert.ToInt32(datalist[nameof(RespinMini_Times)]);
            Sticky_Times = Convert.ToInt32(datalist[nameof(Sticky_Times)]);
            Sticky_Bet = Convert.ToDouble(datalist[nameof(Sticky_Bet)]);
            Sticky_Win = Convert.ToDouble(datalist[nameof(Sticky_Win)]);
            StickyRespin_Times = Convert.ToInt32(datalist[nameof(StickyRespin_Times)]);
            StickyRespin_Win = Convert.ToDouble(datalist[nameof(StickyRespin_Win)]);
            StickyRespinGrand_Times = Convert.ToInt32(datalist[nameof(StickyRespinGrand_Times)]);
            StickyRespinMajor_Times = Convert.ToInt32(datalist[nameof(StickyRespinMajor_Times)]);
            StickyRespinMinor_Times = Convert.ToInt32(datalist[nameof(StickyRespinMinor_Times)]);
            StickyRespinMini_Times = Convert.ToInt32(datalist[nameof(StickyRespinMini_Times)]);
            Bonus_Times = Convert.ToInt32(datalist[nameof(Bonus_Times)]);
            Bonus_Bet = Convert.ToDouble(datalist[nameof(Bonus_Bet)]);
            Bonus_Win = Convert.ToDouble(datalist[nameof(Bonus_Win)]);
            BonusRespin_Times = Convert.ToInt32(datalist[nameof(BonusRespin_Times)]);
            BonusRespin_Win = Convert.ToDouble(datalist[nameof(BonusRespin_Win)]);
            BonusRespinGrand_Times = Convert.ToInt32(datalist[nameof(BonusRespinGrand_Times)]);
            BonusRespinMajor_Times = Convert.ToInt32(datalist[nameof(BonusRespinMajor_Times)]);
            BonusRespinMinor_Times = Convert.ToInt32(datalist[nameof(BonusRespinMinor_Times)]);
            BonusRespinMini_Times = Convert.ToInt32(datalist[nameof(BonusRespinMini_Times)]);
            StickyBonus_Times = Convert.ToInt32(datalist[nameof(StickyBonus_Times)]);
            StickyBonus_Bet = Convert.ToDouble(datalist[nameof(StickyBonus_Bet)]);
            StickyBonus_Win = Convert.ToDouble(datalist[nameof(StickyBonus_Win)]);
            StickyBonusRespin_Times = Convert.ToInt32(datalist[nameof(StickyBonusRespin_Times)]);
            StickyBonusRespin_Win = Convert.ToDouble(datalist[nameof(StickyBonusRespin_Win)]);
            StickyBonusRespinGrand_Times = Convert.ToInt32(datalist[nameof(StickyBonusRespinGrand_Times)]);
            StickyBonusRespinMajor_Times = Convert.ToInt32(datalist[nameof(StickyBonusRespinMajor_Times)]);
            StickyBonusRespinMinor_Times = Convert.ToInt32(datalist[nameof(StickyBonusRespinMinor_Times)]);
            StickyBonusRespinMini_Times = Convert.ToInt32(datalist[nameof(StickyBonusRespinMini_Times)]);
            EvilSpirit5_Times = Convert.ToInt32(datalist[nameof(EvilSpirit5_Times)]);
            EvilSpirit5_Bet = Convert.ToDouble(datalist[nameof(EvilSpirit5_Bet)]);
            EvilSpirit5_Win = Convert.ToDouble(datalist[nameof(EvilSpirit5_Win)]);
            Girl5_Times = Convert.ToInt32(datalist[nameof(Girl5_Times)]);
            Girl5_Bet = Convert.ToDouble(datalist[nameof(Girl5_Bet)]);
            Girl5_Win = Convert.ToDouble(datalist[nameof(Girl5_Win)]);
            Motorcycle5_Times = Convert.ToInt32(datalist[nameof(Motorcycle5_Times)]);
            Motorcycle5_Bet = Convert.ToDouble(datalist[nameof(Motorcycle5_Bet)]);
            Motorcycle5_Win = Convert.ToDouble(datalist[nameof(Motorcycle5_Win)]);
            Gloves5_Times = Convert.ToInt32(datalist[nameof(Gloves5_Times)]);
            Gloves5_Bet = Convert.ToDouble(datalist[nameof(Gloves5_Bet)]);
            Gloves5_Win = Convert.ToDouble(datalist[nameof(Gloves5_Win)]);
            Collar5_Times = Convert.ToInt32(datalist[nameof(Collar5_Times)]);
            Collar5_Bet = Convert.ToDouble(datalist[nameof(Collar5_Bet)]);
            Collar5_Win = Convert.ToDouble(datalist[nameof(Collar5_Win)]);
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
            IndepPlayTimes = Convert.ToInt32(datalist[nameof(IndepPlayTimes)]);
            IndepTotalBet = Convert.ToDouble(datalist[nameof(IndepTotalBet)]);
            IndepTotalWin = Convert.ToDouble(datalist[nameof(IndepTotalWin)]);
            IndepRespinGrand_Times = Convert.ToInt32(datalist[nameof(IndepRespinGrand_Times)]);
            IndepRespinMajor_Times = Convert.ToInt32(datalist[nameof(IndepRespinMajor_Times)]);
            IndepRespinMinor_Times = Convert.ToInt32(datalist[nameof(IndepRespinMinor_Times)]);
            IndepRespinMini_Times = Convert.ToInt32(datalist[nameof(IndepRespinMini_Times)]);
            IndepS1PlayTimes = Convert.ToInt32(datalist[nameof(IndepS1PlayTimes)]);
            IndepS1TotalBet = Convert.ToDouble(datalist[nameof(IndepS1TotalBet)]);
            IndepS1TotalWin = Convert.ToDouble(datalist[nameof(IndepS1TotalWin)]);
            IndepS1RespinGrand_Times = Convert.ToInt32(datalist[nameof(IndepS1RespinGrand_Times)]);
            IndepS1RespinMajor_Times = Convert.ToInt32(datalist[nameof(IndepS1RespinMajor_Times)]);
            IndepS1RespinMinor_Times = Convert.ToInt32(datalist[nameof(IndepS1RespinMinor_Times)]);
            IndepS1RespinMini_Times = Convert.ToInt32(datalist[nameof(IndepS1RespinMini_Times)]);
            IndepS2PlayTimes = Convert.ToInt32(datalist[nameof(IndepS2PlayTimes)]);
            IndepS2TotalBet = Convert.ToDouble(datalist[nameof(IndepS2TotalBet)]);
            IndepS2TotalWin = Convert.ToDouble(datalist[nameof(IndepS2TotalWin)]);
            IndepS2RespinGrand_Times = Convert.ToInt32(datalist[nameof(IndepS2RespinGrand_Times)]);
            IndepS2RespinMajor_Times = Convert.ToInt32(datalist[nameof(IndepS2RespinMajor_Times)]);
            IndepS2RespinMinor_Times = Convert.ToInt32(datalist[nameof(IndepS2RespinMinor_Times)]);
            IndepS2RespinMini_Times = Convert.ToInt32(datalist[nameof(IndepS2RespinMini_Times)]);
        }
    }
}
