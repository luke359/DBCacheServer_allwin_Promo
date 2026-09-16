using Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class GameServerCalcPool
    {
        //GameServer共用機率資訊 (取代原本機台機率帳) #251112
        /// <summary>唯一碼 (int)GameServerCode</summary>
        public int GameServer;
        /// <summary>資料1</summary>
        public string PoolData1;
        /// <summary>資料2</summary>
        public string PoolData2;
        /// <summary>資料3</summary>
        public string PoolData3;

        /// <summary>遊戲局數序號</summary>
        public int GameSerialNumber;
        /// <summary>遊戲序號日期</summary>
        public string SerialDate;

        /// <summary>更新旗號(資料已變更)</summary>
        public bool update;

        /// <summary></summary>
        public GameServerCalcPool()
        {
            Reset();
        }
        public GameServerCalcPool(GameServerCode server)
        {
            GameServer = (int)server;
            SerialDate = "2021-01-01";
            Reset();
        }

        /// <summary></summary>
        public void Reset()
        {
            PoolData1 = "";
            PoolData2 = "";
            PoolData3 = "";

            update = false;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(GameServerCode server, Dictionary<string, string> datalist)
        {
            //GameServer = (GameServerCode)Convert.ToInt32(datalist[nameof(GameServer)]);
            GameServer = (int)server;

            update = false;

            if (datalist == null || datalist.Count == 0) return;

            try
            {
                PoolData1 = datalist[nameof(PoolData1)];
                PoolData2 = datalist[nameof(PoolData2)];
                PoolData3 = datalist[nameof(PoolData3)];

                GameSerialNumber = Convert.ToInt32(datalist[nameof(GameSerialNumber)]);
                if (datalist[nameof(SerialDate)] != null && datalist[nameof(SerialDate)] != "")
                {
                    //SerDate = Convert.ToDateTime(datalist["SerDate"]).Date;
                    SerialDate = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(datalist[nameof(SerialDate)]).Date);
                }
                else
                {
                    SerialDate = "2021-01-01";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [{(GameServerCode)GameServer}]GameServerCalcPool.GetDBData Error: {ex.Message}");
                SerialDate = "2021-01-01";
            }
        }

        /// <summary>從GameServer資料更新內存</summary>
        public void UpdateCache(Dictionary<string, string> updatadata)
        {
            if (updatadata == null || updatadata.Count == 0) return;

            try
            {
                if (updatadata.ContainsKey(nameof(PoolData1)))
                {
                    PoolData1 = updatadata[nameof(PoolData1)];
                    PoolData2 = updatadata[nameof(PoolData2)];
                    PoolData3 = updatadata[nameof(PoolData3)];

                    GameSerialNumber = Convert.ToInt32(updatadata[nameof(GameSerialNumber)]); //.Trim('\''));
                                                                                              //string serDate = updatadata["SerDate"].Trim('\'').Replace('-', '/');
                    SerialDate = updatadata[nameof(SerialDate)]; //serDate = Convert.ToDateTime(serDate);
                }
                update = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [{(GameServerCode)GameServer}]GameServerCalcPool.UpdateCache Error: {ex.Message}");
            }
        }

        /// <summary>依據內存製作更新DB字典</summary>
        public Dictionary<string, string> GetUpdateData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();

            updata.Add(nameof(PoolData1), "'" + PoolData1 + "'");
            updata.Add(nameof(PoolData2), "'" + PoolData2 + "'");
            updata.Add(nameof(PoolData3), "'" + PoolData3 + "'");

            updata.Add(nameof(GameSerialNumber), GameSerialNumber.ToString());
            updata.Add(nameof(SerialDate), "'" + SerialDate + "'");

            return updata;
        }

        /// <summary>製作給GameServer的IP模式機率資料</summary>
        public Dictionary<string, string> GetGameServerCalcPoolData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>()
            {
                { nameof(PoolData1), PoolData1 },
                { nameof(PoolData2), PoolData2 },
                { nameof(PoolData3), PoolData3 },
                { nameof(GameSerialNumber), GameSerialNumber.ToString() },
                { nameof(SerialDate), SerialDate.ToString() }
            };
            return updata;
        }
    }
}
