using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class ProbabilityData
    {
        /// <summary>唯一碼</summary>
        public int ProbabilityUID;
        /// <summary>機台唯一碼</summary>
        public int MachineUID;
        /// <summary>遊戲總局數</summary>
        public int RankCount;

        /// <summary>總押</summary>
        public double TotalBet;
        /// <summary>總贏</summary>
        public double TotalWin;
        /// <summary>抽水</summary>
        public double PumpIn;
        /// <summary>放水</summary>
        public double DrainOut;

        /// <summary>資料1</summary>
        public string data1;
        /// <summary>資料2</summary>
        public string data2;
        /// <summary>資料3</summary>
        public string data3;
        /// <summary>資料4</summary>
        public string data4;
        /// <summary>遊戲局數序號</summary>
        public int Serial;
        /// <summary>遊戲序號日期</summary>
        public string SerDate;
        /// <summary>更新旗號(資料已變更)</summary>
        public bool update;

        /// <summary></summary>
        public ProbabilityData()
        {
            Reset();
            Serial = 0;
            RankCount = 0;
        }

        /// <summary></summary>
        public void Reset()
        {
            TotalBet = 0;
            TotalWin = 0;
            PumpIn = 0;
            DrainOut = 0;

            data1 = "0";
            data2 = "0";
            data3 = "0";
            data4 = "0";

            update = true;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(int machUid, Dictionary<string, string> datalist)
        {
            ProbabilityUID = Convert.ToInt32(datalist["ProbabilityUID"]);
            MachineUID = machUid;

            TotalBet = Convert.ToDouble(datalist["TotalBet"]);
            TotalWin = Convert.ToDouble(datalist["TotalWin"]);
            PumpIn = Convert.ToDouble(datalist["PumpIn"]);
            DrainOut = Convert.ToDouble(datalist["DrainOut"]);

            RankCount = Convert.ToInt32(datalist["RankCount"]);
            data1 = datalist["data1"];
            data2 = datalist["data2"];
            data3 = datalist["data3"];
            data4 = datalist["data4"];
            Serial = Convert.ToInt32(datalist["Serial"]);
            //SerDate = Convert.ToDateTime(datalist["SerDate"]).Date;
            SerDate = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(datalist["SerDate"]).Date);
            update = false;
        }

        /// <summary>從GameServer資料更新內存</summary>
        public void UpdateCache(Dictionary<string, string> updatadata)
        {
            if (updatadata.ContainsKey("SerDate"))
            {
                TotalBet = Convert.ToDouble(updatadata["TotalBet"]); //.Trim('\''));
                TotalWin = Convert.ToDouble(updatadata["TotalWin"]); //.Trim('\''));
                PumpIn = Convert.ToDouble(updatadata["PumpIn"]); //.Trim('\''));
                DrainOut = Convert.ToDouble(updatadata["DrainOut"]); //.Trim('\''));

                RankCount = Convert.ToInt32(updatadata["RankCount"]); //.Trim('\''));
                data1 = updatadata["data1"]; //.Trim('\'');
                data2 = updatadata["data2"]; //.Trim('\'');
                data3 = updatadata["data3"]; //.Trim('\'');
                //data4 = updatadata["data4"]; //.Trim('\'');
                Serial = Convert.ToInt32(updatadata["Serial"]); //.Trim('\''));
                //string serDate = updatadata["SerDate"].Trim('\'').Replace('-', '/');
                SerDate = updatadata["SerDate"]; //serDate = Convert.ToDateTime(serDate);
                update = true;
            }
            else if (updatadata.ContainsKey("data4"))
            {
                data4 = updatadata["data4"]; //.Trim('\'');
                update = true;
            }
            else //假玩Bot沒有帳目資訊, 只有遊玩局數, 因此只更新RankCount
            {
                RankCount = Convert.ToInt32(updatadata["RankCount"]);
                update = true;
            }
        }

        /// <summary>依據內存製作更新DB字典</summary>
        public Dictionary<string, string> GetUpdateData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();
            //updata.Add("ProbabilityUID", ProbabilityUID.ToString());
            //updata.Add("MachineUID", MachineUID.ToString());
            updata.Add("TotalBet", TotalBet.ToString());
            updata.Add("TotalWin", TotalWin.ToString());
            updata.Add("PumpIn", PumpIn.ToString());
            updata.Add("DrainOut", DrainOut.ToString());

            updata.Add("RankCount", RankCount.ToString());
            updata.Add("data1", "'" + data1 + "'");
            updata.Add("data2", "'" + data2 + "'");
            updata.Add("data3", "'" + data3 + "'");
            updata.Add("data4", "'" + data4 + "'");
            updata.Add("Serial", Serial.ToString());
            updata.Add("SerDate", "'" + SerDate + "'");
            return updata;
        }

        /// <summary>製作給GameServer的字典</summary>
        public Dictionary<string, string> GetProbabilityData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>()
            {
                { "ProbabilityUID", ProbabilityUID.ToString() },
                { "MachineUID", MachineUID.ToString() },

                { "TotalBet", TotalBet.ToString() },
                { "TotalWin", TotalWin.ToString() },
                { "PumpIn", PumpIn.ToString() },
                { "DrainOut", DrainOut.ToString() },

                { "RankCount", RankCount.ToString() },
                { "data1", data1 },
                { "data2", data2 },
                { "data3", data3 },
                { "data4", data4 },
                { "Serial", Serial.ToString() },
                { "SerDate", SerDate.ToString() }
            };
            return updata;
        }
    }
}
