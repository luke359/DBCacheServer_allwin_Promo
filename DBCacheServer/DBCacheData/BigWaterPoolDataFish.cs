using Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class BigWaterPoolDataFish
    {
        /// <summary>唯一碼</summary>
        public int SerId;
        /// <summary>水庫累積1</summary>
        public double Pool1 { get; private set; }
        /// <summary>水庫累積2</summary>
        public double Pool2 { get; private set; }
        /// <summary>水庫累積3</summary>
        public double Pool3 { get; private set; }
        /// <summary>水庫累積4</summary>
        public double Pool4 { get; private set; }
        /// <summary>水庫1 計數器1</summary>
        public int Counter11;
        /// <summary>水庫1 計數器2</summary>
        public int Counter12;
        /// <summary>水庫1 計數器3</summary>
        public int Counter13;
        /// <summary>水庫2 計數器1</summary>
        public int Counter21;
        /// <summary>水庫2 計數器2</summary>
        public int Counter22;
        /// <summary>水庫2 計數器3</summary>
        public int Counter23;
        /// <summary>水庫3 計數器1</summary>
        public int Counter31;
        /// <summary>水庫3 計數器2</summary>
        public int Counter32;
        /// <summary>水庫3 計數器3</summary>
        public int Counter33;
        /// <summary>水庫4 計數器1</summary>
        public int Counter41;
        /// <summary>水庫4 計數器2</summary>
        public int Counter42;
        public double Pool11 { get; private set; }
        public double Pool12 { get; private set; }
        public double Pool13 { get; private set; }
        public double Pool14 { get; private set; }
        public int Counter111;
        public int Counter112;
        public int Counter113;
        public int Counter121;
        public int Counter122;
        public int Counter123;
        public int Counter131;
        public int Counter132;
        public int Counter133;
        public int Counter141;
        public int Counter142;
        public double Pool21 { get; private set; }
        public double Pool22 { get; private set; }
        public double Pool23 { get; private set; }
        public double Pool24 { get; private set; }
        public int Counter211;
        public int Counter212;
        public int Counter213;
        public int Counter221;
        public int Counter222;
        public int Counter223;
        public int Counter231;
        public int Counter232;
        public int Counter233;
        public int Counter241;
        public int Counter242;
        public double Pool31 { get; private set; }
        public double Pool32 { get; private set; }
        public double Pool33 { get; private set; }
        public double Pool34 { get; private set; }
        public int Counter311;
        public int Counter312;
        public int Counter313;
        public int Counter321;
        public int Counter322;
        public int Counter323;
        public int Counter331;
        public int Counter332;
        public int Counter333;
        public int Counter341;
        public int Counter342;
        public double Pool41 { get; private set; }
        public double Pool42 { get; private set; }
        public double Pool43 { get; private set; }
        public double Pool44 { get; private set; }
        public int Counter411;
        public int Counter412;
        public int Counter413;
        public int Counter421;
        public int Counter422;
        public int Counter423;
        public int Counter431;
        public int Counter432;
        public int Counter433;
        public int Counter441;
        public int Counter442;

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            SerId = Convert.ToInt32(datalist["SerId"]);
            Pool1 = Convert.ToDouble(datalist["Pool1"]);
            Pool2 = Convert.ToDouble(datalist["Pool2"]);
            Pool3 = Convert.ToDouble(datalist["Pool3"]);
            Pool4 = Convert.ToDouble(datalist["Pool4"]);
            Counter11 = Convert.ToInt32(datalist["Counter11"]);
            Counter12 = Convert.ToInt32(datalist["Counter12"]);
            Counter13 = Convert.ToInt32(datalist["Counter13"]);
            Counter21 = Convert.ToInt32(datalist["Counter21"]);
            Counter22 = Convert.ToInt32(datalist["Counter22"]);
            Counter23 = Convert.ToInt32(datalist["Counter23"]);
            Counter31 = Convert.ToInt32(datalist["Counter31"]);
            Counter32 = Convert.ToInt32(datalist["Counter32"]);
            Counter33 = Convert.ToInt32(datalist["Counter33"]);
            Counter41 = Convert.ToInt32(datalist["Counter41"]);
            Counter42 = Convert.ToInt32(datalist["Counter42"]);
            Pool11 = Convert.ToDouble(datalist["Pool11"]);
            Pool12 = Convert.ToDouble(datalist["Pool12"]);
            Pool13 = Convert.ToDouble(datalist["Pool13"]);
            Pool14 = Convert.ToDouble(datalist["Pool14"]);
            Counter111 = Convert.ToInt32(datalist["Counter111"]);
            Counter112 = Convert.ToInt32(datalist["Counter112"]);
            Counter113 = Convert.ToInt32(datalist["Counter113"]);
            Counter121 = Convert.ToInt32(datalist["Counter121"]);
            Counter122 = Convert.ToInt32(datalist["Counter122"]);
            Counter123 = Convert.ToInt32(datalist["Counter123"]);
            Counter131 = Convert.ToInt32(datalist["Counter131"]);
            Counter132 = Convert.ToInt32(datalist["Counter132"]);
            Counter133 = Convert.ToInt32(datalist["Counter133"]);
            Counter141 = Convert.ToInt32(datalist["Counter141"]);
            Counter142 = Convert.ToInt32(datalist["Counter142"]);
            Pool21 = Convert.ToDouble(datalist["Pool21"]);
            Pool22 = Convert.ToDouble(datalist["Pool22"]);
            Pool23 = Convert.ToDouble(datalist["Pool23"]);
            Pool24 = Convert.ToDouble(datalist["Pool24"]);
            Counter211 = Convert.ToInt32(datalist["Counter211"]);
            Counter212 = Convert.ToInt32(datalist["Counter212"]);
            Counter213 = Convert.ToInt32(datalist["Counter213"]);
            Counter221 = Convert.ToInt32(datalist["Counter221"]);
            Counter222 = Convert.ToInt32(datalist["Counter222"]);
            Counter223 = Convert.ToInt32(datalist["Counter223"]);
            Counter231 = Convert.ToInt32(datalist["Counter231"]);
            Counter232 = Convert.ToInt32(datalist["Counter232"]);
            Counter233 = Convert.ToInt32(datalist["Counter233"]);
            Counter241 = Convert.ToInt32(datalist["Counter241"]);
            Counter242 = Convert.ToInt32(datalist["Counter242"]);
            Pool31 = Convert.ToDouble(datalist["Pool31"]);
            Pool32 = Convert.ToDouble(datalist["Pool32"]);
            Pool33 = Convert.ToDouble(datalist["Pool33"]);
            Pool34 = Convert.ToDouble(datalist["Pool34"]);
            Counter311 = Convert.ToInt32(datalist["Counter311"]);
            Counter312 = Convert.ToInt32(datalist["Counter312"]);
            Counter313 = Convert.ToInt32(datalist["Counter313"]);
            Counter321 = Convert.ToInt32(datalist["Counter321"]);
            Counter322 = Convert.ToInt32(datalist["Counter322"]);
            Counter323 = Convert.ToInt32(datalist["Counter323"]);
            Counter331 = Convert.ToInt32(datalist["Counter331"]);
            Counter332 = Convert.ToInt32(datalist["Counter332"]);
            Counter333 = Convert.ToInt32(datalist["Counter333"]);
            Counter341 = Convert.ToInt32(datalist["Counter341"]);
            Counter342 = Convert.ToInt32(datalist["Counter342"]);
            Pool41 = Convert.ToDouble(datalist["Pool41"]);
            Pool42 = Convert.ToDouble(datalist["Pool42"]);
            Pool43 = Convert.ToDouble(datalist["Pool43"]);
            Pool44 = Convert.ToDouble(datalist["Pool44"]);
            Counter411 = Convert.ToInt32(datalist["Counter411"]);
            Counter412 = Convert.ToInt32(datalist["Counter412"]);
            Counter413 = Convert.ToInt32(datalist["Counter413"]);
            Counter421 = Convert.ToInt32(datalist["Counter421"]);
            Counter422 = Convert.ToInt32(datalist["Counter422"]);
            Counter423 = Convert.ToInt32(datalist["Counter423"]);
            Counter431 = Convert.ToInt32(datalist["Counter431"]);
            Counter432 = Convert.ToInt32(datalist["Counter432"]);
            Counter433 = Convert.ToInt32(datalist["Counter433"]);
            Counter441 = Convert.ToInt32(datalist["Counter441"]);
            Counter442 = Convert.ToInt32(datalist["Counter442"]);
        }

        /// <summary>依據內存製作更新字典</summary>
        public Dictionary<string, string> GetUpdateData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();
            updata.Add("SerId", SerId.ToString());
            updata.Add("Pool1", Pool1.ToString());
            updata.Add("Pool2", Pool2.ToString());
            updata.Add("Pool3", Pool3.ToString());
            updata.Add("Pool4", Pool4.ToString());
            updata.Add("Counter11", Counter11.ToString());
            updata.Add("Counter12", Counter12.ToString());
            updata.Add("Counter13", Counter13.ToString());
            updata.Add("Counter21", Counter21.ToString());
            updata.Add("Counter22", Counter22.ToString());
            updata.Add("Counter23", Counter23.ToString());
            updata.Add("Counter31", Counter31.ToString());
            updata.Add("Counter32", Counter32.ToString());
            updata.Add("Counter33", Counter33.ToString());
            updata.Add("Counter41", Counter41.ToString());
            updata.Add("Counter42", Counter42.ToString());
            updata.Add("Pool11", Pool11.ToString());
            updata.Add("Pool12", Pool12.ToString());
            updata.Add("Pool13", Pool13.ToString());
            updata.Add("Pool14", Pool14.ToString());
            updata.Add("Counter111", Counter111.ToString());
            updata.Add("Counter112", Counter112.ToString());
            updata.Add("Counter113", Counter113.ToString());
            updata.Add("Counter121", Counter121.ToString());
            updata.Add("Counter122", Counter122.ToString());
            updata.Add("Counter123", Counter123.ToString());
            updata.Add("Counter131", Counter131.ToString());
            updata.Add("Counter132", Counter132.ToString());
            updata.Add("Counter133", Counter133.ToString());
            updata.Add("Counter141", Counter141.ToString());
            updata.Add("Counter142", Counter142.ToString());
            updata.Add("Pool21", Pool21.ToString());
            updata.Add("Pool22", Pool22.ToString());
            updata.Add("Pool23", Pool23.ToString());
            updata.Add("Pool24", Pool24.ToString());
            updata.Add("Counter211", Counter211.ToString());
            updata.Add("Counter212", Counter212.ToString());
            updata.Add("Counter213", Counter213.ToString());
            updata.Add("Counter221", Counter221.ToString());
            updata.Add("Counter222", Counter222.ToString());
            updata.Add("Counter223", Counter223.ToString());
            updata.Add("Counter231", Counter231.ToString());
            updata.Add("Counter232", Counter232.ToString());
            updata.Add("Counter233", Counter233.ToString());
            updata.Add("Counter241", Counter241.ToString());
            updata.Add("Counter242", Counter242.ToString());
            updata.Add("Pool31", Pool31.ToString());
            updata.Add("Pool32", Pool32.ToString());
            updata.Add("Pool33", Pool33.ToString());
            updata.Add("Pool34", Pool34.ToString());
            updata.Add("Counter311", Counter311.ToString());
            updata.Add("Counter312", Counter312.ToString());
            updata.Add("Counter313", Counter313.ToString());
            updata.Add("Counter321", Counter321.ToString());
            updata.Add("Counter322", Counter322.ToString());
            updata.Add("Counter323", Counter323.ToString());
            updata.Add("Counter331", Counter331.ToString());
            updata.Add("Counter332", Counter332.ToString());
            updata.Add("Counter333", Counter333.ToString());
            updata.Add("Counter341", Counter341.ToString());
            updata.Add("Counter342", Counter342.ToString());
            updata.Add("Pool41", Pool41.ToString());
            updata.Add("Pool42", Pool42.ToString());
            updata.Add("Pool43", Pool43.ToString());
            updata.Add("Pool44", Pool44.ToString());
            updata.Add("Counter411", Counter411.ToString());
            updata.Add("Counter412", Counter412.ToString());
            updata.Add("Counter413", Counter413.ToString());
            updata.Add("Counter421", Counter421.ToString());
            updata.Add("Counter422", Counter422.ToString());
            updata.Add("Counter423", Counter423.ToString());
            updata.Add("Counter431", Counter431.ToString());
            updata.Add("Counter432", Counter432.ToString());
            updata.Add("Counter433", Counter433.ToString());
            updata.Add("Counter441", Counter441.ToString());
            updata.Add("Counter442", Counter442.ToString());
            return updata;
        }

        /// <summary>依據字典更新內存</summary>
        public void SetUpdateData(Dictionary<string, string> datalist, GameServerCode gameServerCode)
        {
            try
            {
                if (datalist.ContainsKey("Pool1"))
                {
                    //大水庫都是以Kios為單位, 一起更新
                    Pool1 = Convert.ToDouble(datalist["Pool1"]);
                    Pool2 = Convert.ToDouble(datalist["Pool2"]);
                    Pool3 = Convert.ToDouble(datalist["Pool3"]);
                    Pool4 = Convert.ToDouble(datalist["Pool4"]);
                    Counter11 = Convert.ToInt32(datalist["Counter11"]);
                    Counter12 = Convert.ToInt32(datalist["Counter12"]);
                    Counter13 = Convert.ToInt32(datalist["Counter13"]);
                    Counter21 = Convert.ToInt32(datalist["Counter21"]);
                    Counter22 = Convert.ToInt32(datalist["Counter22"]);
                    Counter23 = Convert.ToInt32(datalist["Counter23"]);
                    Counter31 = Convert.ToInt32(datalist["Counter31"]);
                    Counter32 = Convert.ToInt32(datalist["Counter32"]);
                    Counter33 = Convert.ToInt32(datalist["Counter33"]);
                    Counter41 = Convert.ToInt32(datalist["Counter41"]);
                    Counter42 = Convert.ToInt32(datalist["Counter42"]);
                }
                if (datalist.ContainsKey("Pool11"))
                {
                    Pool11 = Convert.ToDouble(datalist["Pool11"]);
                    Pool12 = Convert.ToDouble(datalist["Pool12"]);
                    Pool13 = Convert.ToDouble(datalist["Pool13"]);
                    Pool14 = Convert.ToDouble(datalist["Pool14"]);
                    Counter111 = Convert.ToInt32(datalist["Counter111"]);
                    Counter112 = Convert.ToInt32(datalist["Counter112"]);
                    Counter113 = Convert.ToInt32(datalist["Counter113"]);
                    Counter121 = Convert.ToInt32(datalist["Counter121"]);
                    Counter122 = Convert.ToInt32(datalist["Counter122"]);
                    Counter123 = Convert.ToInt32(datalist["Counter123"]);
                    Counter131 = Convert.ToInt32(datalist["Counter131"]);
                    Counter132 = Convert.ToInt32(datalist["Counter132"]);
                    Counter133 = Convert.ToInt32(datalist["Counter133"]);
                    Counter141 = Convert.ToInt32(datalist["Counter141"]);
                    Counter142 = Convert.ToInt32(datalist["Counter142"]);
                }
                if (datalist.ContainsKey("Pool21"))
                {
                    Pool21 = Convert.ToDouble(datalist["Pool21"]);
                    Pool22 = Convert.ToDouble(datalist["Pool22"]);
                    Pool23 = Convert.ToDouble(datalist["Pool23"]);
                    Pool24 = Convert.ToDouble(datalist["Pool24"]);
                    Counter211 = Convert.ToInt32(datalist["Counter211"]);
                    Counter212 = Convert.ToInt32(datalist["Counter212"]);
                    Counter213 = Convert.ToInt32(datalist["Counter213"]);
                    Counter221 = Convert.ToInt32(datalist["Counter221"]);
                    Counter222 = Convert.ToInt32(datalist["Counter222"]);
                    Counter223 = Convert.ToInt32(datalist["Counter223"]);
                    Counter231 = Convert.ToInt32(datalist["Counter231"]);
                    Counter232 = Convert.ToInt32(datalist["Counter232"]);
                    Counter233 = Convert.ToInt32(datalist["Counter233"]);
                    Counter241 = Convert.ToInt32(datalist["Counter241"]);
                    Counter242 = Convert.ToInt32(datalist["Counter242"]);
                }
                if (datalist.ContainsKey("Pool31"))
                {
                    Pool31 = Convert.ToDouble(datalist["Pool31"]);
                    Pool32 = Convert.ToDouble(datalist["Pool32"]);
                    Pool33 = Convert.ToDouble(datalist["Pool33"]);
                    Pool34 = Convert.ToDouble(datalist["Pool34"]);
                    Counter311 = Convert.ToInt32(datalist["Counter311"]);
                    Counter312 = Convert.ToInt32(datalist["Counter312"]);
                    Counter313 = Convert.ToInt32(datalist["Counter313"]);
                    Counter321 = Convert.ToInt32(datalist["Counter321"]);
                    Counter322 = Convert.ToInt32(datalist["Counter322"]);
                    Counter323 = Convert.ToInt32(datalist["Counter323"]);
                    Counter331 = Convert.ToInt32(datalist["Counter331"]);
                    Counter332 = Convert.ToInt32(datalist["Counter332"]);
                    Counter333 = Convert.ToInt32(datalist["Counter333"]);
                    Counter341 = Convert.ToInt32(datalist["Counter341"]);
                    Counter342 = Convert.ToInt32(datalist["Counter342"]);
                }
                if (datalist.ContainsKey("Pool41"))
                {
                    Pool41 = Convert.ToDouble(datalist["Pool41"]);
                    Pool42 = Convert.ToDouble(datalist["Pool42"]);
                    Pool43 = Convert.ToDouble(datalist["Pool43"]);
                    Pool44 = Convert.ToDouble(datalist["Pool44"]);
                    Counter411 = Convert.ToInt32(datalist["Counter411"]);
                    Counter412 = Convert.ToInt32(datalist["Counter412"]);
                    Counter413 = Convert.ToInt32(datalist["Counter413"]);
                    Counter421 = Convert.ToInt32(datalist["Counter421"]);
                    Counter422 = Convert.ToInt32(datalist["Counter422"]);
                    Counter423 = Convert.ToInt32(datalist["Counter423"]);
                    Counter431 = Convert.ToInt32(datalist["Counter431"]);
                    Counter432 = Convert.ToInt32(datalist["Counter432"]);
                    Counter433 = Convert.ToInt32(datalist["Counter433"]);
                    Counter441 = Convert.ToInt32(datalist["Counter441"]);
                    Counter442 = Convert.ToInt32(datalist["Counter442"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolDataFish: {ex.Message}");
            }
        }

        /// <summary>常規共用水庫放水</summary>
        public (string poolName, string poolVal) SetPumpInData(double value, int poolType, int KiosMode = 0)
        {
            string poolName;
            string poolVal;
            if (KiosMode == 4)
            {
                if (poolType == 0)
                {
                    Pool41 += value;
                    poolName = "Pool41";
                    poolVal = Pool41.ToString();
                }
                else if (poolType == 1)
                {
                    Pool42 += value;
                    poolName = "Pool42";
                    poolVal = Pool42.ToString();
                }
                else if (poolType == 2)
                {
                    Pool43 += value;
                    poolName = "Pool43";
                    poolVal = Pool43.ToString();
                }
                else
                {
                    Pool44 += value;
                    poolName = "Pool44";
                    poolVal = Pool44.ToString();
                }
            }
            else if (KiosMode == 3)
            {
                if (poolType == 0)
                {
                    Pool31 += value;
                    poolName = "Pool31";
                    poolVal = Pool31.ToString();
                }
                else if (poolType == 1)
                {
                    Pool32 += value;
                    poolName = "Pool32";
                    poolVal = Pool32.ToString();
                }
                else if (poolType == 2)
                {
                    Pool33 += value;
                    poolName = "Pool33";
                    poolVal = Pool33.ToString();
                }
                else
                {
                    Pool34 += value;
                    poolName = "Pool34";
                    poolVal = Pool34.ToString();
                }
            }
            else if (KiosMode == 2)
            {
                if (poolType == 0)
                {
                    Pool21 += value;
                    poolName = "Pool21";
                    poolVal = Pool21.ToString();
                }
                else if (poolType == 1)
                {
                    Pool22 += value;
                    poolName = "Pool22";
                    poolVal = Pool22.ToString();
                }
                else if (poolType == 2)
                {
                    Pool23 += value;
                    poolName = "Pool23";
                    poolVal = Pool23.ToString();
                }
                else
                {
                    Pool24 += value;
                    poolName = "Pool24";
                    poolVal = Pool24.ToString();
                }
            }
            else if (KiosMode == 1)
            {
                if (poolType == 0)
                {
                    Pool11 += value;
                    poolName = "Pool11";
                    poolVal = Pool11.ToString();
                }
                else if (poolType == 1)
                {
                    Pool12 += value;
                    poolName = "Pool12";
                    poolVal = Pool12.ToString();
                }
                else if (poolType == 2)
                {
                    Pool13 += value;
                    poolName = "Pool13";
                    poolVal = Pool13.ToString();
                }
                else
                {
                    Pool14 += value;
                    poolName = "Pool14";
                    poolVal = Pool14.ToString();
                }
            }
            else
            {
                if (poolType == 0)
                {
                    Pool1 += value;
                    poolName = "Pool1";
                    poolVal = Pool1.ToString();
                }
                else if (poolType == 1)
                {
                    Pool2 += value;
                    poolName = "Pool2";
                    poolVal = Pool2.ToString();
                }
                else if (poolType == 2)
                {
                    Pool3 += value;
                    poolName = "Pool3";
                    poolVal = Pool3.ToString();
                }
                else
                {
                    Pool4 += value;
                    poolName = "Pool4";
                    poolVal = Pool4.ToString();
                }
            }
            return (poolName, poolVal);
        }
        /// <summary>常規共用水庫抽水</summary>
        public (string poolName, string poolVal) SetDrainOutData(double value, int poolType, int KiosMode = 0)
        {
            double inValue = -value; // 抽水時實際是減少水庫值
            return SetPumpInData(inValue, poolType, KiosMode);
        }
    }
}
