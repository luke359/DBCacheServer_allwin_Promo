using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlX.XDevAPI.Relational;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBCacheServer
{
    public partial class CacheManeger
    {
        bool PRepoDebugFg = true;

        /// <summary>玩家出牌倍率分析</summary>
        class BetOddsInfo
        {
            /// <summary>次數</summary>
            public int Time { get; private set; }
            /// <summary>總押</summary>
            public double Bet { get; private set; }
            /// <summary>總得</summary>
            public double Win { get; private set; }

            public void Reset()
            {
                Time = 0;
                Bet = 0;
                Win = 0;
            }

            public void AddData(double bet, double win)
            {
                Time++;
                Bet += bet;
                Win += win;
            }
        }

        //玩家報告 #251203
        /// <summary>玩家報告</summary>
        class PlayerReport
        {
            /// <summary>玩家遊玩看門狗</summary>
            public int WatchCount { get; private set; }

            /// <summary>玩家UID</summary>
            public int UserUID { get; private set; }

            //以下為更新資料, 更新方式為 使用差值累加, 也就是以下各值會累加至DB, 更新後清除各值資料.
            /// <summary>押得更新鎖</summary>
            public object lockPlay = new object();
            /// <summary>押得更新旗號</summary>
            public bool UpdatePlay;
            /// <summary>總玩</summary>
            public int Play { get; private set; }
            /// <summary>總押</summary>
            public double Bet { get; private set; }
            /// <summary>總得</summary>
            public double Win { get; private set; }

            /// <summary>>獨立買總加買次數</summary>
            public int IndepPlay { get; private set; }
            /// <summary>>獨立買總押</summary>
            public double IndepBet { get; private set; }
            /// <summary>>獨立買總得</summary>
            public double IndepWin { get; private set; }

            /// <summary>>額外押注總加買次數</summary>
            public int ExBetPlay { get; private set; }
            /// <summary>>額外押注總押</summary>
            public double ExBetBet { get; private set; }
            /// <summary>>額外押注總得</summary>
            public double ExBetWin { get; private set; }

            #region 出牌倍率記錄
            /// <summary>倍率更新鎖</summary>
            public object lockBetOdds = new object();
            /// <summary>倍率 0</summary>
            public BetOddsInfo R0 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 小於1</summary>
            public BetOddsInfo R1 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 1~5</summary>
            public BetOddsInfo R5 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 6~10</summary>
            public BetOddsInfo R10 { get; private set; } = new BetOddsInfo();   
            /// <summary>倍率 11~30</summary>
            public BetOddsInfo R30 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 31~50</summary>
            public BetOddsInfo R50 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 51~100</summary>
            public BetOddsInfo R100 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 101~250</summary>
            public BetOddsInfo R250 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 251~500</summary>
            public BetOddsInfo R500 { get; private set; } = new BetOddsInfo();
            /// <summary>倍率 501以上</summary>
            public BetOddsInfo R500ab { get; private set; } = new BetOddsInfo();
            /// <summary>獨立買倍率 0.25</summary>
            public int IndepR025Times { get; private set; }
            /// <summary>獨立買倍率 0.50</summary>
            public int IndepR050Times { get; private set; }
            /// <summary>獨立買倍率 0.75</summary>
            public int IndepR075Times { get; private set; }
            /// <summary>獨立買倍率 1.00</summary>
            public int IndepR100Times { get; private set; }
            /// <summary>獨立買倍率 3.00</summary>
            public int IndepR300Times { get; private set; }
            /// <summary>獨立買倍率 5.00</summary>
            public int IndepR500Times { get; private set; }
            /// <summary>獨立買倍率 10.00</summary>
            public int IndepR1000Times { get; private set; }
            /// <summary>獨立買倍率 25.00</summary>
            public int IndepR2500Times { get; private set; }
            /// <summary>獨立買倍率 50.00</summary>
            public int IndepR5000Times { get; private set; }
            /// <summary>獨立買倍率 100.00</summary>
            public int IndepR10000Times { get; private set; }
            /// <summary>獨立買倍率 100.00 以上</summary>
            public int IndepR10000abTimes { get; private set; }
            #endregion

            /// <summary>開洗更新鎖</summary>
            public object lockKey = new object();
            /// <summary>開洗更新旗號</summary>
            public bool UpdateKey;
            /// <summary>總開</summary>
            public double KeyIn { get; private set; }
            /// <summary>總洗</summary>
            public double KeyOut { get; private set; }

            /// <summary>抽放水更新旗號</summary>
            public bool UpdateWater;
            /// <summary>總放水</summary>
            public double WaterIn { get; private set; }
            /// <summary>總抽水</summary>
            public double WaterOut { get; private set; }
            /// <summary>紅包總放水</summary>
            public double RedBonusWaterIn { get; private set; }
            /// <summary>紅包總抽水</summary>
            public double RedBonusWaterOut { get; private set; }

            /// <summary>H5開洗更新鎖</summary>
            public object lockH5Key = new object();
            /// <summary>H5進出更新旗號</summary>
            public bool UpdateH5;
            /// <summary>H5總進 (H5 -> lgt)</summary>
            public double H5KeyIn { get; private set; }
            /// <summary>H5總出 (lgt -> H5)</summary>
            public double H5KeyOut { get; private set; }

            public PlayerReport(int useruid)
            {
                UserUID = useruid;
                Reset();
            }


            /// <summary>玩家報告 更新押得紀錄</summary>
            public void Betin(double bet, double win, int playTimes)
            {
                lock (lockPlay)
                {
                    Bet += bet;
                    Win += win;
                    Play += playTimes;
                    UpdatePlay = true;
                }
            }
            /// <summary>玩家報告 更新獨立買押得紀錄</summary>
            public void IndepBetin(double bet, double win, int playTimes)
            {
                lock (lockPlay)
                {
                    IndepBet += bet;
                    IndepWin += win;
                    IndepPlay += playTimes;
                    UpdatePlay = true;
                }
            }
            /// <summary>玩家報告 更新額外押注押得紀錄</summary>
            public void ExBetBetin(double bet, double win, int playTimes)
            {
                lock (lockPlay)
                {
                    ExBetBet += bet;
                    ExBetWin += win;
                    ExBetPlay += playTimes;
                    UpdatePlay = true;
                }
            }

            /// <summary>玩家報告 更新押得比率</summary>
            public void BetOddsRatio(double bet, double win)
            {
                lock (lockBetOdds)
                {
                    if (win == 0 || bet == 0)
                    {
                        R0.AddData(bet, 0);
                        return;
                    }

                    double ratio = win / bet;

                    if(ratio < 1)
                    {
                        R1.AddData(bet, win);
                    }
                    else if(ratio <= 5)
                    {
                        R5.AddData(bet, win);
                    }
                    else if (ratio <= 10)
                    {
                        R10.AddData(bet, win);
                    }
                    else if (ratio <= 30)
                    {
                        R30.AddData(bet, win);
                    }
                    else if (ratio <= 50)
                    {
                        R50.AddData(bet, win);
                    }
                    else if (ratio <= 100)
                    {
                        R100.AddData(bet, win);
                    }
                    else if (ratio <= 250)
                    {
                        R250.AddData(bet, win);
                    }
                    else if (ratio <= 500)
                    {
                        R500.AddData(bet, win);
                    }
                    else
                    {
                        R500ab.AddData(bet, win);
                    }
                }
            }
            /// <summary>玩家報告 更新獨立買押得比率</summary>
            public void IndepBetOddsRatio(double bet, double win)
            {
                if (bet == 0)
                    return;

                lock (lockBetOdds)
                {
                    double ratio = win / bet;

                    if (ratio <= 0.25) //獨立倍率<=0.25倍
                        IndepR025Times++;
                    else if (ratio <= 0.50) //獨立倍率<=0.50倍
                        IndepR050Times++;
                    else if (ratio <= 0.75) //獨立倍率<=0.75倍
                        IndepR075Times++;
                    else if (ratio <= 1.00) //獨立倍率<=1倍
                        IndepR100Times++;
                    else if (ratio <= 3.00) //獨立倍率<=3倍
                        IndepR300Times++;
                    else if (ratio <= 5.00) //獨立倍率<=5倍
                        IndepR500Times++;
                    else if (ratio <= 10.00) //獨立倍率<=10倍
                        IndepR1000Times++;
                    else if (ratio <= 25.00) //獨立倍率<=25倍
                        IndepR2500Times++;
                    else if (ratio <= 50.00) //獨立倍率<=50倍
                        IndepR5000Times++;
                    else if (ratio <= 100.00) //獨立倍率<=100倍
                        IndepR10000Times++;
                    else //獨立倍率>100倍以上
                        IndepR10000abTimes++;
                }
            }
            /// <summary>玩家報告 更新開洗紀錄</summary>
            public void KeyInOut(double key)
            {
                lock (lockKey)
                {
                    if (key == 0) return;

                    if (key > 0)
                    {
                        KeyIn += key;
                    }
                    else
                    {
                        KeyOut += -key;
                    }
                    UpdateKey = true;
                }
            }
            /// <summary>玩家報告 更新抽放水紀錄</summary>
            public void WaterInOut(double waterin, double waterout, double redbonuswaterin, double redbonuswaterout)
            {
                WaterIn += waterin;
                WaterOut += waterout;
                RedBonusWaterIn += redbonuswaterin;
                RedBonusWaterOut += redbonuswaterout;
                UpdateWater = true;
            }
            /// <summary>玩家報告 更新H5轉入紀錄</summary>
            public void H5KeyInPR(double key)
            {
                if (key == 0) return;

                lock (lockH5Key)
                {
                    H5KeyIn += key;
                    UpdateH5 = true;
                }
            }
            /// <summary>玩家報告 更新轉出至H5紀錄</summary>
            public void H5KeyOutPR(double key)
            {
                if (key == 0) return;

                lock (lockH5Key)
                {
                    H5KeyOut += key;
                    UpdateH5 = true;
                }
            }

            /// <summary>重置全部 UpdateData</summary>
            void Reset()
            {
                WatchCount = 0;
                ClearDataPlay();
                ClearBetOddsData();
                ClearIndepBetOddsData();
                ClearDataH5();
                ClearDataKey();
                ClearDataWater();
            }
            /// <summary>重置押得 UpdateData</summary>
            public void ClearDataPlay()
            {
                lock (lockPlay)
                {
                    Play = 0;
                    Bet = 0;
                    Win = 0;
                    IndepBet = 0;
                    IndepWin = 0;
                    IndepPlay = 0;
                    ExBetBet = 0;
                    ExBetWin = 0;
                    ExBetPlay = 0;
                    UpdatePlay = false;
                }
            }
            /// <summary>重置常規遊戲倍率 UpdateData</summary>
            public void ClearBetOddsData()
            {
                lock (lockBetOdds)
                {
                    R0.Reset();
                    R1.Reset();
                    R5.Reset();
                    R10.Reset();
                    R30.Reset();
                    R50.Reset();
                    R100.Reset();
                    R250.Reset();
                    R500.Reset();
                    R500ab.Reset();
                }
            }
            /// <summary>重置獨立買倍率 UpdateData</summary>
            public void ClearIndepBetOddsData()
            {
                lock (lockBetOdds)
                {
                    IndepR025Times = 0;
                    IndepR050Times = 0;
                    IndepR075Times = 0;
                    IndepR100Times = 0;
                    IndepR300Times = 0;
                    IndepR500Times = 0;
                    IndepR1000Times = 0;
                    IndepR2500Times = 0;
                    IndepR5000Times = 0;
                    IndepR10000Times = 0;
                    IndepR10000abTimes = 0;
                }
            }
            /// <summary>重置開洗 UpdateData</summary>
            public void ClearDataKey()
            {
                lock (lockKey)
                {
                    KeyIn = 0;
                    KeyOut = 0;
                    UpdateKey = false;
                }
            }
            /// <summary>重置抽放水 UpdateData</summary>
            public void ClearDataWater()
            {
                WaterIn = 0;
                WaterOut = 0;
                RedBonusWaterIn = 0;
                RedBonusWaterOut = 0;
                UpdateWater = false;
            }
            /// <summary>重置H5進出 UpdateData</summary>
            public void ClearDataH5()
            {
                lock (lockH5Key)
                {
                    H5KeyIn = 0;
                    H5KeyOut = 0;
                    UpdateH5 = false;
                }
            }

            const int WatchDogMax = 360; //120約等於10分鐘 720約等於1小時
            /// <summary>重置玩家遊玩看門狗</summary>
            public void WatchDogReset()
            {
                WatchCount = 0;
            }
            /// <summary>玩家遊玩看門狗累加</summary>
            public bool WatchDogCount()
            {
                WatchCount++;

                if (WatchCount > WatchDogMax)
                //if (WatchCount > 6) //測試用 6約等於30秒
                {
                    return true; //玩家太久沒更新資料
                }
                return false;
            }
            /// <summary>Update後即刪除</summary>
            public void WatchDogOverCount()
            {
                WatchCount = WatchDogMax + 1;
            }
            /// <summary>讀取DB資料</summary>
            //void GetDBData(Dictionary<string, string> datalist)
            //{
            //    Play = Convert.ToInt32(datalist[nameof(Play)]);
            //    Bet = Convert.ToDouble(datalist[nameof(Bet)]);
            //    Win = Convert.ToDouble(datalist[nameof(Win)]);
            //    KeyIn = Convert.ToDouble(datalist[nameof(KeyIn)]);
            //    KeyOut = Convert.ToDouble(datalist[nameof(KeyOut)]);
            //    WaterIn = Convert.ToDouble(datalist[nameof(WaterIn)]);
            //    WaterOut = Convert.ToDouble(datalist[nameof(WaterOut)]);
            //    RedBonusWaterIn = Convert.ToDouble(datalist[nameof(RedBonusWaterIn)]);
            //    RedBonusWaterOut = Convert.ToDouble(datalist[nameof(RedBonusWaterOut)]);
            //}
        }
        /// <summary>玩家報告列表</summary>
        Dictionary<int, PlayerReport> playerReportList = new();


        #region 玩家報告紀錄 & DB更新
        /// <summary>玩家報告 Update 旗號</summary>
        bool ReportUpDateFg = false;
        /// <summary>取得玩家報告 Update 實體</summary>
        PlayerReport GetPlayerReport(int userUid)
        {
            lock (playerReportList)
            {
                if (playerReportList.ContainsKey(userUid))
                {
                    return playerReportList[userUid];
                }
            }

            //檢查DB有無此玩家資料
            var datalist = myAcess.select("UserInOutReport", "UserUID", $"UserUID = {userUid}");
            if (datalist.Count <= 0)
            {
                UserData user = Getuser(userUid);
                if (user == null)
                {
                    MyConsole.WriteLine($"！Error！ : UserInOutReport無此玩家資料 UserUID={userUid}"); //UNDONE: TEST
                    return null;
                }

                //新增玩家資料
                if (PRepoDebugFg) MyConsole.WriteLine($"UserInOutReport新增玩家資料 UserUID={userUid}"); //UNDONE: TEST
                var tempdata = new Dictionary<string, string>();
                tempdata.Add("userUid", "'" + userUid.ToString() + "'");
                tempdata.Add("EntityId", "'" + user.EntityId.ToString() + "'");

                int no = myAcess.insert("UserInOutReport", tempdata);
                if (no <= 0)
                {
                    //新增失敗, 再次確認是否已有資料 (DB一定要有玩家紀錄, 否則update時會發生錯誤)
                    datalist = myAcess.select("UserInOutReport", "UserUID", $"UserUID = {userUid}");
                    if (datalist.Count <= 0)
                    {
                        MyConsole.WriteLine($"！Error！ : UserInOutReport新增玩家資料失敗 UserUID={userUid}"); //UNDONE: TEST
                        return null;
                    }
                }
            }

            if (PRepoDebugFg) MyConsole.WriteLine($"    ♦ PlayerReport 加入更新玩家[{Getuser(userUid).UserID}]");

            //UserInOutReport是以傳送累加值方式更新, 所以這裡不需要讀回現有DB資料
            PlayerReport report = new(userUid);
            lock (playerReportList)
            {
                playerReportList.Add(userUid, report);
            }
            return report;
        }

        /// <summary>清除玩家報告</summary>
        public void RemovePlayerReport(int userUid)
        {
            lock (playerReportList)
            {
                if (playerReportList.ContainsKey(userUid))
                {
                    playerReportList.Remove(userUid);
                }
            }
        }

        /// <summary>玩家報告 押得紀錄</summary>
        void PlayerReportBet(int userUid, double bet, double win, int playTimes, GameTypeCode gameTypeCode, PlayGameMode gameMode)
        {
            var report = GetPlayerReport(userUid);
            if (report == null)
            {
                if (PRepoDebugFg) MyConsole.WriteLine($"PlayerReportBet 無此玩家資料 UserUID={userUid}");
                return;
            }

            if (gameMode == PlayGameMode.ExBet) //額外押注模式
            {
                report.ExBetBetin(bet, win, playTimes);
            }
            if (gameMode == PlayGameMode.Indep) //獨立買模式
            {
                report.IndepBetin(bet, win, playTimes);
            }
            else
            {
                report.Betin(bet, win, playTimes);
            }
            ReportUpDateFg = true;
        }
        /// <summary>玩家報告 遊玩遊戲列表</summary>
        void PlayerReportGameList(UserData userData, GameServerCode gameServer, PlayGameMode gameMode)
        {
            var report = GetPlayerReport(userData.UserUID);
            if (report == null)
            {   
                if (PRepoDebugFg) MyConsole.WriteLine($"PlayerReportGameList 無此玩家資料 UserUID={userData.UserUID}");
                return;
            }

            string gameServerStr = null;

            if (gameMode == PlayGameMode.ExBet) //額外押注模式
            {
                if (userData.CheckExBetDayPlayGame(gameServer))
                {
                    gameServerStr = userData.GetExBetDayPlayGamesStr();
                    UpdateDBDayPlayGame(userData.UserUID, gameServerStr, gameMode);
                    //if (PRepoDebugFg) MyConsole.WriteLine($"    新增遊玩遊戲: UserUID={userData.UserUID} GameServer={gameServer} GameServerStr={gameServerStr}");
                }
            }
            if (gameMode == PlayGameMode.Indep) //獨立買模式
            {
                if (userData.CheckIndepDayPlayGame(gameServer))
                {
                    gameServerStr = userData.GetIndepDayPlayGamesStr();
                    UpdateDBDayPlayGame(userData.UserUID, gameServerStr, gameMode);
                    //if (PRepoDebugFg) MyConsole.WriteLine($"    新增遊玩遊戲: UserUID={userData.UserUID} GameServer={gameServer} GameServerStr={gameServerStr}");
                }
            }
            else
            {
                if (userData.CheckDayPlayGame(gameServer))
                {
                    gameServerStr = userData.GetDayPlayGamesStr();
                    UpdateDBDayPlayGame(userData.UserUID, gameServerStr, gameMode);
                    //if (PRepoDebugFg) MyConsole.WriteLine($"    新增遊玩遊戲: UserUID={userData.UserUID} GameServer={gameServer} GameServerStr={gameServerStr}");
                }
            }
        }
        /// <summary>更新DB玩家報告 遊玩遊戲列表</summary>
        void UpdateDBDayPlayGame(int userUid, string val, PlayGameMode gameMode)
        {
            string columnName = null;
            if (gameMode == PlayGameMode.Normal) columnName = "DayPlayGames";
            else if (gameMode == PlayGameMode.Indep) columnName = "IndepDayPlayGames";
            else if (gameMode == PlayGameMode.ExBet) columnName = "ExBetDayPlayGames";
            else return;
            Dictionary<string, string> updata = new() { { columnName, $"'{val}'" } };
            OperTionDBBox TempOperTionDBBoxRD = new OperTionDBBox(OpertionCode.update2, "UserInOutReport", updata, $"UserUID={userUid}");
            lock (Program.opertionDBBoxs)
            {
                Program.opertionDBBoxs.Add(TempOperTionDBBoxRD);
            }
        }


        /// <summary>玩家報告 押得比率</summary>
        void PlayerReportBetOddsRatio(int userUid, double bet, double win, GameTypeCode gameTypeCode, PlayGameMode gameMode)
        {
            if (gameTypeCode == GameTypeCode.Slot) //僅老虎機要記錄
            {
                var report = GetPlayerReport(userUid);
                if (report == null)
                {
                    if (PRepoDebugFg) MyConsole.WriteLine($"PlayerReportBetOddsRatio 無此玩家資料 UserUID={userUid}");
                    return;
                }

                if (gameMode == PlayGameMode.Indep) //獨立買模式
                {
                    report.IndepBetOddsRatio(bet, win);
                }
                else
                {
                    report.BetOddsRatio(bet, win);
                }
                ReportUpDateFg = true;
            }
        }
        /// <summary>玩家報告 開洗紀錄</summary>
        void PlayerReportKeyInOut(int userUid, double key)
        {
            var report = GetPlayerReport(userUid);
            if (report == null)
            {
                if (PRepoDebugFg) MyConsole.WriteLine($"PlayerReportKeyInOut 無此玩家資料 UserUID={userUid}");
                return;
            }
            report.KeyInOut(key);
            ReportUpDateFg = true;

            if (Getuser(userUid).Usersituation == 0)
            {
                //如果玩家不在線上, 更新玩家報告後即刪除資料
                report.WatchDogOverCount();
            }
        }
        /// <summary>玩家報告 抽放水紀錄</summary>
        public void PlayerReportWaterInOut(int userUid, double waterin, double waterout, double redbonuswaterin, double redbonuswaterout)
        {
            var report = GetPlayerReport(userUid);
            if (report == null)
            {
                if (PRepoDebugFg) MyConsole.WriteLine($"PlayerReportWaterInOut 無此玩家資料 UserUID={userUid}");
                return;
            }
            report.WaterInOut(waterin, waterout, redbonuswaterin, redbonuswaterout);
            ReportUpDateFg = true;

            if (Getuser(userUid).Usersituation == 0)
            {
                //如果玩家不在線上, 更新玩家報告後即刪除資料
                report.WatchDogOverCount();
            }
        }
        /// <summary>玩家報告 H5轉回本地 紀錄</summary>
        void PlayerReportH5In(int userUid, double keyValue)
        {
            var report = GetPlayerReport(userUid);
            if (report == null)
            {
                if (PRepoDebugFg) MyConsole.WriteLine($"PlayerReportH5In 無此玩家資料 UserUID={userUid}");
                return;
            }
            report.H5KeyInPR(keyValue);
            ReportUpDateFg = true;

            UserData userdata = Getuser(userUid);
            userdata.H5InRec(keyValue);
            AddUserKyRecdataUpdateList(userUid);
        }
        /// <summary>玩家報告 本地轉出至H5 紀錄</summary>
        void PlayerReportH5Out(int userUid, double keyValue)
        {
            var report = GetPlayerReport(userUid);
            if (report == null)
            {
                if (PRepoDebugFg) MyConsole.WriteLine($"PlayerReportH5Out 無此玩家資料 UserUID={userUid}");
                return;
            }
            report.H5KeyOutPR(keyValue);
            ReportUpDateFg = true;

            UserData userdata = Getuser(userUid);
            userdata.H5OutRec(keyValue);
            AddUserKyRecdataUpdateList(userUid);
        }

        /// <summary>玩家報告 SQL更新字串</summary>
        public string GetPlayerReportSqlString()
        {
            if (playerReportList.Count <= 0) { return null; }
            if(ReportUpDateFg == false) { return null; }

            StringBuilder datUser = new StringBuilder();

            StringBuilder datBet = new StringBuilder();
            StringBuilder datWin = new StringBuilder();
            StringBuilder datPlay = new StringBuilder();

            StringBuilder indepBet = new StringBuilder();
            StringBuilder indepWin = new StringBuilder();
            StringBuilder indepPlay = new StringBuilder();

            StringBuilder exBetBet = new StringBuilder();
            StringBuilder exBetWin = new StringBuilder();
            StringBuilder exBetPlay = new StringBuilder();

            StringBuilder datKeyIn = new StringBuilder();
            StringBuilder datKeyOut = new StringBuilder();
            StringBuilder datWaterIn = new StringBuilder();
            StringBuilder datWaterOut = new StringBuilder();
            StringBuilder datRedBonusWaterIn = new StringBuilder();
            StringBuilder datRedBonusWaterOut = new StringBuilder();

            StringBuilder datH5In = new StringBuilder();
            StringBuilder datH5Out = new StringBuilder();


            StringBuilder r0Bet = new StringBuilder();
            StringBuilder r0Times = new StringBuilder();

            StringBuilder r1Bet = new StringBuilder();
            StringBuilder r1Win = new StringBuilder();
            StringBuilder r1Times = new StringBuilder();

            StringBuilder r5Bet = new StringBuilder();
            StringBuilder r5Win = new StringBuilder();
            StringBuilder r5Times = new StringBuilder();

            StringBuilder r10Bet = new StringBuilder();
            StringBuilder r10Win = new StringBuilder();
            StringBuilder r10Times = new StringBuilder();

            StringBuilder r30Bet = new StringBuilder();
            StringBuilder r30Win = new StringBuilder();
            StringBuilder r30Times = new StringBuilder();

            StringBuilder r50Bet = new StringBuilder();
            StringBuilder r50Win = new StringBuilder();
            StringBuilder r50Times = new StringBuilder();

            StringBuilder r100Bet = new StringBuilder();
            StringBuilder r100Win = new StringBuilder();
            StringBuilder r100Times = new StringBuilder();

            StringBuilder r250Bet = new StringBuilder();
            StringBuilder r250Win = new StringBuilder();
            StringBuilder r250Times = new StringBuilder();

            StringBuilder r500Bet = new StringBuilder();
            StringBuilder r500Win = new StringBuilder();
            StringBuilder r500Times = new StringBuilder();

            StringBuilder r500abBet = new StringBuilder();
            StringBuilder r500abWin = new StringBuilder();
            StringBuilder r500abTimes = new StringBuilder();

            StringBuilder indepR025Times = new StringBuilder();
            StringBuilder indepR050Times = new StringBuilder();
            StringBuilder indepR075Times = new StringBuilder();
            StringBuilder indepR100Times = new StringBuilder();
            StringBuilder indepR300Times = new StringBuilder();
            StringBuilder indepR500Times = new StringBuilder();
            StringBuilder indepR1000Times = new StringBuilder();
            StringBuilder indepR2500Times = new StringBuilder();
            StringBuilder indepR5000Times = new StringBuilder();
            StringBuilder indepR10000Times = new StringBuilder();
            StringBuilder indepR10000abTimes = new StringBuilder();

            int userCnt = 0;
            int playCnt = 0;
            int keyCnt = 0;
            int waterCnt = 0;
            int h5Cnt = 0;

            lock (playerReportList)
            {
                List<int> playerToRemove = new List<int>();

                foreach (KeyValuePair<int, PlayerReport> info in playerReportList)
                {
                    string useruid = info.Key.ToString();
                    bool updateFg = false;

                    if (info.Value.UpdatePlay)
                    {
                        lock (info.Value.lockPlay)
                        {
                            if (info.Value.Play > 0)
                            {
                                datBet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.Bet, RankMoneyPrecision)}");
                                if (info.Value.Win > 0) datWin.Append($" WHEN {useruid} THEN {Math.Round(info.Value.Win, RankMoneyPrecision)}");
                                datPlay.Append($" WHEN {useruid} THEN {info.Value.Play}");
                            }

                            if (info.Value.IndepPlay > 0)
                            {
                                indepBet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.IndepBet, RankMoneyPrecision)}");
                                if (info.Value.IndepWin > 0) indepWin.Append($" WHEN {useruid} THEN {Math.Round(info.Value.IndepWin, RankMoneyPrecision)}");
                                indepPlay.Append($" WHEN {useruid} THEN {info.Value.IndepPlay}");
                            }

                            if (info.Value.ExBetPlay > 0)
                            {
                                exBetBet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.ExBetBet, RankMoneyPrecision)}");
                                if (info.Value.ExBetWin > 0) exBetWin.Append($" WHEN {useruid} THEN {Math.Round(info.Value.ExBetWin, RankMoneyPrecision)}");
                                exBetPlay.Append($" WHEN {useruid} THEN {info.Value.ExBetPlay}");
                            }

                            info.Value.ClearDataPlay();
                        }
                        lock (info.Value.lockBetOdds)
                        {
                            #region 出牌倍率記錄
                            if (info.Value.R0.Time > 0)
                            {
                                r0Times.Append($" WHEN {useruid} THEN {info.Value.R0.Time}");
                                r0Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R0.Bet, RankMoneyPrecision)}");
                                //0倍沒有得分
                            }
                            if (info.Value.R1.Time > 0)
                            {
                                r1Times.Append($" WHEN {useruid} THEN {info.Value.R1.Time}");
                                r1Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R1.Bet, RankMoneyPrecision)}");
                                r1Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R1.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R5.Time > 0)
                            {
                                r5Times.Append($" WHEN {useruid} THEN {info.Value.R5.Time}");
                                r5Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R5.Bet, RankMoneyPrecision)}");
                                r5Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R5.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R10.Time > 0)
                            {
                                r10Times.Append($" WHEN {useruid} THEN {info.Value.R10.Time}");
                                r10Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R10.Bet, RankMoneyPrecision)}");
                                r10Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R10.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R30.Time > 0)
                            {
                                r30Times.Append($" WHEN {useruid} THEN {info.Value.R30.Time}");
                                r30Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R30.Bet, RankMoneyPrecision)}");
                                r30Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R30.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R50.Time > 0)
                            {
                                r50Times.Append($" WHEN {useruid} THEN {info.Value.R50.Time}");
                                r50Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R50.Bet, RankMoneyPrecision)}");
                                r50Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R50.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R100.Time > 0)
                            {
                                r100Times.Append($" WHEN {useruid} THEN {info.Value.R100.Time}");
                                r100Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R100.Bet, RankMoneyPrecision)}");
                                r100Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R100.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R250.Time > 0)
                            {
                                r250Times.Append($" WHEN {useruid} THEN {info.Value.R250.Time}");
                                r250Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R250.Bet, RankMoneyPrecision)}");
                                r250Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R250.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R500.Time > 0)
                            {
                                r500Times.Append($" WHEN {useruid} THEN {info.Value.R500.Time}");
                                r500Bet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R500.Bet, RankMoneyPrecision)}");
                                r500Win.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R500.Win, RankMoneyPrecision)}");
                            }
                            if (info.Value.R500ab.Time > 0)
                            {
                                r500abTimes.Append($" WHEN {useruid} THEN {info.Value.R500ab.Time}");
                                r500abBet.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R500ab.Bet, RankMoneyPrecision)}");
                                r500abWin.Append($" WHEN {useruid} THEN {Math.Round(info.Value.R500ab.Win, RankMoneyPrecision)}");
                            }
                            info.Value.ClearBetOddsData();
                            #endregion

                            #region 獨立買出牌倍率記錄
                            if (info.Value.IndepR025Times > 0)
                            {
                                indepR025Times.Append($" WHEN {useruid} THEN {info.Value.IndepR025Times}");
                            }
                            if (info.Value.IndepR050Times > 0)
                            {
                                indepR050Times.Append($" WHEN {useruid} THEN {info.Value.IndepR050Times}");
                            }
                            if (info.Value.IndepR075Times > 0)
                            {
                                indepR075Times.Append($" WHEN {useruid} THEN {info.Value.IndepR075Times}");
                            }
                            if (info.Value.IndepR100Times > 0)
                            {
                                indepR100Times.Append($" WHEN {useruid} THEN {info.Value.IndepR100Times}");
                            }
                            if (info.Value.IndepR300Times > 0)
                            {
                                indepR300Times.Append($" WHEN {useruid} THEN {info.Value.IndepR300Times}");
                            }
                            if (info.Value.IndepR500Times > 0)
                            {
                                indepR500Times.Append($" WHEN {useruid} THEN {info.Value.IndepR500Times}");
                            }
                            if (info.Value.IndepR1000Times > 0)
                            {
                                indepR1000Times.Append($" WHEN {useruid} THEN {info.Value.IndepR1000Times}");
                            }
                            if (info.Value.IndepR2500Times > 0)
                            {
                                indepR2500Times.Append($" WHEN {useruid} THEN {info.Value.IndepR2500Times}");
                            }
                            if (info.Value.IndepR5000Times > 0)
                            {
                                indepR5000Times.Append($" WHEN {useruid} THEN {info.Value.IndepR5000Times}");
                            }
                            if (info.Value.IndepR10000Times > 0)
                            {
                                indepR10000Times.Append($" WHEN {useruid} THEN {info.Value.IndepR10000Times}");
                            }
                            if (info.Value.IndepR10000abTimes > 0)
                            {
                                indepR10000abTimes.Append($" WHEN {useruid} THEN {info.Value.IndepR10000abTimes}");
                            }
                            info.Value.ClearIndepBetOddsData();
                            #endregion
                        }

                        playCnt++;
                        updateFg = true;
                    }

                    if (info.Value.UpdateKey)
                    {
                        lock (info.Value.lockKey)
                        {
                            if (info.Value.KeyIn > 0) datKeyIn.Append($" WHEN {useruid} THEN {Math.Round(info.Value.KeyIn, RankMoneyPrecision)}");
                            if (info.Value.KeyOut > 0) datKeyOut.Append($" WHEN {useruid} THEN {Math.Round(info.Value.KeyOut, RankMoneyPrecision)}");
                            info.Value.ClearDataKey();
                        }
                        keyCnt++;
                        updateFg = true;
                    }

                    if (info.Value.UpdateWater)
                    {
                        if (info.Value.WaterIn > 0) datWaterIn.Append($" WHEN {useruid} THEN {Math.Round(info.Value.WaterIn, RankMoneyPrecision)}");
                        if (info.Value.WaterOut > 0) datWaterOut.Append($" WHEN {useruid} THEN {Math.Round(info.Value.WaterOut, RankMoneyPrecision)}");
                        if (info.Value.RedBonusWaterIn > 0) datRedBonusWaterIn.Append($" WHEN {useruid} THEN {Math.Round(info.Value.RedBonusWaterIn, RankMoneyPrecision)}");
                        if (info.Value.RedBonusWaterOut > 0) datRedBonusWaterOut.Append($" WHEN {useruid} THEN {Math.Round(info.Value.RedBonusWaterOut, RankMoneyPrecision)}");
                        info.Value.ClearDataWater();
                        waterCnt++;
                        updateFg = true;
                    }

                    if (info.Value.UpdateH5)
                    {
                        lock (info.Value.lockH5Key)
                        {
                            if (info.Value.H5KeyIn > 0) datH5In.Append($" WHEN {useruid} THEN {Math.Round(info.Value.H5KeyIn, RankMoneyPrecision)}");
                            if (info.Value.H5KeyOut > 0) datH5Out.Append($" WHEN {useruid} THEN {Math.Round(info.Value.H5KeyOut, RankMoneyPrecision)}");
                            info.Value.ClearDataH5();
                        }
                        h5Cnt++;
                        updateFg = true;
                    }

                    if (updateFg)
                    {
                        //有更新, 看門狗歸零
                        info.Value.WatchDogReset();

                        if (userCnt > 0) datUser.Append("," + useruid);
                        else datUser.Append(useruid);

                        userCnt++;
                        if (userCnt > 300) //單次更新限制300筆
                        {
                            break;
                        }
                    }
                    else
                    {
                        //無更新, 看門狗累加
                        if (info.Value.WatchDogCount())
                        {
                            //玩家太久沒更新資料, 從列表移除玩家
                            playerToRemove.Add(info.Key);
                        }
                    }
                }

                //移除無更新玩家
                if (playerToRemove.Count > 0)
                {
                    foreach (var uid in playerToRemove)
                    {
                        playerReportList.Remove(uid);
                        if (PRepoDebugFg) MyConsole.WriteLine($"    ♦ PlayerReport 移除無更新玩家[{Getuser(uid).UserID}]!");
                    }
                }
            }

            if (userCnt <= 0)
            {
                ReportUpDateFg = false; //沒有玩家需要更新
                return null;
            }


            //開始製作SQL更新字串
            string sql = "UPDATE UserInOutReport SET ";

            if (playCnt > 0)
            {
                #region 常規遊戲押得記錄
                if (datPlay.Length > 0)
                {
                    string tmp = datPlay.ToString();
                    sql += $" TotalPlay = TotalPlay + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" DayPlay = DayPlay + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" WeekPlay = WeekPlay + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" MonthPlay = MonthPlay + CASE UserUID {tmp} ELSE 0 END,";
                }
                if (datBet.Length > 0)
                {
                    string tmp2 = datBet.ToString();
                    sql += $" TotalBet = TotalBet + CASE UserUID {tmp2} ELSE 0 END,";
                    sql += $" DayBet = DayBet + CASE UserUID {tmp2} ELSE 0 END,";
                    sql += $" WeekBet = WeekBet + CASE UserUID {tmp2} ELSE 0 END,";
                    sql += $" MonthBet = MonthBet + CASE UserUID {tmp2} ELSE 0 END,";
                }
                if (datWin.Length > 0)
                {
                    string tmp3 = datWin.ToString();
                    sql += $" TotalWin = TotalWin + CASE UserUID {tmp3} ELSE 0 END,";
                    sql += $" DayWin = DayWin + CASE UserUID {tmp3} ELSE 0 END,";
                    sql += $" WeekWin = WeekWin + CASE UserUID {tmp3} ELSE 0 END,";
                    sql += $" MonthWin = MonthWin + CASE UserUID {tmp3} ELSE 0 END,";
                }
                #endregion

                #region 獨立買押得記錄
                if (indepPlay.Length > 0)
                {
                    string itmp = indepPlay.ToString();
                    sql += $" IndepTotalPlay = IndepTotalPlay + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepDayPlay = IndepDayPlay + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepWeekPlay = IndepWeekPlay + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepMonthPlay = IndepMonthPlay + CASE UserUID {itmp} ELSE 0 END,";
                }
                if (indepBet.Length > 0)
                {
                    string itmp = indepBet.ToString();
                    sql += $" IndepTotalBet = IndepTotalBet + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepDayBet = IndepDayBet + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepWeekBet = IndepWeekBet + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepMonthBet = IndepMonthBet + CASE UserUID {itmp} ELSE 0 END,";
                }
                if (indepWin.Length > 0)
                {
                    string itmp = indepWin.ToString();
                    sql += $" IndepTotalWin = IndepTotalWin + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepDayWin = IndepDayWin + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepWeekWin = IndepWeekWin + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" IndepMonthWin = IndepMonthWin + CASE UserUID {itmp} ELSE 0 END,";
                }
                #endregion

                #region 額外押注押得記錄
                if (exBetPlay.Length > 0)
                {
                    string itmp = exBetPlay.ToString();
                    sql += $" ExBetTotalPlay = ExBetTotalPlay + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetDayPlay = ExBetDayPlay + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetWeekPlay = ExBetWeekPlay + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetMonthPlay = ExBetMonthPlay + CASE UserUID {itmp} ELSE 0 END,";
                }
                if (exBetBet.Length > 0)
                {
                    string itmp = exBetBet.ToString();
                    sql += $" ExBetTotalBet = ExBetTotalBet + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetDayBet = ExBetDayBet + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetWeekBet = ExBetWeekBet + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetMonthBet = ExBetMonthBet + CASE UserUID {itmp} ELSE 0 END,";
                }
                if (exBetWin.Length > 0)
                {
                    string itmp = exBetWin.ToString();
                    sql += $" ExBetTotalWin = ExBetTotalWin + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetDayWin = ExBetDayWin + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetWeekWin = ExBetWeekWin + CASE UserUID {itmp} ELSE 0 END,";
                    sql += $" ExBetMonthWin = ExBetMonthWin + CASE UserUID {itmp} ELSE 0 END,";
                }
                #endregion

                #region 出牌倍率記錄
                if (r0Times.Length > 0)
                    sql += $" R0_Times = R0_Times + CASE UserUID {r0Times.ToString()} ELSE 0 END,";
                if (r0Bet.Length > 0)
                    sql += $" R0_Bet = R0_Bet + CASE UserUID {r0Bet.ToString()} ELSE 0 END,";

                if (r1Times.Length > 0)
                    sql += $" R1_Times = R1_Times + CASE UserUID {r1Times.ToString()} ELSE 0 END,";
                if (r1Bet.Length > 0)
                    sql += $" R1_Bet = R1_Bet + CASE UserUID {r1Bet.ToString()} ELSE 0 END,";
                if (r1Win.Length > 0)
                    sql += $" R1_Win = R1_Win + CASE UserUID {r1Win.ToString()} ELSE 0 END,";

                if (r5Times.Length > 0)
                    sql += $" R5_Times = R5_Times + CASE UserUID {r5Times.ToString()} ELSE 0 END,";
                if (r5Bet.Length > 0)
                    sql += $" R5_Bet = R5_Bet + CASE UserUID {r5Bet.ToString()} ELSE 0 END,";
                if (r5Win.Length > 0)
                    sql += $" R5_Win = R5_Win + CASE UserUID {r5Win.ToString()} ELSE 0 END,";

                if (r10Times.Length > 0)
                    sql += $" R10_Times = R10_Times + CASE UserUID {r10Times.ToString()} ELSE 0 END,";
                if (r10Bet.Length > 0)
                    sql += $" R10_Bet = R10_Bet + CASE UserUID {r10Bet.ToString()} ELSE 0 END,";
                if (r10Win.Length > 0)
                    sql += $" R10_Win = R10_Win + CASE UserUID {r10Win.ToString()} ELSE 0 END,";

                if (r30Times.Length > 0)
                    sql += $" R30_Times = R30_Times + CASE UserUID {r30Times.ToString()} ELSE 0 END,";
                if (r30Bet.Length > 0)
                    sql += $" R30_Bet = R30_Bet + CASE UserUID {r30Bet.ToString()} ELSE 0 END,";
                if (r30Win.Length > 0)
                    sql += $" R30_Win = R30_Win + CASE UserUID {r30Win.ToString()} ELSE 0 END,";

                if (r50Times.Length > 0)
                    sql += $" R50_Times = R50_Times + CASE UserUID {r50Times.ToString()} ELSE 0 END,";
                if (r50Bet.Length > 0)
                    sql += $" R50_Bet = R50_Bet + CASE UserUID {r50Bet.ToString()} ELSE 0 END,";
                if (r50Win.Length > 0)
                    sql += $" R50_Win = R50_Win + CASE UserUID {r50Win.ToString()} ELSE 0 END,";

                if (r100Times.Length > 0)
                    sql += $" R100_Times = R100_Times + CASE UserUID {r100Times.ToString()} ELSE 0 END,";
                if (r100Bet.Length > 0)
                    sql += $" R100_Bet = R100_Bet + CASE UserUID {r100Bet.ToString()} ELSE 0 END,";
                if (r100Win.Length > 0)
                    sql += $" R100_Win = R100_Win + CASE UserUID {r100Win.ToString()} ELSE 0 END,";

                if (r250Times.Length > 0)
                    sql += $" R250_Times = R250_Times + CASE UserUID {r250Times.ToString()} ELSE 0 END,";
                if (r250Bet.Length > 0)
                    sql += $" R250_Bet = R250_Bet + CASE UserUID {r250Bet.ToString()} ELSE 0 END,";
                if (r250Win.Length > 0)
                    sql += $" R250_Win = R250_Win + CASE UserUID {r250Win.ToString()} ELSE 0 END,";

                if (r500Times.Length > 0)
                    sql += $" R500_Times = R500_Times + CASE UserUID {r500Times.ToString()} ELSE 0 END,";
                if (r500Bet.Length > 0)
                    sql += $" R500_Bet = R500_Bet + CASE UserUID {r500Bet.ToString()} ELSE 0 END,";
                if (r500Win.Length > 0)
                    sql += $" R500_Win = R500_Win + CASE UserUID {r500Win.ToString()} ELSE 0 END,";

                if (r500abTimes.Length > 0)
                    sql += $" R500ab_Times = R500ab_Times + CASE UserUID {r500abTimes.ToString()} ELSE 0 END,";
                if (r500abBet.Length > 0)
                    sql += $" R500ab_Bet = R500ab_Bet + CASE UserUID {r500abBet.ToString()} ELSE 0 END,";
                if (r500abWin.Length > 0)
                    sql += $" R500ab_Win = R500ab_Win + CASE UserUID {r500abWin.ToString()} ELSE 0 END,";

                if (indepR025Times.Length > 0)
                    sql += $" IndepR025_Times = IndepR025_Times + CASE UserUID {indepR025Times.ToString()} ELSE 0 END,";
                if (indepR050Times.Length > 0)
                    sql += $" IndepR050_Times = IndepR050_Times + CASE UserUID {indepR050Times.ToString()} ELSE 0 END,";
                if (indepR075Times.Length > 0)
                    sql += $" IndepR075_Times = IndepR075_Times + CASE UserUID {indepR075Times.ToString()} ELSE 0 END,";
                if (indepR100Times.Length > 0)
                    sql += $" IndepR100_Times = IndepR100_Times + CASE UserUID {indepR100Times.ToString()} ELSE 0 END,";
                if (indepR300Times.Length > 0)
                    sql += $" IndepR300_Times = IndepR300_Times + CASE UserUID {indepR300Times.ToString()} ELSE 0 END,";
                if (indepR500Times.Length > 0)
                    sql += $" IndepR500_Times = IndepR500_Times + CASE UserUID {indepR500Times.ToString()} ELSE 0 END,";
                if (indepR1000Times.Length > 0)
                    sql += $" IndepR1000_Times = IndepR1000_Times + CASE UserUID {indepR1000Times.ToString()} ELSE 0 END,";
                if (indepR2500Times.Length > 0)
                    sql += $" IndepR2500_Times = IndepR2500_Times + CASE UserUID {indepR2500Times.ToString()} ELSE 0 END,";
                if (indepR5000Times.Length > 0)
                    sql += $" IndepR5000_Times = IndepR5000_Times + CASE UserUID {indepR5000Times.ToString()} ELSE 0 END,";
                if (indepR10000Times.Length > 0)
                    sql += $" IndepR10000_Times = IndepR10000_Times + CASE UserUID {indepR10000Times.ToString()} ELSE 0 END,";
                if (indepR10000abTimes.Length > 0)
                    sql += $" IndepR10000ab_Times = IndepR10000ab_Times + CASE UserUID {indepR10000abTimes.ToString()} ELSE 0 END,";
                #endregion
            }

            if (keyCnt > 0)
            {
                if (datKeyIn.Length > 0)
                {
                    string tmp = datKeyIn.ToString();
                    sql += $" TotalKeyIn = TotalKeyIn + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" DayKeyIn = DayKeyIn + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" WeekKeyIn = WeekKeyIn + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" MonthKeyIn = MonthKeyIn + CASE UserUID {tmp} ELSE 0 END,";
                }
                if (datKeyOut.Length > 0)
                {
                    string tmp = datKeyOut.ToString();
                    sql += $" TotalKeyOut = TotalKeyOut + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" DayKeyOut = DayKeyOut + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" WeekKeyOut = WeekKeyOut + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" MonthKeyOut = MonthKeyOut + CASE UserUID {tmp} ELSE 0 END,";
                }
            }

            if (waterCnt > 0)
            {
                if (datWaterIn.Length > 0)
                    sql += $" TotalWaterIn = TotalWaterIn + CASE UserUID {datWaterIn.ToString()} ELSE 0 END,";
                if (datWaterOut.Length > 0)
                    sql += $" TotalWaterOut = TotalWaterOut + CASE UserUID {datWaterOut.ToString()} ELSE 0 END,";
                if (datRedBonusWaterIn.Length > 0)
                    sql += $" TotalRedBonusWaterIn = TotalRedBonusWaterIn + CASE UserUID {datRedBonusWaterIn.ToString()} ELSE 0 END,";
                if (datRedBonusWaterOut.Length > 0)
                    sql += $" TotalRedBonusWaterOut = TotalRedBonusWaterOut + CASE UserUID {datRedBonusWaterOut.ToString()} ELSE 0 END,";
            }

            if (h5Cnt > 0)
            {
                if (datH5In.Length > 0)
                {
                    string tmp = datH5In.ToString();
                    sql += $" H5TotalIn = H5TotalIn + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" H5DayIn = H5DayIn + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" H5WeekIn = H5WeekIn + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" H5MonthIn = H5MonthIn + CASE UserUID {tmp} ELSE 0 END,";
                }
                if (datH5Out.Length > 0)
                {
                    string tmp = datH5Out.ToString();
                    sql += $" H5TotalOut = H5TotalOut + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" H5DayOut = H5DayOut + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" H5WeekOut = H5WeekOut + CASE UserUID {tmp} ELSE 0 END,";
                    sql += $" H5MonthOut = H5MonthOut + CASE UserUID {tmp} ELSE 0 END,";
                }
            }

            if (!string.IsNullOrEmpty(sql) && sql.EndsWith(","))
            {
                sql = sql.Substring(0, sql.Length - 1); //移除最後逗號
            }

            sql += $" WHERE UserUID IN ({datUser.ToString()})";

            //if (PRepoDebugFg) MyConsole.WriteLine($"GetPlayerReportSqlString userFg={userCnt}, playFg={playCnt}, keyFg={keyCnt}, waterFg={waterCnt}");
            //if (PRepoDebugFg) MyConsole.WriteLine($"    SQL=[{sql}]");

            return sql;
        }
        #endregion


        #region DB資料歸零
        /// <summary>玩家報告 每日開洗歸零DB欄位</summary>
        public Dictionary<string, string> GetResetPRDay()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>{
                { "DayPlay", "0" },
                { "DayBet", "0" },
                { "DayWin", "0" },
                { "IndepDayPlay", "0" },
                { "IndepDayBet", "0" },
                { "IndepDayWin", "0" },
                { "ExBetDayPlay", "0" },
                { "ExBetDayBet", "0" },
                { "ExBetDayWin", "0" },
                { "DayPlayGames", "''" }, //字串需加上單引號
                { "IndepDayPlayGames", "''" },
                { "ExbetDayPlayGames", "''" },
                { "DayKeyIn", "0" },
                { "DayKeyOut", "0" },
                { "H5DayIn", "0" },
                { "H5DayOut", "0" }
            };
            return updata;
        }
        /// <summary>玩家報告 每日開洗歸零DB條件</summary>
        static public string GetResetPRDayWhere()
        {
            //(註)DayPlayGames欄位不加入條件, 因為只要DayPlay=0, 就不會有DayPlayGames資料
            return $"DayPlay<>0 OR DayBet<>0 OR DayWin<>0 OR " +
                    $"IndepDayPlay<>0 OR IndepDayBet<>0 OR IndepDayWin<>0 OR " +
                    $"ExBetDayPlay<>0 OR ExBetDayBet<>0 OR ExBetDayWin<>0 OR " +
                    $"DayKeyIn<>0 OR DayKeyOut<>0 OR H5DayIn<>0 OR H5DayOut<>0";
        }

        /// <summary>玩家報告 每週開洗歸零DB欄位</summary>
        public Dictionary<string, string> GetResetPRWeek()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>{
                { "WeekPlay", "0" },
                { "WeekBet", "0" },
                { "WeekWin", "0" },
                { "IndepWeekPlay", "0" },
                { "IndepWeekBet", "0" },
                { "IndepWeekWin", "0" },
                { "ExBetWeekPlay", "0" },
                { "ExBetWeekBet", "0" },
                { "ExBetWeekWin", "0" },
                { "WeekKeyIn", "0" },
                { "WeekKeyOut", "0" },
                { "H5WeekIn", "0" },
                { "H5WeekOut", "0" }
            };
            return updata;
        }
        /// <summary>玩家報告 每週開洗歸零DB條件</summary>
        static public string GetResetPRWeekWhere()
        {
            return $"WeekPlay<>0 OR WeekBet<>0 OR WeekWin<>0 OR " +
                    $"IndepWeekPlay<>0 OR IndepWeekBet<>0 OR IndepWeekWin<>0 OR " +
                    $"ExBetWeekPlay<>0 OR ExBetWeekBet<>0 OR ExBetWeekWin<>0 OR " +
                    $"WeekKeyIn<>0 OR WeekKeyOut<>0 OR H5WeekIn<>0 OR H5WeekOut<>0";
        }

        /// <summary>玩家報告 每月開洗歸零DB欄位</summary>
        public Dictionary<string, string> GetResetPRMonth()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>{
                { "MonthPlay", "0" },
                { "MonthBet", "0" },
                { "MonthWin", "0" },
                { "IndepMonthPlay", "0" },
                { "IndepMonthBet", "0" },
                { "IndepMonthWin", "0" },
                { "ExBetMonthPlay", "0" },
                { "ExBetMonthBet", "0" },
                { "ExBetMonthWin", "0" },
                { "MonthKeyIn", "0" },
                { "MonthKeyOut", "0" },
                { "H5MonthIn", "0" },
                { "H5MonthOut", "0" }
            };
            return updata;
        }
        /// <summary>玩家報告 每月開洗歸零DB條件</summary>
        static public string GetResetPRMonthWhere()
        {
            return $"MonthPlay<>0 OR MonthBet<>0 OR MonthWin<>0 OR " +
                    $"IndepMonthPlay<>0 OR IndepMonthBet<>0 OR IndepMonthWin<>0 OR " +
                    $"ExBetMonthPlay<>0 OR ExBetMonthBet<>0 OR ExBetMonthWin<>0 OR " +
                    $"MonthKeyIn<>0 OR MonthKeyOut<>0 OR H5MonthIn<>0 OR H5MonthOut<>0";
        }
        #endregion
    }
}
