using Org.BouncyCastle.Asn1.Pkcs;
using Protocol;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DBCacheServer
{
    public abstract class CommonGame
    {
        /// <summary>DB寫入錯誤次數限制</summary>
        const int AcUpdataDBBoxsErrorLimit = 50;
        /// <summary>GameServerCode</summary>
        public GameServerCode GameServerCode { get; protected set; }
        /// <summary>新版機率 無固定房間模式</summary>
        public bool NewTypeServer { get; protected set; }
        /// <summary>新版機率 Kios模式組數</summary>
        public int KiosModeCount { get; protected set; } = 5;
        /// <summary>有獨立買遊戲</summary>
        public bool IndepPlay { get; protected set; }
        /// <summary>獨立遊戲 啟動 押分倍數</summary>
        public int IndepGameBetMulti { get; protected set; }
        /// <summary>遊戲名稱(繁體)</summary>
        public string GameNameCht { get; protected set; }
        /// <summary>遊戲名稱(簡體)</summary>
        public string GameNameChs { get; protected set; }
        /// <summary>遊戲名稱(英文)</summary>
        public string GameNameEng { get; protected set; }
        /// <summary>遊戲名稱(泰文)</summary>
        public string GameNameThi { get; protected set; }
        /// <summary>遊戲名稱(越南文)</summary>
        public string GameNameVie { get; protected set; }

        /// <summary>遊戲名稱(內部用)(DB紀錄用)</summary>
        public string GameNameCd { get; protected set; }
        /// <summary>遊戲名稱(除錯訊息用)</summary>
        public string GameNameMsg { get; protected set; }

        //從MachineTableDefine讀取
        //public MachineData machineData;
        /// <summary>遊戲類型</summary>
        public GameTypeCode GameTypeCode { get; protected set; }
        /// <summary>房間基礎號碼(由MachineTableDefine設定)(機台起始號碼從加1開始)</summary>
        public int RoomBaseNumber { get; protected set; }
        /// <summary>房間總數(由MachineTableDefine設定)</summary>
        public int RoomTotalCount { get; protected set; }

        /// <summary>取得機台起始號碼</summary>
        public int MachineStartNumber => RoomBaseNumber + 1;
        /// <summary>取得機台結束號碼</summary>
        public int MachineStopNumber => RoomBaseNumber + RoomTotalCount;
        /// <summary>比對機台號碼是否在建置範圍</summary>
        //public bool CompareMachineNo(int machineNo)
        //{
        //    if (machineNo > RoomBaseNumber && machineNo <= (RoomBaseNumber + RoomTotalCount))
        //        return true;
        //    return false;
        //}

        /// <summary>DB資料表_機器人模式</summary>
        public string DB_BotModeRule { get; protected set; }
        /// <summary>DB資料表_外帳(總帳)</summary>
        public string DB_AccountTable { get; protected set; }
        /// <summary>DB資料表_外帳(日帳)</summary>
        public string DB_AccountDetailTable { get; protected set; }
        /// <summary>DB資料表_共用大水庫出牌紀錄</summary>
        public string DB_BigWaterRecTable { get; protected set; }
        /// <summary>DB資料表_機率起伏紀錄</summary>
        public string DB_RafrLogTable { get; protected set; }
        /// <summary>DB資料表_遊戲獨有設定(不一定每個遊戲都有)</summary>
        public string DB_SettingTable { get; protected set; }
        /// <summary>DB資料表_有無遊戲獨有設定</summary>
        public bool IsUniqueSetting { get; protected set; }

        /// <summary>Game Server 啟用旗號</summary>
        public bool Active { get; protected set; }

        /// <summary>機台共用設定</summary>
        public CommonGlobalSettingData GlobalSettingData;
        /// <summary>盈餘校正用帳目資料</summary>
        public Dictionary<int, List<CommonGameAccountRegulateData>> SurplusRegulateDataDict;
        /// <summary>各分機設定資料列表</summary>
        public List<BaseGameSetting> GameSettingDataList;

        /// <summary>待更新的機台列表 (外總帳)</summary>
        protected List<int> AccountDataUpdateList;
        /// <summary>待更新的機台列表 (外日帳)</summary>
        protected List<int> AccountDataDetailUpdateList;


        public CommonGame(string msg)
        {
            GlobalSettingData = new CommonGlobalSettingData();
            SurplusRegulateDataDict = new Dictionary<int, List<CommonGameAccountRegulateData>>();
            GameSettingDataList = new List<BaseGameSetting>();
            
            AccountDataUpdateList = new List<int>();
            AccountDataDetailUpdateList = new List<int>();

            NewTypeServer = false;
            IsUniqueSetting = false;
            IndepPlay = false;
            IndepGameBetMulti = 100; //這裡為init值, 若資料庫有設定則會覆寫此值

            StartUp();
        }


        /// <summary>在 CacheManeger 的 Init() 裡呼叫</summary>
        public void Init(CountrySettingData countrySetting)
        {
            KiosModeCount = countrySetting.KiosModeCount;

            GetDBMachineData();
            GetDBGlobalSetting();  //先Load GlobalSetting
            GetAllDBGameSettingData(); //再Load GameSetting
            GetDBAccountData();
            GetDBAccountDataDetail();
            Active = true;
        }
        /// <summary>不啟用 Server</summary>
        public void Disable()
        {
            Active = false;
        }
        /// <summary>設定Server類型(true=新版機率)</summary>
        public void SetServerType(bool serverType)
        {
            NewTypeServer = serverType;
        }
        /// <summary>設定獨立遊戲押分倍數</summary>
        public void SetIndepGameBetMulti(int multi)
        {
            IndepGameBetMulti = multi;
        }

        /// <summary>建構時呼叫</summary>
        protected abstract void StartUp(); //Construct Call
        /// <summary>從DB讀取外帳資料</summary>
        public abstract void GetDBAccountData();
        /// <summary>從DB讀取日帳資料(盈餘校正用)</summary>
        public abstract void GetDBAccountDataDetail();
        /// <summary>從DB讀取機台共用設定</summary>
        //public abstract void GetDBGlobalSetting();
        /// <summary>從DB讀取全部分機機台設定</summary>
        public abstract void GetAllDBGameSettingData();
        /// <summary>從DB讀取指定機台設定</summary>
        //public abstract void GetSingleDBGameSettingData(int MachineUID);
        /// <summary>獲取押分設定(押分驗證用)</summary>
        //public abstract VerifyBetInfo GetBetSetting(int roomIndex, int GameMode);
        /// <summary>獲取遊戲共用設定</summary>
        //public abstract List<Dictionary<string, string>> GetGlobalSetting();
        /// <summary>獲取遊戲設定</summary>
        //public abstract List<Dictionary<string, string>> GetGameSettingInfo(bool IsAll, int machineUID = 0);


        #region 全區設定
        /// <summary>從DB讀取 遊戲共用設定</summary>
        public void GetDBGlobalSetting()
        {
            var datalist = CacheManeger.GetDBCommonGlobalGameSetting(GameServerCode, GameTypeCode);

            if (datalist.Count > 0)
            {
                GlobalSettingData.GetDBData(datalist[0]);
            }
            else
            {
                Console.WriteLine($"    [{GameServerCode}]:讀取機台共用設定錯誤(#6101)!!");
            }
        }
        /// <summary>GameServer要求取得 遊戲共用設定</summary>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetGlobalSetting()
        {
            var datalist = CacheManeger.GetDBCommonGlobalGameSetting(GameServerCode, GameTypeCode); //直接從DB讀取

            GlobalSettingData.GetSettingExtra(datalist[0]);

            //List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>> { GlobalSettingData.GetSetting() };
            return datalist;
        }
        /// <summary></summary>
        protected void GetDBSettingDataError()
        {
            //Console.WriteLine($"GetDBGameSettingData Error!!  Server[{GameServerCode}]:遊戲設定DB資料讀取錯誤(#6110)");
            CacheManeger.GetDBGameSettingDataError(GameServerCode, "遊戲設定DB資料讀取錯誤(#6110)");
        }
        #endregion

        #region 單機設定
        /// <summary>從DB讀取全部機台設定, 並建立設定列表</summary>
        public void GetGameDBGameSettingData<T>() where T : BaseGameSetting, new()
        {
            if (RoomBaseNumber > 0)
            {
                (string sRange, List<Dictionary<string, string>> datalist) = GetAllServerSetting();

                var uniqueSettinglist = GetDBUniqueSetting();

                if (datalist.Count > 0)
                {
                    if (GameSettingDataList.Count > 0)
                    {
                        GameSettingDataList.Clear();
                    }

                    bool uniData = uniqueSettinglist != null && uniqueSettinglist.Count >= datalist.Count; //如果此遊戲有 特有的設定

                    if (NewTypeServer)
                    {
                        //MyConsole.WriteLine($"{GameServerCode}: 讀取新版無固定房間模式 遊戲設定資料共 {datalist.Count} 筆  開放={GlobalSettingData.NumberOfOpen}");
                        for (int i = 0; i < datalist.Count; i++)
                        {
                            int kiosMode = Convert.ToInt32(datalist[i]["KiosMode"]);
                            if (kiosMode >= 0) //-1是全區設定的資料, 不加入設定列表
                            {
                                T ndata = new();
                                ndata.GetDBData(kiosMode, datalist[i], this);

                                if (uniData)
                                {
                                    ndata.GetDBDataUnique(uniqueSettinglist[i]);
                                }
                                GameSettingDataList.Add(ndata);
                            }

                            //if (i >= GlobalSettingData.NumberOfOpen - 1) break;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < datalist.Count; i++)
                        {
                            int machineUID = Convert.ToInt32(datalist[i]["MachineUID"]);
                            if (machineUID > 0) //0是全區設定的資料, 不加入設定列表
                            {
                                T ndata = new();
                                ndata.GetDBData(machineUID, datalist[i], this);

                                if (uniData)
                                {
                                    ndata.GetDBDataUnique(uniqueSettinglist[i]);
                                }
                                GameSettingDataList.Add(ndata);
                            }

                            if (i >= GlobalSettingData.NumberOfOpen - 1) break;
                        }
                    }
                    return;
                }
            }
            GetDBSettingDataError();
        }
        /// <summary>從DB讀取指定機台設定, 並更新設定列表</summary>
        /// <param name="MachineUID"></param>
        public void GetSingleDBGameSettingData(int MachineUID)
        {
            (string sRange, List<Dictionary<string, string>> datalist) = GetOneServerSetting(MachineUID);

            var uniqueSettinglist = GetDBUniqueSetting();

            foreach (var temp in GameSettingDataList)
            {
                if (temp.MachineUID == MachineUID)
                {
                    temp.GetDBData(MachineUID, datalist[0], this);

                    if (uniqueSettinglist != null && uniqueSettinglist.Count > 0)  //如果此遊戲有 特有的設定
                    {
                        temp.GetDBDataUnique(uniqueSettinglist[0]);
                    }
                    break;
                }
            }
        }
        /// <summary>取得 GameSetting 列表 (Client端要求)</summary>
        public List<Dictionary<string, string>> GetGameSettingInfo(bool IsAll, int MachineUID = 0)
        {
            if (IsAll)
            {
                (string sRange, List<Dictionary<string, string>> datalist) = GetAllServerSetting();
                var uniqueSettinglist = GetDBUniqueSetting();
                bool uniData = uniqueSettinglist != null && uniqueSettinglist.Count >= datalist.Count; //如果此遊戲有 特有的設定

                if (NewTypeServer)
                {
                    if (datalist.Count >= GameSettingDataList.Count)
                    {
                        int setDataIdx = 0;
                        for (int i = 0; i < datalist.Count; i++)
                        {
                            int kiosMode = Convert.ToInt32(datalist[i]["KiosMode"]);
                            if (kiosMode >= 0) //-1是全區設定的資料, 沒有加入設定列表
                            {
                                GetCommGameExtraSetting(datalist[i]);

                                if (uniData)
                                {
                                    GameSettingDataList[setDataIdx].GetSettingUnique(NewTypeServer, datalist[i], uniqueSettinglist[i]);
                                }
                                setDataIdx++;
                            }
                        }
                    }
                }
                else
                {
                    if (datalist.Count >= GameSettingDataList.Count)
                    {
                        int setDataIdx = 0;
                        for (int i = 0; i < datalist.Count; i++)
                        {
                            int machineUID = Convert.ToInt32(datalist[i]["MachineUID"]);
                            if (machineUID > 0) //0是全區設定的資料, 沒有加入設定列表
                            {
                                GetCommGameExtraSetting(datalist[i]);

                                if (uniData)
                                {
                                    GameSettingDataList[setDataIdx].GetSettingUnique(NewTypeServer, datalist[i], uniqueSettinglist[i]);
                                }
                                setDataIdx++;
                            }
                        }
                    }
                }

                return datalist;
            }
            else
            {
                if (NewTypeServer)
                {
                    if (MachineUID > 10)
                    {
                        MachineUID = (MachineUID & 0x0f) - 1; //UNDONE: 取低4位當KiosMode (因為後台還是以MachineUID傳送, 故先轉換)
                    }
                }

                (string sRange, List<Dictionary<string, string>> datalist) = GetOneServerSetting(MachineUID);
                var uniqueSettinglist = GetDBUniqueSetting();

                for (int i = 0; i < GameSettingDataList.Count; i++)
                {
                    if (GameSettingDataList[i].MachineUID == MachineUID)
                    {
                        GetCommGameExtraSetting(datalist[0]);

                        if (uniqueSettinglist != null && uniqueSettinglist.Count > 0)  //如果此遊戲有 特有的設定
                        {
                            GameSettingDataList[i].GetSettingUnique(NewTypeServer, datalist[0], uniqueSettinglist[0]);
                        }
                        break;
                    }
                }

                return datalist;
            }
        }
        /// <summary>從DB資料庫讀取 遊戲特有的設定</summary>
        List<Dictionary<string, string>> GetDBUniqueSetting()
        {
            if (IsUniqueSetting)
            {
                if (NewTypeServer)
                {
                    string sRange = string.Format("KiosMode >= {0} && KiosMode <{1}", -1, KiosModeCount);
                    return MysqlAcess.GetInstance().select(DB_SettingTable + "Kios", "*", sRange, "KiosMode ASC");
                }
                else
                {
                    int StartMachine = MachineStartNumber;
                    int EndMachine = RoomBaseNumber + GlobalSettingData.NumberOfOpen; //只讀取開放台數
                    //int EndNumber = MachineStopNumber; //讀取總台數
                    string sRange = string.Format("MachineUID >= {0} && MachineUID <= {1}", StartMachine, EndMachine);
                    return MysqlAcess.GetInstance().select(DB_SettingTable, "*", sRange, "MachineUID ASC");
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>獲取押分設定(押分驗證用)</summary>
        //public virtual VerifyBetInfo GetBetSetting(int roomNumber, int gameMode)
        public VerifyBetInfo GetBetSetting(int roomNumber, int gameMode)
        {
            BaseGameSetting gameSettingData;

            string roomMode;
            int roomIdx = -1;
            if (NewTypeServer)
            {
                roomMode = "KiosMode";
                if (roomNumber >= 0 && roomNumber < GameSettingDataList.Count)
                {
                    roomIdx = roomNumber;
                }
            }
            else
            {
                roomMode = "RoomNumber";
                if (roomNumber > 0 && roomNumber <= GameSettingDataList.Count)
                {
                    roomIdx = roomNumber - 1;
                }
            }

            if (roomIdx >= 0)
            {
                gameSettingData = GameSettingDataList[roomIdx];

                if (gameMode != 1) //是常規遊戲
                {
                    return gameSettingData.BetInfo;
                }

                if (IndepPlay) //是獨立買遊戲, 且此遊戲有獨立買設定值
                {
                    return ((SlotCommonGameSetting)gameSettingData).IndepBetInfo; //SlotGame才有的
                }

                Console.WriteLine($"{GameServerCode}:VerifyBetInfo {roomMode}[{roomNumber}] GameMode Error!"); //此遊戲沒有獨立買
            }
            else
            {
                Console.WriteLine($"{GameServerCode}:VerifyBetInfo {roomMode}[{roomNumber}] Not Found!  TotalCount[{GameSettingDataList.Count}]"); //找不到此房間
            }

            return new VerifyBetInfo(100000, 100000, 0); //製作一組不會通過驗算的設定值
        }

        /// <summary>從DB取得全部Server遊戲設定</summary>
        /// <param name="StartNumber">Server起始號碼(0 or 1)</param>
        protected (string sRange, List<Dictionary<string, string>> datalist) GetAllServerSetting()
        {
            if (NewTypeServer)
            {
                int server = (int)GameServerCode;
                string sRange = string.Format("GameServerCode = {0}", server);
                var datalist = CacheManeger.GetDBKiosCommonGameSetting(sRange, GameServerCode, GameTypeCode); //直接從DB讀取
                return (sRange, datalist);
            }
            else
            {
                int StartMachine = MachineStartNumber;
                int EndMachine = RoomBaseNumber + GlobalSettingData.NumberOfOpen; //只讀取開放台數
                //int EndNumber = MachineStopNumber; //讀取總台數
                string sRange = string.Format("MachineUID >= {0} && MachineUID <= {1}", StartMachine, EndMachine);
                var datalist = CacheManeger.GetDBCommonGameSetting(sRange, GameServerCode, GameTypeCode); //直接從DB讀取
                return (sRange, datalist);
            }
        }
        /// <summary>從DB取得指定Server遊戲設定</summary>
        protected (string sRange, List<Dictionary<string, string>> datalist) GetOneServerSetting(int MachineUID)
        {
            if (NewTypeServer)
            {
                int server = (int)GameServerCode;
                string sRange = string.Format("GameServerCode = {0} && KiosMode = {1}", server, MachineUID);
                var datalist = CacheManeger.GetDBKiosCommonGameSetting(sRange, GameServerCode, GameTypeCode); //直接從DB讀取
                return (sRange, datalist);
            }
            else
            {
                string sRange = string.Format("MachineUID = {0}", MachineUID);
                var datalist = CacheManeger.GetDBCommonGameSetting(sRange, GameServerCode, GameTypeCode); //直接從DB讀取
                return (sRange, datalist);
            }
        }
        /// <summary>加入Common額外設定 (不在DB資料庫中的設定)</summary>
        protected void GetCommGameExtraSetting(Dictionary<string, string> data)
        {
            //if (IndepPlay) //為相容舊遊戲(沒有獨立買也會抓此欄位), 故將此Mask掉
            {
                if (!data.ContainsKey("IndepGameBetMulti")) //如果資料庫裡沒有此設定值, 就用預設值
                {
                    data.Add("IndepGameBetMulti", IndepGameBetMulti.ToString());
                }
            }
        }
        #endregion

        /// <summary>取得房間Machine設定</summary>
        public List<Dictionary<string, string>> GetMachineDataInfo()
        {
            List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();
            string gameType = CacheManeger.GameTypeCodeToGameTypeName(GameTypeCode);
            int machUid = MachineStartNumber;
            for (int i = 1; i <= RoomTotalCount; i++)
            {
                //int machIdx = info.Key % 1000;
                //string machName = GameServerCodeToGameName(info.Value.GameServerCode) + machIdx;
                //MyConsole.WriteLine("MachineUID=" + info.Key + ", Idx=" + machIdx + ", Name= " + machName); //UNDONE: 檢查MachineData

                Dictionary<string, string> tempdata = new()
                {
                    { "MachineUID", machUid.ToString() },
                    { "MachineName", GameNameCd + i },
                    { "GameName", GameNameCd },
                    { "GameType", gameType }
                };
                datalist.Add(tempdata);
                machUid++;
            }
            return datalist;
        }
        /// <summary>比對機台號碼是否在建置範圍</summary>
        public bool CompareMachineNo(int machineNo)
        {
            if (machineNo > RoomBaseNumber && machineNo <= MachineStopNumber)
                return true;
            return false;
        }


        /// <summary>更新 AccountDetailData (盈餘校正用資料)</summary>
        //public abstract int UpdateAccountDetailData(int machineUID, string date, Dictionary<string, string> refdata);
        /// <summary>更新 AccountDetailData (Cache資料)</summary>
        public abstract void UpdateAccountDetailData(int machineUID, Dictionary<string, string> refdata);

        /// <summary>檢查此機台是否(存)有外帳資料</summary>
        public abstract void CheckAccountData(int machineUID);
        /// <summary>更新一筆外帳資料</summary>
        public abstract void UpdateAccountData(int machineUID, Dictionary<string, string> refdata);

        /// <summary>更新機台內外帳JP彩金出牌紀錄</summary>
        public abstract void UpdateGameAccountTableForJP(JpAward jPType, double jPBet, double jPWin, int machineUID, string date);

        /// <summary>歸零額外押注外帳資料</summary>
        /// <param name="machineUID">機號(-1=全部機台)</param>
        /// <param name="accType0">內帳</param>
        /// <param name="accType1">外帳</param>
        public abstract void ClearExPlayAccountData(int machineUID, bool accType0, bool accType1);

        /// <summary>歸零AccountData (mode:0=常規, 1=獨立買, 2=全部)</summary>
        /// <param name="machineUID">機號(0=全部機台)</param>
        /// <param name="accType0">內帳</param>
        /// <param name="accType1">外帳</param>
        /// <param name="mode">0=常規, 1=獨立買, 2=全部</param>
        public abstract void ClearAccountData(int machineUID, bool accType0, bool accType1, int mode = 0);

        /// <summary>取出機台 總帳 更新列表</summary>
        protected abstract bool GetAccountDBUpdateData(List<OperTionDBBox> upDataList);
        /// <summary>取出機台 日帳 更新列表</summary>
        protected abstract bool GetAccountDetailDBUpdateData(List<OperTionDBBox> upDataList);
        /// <summary>將整批資訊寫入DB</summary>
        protected abstract int UpdataDBBoxs(MysqlTest test, List<OperTionDBBox> data);


        /// <summary>建立 機台號碼列表</summary>
        protected List<int> MakeMachineListTable()
        {
            int totCnt;
            int startNo;
            if (NewTypeServer)
            {
                startNo = 0; //Kios由0開始編號 (新Server模式的MachineUid是代表KiosMode) #251218
                totCnt = KiosModeCount; //無固定房間模式 只需建立Kios組別數的紀錄
            }
            else
            {
                startNo = MachineStartNumber;
                totCnt = GlobalSettingData.NumberOfOpen;
            }

            List<int> machList = new List<int>();
            for (int i = 0; i < totCnt; i++) { machList.Add(startNo++); }
            return machList;
        }
        /// <summary>從 機台號碼列表 找出索引位置</summary>
        int CheckMachineNo(int machNo, List<int> machList)
        {
            for (int i = 0; i < machList.Count; i++)
            {
                if (machList[i] == machNo)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>將 索引位置 的機台號碼清除(設為-1)</summary>
        protected bool RemoveMachineNoIndex(int index, List<int> machList)
        {
            if (index >= 0 && index < machList.Count)
            {
                machList[index] = -1; //設-1表示此機台已ok
                return true;
            }
            return false;
        }
        /// <summary>計算機台號碼列表還有的機台數量</summary>
        protected int CheckMachineTotalCount(List<int> machList)
        {
            int tcnt = 0;
            for (int i = 0; i < machList.Count; i++)
            {
                if (machList[i] >= 0)
                {
                    tcnt++;
                }
            }
            return tcnt;
        }


        /// <summary>獲取機器人押分規則資料</summary>
        public List<Dictionary<string, string>> GetDBBotRuleSetting()
        {
            if (DB_BotModeRule == null || DB_BotModeRule == string.Empty)
            {
                return new List<Dictionary<string, string>>();
            }

            MysqlAcess myAcess = MysqlAcess.GetInstance();
            List<Dictionary<string, string>> datalist = myAcess.select(DB_BotModeRule, "*");
            return datalist;
        }

        /// <summary>讀取 MachineTableDefine 機台資訊</summary>
        void GetDBMachineData()
        {
            MysqlAcess myAcess = MysqlAcess.GetInstance();
            var datalist = myAcess.select(CacheManeger.DB_MachineTableDefine, "*", String.Format("GameServerCode = {0}", (int)GameServerCode));

            if (datalist.Count > 0)
            {
                RoomTotalCount = Convert.ToInt32(datalist[0]["TotalCount"]);
                RoomBaseNumber = Convert.ToInt32(datalist[0]["RoomBaseNumber"]);
                GameTypeCode = CacheManeger.GameTypeNameToGameTypeCode(datalist[0]["GameType"]);

                //MyConsole.WriteLine("ServerCode=" + datalist[0]["GameServerCode"] + ", GameType=" + datalist[0]["GameType"] + ", TotalCount=" + RoomTotalCount + ", RoomBaseNum=" + RoomBaseNumber); //UNDONE: 讀取 MachineTableDefine 機台資訊
            }
            if (RoomTotalCount == 0 || RoomBaseNumber == 0)
            {
                Console.WriteLine($"    [{GameServerCode}]:MachineTable讀取錯誤(#6100)!! RoomBase={RoomBaseNumber}, TotCnt={RoomTotalCount}");
            }
        }


        /// <summary>從DB讀取總帳資料 至 LocalCache</summary>
        protected void GetDBAccData<T>(Dictionary<int, List<T>> accountDataDict) where T : CommonAccountData, new()
        {
            MysqlAcess myAcess = MysqlAcess.GetInstance();
            var datalist = myAcess.select(DB_AccountTable, "*");

            if (accountDataDict.Count > 0) { accountDataDict.Clear(); }

             List<int> theGameTotalMachList = MakeMachineListTable();

            if (datalist.Count > 0)
            {
                for (int i = 0; i < datalist.Count; i++)
                {
                    T accData = new();
                    accData.GetDBData(datalist[i], false);

                    if (accountDataDict.ContainsKey(accData.MachineUID))
                    {
                        accountDataDict[accData.MachineUID].Add(accData);
                    }
                    else
                    {
                        int midx = CheckMachineNo(accData.MachineUID, theGameTotalMachList);
                        if (midx >= 0)
                        {
                            List<T> tempList = new() { accData };
                            accountDataDict.Add(accData.MachineUID, tempList);

                            RemoveMachineNoIndex(midx, theGameTotalMachList);
                        }
                    }
                }
            }

            int rebuiltCnt = CheckMachineTotalCount(theGameTotalMachList);
            if (rebuiltCnt > 0)
            {
                MyConsole.WriteLine("    重建 " + GameNameMsg + " Account Table 台數:" + rebuiltCnt);

                for (int x = 0; x < theGameTotalMachList.Count; x++)
                {
                    if (theGameTotalMachList[x] >= 0)
                    {
                        int machineUID = theGameTotalMachList[x];
                        InsertAccountData(machineUID, 0, accountDataDict);
                        InsertAccountData(machineUID, 1, accountDataDict);
                        if (NewTypeServer) MyConsole.WriteLine("    重建 " + GameNameMsg + " Account Table KiosMode:" + machineUID);
                    }
                }
            }
        }
        /// <summary>檢查機台是否(存)有外帳資料</summary>
        /// <param name="machineUID"></param>
        protected void CheckAccData<T>(int machineUID, Dictionary<int, List<T>> accountDataDict) where T : CommonAccountData, new()
        {
            bool ishaveType0 = false;
            bool ishaveType1 = false;

            if (accountDataDict.TryGetValue(machineUID, out List<T> acclist))
            {
                foreach (T temp in acclist)
                {
                    if (temp.AccountType == 0)
                    {
                        ishaveType0 = true;
                    }
                    else if (temp.AccountType == 1)
                    {
                        ishaveType1 = true;
                    }
                }
            }

            if ((!ishaveType0))
            {
                InsertAccountData(machineUID, 0, accountDataDict);
            }

            if (!ishaveType1)
            {
                InsertAccountData(machineUID, 1, accountDataDict);
            }
        }
        /// <summary>插入一筆 外帳資料</summary>
        /// <param name="machineUID"></param>
        /// <param name="AccountType"></param>
        protected void InsertAccountData<T>(int machineUID, int AccountType, Dictionary<int, List<T>> accountDataDict) where T : CommonAccountData, new()
        {
            MysqlAcess myAcess = MysqlAcess.GetInstance();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("MachineUID", machineUID.ToString());
            data.Add("AccountType", AccountType.ToString());
            int uid = myAcess.insert(DB_AccountTable, data);

            T hwdata = new();
            hwdata.AccountUID = uid;
            hwdata.MachineUID = machineUID;
            hwdata.AccountType = AccountType;

            if (accountDataDict.ContainsKey(machineUID))
            {
                accountDataDict[machineUID].Add(hwdata);
            }
            else
            {
                List<T> tempList = new() { hwdata };
                accountDataDict.Add(machineUID, tempList);
            }
            AddAccountUpdate(machineUID);
        }


        /// <summary>從DB讀取日帳資料 至 LocalCache(盈餘校正用)</summary>
        protected void GetDBAccDataDetail<T>(Dictionary<int, List<T>> accountDataDict) where T : CommonAccountData, new()
        {
            MysqlAcess myAcess = MysqlAcess.GetInstance();
            var datalist = myAcess.select(DB_AccountDetailTable, "*", "", "RecDate ASC");
            if (datalist != null) {
                if (datalist.Count > 0)
                {
                    //取出Cache用資料(只取當日)
                    if (accountDataDict.Count > 0) accountDataDict.Clear();
                    List<int> theGameTotalMachList = MakeMachineListTable();
                    DateTime todat = DateTime.Today.Date;
                    for (int i = 0; i < datalist.Count; i++)
                    {
                        DateTime recDate = Convert.ToDateTime(datalist[i]["RecDate"]).Date;
                        //只有今日的Detail帳目會更新, 舊帳目不會再更新, 因此只須將今日的帳目Cache起來.
                        if (recDate == todat)
                        {
                            T temp = new();
                            temp.GetDBData(datalist[i], true);

                            if (accountDataDict.ContainsKey(temp.MachineUID))
                            {
                                accountDataDict[temp.MachineUID].Add(temp);
                            }
                            else
                            {
                                if (theGameTotalMachList.Contains(temp.MachineUID))
                                {
                                    List<T> tempList = new() { temp };
                                    accountDataDict.Add(temp.MachineUID, tempList);
                                }
                            }
                        }
                    }

                    //取出盈餘校正用資料
                    if (SurplusRegulateDataDict.Count > 0) SurplusRegulateDataDict.Clear();
                    for (int i = 0; i < datalist.Count; i++)
                    {
                        CommonGameAccountRegulateData temp = new();
                        temp.GetDBData(datalist[i]);

                        int machUid = temp.MachineUID;

                        if (SurplusRegulateDataDict.ContainsKey(machUid))
                        {
                            SurplusRegulateDataDict[machUid].Add(temp);
                        }
                        else
                        {
                            if (theGameTotalMachList.Contains(temp.MachineUID))
                            {
                                List<CommonGameAccountRegulateData> tempList = new() { temp };
                                SurplusRegulateDataDict.Add(machUid, tempList);
                            }
                        }
                    }
                }
            }
            
        }
        object DetailAccLock = new object();
        /// <summary>更新 SurplusRegulateDataDict (盈餘校正用資料)</summary>
        /// <param name="machineUID"></param>
        /// <param name="date"></param>
        /// <param name="refdata"></param>
        public int UpdateAccountDetailData(int machineUID, string date, Dictionary<string, string> refdata)
        {
            DateTime chToday = Convert.ToDateTime(date).Date; //比對用
            string dbToday = date.Replace('/', '-'); //寫入DB用'-'
            double betAdd = Convert.ToDouble(refdata["TotalBet"]);
            double winAdd = Convert.ToDouble(refdata["TotalWin"]);

            bool makeNewRecord = false;
            //Console.WriteLine("machineUID" + machineUID + ", Date=" + date);
            //1.找出 機號相同 且 日期相同 的紀錄
            //        若找到則更新Data.
            //        若找不到則
            //    2.找出 機號相同 的紀錄總數, 
            //            若數量不足則 新增2筆,
            //            若數量足夠則 找出最舊的2筆紀錄, 清除後寫入新Data.

            if (SurplusRegulateDataDict.ContainsKey(machineUID))
            {
                SurplusRegulateDataDict.TryGetValue(machineUID, out List<CommonGameAccountRegulateData> tempList);

                int upCount = 0, atype0 = 0, atype1 = 0;

                foreach (CommonGameAccountRegulateData xdata in tempList)
                {
                    if (xdata.RecDate == chToday)
                    {
                        //int result1 = DBLink.update(accountTable, udata, "AccountUID", xdata.AccountUID.ToString());
                        //if (result1 <= 0)
                        //{
                        //    return result1;
                        //}

                        xdata.AccountPlay(betAdd, winAdd);

                        upCount++;
                        if (xdata.AccountType == 0) atype0++;
                        else if (xdata.AccountType == 1) atype1++;
                    }
                }

                if (upCount < 2)
                {
                    //沒有今日帳, 用最舊的紀錄修改
                    //if (tempList.Count >= 5 * 2)  //UNDONE: 單日帳 先測試5筆 -------------- 
                    if (tempList.Count >= 30 * 2 * 3)  //外帳30筆, 內帳30筆, 保留3個月
                    {
                        MysqlTest myAcess = new MysqlTest();
                        //Console.WriteLine("修改舊紀錄");
                        var dataDic3 = myAcess.select(DB_AccountDetailTable, "AccountUID, RecDate", string.Format("MachineUID = {0}", machineUID), "RecDate ASC", ""); //找出並排序全部紀錄
                        if (dataDic3 == null)
                        {
                            return 0;
                        }
                        Dictionary<string, string> dataDic2 = new Dictionary<string, string>();
                        dataDic2.Add("MachineUID", machineUID.ToString());
                        dataDic2.Add("RecDate", "'" + dbToday + "'");
                        //選出第1筆最舊的紀錄並清除內容
                        dataDic2.Add("AccountUID", dataDic3[0]["AccountUID"]);
                        dataDic2.Add("AccountType", "0");
                        int result2 = myAcess.replace(DB_AccountDetailTable, dataDic2);
                        if (result2 <= 0)
                        {
                            return result2;
                        }
                        //選出第2筆最舊的紀錄並清除內容
                        dataDic2["AccountUID"] = dataDic3[1]["AccountUID"];
                        dataDic2["AccountType"] = "1";
                        //Clear DB紀錄
                        int result3 = myAcess.replace(DB_AccountDetailTable, dataDic2);
                        if (result3 <= 0)
                        {
                            return result3;
                        }
                        //Update DB紀錄
                        //int result4 = DBLink.update(accountTable, udata, string.Format("MachineUID = {0} AND RecDate = '{1}'", machineUID, dbToday));
                        //if (result4 <= 0)
                        //{
                        //    return result4;
                        //}

                        int accuid1 = Convert.ToInt32(dataDic3[0]["AccountUID"]);
                        int accuid2 = Convert.ToInt32(dataDic3[1]["AccountUID"]);

                        //Update Cache 紀錄
                        ReplaceAccountDetailData(machineUID, chToday, accuid1, accuid2);

                        //Update 盈餘校正用資料 紀錄
                        foreach (CommonGameAccountRegulateData xdata in tempList)
                        {
                            if (xdata.AccountUID == accuid1 || xdata.AccountUID == accuid2)
                            {
                                xdata.AccountPlay(betAdd, winAdd);
                                xdata.RecDate = chToday;
                            }
                        }

                        myAcess.CloseCoon();
                        myAcess = null;
                    }
                    else  //紀錄總數不足
                    {
                        makeNewRecord = true;
                        //Console.WriteLine("紀錄總數不足, 新增紀錄");
                    }
                }
            }
            else  //無此機號, 新增此機號Key值
            {
                List<CommonGameAccountRegulateData> tempList = new List<CommonGameAccountRegulateData>();
                SurplusRegulateDataDict.Add(machineUID, tempList);
                makeNewRecord = true;
                //Console.WriteLine("無此機號, 新增此機號Key值");
            }


            if (makeNewRecord)
            {
                MysqlTest myAcess = new MysqlTest();
                //MyConsole.WriteLine("創建新紀錄");
                Dictionary<string, string> dataDic2 = new();
                dataDic2.Add("MachineUID", machineUID.ToString());
                dataDic2.Add("RecDate", "'" + dbToday + "'");

                int accuid1, accuid2;
                lock (DetailAccLock) //鎖住Insert程序, 避免回傳AccUid錯誤. 2021/04/29
                {
                    dataDic2.Add("AccountType", "0");
                    accuid1 = myAcess.insert(DB_AccountDetailTable, dataDic2);
                    if (accuid1 <= 0)
                    {
                        return accuid1;
                    }

                    dataDic2["AccountType"] = "1";
                    accuid2 = myAcess.insert(DB_AccountDetailTable, dataDic2);
                    if (accuid2 <= 0)
                    {
                        return accuid2;
                    }
                }
                //Update DB紀錄
                //int result5 = DBLink.update(accountTable, udata, string.Format("MachineUID = {0} AND RecDate = '{1}'", machineUID, dbToday));
                //if (result5 <= 0)
                //{
                //    return result5;
                //}

                //Cache 加入新紀錄
                ReplaceAccountDetailData(machineUID, chToday, accuid1, accuid2);

                //盈餘校正用資料 加入新紀錄
                CommonGameAccountRegulateData tempAcc1 = new();
                CommonGameAccountRegulateData tempAcc2 = new();
                tempAcc1.AccountPlay(betAdd, winAdd);
                tempAcc2.AccountPlay(betAdd, winAdd);

                tempAcc1.AccountUID = accuid1;
                tempAcc2.AccountUID = accuid2;

                tempAcc1.RecDate = chToday;
                tempAcc2.RecDate = chToday;

                tempAcc1.MachineUID = machineUID;
                tempAcc2.MachineUID = machineUID;

                tempAcc1.AccountType = 0;
                tempAcc2.AccountType = 1;

                SurplusRegulateDataDict[machineUID].Add(tempAcc1);
                SurplusRegulateDataDict[machineUID].Add(tempAcc2);

                myAcess.CloseCoon();
                myAcess = null;
            }
            return 1;
        }
        /// <summary>置換AccountDetailData (Cache資料)</summary>
        protected abstract void ReplaceAccountDetailData(int machineUID, DateTime chToday, int accuid1, int accuid2);
        /// <summary>置換AccountDetailData (Cache資料)</summary>
        protected void ReplaceAccDetailData<T>(int machineUID, DateTime chToday, int accuid1, int accuid2, Dictionary<int, List<T>> accountDataDict) where T : CommonAccountData, new()
        {
            bool ac1 = false, ac2 = false;
            lock (accountDataDict)
            {
                if (accountDataDict.TryGetValue(machineUID, out List<T> values))
                {
                    foreach (T info in values)
                    {
                        info.RecDate = chToday;
                        info.ClearCache(2);

                        if (info.AccountType == 0)
                        {
                            info.AccountUID = accuid1;
                            ac1 = true;
                        }
                        else
                        {
                            info.AccountUID = accuid2;
                            ac2 = true;
                        }
                    }
                }
                else
                {
                    List<T> tempList = new();
                    accountDataDict.Add(machineUID, tempList);
                }

                if (!ac1) //Web內帳
                {
                    T temp = new()
                    {
                        AccountUID = accuid1,
                        MachineUID = machineUID,
                        RecDate = chToday,
                        AccountType = 0,
                        update = true
                    };
                    accountDataDict[machineUID].Add(temp);
                }
                if (!ac2) //Web外帳
                {
                    T temp = new()
                    {
                        AccountUID = accuid2,
                        MachineUID = machineUID,
                        RecDate = chToday,
                        AccountType = 1,
                        update = true
                    };
                    accountDataDict[machineUID].Add(temp);
                }
                AddAccountDetailUpdate(machineUID);
            }
        }


        /// <summary>將機台 總帳 加入待更新列表</summary>
        protected void AddAccountUpdate(int machineUID)
        {
            lock (AccountDataUpdateList)
            {
                if (!AccountDataUpdateList.Contains(machineUID))
                {
                    AccountDataUpdateList.Add(machineUID);
                }
            }
        }
        /// <summary>將機台 日帳 加入待更新列表</summary>
        protected void AddAccountDetailUpdate(int machineUID)
        {
            lock (AccountDataDetailUpdateList)
            {
                if (!AccountDataDetailUpdateList.Contains(machineUID))
                {
                    AccountDataDetailUpdateList.Add(machineUID);
                }
            }
        }

        /// <summary>WEB用資訊</summary>
        public int GetAccountDataUpdateListCount()
        {
            lock (AccountDataUpdateList)
            {
                return AccountDataUpdateList.Count;
            }
        }
        /// <summary>WEB用資訊</summary>
        public int GetAccountDetailDataUpdateListCount()
        {
            lock (AccountDataDetailUpdateList)
            {
                return AccountDataDetailUpdateList.Count;
            }
        }


        /// <summary>新增大水庫出牌紀錄</summary>
        public void InsertBigWaterRec(Dictionary<string, string> updateData)
        {
            InsertOperTionDBBox(updateData, DB_BigWaterRecTable);
        }
        /// <summary>更新機率起伏紀錄</summary>
        public void UpdateRAFRRecord(Dictionary<string, string> updateData)
        {
            if (DB_RafrLogTable != null && DB_RafrLogTable != string.Empty)
            {
                InsertOperTionDBBox(updateData, DB_RafrLogTable);
            }
        }
        /// <summary>新增一筆紀錄至寫入DB佇列</summary>
        protected void InsertOperTionDBBox(Dictionary<string, string> updateData, string tableName)
        {
            OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.insert, tableName, updateData);
            lock (Program.opertionDBBoxs)
            {
                Program.opertionDBBoxs.Add(TempOperTionDBBox);
            }
        }


        List<OperTionDBBox> AccountUpdataDBBoxs = new List<OperTionDBBox>();
        /// <summary>機台總帳 更新程序</summary>
        public void AccountUpdataDBBoxsProc(MysqlTest test, ref bool MySqlErrorFg, ref int ErrorCnt)
        {
            if (AccountUpdataDBBoxs.Count == 0)
            {
                if (GetAccountDBUpdateData(AccountUpdataDBBoxs) == false)
                {
                    return; //沒有更新資料
                }
            }

            if (AccountUpdataDBBoxs.Count > 0)
            {
                int result = 0;
                if (MySqlErrorFg == false)
                {
                    try
                    {
                        result = UpdataDBBoxs(test, AccountUpdataDBBoxs);

                        if (result < 0)
                        {
                            test.CloseCoon();
                            test = new MysqlTest();
                            result = UpdataDBBoxs(test, AccountUpdataDBBoxs);

                            if (result < 0)
                            {
                                ErrorCnt += 1;
                                if (ErrorCnt >= AcUpdataDBBoxsErrorLimit)
                                {
                                    MySqlErrorFg = true;

                                    Program.WriteDBWorkLog(AccountUpdataDBBoxs[0].TableName, test.sqlcontext);

                                    AccountUpdataDBBoxs.Clear();
                                }
                            }
                            else
                            {
                                ErrorCnt = 0;
                                //MyConsole.WriteLine(GameServerCode + " 機台總帳 更新完成 2:" + AccountUpdataDBBoxs.Count); //UNDONE:機台總帳 更新完成
                                AccountUpdataDBBoxs.Clear();
                            }
                        }
                        else
                        {
                            ErrorCnt = 0;
                            //MyConsole.WriteLine(GameServerCode + " 機台總帳 更新完成 1:" + AccountUpdataDBBoxs.Count); //UNDONE:機台總帳 更新完成
                            AccountUpdataDBBoxs.Clear();
                        }
                    }
                    catch (Exception e)
                    {
                        string log = "Exception:" + e.Message;
                        Console.WriteLine(log);

                        log = "result:" + result.ToString();
                        Console.WriteLine(log);

                        test.CloseCoon();
                        test = new MysqlTest();
                        result = UpdataDBBoxs(test, AccountUpdataDBBoxs);

                        if (result < 0)
                        {
                            ErrorCnt += 1;

                            if (ErrorCnt >= AcUpdataDBBoxsErrorLimit)
                            {
                                MySqlErrorFg = true;

                                Program.WriteDBWorkLog(AccountUpdataDBBoxs[0].TableName, test.sqlcontext);

                                AccountUpdataDBBoxs.Clear();
                            }
                        }
                        else
                        {
                            ErrorCnt = 0;
                            //MyConsole.WriteLine(GameServerCode + " 機台總帳 更新完成 3:" + AccountUpdataDBBoxs.Count); //UNDONE:機台總帳 更新完成
                            AccountUpdataDBBoxs.Clear();
                        }
                    }
                }
                else
                {
                    result = UpdataDBBoxs(test, AccountUpdataDBBoxs);

                    if (result < 0)
                    {
                        Program.WriteDBWorkLog(AccountUpdataDBBoxs[0].TableName, test.sqlcontext);
                    }

                    AccountUpdataDBBoxs.Clear();
                }
            }
        }

        List<OperTionDBBox> AccountDetailUpdataDBBoxs = new List<OperTionDBBox>();
        /// <summary>機台日帳 更新程序</summary>
        public void AccountDetailUpdataDBBoxsProc(MysqlTest test, ref bool MySqlErrorFg, ref int ErrorCnt)
        {
            if (AccountDetailUpdataDBBoxs.Count == 0)
            {
                if (GetAccountDetailDBUpdateData(AccountDetailUpdataDBBoxs) == false)
                {
                    return; //沒有更新資料
                }
            }

            if (AccountDetailUpdataDBBoxs.Count > 0)
            {
                int result = 0;
                if (MySqlErrorFg == false)
                {
                    try
                    {
                        result = UpdataDBBoxs(test, AccountDetailUpdataDBBoxs);

                        if (result < 0)
                        {
                            test.CloseCoon();
                            test = new MysqlTest();
                            result = UpdataDBBoxs(test, AccountDetailUpdataDBBoxs);

                            if (result < 0)
                            {
                                ErrorCnt += 1;

                                if (ErrorCnt >= AcUpdataDBBoxsErrorLimit)
                                {
                                    MySqlErrorFg = true;

                                    Program.WriteDBWorkLog(AccountDetailUpdataDBBoxs[0].TableName, test.sqlcontext);

                                    AccountDetailUpdataDBBoxs.Clear();
                                }
                            }
                            else
                            {
                                ErrorCnt = 0;
                                //MyConsole.WriteLine(GameServerCode + " 機台日帳 更新完成 2:" + AccountDetailUpdataDBBoxs.Count); //UNDONE:機台日帳 更新完成
                                AccountDetailUpdataDBBoxs.Clear();
                            }
                        }
                        else
                        {
                            ErrorCnt = 0;
                            //MyConsole.WriteLine(GameServerCode + " 機台日帳 更新完成 1:" + AccountDetailUpdataDBBoxs.Count); //UNDONE:機台日帳 更新完成
                            AccountDetailUpdataDBBoxs.Clear();
                        }
                    }
                    catch (Exception e)
                    {
                        string log = "Exception:" + e.Message;
                        Console.WriteLine(log);

                        log = "result:" + result.ToString();
                        Console.WriteLine(log);

                        test.CloseCoon();
                        test = new MysqlTest();
                        result = UpdataDBBoxs(test, AccountDetailUpdataDBBoxs);

                        if (result < 0)
                        {
                            ErrorCnt += 1;

                            if (ErrorCnt >= AcUpdataDBBoxsErrorLimit)
                            {
                                MySqlErrorFg = true;

                                Program.WriteDBWorkLog(AccountDetailUpdataDBBoxs[0].TableName, test.sqlcontext);

                                AccountDetailUpdataDBBoxs.Clear();
                            }
                        }
                        else
                        {
                            ErrorCnt = 0;
                            //MyConsole.WriteLine(GameServerCode + " 機台日帳 更新完成 3:" + AccountDetailUpdataDBBoxs.Count); //UNDONE:機台日帳 更新完成
                            AccountDetailUpdataDBBoxs.Clear();
                        }
                    }
                }
                else
                {
                    result = UpdataDBBoxs(test, AccountDetailUpdataDBBoxs);

                    if (result < 0)
                    {
                        Program.WriteDBWorkLog(AccountDetailUpdataDBBoxs[0].TableName, test.sqlcontext);
                    }

                    AccountDetailUpdataDBBoxs.Clear();
                }
            }
        }

        /// <summary>取得"內定機台"號碼</summary>
        public int DefaultMachNum()
        {
            int defaultMachNum = NewTypeServer ? -1 : 0;
            return defaultMachNum;
        }

        /// <summary>取得遊戲名稱 全部 語言資訊 (繁中|簡中|英文|越南文|泰文)</summary>
        public string GetAllName()
        {
            return $"{GameNameCht}|{GameNameChs}|{GameNameEng}|{GameNameVie}|{GameNameThi}";
        }
    }
}
