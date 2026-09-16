using Google.Protobuf.WellKnownTypes;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VnpayAPI;

namespace DBCacheServer
{
    public class BigWaterPoolData
    {
        /// <summary>唯一碼</summary>
        public int SerId { get; set; }
        /// <summary>水庫池</summary>
        public double Pool { get; private set; }
        /// <summary>計數器1</summary>
        public int CounterA;
        /// <summary>計數器2</summary>
        public int CounterB;
        /// <summary>計數器3</summary>
        public int CounterC;
        /// <summary>計數器4</summary>
        public int CounterD;
        /// <summary>計數器5</summary>
        public int CounterE;
        public double Pool1 { get; private set; }
        public int CounterA1;
        public int CounterB1;
        public int CounterC1;
        public int CounterD1;
        public int CounterE1;
        public double Pool2 { get; private set; }
        public int CounterA2;
        public int CounterB2;
        public int CounterC2;
        public int CounterD2;
        public int CounterE2;
        public double Pool3 { get; private set; }
        public int CounterA3;
        public int CounterB3;
        public int CounterC3;
        public int CounterD3;
        public int CounterE3;
        public double Pool4 { get; private set; }
        public int CounterA4;
        public int CounterB4;
        public int CounterC4;
        public int CounterD4;
        public int CounterE4;

        /// <summary>獨立買水庫池</summary>
        public double IndepPool  { get; private set; }
        /// <summary>獨立買計數器1</summary>
        public int IndepCounterA { get; set; }
        /// <summary>獨立買計數器2</summary>
        public int IndepCounterB { get; set; }
        /// <summary>獨立買計數器3</summary>
        public int IndepCounterC { get; set; }
        /// <summary>獨立買計數器4</summary>
        public int IndepCounterD { get; set; }
        /// <summary>獨立買計數器5</summary>
        public int IndepCounterE { get; set; }
        public double IndepPool1 { get; private set; }
        public int IndepCounterA1;
        public int IndepCounterB1;
        public int IndepCounterC1;
        public int IndepCounterD1;
        public int IndepCounterE1;
        public double IndepPool2 { get; private set; }
        public int IndepCounterA2;
        public int IndepCounterB2;
        public int IndepCounterC2;
        public int IndepCounterD2;
        public int IndepCounterE2;
        public double IndepPool3 { get; private set; }
        public int IndepCounterA3;
        public int IndepCounterB3;
        public int IndepCounterC3;
        public int IndepCounterD3;
        public int IndepCounterE3;
        public double IndepPool4 { get; private set; }
        public int IndepCounterA4;
        public int IndepCounterB4;
        public int IndepCounterC4;
        public int IndepCounterD4;
        public int IndepCounterE4;

        public string Data1;
        public string Data2;
        public string IndepData1;
        public string IndepData2;

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            SerId = Convert.ToInt32(datalist["SerId"]);

            Pool = Convert.ToDouble(datalist["Pool"]);
            CounterA = Convert.ToInt32(datalist["CounterA"]);
            CounterB = Convert.ToInt32(datalist["CounterB"]);
            CounterC = Convert.ToInt32(datalist["CounterC"]);
            CounterD = Convert.ToInt32(datalist["CounterD"]);
            CounterE = Convert.ToInt32(datalist["CounterE"]);
            Pool1 = Convert.ToDouble(datalist["Pool1"]);
            CounterA1 = Convert.ToInt32(datalist["CounterA1"]);
            CounterB1 = Convert.ToInt32(datalist["CounterB1"]);
            CounterC1 = Convert.ToInt32(datalist["CounterC1"]);
            CounterD1 = Convert.ToInt32(datalist["CounterD1"]);
            CounterE1 = Convert.ToInt32(datalist["CounterE1"]);
            Pool2 = Convert.ToDouble(datalist["Pool2"]);
            CounterA2 = Convert.ToInt32(datalist["CounterA2"]);
            CounterB2 = Convert.ToInt32(datalist["CounterB2"]);
            CounterC2 = Convert.ToInt32(datalist["CounterC2"]);
            CounterD2 = Convert.ToInt32(datalist["CounterD2"]);
            CounterE2 = Convert.ToInt32(datalist["CounterE2"]);
            Pool3 = Convert.ToDouble(datalist["Pool3"]);
            CounterA3 = Convert.ToInt32(datalist["CounterA3"]);
            CounterB3 = Convert.ToInt32(datalist["CounterB3"]);
            CounterC3 = Convert.ToInt32(datalist["CounterC3"]);
            CounterD3 = Convert.ToInt32(datalist["CounterD3"]);
            CounterE3 = Convert.ToInt32(datalist["CounterE3"]);
            Pool4 = Convert.ToDouble(datalist["Pool4"]);
            CounterA4 = Convert.ToInt32(datalist["CounterA4"]);
            CounterB4 = Convert.ToInt32(datalist["CounterB4"]);
            CounterC4 = Convert.ToInt32(datalist["CounterC4"]);
            CounterD4 = Convert.ToInt32(datalist["CounterD4"]);
            CounterE4 = Convert.ToInt32(datalist["CounterE4"]);

            IndepPool = Convert.ToDouble(datalist["IndepPool"]);
            IndepCounterA = Convert.ToInt32(datalist["IndepCounterA"]);
            IndepCounterB = Convert.ToInt32(datalist["IndepCounterB"]);
            IndepCounterC = Convert.ToInt32(datalist["IndepCounterC"]);
            IndepCounterD = Convert.ToInt32(datalist["IndepCounterD"]);
            IndepCounterE = Convert.ToInt32(datalist["IndepCounterE"]);
            IndepPool1 = Convert.ToDouble(datalist["IndepPool1"]);
            IndepCounterA1 = Convert.ToInt32(datalist["IndepCounterA1"]);
            IndepCounterB1 = Convert.ToInt32(datalist["IndepCounterB1"]);
            IndepCounterC1 = Convert.ToInt32(datalist["IndepCounterC1"]);
            IndepCounterD1 = Convert.ToInt32(datalist["IndepCounterD1"]);
            IndepCounterE1 = Convert.ToInt32(datalist["IndepCounterE1"]);
            IndepPool2 = Convert.ToDouble(datalist["IndepPool2"]);
            IndepCounterA2 = Convert.ToInt32(datalist["IndepCounterA2"]);
            IndepCounterB2 = Convert.ToInt32(datalist["IndepCounterB2"]);
            IndepCounterC2 = Convert.ToInt32(datalist["IndepCounterC2"]);
            IndepCounterD2 = Convert.ToInt32(datalist["IndepCounterD2"]);
            IndepCounterE2 = Convert.ToInt32(datalist["IndepCounterE2"]);
            IndepPool3 = Convert.ToDouble(datalist["IndepPool3"]);
            IndepCounterA3 = Convert.ToInt32(datalist["IndepCounterA3"]);
            IndepCounterB3 = Convert.ToInt32(datalist["IndepCounterB3"]);
            IndepCounterC3 = Convert.ToInt32(datalist["IndepCounterC3"]);
            IndepCounterD3 = Convert.ToInt32(datalist["IndepCounterD3"]);
            IndepCounterE3 = Convert.ToInt32(datalist["IndepCounterE3"]);
            IndepPool4 = Convert.ToDouble(datalist["IndepPool4"]);
            IndepCounterA4 = Convert.ToInt32(datalist["IndepCounterA4"]);
            IndepCounterB4 = Convert.ToInt32(datalist["IndepCounterB4"]);
            IndepCounterC4 = Convert.ToInt32(datalist["IndepCounterC4"]);
            IndepCounterD4 = Convert.ToInt32(datalist["IndepCounterD4"]);
            IndepCounterE4 = Convert.ToInt32(datalist["IndepCounterE4"]);

            //新增Server共用欄位
            Data1 = datalist["Data1"];
            Data2 = datalist["Data2"];
            IndepData1 = datalist["IndepData1"];
            IndepData2 = datalist["IndepData2"];
        }

        /// <summary>依據內存製作更新字典</summary>
        public Dictionary<string, string> GetUpdateData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();
            updata.Add("SerId", SerId.ToString());

            updata.Add("Pool", Pool.ToString());
            updata.Add("CounterA", CounterA.ToString());
            updata.Add("CounterB", CounterB.ToString());
            updata.Add("CounterC", CounterC.ToString());
            updata.Add("CounterD", CounterD.ToString());
            updata.Add("CounterE", CounterE.ToString());
            updata.Add("Pool1", Pool1.ToString());
            updata.Add("CounterA1", CounterA1.ToString());
            updata.Add("CounterB1", CounterB1.ToString());
            updata.Add("CounterC1", CounterC1.ToString());
            updata.Add("CounterD1", CounterD1.ToString());
            updata.Add("CounterE1", CounterE1.ToString());
            updata.Add("Pool2", Pool2.ToString());
            updata.Add("CounterA2", CounterA2.ToString());
            updata.Add("CounterB2", CounterB2.ToString());
            updata.Add("CounterC2", CounterC2.ToString());
            updata.Add("CounterD2", CounterD2.ToString());
            updata.Add("CounterE2", CounterE2.ToString());
            updata.Add("Pool3", Pool3.ToString());
            updata.Add("CounterA3", CounterA3.ToString());
            updata.Add("CounterB3", CounterB3.ToString());
            updata.Add("CounterC3", CounterC3.ToString());
            updata.Add("CounterD3", CounterD3.ToString());
            updata.Add("CounterE3", CounterE3.ToString());
            updata.Add("Pool4", Pool4.ToString());
            updata.Add("CounterA4", CounterA4.ToString());
            updata.Add("CounterB4", CounterB4.ToString());
            updata.Add("CounterC4", CounterC4.ToString());
            updata.Add("CounterD4", CounterD4.ToString());
            updata.Add("CounterE4", CounterE4.ToString());

            updata.Add("IndepPool", IndepPool.ToString());
            updata.Add("IndepCounterA", IndepCounterA.ToString());
            updata.Add("IndepCounterB", IndepCounterB.ToString());
            updata.Add("IndepCounterC", IndepCounterC.ToString());
            updata.Add("IndepCounterD", IndepCounterD.ToString());
            updata.Add("IndepCounterE", IndepCounterE.ToString());
            updata.Add("IndepPool1", IndepPool1.ToString());
            updata.Add("IndepCounterA1", IndepCounterA1.ToString());
            updata.Add("IndepCounterB1", IndepCounterB1.ToString());
            updata.Add("IndepCounterC1", IndepCounterC1.ToString());
            updata.Add("IndepCounterD1", IndepCounterD1.ToString());
            updata.Add("IndepCounterE1", IndepCounterE1.ToString());
            updata.Add("IndepPool2", IndepPool2.ToString());
            updata.Add("IndepCounterA2", IndepCounterA2.ToString());
            updata.Add("IndepCounterB2", IndepCounterB2.ToString());
            updata.Add("IndepCounterC2", IndepCounterC2.ToString());
            updata.Add("IndepCounterD2", IndepCounterD2.ToString());
            updata.Add("IndepCounterE2", IndepCounterE2.ToString());
            updata.Add("IndepPool3", IndepPool3.ToString());
            updata.Add("IndepCounterA3", IndepCounterA3.ToString());
            updata.Add("IndepCounterB3", IndepCounterB3.ToString());
            updata.Add("IndepCounterC3", IndepCounterC3.ToString());
            updata.Add("IndepCounterD3", IndepCounterD3.ToString());
            updata.Add("IndepCounterE3", IndepCounterE3.ToString());
            updata.Add("IndepPool4", IndepPool4.ToString());
            updata.Add("IndepCounterA4", IndepCounterA4.ToString());
            updata.Add("IndepCounterB4", IndepCounterB4.ToString());
            updata.Add("IndepCounterC4", IndepCounterC4.ToString());
            updata.Add("IndepCounterD4", IndepCounterD4.ToString());
            updata.Add("IndepCounterE4", IndepCounterE4.ToString());

            updata.Add("Data1", Data1);
            updata.Add("Data2", Data2);
            updata.Add("IndepData1", IndepData1);
            updata.Add("IndepData2", IndepData2);
            return updata;
        }

        /// <summary>更新內存</summary>
        public void SetUpdateData(Dictionary<string, string> datalist, GameServerCode gameServerCode)
        {
            try
            {
                if (datalist.ContainsKey("Pool"))
                {
                    //大水庫都是以Kios為單位, 一起更新
                    Pool = Convert.ToDouble(datalist["Pool"]);
                    CounterA = Convert.ToInt32(datalist["CounterA"]);
                    CounterB = Convert.ToInt32(datalist["CounterB"]);
                    CounterC = Convert.ToInt32(datalist["CounterC"]);
                    CounterD = Convert.ToInt32(datalist["CounterD"]);
                    CounterE = Convert.ToInt32(datalist["CounterE"]);
                }
                if (datalist.ContainsKey("Pool1"))
                {
                    Pool1 = Convert.ToDouble(datalist["Pool1"]);
                    CounterA1 = Convert.ToInt32(datalist["CounterA1"]);
                    CounterB1 = Convert.ToInt32(datalist["CounterB1"]);
                    CounterC1 = Convert.ToInt32(datalist["CounterC1"]);
                    CounterD1 = Convert.ToInt32(datalist["CounterD1"]);
                    CounterE1 = Convert.ToInt32(datalist["CounterE1"]);
                }
                if (datalist.ContainsKey("Pool2"))
                {
                    Pool2 = Convert.ToDouble(datalist["Pool2"]);
                    CounterA2 = Convert.ToInt32(datalist["CounterA2"]);
                    CounterB2 = Convert.ToInt32(datalist["CounterB2"]);
                    CounterC2 = Convert.ToInt32(datalist["CounterC2"]);
                    CounterD2 = Convert.ToInt32(datalist["CounterD2"]);
                    CounterE2 = Convert.ToInt32(datalist["CounterE2"]);
                }
                if (datalist.ContainsKey("Pool3"))
                {
                    Pool3 = Convert.ToDouble(datalist["Pool3"]);
                    CounterA3 = Convert.ToInt32(datalist["CounterA3"]);
                    CounterB3 = Convert.ToInt32(datalist["CounterB3"]);
                    CounterC3 = Convert.ToInt32(datalist["CounterC3"]);
                    CounterD3 = Convert.ToInt32(datalist["CounterD3"]);
                    CounterE3 = Convert.ToInt32(datalist["CounterE3"]);
                }
                if (datalist.ContainsKey("Pool4"))
                {
                    Pool4 = Convert.ToDouble(datalist["Pool4"]);
                    CounterA4 = Convert.ToInt32(datalist["CounterA4"]);
                    CounterB4 = Convert.ToInt32(datalist["CounterB4"]);
                    CounterC4 = Convert.ToInt32(datalist["CounterC4"]);
                    CounterD4 = Convert.ToInt32(datalist["CounterD4"]);
                    CounterE4 = Convert.ToInt32(datalist["CounterE4"]);
                }

                if (datalist.ContainsKey("IndepPool"))
                {
                    IndepPool = Convert.ToDouble(datalist["IndepPool"]);
                    IndepCounterA = Convert.ToInt32(datalist["IndepCounterA"]);
                    IndepCounterB = Convert.ToInt32(datalist["IndepCounterB"]);
                    IndepCounterC = Convert.ToInt32(datalist["IndepCounterC"]);
                    IndepCounterD = Convert.ToInt32(datalist["IndepCounterD"]);
                    IndepCounterE = Convert.ToInt32(datalist["IndepCounterE"]);
                }
                if (datalist.ContainsKey("IndepPool1"))
                {
                    IndepPool1 = Convert.ToDouble(datalist["IndepPool1"]);
                    IndepCounterA1 = Convert.ToInt32(datalist["IndepCounterA1"]);
                    IndepCounterB1 = Convert.ToInt32(datalist["IndepCounterB1"]);
                    IndepCounterC1 = Convert.ToInt32(datalist["IndepCounterC1"]);
                    IndepCounterD1 = Convert.ToInt32(datalist["IndepCounterD1"]);
                    IndepCounterE1 = Convert.ToInt32(datalist["IndepCounterE1"]);
                }
                if (datalist.ContainsKey("IndepPool2"))
                {
                    IndepPool2 = Convert.ToDouble(datalist["IndepPool2"]);
                    IndepCounterA2 = Convert.ToInt32(datalist["IndepCounterA2"]);
                    IndepCounterB2 = Convert.ToInt32(datalist["IndepCounterB2"]);
                    IndepCounterC2 = Convert.ToInt32(datalist["IndepCounterC2"]);
                    IndepCounterD2 = Convert.ToInt32(datalist["IndepCounterD2"]);
                    IndepCounterE2 = Convert.ToInt32(datalist["IndepCounterE2"]);
                }
                if (datalist.ContainsKey("IndepPool3"))
                {
                    IndepPool3 = Convert.ToDouble(datalist["IndepPool3"]);
                    IndepCounterA3 = Convert.ToInt32(datalist["IndepCounterA3"]);
                    IndepCounterB3 = Convert.ToInt32(datalist["IndepCounterB3"]);
                    IndepCounterC3 = Convert.ToInt32(datalist["IndepCounterC3"]);
                    IndepCounterD3 = Convert.ToInt32(datalist["IndepCounterD3"]);
                    IndepCounterE3 = Convert.ToInt32(datalist["IndepCounterE3"]);
                }
                if (datalist.ContainsKey("IndepPool4"))
                {
                    IndepPool4 = Convert.ToDouble(datalist["IndepPool4"]);
                    IndepCounterA4 = Convert.ToInt32(datalist["IndepCounterA4"]);
                    IndepCounterB4 = Convert.ToInt32(datalist["IndepCounterB4"]);
                    IndepCounterC4 = Convert.ToInt32(datalist["IndepCounterC4"]);
                    IndepCounterD4 = Convert.ToInt32(datalist["IndepCounterD4"]);
                    IndepCounterE4 = Convert.ToInt32(datalist["IndepCounterE4"]);
                }

                //以下依據Server不同, 可能有也可能沒有, 所以須個別判斷更新
                if (datalist.ContainsKey("Data1"))
                {
                    string[] ChackTXT = datalist["Data1"].Split("finish");
                    if (ChackTXT.Length >= 2)
                    {
                        int DataNum = 0;
                        string DataNumString = ChackTXT[ChackTXT.Length - 2];
                        try 
                        {
                            DataNum = Convert.ToInt32(DataNumString);
                        } 
                        catch (Exception ex) 
                        {
                            Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolData1 Of Finish Is Wrong: {ex.Message}");
                            DataNum = 0;
                        }
                        string[] ChackData = ChackTXT[ChackTXT.Length - 3].Split(";");
                        if (ChackData.Length != DataNum + 1) 
                        {
                            if (ChackData.Length > DataNum + 1) 
                            {
                                string Recover = "";
                                for (int i = 0; i < DataNum; i++) 
                                {
                                    Recover = Recover + (ChackData[(ChackData.Length - 1) - DataNum + i]) + ";";
                                }
                                datalist["Data1"] = Recover + "finish" + DataNum.ToString() + "finish";
                            }
                            else 
                            {
                                Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolData1 Of Data1 Is Wrong");
                            }
                        }
                    }
                    Data1 = datalist["Data1"];
                }
                if (datalist.ContainsKey("Data2"))
                {
                    string[] ChackTXT = datalist["Data2"].Split("finish");
                    if (ChackTXT.Length >= 2)
                    {
                        int DataNum = 0;
                        string DataNumString = ChackTXT[ChackTXT.Length - 2];
                        try
                        {
                            DataNum = Convert.ToInt32(DataNumString);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolData2 Of Finish Is Wrong: {ex.Message}");
                            DataNum = 0;
                        }
                        string[] ChackData = ChackTXT[ChackTXT.Length - 3].Split(";");
                        if (ChackData.Length != DataNum + 1)
                        {
                            if (ChackData.Length > DataNum + 1)
                            {
                                string Recover = "";
                                for (int i = 0; i < DataNum; i++)
                                {
                                    Recover = Recover + (ChackData[(ChackData.Length - 1) - DataNum + i]) + ";";
                                }
                                datalist["Data2"] = Recover + "finish" + DataNum.ToString() + "finish";
                            }
                            else
                            {
                                Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolData2 Of Data1 Is Wrong");
                            }
                        }
                    }
                    Data2 = datalist["Data2"];
                }
                if (datalist.ContainsKey("IndepData1"))
                {
                    string[] ChackTXT = datalist["IndepData1"].Split("finish");
                    if (ChackTXT.Length >= 2)
                    {
                        int DataNum = 0;
                        string DataNumString = ChackTXT[ChackTXT.Length - 2];
                        try
                        {
                            DataNum = Convert.ToInt32(DataNumString);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolIndepData1 Of Finish Is Wrong: {ex.Message}");
                            DataNum = 0;
                        }
                        string[] ChackData = ChackTXT[ChackTXT.Length - 3].Split(";");
                        if (ChackData.Length != DataNum + 1)
                        {
                            if (ChackData.Length > DataNum + 1)
                            {
                                string Recover = "";
                                for (int i = 0; i < DataNum; i++)
                                {
                                    Recover = Recover + (ChackData[(ChackData.Length - 1) - DataNum + i]) + ";";
                                }
                                datalist["IndepData1"] = Recover + "finish" + DataNum.ToString() + "finish";
                            }
                            else
                            {
                                Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolData Of IndepData1 Is Wrong");
                            }
                        }
                    }
                    IndepData1 = datalist["IndepData1"];
                }
                if (datalist.ContainsKey("IndepData2"))
                {
                    string[] ChackTXT = datalist["IndepData2"].Split("finish");
                    if (ChackTXT.Length >= 2)
                    {
                        int DataNum = 0;
                        string DataNumString = ChackTXT[ChackTXT.Length - 2];
                        try
                        {
                            DataNum = Convert.ToInt32(DataNumString);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolIndepData2 Of Finish Is Wrong: {ex.Message}");
                            DataNum = 0;
                        }
                        string[] ChackData = ChackTXT[ChackTXT.Length - 3].Split(";");
                        if (ChackData.Length != DataNum + 1)
                        {
                            if (ChackData.Length > DataNum + 1)
                            {
                                string Recover = "";
                                for (int i = 0; i < DataNum; i++)
                                {
                                    Recover = Recover + (ChackData[(ChackData.Length - 1) - DataNum + i]) + ";";
                                }
                                datalist["IndepData2"] = Recover + "finish" + DataNum.ToString() + "finish";
                            }
                            else
                            {
                                Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolData Of IndepData2 Is Wrong");
                            }
                        }
                    }
                    IndepData2 = datalist["IndepData2"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating [{gameServerCode}] BigWaterPoolData: {ex.Message}");
            }
        }

        /// <summary>常規共用水庫放水</summary>
        public (string poolName, string poolVal) SetPumpInData(double value, int KiosMode = 0)
        {
            string poolName;
            string poolVal;
            if (KiosMode == 4)
            {
                Pool4 += value;
                poolName = "Pool4";
                poolVal = Pool4.ToString();
            }
            else if (KiosMode == 3)
            {
                Pool3 += value;
                poolName = "Pool3";
                poolVal = Pool3.ToString();
            }
            else if (KiosMode == 2)
            {
                Pool2 += value;
                poolName = "Pool2";
                poolVal = Pool2.ToString();
            }
            else if (KiosMode == 1)
            {
                Pool1 += value;
                poolName = "Pool1";
                poolVal = Pool1.ToString();
            }
            else
            {
                Pool += value;
                poolName = "Pool";
                poolVal = Pool.ToString();
            }
            return (poolName, poolVal);
        }
        /// <summary>常規共用水庫抽水</summary>
        public (string poolName, string poolVal) SetDrainOutData(double value, int KiosMode = 0)
        {
            double inValue = -value; // 負值=抽水
            return SetPumpInData(inValue, KiosMode);
        }
        /// <summary>獨立買共用水庫放水</summary>
        public (string poolName, string poolVal) SetPumpInDataIndep(double value, int KiosMode = 0)
        {
            string poolName;
            string poolVal;
            if (KiosMode == 4)
            {
                IndepPool4 += value;
                poolName = "IndepPool4";
                poolVal = IndepPool4.ToString();
            }
            else if (KiosMode == 3)
            {
                IndepPool3 += value;
                poolName = "IndepPool3";
                poolVal = IndepPool3.ToString();
            }
            else if (KiosMode == 2)
            {
                IndepPool2 += value;
                poolName = "IndepPool2";
                poolVal = IndepPool2.ToString();
            }
            else if (KiosMode == 1)
            {
                IndepPool1 += value;
                poolName = "IndepPool1";
                poolVal = IndepPool1.ToString();
            }
            else
            {
                IndepPool += value;
                poolName = "IndepPool";
                poolVal = IndepPool.ToString();
            }
            return (poolName, poolVal);
        }
        /// <summary>獨立買共用水庫抽水</summary>
        public (string poolName, string poolVal) SetDrainOutDataIndep(double value, int KiosMode = 0)
        {
            double inValue = -value; // 負值=抽水
            return SetPumpInDataIndep(inValue, KiosMode);
        }
    }
}
