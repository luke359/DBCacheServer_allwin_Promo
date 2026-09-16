using Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DBCacheServer
{
    public class CountrySettingData
    {
        /// <summary>國家名稱</summary>
        public string Name { get; private set; }
        /// <summary>國家代號</summary>
        public int CountryNo { get; private set; } = 0;
        /// <summary>貨幣顯示字串</summary>
        public string CurrencyStr { get; private set; } = "MYR";
        /// <summary>貨幣格式化字串</summary>
        public string Currency { get; private set; } = "{0:0.00} MYR";
        /// <summary>機率用調整參數</summary>
        public string CalcPara { get; private set; } = "CurrFact=1.0;LocalFact=1.0;AccPrec=3"; //CurrFact=幣值大小轉換因子, LocalFact=幣值調整轉換因子
        /// <summary>獲取資源包下載路徑</summary>
        public string ResourceDownloadPath { get; private set; } = "127.0.0.1";

        /// <summary>玩家每日任務 啟用旗號</summary>
        public bool PlayerDayMissionFg { get; private set; } = false;
        /// <summary>玩家每週任務 啟用旗號</summary>
        public bool PlayerWeekMissionFg { get; private set; } = false;
        /// <summary>禁用玩家每日任務</summary>
        public bool NoPlayerDayMission => !PlayerDayMissionFg;
        /// <summary>禁用玩家每週任務</summary>
        public bool NoPlayerWeekMission => !PlayerWeekMissionFg;

        /// <summary>每日工作 最後重置日期</summary>
        public DateTime BusinessLastResetDate = Convert.ToDateTime("2021-01-01").Date;

        /// <summary>外部遊戲商 啟用旗號</summary>
        static public bool ExternGameEnableFg { get; private set; } = false;
        /// <summary>外部遊戲商 啟用列表</summary>
        static public List<Walletlocation> ExternGameList = new List<Walletlocation>();
        ///// <summary>外部遊戲商返回url</summary>
        public string BaseHomeUrl { get; private set; } = "";
        ///// <summary>PGS 啟用旗號</summary>
        //public bool PGSEnableFg { get; private set; } = false;
        ///// <summary>JILI 啟用旗號</summary>
        //public bool JILIEnableFg { get; private set; } = false;
        ///// <summary>FC 啟用旗號</summary>
        //public bool FCEnableFg { get; private set; } = false;
        ///// <summary>AceWin 啟用旗號</summary>
        //public bool AceWinEnableFg { get; private set; } = false;
        ///// <summary>JDB 啟用旗號</summary>
        //public bool JDBEnableFg { get; private set; } = false;


        /// <summary>PGS Api 用的貨幣參數</summary>
        public string PGSCurrency { get; private set; } = "MYR";

        /// <summary>幣值大小轉換因子</summary>
        public double CurrencyFactor { get; private set; } = 1.0d; //對馬幣的比值 230320
        /// <summary>地區幣值消費轉換因子</summary>
        public double LocalFactor { get; private set; } = 1.0d;  //消費習慣對馬幣的比值
        /// <summary>JP自動增值</summary>
        public double JpAccumValue { get; private set; } = 0.01d;


        /// <summary>大廳顯示幣分比</summary>
        public double LobbyRatio = 100;
        /// <summary>歷程顯示模式 : 0=以幣值顯示, 1=以分值顯示</summary>
        public int HistoryDispMode = 0;
        /// <summary>歷程顯示幣幣比</summary>
        public double HistoryCoinRatio = 1;
        /// <summary>歷程顯示幣分比</summary>
        public double HistoryScoreRatio = 100;
        /// <summary>歷程顯示幣值小數位數</summary>
        public int HistoryDispDigits = 2;
        /// <summary>歷程顯示分數小數位數</summary>
        public int HistoryScrDispDigits = 2;
        /// <summary>Credit幣分轉換開關 (0=Off分, 1=On幣, 2=On分, 3=Off幣)</summary>
        public int CreditSwitch = 1;
        /// <summary>Credit數值顯示限制 (單位:萬)</summary>
        public int CreditDisplayLimit = 200;
        /// <summary>中大獎訊息Lv1門檻值</summary>
        public int MsgBigWin = 100;
        /// <summary>中大獎訊息Lv2門檻值</summary>
        public int MsgMegaWin = 1000;
        /// <summary>中大獎訊息Lv3門檻值</summary>
        public int MsgSuperWin = 5000;

        /// <summary>玩家Credit上限</summary>
        public double PlayerBalanceLimit { get; private set; } = 50000.0;
        /// <summary>單筆Credit上限</summary>
        public double PlayerDepositUnit { get; private set; } = 100000.0;

        /// <summary>新版機率 Kios模式組數</summary>
        public int KiosModeCount = 5;
        /// <summary>Kios機台占比 依序為 "Vip,大,中,小,Free"</summary>
        public string KiosMachineSetting = "";

        /// <summary>DEMO玩家初始CREDIT</summary>
        public double DemoPlayCredit = 10; //DEMO玩家初始CREDIT
        /// <summary>DEMO玩家代理名稱</summary>
        public string DemoEntityName = "DEMOFREE";
        /// <summary>是否啟用DEMO玩家模式 (是否有落地頁)</summary>
        public bool IsDemoPlayerMode = false;


        /// <summary>進分外送功能旗標</summary>
        public bool ExtraBonusFg = false;
        /// <summary>進分外送進分Credit限制門檻</summary>
        public double ExtraBonusThreshold = 0;
        /// <summary>進分外送單位進分Credit</summary>
        public double ExtraBonusUnitKeyIn = 0;
        /// <summary>進分外送獎勵Credit</summary>
        public double ExtraBonusValue = 0;
        /// <summary>進分外送_出分限制解除門檻上限(參考玩家PlayerPlayTotal)</summary>
        public double ExtraBonusKeyOutRL_U = 1000000.00;
        /// <summary>進分外送_出分限制解除門檻下限(參考玩家剩餘Credit)</summary>
        public double ExtraBonusKeyOutRL_L = 1000.00;

        /// <summary>管理員Credit上限</summary>
        public double ManagerBalanceLimit { get; private set; } = 50000.0;
        /// <summary>單筆Credit上限</summary>
        public double ManagerDepositUnit { get; private set; } = 100000.0;


        public enum CountryCode
        {
            Unknow = 0,
            新加坡 = 1,
            越南 = 2,
            柬埔寨 = 3
        }

        int GetCountryNo(string cname)
        {
            if (Enum.TryParse(cname, out CountryCode result))
                return (int)result;

            Console.WriteLine($"### CountryCode Unknow (國碼名稱錯誤[{cname}])###");
            return 0;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            Name = datalist["Name"];
            CurrencyStr = datalist["CurrencyStr"];
            Currency = datalist["Currency"];
            CalcPara = datalist["CalcPara"];
            LobbyRatio = Convert.ToDouble(datalist["LobbyRatio"]);
            HistoryDispMode = Convert.ToInt32(datalist["HistoryDispMode"]);
            HistoryCoinRatio = Convert.ToDouble(datalist["HistoryCoinRatio"]);
            HistoryScoreRatio = Convert.ToDouble(datalist["HistoryScoreRatio"]);
            HistoryDispDigits = Convert.ToInt32(datalist["HistoryDispDigits"]);
            HistoryScrDispDigits = Convert.ToInt32(datalist["HistoryScrDispDigits"]);
            CreditSwitch = Convert.ToInt32(datalist["CreditSwitch"]);
            CreditDisplayLimit = Convert.ToInt32(datalist["CreditDisplayLimit"]);
            MsgBigWin = Convert.ToInt32(datalist["MsgBigWin"]);
            MsgMegaWin = Convert.ToInt32(datalist["MsgMegaWin"]);
            MsgSuperWin = Convert.ToInt32(datalist["MsgSuperWin"]);

            PlayerBalanceLimit = Convert.ToDouble(datalist["PlayerBalanceLimit"]);
            PlayerDepositUnit = Convert.ToDouble(datalist["PlayerDepositUnit"]);
            ManagerBalanceLimit = Convert.ToDouble(datalist["ManagerBalanceLimit"]);
            ManagerDepositUnit = Convert.ToDouble(datalist["ManagerDepositUnit"]);

            CountryNo = GetCountryNo(Name);
            MyConsole.WriteLine($"Country[{Name} ({CountryNo})]");

            if (datalist.ContainsKey("ResourceDownloadPath"))
            {
                ResourceDownloadPath = datalist["ResourceDownloadPath"];
                MyConsole.WriteLine("    資源包下載路徑[" + ResourceDownloadPath + "]");
            }
            else
            {
                MyConsole.WriteLine("### 資源包下載路徑欄位未設定 ###");
                while (true) { System.Threading.Thread.Sleep(1000); }
            }

            if (datalist.ContainsKey("JpAccumValue"))
            {
                JpAccumValue = Convert.ToDouble(datalist["JpAccumValue"]);
                MyConsole.WriteLine("    JP自動增值[" + JpAccumValue + "]");
            }
            else
            {
                MyConsole.WriteLine("### JP自動增值欄位未設定 ###");
            }

            if (datalist.ContainsKey("ExtraBonusFg"))
            {
                //#20231128
                ExtraBonusFg = Convert.ToBoolean(datalist["ExtraBonusFg"]);
                ExtraBonusThreshold = Convert.ToDouble(datalist["ExtraBonusThreshold"]);
                ExtraBonusUnitKeyIn = Convert.ToDouble(datalist["ExtraBonusUnitKeyIn"]);
                ExtraBonusValue = Convert.ToDouble(datalist["ExtraBonusValue"]);
                ExtraBonusKeyOutRL_U = Convert.ToDouble(datalist["ExtraBonusKeyOutRL_U"]);
                ExtraBonusKeyOutRL_L = Convert.ToDouble(datalist["ExtraBonusKeyOutRL_L"]);
            }

            GetGameProviderEnableFg(datalist);

            GetEntityPlayerMixFg(datalist);

            string[] paraInfo = CalcPara.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (paraInfo.Length > 0)
            {
                string resustr;
                if (GetCountrySettingOne(paraInfo, "CurrFact", out resustr))
                {
                    CurrencyFactor = Convert.ToDouble(resustr);
                }
                if (GetCountrySettingOne(paraInfo, "LocalFact", out resustr))
                {
                    LocalFactor = Convert.ToDouble(resustr);
                }
            }

            if (datalist.ContainsKey(nameof(KiosModeCount)))
            {
                KiosModeCount = Convert.ToInt32(datalist[nameof(KiosModeCount)]); //#251218
            }
            if (datalist.ContainsKey(nameof(KiosMachineSetting)))
            {
                KiosMachineSetting = datalist[nameof(KiosMachineSetting)]; //#250616
                //Console.WriteLine($"取得 網吧分流 機台佔比: {KiosMachineSetting}");
            }

            //落地頁 DEMO玩家 #251209
            IsDemoPlayerMode = false;
            if (datalist.ContainsKey(nameof(DemoPlayCredit)))
            {
                DemoPlayCredit = Convert.ToInt32(datalist[nameof(DemoPlayCredit)]);

                string[] demoPara = datalist["DemoEntityInfo"].Split(';');
                DemoEntityName = demoPara[0];
                if (demoPara[1] == "ON")
                {
                    IsDemoPlayerMode = true;
                }
                MyConsole.WriteLine($"    DEMO玩家模式:[{(IsDemoPlayerMode ? "ON" : "OFF")}], 代理名稱[{DemoEntityName}], 初始CREDIT[{DemoPlayCredit}]幣");
            }
            else
            {
                //MyConsole.WriteLine("### DEMO玩家模式欄位未設定 ###");
            }
        }

        bool GetCountrySettingOne(string[] paraInfo, string key, out string resustr)
        {
            for (int i = 0; i < paraInfo.Length; i++)
            {
                string[] paraData = paraInfo[i].Split('=');
                if (paraData.Length == 2 && paraData[0] == key)
                {
                    resustr = paraData[1];
                    return true;
                }
            }
            resustr = "";
            return false;
        }

        /// <summary>將貨幣依設定轉成顯示用值</summary>
        public double TransToDisplayScore(double money)
        {
            double result;
            if (CreditSwitch == 1 || CreditSwitch == 3)
            {
                //以幣顯示
                double xMoney = money * HistoryCoinRatio;
                result = Math.Round(xMoney, HistoryDispDigits);
                //MyConsole.WriteLine($"以幣顯示={result}, CoinRatio={HistoryCoinRatio}");
            }
            else
            {
                //以分顯示
                double xMoney = money * HistoryScoreRatio;
                result = Math.Round(xMoney, HistoryScrDispDigits); //這邊須轉成分數
                //MyConsole.WriteLine($"以分顯示={result}, ScoreRatio={HistoryScoreRatio}");
            }
            return result;
        }
        /// <summary>將貨幣依設定轉成顯示用值</summary>
        public string TransToDisplayString(double money)
        {
            double trmoney = TransToDisplayScore(money);

            if (CreditSwitch == 1 || CreditSwitch == 3)
            {
                if (HistoryDispDigits == 0) return trmoney.ToString("f0");
                if (HistoryDispDigits == 1) return trmoney.ToString("f1");
                if (HistoryDispDigits == 2) return trmoney.ToString("f2");
                if (HistoryDispDigits == 3) return trmoney.ToString("f3");
            }
            else
            {
                if (HistoryScrDispDigits == 0) return trmoney.ToString("f0");
                if (HistoryScrDispDigits == 1) return trmoney.ToString("f1");
                if (HistoryScrDispDigits == 2) return trmoney.ToString("f2");
                if (HistoryScrDispDigits == 3) return trmoney.ToString("f3");
            }
            return trmoney.ToString();
        }

        /// <summary>魚機 不分代理商混合遊玩旗號</summary>
        public bool EntityMixFish { get; private set; } = false;
        /// <summary>押分機 不分代理商混合遊玩旗號</summary>
        public bool EntityMixArcade { get; private set; } = false;

        /// <summary></summary>
        void GetEntityPlayerMixFg(Dictionary<string, string> datalist)
        {
            //多人遊戲不分代理商混合遊玩
            //Fish=1,Arcad=1
            EntityMixFish = false;
            EntityMixArcade = false;
            if (datalist.ContainsKey("EntityPlayerMixFg"))
            {
                if (datalist["EntityPlayerMixFg"] != null)
                {
                    string[] paras = datalist["EntityPlayerMixFg"].Split(',');
                    if (paras.Length > 0)
                    {
                        for (int i = 0; i < paras.Length; i++)
                        {
                            string[] data = paras[i].Split('=');

                            if (data[0] == "Fish")
                            {
                                if (data[1] == "1") EntityMixFish = true;
                            }
                            else if (data[0] == "Arcade")
                            {
                                if (data[1] == "1") EntityMixArcade = true;
                            }
                        }

                        if (EntityMixFish || EntityMixArcade)
                        {
                            MyConsole.WriteLine($"    多人遊戲不分代理商混合遊玩: Fish[{EntityMixFish}], Arcade[{EntityMixArcade}]");
                        }
                    }
                }
            }
        }
        /// <summary>從DB資料 取得 外部遊戲商啟用旗號</summary>
        void GetGameProviderEnableFg(Dictionary<string, string> datalist)
        {
            ExternGameEnableFg = false;
            ExternGameList = new List<Walletlocation>();
            //PGSEnableFg = false;
            //JILIEnableFg = false;
            //FCEnableFg = false;
            //AceWinEnableFg = false;
            //JDBEnableFg = false;


            //GameProviderEnableFg  ExternGameEnableFg "PGS=1,JILI=1,FC=1"
            if (datalist.ContainsKey("GameProviderEnableFg"))
            {
                if (datalist["GameProviderEnableFg"] != null)
                {
                    string[] paras = datalist["GameProviderEnableFg"].Split(',');
                    if (paras.Length > 0)
                    {
                        for (int i = 0; i < paras.Length; i++)
                        {
                            string[] data = paras[i].Split('=');

                            if (data[0] == "PGS")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.PGS);
                            }
                            else if (data[0] == "JILI")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.JILI);
                            }
                            else if (data[0] == "FC")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.FC);
                            }
                            else if (data[0] == "AceWin")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.AceWin);
                            }
                            else if (data[0] == "JDB")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.JDB);
                            }
                            else if (data[0] == "DCT")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.DCT);
                            }
                            else if (data[0] == "HABA")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.HABA);
                            }
                            else if (data[0] == "FastSpin")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.FastSpin);
                            }
                            else if (data[0] == "Spade")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.Spade);
                            }
                            else if (data[0] == "PlayTech")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.PlayTech);
                            }
                            else if (data[0] == "CP")
                            {
                                if (data[1] == "1") ExternGameList.Add(Walletlocation.CP);
                            }
                        }
                    }
                }
            }

            //if (PGSEnableFg || JILIEnableFg || FCEnableFg || AceWinEnableFg || JDBEnableFg)
            if (ExternGameList.Count > 0)
            {
                ExternGameEnableFg = true;

                StringBuilder sb = new StringBuilder();
                foreach (var item in ExternGameList)
                {
                    sb.Append("[" + item.ToString() + "],");
                }

                MyConsole.WriteLine($"    H5遊戲商啟用旗號: {sb.ToString()}");
            }


            if (datalist.ContainsKey("BaseHomeUrl") && datalist["BaseHomeUrl"] != null)
            {
                BaseHomeUrl = datalist["BaseHomeUrl"];
                MyConsole.WriteLine($"    H5遊戲商返回URL: {BaseHomeUrl}");
            }
        }

        /// <summary>大廳幣值格式設定字串(玩家Login時)</summary>
        public string GetCurrencyLobbySetting()
        {
            return LobbyRatio + ";" + HistoryDispMode + ";" + HistoryCoinRatio + ";" + HistoryScoreRatio + ";" + HistoryDispDigits + ";" + HistoryScrDispDigits + ";" + 
                   CreditSwitch + ";" + CreditDisplayLimit + ";" + CountryNo;
        }

        /// <summary>獲取貨幣格式化字串(GameServer要設定時)</summary>
        public Dictionary<string, string> GetCurrencyParaString(GameTypeCode gType)
        {
            Dictionary<string, string> datalist = new Dictionary<string, string>();
            datalist.Add("CurrencyStr", CurrencyStr);
            datalist.Add("Currency", Currency);
            datalist.Add("CalcPara", CalcPara);

            datalist.Add("MsgBigWin", MsgBigWin.ToString());
            datalist.Add("MsgMegaWin", MsgMegaWin.ToString());
            datalist.Add("MsgSuperWin", MsgSuperWin.ToString());

            datalist.Add("LobbyRatio", LobbyRatio.ToString());
            datalist.Add("HistoryDispMode", HistoryDispMode.ToString());
            datalist.Add("HistoryCoinRatio", HistoryCoinRatio.ToString());
            datalist.Add("HistoryScoreRatio", HistoryScoreRatio.ToString());
            datalist.Add("HistoryDispDigits", HistoryDispDigits.ToString());
            datalist.Add("HistoryScrDispDigits", HistoryScrDispDigits.ToString());

            //datalist.Add("KiosMachineRate", KiosMachineRate.ToString()); //#250310
            datalist.Add("KiosMachineSetting", KiosMachineSetting); //#250616
            datalist.Add("KiosModeCount", KiosModeCount.ToString()); //#251218

            if((gType == GameTypeCode.Fish && EntityMixFish) ||
               (gType == GameTypeCode.Arcade && EntityMixArcade))
            {
                datalist.Add("EntityMixFg", "1");
            }

            return datalist;
        }
    }
}
