using Google.Protobuf;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace DBCacheServer
{
    /// <summary>APP平台 (以Client前端代號為準)</summary>
    public enum LgtPlatform
    {
        None = 0,
        Alibaba = 4,
        Lucky7 = 6,
    }

    public class EntityData
    {
        /// <summary>代理商ID</summary>
        public int EntityId;
        /// <summary>代理商名稱</summary>
        public string Name = "";
        /// <summary>前端網頁平台ID(對照表單LandPlatformName)</summary>
        public int LandID { get; private set; }

        /// <summary>Kios模式 (0=Vip, 1=大, 2=中, 3=小, 4=Free)</summary>
        public int KiosMode { get; private set; }

        /// <summary>代理商 可用平台列表</summary>
        List<LgtPlatform> PlatformNameList = new();
        /// <summary>代理商 受控制平台列表</summary>
        readonly List<LgtPlatform> PlatformCtrlList = new() { LgtPlatform.Alibaba, LgtPlatform.Lucky7 };

        /// <summary>玩家機率內定值</summary>
        string CalcSet = "10,10;150,300;500;10;30,50;100;0;0;100;50;100;50;1;0";

        /// <summary>第三方支付 開關</summary>
        public int PaymentFg;

        /// <summary>第三方支付 開關</summary>
        public bool CreditReviewFg;

        /// <summary>代理商客服資訊</summary>
        public string ServiceInfo { get; private set; } = "";

        //排行榜 #250924
        /// <summary>排行榜活動 任務號碼</summary>
        public int RankEventTaskNo;
        /// <summary>排行榜活動 開關</summary>
        public bool RankingFg { get; private set; } = false;
        /// <summary>排行榜 龍榜開關</summary>
        public bool RankDragonFg { get; private set; } = false;
        /// <summary>排行榜 虎榜開關</summary>
        public bool RankTigerFg { get; private set; } = false;
        /// <summary>排行榜 單局勝分榜開關</summary>
        public bool RankTopWinFg { get; private set; } = false;
        /// <summary>排行榜 單局倍率榜開關</summary>
        public bool RankTopOddsFg { get; private set; } = false;
        /// <summary>排行榜 日勝分榜開關</summary>
        public bool RankDailyTopWinFg { get; private set; } = false;
        /// <summary>排行榜 日倍率榜開關</summary>
        public bool RankDailyTopOddsFg { get; private set; } = false;


        #region 網吧炒場
        /// <summary>網吧炒場 開關</summary>
        public bool HypeFg { get; private set; }
        /// <summary>網吧炒場 %差值</summary>
        double HypeProbabilityDiff;

        /// <summary>網吧炒場 老虎機放水率</summary>
        int SlotHypeProbability;
        /// <summary>網吧炒場 老虎機放水最低倍</summary>
        int SlotHypeMinOdds;
        /// <summary>網吧炒場 老虎機放水最高倍</summary>
        int SlotHypeMaxOdds;

        /// <summary>網吧炒場 押分機放水率</summary>
        int ArcadesHypeProbability;
        /// <summary>網吧炒場 押分機放水最低倍</summary>
        int ArcadesHypeMinOdds;
        /// <summary>網吧炒場 押分機放水最高倍</summary>
        int ArcadesHypeMaxOdds;

        /// <summary>網吧炒場 捕魚機放水率</summary>
        int FishHypeProbability;
        /// <summary>網吧炒場 捕魚機放水最低倍</summary>
        int FishHypeMinOdds;
        /// <summary>網吧炒場 捕魚機放水最高倍</summary>
        int FishHypeMaxOdds;

        /// <summary>網吧炒場 撲克機放水率</summary>
        //public int PokerGameProb;

        /// <summary>網吧炒場 使用Server列表 (false=不使用=全部都炒場)</summary>
        public bool HypeServerFg { get; private set; } = false;
        /// <summary>網吧炒場 Server列表</summary>
        public List<GameServerCode> HypeServer { get; private set; }

        /// <summary>網吧炒場 當前水庫值</summary>
        public double CurrentHypeBigWater;

        /// <summary>從DB取得 網吧炒場 設定</summary>
        void GetDBHypeInfo(Dictionary<string, string> datalist)
        {
            HypeFg = false;
            HypeServerFg = false;

            if (datalist.ContainsKey("HypeServer"))
            {
                HypeFg = Convert.ToBoolean(datalist["HypeFg"]);
                HypeProbabilityDiff = Convert.ToDouble(datalist["HypeProbabilityDiff"]);

                CurrentHypeBigWater = Convert.ToDouble(datalist["CurrentHypeBigWater"]);

                SlotHypeProbability = Convert.ToInt32(datalist["SlotHypeProbability"]);
                SlotHypeMinOdds = Convert.ToInt32(datalist["SlotHypeMinOdds"]);
                SlotHypeMaxOdds = Convert.ToInt32(datalist["SlotHypeMaxOdds"]);

                ArcadesHypeProbability = Convert.ToInt32(datalist["ArcadesHypeProbability"]);
                ArcadesHypeMinOdds = Convert.ToInt32(datalist["ArcadesHypeMinOdds"]);
                ArcadesHypeMaxOdds = Convert.ToInt32(datalist["ArcadesHypeMaxOdds"]);

                FishHypeProbability = Convert.ToInt32(datalist["FishHypeProbability"]);
                FishHypeMinOdds = Convert.ToInt32(datalist["FishHypeMinOdds"]);
                FishHypeMaxOdds = Convert.ToInt32(datalist["FishHypeMaxOdds"]);
                //MyConsole.WriteLine("Slot[" + SlotHypeProbability + "," + SlotHypeMinOdds + "~" + SlotHypeMaxOdds + 
                //               "], Fish[" + FishHypeProbability + "," + FishHypeMinOdds + "~" + FishHypeMaxOdds + "]");


                if (datalist.ContainsKey("HypeServer"))
                {
                    HypeServer = CacheManeger.DecodeServerList(datalist["HypeServer"], $"[{Name}]網吧炒場");
                    
                    if (HypeServer != null && HypeServer.Count > 0)
                    {
                        HypeServerFg = true; //有指定遊戲
                    }
                }
            }
        }

        /// <summary>WEB更新 網吧炒場 設定</summary>
        public void UpdataHypeSetting(bool hypeFg, double hypeProbabilityDiff, string probSetInfo)
        {
            //MyConsole.WriteLine("probSetInfo=" + probSetInfo);
            string[] data = probSetInfo.Split(',');
            if (data.Length >= 9)
            {
                int SlotProb = Convert.ToInt32(data[0]);
                int SlotMin = Convert.ToInt32(data[1]);
                int SlotMax = Convert.ToInt32(data[2]);

                int ArcadesProb = Convert.ToInt32(data[3]);
                int ArcadesMin = Convert.ToInt32(data[4]);
                int ArcadesMax = Convert.ToInt32(data[5]);

                int FishProb = Convert.ToInt32(data[6]);
                int FishMin = Convert.ToInt32(data[7]);
                int FishMax = Convert.ToInt32(data[8]);

                //bool hypeNCFg = data[9] == "1";

                HypeFg = hypeFg;
                HypeProbabilityDiff = hypeProbabilityDiff;

                SlotHypeProbability = SlotProb;
                SlotHypeMinOdds = SlotMin;
                SlotHypeMaxOdds = SlotMax;

                ArcadesHypeProbability = ArcadesProb;
                ArcadesHypeMinOdds = ArcadesMin;
                ArcadesHypeMaxOdds = ArcadesMax;

                FishHypeProbability = FishProb;
                FishHypeMinOdds = FishMin;
                FishHypeMaxOdds = FishMax;

                //HypeNCFg = hypeNCFg;
            }
        }
        #endregion


        #region 大廳資訊開關
        /// <summary>簽到獎勵開關</summary>
        bool SignInRewardsFg = true;
        /// <summary>任務獎勵開關</summary>
        bool TaskRewardsFg = true;
        /// <summary>彩券專區開關</summary>
        bool LotteryFg = true;
        /// <summary>玩家資訊開關</summary>
        bool PlayerInfoFg = true;
        /// <summary>促銷專區開關</summary>
        bool PromotionAreaFg = true;

        /// <summary>從DB取得 大廳資訊開關 設定</summary>
        void GetDBLobbyInfo(Dictionary<string, string> datalist)
        {
            if (datalist.ContainsKey("SignInRewardsFg"))
            {
                SignInRewardsFg = Convert.ToBoolean(datalist["SignInRewardsFg"]);
                TaskRewardsFg = Convert.ToBoolean(datalist["TaskRewardsFg"]);
                LotteryFg = Convert.ToBoolean(datalist["LotteryFg"]);
                PlayerInfoFg = Convert.ToBoolean(datalist["PlayerInfoFg"]);
                PromotionAreaFg = Convert.ToBoolean(datalist["PromotionAreaFg"]);
            }
        }
        /// <summary>WEB更新 大廳資訊開關 設定</summary>
        public bool UpdateLobbyInfoFg(string dataInfo)
        {
            string[] sary = dataInfo.Split(',');

            if (sary.Length < 4) return false;

            SignInRewardsFg = sary[0] == "0" ? false : true;
            TaskRewardsFg = sary[1] == "0" ? false : true;
            LotteryFg = sary[2] == "0" ? false : true;
            PlayerInfoFg = sary[3] == "0" ? false : true;
            PromotionAreaFg = sary[4] == "0" ? false : true;

            return true;
        }
        /// <summary>前端Client Login時 取得 大廳資訊開關</summary>
        public void GetLobbyInfoFg(ref Dictionary<string, string> datalist, CountrySettingData countrySetting)
        {
            datalist.Add("SignInRewardsFg", BoolToString(SignInRewardsFg));
            datalist.Add("TaskRewardsFg", BoolToString(TaskRewardsFg));
            if (countrySetting.PlayerDayMissionFg) datalist.Add("LotteryFg", BoolToString(LotteryFg));
            else datalist.Add("LotteryFg", "0");
            datalist.Add("PlayerInfoFg", BoolToString(PlayerInfoFg));
            datalist.Add("PromotionAreaFg", BoolToString(PromotionAreaFg));
        }
        /// <summary>布林值轉字串</summary>
        string BoolToString(bool fg)
        {
            if (fg) return "1";
            else return "0";
        }
        #endregion


        #region 外部遊戲商開關
        /// <summary>PgSlot Game 開關</summary>
        public int PgGameFg;
        /// <summary>JILI Game 開關</summary>
        public int JILIGameFg;
        /// <summary>FC Game 開關</summary>
        public int FCGameFg;
        /// <summary>AceWin Game 開關</summary>
        public int AceWinGameFg;
        /// <summary>JDB Game 開關</summary>
        public int JDBGameFg;
        /// <summary>HABA Game 開關</summary>
        public int HABAGameFg;
        /// <summary>FastSpin Game 開關</summary>
        public int FastSpinGameFg;
        /// <summary>Spade Game 開關</summary>
        public int SpadeGameFg;
        /// <summary>PlayTech Game 開關</summary>
        //public int PlayTechGameFg;

        /// <summary>DCT Game 開關</summary>
        public int DCTGameFg;
        /// <summary>DCT RawGaming 開關</summary>
        public int DCTRAWFg;
        /// <summary>DCT Octoplay 開關</summary>
        public int DCTOPFg;
        /// <summary>DCT Slotmill 開關</summary>
        public int DCTSMFg;
        /// <summary>DCT ThunderKick 開關</summary>
        public int DCTTKFg;
        /// <summary>DCT Evoplay 開關</summary>
        public int DCTEVOFg;
        /// <summary>DCT Tequity 開關</summary>
        public int DCTTEQFg;
        /// <summary>DCT TurboGames 開關</summary>
        public int DCTTGFg;
        /// <summary>DCT Spribe 開關</summary>
        public int DCTSPRFg;
        /// <summary>DCT WinFast 開關</summary>
        public int DCTWFFg;
        /// <summary>DCT PeterSons 開關</summary>
        public int DCTPSFg;
        /// <summary>DCT HacksawGaming Slot開關</summary>
        public int DCTHSGSlotFg;
        /// <summary>DCT HacksawGaming Inst開關</summary>
        public int DCTHSGInstFg;
        /// <summary>DCT LuckyMonaco 開關</summary>
        public int DCTLMFg;

        /// <summary>PlayTeck Slot 開關</summary>
        public int PTSlotFg;
        /// <summary>PlayTeck Live 開關</summary>
        public int PTLiveFg;
        /// <summary>PlayTeck Other 開關</summary>
        public int PTOtherFg;

        /// <summary>CP SlotGame 開關</summary>
        public int CPSlotFg;
        /// <summary>CP MiniGame 開關</summary>
        public int CPMiniFg;
        /// <summary>LGT廣告 開關</summary>
        public string LGTAdFg;


        /// <summary>從DB取得 外部遊戲商開關 設定</summary>
        void GetDBGameProviderFg(Dictionary<string, string> datalist)
        {
            if (datalist.ContainsKey("PgGameFg"))
                PgGameFg = Convert.ToInt32(datalist["PgGameFg"]);
            if (datalist.ContainsKey("JILIGameFg"))
                JILIGameFg = Convert.ToInt32(datalist["JILIGameFg"]);
            if (datalist.ContainsKey("FCGameFg"))
                FCGameFg = Convert.ToInt32(datalist["FCGameFg"]);
            if (datalist.ContainsKey("AceWinGameFg"))
                AceWinGameFg = Convert.ToInt32(datalist["AceWinGameFg"]);
            if (datalist.ContainsKey("JDBGameFg"))
                JDBGameFg = Convert.ToInt32(datalist["JDBGameFg"]);
            if (datalist.ContainsKey("DCTGameFg"))
                DCTGameFg = Convert.ToInt32(datalist["DCTGameFg"]);
            if (datalist.ContainsKey("HABAGameFg"))
                HABAGameFg = Convert.ToInt32(datalist["HABAGameFg"]);
            if (datalist.ContainsKey("FastSpinGameFg"))
                FastSpinGameFg = Convert.ToInt32(datalist["FastSpinGameFg"]);
            if (datalist.ContainsKey("SPADEGameFg"))
                SpadeGameFg = Convert.ToInt32(datalist["SPADEGameFg"]);
            //if (datalist.ContainsKey("PlayTechGameFg"))
            //    PlayTechGameFg = Convert.ToInt32(datalist["PlayTechGameFg"]);
            if (datalist.ContainsKey("CPSlotFg"))
                CPSlotFg = Convert.ToInt32(datalist["CPSlotFg"]);
            if (datalist.ContainsKey("CPMiniFg"))
                CPMiniFg = Convert.ToInt32(datalist["CPMiniFg"]);

            if (datalist.ContainsKey("DCTRawgamingFg"))
                DCTRAWFg = Convert.ToInt32(datalist["DCTRawgamingFg"]);
            if (datalist.ContainsKey("DCTOctoplayFg"))
                DCTOPFg = Convert.ToInt32(datalist["DCTOctoplayFg"]);
            if (datalist.ContainsKey("DCTSlotmillFg"))
                DCTSMFg = Convert.ToInt32(datalist["DCTSlotmillFg"]);
            if (datalist.ContainsKey("DCTThunderkickFg"))
                DCTTKFg = Convert.ToInt32(datalist["DCTThunderkickFg"]);
            if (datalist.ContainsKey("DCTEvoplayFg"))
                DCTEVOFg = Convert.ToInt32(datalist["DCTEvoplayFg"]);
            if (datalist.ContainsKey("DCTTequityFg"))
                DCTTEQFg = Convert.ToInt32(datalist["DCTTequityFg"]);
            if (datalist.ContainsKey("DCTTurboFg"))
                DCTTGFg = Convert.ToInt32(datalist["DCTTurboFg"]);
            if (datalist.ContainsKey("DCTSprideFg"))
                DCTSPRFg = Convert.ToInt32(datalist["DCTSprideFg"]);
            if (datalist.ContainsKey("DCTWinFastFg"))
                DCTWFFg = Convert.ToInt32(datalist["DCTWinFastFg"]);
            if (datalist.ContainsKey("DCTPeterSonsFg"))
                DCTPSFg = Convert.ToInt32(datalist["DCTPeterSonsFg"]);
            if (datalist.ContainsKey("DCTHacksawSlotFg"))
                DCTHSGSlotFg = Convert.ToInt32(datalist["DCTHacksawSlotFg"]);
            if (datalist.ContainsKey("DCTHacksawInsFg"))
                DCTHSGInstFg = Convert.ToInt32(datalist["DCTHacksawInsFg"]);
            if (datalist.ContainsKey("DCTLuckyMonacoFg"))
                DCTLMFg = Convert.ToInt32(datalist["DCTLuckyMonacoFg"]);

            if (datalist.ContainsKey("PTSlotFg"))
                PTSlotFg = Convert.ToInt32(datalist["PTSlotFg"]);
            if (datalist.ContainsKey("PTLiveFg"))
                PTLiveFg = Convert.ToInt32(datalist["PTLiveFg"]);
            if (datalist.ContainsKey("PTOtherFg"))
                PTOtherFg = Convert.ToInt32(datalist["PTOtherFg"]);

            if (datalist.ContainsKey("LGTAdFg"))
                LGTAdFg = datalist["LGTAdFg"];
        }

        /// <summary>前端Client Login時 要給的設定字串</summary>
        public string GetSysSettingString()
        {
            string syssetting = "PaymentFg:" + PaymentFg +
                          ";" + "PgGameFg:" + PgGameFg +
                          ";" + "JILIGameFg:" + JILIGameFg +
                          ";" + "FCGameFg:" + FCGameFg +
                          ";" + "AceWinGameFg:" + AceWinGameFg +
                          ";" + "JDBGameFg:" + JDBGameFg +
                          ";" + "HABAGameFg:" + HABAGameFg +
                          ";" + "FastSpinGameFg:" + FastSpinGameFg +

                          ";" + "DCTRAWFg:" + DCTRAWFg +
                          ";" + "DCTOPFg:" + DCTOPFg +
                          ";" + "DCTSMFg:" + DCTSMFg +
                          ";" + "DCTTKFg:" + DCTTKFg +
                          ";" + "DCTEVOFg:" + DCTEVOFg +
                          ";" + "DCTTEQFg:" + DCTTEQFg +
                          ";" + "DCTTGFg:" + DCTTGFg +
                          ";" + "DCTSPRFg:" + DCTSPRFg +
                          ";" + "DCTWFFg:" + DCTWFFg +
                          ";" + "DCTPSFg:" + DCTPSFg +
                          ";" + "DCTHSGSlotFg:" + DCTHSGSlotFg +
                          ";" + "DCTHSGInstFg:" + DCTHSGInstFg +
                          ";" + "DCTLMFg:" + DCTLMFg +

                          //";" + "PlayTechGameFg:" + PlayTechGameFg +
                          ";" + "PTSlotFg:" + PTSlotFg +
                          ";" + "PTLiveFg:" + PTLiveFg +
                          ";" + "PTOtherFg:" + PTOtherFg +

                          ";" + "SpadeGameFg:" + SpadeGameFg +

                          ";" + "CPSlotFg:" + CPSlotFg +
                          ";" + "CPMiniFg:" + CPMiniFg +
                          ";" + "LGTAdFg:" + LGTAdFg;
            return syssetting;
        }
        #endregion


        /// <summary>從DB的Entity表格 更新本地Entity內存</summary>
        public void GetDBData(Dictionary<string, string> datalist, CountrySettingData countrySetting)
        {
            EntityId = Convert.ToInt32(datalist["id"]);

            Name = datalist["name"];

            //Kios模式
            if (datalist.ContainsKey("KiosMode")) //#250310
            {
                KiosMode = Convert.ToInt32(datalist["KiosMode"]);
            }
            else
            {
                KiosMode = 0;
            }

            if (datalist.ContainsKey("LandID"))
            {
                if (datalist["LandID"] != null)
                {
                    LandID = Convert.ToInt32(datalist["LandID"]);
                    MyConsole.WriteLine($"    取得代理商[{Name}]前端平台ID[{LandID}]");
                }
            }

            //網吧試玩
            //HypeNCFg = Convert.ToBoolean(datalist["HypeNCFg"]);
            //HypeNCFg = false;
            //HypeNCTakeOutLimit = 3000;
            //HypeNCTakeOutProbability = 0.3;

            //第三方支付
            PaymentFg = Convert.ToInt32(datalist["PaymentFg"]);
            CreditReviewFg = Convert.ToBoolean(datalist["CreditReviewFg"]);

            //大廳資訊開關
            GetDBLobbyInfo(datalist);

            //網吧炒場
            GetDBHypeInfo(datalist);

            //紅包炒場
            FeverRedBonusGetDBData(datalist);

            //外部遊戲商開關
            GetDBGameProviderFg(datalist);

            //返水 #250807
            RebateGetDBData(datalist);

            //平台
            GetPlatformNameData(datalist);

            //排行榜活動開關 #250924
            if (datalist.ContainsKey("RankingFg"))
            {
                RankingFg = Convert.ToBoolean(datalist["RankingFg"]);
                RankEventTaskNo = Convert.ToInt32(datalist["RankEventTaskNo"]);

                //榜單種類啟用開關
                RankDragonFg = false;
                RankTigerFg = false;
                RankTopWinFg = false;
                RankTopOddsFg = false;
                RankDailyTopWinFg = false;
                RankDailyTopOddsFg = false;

                string[] type = datalist["RankEventData"].Split(',');
                if (type.Length >= 6)
                {
                    RankDragonFg = type[0] == "1";
                    RankTigerFg = type[1] == "1";
                    RankTopWinFg = type[2] == "1";
                    RankTopOddsFg = type[3] == "1";
                    RankDailyTopWinFg = type[4] == "1";
                    RankDailyTopOddsFg = type[5] == "1";
                }
            }


            if (datalist.ContainsKey("ServiceInfo"))
            {
                if (datalist["ServiceInfo"] != null)
                {
                    ServiceInfo = datalist["ServiceInfo"];
                }
            }
        }

        /// <summary>更新Entity Definition</summary>
        public void ExtraDefinitionData(Dictionary<string, string> datalist)
        {
            if (datalist.ContainsKey("CalcSet"))
            {
                CalcSet = datalist["CalcSet"];
                //MyConsole.WriteLine($"    取得代理商[{Name}]玩家機率內定值[{CalcSet}]");
            }
        }
        /// <summary>更新Entity Definition (CalcSet)</summary>
        public void SetDefinitionData(string calaSet)
        {
            CalcSet = calaSet;
            //MyConsole.WriteLine($"    重設代理商[{Name}]玩家機率內定值[{CalcSet}]");
        }
        /// <summary>玩家進入遊戲時, 要給玩家的Entity資訊</summary>
        public void GetEntityExDataEnterGame(Dictionary<string, string> updata)
        {
            updata.Add("DefaultCalcSet", CalcSet);
        }

        /// <summary>GameServer 取得網吧炒場資訊</summary>
        public string GetHypeInfo()
        {
            string hype = HypeFg ? "1" : "0";
            //string hypeNc = HypeNCFg ? "1" : "0";
            string DataTxt = ";HypeFg:" + hype +
                           ";HypeProbabilityDiff:" + HypeProbabilityDiff.ToString("f2") +
                           ";HypeGameProb:" + SlotHypeProbability + "," + SlotHypeMinOdds + "," + SlotHypeMaxOdds +
                           "," + ArcadesHypeProbability + "," + ArcadesHypeMinOdds + "," + ArcadesHypeMaxOdds +
                           "," + FishHypeProbability + "," + FishHypeMinOdds + "," + FishHypeMaxOdds;
                           //";HypeNCFg:" + hypeNc;
            return DataTxt;
        }
        /// <summary>GameServer 取得玩家代理商額外資訊 (網吧炒場, Kios)</summary>
        public string GetExtraDataInfo()
        {
            //資料格式 Key:Data
            //每組資料以";"分隔
            //StringBuilder extData = new StringBuilder();
            //extData.Append(";HypeFg:"); //字首記得加分號";"
            //extData.Append(HypeFg.ToString());
            //extData.Append(";HypeProbabilityDiff:");
            //extData.Append(HypeProbabilityDiff.ToString("f2"));
            //string DataTxt = extData.ToString();

            string DataTxt = GetHypeInfo() +
                           ";KiosMode:" + KiosMode.ToString() +
                           ";EntityName:" + Name;

            return DataTxt;
        }

        /// <summary>設定Kios模式 (0=正常, 1=Kios)</summary>
        public void SetKiosMode(int kiosMode)
        {
            KiosMode = kiosMode;
        }

        /// <summary>取得代理商平台資訊</summary>
        void GetPlatformNameData(Dictionary<string, string> datalist)
        {
            PlatformNameList.Clear();
            if (datalist.ContainsKey("PlatformNameData"))
            {
                if (datalist["PlatformNameData"] != null && datalist["PlatformNameData"] != "")
                {
                    var plist = datalist["PlatformNameData"].Split(',');
                    if (plist.Length > 0)
                    {
                        if (plist[0] == "1")
                        {
                            PlatformNameList.Add(LgtPlatform.Alibaba); //Alibaba 平台
                            //MyConsole.WriteLine($"[{Name}] Alibaba Platform Enable");
                        }
                        if (plist.Length > 1 && plist[1] == "1")
                        {
                            PlatformNameList.Add(LgtPlatform.Lucky7); //Lucky7 平台
                            //MyConsole.WriteLine($"[{Name}] Lucky7 Platform Enable");
                        }
                    }
                }
            }
            if (PlatformNameList.Count == 0)
            {
                //預設啟用 Alibaba 平台
                PlatformNameList.Add(LgtPlatform.Alibaba);
                //MyConsole.WriteLine($"[{Name}] Default Alibaba Platform Enable");
            }
        }
        /// <summary>檢查代理商平台是否開啟</summary>
        public bool IsPlatformEnable(int platformId)
        {
            if(platformId <= 0) return false; //無平台 關閉

            LgtPlatform plat = (LgtPlatform)platformId;

            //檢查是否為受控制平台
            if(!PlatformCtrlList.Contains(plat))
            {
                return true; //非受控制平台都開啟
            }

            return PlatformNameList.Contains(plat);
        }

        #region 紅包炒場 //#250211
        const bool FRBnsDbuFg = false; //_UNDONE: 紅包炒場 DEBUG

        /// <summary>代理商 紅包炒場啟用開關</summary>
        public bool FeverRedBonusFg { get; set; } = false;
        /// <summary>代理商 紅包炒場水庫</summary>
        public double FeverRedBonusPool;

        /// <summary>紅包炒場 使用Server列表 (false=不使用=全部都炒場)</summary>
        bool RedHypeServerFg = false;
        /// <summary>紅包炒場 Server列表</summary>
        List<GameServerCode> RedHypeServer;

        /// <summary>代理商 紅包炒場發放率秒數</summary>
        int FeverRedBonusIncidence_sec;
        /// <summary>代理商 紅包炒場玩家1日紅包限制</summary>
        public int PlayerDaysFeverRedBonusLimit { get; private set; }

        /// <summary>代理商 老虎機 紅包炒場最低押分限制</summary>
        double FeverRedBonusSlotMinBetLimit;
        /// <summary>代理商 押分機 紅包炒場最低押分限制</summary>
        double FeverRedBonusArcadesMinBetLimit;
        /// <summary>代理商 魚機 紅包炒場最低押分限制</summary>
        double FeverRedBonusFishingMinBetLimit;

        /// <summary>代理商 老虎機 紅包炒場遊戲總次數門檻</summary>
        int FeverRedBonusSlotGamesLimit;
        /// <summary>代理商 押分機 紅包炒場遊戲總次數門檻</summary>
        int FeverRedBonusArcadesGamesLimit;
        /// <summary>代理商 魚機 紅包炒場遊戲總次數門檻</summary>
        int FeverRedBonusFishingGamesLimit;

        /// <summary>代理商 老虎機 遊戲連續玩X秒後出</summary>
        int FeverRedBonusSlotPlaySec;
        /// <summary>代理商 押分機 遊戲連續玩X秒後出</summary>
        int FeverRedBonusArcadesPlaySec;
        /// <summary>代理商 魚機 遊戲連續玩X秒後出</summary>
        int FeverRedBonusFishingPlaySec;

        /// <summary>代理商 老虎機 遊戲待機X秒後不出</summary>
        int FeverRedBonusSlotStopPlaySec;
        /// <summary>代理商 押分機 遊戲待機X秒後不出</summary>
        int FeverRedBonusArcadesStopPlaySec;
        /// <summary>代理商 魚機 遊戲待機X秒後不出</summary>
        int FeverRedBonusFishingStopPlaySec;

        /// <summary></summary>
        int FeverRedBonusFgBonus1;
        double FeverRedBonusFgBonus1Rate;
        int FeverRedBonusFgBonus2;
        double FeverRedBonusFgBonus2Rate;
        int FeverRedBonusFgBonus3;
        double FeverRedBonusFgBonus3Rate;
        int FeverRedBonusFgBonus4;
        double FeverRedBonusFgBonus4Rate;
        int FeverRedBonusFgBonus5;
        double FeverRedBonusFgBonus5Rate;
        int FeverRedBonusFgBonus6;
        double FeverRedBonusFgBonus6Rate;
        int FeverRedBonusFgBonus7;
        double FeverRedBonusFgBonus7Rate;
        int FeverRedBonusFgBonus8;
        double FeverRedBonusFgBonus8Rate;
        int FeverRedBonusFgBonus9;
        double FeverRedBonusFgBonus9Rate;
        int FeverRedBonusFgBonus10;
        double FeverRedBonusFgBonus10Rate;

        int FRBAvgOTimeMin;
        int FRBAvgOTimeMax;

        /// <summary>從DB讀取紅包炒場功能設定</summary>
        void FeverRedBonusGetDBData(Dictionary<string, string> datalist)
        {
            if (datalist.ContainsKey("FeverRedBonusFg"))
            {
                FeverRedBonusFg = Convert.ToBoolean(datalist["FeverRedBonusFg"]);
                FeverRedBonusPool = Convert.ToDouble(datalist["FeverRedBonusPool"]);

                FeverRedBonusIncidence_sec = Convert.ToInt32(datalist["FeverRedBonusIncidence_sec"]);
                PlayerDaysFeverRedBonusLimit = Convert.ToInt32(datalist["PlayerDaysFeverRedBonusLimit"]);

                FeverRedBonusSlotMinBetLimit = Convert.ToDouble(datalist["FeverRedBonusSlotMinBetLimit"]);
                FeverRedBonusArcadesMinBetLimit = Convert.ToDouble(datalist["FeverRedBonusArcadesMinBetLimit"]);
                FeverRedBonusFishingMinBetLimit = Convert.ToDouble(datalist["FeverRedBonusFishingMinBetLimit"]);

                FeverRedBonusSlotGamesLimit = Convert.ToInt32(datalist["FeverRedBonusSlotGamesLimit"]);
                FeverRedBonusArcadesGamesLimit = Convert.ToInt32(datalist["FeverRedBonusArcadesGamesLimit"]);
                FeverRedBonusFishingGamesLimit = Convert.ToInt32(datalist["FeverRedBonusFishingGamesLimit"]);

                FeverRedBonusSlotPlaySec = Convert.ToInt32(datalist["FeverRedBonusSlotPlaySec"]);
                FeverRedBonusArcadesPlaySec = Convert.ToInt32(datalist["FeverRedBonusArcadesPlaySec"]);
                FeverRedBonusFishingPlaySec = Convert.ToInt32(datalist["FeverRedBonusFishingPlaySec"]);

                FeverRedBonusSlotStopPlaySec = Convert.ToInt32(datalist["FeverRedBonusSlotStopPlaySec"]);
                FeverRedBonusArcadesStopPlaySec = Convert.ToInt32(datalist["FeverRedBonusArcadesStopPlaySec"]);
                FeverRedBonusFishingStopPlaySec = Convert.ToInt32(datalist["FeverRedBonusFishingStopPlaySec"]);

                FeverRedBonusFgBonus1 = Convert.ToInt32(datalist["FeverRedBonusFgBonus1"]);
                FeverRedBonusFgBonus1Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus1Rate"]);
                FeverRedBonusFgBonus2 = Convert.ToInt32(datalist["FeverRedBonusFgBonus2"]);
                FeverRedBonusFgBonus2Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus2Rate"]);
                FeverRedBonusFgBonus3 = Convert.ToInt32(datalist["FeverRedBonusFgBonus3"]);
                FeverRedBonusFgBonus3Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus3Rate"]);
                FeverRedBonusFgBonus4 = Convert.ToInt32(datalist["FeverRedBonusFgBonus4"]);
                FeverRedBonusFgBonus4Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus4Rate"]);
                FeverRedBonusFgBonus5 = Convert.ToInt32(datalist["FeverRedBonusFgBonus5"]);
                FeverRedBonusFgBonus5Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus5Rate"]);
                FeverRedBonusFgBonus6 = Convert.ToInt32(datalist["FeverRedBonusFgBonus6"]);
                FeverRedBonusFgBonus6Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus6Rate"]);
                FeverRedBonusFgBonus7 = Convert.ToInt32(datalist["FeverRedBonusFgBonus7"]);
                FeverRedBonusFgBonus7Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus7Rate"]);
                FeverRedBonusFgBonus8 = Convert.ToInt32(datalist["FeverRedBonusFgBonus8"]);
                FeverRedBonusFgBonus8Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus8Rate"]);
                FeverRedBonusFgBonus9 = Convert.ToInt32(datalist["FeverRedBonusFgBonus9"]);
                FeverRedBonusFgBonus9Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus9Rate"]);
                FeverRedBonusFgBonus10 = Convert.ToInt32(datalist["FeverRedBonusFgBonus10"]);
                FeverRedBonusFgBonus10Rate = Convert.ToDouble(datalist["FeverRedBonusFgBonus10Rate"]);

                //發放率
                if (FeverRedBonusIncidence_sec >= 10)
                {
                    int basSec = (int)((double)FeverRedBonusIncidence_sec * 0.25);
                    FRBAvgOTimeMin = FeverRedBonusIncidence_sec - basSec;
                    FRBAvgOTimeMax = FeverRedBonusIncidence_sec + basSec;
                }
                else
                {
                    FRBAvgOTimeMin = FeverRedBonusIncidence_sec;
                    FRBAvgOTimeMax = FeverRedBonusIncidence_sec;
                }


                RedHypeServer = CacheManeger.DecodeServerList(datalist["RedHypeServer"], $"[{Name}]紅包炒場");

                if (RedHypeServer != null && RedHypeServer.Count > 0) RedHypeServerFg = true;
                else                                                  RedHypeServerFg = false;

                //if (RedHypeServerFg) MyConsole.WriteLine($"[{Name}]RedHypeServerFg={RedHypeServerFg}");


                if (FRBnsDbuFg && Name == "JAFbar1") MyConsole.WriteLine($"紅包炒場[{Name}] 發放率={FRBAvgOTimeMin}<-({FeverRedBonusIncidence_sec})->{FRBAvgOTimeMax} 指定遊戲[{RedHypeServerFg}]");

                FREnvSelTRang = 0; //重建選牌表
                //MyConsole.WriteLine($"紅包炒場[{Name}] SW={FeverRedBonusFg} BetLimit={FeverRedBonusMinBetLimit}");
                return;
            }

            TurnOffFeverRedBonus();
        }

        /// <summary>取得最低押分限制</summary>
        public double GetFeverRedBonusMinBetLimit(GameTypeCode gameType)
        {
            if (gameType == GameTypeCode.Slot) return FeverRedBonusSlotMinBetLimit;
            if (gameType == GameTypeCode.Arcade) return FeverRedBonusArcadesMinBetLimit;
            if (gameType == GameTypeCode.Fish) return FeverRedBonusFishingMinBetLimit;
            return 9999999;
        }
        /// <summary>取得遊戲總次數門檻</summary>
        public int GetFeverRedBonusGamesLimit(GameTypeCode gameType)
        {
            if (gameType == GameTypeCode.Slot) return FeverRedBonusSlotGamesLimit;
            if (gameType == GameTypeCode.Arcade) return FeverRedBonusArcadesGamesLimit;
            if (gameType == GameTypeCode.Fish) return FeverRedBonusFishingGamesLimit;
            return 9999999;
        }
        /// <summary>取得連續玩幾秒後出的秒數</summary>
        public int GetFeverRedBonusPlayTimesLimit(GameTypeCode gameType)
        {
            if (gameType == GameTypeCode.Slot) return FeverRedBonusSlotPlaySec;
            if (gameType == GameTypeCode.Arcade) return FeverRedBonusArcadesPlaySec;
            if (gameType == GameTypeCode.Fish) return FeverRedBonusFishingPlaySec;
            return 9999999;
        }
        /// <summary>取得停機幾秒後不出的秒數</summary>
        public int GetFeverRedBonusStopTimesLimit(GameTypeCode gameType)
        {
            if (gameType == GameTypeCode.Slot) return FeverRedBonusSlotStopPlaySec;
            if (gameType == GameTypeCode.Arcade) return FeverRedBonusArcadesStopPlaySec;
            if (gameType == GameTypeCode.Fish) return FeverRedBonusFishingStopPlaySec;
            return 0;
        }

        /// <summary>取得發放率 下次發放秒數</summary>
        public int GetNextFeverRedBonusOutTimes()
        {
            if (FRBAvgOTimeMin >= FRBAvgOTimeMax)
            {
                return FRBAvgOTimeMin;
            }

            lock (rfEnvRnd)
            {
                int nextSec = rfEnvRnd.Next(FRBAvgOTimeMin, FRBAvgOTimeMax);
                return nextSec;
            }
        }

        /// <summary>紅包炒場功能 是否已開啟, 押分是否足夠</summary>
        public bool FeverRedBonusIsEnabled()
        {
            if (FeverRedBonusFg)
            {
                if (FeverRedBonusPool >= FREnvMinBonus) //水庫錢足夠
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>紅包炒場 檢查是否為指定遊戲</summary>
        public bool CheckServerCode(GameServerCode serverCode)
        {
            if (RedHypeServerFg)
            {
                if (RedHypeServer != null)
                {
                    if (!RedHypeServer.Contains(serverCode))
                    {
                        return false; //非指定遊戲
                    }
                }
            }
            return true;
        }

        /// <summary>取得要出的紅包值</summary>
        public double GetFeverRedEnvelope(double availPool)
        {
            if (FREnvNextBonus > 0 && FREnvNextBonus <= availPool)
            {
                return FREnvNextBonus;
            }

            if (availPool >= FREnvMinBonus)
            {
                //選出紅包值
                for (int i = 0; i < 10; i++)
                {
                    double rbonus = SelectFeverRedEnvelopeBonus();
                    if (rbonus <= availPool)
                    {
                        FREnvNextBonus = rbonus;
                        return FREnvNextBonus;
                    }
                }
            }

            //選不到
            FREnvNextBonus = 0;
            return 0;
        }
        /// <summary>紅包已出, 重設選牌機率</summary>
        public void RedEnvelopeOut()
        {
            FREnvNextBonus = 0; //0=下次重選
        }
        /// <summary>選擇要出的炒場紅包值</summary>
        public double SelectFeverRedEnvelopeBonus()
        {
            bool resortfg = false;
            if (FREnvSelTRang <= 0) //DB值重取時會設為-1
            {
                FREnvSelTRang = MakeFeverRedEnvelopeTotalRate();

                if (FREnvSelTRang <= 0) //WEB沒有設定任何紅包
                {
                    TurnOffFeverRedBonus();
                    return 0;
                }

                resortfg = true; //打亂紅包選取表

                if (FRBnsDbuFg) MyConsole.WriteLine($"    重建紅包炒場選牌表: 選牌率總範圍={FREnvSelTRang}%, 最小紅包值={FREnvMinBonus}幣");
            }

            double bonus = 0;

            if (FREnvSelTRang > 0 && FeverRedEnvelopeSelectTable.Count > 0)
            {
                int resortrnd;
                int selrange;
                lock (rfEnvRnd)
                {
                    resortrnd = rfEnvRnd.Next(50);
                    selrange = rfEnvRnd.Next(FREnvSelTRang);
                }

                if (resortrnd == 1 || resortfg)
                {
                    //重新打亂紅包選取表
                    int totcnt = FeverRedEnvelopeSelectTable.Count;
                    for (int i = 0; i < totcnt; i++)
                    {
                        int idx;
                        lock (rfEnvRnd) { idx = rfEnvRnd.Next(totcnt); }
                        var tmp = FeverRedEnvelopeSelectTable[i];
                        FeverRedEnvelopeSelectTable[i] = FeverRedEnvelopeSelectTable[idx];
                        FeverRedEnvelopeSelectTable[idx] = tmp;
                    }
                }

                int addrange = 0;
                foreach (var info in FeverRedEnvelopeSelectTable)
                {
                    addrange += info.BonusRate;
                    if (addrange > selrange)
                    {
                        bonus = info.Bonus;
                        if (FRBnsDbuFg) MyConsole.WriteLine($"選出下一次要出的炒場紅包:{bonus}, Rate={selrange}");
                        break;
                    }
                }
            }
            return bonus;
        }

        /// <summary>關閉紅包炒場功能</summary>
        void TurnOffFeverRedBonus()
        {
            FeverRedBonusFg = false; //關閉紅包炒場功能
            FeverRedEnvelopeSelectTable = null;
            FREnvSelTRang = 0;
            FREnvNextBonus = 0;
        }

        Random rfEnvRnd = new Random(Guid.NewGuid().GetHashCode());
        /// <summary>下一次要出的紅包值</summary>
        double FREnvNextBonus = 0;
        /// <summary>紅包選牌總Range</summary>
        int FREnvSelTRang = 0;
        /// <summary>WEB設定的最小紅包值</summary>
        public int FREnvMinBonus { get; private set; } = 10;
        /// <summary>摸彩箱彩球</summary>
        private class RedEnvelopeBox
        {
            public int BonusRate;
            public int Bonus;

            public RedEnvelopeBox(int rate, int bonus)
            {
                BonusRate = rate;
                Bonus = bonus;
            }
        }
        /// <summary>紅包炒場選牌表</summary>
        private List<RedEnvelopeBox> FeverRedEnvelopeSelectTable;
        /// <summary>建立紅包炒場選牌表</summary>
        int MakeFeverRedEnvelopeTotalRate()
        {
            FREnvMinBonus = 99999;
            FeverRedEnvelopeSelectTable = new List<RedEnvelopeBox>();
            int totRate = SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus1, FeverRedBonusFgBonus1Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus2, FeverRedBonusFgBonus2Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus3, FeverRedBonusFgBonus3Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus4, FeverRedBonusFgBonus4Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus5, FeverRedBonusFgBonus5Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus6, FeverRedBonusFgBonus6Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus7, FeverRedBonusFgBonus7Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus8, FeverRedBonusFgBonus8Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus9, FeverRedBonusFgBonus9Rate);
            totRate += SetFeverRedEnvelopeSelectTable(FeverRedBonusFgBonus10, FeverRedBonusFgBonus10Rate);
            return totRate;
        }
        int SetFeverRedEnvelopeSelectTable(int bonus, double rate)
        {
            //int bonusRate = (int)Math.Round((rate * 100d), 2);
            int bonusRate = (int)Math.Truncate(rate); //直接取整數值 (WEB設定 0~10000)
            if (bonusRate > 0 && bonus > 0)
            {
                FeverRedEnvelopeSelectTable.Add(new RedEnvelopeBox(bonusRate, bonus));

                if (FREnvMinBonus > bonus)
                {
                    FREnvMinBonus = bonus;
                }

                return bonusRate;
            }
            return 0;
        }






        class FRateBox
        {
            public int Min;
            public int Max;

            public FRateBox(int min, int max)
            {
                Min = min;
                Max = max;
            }
        }
        List<FRateBox> FRateBoxList;

        /// <summary>炒場請求次數</summary>
        int FRateAskCnt = 0;
        /// <summary>炒場出牌請求限制</summary>
        int FRateTagCnt = 0;

        /// <summary>出牌機率計算 (bet=玩家押分, hcnt=玩家連續未中次數)</summary>
        public bool FRateCalcProc(double bet, int hcnt)
        {
            FRateAskCnt++;

            if (FRateTagCnt <= 0)
            {
                FRateTagCnt = FRateGetNextTagCnt();
            }

            bool forcefg = false;

            if (forcefg || FRateTagCnt > 0 && FRateAskCnt > FRateTagCnt)
            {
                FRateAskCnt = 0;
                FRateTagCnt = 0;
                return true;
            }

            return false;
        }

        int FRateBoxCnt = 0;

        int FRateGetNextTagCnt()
        {
            int rateMin, rateMax, nextCnt;

            lock (rfEnvRnd)
            {
                nextCnt = rfEnvRnd.Next(50);
            }
            return nextCnt;
        }

        /// <summary>建立紅包炒場出牌率</summary>
        //void jhdbvhjdfvb()
        //{
        //    if (FeverRedBonusRate1 == FeverRedBonusRate2)
        //    {
        //        FRateBoxCnt = 0;
        //        return;
        //    }
        //
        //    int rateMin, rateMax;
        //    if (FeverRedBonusRate1 < FeverRedBonusRate2)
        //    {
        //        rateMin = FeverRedBonusRate1;
        //        rateMax = FeverRedBonusRate2;
        //    }
        //    else
        //    {
        //        rateMin = FeverRedBonusRate2;
        //        rateMax = FeverRedBonusRate1;
        //    }
        //
        //    int diff = rateMax - rateMin; //差距值
        //    int step = 0;
        //    if (diff > 750)      { step = 4; }
        //    else if (diff > 500) { step = 3; }
        //    else if (diff > 250) { step = 2; }
        //    else                 { step = 1; }
        //
        //    int stepBase = diff / step;
        //
        //    FRateBoxList = new List<FRateBox>();
        //
        //    int strV1 = rateMin;
        //    for (int i = 0; i < step; i++)
        //    {
        //        int strV2;
        //        if (i < step - 1) { strV2 = strV1 + stepBase;}
        //        else { strV2 = rateMax; }
        //
        //        FRateBoxList.Add(new FRateBox(strV1, strV2));
        //
        //        strV1 = strV2;
        //    }
        //
        //    FRateBoxCnt = step;
        //}
        #endregion


        #region 返水 //#250807
        const bool RebateDebugFg = true; //UNDONE: 返水 DEBUG
        /// <summary>返水開關(設定變更判斷用)(-1=未設定)</summary>
        int RebateFgSave = -1;
        /// <summary>返水開關</summary>
        public bool RebateFg { get; private set; }
        /// <summary>返水有效(未過期)</summary>
        public bool RebateValid { get; private set; }
        /// <summary>返水結算開關</summary>
        bool RebateSettlementFg;
        /// <summary>領獎有效(未過期)</summary>
        bool RebateClaimValid;
        /// <summary>返水活動起始日期</summary>
        public DateTime RebateEventDateSt { get; private set; }
        /// <summary>返水活動結束日期</summary>
        public DateTime RebateEventDateEnd { get; private set; }
        /// <summary>返水領獎期限</summary>
        public DateTime RebateClaimDateEnd { get; private set; }

        enum RebateActionEnum
        {
            /// <summary>未設定狀態</summary>
            None = 0,
            尚未開始,
            活動中,
            活動結束,
            領獎結束,
            已停止
        }
        /// <summary>返水程序狀態</summary>
        RebateActionEnum RebateAction = RebateActionEnum.None;

        /// <summary>返水結算方法(設定變更判斷用)(-1=未設定)</summary>
        int RebateSettlementTypeSave = -1;
        /// <summary>返水結算天數(設定變更判斷用)(-1=未設定)</summary>
        int RebateSettlementDaySave = -1;
        /// <summary>返水程序狀態(設定變更判斷用)(-1=未設定)</summary>
        int RebateActionSave = -1;
        /// <summary>返水結算方法(0=即時, 1=日, 2=週)</summary>
        public int RebateSettlementType { get; private set; }
        /// <summary>返水日結算</summary>
        public int RebateSettlementDate { get; private set; }
        /// <summary>返水週結算</summary>
        public int RebateSettlementWeek { get; private set; }
        /// <summary>返水結算日期</summary>
        public DateTime SettlementDateNext { get; private set; }

        /// <summary>返水有效投注量</summary>
        public bool RebateValidBets { get; private set; }
        /// <summary>返水退款開關</summary>
        //public bool RebateOut { get; private set; }
        /// <summary>返水H5遊戲開關</summary>
        //public bool RebateH5 { get; private set; }
        /// <summary>最小返水退分限制</summary>
        public double RebateMinOut { get; private set; }

        /// <summary>返水投注量範圍 [第1段]</summary>
        public int RebateRange1 { get; private set; }
        /// <summary>返水投注量範圍 [第2段]</summary>
        public int RebateRange2 { get; private set; }
        /// <summary>返水投注量範圍 [第3段]</summary>
        public int RebateRange3 { get; private set; }
        /// <summary>返水投注量範圍 [第4段]</summary>
        public int RebateRange4 { get; private set; }
        /// <summary>返水投注量範圍 [第5段]</summary>
        public int RebateRange5 { get; private set; }

        /// <summary>返水%值 [第1段]</summary>
        public double RebateValue1 { get; private set; }
        /// <summary>返水%值 [第2段]</summary>
        public double RebateValue2 { get; private set; }
        /// <summary>返水%值 [第3段]</summary>
        public double RebateValue3 { get; private set; }
        /// <summary>返水%值 [第4段]</summary>
        public double RebateValue4 { get; private set; }
        /// <summary>返水%值 [第5段]</summary>
        public double RebateValue5 { get; private set; }

        /// <summary>返水投注量%值 有變</summary>
        bool RebateRangeValueChangedFg = false;
        /// <summary>檢查返水投注量%值 是否有變</summary>
        public void CheckRebateSettingChange(EntityData oldEntity)
        {
            if (RebateRange1 != oldEntity.RebateRange1 ||
               RebateRange2 != oldEntity.RebateRange2 ||
               RebateRange3 != oldEntity.RebateRange3 ||
               RebateRange4 != oldEntity.RebateRange4 ||
               RebateRange5 != oldEntity.RebateRange5 ||
               RebateValue1 != oldEntity.RebateValue1 ||
               RebateValue2 != oldEntity.RebateValue2 ||
               RebateValue3 != oldEntity.RebateValue3 ||
               RebateValue4 != oldEntity.RebateValue4 ||
               RebateValue5 != oldEntity.RebateValue5)
            {
                RebateRangeValueChangedFg = true;
                Console.WriteLine($"代理商[{Name}] 返水投注量%值 設定有變更:{RebateValue1}");
            }
            else
            {
                RebateRangeValueChangedFg = false;
            }
        }

        /// <summary>從DB取得返水設定值</summary>
        void RebateGetDBData(Dictionary<string, string> datalist)
        {
            if (datalist.ContainsKey("RebateFg"))
            {
                RebateFg = Convert.ToBoolean(datalist["RebateFg"]);

                RebateEventDateSt = Convert.ToDateTime(datalist["RebateEventDateSt"]).Date; //只取日期
                RebateEventDateEnd = Convert.ToDateTime(datalist["RebateEventDateEnd"]).Date;
                RebateClaimDateEnd = Convert.ToDateTime(datalist["RebateClaimDateEnd"]).Date;

                RebateSettlementType = Convert.ToInt32(datalist["RebateSettlementType"]);
                RebateSettlementDate = Convert.ToInt32(datalist["RebateSettlementDate"]);
                RebateSettlementWeek = Convert.ToInt32(datalist["RebateSettlementWeek"]);

                if (RebateSettlementType == 1 && RebateSettlementDate < 1)
                {
                    Console.WriteLine($"代理商[{Name}] 返水结算週期 日設定值錯誤:{RebateSettlementDate}");
                    RebateSettlementDate = 1;
                }
                if (RebateSettlementType == 2 && (RebateSettlementWeek < 1 || RebateSettlementWeek > 7))
                {
                    Console.WriteLine($"代理商[{Name}] 返水结算週期 週設定值錯誤:{RebateSettlementWeek}");
                    if (RebateSettlementWeek < 1) RebateSettlementWeek = 1;
                    if (RebateSettlementWeek > 7) RebateSettlementWeek = 7;
                }

                RebateValidBets = Convert.ToBoolean(datalist["RebateValidBets"]);
                //RebateOut = Convert.ToBoolean(datalist["RebateOut"]);
                //RebateH5 = Convert.ToBoolean(datalist["RebateH5"]);
                RebateMinOut = Math.Round(Convert.ToDouble(datalist["RebateMinOut"]), 1); //(小數1位)

                RebateRange1 = Convert.ToInt32(datalist["RebateRange1"]);
                RebateRange2 = Convert.ToInt32(datalist["RebateRange2"]);
                RebateRange3 = Convert.ToInt32(datalist["RebateRange3"]);
                RebateRange4 = Convert.ToInt32(datalist["RebateRange4"]);
                RebateRange5 = Convert.ToInt32(datalist["RebateRange5"]);

                //取出返水%值 (小數2位)
                RebateValue1 = Math.Round(Convert.ToDouble(datalist["RebateValue1"]), 2);
                RebateValue2 = Math.Round(Convert.ToDouble(datalist["RebateValue2"]), 2);
                RebateValue3 = Math.Round(Convert.ToDouble(datalist["RebateValue3"]), 2);
                RebateValue4 = Math.Round(Convert.ToDouble(datalist["RebateValue4"]), 2);
                RebateValue5 = Math.Round(Convert.ToDouble(datalist["RebateValue5"]), 2);

                string saveData = datalist["SaveData"];
                ExtraRebateSaveData(saveData);

                if (RebateFg)
                {
                    CheckRebateValidDate(DateTime.Now);

                    if (RebateDebugFg)
                    {
                        string setType;
                        if (RebateSettlementType == 1) { setType = $"每{RebateSettlementDate}日結算"; }
                        else if (RebateSettlementType == 2) { setType = $"每週{RebateSettlementWeek}結算"; }
                        else { setType = "即時結算"; }

                        string validDate = RebateValid ? "" : "[已過期]";

                        MyConsole.WriteLine($"代理商[{Name}] 返水功能開啟: 結算方式={setType} {validDate} 下次結算日{SettlementDateNext.ToString("yyyy-MM-dd")}");
                        MyConsole.WriteLine($"    活動日期={RebateEventDateSt.ToString("yyyy-MM-dd")}{RebateEventStartTimeStr} 至 {RebateEventDateEnd.ToString("yyyy-MM-dd")}{RebateEventEndTimeStr}, 領獎期限={RebateClaimDateEnd.ToString("yyyy/MM/dd")}{RebateEventEndTimeStr}");
                        //MyConsole.WriteLine($"    有效投注開關={RebateValidBets}, 退款開關={RebateOut}, H5開關={RebateH5}, 最小退分={RebateMinOut}幣");
                        MyConsole.WriteLine($"    有效投注開關={RebateValidBets}, 最小退分={RebateMinOut}幣");
                        MyConsole.WriteLine($"    投注量 1[{RebateRange1},{RebateValue1}%], 2[{RebateRange2},{RebateValue2}%], 3[{RebateRange3},{RebateValue3}%], 4[{RebateRange4},{RebateValue4}%], 5[{RebateRange5},{RebateValue5}%]");
                    }
                    return;
                }
            }

            //關閉返水功能
            RebateFg = false;
            RebateValid = false; //設為已過期
        }

        /// <summary>製作返水SaveData儲存字串</summary>
        public string GetRebateSaveData()
        {
            RebateActionSave = (int)RebateAction;

            string info = "Rebate:" +
                    RebateFgSave + "," +
                    RebateSettlementTypeSave + "," +
                    RebateSettlementDaySave + "," +
                    SettlementDateNext.Date.ToString("yyyy-MM-dd") + "," +
                    RebateActionSave;
            return info;
        }
        /// <summary>取回返水SaveData資訊</summary>
        void ExtraRebateSaveData(string info)
        {
            if(info == null || info == "") return;

            string[] daL1 = info.Split(':');
            if (daL1.Length < 2 || daL1[0] != "Rebate") return;

            try
            {
                string[] daL2 = daL1[1].Split(',');
                if (daL2.Length < 4) return;

                RebateFgSave = Convert.ToInt32(daL2[0]);
                RebateSettlementTypeSave = Convert.ToInt32(daL2[1]);
                RebateSettlementDaySave = Convert.ToInt32(daL2[2]);
                SettlementDateNext = DateTime.ParseExact(daL2[3], "yyyy-MM-dd", null);

                if (daL2.Length >= 5)
                {
                    RebateActionSave = Convert.ToInt32(daL2[4]);
                }
                else
                {
                    RebateActionSave = 0;
                }

                //Console.WriteLine($"代理商[{Name}] SaveData取回: RebateFgSave={RebateFgSave}, RebateSettlementTypeSave={RebateSettlementTypeSave}, SettlementDateNext={SettlementDateNext.ToString("yyyy-MM-dd")}, RebateAction={RebateAction}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"代理商[{Name}] ExRebateSaveData:{ex}");
            }

        }

        /// <summary>返水狀態 有變更</summary>
        public bool RebateActionChanged()
        {
            if (RebateActionSave != -1 && RebateActionSave == (int)RebateAction)
            {
                return false; //沒有變更
            }
            RebateActionSave = (int)RebateAction;
            return true; //有變更
        }
        /// <summary>返水功能開關 設定有變更</summary>
        public bool RebateFgChanged()
        {
            int iRebateFg = Convert.ToInt32(RebateFg);
            //Check是否有變更
            if (RebateFgSave != -1 && RebateFgSave == iRebateFg)
            {
                return false; //沒有變更
            }
            RebateFgSave = iRebateFg;
            return true; //有變更
        }
        /// <summary>結算方法 設定有變更</summary>
        public bool RebateSettlementTypeChanged()
        {
            if (RebateSettlementTypeSave != -1 && RebateSettlementTypeSave == RebateSettlementType)
            {
                //方法未變更, 判斷天數是否有變更
                if (RebateSettlementType == 0) return false; //即時不須判斷

                int settleDay = -1;
                if (RebateSettlementType == 1) settleDay = RebateSettlementDate;
                if (RebateSettlementType == 2) settleDay = RebateSettlementWeek;

                if (RebateSettlementDaySave != -1 && RebateSettlementDaySave == settleDay)
                {
                    return false; //沒有變更
                }
                RebateSettlementDaySave = settleDay;
            }
            RebateSettlementTypeSave = RebateSettlementType;
            return true; //有變更
        }
        /// <summary>返水結算 投注量%值 設定有變更</summary>
        public bool RebateRangeValueChanged()
        {
            if (RebateRangeValueChangedFg)
            {
                RebateRangeValueChangedFg = false;
                return true;
            }
            return false;
        }
        /// <summary>返水結算處理旗號</summary>
        bool RebateSettlementXFg = false;
        /// <summary>返水結算處理旗號</summary>
        public bool IsRebateSettlementFg => RebateSettlementXFg;
        /// <summary>返水投注量%值重算旗號</summary>
        bool RebateRecountXFg = false;
        /// <summary>返水投注量%值重算旗號</summary>
        public bool IsRebateRecountFg => RebateRecountXFg;

        public void SetRebateSettlementFg()
        {
            RebateSettlementXFg = true;
        }
        public void SetRebateRecountFg()
        {
            RebateRecountXFg = true;
        }
        public void ResetRebateSettlementRecountFg()
        {
            RebateSettlementXFg = false;
            RebateRecountXFg = false;
        }

        public string RebateEventStartTimeStr { get; private set; } = " " + new TimeOnly(RebateHour, 0, 0).ToString("hh:mm:ss"); //活動開始時間
        public string RebateEventEndTimeStr { get; private set; } = " " + new TimeOnly(RebateHour - 1, 59, 59).ToString("hh:mm:ss"); //活動結束時間

        /// <summary>返水結算時間 (中午12:00)</summary>
        public const int RebateHour = 12; //活動結束時間, 中午12:00
        //public const int RebateHour = 10; //UNDONE: 活動結束時間, 上午10:00
        /// <summary>設定下次返水結算日</summary>
        public void SetNextSettlementDate(DateTime now)
        {
            if (!RebateValid) //活動已過期
            {
                RebateSettlementFg = false; //關閉結算功能 (沒有下次結算日)
                return;
            }

            //日結算
            if (RebateSettlementType == 1)
            {
                RebateSettlementFg = true;
                RebateSettlementDaySave = RebateSettlementDate;
                SettlementDateNext = now.Date.AddDays(RebateSettlementDate); //下次結算日
                if (RebateDebugFg) MyConsole.WriteLine($"代理商[{Name}] 設定(日)下次返水結算{RebateSettlementDate}日後: {SettlementDateNext.ToString("yyyy-MM-dd")}");
                return;
            }

            //周結算
            if (RebateSettlementType == 2)
            {
                RebateSettlementFg = true;
                RebateSettlementDaySave = RebateSettlementWeek;
                // 取得今天是星期幾 (1 = Monday, 7 = Sunday)
                int todayDayOfWeek = (int)now.DayOfWeek == 0 ? 7 : (int)now.DayOfWeek;
                // 計算到下週指定日的天數
                int daysUntil = (RebateSettlementWeek - todayDayOfWeek + 7) % 7;
                // 如果是今天是結算日，則加 7 天
                daysUntil = daysUntil == 0 ? 7 : daysUntil;

                SettlementDateNext = now.Date.AddDays(daysUntil); //下周結算日
                if (RebateDebugFg) MyConsole.WriteLine($"代理商[{Name}] 設定(週)下次返水結算{daysUntil}日後: {SettlementDateNext.ToString("yyyy-MM-dd")}");
                return;
            }

            //即時結算
            RebateSettlementFg = false;
        }

        /// <summary>Check活動期限</summary>
        public bool CheckRebateValidDate(DateTime now)
        {
            //重取EntityData時, 結算時, 都會呼叫此函式
            //取出 RebateEventDateEnd 日期, 並將時間設為中午12:00
            DateTime startDate = RebateEventDateSt.Date.AddHours(RebateHour);
            DateTime endDate = RebateEventDateEnd.Date.AddHours(RebateHour);
            DateTime claimEndDate = RebateClaimDateEnd.Date.AddHours(RebateHour);

            if (now < startDate)
            {
                RebateValid = false; //活動未開始
                RebateSettlementFg = false; //不結算
                RebateClaimValid = false; //不可領獎
                RebateAction = RebateActionEnum.尚未開始;
            }
            else
            {
                if (now < claimEndDate)
                {
                    if (now < endDate)
                    {
                        RebateValid = true; //活動已開始 有效
                        RebateSettlementFg = true;
                        RebateAction = RebateActionEnum.活動中;
                    }
                    else
                    {
                        RebateValid = false; //已過期
                        RebateAction = RebateActionEnum.活動結束;
                    }

                    RebateClaimValid = true; //領獎期限內
                }
                else
                {
                    RebateClaimValid = false; //已過領獎期限

                    if (RebateActionSave != (int)RebateActionEnum.已停止)
                    {
                        RebateAction = RebateActionEnum.領獎結束;
                    }
                    else
                    {
                        RebateAction = RebateActionEnum.已停止;
                    }
                }
            }

            return RebateValid;
        }
        /// <summary>在領獎期限內</summary>
        public bool CheckRebateClaimDate()
        {
            return RebateClaimValid;

            //DateTime endDate = RebateClaimDateEnd.Date.AddHours(RebateHour);
            //
            //if (now < endDate)
            //{
            //    return true; //有效
            //}
            //
            //return false;
        }
        /// <summary>返水功能是否啟用 且正在活動中</summary>
        public bool CheckRebateEventON()
        {
            if (RebateFg == false || RebateAction == RebateActionEnum.已停止 || RebateAction == RebateActionEnum.None)
            {
                return false;
            }
            return true;
        }
        /// <summary>返水活動已進行中</summary>
        public bool CheckRebateEventInAction()
        {
            if (RebateAction == EntityData.RebateActionEnum.尚未開始 || RebateAction == RebateActionEnum.已停止)
            {
                return false;
            }
            return true;
        }
        /// <summary>設定返水功能已全部結束</summary>
        public void SetRebateEventOff()
        {
            RebateAction = RebateActionEnum.已停止;
        }
        /// <summary>返水功能已啟用</summary>
        public bool CheckRebateFgSave()
        {
            if(RebateFgSave == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>現在是否可結算</summary>
        public bool CheckRebateSettlementDate(DateTime now)
        {
            if (!RebateSettlementFg) return false; //結算功能已關閉

            if (RebateSettlementType == 0) return false; //即時結算模式, 不用結算 (每次押分時就已結算)

            if (!RebateValid) return true; //活動已過期, 可立即結算最後一次

            //DateTime endDate = SettlementDateNext.Date.AddHours(RebateHour);
            DateTime endDate = SettlementDateNext.Date;
            if (now >= endDate)
            {
                return true; //結算日已到, 可結算
            }

            return false;
        }

        /// <summary>檢查Credit受限旗號</summary>
        public bool CheckCreditRestricted()
        {
            //if (!RebateOut || !RebateH5 || !RebateValidBets) //不可退款 或 不可玩H5遊戲 或 無效投注量
            //{
            //    return true; //受限
            //}
            return false; //不限 8/22改為->返水一律不限制
        }

        /// <summary>取得即時返水投注%數</summary>
        public double ImmedPers()
        {
            return RebateValue1;
        }

        /// <summary>取得投注量範圍%數</summary>
        public double ValidBetsLevelPers(double validBets)
        {
            //將validBets轉為整數, 直接捨去小數位數不要四捨五入
            int validBetsInt = (int)Math.Truncate(validBets);

            double targetPers = RebateValue1;
            if (validBetsInt <= RebateRange1) return RebateValue1;

            if (RebateValue2 > targetPers) targetPers = RebateValue2;
            if (validBetsInt <= RebateRange2)
            {
                return targetPers;
            }

            if (RebateValue3 > targetPers) targetPers = RebateValue3;
            if (validBetsInt <= RebateRange3)
            {
                return targetPers;
            }

            if (RebateValue4 > targetPers) targetPers = RebateValue4;
            if (validBetsInt <= RebateRange4)
            {
                return targetPers;
            }

            if (RebateValue5 > targetPers) return RebateValue5;
            return targetPers;
        }
        #endregion
    }
}
