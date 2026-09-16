using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class PanJinlianAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        public int BonusBonus_Times;
        public int Bonus5_Times;
        public double Bonus5_Bet;
        public double Bonus5_Win;
        public int Bonus4_Times;
        public double Bonus4_Bet;
        public double Bonus4_Win;
        public int Bonus3_Times;
        public double Bonus3_Bet;
        public double Bonus3_Win;
        public int WildAll_Times;
        public double WildAll_Bet;
        public double WildAll_Win;
        public int Wild5_Times;
        public double Wild5_Bet;
        public double Wild5_Win;
        public int PanJinlianAll_Times;
        public double PanJinlianAll_Bet;
        public double PanJinlianAll_Win;
        public int PanJinlian5_Times;
        public double PanJinlian5_Bet;
        public double PanJinlian5_Win;
        public int PanJinlian4_Times;
        public double PanJinlian4_Bet;
        public double PanJinlian4_Win;
        public int PanJinlian3_Times;
        public double PanJinlian3_Bet;
        public double PanJinlian3_Win;
        public int XimenQingAll_Times;
        public double XimenQingAll_Bet;
        public double XimenQingAll_Win;
        public int XimenQing5_Times;
        public double XimenQing5_Bet;
        public double XimenQing5_Win;
        public int XimenQing4_Times;
        public double XimenQing4_Bet;
        public double XimenQing4_Win;
        public int XimenQing3_Times;
        public double XimenQing3_Bet;
        public double XimenQing3_Win;
        public int WuDalangAll_Times;
        public double WuDalangAll_Bet;
        public double WuDalangAll_Win;
        public int WuDalang5_Times;
        public double WuDalang5_Bet;
        public double WuDalang5_Win;
        public int WuDalang4_Times;
        public double WuDalang4_Bet;
        public double WuDalang4_Win;
        public int WuDalang3_Times;
        public double WuDalang3_Bet;
        public double WuDalang3_Win;
        public int WuSongAll_Times;
        public double WuSongAll_Bet;
        public double WuSongAll_Win;
        public int WuSong5_Times;
        public double WuSong5_Bet;
        public double WuSong5_Win;
        public int WuSong4_Times;
        public double WuSong4_Bet;
        public double WuSong4_Win;
        public int WuSong3_Times;
        public double WuSong3_Bet;
        public double WuSong3_Win;
        public int FlagonAll_Times;
        public double FlagonAll_Bet;
        public double FlagonAll_Win;
        public int Flagon5_Times;
        public double Flagon5_Bet;
        public double Flagon5_Win;
        public int Flagon4_Times;
        public double Flagon4_Bet;
        public double Flagon4_Win;
        public int Flagon3_Times;
        public double Flagon3_Bet;
        public double Flagon3_Win;
        public int ShoesAll_Times;
        public double ShoesAll_Bet;
        public double ShoesAll_Win;
        public int Shoes5_Times;
        public double Shoes5_Bet;
        public double Shoes5_Win;
        public int Shoes4_Times;
        public double Shoes4_Bet;
        public double Shoes4_Win;
        public int Shoes3_Times;
        public double Shoes3_Bet;
        public double Shoes3_Win;
        public int FanAll_Times;
        public double FanAll_Bet;
        public double FanAll_Win;
        public int Fan5_Times;
        public double Fan5_Bet;
        public double Fan5_Win;
        public int Fan4_Times;
        public double Fan4_Bet;
        public double Fan4_Win;
        public int Fan3_Times;
        public double Fan3_Bet;
        public double Fan3_Win;
        public int HairpinAll_Times;
        public double HairpinAll_Bet;
        public double HairpinAll_Win;
        public int Hairpin5_Times;
        public double Hairpin5_Bet;
        public double Hairpin5_Win;
        public int Hairpin4_Times;
        public double Hairpin4_Bet;
        public double Hairpin4_Win;
        public int Hairpin3_Times;
        public double Hairpin3_Bet;
        public double Hairpin3_Win;
        #endregion

        /// <summary>清除額外押注</summary>
        public override void ClearCacheGameExPlay()
        {
            //update = true;
        }

        /// <summary>清除內存(mode:0=常規, 1=獨立買, 2=全部)</summary>
        public override void ClearCacheGame(int mode)
        {
            BonusBonus_Times = 0;
            Bonus5_Times = 0;
            Bonus5_Bet = 0;
            Bonus5_Win = 0;
            Bonus4_Times = 0;
            Bonus4_Bet = 0;
            Bonus4_Win = 0;
            Bonus3_Times = 0;
            Bonus3_Bet = 0;
            Bonus3_Win = 0;
            WildAll_Times = 0;
            WildAll_Bet = 0;
            WildAll_Win = 0;
            Wild5_Times = 0;
            Wild5_Bet = 0;
            Wild5_Win = 0;
            PanJinlianAll_Times = 0;
            PanJinlianAll_Bet = 0;
            PanJinlianAll_Win = 0;
            PanJinlian5_Times = 0;
            PanJinlian5_Bet = 0;
            PanJinlian5_Win = 0;
            PanJinlian4_Times = 0;
            PanJinlian4_Bet = 0;
            PanJinlian4_Win = 0;
            PanJinlian3_Times = 0;
            PanJinlian3_Bet = 0;
            PanJinlian3_Win = 0;
            XimenQingAll_Times = 0;
            XimenQingAll_Bet = 0;
            XimenQingAll_Win = 0;
            XimenQing5_Times = 0;
            XimenQing5_Bet = 0;
            XimenQing5_Win = 0;
            XimenQing4_Times = 0;
            XimenQing4_Bet = 0;
            XimenQing4_Win = 0;
            XimenQing3_Times = 0;
            XimenQing3_Bet = 0;
            XimenQing3_Win = 0;
            WuDalangAll_Times = 0;
            WuDalangAll_Bet = 0;
            WuDalangAll_Win = 0;
            WuDalang5_Times = 0;
            WuDalang5_Bet = 0;
            WuDalang5_Win = 0;
            WuDalang4_Times = 0;
            WuDalang4_Bet = 0;
            WuDalang4_Win = 0;
            WuDalang3_Times = 0;
            WuDalang3_Bet = 0;
            WuDalang3_Win = 0;
            WuSongAll_Times = 0;
            WuSongAll_Bet = 0;
            WuSongAll_Win = 0;
            WuSong5_Times = 0;
            WuSong5_Bet = 0;
            WuSong5_Win = 0;
            WuSong4_Times = 0;
            WuSong4_Bet = 0;
            WuSong4_Win = 0;
            WuSong3_Times = 0;
            WuSong3_Bet = 0;
            WuSong3_Win = 0;
            FlagonAll_Times = 0;
            FlagonAll_Bet = 0;
            FlagonAll_Win = 0;
            Flagon5_Times = 0;
            Flagon5_Bet = 0;
            Flagon5_Win = 0;
            Flagon4_Times = 0;
            Flagon4_Bet = 0;
            Flagon4_Win = 0;
            Flagon3_Times = 0;
            Flagon3_Bet = 0;
            Flagon3_Win = 0;
            ShoesAll_Times = 0;
            ShoesAll_Bet = 0;
            ShoesAll_Win = 0;
            Shoes5_Times = 0;
            Shoes5_Bet = 0;
            Shoes5_Win = 0;
            Shoes4_Times = 0;
            Shoes4_Bet = 0;
            Shoes4_Win = 0;
            Shoes3_Times = 0;
            Shoes3_Bet = 0;
            Shoes3_Win = 0;
            FanAll_Times = 0;
            FanAll_Bet = 0;
            FanAll_Win = 0;
            Fan5_Times = 0;
            Fan5_Bet = 0;
            Fan5_Win = 0;
            Fan4_Times = 0;
            Fan4_Bet = 0;
            Fan4_Win = 0;
            Fan3_Times = 0;
            Fan3_Bet = 0;
            Fan3_Win = 0;
            HairpinAll_Times = 0;
            HairpinAll_Bet = 0;
            HairpinAll_Win = 0;
            Hairpin5_Times = 0;
            Hairpin5_Bet = 0;
            Hairpin5_Win = 0;
            Hairpin4_Times = 0;
            Hairpin4_Bet = 0;
            Hairpin4_Win = 0;
            Hairpin3_Times = 0;
            Hairpin3_Bet = 0;
            Hairpin3_Win = 0;
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(BonusBonus_Times)) { BonusBonus_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bonus5_Times)) { Bonus5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bonus5_Bet)) { Bonus5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bonus5_Win)) { Bonus5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bonus4_Times)) { Bonus4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bonus4_Bet)) { Bonus4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bonus4_Win)) { Bonus4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Bonus3_Times)) { Bonus3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Bonus3_Bet)) { Bonus3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Bonus3_Win)) { Bonus3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WildAll_Times)) { WildAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WildAll_Bet)) { WildAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WildAll_Win)) { WildAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Wild5_Times)) { Wild5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Wild5_Bet)) { Wild5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Wild5_Win)) { Wild5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlianAll_Times)) { PanJinlianAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PanJinlianAll_Bet)) { PanJinlianAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlianAll_Win)) { PanJinlianAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlian5_Times)) { PanJinlian5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PanJinlian5_Bet)) { PanJinlian5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlian5_Win)) { PanJinlian5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlian4_Times)) { PanJinlian4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PanJinlian4_Bet)) { PanJinlian4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlian4_Win)) { PanJinlian4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlian3_Times)) { PanJinlian3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PanJinlian3_Bet)) { PanJinlian3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PanJinlian3_Win)) { PanJinlian3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQingAll_Times)) { XimenQingAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(XimenQingAll_Bet)) { XimenQingAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQingAll_Win)) { XimenQingAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQing5_Times)) { XimenQing5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(XimenQing5_Bet)) { XimenQing5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQing5_Win)) { XimenQing5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQing4_Times)) { XimenQing4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(XimenQing4_Bet)) { XimenQing4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQing4_Win)) { XimenQing4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQing3_Times)) { XimenQing3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(XimenQing3_Bet)) { XimenQing3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(XimenQing3_Win)) { XimenQing3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalangAll_Times)) { WuDalangAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuDalangAll_Bet)) { WuDalangAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalangAll_Win)) { WuDalangAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalang5_Times)) { WuDalang5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuDalang5_Bet)) { WuDalang5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalang5_Win)) { WuDalang5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalang4_Times)) { WuDalang4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuDalang4_Bet)) { WuDalang4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalang4_Win)) { WuDalang4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalang3_Times)) { WuDalang3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuDalang3_Bet)) { WuDalang3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuDalang3_Win)) { WuDalang3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuSongAll_Times)) { WuSongAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuSongAll_Bet)) { WuSongAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuSongAll_Win)) { WuSongAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuSong5_Times)) { WuSong5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuSong5_Bet)) { WuSong5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuSong5_Win)) { WuSong5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuSong4_Times)) { WuSong4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuSong4_Bet)) { WuSong4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuSong4_Win)) { WuSong4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WuSong3_Times)) { WuSong3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WuSong3_Bet)) { WuSong3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WuSong3_Win)) { WuSong3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FlagonAll_Times)) { FlagonAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FlagonAll_Bet)) { FlagonAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FlagonAll_Win)) { FlagonAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Flagon5_Times)) { Flagon5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Flagon5_Bet)) { Flagon5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Flagon5_Win)) { Flagon5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Flagon4_Times)) { Flagon4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Flagon4_Bet)) { Flagon4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Flagon4_Win)) { Flagon4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Flagon3_Times)) { Flagon3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Flagon3_Bet)) { Flagon3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Flagon3_Win)) { Flagon3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ShoesAll_Times)) { ShoesAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ShoesAll_Bet)) { ShoesAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ShoesAll_Win)) { ShoesAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Shoes5_Times)) { Shoes5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Shoes5_Bet)) { Shoes5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Shoes5_Win)) { Shoes5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Shoes4_Times)) { Shoes4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Shoes4_Bet)) { Shoes4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Shoes4_Win)) { Shoes4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Shoes3_Times)) { Shoes3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Shoes3_Bet)) { Shoes3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Shoes3_Win)) { Shoes3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FanAll_Times)) { FanAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FanAll_Bet)) { FanAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(FanAll_Win)) { FanAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan5_Times)) { Fan5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan5_Bet)) { Fan5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan5_Win)) { Fan5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan4_Times)) { Fan4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan4_Bet)) { Fan4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan4_Win)) { Fan4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Fan3_Times)) { Fan3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Fan3_Bet)) { Fan3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Fan3_Win)) { Fan3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(HairpinAll_Times)) { HairpinAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(HairpinAll_Bet)) { HairpinAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(HairpinAll_Win)) { HairpinAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Hairpin5_Times)) { Hairpin5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Hairpin5_Bet)) { Hairpin5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Hairpin5_Win)) { Hairpin5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Hairpin4_Times)) { Hairpin4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Hairpin4_Bet)) { Hairpin4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Hairpin4_Win)) { Hairpin4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Hairpin3_Times)) { Hairpin3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Hairpin3_Bet)) { Hairpin3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Hairpin3_Win)) { Hairpin3_Win += Convert.ToDouble(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(BonusBonus_Times), BonusBonus_Times.ToString());
            updata.Add(nameof(Bonus5_Times), Bonus5_Times.ToString());
            updata.Add(nameof(Bonus5_Bet), Bonus5_Bet.ToString());
            updata.Add(nameof(Bonus5_Win), Bonus5_Win.ToString());
            updata.Add(nameof(Bonus4_Times), Bonus4_Times.ToString());
            updata.Add(nameof(Bonus4_Bet), Bonus4_Bet.ToString());
            updata.Add(nameof(Bonus4_Win), Bonus4_Win.ToString());
            updata.Add(nameof(Bonus3_Times), Bonus3_Times.ToString());
            updata.Add(nameof(Bonus3_Bet), Bonus3_Bet.ToString());
            updata.Add(nameof(Bonus3_Win), Bonus3_Win.ToString());
            updata.Add(nameof(WildAll_Times), WildAll_Times.ToString());
            updata.Add(nameof(WildAll_Bet), WildAll_Bet.ToString());
            updata.Add(nameof(WildAll_Win), WildAll_Win.ToString());
            updata.Add(nameof(Wild5_Times), Wild5_Times.ToString());
            updata.Add(nameof(Wild5_Bet), Wild5_Bet.ToString());
            updata.Add(nameof(Wild5_Win), Wild5_Win.ToString());
            updata.Add(nameof(PanJinlianAll_Times), PanJinlianAll_Times.ToString());
            updata.Add(nameof(PanJinlianAll_Bet), PanJinlianAll_Bet.ToString());
            updata.Add(nameof(PanJinlianAll_Win), PanJinlianAll_Win.ToString());
            updata.Add(nameof(PanJinlian5_Times), PanJinlian5_Times.ToString());
            updata.Add(nameof(PanJinlian5_Bet), PanJinlian5_Bet.ToString());
            updata.Add(nameof(PanJinlian5_Win), PanJinlian5_Win.ToString());
            updata.Add(nameof(PanJinlian4_Times), PanJinlian4_Times.ToString());
            updata.Add(nameof(PanJinlian4_Bet), PanJinlian4_Bet.ToString());
            updata.Add(nameof(PanJinlian4_Win), PanJinlian4_Win.ToString());
            updata.Add(nameof(PanJinlian3_Times), PanJinlian3_Times.ToString());
            updata.Add(nameof(PanJinlian3_Bet), PanJinlian3_Bet.ToString());
            updata.Add(nameof(PanJinlian3_Win), PanJinlian3_Win.ToString());
            updata.Add(nameof(XimenQingAll_Times), XimenQingAll_Times.ToString());
            updata.Add(nameof(XimenQingAll_Bet), XimenQingAll_Bet.ToString());
            updata.Add(nameof(XimenQingAll_Win), XimenQingAll_Win.ToString());
            updata.Add(nameof(XimenQing5_Times), XimenQing5_Times.ToString());
            updata.Add(nameof(XimenQing5_Bet), XimenQing5_Bet.ToString());
            updata.Add(nameof(XimenQing5_Win), XimenQing5_Win.ToString());
            updata.Add(nameof(XimenQing4_Times), XimenQing4_Times.ToString());
            updata.Add(nameof(XimenQing4_Bet), XimenQing4_Bet.ToString());
            updata.Add(nameof(XimenQing4_Win), XimenQing4_Win.ToString());
            updata.Add(nameof(XimenQing3_Times), XimenQing3_Times.ToString());
            updata.Add(nameof(XimenQing3_Bet), XimenQing3_Bet.ToString());
            updata.Add(nameof(XimenQing3_Win), XimenQing3_Win.ToString());
            updata.Add(nameof(WuDalangAll_Times), WuDalangAll_Times.ToString());
            updata.Add(nameof(WuDalangAll_Bet), WuDalangAll_Bet.ToString());
            updata.Add(nameof(WuDalangAll_Win), WuDalangAll_Win.ToString());
            updata.Add(nameof(WuDalang5_Times), WuDalang5_Times.ToString());
            updata.Add(nameof(WuDalang5_Bet), WuDalang5_Bet.ToString());
            updata.Add(nameof(WuDalang5_Win), WuDalang5_Win.ToString());
            updata.Add(nameof(WuDalang4_Times), WuDalang4_Times.ToString());
            updata.Add(nameof(WuDalang4_Bet), WuDalang4_Bet.ToString());
            updata.Add(nameof(WuDalang4_Win), WuDalang4_Win.ToString());
            updata.Add(nameof(WuDalang3_Times), WuDalang3_Times.ToString());
            updata.Add(nameof(WuDalang3_Bet), WuDalang3_Bet.ToString());
            updata.Add(nameof(WuDalang3_Win), WuDalang3_Win.ToString());
            updata.Add(nameof(WuSongAll_Times), WuSongAll_Times.ToString());
            updata.Add(nameof(WuSongAll_Bet), WuSongAll_Bet.ToString());
            updata.Add(nameof(WuSongAll_Win), WuSongAll_Win.ToString());
            updata.Add(nameof(WuSong5_Times), WuSong5_Times.ToString());
            updata.Add(nameof(WuSong5_Bet), WuSong5_Bet.ToString());
            updata.Add(nameof(WuSong5_Win), WuSong5_Win.ToString());
            updata.Add(nameof(WuSong4_Times), WuSong4_Times.ToString());
            updata.Add(nameof(WuSong4_Bet), WuSong4_Bet.ToString());
            updata.Add(nameof(WuSong4_Win), WuSong4_Win.ToString());
            updata.Add(nameof(WuSong3_Times), WuSong3_Times.ToString());
            updata.Add(nameof(WuSong3_Bet), WuSong3_Bet.ToString());
            updata.Add(nameof(WuSong3_Win), WuSong3_Win.ToString());
            updata.Add(nameof(FlagonAll_Times), FlagonAll_Times.ToString());
            updata.Add(nameof(FlagonAll_Bet), FlagonAll_Bet.ToString());
            updata.Add(nameof(FlagonAll_Win), FlagonAll_Win.ToString());
            updata.Add(nameof(Flagon5_Times), Flagon5_Times.ToString());
            updata.Add(nameof(Flagon5_Bet), Flagon5_Bet.ToString());
            updata.Add(nameof(Flagon5_Win), Flagon5_Win.ToString());
            updata.Add(nameof(Flagon4_Times), Flagon4_Times.ToString());
            updata.Add(nameof(Flagon4_Bet), Flagon4_Bet.ToString());
            updata.Add(nameof(Flagon4_Win), Flagon4_Win.ToString());
            updata.Add(nameof(Flagon3_Times), Flagon3_Times.ToString());
            updata.Add(nameof(Flagon3_Bet), Flagon3_Bet.ToString());
            updata.Add(nameof(Flagon3_Win), Flagon3_Win.ToString());
            updata.Add(nameof(ShoesAll_Times), ShoesAll_Times.ToString());
            updata.Add(nameof(ShoesAll_Bet), ShoesAll_Bet.ToString());
            updata.Add(nameof(ShoesAll_Win), ShoesAll_Win.ToString());
            updata.Add(nameof(Shoes5_Times), Shoes5_Times.ToString());
            updata.Add(nameof(Shoes5_Bet), Shoes5_Bet.ToString());
            updata.Add(nameof(Shoes5_Win), Shoes5_Win.ToString());
            updata.Add(nameof(Shoes4_Times), Shoes4_Times.ToString());
            updata.Add(nameof(Shoes4_Bet), Shoes4_Bet.ToString());
            updata.Add(nameof(Shoes4_Win), Shoes4_Win.ToString());
            updata.Add(nameof(Shoes3_Times), Shoes3_Times.ToString());
            updata.Add(nameof(Shoes3_Bet), Shoes3_Bet.ToString());
            updata.Add(nameof(Shoes3_Win), Shoes3_Win.ToString());
            updata.Add(nameof(FanAll_Times), FanAll_Times.ToString());
            updata.Add(nameof(FanAll_Bet), FanAll_Bet.ToString());
            updata.Add(nameof(FanAll_Win), FanAll_Win.ToString());
            updata.Add(nameof(Fan5_Times), Fan5_Times.ToString());
            updata.Add(nameof(Fan5_Bet), Fan5_Bet.ToString());
            updata.Add(nameof(Fan5_Win), Fan5_Win.ToString());
            updata.Add(nameof(Fan4_Times), Fan4_Times.ToString());
            updata.Add(nameof(Fan4_Bet), Fan4_Bet.ToString());
            updata.Add(nameof(Fan4_Win), Fan4_Win.ToString());
            updata.Add(nameof(Fan3_Times), Fan3_Times.ToString());
            updata.Add(nameof(Fan3_Bet), Fan3_Bet.ToString());
            updata.Add(nameof(Fan3_Win), Fan3_Win.ToString());
            updata.Add(nameof(HairpinAll_Times), HairpinAll_Times.ToString());
            updata.Add(nameof(HairpinAll_Bet), HairpinAll_Bet.ToString());
            updata.Add(nameof(HairpinAll_Win), HairpinAll_Win.ToString());
            updata.Add(nameof(Hairpin5_Times), Hairpin5_Times.ToString());
            updata.Add(nameof(Hairpin5_Bet), Hairpin5_Bet.ToString());
            updata.Add(nameof(Hairpin5_Win), Hairpin5_Win.ToString());
            updata.Add(nameof(Hairpin4_Times), Hairpin4_Times.ToString());
            updata.Add(nameof(Hairpin4_Bet), Hairpin4_Bet.ToString());
            updata.Add(nameof(Hairpin4_Win), Hairpin4_Win.ToString());
            updata.Add(nameof(Hairpin3_Times), Hairpin3_Times.ToString());
            updata.Add(nameof(Hairpin3_Bet), Hairpin3_Bet.ToString());
            updata.Add(nameof(Hairpin3_Win), Hairpin3_Win.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            BonusBonus_Times = Convert.ToInt32(datalist[nameof(BonusBonus_Times)]);
            Bonus5_Times = Convert.ToInt32(datalist[nameof(Bonus5_Times)]);
            Bonus5_Bet = Convert.ToDouble(datalist[nameof(Bonus5_Bet)]);
            Bonus5_Win = Convert.ToDouble(datalist[nameof(Bonus5_Win)]);
            Bonus4_Times = Convert.ToInt32(datalist[nameof(Bonus4_Times)]);
            Bonus4_Bet = Convert.ToDouble(datalist[nameof(Bonus4_Bet)]);
            Bonus4_Win = Convert.ToDouble(datalist[nameof(Bonus4_Win)]);
            Bonus3_Times = Convert.ToInt32(datalist[nameof(Bonus3_Times)]);
            Bonus3_Bet = Convert.ToDouble(datalist[nameof(Bonus3_Bet)]);
            Bonus3_Win = Convert.ToDouble(datalist[nameof(Bonus3_Win)]);
            WildAll_Times = Convert.ToInt32(datalist[nameof(WildAll_Times)]);
            WildAll_Bet = Convert.ToDouble(datalist[nameof(WildAll_Bet)]);
            WildAll_Win = Convert.ToDouble(datalist[nameof(WildAll_Win)]);
            Wild5_Times = Convert.ToInt32(datalist[nameof(Wild5_Times)]);
            Wild5_Bet = Convert.ToDouble(datalist[nameof(Wild5_Bet)]);
            Wild5_Win = Convert.ToDouble(datalist[nameof(Wild5_Win)]);
            PanJinlianAll_Times = Convert.ToInt32(datalist[nameof(PanJinlianAll_Times)]);
            PanJinlianAll_Bet = Convert.ToDouble(datalist[nameof(PanJinlianAll_Bet)]);
            PanJinlianAll_Win = Convert.ToDouble(datalist[nameof(PanJinlianAll_Win)]);
            PanJinlian5_Times = Convert.ToInt32(datalist[nameof(PanJinlian5_Times)]);
            PanJinlian5_Bet = Convert.ToDouble(datalist[nameof(PanJinlian5_Bet)]);
            PanJinlian5_Win = Convert.ToDouble(datalist[nameof(PanJinlian5_Win)]);
            PanJinlian4_Times = Convert.ToInt32(datalist[nameof(PanJinlian4_Times)]);
            PanJinlian4_Bet = Convert.ToDouble(datalist[nameof(PanJinlian4_Bet)]);
            PanJinlian4_Win = Convert.ToDouble(datalist[nameof(PanJinlian4_Win)]);
            PanJinlian3_Times = Convert.ToInt32(datalist[nameof(PanJinlian3_Times)]);
            PanJinlian3_Bet = Convert.ToDouble(datalist[nameof(PanJinlian3_Bet)]);
            PanJinlian3_Win = Convert.ToDouble(datalist[nameof(PanJinlian3_Win)]);
            XimenQingAll_Times = Convert.ToInt32(datalist[nameof(XimenQingAll_Times)]);
            XimenQingAll_Bet = Convert.ToDouble(datalist[nameof(XimenQingAll_Bet)]);
            XimenQingAll_Win = Convert.ToDouble(datalist[nameof(XimenQingAll_Win)]);
            XimenQing5_Times = Convert.ToInt32(datalist[nameof(XimenQing5_Times)]);
            XimenQing5_Bet = Convert.ToDouble(datalist[nameof(XimenQing5_Bet)]);
            XimenQing5_Win = Convert.ToDouble(datalist[nameof(XimenQing5_Win)]);
            XimenQing4_Times = Convert.ToInt32(datalist[nameof(XimenQing4_Times)]);
            XimenQing4_Bet = Convert.ToDouble(datalist[nameof(XimenQing4_Bet)]);
            XimenQing4_Win = Convert.ToDouble(datalist[nameof(XimenQing4_Win)]);
            XimenQing3_Times = Convert.ToInt32(datalist[nameof(XimenQing3_Times)]);
            XimenQing3_Bet = Convert.ToDouble(datalist[nameof(XimenQing3_Bet)]);
            XimenQing3_Win = Convert.ToDouble(datalist[nameof(XimenQing3_Win)]);
            WuDalangAll_Times = Convert.ToInt32(datalist[nameof(WuDalangAll_Times)]);
            WuDalangAll_Bet = Convert.ToDouble(datalist[nameof(WuDalangAll_Bet)]);
            WuDalangAll_Win = Convert.ToDouble(datalist[nameof(WuDalangAll_Win)]);
            WuDalang5_Times = Convert.ToInt32(datalist[nameof(WuDalang5_Times)]);
            WuDalang5_Bet = Convert.ToDouble(datalist[nameof(WuDalang5_Bet)]);
            WuDalang5_Win = Convert.ToDouble(datalist[nameof(WuDalang5_Win)]);
            WuDalang4_Times = Convert.ToInt32(datalist[nameof(WuDalang4_Times)]);
            WuDalang4_Bet = Convert.ToDouble(datalist[nameof(WuDalang4_Bet)]);
            WuDalang4_Win = Convert.ToDouble(datalist[nameof(WuDalang4_Win)]);
            WuDalang3_Times = Convert.ToInt32(datalist[nameof(WuDalang3_Times)]);
            WuDalang3_Bet = Convert.ToDouble(datalist[nameof(WuDalang3_Bet)]);
            WuDalang3_Win = Convert.ToDouble(datalist[nameof(WuDalang3_Win)]);
            WuSongAll_Times = Convert.ToInt32(datalist[nameof(WuSongAll_Times)]);
            WuSongAll_Bet = Convert.ToDouble(datalist[nameof(WuSongAll_Bet)]);
            WuSongAll_Win = Convert.ToDouble(datalist[nameof(WuSongAll_Win)]);
            WuSong5_Times = Convert.ToInt32(datalist[nameof(WuSong5_Times)]);
            WuSong5_Bet = Convert.ToDouble(datalist[nameof(WuSong5_Bet)]);
            WuSong5_Win = Convert.ToDouble(datalist[nameof(WuSong5_Win)]);
            WuSong4_Times = Convert.ToInt32(datalist[nameof(WuSong4_Times)]);
            WuSong4_Bet = Convert.ToDouble(datalist[nameof(WuSong4_Bet)]);
            WuSong4_Win = Convert.ToDouble(datalist[nameof(WuSong4_Win)]);
            WuSong3_Times = Convert.ToInt32(datalist[nameof(WuSong3_Times)]);
            WuSong3_Bet = Convert.ToDouble(datalist[nameof(WuSong3_Bet)]);
            WuSong3_Win = Convert.ToDouble(datalist[nameof(WuSong3_Win)]);
            FlagonAll_Times = Convert.ToInt32(datalist[nameof(FlagonAll_Times)]);
            FlagonAll_Bet = Convert.ToDouble(datalist[nameof(FlagonAll_Bet)]);
            FlagonAll_Win = Convert.ToDouble(datalist[nameof(FlagonAll_Win)]);
            Flagon5_Times = Convert.ToInt32(datalist[nameof(Flagon5_Times)]);
            Flagon5_Bet = Convert.ToDouble(datalist[nameof(Flagon5_Bet)]);
            Flagon5_Win = Convert.ToDouble(datalist[nameof(Flagon5_Win)]);
            Flagon4_Times = Convert.ToInt32(datalist[nameof(Flagon4_Times)]);
            Flagon4_Bet = Convert.ToDouble(datalist[nameof(Flagon4_Bet)]);
            Flagon4_Win = Convert.ToDouble(datalist[nameof(Flagon4_Win)]);
            Flagon3_Times = Convert.ToInt32(datalist[nameof(Flagon3_Times)]);
            Flagon3_Bet = Convert.ToDouble(datalist[nameof(Flagon3_Bet)]);
            Flagon3_Win = Convert.ToDouble(datalist[nameof(Flagon3_Win)]);
            ShoesAll_Times = Convert.ToInt32(datalist[nameof(ShoesAll_Times)]);
            ShoesAll_Bet = Convert.ToDouble(datalist[nameof(ShoesAll_Bet)]);
            ShoesAll_Win = Convert.ToDouble(datalist[nameof(ShoesAll_Win)]);
            Shoes5_Times = Convert.ToInt32(datalist[nameof(Shoes5_Times)]);
            Shoes5_Bet = Convert.ToDouble(datalist[nameof(Shoes5_Bet)]);
            Shoes5_Win = Convert.ToDouble(datalist[nameof(Shoes5_Win)]);
            Shoes4_Times = Convert.ToInt32(datalist[nameof(Shoes4_Times)]);
            Shoes4_Bet = Convert.ToDouble(datalist[nameof(Shoes4_Bet)]);
            Shoes4_Win = Convert.ToDouble(datalist[nameof(Shoes4_Win)]);
            Shoes3_Times = Convert.ToInt32(datalist[nameof(Shoes3_Times)]);
            Shoes3_Bet = Convert.ToDouble(datalist[nameof(Shoes3_Bet)]);
            Shoes3_Win = Convert.ToDouble(datalist[nameof(Shoes3_Win)]);
            FanAll_Times = Convert.ToInt32(datalist[nameof(FanAll_Times)]);
            FanAll_Bet = Convert.ToDouble(datalist[nameof(FanAll_Bet)]);
            FanAll_Win = Convert.ToDouble(datalist[nameof(FanAll_Win)]);
            Fan5_Times = Convert.ToInt32(datalist[nameof(Fan5_Times)]);
            Fan5_Bet = Convert.ToDouble(datalist[nameof(Fan5_Bet)]);
            Fan5_Win = Convert.ToDouble(datalist[nameof(Fan5_Win)]);
            Fan4_Times = Convert.ToInt32(datalist[nameof(Fan4_Times)]);
            Fan4_Bet = Convert.ToDouble(datalist[nameof(Fan4_Bet)]);
            Fan4_Win = Convert.ToDouble(datalist[nameof(Fan4_Win)]);
            Fan3_Times = Convert.ToInt32(datalist[nameof(Fan3_Times)]);
            Fan3_Bet = Convert.ToDouble(datalist[nameof(Fan3_Bet)]);
            Fan3_Win = Convert.ToDouble(datalist[nameof(Fan3_Win)]);
            HairpinAll_Times = Convert.ToInt32(datalist[nameof(HairpinAll_Times)]);
            HairpinAll_Bet = Convert.ToDouble(datalist[nameof(HairpinAll_Bet)]);
            HairpinAll_Win = Convert.ToDouble(datalist[nameof(HairpinAll_Win)]);
            Hairpin5_Times = Convert.ToInt32(datalist[nameof(Hairpin5_Times)]);
            Hairpin5_Bet = Convert.ToDouble(datalist[nameof(Hairpin5_Bet)]);
            Hairpin5_Win = Convert.ToDouble(datalist[nameof(Hairpin5_Win)]);
            Hairpin4_Times = Convert.ToInt32(datalist[nameof(Hairpin4_Times)]);
            Hairpin4_Bet = Convert.ToDouble(datalist[nameof(Hairpin4_Bet)]);
            Hairpin4_Win = Convert.ToDouble(datalist[nameof(Hairpin4_Win)]);
            Hairpin3_Times = Convert.ToInt32(datalist[nameof(Hairpin3_Times)]);
            Hairpin3_Bet = Convert.ToDouble(datalist[nameof(Hairpin3_Bet)]);
            Hairpin3_Win = Convert.ToDouble(datalist[nameof(Hairpin3_Win)]);
        }
    }
}
