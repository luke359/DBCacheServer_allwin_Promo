using System.Text;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebProtocol
{
    public enum ResultCode
    {
        Ok,
        InvalidOperation,
        InvalidParameter,
        CustomError,
        InvalidMinStartRateLimit,
        IllegalUser,
        GameServerNotWork
    }

    public enum WebActorUpdataCode
    {
        UpdateCredit,
        UpdateBetInfo,
        UpdateGameState,
        UpdateStar,
        UpdateGameData
    }

    public enum WebMsgType
    {
        None,
        ServerMaintenance,
        SendCodeToGameServer,
        Verification,
        VerificationNoLobby,
        JoinRoom,
        QuitRoom,
        QuitRoomNoLobby,
        QuitServer,
        GetAllRoomInfo,
        GetMagnificationForm,
        RoomActorActionUpdate,
        VerificationUserBalance,
        GetMachineGameData,
        GetJP,
        GetUserAccountData,
        GetOtherGameData,
        GetGameRedEnvelope,
        RefreshSettingOne,
        RefreshSettingAll,
        GameProcessBroadcast,
        ReConnect,
        CheckVersion,
        PhoneNumberData,
        CommonCommandData,
        UserRegisteredData,
        VnpayInfo,
        CommonMessageData,
        CheckGameServerType,
        ModifyPassWord,
        Login,
        Logout,
        SelectGame,
        BroadcastMessage,
        BroadcastSystemCommand,
        BroadcastJP,
        ManagerChangeClientCridet,
        UpdateUserBalance,
        GetJPAcoountData,
        CommonInfoData,
        GetUserTransaction,
        RefreshJpSetting,
        GetSelfeGameData,
        GetSelfeDayMissionData,
        GetSelfLottoTicketData,
        SetSelfLottoTicketNumber,
        GetReward,
        GetSelfSignInData,
        SyncUserData,
        LockGameStatus,
        WebLobbyBonus,
        ReceiveLobbyBonus,
        EditPhoneNumber,
        GetPhoneNumber,
        IntroducerPayOutFg,
        ReviewTransaction,
        GetWhatsAppGroupR,
        GetWhatsAppGroupU,
        WebH5Command,
        GetAnnouncementCustomer,
        ReturnToLocalLobby,
        WebGameLogin,
    }

    public enum GameServerCodeWeb
    {
        None,
        Login,
        XiyouGame,
        HightWayGame,
        GoldenRooster,
        LargeBlue,
        DolphinReef,
        PantherMoon,
        GolfTour,
        FunkyMomkey,
        FaFaFa,
        ForTuneTree,
        Acrobatics3,
        Butterfly,
        BonusBears,
        PandaFortune,
        SeaCaptain,
        TripleDragons,
        GoldenChips,
        GoldBar,
        SuperKingDerby,
        BearHoliday,
        HooksVoyage,
        WildBuffalo,
        Ferrari,
        DrollMonkey,
        BeerTent,
        Pandarcher,
        BMW3D,
        AstonMartin,
        SuperAltyn,
        MonkeyKing2,
        Super8,
        GodOfWealth2,
        Star97,
        FIFA2022,
        JungleParty,
        A12Bonus,
        KungFu,
        BoyKingsTreasure,
        NezhaReborn,
        PiratesTreasure,
        SuperMiner,
        HighWayKingPlus,
        PussInBoots,
        FireSpin,
        Vault,
        FuXingGaoZhao3,
        ModernValhalla,
        BigWinBurst,
        WildWest,
        HooksVoyagePlus,
        WildBeach,
        Aladdin,
        OceanKing,
        StarCafeBar,
        TopGun,
        WildWestMegaWay,
        WildBeachParty,
        PoisonApple,
        MuseumHeist,
        PinupBeauty,
        OceanKing2,
        OceanKing3,
        OceanKing4,
        OceanKing5,
        OceanKing6,
        RobinHood,
        HuaMulan,
        VietnamCuisine,
        PanJinlian,
        PastaPomodoro,
        Alice,
        WildWest3,
        NezhaReborn2,
        OceanKing7,
        OceanKing8,
        PrincessRose,
        DeepSeaWitch,
        NieXiaoqian,
        GoldenBell,
        Juicy7,
        SuperCoin1000,
        WildWest3H,
        MaJiangWins,
        MuseumHeist2,
        BigSmall,
        Ninja,
        SuperAce,
        GoldenDragon,
        ClownsBonusBash,
        PgSlot,
        JILI,
        FC,
        AceWin,
        JDB,
        DCT,
        Habanero,
        DeadDrifter,
        RoseHeroZ,
        FastSpin,
        Spade,
        PlayTech,
        CP,
        FortuneNeko3,
        Medusa,
        WebMuseumHeist2,
        ScroogesWinterTreasure,
        Wukong,
        WebFireSpin,
        MahjongWinsSuperScatter,
        TreasureofAZTEC,
        WitchsBrew,
        MythofNezha,
        DoubleFortune,
        Olympus1000Plus,
        HangTuah,
        UrbansCannon,
        SetAwakened,
        Nusantara,
    }

    /// <summary>JP獎項種類</summary>
    public enum JpAwardWeb
    {
        None = 0,
        MINOR = 1,
        MAJOR = 2,
        MEGA = 3,
        SUPER = 4,
        BONUS = 5
    }

    public enum SystemTextColorWeb
    {
        None = 0,
        Bluegreen = 1,
        Yellow = 2,
        Orange = 3,
        Gold = 4,
        Green = 5,
        Skyblue = 6,
        Red = 7,
        DeepBlue = 8,
    }

    public enum WebVerVersionCode
    {
        APK,
    }

    public enum WebOperationCode
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

    public enum WebRewardType
    {
        Mission,
        LottoTicket,
        SignIn,
        StarVoucher,
        Week,
        Rebate
    }

    public enum WebGameStateCode
    {
        None,
        SetBetTime,
        GetAllBetInfo,
        SetGameInfo,
        GetAllRoomPlayerDone,
        GameEnd,
    }

    [Serializable]
    public class Packet
    {
        public string Type;
        public string Content;
        public DateTime Timestamp = DateTime.UtcNow;
    }

    [Serializable]
    public class CodeToGame
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class UserDataWeb
    {
        public int memberUniquelID;

        public int entityUID;

        public double SessionID;

        public string RemoteEndPoint;

        public string IP;

        public string memberID;

        public string memberPW;

        public string NewPW;

        public string Nickname;

        public int Sex;

        public string Email;

        public double Balnce;

        public int ratio;

        public double WinCredit;

        public short Usersituation;

        public int Energy;

        public int Star;

        public int LogUid;

        public double GameRedEnvelope;

        public double Profit;

        public double Pool;

        public string ExtInfo;

        public Dictionary<string, SystemTextColorWeb> SysTemText = new Dictionary<string, SystemTextColorWeb>();
    }

    [Serializable]
    public class GameInfo
    {
        #region 押分機

        public List<int> MagnificationForm = new List<int>();

        public int Prize;

        public Dictionary<int, int> MainLamp = new Dictionary<int, int>();

        public Dictionary<int, int> SubLamp = new Dictionary<int, int>();

        public int BankerPlayer;

        public bool OpenPrizeType;

        public int LeftRollActor;

        public int RightRollActor;

        public List<int> EnemyFightMove = new List<int>();

        public List<int> PlayerFightMove = new List<int>();

        public double Bonus;
        #endregion

        #region 老虎機

        public List<int> SlotIcon = new List<int>();

        public List<int> WildMultiplier = new List<int>();

        public int FreeGameAction;

        public int FreeGameTotalRound;

        public int FreeGameRound;

        public int FreeGameMultiplier;

        public double FreeGameTotalWin;

        public double FreeGameFeatureWin;

        public int FreeGameType;

        public int FreeGameMode;

        public List<int> LeftWild_X = new List<int>();

        public List<int> RightWild_X = new List<int>();

        public int JpGameType;  //金雞JP Game 中獎種類

        public double JpGamePoint; //金雞JP Game 中獎分數

        public double JpGrand; //金雞JP Grand 分數

        public double JpMajor; //金雞JP Major 分數

        public double JpMinor; //金雞JP Minor 分數

        public double JpMini; //金雞JP Mini 分數

        public double JpMultiplier; //金雞JP倍數

        public List<double> ExtData2 = new List<double>();

        public List<string> ExtData3 = new List<string>();
        #endregion


        public JpAwardWeb JPType;

        public double JPPoint;

        public bool AllGetJP;

        public int JPCommandType;

        public int GameMode;  //獨立買遊戲旗號

        public int IndepGameRestRound;  //獨立買遊戲剩餘局數

        public int IndepGameThisRound;  //獨立買遊戲已玩局數
    }

    [Serializable]
    public class BetInfo
    {
        public double[] Info = new double[1];

        public int LevelInfo;  //押分Level

        public double PlayInfo;  //押分單位

        public Dictionary<int, bool> AskRedEnvelope = new Dictionary<int, bool>(); //要求遊戲紅包 <玩家代號, 要求旗號>

        public List<int> IllegalUser = new List<int>();

        public List<int> illegalMinStartRate = new List<int>();

        public Dictionary<int, Equipment> UserEquipment = new Dictionary<int, Equipment>();

        public Dictionary<int, RedEnvelope> UserRedEnvelope = new Dictionary<int, RedEnvelope>();

        public Dictionary<int, double[]> AllRoomActorBetinfo = new Dictionary<int, double[]>();

        public Dictionary<int, double> AllRoomActorCreditInfo = new Dictionary<int, double>(); //全部玩家本局Credit資訊

        public int PlayMode;  //黃金樹FreeGame模式

        public int GameMode;  //獨立買遊戲旗號

        public double IndepPlayInfo;  //獨立買總押分

        public string ExtInfo;

        public BetInfo()
        {
            GameMode = 0;
            ExtInfo = null;
        }
    }

    [Serializable]
    public class Equipment
    {

        public int Energy;  //能量條

        public int Star;    //星星總數

        public int Instruction;    //額外資訊

        public string CommandType; //額外資訊總類

        public Dictionary<string, string> ExtInfo = new Dictionary<string, string>(); //額外資訊
    }

    [Serializable]
    public class RedEnvelope
    {

        public int LogUid;

        public int MachineUid;

        public int UserUid;

        public double EnvelopeValue;
    }

    [Serializable]
    public class WebVerificationData
    {
        public GameServerCodeWeb gameServerCode;
        public UserDataWeb userData = new UserDataWeb();
    }

    [Serializable]
    public class WebVerificationNoLobbyData
    {
        public GameServerCodeWeb gameServerCode;
        public UserDataWeb userData = new UserDataWeb();
        public JoinRoomInfo roomInfo = new JoinRoomInfo();
        public GameInfo gameInfo = new GameInfo();
        public BetInfo betInfo = new BetInfo();
        public ExtraDataMsg extraData = new ExtraDataMsg();
    }

    [Serializable]
    public class JoinRoom
    {
        public int memberUniquelID;
        public short roomIndex;
        public short seatIndex;
        public DBCacheData dbcacheData;
    }

    [Serializable]
    public class JoinRoomReply
    {
        public ResultCode result;

        public string DebugMessage;

        public GameServerCodeWeb gameServerCode;

        public JoinRoomInfo roomInfo = new JoinRoomInfo();

        public GameInfo gameInfo = new GameInfo();

        public BetInfo betInfo = new BetInfo();

        public ExtraDataMsg extraData = new ExtraDataMsg();
    }

    [Serializable]
    public class FishJoinRoomReply
    {
        public ResultCode result;

        public string DebugMessage;

        public GameServerCodeWeb gameServerCode;

        public WebMsgType webMsgType;

        public JoinRoomInfo roomInfo = new JoinRoomInfo();

        public WebFishingGameInit mFishingGameInit = new WebFishingGameInit();

        public List<WebHistoryInfo> historyData = new List<WebHistoryInfo>();

        public WebGameTimeInfo gameTimeInfo = new WebGameTimeInfo();

        public UserDataWeb userData = new UserDataWeb();

        public FishJoinRoomReply()
        {
            webMsgType = WebMsgType.None;
        }
    }

    [Serializable]
    public class MultiJoinRoomReply
    {
        public ResultCode result;

        public string DebugMessage;

        public GameServerCodeWeb gameServerCode;

        public JoinRoomInfo roomInfo = new JoinRoomInfo();

        public List<WebHistoryInfo> historyData = new List<WebHistoryInfo>();

        public WebGameTimeInfo gameTimeInfo = new WebGameTimeInfo();

        public UserDataWeb userData = new UserDataWeb();
    }

    [Serializable]
    public class FishRefreshSetting
    {
        public GameServerCodeWeb gameServerCode;

        public WebMsgType webMsgType;

        public JoinRoomInfo roomInfo = new JoinRoomInfo();

        public FishRefreshSetting()
        {
            webMsgType = WebMsgType.None;
        }
    }

    [Serializable]
    public class ReConnectReply
    {
        public ResultCode result;

        public string DebugMessage;

        public GameServerCodeWeb gameServerCode;

        public JoinRoomInfo roomInfo = new JoinRoomInfo();

        public UserDataWeb userData = new UserDataWeb();

        public ServerDataWeb serverData = new ServerDataWeb();
    }

    [Serializable]
    public class JoinRoomInfo
    {
        public int RoomIndex;

        public string RoomName;

        public int Limit;

        public int BetTime;

        public int Keeptime;

        public double ConversionRaito;

        public double MinRateLimit;

        public double MaxRateLimit;

        public int LineMinLimit;

        public int LineMaxLimit;

        public double MinStartRateLimit;

        public string BetUnit;

        public string LevelUnit;

        public int RoomUserCount;

        public string[] SeatArray;

        public int BackGroundIndex;

        public bool IsFastGame;

        public List<int> ExtData1 = new List<int>();

        public List<double> ExtData2 = new List<double>();

        public double IndepMinRateLimit; //獨立買 最小押分限制

        public double IndepMaxRateLimit; //獨立買 最大押分限制

        public int IndepGameBetMulti;  //獨立買 押分倍數

        public int DefaultLevel;  //Level內定植

        public double DefaultBet; //押分內定植

        public string ExtInfo;
    }

    [Serializable]
    public class WebHistoryInfo
    {
        public int HistoryUID;

        public int BankPlayer;

        public List<int> MainLamp;

        public int PrizType;

        public List<int> SubLamp;

        public double Bonus1;

        public double Bonus2;

        public double Bonus3;

        public int JP1;

        public int JP2;

        public int JP3;

        public Dictionary<string, string> ExtInfo;
    }

    [Serializable]
    public class WebGameTimeInfo
    {
        public int Bettime;

        public bool isWait;
    }

    [Serializable]
    public class QuitRoom
    {
        public int memberUniquelID;

        public ResultCode result;
    }

    [Serializable]
    public class QuitServer
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class GetAllRoominfo
    {
        public ResultCode result;

        public string DebugMessage;

        public List<JoinRoomInfo> roomInfo = new List<JoinRoomInfo>();
    }

    [Serializable]
    public class FishGetRoominfo
    {
        public int roomIndex;
    }

    [Serializable]
    public class FishingGameInitReply
    {
        public ResultCode result;

        public string DebugMessage;

        public WebFishingGameInit mWebFishingGameInit = new WebFishingGameInit();
    }

    [Serializable]
    public class WebFishingGameInit
    {
        public int ScenesType = 0;

        public List<WebGunPlatformInfo> mGunPlatformInfoList = new List<WebGunPlatformInfo>();

        public List<WebFishPathInfo> mFishPathInfoList = new List<WebFishPathInfo>();
    }

    [Serializable]
    public class WebGunPlatformInfo
    {
        public int GunPlatformNo;

        public string PlayerID;

        public double Credit;
   
        public string Action;

        public double GunAngle;

        public int GunType;

        public double BetCoin;
    }

    [Serializable]
    public class WebFishPathInfo
    {
        public int No;

        public int FishType;

        public int PathType;

        public Double SyncValue;

        public Double TotalTime;

        public string Info;
    }

    [Serializable]
    public class WebGunActionInfo
    {
        public string MsgType = "GunActionInfo";

        public WebOperationCode operationCode;

        public int GunNum;

        public string Action;

        public double GunAngle;

        public int GunType;

        public double BetCoin;

        public int FishNo;

        public double Credit;

        public double Win;

        public string Info;

        public double GameRedEnvelope;

        public int Star;

        public int Energy;

        public int ExtData1;

        public double ExtData2;

        public string ExtInfo;

        public WebGunActionInfo()
        {
            MsgType = "GunActionInfo";
            Action = "";
            Info = "";
            ExtInfo = "";
        }
    }

    [Serializable]
    public class MemberUniquelID
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class MagnificationFormReply
    {
        public ResultCode result;

        public string DebugMessage;

        public List<int> MagnificationForm = new List<int>();
    }

    [Serializable]
    public class EquipmentWeb
    {
        public int Energy;  //能量條

        public int Star;    //星星總數

        public int Instruction;    //額外資訊

        public string CommandType; //額外資訊總類

        public Dictionary<string, string> ExtInfo = new Dictionary<string, string>(); //額外資訊
    }

    [Serializable]
    public class RedEnvelopeWeb
    {

        public int LogUid;

        public int MachineUid;

        public int UserUid;

        public double EnvelopeValue;
    }

    [Serializable]
    public class RoomActorActionUpdate
    {
        public int memberUniquelID;

        public WebActorUpdataCode actorUpdataCode;

        public BetInfo betInfo = new BetInfo();

        public WebRoomActionInfo roomActionInfo = new WebRoomActionInfo();
    }

    [Serializable]
    public class ServerReply
    {
        public ResultCode result;

        public string DebugMessage;
    }

    [Serializable]
    public class GetMachineGameData
    {
        public short roomIndex;
    }

    [Serializable]
    public class MachineGameDataWeb
    {
        public int AllRunCount;

        public int PrizeCount;
    }

    [Serializable]
    public class MachineRankStruWeb
    {

        public int RankType;

        public int RankCount;

        public string RankData;

        public string RankList;
    }

    [Serializable]
    public class MachineGameDataReply
    {
        public ResultCode result;

        public int memberUniquelID;

        public MachineGameDataWeb machineGameData = new MachineGameDataWeb();

        public List<MachineRankStruWeb> machineRankStru = new List<MachineRankStruWeb>();
    }

    [Serializable]
    public class GetJP
    {
        public ResultCode result;

        public int memberUniquelID;

        public JpAwardWeb JPType;

        public double JPPoint;

        public bool AllGetJP;

        public double Balance;
    }

    [Serializable]
    public class UserAccountDataWeb
    {

        public GameServerCodeWeb GameType;

        public string StartTime;

        public string EndTime;

        public int CurrentPage;

        public int TotalPage;

        public int DataUnitNum;
    }

    [Serializable]
    public class UserGameAccountDataWeb
    {

        public string SerialNo;

        public string LogTime;

        public double startBalance;

        public double TotalBet;

        public double TotalWin;

        public double EndBalance;

        public string Result;

        public string PlayInfo;

        public string JPType;

        public string JPWin;
    }

    [Serializable]
    public class UserAccountPacket
    {
        public ResultCode result;

        public int memberUniquelID;

        public UserAccountDataWeb useraccountdata = new UserAccountDataWeb();

        public List<UserGameAccountDataWeb> usergameaccountdata = new List<UserGameAccountDataWeb>();
    }

    [Serializable]
    public class OtherUserPacket
    {
        public ResultCode result;

        public int memberUniquelID;
    }

    [Serializable]
    public class GameRedEnvelopeReply
    {
        public ResultCode result;

        public double Balnce;

        public double GameRedEnvelope;
    }


    [Serializable]
    public class VerificationUserBalanceMsg
    {
        public ResultCode result;
    }

    [Serializable]
    public class UserRankStruWeb
    {
        public string GameName;

        public int RankType;

        public string RankData;

        public string BuildTime;
    }

    [Serializable]
    public class GetOtherGameDataMsg
    {
        public ResultCode result;
        public int memberUniquelID;
        public string Nickname;
        public int Sex;
        public List<UserRankStruWeb> userRankStru = new List<UserRankStruWeb>();
    }

    [Serializable]
    public class RefreshSettingMsg
    {
        public List<JoinRoomInfo> roomInfo = new List<JoinRoomInfo>();
    }


    [Serializable]
    public class GameProcessBroadcastMsg
    {
        public ResultCode result;

        public GameInfo gameInfo = new GameInfo();

        public double userBalnce;

        public double userWinCredit;

        public int userEnergy;

        public int userStar;

        public double userGameRedEnvelope;

        /// <summary>返水資訊</summary>
        public Dictionary<string, string> rebateInfo = new Dictionary<string, string>();

        public ExtraDataMsg extraData = new ExtraDataMsg();
    }

    [Serializable]
    public class ServerDataWeb
    {
        public string IP;

        public int Port;

        public int EmptSeatNum;
    }

    [Serializable]
    public class ReConnectMsg
    {
        public ResultCode result;

        public GameServerCodeWeb gameServerCode;

        public JoinRoomInfo roomInfo = new JoinRoomInfo();

        public UserDataWeb userData = new UserDataWeb();

        public ServerDataWeb serverData = new ServerDataWeb();
    }

    [Serializable]
    public class WebVerData
    {
        public WebVerVersionCode type;
        public int SameTypeGameCount;
        public string RemoteEndPoint;
    }

    [Serializable]
    public class WebPhoneNumberData
    {
        /// <summary>傳送訊信的類型</summary>
        public string Type;
        public string Status;
        public int UserUID;
        public string PhoneNumber;
    }

    [Serializable]
    public class WebCommonCommandData
    {
        /// <summary>傳送訊信的類型</summary>
        public string CommandType;
        public string CommandContext;
    }

    [Serializable]
    public class WebVnpayInfo
    {
        public WebOperationCode operationCode { get; set; }

        public int UserUID { get; set; }

        public string Msg { get; set; }
    }

    [Serializable]
    public class WebCommonMessageData
    {
        /// <summary>傳送訊息的類型</summary>
        public string CommandType;
        /// <summary>傳送訊息的內容</summary>
        public string CommandContext;
        /// <summary>傳送訊息副指令</summary>
        public int Instruction;
        /// <summary>傳送訊息的遊戲伺服器</summary>
        public GameServerCodeWeb GameServerCode;
        /// <summary>要接收訊息的玩家</summary>
        public int UserUID;
        /// <summary>傳送訊息的內容</summary>
        public Dictionary<string, string> DataContext = new Dictionary<string, string>();
        /// <summary>要接收訊息的玩家List</summary>
        public List<int> UserList;
        /// <summary>額外公用資訊</summary>
        //[DataMember]
        //public ExtraData extraData;

        public WebCommonMessageData(string commandType)
        {
            CommandType = commandType; //CommandType為必要資訊
        }
    }

    [Serializable]
    public class WebCheckGameServerType
    {
        public GameServerCodeWeb gameServerCode;
    }

    [Serializable]
    public class WebCheckGameServerTypeReply
    {
        public ResultCode result;

        public string DebugMessage;
    }

    [Serializable]
    public class WebModifyPassWord
    {
        public int memberUniquelID;

        public string memberPW;

        public string NewPW;
    }

    [Serializable]
    public class WebLogin
    {
        public string memberID;

        public string memberPW;

        public int LogUid;
    }

    [Serializable]
    public class WebLoginReply
    {
        public ResultCode result;

        public string DebugMessage;

        public string RemoteEndPoint;

        public int memberUniquelID;

        public string memberID;

        public string Nickname;

        public double SessionID;

        public double Balnce;

        public int Sex;

        public int ratio;

        public int Energy;

        public int Star;

        public double Profit;

        public double Pool;

        public string ExtInfo;

        public string NewPW;

        public Dictionary<string, SystemTextColorWeb> SysTemText = new Dictionary<string, SystemTextColorWeb>();

        public double Super;

        public double Mege;

        public double Major;

        public double Minor;

        public double SuperBase;

        public double MegeBase;

        public double MajorBase;

        public double MinorBase;

        public double HalfJPGetMinBet;

        public double AllJPGetMinBet;

        public int JPGetLevelCount;

        public Dictionary<int, int> Data3 = new Dictionary<int, int>();

        public Dictionary<int, int> LockGameList = new Dictionary<int, int>();

        public List<string> CalculateInfo = new List<string>();

        public List<string> Data2 = new List<string>();

        public Dictionary<string, string> Data4 = new Dictionary<string, string>();

        public int SelectGameCode;

        public string EntityAccount;
    }

    [Serializable]
    public class WebSelectGame
    {
        public GameServerCodeWeb gameServerCode;
    }

    [Serializable]
    public class WebSelectGameReply
    {
        public ResultCode result;

        public GameServerCodeWeb gameServerCode;

        public string url = "";
    }

    [Serializable]
    public class WebVerification
    {
        public GameServerCodeWeb gameServerCode;

        public int memberUniquelID;
    }

    [Serializable]
    public class WebBroadcast
    {
        public ResultCode result;

        public Dictionary<string, SystemTextColorWeb> SysTemText = new Dictionary<string, SystemTextColorWeb>();
    }

    /// <summary>系統命令</summary>
    public enum SystemCommandWeb
    {
        /// <summary>系統維護公告廣播</summary>
        ServiceNotification,
        /// <summary>變更公告廣播</summary>
        ChangeNotification,
        /// <summary>返水</summary>
        RebateInfo, //#250807
    }

    [Serializable]
    public class WebSystemCommandData
    {
        public ResultCode result;

        public SystemCommandWeb Command;

        public Dictionary<string, string> Instruction;

        public List<int> DataList;

        public List<string> Message;
    }

    [Serializable]
    public class WebJPInfo
    {
        public double Super;

        public double Mege;

        public double Major;

        public double Minor;

        public double SuperBase;

        public double MegeBase;

        public double MajorBase;

        public double MinorBase;

        public double HalfJPGetMinBet;

        public double AllJPGetMinBet;

        public int JPGetLevelCount;

        public int TreasuryStatus;
    }

    [Serializable]
    public class WebCheckVersion
    {
        public ResultCode result;
        public string RemoteEndPoint;
        public string memberID;
        public string memberPW;
        public string NewPW;
        public string DebugMessage;
    }

    [Serializable]
    public class WebModifyPassWordReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public string DebugMessage;
    }

    
    [Serializable]
    public class WebManagerChangeClientCridet
    {
        public WebActorUpdataCode actorUpdataCode;
        public double WinCredit;
        public double Balnce;
        public int memberUniquelID;
        public double Profit;
        public double Pool;
        public double SessionID;
    }

    [Serializable]
    public class WebUpdateUserBalance
    {
        public WebActorUpdataCode actorUpdataCode;
        public int memberUniquelID;
        public double Balnce;
        public int Energy;
        public int Star;
        public double SessionID;
    }

    [Serializable]
    public class WebJPAccountData
    {

        public JpAwardWeb JPType;

        public string GameNmae;

        public double TotalWin;

        public string User;

        public string DateTime;
    }

    [Serializable]
    public class WebGetJPAcoountData
    {
        public int memberUniquelID;
        public JpAwardWeb JPType;
    }

    [Serializable]
    public class WebGetJPAcoountDataReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public List<WebJPAccountData> jpgameaccountdata = new List<WebJPAccountData>();
    }

    [Serializable]
    public class WebCommonInfoData
    {
        public string Command;

        public GameServerCodeWeb GameServer;

        public int MachineUID;

        public int UserUID;

        public int Type;

        public string Message;

        public Dictionary<string, string> Data;


        public WebCommonInfoData()
        {
            Command = "";
            Message = "";
            Data = new Dictionary<string, string>();
        }
    }

    [Serializable]
    public class WebGetUserTransaction
    {
        public int memberUniquelID;
        public string StartTime;
        public string EndTime;
    }

    [Serializable]
    public class WebUserTransaction
    {
        public string TransactionNo;

        public string TransactionTime;

        public string TransactionType;

        public string PaymentType;

        public double TransactionAmount;

        public string Status;
    }

    [Serializable]
    public class WebGetUserTransactionReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public List<WebUserTransaction> usertransaction;
    }

    [Serializable]
    public class WebGetSelfeGameData
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class WebGetSelfeGameDataReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public string Nickname;
        public int Sex;
        public List<UserRankStruWeb> userRankStru = new List<UserRankStruWeb>();
    }

    [Serializable]
    public class WebGetSelfeDayMissionData
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class WebPlayerDayMissionInfo
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>任務號碼</summary>
        public int MissionID { get; set; }
        /// <summary>狀態 0未完成 1已完成 2已兌獎</summary>
        public int Status { get; set; }
        /// <summary>任務編碼</summary>
        public string TaskDetail { get; set; }
        /// <summary>獎勵編碼</summary>
        public string AwardDetail { get; set; }
        /// <summary>時間戳記</summary>
        public string BuildTime { get; set; }
    }

    [Serializable]
    public class WebDayMissionInfo
    {
        public int WeekMissionStatus;
        public int WeekMissionCount;
        public string WeekMissionAward;
        public List<WebPlayerDayMissionInfo> playerDayMissionInfo = new List<WebPlayerDayMissionInfo>();
    }

    [Serializable]
    public class WebWebDayMissionInfoReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public WebDayMissionInfo dayMissionInfo  = new WebDayMissionInfo();
    }

    [Serializable]
    public class WebGetSelfLottoTicketData
    {
        public int memberUniquelID;
        public string SignInData;
    }
   
    [Serializable]
    public class WebLottoTicketData
    {
        /// <summary>唯一碼</summary>
        
        public int TicketUID { get; set; }
        /// <summary>期號</summary>
        
        public string ISSNID { get; set; }
        /// <summary>玩家UID</summary>
        
        public int UserUID { get; set; }
        /// <summary>彩卷狀態</summary>
        
        public int Status { get; set; }
        /// <summary>彩卷號碼</summary>
        
        public string TicketNumber { get; set; }
        /// <summary>獎項類型</summary>
        
        public string PrizeType { get; set; }
        /// <summary>時間戳記</summary>
        
        public string TimeStamp { get; set; }
    }

    [Serializable]
    public class WebPhaseLottoTicketData
    {
        /// <summary>頭獎</summary>
        
        public string FirstPrize { get; set; }
        /// <summary>二獎</summary>
        
        public string SecondPrize { get; set; }
        /// <summary>三獎</summary>
        
        public string ThirdPrize { get; set; }
        /// <summary>特獎</summary>
        
        public string SpecialPrize { get; set; }
        /// <summary>封牌時間</summary>
        
        public string ClosureTime { get; set; }
        /// <summary>封牌時間</summary>
        
        public string CurrentTime { get; set; }
        /// <summary>玩家該期彩票</summary>
        
        public List<WebLottoTicketData> lottoTicketData = new List<WebLottoTicketData>();
    }

    [Serializable]
    public class WebUserLottoTicketData
    {
        
        public string Prev_ISSNID;
        
        public string Next_ISSNID;
        /// <summary>玩家所有彩票</summary>
        
        public Dictionary<string, WebPhaseLottoTicketData> userLottoTicketData = new Dictionary<string, WebPhaseLottoTicketData>();
    }

    [Serializable]
    public class WebGetSelfLottoTicketDataReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public WebUserLottoTicketData userLottoTicketData = new WebUserLottoTicketData();
    }

    [Serializable]
    public class WebSetLottoTicketNumber
    {
        /// <summary>唯一碼</summary>
        
        public int TicketUID { get; set; }
        /// <summary>彩票號</summary>
        
        public string TicketNumber { get; set; }
        /// <summary>設定結果</summary>
        
        public string Result { get; set; }
    }

    [Serializable]
    public class WebSetSelfLottoTicketNumber
    {
        public int memberUniquelID;
        public WebSetLottoTicketNumber setLottoTicketNumber;
    }

    [Serializable]
    public class WebSetSelfLottoTicketNumberReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public WebSetLottoTicketNumber setLottoTicketNumber;
    }

    [Serializable]
    public class WebGetReward
    {
        public int memberUniquelID;
        public WebRewardType rewardType;
        public int RewardUID;
    }

    [Serializable]
    public class WebGetRewardReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public WebRewardType rewardType;
        public int RewardUID;
        public string RewardResult;
    }

    [Serializable]
    public class WebGetSelfSignInData
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class WebGetSelfSignInDataReply
    {
        public ResultCode result;
        public int memberUniquelID;
        public string SignInData;
    }

    [Serializable]
    public class WebSyncUserData
    {
        public int memberUniquelID;
        public int Star;
    }

    [Serializable]
    public class WebDBCacheData
    {
        [DataMember]
        public string dayDate;
        [DataMember]
        public List<Dictionary<string, string>> userGameData = new List<Dictionary<string, string>>();
        [DataMember]
        public Dictionary<string, string> UpdateXiyouBigWaterPool = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> CheckXiyouBigWaterProFitDays = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> UpdataProbabilityData = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> UpdataHistoryData = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> UpdateXiyouAccountDetailData = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> GlobalJPSettingData = new Dictionary<string, string>();
        [DataMember]
        public string LobbyMode;
        [DataMember]
        public List<Dictionary<string, string>> MachineData = new List<Dictionary<string, string>>();
        [DataMember]
        public List<Dictionary<string, string>> SettingData = new List<Dictionary<string, string>>();
        [DataMember]
        public List<Dictionary<string, string>> GameGlobalSettingData = new List<Dictionary<string, string>>();
        [DataMember]
        public List<Dictionary<string, string>> HistoryData = new List<Dictionary<string, string>>();
        [DataMember]
        public List<Dictionary<string, string>> ProbabilityData = new List<Dictionary<string, string>>();
        [DataMember]
        public List<Dictionary<string, string>> XiyouAccountDetailData = new List<Dictionary<string, string>>();
        [DataMember]
        public Dictionary<string, string> JPBonusAccountData = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> GameAwardAccountData = new Dictionary<string, string>();
        [DataMember]
        public List<Dictionary<string, string>> BotRuleData = new List<Dictionary<string, string>>();
        [DataMember]
        public List<Dictionary<string, string>> BotRestData = new List<Dictionary<string, string>>();
        [DataMember]
        public Dictionary<string, string> BotModeData = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> MachineRankData = new Dictionary<string, string>();
        [DataMember]
        public List<Dictionary<string, string>> FakeBotRankGameData = new List<Dictionary<string, string>>();
        [DataMember]
        public string LogData;
    }

    [Serializable]
    public class WebVerificationReply
    {
        public ResultCode result;

        public string DebugMessage;

        public string IP;

        public int memberUniquelID;

        public int entityUID;

        public string memberID;

        public string memberPW;

        public int Sex;

        public string Nickname;

        public double Balnce;

        public double SessionID;

        public int Energy;

        public int Star;

        public double Profit;

        public double Pool;

        public string ExtInfo;

        public WebDBCacheData dbcacheData;
    }
    
    [Serializable]
    public class WebLockGameData
    {
        public Dictionary<int, int> LockGameList = new Dictionary<int, int>();
    }

    [Serializable]
    public class WebUserRegisteredData
    {
        public WebOperationCode operationCode;

        public string Status;

        public string UserAccount;

        public string Password;

        public string CPassword;

        public string ManagerAccount;

        public string RemoteEndPoint;

        public string PhoneNumber;

        public string IntroducerAccount;
    }

    [Serializable]
    public class WebLobbyBonus
    {
        public string LobbyBonus;
    }

    [Serializable]
    public class WebReceiveLobbyBonus
    {
        public string LobbyBonus;
    }

    [Serializable]
    public class WebIntroducerPayOutFg
    {
        public string Command;
    }

    [Serializable]
    public class WebReviewTransaction
    {
        public string Command;
    }

    [Serializable]
    public class WebGetWhatsAppGroup
    {
        public string Result;
    }

    [Serializable]
    public class WebH5Command
    {
        public string Command;
        public ResultCode result;
        public string info;
    }

    [Serializable]
    public class WebGetAnnouncementCustomer
    {
        public ResultCode result;
        public string info;
    }

    [Serializable]
    public class WebClientVnpayBankList
    {
        public WebClientVnpayBankInfo[] mBankList;
    }

    [Serializable]
    public class WebClientVnpayBankInfo
    {
        [DataMember]
        public string STT { get; set; }

        [DataMember]
        public string English_name { get; set; }

        [DataMember]
        public string Vietnamese_name { get; set; }

        [DataMember]
        public string Bank_code { get; set; }

        [DataMember]
        public string BIC_code { get; set; }

        public WebClientVnpayBankInfo()
        {
            STT = "";
            English_name = "";
            Vietnamese_name = "";
            Bank_code = "";
            BIC_code = "";
        }
    }

    [Serializable]
    public class WebClientVnpayTransactionList
    {
        public WebClientVnpayTransactionInfo[] mTransactionList;
    }

    [Serializable]
    public class WebClientVnpayTransactionInfo
    {
        [DataMember]
        public string TransactionID;

        [DataMember]
        public string Type;

        [DataMember]
        public string Amount;

        [DataMember]
        public string Status;

        public WebClientVnpayTransactionInfo()
        {
            TransactionID = "";
            Type = "";
            Amount = "";
            Status = "";
        }
    }

    [Serializable]
    public class WebVnpayDepositRequest
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Ret_Msg { get; set; }

        [DataMember]
        public string Call_Back_Url { get; set; }

        public WebVnpayDepositRequest()
        {
            Status = "";
            Ret_Msg = "";
            Call_Back_Url = "";
        }
    }

    [Serializable]
    public class WebVnpayPayoutRequest
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Ret_Msg { get; set; }

        [DataMember]
        public string Transaction_ID { get; set; }

        public WebVnpayPayoutRequest()
        {
            Status = "";
            Ret_Msg = "";
            Transaction_ID = "";
        }
    }

    [Serializable]
    public class WebVnpayBankList
    {
        public WebVnpayBankInfo[] mBankList { get; set; }
        public string info { get; set; }
        public List<string> mChannelList { get; set; }

        public WebVnpayBankList()
        {
            info = "";
            mChannelList = new List<string>();
        }
    }

    [Serializable, DataContract]
    public class WebVnpayBankInfo
    {
        public string STT { get; set; }

        public string English_name { get; set; }

        public string Vietnamese_name { get; set; }

        public string Bank_code { get; set; }

        public string BIC_code { get; set; }

        public WebVnpayBankInfo()
        {
            STT = "";
            English_name = "";
            Vietnamese_name = "";
            Bank_code = "";
            BIC_code = "";
        }
    }

    [Serializable]
    public class WebReturnToLocalLobby
    {
        public GameServerCodeWeb gameServerCode;

        public int memberUniquelID;
    }

    [Serializable]
    public class DBCacheData
    {
        public string dayDate;

        public List<Dictionary<string, string>> userGameData;

        public Dictionary<string, string> UpdateXiyouBigWaterPool;

        public Dictionary<string, string> CheckXiyouBigWaterProFitDays;

        public Dictionary<string, string> UpdataProbabilityData;
   
        public Dictionary<string, string> UpdataHistoryData;

        public Dictionary<string, string> UpdateXiyouAccountDetailData;
 
        public Dictionary<string, string> GlobalJPSettingData;
    
        public string LobbyMode;
     
        public List<Dictionary<string, string>> MachineData;

        public List<Dictionary<string, string>> SettingData;

        public List<Dictionary<string, string>> GameGlobalSettingData;
  
        public List<Dictionary<string, string>> HistoryData;
  
        public List<Dictionary<string, string>> ProbabilityData;
  
        public List<Dictionary<string, string>> XiyouAccountDetailData;
   
        public Dictionary<string, string> JPBonusAccountData;

        public Dictionary<string, string> GameAwardAccountData;
  
        public List<Dictionary<string, string>> BotRuleData;

        public List<Dictionary<string, string>> BotRestData;
 
        public Dictionary<string, string> BotModeData;
 
        public Dictionary<string, string> MachineRankData;

        public List<Dictionary<string, string>> FakeBotRankGameData;

        public string LogData;
    }

    [Serializable]
    public class WebGameLogin
    {
        public int LogUid;

        public string LoginToken;

        public string SelectGameCode;
    }

    [Serializable]
    public class WebRoomActionInfo
    {
        public ResultCode result;

        public string DebugMessage;

        public int memberUniquelID;

        public string Nickname;

        public short Sex;

        public short Roomindx;

        public short Seatindex;

        public bool IsGameEnd;

        public List<WebRoomPlayerInfo> RoomPlayerInfos = new List<WebRoomPlayerInfo>();
    }

    [Serializable]
    public class WebRoomPlayerInfo
    {
        /// <summary>座位代號</summary>

        public short Seatindex;
        /// <summary>使用者唯一碼</summary>
        
        public int memberUniquelID;
        /// <summary>使用者帳號</summary>
        
        public int UserAccount;
        /// <summary>暱稱</summary>
        
        public string Nickname;
        /// <summary>性別</summary>
        
        public short Sex;

        public string Message;
    }

    [Serializable]
    public class RoomActorBorning
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class RoomActorActionUpdateRequest
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class RoomBroadcastActorFishScript
    {
        public List<WebFishPathInfo> mAllGameFishPathInfo = new List<WebFishPathInfo>();

        public WebFishCommand mFishCommand;
    }

    [Serializable]
    public class WebFishCommand
    {
        public string Command;
    }

    //[Serializable]
    //public class RoomBroadcastActorAction
    //{
    //    public List<WebFishPathInfo> mAllGameFishPathInfo = new List<WebFishPathInfo>();

    //    public WebFishCommand mFishCommand;
    //}

    [Serializable]
    public class RoomBroadcastActorQuit
    {
        public WebRoomActorQuit roomActorQuitData = new WebRoomActorQuit();
    }

    [Serializable]
    public class WebRoomActorQuit
    {
        public int memberUniquelID;
    }

    [Serializable]
    public class MultiGameBroadcast
    {
        public ResultCode result;

        public string DebugMessage;

        public GameServerCodeWeb gameServerCode;

        public WebGameStateCode GameState;

        public List<JoinRoomInfo> mRoomInfo = new List<JoinRoomInfo>();

        public List<WebHistoryInfo> historyData = new List<WebHistoryInfo>();

        public WebGameTimeInfo gameTimeInfo = new WebGameTimeInfo();

        public UserDataWeb userData = new UserDataWeb();

        public GameInfo gameInfo = new GameInfo();
    }

    [Serializable]
    public sealed class ProbeAckPayload
    {
        /// <summary>對應 probe 請求的 ProbeId，讓 Client 能識別是哪一次的回覆。</summary>
        public string ProbeId { get; set; } = string.Empty;

        /// <summary>Server 處理 probe 的 UTC 時間。</summary>
        public DateTime ServerTime { get; set; }
    }

    [Serializable]
    public class ExtraDataMsg
    {
        public string Message;

        public List<int> Data1;

        public List<string> Data2;

        public Dictionary<int, int> Data3;

        public Dictionary<string, string> Data4;

        public List<double> Data5;
    }

    [Serializable]
    public class GetRoomInfoBroadcastMsg
    {
        public ResultCode result;

        public string DebugMessage;

        public ExtraDataMsg extraData = new ExtraDataMsg();
    }

    [Serializable]
    public sealed class StressTestMsg
    {
        public string Command;
    }
}
