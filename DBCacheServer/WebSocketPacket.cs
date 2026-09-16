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
        ReConnect
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
        WebMuseumHeist2
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

    [Serializable]
    public class Packet
    {
        public WebMsgType Type;
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

        public List<int> MagnificationForm;
        
        public int Prize;
        
        public Dictionary<int, int> MainLamp;
        
        public Dictionary<int, int> SubLamp;
        
        public int BankerPlayer;
        
        public bool OpenPrizeType;
        
        public int LeftRollActor;
        
        public int RightRollActor;
        
        public List<int> EnemyFightMove;
        
        public List<int> PlayerFightMove;
        
        public double Bonus;
        #endregion

        #region 老虎機
        
        public List<int> SlotIcon { get; set; }  ////Highway, 金雞 15輪資訊
        
        public List<int> WildMultiplier { get; set; }  //Highway 9 線附加倍率
        
        public int FreeGameAction;
        
        public int FreeGameTotalRound;
        
        public int FreeGameRound;
        
        public int FreeGameMultiplier;
        
        public double FreeGameTotalWin;
        
        public double FreeGameFeatureWin;
        
        public int FreeGameType;
        
        public int FreeGameMode;
        
        public List<int> LeftWild_X { get; set; }
        
        public List<int> RightWild_X { get; set; }
        
        public int JpGameType;  //金雞JP Game 中獎種類
        
        public double JpGamePoint; //金雞JP Game 中獎分數
        
        public double JpGrand; //金雞JP Grand 分數
        
        public double JpMajor; //金雞JP Major 分數
        
        public double JpMinor; //金雞JP Minor 分數
        
        public double JpMini; //金雞JP Mini 分數
        
        public double JpMultiplier; //金雞JP倍數
        
        public List<double> ExtData2;
        
        public List<string> ExtData3;
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
        public double[] Info;
        
        public int LevelInfo;  //押分Level
        
        public double PlayInfo;  //押分單位
        
        public Dictionary<int, bool> AskRedEnvelope; //要求遊戲紅包 <玩家代號, 要求旗號>
        
        public List<int> IllegalUser;
        
        public List<int> illegalMinStartRate;
        
        public Dictionary<int, Equipment> UserEquipment;
        
        public Dictionary<int, RedEnvelope> UserRedEnvelope;
        
        public Dictionary<int, double[]> AllRoomActorBetinfo;
        
        public Dictionary<int, double> AllRoomActorCreditInfo; //全部玩家本局Credit資訊
        
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
        
        public Dictionary<string, string> ExtInfo; //額外資訊
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
    }

    [Serializable]
    public class JoinRoom
    {
        public int memberUniquelID;
        public short roomIndex;
    }

    [Serializable]
    public class JoinRoomReply
    {
        public ResultCode result;

        public string DebugMessage;

        public GameServerCodeWeb gameServerCode;

        public JoinRoomInfo roomInfo  = new JoinRoomInfo();

        public GameInfo gameInfo  = new GameInfo();

        public BetInfo betInfo  = new BetInfo();
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

        public List<JoinRoomInfo> roomInfo =new List<JoinRoomInfo>();
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
    
}
