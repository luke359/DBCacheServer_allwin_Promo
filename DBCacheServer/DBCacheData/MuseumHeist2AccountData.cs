using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class MuseumHeist2AccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int FreeThiefR_Times;
        private int FreeThiefB_Times;
        private int ThiefR_Times;
        private double ThiefR_Bet;
        private double ThiefR_Win;
        private int ThiefB_Times;
        private double ThiefB_Bet;
        private double ThiefB_Win;
        private int Scatter5_Times;
        private double Scatter5_Bet;
        private double Scatter5_Win;
        private int Scatter4_Times;
        private double Scatter4_Bet;
        private double Scatter4_Win;
        private int Scatter3_Times;
        private double Scatter3_Bet;
        private double Scatter3_Win;
        private int Logo5_Times;
        private double Logo5_Bet;
        private double Logo5_Win;
        private int Logo4_Times;
        private double Logo4_Bet;
        private double Logo4_Win;
        private int Logo3_Times;
        private double Logo3_Bet;
        private double Logo3_Win;
        private int Police5_Times;
        private double Police5_Bet;
        private double Police5_Win;
        private int Police4_Times;
        private double Police4_Bet;
        private double Police4_Win;
        private int Police3_Times;
        private double Police3_Bet;
        private double Police3_Win;
        private int PoliceHat5_Times;
        private double PoliceHat5_Bet;
        private double PoliceHat5_Win;
        private int PoliceHat4_Times;
        private double PoliceHat4_Bet;
        private double PoliceHat4_Win;
        private int PoliceHat3_Times;
        private double PoliceHat3_Bet;
        private double PoliceHat3_Win;
        private int Handcuffs5_Times;
        private double Handcuffs5_Bet;
        private double Handcuffs5_Win;
        private int Handcuffs4_Times;
        private double Handcuffs4_Bet;
        private double Handcuffs4_Win;
        private int Handcuffs3_Times;
        private double Handcuffs3_Bet;
        private double Handcuffs3_Win;
        private int PoliceBadge5_Times;
        private double PoliceBadge5_Bet;
        private double PoliceBadge5_Win;
        private int PoliceBadge4_Times;
        private double PoliceBadge4_Bet;
        private double PoliceBadge4_Win;
        private int PoliceBadge3_Times;
        private double PoliceBadge3_Bet;
        private double PoliceBadge3_Win;
        private int WalkieTalkie5_Times;
        private double WalkieTalkie5_Bet;
        private double WalkieTalkie5_Win;
        private int WalkieTalkie4_Times;
        private double WalkieTalkie4_Bet;
        private double WalkieTalkie4_Win;
        private int WalkieTalkie3_Times;
        private double WalkieTalkie3_Bet;
        private double WalkieTalkie3_Win;
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
        #endregion

        /// <summary>清除額外押注</summary>
        public override void ClearCacheGameExPlay()
        {
            //update = true;
        }

        /// <summary>清除內存(mode:0=常規, 1=獨立買, 2=全部)</summary>
        public override void ClearCacheGame(int mode)
        {
            FreeThiefR_Times = 0;
            FreeThiefB_Times = 0;
            ThiefR_Times = 0;
            ThiefR_Bet = 0;
            ThiefR_Win = 0;
            ThiefB_Times = 0;
            ThiefB_Bet = 0;
            ThiefB_Win = 0;
            Scatter5_Times = 0;
            Scatter5_Bet = 0;
            Scatter5_Win = 0;
            Scatter4_Times = 0;
            Scatter4_Bet = 0;
            Scatter4_Win = 0;
            Scatter3_Times = 0;
            Scatter3_Bet = 0;
            Scatter3_Win = 0;
            Logo5_Times = 0;
            Logo5_Bet = 0;
            Logo5_Win = 0;
            Logo4_Times = 0;
            Logo4_Bet = 0;
            Logo4_Win = 0;
            Logo3_Times = 0;
            Logo3_Bet = 0;
            Logo3_Win = 0;
            Police5_Times = 0;
            Police5_Bet = 0;
            Police5_Win = 0;
            Police4_Times = 0;
            Police4_Bet = 0;
            Police4_Win = 0;
            Police3_Times = 0;
            Police3_Bet = 0;
            Police3_Win = 0;
            PoliceHat5_Times = 0;
            PoliceHat5_Bet = 0;
            PoliceHat5_Win = 0;
            PoliceHat4_Times = 0;
            PoliceHat4_Bet = 0;
            PoliceHat4_Win = 0;
            PoliceHat3_Times = 0;
            PoliceHat3_Bet = 0;
            PoliceHat3_Win = 0;
            Handcuffs5_Times = 0;
            Handcuffs5_Bet = 0;
            Handcuffs5_Win = 0;
            Handcuffs4_Times = 0;
            Handcuffs4_Bet = 0;
            Handcuffs4_Win = 0;
            Handcuffs3_Times = 0;
            Handcuffs3_Bet = 0;
            Handcuffs3_Win = 0;
            PoliceBadge5_Times = 0;
            PoliceBadge5_Bet = 0;
            PoliceBadge5_Win = 0;
            PoliceBadge4_Times = 0;
            PoliceBadge4_Bet = 0;
            PoliceBadge4_Win = 0;
            PoliceBadge3_Times = 0;
            PoliceBadge3_Bet = 0;
            PoliceBadge3_Win = 0;
            WalkieTalkie5_Times = 0;
            WalkieTalkie5_Bet = 0;
            WalkieTalkie5_Win = 0;
            WalkieTalkie4_Times = 0;
            WalkieTalkie4_Bet = 0;
            WalkieTalkie4_Win = 0;
            WalkieTalkie3_Times = 0;
            WalkieTalkie3_Bet = 0;
            WalkieTalkie3_Win = 0;
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

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(FreeThiefR_Times)) { FreeThiefR_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeThiefB_Times)) { FreeThiefB_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ThiefR_Times)) { ThiefR_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ThiefR_Bet)) { ThiefR_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ThiefR_Win)) { ThiefR_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ThiefB_Times)) { ThiefB_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ThiefB_Bet)) { ThiefB_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ThiefB_Win)) { ThiefB_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Times)) { Scatter5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter5_Bet)) { Scatter5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Win)) { Scatter5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Times)) { Scatter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter4_Bet)) { Scatter4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Win)) { Scatter4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Times)) { Scatter3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter3_Bet)) { Scatter3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Win)) { Scatter3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Times)) { Logo5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo5_Bet)) { Logo5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Win)) { Logo5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Times)) { Logo4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo4_Bet)) { Logo4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Win)) { Logo4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Times)) { Logo3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo3_Bet)) { Logo3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Win)) { Logo3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Police5_Times)) { Police5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Police5_Bet)) { Police5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Police5_Win)) { Police5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Police4_Times)) { Police4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Police4_Bet)) { Police4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Police4_Win)) { Police4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Police3_Times)) { Police3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Police3_Bet)) { Police3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Police3_Win)) { Police3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceHat5_Times)) { PoliceHat5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PoliceHat5_Bet)) { PoliceHat5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceHat5_Win)) { PoliceHat5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceHat4_Times)) { PoliceHat4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PoliceHat4_Bet)) { PoliceHat4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceHat4_Win)) { PoliceHat4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceHat3_Times)) { PoliceHat3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PoliceHat3_Bet)) { PoliceHat3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceHat3_Win)) { PoliceHat3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Handcuffs5_Times)) { Handcuffs5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Handcuffs5_Bet)) { Handcuffs5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Handcuffs5_Win)) { Handcuffs5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Handcuffs4_Times)) { Handcuffs4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Handcuffs4_Bet)) { Handcuffs4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Handcuffs4_Win)) { Handcuffs4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Handcuffs3_Times)) { Handcuffs3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Handcuffs3_Bet)) { Handcuffs3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Handcuffs3_Win)) { Handcuffs3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceBadge5_Times)) { PoliceBadge5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PoliceBadge5_Bet)) { PoliceBadge5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceBadge5_Win)) { PoliceBadge5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceBadge4_Times)) { PoliceBadge4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PoliceBadge4_Bet)) { PoliceBadge4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceBadge4_Win)) { PoliceBadge4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceBadge3_Times)) { PoliceBadge3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(PoliceBadge3_Bet)) { PoliceBadge3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(PoliceBadge3_Win)) { PoliceBadge3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WalkieTalkie5_Times)) { WalkieTalkie5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WalkieTalkie5_Bet)) { WalkieTalkie5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WalkieTalkie5_Win)) { WalkieTalkie5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WalkieTalkie4_Times)) { WalkieTalkie4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WalkieTalkie4_Bet)) { WalkieTalkie4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WalkieTalkie4_Win)) { WalkieTalkie4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WalkieTalkie3_Times)) { WalkieTalkie3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WalkieTalkie3_Bet)) { WalkieTalkie3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WalkieTalkie3_Win)) { WalkieTalkie3_Win += Convert.ToDouble(value); return; }
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
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(FreeThiefR_Times), FreeThiefR_Times.ToString());
            updata.Add(nameof(FreeThiefB_Times), FreeThiefB_Times.ToString());
            updata.Add(nameof(ThiefR_Times), ThiefR_Times.ToString());
            updata.Add(nameof(ThiefR_Bet), ThiefR_Bet.ToString());
            updata.Add(nameof(ThiefR_Win), ThiefR_Win.ToString());
            updata.Add(nameof(ThiefB_Times), ThiefB_Times.ToString());
            updata.Add(nameof(ThiefB_Bet), ThiefB_Bet.ToString());
            updata.Add(nameof(ThiefB_Win), ThiefB_Win.ToString());
            updata.Add(nameof(Scatter5_Times), Scatter5_Times.ToString());
            updata.Add(nameof(Scatter5_Bet), Scatter5_Bet.ToString());
            updata.Add(nameof(Scatter5_Win), Scatter5_Win.ToString());
            updata.Add(nameof(Scatter4_Times), Scatter4_Times.ToString());
            updata.Add(nameof(Scatter4_Bet), Scatter4_Bet.ToString());
            updata.Add(nameof(Scatter4_Win), Scatter4_Win.ToString());
            updata.Add(nameof(Scatter3_Times), Scatter3_Times.ToString());
            updata.Add(nameof(Scatter3_Bet), Scatter3_Bet.ToString());
            updata.Add(nameof(Scatter3_Win), Scatter3_Win.ToString());
            updata.Add(nameof(Logo5_Times), Logo5_Times.ToString());
            updata.Add(nameof(Logo5_Bet), Logo5_Bet.ToString());
            updata.Add(nameof(Logo5_Win), Logo5_Win.ToString());
            updata.Add(nameof(Logo4_Times), Logo4_Times.ToString());
            updata.Add(nameof(Logo4_Bet), Logo4_Bet.ToString());
            updata.Add(nameof(Logo4_Win), Logo4_Win.ToString());
            updata.Add(nameof(Logo3_Times), Logo3_Times.ToString());
            updata.Add(nameof(Logo3_Bet), Logo3_Bet.ToString());
            updata.Add(nameof(Logo3_Win), Logo3_Win.ToString());
            updata.Add(nameof(Police5_Times), Police5_Times.ToString());
            updata.Add(nameof(Police5_Bet), Police5_Bet.ToString());
            updata.Add(nameof(Police5_Win), Police5_Win.ToString());
            updata.Add(nameof(Police4_Times), Police4_Times.ToString());
            updata.Add(nameof(Police4_Bet), Police4_Bet.ToString());
            updata.Add(nameof(Police4_Win), Police4_Win.ToString());
            updata.Add(nameof(Police3_Times), Police3_Times.ToString());
            updata.Add(nameof(Police3_Bet), Police3_Bet.ToString());
            updata.Add(nameof(Police3_Win), Police3_Win.ToString());
            updata.Add(nameof(PoliceHat5_Times), PoliceHat5_Times.ToString());
            updata.Add(nameof(PoliceHat5_Bet), PoliceHat5_Bet.ToString());
            updata.Add(nameof(PoliceHat5_Win), PoliceHat5_Win.ToString());
            updata.Add(nameof(PoliceHat4_Times), PoliceHat4_Times.ToString());
            updata.Add(nameof(PoliceHat4_Bet), PoliceHat4_Bet.ToString());
            updata.Add(nameof(PoliceHat4_Win), PoliceHat4_Win.ToString());
            updata.Add(nameof(PoliceHat3_Times), PoliceHat3_Times.ToString());
            updata.Add(nameof(PoliceHat3_Bet), PoliceHat3_Bet.ToString());
            updata.Add(nameof(PoliceHat3_Win), PoliceHat3_Win.ToString());
            updata.Add(nameof(Handcuffs5_Times), Handcuffs5_Times.ToString());
            updata.Add(nameof(Handcuffs5_Bet), Handcuffs5_Bet.ToString());
            updata.Add(nameof(Handcuffs5_Win), Handcuffs5_Win.ToString());
            updata.Add(nameof(Handcuffs4_Times), Handcuffs4_Times.ToString());
            updata.Add(nameof(Handcuffs4_Bet), Handcuffs4_Bet.ToString());
            updata.Add(nameof(Handcuffs4_Win), Handcuffs4_Win.ToString());
            updata.Add(nameof(Handcuffs3_Times), Handcuffs3_Times.ToString());
            updata.Add(nameof(Handcuffs3_Bet), Handcuffs3_Bet.ToString());
            updata.Add(nameof(Handcuffs3_Win), Handcuffs3_Win.ToString());
            updata.Add(nameof(PoliceBadge5_Times), PoliceBadge5_Times.ToString());
            updata.Add(nameof(PoliceBadge5_Bet), PoliceBadge5_Bet.ToString());
            updata.Add(nameof(PoliceBadge5_Win), PoliceBadge5_Win.ToString());
            updata.Add(nameof(PoliceBadge4_Times), PoliceBadge4_Times.ToString());
            updata.Add(nameof(PoliceBadge4_Bet), PoliceBadge4_Bet.ToString());
            updata.Add(nameof(PoliceBadge4_Win), PoliceBadge4_Win.ToString());
            updata.Add(nameof(PoliceBadge3_Times), PoliceBadge3_Times.ToString());
            updata.Add(nameof(PoliceBadge3_Bet), PoliceBadge3_Bet.ToString());
            updata.Add(nameof(PoliceBadge3_Win), PoliceBadge3_Win.ToString());
            updata.Add(nameof(WalkieTalkie5_Times), WalkieTalkie5_Times.ToString());
            updata.Add(nameof(WalkieTalkie5_Bet), WalkieTalkie5_Bet.ToString());
            updata.Add(nameof(WalkieTalkie5_Win), WalkieTalkie5_Win.ToString());
            updata.Add(nameof(WalkieTalkie4_Times), WalkieTalkie4_Times.ToString());
            updata.Add(nameof(WalkieTalkie4_Bet), WalkieTalkie4_Bet.ToString());
            updata.Add(nameof(WalkieTalkie4_Win), WalkieTalkie4_Win.ToString());
            updata.Add(nameof(WalkieTalkie3_Times), WalkieTalkie3_Times.ToString());
            updata.Add(nameof(WalkieTalkie3_Bet), WalkieTalkie3_Bet.ToString());
            updata.Add(nameof(WalkieTalkie3_Win), WalkieTalkie3_Win.ToString());
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
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            FreeThiefR_Times = Convert.ToInt32(datalist[nameof(FreeThiefR_Times)]);
            FreeThiefB_Times = Convert.ToInt32(datalist[nameof(FreeThiefB_Times)]);
            ThiefR_Times = Convert.ToInt32(datalist[nameof(ThiefR_Times)]);
            ThiefR_Bet = Convert.ToDouble(datalist[nameof(ThiefR_Bet)]);
            ThiefR_Win = Convert.ToDouble(datalist[nameof(ThiefR_Win)]);
            ThiefB_Times = Convert.ToInt32(datalist[nameof(ThiefB_Times)]);
            ThiefB_Bet = Convert.ToDouble(datalist[nameof(ThiefB_Bet)]);
            ThiefB_Win = Convert.ToDouble(datalist[nameof(ThiefB_Win)]);
            Scatter5_Times = Convert.ToInt32(datalist[nameof(Scatter5_Times)]);
            Scatter5_Bet = Convert.ToDouble(datalist[nameof(Scatter5_Bet)]);
            Scatter5_Win = Convert.ToDouble(datalist[nameof(Scatter5_Win)]);
            Scatter4_Times = Convert.ToInt32(datalist[nameof(Scatter4_Times)]);
            Scatter4_Bet = Convert.ToDouble(datalist[nameof(Scatter4_Bet)]);
            Scatter4_Win = Convert.ToDouble(datalist[nameof(Scatter4_Win)]);
            Scatter3_Times = Convert.ToInt32(datalist[nameof(Scatter3_Times)]);
            Scatter3_Bet = Convert.ToDouble(datalist[nameof(Scatter3_Bet)]);
            Scatter3_Win = Convert.ToDouble(datalist[nameof(Scatter3_Win)]);
            Logo5_Times = Convert.ToInt32(datalist[nameof(Logo5_Times)]);
            Logo5_Bet = Convert.ToDouble(datalist[nameof(Logo5_Bet)]);
            Logo5_Win = Convert.ToDouble(datalist[nameof(Logo5_Win)]);
            Logo4_Times = Convert.ToInt32(datalist[nameof(Logo4_Times)]);
            Logo4_Bet = Convert.ToDouble(datalist[nameof(Logo4_Bet)]);
            Logo4_Win = Convert.ToDouble(datalist[nameof(Logo4_Win)]);
            Logo3_Times = Convert.ToInt32(datalist[nameof(Logo3_Times)]);
            Logo3_Bet = Convert.ToDouble(datalist[nameof(Logo3_Bet)]);
            Logo3_Win = Convert.ToDouble(datalist[nameof(Logo3_Win)]);
            Police5_Times = Convert.ToInt32(datalist[nameof(Police5_Times)]);
            Police5_Bet = Convert.ToDouble(datalist[nameof(Police5_Bet)]);
            Police5_Win = Convert.ToDouble(datalist[nameof(Police5_Win)]);
            Police4_Times = Convert.ToInt32(datalist[nameof(Police4_Times)]);
            Police4_Bet = Convert.ToDouble(datalist[nameof(Police4_Bet)]);
            Police4_Win = Convert.ToDouble(datalist[nameof(Police4_Win)]);
            Police3_Times = Convert.ToInt32(datalist[nameof(Police3_Times)]);
            Police3_Bet = Convert.ToDouble(datalist[nameof(Police3_Bet)]);
            Police3_Win = Convert.ToDouble(datalist[nameof(Police3_Win)]);
            PoliceHat5_Times = Convert.ToInt32(datalist[nameof(PoliceHat5_Times)]);
            PoliceHat5_Bet = Convert.ToDouble(datalist[nameof(PoliceHat5_Bet)]);
            PoliceHat5_Win = Convert.ToDouble(datalist[nameof(PoliceHat5_Win)]);
            PoliceHat4_Times = Convert.ToInt32(datalist[nameof(PoliceHat4_Times)]);
            PoliceHat4_Bet = Convert.ToDouble(datalist[nameof(PoliceHat4_Bet)]);
            PoliceHat4_Win = Convert.ToDouble(datalist[nameof(PoliceHat4_Win)]);
            PoliceHat3_Times = Convert.ToInt32(datalist[nameof(PoliceHat3_Times)]);
            PoliceHat3_Bet = Convert.ToDouble(datalist[nameof(PoliceHat3_Bet)]);
            PoliceHat3_Win = Convert.ToDouble(datalist[nameof(PoliceHat3_Win)]);
            Handcuffs5_Times = Convert.ToInt32(datalist[nameof(Handcuffs5_Times)]);
            Handcuffs5_Bet = Convert.ToDouble(datalist[nameof(Handcuffs5_Bet)]);
            Handcuffs5_Win = Convert.ToDouble(datalist[nameof(Handcuffs5_Win)]);
            Handcuffs4_Times = Convert.ToInt32(datalist[nameof(Handcuffs4_Times)]);
            Handcuffs4_Bet = Convert.ToDouble(datalist[nameof(Handcuffs4_Bet)]);
            Handcuffs4_Win = Convert.ToDouble(datalist[nameof(Handcuffs4_Win)]);
            Handcuffs3_Times = Convert.ToInt32(datalist[nameof(Handcuffs3_Times)]);
            Handcuffs3_Bet = Convert.ToDouble(datalist[nameof(Handcuffs3_Bet)]);
            Handcuffs3_Win = Convert.ToDouble(datalist[nameof(Handcuffs3_Win)]);
            PoliceBadge5_Times = Convert.ToInt32(datalist[nameof(PoliceBadge5_Times)]);
            PoliceBadge5_Bet = Convert.ToDouble(datalist[nameof(PoliceBadge5_Bet)]);
            PoliceBadge5_Win = Convert.ToDouble(datalist[nameof(PoliceBadge5_Win)]);
            PoliceBadge4_Times = Convert.ToInt32(datalist[nameof(PoliceBadge4_Times)]);
            PoliceBadge4_Bet = Convert.ToDouble(datalist[nameof(PoliceBadge4_Bet)]);
            PoliceBadge4_Win = Convert.ToDouble(datalist[nameof(PoliceBadge4_Win)]);
            PoliceBadge3_Times = Convert.ToInt32(datalist[nameof(PoliceBadge3_Times)]);
            PoliceBadge3_Bet = Convert.ToDouble(datalist[nameof(PoliceBadge3_Bet)]);
            PoliceBadge3_Win = Convert.ToDouble(datalist[nameof(PoliceBadge3_Win)]);
            WalkieTalkie5_Times = Convert.ToInt32(datalist[nameof(WalkieTalkie5_Times)]);
            WalkieTalkie5_Bet = Convert.ToDouble(datalist[nameof(WalkieTalkie5_Bet)]);
            WalkieTalkie5_Win = Convert.ToDouble(datalist[nameof(WalkieTalkie5_Win)]);
            WalkieTalkie4_Times = Convert.ToInt32(datalist[nameof(WalkieTalkie4_Times)]);
            WalkieTalkie4_Bet = Convert.ToDouble(datalist[nameof(WalkieTalkie4_Bet)]);
            WalkieTalkie4_Win = Convert.ToDouble(datalist[nameof(WalkieTalkie4_Win)]);
            WalkieTalkie3_Times = Convert.ToInt32(datalist[nameof(WalkieTalkie3_Times)]);
            WalkieTalkie3_Bet = Convert.ToDouble(datalist[nameof(WalkieTalkie3_Bet)]);
            WalkieTalkie3_Win = Convert.ToDouble(datalist[nameof(WalkieTalkie3_Win)]);
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
        }
    }
}
