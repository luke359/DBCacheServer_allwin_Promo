using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class MedusaAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int JpGrand_Times;
        private double JpGrand_Bet;
        private double JpGrand_Win;
        private int JpMajor_Times;
        private double JpMajor_Bet;
        private double JpMajor_Win;
        private int JpMinor_Times;
        private double JpMinor_Bet;
        private double JpMinor_Win;
        private int JpMini_Times;
        private double JpMini_Bet;
        private double JpMini_Win;
        private int Free_Times;
        private double Free_Bet;
        private double Free_Win;
        private int Scatter5_Times;
        private double Scatter5_Bet;
        private double Scatter5_Win;
        private int Scatter4_Times;
        private double Scatter4_Bet;
        private double Scatter4_Win;
        private int Scatter3_Times;
        private double Scatter3_Bet;
        private double Scatter3_Win;
        private int FreeApp_Times;
        private int App_Times;
        private double App_Bet;
        private double App_Win;
        private int Logo5_Times;
        private double Logo5_Bet;
        private double Logo5_Win;
        private int Logo4_Times;
        private double Logo4_Bet;
        private double Logo4_Win;
        private int Logo3_Times;
        private double Logo3_Bet;
        private double Logo3_Win;
        private int Medusa5_Times;
        private double Medusa5_Bet;
        private double Medusa5_Win;
        private int Medusa4_Times;
        private double Medusa4_Bet;
        private double Medusa4_Win;
        private int Medusa3_Times;
        private double Medusa3_Bet;
        private double Medusa3_Win;
        private int Shield5_Times;
        private double Shield5_Bet;
        private double Shield5_Win;
        private int Shield4_Times;
        private double Shield4_Bet;
        private double Shield4_Win;
        private int Shield3_Times;
        private double Shield3_Bet;
        private double Shield3_Win;
        private int HandArmor5_Times;
        private double HandArmor5_Bet;
        private double HandArmor5_Win;
        private int HandArmor4_Times;
        private double HandArmor4_Bet;
        private double HandArmor4_Win;
        private int HandArmor3_Times;
        private double HandArmor3_Bet;
        private double HandArmor3_Win;
        private int Flute5_Times;
        private double Flute5_Bet;
        private double Flute5_Win;
        private int Flute4_Times;
        private double Flute4_Bet;
        private double Flute4_Win;
        private int Flute3_Times;
        private double Flute3_Bet;
        private double Flute3_Win;
        private int Dagger5_Times;
        private double Dagger5_Bet;
        private double Dagger5_Win;
        private int Dagger4_Times;
        private double Dagger4_Bet;
        private double Dagger4_Win;
        private int Dagger3_Times;
        private double Dagger3_Bet;
        private double Dagger3_Win;
        private int Ruby5_Times;
        private double Ruby5_Bet;
        private double Ruby5_Win;
        private int Ruby4_Times;
        private double Ruby4_Bet;
        private double Ruby4_Win;
        private int Ruby3_Times;
        private double Ruby3_Bet;
        private double Ruby3_Win;
        private int Amethyst5_Times;
        private double Amethyst5_Bet;
        private double Amethyst5_Win;
        private int Amethyst4_Times;
        private double Amethyst4_Bet;
        private double Amethyst4_Win;
        private int Amethyst3_Times;
        private double Amethyst3_Bet;
        private double Amethyst3_Win;
        private int Sapphire5_Times;
        private double Sapphire5_Bet;
        private double Sapphire5_Win;
        private int Sapphire4_Times;
        private double Sapphire4_Bet;
        private double Sapphire4_Win;
        private int Sapphire3_Times;
        private double Sapphire3_Bet;
        private double Sapphire3_Win;
        private int Emerald5_Times;
        private double Emerald5_Bet;
        private double Emerald5_Win;
        private int Emerald4_Times;
        private double Emerald4_Bet;
        private double Emerald4_Win;
        private int Emerald3_Times;
        private double Emerald3_Bet;
        private double Emerald3_Win;
        private int MedusaGreen_Times;
        private double MedusaGreen_Bet;
        private double MedusaGreen_Win;
        private int MedusaBlue_Times;
        private double MedusaBlue_Bet;
        private double MedusaBlue_Win;
        private int MedusaPurple_Times;
        private double MedusaPurple_Bet;
        private double MedusaPurple_Win;
        private int MedusaRed_Times;
        private double MedusaRed_Bet;
        private double MedusaRed_Win;
        private int MedusaGold_Times;
        private double MedusaGold_Bet;
        private double MedusaGold_Win;
        private int FreeLine_Times;
        private double FreeLine_Win;
        private int FreeMedusa_Times;
        private double FreeMedusa_Win;
        private int FreeGrandMedusa_Times;
        private double FreeGrandMedusa_Win;
        private int FreeMajorMedusa_Times;
        private double FreeMajorMedusa_Win;
        private int FreeMinorMedusa_Times;
        private double FreeMinorMedusa_Win;
        private int FreeMiniMedusa_Times;
        private double FreeMiniMedusa_Win;
        private int FreeBonusMedusa_Times;
        private double FreeBonusMedusa_Win;
        private int FreeGrand_Times;
        private double FreeGrand_Win;
        private int FreeMajor_Times;
        private double FreeMajor_Win;
        private int FreeMinor_Times;
        private double FreeMinor_Win;
        private int FreeMini_Times;
        private double FreeMini_Win;
        private int FreeBonus_Times;
        private double FreeBonus_Win;
        private int IndepPlayTimes;
        private double IndepTotalBet;
        private double IndepTotalWin;
        private int IndepRoundTimes;
        private int IndepWinTimes;
        private int IndepFreeApp_Times;
        private double IndepFreeApp_Win;
        private int IndepFreeLine_Times;
        private double IndepFreeLine_Win;
        private int IndepFreeMedusa_Times;
        private double IndepFreeMedusa_Win;
        private int IndepFreeGrandMedusa_Times;
        private double IndepFreeGrandMedusa_Win;
        private int IndepFreeMajorMedusa_Times;
        private double IndepFreeMajorMedusa_Win;
        private int IndepFreeMinorMedusa_Times;
        private double IndepFreeMinorMedusa_Win;
        private int IndepFreeMiniMedusa_Times;
        private double IndepFreeMiniMedusa_Win;
        private int IndepFreeBonusMedusa_Times;
        private double IndepFreeBonusMedusa_Win;
        private int IndepFreeGrand_Times;
        private double IndepFreeGrand_Win;
        private int IndepFreeMajor_Times;
        private double IndepFreeMajor_Win;
        private int IndepFreeMinor_Times;
        private double IndepFreeMinor_Win;
        private int IndepFreeMini_Times;
        private double IndepFreeMini_Win;
        private int IndepFreeBonus_Times;
        private double IndepFreeBonus_Win;
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
                JpGrand_Times = 0;
                JpGrand_Bet = 0;
                JpGrand_Win = 0;
                JpMajor_Times = 0;
                JpMajor_Bet = 0;
                JpMajor_Win = 0;
                JpMinor_Times = 0;
                JpMinor_Bet = 0;
                JpMinor_Win = 0;
                JpMini_Times = 0;
                JpMini_Bet = 0;
                JpMini_Win = 0;
                Free_Times = 0;
                Free_Bet = 0;
                Free_Win = 0;
                Scatter5_Times = 0;
                Scatter5_Bet = 0;
                Scatter5_Win = 0;
                Scatter4_Times = 0;
                Scatter4_Bet = 0;
                Scatter4_Win = 0;
                Scatter3_Times = 0;
                Scatter3_Bet = 0;
                Scatter3_Win = 0;
                FreeApp_Times = 0;
                App_Times = 0;
                App_Bet = 0;
                App_Win = 0;
                Logo5_Times = 0;
                Logo5_Bet = 0;
                Logo5_Win = 0;
                Logo4_Times = 0;
                Logo4_Bet = 0;
                Logo4_Win = 0;
                Logo3_Times = 0;
                Logo3_Bet = 0;
                Logo3_Win = 0;
                Medusa5_Times = 0;
                Medusa5_Bet = 0;
                Medusa5_Win = 0;
                Medusa4_Times = 0;
                Medusa4_Bet = 0;
                Medusa4_Win = 0;
                Medusa3_Times = 0;
                Medusa3_Bet = 0;
                Medusa3_Win = 0;
                Shield5_Times = 0;
                Shield5_Bet = 0;
                Shield5_Win = 0;
                Shield4_Times = 0;
                Shield4_Bet = 0;
                Shield4_Win = 0;
                Shield3_Times = 0;
                Shield3_Bet = 0;
                Shield3_Win = 0;
                HandArmor5_Times = 0;
                HandArmor5_Bet = 0;
                HandArmor5_Win = 0;
                HandArmor4_Times = 0;
                HandArmor4_Bet = 0;
                HandArmor4_Win = 0;
                HandArmor3_Times = 0;
                HandArmor3_Bet = 0;
                HandArmor3_Win = 0;
                Flute5_Times = 0;
                Flute5_Bet = 0;
                Flute5_Win = 0;
                Flute4_Times = 0;
                Flute4_Bet = 0;
                Flute4_Win = 0;
                Flute3_Times = 0;
                Flute3_Bet = 0;
                Flute3_Win = 0;
                Dagger5_Times = 0;
                Dagger5_Bet = 0;
                Dagger5_Win = 0;
                Dagger4_Times = 0;
                Dagger4_Bet = 0;
                Dagger4_Win = 0;
                Dagger3_Times = 0;
                Dagger3_Bet = 0;
                Dagger3_Win = 0;
                Ruby5_Times = 0;
                Ruby5_Bet = 0;
                Ruby5_Win = 0;
                Ruby4_Times = 0;
                Ruby4_Bet = 0;
                Ruby4_Win = 0;
                Ruby3_Times = 0;
                Ruby3_Bet = 0;
                Ruby3_Win = 0;
                Amethyst5_Times = 0;
                Amethyst5_Bet = 0;
                Amethyst5_Win = 0;
                Amethyst4_Times = 0;
                Amethyst4_Bet = 0;
                Amethyst4_Win = 0;
                Amethyst3_Times = 0;
                Amethyst3_Bet = 0;
                Amethyst3_Win = 0;
                Sapphire5_Times = 0;
                Sapphire5_Bet = 0;
                Sapphire5_Win = 0;
                Sapphire4_Times = 0;
                Sapphire4_Bet = 0;
                Sapphire4_Win = 0;
                Sapphire3_Times = 0;
                Sapphire3_Bet = 0;
                Sapphire3_Win = 0;
                Emerald5_Times = 0;
                Emerald5_Bet = 0;
                Emerald5_Win = 0;
                Emerald4_Times = 0;
                Emerald4_Bet = 0;
                Emerald4_Win = 0;
                Emerald3_Times = 0;
                Emerald3_Bet = 0;
                Emerald3_Win = 0;
                MedusaGreen_Times = 0;
                MedusaGreen_Bet = 0;
                MedusaGreen_Win = 0;
                MedusaBlue_Times = 0;
                MedusaBlue_Bet = 0;
                MedusaBlue_Win = 0;
                MedusaPurple_Times = 0;
                MedusaPurple_Bet = 0;
                MedusaPurple_Win = 0;
                MedusaRed_Times = 0;
                MedusaRed_Bet = 0;
                MedusaRed_Win = 0;
                MedusaGold_Times = 0;
                MedusaGold_Bet = 0;
                MedusaGold_Win = 0;
                FreeLine_Times = 0;
                FreeLine_Win = 0;
                FreeMedusa_Times = 0;
                FreeMedusa_Win = 0;
                FreeGrandMedusa_Times = 0;
                FreeGrandMedusa_Win = 0;
                FreeMajorMedusa_Times = 0;
                FreeMajorMedusa_Win = 0;
                FreeMinorMedusa_Times = 0;
                FreeMinorMedusa_Win = 0;
                FreeMiniMedusa_Times = 0;
                FreeMiniMedusa_Win = 0;
                FreeBonusMedusa_Times = 0;
                FreeBonusMedusa_Win = 0;
                FreeGrand_Times = 0;
                FreeGrand_Win = 0;
                FreeMajor_Times = 0;
                FreeMajor_Win = 0;
                FreeMinor_Times = 0;
                FreeMinor_Win = 0;
                FreeMini_Times = 0;
                FreeMini_Win = 0;
                FreeBonus_Times = 0;
                FreeBonus_Win = 0;
            }
            if (mode == 1 || mode == 2)
            {
                IndepPlayTimes = 0;
                IndepTotalBet = 0;
                IndepTotalWin = 0;
                IndepRoundTimes = 0;
                IndepWinTimes = 0;
                IndepFreeApp_Times = 0;
                IndepFreeApp_Win = 0;
                IndepFreeLine_Times = 0;
                IndepFreeLine_Win = 0;
                IndepFreeMedusa_Times = 0;
                IndepFreeMedusa_Win = 0;
                IndepFreeGrandMedusa_Times = 0;
                IndepFreeGrandMedusa_Win = 0;
                IndepFreeMajorMedusa_Times = 0;
                IndepFreeMajorMedusa_Win = 0;
                IndepFreeMinorMedusa_Times = 0;
                IndepFreeMinorMedusa_Win = 0;
                IndepFreeMiniMedusa_Times = 0;
                IndepFreeMiniMedusa_Win = 0;
                IndepFreeBonusMedusa_Times = 0;
                IndepFreeBonusMedusa_Win = 0;
                IndepFreeGrand_Times = 0;
                IndepFreeGrand_Win = 0;
                IndepFreeMajor_Times = 0;
                IndepFreeMajor_Win = 0;
                IndepFreeMinor_Times = 0;
                IndepFreeMinor_Win = 0;
                IndepFreeMini_Times = 0;
                IndepFreeMini_Win = 0;
                IndepFreeBonus_Times = 0;
                IndepFreeBonus_Win = 0;
            }
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(JpGrand_Times)) { JpGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(JpGrand_Bet)) { JpGrand_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(JpGrand_Win)) { JpGrand_Win += Convert.ToDouble(value); return; }
            if (field == nameof(JpMajor_Times)) { JpMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(JpMajor_Bet)) { JpMajor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(JpMajor_Win)) { JpMajor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(JpMinor_Times)) { JpMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(JpMinor_Bet)) { JpMinor_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(JpMinor_Win)) { JpMinor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(JpMini_Times)) { JpMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(JpMini_Bet)) { JpMini_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(JpMini_Win)) { JpMini_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Free_Times)) { Free_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Free_Bet)) { Free_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Free_Win)) { Free_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Times)) { Scatter5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter5_Bet)) { Scatter5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Win)) { Scatter5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Times)) { Scatter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter4_Bet)) { Scatter4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Win)) { Scatter4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Times)) { Scatter3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter3_Bet)) { Scatter3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Win)) { Scatter3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeApp_Times)) { FreeApp_Times += Convert.ToInt32(value); return; }
            if (field == nameof(App_Times)) { App_Times += Convert.ToInt32(value); return; }
            if (field == nameof(App_Bet)) { App_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(App_Win)) { App_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Times)) { Logo5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo5_Bet)) { Logo5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Win)) { Logo5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Times)) { Logo4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo4_Bet)) { Logo4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Win)) { Logo4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Times)) { Logo3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo3_Bet)) { Logo3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Win)) { Logo3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Medusa5_Times)) { Medusa5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Medusa5_Bet)) { Medusa5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Medusa5_Win)) { Medusa5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Medusa4_Times)) { Medusa4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Medusa4_Bet)) { Medusa4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Medusa4_Win)) { Medusa4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Medusa3_Times)) { Medusa3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Medusa3_Bet)) { Medusa3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Medusa3_Win)) { Medusa3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Shield5_Times)) { Shield5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Shield5_Bet)) { Shield5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Shield5_Win)) { Shield5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Shield4_Times)) { Shield4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Shield4_Bet)) { Shield4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Shield4_Win)) { Shield4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Shield3_Times)) { Shield3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Shield3_Bet)) { Shield3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Shield3_Win)) { Shield3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(HandArmor5_Times)) { HandArmor5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(HandArmor5_Bet)) { HandArmor5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(HandArmor5_Win)) { HandArmor5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(HandArmor4_Times)) { HandArmor4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(HandArmor4_Bet)) { HandArmor4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(HandArmor4_Win)) { HandArmor4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(HandArmor3_Times)) { HandArmor3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(HandArmor3_Bet)) { HandArmor3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(HandArmor3_Win)) { HandArmor3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Flute5_Times)) { Flute5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Flute5_Bet)) { Flute5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Flute5_Win)) { Flute5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Flute4_Times)) { Flute4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Flute4_Bet)) { Flute4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Flute4_Win)) { Flute4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Flute3_Times)) { Flute3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Flute3_Bet)) { Flute3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Flute3_Win)) { Flute3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger5_Times)) { Dagger5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Dagger5_Bet)) { Dagger5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger5_Win)) { Dagger5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger4_Times)) { Dagger4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Dagger4_Bet)) { Dagger4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger4_Win)) { Dagger4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger3_Times)) { Dagger3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Dagger3_Bet)) { Dagger3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Dagger3_Win)) { Dagger3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby5_Times)) { Ruby5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ruby5_Bet)) { Ruby5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby5_Win)) { Ruby5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby4_Times)) { Ruby4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ruby4_Bet)) { Ruby4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby4_Win)) { Ruby4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby3_Times)) { Ruby3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Ruby3_Bet)) { Ruby3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Ruby3_Win)) { Ruby3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst5_Times)) { Amethyst5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Amethyst5_Bet)) { Amethyst5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst5_Win)) { Amethyst5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst4_Times)) { Amethyst4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Amethyst4_Bet)) { Amethyst4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst4_Win)) { Amethyst4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst3_Times)) { Amethyst3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Amethyst3_Bet)) { Amethyst3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Amethyst3_Win)) { Amethyst3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire5_Times)) { Sapphire5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Sapphire5_Bet)) { Sapphire5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire5_Win)) { Sapphire5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire4_Times)) { Sapphire4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Sapphire4_Bet)) { Sapphire4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire4_Win)) { Sapphire4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire3_Times)) { Sapphire3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Sapphire3_Bet)) { Sapphire3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Sapphire3_Win)) { Sapphire3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald5_Times)) { Emerald5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Emerald5_Bet)) { Emerald5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald5_Win)) { Emerald5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald4_Times)) { Emerald4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Emerald4_Bet)) { Emerald4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald4_Win)) { Emerald4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald3_Times)) { Emerald3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Emerald3_Bet)) { Emerald3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Emerald3_Win)) { Emerald3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaGreen_Times)) { MedusaGreen_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MedusaGreen_Bet)) { MedusaGreen_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaGreen_Win)) { MedusaGreen_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaBlue_Times)) { MedusaBlue_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MedusaBlue_Bet)) { MedusaBlue_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaBlue_Win)) { MedusaBlue_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaPurple_Times)) { MedusaPurple_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MedusaPurple_Bet)) { MedusaPurple_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaPurple_Win)) { MedusaPurple_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaRed_Times)) { MedusaRed_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MedusaRed_Bet)) { MedusaRed_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaRed_Win)) { MedusaRed_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaGold_Times)) { MedusaGold_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MedusaGold_Bet)) { MedusaGold_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MedusaGold_Win)) { MedusaGold_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeLine_Times)) { FreeLine_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeLine_Win)) { FreeLine_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMedusa_Times)) { FreeMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMedusa_Win)) { FreeMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeGrandMedusa_Times)) { FreeGrandMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeGrandMedusa_Win)) { FreeGrandMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMajorMedusa_Times)) { FreeMajorMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMajorMedusa_Win)) { FreeMajorMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMinorMedusa_Times)) { FreeMinorMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMinorMedusa_Win)) { FreeMinorMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMiniMedusa_Times)) { FreeMiniMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMiniMedusa_Win)) { FreeMiniMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeBonusMedusa_Times)) { FreeBonusMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeBonusMedusa_Win)) { FreeBonusMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeGrand_Times)) { FreeGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeGrand_Win)) { FreeGrand_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMajor_Times)) { FreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMajor_Win)) { FreeMajor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMinor_Times)) { FreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMinor_Win)) { FreeMinor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeMini_Times)) { FreeMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeMini_Win)) { FreeMini_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeBonus_Times)) { FreeBonus_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeBonus_Win)) { FreeBonus_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepPlayTimes)) { IndepPlayTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepTotalBet)) { IndepTotalBet += Convert.ToDouble(value); return; }
            if (field == nameof(IndepTotalWin)) { IndepTotalWin += Convert.ToDouble(value); return; }
            if (field == nameof(IndepRoundTimes)) { IndepRoundTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepWinTimes)) { IndepWinTimes += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeApp_Times)) { IndepFreeApp_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeApp_Win)) { IndepFreeApp_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeLine_Times)) { IndepFreeLine_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeLine_Win)) { IndepFreeLine_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMedusa_Times)) { IndepFreeMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMedusa_Win)) { IndepFreeMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeGrandMedusa_Times)) { IndepFreeGrandMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeGrandMedusa_Win)) { IndepFreeGrandMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMajorMedusa_Times)) { IndepFreeMajorMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMajorMedusa_Win)) { IndepFreeMajorMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMinorMedusa_Times)) { IndepFreeMinorMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMinorMedusa_Win)) { IndepFreeMinorMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMiniMedusa_Times)) { IndepFreeMiniMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMiniMedusa_Win)) { IndepFreeMiniMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeBonusMedusa_Times)) { IndepFreeBonusMedusa_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeBonusMedusa_Win)) { IndepFreeBonusMedusa_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeGrand_Times)) { IndepFreeGrand_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeGrand_Win)) { IndepFreeGrand_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMajor_Times)) { IndepFreeMajor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMajor_Win)) { IndepFreeMajor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMinor_Times)) { IndepFreeMinor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMinor_Win)) { IndepFreeMinor_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeMini_Times)) { IndepFreeMini_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeMini_Win)) { IndepFreeMini_Win += Convert.ToDouble(value); return; }
            if (field == nameof(IndepFreeBonus_Times)) { IndepFreeBonus_Times += Convert.ToInt32(value); return; }
            if (field == nameof(IndepFreeBonus_Win)) { IndepFreeBonus_Win += Convert.ToDouble(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(JpGrand_Times), JpGrand_Times.ToString());
            updata.Add(nameof(JpGrand_Bet), JpGrand_Bet.ToString());
            updata.Add(nameof(JpGrand_Win), JpGrand_Win.ToString());
            updata.Add(nameof(JpMajor_Times), JpMajor_Times.ToString());
            updata.Add(nameof(JpMajor_Bet), JpMajor_Bet.ToString());
            updata.Add(nameof(JpMajor_Win), JpMajor_Win.ToString());
            updata.Add(nameof(JpMinor_Times), JpMinor_Times.ToString());
            updata.Add(nameof(JpMinor_Bet), JpMinor_Bet.ToString());
            updata.Add(nameof(JpMinor_Win), JpMinor_Win.ToString());
            updata.Add(nameof(JpMini_Times), JpMini_Times.ToString());
            updata.Add(nameof(JpMini_Bet), JpMini_Bet.ToString());
            updata.Add(nameof(JpMini_Win), JpMini_Win.ToString());
            updata.Add(nameof(Free_Times), Free_Times.ToString());
            updata.Add(nameof(Free_Bet), Free_Bet.ToString());
            updata.Add(nameof(Free_Win), Free_Win.ToString());
            updata.Add(nameof(Scatter5_Times), Scatter5_Times.ToString());
            updata.Add(nameof(Scatter5_Bet), Scatter5_Bet.ToString());
            updata.Add(nameof(Scatter5_Win), Scatter5_Win.ToString());
            updata.Add(nameof(Scatter4_Times), Scatter4_Times.ToString());
            updata.Add(nameof(Scatter4_Bet), Scatter4_Bet.ToString());
            updata.Add(nameof(Scatter4_Win), Scatter4_Win.ToString());
            updata.Add(nameof(Scatter3_Times), Scatter3_Times.ToString());
            updata.Add(nameof(Scatter3_Bet), Scatter3_Bet.ToString());
            updata.Add(nameof(Scatter3_Win), Scatter3_Win.ToString());
            updata.Add(nameof(FreeApp_Times), FreeApp_Times.ToString());
            updata.Add(nameof(App_Times), App_Times.ToString());
            updata.Add(nameof(App_Bet), App_Bet.ToString());
            updata.Add(nameof(App_Win), App_Win.ToString());
            updata.Add(nameof(Logo5_Times), Logo5_Times.ToString());
            updata.Add(nameof(Logo5_Bet), Logo5_Bet.ToString());
            updata.Add(nameof(Logo5_Win), Logo5_Win.ToString());
            updata.Add(nameof(Logo4_Times), Logo4_Times.ToString());
            updata.Add(nameof(Logo4_Bet), Logo4_Bet.ToString());
            updata.Add(nameof(Logo4_Win), Logo4_Win.ToString());
            updata.Add(nameof(Logo3_Times), Logo3_Times.ToString());
            updata.Add(nameof(Logo3_Bet), Logo3_Bet.ToString());
            updata.Add(nameof(Logo3_Win), Logo3_Win.ToString());
            updata.Add(nameof(Medusa5_Times), Medusa5_Times.ToString());
            updata.Add(nameof(Medusa5_Bet), Medusa5_Bet.ToString());
            updata.Add(nameof(Medusa5_Win), Medusa5_Win.ToString());
            updata.Add(nameof(Medusa4_Times), Medusa4_Times.ToString());
            updata.Add(nameof(Medusa4_Bet), Medusa4_Bet.ToString());
            updata.Add(nameof(Medusa4_Win), Medusa4_Win.ToString());
            updata.Add(nameof(Medusa3_Times), Medusa3_Times.ToString());
            updata.Add(nameof(Medusa3_Bet), Medusa3_Bet.ToString());
            updata.Add(nameof(Medusa3_Win), Medusa3_Win.ToString());
            updata.Add(nameof(Shield5_Times), Shield5_Times.ToString());
            updata.Add(nameof(Shield5_Bet), Shield5_Bet.ToString());
            updata.Add(nameof(Shield5_Win), Shield5_Win.ToString());
            updata.Add(nameof(Shield4_Times), Shield4_Times.ToString());
            updata.Add(nameof(Shield4_Bet), Shield4_Bet.ToString());
            updata.Add(nameof(Shield4_Win), Shield4_Win.ToString());
            updata.Add(nameof(Shield3_Times), Shield3_Times.ToString());
            updata.Add(nameof(Shield3_Bet), Shield3_Bet.ToString());
            updata.Add(nameof(Shield3_Win), Shield3_Win.ToString());
            updata.Add(nameof(HandArmor5_Times), HandArmor5_Times.ToString());
            updata.Add(nameof(HandArmor5_Bet), HandArmor5_Bet.ToString());
            updata.Add(nameof(HandArmor5_Win), HandArmor5_Win.ToString());
            updata.Add(nameof(HandArmor4_Times), HandArmor4_Times.ToString());
            updata.Add(nameof(HandArmor4_Bet), HandArmor4_Bet.ToString());
            updata.Add(nameof(HandArmor4_Win), HandArmor4_Win.ToString());
            updata.Add(nameof(HandArmor3_Times), HandArmor3_Times.ToString());
            updata.Add(nameof(HandArmor3_Bet), HandArmor3_Bet.ToString());
            updata.Add(nameof(HandArmor3_Win), HandArmor3_Win.ToString());
            updata.Add(nameof(Flute5_Times), Flute5_Times.ToString());
            updata.Add(nameof(Flute5_Bet), Flute5_Bet.ToString());
            updata.Add(nameof(Flute5_Win), Flute5_Win.ToString());
            updata.Add(nameof(Flute4_Times), Flute4_Times.ToString());
            updata.Add(nameof(Flute4_Bet), Flute4_Bet.ToString());
            updata.Add(nameof(Flute4_Win), Flute4_Win.ToString());
            updata.Add(nameof(Flute3_Times), Flute3_Times.ToString());
            updata.Add(nameof(Flute3_Bet), Flute3_Bet.ToString());
            updata.Add(nameof(Flute3_Win), Flute3_Win.ToString());
            updata.Add(nameof(Dagger5_Times), Dagger5_Times.ToString());
            updata.Add(nameof(Dagger5_Bet), Dagger5_Bet.ToString());
            updata.Add(nameof(Dagger5_Win), Dagger5_Win.ToString());
            updata.Add(nameof(Dagger4_Times), Dagger4_Times.ToString());
            updata.Add(nameof(Dagger4_Bet), Dagger4_Bet.ToString());
            updata.Add(nameof(Dagger4_Win), Dagger4_Win.ToString());
            updata.Add(nameof(Dagger3_Times), Dagger3_Times.ToString());
            updata.Add(nameof(Dagger3_Bet), Dagger3_Bet.ToString());
            updata.Add(nameof(Dagger3_Win), Dagger3_Win.ToString());
            updata.Add(nameof(Ruby5_Times), Ruby5_Times.ToString());
            updata.Add(nameof(Ruby5_Bet), Ruby5_Bet.ToString());
            updata.Add(nameof(Ruby5_Win), Ruby5_Win.ToString());
            updata.Add(nameof(Ruby4_Times), Ruby4_Times.ToString());
            updata.Add(nameof(Ruby4_Bet), Ruby4_Bet.ToString());
            updata.Add(nameof(Ruby4_Win), Ruby4_Win.ToString());
            updata.Add(nameof(Ruby3_Times), Ruby3_Times.ToString());
            updata.Add(nameof(Ruby3_Bet), Ruby3_Bet.ToString());
            updata.Add(nameof(Ruby3_Win), Ruby3_Win.ToString());
            updata.Add(nameof(Amethyst5_Times), Amethyst5_Times.ToString());
            updata.Add(nameof(Amethyst5_Bet), Amethyst5_Bet.ToString());
            updata.Add(nameof(Amethyst5_Win), Amethyst5_Win.ToString());
            updata.Add(nameof(Amethyst4_Times), Amethyst4_Times.ToString());
            updata.Add(nameof(Amethyst4_Bet), Amethyst4_Bet.ToString());
            updata.Add(nameof(Amethyst4_Win), Amethyst4_Win.ToString());
            updata.Add(nameof(Amethyst3_Times), Amethyst3_Times.ToString());
            updata.Add(nameof(Amethyst3_Bet), Amethyst3_Bet.ToString());
            updata.Add(nameof(Amethyst3_Win), Amethyst3_Win.ToString());
            updata.Add(nameof(Sapphire5_Times), Sapphire5_Times.ToString());
            updata.Add(nameof(Sapphire5_Bet), Sapphire5_Bet.ToString());
            updata.Add(nameof(Sapphire5_Win), Sapphire5_Win.ToString());
            updata.Add(nameof(Sapphire4_Times), Sapphire4_Times.ToString());
            updata.Add(nameof(Sapphire4_Bet), Sapphire4_Bet.ToString());
            updata.Add(nameof(Sapphire4_Win), Sapphire4_Win.ToString());
            updata.Add(nameof(Sapphire3_Times), Sapphire3_Times.ToString());
            updata.Add(nameof(Sapphire3_Bet), Sapphire3_Bet.ToString());
            updata.Add(nameof(Sapphire3_Win), Sapphire3_Win.ToString());
            updata.Add(nameof(Emerald5_Times), Emerald5_Times.ToString());
            updata.Add(nameof(Emerald5_Bet), Emerald5_Bet.ToString());
            updata.Add(nameof(Emerald5_Win), Emerald5_Win.ToString());
            updata.Add(nameof(Emerald4_Times), Emerald4_Times.ToString());
            updata.Add(nameof(Emerald4_Bet), Emerald4_Bet.ToString());
            updata.Add(nameof(Emerald4_Win), Emerald4_Win.ToString());
            updata.Add(nameof(Emerald3_Times), Emerald3_Times.ToString());
            updata.Add(nameof(Emerald3_Bet), Emerald3_Bet.ToString());
            updata.Add(nameof(Emerald3_Win), Emerald3_Win.ToString());
            updata.Add(nameof(MedusaGreen_Times), MedusaGreen_Times.ToString());
            updata.Add(nameof(MedusaGreen_Bet), MedusaGreen_Bet.ToString());
            updata.Add(nameof(MedusaGreen_Win), MedusaGreen_Win.ToString());
            updata.Add(nameof(MedusaBlue_Times), MedusaBlue_Times.ToString());
            updata.Add(nameof(MedusaBlue_Bet), MedusaBlue_Bet.ToString());
            updata.Add(nameof(MedusaBlue_Win), MedusaBlue_Win.ToString());
            updata.Add(nameof(MedusaPurple_Times), MedusaPurple_Times.ToString());
            updata.Add(nameof(MedusaPurple_Bet), MedusaPurple_Bet.ToString());
            updata.Add(nameof(MedusaPurple_Win), MedusaPurple_Win.ToString());
            updata.Add(nameof(MedusaRed_Times), MedusaRed_Times.ToString());
            updata.Add(nameof(MedusaRed_Bet), MedusaRed_Bet.ToString());
            updata.Add(nameof(MedusaRed_Win), MedusaRed_Win.ToString());
            updata.Add(nameof(MedusaGold_Times), MedusaGold_Times.ToString());
            updata.Add(nameof(MedusaGold_Bet), MedusaGold_Bet.ToString());
            updata.Add(nameof(MedusaGold_Win), MedusaGold_Win.ToString());
            updata.Add(nameof(FreeLine_Times), FreeLine_Times.ToString());
            updata.Add(nameof(FreeLine_Win), FreeLine_Win.ToString());
            updata.Add(nameof(FreeMedusa_Times), FreeMedusa_Times.ToString());
            updata.Add(nameof(FreeMedusa_Win), FreeMedusa_Win.ToString());
            updata.Add(nameof(FreeGrandMedusa_Times), FreeGrandMedusa_Times.ToString());
            updata.Add(nameof(FreeGrandMedusa_Win), FreeGrandMedusa_Win.ToString());
            updata.Add(nameof(FreeMajorMedusa_Times), FreeMajorMedusa_Times.ToString());
            updata.Add(nameof(FreeMajorMedusa_Win), FreeMajorMedusa_Win.ToString());
            updata.Add(nameof(FreeMinorMedusa_Times), FreeMinorMedusa_Times.ToString());
            updata.Add(nameof(FreeMinorMedusa_Win), FreeMinorMedusa_Win.ToString());
            updata.Add(nameof(FreeMiniMedusa_Times), FreeMiniMedusa_Times.ToString());
            updata.Add(nameof(FreeMiniMedusa_Win), FreeMiniMedusa_Win.ToString());
            updata.Add(nameof(FreeBonusMedusa_Times), FreeBonusMedusa_Times.ToString());
            updata.Add(nameof(FreeBonusMedusa_Win), FreeBonusMedusa_Win.ToString());
            updata.Add(nameof(FreeGrand_Times), FreeGrand_Times.ToString());
            updata.Add(nameof(FreeGrand_Win), FreeGrand_Win.ToString());
            updata.Add(nameof(FreeMajor_Times), FreeMajor_Times.ToString());
            updata.Add(nameof(FreeMajor_Win), FreeMajor_Win.ToString());
            updata.Add(nameof(FreeMinor_Times), FreeMinor_Times.ToString());
            updata.Add(nameof(FreeMinor_Win), FreeMinor_Win.ToString());
            updata.Add(nameof(FreeMini_Times), FreeMini_Times.ToString());
            updata.Add(nameof(FreeMini_Win), FreeMini_Win.ToString());
            updata.Add(nameof(FreeBonus_Times), FreeBonus_Times.ToString());
            updata.Add(nameof(FreeBonus_Win), FreeBonus_Win.ToString());
            updata.Add(nameof(IndepPlayTimes), IndepPlayTimes.ToString());
            updata.Add(nameof(IndepTotalBet), IndepTotalBet.ToString());
            updata.Add(nameof(IndepTotalWin), IndepTotalWin.ToString());
            updata.Add(nameof(IndepRoundTimes), IndepRoundTimes.ToString());
            updata.Add(nameof(IndepWinTimes), IndepWinTimes.ToString());
            updata.Add(nameof(IndepFreeApp_Times), IndepFreeApp_Times.ToString());
            updata.Add(nameof(IndepFreeApp_Win), IndepFreeApp_Win.ToString());
            updata.Add(nameof(IndepFreeLine_Times), IndepFreeLine_Times.ToString());
            updata.Add(nameof(IndepFreeLine_Win), IndepFreeLine_Win.ToString());
            updata.Add(nameof(IndepFreeMedusa_Times), IndepFreeMedusa_Times.ToString());
            updata.Add(nameof(IndepFreeMedusa_Win), IndepFreeMedusa_Win.ToString());
            updata.Add(nameof(IndepFreeGrandMedusa_Times), IndepFreeGrandMedusa_Times.ToString());
            updata.Add(nameof(IndepFreeGrandMedusa_Win), IndepFreeGrandMedusa_Win.ToString());
            updata.Add(nameof(IndepFreeMajorMedusa_Times), IndepFreeMajorMedusa_Times.ToString());
            updata.Add(nameof(IndepFreeMajorMedusa_Win), IndepFreeMajorMedusa_Win.ToString());
            updata.Add(nameof(IndepFreeMinorMedusa_Times), IndepFreeMinorMedusa_Times.ToString());
            updata.Add(nameof(IndepFreeMinorMedusa_Win), IndepFreeMinorMedusa_Win.ToString());
            updata.Add(nameof(IndepFreeMiniMedusa_Times), IndepFreeMiniMedusa_Times.ToString());
            updata.Add(nameof(IndepFreeMiniMedusa_Win), IndepFreeMiniMedusa_Win.ToString());
            updata.Add(nameof(IndepFreeBonusMedusa_Times), IndepFreeBonusMedusa_Times.ToString());
            updata.Add(nameof(IndepFreeBonusMedusa_Win), IndepFreeBonusMedusa_Win.ToString());
            updata.Add(nameof(IndepFreeGrand_Times), IndepFreeGrand_Times.ToString());
            updata.Add(nameof(IndepFreeGrand_Win), IndepFreeGrand_Win.ToString());
            updata.Add(nameof(IndepFreeMajor_Times), IndepFreeMajor_Times.ToString());
            updata.Add(nameof(IndepFreeMajor_Win), IndepFreeMajor_Win.ToString());
            updata.Add(nameof(IndepFreeMinor_Times), IndepFreeMinor_Times.ToString());
            updata.Add(nameof(IndepFreeMinor_Win), IndepFreeMinor_Win.ToString());
            updata.Add(nameof(IndepFreeMini_Times), IndepFreeMini_Times.ToString());
            updata.Add(nameof(IndepFreeMini_Win), IndepFreeMini_Win.ToString());
            updata.Add(nameof(IndepFreeBonus_Times), IndepFreeBonus_Times.ToString());
            updata.Add(nameof(IndepFreeBonus_Win), IndepFreeBonus_Win.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            JpGrand_Times = Convert.ToInt32(datalist[nameof(JpGrand_Times)]);
            JpGrand_Bet = Convert.ToDouble(datalist[nameof(JpGrand_Bet)]);
            JpGrand_Win = Convert.ToDouble(datalist[nameof(JpGrand_Win)]);
            JpMajor_Times = Convert.ToInt32(datalist[nameof(JpMajor_Times)]);
            JpMajor_Bet = Convert.ToDouble(datalist[nameof(JpMajor_Bet)]);
            JpMajor_Win = Convert.ToDouble(datalist[nameof(JpMajor_Win)]);
            JpMinor_Times = Convert.ToInt32(datalist[nameof(JpMinor_Times)]);
            JpMinor_Bet = Convert.ToDouble(datalist[nameof(JpMinor_Bet)]);
            JpMinor_Win = Convert.ToDouble(datalist[nameof(JpMinor_Win)]);
            JpMini_Times = Convert.ToInt32(datalist[nameof(JpMini_Times)]);
            JpMini_Bet = Convert.ToDouble(datalist[nameof(JpMini_Bet)]);
            JpMini_Win = Convert.ToDouble(datalist[nameof(JpMini_Win)]);
            Free_Times = Convert.ToInt32(datalist[nameof(Free_Times)]);
            Free_Bet = Convert.ToDouble(datalist[nameof(Free_Bet)]);
            Free_Win = Convert.ToDouble(datalist[nameof(Free_Win)]);
            Scatter5_Times = Convert.ToInt32(datalist[nameof(Scatter5_Times)]);
            Scatter5_Bet = Convert.ToDouble(datalist[nameof(Scatter5_Bet)]);
            Scatter5_Win = Convert.ToDouble(datalist[nameof(Scatter5_Win)]);
            Scatter4_Times = Convert.ToInt32(datalist[nameof(Scatter4_Times)]);
            Scatter4_Bet = Convert.ToDouble(datalist[nameof(Scatter4_Bet)]);
            Scatter4_Win = Convert.ToDouble(datalist[nameof(Scatter4_Win)]);
            Scatter3_Times = Convert.ToInt32(datalist[nameof(Scatter3_Times)]);
            Scatter3_Bet = Convert.ToDouble(datalist[nameof(Scatter3_Bet)]);
            Scatter3_Win = Convert.ToDouble(datalist[nameof(Scatter3_Win)]);
            FreeApp_Times = Convert.ToInt32(datalist[nameof(FreeApp_Times)]);
            App_Times = Convert.ToInt32(datalist[nameof(App_Times)]);
            App_Bet = Convert.ToDouble(datalist[nameof(App_Bet)]);
            App_Win = Convert.ToDouble(datalist[nameof(App_Win)]);
            Logo5_Times = Convert.ToInt32(datalist[nameof(Logo5_Times)]);
            Logo5_Bet = Convert.ToDouble(datalist[nameof(Logo5_Bet)]);
            Logo5_Win = Convert.ToDouble(datalist[nameof(Logo5_Win)]);
            Logo4_Times = Convert.ToInt32(datalist[nameof(Logo4_Times)]);
            Logo4_Bet = Convert.ToDouble(datalist[nameof(Logo4_Bet)]);
            Logo4_Win = Convert.ToDouble(datalist[nameof(Logo4_Win)]);
            Logo3_Times = Convert.ToInt32(datalist[nameof(Logo3_Times)]);
            Logo3_Bet = Convert.ToDouble(datalist[nameof(Logo3_Bet)]);
            Logo3_Win = Convert.ToDouble(datalist[nameof(Logo3_Win)]);
            Medusa5_Times = Convert.ToInt32(datalist[nameof(Medusa5_Times)]);
            Medusa5_Bet = Convert.ToDouble(datalist[nameof(Medusa5_Bet)]);
            Medusa5_Win = Convert.ToDouble(datalist[nameof(Medusa5_Win)]);
            Medusa4_Times = Convert.ToInt32(datalist[nameof(Medusa4_Times)]);
            Medusa4_Bet = Convert.ToDouble(datalist[nameof(Medusa4_Bet)]);
            Medusa4_Win = Convert.ToDouble(datalist[nameof(Medusa4_Win)]);
            Medusa3_Times = Convert.ToInt32(datalist[nameof(Medusa3_Times)]);
            Medusa3_Bet = Convert.ToDouble(datalist[nameof(Medusa3_Bet)]);
            Medusa3_Win = Convert.ToDouble(datalist[nameof(Medusa3_Win)]);
            Shield5_Times = Convert.ToInt32(datalist[nameof(Shield5_Times)]);
            Shield5_Bet = Convert.ToDouble(datalist[nameof(Shield5_Bet)]);
            Shield5_Win = Convert.ToDouble(datalist[nameof(Shield5_Win)]);
            Shield4_Times = Convert.ToInt32(datalist[nameof(Shield4_Times)]);
            Shield4_Bet = Convert.ToDouble(datalist[nameof(Shield4_Bet)]);
            Shield4_Win = Convert.ToDouble(datalist[nameof(Shield4_Win)]);
            Shield3_Times = Convert.ToInt32(datalist[nameof(Shield3_Times)]);
            Shield3_Bet = Convert.ToDouble(datalist[nameof(Shield3_Bet)]);
            Shield3_Win = Convert.ToDouble(datalist[nameof(Shield3_Win)]);
            HandArmor5_Times = Convert.ToInt32(datalist[nameof(HandArmor5_Times)]);
            HandArmor5_Bet = Convert.ToDouble(datalist[nameof(HandArmor5_Bet)]);
            HandArmor5_Win = Convert.ToDouble(datalist[nameof(HandArmor5_Win)]);
            HandArmor4_Times = Convert.ToInt32(datalist[nameof(HandArmor4_Times)]);
            HandArmor4_Bet = Convert.ToDouble(datalist[nameof(HandArmor4_Bet)]);
            HandArmor4_Win = Convert.ToDouble(datalist[nameof(HandArmor4_Win)]);
            HandArmor3_Times = Convert.ToInt32(datalist[nameof(HandArmor3_Times)]);
            HandArmor3_Bet = Convert.ToDouble(datalist[nameof(HandArmor3_Bet)]);
            HandArmor3_Win = Convert.ToDouble(datalist[nameof(HandArmor3_Win)]);
            Flute5_Times = Convert.ToInt32(datalist[nameof(Flute5_Times)]);
            Flute5_Bet = Convert.ToDouble(datalist[nameof(Flute5_Bet)]);
            Flute5_Win = Convert.ToDouble(datalist[nameof(Flute5_Win)]);
            Flute4_Times = Convert.ToInt32(datalist[nameof(Flute4_Times)]);
            Flute4_Bet = Convert.ToDouble(datalist[nameof(Flute4_Bet)]);
            Flute4_Win = Convert.ToDouble(datalist[nameof(Flute4_Win)]);
            Flute3_Times = Convert.ToInt32(datalist[nameof(Flute3_Times)]);
            Flute3_Bet = Convert.ToDouble(datalist[nameof(Flute3_Bet)]);
            Flute3_Win = Convert.ToDouble(datalist[nameof(Flute3_Win)]);
            Dagger5_Times = Convert.ToInt32(datalist[nameof(Dagger5_Times)]);
            Dagger5_Bet = Convert.ToDouble(datalist[nameof(Dagger5_Bet)]);
            Dagger5_Win = Convert.ToDouble(datalist[nameof(Dagger5_Win)]);
            Dagger4_Times = Convert.ToInt32(datalist[nameof(Dagger4_Times)]);
            Dagger4_Bet = Convert.ToDouble(datalist[nameof(Dagger4_Bet)]);
            Dagger4_Win = Convert.ToDouble(datalist[nameof(Dagger4_Win)]);
            Dagger3_Times = Convert.ToInt32(datalist[nameof(Dagger3_Times)]);
            Dagger3_Bet = Convert.ToDouble(datalist[nameof(Dagger3_Bet)]);
            Dagger3_Win = Convert.ToDouble(datalist[nameof(Dagger3_Win)]);
            Ruby5_Times = Convert.ToInt32(datalist[nameof(Ruby5_Times)]);
            Ruby5_Bet = Convert.ToDouble(datalist[nameof(Ruby5_Bet)]);
            Ruby5_Win = Convert.ToDouble(datalist[nameof(Ruby5_Win)]);
            Ruby4_Times = Convert.ToInt32(datalist[nameof(Ruby4_Times)]);
            Ruby4_Bet = Convert.ToDouble(datalist[nameof(Ruby4_Bet)]);
            Ruby4_Win = Convert.ToDouble(datalist[nameof(Ruby4_Win)]);
            Ruby3_Times = Convert.ToInt32(datalist[nameof(Ruby3_Times)]);
            Ruby3_Bet = Convert.ToDouble(datalist[nameof(Ruby3_Bet)]);
            Ruby3_Win = Convert.ToDouble(datalist[nameof(Ruby3_Win)]);
            Amethyst5_Times = Convert.ToInt32(datalist[nameof(Amethyst5_Times)]);
            Amethyst5_Bet = Convert.ToDouble(datalist[nameof(Amethyst5_Bet)]);
            Amethyst5_Win = Convert.ToDouble(datalist[nameof(Amethyst5_Win)]);
            Amethyst4_Times = Convert.ToInt32(datalist[nameof(Amethyst4_Times)]);
            Amethyst4_Bet = Convert.ToDouble(datalist[nameof(Amethyst4_Bet)]);
            Amethyst4_Win = Convert.ToDouble(datalist[nameof(Amethyst4_Win)]);
            Amethyst3_Times = Convert.ToInt32(datalist[nameof(Amethyst3_Times)]);
            Amethyst3_Bet = Convert.ToDouble(datalist[nameof(Amethyst3_Bet)]);
            Amethyst3_Win = Convert.ToDouble(datalist[nameof(Amethyst3_Win)]);
            Sapphire5_Times = Convert.ToInt32(datalist[nameof(Sapphire5_Times)]);
            Sapphire5_Bet = Convert.ToDouble(datalist[nameof(Sapphire5_Bet)]);
            Sapphire5_Win = Convert.ToDouble(datalist[nameof(Sapphire5_Win)]);
            Sapphire4_Times = Convert.ToInt32(datalist[nameof(Sapphire4_Times)]);
            Sapphire4_Bet = Convert.ToDouble(datalist[nameof(Sapphire4_Bet)]);
            Sapphire4_Win = Convert.ToDouble(datalist[nameof(Sapphire4_Win)]);
            Sapphire3_Times = Convert.ToInt32(datalist[nameof(Sapphire3_Times)]);
            Sapphire3_Bet = Convert.ToDouble(datalist[nameof(Sapphire3_Bet)]);
            Sapphire3_Win = Convert.ToDouble(datalist[nameof(Sapphire3_Win)]);
            Emerald5_Times = Convert.ToInt32(datalist[nameof(Emerald5_Times)]);
            Emerald5_Bet = Convert.ToDouble(datalist[nameof(Emerald5_Bet)]);
            Emerald5_Win = Convert.ToDouble(datalist[nameof(Emerald5_Win)]);
            Emerald4_Times = Convert.ToInt32(datalist[nameof(Emerald4_Times)]);
            Emerald4_Bet = Convert.ToDouble(datalist[nameof(Emerald4_Bet)]);
            Emerald4_Win = Convert.ToDouble(datalist[nameof(Emerald4_Win)]);
            Emerald3_Times = Convert.ToInt32(datalist[nameof(Emerald3_Times)]);
            Emerald3_Bet = Convert.ToDouble(datalist[nameof(Emerald3_Bet)]);
            Emerald3_Win = Convert.ToDouble(datalist[nameof(Emerald3_Win)]);
            MedusaGreen_Times = Convert.ToInt32(datalist[nameof(MedusaGreen_Times)]);
            MedusaGreen_Bet = Convert.ToDouble(datalist[nameof(MedusaGreen_Bet)]);
            MedusaGreen_Win = Convert.ToDouble(datalist[nameof(MedusaGreen_Win)]);
            MedusaBlue_Times = Convert.ToInt32(datalist[nameof(MedusaBlue_Times)]);
            MedusaBlue_Bet = Convert.ToDouble(datalist[nameof(MedusaBlue_Bet)]);
            MedusaBlue_Win = Convert.ToDouble(datalist[nameof(MedusaBlue_Win)]);
            MedusaPurple_Times = Convert.ToInt32(datalist[nameof(MedusaPurple_Times)]);
            MedusaPurple_Bet = Convert.ToDouble(datalist[nameof(MedusaPurple_Bet)]);
            MedusaPurple_Win = Convert.ToDouble(datalist[nameof(MedusaPurple_Win)]);
            MedusaRed_Times = Convert.ToInt32(datalist[nameof(MedusaRed_Times)]);
            MedusaRed_Bet = Convert.ToDouble(datalist[nameof(MedusaRed_Bet)]);
            MedusaRed_Win = Convert.ToDouble(datalist[nameof(MedusaRed_Win)]);
            MedusaGold_Times = Convert.ToInt32(datalist[nameof(MedusaGold_Times)]);
            MedusaGold_Bet = Convert.ToDouble(datalist[nameof(MedusaGold_Bet)]);
            MedusaGold_Win = Convert.ToDouble(datalist[nameof(MedusaGold_Win)]);
            FreeLine_Times = Convert.ToInt32(datalist[nameof(FreeLine_Times)]);
            FreeLine_Win = Convert.ToDouble(datalist[nameof(FreeLine_Win)]);
            FreeMedusa_Times = Convert.ToInt32(datalist[nameof(FreeMedusa_Times)]);
            FreeMedusa_Win = Convert.ToDouble(datalist[nameof(FreeMedusa_Win)]);
            FreeGrandMedusa_Times = Convert.ToInt32(datalist[nameof(FreeGrandMedusa_Times)]);
            FreeGrandMedusa_Win = Convert.ToDouble(datalist[nameof(FreeGrandMedusa_Win)]);
            FreeMajorMedusa_Times = Convert.ToInt32(datalist[nameof(FreeMajorMedusa_Times)]);
            FreeMajorMedusa_Win = Convert.ToDouble(datalist[nameof(FreeMajorMedusa_Win)]);
            FreeMinorMedusa_Times = Convert.ToInt32(datalist[nameof(FreeMinorMedusa_Times)]);
            FreeMinorMedusa_Win = Convert.ToDouble(datalist[nameof(FreeMinorMedusa_Win)]);
            FreeMiniMedusa_Times = Convert.ToInt32(datalist[nameof(FreeMiniMedusa_Times)]);
            FreeMiniMedusa_Win = Convert.ToDouble(datalist[nameof(FreeMiniMedusa_Win)]);
            FreeBonusMedusa_Times = Convert.ToInt32(datalist[nameof(FreeBonusMedusa_Times)]);
            FreeBonusMedusa_Win = Convert.ToDouble(datalist[nameof(FreeBonusMedusa_Win)]);
            FreeGrand_Times = Convert.ToInt32(datalist[nameof(FreeGrand_Times)]);
            FreeGrand_Win = Convert.ToDouble(datalist[nameof(FreeGrand_Win)]);
            FreeMajor_Times = Convert.ToInt32(datalist[nameof(FreeMajor_Times)]);
            FreeMajor_Win = Convert.ToDouble(datalist[nameof(FreeMajor_Win)]);
            FreeMinor_Times = Convert.ToInt32(datalist[nameof(FreeMinor_Times)]);
            FreeMinor_Win = Convert.ToDouble(datalist[nameof(FreeMinor_Win)]);
            FreeMini_Times = Convert.ToInt32(datalist[nameof(FreeMini_Times)]);
            FreeMini_Win = Convert.ToDouble(datalist[nameof(FreeMini_Win)]);
            FreeBonus_Times = Convert.ToInt32(datalist[nameof(FreeBonus_Times)]);
            FreeBonus_Win = Convert.ToDouble(datalist[nameof(FreeBonus_Win)]);
            IndepPlayTimes = Convert.ToInt32(datalist[nameof(IndepPlayTimes)]);
            IndepTotalBet = Convert.ToDouble(datalist[nameof(IndepTotalBet)]);
            IndepTotalWin = Convert.ToDouble(datalist[nameof(IndepTotalWin)]);
            IndepRoundTimes = Convert.ToInt32(datalist[nameof(IndepRoundTimes)]);
            IndepWinTimes = Convert.ToInt32(datalist[nameof(IndepWinTimes)]);
            IndepFreeApp_Times = Convert.ToInt32(datalist[nameof(IndepFreeApp_Times)]);
            IndepFreeApp_Win = Convert.ToDouble(datalist[nameof(IndepFreeApp_Win)]);
            IndepFreeLine_Times = Convert.ToInt32(datalist[nameof(IndepFreeLine_Times)]);
            IndepFreeLine_Win = Convert.ToDouble(datalist[nameof(IndepFreeLine_Win)]);
            IndepFreeMedusa_Times = Convert.ToInt32(datalist[nameof(IndepFreeMedusa_Times)]);
            IndepFreeMedusa_Win = Convert.ToDouble(datalist[nameof(IndepFreeMedusa_Win)]);
            IndepFreeGrandMedusa_Times = Convert.ToInt32(datalist[nameof(IndepFreeGrandMedusa_Times)]);
            IndepFreeGrandMedusa_Win = Convert.ToDouble(datalist[nameof(IndepFreeGrandMedusa_Win)]);
            IndepFreeMajorMedusa_Times = Convert.ToInt32(datalist[nameof(IndepFreeMajorMedusa_Times)]);
            IndepFreeMajorMedusa_Win = Convert.ToDouble(datalist[nameof(IndepFreeMajorMedusa_Win)]);
            IndepFreeMinorMedusa_Times = Convert.ToInt32(datalist[nameof(IndepFreeMinorMedusa_Times)]);
            IndepFreeMinorMedusa_Win = Convert.ToDouble(datalist[nameof(IndepFreeMinorMedusa_Win)]);
            IndepFreeMiniMedusa_Times = Convert.ToInt32(datalist[nameof(IndepFreeMiniMedusa_Times)]);
            IndepFreeMiniMedusa_Win = Convert.ToDouble(datalist[nameof(IndepFreeMiniMedusa_Win)]);
            IndepFreeBonusMedusa_Times = Convert.ToInt32(datalist[nameof(IndepFreeBonusMedusa_Times)]);
            IndepFreeBonusMedusa_Win = Convert.ToDouble(datalist[nameof(IndepFreeBonusMedusa_Win)]);
            IndepFreeGrand_Times = Convert.ToInt32(datalist[nameof(IndepFreeGrand_Times)]);
            IndepFreeGrand_Win = Convert.ToDouble(datalist[nameof(IndepFreeGrand_Win)]);
            IndepFreeMajor_Times = Convert.ToInt32(datalist[nameof(IndepFreeMajor_Times)]);
            IndepFreeMajor_Win = Convert.ToDouble(datalist[nameof(IndepFreeMajor_Win)]);
            IndepFreeMinor_Times = Convert.ToInt32(datalist[nameof(IndepFreeMinor_Times)]);
            IndepFreeMinor_Win = Convert.ToDouble(datalist[nameof(IndepFreeMinor_Win)]);
            IndepFreeMini_Times = Convert.ToInt32(datalist[nameof(IndepFreeMini_Times)]);
            IndepFreeMini_Win = Convert.ToDouble(datalist[nameof(IndepFreeMini_Win)]);
            IndepFreeBonus_Times = Convert.ToInt32(datalist[nameof(IndepFreeBonus_Times)]);
            IndepFreeBonus_Win = Convert.ToDouble(datalist[nameof(IndepFreeBonus_Win)]);
        }
    }
}
