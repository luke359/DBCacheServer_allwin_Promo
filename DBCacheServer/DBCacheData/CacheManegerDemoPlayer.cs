using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCacheServer
{
    public partial class CacheManeger
    {
        //CacheManegerDemoPlayer #251209

        bool DmPlDbuFg = true; //DEMO玩家 Debug 開關

        /// <summary>是否啟用DEMO玩家模式 (是否有落地頁)</summary>
        //bool IsDemoPlayerMode = true;
        /// <summary>DEMO玩家初始CREDIT</summary>
        //double DemoPlayCredit = 10; //DEMO玩家初始CREDIT
        /// <summary>DEMO玩家代理名稱</summary>
        //string DemoEntityName = "DEMOFREE";
        /// <summary>DEMO玩家代理ID</summary>
        int DemoEntityUid = 0;
        /// <summary>取得DEMO代理ID (取得ENTITY資料後呼叫)</summary>
        void GetDemoEntityID()
        {
            if(CountrySetting.IsDemoPlayerMode == false)
            {
                DemoEntityUid = 0;
                MyConsole.WriteLine("DEMO玩家模式未啟用 (無法使用落地頁DEMO功能)");
                return;
            }

            lock (EntityDataList)
            {
                foreach (var entity in EntityDataList.Values)
                {
                    //if (entity.Name.ToLower() == "demofree")
                    if (entity.Name == CountrySetting.DemoEntityName)
                    {
                        entity.SetKiosMode(4); //強制 KIOS=Free
                        DemoEntityUid = entity.EntityId;
                        return;
                    }
                }
            }

            DemoEntityUid = 0;
            MyConsole.WriteLine($"警告：無法取得DEMO玩家代理ID，請確認資料庫是否有建立 {CountrySetting.DemoEntityName} 代理。");
        }

        /// <summary>不是DEMO玩家代理ID</summary>
        public bool NotDemoEntity(int entityId)
        {
            return entityId != DemoEntityUid;
        }

        class DemoPlayerInfo
        {
            const int MAX_DOG_CNT = 2; //2約等於3～6分鐘, 3約等於6～10分鐘
            /// <summary>使用中</summary>
            public bool IsBusy { get; private set; }
            /// <summary>閒置中</summary>
            public bool IsIdle => !IsBusy;
            /// <summary></summary>
            int DogCnt;

            public DemoPlayerInfo()
            {
                IsBusy = false;
                DogCnt = 0;
            }

            /// <summary> 設定使用中</summary>
            public void SetBusy()
            {
                IsBusy = true;
                DogCnt = 0;
            }
            /// <summary> 設定閒置中</summary>
            public void SetIdle()
            {
                IsBusy = false;
                DogCnt = 0;
            }
            /// <summary>檢查是否超時</summary>
            public bool CheckDog()
            {
                //if(IsIdly)
                //{
                //    return false; //閒置中不檢查
                //}

                DogCnt++;
                if(DogCnt >= MAX_DOG_CNT)
                {
                    return true;
                }

                return false;
            }
        }


        /// <summary>DEMO玩家列表</summary>
        Dictionary<int, DemoPlayerInfo> DemoUserUidList = new();
        /// <summary>製作DEMO玩家列表 (取得玩家資料後呼叫)</summary>
        void InitDemoPlayer()
        {
            if(CountrySetting.IsDemoPlayerMode == false) { return; }

            lock (UserDataList)
            {
                lock (DemoUserUidList)
                {
                    if (DemoUserUidList.Count > 0) { DemoUserUidList.Clear(); }

                    foreach (var userData in UserDataList.Values)
                    {
                        if (userData.EntityId != DemoEntityUid)
                        {
                            continue;
                        }
                        DemoUserUidList.Add(userData.UserUID, new DemoPlayerInfo());
                    }
                }
            }

            if (DemoUserUidList.Count > 0)
                MyConsole.WriteLine($"初始化DEMO帳號列表完成 : {CountrySetting.DemoEntityName} 共有 {DemoUserUidList.Count} 位DEMO帳號可用。");
            else
                MyConsole.WriteLine($"初始化DEMO帳號列表失敗 : 請確認資料庫是否有建立 {CountrySetting.DemoEntityName} 的DEMO玩家帳號。");
        }

        /// <summary>若是DEMO玩家則加入DEMO玩家列表</summary>
        public void CheckAndAddDemoPlayer(UserData userData)
        {
            if (CountrySetting.IsDemoPlayerMode == false) { return; }

            if (userData.EntityId != DemoEntityUid)
            {
                return;
            }

            lock (DemoUserUidList)
            {
                if (DemoUserUidList.ContainsKey(userData.UserUID) == false)
                {
                    DemoUserUidList.Add(userData.UserUID, new DemoPlayerInfo());
                    if (DmPlDbuFg) MyConsole.WriteLine($"新增DEMO帳號[{userData.UserID}], 共有 {DemoUserUidList.Count} 位DEMO帳號可用");
                }
            }
        }

        /// <summary>檢查並釋放全部未使用的DEMO玩家 (每200秒檢查1次)</summary>
        public void CheckAllDemoPlayer()
        {
            if (CountrySetting.IsDemoPlayerMode == false) { return; }

            if (DemoUserUidList.Count <= 0) { return; }

            int inUseCnt = 0;
            int inIdleCnt = 0;
            List<string> inuseList = new();
            List<string> InIdleList = new();
            lock (DemoUserUidList)
            {
                foreach (var info in DemoUserUidList)
                {
                    if (info.Value.IsIdle) continue; //DEMO玩家閒置中，跳過檢查

                    var userData = Getuser(info.Key);
                    if (userData != null)
                    {
                        if (userData.Usersituation != 0)
                        {
                            inUseCnt++;
                            inuseList.Add(userData.UserID);
                            continue; //Demo玩家 正在遊戲中
                        }

                        if (info.Value.CheckDog())
                        {
                            DemoUserUidList[info.Key].SetIdle(); //標記該玩家閒置可用
                            MyConsole.WriteLine($"  強制釋放DEMO帳號 : {userData.UserID}");
                        }
                        else
                        {
                            inIdleCnt++; //使用中, 但玩家狀態為離線
                            InIdleList.Add(userData.UserID);
                        }
                    }
                }
            }
            if (DmPlDbuFg && (inuseList.Count > 0 || InIdleList.Count > 0)) MyConsole.WriteLine($"  檢查DEMO帳號完成 : 使用中帳號 {inUseCnt} 個, 卡帳號 {inIdleCnt} 個");
            if (DmPlDbuFg)
            {
                if (inuseList.Count > 0)
                {
                    string inuseStr = string.Join(", ", inuseList);
                    MyConsole.WriteLine($"    使用中帳號清單 : {inuseStr}");
                }
                if (InIdleList.Count > 0)
                {
                    string inIdleStr = string.Join(", ", InIdleList);
                    MyConsole.WriteLine($"    卡帳號清單 : {inIdleStr}");
                }
            }
        }

        /// <summary>釋放DEMO玩家帳號 (變更玩家Usersituation後呼叫)</summary>
        void CheckDemoUserIdle(UserData userData)
        {
            if (userData.EntityId != DemoEntityUid) { return; } //非DEMO玩家

            if (userData.Usersituation != 0) { return; } //玩家狀態非離線

            lock (DemoUserUidList)
            {
                if (DemoUserUidList.ContainsKey(userData.UserUID))
                {
                    if (DemoUserUidList[userData.UserUID].IsBusy)
                    {
                        DemoUserUidList[userData.UserUID].SetIdle(); //標記該玩家可用
                        if (DmPlDbuFg) MyConsole.WriteLine($"釋放DEMO帳號 : {userData.UserID}");
                    }
                }
            }
        }

        /// <summary>取得DEMO玩家帳密 (落地頁按DEMO按鈕時呼叫)</summary>
        public bool GetDemoUserNamePass(string userID, out string name, out string pass)
        {
            lock (DemoUserUidList)
            {
                //找出 DemoUserUidList 裡尚未分配的玩家 
                foreach (var info in DemoUserUidList)
                {
                    if (info.Value.IsIdle)
                    {
                        var userData = Getuser(info.Key);
                        if (userData != null)
                        {
                            userData.UserBalance = CountrySetting.DemoPlayCredit; //重設DEMO玩家CREDIT
                            DemoUserUidList[info.Key].SetBusy(); //標記該玩家已被分配
                            name = userData.UserID;
                            pass = userData.UserPwd;
                            if (DmPlDbuFg) MyConsole.WriteLine($"玩家({userID}) 分配DEMO帳號 : {userData.UserID}");
                            return true;
                        }
                    }
                }
            }

            name = "";
            pass = "";

            //if (DmPlDbuFg)
            {
                if (CountrySetting.IsDemoPlayerMode) MyConsole.WriteLine($"警告：玩家({userID}) 無可用的 DEMO 帳號，請稍後再試。");
                else MyConsole.WriteLine($"警告：DEMO玩家模式未啟用，玩家({userID})無法使用落地頁DEMO功能。");
            }
            return false; 
        }

        /// <summary>重設DEMO玩家CREDIT (Loging時呼叫) (回傳false=非DEMO玩家)</summary>
        public bool SetDemoPlayerCredit(UserData userData, EntityData entity)
        {
            if (userData.EntityId != DemoEntityUid) { return false; } //非DEMO玩家

            userData.UserBalance = CountrySetting.DemoPlayCredit;

            //if (DmPlDbuFg) MyConsole.WriteLine($"重設DEMO玩家CREDIT : {userData.UserID} CREDIT={userData.UserBalance}");
            return true;
        }

        /// <summary>不是DEMO玩家帳號</summary>
        //public bool NotDemoPlayer(UserData userData)
        //{
        //    return userData.EntityId != DemoEntityUid;
        //}
        /// <summary>是DEMO玩家帳號</summary>
        //public bool IsDemoPlayer(UserData userData)
        //{
        //    return userData.EntityId == DemoEntityUid;
        //}
    }
}
