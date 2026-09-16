using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class XiyouAccountDetailData
    {
        /// <summary>唯一碼</summary>
        public int AccountUID { get; set; }
        /// <summary>機台唯一碼</summary>
        public int MachineUID { get; set; }
        /// <summary>總押</summary>
        public double TotalBet { get; set; }
        /// <summary>總贏</summary>
        public double TotalWin { get; set; }
        /// <summary>總盈餘</summary>
        public double TotalSurplus { get; set; }

        ///// <summary>莊閒總押</summary>
        public double BPTotalBet { get; set; }
        ///// <summary>莊閒總贏</summary>
        public double BPTotalWin { get; set; }
        ///// <summary>莊閒總盈餘</summary>
        public double BPTotalSurplus { get; set; }

        /// <summary>內帳或外帳</summary>
        public int AccountType { get; set; }
        /// <summary>紀錄時間</summary>
        public DateTime RecDate { get; set; }

        /// <summary>紀錄(盈餘校正)押得分和計算盈餘</summary>
        public void AccountPlay(double betAdd, double winAdd, double bbetAdd, double bwinAdd)
        {
            if (betAdd > 0)
            {
                TotalBet += betAdd;
                TotalWin += winAdd;
                TotalSurplus = TotalBet - TotalWin;
            }

            if (bbetAdd > 0)
            {
                BPTotalBet += bbetAdd;
                BPTotalWin += bwinAdd;
                BPTotalSurplus = BPTotalBet - BPTotalWin;
            }
        }

        /// <summary>從DB資料更新盈餘校正內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            AccountUID = Convert.ToInt32(datalist["AccountDatailUID"]);
            MachineUID = Convert.ToInt32(datalist["MachineUID"]);
            TotalBet = Convert.ToDouble(datalist["TotalBet"]);
            TotalWin = Convert.ToDouble(datalist["TotalWin"]);
            TotalSurplus = Convert.ToDouble(datalist["TotalSurplus"]);

            BPTotalBet = Convert.ToDouble(datalist["BPTotalBet"]);  //Xiyou有莊閒押分, 因此不使用共用結構.
            BPTotalWin = Convert.ToDouble(datalist["BPTotalWin"]);
            BPTotalSurplus = Convert.ToDouble(datalist["BPTotalSurplus"]);

            AccountType = Convert.ToInt32(datalist["AccountType"]);
            RecDate = Convert.ToDateTime(datalist["RecDate"]).Date;

            //盈餘校正扣掉JP獎得分
            double JPTotalWin = Convert.ToDouble(datalist["Super_Win"]) + Convert.ToDouble(datalist["Mega_Win"]) + Convert.ToDouble(datalist["Major_Win"]) + Convert.ToDouble(datalist["Minor_Win"]);
            TotalWin -= JPTotalWin;
            TotalSurplus += JPTotalWin;
        }
    }
}
