using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class DeepSeaWitchAccountData : CommonAccountData
    {
        #region 遊戲紀錄
        private int RespinPurple_Times;
        private double RespinPurple_Bet;
        private double RespinPurple_Win;
        private int RespinBlue_Times;
        private double RespinBlue_Bet;
        private double RespinBlue_Win;
        private int Scatter5_Times;
        private double Scatter5_Bet;
        private double Scatter5_Win;
        private int Scatter4_Times;
        private double Scatter4_Bet;
        private double Scatter4_Win;
        private int Scatter3_Times;
        private double Scatter3_Bet;
        private double Scatter3_Win;
        private int Gem_1115_Times;
        private double Gem_1115_Bet;
        private double Gem_1115_Win;
        private int Gem_0810_Times;
        private double Gem_0810_Bet;
        private double Gem_0810_Win;
        private int Gem_0507_Times;
        private double Gem_0507_Bet;
        private double Gem_0507_Win;
        private int Gem_0104_Times;
        private double Gem_0104_Bet;
        private double Gem_0104_Win;
        private int Logo5_Times;
        private double Logo5_Bet;
        private double Logo5_Win;
        private int Logo4_Times;
        private double Logo4_Bet;
        private double Logo4_Win;
        private int Logo3_Times;
        private double Logo3_Bet;
        private double Logo3_Win;
        private int Witch5_Times;
        private double Witch5_Bet;
        private double Witch5_Win;
        private int Witch4_Times;
        private double Witch4_Bet;
        private double Witch4_Win;
        private int Witch3_Times;
        private double Witch3_Bet;
        private double Witch3_Win;
        private int Princess5_Times;
        private double Princess5_Bet;
        private double Princess5_Win;
        private int Princess4_Times;
        private double Princess4_Bet;
        private double Princess4_Win;
        private int Princess3_Times;
        private double Princess3_Bet;
        private double Princess3_Win;
        private int Prince5_Times;
        private double Prince5_Bet;
        private double Prince5_Win;
        private int Prince4_Times;
        private double Prince4_Bet;
        private double Prince4_Win;
        private int Prince3_Times;
        private double Prince3_Bet;
        private double Prince3_Win;
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
        private int Wild5_Times;
        private double Wild5_Bet;
        private double Wild5_Win;
        private int Wild4_Times;
        private double Wild4_Bet;
        private double Wild4_Win;
        private int Wild3_Times;
        private double Wild3_Bet;
        private double Wild3_Win;
        #endregion

        /// <summary>清除額外押注</summary>
        public override void ClearCacheGameExPlay()
        {
            //update = true;
        }
		
        /// <summary>清除內存(mode:0=常規, 1=獨立買, 2=全部)</summary>
        public override void ClearCacheGame(int mode)
        {
            RespinPurple_Times = 0;
            RespinPurple_Bet = 0;
            RespinPurple_Win = 0;
            RespinBlue_Times = 0;
            RespinBlue_Bet = 0;
            RespinBlue_Win = 0;
            Scatter5_Times = 0;
            Scatter5_Bet = 0;
            Scatter5_Win = 0;
            Scatter4_Times = 0;
            Scatter4_Bet = 0;
            Scatter4_Win = 0;
            Scatter3_Times = 0;
            Scatter3_Bet = 0;
            Scatter3_Win = 0;
            Gem_1115_Times = 0;
            Gem_1115_Bet = 0;
            Gem_1115_Win = 0;
            Gem_0810_Times = 0;
            Gem_0810_Bet = 0;
            Gem_0810_Win = 0;
            Gem_0507_Times = 0;
            Gem_0507_Bet = 0;
            Gem_0507_Win = 0;
            Gem_0104_Times = 0;
            Gem_0104_Bet = 0;
            Gem_0104_Win = 0;
            Logo5_Times = 0;
            Logo5_Bet = 0;
            Logo5_Win = 0;
            Logo4_Times = 0;
            Logo4_Bet = 0;
            Logo4_Win = 0;
            Logo3_Times = 0;
            Logo3_Bet = 0;
            Logo3_Win = 0;
            Witch5_Times = 0;
            Witch5_Bet = 0;
            Witch5_Win = 0;
            Witch4_Times = 0;
            Witch4_Bet = 0;
            Witch4_Win = 0;
            Witch3_Times = 0;
            Witch3_Bet = 0;
            Witch3_Win = 0;
            Princess5_Times = 0;
            Princess5_Bet = 0;
            Princess5_Win = 0;
            Princess4_Times = 0;
            Princess4_Bet = 0;
            Princess4_Win = 0;
            Princess3_Times = 0;
            Princess3_Bet = 0;
            Princess3_Win = 0;
            Prince5_Times = 0;
            Prince5_Bet = 0;
            Prince5_Win = 0;
            Prince4_Times = 0;
            Prince4_Bet = 0;
            Prince4_Win = 0;
            Prince3_Times = 0;
            Prince3_Bet = 0;
            Prince3_Win = 0;
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
            Wild5_Times = 0;
            Wild5_Bet = 0;
            Wild5_Win = 0;
            Wild4_Times = 0;
            Wild4_Bet = 0;
            Wild4_Win = 0;
            Wild3_Times = 0;
            Wild3_Bet = 0;
            Wild3_Win = 0;
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public override void AccumulatecCacheDataGame(string field, string value)
        {
            if (field == nameof(RespinPurple_Times)) { RespinPurple_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RespinPurple_Bet)) { RespinPurple_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(RespinPurple_Win)) { RespinPurple_Win += Convert.ToDouble(value); return; }
            if (field == nameof(RespinBlue_Times)) { RespinBlue_Times += Convert.ToInt32(value); return; }
            if (field == nameof(RespinBlue_Bet)) { RespinBlue_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(RespinBlue_Win)) { RespinBlue_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Times)) { Scatter5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter5_Bet)) { Scatter5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter5_Win)) { Scatter5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Times)) { Scatter4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter4_Bet)) { Scatter4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter4_Win)) { Scatter4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Times)) { Scatter3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Scatter3_Bet)) { Scatter3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Scatter3_Win)) { Scatter3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_1115_Times)) { Gem_1115_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gem_1115_Bet)) { Gem_1115_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_1115_Win)) { Gem_1115_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_0810_Times)) { Gem_0810_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gem_0810_Bet)) { Gem_0810_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_0810_Win)) { Gem_0810_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_0507_Times)) { Gem_0507_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gem_0507_Bet)) { Gem_0507_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_0507_Win)) { Gem_0507_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_0104_Times)) { Gem_0104_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Gem_0104_Bet)) { Gem_0104_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Gem_0104_Win)) { Gem_0104_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Times)) { Logo5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo5_Bet)) { Logo5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo5_Win)) { Logo5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Times)) { Logo4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo4_Bet)) { Logo4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo4_Win)) { Logo4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Times)) { Logo3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Logo3_Bet)) { Logo3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Logo3_Win)) { Logo3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Witch5_Times)) { Witch5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Witch5_Bet)) { Witch5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Witch5_Win)) { Witch5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Witch4_Times)) { Witch4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Witch4_Bet)) { Witch4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Witch4_Win)) { Witch4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Witch3_Times)) { Witch3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Witch3_Bet)) { Witch3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Witch3_Win)) { Witch3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Princess5_Times)) { Princess5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Princess5_Bet)) { Princess5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Princess5_Win)) { Princess5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Princess4_Times)) { Princess4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Princess4_Bet)) { Princess4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Princess4_Win)) { Princess4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Princess3_Times)) { Princess3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Princess3_Bet)) { Princess3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Princess3_Win)) { Princess3_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Prince5_Times)) { Prince5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Prince5_Bet)) { Prince5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Prince5_Win)) { Prince5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Prince4_Times)) { Prince4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Prince4_Bet)) { Prince4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Prince4_Win)) { Prince4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Prince3_Times)) { Prince3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Prince3_Bet)) { Prince3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Prince3_Win)) { Prince3_Win += Convert.ToDouble(value); return; }
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
            if (field == nameof(Wild5_Times)) { Wild5_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Wild5_Bet)) { Wild5_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Wild5_Win)) { Wild5_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Wild4_Times)) { Wild4_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Wild4_Bet)) { Wild4_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Wild4_Win)) { Wild4_Win += Convert.ToDouble(value); return; }
            if (field == nameof(Wild3_Times)) { Wild3_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Wild3_Bet)) { Wild3_Bet += Convert.ToDouble(value); return; }
            if (field == nameof(Wild3_Win)) { Wild3_Win += Convert.ToDouble(value); return; }
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public override void GetUpdateDataGame(Dictionary<string, string> updata)
        {
            updata.Add(nameof(RespinPurple_Times), RespinPurple_Times.ToString());
            updata.Add(nameof(RespinPurple_Bet), RespinPurple_Bet.ToString());
            updata.Add(nameof(RespinPurple_Win), RespinPurple_Win.ToString());
            updata.Add(nameof(RespinBlue_Times), RespinBlue_Times.ToString());
            updata.Add(nameof(RespinBlue_Bet), RespinBlue_Bet.ToString());
            updata.Add(nameof(RespinBlue_Win), RespinBlue_Win.ToString());
            updata.Add(nameof(Scatter5_Times), Scatter5_Times.ToString());
            updata.Add(nameof(Scatter5_Bet), Scatter5_Bet.ToString());
            updata.Add(nameof(Scatter5_Win), Scatter5_Win.ToString());
            updata.Add(nameof(Scatter4_Times), Scatter4_Times.ToString());
            updata.Add(nameof(Scatter4_Bet), Scatter4_Bet.ToString());
            updata.Add(nameof(Scatter4_Win), Scatter4_Win.ToString());
            updata.Add(nameof(Scatter3_Times), Scatter3_Times.ToString());
            updata.Add(nameof(Scatter3_Bet), Scatter3_Bet.ToString());
            updata.Add(nameof(Scatter3_Win), Scatter3_Win.ToString());
            updata.Add(nameof(Gem_1115_Times), Gem_1115_Times.ToString());
            updata.Add(nameof(Gem_1115_Bet), Gem_1115_Bet.ToString());
            updata.Add(nameof(Gem_1115_Win), Gem_1115_Win.ToString());
            updata.Add(nameof(Gem_0810_Times), Gem_0810_Times.ToString());
            updata.Add(nameof(Gem_0810_Bet), Gem_0810_Bet.ToString());
            updata.Add(nameof(Gem_0810_Win), Gem_0810_Win.ToString());
            updata.Add(nameof(Gem_0507_Times), Gem_0507_Times.ToString());
            updata.Add(nameof(Gem_0507_Bet), Gem_0507_Bet.ToString());
            updata.Add(nameof(Gem_0507_Win), Gem_0507_Win.ToString());
            updata.Add(nameof(Gem_0104_Times), Gem_0104_Times.ToString());
            updata.Add(nameof(Gem_0104_Bet), Gem_0104_Bet.ToString());
            updata.Add(nameof(Gem_0104_Win), Gem_0104_Win.ToString());
            updata.Add(nameof(Logo5_Times), Logo5_Times.ToString());
            updata.Add(nameof(Logo5_Bet), Logo5_Bet.ToString());
            updata.Add(nameof(Logo5_Win), Logo5_Win.ToString());
            updata.Add(nameof(Logo4_Times), Logo4_Times.ToString());
            updata.Add(nameof(Logo4_Bet), Logo4_Bet.ToString());
            updata.Add(nameof(Logo4_Win), Logo4_Win.ToString());
            updata.Add(nameof(Logo3_Times), Logo3_Times.ToString());
            updata.Add(nameof(Logo3_Bet), Logo3_Bet.ToString());
            updata.Add(nameof(Logo3_Win), Logo3_Win.ToString());
            updata.Add(nameof(Witch5_Times), Witch5_Times.ToString());
            updata.Add(nameof(Witch5_Bet), Witch5_Bet.ToString());
            updata.Add(nameof(Witch5_Win), Witch5_Win.ToString());
            updata.Add(nameof(Witch4_Times), Witch4_Times.ToString());
            updata.Add(nameof(Witch4_Bet), Witch4_Bet.ToString());
            updata.Add(nameof(Witch4_Win), Witch4_Win.ToString());
            updata.Add(nameof(Witch3_Times), Witch3_Times.ToString());
            updata.Add(nameof(Witch3_Bet), Witch3_Bet.ToString());
            updata.Add(nameof(Witch3_Win), Witch3_Win.ToString());
            updata.Add(nameof(Princess5_Times), Princess5_Times.ToString());
            updata.Add(nameof(Princess5_Bet), Princess5_Bet.ToString());
            updata.Add(nameof(Princess5_Win), Princess5_Win.ToString());
            updata.Add(nameof(Princess4_Times), Princess4_Times.ToString());
            updata.Add(nameof(Princess4_Bet), Princess4_Bet.ToString());
            updata.Add(nameof(Princess4_Win), Princess4_Win.ToString());
            updata.Add(nameof(Princess3_Times), Princess3_Times.ToString());
            updata.Add(nameof(Princess3_Bet), Princess3_Bet.ToString());
            updata.Add(nameof(Princess3_Win), Princess3_Win.ToString());
            updata.Add(nameof(Prince5_Times), Prince5_Times.ToString());
            updata.Add(nameof(Prince5_Bet), Prince5_Bet.ToString());
            updata.Add(nameof(Prince5_Win), Prince5_Win.ToString());
            updata.Add(nameof(Prince4_Times), Prince4_Times.ToString());
            updata.Add(nameof(Prince4_Bet), Prince4_Bet.ToString());
            updata.Add(nameof(Prince4_Win), Prince4_Win.ToString());
            updata.Add(nameof(Prince3_Times), Prince3_Times.ToString());
            updata.Add(nameof(Prince3_Bet), Prince3_Bet.ToString());
            updata.Add(nameof(Prince3_Win), Prince3_Win.ToString());
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
            updata.Add(nameof(Wild5_Times), Wild5_Times.ToString());
            updata.Add(nameof(Wild5_Bet), Wild5_Bet.ToString());
            updata.Add(nameof(Wild5_Win), Wild5_Win.ToString());
            updata.Add(nameof(Wild4_Times), Wild4_Times.ToString());
            updata.Add(nameof(Wild4_Bet), Wild4_Bet.ToString());
            updata.Add(nameof(Wild4_Win), Wild4_Win.ToString());
            updata.Add(nameof(Wild3_Times), Wild3_Times.ToString());
            updata.Add(nameof(Wild3_Bet), Wild3_Bet.ToString());
            updata.Add(nameof(Wild3_Win), Wild3_Win.ToString());
        }

        /// <summary>從DB資料更新內存(從DB讀回)</summary>
        public override void GetDBDataGame(Dictionary<string, string> datalist)
        {
            RespinPurple_Times = Convert.ToInt32(datalist[nameof(RespinPurple_Times)]);
            RespinPurple_Bet = Convert.ToDouble(datalist[nameof(RespinPurple_Bet)]);
            RespinPurple_Win = Convert.ToDouble(datalist[nameof(RespinPurple_Win)]);
            RespinBlue_Times = Convert.ToInt32(datalist[nameof(RespinBlue_Times)]);
            RespinBlue_Bet = Convert.ToDouble(datalist[nameof(RespinBlue_Bet)]);
            RespinBlue_Win = Convert.ToDouble(datalist[nameof(RespinBlue_Win)]);
            Scatter5_Times = Convert.ToInt32(datalist[nameof(Scatter5_Times)]);
            Scatter5_Bet = Convert.ToDouble(datalist[nameof(Scatter5_Bet)]);
            Scatter5_Win = Convert.ToDouble(datalist[nameof(Scatter5_Win)]);
            Scatter4_Times = Convert.ToInt32(datalist[nameof(Scatter4_Times)]);
            Scatter4_Bet = Convert.ToDouble(datalist[nameof(Scatter4_Bet)]);
            Scatter4_Win = Convert.ToDouble(datalist[nameof(Scatter4_Win)]);
            Scatter3_Times = Convert.ToInt32(datalist[nameof(Scatter3_Times)]);
            Scatter3_Bet = Convert.ToDouble(datalist[nameof(Scatter3_Bet)]);
            Scatter3_Win = Convert.ToDouble(datalist[nameof(Scatter3_Win)]);
            Gem_1115_Times = Convert.ToInt32(datalist[nameof(Gem_1115_Times)]);
            Gem_1115_Bet = Convert.ToDouble(datalist[nameof(Gem_1115_Bet)]);
            Gem_1115_Win = Convert.ToDouble(datalist[nameof(Gem_1115_Win)]);
            Gem_0810_Times = Convert.ToInt32(datalist[nameof(Gem_0810_Times)]);
            Gem_0810_Bet = Convert.ToDouble(datalist[nameof(Gem_0810_Bet)]);
            Gem_0810_Win = Convert.ToDouble(datalist[nameof(Gem_0810_Win)]);
            Gem_0507_Times = Convert.ToInt32(datalist[nameof(Gem_0507_Times)]);
            Gem_0507_Bet = Convert.ToDouble(datalist[nameof(Gem_0507_Bet)]);
            Gem_0507_Win = Convert.ToDouble(datalist[nameof(Gem_0507_Win)]);
            Gem_0104_Times = Convert.ToInt32(datalist[nameof(Gem_0104_Times)]);
            Gem_0104_Bet = Convert.ToDouble(datalist[nameof(Gem_0104_Bet)]);
            Gem_0104_Win = Convert.ToDouble(datalist[nameof(Gem_0104_Win)]);
            Logo5_Times = Convert.ToInt32(datalist[nameof(Logo5_Times)]);
            Logo5_Bet = Convert.ToDouble(datalist[nameof(Logo5_Bet)]);
            Logo5_Win = Convert.ToDouble(datalist[nameof(Logo5_Win)]);
            Logo4_Times = Convert.ToInt32(datalist[nameof(Logo4_Times)]);
            Logo4_Bet = Convert.ToDouble(datalist[nameof(Logo4_Bet)]);
            Logo4_Win = Convert.ToDouble(datalist[nameof(Logo4_Win)]);
            Logo3_Times = Convert.ToInt32(datalist[nameof(Logo3_Times)]);
            Logo3_Bet = Convert.ToDouble(datalist[nameof(Logo3_Bet)]);
            Logo3_Win = Convert.ToDouble(datalist[nameof(Logo3_Win)]);
            Witch5_Times = Convert.ToInt32(datalist[nameof(Witch5_Times)]);
            Witch5_Bet = Convert.ToDouble(datalist[nameof(Witch5_Bet)]);
            Witch5_Win = Convert.ToDouble(datalist[nameof(Witch5_Win)]);
            Witch4_Times = Convert.ToInt32(datalist[nameof(Witch4_Times)]);
            Witch4_Bet = Convert.ToDouble(datalist[nameof(Witch4_Bet)]);
            Witch4_Win = Convert.ToDouble(datalist[nameof(Witch4_Win)]);
            Witch3_Times = Convert.ToInt32(datalist[nameof(Witch3_Times)]);
            Witch3_Bet = Convert.ToDouble(datalist[nameof(Witch3_Bet)]);
            Witch3_Win = Convert.ToDouble(datalist[nameof(Witch3_Win)]);
            Princess5_Times = Convert.ToInt32(datalist[nameof(Princess5_Times)]);
            Princess5_Bet = Convert.ToDouble(datalist[nameof(Princess5_Bet)]);
            Princess5_Win = Convert.ToDouble(datalist[nameof(Princess5_Win)]);
            Princess4_Times = Convert.ToInt32(datalist[nameof(Princess4_Times)]);
            Princess4_Bet = Convert.ToDouble(datalist[nameof(Princess4_Bet)]);
            Princess4_Win = Convert.ToDouble(datalist[nameof(Princess4_Win)]);
            Princess3_Times = Convert.ToInt32(datalist[nameof(Princess3_Times)]);
            Princess3_Bet = Convert.ToDouble(datalist[nameof(Princess3_Bet)]);
            Princess3_Win = Convert.ToDouble(datalist[nameof(Princess3_Win)]);
            Prince5_Times = Convert.ToInt32(datalist[nameof(Prince5_Times)]);
            Prince5_Bet = Convert.ToDouble(datalist[nameof(Prince5_Bet)]);
            Prince5_Win = Convert.ToDouble(datalist[nameof(Prince5_Win)]);
            Prince4_Times = Convert.ToInt32(datalist[nameof(Prince4_Times)]);
            Prince4_Bet = Convert.ToDouble(datalist[nameof(Prince4_Bet)]);
            Prince4_Win = Convert.ToDouble(datalist[nameof(Prince4_Win)]);
            Prince3_Times = Convert.ToInt32(datalist[nameof(Prince3_Times)]);
            Prince3_Bet = Convert.ToDouble(datalist[nameof(Prince3_Bet)]);
            Prince3_Win = Convert.ToDouble(datalist[nameof(Prince3_Win)]);
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
            Wild5_Times = Convert.ToInt32(datalist[nameof(Wild5_Times)]);
            Wild5_Bet = Convert.ToDouble(datalist[nameof(Wild5_Bet)]);
            Wild5_Win = Convert.ToDouble(datalist[nameof(Wild5_Win)]);
            Wild4_Times = Convert.ToInt32(datalist[nameof(Wild4_Times)]);
            Wild4_Bet = Convert.ToDouble(datalist[nameof(Wild4_Bet)]);
            Wild4_Win = Convert.ToDouble(datalist[nameof(Wild4_Win)]);
            Wild3_Times = Convert.ToInt32(datalist[nameof(Wild3_Times)]);
            Wild3_Bet = Convert.ToDouble(datalist[nameof(Wild3_Bet)]);
            Wild3_Win = Convert.ToDouble(datalist[nameof(Wild3_Win)]);
        }
    }
}
