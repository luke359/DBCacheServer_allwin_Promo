using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class NieXiaoqianAccountData : CommonAccountData
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
        public int NieXiaoqianAll_Times;
        public double NieXiaoqianAll_Bet;
        public double NieXiaoqianAll_Win;
        public int NieXiaoqian5_Times;
        public double NieXiaoqian5_Bet;
        public double NieXiaoqian5_Win;
        public int NieXiaoqian4_Times;
        public double NieXiaoqian4_Bet;
        public double NieXiaoqian4_Win;
        public int NieXiaoqian3_Times;
        public double NieXiaoqian3_Bet;
        public double NieXiaoqian3_Win;
        public int YanChixiaAll_Times;
        public double YanChixiaAll_Bet;
        public double YanChixiaAll_Win;
        public int YanChixia5_Times;
        public double YanChixia5_Bet;
        public double YanChixia5_Win;
        public int YanChixia4_Times;
        public double YanChixia4_Bet;
        public double YanChixia4_Win;
        public int YanChixia3_Times;
        public double YanChixia3_Bet;
        public double YanChixia3_Win;
        public int NingCaichenAll_Times;
        public double NingCaichenAll_Bet;
        public double NingCaichenAll_Win;
        public int NingCaichen5_Times;
        public double NingCaichen5_Bet;
        public double NingCaichen5_Win;
        public int NingCaichen4_Times;
        public double NingCaichen4_Bet;
        public double NingCaichen4_Win;
        public int NingCaichen3_Times;
        public double NingCaichen3_Bet;
        public double NingCaichen3_Win;
        public int TreeDemonAll_Times;
        public double TreeDemonAll_Bet;
        public double TreeDemonAll_Win;
        public int TreeDemon5_Times;
        public double TreeDemon5_Bet;
        public double TreeDemon5_Win;
        public int TreeDemon4_Times;
        public double TreeDemon4_Bet;
        public double TreeDemon4_Win;
        public int TreeDemon3_Times;
        public double TreeDemon3_Bet;
        public double TreeDemon3_Win;
        public int GuqinAll_Times;
        public double GuqinAll_Bet;
        public double GuqinAll_Win;
        public int Guqin5_Times;
        public double Guqin5_Bet;
        public double Guqin5_Win;
        public int Guqin4_Times;
        public double Guqin4_Bet;
        public double Guqin4_Win;
        public int Guqin3_Times;
        public double Guqin3_Bet;
        public double Guqin3_Win;
        public int MagicalMirrorAll_Times;
        public double MagicalMirrorAll_Bet;
        public double MagicalMirrorAll_Win;
        public int MagicalMirror5_Times;
        public double MagicalMirror5_Bet;
        public double MagicalMirror5_Win;
        public int MagicalMirror4_Times;
        public double MagicalMirror4_Bet;
        public double MagicalMirror4_Win;
        public int MagicalMirror3_Times;
        public double MagicalMirror3_Bet;
        public double MagicalMirror3_Win;
        public int PouchAll_Times;
        public double PouchAll_Bet;
        public double PouchAll_Win;
        public int Pouch5_Times;
        public double Pouch5_Bet;
        public double Pouch5_Win;
        public int Pouch4_Times;
        public double Pouch4_Bet;
        public double Pouch4_Win;
        public int Pouch3_Times;
        public double Pouch3_Bet;
        public double Pouch3_Win;
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
            NieXiaoqianAll_Times = 0;
            NieXiaoqianAll_Bet = 0;
            NieXiaoqianAll_Win = 0;
            NieXiaoqian5_Times = 0;
            NieXiaoqian5_Bet = 0;
            NieXiaoqian5_Win = 0;
            NieXiaoqian4_Times = 0;
            NieXiaoqian4_Bet = 0;
            NieXiaoqian4_Win = 0;
            NieXiaoqian3_Times = 0;
            NieXiaoqian3_Bet = 0;
            NieXiaoqian3_Win = 0;
            YanChixiaAll_Times = 0;
            YanChixiaAll_Bet = 0;
            YanChixiaAll_Win = 0;
            YanChixia5_Times = 0;
            YanChixia5_Bet = 0;
            YanChixia5_Win = 0;
            YanChixia4_Times = 0;
            YanChixia4_Bet = 0;
            YanChixia4_Win = 0;
            YanChixia3_Times = 0;
            YanChixia3_Bet = 0;
            YanChixia3_Win = 0;
            NingCaichenAll_Times = 0;
            NingCaichenAll_Bet = 0;
            NingCaichenAll_Win = 0;
            NingCaichen5_Times = 0;
            NingCaichen5_Bet = 0;
            NingCaichen5_Win = 0;
            NingCaichen4_Times = 0;
            NingCaichen4_Bet = 0;
            NingCaichen4_Win = 0;
            NingCaichen3_Times = 0;
            NingCaichen3_Bet = 0;
            NingCaichen3_Win = 0;
            TreeDemonAll_Times = 0;
            TreeDemonAll_Bet = 0;
            TreeDemonAll_Win = 0;
            TreeDemon5_Times = 0;
            TreeDemon5_Bet = 0;
            TreeDemon5_Win = 0;
            TreeDemon4_Times = 0;
            TreeDemon4_Bet = 0;
            TreeDemon4_Win = 0;
            TreeDemon3_Times = 0;
            TreeDemon3_Bet = 0;
            TreeDemon3_Win = 0;
            GuqinAll_Times = 0;
            GuqinAll_Bet = 0;
            GuqinAll_Win = 0;
            Guqin5_Times = 0;
            Guqin5_Bet = 0;
            Guqin5_Win = 0;
            Guqin4_Times = 0;
            Guqin4_Bet = 0;
            Guqin4_Win = 0;
            Guqin3_Times = 0;
            Guqin3_Bet = 0;
            Guqin3_Win = 0;
            MagicalMirrorAll_Times = 0;
            MagicalMirrorAll_Bet = 0;
            MagicalMirrorAll_Win = 0;
            MagicalMirror5_Times = 0;
            MagicalMirror5_Bet = 0;
            MagicalMirror5_Win = 0;
            MagicalMirror4_Times = 0;
            MagicalMirror4_Bet = 0;
            MagicalMirror4_Win = 0;
            MagicalMirror3_Times = 0;
            MagicalMirror3_Bet = 0;
            MagicalMirror3_Win = 0;
            PouchAll_Times = 0;
            PouchAll_Bet = 0;
            PouchAll_Win = 0;
            Pouch5_Times = 0;
            Pouch5_Bet = 0;
            Pouch5_Win = 0;
            Pouch4_Times = 0;
            Pouch4_Bet = 0;
            Pouch4_Win = 0;
            Pouch3_Times = 0;
            Pouch3_Bet = 0;
            Pouch3_Win = 0;
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
            if (field == nameof(NieXiaoqianAll_Times)) { NieXiaoqianAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NieXiaoqianAll_Bet)) { NieXiaoqianAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NieXiaoqianAll_Win)) { NieXiaoqianAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(NieXiaoqian5_Times)) { NieXiaoqian5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NieXiaoqian5_Bet)) { NieXiaoqian5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NieXiaoqian5_Win)) { NieXiaoqian5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(NieXiaoqian4_Times)) { NieXiaoqian4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NieXiaoqian4_Bet)) { NieXiaoqian4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NieXiaoqian4_Win)) { NieXiaoqian4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(NieXiaoqian3_Times)) { NieXiaoqian3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NieXiaoqian3_Bet)) { NieXiaoqian3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NieXiaoqian3_Win)) { NieXiaoqian3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixiaAll_Times)) { YanChixiaAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(YanChixiaAll_Bet)) { YanChixiaAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixiaAll_Win)) { YanChixiaAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixia5_Times)) { YanChixia5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(YanChixia5_Bet)) { YanChixia5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixia5_Win)) { YanChixia5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixia4_Times)) { YanChixia4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(YanChixia4_Bet)) { YanChixia4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixia4_Win)) { YanChixia4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixia3_Times)) { YanChixia3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(YanChixia3_Bet)) { YanChixia3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(YanChixia3_Win)) { YanChixia3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichenAll_Times)) { NingCaichenAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NingCaichenAll_Bet)) { NingCaichenAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichenAll_Win)) { NingCaichenAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichen5_Times)) { NingCaichen5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NingCaichen5_Bet)) { NingCaichen5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichen5_Win)) { NingCaichen5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichen4_Times)) { NingCaichen4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NingCaichen4_Bet)) { NingCaichen4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichen4_Win)) { NingCaichen4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichen3_Times)) { NingCaichen3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(NingCaichen3_Bet)) { NingCaichen3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(NingCaichen3_Win)) { NingCaichen3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemonAll_Times)) { TreeDemonAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TreeDemonAll_Bet)) { TreeDemonAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemonAll_Win)) { TreeDemonAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemon5_Times)) { TreeDemon5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TreeDemon5_Bet)) { TreeDemon5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemon5_Win)) { TreeDemon5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemon4_Times)) { TreeDemon4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TreeDemon4_Bet)) { TreeDemon4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemon4_Win)) { TreeDemon4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemon3_Times)) { TreeDemon3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(TreeDemon3_Bet)) { TreeDemon3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(TreeDemon3_Win)) { TreeDemon3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(GuqinAll_Times)) { GuqinAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GuqinAll_Bet)) { GuqinAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GuqinAll_Win)) { GuqinAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Guqin5_Times)) { Guqin5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Guqin5_Bet)) { Guqin5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Guqin5_Win)) { Guqin5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Guqin4_Times)) { Guqin4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Guqin4_Bet)) { Guqin4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Guqin4_Win)) { Guqin4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Guqin3_Times)) { Guqin3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Guqin3_Bet)) { Guqin3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Guqin3_Win)) { Guqin3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirrorAll_Times)) { MagicalMirrorAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MagicalMirrorAll_Bet)) { MagicalMirrorAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirrorAll_Win)) { MagicalMirrorAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirror5_Times)) { MagicalMirror5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MagicalMirror5_Bet)) { MagicalMirror5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirror5_Win)) { MagicalMirror5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirror4_Times)) { MagicalMirror4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MagicalMirror4_Bet)) { MagicalMirror4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirror4_Win)) { MagicalMirror4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirror3_Times)) { MagicalMirror3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(MagicalMirror3_Bet)) { MagicalMirror3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(MagicalMirror3_Win)) { MagicalMirror3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PouchAll_Times)) { PouchAll_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PouchAll_Bet)) { PouchAll_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PouchAll_Win)) { PouchAll_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Pouch5_Times)) { Pouch5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Pouch5_Bet)) { Pouch5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Pouch5_Win)) { Pouch5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Pouch4_Times)) { Pouch4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Pouch4_Bet)) { Pouch4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Pouch4_Win)) { Pouch4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Pouch3_Times)) { Pouch3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Pouch3_Bet)) { Pouch3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Pouch3_Win)) { Pouch3_Win += Convert.ToDouble(value); return; }
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
            updata.Add(nameof(NieXiaoqianAll_Times), NieXiaoqianAll_Times.ToString());
            updata.Add(nameof(NieXiaoqianAll_Bet), NieXiaoqianAll_Bet.ToString());
            updata.Add(nameof(NieXiaoqianAll_Win), NieXiaoqianAll_Win.ToString());
            updata.Add(nameof(NieXiaoqian5_Times), NieXiaoqian5_Times.ToString());
            updata.Add(nameof(NieXiaoqian5_Bet), NieXiaoqian5_Bet.ToString());
            updata.Add(nameof(NieXiaoqian5_Win), NieXiaoqian5_Win.ToString());
            updata.Add(nameof(NieXiaoqian4_Times), NieXiaoqian4_Times.ToString());
            updata.Add(nameof(NieXiaoqian4_Bet), NieXiaoqian4_Bet.ToString());
            updata.Add(nameof(NieXiaoqian4_Win), NieXiaoqian4_Win.ToString());
            updata.Add(nameof(NieXiaoqian3_Times), NieXiaoqian3_Times.ToString());
            updata.Add(nameof(NieXiaoqian3_Bet), NieXiaoqian3_Bet.ToString());
            updata.Add(nameof(NieXiaoqian3_Win), NieXiaoqian3_Win.ToString());
            updata.Add(nameof(YanChixiaAll_Times), YanChixiaAll_Times.ToString());
            updata.Add(nameof(YanChixiaAll_Bet), YanChixiaAll_Bet.ToString());
            updata.Add(nameof(YanChixiaAll_Win), YanChixiaAll_Win.ToString());
            updata.Add(nameof(YanChixia5_Times), YanChixia5_Times.ToString());
            updata.Add(nameof(YanChixia5_Bet), YanChixia5_Bet.ToString());
            updata.Add(nameof(YanChixia5_Win), YanChixia5_Win.ToString());
            updata.Add(nameof(YanChixia4_Times), YanChixia4_Times.ToString());
            updata.Add(nameof(YanChixia4_Bet), YanChixia4_Bet.ToString());
            updata.Add(nameof(YanChixia4_Win), YanChixia4_Win.ToString());
            updata.Add(nameof(YanChixia3_Times), YanChixia3_Times.ToString());
            updata.Add(nameof(YanChixia3_Bet), YanChixia3_Bet.ToString());
            updata.Add(nameof(YanChixia3_Win), YanChixia3_Win.ToString());
            updata.Add(nameof(NingCaichenAll_Times), NingCaichenAll_Times.ToString());
            updata.Add(nameof(NingCaichenAll_Bet), NingCaichenAll_Bet.ToString());
            updata.Add(nameof(NingCaichenAll_Win), NingCaichenAll_Win.ToString());
            updata.Add(nameof(NingCaichen5_Times), NingCaichen5_Times.ToString());
            updata.Add(nameof(NingCaichen5_Bet), NingCaichen5_Bet.ToString());
            updata.Add(nameof(NingCaichen5_Win), NingCaichen5_Win.ToString());
            updata.Add(nameof(NingCaichen4_Times), NingCaichen4_Times.ToString());
            updata.Add(nameof(NingCaichen4_Bet), NingCaichen4_Bet.ToString());
            updata.Add(nameof(NingCaichen4_Win), NingCaichen4_Win.ToString());
            updata.Add(nameof(NingCaichen3_Times), NingCaichen3_Times.ToString());
            updata.Add(nameof(NingCaichen3_Bet), NingCaichen3_Bet.ToString());
            updata.Add(nameof(NingCaichen3_Win), NingCaichen3_Win.ToString());
            updata.Add(nameof(TreeDemonAll_Times), TreeDemonAll_Times.ToString());
            updata.Add(nameof(TreeDemonAll_Bet), TreeDemonAll_Bet.ToString());
            updata.Add(nameof(TreeDemonAll_Win), TreeDemonAll_Win.ToString());
            updata.Add(nameof(TreeDemon5_Times), TreeDemon5_Times.ToString());
            updata.Add(nameof(TreeDemon5_Bet), TreeDemon5_Bet.ToString());
            updata.Add(nameof(TreeDemon5_Win), TreeDemon5_Win.ToString());
            updata.Add(nameof(TreeDemon4_Times), TreeDemon4_Times.ToString());
            updata.Add(nameof(TreeDemon4_Bet), TreeDemon4_Bet.ToString());
            updata.Add(nameof(TreeDemon4_Win), TreeDemon4_Win.ToString());
            updata.Add(nameof(TreeDemon3_Times), TreeDemon3_Times.ToString());
            updata.Add(nameof(TreeDemon3_Bet), TreeDemon3_Bet.ToString());
            updata.Add(nameof(TreeDemon3_Win), TreeDemon3_Win.ToString());
            updata.Add(nameof(GuqinAll_Times), GuqinAll_Times.ToString());
            updata.Add(nameof(GuqinAll_Bet), GuqinAll_Bet.ToString());
            updata.Add(nameof(GuqinAll_Win), GuqinAll_Win.ToString());
            updata.Add(nameof(Guqin5_Times), Guqin5_Times.ToString());
            updata.Add(nameof(Guqin5_Bet), Guqin5_Bet.ToString());
            updata.Add(nameof(Guqin5_Win), Guqin5_Win.ToString());
            updata.Add(nameof(Guqin4_Times), Guqin4_Times.ToString());
            updata.Add(nameof(Guqin4_Bet), Guqin4_Bet.ToString());
            updata.Add(nameof(Guqin4_Win), Guqin4_Win.ToString());
            updata.Add(nameof(Guqin3_Times), Guqin3_Times.ToString());
            updata.Add(nameof(Guqin3_Bet), Guqin3_Bet.ToString());
            updata.Add(nameof(Guqin3_Win), Guqin3_Win.ToString());
            updata.Add(nameof(MagicalMirrorAll_Times), MagicalMirrorAll_Times.ToString());
            updata.Add(nameof(MagicalMirrorAll_Bet), MagicalMirrorAll_Bet.ToString());
            updata.Add(nameof(MagicalMirrorAll_Win), MagicalMirrorAll_Win.ToString());
            updata.Add(nameof(MagicalMirror5_Times), MagicalMirror5_Times.ToString());
            updata.Add(nameof(MagicalMirror5_Bet), MagicalMirror5_Bet.ToString());
            updata.Add(nameof(MagicalMirror5_Win), MagicalMirror5_Win.ToString());
            updata.Add(nameof(MagicalMirror4_Times), MagicalMirror4_Times.ToString());
            updata.Add(nameof(MagicalMirror4_Bet), MagicalMirror4_Bet.ToString());
            updata.Add(nameof(MagicalMirror4_Win), MagicalMirror4_Win.ToString());
            updata.Add(nameof(MagicalMirror3_Times), MagicalMirror3_Times.ToString());
            updata.Add(nameof(MagicalMirror3_Bet), MagicalMirror3_Bet.ToString());
            updata.Add(nameof(MagicalMirror3_Win), MagicalMirror3_Win.ToString());
            updata.Add(nameof(PouchAll_Times), PouchAll_Times.ToString());
            updata.Add(nameof(PouchAll_Bet), PouchAll_Bet.ToString());
            updata.Add(nameof(PouchAll_Win), PouchAll_Win.ToString());
            updata.Add(nameof(Pouch5_Times), Pouch5_Times.ToString());
            updata.Add(nameof(Pouch5_Bet), Pouch5_Bet.ToString());
            updata.Add(nameof(Pouch5_Win), Pouch5_Win.ToString());
            updata.Add(nameof(Pouch4_Times), Pouch4_Times.ToString());
            updata.Add(nameof(Pouch4_Bet), Pouch4_Bet.ToString());
            updata.Add(nameof(Pouch4_Win), Pouch4_Win.ToString());
            updata.Add(nameof(Pouch3_Times), Pouch3_Times.ToString());
            updata.Add(nameof(Pouch3_Bet), Pouch3_Bet.ToString());
            updata.Add(nameof(Pouch3_Win), Pouch3_Win.ToString());
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
            NieXiaoqianAll_Times = Convert.ToInt32(datalist[nameof(NieXiaoqianAll_Times)]);
            NieXiaoqianAll_Bet = Convert.ToDouble(datalist[nameof(NieXiaoqianAll_Bet)]);
            NieXiaoqianAll_Win = Convert.ToDouble(datalist[nameof(NieXiaoqianAll_Win)]);
            NieXiaoqian5_Times = Convert.ToInt32(datalist[nameof(NieXiaoqian5_Times)]);
            NieXiaoqian5_Bet = Convert.ToDouble(datalist[nameof(NieXiaoqian5_Bet)]);
            NieXiaoqian5_Win = Convert.ToDouble(datalist[nameof(NieXiaoqian5_Win)]);
            NieXiaoqian4_Times = Convert.ToInt32(datalist[nameof(NieXiaoqian4_Times)]);
            NieXiaoqian4_Bet = Convert.ToDouble(datalist[nameof(NieXiaoqian4_Bet)]);
            NieXiaoqian4_Win = Convert.ToDouble(datalist[nameof(NieXiaoqian4_Win)]);
            NieXiaoqian3_Times = Convert.ToInt32(datalist[nameof(NieXiaoqian3_Times)]);
            NieXiaoqian3_Bet = Convert.ToDouble(datalist[nameof(NieXiaoqian3_Bet)]);
            NieXiaoqian3_Win = Convert.ToDouble(datalist[nameof(NieXiaoqian3_Win)]);
            YanChixiaAll_Times = Convert.ToInt32(datalist[nameof(YanChixiaAll_Times)]);
            YanChixiaAll_Bet = Convert.ToDouble(datalist[nameof(YanChixiaAll_Bet)]);
            YanChixiaAll_Win = Convert.ToDouble(datalist[nameof(YanChixiaAll_Win)]);
            YanChixia5_Times = Convert.ToInt32(datalist[nameof(YanChixia5_Times)]);
            YanChixia5_Bet = Convert.ToDouble(datalist[nameof(YanChixia5_Bet)]);
            YanChixia5_Win = Convert.ToDouble(datalist[nameof(YanChixia5_Win)]);
            YanChixia4_Times = Convert.ToInt32(datalist[nameof(YanChixia4_Times)]);
            YanChixia4_Bet = Convert.ToDouble(datalist[nameof(YanChixia4_Bet)]);
            YanChixia4_Win = Convert.ToDouble(datalist[nameof(YanChixia4_Win)]);
            YanChixia3_Times = Convert.ToInt32(datalist[nameof(YanChixia3_Times)]);
            YanChixia3_Bet = Convert.ToDouble(datalist[nameof(YanChixia3_Bet)]);
            YanChixia3_Win = Convert.ToDouble(datalist[nameof(YanChixia3_Win)]);
            NingCaichenAll_Times = Convert.ToInt32(datalist[nameof(NingCaichenAll_Times)]);
            NingCaichenAll_Bet = Convert.ToDouble(datalist[nameof(NingCaichenAll_Bet)]);
            NingCaichenAll_Win = Convert.ToDouble(datalist[nameof(NingCaichenAll_Win)]);
            NingCaichen5_Times = Convert.ToInt32(datalist[nameof(NingCaichen5_Times)]);
            NingCaichen5_Bet = Convert.ToDouble(datalist[nameof(NingCaichen5_Bet)]);
            NingCaichen5_Win = Convert.ToDouble(datalist[nameof(NingCaichen5_Win)]);
            NingCaichen4_Times = Convert.ToInt32(datalist[nameof(NingCaichen4_Times)]);
            NingCaichen4_Bet = Convert.ToDouble(datalist[nameof(NingCaichen4_Bet)]);
            NingCaichen4_Win = Convert.ToDouble(datalist[nameof(NingCaichen4_Win)]);
            NingCaichen3_Times = Convert.ToInt32(datalist[nameof(NingCaichen3_Times)]);
            NingCaichen3_Bet = Convert.ToDouble(datalist[nameof(NingCaichen3_Bet)]);
            NingCaichen3_Win = Convert.ToDouble(datalist[nameof(NingCaichen3_Win)]);
            TreeDemonAll_Times = Convert.ToInt32(datalist[nameof(TreeDemonAll_Times)]);
            TreeDemonAll_Bet = Convert.ToDouble(datalist[nameof(TreeDemonAll_Bet)]);
            TreeDemonAll_Win = Convert.ToDouble(datalist[nameof(TreeDemonAll_Win)]);
            TreeDemon5_Times = Convert.ToInt32(datalist[nameof(TreeDemon5_Times)]);
            TreeDemon5_Bet = Convert.ToDouble(datalist[nameof(TreeDemon5_Bet)]);
            TreeDemon5_Win = Convert.ToDouble(datalist[nameof(TreeDemon5_Win)]);
            TreeDemon4_Times = Convert.ToInt32(datalist[nameof(TreeDemon4_Times)]);
            TreeDemon4_Bet = Convert.ToDouble(datalist[nameof(TreeDemon4_Bet)]);
            TreeDemon4_Win = Convert.ToDouble(datalist[nameof(TreeDemon4_Win)]);
            TreeDemon3_Times = Convert.ToInt32(datalist[nameof(TreeDemon3_Times)]);
            TreeDemon3_Bet = Convert.ToDouble(datalist[nameof(TreeDemon3_Bet)]);
            TreeDemon3_Win = Convert.ToDouble(datalist[nameof(TreeDemon3_Win)]);
            GuqinAll_Times = Convert.ToInt32(datalist[nameof(GuqinAll_Times)]);
            GuqinAll_Bet = Convert.ToDouble(datalist[nameof(GuqinAll_Bet)]);
            GuqinAll_Win = Convert.ToDouble(datalist[nameof(GuqinAll_Win)]);
            Guqin5_Times = Convert.ToInt32(datalist[nameof(Guqin5_Times)]);
            Guqin5_Bet = Convert.ToDouble(datalist[nameof(Guqin5_Bet)]);
            Guqin5_Win = Convert.ToDouble(datalist[nameof(Guqin5_Win)]);
            Guqin4_Times = Convert.ToInt32(datalist[nameof(Guqin4_Times)]);
            Guqin4_Bet = Convert.ToDouble(datalist[nameof(Guqin4_Bet)]);
            Guqin4_Win = Convert.ToDouble(datalist[nameof(Guqin4_Win)]);
            Guqin3_Times = Convert.ToInt32(datalist[nameof(Guqin3_Times)]);
            Guqin3_Bet = Convert.ToDouble(datalist[nameof(Guqin3_Bet)]);
            Guqin3_Win = Convert.ToDouble(datalist[nameof(Guqin3_Win)]);
            MagicalMirrorAll_Times = Convert.ToInt32(datalist[nameof(MagicalMirrorAll_Times)]);
            MagicalMirrorAll_Bet = Convert.ToDouble(datalist[nameof(MagicalMirrorAll_Bet)]);
            MagicalMirrorAll_Win = Convert.ToDouble(datalist[nameof(MagicalMirrorAll_Win)]);
            MagicalMirror5_Times = Convert.ToInt32(datalist[nameof(MagicalMirror5_Times)]);
            MagicalMirror5_Bet = Convert.ToDouble(datalist[nameof(MagicalMirror5_Bet)]);
            MagicalMirror5_Win = Convert.ToDouble(datalist[nameof(MagicalMirror5_Win)]);
            MagicalMirror4_Times = Convert.ToInt32(datalist[nameof(MagicalMirror4_Times)]);
            MagicalMirror4_Bet = Convert.ToDouble(datalist[nameof(MagicalMirror4_Bet)]);
            MagicalMirror4_Win = Convert.ToDouble(datalist[nameof(MagicalMirror4_Win)]);
            MagicalMirror3_Times = Convert.ToInt32(datalist[nameof(MagicalMirror3_Times)]);
            MagicalMirror3_Bet = Convert.ToDouble(datalist[nameof(MagicalMirror3_Bet)]);
            MagicalMirror3_Win = Convert.ToDouble(datalist[nameof(MagicalMirror3_Win)]);
            PouchAll_Times = Convert.ToInt32(datalist[nameof(PouchAll_Times)]);
            PouchAll_Bet = Convert.ToDouble(datalist[nameof(PouchAll_Bet)]);
            PouchAll_Win = Convert.ToDouble(datalist[nameof(PouchAll_Win)]);
            Pouch5_Times = Convert.ToInt32(datalist[nameof(Pouch5_Times)]);
            Pouch5_Bet = Convert.ToDouble(datalist[nameof(Pouch5_Bet)]);
            Pouch5_Win = Convert.ToDouble(datalist[nameof(Pouch5_Win)]);
            Pouch4_Times = Convert.ToInt32(datalist[nameof(Pouch4_Times)]);
            Pouch4_Bet = Convert.ToDouble(datalist[nameof(Pouch4_Bet)]);
            Pouch4_Win = Convert.ToDouble(datalist[nameof(Pouch4_Win)]);
            Pouch3_Times = Convert.ToInt32(datalist[nameof(Pouch3_Times)]);
            Pouch3_Bet = Convert.ToDouble(datalist[nameof(Pouch3_Bet)]);
            Pouch3_Win = Convert.ToDouble(datalist[nameof(Pouch3_Win)]);
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
