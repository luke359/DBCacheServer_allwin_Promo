using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using VerProtocol;

namespace Protocol
{
    public class Message
    {
        //默认密钥向量 
        static byte[] _IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        static string _Key = "dongbinhuiasxiny";//密钥,128位  

        /// <summary>
        /// 運算SessionID
        /// </summary>
        /// <returns></returns>
        public static double GetSessionID()
        {
            DateTime DateStart = new DateTime(2000, 1, 1, 0, 0, 0);

            return Convert.ToDouble(Math.Round((DateTime.Now - DateStart).TotalSeconds));
        }

        /// <summary>
        /// 計算SearchDays
        /// </summary>
        public static int GetSearchDays(DateTime date)
        {
            return (date - new DateTime(2020, 1, 1)).Days;
        }

        #region OperationCode
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <returns></returns
        public static GameData Pack(OperationCode oper)
        {
            GameData Sendata = new GameData();
            Sendata.operationCode = oper;

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="botDatas"></param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, List<BotData> botDatas)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.BotData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.botdatalist = botDatas;

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="JPType">JP類型</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, JpAward JPType, bool surplusRegulate, bool IsAccount, int commandType, int machUid, int playerUid)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.pushJP);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.pushJPData.JPType = JPType;
            Sendata.pushJPData.SurplusRegulate = surplusRegulate;
            Sendata.pushJPData.IsAccount = IsAccount;
            Sendata.pushJPData.CommandType = commandType;
            Sendata.pushJPData.MachineUid = machUid;
            Sendata.pushJPData.PlayerUid = playerUid;

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="JPType">JP類型</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, JpAward JPType, int UserUID)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.pushJP);
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.jpgameaccount.JPType = JPType;
            Sendata.operationCode = oper;
            Sendata.userData.memberUniquelID = UserUID;

            return Sendata;
        }

        ///// <summary>發送紅包金額</summary>
        ///// <returns></returns>
        //public static GameData SendGameRedEnvelopePointPack(double point)
        //{
        //    List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
        //    GameData Sendata = new GameData(tempClassType);
        //    Sendata.operationCode = OperationCode.SendGameRedEnvelope;
        //    Sendata.GameRedEnvelopeData = point;
        //    return Sendata;
        //}
        /// <summary>發送要求取得紅包金額</summary>
        /// <returns></returns>
        public static GameData GetGameRedEnvelopePointPack(int UserUID)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = OperationCode.GetGameRedEnvelope;
            Sendata.userData.memberUniquelID = UserUID;
            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="updatedata">DB資料</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, Dictionary<string, string> updatedata)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            GameData SendData = new GameData(tempClassType);

            SendData.operationCode = oper;

            switch (oper)
            {
                case OperationCode.ReplaceXiyouAccountDetailData:
                    SendData.dbcacheData.UpdateXiyouAccountDetailData = updatedata;
                    break;
                case OperationCode.UpdateXiyouBigWaterPool:
                    SendData.dbcacheData.UpdateXiyouBigWaterPool = updatedata;
                    break;
                case OperationCode.InsertXiyouBigWaterRec:
                    SendData.dbcacheData.UpdateXiyouBigWaterPool = updatedata;
                    break;
                case OperationCode.UpdateRealJPData:
                    SendData.dbcacheData.JPBonusAccountData = updatedata;
                    break;
                case OperationCode.EnforceJPUpdateWebLog:
                    SendData.dbcacheData.JPBonusAccountData = updatedata;
                    break;
                case OperationCode.PullJPBonus:
                    SendData.dbcacheData.JPBonusAccountData = updatedata;
                    break;
                case OperationCode.EnforceJPInsertAccountData:
                    SendData.dbcacheData.JPBonusAccountData = updatedata;
                    break;
                case OperationCode.DBPushJPBonus:
                case OperationCode.AskDBPushJPBonus:
                    break;
                case OperationCode.EnforceAwardUpdateWebLog:
                    SendData.dbcacheData.GameAwardAccountData = updatedata;
                    break;
                case OperationCode.insertMachineRankData:
                    SendData.dbcacheData.MachineRankData = updatedata;
                    break;
            }

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Roomindex">房間編號</param>
        /// <param name="allroomactorbetinfo">房間內所有人的押注資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, int Roomindex, int machUid, Dictionary<int, double[]> allroomactorbetinfo, Dictionary<int, bool> askRedEnvelope)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.roomData);
            tempClassType.Add(GameData.ClassType.betInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.roomData.roomIndex = (short)Roomindex;
            SendData.roomData.MachineUID = machUid;

            SendData.betInfo.AllRoomActorBetinfo = allroomactorbetinfo;
            SendData.betInfo.AskRedEnvelope = askRedEnvelope;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Roomindex">房間編號</param>
        /// <param name="illegalUser">違法的玩家</param>
        /// <param name="illegalMinStartRate">總押分不足的玩家</param>
        /// <returns></returns>
        //public static GameData SendUserBalanceCheckResult(OperationCode oper, int Roomindex, List<int> illegalUser, List<int> illegalMinStartRate, Dictionary<int, Equipment> userEquipment, Dictionary<int, RedEnvelope> userRedEnvelope, Dictionary<int, double> userCredit, string extInfo = null)
        //{
        //    List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
        //    tempClassType.Add(GameData.ClassType.roomData);
        //    tempClassType.Add(GameData.ClassType.betInfo);
        //    GameData SendData = new GameData(tempClassType);
        //    SendData.operationCode = oper;
        //    SendData.roomData.roomIndex = (short)Roomindex;
        //
        //    //SendData.betInfo.IllegalUser = (illegalUser != null ? illegalUser : null);
        //    //SendData.betInfo.illegalMinStartRate = (illegalMinStartRate != null ? illegalMinStartRate : null);
        //    SendData.betInfo.IllegalUser = illegalUser;
        //    SendData.betInfo.illegalMinStartRate = illegalMinStartRate;
        //    SendData.betInfo.UserEquipment = userEquipment;
        //    SendData.betInfo.UserRedEnvelope = userRedEnvelope;
        //    SendData.betInfo.AllRoomActorCreditInfo = userCredit;
        //    SendData.betInfo.ExtInfo = extInfo;
        //
        //    return SendData;
        //}
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Dbdata">DB資料</param>
        /// <returns></returnsRoomindex
        public static GameData Pack(OperationCode oper, int index, Dictionary<string, string> updatedata, string date = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            tempClassType.Add(GameData.ClassType.roomData);
            tempClassType.Add(GameData.ClassType.historyData);
            GameData SendData = new GameData(tempClassType);

            SendData.operationCode = oper;
            switch (oper)
            {
                case OperationCode.UpdateProbabilitytabl:
                    SendData.roomData.roomIndex = (short)index;
                    SendData.roomData.MachineUID = index;
                    SendData.dbcacheData.UpdataProbabilityData = updatedata;
                    break;
                case OperationCode.UpdateHistory:
                    SendData.historyData.Add(new HistoryInfo());
                    SendData.historyData[0].HistoryUID = index;
                    SendData.dbcacheData.UpdataHistoryData = updatedata;
                    break;
                //case OperationCode.UpdateXiyouAccountDetailData:
                //    SendData.roomData.roomIndex = (short)index;
                //    SendData.roomData.MachineUID = index;
                //    SendData.dbcacheData.dayDate = (date != "" ? date : "");
                //    SendData.dbcacheData.UpdateXiyouAccountDetailData = updatedata;
                //    break;
                //case OperationCode.UpdateGameOuterAccount:
                //    SendData.roomData.roomIndex = (short)index;
                //    SendData.roomData.MachineUID = index;
                //    SendData.dbcacheData.dayDate = (date != "" ? date : "");
                //    SendData.dbcacheData.UpdateXiyouAccountDetailData = updatedata;
                //    break;
                case OperationCode.CheckXiyouBigWaterProFitDays: //Client
                    SendData.roomData.roomIndex = (short)index;
                    SendData.roomData.MachineUID = index;
                    SendData.dbcacheData.dayDate = (date != "" ? date : "");
                    SendData.dbcacheData.CheckXiyouBigWaterProFitDays = updatedata;
                    break;
            }

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="data">遊戲玩家資料</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, List<Dictionary<string, string>> data)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;

            SendData.dbcacheData.userGameData = data;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="code">遊戲類型</param>
        /// <param name="UserUID">玩家UID</param>
        /// <param name="StartTime">開始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, int UserUID, string Data1, string Data2)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();

            if (oper == OperationCode.GetUserAccountData || oper == OperationCode.GetUserTransaction)
            {
                tempClassType.Add(GameData.ClassType.userData);
                tempClassType.Add(GameData.ClassType.usergameAccount);
                GameData SendData = new GameData(tempClassType);
                SendData.operationCode = oper;
                SendData.userData.memberUniquelID = UserUID;
                SendData.useraccountdata.StartTime = Data1;
                SendData.useraccountdata.EndTime = Data2;

                return SendData;
            }
            else if (oper == OperationCode.ModifyPassWord || oper == OperationCode.WhatsAppModifyPassWord)
            {
                tempClassType.Add(GameData.ClassType.userData);
                GameData SendData = new GameData(tempClassType);
                SendData.operationCode = oper;
                SendData.userData.memberUniquelID = UserUID;
                SendData.userData.memberPW = Data1;
                SendData.userData.NewPW = Data2;

                return SendData;
            }
            return null;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="code">遊戲類型</param>
        /// <param name="UserUID">玩家UID</param>
        /// <param name="StartTime">開始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, int UserUID, UserAccountData mUserAccountData)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();

            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.usergameAccount);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.userData.memberUniquelID = UserUID;
            SendData.useraccountdata = mUserAccountData;

            return SendData;
        }

        /// <summary>
        /// 封裝JP獎項資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="superJP"></param>
        /// <param name="MegaJP"></param>
        /// <param name="MajorJP"></param>
        /// <param name="MinorJP"></param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, double superJP, double MegaJP, double MajorJP, double MinorJP, int TreasuryStatus)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.jpInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            //MARKLU: JP廣播 傳新值
            SendData.jpInfo.Super = superJP;
            SendData.jpInfo.Mege = MegaJP;
            SendData.jpInfo.Major = MajorJP;
            SendData.jpInfo.Minor = MinorJP;
            //SendData.jpInfo.SuperBase = superJPBase;
            //SendData.jpInfo.MegeBase = MegaJPBase;
            //SendData.jpInfo.MajorBase = MajorJPBase;
            //SendData.jpInfo.MinorBase = MinorJPBase;
            SendData.jpInfo.TreasuryStatus = TreasuryStatus; //20201008
            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="gameServer">遊戲伺服器</param>
        /// <param name="code">驗證碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, GameServerCode gameServer, int code = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.gameServerCode = gameServer;
            SendData.userData.memberUniquelID = (code != 0 ? code : 0); ;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="actor">玩家更新類型</param>
        /// <param name="Betinfo">押注資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , ActorUpdataCode actor , double[] Betinfo)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.betInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.actorUpdataCode = actor;

            SendData.betInfo.Info = Betinfo;
            return SendData;
        }
        /// <summary>封裝遊戲押分資訊(金雞, 黃金樹)</summary>
        public static GameData PackBetInfo(int level, double play, int playMode = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.betInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = OperationCode.RoomActorActionUpdate;
            SendData.actorUpdataCode = ActorUpdataCode.UpdateBetInfo;
            SendData.betInfo.PlayInfo = play;
            SendData.betInfo.LevelInfo = level;
            SendData.betInfo.PlayMode = playMode;
            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="actor">玩家更新類型</param>
        /// <param name="isGameEnd">遊戲結束旗標</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , ActorUpdataCode actor , bool isGameEnd)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.roomActionInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.actorUpdataCode = actor;
            SendData.roomActionInfo.IsGameEnd = isGameEnd;

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="UniquelIDOrport">使用者唯一碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, int UniquelIDOrport)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;

            if (oper == OperationCode.HeartBeat)
            {
                SendData.serverData.Port = UniquelIDOrport;
            }
            else
            {
                SendData.userData.memberUniquelID = UniquelIDOrport;
            }

            //switch (oper)
            //{
            //    case OperationCode.JoinRoom:
            //    case OperationCode.QuitServer:
            //    case OperationCode.SendCodeToGameServer:
            //    case OperationCode.GetOtherGameData:
            //    case OperationCode.GetSelfeGameData:
            //    case OperationCode.GetSelfeDayMissionData:
            //    case OperationCode.GetSelfLottoTicketData:
            //    case OperationCode.GetSelfSignInData:
            //    case OperationCode.ReConnect:
            //        {
            //            SendData.userData.memberUniquelID = UniquelIDOrport;
            //        }
            //        break;
            //    case OperationCode.HeartBeat:
            //        {
            //            SendData.serverData.Port = UniquelIDOrport;
            //        }
            //        break;
            //}
            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="SelfUniuelID">自己的唯一碼</param>
        /// <param name="OtherUniquelID">要查的人的唯一碼</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , int SelfUniuelID , int OtherUniquelID , int index = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.roomData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.userData.memberUniquelID = SelfUniuelID;
            SendData.roomData.UniquelID = OtherUniquelID;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="RoomIndex">房間編號</param>
        /// <param name="SeatIndex">位子編號</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, short RoomIndex = 0, short SeatIndex = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.roomData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.roomData.roomIndex = (RoomIndex != (short)0 ? RoomIndex : (short)0);
            Sendata.roomData.SeatIndex = (SeatIndex != (short)0 ? SeatIndex : (short)0);

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="UniqeulID">玩家唯一碼</param>
        /// <param name="RoomIndex">房間編號</param>
        /// <param name="SeatIndex">位子編號</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , int UniqeulID , int MachineUID = 0, short SeatIndex = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.roomData);
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.userData.memberUniquelID = UniqeulID;
            Sendata.roomData.MachineUID = (MachineUID != 0 ? MachineUID : 0);
            Sendata.roomData.SeatIndex = (SeatIndex != (short)0 ? SeatIndex : (short)0);

            return Sendata;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Username">帳號</param>
        /// <param name="PassWord">密碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, string Username, string PassWord, string RemoteEndPoint = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.userData.memberID = Username;
            Sendata.userData.memberPW = PassWord;
            Sendata.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Username">帳號</param>
        /// <param name="PassWord">密碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, string Username, string PassWord, int UserStatus, int GameServer, string RemoteEndPoint = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.userData.memberID = Username;
            Sendata.userData.memberPW = PassWord;
            Sendata.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");
            Sendata.userData.Usersituation = (short)UserStatus;
            if (UserStatus > 1)
            {
                if (GameServer == 4)
                {
                    Sendata.userData.ratio = 5;
                }
                else if (GameServer == 5)
                {
                    Sendata.userData.ratio = 4;
                }
                else
                {
                    Sendata.userData.ratio = GameServer;
                }
            }
            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Username">帳號</param>
        /// <param name="PassWord">密碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, string Username, string PassWord, int UserStatus, int GameServer, int RoomIndex, string RemoteEndPoint = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.mRoomInfo);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.userData.memberID = Username;
            Sendata.userData.memberPW = PassWord;
            Sendata.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");
            Sendata.userData.Usersituation = (short)UserStatus;
            if (UserStatus > 1)
            {
                if (GameServer == 4)
                {
                    Sendata.userData.ratio = 5;
                }
                else if (GameServer == 5)
                {
                    Sendata.userData.ratio = 4;
                }
                else
                {
                    Sendata.userData.ratio = GameServer;
                }
            }

            RoomInfo info = new RoomInfo();

            info.RoomIndex = RoomIndex;

            Sendata.mRoomInfo.Add(info);

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Username">帳號</param>
        /// <param name="PassWord">密碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, string Username, string PassWord, int UserStatus, int GameServer, int RoomIndex, int Seat, string RemoteEndPoint = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.mRoomInfo);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.userData.memberID = Username;
            Sendata.userData.memberPW = PassWord;
            Sendata.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");
            Sendata.userData.Usersituation = (short)UserStatus;
            if (UserStatus > 1)
            {
                if (GameServer == 4)
                {
                    Sendata.userData.ratio = 5;
                }
                else if (GameServer == 5)
                {
                    Sendata.userData.ratio = 4;
                }
                else
                {
                    Sendata.userData.ratio = GameServer;
                }
            }

            RoomInfo info = new RoomInfo();

            info.RoomIndex = RoomIndex;
            info.SeatArray = new string[1];
            info.SeatArray[0] = Seat.ToString();

            Sendata.mRoomInfo.Add(info);

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Situation">使用者狀態</param>
        /// <param name="UserUID">使用者唯一碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , int Situation , int UserUID)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.userData.Usersituation = (short)Situation;
            Sendata.userData.memberUniquelID = UserUID;

            return Sendata;
        }
        /// <summary>
        /// 封裝遊戲資訊 更新DBCache玩家餘額
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="actorUpdata">傳送更新資料表的項目類型</param>
        /// <param name="Balance">玩家遊戲差額</param>
        /// <param name="UniquelID">唯一碼</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ActorUpdataCode actorUpdata, int UniquelID, double Amount = 0, double Balance = 0, double Profit = 0, double Pool = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.actorUpdataCode = actorUpdata;
            Sendata.userData.WinCredit = Amount;
            Sendata.userData.Balnce = Balance;
            Sendata.userData.memberUniquelID = UniquelID;
            Sendata.userData.Profit = Profit;
            Sendata.userData.Pool = Pool; 
            return Sendata;
        }
        /// <summary>
        /// 封裝遊戲資訊, 更新LoginServer玩家餘額,能量條,星星總數
        /// </summary>
        /// <param name="UniquelID">唯一碼</param>
        /// <param name="Balance">玩家遊戲差額</param>
        /// <param name="Energy">能量條</param>
        /// <param name="Star">星星總數</param>
        public static GameData RoomActorUpdateCreditPack(int UniquelID, double Balance, int Energy, int Star, double Profit = 0, double Pool = 0, string ExtInfo = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = OperationCode.RoomActorActionUpdate;
            Sendata.actorUpdataCode = ActorUpdataCode.UpdateCredit;
            Sendata.userData.Balnce = Balance;
            Sendata.userData.Energy = Energy;
            Sendata.userData.Star = Star;
            Sendata.userData.Profit = Profit;
            Sendata.userData.Pool = Pool; 
            Sendata.userData.ExtInfo = ExtInfo;
            Sendata.userData.memberUniquelID = UniquelID;
            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">版本類型</param>
        /// <param name="RemoteEndPoint">同類型遊戲數量</param>
        /// <param name="Count">玩家遠端位址</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, VersionCode type, int Count = 0, string RemoteEndPoint = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.versionData);
            tempClassType.Add(GameData.ClassType.userData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.versionData.type = type;
            SendData.versionData.SameTypeGameCount = (Count != 0 ? Count : 0);
            SendData.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊, 更新LoginServer玩家星星總數
        /// </summary>
        /// <param name="UniquelID">唯一碼</param>
        /// <param name="Star">星星總數</param>
        public static GameData RoomActorUpdateSatrPack(int UniquelID, int Star)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = OperationCode.SyncUserData;
            Sendata.userData.Star = Star;
            Sendata.userData.memberUniquelID = UniquelID;
            return Sendata;
        }
        #endregion

        #region Pack封裝遊戲資訊
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="error">錯誤訊息類型</param>
        /// <returns></returns>
        public static GameData Pack(ErrorCode error, string DebugMessage = "")
        {
            GameData SendData = new GameData();
            SendData.errorCode = error;
            SendData.DebugMessage = DebugMessage;
            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper,ErrorCode error)
        {
            GameData SendData = new GameData();
            SendData.operationCode = oper;
            SendData.errorCode = error;

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="MagnificationForm">倍率表</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, List<int> MagnificationForm)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.gameInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.gameInfo.MagnificationForm = MagnificationForm;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="Bot">機器人資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, List<BotData> Bot, int emptSeatNum = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.BotData);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.botdatalist = Bot;
            SendData.serverData.EmptSeatNum = (emptSeatNum != 0 ? emptSeatNum : 0);

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="DBdata1">DB資訊</param>
        /// <param name="DBdata2">DB資訊</param>
        /// <param name="DBdata3">DB資訊</param>
        /// <param name="DBdata4">DB資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, List<Dictionary<string, string>> DBdata1, List<Dictionary<string, string>> DBdata2, Dictionary<string, string> DBdata3 = null, List<Dictionary<string, string>> DBdata4 = null, string lobbymode = null)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            switch (oper)
            {
                case OperationCode.GetGameSetting:
                    {
                        SendData.dbcacheData.LobbyMode = lobbymode;
                        SendData.dbcacheData.MachineData = DBdata1;
                        SendData.dbcacheData.SettingData = DBdata2;
                        SendData.dbcacheData.GlobalJPSettingData = DBdata3;
                        SendData.dbcacheData.GameGlobalSettingData = DBdata4;
                    }
                    break;
                case OperationCode.GetBotModeRuleTable:
                    {
                        SendData.dbcacheData.BotRuleData = DBdata1;
                        SendData.dbcacheData.BotRestData = DBdata2;
                        SendData.dbcacheData.BotModeData = DBdata3;
                    }
                    break;
            }
            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">使用者唯一碼</param>
        /// <param name="AllRunCount">總轉數</param>
        /// <param name="MachineGamedata">遊戲資料</param>
        /// <param name="PrizeCount">5000轉開獎數</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , ErrorCode error , int UserUID , int AllRunCount , List<MachineRankStru> MachineGamedata , int PrizeCount)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.machineGameData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.userData.memberUniquelID = UserUID;
            SendData.machineGameData.AllRunCount = AllRunCount;
            SendData.machineRankStru = MachineGamedata;
            SendData.machineGameData.PrizeCount = PrizeCount;
            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家UID</param>
        /// <param name="userGameAccount">玩家遊戲帳目</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int UserUID, List<UserGameAccountData> userGameAccount)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.usergameAccountList);

            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.userData.memberUniquelID = UserUID;
            SendData.usergameaccountdata = userGameAccount;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家UID</param>
        /// <param name="userGameAccount">玩家遊戲帳目</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int UserUID, SearchUserGameAccountInfo Info)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.usergameAccountList);

            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.userData.memberUniquelID = UserUID;
            SendData.useraccountdata = Info.mUserAccountData;
            SendData.usergameaccountdata = Info.mUserGameAccountDataList;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家UID</param>
        /// <param name="userTransaction">玩家交易紀錄</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , ErrorCode error , int UserUID , List<UserTransaction> userTransaction)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.userTranssaction);

            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.userData.memberUniquelID = UserUID;
            SendData.usertransaction = userTransaction;

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="DBdata">DB資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , ErrorCode error , List<Dictionary<string, string>> DBdata)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            switch (oper)
            {
                case OperationCode.GetHistory:
                    SendData.dbcacheData.HistoryData = DBdata;
                    break;
                case OperationCode.GetGameInnerAccount:
                    SendData.dbcacheData.ProbabilityData = DBdata;
                    break;
                case OperationCode.GetXiyouAccountDetailData:
                    SendData.dbcacheData.XiyouAccountDetailData = DBdata;
                    break;
                case OperationCode.GetBotModeRuleTable:
                    SendData.dbcacheData.BotRuleData = DBdata;
                    break;
            }

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="DBdata">DB資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , ErrorCode error , Dictionary<string, string> DBdata)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            switch (oper)
            {
                case OperationCode.GetXiyouBigWaterPool:
                    SendData.dbcacheData.UpdateXiyouBigWaterPool = DBdata;
                    break;
                case OperationCode.CheckXiyouBigWaterProFitDays: //Server
                    SendData.dbcacheData.CheckXiyouBigWaterProFitDays = DBdata;
                    break;
                case OperationCode.PullJPBonus:
                    SendData.dbcacheData.JPBonusAccountData = DBdata;
                    break;
            }

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="Data">JP遊戲資料</param>
        /// <param name="ip">玩家IP</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , ErrorCode error , List<JPAccountData> Data, int UserUID)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.jpAccount);
            tempClassType.Add(GameData.ClassType.userData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.userData.memberUniquelID = UserUID;

            SendData.jpgameaccountdata = Data;

            return SendData;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="DebugMessage">訊息字串</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, string DebugMessage , string RemoteEndPoint = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.DebugMessage = DebugMessage;
            Sendata.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");

            return Sendata;
        }
        /// <summary>
        /// 封裝遊戲資訊 (Login Server : OperationCode.Verification)
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="IP">使用者IP</param>
        /// <param name="uniquelID">使用者唯一碼</param>
        /// <param name="Nickname">使用者暱稱</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, string IP, int uniquelID, int entityUID, string userID, string userPW, int Sex, string Nickname, double Balance, double SessionID, int Energy, int Star, double Profit, double Pool, string ExtInfo, DBCacheData DbcacheData)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.userData.IP = IP;
            Sendata.userData.memberUniquelID = uniquelID;
            Sendata.userData.entityUID = entityUID;
            Sendata.userData.memberID = userID;
            Sendata.userData.memberPW = userPW;
            Sendata.userData.Sex = Sex;
            Sendata.userData.Nickname = Nickname;
            Sendata.userData.Balnce = Balance;
            Sendata.userData.SessionID = SessionID;
            Sendata.userData.Energy = Energy;
            Sendata.userData.Star = Star;
            Sendata.userData.Profit = Profit;
            Sendata.userData.Pool = Pool;
            Sendata.userData.ExtInfo = ExtInfo;
            Sendata.dbcacheData = DbcacheData;

            return Sendata;
        }
        /// <summary>
        /// GameServer驗證玩家 (GameServer : OperationCode.VerificationDB)
        /// </summary>
        //public static VerificationData PackVerificationData(OperationCode oper, ErrorCode error, GameServerCode gameServer, string IP, int uniquelID, int entityUID, string userID, string userPW, int Sex, string Nickname, double Balance, double SessionID, int Energy, int Star, double Profit, double Pool, string ExtInfo, DBCacheData DbcacheData)
        //{
        //    VerificationData Sendata = new VerificationData();
        //    Sendata.operationCode = oper;
        //    Sendata.errorCode = error;
        //    Sendata.gameServerCode = gameServer;
        //    Sendata.IP = IP;
        //    Sendata.memberUniquelID = uniquelID;
        //    Sendata.entityUID = entityUID;
        //    Sendata.memberID = userID;
        //    Sendata.memberPW = userPW;
        //    Sendata.Sex = Sex;
        //    Sendata.Nickname = Nickname;
        //    Sendata.Balnce = Balance;
        //    Sendata.SessionID = SessionID;
        //    Sendata.Energy = Energy;
        //    Sendata.Star = Star;
        //    Sendata.Profit = Profit;
        //    Sendata.Pool = Pool;
        //    Sendata.ExtInfo = ExtInfo;
        //    Sendata.DbcacheData = DbcacheData;
        //
        //    return Sendata;
        //}

        public static VerificationData PackVerificationData(OperationCode oper, GameServerCode gameServer, int code = 0)
        {
            VerificationData SendData = new VerificationData();
            SendData.operationCode = oper;
            SendData.gameServerCode = gameServer;
            SendData.memberUniquelID = (code != 0 ? code : 0); ;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="OtherUserGameData">其他玩家的遊戲資料</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int UserUID, string OtherUserName, int Sex, List<UserRankStru> userRank)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.DBinfo);
            tempClassType.Add(GameData.ClassType.RankStru);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.userData.memberUniquelID = UserUID;
            Sendata.userData.Nickname = OtherUserName;
            Sendata.userData.Sex = Sex;
            Sendata.userRankStru = userRank;

            return Sendata;
        }
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="game">傳送遊戲類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="IP">遊戲伺服器IP</param>
        /// <param name="Port">遊戲伺服器Port</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, GameServerCode game, ErrorCode error, string IP, int Port)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.serverData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.gameServerCode = game;
            Sendata.errorCode = error;
            Sendata.serverData.IP = IP;
            Sendata.serverData.Port = Port;

            return Sendata;
        }


        /// <summary>
        /// 封裝遊戲資訊 (OperationCode.Login)
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="userUID">使用者唯一碼</param>
        /// <param name="nickname">使用者暱稱</param>
        /// <param name="balance">使用者餘額</param>
        /// <param name="Sex">使用者性別</param>
        /// <param name="ratio">換分比例</param>
        /// <param name="SuperJP"></param>
        /// <param name="MegaJP"></param>
        /// <param name="MajorJP"></param>
        /// <param name="MinorJP"></param>
        /// <param name="RemoteEndPoint"></param>
        /// <param name="DebugMessage">訊息字串</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int userUID, string userID, string nickname, double sessionID, double balance, int Sex, int ratio, int Energy, int Star, double Profit, double Pool, string ExtInfo,
            Dictionary<string, SystemTextColor> Text,
            double SuperJP, double MegaJP, double MajorJP, double MinorJP,
            double SuperJPBase, double MegaJPBase, double MajorJPBase, double MinorJPBase,
            double HalfJPGetMinBet, double AllJPGetMinBet, int JPGetLevelCount,
            string RemoteEndPoint = "", string DebugMessage = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.jpInfo);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.DebugMessage = (DebugMessage != "" ? DebugMessage : "");
            Sendata.userData.memberUniquelID = userUID;
            Sendata.userData.memberID = userID;
            Sendata.userData.Nickname = nickname;
            Sendata.userData.SessionID = sessionID;
            Sendata.userData.Balnce = balance;
            Sendata.userData.Sex = Sex;
            Sendata.userData.ratio = ratio;
            Sendata.userData.Energy = Energy;
            Sendata.userData.Star = Star;
            Sendata.userData.Profit = Profit;
            Sendata.userData.Pool = Pool;
            Sendata.userData.ExtInfo = ExtInfo;
            Sendata.userData.RemoteEndPoint = RemoteEndPoint;
            Sendata.userData.SysTemText = Text;
            //MARKLU: JP廣播 Login
            Sendata.jpInfo.Super = SuperJP;
            Sendata.jpInfo.Mege = MegaJP;
            Sendata.jpInfo.Major = MajorJP;
            Sendata.jpInfo.Minor = MinorJP;

            Sendata.jpInfo.SuperBase = SuperJPBase;  //20201008
            Sendata.jpInfo.MegeBase = MegaJPBase;
            Sendata.jpInfo.MajorBase = MajorJPBase;
            Sendata.jpInfo.MinorBase = MinorJPBase;

            Sendata.jpInfo.HalfJPGetMinBet = HalfJPGetMinBet;
            Sendata.jpInfo.AllJPGetMinBet = AllJPGetMinBet;
            Sendata.jpInfo.JPGetLevelCount = JPGetLevelCount;

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊 DBCache->Login (OperationCode.Login)
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="userUID">使用者唯一碼</param>
        /// <param name="nickname">使用者暱稱</param>
        /// <param name="balance">使用者餘額</param>
        /// <param name="Sex">使用者性別</param>
        /// <param name="ratio">換分比例</param>
        /// <param name="SuperJP"></param>
        /// <param name="MegaJP"></param>
        /// <param name="MajorJP"></param>
        /// <param name="MinorJP"></param>
        /// <param name="RemoteEndPoint"></param>
        /// <param name="DebugMessage">訊息字串</param>
        /// <param name="GameBetMinMax">取得機台最低和最高押分設定</param>
        /// <returns></returns>
        //public static GameData Pack(OperationCode oper, ErrorCode error, int userUID, int EntityUID, string userID, string userPW, string nickname, double sessionID, double balance, int Sex, int ratio, int Energy, int Star, double Profit, double Pool, string LobbyInfo, string LittleJP,
        //    Dictionary<string, SystemTextColor> Text,
        //    double SuperJP, double MegaJP, double MajorJP, double MinorJP,
        //    double SuperJPBase, double MegaJPBase, double MajorJPBase, double MinorJPBase,
        //    double HalfJPGetMinBet, double AllJPGetMinBet, int JPGetLevelCount,
        //    Dictionary<int, int> VersionList,
        //    List<string> GameBetMinMax,
        //    Dictionary<string,string> ExtraInfo,
        //    string RemoteEndPoint = "", string DebugMessage = "", string syssetting = "")
        //{
        //    List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
        //    tempClassType.Add(GameData.ClassType.userData);
        //    tempClassType.Add(GameData.ClassType.jpInfo);
        //    tempClassType.Add(GameData.ClassType.ExtraData);
        //    GameData Sendata = new GameData(tempClassType);
        //    Sendata.operationCode = oper;
        //    Sendata.errorCode = error;
        //    Sendata.DebugMessage = (DebugMessage != "" ? DebugMessage : "");
        //    Sendata.userData.memberUniquelID = userUID;
        //    Sendata.userData.entityUID = EntityUID;
        //    Sendata.userData.memberID = userID;
        //    Sendata.userData.memberPW = userPW;
        //    Sendata.userData.Nickname = nickname;
        //    Sendata.userData.SessionID = sessionID;
        //    Sendata.userData.Balnce = balance;
        //    Sendata.userData.Sex = Sex;
        //    Sendata.userData.ratio = ratio;
        //    Sendata.userData.Energy = Energy;
        //    Sendata.userData.Star = Star;
        //    Sendata.userData.Profit = Profit;
        //    Sendata.userData.Pool = Pool;
        //    Sendata.userData.ExtInfo = LobbyInfo;
        //    Sendata.userData.NewPW = LittleJP;
        //    Sendata.userData.RemoteEndPoint = RemoteEndPoint;
        //    Sendata.userData.SysTemText = Text;
        //    //MARKLU: JP廣播 Login
        //    Sendata.jpInfo.Super = SuperJP;
        //    Sendata.jpInfo.Mege = MegaJP;
        //    Sendata.jpInfo.Major = MajorJP;
        //    Sendata.jpInfo.Minor = MinorJP;
        //
        //    Sendata.jpInfo.SuperBase = SuperJPBase;  //20201008
        //    Sendata.jpInfo.MegeBase = MegaJPBase;
        //    Sendata.jpInfo.MajorBase = MajorJPBase;
        //    Sendata.jpInfo.MinorBase = MinorJPBase;
        //
        //    Sendata.jpInfo.HalfJPGetMinBet = HalfJPGetMinBet;
        //    Sendata.jpInfo.AllJPGetMinBet = AllJPGetMinBet;
        //    Sendata.jpInfo.JPGetLevelCount = JPGetLevelCount;
        //    Sendata.extraData.Data3 = VersionList;
        //    Sendata.webInfo = new WebInfo();
        //    Sendata.webInfo.CalculateInfo = new List<string>();
        //    Sendata.webInfo.CalculateInfo.Add(syssetting);
        //
        //    Sendata.extraData.Data2 = GameBetMinMax;
        //    Sendata.extraData.Data4 = ExtraInfo;
        //
        //    return Sendata;
        //}

        /// <summary>
        /// 封裝遊戲資訊 Login->Client (OperationCode.Login)
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="userUID">使用者唯一碼</param>
        /// <param name="nickname">使用者暱稱</param>
        /// <param name="balance">使用者餘額</param>
        /// <param name="Sex">使用者性別</param>
        /// <param name="ratio">換分比例</param>
        /// <param name="SuperJP"></param>
        /// <param name="MegaJP"></param>
        /// <param name="MajorJP"></param>
        /// <param name="MinorJP"></param>
        /// <param name="RemoteEndPoint"></param>
        /// <param name="DebugMessage">訊息字串</param>
        /// <param name="GameBetMinMax">取得機台最低和最高押分設定</param>
        /// <returns></returns>
        //public static GameData Pack(OperationCode oper, ErrorCode error, int userUID, string userID, string nickname, double sessionID, double balance, int Sex, int ratio, int Energy, int Star, double Profit, double Pool, string ExtInfo, string LittleJP,
        //    Dictionary<string, SystemTextColor> Text,
        //    double SuperJP, double MegaJP, double MajorJP, double MinorJP,
        //    double SuperJPBase, double MegaJPBase, double MajorJPBase, double MinorJPBase,
        //    double HalfJPGetMinBet, double AllJPGetMinBet, int JPGetLevelCount,
        //    Dictionary<int, int> VersionList,
        //    Dictionary<int, int> lockGameList,
        //    List<string> GameBetMinMax,
        //    Dictionary<string,string> ExtraInfo,
        //    string RemoteEndPoint = "", string DebugMessage = "", string syssetting = "")
        //{
        //    List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
        //    tempClassType.Add(GameData.ClassType.userData);
        //    tempClassType.Add(GameData.ClassType.jpInfo);
        //    tempClassType.Add(GameData.ClassType.ExtraData);
        //    GameData Sendata = new GameData(tempClassType);
        //    Sendata.operationCode = oper;
        //    Sendata.errorCode = error;
        //    Sendata.DebugMessage = (DebugMessage != "" ? DebugMessage : "");
        //    Sendata.userData.memberUniquelID = userUID;
        //    Sendata.userData.memberID = userID;
        //    Sendata.userData.Nickname = nickname;
        //    Sendata.userData.SessionID = sessionID;
        //    Sendata.userData.Balnce = balance;
        //    Sendata.userData.Sex = Sex;
        //    Sendata.userData.ratio = ratio;
        //    Sendata.userData.Energy = Energy;
        //    Sendata.userData.Star = Star;
        //    Sendata.userData.Profit = Profit;
        //    Sendata.userData.Pool = Pool;
        //    Sendata.userData.ExtInfo = ExtInfo;
        //    Sendata.userData.NewPW = LittleJP;
        //    Sendata.userData.RemoteEndPoint = RemoteEndPoint;
        //    Sendata.userData.SysTemText = Text;
        //    //MARKLU: JP廣播 Login
        //    Sendata.jpInfo.Super = SuperJP;
        //    Sendata.jpInfo.Mege = MegaJP;
        //    Sendata.jpInfo.Major = MajorJP;
        //    Sendata.jpInfo.Minor = MinorJP;
        //
        //    Sendata.jpInfo.SuperBase = SuperJPBase;  //20201008
        //    Sendata.jpInfo.MegeBase = MegaJPBase;
        //    Sendata.jpInfo.MajorBase = MajorJPBase;
        //    Sendata.jpInfo.MinorBase = MinorJPBase;
        //
        //    Sendata.jpInfo.HalfJPGetMinBet = HalfJPGetMinBet;
        //    Sendata.jpInfo.AllJPGetMinBet = AllJPGetMinBet;
        //    Sendata.jpInfo.JPGetLevelCount = JPGetLevelCount;
        //
        //    Sendata.extraData.Data3 = VersionList;
        //
        //    Sendata.LockGameList = new Dictionary<int, int>();
        //
        //    Sendata.LockGameList = lockGameList;
        //
        //    Sendata.webInfo = new WebInfo();
        //    Sendata.webInfo.CalculateInfo = new List<string>();
        //    Sendata.webInfo.CalculateInfo.Add(syssetting);
        //
        //    Sendata.extraData.Data2 = GameBetMinMax;
        //    Sendata.extraData.Data4 = ExtraInfo;
        //
        //    return Sendata;
        //}

        /// <summary>
        /// 封裝遊戲資訊 Login->Client (OperationCode.LoginCheckPlayerStatus / OperationCode.ReConnect)
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="userUID">使用者唯一碼</param>
        /// <param name="nickname">使用者暱稱</param>
        /// <param name="balance">使用者餘額</param>
        /// <param name="Sex">使用者性別</param>
        /// <param name="ratio">換分比例</param>
        /// <param name="SuperJP"></param>
        /// <param name="MegaJP"></param>
        /// <param name="MajorJP"></param>
        /// <param name="MinorJP"></param>
        /// <param name="RemoteEndPoint"></param>
        /// <param name="DebugMessage">訊息字串</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int userUID, string userID, string nickname, double balance, int Sex, int ratio, int Energy, int Star, double Profit, double Pool, string ExtInfo,
            Dictionary<string, SystemTextColor> Text,
            double SuperJP, double MegaJP, double MajorJP, double MinorJP,
            double SuperJPBase, double MegaJPBase, double MajorJPBase, double MinorJPBase,
            double HalfJPGetMinBet, double AllJPGetMinBet, int JPGetLevelCount,
            string RemoteEndPoint = "", string DebugMessage = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.jpInfo);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.DebugMessage = DebugMessage;
            Sendata.userData.memberUniquelID = userUID;
            Sendata.userData.memberID = userID;
            Sendata.userData.Nickname = nickname;
            Sendata.userData.Balnce = balance;
            Sendata.userData.Sex = Sex;
            Sendata.userData.ratio = ratio;
            Sendata.userData.Energy = Energy;
            Sendata.userData.Star = Star;
            Sendata.userData.Profit = Profit;
            Sendata.userData.Pool = Pool;
            Sendata.userData.ExtInfo = ExtInfo;
            Sendata.userData.RemoteEndPoint = RemoteEndPoint;
            Sendata.userData.SysTemText = Text;
            //MARKLU: JP廣播 Login
            Sendata.jpInfo.Super = SuperJP;
            Sendata.jpInfo.Mege = MegaJP;
            Sendata.jpInfo.Major = MajorJP;
            Sendata.jpInfo.Minor = MinorJP;
        
            Sendata.jpInfo.SuperBase = SuperJPBase;  //20201008
            Sendata.jpInfo.MegeBase = MegaJPBase;
            Sendata.jpInfo.MajorBase = MajorJPBase;
            Sendata.jpInfo.MinorBase = MinorJPBase;
        
            Sendata.jpInfo.HalfJPGetMinBet = HalfJPGetMinBet;
            Sendata.jpInfo.AllJPGetMinBet = AllJPGetMinBet;
            Sendata.jpInfo.JPGetLevelCount = JPGetLevelCount;
        
            return Sendata;
        }


        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="GameVersion">同類型遊戲儲存字典</param>
        /// <param name="RemoteEndPoint">玩家遠端位址</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, VersionCode type,Dictionary<int, string> GameVersion, string RemoteEndPoint = "")
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.versionData);
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.versionData.type = type;
            Sendata.versionData.SameTypeGameVersion = GameVersion;
            Sendata.userData.RemoteEndPoint = (RemoteEndPoint != "" ? RemoteEndPoint : "");

            return Sendata;
        }

        /// <summary>
        /// 封裝玩家每日任務資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="mDayMissionInfo">玩家的每日任務資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int UserUID, DayMissionInfo mDayMissionInfo)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.DBinfo);
            tempClassType.Add(GameData.ClassType.dayMission);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.userData.memberUniquelID = UserUID;
            Sendata.dayMissionInfo = mDayMissionInfo;

            return Sendata;
        }

        /// <summary>
        /// 封裝玩家彩票資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="mLottoTicketData">玩家的彩票資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int UserUID, UserLottoTicketData mLottoTicketData)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.DBinfo);
            tempClassType.Add(GameData.ClassType.LottoTicket);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.userData.memberUniquelID = UserUID;
            Sendata.userLottoTicketData = mLottoTicketData;

            return Sendata;
        }

        /// <summary>
        /// 封裝設定彩票號碼回覆
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="mLottoTicketData">玩家的彩票資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int UserUID, SetLottoTicketNumber mSetLottoTicketNumber)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.DBinfo);
            tempClassType.Add(GameData.ClassType.LottoTicket);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.errorCode = error;
            Sendata.userData.memberUniquelID = UserUID;
            Sendata.setLottoTicketNumber = mSetLottoTicketNumber;

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="OtherUserGameData">其他玩家的遊戲資料</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, RewardType type, int UserUID, int RewardUID)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;

            switch (oper)
            {
                case OperationCode.JoinRoom:
                case OperationCode.QuitServer:
                case OperationCode.SendCodeToGameServer:
                case OperationCode.GetOtherGameData:
                case OperationCode.GetSelfeGameData:
                case OperationCode.GetSelfeDayMissionData:
                case OperationCode.GetSelfLottoTicketData:
                case OperationCode.GetReward:
                    {
                        SendData.userData.memberUniquelID = UserUID;
                    }
                    break;
                case OperationCode.HeartBeat:
                    {
                        SendData.serverData.Port = UserUID;
                    }
                    break;
            }
            SendData.rewardType = type;
            SendData.RewardUID = RewardUID;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="OtherUserGameData">其他玩家的遊戲資料</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, RewardType type, int UserUID, int RewardUID, string RewardResult)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;

            switch (oper)
            {
                case OperationCode.JoinRoom:
                case OperationCode.QuitServer:
                case OperationCode.SendCodeToGameServer:
                case OperationCode.GetOtherGameData:
                case OperationCode.GetSelfeGameData:
                case OperationCode.GetSelfeDayMissionData:
                case OperationCode.GetSelfLottoTicketData:
                case OperationCode.GetReward:
                    {
                        SendData.userData.memberUniquelID = UserUID;
                    }
                    break;
                case OperationCode.HeartBeat:
                    {
                        SendData.serverData.Port = UserUID;
                    }
                    break;
            }
            SendData.rewardType = type;
            SendData.RewardUID = RewardUID;
            SendData.RewardResult = RewardResult;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="SignInData">簽到表</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, ErrorCode error, int UserUID, string SignInData)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;

            switch (oper)
            {
                case OperationCode.JoinRoom:
                case OperationCode.QuitServer:
                case OperationCode.SendCodeToGameServer:
                case OperationCode.GetOtherGameData:
                case OperationCode.GetSelfeGameData:
                case OperationCode.GetSelfeDayMissionData:
                case OperationCode.GetSelfLottoTicketData:
                case OperationCode.GetReward:
                case OperationCode.GetSelfSignInData:
                    {
                        SendData.userData.memberUniquelID = UserUID;
                    }
                    break;
                case OperationCode.HeartBeat:
                    {
                        SendData.serverData.Port = UserUID;
                    }
                    break;
            }

            SendData.SignInData = SignInData;

            return SendData;
        }

        /// <summary>
        /// 封裝玩家彩票資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="UserUID">玩家唯一碼</param>
        /// <param name="mLottoTicketData">玩家的彩票資訊</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, int UserUID, string issnid)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.userData.memberUniquelID = UserUID;
            SendData.SignInData = issnid;
            return SendData;
        }

        /// <summary>新增遊戲保留資料打包</summary>
        /// <param name="machine">機台號碼</param>
        /// <param name="reserveInfo">保留訊息</param>
        /// <param name="note">說明(給人看的, 可不要)</param>
        public static GameData PackGameReserveInfo(OperationCode oper, GameServerCode gameServer, int userUid, int machine, string reserveInfo, string note = "")
        {
            GameData SendData = new GameData();
            SendData.operationCode = oper;
            SendData.gameServerCode = gameServer;
            SendData.extraData = new ExtraData();
            SendData.extraData.Data1 = new List<int>() { userUid, machine };
            SendData.extraData.Data2 = new List<string>() { reserveInfo, note };
            return SendData;
        }
        /// <summary>刪除遊戲保留資料打包</summary>
        public static GameData PackGameReserveInfo(OperationCode oper, GameServerCode gameServer, int userUid)
        {
            GameData SendData = new GameData();
            SendData.operationCode = oper;
            SendData.gameServerCode = gameServer;
            SendData.extraData = new ExtraData();
            SendData.extraData.Data1 = new List<int>() { userUid };
            return SendData;
        }
        #endregion

        #region Web
        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">Web操作訊息類型</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, WebCode type, ErrorCode error)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.webInfo.Type = type;

            return SendData;
        }

        public static GameData Pack(WebCode type, ErrorCode error, BatchDepositV2Result result)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData sendData = new GameData(tempClassType);
            sendData.errorCode = error;
            sendData.webInfo.Type = type;
            sendData.webInfo.BatchDepositV2Result = result;
            return sendData;
        }

        public static GameData Pack(WebCode type, ErrorCode error, BatchDepositV2QueryResult result)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData sendData = new GameData(tempClassType);
            sendData.errorCode = error;
            sendData.webInfo.Type = type;
            sendData.webInfo.BatchDepositV2QueryResult = result;
            return sendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">Web操作訊息類型</param>
        /// <param name="DBdata1">DB資料1</param>
        /// <param name="DBdata2">DB資料2</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, WebCode type, List<Dictionary<string, string>> DBdata1, List<Dictionary<string, string>> DBdata2, int machineUID = 0)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            tempClassType.Add(GameData.ClassType.roomData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;

            WebInfo info = new WebInfo();
            info.Type = type;
            SendData.webInfo = info;
            SendData.dbcacheData.SettingData = DBdata1;
            SendData.dbcacheData.GameGlobalSettingData = DBdata2;
            SendData.roomData.MachineUID = (machineUID != 0 ? machineUID : 0);

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">Web操作訊息類型</param>
        /// <param name="botlist"></param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , WebCode type , List<BotData> botlist)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.BotData);
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.webInfo.Type = type;
            SendData.botdatalist = botlist;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">Web操作訊息類型</param>
        /// <param name="DBdata1">JP資料</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, WebCode type, Dictionary<string, string> DBdata)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.DBinfo);
            tempClassType.Add(GameData.ClassType.roomData);
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.webInfo.Type = type;
            SendData.dbcacheData.GlobalJPSettingData = DBdata;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Type">Web訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="HalfJPGetMinBet"></param>
        /// <param name="AllJPGetMinBet"></param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, WebCode Type, ErrorCode error, double superJPBase, double MegaJPBase, double MajorJPBase, double MinorJPBase, double HalfJPGetMinBet, double AllJPGetMinBet, int JPGetLevelCount)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.jpInfo);
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.webInfo.Type = Type;
            //MARKLU: JP廣播 傳新JP設定值
            SendData.jpInfo.SuperBase = superJPBase; //20201008
            SendData.jpInfo.MegeBase = MegaJPBase;
            SendData.jpInfo.MajorBase = MajorJPBase;
            SendData.jpInfo.MinorBase = MinorJPBase;
            SendData.jpInfo.HalfJPGetMinBet = HalfJPGetMinBet;
            SendData.jpInfo.AllJPGetMinBet = AllJPGetMinBet;
            SendData.jpInfo.JPGetLevelCount = JPGetLevelCount;
            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="Type">Web訊息類型</param>
        /// <param name="error">錯誤訊息類型</param>
        /// <param name="emptSeatNum"></param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper , WebCode Type , ErrorCode error, int emptSeatNum)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.webInfo.Type = Type;
            SendData.serverData.EmptSeatNum = emptSeatNum;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="error">錯誤訊息類型</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, WebCode Type, ErrorCode error, Dictionary<string,string> verdata)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            tempClassType.Add(GameData.ClassType.serverData);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.webInfo.Type = Type;
            SendData.webInfo.VerData = verdata;

            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="error">錯誤訊息類型</param>
        /// <returns></returns>
        public static GameData Pack(OperationCode oper, List<UserStatus> userstatuses)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.userData);
            GameData Sendata = new GameData(tempClassType);
            Sendata.operationCode = oper;
            Sendata.userStatuses = userstatuses;

            return Sendata;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">Web操作訊息類型</param>
        /// <returns></returns>
        public static GameData PackDBCount(OperationCode oper, WebCode type, ErrorCode error, string DBQueryCount)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.webInfo.Type = type;
            SendData.webInfo.CalculateInfo = new List<string>();
            SendData.webInfo.CalculateInfo.Add(DBQueryCount);
            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">Web操作訊息類型</param>
        /// <returns></returns>
        public static GameData PackDBCount(OperationCode oper, WebCode type, GameServerCode gameserver, string Command, ErrorCode error, string DBQueryCount)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.gameServerCode = gameserver;
            SendData.webInfo.Type = type;
            SendData.webInfo.CalculateInfo = new List<string>();
            SendData.webInfo.CalculateInfo.Add(DBQueryCount);
            SendData.webInfo.WebCommand = Command;
            return SendData;
        }

        /// <summary>
        /// 封裝遊戲資訊
        /// </summary>
        /// <param name="oper">傳送訊息類型</param>
        /// <param name="type">Web操作訊息類型</param>
        /// <returns></returns>
        public static GameData PackDBCount(OperationCode oper, WebCode type, GameServerCode gameserver, string Command, ErrorCode error, List<string> DBQueryCount)
        {
            List<GameData.ClassType> tempClassType = new List<GameData.ClassType>();
            tempClassType.Add(GameData.ClassType.webInfo);
            GameData SendData = new GameData(tempClassType);
            SendData.operationCode = oper;
            SendData.errorCode = error;
            SendData.gameServerCode = gameserver;
            SendData.webInfo.Type = type;
            SendData.webInfo.CalculateInfo = DBQueryCount;
            SendData.webInfo.WebCommand = Command;
            return SendData;
        }
        #endregion

        public static HeartBeatData PackHeartBeatData(OperationCode oper)
        {
            HeartBeatData Sendata = new HeartBeatData();
            Sendata.operationCode = oper;
            return Sendata;
        }

        public static HeartBeatData PackHeartBeatData(OperationCode oper,int port)
        {
            HeartBeatData Sendata = new HeartBeatData();
            Sendata.operationCode = oper;
            Sendata.Port = port;
            return Sendata;
        }

        public static LockGameData PackLockGameData(OperationCode oper, Dictionary<int,int> dataList)
        {
            LockGameData Sendata = new LockGameData();
            Sendata.operationCode = oper;
            Sendata.LockGameList = dataList;
            return Sendata;
        }

        public static GunActionInfo PackGunActionInfo(OperationCode oper, GunActionInfo gunActionInfo)
        {
            GunActionInfo Sendata = new GunActionInfo();
            Sendata = gunActionInfo;
            Sendata.operationCode = oper;
            return Sendata;
        }

        public static UserRegisteredData PackUserRegisteredData(OperationCode oper, string useraccount = "", string password = "", string cpassword = "", string manageraccount = "", string status = "", string remoteendpoint = "", string phoneNum = "", string introduceraccount = "")
        {
            UserRegisteredData Sendata = new UserRegisteredData();
            Sendata.operationCode = oper;
            Sendata.UserAccount = useraccount;
            Sendata.Password = password;
            Sendata.CPassword = cpassword;
            Sendata.ManagerAccount = manageraccount;
            Sendata.Status = status;
            Sendata.RemoteEndPoint = remoteendpoint;
            Sendata.PhoneNumber = phoneNum;
            Sendata.IntroducerAccount = introduceraccount;
            return Sendata;
        }

        public static PhoneNumberData PackPhoneNumberData(string oderType, int useruid, string status = "",  string phoneNum = "")
        {
            PhoneNumberData Sendata = new PhoneNumberData();
            Sendata.Type = oderType;
            Sendata.Status = status;
            Sendata.UserUID = useruid;
            Sendata.PhoneNumber = phoneNum;
            return Sendata;
        }

        public static CommonCommandData PackCommonCommandData(string tyoe, string context)
        {
            CommonCommandData Sendata = new CommonCommandData();
            Sendata.CommandType = tyoe;
            Sendata.CommandContext = context;
            return Sendata;
        }
        
        public static VnpayInfo PackVnpayInfo(OperationCode oper, int userUID = 0, string msg = "")
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            Sendata.UserUID = userUID;
            Sendata.Msg = msg;
            return Sendata;
        }

        public static VnpayInfo PackVnpayTransactionLog(OperationCode oper, int userUID = 0, string startDate = "", string endDate = "")
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            Sendata.UserUID = userUID;
            string Temp = "";
            Temp = "UserUID:" + userUID.ToString() + ",StartDate:" + startDate  + ",EndDate:" + endDate;
            Sendata.Msg = Temp;
            return Sendata;
        }

        public static VnpayInfo PackVnpayDepositInfo(OperationCode oper, int UserUID, double Amount, string IP_Address, string PaymentType)
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            string Temp = "";
            Temp = "UserUID:" + UserUID.ToString() + ",Amount:" + Amount.ToString("f2") + ",IP_Address:" + IP_Address+ ",PaymentType:"+ PaymentType;
            Sendata.Msg = Temp;
            return Sendata;
        }

        public static VnpayInfo PackVnpayDepositRequest(OperationCode oper, int useruid, string status, string ret_msg, string call_back_url)
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            Sendata.UserUID = useruid;
            string Temp = "";
            Temp = "Status:" + status + ",Ret_Msg:" + ret_msg + ",Call_Back_Url:" + call_back_url;
            Sendata.Msg = Temp;
            return Sendata;
        }

        public static VnpayInfo PackVnpayPayoutInfo(OperationCode oper, int UserUID, double Amount, string Bank_Code, string Bank_Account_Name, string Bank_Account_Number, string IP_Address, string PaymentType)
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            string Temp = "";
            Temp = "UserUID:" + UserUID.ToString() + ",Amount:" + Amount.ToString("f2") + ",Bank_Code:" + Bank_Code + ",Bank_Account_Name:" + Bank_Account_Name + ",Bank_Account_Number:" + Bank_Account_Number+ ",IP_Address: " + IP_Address + ",PaymentType: " + PaymentType;
            Sendata.Msg = Temp;
            return Sendata;
        }

        public static VnpayInfo PackVnpayPayoutRequest(OperationCode oper, int useruid, string status, string ret_msg, string transaction_id)
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            Sendata.UserUID = useruid;
            string Temp = "";
            Temp = "Status:" + status + ",Ret_Msg:" + ret_msg + ",Transaction_ID:" + transaction_id;
            Sendata.Msg = Temp;
            return Sendata;
        }

        public static VnpayPayoutBankList PackVnpayPayoutBankList(OperationCode oper, int useruid, List<VnpayBankInfo> list)
        {
            VnpayPayoutBankList Sendata = new VnpayPayoutBankList();
            Sendata.operationCode = oper;
            Sendata.UserUID = useruid;
            Sendata.mBankList = list;
            return Sendata;
        }

        public static VnpayInfo PackVnpayCommonInfo(OperationCode oper, int UserUID)
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            string Temp = "";
            Temp = "User_UID:" + UserUID.ToString();
            Sendata.Msg = Temp;
            return Sendata;
        }

        public static VnpayInfo PackVnpayCommonInfo(OperationCode oper)
        {
            VnpayInfo Sendata = new VnpayInfo();
            Sendata.operationCode = oper;
            string Temp = "";
            Sendata.Msg = Temp;
            return Sendata;
        }

        public static VnpayDepositRequest GetVnpayDepositRequest(string Msg)
        {
            VnpayDepositRequest Sendata = new VnpayDepositRequest();

            try
            {
                string[] data = Msg.Split(',');

                Sendata.Status = data[0].Replace("Status:", "");
                Sendata.Ret_Msg = data[1].Replace("Ret_Msg:", "");
                Sendata.Call_Back_Url = data[2].Replace("Call_Back_Url:", "");

            }
            catch (Exception ex)
            {
                Sendata = new VnpayDepositRequest();
                //MyConsole.WriteLine("VnpayDepositRequest transform failed")
            }

            return Sendata;
        }

        public static VnpayPayoutRequest GetVnpayPayoutRequest(string Msg)
        {
            VnpayPayoutRequest Sendata = new VnpayPayoutRequest();

            try
            {
                string[] data = Msg.Split(',');

                Sendata.Status = data[0].Replace("Status:", "");
                Sendata.Ret_Msg = data[1].Replace("Ret_Msg:", "");
                Sendata.Transaction_ID = data[2].Replace("Transaction_ID:", "");

            }
            catch (Exception ex)
            {
                Sendata = new VnpayPayoutRequest();
                //MyConsole.WriteLine("VnpayDepositRequest transform failed")
            }

            return Sendata;
        }

        public static VnpayTransactionPre GetVnpayTransactionPre(string Msg)
        {
            VnpayTransactionPre Sendata = new VnpayTransactionPre();

            try
            {
                string[] data = Msg.Split(',');

                Sendata.userUID = Int32.Parse(data[0].Replace("UserUID:", ""));
                Sendata.startDate = data[1].Replace("StartDate:", "");
                Sendata.endDate = data[2].Replace("EndDate:", "");

            }
            catch (Exception ex)
            {
                Sendata = new VnpayTransactionPre();
            }

            return Sendata;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="UnSerializeObj"></param>
        /// <returns></returns>
        public static byte[] SerializrToStream(object UnSerializeObj)
        {
            try
            {
                //分组加密算法
                SymmetricAlgorithm des = Rijndael.Create();
                des.Padding = PaddingMode.Zeros;
                //设置密钥及密钥向量
                des.Key = Encoding.UTF8.GetBytes(_Key);
                des.IV = _IV;

                MemoryStream stream = new MemoryStream();
                MemoryStream SerializrObject = new MemoryStream();

                CryptoStream cs = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] datainfo = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(UnSerializeObj));

                cs.Write(datainfo, 0, datainfo.Length);
                cs.FlushFinalBlock();

                cs.Close();
                stream.Close();
                SerializrObject.Close();

                byte[] decryptBytes = new byte[stream.ToArray().Length];
                MemoryStream ms = new MemoryStream(stream.ToArray());

                CryptoStream cs2 = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs2.Read(decryptBytes, 0, decryptBytes.Length);

                cs2.Close();
                ms.Close();

                //MemoryStream stream = new MemoryStream(SerializeArray);
                MemoryStream stream2 = new MemoryStream(decryptBytes);
                //IFormatter formatter = new BinaryFormatter();
                stream2.Seek(0, SeekOrigin.Begin);

                byte[] datainfo3 = stream2.ToArray();

                //int OLength = 0;

                //for (int i = (datainfo3.Length - 1); i >= 0; i--)
                //{
                //    if (datainfo3[i] == '}')
                //    {
                //        OLength = i + 1;

                //        break;
                //    }
                //}

                //byte[] datainfo4 = new byte[OLength];

                //Array.Copy(datainfo3, datainfo4, OLength);

                string datainfo4 = Encoding.UTF8.GetString(datainfo3);

                var values = JsonConvert.DeserializeObject<Dictionary<string, Object>>(datainfo4);

                if (!values.ContainsKey("MsgType"))
                {
                    int abc = 0;

                    abc += 1;
                }

                return stream.ToArray();//得到加密后的字节数组
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Message:Serialize Error! Message : {0}", ex.Message), Console.ForegroundColor = ConsoleColor.Red);

                byte[] temp = new byte[10];

                return temp;
            }
            
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="SerializeArray"></param>
        /// <returns></returns>
        public static object DeserializeFromStream(byte[] SerializeArray)
        {
            try
            {
                SymmetricAlgorithm des = Rijndael.Create();
                des.Padding = PaddingMode.Zeros;
                des.Key = Encoding.UTF8.GetBytes(_Key);
                des.IV = _IV;

                byte[] decryptBytes = new byte[SerializeArray.Length];
                MemoryStream ms = new MemoryStream(SerializeArray);

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs.Read(decryptBytes, 0, decryptBytes.Length);

                cs.Close();
                ms.Close();

                MemoryStream stream = new MemoryStream(decryptBytes);
                stream.Seek(0, SeekOrigin.Begin);

                string datainfo3 = Encoding.UTF8.GetString(stream.ToArray());
                var values = JsonConvert.DeserializeObject<Dictionary<string, Object>>(datainfo3);
                object UnSerializeObj;
                string msgtype = values["MsgType"].ToString();

                switch (msgtype)
                {
                    case "PhoneNumberData":
                        UnSerializeObj = JsonConvert.DeserializeObject<PhoneNumberData>(datainfo3);
                        break;
                    case "UserRegisteredData":
                        UnSerializeObj = JsonConvert.DeserializeObject<UserRegisteredData>(datainfo3);
                        break;
                    case "VnpayInfo":
                        UnSerializeObj = JsonConvert.DeserializeObject<VnpayInfo>(datainfo3);
                        break;
                    case "VerGameData":
                        UnSerializeObj = JsonConvert.DeserializeObject<VerGameData>(datainfo3);
                        break;
                    case "VerificationData":
                        UnSerializeObj = JsonConvert.DeserializeObject<VerificationData>(datainfo3);
                        break;
                    case "HeartBeatData":
                        UnSerializeObj = JsonConvert.DeserializeObject<HeartBeatData>(datainfo3);
                        break;
                    case "GameData":
                        UnSerializeObj = JsonConvert.DeserializeObject<GameData>(datainfo3);
                        break;
                    case "LockGameData":
                        UnSerializeObj = JsonConvert.DeserializeObject<LockGameData>(datainfo3);
                        break;
                    case "GunActionInfo":
                        UnSerializeObj = JsonConvert.DeserializeObject<GunActionInfo>(datainfo3);
                        break;
                    case "CommonCommandData":
                        UnSerializeObj = JsonConvert.DeserializeObject<CommonCommandData>(datainfo3);
                        break;
                    case "CommonMessageData":
                        UnSerializeObj = JsonConvert.DeserializeObject<CommonMessageData>(datainfo3);
                        break;
                    case "GameServerStatusData":
                        UnSerializeObj = JsonConvert.DeserializeObject<GameServerStatusData>(datainfo3);
                        break;
                    case "CommonInfoData":
                        UnSerializeObj = JsonConvert.DeserializeObject<CommonInfoData>(datainfo3);
                        break;
                    default:
                        UnSerializeObj = null;
                        break;
                }

                return UnSerializeObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Message:Deserialize Error! Message : {0}", ex.Message), Console.ForegroundColor = ConsoleColor.Red);
            }
            return null;
        }
    }

    [Serializable, DataContract]
    public class VnpayInfo
    {
        [DataMember]
        public string MsgType = "VnpayInfo";

        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public OperationCode operationCode { get; set; }

        [DataMember]
        public int UserUID { get; set; }

        [DataMember]
        public string Msg { get; set; }

        public VnpayInfo()
        {
            MsgType = "VnpayInfo";

            operationCode = OperationCode.Null;
            Msg = "";
            UserUID = 0;
        }
    }

    [Serializable, DataContract]
    public class VnpayPayoutBankList
    {
        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public OperationCode operationCode { get; set; }

        /// <summary>玩家UID</summary>
        [DataMember]
        public int UserUID { get; set; }

        /// <summary>銀行列表</summary>
        [DataMember]
        public List<VnpayBankInfo> mBankList { get; set; }

        public VnpayPayoutBankList()
        {
            operationCode = OperationCode.HeartBeat;
            UserUID = 0;
            mBankList = new List<VnpayBankInfo>();
        }
    }

    [Serializable, DataContract]
    public class VnpayDepositRequest
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Ret_Msg { get; set; }

        [DataMember]
        public string Call_Back_Url { get; set; }

        public VnpayDepositRequest()
        {
            Status = "";
            Ret_Msg = "";
            Call_Back_Url = "";
        }
    }

    [Serializable, DataContract]
    public class VnpayPayoutRequest
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Ret_Msg { get; set; }

        [DataMember]
        public string Transaction_ID { get; set; }

        public VnpayPayoutRequest()
        {
            Status = "";
            Ret_Msg = "";
            Transaction_ID = "";
        }
    }

    [Serializable, DataContract]
    public class VnpayDepositLog
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int UserUID { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string TransactionID { get; set; }

        [DataMember]
        public string ClientTransactionID { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public double Credit { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int StatusValue { get; set; }

        [DataMember]
        public string IP_Address { get; set; }

        [DataMember]
        public long SearchIndex { get; set; }

        [DataMember]
        public int TimeIndex { get; set; }

        [DataMember]
        public string Buildtime { get; set; }

        public VnpayDepositLog()
        {
            ID = 0;
            UserUID = 0;
            UserID = "";
            TransactionID = "";
            ClientTransactionID = "";
            Amount = 0.00d;
            Credit = 0.00d;
            Status = "";
            StatusValue = 0;
            IP_Address = "";
            SearchIndex = 0;
            TimeIndex = 0;
            Buildtime = "";
        }
    }

    [Serializable, DataContract]
    public class VnpayPayoutLog
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int UserUID { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string TransactionID { get; set; }

        [DataMember]
        public string ClientTransactionID { get; set; }

        [DataMember]
        public string Bank_Code { get; set; }

        [DataMember]
        public string Bank_Account_Name { get; set; }

        [DataMember]
        public string Bank_Account_Number { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public double Credit { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int StatusValue { get; set; }

        [DataMember]
        public string IP_Address { get; set; }

        [DataMember]
        public long SearchIndex { get; set; }

        [DataMember]
        public int TimeIndex { get; set; }

        [DataMember]
        public string Buildtime { get; set; }

        public VnpayPayoutLog()
        {
            ID = 0;
            UserUID = 0;
            UserID = "";
            TransactionID = "";
            ClientTransactionID = "";
            Bank_Code = "";
            Bank_Account_Name = "";
            Bank_Account_Number = "";
            Amount = 0.00d;
            Credit = 0.00d;
            Status = "";
            StatusValue = 0;
            IP_Address = "";
            SearchIndex = 0;
            TimeIndex = 0;
            Buildtime = "";
        }
    }

    [Serializable, DataContract]
    public class VnpayTransactionPre
    {
        [DataMember]
        public int userUID { get; set; }

        [DataMember]
        public string startDate { get; set; }

        [DataMember]
        public string endDate { get; set; }

        public VnpayTransactionPre()
        {
            userUID = 0;
            startDate = "";
            endDate = "";
        }
    }

    [Serializable]
    public class VnpayBankList
    {
        public VnpayBankInfo[] mBankList { get; set; }
        public string info { get; set; }
        public List<string> mChannelList { get; set; }

        public VnpayBankList()
        {
            info = "";
            mChannelList = new List<string>();
        }
    }

    [Serializable, DataContract]
    public class VnpayBankInfo
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

        public VnpayBankInfo()
        {
            STT = "";
            English_name = "";
            Vietnamese_name = "";
            Bank_code = "";
            BIC_code = "";
        }
    }

    [Serializable]
    public class ClientVnpayBankList
    {
        public ClientVnpayBankInfo[] mBankList;
    }

    [Serializable, DataContract]
    public class ClientVnpayBankInfo
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

        public ClientVnpayBankInfo()
        {
            STT = "";
            English_name = "";
            Vietnamese_name = "";
            Bank_code = "";
            BIC_code = "";
        }
    }

    [Serializable]
    public class VnpayTransactionList
    {
        public VnpayTransactionInfo[] mTransactionList { get; set; }
    }

    [Serializable, DataContract]
    public class VnpayTransactionInfo
    {
        [DataMember]
        public string TransactionID { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string Status { get; set; }

        public VnpayTransactionInfo()
        {
            TransactionID = "";
            Type = "";
            Amount = "";
            Status = "";
        }
    }

    [Serializable]
    public class ClientVnpayTransactionList
    {
        public ClientVnpayTransactionInfo[] mTransactionList;
    }

    [Serializable, DataContract]
    public class ClientVnpayTransactionInfo
    {
        [DataMember]
        public string TransactionID;

        [DataMember]
        public string Type;

        [DataMember]
        public string Amount;

        [DataMember]
        public string Status;

        public ClientVnpayTransactionInfo()
        {
            TransactionID = "";
            Type = "";
            Amount = "";
            Status = "";
        }
    }

    [Serializable, DataContract]
    public class UserRegisteredData
    {
        [DataMember]
        public string MsgType = "UserRegisteredData";

        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public OperationCode operationCode;
        [DataMember]
        public string Status;
        [DataMember]
        public string UserAccount;
        [DataMember]
        public string Password;
        [DataMember]
        public string CPassword;
        [DataMember]
        public string ManagerAccount;
        [DataMember]
        public string RemoteEndPoint;
        [DataMember]
        public string PhoneNumber;
        [DataMember]
        public string IntroducerAccount;

        public UserRegisteredData()
        {
            MsgType = "UserRegisteredData";
        }
    }

    [Serializable, DataContract]
    public class PhoneNumberData
    {
        [DataMember]
        public string MsgType = "PhoneNumberData";

        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public string Type;
        [DataMember]
        public string Status;
        [DataMember]
        public int UserUID;
        [DataMember]
        public string PhoneNumber;

        public PhoneNumberData()
        {
            MsgType = "PhoneNumberData";
        }
    }

    [Serializable, DataContract]
    public class CommonCommandData
    {
        [DataMember]
        public string MsgType = "CommonCommandData";

        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public string CommandType;
        [DataMember]
        public string CommandContext;

        public CommonCommandData()
        {
            MsgType = "CommonCommandData";
        }
    }

    /// <summary>Client通用訊息(激爆骰子新增)</summary>
    [Serializable, DataContract]
    public class CommonMessageData
    {
        [DataMember]
        public string MsgType = "CommonMessageData";

        /// <summary>傳送訊息的類型</summary>
        [DataMember]
        public string CommandType;
        /// <summary>傳送訊息的內容</summary>
        [DataMember]
        public string CommandContext;
        /// <summary>傳送訊息副指令</summary>
        [DataMember]
        public int Instruction;
        /// <summary>傳送訊息的遊戲伺服器</summary>
        [DataMember]
        public GameServerCode GameServerCode;
        /// <summary>要接收訊息的玩家</summary>
        [DataMember]
        public int UserUID;
        /// <summary>傳送訊息的內容</summary>
        [DataMember]
        public Dictionary<string, string> DataContext;
        /// <summary>要接收訊息的玩家List</summary>
        [DataMember]
        public List<int> UserList;

        /// <summary>額外公用資訊</summary>
        //[DataMember]
        //public ExtraData extraData;

        public CommonMessageData(string commandType)
        {
            MsgType = "CommonMessageData";
            CommandType = commandType; //CommandType為必要資訊
        }
    }

    /// <summary>GameServer狀態通知(激爆骰子新增)</summary>
    [Serializable, DataContract]
    public class GameServerStatusData
    {
        [DataMember]
        public string MsgType = "GameServerStatusData";

        /// <summary>傳送訊息的類型</summary>
        [DataMember]
        public string CommandType;
        /// <summary>傳送訊息的內容</summary>
        [DataMember]
        public string CommandContext;
        /// <summary>傳送訊息副指令</summary>
        [DataMember]
        public int Instruction;
        /// <summary>傳送訊息的遊戲伺服器</summary>
        [DataMember]
        public GameServerCode GameServerCode;

        public GameServerStatusData()
        {
            MsgType = "GameServerStatusData";
        }
    }

    [Serializable]
    public class GameData
    {
        public enum ClassType
        {
            userData,
            serverData,
            mRoomInfo,
            roomData,
            gameTimeInfo,
            roomActionInfo,
            roomActorQuitData,
            gameInfo,
            betInfo,
            versionData,
            historyData,
            webInfo,
            jpInfo,
            jpAccount,
            usergameAccount,
            usergameAccountList,
            pushJP,
            DBinfo,
            BotData,
            userTranssaction,
            machineGameData,
            RankStru,
            dayMission,
            LottoTicket,
            GetReward,
            GetSignIn,
            SystemCommand,
            ExtraData,
            GunActionInfo
        }

        public string MsgType = "GameData";

        /// <summary>傳送訊信的類型</summary>
        public OperationCode operationCode;
        /// <summary>回傳訊息是否成功的類型</summary>
        public ErrorCode errorCode;
        /// <summary>回傳遊戲伺服器類型</summary>
        public GameServerCode gameServerCode;
        /// <summary>遊戲運行狀態</summary>
        public GameStateCode GameState;
        /// <summary>玩家更新項目</summary>
        public ActorUpdataCode actorUpdataCode;
        /// <summary>獎勵類型</summary>
        public RewardType rewardType;
        /// <summary>回傳偵錯訊息</summary>
        public string DebugMessage;
        /// <summary>伺服器資料</summary>
        public ServerData serverData;
        /// <summary>使用者資料</summary>
        public UserData userData;
        /// <summary>開獎遊戲內容</summary>
        public GameInfo gameInfo;
        /// <summary>多個房間資料</summary>
        public List<RoomInfo> mRoomInfo;
        /// <summary>房間資料</summary>
        public RoomData roomData;
        /// <summary>房間中時間資訊</summary>
        public GameTimeInfo gameTimeInfo;
        /// <summary>房間中玩家的動作資訊</summary>
        public RoomActionInfo roomActionInfo;
        /// <summary>房間中玩家離開資訊</summary>
        public RoomActorQuit roomActorQuitData;
        /// <summary>歷史紀錄資訊</summary>
        public List<HistoryInfo> historyData;
        /// <summary>版本資訊</summary>
        public VersionInfo versionData;
        /// <summary>網頁資訊</summary>
        public WebInfo webInfo;
        /// <summary>JP資訊</summary>
        public JPInfo jpInfo;
        /// <summary>DB資訊</summary>
        public DBCacheData dbcacheData;
        /// <summary>押注資訊</summary>
        public BetInfo betInfo;
        /// <summary>JP帳目資訊</summary>
        public List<JPAccountData> jpgameaccountdata;
        /// <summary>JP帳目資訊</summary>
        public JPAccountData jpgameaccount;
        /// <summary>玩家帳目資訊</summary>
        public UserAccountData useraccountdata;
        /// <summary>玩家遊玩帳目資訊</summary>
        public List<UserGameAccountData> usergameaccountdata;
        /// <summary>JP機率出牌資訊</summary>
        public PushJPData pushJPData;
        /// <summary>機器人資訊</summary>
        public List<BotData> botdatalist;
        /// <summary>玩家交易資訊</summary>
        public List<UserTransaction> usertransaction;
        /// <summary>機台遊戲資訊</summary>
        public MachineGameData machineGameData;
        /// <summary>玩家Rank資訊</summary>
        public List<UserRankStru> userRankStru;
        /// <summary>機台Rank資訊</summary>
        public List<MachineRankStru> machineRankStru;
        /// <summary>玩家每日任務資訊</summary>
        public DayMissionInfo dayMissionInfo;
        /// <summary>彩票期號</summary>
        public string ISSNID;
        /// <summary>玩家彩票資訊</summary>
        public UserLottoTicketData userLottoTicketData;
        /// <summary>設定玩家彩票號碼</summary>
        public SetLottoTicketNumber setLottoTicketNumber;
        /// <summary>獎勵UID</summary>
        public int RewardUID;
        /// <summary>領獎結果</summary>
        public string RewardResult;
        /// <summary>簽到表</summary>
        public string SignInData;
        /// <summary>系統命令</summary>
        public SystemCommandData systemCommandData;
        /// <summary>額外公用資訊</summary>
        public ExtraData extraData;
        ///// <summary>紅包金額</summary>
        //public double GameRedEnvelopeData;

        ///// <summary>批次修改玩家狀態</summary>
        public List<UserStatus> userStatuses;
        
        /// <summary>多個房間資料</summary>
        public FishingGameInit mFishingGameInit;

        /// <summary>魚群腳本</summary>
        public List<FishPathInfo> mAllGameFishPathInfo;

        /// <summary>魚群指令</summary>
        public FishCommand mFishCommand;

        public Dictionary<int, int> LockGameList;

        /// <summary>網吧炒場</summary>
        public HypeBigWater mHypeBigWater;

        public GameData()
        {
            MsgType = "GameData";
        }

        public GameData(List<ClassType> type)
        {
            MsgType = "GameData";

            for (int i = 0; i < type.Count; i++)
            {
                switch(type[i])
                {
                    case ClassType.serverData:
                        serverData = new ServerData();
                        break;
                    case ClassType.userData:
                        userData = new UserData();
                        break;
                    case ClassType.versionData:
                        versionData = new VersionInfo();
                        break;
                    case ClassType.jpInfo:
                        jpInfo = new JPInfo();
                        break;
                    case ClassType.mRoomInfo:
                        mRoomInfo = new List<RoomInfo>();
                        break;
                    case ClassType.roomData:
                        roomData = new RoomData();
                        break;
                    case ClassType.gameTimeInfo:
                        gameTimeInfo = new GameTimeInfo();
                        break;
                    case ClassType.roomActionInfo:
                        roomActionInfo = new RoomActionInfo();
                        break;
                    case ClassType.roomActorQuitData:
                        roomActorQuitData = new RoomActorQuit();
                        break;
                    case ClassType.gameInfo:
                        gameInfo = new GameInfo();
                        break;
                    case ClassType.historyData:
                        historyData = new List<HistoryInfo>();
                        break;
                    case ClassType.webInfo:
                        webInfo = new WebInfo();
                        break;
                    case ClassType.DBinfo:
                        dbcacheData = new DBCacheData();
                        break;
                    case ClassType.betInfo:
                        betInfo = new BetInfo();
                        break;
                    case ClassType.jpAccount:
                        jpgameaccountdata = new List<JPAccountData>();
                        break;
                    case ClassType.usergameAccount:
                        useraccountdata = new UserAccountData();
                        break;
                    case ClassType.usergameAccountList:
                        usergameaccountdata = new List<UserGameAccountData>();
                        break;
                    case ClassType.pushJP:
                        pushJPData = new PushJPData();
                        jpgameaccount = new JPAccountData();
                        break;
                    case ClassType.BotData:
                        botdatalist = new List<BotData>();
                        break;
                    case ClassType.userTranssaction:
                        usertransaction = new List<UserTransaction>();
                        break;
                    case ClassType.machineGameData:
                        machineGameData = new MachineGameData();
                        break;
                    case ClassType.RankStru:
                        userRankStru = new List<UserRankStru>();
                        machineRankStru = new List<MachineRankStru>();
                        break;
                    case ClassType.dayMission:
                        dayMissionInfo = new DayMissionInfo();
                        break;
                    case ClassType.LottoTicket:
                        userLottoTicketData = new UserLottoTicketData();
                        break;
                    case ClassType.SystemCommand:
                        systemCommandData = new SystemCommandData();
                        break;
                    case ClassType.ExtraData:
                        extraData = new ExtraData();
                        break;
                }
            }
        }
    }

    [Serializable]
    public class OperationCodeData
    {
        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public OperationCode operationCode;

        public OperationCodeData()
        {
            operationCode = OperationCode.Null;
        }
    }

    [Serializable,DataContract]
    public class VerificationData
    {
        [DataMember]
        public string MsgType = "VerificationData";

        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public OperationCode operationCode;
        /// <summary>回傳訊息是否成功的類型</summary>
        [DataMember]
        public ErrorCode errorCode;
        /// <summary>回傳遊戲伺服器類型</summary>
        [DataMember]
        public GameServerCode gameServerCode;

        [DataMember]
        public int memberUniquelID;

        [DataMember]
        public int entityUID;

        [DataMember]
        public double SessionID;

        [DataMember]
        public string RemoteEndPoint;

        [DataMember]
        public string IP;

        [DataMember]
        public string memberID;

        [DataMember]
        public string memberPW;

        [DataMember]
        public string NewPW;

        [DataMember]
        public string Nickname;

        [DataMember]
        public int Sex;

        [DataMember]
        public string Email;

        [DataMember]
        public double Balnce;

        /// <summary>建立本地餘額狀態時的固定BDV2 Outbox基準</summary>
        [DataMember]
        public long BaselineFenceId;

        [DataMember]
        public int ratio;

        [DataMember]
        public double WinCredit;

        [DataMember]
        public short Usersituation;

        [DataMember]
        public int Energy;

        [DataMember]
        public int Star;

        [DataMember]
        public int LogUid;

        [DataMember]
        public double GameRedEnvelope;

        [DataMember]
        public double Profit;

        [DataMember]
        public double Pool;

        [DataMember]
        public string ExtInfo;

        [DataMember]
        public DBCacheData DbcacheData;

        public VerificationData()
        {
            MsgType = "VerificationData";

            memberUniquelID = 0;

            entityUID = 0;

            SessionID = 0;

            RemoteEndPoint = "";

            IP = "";

            memberID = "";

            memberPW = "";

            NewPW = "";

            Nickname = "";

            Sex  = 1;

            Email = "";

            Balnce = 0;

            ratio = 0;

            WinCredit = 0;

            Usersituation = 0;

            Energy = 0;

            Star = 0;

            LogUid = 0;

            GameRedEnvelope = 0;

            Profit = 0;

            Pool = 0;

            ExtInfo  = "";
        }
    }

    [Serializable,DataContract]
    public class HeartBeatData
    {
        [DataMember]
        public string MsgType = "HeartBeatData";

        [DataMember]
        /// <summary>傳送訊信的類型</summary>
        public OperationCode operationCode;

        [DataMember]
        public int Port;

        public HeartBeatData()
        {
            MsgType = "HeartBeatData";

            operationCode = OperationCode.Null;
            Port = 0;
        }
    }

    [Serializable,DataContract]
    public class UserData
    {
        [DataMember]
        public int memberUniquelID;
        [DataMember]
        public int entityUID;
        [DataMember]
        public double SessionID;
        [DataMember]
        public string RemoteEndPoint;
        [DataMember]
        public string IP;
        [DataMember]
        public string memberID;
        [DataMember]
        public string memberPW;
        [DataMember]
        public string NewPW;
        [DataMember]
        public string Nickname;
        [DataMember]
        public int Sex;
        [DataMember]
        public string Email;
        [DataMember]
        public double Balnce;
        /// <summary>建立本地餘額狀態時的固定BDV2 Outbox基準</summary>
        [DataMember]
        public long BaselineFenceId;
        [DataMember]
        public int ratio;
        [DataMember]
        public double WinCredit;
        [DataMember]
        public short Usersituation;
        [DataMember]
        public int Energy;
        [DataMember]
        public int Star;
        [DataMember]
        public int LogUid;
        [DataMember]
        public double GameRedEnvelope;
        [DataMember]
        public double Profit;
        [DataMember]
        public double Pool;
        [DataMember]
        public string ExtInfo;
        [DataMember]
        public Dictionary<string, SystemTextColor> SysTemText;
    }

    [Serializable,DataContract]
    public class LockGameData
    {
        [DataMember]
        public string MsgType = "LockGameData";

        /// <summary>傳送訊信的類型</summary>
        [DataMember]
        public OperationCode operationCode;

        [DataMember]
        public Dictionary<int,int> LockGameList;

        public LockGameData()
        {
            MsgType = "LockGameData";

            operationCode = OperationCode.Null;
            LockGameList = new Dictionary<int, int>();
        }
    }


    /// <summary>系統命令</summary>
    public enum SystemCommand
    {
        /// <summary>系統維護公告廣播</summary>
        ServiceNotification,
        /// <summary>變更公告廣播</summary>
        ChangeNotification,
        /// <summary>返水</summary>
        RebateInfo, //#250807
    }

    [Serializable, DataContract]
    public class SystemCommandData
    {
        [DataMember]
        public SystemCommand Command;
        [DataMember]
        public Dictionary<string, string> Instruction;
        [DataMember]
        public List<int> DataList;
        [DataMember]
        public List<string> Message;
    }

    [Serializable, DataContract]
    public class ExtraData
    {
        [DataMember]
        public List<int> Data1;
        [DataMember]
        public List<string> Data2;
        [DataMember]
        public Dictionary<int, int> Data3;
        [DataMember]
        public Dictionary<string, string> Data4;
        [DataMember]
        public List<double> Data5;
    }

    [Serializable,DataContract]
    public class BotData
    {
        [DataMember]
        public int memberUniquelID;
        [DataMember]
        public int MachineUID;
        [DataMember]
        public short SeatIndex;
        [DataMember]
        public string Nickname;
        [DataMember]
        public bool IsBotBill;
        [DataMember]
        public double Balnce;
        [DataMember]
        public int Sex;
    }

    [Serializable,DataContract]
    public class GameTimeInfo
    {
        [DataMember]
        public int Bettime;
        [DataMember]
        public bool isWait;
    }

    [Serializable,DataContract]
    public class ServerData
    {
        [DataMember]
        public string IP;
        [DataMember]
        public int Port;
        [DataMember]
        public int EmptSeatNum;
    }

    [Serializable,DataContract]
    public class RoomData
    {
        [DataMember]
        public int UniquelID;
        [DataMember]
        public int MachineUID;
        [DataMember]
        public short roomIndex;
        [DataMember]
        public short SeatIndex;
    }

    [Serializable, DataContract]
    public class RoomInfo
    {
        [DataMember]
        public int RoomIndex;
        [DataMember]
        public string RoomName;
        [DataMember]
        public int Limit;
        [DataMember]
        public int BetTime;
        [DataMember]
        public int Keeptime;
        [DataMember]
        public double ConversionRaito;
        [DataMember]
        public double MinRateLimit;
        [DataMember]
        public double MaxRateLimit;
        [DataMember]
        public int LineMinLimit;
        [DataMember]
        public int LineMaxLimit;
        [DataMember]
        public double MinStartRateLimit;
        [DataMember]
        public string BetUnit;
        [DataMember]
        public string LevelUnit;
        [DataMember]
        public int RoomUserCount;
        [DataMember]
        public string[] SeatArray;
        [DataMember]
        public int BackGroundIndex;
        [DataMember]
        public bool IsFastGame;
        [DataMember]
        public List<int> ExtData1;
        [DataMember]
        public List<double> ExtData2;
        [DataMember]
        public double IndepMinRateLimit; //獨立買 最小押分限制
        [DataMember]
        public double IndepMaxRateLimit; //獨立買 最大押分限制
        [DataMember]
        public int IndepGameBetMulti;  //獨立買 押分倍數
        [DataMember]
        public int DefaultLevel;  //Level內定植
        [DataMember]
        public double DefaultBet; //押分內定植
        [DataMember]
        public string ExtInfo;
    }

    [Serializable,DataContract]
    public class RoomPlayerInfo
    {
        /// <summary>座位代號</summary>
        [DataMember]
        public short Seatindex;
        /// <summary>使用者唯一碼</summary>
        [DataMember]
        public int memberUniquelID;
        /// <summary>使用者帳號</summary>
        [DataMember]
        public int UserAccount;
        /// <summary>暱稱</summary>
        [DataMember]
        public string Nickname;
        /// <summary>性別</summary>
        [DataMember]
        public short Sex;
        /// <summary>押分資訊</summary>
        //[DataMember]
        //public double[] BetInfo;
        ///// <summary>贏分資訊</summary>
        //[DataMember]
        //public double[] WinInfo;
        /// <summary>額外資訊訊息</summary>
        [DataMember]
        public string Message;
    }

    [Serializable,DataContract]
    public class RoomActionInfo
    {
        [DataMember]
        public int memberUniquelID;
        [DataMember]
        public string Nickname;
        [DataMember]
        public short Sex;
        [DataMember]
        public short Roomindx;
        [DataMember]
        public short Seatindex;
        [DataMember]
        public bool IsGameEnd;
        [DataMember]
        public List<RoomPlayerInfo> RoomPlayerInfos;
    }

    [Serializable,DataContract]
    public class RoomActorQuit
    {
        [DataMember]
        public int memberUniquelID;
    }

    [Serializable, DataContract]
    public class GameInfo
    {
        #region 押分機
        [DataMember]
        public List<int> MagnificationForm;
        [DataMember]
        public int Prize;
        [DataMember]
        public Dictionary<int, int> MainLamp;
        [DataMember]
        public Dictionary<int, int> SubLamp;
        [DataMember]
        public int BankerPlayer;
        [DataMember]
        public bool OpenPrizeType;
        [DataMember]
        public int LeftRollActor;
        [DataMember]
        public int RightRollActor;
        [DataMember]
        public List<int> EnemyFightMove;
        [DataMember]
        public List<int> PlayerFightMove;
        [DataMember]
        public double Bonus;
        #endregion

        #region 老虎機
        [DataMember]
        public List<int> SlotIcon { get; set; }  ////Highway, 金雞 15輪資訊
        [DataMember]
        public List<int> WildMultiplier { get; set; }  //Highway 9 線附加倍率
        [DataMember]
        public int FreeGameAction;
        [DataMember]
        public int FreeGameTotalRound;
        [DataMember]
        public int FreeGameRound;
        [DataMember]
        public int FreeGameMultiplier;
        [DataMember]
        public double FreeGameTotalWin;
        [DataMember]
        public double FreeGameFeatureWin;
        [DataMember]
        public int FreeGameType;
        [DataMember]
        public int FreeGameMode;
        [DataMember]
        public List<int> LeftWild_X { get; set; }
        [DataMember]
        public List<int> RightWild_X { get; set; }
        [DataMember]
        public int JpGameType;  //金雞JP Game 中獎種類
        [DataMember]
        public double JpGamePoint; //金雞JP Game 中獎分數
        [DataMember]
        public double JpGrand; //金雞JP Grand 分數
        [DataMember]
        public double JpMajor; //金雞JP Major 分數
        [DataMember]
        public double JpMinor; //金雞JP Minor 分數
        [DataMember]
        public double JpMini; //金雞JP Mini 分數
        [DataMember]
        public double JpMultiplier; //金雞JP倍數
        [DataMember]
        public List<double> ExtData2;
        [DataMember]
        public List<string> ExtData3;
        #endregion

        [DataMember]
        public JpAward JPType;
        [DataMember]
        public double JPPoint;
        [DataMember]
        public bool AllGetJP;
        [DataMember]
        public int JPCommandType;
        [DataMember]
        public int GameMode;  //獨立買遊戲旗號
        [DataMember]
        public int IndepGameRestRound;  //獨立買遊戲剩餘局數
        [DataMember]
        public int IndepGameThisRound;  //獨立買遊戲已玩局數
    }

    [Serializable,DataContract]
    public class HistoryInfo
    {
        [DataMember]
        public int HistoryUID;
        [DataMember]
        public int BankPlayer;
        [DataMember]
        public List<int> MainLamp;
        [DataMember]
        public int PrizType;
        [DataMember]
        public List<int> SubLamp;
        [DataMember]
        public double Bonus1;
        [DataMember]
        public double Bonus2;
        [DataMember]
        public double Bonus3;
        [DataMember]
        public int JP1;
        [DataMember]
        public int JP2;
        [DataMember]
        public int JP3;
        [DataMember]
        public Dictionary<string, string> ExtInfo;
    }

    [Serializable, DataContract]
    public class UserRankStru
    {
        [DataMember]
        public string GameName;
        [DataMember]
        public int RankType;
        [DataMember]
        public string RankData;
        [DataMember]
        public string BuildTime;
    }

    [Serializable, DataContract]
    public class MachineRankStru
    {
        [DataMember]
        public int RankType;
        [DataMember]
        public int RankCount;
        [DataMember]
        public string RankData;
        [DataMember]
        public string RankList;
    }

    [Serializable, DataContract]
    public class PlayerDayMissionInfo
    {
        /// <summary>唯一碼</summary>
        [DataMember]
        public int UID { get; set; }
        /// <summary>任務號碼</summary>
        [DataMember]
        public int MissionID { get; set; }
        /// <summary>狀態 0未完成 1已完成 2已兌獎</summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>任務編碼</summary>
        [DataMember]
        public string TaskDetail { get; set; }
        /// <summary>獎勵編碼</summary>
        [DataMember]
        public string AwardDetail { get; set; }
        /// <summary>時間戳記</summary>
        [DataMember]
        public string BuildTime { get; set; }
    }

    [Serializable, DataContract]
    public class DayMissionInfo
    {
        [DataMember]
        public int WeekMissionStatus;
        [DataMember]
        public int WeekMissionCount;
        [DataMember]
        public string WeekMissionAward;
        [DataMember]
        public List<PlayerDayMissionInfo> playerDayMissionInfo;
    }

    [Serializable, DataContract]
    public class LottoTicketData
    {
        /// <summary>唯一碼</summary>
        [DataMember]
        public int TicketUID { get; set; }
        /// <summary>期號</summary>
        [DataMember]
        public string ISSNID { get; set; }
        /// <summary>玩家UID</summary>
        [DataMember]
        public int UserUID { get; set; }
        /// <summary>彩卷狀態</summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>彩卷號碼</summary>
        [DataMember]
        public string TicketNumber { get; set; }
        /// <summary>獎項類型</summary>
        [DataMember]
        public string PrizeType { get; set; }
        /// <summary>時間戳記</summary>
        [DataMember]
        public string TimeStamp { get; set; }
    }

    [Serializable, DataContract]
    public class PhaseLottoTicketData
    {
        /// <summary>頭獎</summary>
        [DataMember]
        public string FirstPrize { get; set; }
        /// <summary>二獎</summary>
        [DataMember]
        public string SecondPrize { get; set; }
        /// <summary>三獎</summary>
        [DataMember]
        public string ThirdPrize { get; set; }
        /// <summary>特獎</summary>
        [DataMember]
        public string SpecialPrize { get; set; }
        /// <summary>封牌時間</summary>
        [DataMember]
        public string ClosureTime { get; set; }
        /// <summary>封牌時間</summary>
        [DataMember]
        public string CurrentTime { get; set; }
        /// <summary>玩家該期彩票</summary>
        [DataMember]
        public List<LottoTicketData> lottoTicketData;
    }

    [Serializable, DataContract]
    public class UserLottoTicketData
    {
        [DataMember]
        public string Prev_ISSNID;
        [DataMember]
        public string Next_ISSNID;
        /// <summary>玩家所有彩票</summary>
        [DataMember]
        public Dictionary<string, PhaseLottoTicketData> userLottoTicketData;
    }

    [Serializable, DataContract]
    public class SetLottoTicketNumber
    {
        /// <summary>唯一碼</summary>
        [DataMember]
        public int TicketUID { get; set; }
        /// <summary>彩票號</summary>
        [DataMember]
        public string TicketNumber { get; set; }
        /// <summary>設定結果</summary>
        [DataMember]
        public string Result { get; set; }
    }

    [Serializable, DataContract]
    public class MachineGameData
    {
        [DataMember]
        public int AllRunCount;
        [DataMember]
        public int PrizeCount;
    }

    [Serializable,DataContract]
    public class VersionInfo
    {
        [DataMember]
        public VersionCode type;
        [DataMember]
        public Dictionary<int, string> SameTypeGameVersion;
        [DataMember]
        public int SameTypeGameCount;
    }

    [Serializable,DataContract]
    public class BalanceDeal
    {
        [DataMember]
        public string UserUID;           // 玩家
        [DataMember]
        public string OperatorId;        // 管理員
        [DataMember]
        public int OperatorEntityId;     // 代理商
        [DataMember]
        public int EntityId;             // 代理商
        [DataMember]
        public double Amount;            // 數量
        [DataMember]
        public string IP_Address;        // IP位置
    }

    /// <summary>批次開分 V2 的請求主體類型。</summary>
    public enum BatchDepositV2ActorType
    {
        None = 0,
        /// <summary>ActorId 是實際登入後台操作的 Manager ID。</summary>
        WebUser = 1,
        /// <summary>ActorId 是發起請求的代理商／商戶識別 ID。</summary>
        ApiClient = 2
    }

    /// <summary>批次開分 V2 的處理狀態。</summary>
    public enum BatchDepositV2Status
    {
        None = 0,
        Pending = 1,
        Processing = 2,
        Succeeded = 3,
        Failed = 4
    }

    /// <summary>批次開洗分 V2 的金額處理模式。</summary>
    public enum BatchDepositV2OperationMode
    {
        Deposit = 0,
        WithdrawAll = 1,
        Withdraw = 2
    }

    /// <summary>批次開分 V2 的請求明細。</summary>
    [Serializable, DataContract]
    public class BatchDepositV2RequestDetail
    {
        [DataMember]
        public int UserUID;
        [DataMember]
        public decimal RequestAmount;
        [DataMember]
        public BatchDepositV2OperationMode OperationMode;
    }

    /// <summary>批次開分 V2 的請求資料。</summary>
    [Serializable, DataContract]
    public class BatchDepositV2Request
    {
        [DataMember]
        public string BatchId;
        [DataMember]
        public BatchDepositV2ActorType ActorType;
        [DataMember]
        public string ActorId;
        [DataMember]
        public string IdempotencyKey;
        [DataMember]
        public string PayerManagerId;
        // 以下兩個欄位僅保留舊封包相容性。DBCache 必須從 Actor 與玩家權威資料解析實際值。
        [DataMember]
        public int OperatorEntityId;
        [DataMember]
        public int EntityId;
        [DataMember]
        public string RequestIp;
        [DataMember]
        public List<BatchDepositV2RequestDetail> Details;
    }

    /// <summary>以 actor 與冪等鍵查詢批次開分 V2 結果。</summary>
    [Serializable, DataContract]
    public class BatchDepositV2QueryRequest
    {
        [DataMember]
        public BatchDepositV2ActorType ActorType;
        [DataMember]
        public string ActorId;
        [DataMember]
        public string IdempotencyKey;
    }

    /// <summary>批次開分 V2 的結果明細。</summary>
    [Serializable, DataContract]
    public class BatchDepositV2ResultDetail
    {
        [DataMember]
        public int DetailSequence;
        [DataMember]
        public int UserUID;
        [DataMember]
        public decimal RequestAmount;
        [DataMember]
        public BatchDepositV2OperationMode OperationMode;
        [DataMember]
        public decimal ExtraBonus;
        [DataMember]
        public decimal BeforeBalance;
        [DataMember]
        public decimal AfterBalance;
        [DataMember]
        public double SessionId;
        [DataMember]
        public int TradeRecordId;
        [DataMember]
        public BatchDepositV2Status Status;
        [DataMember]
        public string FailureCode;
    }

    /// <summary>批次開分 V2 的處理結果。</summary>
    [Serializable, DataContract]
    public class BatchDepositV2Result
    {
        [DataMember]
        public string BatchId;
        [DataMember]
        public string IdempotencyKey;
        [DataMember]
        public string RequestHash;
        [DataMember]
        public BatchDepositV2Status Status;
        [DataMember]
        public string FailureCode;
        [DataMember]
        public List<BatchDepositV2ResultDetail> Details;
    }

    /// <summary>批次開分 V2 結果查詢回應。</summary>
    [Serializable, DataContract]
    public class BatchDepositV2QueryResult
    {
        [DataMember]
        public bool Found;
        [DataMember]
        public string FailureCode;
        [DataMember]
        public BatchDepositV2Result Result;
    }

    [Serializable, DataContract]
    public class DBCacheData
    {
        [DataMember]
        public string dayDate;
        [DataMember]
        public List<Dictionary<string, string>> userGameData;
        [DataMember]
        public Dictionary<string, string> UpdateXiyouBigWaterPool;
        [DataMember]
        public Dictionary<string, string> CheckXiyouBigWaterProFitDays;
        [DataMember]
        public Dictionary<string, string> UpdataProbabilityData;
        [DataMember]
        public Dictionary<string, string> UpdataHistoryData;
        [DataMember]
        public Dictionary<string, string> UpdateXiyouAccountDetailData;
        [DataMember]
        public Dictionary<string, string> GlobalJPSettingData;
        [DataMember]
        public string LobbyMode;
        [DataMember]
        public List<Dictionary<string, string>> MachineData;
        [DataMember]
        public List<Dictionary<string, string>> SettingData;
        [DataMember]
        public List<Dictionary<string, string>> GameGlobalSettingData;
        [DataMember]
        public List<Dictionary<string, string>> HistoryData;
        [DataMember]
        public List<Dictionary<string, string>> ProbabilityData;
        [DataMember]
        public List<Dictionary<string, string>> XiyouAccountDetailData;
        [DataMember]
        public Dictionary<string, string> JPBonusAccountData;
        [DataMember]
        public Dictionary<string, string> GameAwardAccountData;
        [DataMember]
        public List<Dictionary<string, string>> BotRuleData;
        [DataMember]
        public List<Dictionary<string, string>> BotRestData;
        [DataMember]
        public Dictionary<string, string> BotModeData;
        [DataMember]
        public Dictionary<string, string> MachineRankData;
        [DataMember]
        public List<Dictionary<string, string>> FakeBotRankGameData;
        [DataMember]
        public string LogData;
    }

    [Serializable, DataContract]
    public class PushJPData
    {
        [DataMember]
        public JpAward JPType;
        [DataMember]
        public bool SurplusRegulate;    // 盈餘校正
        [DataMember]
        public bool IsAccount;         //入內帳
        [DataMember]
        public int CommandType;    //出牌命令來源
        [DataMember]
        public int MachineUid;    //指定機台
        [DataMember]
        public int PlayerUid;    //指定玩家
    }

    [Serializable, DataContract]
    public class JPAccountData
    {
        [DataMember]
        public JpAward JPType;
        [DataMember]
        public string GameNmae;
        [DataMember]
        public double TotalWin;
        [DataMember]
        public string User;
        [DataMember]
        public string DateTime;
    }

    [Serializable, DataContract]
    public class UserAccountData
    {
        [DataMember]
        public GameServerCode GameType;
        [DataMember]
        public string StartTime;
        [DataMember]
        public string EndTime;
        [DataMember]
        public int CurrentPage;
        [DataMember]
        public int TotalPage;
        [DataMember]
        public int DataUnitNum;
    }

    [Serializable, DataContract]
    public class UserGameAccountData
    {
        [DataMember]
        public string SerialNo;
        [DataMember]
        public string LogTime;
        [DataMember]
        public double startBalance;
        [DataMember]
        public double TotalBet;
        [DataMember]
        public double TotalWin;
        [DataMember]
        public double EndBalance;
        [DataMember]
        public string Result;
        [DataMember]
        public string PlayInfo;
        [DataMember]
        public string JPType;
        [DataMember]
        public string JPWin;
    }

    [Serializable, DataContract]
    public class SearchUserGameAccountInfo
    {
        [DataMember]
        public UserAccountData mUserAccountData;

        [DataMember] 
        public List<UserGameAccountData> mUserGameAccountDataList;
    }

    [Serializable,DataContract]
    public class UserTransaction
    {
        [DataMember]
        public string TransactionNo;
        [DataMember]
        public string TransactionTime;
        [DataMember]
        public string TransactionType;
        [DataMember]
        public string PaymentType;
        [DataMember]
        public double TransactionAmount;
        [DataMember]
        public string Status;
    }

    [Serializable,DataContract]
    public class BetInfo
    {
        [DataMember]
        public double[] Info;
        [DataMember]
        public int LevelInfo;  //押分Level
        [DataMember]
        public double PlayInfo;  //押分單位
        [DataMember]
        public Dictionary<int, bool> AskRedEnvelope; //要求遊戲紅包 <玩家代號, 要求旗號>
        [DataMember]
        public List<int> IllegalUser;
        [DataMember]
        public List<int> illegalMinStartRate;
        [DataMember]
        public Dictionary<int, Equipment> UserEquipment;
        [DataMember]
        public Dictionary<int, RedEnvelope> UserRedEnvelope;
        [DataMember]
        public Dictionary<int, double[]> AllRoomActorBetinfo;
        [DataMember]
        public Dictionary<int, double> AllRoomActorCreditInfo; //全部玩家本局Credit資訊
        [DataMember]
        public int PlayMode;  //黃金樹FreeGame模式
        [DataMember]
        public int GameMode;  //獨立買遊戲旗號
        [DataMember]
        public double IndepPlayInfo;  //獨立買總押分
        [DataMember]
        public string ExtInfo;

        public BetInfo()
        {
            GameMode = 0;
            ExtInfo = null;
        }
    }

    [Serializable, DataContract]
    public class Equipment
    {
        [DataMember]
        public int Energy;  //能量條
        [DataMember]
        public int Star;    //星星總數
        [DataMember]
        public int Instruction;    //額外資訊
        [DataMember]
        public string CommandType; //額外資訊總類
        [DataMember]
        public Dictionary<string, string> ExtInfo; //額外資訊
    }

    [Serializable, DataContract]
    public class RedEnvelope
    {
        [DataMember]
        public int LogUid;
        [DataMember]
        public int MachineUid;
        [DataMember]
        public int UserUid;
        [DataMember]
        public double EnvelopeValue;
    }

    //[Serializable, DataContract]
    //public class GameRedEnvelopeInfo
    //{
    //    //[DataMember]
    //    //public int Serial;    //紅包序號
    //    //[DataMember]
    //    //public int UserUid;
    //    //[DataMember]
    //    //public int MachineUid;
    //    [DataMember]
    //    public double Bonus;  //紅包金額
    //}

    [Serializable, DataContract]
    public class CreatePlayerInfo
    {
        [DataMember]
        public int EntityUID;            // 代理商UID
        [DataMember]
        public string ManagerId;        // 管理員
        [DataMember]
        public int ManagerEntityId;     // 管理員代理商UID
        [DataMember]
        public string OperatorId;        // 開分員
        [DataMember]
        public string PlayerID;          // 玩家帳號
        [DataMember]
        public string PlayerPwd;         // 玩家密碼
        [DataMember]
        public int Sex;                  // 玩家性別
        [DataMember]
        public string NickName;          // 玩家暱稱
        [DataMember]
        public string Email;             // 玩家郵箱
        [DataMember]
        public double Balance;           // 交易金額
        [DataMember]
        public string IP_Address;        // IP位置
    }

    [Serializable, DataContract]
    public class EditPlayerInfo
    {
        [DataMember]
        public int PlayerUID;         // 玩家UID
        [DataMember]
        public string Nickname;       // 玩家密碼
        [DataMember]
        public int Sex;               // 玩家性別
        [DataMember]
        public string Email;          // 玩家電子信箱
        [DataMember]
        public string PlayerPwd;      // 玩家密碼
    }

    [Serializable, DataContract]
    public class EditManagerInfo
    {
        [DataMember]
        public String ManagerUID;         // 玩家UID
    }

    [Serializable, DataContract]
    public class EditBlockFlag
    {
        [DataMember]
        public Dictionary<string, int> ManagerBlockFlag; // 管理員凍結(解凍)名單
        [DataMember]
        public Dictionary<int, int> PlayerBlockFlag;    // 玩家凍結(解凍)名單
    }

    [Serializable, DataContract]
    public class CreateBotInfo
    {
        [DataMember]
        public int EntityUID;            // 代理商UID
        [DataMember]
        public string ManagerId;        // 管理員
        [DataMember]
        public string OperatorId;        // 開分員
        [DataMember]
        public int AddBotNum;           //增加機器人人數
        [DataMember]
        public string IP_Address;        // IP位置
    }

    [Serializable, DataContract]
    public class PumpInDrainOutValue
    {
        [DataMember]
        public string ManagerUID;
        [DataMember]
        public int Times;            // 次數
        [DataMember]
        public double Value;    // 數值
        [DataMember]
        public string IP_Address;        // IP位置
    }

    [Serializable,DataContract]
    public class WebInfo
    {
        [DataMember]
        public WebCode Type;            // 通訊類型
        [DataMember]
        public GameServerCode GameName; // 遊戲
        [DataMember]
        public int MachineUID;          // 機台
        [DataMember]
        public int UserUID;             // 玩家
        [DataMember]
        public int LogID;               // 出牌紀錄ID
        [DataMember]
        public int AwardType;           // 獎項類型
        [DataMember]
        public string AwardResult;  // 獎項類型
        [DataMember]
        public string CheckResult;  // 獎項類型
        [DataMember]
        public bool SurplusRegulate;    // 盈餘校正
        [DataMember]
        public bool IsAccount;          //入內帳
        [DataMember]
        public double Bouns;            //彩金值
        [DataMember]
        public List<BalanceDeal> Deals; // 代幣交易名單
        [DataMember]
        public BatchDepositV2Request BatchDepositV2Request;
        [DataMember]
        public BatchDepositV2Result BatchDepositV2Result;
        [DataMember]
        public BatchDepositV2QueryRequest BatchDepositV2QueryRequest;
        [DataMember]
        public BatchDepositV2QueryResult BatchDepositV2QueryResult;
        [DataMember]
        public CreatePlayerInfo CreateInfo; // 新增玩家
        [DataMember]
        public EditPlayerInfo editPlayerInfo; // 修改玩家資訊
        [DataMember]
        public EditManagerInfo editManagerInfo; // 修改玩家資訊
        [DataMember]
        public EditBlockFlag editBlockFlag; // 修改玩家資訊
        [DataMember]
        public CreateBotInfo createBotInfo; // 建立機器人資訊
        [DataMember]
        public List<string> CalculateInfo; // 機率帳資訊
        [DataMember]
        public List<GameServerCode> BotGameServer; // 調整機器人設定的遊戲
        [DataMember]
        public PumpInDrainOutValue pumpindrainoutValue; //抽放水金額(含次數)
        [DataMember]
        public Dictionary<string,string> VerData; //抽放水金額(含次數)
        [DataMember]
        public string WebCommand; 

        public WebInfo()
        {
            Type = WebCode.RefreshSettingOne;
            GameName = GameServerCode.None;
            MachineUID = 0;
            UserUID = 0;
            LogID = 0;
            AwardType = 0;
            SurplusRegulate = true;
            Deals = new List<BalanceDeal>();
            BatchDepositV2Request = new BatchDepositV2Request();
            BatchDepositV2Result = new BatchDepositV2Result();
            BatchDepositV2QueryRequest = new BatchDepositV2QueryRequest();
            BatchDepositV2QueryResult = new BatchDepositV2QueryResult();
            CreateInfo = new CreatePlayerInfo();
            editPlayerInfo = new EditPlayerInfo();
            editManagerInfo = new EditManagerInfo();
            editBlockFlag = new EditBlockFlag();
            createBotInfo = new CreateBotInfo();
            CalculateInfo = new List<string>();
            BotGameServer = new List<GameServerCode>();
            pumpindrainoutValue = new PumpInDrainOutValue();
            WebCommand = "";
            //VerData = new Dictionary<string, string>();  //加了這行, 但是沒有設值, 解序列時會出錯!!
        }
    }

    [Serializable,DataContract]
    public class JPInfo
    {
        [DataMember]
        public double Super;
        [DataMember]
        public double Mege;
        [DataMember]
        public double Major;
        [DataMember]
        public double Minor;
        [DataMember]
        public double SuperBase;
        [DataMember]
        public double MegeBase;
        [DataMember]
        public double MajorBase;
        [DataMember]
        public double MinorBase;
        [DataMember]
        public double HalfJPGetMinBet;
        [DataMember]
        public double AllJPGetMinBet;
        [DataMember]
        public int JPGetLevelCount;
        [DataMember]
        public int TreasuryStatus;
    }

    [Serializable, DataContract]
    public class UserStatus
    {
        [DataMember]
        public int memberUniquelID;
        [DataMember]
        public short Usersituation;
    }

    [Serializable, DataContract]
    public class FishingGameInit
    {
        [DataMember]
        public int ScenesType;
        [DataMember]
        public List<GunPlatformInfo> mGunPlatformInfoList;
        [DataMember]
        public List<FishPathInfo> mFishPathInfoList;

        public FishingGameInit()

        {
            mGunPlatformInfoList = new List<GunPlatformInfo>();

            mFishPathInfoList = new List<FishPathInfo>();

        }
    }

    [Serializable, DataContract]
    public class GunPlatformInfo
    {
        [DataMember]
        public int GunPlatformNo;
        [DataMember]
        public string PlayerID;
        [DataMember]
        public double Credit;
        [DataMember]
        public string Action;
        [DataMember]
        public double GunAngle;
        [DataMember]
        public int GunType;
        [DataMember]
        public double BetCoin;
    }

    [Serializable,DataContract]
    public class GunActionInfo
    {
        [DataMember]
        public string MsgType = "GunActionInfo";

        [DataMember]
        /// <summary>傳送訊信的類型</summary>
        public OperationCode operationCode;

        [DataMember]
        public int GunNum;

        [DataMember]
        public string Action;

        [DataMember]
        public double GunAngle;

        [DataMember]
        public int GunType;

        [DataMember]
        public double BetCoin;

        [DataMember]
        public int FishNo;

        [DataMember]
        public double Credit;

        [DataMember]
        public double Win;

        [DataMember]
        public string Info;

        [DataMember]
        public double GameRedEnvelope;

        [DataMember]
        public int Star;

        [DataMember]
        public int Energy;

        [DataMember]
        public int ExtData1;

        [DataMember]
        public double ExtData2;

        [DataMember]
        public string ExtInfo;

        public GunActionInfo()
        {
            MsgType = "GunActionInfo";
            Action = "";
            Info = "";
            ExtInfo = "";
        }
    }

    [Serializable, DataContract]
    public class FishPathInfo
    {
        [DataMember]
        public int No;
        [DataMember]
        public int FishType;
        [DataMember]
        public int PathType;
        [DataMember]
        public Double SyncValue;
        [DataMember]
        public Double TotalTime;
        [DataMember]
        public string Info;
    }

    [Serializable, DataContract]
    public class FishCommand
    {
        [DataMember]
        public string Command;
    }


    [Serializable, DataContract]
    public class HypeBigWater
    {
        /// <summary>指令類型</summary>
        public enum TypeCode
        {
            None = 0,
            /// <summary>變更炒場旗號</summary>
            SetHypeFg,
            /// <summary>詢問可用炒場水庫值</summary>
            AskBigWater,
            /// <summary>取用炒場水庫值</summary>
            GetBigWater,
        }

        [DataMember]
        public TypeCode Type; // 指令類型
        [DataMember]
        public GameServerCode GameServer; // 遊戲
        [DataMember]
        public int MachineUID; // 機台
        [DataMember]
        public int EntityID; // 代理商(網吧)
        [DataMember]
        public Double BigWater; //水庫值
        [DataMember]
        public Dictionary<int, Double> UserWin; //玩家贏分列表
        [DataMember]
        public string Info;
    }

    /// <summary>錢包所在位置</summary>
    public enum Walletlocation
    {
        none,
        LOCAL,
        PGS,
        JILI,
        FC,
        AceWin,
        JDB,
        DCT,
        HABA,
        Evolution,
        KingMidas,
        SexyBaccarat,
        FastSpin,
        Spade,
        PlayTech,
        CP
    }


    [Serializable, DataContract]
    public class CommonInfoData
    {
        [DataMember]
        public string MsgType = "CommonInfoData";

        [DataMember]
        public string Command;

        [DataMember]
        public GameServerCode GameServer;
        [DataMember]
        public int MachineUID;
        [DataMember]
        public int UserUID;
        [DataMember]
        public int Type;
        [DataMember]
        public string Message;

        [DataMember]
        public Dictionary<string, string> Data;


        public CommonInfoData()
        {
            MsgType = "CommonInfoData";
            Command = "";
            Message = "";
            Data = new Dictionary<string, string>();
        }
    }

    /// <summary>玩家玩遊戲時的模式</summary>
    public enum PlayerPlayType
    {
        /// <summary>正式遊戲</summary>
        Normal = 0,
        /// <summary>試玩遊戲</summary>
        Demo = 2
    }

    /// <summary>玩家玩遊戲時的押分模式</summary>
    public enum PlayGameMode
    {
        /// <summary>常規遊戲</summary>
        Normal = 0,
        /// <summary>獨立買</summary>
        Indep = 1,
        /// <summary>額外押注</summary>
        ExBet = 2
    }
}
