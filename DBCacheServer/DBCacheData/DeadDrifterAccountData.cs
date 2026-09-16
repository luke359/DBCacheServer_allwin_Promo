using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class DeadDrifterAccountData : CommonAccountData
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
        private int EvilSpirit4_Times;
        private double EvilSpirit4_Bet;
        private double EvilSpirit4_Win;
        private int EvilSpirit3_Times;
        private double EvilSpirit3_Bet;
        private double EvilSpirit3_Win;
        private int EvilSpirit2_Times;
        private double EvilSpirit2_Bet;
        private double EvilSpirit2_Win;
        private int Girl5_Times;
        private double Girl5_Bet;
        private double Girl5_Win;
        private int Girl4_Times;
        private double Girl4_Bet;
        private double Girl4_Win;
        private int Girl3_Times;
        private double Girl3_Bet;
        private double Girl3_Win;
        private int Motorcycle5_Times;
        private double Motorcycle5_Bet;
        private double Motorcycle5_Win;
        private int Motorcycle4_Times;
        private double Motorcycle4_Bet;
        private double Motorcycle4_Win;
        private int Motorcycle3_Times;
        private double Motorcycle3_Bet;
        private double Motorcycle3_Win;
        private int Gloves5_Times;
        private double Gloves5_Bet;
        private double Gloves5_Win;
        private int Gloves4_Times;
        private double Gloves4_Bet;
        private double Gloves4_Win;
        private int Gloves3_Times;
        private double Gloves3_Bet;
        private double Gloves3_Win;
        private int Collar5_Times;
        private double Collar5_Bet;
        private double Collar5_Win;
        private int Collar4_Times;
        private double Collar4_Bet;
        private double Collar4_Win;
        private int Collar3_Times;
        private double Collar3_Bet;
        private double Collar3_Win;
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
        private int IndepRespinGrand_Times;
        private int IndepRespinMajor_Times;
        private int IndepRespinMinor_Times;
        private int IndepRespinMini_Times;
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
                EvilSpirit4_Times = 0;
                EvilSpirit4_Bet = 0;
                EvilSpirit4_Win = 0;
                EvilSpirit3_Times = 0;
                EvilSpirit3_Bet = 0;
                EvilSpirit3_Win = 0;
                EvilSpirit2_Times = 0;
                EvilSpirit2_Bet = 0;
                EvilSpirit2_Win = 0;
                Girl5_Times = 0;
                Girl5_Bet = 0;
                Girl5_Win = 0;
                Girl4_Times = 0;
                Girl4_Bet = 0;
                Girl4_Win = 0;
                Girl3_Times = 0;
                Girl3_Bet = 0;
                Girl3_Win = 0;
                Motorcycle5_Times = 0;
                Motorcycle5_Bet = 0;
                Motorcycle5_Win = 0;
                Motorcycle4_Times = 0;
                Motorcycle4_Bet = 0;
                Motorcycle4_Win = 0;
                Motorcycle3_Times = 0;
                Motorcycle3_Bet = 0;
                Motorcycle3_Win = 0;
                Gloves5_Times = 0;
                Gloves5_Bet = 0;
                Gloves5_Win = 0;
                Gloves4_Times = 0;
                Gloves4_Bet = 0;
                Gloves4_Win = 0;
                Gloves3_Times = 0;
                Gloves3_Bet = 0;
                Gloves3_Win = 0;
                Collar5_Times = 0;
                Collar5_Bet = 0;
                Collar5_Win = 0;
                Collar4_Times = 0;
                Collar4_Bet = 0;
                Collar4_Win = 0;
                Collar3_Times = 0;
                Collar3_Bet = 0;
                Collar3_Win = 0;
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
            if (field == nameof(EvilSpirit4_Times)) { EvilSpirit4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EvilSpirit4_Bet)) { EvilSpirit4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(EvilSpirit4_Win)) { EvilSpirit4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(EvilSpirit3_Times)) { EvilSpirit3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EvilSpirit3_Bet)) { EvilSpirit3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(EvilSpirit3_Win)) { EvilSpirit3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(EvilSpirit2_Times)) { EvilSpirit2_Times += Convert.ToInt32(value); return; }
            if (field == nameof(EvilSpirit2_Bet)) { EvilSpirit2_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(EvilSpirit2_Win)) { EvilSpirit2_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Girl5_Times)) { Girl5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Girl5_Bet)) { Girl5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Girl5_Win)) { Girl5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Girl4_Times)) { Girl4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Girl4_Bet)) { Girl4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Girl4_Win)) { Girl4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Girl3_Times)) { Girl3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Girl3_Bet)) { Girl3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Girl3_Win)) { Girl3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle5_Times)) { Motorcycle5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Motorcycle5_Bet)) { Motorcycle5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle5_Win)) { Motorcycle5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle4_Times)) { Motorcycle4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Motorcycle4_Bet)) { Motorcycle4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle4_Win)) { Motorcycle4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle3_Times)) { Motorcycle3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Motorcycle3_Bet)) { Motorcycle3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Motorcycle3_Win)) { Motorcycle3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves5_Times)) { Gloves5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gloves5_Bet)) { Gloves5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves5_Win)) { Gloves5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves4_Times)) { Gloves4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gloves4_Bet)) { Gloves4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves4_Win)) { Gloves4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves3_Times)) { Gloves3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gloves3_Bet)) { Gloves3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gloves3_Win)) { Gloves3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Collar5_Times)) { Collar5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Collar5_Bet)) { Collar5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Collar5_Win)) { Collar5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Collar4_Times)) { Collar4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Collar4_Bet)) { Collar4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Collar4_Win)) { Collar4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Collar3_Times)) { Collar3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Collar3_Bet)) { Collar3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Collar3_Win)) { Collar3_Win += Convert.ToDouble(value); return; }
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
            if (field == nameof(IndepRespinGrand_Times)) { IndepRespinGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepRespinMajor_Times)) { IndepRespinMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepRespinMinor_Times)) { IndepRespinMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepRespinMini_Times)) { IndepRespinMini_Times += Convert.ToInt32(value); return; }
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
            updata.Add(nameof(EvilSpirit4_Times), EvilSpirit4_Times.ToString());
            updata.Add(nameof(EvilSpirit4_Bet), EvilSpirit4_Bet.ToString());
            updata.Add(nameof(EvilSpirit4_Win), EvilSpirit4_Win.ToString());
            updata.Add(nameof(EvilSpirit3_Times), EvilSpirit3_Times.ToString());
            updata.Add(nameof(EvilSpirit3_Bet), EvilSpirit3_Bet.ToString());
            updata.Add(nameof(EvilSpirit3_Win), EvilSpirit3_Win.ToString());
            updata.Add(nameof(EvilSpirit2_Times), EvilSpirit2_Times.ToString());
            updata.Add(nameof(EvilSpirit2_Bet), EvilSpirit2_Bet.ToString());
            updata.Add(nameof(EvilSpirit2_Win), EvilSpirit2_Win.ToString());
            updata.Add(nameof(Girl5_Times), Girl5_Times.ToString());
            updata.Add(nameof(Girl5_Bet), Girl5_Bet.ToString());
            updata.Add(nameof(Girl5_Win), Girl5_Win.ToString());
            updata.Add(nameof(Girl4_Times), Girl4_Times.ToString());
            updata.Add(nameof(Girl4_Bet), Girl4_Bet.ToString());
            updata.Add(nameof(Girl4_Win), Girl4_Win.ToString());
            updata.Add(nameof(Girl3_Times), Girl3_Times.ToString());
            updata.Add(nameof(Girl3_Bet), Girl3_Bet.ToString());
            updata.Add(nameof(Girl3_Win), Girl3_Win.ToString());
            updata.Add(nameof(Motorcycle5_Times), Motorcycle5_Times.ToString());
            updata.Add(nameof(Motorcycle5_Bet), Motorcycle5_Bet.ToString());
            updata.Add(nameof(Motorcycle5_Win), Motorcycle5_Win.ToString());
            updata.Add(nameof(Motorcycle4_Times), Motorcycle4_Times.ToString());
            updata.Add(nameof(Motorcycle4_Bet), Motorcycle4_Bet.ToString());
            updata.Add(nameof(Motorcycle4_Win), Motorcycle4_Win.ToString());
            updata.Add(nameof(Motorcycle3_Times), Motorcycle3_Times.ToString());
            updata.Add(nameof(Motorcycle3_Bet), Motorcycle3_Bet.ToString());
            updata.Add(nameof(Motorcycle3_Win), Motorcycle3_Win.ToString());
            updata.Add(nameof(Gloves5_Times), Gloves5_Times.ToString());
            updata.Add(nameof(Gloves5_Bet), Gloves5_Bet.ToString());
            updata.Add(nameof(Gloves5_Win), Gloves5_Win.ToString());
            updata.Add(nameof(Gloves4_Times), Gloves4_Times.ToString());
            updata.Add(nameof(Gloves4_Bet), Gloves4_Bet.ToString());
            updata.Add(nameof(Gloves4_Win), Gloves4_Win.ToString());
            updata.Add(nameof(Gloves3_Times), Gloves3_Times.ToString());
            updata.Add(nameof(Gloves3_Bet), Gloves3_Bet.ToString());
            updata.Add(nameof(Gloves3_Win), Gloves3_Win.ToString());
            updata.Add(nameof(Collar5_Times), Collar5_Times.ToString());
            updata.Add(nameof(Collar5_Bet), Collar5_Bet.ToString());
            updata.Add(nameof(Collar5_Win), Collar5_Win.ToString());
            updata.Add(nameof(Collar4_Times), Collar4_Times.ToString());
            updata.Add(nameof(Collar4_Bet), Collar4_Bet.ToString());
            updata.Add(nameof(Collar4_Win), Collar4_Win.ToString());
            updata.Add(nameof(Collar3_Times), Collar3_Times.ToString());
            updata.Add(nameof(Collar3_Bet), Collar3_Bet.ToString());
            updata.Add(nameof(Collar3_Win), Collar3_Win.ToString());
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
            updata.Add(nameof(IndepRespinGrand_Times), IndepRespinGrand_Times.ToString());
            updata.Add(nameof(IndepRespinMajor_Times), IndepRespinMajor_Times.ToString());
            updata.Add(nameof(IndepRespinMinor_Times), IndepRespinMinor_Times.ToString());
            updata.Add(nameof(IndepRespinMini_Times), IndepRespinMini_Times.ToString());
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
            EvilSpirit4_Times = Convert.ToInt32(datalist[nameof(EvilSpirit4_Times)]);
            EvilSpirit4_Bet = Convert.ToDouble(datalist[nameof(EvilSpirit4_Bet)]);
            EvilSpirit4_Win = Convert.ToDouble(datalist[nameof(EvilSpirit4_Win)]);
            EvilSpirit3_Times = Convert.ToInt32(datalist[nameof(EvilSpirit3_Times)]);
            EvilSpirit3_Bet = Convert.ToDouble(datalist[nameof(EvilSpirit3_Bet)]);
            EvilSpirit3_Win = Convert.ToDouble(datalist[nameof(EvilSpirit3_Win)]);
            EvilSpirit2_Times = Convert.ToInt32(datalist[nameof(EvilSpirit2_Times)]);
            EvilSpirit2_Bet = Convert.ToDouble(datalist[nameof(EvilSpirit2_Bet)]);
            EvilSpirit2_Win = Convert.ToDouble(datalist[nameof(EvilSpirit2_Win)]);
            Girl5_Times = Convert.ToInt32(datalist[nameof(Girl5_Times)]);
            Girl5_Bet = Convert.ToDouble(datalist[nameof(Girl5_Bet)]);
            Girl5_Win = Convert.ToDouble(datalist[nameof(Girl5_Win)]);
            Girl4_Times = Convert.ToInt32(datalist[nameof(Girl4_Times)]);
            Girl4_Bet = Convert.ToDouble(datalist[nameof(Girl4_Bet)]);
            Girl4_Win = Convert.ToDouble(datalist[nameof(Girl4_Win)]);
            Girl3_Times = Convert.ToInt32(datalist[nameof(Girl3_Times)]);
            Girl3_Bet = Convert.ToDouble(datalist[nameof(Girl3_Bet)]);
            Girl3_Win = Convert.ToDouble(datalist[nameof(Girl3_Win)]);
            Motorcycle5_Times = Convert.ToInt32(datalist[nameof(Motorcycle5_Times)]);
            Motorcycle5_Bet = Convert.ToDouble(datalist[nameof(Motorcycle5_Bet)]);
            Motorcycle5_Win = Convert.ToDouble(datalist[nameof(Motorcycle5_Win)]);
            Motorcycle4_Times = Convert.ToInt32(datalist[nameof(Motorcycle4_Times)]);
            Motorcycle4_Bet = Convert.ToDouble(datalist[nameof(Motorcycle4_Bet)]);
            Motorcycle4_Win = Convert.ToDouble(datalist[nameof(Motorcycle4_Win)]);
            Motorcycle3_Times = Convert.ToInt32(datalist[nameof(Motorcycle3_Times)]);
            Motorcycle3_Bet = Convert.ToDouble(datalist[nameof(Motorcycle3_Bet)]);
            Motorcycle3_Win = Convert.ToDouble(datalist[nameof(Motorcycle3_Win)]);
            Gloves5_Times = Convert.ToInt32(datalist[nameof(Gloves5_Times)]);
            Gloves5_Bet = Convert.ToDouble(datalist[nameof(Gloves5_Bet)]);
            Gloves5_Win = Convert.ToDouble(datalist[nameof(Gloves5_Win)]);
            Gloves4_Times = Convert.ToInt32(datalist[nameof(Gloves4_Times)]);
            Gloves4_Bet = Convert.ToDouble(datalist[nameof(Gloves4_Bet)]);
            Gloves4_Win = Convert.ToDouble(datalist[nameof(Gloves4_Win)]);
            Gloves3_Times = Convert.ToInt32(datalist[nameof(Gloves3_Times)]);
            Gloves3_Bet = Convert.ToDouble(datalist[nameof(Gloves3_Bet)]);
            Gloves3_Win = Convert.ToDouble(datalist[nameof(Gloves3_Win)]);
            Collar5_Times = Convert.ToInt32(datalist[nameof(Collar5_Times)]);
            Collar5_Bet = Convert.ToDouble(datalist[nameof(Collar5_Bet)]);
            Collar5_Win = Convert.ToDouble(datalist[nameof(Collar5_Win)]);
            Collar4_Times = Convert.ToInt32(datalist[nameof(Collar4_Times)]);
            Collar4_Bet = Convert.ToDouble(datalist[nameof(Collar4_Bet)]);
            Collar4_Win = Convert.ToDouble(datalist[nameof(Collar4_Win)]);
            Collar3_Times = Convert.ToInt32(datalist[nameof(Collar3_Times)]);
            Collar3_Bet = Convert.ToDouble(datalist[nameof(Collar3_Bet)]);
            Collar3_Win = Convert.ToDouble(datalist[nameof(Collar3_Win)]);
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
            IndepRespinGrand_Times = Convert.ToInt32(datalist[nameof(IndepRespinGrand_Times)]);
            IndepRespinMajor_Times = Convert.ToInt32(datalist[nameof(IndepRespinMajor_Times)]);
            IndepRespinMinor_Times = Convert.ToInt32(datalist[nameof(IndepRespinMinor_Times)]);
            IndepRespinMini_Times = Convert.ToInt32(datalist[nameof(IndepRespinMini_Times)]);
        }
    }
}
