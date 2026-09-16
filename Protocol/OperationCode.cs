namespace Protocol
{
    public enum OperationCode
    {
        Null = 0,
        //====================訪問DB用==============================
        /// <summary>確認版本</summary>
        CheckVersion = 1,
        /// <summary>確認登入資訊</summary>
        Login = 3,
        /// <summary>修改玩家狀態</summary>
        UpdataUsersituation = 4,
        /// <summary>獲取倍率表</summary>
        GetMagnificationForm = 20,
        /// <summary>寫歷史紀錄</summary>
        WriteHistory = 21,
        /// <summary>驗證玩家餘額夠不夠</summary>
        VerificationUserBalance = 25,
        /// <summary>獲取遊戲設定值</summary>
        GetGameSetting = 26,
        /// <summary>獲取歷史紀錄</summary>
        GetHistory = 27,
        /// <summary>更新歷史紀錄</summary>
        UpdateHistory = 28,
        /// <summary>獲取內帳</summary>
        GetGameInnerAccount = 29,
        /// <summary>更新機率表</summary>
        UpdateProbabilitytabl = 30,
        /// <summary>獲取外帳</summary>
        GetGameOuterAccount = 31,
        /// <summary>更新外帳</summary>
        UpdateGameOuterAccount = 32,
        /// <summary>插入一筆玩家遊玩紀錄</summary>
        insertUserGamedata = 33,
        /// <summary>獲取西遊詳細遊戲紀錄</summary>
        GetXiyouAccountDetailData = 34,
        /// <summary>替換西遊詳細遊戲資料</summary>
        ReplaceXiyouAccountDetailData = 35,
        /// <summary>更新西遊詳細遊戲資料</summary>
        UpdateXiyouAccountDetailData = 36,
        /// <summary>獲取西遊大水庫</summary>
        GetXiyouBigWaterPool = 37,
        /// <summary>更新西遊大水庫</summary>
        UpdateXiyouBigWaterPool = 38,
        /// <summary>插入一筆西遊大水庫出牌紀錄</summary>
        InsertXiyouBigWaterRec = 39,
        /// <summary>檢查西遊大水庫校正天數的盈餘</summary>
        CheckXiyouBigWaterProFitDays = 40,
        /// <summary>更新JP真實彩金資料</summary>
        UpdateRealJPData = 42,
        /// <summary>更新後台逼JP彩金Log</summary>
        EnforceJPUpdateWebLog = 43,
        /// <summary>玩家獲取JP彩金</summary>
        PullJPBonus = 44,
        /// <summary>插入一筆JP彩金出牌紀錄</summary>
        EnforceJPInsertAccountData = 45,
        /// <summary>機率JP出牌</summary>
        DBPushJPBonus = 46,
        /// <summary>GameServer要求出JP彩金</summary>
        AskDBPushJPBonus = 47,
        /// <summary>獲取彩金值</summary>
        GetJP = 50,
        /// <summary>更新後台逼遊戲獎項Log</summary>
        EnforceAwardUpdateWebLog = 48,
        /// <summary>獲取JP帳目資料</summary>
        GetJPAcoountData = 52,
        /// <summary>獲取玩家帳目資料</summary>
        GetUserAccountData = 53,
        /// <summary>獲取機器人押分規則</summary>
        GetBotModeRuleTable = 54,
        /// <summary>獲取遊戲伺服器相對應的機器人</summary>
        GetGamerServerBot = 55,
        /// <summary>修改密碼</summary>
        ModifyPassWord = 56,
        /// <summary>廣播訊息</summary>
        BroadcastMessage = 57,
        /// <summary>獲取玩家交易紀錄</summary>
        GetUserTransaction = 58,
        /// <summary>傳送機器人配置</summary>
        SendBotConfiguration = 59,
        /// <summary>插入一筆機台成就紀錄</summary>
        insertMachineRankData = 60,
        /// <summary>插入一筆除錯用Log紀錄</summary>
        InsertCalcLogData = 65,
        /// <summary>取得獎勵</summary>
        GetReward = 68,
        /// <summary>獲取自己的每日任務內容</summary>
        GetSelfSignInData = 69,
        /// <summary>同步玩家星星資訊</summary>
        SyncUserData = 71,
        /// <summary>發出紅包</summary>
        SendGameRedEnvelope = 73,
        /// <summary>更新遊戲報告紀錄</summary>
        UpdateReportGameData = 74,
        /// <summary>驗證玩家餘額及更新玩家紀錄</summary>
        VerificationAndUpdateHistory = 77,
        /// <summary>更新假玩機器人RANK紀錄</summary>
        UpdateFakeBotRank = 78,
        /// <summary>廣播系統命令</summary>
        BroadcastSystemCommand = 79,
        // <summary>批量更新玩家狀態</summary>
        UpdateBatchUserStaatus = 84,
        // <summary>Vnpay儲值</summary>
        VnpayDeposit = 91,
        // <summary>Vnpay儲值回覆</summary>
        VnpayDepositRequest = 92,
        // <summary>Vnpay除值</summary>
        VnpayPayout = 93,
        // <summary>Vnpay除值回覆</summary>
        VnpayPayoutRequest = 94,
        // <summary>Vnpay儲除值結果檢驗</summary>
        VnpayResultCheck = 95,
        // <summary>Vnpay支援銀行清單</summary>
        VnpayBankList = 96,
        // <summary>Vnpay交易紀錄清單</summary>
        VnpayTransactionList = 97,
        // <summary>網吧炒場</summary>
        HypeBigWater = 99,
        /// <summary>WhatsApp修改密碼</summary>
        WhatsAppModifyPassWord = 100,

        /// <summary>PGSlot 驗證用</summary>
        ApiVerifySession = 101,
        /// <summary>PGSlot 玩家返回本地大廳</summary>
        ReturnToLocalLobby = 102,

        /// <summary>寫入遊戲保留資料</summary>
        InsertGameReserveInfo = 117,
        /// <summary>刪除遊戲保留資料</summary>
        DeleteGameReserveInfo = 118,
        /// <summary>取得遊戲保留資料</summary>
        GetGameReserveInfo = 119,

        /// <summary>玩家預押分驗證(激爆骰子專用)</summary>
        VerificationUserBalancePre = 120,
        /// <summary>開獎結果(激爆骰子專用)</summary>
        UpdataFinalPrizeResult = 121,
        /// <summary>取得玩家權威餘額及BDV2基準柵欄</summary>
        //====================訪問登入伺服器用==============================
        /// <summary>玩家註冊</summary>
        Registered = 2,
        /// <summary>玩家登出</summary>
        Logout = 5,
        /// <summary>玩家選擇遊戲伺服器</summary>
        SelectGame = 6,
        /// <summary>遊戲伺服器像登入伺服器驗證玩家</summary>
        Verification = 8,
        /// <summary>遊戲伺服器像DB伺服器驗證玩家</summary>
        VerificationDB = 89,
        /// <summary>遊戲伺服器上鎖狀態</summary>
        LockGameStatus = 90,
        /// <summary>廣播JP (傳新值)</summary>
        BroadcastJP = 24,
        /// <summary>傳送遊戲類型</summary>
        CheckGameServerType = 41,
        /// <summary>獲取自己的遊玩資訊/summary>
        GetSelfeGameData = 62,
        /// <summary>獲取自己的每日任務內容/summary>
        GetSelfeDayMissionData = 66,
        /// <summary>獲取自己的彩票內容/summary>
        GetSelfLottoTicketData = 67,
        /// <summary>設定自己的彩票號碼/summary>
        SetSelfLottoTicketNumber = 72,
        /// <summary>遊戲伺服器像登入伺服器驗證玩家</summary>
        VerificationNoLobby = 75,
        /// <summary>重新連線</summary>
        ReConnect = 81,
        /// <summary>更新玩家Credit</summary>
        UpdateUserBalance = 98,
        //====================訪問遊戲伺服器用==============================
        /// <summary>玩家傳送認證碼給伺服器</summary>
        SendCodeToGameServer = 7,
        /// <summary>獲取指定房間資訊</summary>
        GetRoomInfo = 9,
        /// <summary>獲取所有房間資訊</summary>
        GetAllRoomInfo = 10,
        /// <summary>加入房間</summary>
        JoinRoom = 11,
        /// <summary>退出房間</summary>
        QuitRoom = 12,
        /// <summary>退出房間(直接到LoginLobby)</summary>
        QuitRoomNoLobby = 76,
        /// <summary>退出遊戲伺服器</summary>
        QuitServer = 13,
        /// <summary>房間會員誕生</summary>
        RoomActorBorning = 14,
        /// <summary>房間會員操作更新</summary>
        RoomActorActionUpdate = 15,
        /// <summary>廣播房間會員操作</summary>
        RoomBroadcastActorAction = 16,
        /// <summary>廣播房間會員離開</summary>
        RoomBroadcastActorQuit = 17,
        /// <summary>廣播遊戲進程</summary>
        GameProcessBroadcast = 18,
        /// <summary>玩家傳送押分訊息</summary>
        SendBetInfo = 19,
        /// <summary>獲取遊戲資訊</summary>
        SendGameInfo = 51,
        /// <summary>獲取別的玩家的遊玩資訊/summary>
        GetOtherGameData = 61,
        /// <summary>獲取機台的遊戲資訊/summary>
        GetMachineGameData = 63,
        /// <summary>獲取玩家玩完JP/summary>
        GetUserPLayJPEnd = 64,
        /// <summary>取得紅包</summary>
        GetGameRedEnvelope = 70,
        /// <summary>玩家傳送砲台資訊</summary>
        SendGunAction = 85,


        /// <summary>廣播房間會員砲台操作</summary>
        RoomBroadcastActorGunAction = 86,
        /// <summary>廣播房間會員魚群腳本</summary>
        RoomBroadcastActorFishScript = 87,
        /// <summary>廣播魚機指令</summary>
        RoomBroadcastFishCommand = 88,
        //====================訪問Web伺服器用==============================
        Web = 22,
        ManagerChangeClientCridet = 23,
        //====================心跳包用=====================================
        HeartBeat = 49,
        //====================維護用=====================================
        ServerMaintenance = 80,
        /// <summary>確認玩家身分</summary>
        LoginCheckPlayerStatus = 82,
        /// <summary>確認玩家身分</summary>
        GameCheckPlayerStatus = 83
    }
}
