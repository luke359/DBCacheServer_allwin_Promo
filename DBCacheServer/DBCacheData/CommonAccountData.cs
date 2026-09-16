using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public abstract class CommonAccountData
    {
        /// <summary>唯一碼</summary>
        public int AccountUID;
        /// <summary>機台唯一碼</summary>
        public int MachineUID;
        /// <summary>總押</summary>
        public decimal TotalBet;
        /// <summary>總贏</summary>
        public decimal TotalWin;
        /// <summary>總盈餘</summary>
        public decimal TotalSurplus;
        /// <summary>遊戲次數</summary>
        public int GameTimes;
        /// <summary>贏的次數</summary>
        public int WinTimes;

        /// <summary>Super次數</summary>
        public int Super_Times;
        /// <summary>Super總押</summary>
        public decimal Super_Bet;
        /// <summary>Super總贏</summary>
        public decimal Super_Win;
        /// <summary>Mega次數</summary>
        public int Mega_Times;
        /// <summary>Mega總押</summary>
        public decimal Mega_Bet;
        /// <summary>Mega總贏</summary>
        public decimal Mega_Win;
        /// <summary>Major次數</summary>
        public int Major_Times;
        /// <summary>Major總押</summary>
        public decimal Major_Bet;
        /// <summary>Major總贏</summary>
        public decimal Major_Win;
        /// <summary>Minor次數</summary>
        public int Minor_Times;
        /// <summary>Minor總押</summary>
        public decimal Minor_Bet;
        /// <summary>Minor總贏</summary>
        public decimal Minor_Win;

        /// <summary>Web秀出的內帳或外帳</summary>
        public int AccountType;
        /// <summary>紀錄時間(日帳使用)</summary>
        public DateTime RecDate;
        /// <summary>更新旗號(資料已變更)</summary>
        public bool update;

        /// <summary>累計更新遊戲紀錄內存</summary>
        public void AccumulatecGameData(Dictionary<string, string> refdata)
        {
            double betAdd = Convert.ToDouble(refdata[nameof(TotalBet)]);
            double winAdd = Convert.ToDouble(refdata[nameof(TotalWin)]);

            if (betAdd > 0)
            {
                TotalBet += (decimal)betAdd;
                GameTimes++;
            }
            if (winAdd > 0)
            {
                TotalWin += (decimal)winAdd;
                WinTimes++;
            }
            TotalSurplus = TotalBet - TotalWin;

            foreach (KeyValuePair<string, string> info in refdata)
            {
                AccumulatecCacheData(info.Key, info.Value);
            }

            update = true;
        }

        /// <summary>累計更新JP內存</summary>
        public void AccumulatecJPData(JpAward jPType, double jPBet, double jPWin)
        {
            if (jPWin > 0)
            {
                TotalWin += (decimal)jPWin;
                TotalSurplus = TotalBet - TotalWin;
            }

            if (jPType == JpAward.MINOR)
            {
                Minor_Times += 1;
                Minor_Bet += (decimal)jPBet;
                Minor_Win += (decimal)jPWin;
            }
            else if (jPType == JpAward.MAJOR)
            {
                Major_Times += 1;
                Major_Bet += (decimal)jPBet;
                Major_Win += (decimal)jPWin;
            }
            else if (jPType == JpAward.MEGA)
            {
                Mega_Times += 1;
                Mega_Bet += (decimal)jPBet;
                Mega_Win += (decimal)jPWin;
            }
            else if (jPType == JpAward.SUPER)
            {
                Super_Times += 1;
                Super_Bet += (decimal)jPBet;
                Super_Win += (decimal)jPWin;
            }
        }


        /// <summary>清除額外押注</summary>
        public abstract void ClearCacheGameExPlay();
        /// <summary>清除內存(mode:0=常規, 1=獨立買, 2=全部)</summary>
        public abstract void ClearCacheGame(int mode);
        /// <summary>清除內存(mode:0=常規, 1=獨立買, 2=全部)</summary>
        public void ClearCache(int mode)
        {
            if (mode == 0 || mode == 2)
            {
                TotalBet = 0;
                TotalWin = 0;
                TotalSurplus = 0;
                GameTimes = 0;
                WinTimes = 0;

                Super_Times = 0;
                Super_Bet = 0;
                Super_Win = 0;
                Mega_Times = 0;
                Mega_Bet = 0;
                Mega_Win = 0;
                Major_Times = 0;
                Major_Bet = 0;
                Major_Win = 0;
                Minor_Times = 0;
                Minor_Bet = 0;
                Minor_Win = 0;
            }

            //Fortune_Times = 0;
            ClearCacheGame(mode);

            update = true;
        }

        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        public abstract void AccumulatecCacheDataGame(string field, string value);
        /// <summary>累計更新出牌紀錄內存(GameServer通知更新)</summary>
        void AccumulatecCacheData(string field, string value)
        {
            if (field == nameof(Super_Times)) { Super_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Super_Bet)) { Super_Bet += (decimal)Convert.ToDouble(value); return; }
            if (field == nameof(Super_Win)) { Super_Win += (decimal)Convert.ToDouble(value); return; }
            if (field == nameof(Mega_Times)) { Mega_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Mega_Bet)) { Mega_Bet += (decimal)Convert.ToDouble(value); return; }
            if (field == nameof(Mega_Win)) { Mega_Win += (decimal)Convert.ToDouble(value); return; }
            if (field == nameof(Major_Times)) { Major_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Major_Bet)) { Major_Bet += (decimal)Convert.ToDouble(value); return; }
            if (field == nameof(Major_Win)) { Major_Win += (decimal)Convert.ToDouble(value); return; }
            if (field == nameof(Minor_Times)) { Minor_Times += Convert.ToInt32(value); return; }
            if (field == nameof(Minor_Bet)) { Minor_Bet += (decimal)Convert.ToDouble(value); return; }
            if (field == nameof(Minor_Win)) { Minor_Win += (decimal)Convert.ToDouble(value); return; }

            //else if (field == "Fortune_Times") Fortune_Times += Convert.ToInt32(value);
            AccumulatecCacheDataGame(field, value);
        }

        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public abstract void GetUpdateDataGame(Dictionary<string, string> updata);
        /// <summary>依據內存製作更新字典(寫出至DB)</summary>
        public Dictionary<string, string> GetUpdateData(bool detail)
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();

            updata.Add(nameof(TotalBet), TotalBet.ToString());
            updata.Add(nameof(TotalWin), TotalWin.ToString());
            updata.Add(nameof(TotalSurplus), TotalSurplus.ToString());
            updata.Add(nameof(GameTimes), GameTimes.ToString());
            updata.Add(nameof(WinTimes), WinTimes.ToString());

            updata.Add(nameof(Super_Times), Super_Times.ToString());
            updata.Add(nameof(Super_Bet), Super_Bet.ToString());
            updata.Add(nameof(Super_Win), Super_Win.ToString());
            updata.Add(nameof(Mega_Times), Mega_Times.ToString());
            updata.Add(nameof(Mega_Bet), Mega_Bet.ToString());
            updata.Add(nameof(Mega_Win), Mega_Win.ToString());
            updata.Add(nameof(Major_Times), Major_Times.ToString());
            updata.Add(nameof(Major_Bet), Major_Bet.ToString());
            updata.Add(nameof(Major_Win), Major_Win.ToString());
            updata.Add(nameof(Minor_Times), Minor_Times.ToString());
            updata.Add(nameof(Minor_Bet), Minor_Bet.ToString());
            updata.Add(nameof(Minor_Win), Minor_Win.ToString());

            //updata.Add("Fortune_Times", Fortune_Times.ToString());
            GetUpdateDataGame(updata);

            if (detail)
            {
                updata.Add(nameof(RecDate), "'" + RecDate.ToString("yyyy-MM-dd") + "'");
            }
            else
            {
                updata.Add(nameof(RecDate), "'2021-01-01'"); //非detail此欄用不到, 增加此欄位是為了共用DB"更新表單"函式.
            }
            return updata;
        }

        /// <summary>從DB資料更新內存(遊戲)(從DB讀回)</summary>
        public abstract void GetDBDataGame(Dictionary<string, string> datalist);
        /// <summary>從DB資料更新內存 (從DB讀回)</summary>
        /// <param name="detail">false=總帳, true=日帳</param>
        public void GetDBData(Dictionary<string, string> datalist, bool detail)
        {
            AccountUID = Convert.ToInt32(datalist[nameof(AccountUID)]);
            MachineUID = Convert.ToInt32(datalist[nameof(MachineUID)]);
            TotalBet = Convert.ToDecimal(datalist[nameof(TotalBet)]);
            TotalWin = Convert.ToDecimal(datalist[nameof(TotalWin)]);
            TotalSurplus = Convert.ToDecimal(datalist[nameof(TotalSurplus)]);
            GameTimes = Convert.ToInt32(datalist[nameof(GameTimes)]);
            WinTimes = Convert.ToInt32(datalist[nameof(WinTimes)]);

            Super_Times = Convert.ToInt32(datalist[nameof(Super_Times)]);
            Super_Bet = Convert.ToDecimal(datalist[nameof(Super_Bet)]);
            Super_Win = Convert.ToDecimal(datalist[nameof(Super_Win)]);
            Mega_Times = Convert.ToInt32(datalist[nameof(Mega_Times)]);
            Mega_Bet = Convert.ToDecimal(datalist[nameof(Mega_Bet)]);
            Mega_Win = Convert.ToDecimal(datalist[nameof(Mega_Win)]);
            Major_Times = Convert.ToInt32(datalist[nameof(Major_Times)]);
            Major_Bet = Convert.ToDecimal(datalist[nameof(Major_Bet)]);
            Major_Win = Convert.ToDecimal(datalist[nameof(Major_Win)]);
            Minor_Times = Convert.ToInt32(datalist[nameof(Minor_Times)]);
            Minor_Bet = Convert.ToDecimal(datalist[nameof(Minor_Bet)]);
            Minor_Win = Convert.ToDecimal(datalist[nameof(Minor_Win)]);

            AccountType = Convert.ToInt32(datalist[nameof(AccountType)]);

            if (detail) { RecDate = Convert.ToDateTime(datalist[nameof(RecDate)]).Date; } //日帳有日期            

            //Fortune_Times = Convert.ToInt32(datalist["Fortune_Times"]);
            GetDBDataGame(datalist);

            update = false;
        }
    }
}
