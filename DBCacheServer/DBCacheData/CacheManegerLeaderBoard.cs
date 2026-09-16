using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlX.XDevAPI.Relational;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBCacheServer
{
    public partial class CacheManeger
    {
        static bool RankDbfg = true; //排行榜除錯開關
        static bool RankDbfg2 = false; //排行榜除錯開關
        /// <summary>排行榜 取排名人數</summary>
        const int RankTopCount = 100; //排行榜人數
        /// <summary>排行榜 錢精度</summary>
        const int RankMoneyPrecision = 3;
        /// <summary>排行榜 倍率精度</summary>
        const int RankOddsPrecision = 2;
        /// <summary>排行榜 結算時間 (凌晨00:00)</summary>
        const int RankEventHour = 0; //活動結束時間, 凌晨00:00

        /// <summary>排行榜 取日期字串</summary>
        static string RankDateStr(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        #region 設定資訊
        class RankEventSetData
        {
            /// <summary>活動狀態 (0=無, 1=活動尚未開始, 2=活動中, 3=領獎期限中)</summary>
            public int Active = 0;
            /// <summary>活動期號 (0=無)</summary>
            public int EventNo = 0;
            /// <summary>活動名稱</summary>
            public string EventName { get; private set; }
            /// <summary>活動遊戲列表</summary>
            public string EventGameData { get; private set; }
            /// <summary>活動開始時間</summary>
            public DateTime EventDateSt { get; private set; }
            /// <summary>活動結束時間</summary>
            public DateTime EventDateEnd { get; private set; }
            /// <summary>活動領獎結束時間</summary>
            public DateTime ClaimDateEnd { get; private set; }
            /// <summary>參與活動的遊戲伺服器列表</summary>
            public List<GameServerCode> GameServerList { get; private set; }

            public RankEventSetData()
            {
                Clear();
            }

            /// <summary>從DB取回 排行榜 設定值</summary>
            public void ExtraSettingData(Dictionary<string, string> sdata)
            {
                try
                {
                    EventNo = int.Parse(sdata["EventNo"]);
                    EventDateSt = DateTime.Parse(sdata["EventDateSt"]).Date.AddHours(RankEventHour);
                    EventDateEnd = DateTime.Parse(sdata["EventDateEnd"]).Date.AddHours(RankEventHour);
                    ClaimDateEnd = DateTime.Parse(sdata["ClaimDateEnd"]).Date.AddHours(RankEventHour);

                    CheckVaildDateToActive(DateTime.Now);

                    if (Active <= 0) //活動設定Error
                    {
                        MyConsole.WriteLine("排行榜: DB目前排行榜活動設定有誤!");
                        return;
                    }

                    EventName = sdata["EventName"];
                    EventGameData = sdata["EventGameData"];
                    GameServerList = CacheManeger.DecodeServerList(EventGameData, "排行榜");
                }
                catch (Exception ex)
                {
                    MyConsole.WriteLine($"排行榜: DB取回設定錯誤: {ex.ToString()}");
                }
            }

            /// <summary>檢查活動期限 並設定狀態旗號</summary>
            public int CheckVaildDateToActive(DateTime now)
            {
                Active = CheckVaildDate(now);
                return Active;
            }
            /// <summary>檢查活動期限</summary>
            public int CheckVaildDate(DateTime now)
            {
                //TimeOnly SettlementTime = new TimeOnly(RankEventHour, 0, 0);
                //TimeOnly timeNow = TimeOnly.FromDateTime(now);
                //if (timeNow >= SettlementTime) //如果現在時間已經過了活動時間
                //{
                //}

                now = now.Date; //只比對日期部分

                if (now < EventDateSt)
                {
                    return 1; //活動尚未開始
                }
                else
                {
                    if (now <= EventDateEnd)
                    {
                        return 2; //活動中
                    }
                    else if (now <= ClaimDateEnd)
                    {
                        return 3; //領獎期限中
                    }
                }
                return 0; //無效期
            }

            /// <summary>晴除活動</summary>
            public void Clear()
            {
                Active = 0;
                EventNo = 0;
                EventName = "";
                GameServerList = null;
            }
        }
        #endregion


        #region 執行資訊
        /// <summary>排行榜 執行資訊</summary>
        class RankEventExecuteInfo
        {
            //#250924 排行榜 執行機
            #region 設定資訊
            /// <summary>排行榜 活動任務號碼</summary>
            public int TaskNo;

            /// <summary>排行榜 設定資訊</summary>
            public RankEventSetData SetInfo = new();

            /// <summary>活動狀態 (0=無, 1=活動尚未開始, 2=活動中, 3=領獎期限中)</summary>
            public int Active => SetInfo.Active;
            /// <summary>活動期號</summary>
            public int EventNo => SetInfo.EventNo;
            /// <summary>活動名稱</summary>
            public string EventName => SetInfo.EventName;
            /// <summary>活動遊戲列表</summary>
            public string EventGameData => SetInfo.EventGameData;
            /// <summary>活動開始日期</summary>
            public DateTime EventDateSt => SetInfo.EventDateSt;
            /// <summary>活動結束日期</summary>
            public DateTime EventDateEnd => SetInfo.EventDateEnd;
            /// <summary>活動領獎結束日期</summary>
            public DateTime ClaimDateEnd => SetInfo.ClaimDateEnd;

            /// <summary>取得 排行榜設定資訊</summary>
            public void GetRankEventInfo(ref Dictionary<string, string> extraInfo)
            {
                if (IsActive())
                {
                    extraInfo.Add("RankStat", "1"); //排行榜功能狀態

                    extraInfo.Add("RankEventDateSt", EventDateSt.ToString("yyyy/MM/dd")); //+ entity.RebateEventStartTimeStr); //
                    extraInfo.Add("RankEventDateEnd", EventDateEnd.ToString("yyyy/MM/dd")); //+ entity.RebateEventEndTimeStr); //
                    extraInfo.Add("RankClaimDateEnd", ClaimDateEnd.ToString("yyyy/MM/dd")); //+ entity.RebateEventEndTimeStr); //

                    extraInfo.Add("RankEventName", EventName); //活動名稱
                    extraInfo.Add("RankEventGameData", EventGameData); //活動遊戲列表
                    return;
                }

                NoRankEventInfo(ref extraInfo);
            }
            /// <summary>取得 排行榜設定資訊(無資訊)</summary>
            static public void NoRankEventInfo(ref Dictionary<string, string> extraInfo)
            {
                extraInfo.Add("RankStat", "0"); //排行榜功能狀態
            }
            #endregion

            #region 執行單元
            /// <summary>活動期號 (SaveData)</summary>
            public int EventNoSav { get; private set; } = -1;
            /// <summary>活動狀態 (設定變更判斷用)(-1=未設定, 0=無, 1=活動尚未開始, 2=活動中, 3=領獎期限中) (SaveData)</summary>
            public int ActiveSave { get; private set; } = -1;
            /// <summary>(新)活動 已重置 (SaveData)</summary>
            public int ResetFg = 0;
            /// <summary>活動 已結束並結算 (SaveData)</summary>
            public int EventEndFg = 0;

            /// <summary>每日紀錄 寫入旗號</summary>
            public bool DailyRecFg = false;
            /// <summary>當期排行榜 寫入旗號</summary>
            public bool HistoryRecFg = false;
            /// <summary>SaveData 寫入旗號</summary>
            public bool SaveDataFg = false;
            /// <summary>SaveData 不更新旗號</summary>
            public bool NoUpdateSaveDataFg = false;

            //static bool RankDbfg = true; //排行榜除錯開關

            //static public RankEventExecuteInfo(bool rankDbfg)
            //{
            //    //RankDbfg = rankDbfg; //排行榜除錯開關跟隨外部設定
            //}

            public RankEventExecuteInfo(int taskNo)
            {
                TaskNo = taskNo;
            }

            /// <summary>指定 排行榜 設定值</summary>
            public void SetRankEventInfo(RankEventSetData setInfo)
            {
                SetInfo = setInfo;

                if (Active <= 0) //沒有活動
                {
                    //if (RankDbfg) MyConsole.WriteLine("SetRankEventInfo: 目前沒有排行榜活動");
                    //CloseRankEvent();
                    return;
                }

                if (RankDbfg)
                {
                    string activeStr = Active == 1 ? "活動尚未開始" : (Active == 2 ? "活動中" : (Active == 3 ? "領獎期限中" : "無效"));
                    MyConsole.WriteLine($"目前排行榜[#{TaskNo}]活動  序號:{EventNo}  狀態:{activeStr}[{Active}]");
                    MyConsole.WriteLine($"    活動日期:{RankDateStr(EventDateSt)} ~ {RankDateStr(EventDateEnd)}, 領獎結束:{RankDateStr(ClaimDateEnd)}");
                    MyConsole.WriteLine($"    遊戲列表:{EventGameData}");
                }
            }

            /// <summary>排行榜 活動中或領獎中</summary>
            public bool IsActive()
            {
                if (Active == 2 || Active == 3)
                    return true;
                return false;
            }

            /// <summary>關閉排行榜活動</summary>
            public void CloseRankEvent()
            {
                if (SetInfo == null) SetInfo = new();
                else SetInfo.Clear();

                ResetFg = 0;
                EventEndFg = 0;
                SaveDataFg = true;
            }

            /// <summary>檢查活動期限</summary>
            public void CheckVaildDate(DateTime now)
            {
                int act = SetInfo.CheckVaildDateToActive(now);

                if (RankDbfg)
                {
                    string activeStr = act == 1 ? "活動尚未開始" : (act == 2 ? "活動中" : (act == 3 ? "領獎期限中" : "無效"));
                    MyConsole.WriteLine($"排行榜[#{TaskNo}]檢查活動序號[{EventNo}]期限: {activeStr}");
                }
            }

            /// <summary>檢查GameServer</summary>
            public bool CheckGameServer(GameServerCode gameServer)
            {
                if (SetInfo.GameServerList == null || SetInfo.GameServerList.Count <= 0) return true; //沒有設定遊戲伺服器, 則全部遊戲伺服器都可參加
                return SetInfo.GameServerList.Contains(gameServer);
            }

            const string SaveDataST = "TK";
            const string SaveDataS1 = "ENo";
            const string SaveDataS2 = "Act";
            const string SaveDataS3 = "Rst";
            const string SaveDataS4 = "End";
            /// <summary>保存SaveData</summary>
            public string GetUpdateSaveData()
            {
                if (!NoUpdateSaveDataFg)
                {
                    EventNoSav = EventNo;
                    ActiveSave = Active;
                }

                SaveDataFg = false;
                NoUpdateSaveDataFg = false;

                string savedata = $"{SaveDataST}={TaskNo};{SaveDataS1}={EventNoSav};{SaveDataS2}={ActiveSave};{SaveDataS3}={ResetFg};{SaveDataS4}={EventEndFg}";
                return savedata;
            }
            /// <summary>找出屬於我的SaveData</summary>
            string FindMyTaskSaveData(string info)
            {
                if (info != null && info != "")
                {
                    string[] task = info.Split('#');
                    for (int i = 0; i < task.Length; i++)
                    {
                        if (task[i].StartsWith($"{SaveDataST}={TaskNo}"))
                        {
                            return task[i]; //取出對應任務的SaveData內容
                        }
                    }
                }
                return "";
            }
            /// <summary>取回SaveData</summary>
            public void ExtractSaveData(string info)
            {
                EventNoSav = -1;
                ActiveSave = -1;
                ResetFg = 0;

                string myinfo = FindMyTaskSaveData(info);
                if (myinfo == "") return;

                string[] daL1 = myinfo.Split(';');
                if (daL1.Length <= 0) return;

                for (int i = 0; i < daL1.Length; i++)
                {
                    string[] datas = daL1[i].Split('=');
                    if (datas.Length == 2)
                    {
                        if (datas[0] == SaveDataS1)
                        {
                            if (int.TryParse(datas[1], out int eventnosav))
                            {
                                EventNoSav = eventnosav;
                            }
                        }
                        else if (datas[0] == SaveDataS2)
                        {
                            if (int.TryParse(datas[1], out int active))
                            {
                                ActiveSave = active;
                            }
                        }
                        else if (datas[0] == SaveDataS3)
                        {
                            if (int.TryParse(datas[1], out int resetfg))
                            {
                                ResetFg = resetfg;
                            }
                        }
                        else if (datas[0] == SaveDataS4)
                        {
                            if (int.TryParse(datas[1], out int eventEnd))
                            {
                                EventEndFg = eventEnd;
                            }
                        }
                    }
                }

                if (RankDbfg) MyConsole.WriteLine($"排行榜[#{TaskNo}]取回SaveData:{SaveDataS1}={EventNoSav}, {SaveDataS2}={ActiveSave}, {SaveDataS3}={ResetFg}, {SaveDataS4}={EventEndFg}");
            }
            #endregion

            #region 榜單List
            /// <summary>龍榜 榜單List</summary>
            public List<RankEventPlayer> RankEventListDragon;
            /// <summary>虎榜 榜單List</summary>
            public List<RankEventPlayer> RankEventListTiger;
            /// <summary>單局勝分榜 榜單List</summary>
            public List<RankEventPlayer> RankEventListTopWin;
            /// <summary>單局倍率榜 榜單List</summary>
            public List<RankEventPlayer> RankEventListTopOdds;
            /// <summary>日勝分榜 榜單List</summary>
            public List<RankEventPlayer> RankEventListDailyTopWin;
            /// <summary>日倍率榜 榜單List</summary>
            public List<RankEventPlayer> RankEventListDailyTopOdds;

            /// <summary>龍榜, 排序並取得歷史 累計最高總贏分</summary>
            List<RankEventPlayer> GetRankEventListDragon(int topCount)
            {
                lock (_relock)
                {
                    return RankEventPlayerList.Values
                        .Where(x => x.TotalWin > 0)
                        .OrderByDescending(x => x.TotalWin)
                        .Take(topCount)
                        .Select(x => x.DeepCopy())
                        .ToList();
                }
            }
            /// <summary>虎榜, 排序並取得歷史 累計最高總押分</summary>
            List<RankEventPlayer> GetRankEventListTiger(int topCount)
            {
                lock (_relock)
                {
                    return RankEventPlayerList.Values
                        .Where(x => x.TotalBet > 0)
                        .OrderByDescending(x => x.TotalBet)
                        .Take(topCount)
                        .Select(x => x.DeepCopy())
                        .ToList();
                }
            }
            /// <summary>單局勝分榜, 排序並取得歷史 單局最高贏分</summary>
            List<RankEventPlayer> GetRankEventListTopWin(int topCount)
            {
                lock (_relock)
                {
                    return RankEventPlayerList.Values
                        .Where(x => x.TopWin > 0)
                        .OrderByDescending(x => x.TopWin)
                        .Take(topCount)
                        .Select(x => x.DeepCopy())
                        .ToList();
                }
            }
            /// <summary>單局倍率榜, 排序並取得歷史 單局最高倍率</summary>
            List<RankEventPlayer> GetRankEventListTopOdds(int topCount)
            {
                lock (_relock)
                {
                    return RankEventPlayerList.Values
                        .Where(x => x.TopOdds > 0)
                        .OrderByDescending(x => x.TopOdds)
                        .Take(topCount)
                        .Select(x => x.DeepCopy())
                        .ToList();
                }
            }
            /// <summary>日勝分榜, 排序並取得本日 累計最高總贏分</summary>
            List<RankEventPlayer> GetRankEventListDailyTopWin(int topCount)
            {
                lock (_relock)
                {
                    return RankEventPlayerList.Values
                        .Where(x => x.DailyTopWin > 0)
                        .OrderByDescending(x => x.DailyTopWin)
                        .Take(topCount)
                        .Select(x => x.DeepCopy())
                        .ToList();
                }
            }
            /// <summary>日倍率榜, 排序並取得本日 單局最高倍率</summary>
            List<RankEventPlayer> GetRankEventListDailyTopOdds(int topCount)
            {
                lock (_relock)
                {
                    return RankEventPlayerList.Values
                        .Where(x => x.DailyTopOdds > 0)
                        .OrderByDescending(x => x.DailyTopOdds)
                        .Take(topCount)
                        .Select(x => x.DeepCopy())
                        .ToList();
                }
            }

            bool RankRenewFg = true;
            /// <summary>計算並取得全部排行榜單</summary>
            public bool GetAllRankEventList(bool forceRenew)
            {
                if (!forceRenew && !RankRenewFg) return false;

                if (RankEventPlayerList.Count <= 0)
                {
                    MyConsole.WriteLine($"刷新排行榜[#{TaskNo}] RefreshRankEventLists: 無排名紀錄可使用");

                    RankEventListDragon = null;
                    RankEventListTiger = null;
                    RankEventListTopWin = null;
                    RankEventListDailyTopWin = null;
                    RankEventListTopOdds = null;
                    lock (_relock)
                    {
                        RankRenewFg = false;
                    }
                    return true;
                }

                //MyConsole.WriteLine("刷新排行榜 RefreshRankEventLists");
                //int topCount = RankTopCount;
                int topCount = RankEventPlayerList.Count;
                RankEventListDragon = GetRankEventListDragon(topCount);
                RankEventListTiger = GetRankEventListTiger(topCount);
                RankEventListTopWin = GetRankEventListTopWin(topCount);
                RankEventListTopOdds = GetRankEventListTopOdds(topCount);
                RankEventListDailyTopWin = GetRankEventListDailyTopWin(topCount);
                RankEventListDailyTopOdds = GetRankEventListDailyTopOdds(topCount);

                lock (_relock)
                {
                    RankRenewFg = false;
                }
                return true;
            }
            #endregion

            #region 統計紀錄
            readonly object _relock = new object();
            /// <summary>當期 累計統計紀錄表</summary>
            public Dictionary<int, RankEventPlayerAcc> RankEventPlayerList = new();

            int RankEventPlayerListEventNo = -1;
            /// <summary>從DB取回玩家累計統計紀錄表 (指定期號)</summary>
            public void GetRankEventPlayerListFromDB(int eventNo = 0)
            {
                if(eventNo == 0) eventNo = EventNo;

                if (eventNo == 0)
                {
                    RankEventPlayerList.Clear();
                    return; //無效期號不取回
                }

                lock (_relock)
                {
                    if (RankEventPlayerListEventNo == eventNo)
                    {
                        if (RankEventPlayerList.Count > 0)
                        {
                            return; //活動期號相同, 且已取回過
                        }
                    }
                    else
                    {
                        if (RankEventPlayerList.Count > 0)
                        {
                            RankEventPlayerList.Clear();
                        }
                    }

                    var datalist2 = MysqlAcess.GetInstance().select("RankEventPlayerAccount", "*", $"EventNo={eventNo}");
                    if (datalist2.Count > 0)
                    {
                        foreach (var sdata in datalist2)
                        {
                            RankEventPlayerAcc rep = new();
                            rep.ExtractData(sdata);
                            if (!RankEventPlayerList.ContainsKey(rep.UserUid))
                            {
                                RankEventPlayerList.Add(rep.UserUid, rep);
                            }
                            else
                            {
                                MyConsole.WriteLine($"    GetRankEventFromDB Error: UserUid {rep.UserUid} already exists in RankEventPlayerList");
                            }
                        }
                        if (RankDbfg) MyConsole.WriteLine($"    DB取回玩家 排行榜[#{TaskNo}][{eventNo}期]統計紀錄表:{datalist2.Count}人");
                    }

                    RankEventPlayerListEventNo = eventNo;
                }
            }

            //↓↓↓↓↓ 除錯用, 完成後可刪除 ↓↓↓↓↓
            int RKDbfg1 = 10;
            int RKDbfg2 = 10;
            int RKDbfg3 = 10;
            public void RKDAdd()
            {
                RKDbfg1++;
                RKDbfg2++;
                RKDbfg3++;
            }

            int RERDbSrv = 0;
            int RERDbUser = 0;
            void RERDChk(int srv, int useruid)
            {
                if (RERDbSrv != srv || RERDbUser != useruid)
                {
                    RERDbSrv = srv;
                    RERDbUser = useruid;
                    RERDbFg = -1;
                }
            }
            int RERDbFg = -1;
            void RankEventRecordDbShow(string msg, int no)
            {
                //if(RERDbFg != no)
                {
                    RERDbFg = no;
                    if (RankDbfg2) MyConsole.WriteLine(msg);
                }
            }
            //↑↑↑↑↑ 除錯用, 完成後可刪除 ↑↑↑↑↑

            /// <summary>排行榜 玩家統計紀錄</summary>
            public void RankEventRecord(EntityData entity, UserData userData, double win, double bet, GameServerCode gameServer)
            {
                //#250924 排行榜 玩家統計紀錄
                //判斷排行榜活動是否 啟動且在有效期
                //if (RankEventExecuteInfo.IsActive() == false)
                if (Active != 2)
                {
                    if (RankDbfg)
                    {
                        if (RKDbfg1 > 1)
                        {
                            RKDbfg1 = 0;
                            //RERDChk((int)gameServer, RankEventExecuteInfo.EventNo);
                            if (Active == 3)
                            {
                                RankEventRecordDbShow($"排行榜[#{TaskNo}]統計紀錄[{userData.UserID}]: 活動已截止", 1);
                            }
                            else
                            {
                                RankEventRecordDbShow($"排行榜[#{TaskNo}]統計紀錄[{userData.UserID}]: 活動無效", 2);
                            }
                        }
                    }
                    return;
                }

                //判斷Entity是否 有參與排行榜活動
                if (entity == null || !entity.RankingFg)
                {
                    if (RKDbfg2 > 1)
                    {
                        RKDbfg2 = 0;
                        //RERDChk((int)gameServer, entity.EntityId);
                        if (RankDbfg) RankEventRecordDbShow($"排行榜[#{TaskNo}]統計紀錄[{userData.UserID}]: 代理[{entity.Name}]活動無效", 3);
                    }
                    return;
                }

                //判斷gameServer是否 在排行榜遊戲列表
                if (CheckGameServer(gameServer) == false)
                {
                    if (RKDbfg3 > 1)
                    {
                        RKDbfg3 = 0;
                        //RERDChk((int)gameServer, entity.EntityId);
                        //RERDChk((int)gameServer, userData.UserUID);
                        if (RankDbfg) RankEventRecordDbShow($"排行榜[#{TaskNo}]統計紀錄[{userData.UserID}]: 遊戲[{gameServer}]活動無效", 4);
                    }
                    return;
                }

                //string gameName = gameServer.ToString();
                int entityUid = entity.EntityId;
                int userUid = userData.UserUID;
                string userName = userData.UserID;
                double odds = bet != 0 ? Math.Round(win / bet, RankOddsPrecision) : 0; //倍率 = 贏分 / 押分

                lock (_relock)
                {
                    if (RankEventPlayerList.ContainsKey(userUid))
                    {
                        RankEventPlayerList[userUid].Record(bet, win, odds, gameServer);
                    }
                    else
                    {
                        //此玩家尚未有紀錄，新增一筆
                        RankEventPlayerList.Add(userUid, new RankEventPlayerAcc(entityUid, userUid, userName, bet, win, odds, (int)gameServer));
                    }

                    RankRenewFg = true;
                }
            }

            /// <summary>每日紀錄重置</summary>
            public void ResetRankEventDailyRecord()
            {
                lock (_relock)
                {
                    foreach (var player in RankEventPlayerList.Values)
                    {
                        player.ClearDaily();
                    }
                }
            }

            /// <summary>取得玩家統計資料更新資訊</summary>
            public void GetAllPlayerUpdateData(ref List<string> valueList)
            {
                lock (_relock)
                {
                    foreach (var player in RankEventPlayerList.Values)
                    {
                        if (!player.UpdateFg) continue;
                        valueList.Add(player.GetUpdateDataVal(EventNo));
                        player.UpdateFg = false;
                    }
                }
            }
            #endregion
        }
        /// <summary>排行榜 執行列表</summary>
        List<RankEventExecuteInfo> RankEventExec = new() 
        { new RankEventExecuteInfo(0), 
          new RankEventExecuteInfo(1),
          new RankEventExecuteInfo(2),};

        /// <summary>從DB取得排行榜設定資訊</summary>
        RankEventSetData GetRankEventFromDB(int taskNo)
        {
            string today = RankDateStr(DateTime.Now);
            //var datalist = myAcess.select("RankEventSettingTable", "*", "", "EventNo DESC", "1");
            //從DB RankEventSettingTable 表格取得排行榜活動設定, 取得未過期且最早的那一筆資料即可
            var datalist = myAcess.select("RankEventSettingTable",
                                          "*",
                                          $"TaskNo = {taskNo} AND (DATE(EventDateEnd) >= '{today}' OR DATE(ClaimDateEnd) >= '{today}')",
                                          "EventDateSt ASC", "1");
            //"DATE(EventDateEnd) >= @today OR DATE(ClaimDateEnd) >= @today",

            RankEventSetData rankEventInfo = new();

            if (datalist.Count <= 0)
            {
                if (RankDbfg) MyConsole.WriteLine($"★ GetRankEventFromDB: 沒有排行榜[#{taskNo}]活動設定!");
                return rankEventInfo;
            }

            rankEventInfo.ExtraSettingData(datalist[0]);
            return rankEventInfo;
        }

        /// <summary>保存任務狀態(SaveData)至DB</summary>
        void UpdateRankEventSaveData()
        {
            //bool updateFg = false;
            //foreach (var rankExec in RankEventExec) { if (rankExec.SaveDataFg) updateFg = true; }
            //if (!updateFg) return; //沒有任一組需要更新資料

            //有一組要更新 就必須全部更新
            List<string> updateData = new List<string>();
            foreach (var rankExec in RankEventExec)
            {
                updateData.Add(rankExec.GetUpdateSaveData());
            }
            string savedata = string.Join("#", updateData);
            SaveDBCacheData(DBSaveType.RankEventSetting, savedata);
        }
        /// <summary>從DB取回任務狀態(SaveData)</summary>
        void GetRankEventSaveData()
        {
            bool updateFg = false;
            foreach (var rankExec in RankEventExec) { if (rankExec.EventNoSav == -1) updateFg = true; }
            if (!updateFg) return; //已取回過就不再取回

            string info = LoadDBCacheData(DBSaveType.RankEventSetting);

            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.EventNoSav == -1)
                {
                    rankExec.ExtractSaveData(info);
                }
            }
        }

        /// <summary>排行榜程序重新設定(開機 &amp; WEB變更 時呼叫)</summary>
        void RankEventProcInit()
        {
            GetRankEventSaveData();

            foreach (var rankExec in RankEventExec)
            {
                RankEventProcInit(rankExec);
            }
            
            UpdateRankEventSaveData();
            // 日榜紀錄 寫入DB
            DailyHistoryRecord();
            // 當期排行榜 寫入歷史紀錄
            RankStageHistoryRecord();

            int actCnt = ResetRankRefreshTask();
            if (actCnt > 0)
            {
                ScheduleRankDailyTask();
            }
            else
            {
                StopRankDailyTask();
            }
        }
        void RankEventProcInit(RankEventExecuteInfo rankExec)
        {
            int taskNo = rankExec.TaskNo;
            RankEventSetData rankEventInfo = GetRankEventFromDB(taskNo);
            rankExec.SetRankEventInfo(rankEventInfo);

            bool accountFg = false; //需結算 旗號
            bool clearAccFg = false; //需清除統計紀錄表 旗號
            string accmsg = "";
            string clrmsg = "";

            //先判斷活動期號是否相同
            if (rankEventInfo.EventNo != rankExec.EventNoSav) //活動期號不相同
            {
                //取回 RankEventPlayerAccount 前期玩家累計統計紀錄表
                rankExec.GetRankEventPlayerListFromDB(rankExec.EventNoSav);

                //UNDONE: 前期活動若為 "無活動" 時, 尚未處理

                if (rankExec.ActiveSave == 2 && rankExec.EventEndFg == 0)  //前次活動已開始且尚未結算
                {
                    accountFg = true; //需結算
                    accmsg = $"排行榜[#{taskNo}] 結算 前期({rankExec.EventNoSav})玩家排行!";
                }

                if (rankEventInfo.Active > 0)
                {
                    clearAccFg = true; //需清除統計紀錄表
                    clrmsg = $"排行榜[#{taskNo}]新活動期({rankEventInfo.EventNo})號已變更, 重置 前期({rankExec.EventNoSav})玩家累計統計紀錄表!";
                }
            }
            else //活動期號相同 (同一期)
            {
                //取得 RankEventPlayerAccount 當期玩家累計統計紀錄表
                rankExec.GetRankEventPlayerListFromDB();

                if (rankEventInfo.Active != rankExec.ActiveSave) //狀態不相同
                {
                    if (rankEventInfo.Active > rankExec.ActiveSave) //狀態前進
                    {
                        if (rankExec.ActiveSave == 2 && rankExec.EventEndFg == 0)  //活動已開始且尚未結算
                        {
                            accountFg = true; //需結算
                            accmsg = $"排行榜[#{taskNo}] 結算 本期({rankExec.EventNoSav})玩家排行!";
                        }
                    }
                    else //狀態倒退(不合理的): 統計表保持原狀, 不做任何處理, 直接停止活動. (等待人為介入修正)
                    {
                        rankEventInfo.Active = 0; //停止活動
                        rankExec.NoUpdateSaveDataFg = true;
                        MyConsole.WriteLine($"    RankEventProcInit: 排行榜[#{taskNo}]活動設定 !錯誤! [{rankEventInfo.Active}]<[{rankExec.ActiveSave}]");
                        return;
                    }
                }
            }

            if (accountFg) //需結算
            {
                if (RankDbfg) MyConsole.WriteLine("    " + accmsg);
                // 先刷新一次排行榜
                rankExec.GetAllRankEventList(true);
                // 清空今日日榜統計資料
                rankExec.ResetRankEventDailyRecord();
                // 日榜紀錄 寫入DB
                rankExec.DailyRecFg = true;
                // 當期排行榜 寫入歷史紀錄
                rankExec.HistoryRecFg = true;
                rankExec.EventEndFg = 1;
            }

            if (clearAccFg) //需清除統計紀錄表
            {
                //歸零統計紀錄表
                if (RankDbfg) MyConsole.WriteLine("    " + clrmsg);
                ClearAllRankEventPlayerAccount(taskNo);
                rankExec.RankEventPlayerList.Clear();
                rankExec.ResetFg = 1;
            }

            if (rankEventInfo.Active <= 0)
            {
                if (RankDbfg) MyConsole.WriteLine($"    RankEventProcInit: 排行榜[#{taskNo}]沒有活動設定 -> 關閉活動!");
                rankExec.CloseRankEvent();
                rankExec.RankEventPlayerList.Clear();
                //StopRankRefreshTask();
                //StopRankDailyTask();
                return;
            }

            rankExec.SaveDataFg = true;

            //判斷排行榜是否 啟用且在有效期
            if (rankExec.IsActive())
            {
                //取得 RankEventPlayerAccount 當期玩家累計統計紀錄表
                rankExec.GetRankEventPlayerListFromDB();

                //if (rankExec.Active == 2)
                //{
                //    ScheduleRankRefreshTask();
                //}

                //ScheduleRankDailyTask();
                if (RankDbfg) MyConsole.WriteLine($"    RankEventProcInit: 排行榜[#{taskNo}]活動程序已啟動");
            }
            else if (rankExec.Active == 1)
            {
                //ScheduleRankDailyTask();
                if (RankDbfg) MyConsole.WriteLine($"    RankEventProcInit: 排行榜[#{taskNo}]活動尚未開始!");
            }
        }
        /// <summary>判斷是否有新的排行榜活動</summary>
        bool CheckNewRankEvent(RankEventExecuteInfo rankExec)
        {
            int oldEventNo = rankExec.EventNo;

            var rankEventInfo = GetRankEventFromDB(rankExec.TaskNo);
            if (rankEventInfo.Active == 0 && rankEventInfo.EventNo == 0 && rankEventInfo.EventNo == oldEventNo)
            {
                //if (RankDbfg) 
                    MyConsole.WriteLine("CheckNewRankEvent: 沒有新的排行榜活動!");
                return false;
            }

            //if (RankDbfg) 
                MyConsole.WriteLine($"CheckNewRankEvent: 發現有新的排行榜[#{rankExec.TaskNo}]活動, 載入執行程序");
            rankExec.SetRankEventInfo(rankEventInfo);

            ClearAllRankEventPlayerAccount(rankExec.TaskNo);
            rankExec.RankEventPlayerList.Clear();
            rankExec.ResetFg = 1;

            if (rankExec.IsActive())
            {
                //取得 RankEventPlayerAccount 當期玩家累計統計紀錄表
                rankExec.GetRankEventPlayerListFromDB();
            }

            //if (RankDbfg) MyConsole.WriteLine("    新的排行榜活動程序已啟動");
            return true;
        }
        #endregion


        #region 統計紀錄
        /// <summary>玩家榜單紀錄結構</summary>
        class RankEventPlayer
        {
            #region 欄位
            /// <summary>玩家Uid (DB主key)</summary>
            public int UserUid;
            /// <summary>代理商Uid</summary>
            public int EntityUid;
            /// <summary>玩家名稱</summary>
            public string UserName;

            /// <summary>總贏分</summary>
            public double TotalWin;
            /// <summary>總押分</summary>
            public double TotalBet;

            /// <summary>單局最高贏分</summary>
            public double TopWin;
            /// <summary>單局最高贏分的押分</summary>
            public double TopWinBet;
            /// <summary>單局最高贏分 遊戲名稱</summary>
            public int TopWinGame;

            /// <summary>單局最高倍率</summary>
            public double TopOdds;
            /// <summary>單局最高倍率 的押分</summary>
            public double TopOddsBet;
            /// <summary>單局最高倍率 的贏分</summary>
            public double TopOddsWin;
            /// <summary>單局最高倍率 遊戲名稱</summary>
            public int TopOddsGame;

            /// <summary>單日最高贏分</summary>
            public double DailyTopWin;
            /// <summary>單日押分累計</summary>
            public double DailyTopBet;

            /// <summary>單日最高倍率</summary>
            public double DailyTopOdds;
            /// <summary>單日最高倍率 的押分</summary>
            public double DailyTopOddsBet;
            /// <summary>單日最高倍率 的贏分</summary>
            public double DailyTopOddsWin;
            /// <summary>單日最高倍率 遊戲名稱</summary>
            public int DailyTopOddsGame;

            /// <summary>更新旗號</summary>
            public bool UpdateFg;
            #endregion

            #region 公用函式
            /// <summary>從DB取回值</summary>
            public void ExtractData(Dictionary<string, string> sdata)
            {
                try
                {
                    UserUid = int.Parse(sdata[nameof(UserUid)]);
                    EntityUid = int.Parse(sdata[nameof(EntityUid)]);
                    UserName = sdata[nameof(UserName)];

                    TotalWin = Math.Round(double.Parse(sdata[nameof(TotalWin)]), RankMoneyPrecision);
                    TotalBet = Math.Round(double.Parse(sdata[nameof(TotalBet)]), RankMoneyPrecision);

                    TopWin = Math.Round(double.Parse(sdata[nameof(TopWin)]), RankMoneyPrecision);
                    TopWinBet = Math.Round(double.Parse(sdata[nameof(TopWinBet)]), RankMoneyPrecision);
                    TopWinGame = int.Parse(sdata[nameof(TopWinGame)]);

                    TopOdds = Math.Round(double.Parse(sdata[nameof(TopOdds)]), RankOddsPrecision);
                    TopOddsBet = Math.Round(double.Parse(sdata[nameof(TopOddsBet)]), RankMoneyPrecision);
                    TopOddsWin = Math.Round(double.Parse(sdata[nameof(TopOddsWin)]), RankMoneyPrecision);
                    TopOddsGame = int.Parse(sdata[nameof(TopOddsGame)]);

                    DailyTopWin = Math.Round(double.Parse(sdata[nameof(DailyTopWin)]), RankMoneyPrecision);
                    DailyTopBet = Math.Round(double.Parse(sdata[nameof(DailyTopBet)]), RankMoneyPrecision);

                    DailyTopOdds = Math.Round(double.Parse(sdata[nameof(DailyTopOdds)]), RankOddsPrecision);
                    DailyTopOddsBet = Math.Round(double.Parse(sdata[nameof(DailyTopOddsBet)]), RankMoneyPrecision);
                    DailyTopOddsWin = Math.Round(double.Parse(sdata[nameof(DailyTopOddsWin)]), RankMoneyPrecision);
                    DailyTopOddsGame = int.Parse(sdata[nameof(DailyTopOddsGame)]);
                }
                catch (Exception ex)
                {
                    MyConsole.WriteLine($"RankEventPlayer ExtraData Error: {ex.ToString()}");
                }
            }

            /// <summary>深拷貝 RankEventPlayer</summary>
            public RankEventPlayer DeepCopy()
            {
                return new RankEventPlayer
                {
                    EntityUid = this.EntityUid,
                    UserUid = this.UserUid,
                    UserName = this.UserName,

                    TotalWin = this.TotalWin,
                    TotalBet = this.TotalBet,

                    TopWin = this.TopWin,
                    TopWinBet = this.TopWinBet,
                    TopWinGame = this.TopWinGame,

                    TopOdds = this.TopOdds,
                    TopOddsBet = this.TopOddsBet,
                    TopOddsWin = this.TopOddsWin,
                    TopOddsGame = this.TopOddsGame,

                    DailyTopWin = this.DailyTopWin,
                    DailyTopBet = this.DailyTopBet,

                    DailyTopOdds = this.DailyTopOdds,
                    DailyTopOddsBet = this.DailyTopOddsBet,
                    DailyTopOddsWin = this.DailyTopOddsWin,
                    DailyTopOddsGame = this.DailyTopOddsGame
                };
            }

            /// <summary>清除每日最高</summary>
            public void ClearDaily()
            {
                DailyTopWin = 0;
                DailyTopBet = 0;
                DailyTopOdds = 0;
                DailyTopOddsBet = 0;
                DailyTopOddsWin = 0;
                DailyTopOddsGame = 0;
                UpdateFg = true;
            }
            #endregion

            #region 批次更新 玩家統計 欄位 (注意各函式欄位順序需相同)
            /// <summary>取得批次更新 欄位名稱</summary>
            static public string GetUpdateDataCol()
            {
                string updata = "EventNo," +
                                nameof(EntityUid) + "," +
                                nameof(UserUid) + "," +
                                nameof(UserName) + "," +

                                nameof(TotalWin) + "," +
                                nameof(TotalBet) + "," +

                                nameof(TopWin) + "," +
                                nameof(TopWinBet) + "," +
                                nameof(TopWinGame) + "," +

                                nameof(TopOdds) + "," +
                                nameof(TopOddsBet) + "," +
                                nameof(TopOddsWin) + "," +
                                nameof(TopOddsGame) + "," +

                                nameof(DailyTopWin) + "," +
                                nameof(DailyTopBet) + "," +

                                nameof(DailyTopOdds) + "," +
                                nameof(DailyTopOddsBet) + "," +
                                nameof(DailyTopOddsWin) + "," +
                                nameof(DailyTopOddsGame);
                return updata;
            }
            /// <summary>取得批次更新 欄位值</summary>
            public string GetUpdateDataVal(int eventNo)
            {
                string updata = "(" + eventNo.ToString() + "," +
                                EntityUid.ToString() + "," +
                                UserUid.ToString() + ",'" + UserName + "'," +

                                Math.Round(TotalWin, RankMoneyPrecision).ToString() + "," +
                                Math.Round(TotalBet, RankMoneyPrecision).ToString() + "," +

                                Math.Round(TopWin, RankMoneyPrecision).ToString() + "," +
                                Math.Round(TopWinBet, RankMoneyPrecision).ToString() + "," +
                                TopWinGame.ToString() + "," +

                                Math.Round(TopOdds, RankOddsPrecision).ToString() + "," +
                                Math.Round(TopOddsBet, RankMoneyPrecision).ToString() + "," +
                                Math.Round(TopOddsWin, RankMoneyPrecision).ToString() + "," +
                                TopOddsGame.ToString() + "," +

                                Math.Round(DailyTopWin, RankMoneyPrecision).ToString() + "," +
                                Math.Round(DailyTopBet, RankMoneyPrecision).ToString() + "," +

                                Math.Round(DailyTopOdds, RankOddsPrecision).ToString() + "," +
                                Math.Round(DailyTopOddsBet, RankMoneyPrecision).ToString() + "," +
                                Math.Round(DailyTopOddsWin, RankMoneyPrecision).ToString() + "," +
                                DailyTopOddsGame.ToString() + ")";
                return updata;
            }
            /// <summary>取得批次更新 Update欄位名稱值</summary>
            static public string GetUpdateDataKey()
            {
                string updata = "EventNo=VALUES(EventNo), " +
                                nameof(EntityUid) + "=VALUES(" + nameof(EntityUid) + "), " +
                                nameof(UserUid) + "=VALUES(" + nameof(UserUid) + "), " +
                                nameof(UserName) + "=VALUES(" + nameof(UserName) + "), " +

                                nameof(TotalWin) + "=VALUES(" + nameof(TotalWin) + "), " +
                                nameof(TotalBet) + "=VALUES(" + nameof(TotalBet) + "), " +

                                nameof(TopWin) + "=VALUES(" + nameof(TopWin) + "), " +
                                nameof(TopWinBet) + "=VALUES(" + nameof(TopWinBet) + "), " +
                                nameof(TopWinGame) + "=VALUES(" + nameof(TopWinGame) + "), " +

                                nameof(TopOdds) + "=VALUES(" + nameof(TopOdds) + "), " +
                                nameof(TopOddsBet) + "=VALUES(" + nameof(TopOddsBet) + "), " +
                                nameof(TopOddsWin) + "=VALUES(" + nameof(TopOddsWin) + "), " +
                                nameof(TopOddsGame) + "=VALUES(" + nameof(TopOddsGame) + "), " +

                                nameof(DailyTopWin) + "=VALUES(" + nameof(DailyTopWin) + "), " +
                                nameof(DailyTopBet) + "=VALUES(" + nameof(DailyTopBet) + "), " +

                                nameof(DailyTopOdds) + "=VALUES(" + nameof(DailyTopOdds) + "), " +
                                nameof(DailyTopOddsBet) + "=VALUES(" + nameof(DailyTopOddsBet) + "), " +
                                nameof(DailyTopOddsWin) + "=VALUES(" + nameof(DailyTopOddsWin) + "), " +
                                nameof(DailyTopOddsGame) + "=VALUES(" + nameof(DailyTopOddsGame) + ")";
                return updata;
            }
            #endregion

            #region 批次更新 日榜紀錄 欄位 (注意各函式欄位順序需相同)
            /// <summary>日榜紀錄 取得批次更新 欄位名稱 DailyTopWin</summary>
            public static string GetUpdateDataColDailyTopWin()
            {
                string updata = "EventNo," +
                                "RecDate," +
                                "UserUid," +
                                "EntityUid," +
                                "UserName," +

                                "DailyTopWin," +
                                "DailyTopBet," +

                                "RankNo";
                return updata;
            }
            /// <summary>日榜紀錄 取得批次更新 欄位值</summary>
            public string GetUpdateDataValDailyTopWin(string eventNo, int rankNo, string recDate)
            {
                string temp = "(" + eventNo.ToString() + "," +
                              "'" + recDate + "'," +
                              UserUid.ToString() + "," +
                              EntityUid.ToString() + "," +
                              "'" + UserName + "'," +
                              Math.Round(DailyTopWin, RankMoneyPrecision).ToString() + "," +
                              Math.Round(DailyTopBet, RankMoneyPrecision).ToString() + "," +
                              rankNo.ToString() + ")";
                return temp;
            }

            /// <summary>日榜紀錄 取得批次更新 欄位名稱 DailyTopOdds</summary>
            public static string GetUpdateDataColDailyTopOdds()
            {
                string updata = "EventNo," +
                                "RecDate," +
                                "UserUid," +
                                "EntityUid," +
                                "UserName," +

                                "DailyTopOdds," +
                                "DailyTopOddsBet," +
                                "DailyTopOddsWin," +
                                "DailyTopOddsGame," +

                                "RankNo";
                return updata;
            }
            /// <summary>日榜紀錄 取得批次更新 欄位值</summary>
            public string GetUpdateDataValDailyTopOdds(string eventNo, int rankNo, string recDate)
            {
                string temp = "(" + eventNo.ToString() + "," +
                              "'" + recDate + "'," +
                              UserUid.ToString() + "," +
                              EntityUid.ToString() + "," +
                              "'" + UserName + "'," +
                              Math.Round(DailyTopOdds, RankMoneyPrecision).ToString() + "," +
                              Math.Round(DailyTopOddsBet, RankMoneyPrecision).ToString() + "," +
                              Math.Round(DailyTopOddsWin, RankMoneyPrecision).ToString() + "," +
                              DailyTopOddsGame.ToString() + "," +
                              rankNo.ToString() + ")";
                return temp;
            }
            #endregion

            #region 批次更新 整期活動歷史紀錄 欄位 (注意各函式欄位順序需相同)
            /// <summary>整期活動歷史紀錄 取得批次更新 欄位名稱 (龍榜)</summary>
            public static string GetUpdateDataColDragon()
            {
                string updata = "EventNo," +
                                //"RecDate," +
                                "UserUid," +
                                "EntityUid," +
                                "UserName," +

                                "TotalWin," +
                                "TotalBet," +

                                "RankNo";
                return updata;
            }
            /// <summary>整期活動歷史紀錄 取得批次更新 欄位值 (龍榜)</summary>
            public string GetUpdateDataValDragon(string eventNo, int rankNo, string recDate)
            {
                string temp = "(" + eventNo.ToString() + "," +
                              //"'" + recDate + "'," +
                              UserUid.ToString() + "," +
                              EntityUid.ToString() + "," +
                              "'" + UserName + "'," +
                              Math.Round(TotalWin, RankMoneyPrecision).ToString() + "," +
                              Math.Round(TotalBet, RankMoneyPrecision).ToString() + "," +
                              rankNo.ToString() + ")";
                return temp;
            }

            /// <summary>整期活動歷史紀錄 取得批次更新 欄位名稱 (虎榜)</summary>
            public static string GetUpdateDataColTiger()
            {
                string updata = "EventNo," +
                                //"RecDate," +
                                "UserUid," +
                                "EntityUid," +
                                "UserName," +

                                "TotalWin," +
                                "TotalBet," +

                                "RankNo";
                return updata;
            }
            /// <summary>整期活動歷史紀錄 取得批次更新 欄位值 (虎榜)</summary>
            public string GetUpdateDataValTiger(string eventNo, int rankNo, string recDate)
            {
                string temp = "(" + eventNo.ToString() + "," +
                              //"'" + recDate + "'," +
                              UserUid.ToString() + "," +
                              EntityUid.ToString() + "," +
                              "'" + UserName + "'," +
                              Math.Round(TotalWin, RankMoneyPrecision).ToString() + "," +
                              Math.Round(TotalBet, RankMoneyPrecision).ToString() + "," +
                              rankNo.ToString() + ")";
                return temp;
            }

            /// <summary>整期活動歷史紀錄 取得批次更新 欄位名稱 (單局勝分榜)</summary>
            public static string GetUpdateDataColTopWin()
            {
                string updata = "EventNo," +
                                //"RecDate," +
                                "UserUid," +
                                "EntityUid," +
                                "UserName," +

                                "TopWin," +
                                "TopWinBet," +
                                "TopWinGame," +

                                "RankNo";
                return updata;
            }
            /// <summary>整期活動歷史紀錄 取得批次更新 欄位值 (單局勝分榜)</summary>
            public string GetUpdateDataValTopWin(string eventNo, int rankNo, string recDate)
            {
                string temp = "(" + eventNo.ToString() + "," +
                              //"'" + recDate + "'," +
                              UserUid.ToString() + "," +
                              EntityUid.ToString() + "," +
                              "'" + UserName + "'," +
                              Math.Round(TopWin, RankMoneyPrecision).ToString() + "," +
                              Math.Round(TopWinBet, RankMoneyPrecision).ToString() + "," +
                              TopWinGame.ToString() + "," +
                              rankNo.ToString() + ")";
                return temp;
            }

            /// <summary>整期活動歷史紀錄 取得批次更新 欄位名稱 (單局倍率榜)</summary>
            public static string GetUpdateDataColTopOdds()
            {
                string updata = "EventNo," +
                                //"RecDate," +
                                "UserUid," +
                                "EntityUid," +
                                "UserName," +

                                "TopOdds," +
                                "TopOddsBet," +
                                "TopOddsWin," +
                                "TopOddsGame," +

                                "RankNo";
                return updata;
            }
            /// <summary>整期活動歷史紀錄 取得批次更新 欄位值 (單局倍率榜)</summary>
            public string GetUpdateDataValTopOdds(string eventNo, int rankNo, string recDate)
            {
                string temp = "(" + eventNo.ToString() + "," +
                              //"'" + recDate + "'," +
                              UserUid.ToString() + "," +
                              EntityUid.ToString() + "," +
                              "'" + UserName + "'," +
                              Math.Round(TopOdds, RankMoneyPrecision).ToString() + "," +
                              Math.Round(TopOddsBet, RankMoneyPrecision).ToString() + "," +
                              Math.Round(TopOddsWin, RankMoneyPrecision).ToString() + "," +
                              TopOddsGame.ToString() + "," +
                              rankNo.ToString() + ")";
                return temp;
            }
            #endregion
        }
        /// <summary>玩家統計紀錄結構</summary>
        class RankEventPlayerAcc : RankEventPlayer
        {
            //統計紀錄輔助欄位
            public double LastBet = 0;
            public double FreeWinAcc = 0;
            public int LastGame = 0;
            bool FreeGameFg = false;

            public RankEventPlayerAcc() { }
            public RankEventPlayerAcc(int entityUid, int userUid, string userName, double bet, double win, double odds, int gameServer)
            {
                EntityUid = entityUid;
                UserUid = userUid;
                UserName = userName;

                TotalBet = bet;
                TotalWin = win;

                TopWin = win;
                TopWinBet = bet;
                TopWinGame = gameServer;

                if (odds > 0)
                {
                    TopOdds = odds;
                    TopOddsBet = bet;
                    TopOddsWin = win;
                    TopOddsGame = gameServer;
                    DailyTopOdds = odds;
                    DailyTopOddsBet = bet;
                    DailyTopOddsWin = win;
                    DailyTopOddsGame = gameServer;
                }

                DailyTopWin = win;
                DailyTopBet = bet;

                LastBet = bet;
                LastGame = gameServer;
                FreeWinAcc = win;

                UpdateFg = true;
            }

            /// <summary>玩家統計紀錄 記帳</summary>
            public void Record(double bet, double win, double odds, GameServerCode gameServerCode)
            {
                odds = Math.Round(odds, RankOddsPrecision);
                int gameSrv = (int)gameServerCode;

                //歷史累計 (龍/虎榜)
                TotalWin = Math.Round(TotalWin + win, RankMoneyPrecision);
                TotalBet = Math.Round(TotalBet + bet, RankMoneyPrecision);

                //每日最高 累計總贏分 (日勝分)
                DailyTopWin = Math.Round(DailyTopWin + win, RankMoneyPrecision);
                DailyTopBet = Math.Round(DailyTopBet + bet, RankMoneyPrecision);

                double recOdds = odds;
                double recBet = bet;
                double recWin = win;
                int recGame = gameSrv;
                //歷史最高 單局最高贏分 (單局勝分)
                if (bet > 0)
                {
                    if (FreeGameFg && FreeWinAcc > 0 && LastBet > 0)
                    {
                        FreeWinAcc = Math.Round(FreeWinAcc, RankMoneyPrecision);
                        //FreeGame結束, 結算FreeGame贏分
                        if (TopWin < FreeWinAcc)
                        {
                            TopWin = FreeWinAcc;
                            TopWinBet = LastBet;
                            TopWinGame = LastGame;
                        }

                        double fodds = Math.Round(FreeWinAcc / LastBet, RankOddsPrecision); //Free倍率 = 贏分 / 押分
                        if (odds < fodds)
                        {
                            recOdds = fodds;
                            recGame = LastGame;
                            recBet = LastBet;
                            recWin = FreeWinAcc;
                        }

                        //MyConsole.WriteLine($"    FreeGame結算: 玩家{UserName} FreeWinAcc={FreeWinAcc}, LastBet={LastBet}, FreeOdds={fodds}, LastGame={(GameServerCode)LastGame}");
                    }

                    if (TopWin < win)
                    {
                        TopWin = win;
                        TopWinBet = bet;
                        TopWinGame = gameSrv;
                    }

                    LastBet = bet;
                    LastGame = gameSrv;
                    FreeWinAcc = win;
                    FreeGameFg = false;

                    //歷史最高 單局最高倍率  (單局倍率)
                    if (Math.Round(TopOdds, RankOddsPrecision) < recOdds)
                    {
                        TopOdds = recOdds;
                        TopOddsBet = recBet;
                        TopOddsWin = recWin;
                        TopOddsGame = recGame;
                    }

                    //每日最高 倍率 (日倍率)
                    if (Math.Round(DailyTopOdds, RankOddsPrecision) < recOdds)
                    {
                        DailyTopOdds = recOdds;
                        DailyTopOddsBet = recBet;
                        DailyTopOddsWin = recWin;
                        DailyTopOddsGame = recGame;
                    }
                }
                else //押分為0時 = FreeGame
                {
                    FreeGameFg = true;
                    FreeWinAcc += win;
                    //MyConsole.WriteLine($"    FreeGame: 玩家{UserName} Win={win}, FreeWinAcc={FreeWinAcc}, LastBet={LastBet}, LastGame={(GameServerCode)LastGame}");
                }

                UpdateFg = true;
            }
        }

        /// <summary>排行榜 玩家統計紀錄 (玩家押分時呼叫)</summary>
        void RankEventRecord(EntityData entity, UserData userData, double win, double bet, GameServerCode gameServer)
        {
            //#250924 排行榜 玩家統計紀錄
            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.TaskNo == entity.RankEventTaskNo) //是代理所屬的排行榜任務編號
                {
                    rankExec.RankEventRecord(entity, userData, win, bet, gameServer);
                    return;
                }
            }
        }
        #endregion


        #region 排行榜刷新
        System.Threading.Timer _rankEventRefreshTimer;
        System.Threading.Timer _rankEventDailyTimer;
        bool _rankEventStarted;
        object _relocktmr = new object();

        /// <summary>排行榜 停止榜單刷新工作</summary>
        void StopRankRefreshTask()
        {
            lock (_relocktmr)
            {
                if (_rankEventStarted)
                {
                    _rankEventRefreshTimer?.Dispose();
                    _rankEventRefreshTimer = null;
                    _rankEventStarted = false;
                    if (RankDbfg) MyConsole.WriteLine("排行榜 停止榜單刷新工作");
                }
            }
        }
        /// <summary>排行榜 停止每日結算工作</summary>
        void StopRankDailyTask()
        {
            lock (_relocktmr)
            {
                _rankEventDailyTimer?.Dispose();
                _rankEventDailyTimer = null;
            }
        }

        /// <summary>排行榜 啟動榜單刷新工作</summary>
        void ScheduleRankRefreshTask()
        {
            lock (_relocktmr)
            {
                if (_rankEventStarted)
                {
                    if (RankDbfg) MyConsole.WriteLine("排行榜 榜單刷新工作 已是啟動狀態");
                    return;
                }

                // 先立即刷新一次，之後每 3 分鐘刷新
                _rankEventRefreshTimer?.Dispose();
                _rankEventRefreshTimer = new System.Threading.Timer(RefreshRankEventLists, null, TimeSpan.Zero, TimeSpan.FromSeconds(20)); //.FromMinutes(3));
                if (RankDbfg) MyConsole.WriteLine("排行榜 啟動榜單刷新工作");

                _rankEventStarted = true;
            }
        }
        /// <summary>排行榜 啟動每日結算工作 Timer</summary>
        void ScheduleRankDailyTask()
        {
            DateTime now = DateTime.Now;
            DateTime nextMidnight = now.Date.AddDays(1).AddHours(RankEventHour).AddMinutes(2); //明日 00:02     .AddMinutes(5); // 明日 00:05 

            //如果現在時間大於23:50, 則再加一天  (啟動時間太過於接近結算時間, 改為明日再結算)
            int overHour;
            if (RankEventHour == 0) overHour = 23;
            else overHour = RankEventHour - 1;

            if (now.Hour == RankEventHour && now.Minute >= 50)
            {
                nextMidnight = nextMidnight.AddDays(1);
            }

            //DateTime nextMidnight = now.AddMinutes(1.5); //UNDONE: 2分鐘後 啟動每日工作
            TimeSpan due = nextMidnight - now;

            if (RankDbfg) MyConsole.WriteLine($"排行榜 ScheduleRankDailyTask: 排定下次每日結算時間 {nextMidnight.ToString("yyyy-MM-dd HH:mm:ss")}, 間隔 {Math.Round(due.TotalMinutes, 2)} 分鐘");

            lock (_relocktmr)
            {
                _rankEventDailyTimer?.Dispose();
                _rankEventDailyTimer = new System.Threading.Timer(RankDailyTaskCallback, null, due, Timeout.InfiniteTimeSpan);
            }
        }

        /// <summary>排行榜 榜單刷新工作 判斷 啟動or停止</summary>
        int ResetRankRefreshTask()
        {
            int actCnt = 0;
            int avCnt = 0;

            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.Active > 0)
                {
                    actCnt++; //有效期內數量

                    if (rankExec.Active == 2)
                    {
                        avCnt++; //活動中數量
                    }
                }
            }

            if (avCnt > 0)
            {
                ScheduleRankRefreshTask();
            }
            else
            {
                StopRankRefreshTask();
            }

            return actCnt;
        }

        /// <summary>刷新排行榜回調</summary>
        void RefreshRankEventLists(object state)
        {
            try
            {
                bool renewfg = false;
                foreach (var rankExec in RankEventExec)
                {
                    rankExec.RKDAdd();

                    if (rankExec.GetAllRankEventList(false))
                    {
                        renewfg = true;
                    }
                }

                if (renewfg)
                {
                    // 排行榜 刷新後 將統計資料寫入DB
                    UpdateRankEventPlayerAccount();
                }
            }
            catch (Exception ex)
            {
                //Program.WriteDBWorkLog("PlayerStatisticsService_RefreshTopLists", ex.ToString());
            }
        }

        /// <summary>排行榜 每日結算工作回調</summary>
        void RankDailyTaskCallback(object state)
        {
            if (RankDbfg) MyConsole.WriteLine("排行榜 每日工作: 工作開始");

            DateTime now = DateTime.Now;

            foreach (var rankExec in RankEventExec)
            {
                int status = RankDailyTaskProc(rankExec, now);
            }

            // 玩家統計資料 寫入DB
            UpdateRankEventPlayerAccount();
            // 日榜紀錄 寫入DB
            DailyHistoryRecord();
            // 當期排行榜 寫入歷史紀錄
            RankStageHistoryRecord();
            // 重新檢查是否有新的排行榜活動
            LookingforRankEventNewTask();
            // 任務狀態 寫入DB
            UpdateRankEventSaveData();

            int actCnt = ResetRankRefreshTask();
            //if (actCnt > 0)
            {
                // 重新排程下一次 00:00
                //if (RankDbfg) MyConsole.WriteLine("排行榜 每日結算: 排程下一次 每日結算工作");
                ScheduleRankDailyTask();
            }
        }
        /// <summary>排行榜 每日結算工作</summary>
        int RankDailyTaskProc(RankEventExecuteInfo rankExec, DateTime now)
        {
            int status = 0;
            //保存舊的活動狀態
            //int oldActive = RankEventExecuteInfo.Active;
            int oldActive = rankExec.ActiveSave;

            // 檢查活動期限 (重設新的活動狀態)
            rankExec.CheckVaildDate(now);

            if (rankExec.Active > 0 && rankExec.Active < oldActive)
            {
                if (RankDbfg)
                {
                    MyConsole.WriteLine($"排行榜 每日工作: !ERROR! 活動狀態逆行[{oldActive}]->[{rankExec.Active}]");
                    MyConsole.WriteLine($"    活動序號:{rankExec.EventNo}");
                    MyConsole.WriteLine($"    活動日期:{RankDateStr(rankExec.EventDateSt)} ~ {RankDateStr(rankExec.EventDateEnd)}, 領獎結束:{RankDateStr(rankExec.ClaimDateEnd)}");
                }
            }

            try
            {
                if (oldActive == 1) // 原來活動尚未開始
                {
                    status = 10;
                    if (rankExec.Active == 2) // 現在活動已開始
                    {
                        status = 11;
                        if (rankExec.ResetFg == 0)
                        {
                            if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 活動已開始, 重置 當期玩家累計統計紀錄表!");
                            ClearAllRankEventPlayerAccount(rankExec.TaskNo);
                            rankExec.RankEventPlayerList.Clear();
                            rankExec.ResetFg = 1;
                        }

                        if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 活動已開始, 重置 當期玩家累計統計紀錄表!");
                        //取得 RankEventPlayerAccount 當期玩家累計統計紀錄表
                        rankExec.GetRankEventPlayerListFromDB();
                    }
                    else
                    {
                        if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 活動尚未開始");
                    }
                }
                else if (oldActive == 2) // 原來活動已經開始
                {
                    // 先刷新一次排行榜
                    rankExec.GetAllRankEventList(true);
                    // 清空今日日榜統計資料
                    rankExec.ResetRankEventDailyRecord();
                    // 日榜紀錄 寫入DB
                    rankExec.DailyRecFg = true;

                    if (rankExec.Active == 2) //現在活動持續中
                    {
                        status = 20;
                        if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 活動持續進行中!");
                    }
                    else  //現在活動結束, 結算
                    {
                        status = 21;
                        if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 活動結束 當期排行榜結算");
                        // 當期排行榜 寫入歷史紀錄
                        rankExec.HistoryRecFg = true;
                        rankExec.EventEndFg = 1;

                        if (rankExec.Active == 3)
                        {
                            //UNDONE: 排行榜 獎金發放計算
                            if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 進入領獎期限!");
                        }
                    }
                }
                else if (oldActive == 3) // 原來領獎期限中
                {
                    status = 30;
                    if (rankExec.Active == 3) //現在還是領獎期限
                    {
                        if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 領獎期限持續中");
                    }
                }

                if (rankExec.Active == 0) //現在活動已結束
                {
                    if (oldActive != 0)
                    {
                        if (RankDbfg) MyConsole.WriteLine($"    排行榜 每日工作: 活動與領獎已全部結束!");
                        //RankEventExecuteInfo.CloseRankEvent();
                    }
                //
                //    //檢查是否有新的活動排程, 有的話重新啟動活動
                //    bool newEvent = CheckNewRankEvent(rankExec);
                //    if (newEvent)
                //    {
                //        //此處已是載入新的活動資料
                //        if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 新的排行榜活動程序已啟動");
                //        oldActive = -1; //強制更新SaveData
                //    }
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine($"DailyTaskCallback Exception: {ex.ToString()}");
            }
            finally
            {
                rankExec.SaveDataFg = true;
            }
            return status;
        }

        /// <summary>尋找新的排行榜活動</summary>
        void LookingforRankEventNewTask()
        {
            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.Active == 0) //現在活動已結束
                {
                    //檢查是否有新的活動排程, 有的話重新啟動活動
                    bool newEvent = CheckNewRankEvent(rankExec);
                    if (newEvent)
                    {
                        //此處已是載入新的活動資料
                        if (RankDbfg) MyConsole.WriteLine("    排行榜 每日工作: 新的排行榜活動程序已啟動");
                    }
                }
            }
        }

        /// <summary>將 RankEventPlayerAccount 資料表清空</summary>
        void ClearAllRankEventPlayerAccount(int taskNo)
        {
            try
            {
                //UNDONE: 251008 改為不清掉, 先留著備查詢
                //int result = myAcess.delete("RankEventPlayerAccount", "1=1");
                //if (result < 0)
                //{
                //    MyConsole.WriteLine("ClearAllRankEventPlayerAccount Error");
                //}
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine($"ClearAllRankEventPlayerAccount Exception: {ex.ToString()}");
            }
        }

        /// <summary>更新玩家統計資料 (Cache寫入DB)</summary>
        void UpdateRankEventPlayerAccount()
        {
            // 先收集需要更新的玩家
            List<string> valueList = new();

            foreach (var rankExec in RankEventExec)
            {
                rankExec.GetAllPlayerUpdateData(ref valueList);
            }

            // 有資料才更新
            if (valueList.Count > 0)
            {
                string tablename = "RankEventPlayerAccount";
                string col = RankEventPlayer.GetUpdateDataCol();
                string update = RankEventPlayer.GetUpdateDataKey();
                PushToOperTionDBBoxInsertUpdate(tablename, col, valueList, update);

                if (RankDbfg2) MyConsole.WriteLine($" -排行榜 更新玩家統計紀錄表:{valueList.Count}人");
            }
        }

        /// <summary>日榜歷史紀錄 (Cache寫入DB)</summary>
        void DailyHistoryRecord()
        {
            string recDate = RankDateStr(DateTime.Now.AddDays(-1).Date); //記錄的是昨天的日期

            List<string> updateTopWin = new();
            List<string> updateTopOdds = new();

            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.DailyRecFg)
                {
                    rankExec.DailyRecFg = false;
                    DailyHistoryRecord(rankExec, ref updateTopWin, ref updateTopOdds, recDate);
                }
            }

            if (updateTopWin.Count > 0)
            {
                string tablename = "RankEventDailyTopWinBoard";
                string col = RankEventPlayer.GetUpdateDataColDailyTopWin();
                PushToOperTionDBBoxInsert(tablename, col, updateTopWin);
            }

            if (updateTopOdds.Count > 0)
            {
                string tablename = "RankEventDailyTopOddsBoard";
                string col = RankEventPlayer.GetUpdateDataColDailyTopOdds();
                PushToOperTionDBBoxInsert(tablename, col, updateTopOdds);
            }
        }
        /// <summary>製作任務日榜紀錄 (Cache寫入DB)</summary>
        void DailyHistoryRecord(RankEventExecuteInfo rankExec, ref List<string> updateTopWin, ref List<string> updateTopOdds, string recDate)
        {
            //排行榜 日勝分榜歷史紀錄
            string eventNo = rankExec.EventNo.ToString();

            if (rankExec.RankEventListDailyTopWin != null && rankExec.RankEventListDailyTopWin.Count > 0)
            {
                int rankNo = 1;
                foreach (var rank in rankExec.RankEventListDailyTopWin)
                {
                    updateTopWin.Add(rank.GetUpdateDataValDailyTopWin(eventNo, rankNo, recDate));
                    rankNo++;
                }
            }
            else
            {
                MyConsole.WriteLine($"DailyHistoryRecord[#{rankExec.TaskNo}]: 日勝分榜 無資料");
            }


            if (rankExec.RankEventListDailyTopOdds != null && rankExec.RankEventListDailyTopOdds.Count > 0)
            {
                //排行榜 日倍率榜歷史紀錄
                int rankNo = 1;
                foreach (var rank in rankExec.RankEventListDailyTopOdds)
                {
                    updateTopOdds.Add(rank.GetUpdateDataValDailyTopOdds(eventNo, rankNo, recDate));
                    rankNo++;
                }
            }
            else
            {
                MyConsole.WriteLine($"DailyHistoryRecord[#{rankExec.TaskNo}]: 日倍率榜 無資料");
            }
        }

        /// <summary>整期活動歷史紀錄 (Cache寫入DB)</summary>
        void RankStageHistoryRecord()
        {
            bool recfg = false;
            foreach (var rankExec in RankEventExec) { if (rankExec.HistoryRecFg) { recfg = true; break; } }
            if (!recfg) { return; } //沒有需要記錄的, 直接返回

            //MyConsole.WriteLine("排行榜活動: 整期活動歷史紀錄 寫入DB");

            string recDate = RankDateStr(DateTime.Now.AddDays(-1).Date); //記錄的是昨天的日期

            List<string> updateDragon = new();
            List<string> updateTiger = new();
            List<string> updateTopWin = new();
            List<string> updateTopOdds = new();

            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.HistoryRecFg)
                {
                    rankExec.HistoryRecFg = false;
                    RankStageHistoryRecord(rankExec, ref updateDragon, ref updateTiger, ref updateTopWin, ref updateTopOdds, recDate);
                }
            }

            if (updateDragon.Count > 0)
            {
                string tablename = "RankEventDragonBoard";
                string col = RankEventPlayer.GetUpdateDataColDragon();
                PushToOperTionDBBoxInsert(tablename, col, updateDragon);
            }

            if (updateTiger.Count > 0)
            {
                string tablename = "RankEventTigerBoard";
                string col = RankEventPlayer.GetUpdateDataColTiger();
                PushToOperTionDBBoxInsert(tablename, col, updateTiger);
            }

            if (updateTopWin.Count > 0)
            {
                string tablename = "RankEventTopWinBoard";
                string col = RankEventPlayer.GetUpdateDataColTopWin();
                PushToOperTionDBBoxInsert(tablename, col, updateTopWin);
            }

            if (updateTopOdds.Count > 0)
            {
                string tablename = "RankEventTopOddsBoard";
                string col = RankEventPlayer.GetUpdateDataColTopOdds();
                PushToOperTionDBBoxInsert(tablename, col, updateTopOdds);
            }
        }
        void RankStageHistoryRecord(RankEventExecuteInfo rankExec, ref List<string> updateDragon, ref List<string> updateTiger, ref List<string> updateTopWin, ref List<string> updateTopOdds, string recDate)
        {
            MyConsole.WriteLine($"排行榜活動[#{rankExec.TaskNo}]: 整期活動歷史紀錄 寫入DB");

            string eventNo = rankExec.EventNo.ToString();

            //龍榜榜單List: RankEventListDragon  資料表:RankEventDragonBoard  資料表欄位:EventNo,EntityUid,UserUid,UserName,TotalWin,TotalBet
            if (rankExec.RankEventListDragon != null && rankExec.RankEventListDragon.Count > 0)
            {
                int rankNo = 1;
                foreach (var rank in rankExec.RankEventListDragon)
                {
                    updateDragon.Add(rank.GetUpdateDataValDragon(eventNo, rankNo, recDate));
                    rankNo++;
                }
            }
            else
            {
                MyConsole.WriteLine("    排行榜活動: 龍榜 無資料");
            }


            //虎榜榜單List: RankEventListTiger   資料表:RankEventTigerBoard   資料表欄位:EventNo,EntityUid,UserUid,UserName,TotalWin,TotalBet
            if (rankExec.RankEventListTiger != null && rankExec.RankEventListTiger.Count > 0)
            {
                int rankNo = 1;
                foreach (var rank in rankExec.RankEventListTiger)
                {
                    updateTiger.Add(rank.GetUpdateDataValTiger(eventNo, rankNo, recDate));
                    rankNo++;
                }
            }
            else
            {
                MyConsole.WriteLine("    排行榜活動: 虎榜 無資料");
            }


            //單局勝分榜榜單List: RankEventListTopWin   資料表:RankEventTopWinBoard   資料表欄位:EventNo,EntityUid,UserUid,UserName,TotalWin,TopWinBet,TopWinGame
            if (rankExec.RankEventListTopWin != null && rankExec.RankEventListTopWin.Count > 0)
            {
                int rankNo = 1;
                foreach (var rank in rankExec.RankEventListTopWin)
                {
                    updateTopWin.Add(rank.GetUpdateDataValTopWin(eventNo, rankNo, recDate));
                    rankNo++;
                }
            }
            else
            {
                MyConsole.WriteLine("    排行榜活動: 單局勝分榜 無資料");
            }


            //單局倍率榜榜單List: RankEventListTopOdds   資料表:RankEventTopOddsBoard   資料表欄位:EventNo,EntityUid,UserUid,UserName,TopOdds,TopOddsGame
            if (rankExec.RankEventListTopOdds != null && rankExec.RankEventListTopOdds.Count > 0)
            {
                int rankNo = 1;
                foreach (var rank in rankExec.RankEventListTopOdds)
                {
                    updateTopOdds.Add(rank.GetUpdateDataValTopOdds(eventNo, rankNo, recDate));
                    rankNo++;
                }
            }
            else
            {
                MyConsole.WriteLine("    排行榜活動: 單局倍率榜 無資料");
            }
        }
        #endregion


        #region 排行榜接口
        /// <summary>WEB通知更新排行榜設定</summary>
        public void RefreshRankEventSetting(int operation, int eventNo)
        {
            RankEventProcInit();

        }

        /// <summary>取得 指定任務 要廣播的玩家List</summary>
        public List<int> GetRankEventNowPlayerList(int taskNo)
        {
            List<int> playerList = new();

            //if (RankEventExecuteInfo.IsActive())
            {
                int lastEntity = 0;
                bool lastRankingFg = false;
                Dictionary<int, bool> entityRankingFg = new();

                lock (UserDataList)
                {
                    //var userDatas = UserDataList.Values.Where(x => x.Usersituation != 0).ToList();

                    foreach (var data in UserDataList)
                    {
                        if (data.Value.Usersituation == 0) //只通知線上玩家
                        {
                            continue;
                        }

                        int entityId = data.Value.EntityId;

                        bool isRank = false;
                        if (entityId == lastEntity)
                        {
                            isRank = lastRankingFg;
                        }
                        else
                        {
                            if (entityRankingFg.ContainsKey(entityId))
                            {
                                isRank = entityRankingFg[entityId];
                            }
                            else
                            {
                                EntityData entity = GetEntityData(entityId);
                                if (entity != null)
                                {
                                    if (entity.RankEventTaskNo == taskNo)
                                    {
                                        isRank = entity.RankingFg;
                                    }
                                    else  //此代理非屬本活動
                                    {
                                        isRank = false;
                                    }

                                    entityRankingFg.Add(entityId, isRank);
                                }
                            }

                            lastEntity = entityId;
                            lastRankingFg = isRank;
                        }

                        if (isRank)
                        {
                            playerList.Add(data.Key);
                        }
                    }
                }
            }
            return playerList;
        }

        /// <summary>玩家獲取 排行榜設定資訊</summary>
        public void GetRankEventInfo(UserData userData, EntityData entity, ref Dictionary<string, string> extraInfo)
        {
            if(!entity.RankingFg) return; //代理未啟用排行榜功能

            GetRankEventInfo(entity.RankEventTaskNo, ref extraInfo);
        }
        /// <summary>取得 排行榜設定資訊</summary>
        public void GetRankEventInfo(int taskNo, ref Dictionary<string, string> extraInfo)
        {
            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.TaskNo == taskNo) //是代理所屬的排行榜任務編號
                {
                    //if (rankExec.IsActive())
                    {
                        rankExec.GetRankEventInfo(ref extraInfo);
                    }
                    return;
                }
            }

            RankEventExecuteInfo.NoRankEventInfo(ref extraInfo); //無排行榜活動資訊
        }

        /// <summary>玩家獲取 排行榜榜單</summary>
        public void GetRankEventTopData(UserData userData, EntityData entity, ref CommonInfoData infoData)
        {
            Dictionary<string, string> extraInfo = infoData.Data;

            foreach (var rankExec in RankEventExec)
            {
                if (rankExec.TaskNo == entity.RankEventTaskNo) //是代理所屬的排行榜任務編號
                {
                    if (rankExec.IsActive() && entity.RankingFg)
                    {
                        //龍榜
                        if (entity.RankDragonFg)
                        {
                            GetRankListDragon(rankExec, userData.UserUID, out string tList, out string myData);
                            extraInfo.Add("Dragon", tList);
                            extraInfo.Add("Dragon_M", myData);
                        }
                        else
                        {
                            extraInfo.Add("Dragon", RankBonusDisable);
                        }

                        //虎榜
                        if (entity.RankTigerFg)
                        {
                            GetRankListTiger(rankExec, userData.UserUID, out string tList, out string myData);
                            extraInfo.Add("Tiger", tList);
                            extraInfo.Add("Tiger_M", myData);
                        }
                        else
                        {
                            extraInfo.Add("Tiger", RankBonusDisable);
                        }

                        //單局勝分榜
                        if (entity.RankTopWinFg)
                        {
                            GetRankListTopWin(rankExec, userData.UserUID, out string tList, out string myData);
                            extraInfo.Add("TopWin", tList);
                            extraInfo.Add("TopWin_M", myData);
                        }
                        else
                        {
                            extraInfo.Add("TopWin", RankBonusDisable);
                        }

                        //單局倍率榜
                        if (entity.RankTopOddsFg)
                        {
                            GetRankListTopOdds(rankExec, userData.UserUID, out string tList, out string myData);
                            extraInfo.Add("TopOdds", tList);
                            extraInfo.Add("TopOdds_M", myData);
                        }
                        else
                        {
                            extraInfo.Add("TopOdds", RankBonusDisable);
                        }

                        //日榜單
                        if (entity.RankDailyTopWinFg)
                        {
                            GetRankListDailyTopWin(rankExec, userData.UserUID, out string tList, out string myData);
                            extraInfo.Add("DailyTopWin", tList);
                            extraInfo.Add("DailyTopWin_M", myData);
                        }
                        else
                        {
                            extraInfo.Add("DailyTopWin", RankBonusDisable);
                        }

                        //日倍率榜
                        if (entity.RankDailyTopOddsFg)
                        {
                            GetRankListDailyTopOdds(rankExec, userData.UserUID, out string tList, out string myData);
                            extraInfo.Add("DailyTopOdds", tList);
                            extraInfo.Add("DailyTopOdds_M", myData);
                        }
                        else
                        {
                            extraInfo.Add("DailyTopOdds", RankBonusDisable);
                        }

                        infoData.Type = 0; //成功
                    }
                    else
                    {
                        infoData.Type = 1; //失敗
                        infoData.Message = "排行榜功能未啟用";
                    }

                    return;
                }
            }

            infoData.Type = 1; //失敗
            infoData.Message = "代理無指定排行榜活動";
        }

        /// <summary>無獎金 識別字</summary>
        const string RankBonusNone = "0";
        /// <summary>榜單未開啟 識別字</summary>
        const string RankBonusDisable = "DISABLE";

        /// <summary>取得 龍榜榜單</summary>
        void GetRankListDragon(RankEventExecuteInfo rankExec, int userUid, out string tList, out string myData)
        {
            myData = "";
            if (rankExec.RankEventListDragon == null || rankExec.RankEventListDragon.Count <= 0)
            {
                tList = "";
                return;
            }

            StringBuilder sb = new StringBuilder();
            int no = 1; //排名從1開始
            bool foundMe = false;
            foreach (var player in rankExec.RankEventListDragon)
            {
                string record = $"{no},{player.UserName},{player.TotalWin},{RankBonusNone}";

                if (!foundMe && player.UserUid == userUid)
                {
                    //玩家自己的 排名資訊
                    myData = record;
                    foundMe = true;
                }

                if (no <= RankTopCount)
                {
                    //榜單資訊 排名,玩家名稱,總贏分,預計獲得獎金
                    sb.Append(record + ";");
                }

                no++;
                if (no > RankTopCount && foundMe) break;
            }

            tList = sb.ToString().TrimEnd(';');
        }
        /// <summary>取得 虎榜榜單</summary>
        void GetRankListTiger(RankEventExecuteInfo rankExec, int userUid, out string tList, out string myData)
        {
            myData = "";
            if (rankExec.RankEventListTiger == null || rankExec.RankEventListTiger.Count <= 0)
            {
                tList = "";
                return;
            }
            StringBuilder sb = new StringBuilder();
            int no = 1; //排名從1開始
            bool foundMe = false;
            foreach (var player in rankExec.RankEventListTiger)
            {
                string record = $"{no},{player.UserName},{player.TotalBet},{RankBonusNone}";

                if (!foundMe && player.UserUid == userUid)
                {
                    //玩家自己的 排名資訊
                    myData = record;
                    foundMe = true;
                }
                if (no <= RankTopCount)
                {
                    //榜單資訊 排名,玩家名稱,總押分,預計獲得獎金
                    sb.Append(record + ";");
                }
                no++;
                if (no > RankTopCount && foundMe) break;
            }
            tList = sb.ToString().TrimEnd(';');
        }
        /// <summary>取得 單局勝分榜榜單</summary>
        void GetRankListTopWin(RankEventExecuteInfo rankExec, int userUid, out string tList, out string myData)
        {
            myData = "";
            if (rankExec.RankEventListTopWin == null || rankExec.RankEventListTopWin.Count <= 0)
            {
                tList = "";
                return;
            }
            StringBuilder sb = new StringBuilder();
            int no = 1; //排名從1開始
            bool foundMe = false;
            foreach (var player in rankExec.RankEventListTopWin)
            {
                string gameNameInfo = GetGameNameAllLang((GameServerCode)player.TopWinGame);
                string record = $"{no},{player.UserName},{player.TopWin},{player.TopWinBet},{gameNameInfo},{RankBonusNone}";

                if (!foundMe && player.UserUid == userUid)
                {
                    //玩家自己的 排名資訊
                    myData = record;
                    foundMe = true;
                }
                if (no <= RankTopCount)
                {
                    //榜單資訊 排名,玩家名稱,單局最高贏分,單局最高贏分的押分,單局最高贏分的遊戲,預計獲得獎金
                    sb.Append(record + ";");
                }
                no++;
                if (no > RankTopCount && foundMe) break;
            }
            tList = sb.ToString().TrimEnd(';');
        }
        /// <summary>取得 單局倍率榜榜單</summary>
        void GetRankListTopOdds(RankEventExecuteInfo rankExec, int userUid, out string tList, out string myData)
        {
            myData = "";
            if (rankExec.RankEventListTopOdds == null || rankExec.RankEventListTopOdds.Count <= 0)
            {
                tList = "";
                return;
            }
            StringBuilder sb = new StringBuilder();
            int no = 1; //排名從1開始
            bool foundMe = false;
            foreach (var player in rankExec.RankEventListTopOdds)
            {
                string gameNameInfo = GetGameNameAllLang((GameServerCode)player.TopOddsGame);
                string record = $"{no},{player.UserName},{player.TopOdds},{player.TopOddsBet},{player.TopOddsWin},{gameNameInfo},{RankBonusNone}";

                if (!foundMe && player.UserUid == userUid)
                {
                    //玩家自己的 排名資訊
                    myData = record;
                    foundMe = true;
                }
                if (no <= RankTopCount)
                {
                    //榜單資訊 排名,玩家名稱,單局最高倍率,遊戲名稱,押分,贏分,預計獲得獎金
                    sb.Append(record + ";");
                }
                no++;
                if (no > RankTopCount && foundMe) break;
            }
            tList = sb.ToString().TrimEnd(';');
        }
        /// <summary>取得 日勝分榜榜單</summary>
        void GetRankListDailyTopWin(RankEventExecuteInfo rankExec, int userUid, out string tList, out string myData)
        {
            myData = "";
            if (rankExec.RankEventListDailyTopWin == null || rankExec.RankEventListDailyTopWin.Count <= 0)
            {
                tList = "";
                return;
            }
            StringBuilder sb = new StringBuilder();
            int no = 1; //排名從1開始
            bool foundMe = false;
            foreach (var player in rankExec.RankEventListDailyTopWin)
            {
                string record = $"{no},{player.UserName},{player.DailyTopWin},{player.DailyTopBet},{RankBonusNone}";

                if (!foundMe && player.UserUid == userUid)
                {
                    //玩家自己的 排名資訊
                    myData = record;
                    foundMe = true;
                }
                if (no <= RankTopCount)
                {
                    //榜單資訊 排名,玩家名稱,單日最高贏分,預計獲得獎金
                    sb.Append(record + ";");
                }
                no++;
                if (no > RankTopCount && foundMe) break;
            }
            tList = sb.ToString().TrimEnd(';');
        }
        /// <summary>取得 日倍率榜榜單</summary>
        void GetRankListDailyTopOdds(RankEventExecuteInfo rankExec, int userUid, out string tList, out string myData)
        {
            myData = "";
            if (rankExec.RankEventListDailyTopOdds == null || rankExec.RankEventListDailyTopOdds.Count <= 0)
            {
                tList = "";
                return;
            }
            StringBuilder sb = new StringBuilder();
            int no = 1; //排名從1開始
            bool foundMe = false;
            foreach (var player in rankExec.RankEventListDailyTopOdds)
            {
                string gameNameInfo = GetGameNameAllLang((GameServerCode)player.DailyTopOddsGame);
                string record = $"{no},{player.UserName},{player.DailyTopOdds},{player.DailyTopOddsBet},{player.DailyTopOddsWin},{gameNameInfo},{RankBonusNone}";

                if (!foundMe && player.UserUid == userUid)
                {
                    //玩家自己的 排名資訊
                    myData = record;
                    foundMe = true;
                }
                if (no <= RankTopCount)
                {
                    //榜單資訊 排名,玩家名稱,單日最高倍率,押分,贏分,遊戲名稱,預計獲得獎金
                    sb.Append(record + ";");
                }
                no++;
                if (no > RankTopCount && foundMe) break;
            }
            tList = sb.ToString().TrimEnd(';');
        }
        #endregion


        #region DB資料保存
        /// <summary>DB資料保存種類</summary>
        enum DBSaveType
        {
            None = 0,
            RankEventSetting = 1,
        }
        /// <summary>將資料寫入DB保存</summary>
        static void SaveDBCacheData(DBSaveType stype, string data)
        {
            string type = ((int)stype).ToString();
            string name = stype.ToString();

            Dictionary<string, string> updateData = new()
            {
                { "Type", "'" + type + "'" },
                { "Name", "'" + name + "'" },
                { "Data", "'" + data + "'" }
            };
            OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.insertUpdate, "DBCacheSaveData", updateData);
            lock (Program.opertionDBBoxs)
            {
                Program.opertionDBBoxs.Add(TempOperTionDBBox);
            }
        }
        /// <summary>將DB保存資料讀回</summary>
        static string LoadDBCacheData(DBSaveType stype)
        {
            try
            {
                MysqlAcess myAcc = MysqlAcess.GetInstance();
                string type = ((int)stype).ToString();
                var datalist = myAcc.select("DBCacheSaveData", "*", $"Type='{type}'", "", "1");
                if (datalist.Count > 0 && datalist[0].ContainsKey("Data"))
                {
                    return datalist[0]["Data"];
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine($"LoadDBCacheData Exception: {ex.ToString()}");
            }
            return "";
        }

        /// <summary>DB寫入佇列</summary>
        void PushToOperTionDBBoxInsert(string tablename, string col, List<string> valueList)
        {
            //批次更新語法
            // INSERT INTO table_name (col1, col2)
            // VALUES (v1_1, v1_2), (v2_1, v2_2), (v3_1, v3_2)

            //方法1:一次更新所有玩家資料
            //string val = string.Join(",", valueList);
            //string sql = $"INSERT INTO {tablename} ({col}) VALUES {val};";
            //OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.runsql, tablename, sql);
            //lock (Program.opertionDBBoxs)
            //{
            //    Program.opertionDBBoxs.Add(TempOperTionDBBox);
            //}

            //方法2:分批次更新, 避免SQL敘述過長
            const int BatchSize = 500;
            for (int i = 0; i < valueList.Count; i += BatchSize)
            {
                var batch = valueList.Skip(i).Take(BatchSize);
                string sql = $"INSERT INTO {tablename} ({col}) VALUES {string.Join(",", batch)};";
                OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.runsql, tablename, sql);
                lock (Program.opertionDBBoxs)
                {
                    Program.opertionDBBoxs.Add(TempOperTionDBBox);
                }
            }
        }
        /// <summary>DB寫入佇列</summary>
        void PushToOperTionDBBoxInsertUpdate(string tablename, string col, List<string> valueList, string update)
        {
            //批次更新語法
            // INSERT INTO table_name (col1, col2)
            // VALUES (v1_1, v1_2), (v2_1, v2_2), (v3_1, v3_2)
            // ON DUPLICATE KEY UPDATE
            // col1 = VALUES(col1),
            // col2 = VALUES(col2);

            //方法1:一次更新所有玩家資料
            //string val = string.Join(",", valueList);
            //string sql = $"INSERT INTO {tablename} ({col}) VALUES {val} ON DUPLICATE KEY UPDATE {update};";
            //OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.runsql, tablename, sql);
            //lock (Program.opertionDBBoxs)
            //{
            //    Program.opertionDBBoxs.Add(TempOperTionDBBox);
            //}

            //方法2:分批次更新, 避免SQL敘述過長
            const int BatchSize = 500;
            for (int i = 0; i < valueList.Count; i += BatchSize)
            {
                var batch = valueList.Skip(i).Take(BatchSize);
                string sql = $"INSERT INTO {tablename} ({col}) VALUES {string.Join(",", batch)} ON DUPLICATE KEY UPDATE {update};";
                OperTionDBBox TempOperTionDBBox = new OperTionDBBox(OpertionCode.runsql, tablename, sql);
                lock (Program.opertionDBBoxs)
                {
                    Program.opertionDBBoxs.Add(TempOperTionDBBox);
                }
            }
        }
        #endregion
    }
}
