using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class WukongAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int WukongRespin_Times;
        private double WukongRespin_Bet;
        private double WukongRespin_Win;
        private int WukongB_Times;
        private double WukongB_Bet;
        private double WukongB_Win;
        private int Scatter5_Times;
        private double Scatter5_Bet;
        private double Scatter5_Win;
        private int Scatter4_Times;
        private double Scatter4_Bet;
        private double Scatter4_Win;
        private int Scatter3_Times;
        private double Scatter3_Bet;
        private double Scatter3_Win;
        private int FreeWukongRespin_Times;
        private int FreeWukongB_Times;
        private int GoldenCudgel5_Times;
        private double GoldenCudgel5_Bet;
        private double GoldenCudgel5_Win;
        private int GoldenCudgel4_Times;
        private double GoldenCudgel4_Bet;
        private double GoldenCudgel4_Win;
        private int GoldenCudgel3_Times;
        private double GoldenCudgel3_Bet;
        private double GoldenCudgel3_Win;
        private int Soldier5_Times;
        private double Soldier5_Bet;
        private double Soldier5_Win;
        private int Soldier4_Times;
        private double Soldier4_Bet;
        private double Soldier4_Win;
        private int Soldier3_Times;
        private double Soldier3_Bet;
        private double Soldier3_Win;
        private int Spear5_Times;
        private double Spear5_Bet;
        private double Spear5_Win;
        private int Spear4_Times;
        private double Spear4_Bet;
        private double Spear4_Win;
        private int Spear3_Times;
        private double Spear3_Bet;
        private double Spear3_Win;
        private int Weapon5_Times;
        private double Weapon5_Bet;
        private double Weapon5_Win;
        private int Weapon4_Times;
        private double Weapon4_Bet;
        private double Weapon4_Win;
        private int Weapon3_Times;
        private double Weapon3_Bet;
        private double Weapon3_Win;
        private int Token5_Times;
        private double Token5_Bet;
        private double Token5_Win;
        private int Token4_Times;
        private double Token4_Bet;
        private double Token4_Win;
        private int Token3_Times;
        private double Token3_Bet;
        private double Token3_Win;
        private int Drum5_Times;
        private double Drum5_Bet;
        private double Drum5_Win;
        private int Drum4_Times;
        private double Drum4_Bet;
        private double Drum4_Win;
        private int Drum3_Times;
        private double Drum3_Bet;
        private double Drum3_Win;
        private int Gourd5_Times;
        private double Gourd5_Bet;
        private double Gourd5_Win;
        private int Gourd4_Times;
        private double Gourd4_Bet;
        private double Gourd4_Win;
        private int Gourd3_Times;
        private double Gourd3_Bet;
        private double Gourd3_Win;
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
        private int ExPlay05_Times;
        private double ExPlay05_Bet;
        private double ExPlay05_Win;
        private double ExPlay05_WinTimes;
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
            WukongRespin_Times = 0;
            WukongRespin_Bet = 0;
            WukongRespin_Win = 0;
            WukongB_Times = 0;
            WukongB_Bet = 0;
            WukongB_Win = 0;
            Scatter5_Times = 0;
            Scatter5_Bet = 0;
            Scatter5_Win = 0;
            Scatter4_Times = 0;
            Scatter4_Bet = 0;
            Scatter4_Win = 0;
            Scatter3_Times = 0;
            Scatter3_Bet = 0;
            Scatter3_Win = 0;
            FreeWukongRespin_Times = 0;
            FreeWukongB_Times = 0;
            GoldenCudgel5_Times = 0;
            GoldenCudgel5_Bet = 0;
            GoldenCudgel5_Win = 0;
            GoldenCudgel4_Times = 0;
            GoldenCudgel4_Bet = 0;
            GoldenCudgel4_Win = 0;
            GoldenCudgel3_Times = 0;
            GoldenCudgel3_Bet = 0;
            GoldenCudgel3_Win = 0;
            Soldier5_Times = 0;
            Soldier5_Bet = 0;
            Soldier5_Win = 0;
            Soldier4_Times = 0;
            Soldier4_Bet = 0;
            Soldier4_Win = 0;
            Soldier3_Times = 0;
            Soldier3_Bet = 0;
            Soldier3_Win = 0;
            Spear5_Times = 0;
            Spear5_Bet = 0;
            Spear5_Win = 0;
            Spear4_Times = 0;
            Spear4_Bet = 0;
            Spear4_Win = 0;
            Spear3_Times = 0;
            Spear3_Bet = 0;
            Spear3_Win = 0;
            Weapon5_Times = 0;
            Weapon5_Bet = 0;
            Weapon5_Win = 0;
            Weapon4_Times = 0;
            Weapon4_Bet = 0;
            Weapon4_Win = 0;
            Weapon3_Times = 0;
            Weapon3_Bet = 0;
            Weapon3_Win = 0;
            Token5_Times = 0;
            Token5_Bet = 0;
            Token5_Win = 0;
            Token4_Times = 0;
            Token4_Bet = 0;
            Token4_Win = 0;
            Token3_Times = 0;
            Token3_Bet = 0;
            Token3_Win = 0;
            Drum5_Times = 0;
            Drum5_Bet = 0;
            Drum5_Win = 0;
            Drum4_Times = 0;
            Drum4_Bet = 0;
            Drum4_Win = 0;
            Drum3_Times = 0;
            Drum3_Bet = 0;
            Drum3_Win = 0;
            Gourd5_Times = 0;
            Gourd5_Bet = 0;
            Gourd5_Win = 0;
            Gourd4_Times = 0;
            Gourd4_Bet = 0;
            Gourd4_Win = 0;
            Gourd3_Times = 0;
            Gourd3_Bet = 0;
            Gourd3_Win = 0;
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
            if (field == nameof(WukongRespin_Times)) { WukongRespin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WukongRespin_Bet)) { WukongRespin_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WukongRespin_Win)) { WukongRespin_Win += Convert.ToDouble(value); return; }
            if (field == nameof(WukongB_Times)) { WukongB_Times += Convert.ToInt32(value); return; }
            if (field == nameof(WukongB_Bet)) { WukongB_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(WukongB_Win)) { WukongB_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Times)) { Scatter5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter5_Bet)) { Scatter5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Win)) { Scatter5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Times)) { Scatter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter4_Bet)) { Scatter4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Win)) { Scatter4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Times)) { Scatter3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter3_Bet)) { Scatter3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Win)) { Scatter3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(FreeWukongRespin_Times)) { FreeWukongRespin_Times += Convert.ToInt32(value); return; }
            if (field == nameof(FreeWukongB_Times)) { FreeWukongB_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GoldenCudgel5_Times)) { GoldenCudgel5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GoldenCudgel5_Bet)) { GoldenCudgel5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GoldenCudgel5_Win)) { GoldenCudgel5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(GoldenCudgel4_Times)) { GoldenCudgel4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GoldenCudgel4_Bet)) { GoldenCudgel4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GoldenCudgel4_Win)) { GoldenCudgel4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(GoldenCudgel3_Times)) { GoldenCudgel3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(GoldenCudgel3_Bet)) { GoldenCudgel3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(GoldenCudgel3_Win)) { GoldenCudgel3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Soldier5_Times)) { Soldier5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Soldier5_Bet)) { Soldier5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Soldier5_Win)) { Soldier5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Soldier4_Times)) { Soldier4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Soldier4_Bet)) { Soldier4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Soldier4_Win)) { Soldier4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Soldier3_Times)) { Soldier3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Soldier3_Bet)) { Soldier3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Soldier3_Win)) { Soldier3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Spear5_Times)) { Spear5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Spear5_Bet)) { Spear5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Spear5_Win)) { Spear5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Spear4_Times)) { Spear4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Spear4_Bet)) { Spear4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Spear4_Win)) { Spear4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Spear3_Times)) { Spear3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Spear3_Bet)) { Spear3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Spear3_Win)) { Spear3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Weapon5_Times)) { Weapon5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Weapon5_Bet)) { Weapon5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Weapon5_Win)) { Weapon5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Weapon4_Times)) { Weapon4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Weapon4_Bet)) { Weapon4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Weapon4_Win)) { Weapon4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Weapon3_Times)) { Weapon3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Weapon3_Bet)) { Weapon3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Weapon3_Win)) { Weapon3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Token5_Times)) { Token5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Token5_Bet)) { Token5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Token5_Win)) { Token5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Token4_Times)) { Token4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Token4_Bet)) { Token4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Token4_Win)) { Token4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Token3_Times)) { Token3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Token3_Bet)) { Token3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Token3_Win)) { Token3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Drum5_Times)) { Drum5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum5_Bet)) { Drum5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Drum5_Win)) { Drum5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Drum4_Times)) { Drum4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum4_Bet)) { Drum4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Drum4_Win)) { Drum4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Drum3_Times)) { Drum3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Drum3_Bet)) { Drum3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Drum3_Win)) { Drum3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd5_Times)) { Gourd5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gourd5_Bet)) { Gourd5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd5_Win)) { Gourd5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd4_Times)) { Gourd4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gourd4_Bet)) { Gourd4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd4_Win)) { Gourd4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd3_Times)) { Gourd3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gourd3_Bet)) { Gourd3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gourd3_Win)) { Gourd3_Win += Convert.ToDouble(value); return; }
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
            if (field == nameof(ExPlay05_Times)) { ExPlay05_Times += Convert.ToInt32(value); return; }
            if (field == nameof(ExPlay05_Bet)) { ExPlay05_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay05_Win)) { ExPlay05_Win += Convert.ToDouble(value); return; }
            if (field == nameof(ExPlay05_WinTimes)) { ExPlay05_WinTimes += Convert.ToDouble(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(WukongRespin_Times), WukongRespin_Times.ToString());
            updata.Add(nameof(WukongRespin_Bet), WukongRespin_Bet.ToString());
            updata.Add(nameof(WukongRespin_Win), WukongRespin_Win.ToString());
            updata.Add(nameof(WukongB_Times), WukongB_Times.ToString());
            updata.Add(nameof(WukongB_Bet), WukongB_Bet.ToString());
            updata.Add(nameof(WukongB_Win), WukongB_Win.ToString());
            updata.Add(nameof(Scatter5_Times), Scatter5_Times.ToString());
            updata.Add(nameof(Scatter5_Bet), Scatter5_Bet.ToString());
            updata.Add(nameof(Scatter5_Win), Scatter5_Win.ToString());
            updata.Add(nameof(Scatter4_Times), Scatter4_Times.ToString());
            updata.Add(nameof(Scatter4_Bet), Scatter4_Bet.ToString());
            updata.Add(nameof(Scatter4_Win), Scatter4_Win.ToString());
            updata.Add(nameof(Scatter3_Times), Scatter3_Times.ToString());
            updata.Add(nameof(Scatter3_Bet), Scatter3_Bet.ToString());
            updata.Add(nameof(Scatter3_Win), Scatter3_Win.ToString());
            updata.Add(nameof(FreeWukongRespin_Times), FreeWukongRespin_Times.ToString());
            updata.Add(nameof(FreeWukongB_Times), FreeWukongB_Times.ToString());
            updata.Add(nameof(GoldenCudgel5_Times), GoldenCudgel5_Times.ToString());
            updata.Add(nameof(GoldenCudgel5_Bet), GoldenCudgel5_Bet.ToString());
            updata.Add(nameof(GoldenCudgel5_Win), GoldenCudgel5_Win.ToString());
            updata.Add(nameof(GoldenCudgel4_Times), GoldenCudgel4_Times.ToString());
            updata.Add(nameof(GoldenCudgel4_Bet), GoldenCudgel4_Bet.ToString());
            updata.Add(nameof(GoldenCudgel4_Win), GoldenCudgel4_Win.ToString());
            updata.Add(nameof(GoldenCudgel3_Times), GoldenCudgel3_Times.ToString());
            updata.Add(nameof(GoldenCudgel3_Bet), GoldenCudgel3_Bet.ToString());
            updata.Add(nameof(GoldenCudgel3_Win), GoldenCudgel3_Win.ToString());
            updata.Add(nameof(Soldier5_Times), Soldier5_Times.ToString());
            updata.Add(nameof(Soldier5_Bet), Soldier5_Bet.ToString());
            updata.Add(nameof(Soldier5_Win), Soldier5_Win.ToString());
            updata.Add(nameof(Soldier4_Times), Soldier4_Times.ToString());
            updata.Add(nameof(Soldier4_Bet), Soldier4_Bet.ToString());
            updata.Add(nameof(Soldier4_Win), Soldier4_Win.ToString());
            updata.Add(nameof(Soldier3_Times), Soldier3_Times.ToString());
            updata.Add(nameof(Soldier3_Bet), Soldier3_Bet.ToString());
            updata.Add(nameof(Soldier3_Win), Soldier3_Win.ToString());
            updata.Add(nameof(Spear5_Times), Spear5_Times.ToString());
            updata.Add(nameof(Spear5_Bet), Spear5_Bet.ToString());
            updata.Add(nameof(Spear5_Win), Spear5_Win.ToString());
            updata.Add(nameof(Spear4_Times), Spear4_Times.ToString());
            updata.Add(nameof(Spear4_Bet), Spear4_Bet.ToString());
            updata.Add(nameof(Spear4_Win), Spear4_Win.ToString());
            updata.Add(nameof(Spear3_Times), Spear3_Times.ToString());
            updata.Add(nameof(Spear3_Bet), Spear3_Bet.ToString());
            updata.Add(nameof(Spear3_Win), Spear3_Win.ToString());
            updata.Add(nameof(Weapon5_Times), Weapon5_Times.ToString());
            updata.Add(nameof(Weapon5_Bet), Weapon5_Bet.ToString());
            updata.Add(nameof(Weapon5_Win), Weapon5_Win.ToString());
            updata.Add(nameof(Weapon4_Times), Weapon4_Times.ToString());
            updata.Add(nameof(Weapon4_Bet), Weapon4_Bet.ToString());
            updata.Add(nameof(Weapon4_Win), Weapon4_Win.ToString());
            updata.Add(nameof(Weapon3_Times), Weapon3_Times.ToString());
            updata.Add(nameof(Weapon3_Bet), Weapon3_Bet.ToString());
            updata.Add(nameof(Weapon3_Win), Weapon3_Win.ToString());
            updata.Add(nameof(Token5_Times), Token5_Times.ToString());
            updata.Add(nameof(Token5_Bet), Token5_Bet.ToString());
            updata.Add(nameof(Token5_Win), Token5_Win.ToString());
            updata.Add(nameof(Token4_Times), Token4_Times.ToString());
            updata.Add(nameof(Token4_Bet), Token4_Bet.ToString());
            updata.Add(nameof(Token4_Win), Token4_Win.ToString());
            updata.Add(nameof(Token3_Times), Token3_Times.ToString());
            updata.Add(nameof(Token3_Bet), Token3_Bet.ToString());
            updata.Add(nameof(Token3_Win), Token3_Win.ToString());
            updata.Add(nameof(Drum5_Times), Drum5_Times.ToString());
            updata.Add(nameof(Drum5_Bet), Drum5_Bet.ToString());
            updata.Add(nameof(Drum5_Win), Drum5_Win.ToString());
            updata.Add(nameof(Drum4_Times), Drum4_Times.ToString());
            updata.Add(nameof(Drum4_Bet), Drum4_Bet.ToString());
            updata.Add(nameof(Drum4_Win), Drum4_Win.ToString());
            updata.Add(nameof(Drum3_Times), Drum3_Times.ToString());
            updata.Add(nameof(Drum3_Bet), Drum3_Bet.ToString());
            updata.Add(nameof(Drum3_Win), Drum3_Win.ToString());
            updata.Add(nameof(Gourd5_Times), Gourd5_Times.ToString());
            updata.Add(nameof(Gourd5_Bet), Gourd5_Bet.ToString());
            updata.Add(nameof(Gourd5_Win), Gourd5_Win.ToString());
            updata.Add(nameof(Gourd4_Times), Gourd4_Times.ToString());
            updata.Add(nameof(Gourd4_Bet), Gourd4_Bet.ToString());
            updata.Add(nameof(Gourd4_Win), Gourd4_Win.ToString());
            updata.Add(nameof(Gourd3_Times), Gourd3_Times.ToString());
            updata.Add(nameof(Gourd3_Bet), Gourd3_Bet.ToString());
            updata.Add(nameof(Gourd3_Win), Gourd3_Win.ToString());
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
            updata.Add(nameof(ExPlay05_Times), ExPlay05_Times.ToString());
            updata.Add(nameof(ExPlay05_Bet), ExPlay05_Bet.ToString());
            updata.Add(nameof(ExPlay05_Win), ExPlay05_Win.ToString());
            updata.Add(nameof(ExPlay05_WinTimes), ExPlay05_WinTimes.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            WukongRespin_Times = Convert.ToInt32(datalist[nameof(WukongRespin_Times)]);
            WukongRespin_Bet = Convert.ToDouble(datalist[nameof(WukongRespin_Bet)]);
            WukongRespin_Win = Convert.ToDouble(datalist[nameof(WukongRespin_Win)]);
            WukongB_Times = Convert.ToInt32(datalist[nameof(WukongB_Times)]);
            WukongB_Bet = Convert.ToDouble(datalist[nameof(WukongB_Bet)]);
            WukongB_Win = Convert.ToDouble(datalist[nameof(WukongB_Win)]);
            Scatter5_Times = Convert.ToInt32(datalist[nameof(Scatter5_Times)]);
            Scatter5_Bet = Convert.ToDouble(datalist[nameof(Scatter5_Bet)]);
            Scatter5_Win = Convert.ToDouble(datalist[nameof(Scatter5_Win)]);
            Scatter4_Times = Convert.ToInt32(datalist[nameof(Scatter4_Times)]);
            Scatter4_Bet = Convert.ToDouble(datalist[nameof(Scatter4_Bet)]);
            Scatter4_Win = Convert.ToDouble(datalist[nameof(Scatter4_Win)]);
            Scatter3_Times = Convert.ToInt32(datalist[nameof(Scatter3_Times)]);
            Scatter3_Bet = Convert.ToDouble(datalist[nameof(Scatter3_Bet)]);
            Scatter3_Win = Convert.ToDouble(datalist[nameof(Scatter3_Win)]);
            FreeWukongRespin_Times = Convert.ToInt32(datalist[nameof(FreeWukongRespin_Times)]);
            FreeWukongB_Times = Convert.ToInt32(datalist[nameof(FreeWukongB_Times)]);
            GoldenCudgel5_Times = Convert.ToInt32(datalist[nameof(GoldenCudgel5_Times)]);
            GoldenCudgel5_Bet = Convert.ToDouble(datalist[nameof(GoldenCudgel5_Bet)]);
            GoldenCudgel5_Win = Convert.ToDouble(datalist[nameof(GoldenCudgel5_Win)]);
            GoldenCudgel4_Times = Convert.ToInt32(datalist[nameof(GoldenCudgel4_Times)]);
            GoldenCudgel4_Bet = Convert.ToDouble(datalist[nameof(GoldenCudgel4_Bet)]);
            GoldenCudgel4_Win = Convert.ToDouble(datalist[nameof(GoldenCudgel4_Win)]);
            GoldenCudgel3_Times = Convert.ToInt32(datalist[nameof(GoldenCudgel3_Times)]);
            GoldenCudgel3_Bet = Convert.ToDouble(datalist[nameof(GoldenCudgel3_Bet)]);
            GoldenCudgel3_Win = Convert.ToDouble(datalist[nameof(GoldenCudgel3_Win)]);
            Soldier5_Times = Convert.ToInt32(datalist[nameof(Soldier5_Times)]);
            Soldier5_Bet = Convert.ToDouble(datalist[nameof(Soldier5_Bet)]);
            Soldier5_Win = Convert.ToDouble(datalist[nameof(Soldier5_Win)]);
            Soldier4_Times = Convert.ToInt32(datalist[nameof(Soldier4_Times)]);
            Soldier4_Bet = Convert.ToDouble(datalist[nameof(Soldier4_Bet)]);
            Soldier4_Win = Convert.ToDouble(datalist[nameof(Soldier4_Win)]);
            Soldier3_Times = Convert.ToInt32(datalist[nameof(Soldier3_Times)]);
            Soldier3_Bet = Convert.ToDouble(datalist[nameof(Soldier3_Bet)]);
            Soldier3_Win = Convert.ToDouble(datalist[nameof(Soldier3_Win)]);
            Spear5_Times = Convert.ToInt32(datalist[nameof(Spear5_Times)]);
            Spear5_Bet = Convert.ToDouble(datalist[nameof(Spear5_Bet)]);
            Spear5_Win = Convert.ToDouble(datalist[nameof(Spear5_Win)]);
            Spear4_Times = Convert.ToInt32(datalist[nameof(Spear4_Times)]);
            Spear4_Bet = Convert.ToDouble(datalist[nameof(Spear4_Bet)]);
            Spear4_Win = Convert.ToDouble(datalist[nameof(Spear4_Win)]);
            Spear3_Times = Convert.ToInt32(datalist[nameof(Spear3_Times)]);
            Spear3_Bet = Convert.ToDouble(datalist[nameof(Spear3_Bet)]);
            Spear3_Win = Convert.ToDouble(datalist[nameof(Spear3_Win)]);
            Weapon5_Times = Convert.ToInt32(datalist[nameof(Weapon5_Times)]);
            Weapon5_Bet = Convert.ToDouble(datalist[nameof(Weapon5_Bet)]);
            Weapon5_Win = Convert.ToDouble(datalist[nameof(Weapon5_Win)]);
            Weapon4_Times = Convert.ToInt32(datalist[nameof(Weapon4_Times)]);
            Weapon4_Bet = Convert.ToDouble(datalist[nameof(Weapon4_Bet)]);
            Weapon4_Win = Convert.ToDouble(datalist[nameof(Weapon4_Win)]);
            Weapon3_Times = Convert.ToInt32(datalist[nameof(Weapon3_Times)]);
            Weapon3_Bet = Convert.ToDouble(datalist[nameof(Weapon3_Bet)]);
            Weapon3_Win = Convert.ToDouble(datalist[nameof(Weapon3_Win)]);
            Token5_Times = Convert.ToInt32(datalist[nameof(Token5_Times)]);
            Token5_Bet = Convert.ToDouble(datalist[nameof(Token5_Bet)]);
            Token5_Win = Convert.ToDouble(datalist[nameof(Token5_Win)]);
            Token4_Times = Convert.ToInt32(datalist[nameof(Token4_Times)]);
            Token4_Bet = Convert.ToDouble(datalist[nameof(Token4_Bet)]);
            Token4_Win = Convert.ToDouble(datalist[nameof(Token4_Win)]);
            Token3_Times = Convert.ToInt32(datalist[nameof(Token3_Times)]);
            Token3_Bet = Convert.ToDouble(datalist[nameof(Token3_Bet)]);
            Token3_Win = Convert.ToDouble(datalist[nameof(Token3_Win)]);
            Drum5_Times = Convert.ToInt32(datalist[nameof(Drum5_Times)]);
            Drum5_Bet = Convert.ToDouble(datalist[nameof(Drum5_Bet)]);
            Drum5_Win = Convert.ToDouble(datalist[nameof(Drum5_Win)]);
            Drum4_Times = Convert.ToInt32(datalist[nameof(Drum4_Times)]);
            Drum4_Bet = Convert.ToDouble(datalist[nameof(Drum4_Bet)]);
            Drum4_Win = Convert.ToDouble(datalist[nameof(Drum4_Win)]);
            Drum3_Times = Convert.ToInt32(datalist[nameof(Drum3_Times)]);
            Drum3_Bet = Convert.ToDouble(datalist[nameof(Drum3_Bet)]);
            Drum3_Win = Convert.ToDouble(datalist[nameof(Drum3_Win)]);
            Gourd5_Times = Convert.ToInt32(datalist[nameof(Gourd5_Times)]);
            Gourd5_Bet = Convert.ToDouble(datalist[nameof(Gourd5_Bet)]);
            Gourd5_Win = Convert.ToDouble(datalist[nameof(Gourd5_Win)]);
            Gourd4_Times = Convert.ToInt32(datalist[nameof(Gourd4_Times)]);
            Gourd4_Bet = Convert.ToDouble(datalist[nameof(Gourd4_Bet)]);
            Gourd4_Win = Convert.ToDouble(datalist[nameof(Gourd4_Win)]);
            Gourd3_Times = Convert.ToInt32(datalist[nameof(Gourd3_Times)]);
            Gourd3_Bet = Convert.ToDouble(datalist[nameof(Gourd3_Bet)]);
            Gourd3_Win = Convert.ToDouble(datalist[nameof(Gourd3_Win)]);
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
            ExPlay05_Times = Convert.ToInt32(datalist[nameof(ExPlay05_Times)]);
            ExPlay05_Bet = Convert.ToDouble(datalist[nameof(ExPlay05_Bet)]);
            ExPlay05_Win = Convert.ToDouble(datalist[nameof(ExPlay05_Win)]);
            ExPlay05_WinTimes = Convert.ToDouble(datalist[nameof(ExPlay05_WinTimes)]);
        }
    }
}
