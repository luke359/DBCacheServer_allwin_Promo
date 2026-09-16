using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DBCacheServer
{
    public class TreasureofAZTEC : CommonGame
    {
        /// <summary>建構時呼叫(設定遊戲各表格名稱)</summary>
        protected override void StartUp()
        {
            GameServerCode = GameServerCode.TreasureofAZTEC;
            NewTypeServer = true; //是無固定房間模式
            IndepPlay = true; //有獨立遊戲
            IndepGameBetMulti = 50; //獨立遊戲須押50倍押分(若資料庫有設定則會覆寫此值)
            GameNameCd = "阿兹特克秘宝"; //遊戲名稱(內部辨識用)(房間名)
            GameNameMsg = "TreasureofAZTEC"; //遊戲名稱(除錯訊息用)
            
            //多國語系名稱設定, 不知道的就用英文
            GameNameCht = "阿茲特克秘寶"; //遊戲名稱(繁體)
            GameNameChs = "阿兹特克秘宝"; //遊戲名稱(簡體)
            GameNameEng = "Treasure of AZTEC"; //遊戲名稱(英文)
            GameNameThi = "สมบัติของชาวแอซเท็ก"; //遊戲名稱(泰文)
            GameNameVie = "Kho báu Aztec"; //遊戲名稱(越南文)

            /// <summary>DB資料表_機器人模式</summary>
            DB_BotModeRule = "TreasureofAZTECBotModeRule";
            /// <summary>DB資料表_(外總帳)</summary>
            DB_AccountTable = "TreasureofAZTECAccountTable";
            /// <summary>DB資料表_(外日帳)</summary>
            DB_AccountDetailTable = "TreasureofAZTECAccountDetailTable";
            /// <summary>DB資料表_</summary>
            DB_BigWaterRecTable = "TreasureofAZTECBigWaterRecTable";
            /// <summary>DB資料表_</summary>
            DB_RafrLogTable = "TreasureofAZTECRafrLog";
            /// <summary>DB資料表_遊戲獨有設定</summary>
            DB_SettingTable = "TreasureofAZTECSettingTable";
        }

        
        /// <summary>機台帳目LocalCache (外總帳)</summary>
        Dictionary<int, List<TreasureofAZTECAccountData>> AccountDataUpdateDict; //每台機台有2份帳目
        /// <summary>機台帳目LocalCache (外日帳)</summary>
        Dictionary<int, List<TreasureofAZTECAccountData>> AccountDataDetailUpdateDict;
        

        /// <summary></summary>
        public TreasureofAZTEC() : base("")
        {
            AccountDataUpdateDict = new Dictionary<int, List<TreasureofAZTECAccountData>>();
            AccountDataDetailUpdateDict = new Dictionary<int, List<TreasureofAZTECAccountData>>();
        }


        #region 遊戲設定資訊
        /// <summary>獲取押分設定(押分驗證用)</summary>
        //public override VerifyBetInfo GetBetSetting(int roomNumber, int gameMode)
        //{
        //}

        /// <summary>獲取遊戲設定DB資料</summary>
        public override void GetAllDBGameSettingData()
        {
            GetGameDBGameSettingData<TreasureofAZTECSettingData>();
        }
        #endregion


        #region 外帳(總帳)
        /// <summary>從DB讀取總帳資料 至 LocalCache</summary>
        public override void GetDBAccountData()
        {
            GetDBAccData<TreasureofAZTECAccountData>(AccountDataUpdateDict);
        }
        /// <summary>檢查機台是否(存)有外帳資料</summary>
        /// <param name="machineUID"></param>
        public override void CheckAccountData(int machineUID)
        {
            CheckAccData<TreasureofAZTECAccountData>(machineUID, AccountDataUpdateDict);
        }
        /// <summary>更新一筆 外帳資料</summary>
        /// <param name="machineUID"></param>
        /// <param name="AccountType"></param>
        public override void UpdateAccountData(int machineUID, Dictionary<string, string> refdata)
        {
            lock (AccountDataUpdateDict)
            {
                if (AccountDataUpdateDict.TryGetValue(machineUID, out List<TreasureofAZTECAccountData> acclist))
                {
                    foreach (TreasureofAZTECAccountData temp in acclist)
                    {
                        temp.AccumulatecGameData(refdata);
                        AddAccountUpdate(machineUID);
                    }
                }
            }
        }
        /// <summary>歸零外帳資料 (mode:0=常規, 1=獨立買, 2=全部)</summary>
        /// <param name="machineUID">機號(0=全部機台)</param>
        /// <param name="accType0">內帳</param>
        /// <param name="accType1">外帳</param>
        /// <param name="mode">0=常規, 1=獨立買, 2=全部</param>
        public override void ClearAccountData(int machineUID, bool accType0, bool accType1, int mode = 0)
        {
            lock (AccountDataUpdateDict)
            {
                int defaultMachNum = DefaultMachNum();
                if (machineUID > defaultMachNum) //清指定機台
                {
                    if (AccountDataUpdateDict.TryGetValue(machineUID, out List<TreasureofAZTECAccountData> acclist))
                    {
                        foreach (TreasureofAZTECAccountData temp in acclist)
                        {
                            if ((temp.AccountType == 0 && accType0) || (temp.AccountType == 1 && accType1))
                            {
                                temp.ClearCache(mode);
                                AddAccountUpdate(machineUID);
                            }
                        }
                    }
                }
                else //清全部機台
                {
                    foreach (List<TreasureofAZTECAccountData> acclist in AccountDataUpdateDict.Values)
                    {
                        foreach (TreasureofAZTECAccountData temp in acclist)
                        {
                            if ((temp.AccountType == 0 && accType0) || (temp.AccountType == 1 && accType1))
                            {
                                temp.ClearCache(mode);
                                AddAccountUpdate(temp.MachineUID);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>歸零額外押注外帳資料</summary>
        /// <param name="machineUID">機號(-1=全部機台)</param>
        /// <param name="accType0">內帳</param>
        /// <param name="accType1">外帳</param>
        public override void ClearExPlayAccountData(int machineUID, bool accType0, bool accType1)
        {
            lock (AccountDataUpdateDict)
            {
                int defaultMachNum = DefaultMachNum();
                if (machineUID > defaultMachNum) //清指定機台
                {
                    if (AccountDataUpdateDict.TryGetValue(machineUID, out List<TreasureofAZTECAccountData> acclist))
                    {
                        foreach (TreasureofAZTECAccountData temp in acclist)
                        {
                            if ((temp.AccountType == 0 && accType0) || (temp.AccountType == 1 && accType1))
                            {
                                temp.ClearCacheGameExPlay();
                                AddAccountUpdate(machineUID);
                            }
                        }
                    }
                }
                else //清全部機台
                {
                    foreach (List<TreasureofAZTECAccountData> acclist in AccountDataUpdateDict.Values)
                    {
                        foreach (TreasureofAZTECAccountData temp in acclist)
                        {
                            if ((temp.AccountType == 0 && accType0) || (temp.AccountType == 1 && accType1))
                            {
                                temp.ClearCacheGameExPlay();
                                AddAccountUpdate(temp.MachineUID);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 外帳(日帳)
        /// <summary>從DB讀取日帳資料 至 LocalCache(盈餘校正用)</summary>
        public override void GetDBAccountDataDetail()
        {
            GetDBAccDataDetail<TreasureofAZTECAccountData>(AccountDataDetailUpdateDict);
        }
        /// <summary>更新AccountDetailData (Cache資料)</summary>
        public override void UpdateAccountDetailData(int machineUID, Dictionary<string, string> refdata)
        {
            lock (AccountDataDetailUpdateDict)
            {
                if (AccountDataDetailUpdateDict.TryGetValue(machineUID, out List<TreasureofAZTECAccountData> acclist))
                {
                    foreach (TreasureofAZTECAccountData temp in acclist)
                    {
                        temp.AccumulatecGameData(refdata);
                        AddAccountDetailUpdate(machineUID);
                    }
                }
            }
        }
        /// <summary>置換AccountDetailData (Cache資料)</summary>
        protected override void ReplaceAccountDetailData(int machineUID, DateTime chToday, int accuid1, int accuid2)
        {
            ReplaceAccDetailData<TreasureofAZTECAccountData>(machineUID, chToday, accuid1, accuid2, AccountDataDetailUpdateDict);
        }
        #endregion

        #region 帳目更新
        /// <summary>更新機台內外帳JP彩金出牌紀錄</summary>
        public override void UpdateGameAccountTableForJP(JpAward jPType, double jPBet, double jPWin, int machineUID, string date)
        {
            if (AccountDataUpdateDict.ContainsKey(machineUID))
            {
                foreach (TreasureofAZTECAccountData info in AccountDataUpdateDict[machineUID])
                {
                    //更新內存Cache
                    info.AccumulatecJPData(jPType, jPBet, jPWin);
                    //更新DB
                    info.update = true;
                    AddAccountUpdate(machineUID);
                }
            }

            if (AccountDataDetailUpdateDict.ContainsKey(machineUID))
            {
                foreach (TreasureofAZTECAccountData info in AccountDataDetailUpdateDict[machineUID])
                {
                    //更新內存Cache
                    info.AccumulatecJPData(jPType, jPBet, jPWin);
                    //更新DB
                    info.update = true;
                    AddAccountDetailUpdate(machineUID);
                }
            }
        }

        /// <summary>取出機台 總帳 更新列表</summary>
        protected override bool GetAccountDBUpdateData(List<OperTionDBBox> upDataList)
        {
            if(AccountDataUpdateList.Count == 0)
            {
                return false;
            }

            lock (AccountDataUpdateDict)
            {
                lock (AccountDataUpdateList)
                {
                    foreach (int machUid in AccountDataUpdateList)
                    {
                        if (AccountDataUpdateDict.TryGetValue(machUid, out List<TreasureofAZTECAccountData> acclist))
                        {
                            foreach (TreasureofAZTECAccountData d in acclist)
                            {
                                if (d.update)
                                {
                                    d.update = false;
                                    Dictionary<string, string> updatadata = d.GetUpdateData(false);
                                    OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.update1, DB_AccountTable, updatadata, "AccountUID", d.AccountUID.ToString());
                                    upDataList.Add(TempOperTionDBBox);
                                }
                            }
                        }
                    }
                    AccountDataUpdateList.Clear();
                }
            }
            return true;
        }
        /// <summary>取出機台 日帳 更新列表</summary>
        protected override bool GetAccountDetailDBUpdateData(List<OperTionDBBox> upDataList)
        {
            if(AccountDataDetailUpdateList.Count == 0)
            {
                return false;
            }

            lock (AccountDataDetailUpdateDict)
            {
                lock (AccountDataDetailUpdateList)
                {
                    foreach (int machUid in AccountDataDetailUpdateList)
                    {
                        if (AccountDataDetailUpdateDict.TryGetValue(machUid, out List<TreasureofAZTECAccountData> acclist))
                        {
                            foreach (TreasureofAZTECAccountData d in acclist)
                            {
                                d.update = false;
                                Dictionary<string, string> updatadata = d.GetUpdateData(true);
                                OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.update1, DB_AccountDetailTable, updatadata, "AccountUID", d.AccountUID.ToString());
                                upDataList.Add(TempOperTionDBBox);
                            }
                        }
                    }
                    AccountDataDetailUpdateList.Clear();
                }
            }
            return true;
        }

        /// <summary>將整批資訊寫入DB</summary>
        protected override int UpdataDBBoxs(MysqlTest test, List<OperTionDBBox> data)
        {
            //return test.TreasureofAZTECDetail(data);
            return test.GameAccount(data);
        }
        #endregion
    }
}
