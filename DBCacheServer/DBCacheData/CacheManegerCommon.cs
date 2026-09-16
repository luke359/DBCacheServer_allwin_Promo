using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnpayAPI;

namespace DBCacheServer
{
    public partial class CacheManeger
    {
        /// <summary>解Server列表 字串</summary>
        static public List<GameServerCode> DecodeServerList(string serverListStr, string fmsg)
        {
            if (!string.IsNullOrWhiteSpace(serverListStr))
            {
                try
                {
                    var plist = serverListStr.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries); //移除空白與空項目
                    if (plist.Length > 0)
                    {
                        List<GameServerCode> Server = new();

                        for (int i = 0; i < plist.Length; i++)
                        {
                            if (int.TryParse(plist[i], out int gcode))
                            {
                                if (!Enum.IsDefined(typeof(GameServerCode), gcode) || gcode < (int)GameServerCode.XiyouGame)
                                {
                                    if (gcode != 0) MyConsole.WriteLine($"    {fmsg}:無效的ServerCode[{gcode}]");
                                    continue;
                                }

                                GameServerCode srv = (GameServerCode)gcode;

                                if (Server.Contains(srv))
                                {
                                    MyConsole.WriteLine($"    {fmsg}:重複的ServerCode[{srv}]");
                                    continue;
                                }

                                Server.Add(srv);
                                //MyConsole.WriteLine($"    {fmsg}:加入ServerCode[{srv}]");
                            }
                        }
                        return Server;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{fmsg}: DecodeServerList Exception: {ex}");
                }
            }
            return null;
        }

        /// <summary>是否為每週一</summary>
        static public bool IsFirstDayOfWeek(DateTime now)
        {
            if (now.DayOfWeek == DayOfWeek.Monday)
            {
                return true;
            }
            return false;
        }

        /// <summary>是否為每月1日</summary>
        static public bool IsFirstDayOfMonth(DateTime now)
        {
            if (now.Day == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>Server是否為無房間模式</summary>
        static public bool IsNewTypeGameServer(GameServerCode gcode)
        {
            return gcode switch
            {
                GameServerCode.ScroogesWinterTreasure or
                GameServerCode.RoseHeroZ or
                GameServerCode.Wukong or
                GameServerCode.HighWayKingPlus => true,
                _ => false,
            };
        }


        /// <summary>WEB修改玩家設定值參數</summary>
        public class IpParaDataInfo
        {
            public const int ParaDataLength1 = 3; //參數數量 RTP;放水段數;抽水段數
            public const int ParaDataLength2 = 6 * 2; //參數數量 紅包幣值1;選中率1 ~ 紅包幣值6;選中率6

            public const string ParaDataDef1 = "98.0;1;1";
            public const string ParaDataDef2 = "1;49;2;20;3;15;5;10;8;5;10;1";

            /// <summary>ParaData內定值字串</summary>
            public static string ParaDataDefaultString()
            {
                return $"{ParaDataDef1}#{ParaDataDef2}";
            }

            /// <summary>玩家UID</summary>
            public int Uid;

            //第一組參數
            /// <summary>玩家%</summary>
            public double Rtp;
            /// <summary>放水分段</summary>
            public int WaterInLevel;
            /// <summary>抽水分段</summary>
            public int WaterOutLevel;
            /// <summary>第一組參數 解包結果</summary>
            bool DataOk1 = false;

            //第二組參數
            /// <summary>遊戲紅包幣值 1</summary>
            public int RedWaterValue1;
            /// <summary>遊戲紅包選中率 1</summary>
            public int RedWaterRate1;
            /// <summary>遊戲紅包幣值 2</summary>
            public int RedWaterValue2;
            /// <summary>遊戲紅包選中率 2</summary>
            public int RedWaterRate2;
            /// <summary>遊戲紅包幣值 3</summary>
            public int RedWaterValue3;
            /// <summary>遊戲紅包選中率 3</summary>
            public int RedWaterRate3;
            /// <summary>遊戲紅包幣值 4</summary>
            public int RedWaterValue4;
            /// <summary>遊戲紅包選中率 4</summary>
            public int RedWaterRate4;
            /// <summary>遊戲紅包幣值 5</summary>
            public int RedWaterValue5;
            /// <summary>遊戲紅包選中率 5</summary>
            public int RedWaterRate5;
            /// <summary>遊戲紅包幣值 6</summary>
            public int RedWaterValue6;
            /// <summary>遊戲紅包選中率 6</summary>
            public int RedWaterRate6;
            /// <summary>第二組參數 解包結果</summary>
            bool DataOk2 = false;


            public IpParaDataInfo(int uid)
            {
                Uid = uid;
            }
            /// <summary>ParaData字串解包 並檢查正確性</summary>
            public IpParaDataInfo(string paraData)
            {
                DataOk1 = false;
                DataOk2 = false;

                if (string.IsNullOrEmpty(paraData) == false)
                {
                    string[] arrData = paraData.Split('#'); //各組設定以"#"號區隔

                    //第一組參數
                    if (arrData.Length >= 1)
                    {
                        string[] para1 = arrData[0].Split(';');
                        if (para1.Length >= ParaDataLength1)
                        {
                            try
                            {
                                Rtp = Convert.ToDouble(para1[0]);
                                WaterInLevel = Convert.ToInt32(para1[1]);
                                WaterOutLevel = Convert.ToInt32(para1[2]);
                                DataOk1 = true;
                            }
                            catch (Exception ex)
                            {
                                MyConsole.WriteLine($"    IpParaDataInfo ExpandData para1 Error: {ex.Message}");
                            }
                        }
                    }

                    //第二組參數
                    if (arrData.Length >= 2)
                    {
                        string[] para2 = arrData[1].Split(';');
                        if (para2.Length >= ParaDataLength2)
                        {
                            try
                            {
                                RedWaterValue1 = Convert.ToInt32(para2[0]);
                                RedWaterRate1 = Convert.ToInt32(para2[1]);
                                RedWaterValue2 = Convert.ToInt32(para2[2]);
                                RedWaterRate2 = Convert.ToInt32(para2[3]);
                                RedWaterValue3 = Convert.ToInt32(para2[4]);
                                RedWaterRate3 = Convert.ToInt32(para2[5]);
                                RedWaterValue4 = Convert.ToInt32(para2[6]);
                                RedWaterRate4 = Convert.ToInt32(para2[7]);
                                RedWaterValue5 = Convert.ToInt32(para2[8]);
                                RedWaterRate5 = Convert.ToInt32(para2[9]);
                                RedWaterValue6 = Convert.ToInt32(para2[10]);
                                RedWaterRate6 = Convert.ToInt32(para2[11]);
                                DataOk2 = true;
                            }
                            catch (Exception ex)
                            {
                                MyConsole.WriteLine($"    IpParaDataInfo ExpandData para2 Error: {ex.Message}");
                            }
                        }
                    }
                }

                if (DataOk1 == false)
                {
                    SetDefaultGroup1();
                }

                if (DataOk2 == false)
                {
                    SetDefaultGroup2();
                }
            }


            /// <summary>設定第一組參數為預設值</summary>
            public void SetDefaultGroup1()
            {
                string[] para1 = ParaDataDef1.Split(';');
                Rtp = Convert.ToDouble(para1[0]);
                WaterInLevel = Convert.ToInt32(para1[1]);
                WaterOutLevel = Convert.ToInt32(para1[2]);
            }
            /// <summary>設定第二組參數為預設值</summary>
            public void SetDefaultGroup2()
            {
                string[] para2 = ParaDataDef2.Split(';');
                RedWaterValue1 = Convert.ToInt32(para2[0]);
                RedWaterRate1 = Convert.ToInt32(para2[1]);
                RedWaterValue2 = Convert.ToInt32(para2[2]);
                RedWaterRate2 = Convert.ToInt32(para2[3]);
                RedWaterValue3 = Convert.ToInt32(para2[4]);
                RedWaterRate3 = Convert.ToInt32(para2[5]);
                RedWaterValue4 = Convert.ToInt32(para2[6]);
                RedWaterRate4 = Convert.ToInt32(para2[7]);
                RedWaterValue5 = Convert.ToInt32(para2[8]);
                RedWaterRate5 = Convert.ToInt32(para2[9]);
                RedWaterValue6 = Convert.ToInt32(para2[10]);
                RedWaterRate6 = Convert.ToInt32(para2[11]);
            }

            /// <summary>ParaData字串製作</summary>
            public string CombineData()
            {
                string para1 = $"{Rtp};{WaterInLevel};{WaterOutLevel}";
                string para2 = $"{RedWaterValue1};{RedWaterRate1};{RedWaterValue2};{RedWaterRate2};" +
                               $"{RedWaterValue3};{RedWaterRate3};{RedWaterValue4};{RedWaterRate4};" +
                               $"{RedWaterValue5};{RedWaterRate5};{RedWaterValue6};{RedWaterRate6}";
                return para1 + "#" + para2;
            }

            /// <summary>ParaData解字串成功</summary>
            public bool DataExpandOk()
            {
                if(DataOk1 && DataOk2)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>讀取玩家ParaData</summary>
        void ExpandUserParaData(Dictionary<string, string> datalist, UserData userData)
        {
            if (datalist.ContainsKey(nameof(UserData.ParaData)))
            {
                IpParaDataInfo ipParaDataInfo = new IpParaDataInfo(datalist[nameof(UserData.ParaData)]);
                userData.SetParaData(ipParaDataInfo.CombineData());

                if (ipParaDataInfo.DataExpandOk())
                {
                    return;
                }
            }
            else
            {
                userData.SetParaData(IpParaDataInfo.ParaDataDefaultString());
            }

            //將 ParaData 寫回 DB資料庫
            Dictionary<string, string> updata = new() { { "ParaData", "'" + userData.ParaData + "'" } };
            UpdateUsertable(userData.UserUID, updata);
        }

        /// <summary>後台修改 玩家ParaData參數 處理程序</summary>
        public string IPGetWebSetParaData(WebIpSetPara spara, string paraDataSrc)
        {
            bool IpSetDbFg = true; //WEB設定玩家 Debug用

            bool isChangePara = false;

            IpParaDataInfo ipParaDataInfo = new IpParaDataInfo(paraDataSrc); //先解出原有的參數值

            if (spara.Rtp > WebIpSetPara.NO_CHANGE)
            {
                //玩家% (RTP)
                ipParaDataInfo.Rtp = spara.Rtp;
                isChangePara = true;
                if (IpSetDbFg) MyConsole.WriteLine($"    RTP[{ipParaDataInfo.Rtp}]");
            }
            if (spara.WaterInLevel > WebIpSetPara.NO_CHANGE)
            {
                //放水分段
                ipParaDataInfo.WaterInLevel = spara.WaterInLevel;
                isChangePara = true;
                if (IpSetDbFg) MyConsole.WriteLine($"    放水分段[{ipParaDataInfo.WaterInLevel}]");
            }
            if (spara.WaterOutLevel > WebIpSetPara.NO_CHANGE)
            {
                //抽水分段
                ipParaDataInfo.WaterOutLevel = spara.WaterOutLevel;
                isChangePara = true;
                if (IpSetDbFg) MyConsole.WriteLine($"    抽水分段[{ipParaDataInfo.WaterOutLevel}]");
            }
            if (spara.RedWaterValue1 > WebIpSetPara.NO_CHANGE)
            {
                //红包幣值選中率
                ipParaDataInfo.RedWaterValue1 = spara.RedWaterValue1;
                ipParaDataInfo.RedWaterRate1 = spara.RedWaterRate1;
                ipParaDataInfo.RedWaterValue2 = spara.RedWaterValue2;
                ipParaDataInfo.RedWaterRate2 = spara.RedWaterRate2;
                ipParaDataInfo.RedWaterValue3 = spara.RedWaterValue3;
                ipParaDataInfo.RedWaterRate3 = spara.RedWaterRate3;
                ipParaDataInfo.RedWaterValue4 = spara.RedWaterValue4;
                ipParaDataInfo.RedWaterRate4 = spara.RedWaterRate4;
                ipParaDataInfo.RedWaterValue5 = spara.RedWaterValue5;
                ipParaDataInfo.RedWaterRate5 = spara.RedWaterRate5;
                ipParaDataInfo.RedWaterValue6 = spara.RedWaterValue6;
                ipParaDataInfo.RedWaterRate6 = spara.RedWaterRate6;
                isChangePara = true;
                if (IpSetDbFg) MyConsole.WriteLine($"    红包幣值1[{ipParaDataInfo.RedWaterValue1}({ipParaDataInfo.RedWaterRate1})]" +
                                                            $" 2[{ipParaDataInfo.RedWaterValue2}({ipParaDataInfo.RedWaterRate2})]" +
                                                            $" 3[{ipParaDataInfo.RedWaterValue3}({ipParaDataInfo.RedWaterRate3})]" +
                                                            $" 4[{ipParaDataInfo.RedWaterValue4}({ipParaDataInfo.RedWaterRate4})]" +
                                                            $" 5[{ipParaDataInfo.RedWaterValue5}({ipParaDataInfo.RedWaterRate5})]" +
                                                            $" 6[{ipParaDataInfo.RedWaterValue6}({ipParaDataInfo.RedWaterRate6})]");
            }

            if (isChangePara)
            {
                string paraDataNew = ipParaDataInfo.CombineData();
                spara.ParaDataChgFg = true;
                if (IpSetDbFg) MyConsole.WriteLine($"    ParaData[{paraDataNew}]");
                return paraDataNew;
            }

            return paraDataSrc;
        }

        /// <summary>WEB修改玩家設定值參數</summary>
        public class WebIpSetPara
        {
            public const int NO_CHANGE = -1;

            /// <summary>玩家UID</summary>
            public int Uid;
            /// <summary>EntityId</summary>
            public int EntityId;
            /// <summary>玩家% (-1=不變)</summary>
            public double Rtp;

            //NWO  (-1=不變)
            /// <summary>抽水幣值</summary>
            public double WaterOut;
            /// <summary>清除抽水幣值</summary>
            public bool WaterOutClear;
            /// <summary>抽水分段</summary>
            public int WaterOutLevel;
            /// <summary>抽水斜率</summary>
            //public int WaterOutSlope;

            //NWI  (-1=不變)
            /// <summary>放水幣值</summary>
            public double WaterIn;
            /// <summary>清除放水幣值</summary>
            public bool WaterInClear;
            /// <summary>放水分段</summary>
            public int WaterInLevel;
            //NWS  (-1=不變)
            /// <summary>放水游戏 (""=清除, "0"=不變)</summary>
            public string WaterInServer;

            //BWV  (-1=不變)
            /// <summary>红包抽水幣值</summary>
            public double RedWaterOut;
            /// <summary>红包放水幣值</summary>
            public double RedWaterIn;
            /// <summary>清除红包放水幣值</summary>
            public bool RedWaterInClear;

            //BWS  (-1=不變)
            /// <summary>红包放水游戏 (""=清除, "0"=不變)</summary>
            public string RedWaterInServer;

            //BWR  (-1=不變)
            /// <summary>遊戲紅包幣值 1</summary>
            public int RedWaterValue1;
            /// <summary>遊戲紅包選中率 1</summary>
            public int RedWaterRate1;
            /// <summary>遊戲紅包幣值 2</summary>
            public int RedWaterValue2;
            /// <summary>遊戲紅包選中率 2</summary>
            public int RedWaterRate2;
            /// <summary>遊戲紅包幣值 3</summary>
            public int RedWaterValue3;
            /// <summary>遊戲紅包選中率 3</summary>
            public int RedWaterRate3;
            /// <summary>遊戲紅包幣值 4</summary>
            public int RedWaterValue4;
            /// <summary>遊戲紅包選中率 4</summary>
            public int RedWaterRate4;
            /// <summary>遊戲紅包幣值 5</summary>
            public int RedWaterValue5;
            /// <summary>遊戲紅包選中率 5</summary>
            public int RedWaterRate5;
            /// <summary>遊戲紅包幣值 6</summary>
            public int RedWaterValue6;
            /// <summary>遊戲紅包選中率 6</summary>
            public int RedWaterRate6;

            //KOS  (-1=不變)
            /// <summary>日總洗分限制</summary>
            public int PayOutLimitDay;
            /// <summary>週總洗分限制</summary>
            public int PayOutLimitWeek;
            /// <summary>月總洗分限制</summary>
            public int PayOutLimitMonth;
            /// <summary>日總洗分倍率限制</summary>
            public int PayOutLimitDayMx;

            //CSW  (-1=不變)
            /// <summary>大水庫出牌 開關 (0=關, 1=開)</summary>
            public int BigWaterFg;
            /// <summary>4大JP出牌 開關 (0=關, 1=開)</summary>
            public int BigJPFg;
            /// <summary>強制控制 開關 (0=關, 1=開)</summary>
            public int CalFg;

            /// <summary>玩家ParaData已變更旗號</summary>
            public bool ParaDataChgFg;

            void Init()
            {
                Uid = 0;
                EntityId = 0;
                Rtp = NO_CHANGE;

                WaterOut = NO_CHANGE;
                WaterOutClear = false;
                WaterOutLevel = NO_CHANGE;
                //WaterOutSlope = NO_CHANGE;
                WaterIn = NO_CHANGE;
                WaterInClear = false;
                WaterInLevel = NO_CHANGE;
                WaterInServer = "0";

                RedWaterOut = NO_CHANGE;
                RedWaterIn = NO_CHANGE;
                RedWaterInClear = false;
                RedWaterInServer = "0";

                RedWaterValue1 = NO_CHANGE;
                RedWaterRate1 = NO_CHANGE;
                RedWaterValue2 = NO_CHANGE;
                RedWaterRate2 = NO_CHANGE;
                RedWaterValue3 = NO_CHANGE;
                RedWaterRate3 = NO_CHANGE;
                RedWaterValue4 = NO_CHANGE;
                RedWaterRate4 = NO_CHANGE;
                RedWaterValue5 = NO_CHANGE;
                RedWaterRate5 = NO_CHANGE;
                RedWaterRate6 = NO_CHANGE;
                RedWaterValue6 = NO_CHANGE;

                PayOutLimitDay = NO_CHANGE;
                PayOutLimitWeek = NO_CHANGE;
                PayOutLimitMonth = NO_CHANGE;
                PayOutLimitDayMx = NO_CHANGE;

                BigWaterFg = NO_CHANGE;
                BigJPFg = NO_CHANGE;
                CalFg = NO_CHANGE;

                ParaDataChgFg = false;
            }

            public WebIpSetPara()
            {
                Init();
            }

            /// <summary>解 設定參數字串</summary>
            static public WebIpSetPara ExpandData(string sData)
            {
                WebIpSetPara setData = new();

                string[] arrData = sData.Split('#'); //各組設定以"#"號區隔

                try
                {
                    foreach (string item in arrData)
                    {
                        string[] keyVal = item.Split('=');
                        if (keyVal.Length != 2) continue;

                        string key = keyVal[0].Trim();
                        string value = keyVal[1].Trim();

                        if (key == "UID")
                        {
                            setData.Uid = Convert.ToInt32(value);
                        }
                        if (key == "EntityId")
                        {
                            setData.EntityId = Convert.ToInt32(value);
                        }
                        else if (key == "RTP")
                        {
                            setData.Rtp = ExpDouble(value);
                        }
                        else if (key == "NWO")
                        {
                            string[] vals = value.Split(';');
                            double tmpVal = ExpDouble(vals[0], false);
                            
                            if (tmpVal > 0)
                            {
                                setData.WaterOut = tmpVal;
                                setData.WaterOutLevel = ExpInt(vals[1]);
                                //setData.WaterOutSlope = ExpInt(vals[2]);
                            }
                            else if (tmpVal == -1)
                            {
                                setData.WaterOutClear = true;
                            }
                        }
                        else if (key == "NWI")
                        {
                            string[] vals = value.Split(';');
                            double tmpVal = ExpDouble(vals[0], false);

                            if (tmpVal > 0)
                            {
                                setData.WaterIn = tmpVal;
                                setData.WaterInLevel = ExpInt(vals[1]);
                            }
                            else if (tmpVal == -1)
                            {
                                setData.WaterInClear = true;
                            }
                        }
                        else if (key == "NWS")
                        {
                            setData.WaterInServer = ExpSrv(value);
                        }
                        else if (key == "BWV")
                        {
                            string[] vals = value.Split(';');
                            double tmpValout = ExpDouble(vals[0]);
                            if(tmpValout > 0)
                            {
                                setData.RedWaterOut = tmpValout;
                            }

                            double tmpValin = ExpDouble(vals[1], false);
                            if (tmpValin > 0)
                            {
                                setData.RedWaterIn = tmpValin;
                            }
                            else if (tmpValin == -1)
                            {
                                setData.RedWaterInClear = true;
                            }
                        }
                        else if (key == "BWS")
                        {
                            setData.RedWaterInServer = ExpSrv(value);
                        }
                        else if (key == "BWR")
                        {
                            string[] vals = value.Split(';');
                            setData.RedWaterValue1 = ExpInt(vals[0]);
                            setData.RedWaterRate1 = ExpInt(vals[1]);
                            setData.RedWaterValue2 = ExpInt(vals[2]);
                            setData.RedWaterRate2 = ExpInt(vals[3]);
                            setData.RedWaterValue3 = ExpInt(vals[4]);
                            setData.RedWaterRate3 = ExpInt(vals[5]);
                            setData.RedWaterValue4 = ExpInt(vals[6]);
                            setData.RedWaterRate4 = ExpInt(vals[7]);
                            setData.RedWaterValue5 = ExpInt(vals[8]);
                            setData.RedWaterRate5 = ExpInt(vals[9]);
                            setData.RedWaterValue6 = ExpInt(vals[10]);
                            setData.RedWaterRate6 = ExpInt(vals[11]);
                        }
                        else if (key == "KOS")
                        {
                            string[] vals = value.Split(';');
                            setData.PayOutLimitDay = ExpInt(vals[0]);
                            setData.PayOutLimitWeek = ExpInt(vals[1]);
                            setData.PayOutLimitMonth = ExpInt(vals[2]);
                            setData.PayOutLimitDayMx = ExpInt(vals[3]);
                        }
                        else if (key == "CSW")
                        {
                            string[] vals = value.Split(';');
                            setData.BigWaterFg = ExpOnOff(vals[0]);
                            setData.BigJPFg = ExpOnOff(vals[1]);
                            setData.CalFg = ExpOnOff(vals[2]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    setData.Uid = 0; //表示資料錯誤
                    setData.EntityId = 0;
                    MyConsole.WriteLine("IpSetPara ExpandSetData Error: " + ex.Message);
                }

                return setData;
            }

            /// <summary>判斷 玩家參數 是否有變更 (不含ParaData參數)</summary>
            public bool IsSetDataChange()
            {
                if (WaterIn > 0) return true;
                if (WaterOut > 0) return true;
                if (WaterInServer != "0") return true;

                if (RedWaterOut > 0) return true;
                if (RedWaterIn > 0) return true;
                if (RedWaterInServer != "0") return true;

                if (PayOutLimitDay > 0) return true;
                if (PayOutLimitWeek > 0) return true;
                if (PayOutLimitMonth > 0) return true;
                if (PayOutLimitDayMx > 0) return true;

                if (BigWaterFg > 0) return true;
                if (BigJPFg > 0) return true;
                if (CalFg > 0) return true;

                return false;
            }

            /// <summary>轉double (-1=不變)</summary>
            private static double ExpDouble(string s, bool noZero = true)
            {
                if (string.IsNullOrEmpty(s))
                    return NO_CHANGE;

                double temp = Convert.ToDouble(s.Trim());

                if (noZero) //不可為0和負值
                {
                    if (temp <= 0)
                        return NO_CHANGE;
                }

                return temp;
            }
            /// <summary>轉int (-1=不變)</summary>
            private static int ExpInt(string s)
            {
                if (string.IsNullOrEmpty(s))
                    return NO_CHANGE;
                return Convert.ToInt32(s.Trim());
            }
            /// <summary>控制開關 (0=關, 1=開, -1=不變)</summary>
            private static int ExpOnOff(string s)
            {
                if (string.IsNullOrEmpty(s))
                    return NO_CHANGE;

                int val = Convert.ToInt32(s.Trim());

                if (val == 1)
                {
                    return 1;
                }

                return 0;
            }
            /// <summary>轉字串 ("0"=不變)</summary>
            private static string ExpSrv(string s)
            {
                if (s == null)
                    return "0";
                return s.Trim();
            }
        }
    }
}
