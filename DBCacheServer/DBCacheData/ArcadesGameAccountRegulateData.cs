using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class ArcadesGameAccountRegulateData
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
        /// <summary>內帳或外帳</summary>
        public int AccountType { get; set; }
        /// <summary>紀錄時間</summary>
        public DateTime RecDate { get; set; }

        /// <summary>紀錄(盈餘校正)押得分和計算盈餘</summary>
        public void AccountPlay(double betAdd, double winAdd)
        {
            TotalBet += betAdd;
            TotalWin += winAdd;
            TotalSurplus = TotalBet - TotalWin;
        }

        /// <summary>從DB資料更新盈餘校正內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            AccountUID = Convert.ToInt32(datalist["AccountUID"]);
            MachineUID = Convert.ToInt32(datalist["MachineUID"]);
            TotalBet = Convert.ToDouble(datalist["TotalBet"]);
            TotalWin = Convert.ToDouble(datalist["TotalWin"]);
            TotalSurplus = Convert.ToDouble(datalist["TotalSurplus"]);
            AccountType = Convert.ToInt32(datalist["AccountType"]);
            RecDate = Convert.ToDateTime(datalist["RecDate"]).Date;
            //盈餘校正扣掉JP獎得分
            double JPTotalWin = Convert.ToDouble(datalist["Super_Win"]) + Convert.ToDouble(datalist["Mega_Win"]) + Convert.ToDouble(datalist["Major_Win"]) + Convert.ToDouble(datalist["Minor_Win"]);
            TotalWin -= JPTotalWin;
            TotalSurplus += JPTotalWin;
        }
    }
}
