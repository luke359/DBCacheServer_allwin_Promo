using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Protocol
{
    public class AsyncUserToken
    {
        /// <summary>
        /// 客戶端類型
        /// </summary>
        public int ClientType { get; set; } // 1.gameserver 2.user 3.web

        /// <summary>
        /// 客戶端IP地址
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// 遊戲伺服器類型
        /// </summary>
        public GameServerCode gameType { get; set; }

        /// <summary>
        /// 遠端地址
        /// </summary>
        public EndPoint Remote { get; set; }

        /// <summary>
        /// 通信Socket
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// 連接時間
        /// </summary>
        public DateTime ConnectTime { get; set; }

        ///// <summary>
        ///// 所屬用戶訊息
        ///// </summary>
        public UserInfoModel UserInfo { get; set; }

        /// <summary>
        /// 遠端地址(連到GameServer)
        /// </summary>
        public string RemoteToGameServer { get; set; }

        /// <summary>
        /// 檢測心跳旗標
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// 待定狀態
        /// </summary>
        public bool IsPendingstatus { get; set; }

        /// <summary>
        /// 維護通知旗標
        /// </summary>
        public bool MaintenanceFg { get; set; }

        /// <summary>
        /// 未簽到次數
        /// </summary>
        public int NoCheckTime { get; set; }

        public int BufferOffset { get; set; }

        /// <summary>
        /// 數據緩存區
        /// </summary>
        public List<byte> Buffer { get; set; }

        private Object SendLock = new object();

        private List<SocketAsyncEventArgs> SendMsgList = new List<SocketAsyncEventArgs>();

        private bool completedAsync = false;

        public AsyncUserToken()
        {
            this.Buffer = new List<byte>();
            UserInfo = new UserInfoModel();
            MaintenanceFg = false;
            BufferOffset = 0;
        }

        public AsyncUserToken(int BetInfoLength)
        {
            this.Buffer = new List<byte>();
            UserInfo = new UserInfoModel(BetInfoLength);
            MaintenanceFg = false;
            BufferOffset = 0;
        }

        public void BeginSend(SocketAsyncEventArgs msg)
        {
            bool beSend = false;

            lock (SendLock)
            {
                if (completedAsync == false && SendMsgList.Count == 0)
                {
                    SendMsgList.Add(msg);

                    beSend = true;
                }
                else
                {
                    SendMsgList.Add(msg);
                }
            }

            if (beSend) SendMessage(msg);
        }

        public void SendMessage(SocketAsyncEventArgs msg)
        {
            bool SendResult = false;

            if (Socket != null)
            {
                try
                {
                    lock (SendLock)
                    {
                        completedAsync = Socket.SendAsync(msg);

                        SendResult = completedAsync;
                    }

                    if (!SendResult)
                    {
                        SendCallback(this, msg);
                    }
                }
                catch (Exception e)
                {
                    MyConsole.WriteLine("ClientSocketManager.cs 384", Console.ForegroundColor = ConsoleColor.Red);
                    MyConsole.WriteLine(e.Message, Console.ForegroundColor = ConsoleColor.Red);
                }
            }
        }

        public void SendCallback(object sender, SocketAsyncEventArgs e)
        {
            bool beSend = false;

            SocketAsyncEventArgs temp = new SocketAsyncEventArgs();

            lock (SendLock)
            {
                completedAsync = false;

                SendMsgList.RemoveAt(0);

                if (e.SocketError == SocketError.Success)
                {
                    if (SendMsgList.Count > 0)
                    {
                        temp = SendMsgList[0];

                        beSend = true;
                    }
                }
            }

            if (beSend)
            {
                SendMessage(temp);
            }
        }

        public bool ReceiveMessage(SocketAsyncEventArgs msg)
        {
            lock (SendLock)
            {
                return Socket.ReceiveAsync(msg);
            }
        }

        public void ClearSendMsg()
        {
            lock (SendLock)
            {
                for (int i = 0; i < SendMsgList.Count; i++)
                {
                    SendMsgList[i].Completed -= SendCallback;
                }
            }
        }
    }

    public class MessageData
    {
        public AsyncUserToken token;
        public byte[] buffer;

        public MessageData(AsyncUserToken Token, byte[] Buffer)
        {
            this.token = Token;

            this.buffer = new byte[Buffer.LongLength];

            Array.Copy(Buffer, 0, this.buffer, 0, Buffer.LongLength);
        }
    }


    public class UserInfoModel
    {
        /// <summary>
        /// 使用者狀態
        /// </summary>
        public short status { get; set; }       //狀態 1:主大廳 2.西遊遊戲大廳 3.公路之王遊戲大廳 4.湛藍遊戲大廳
        /// <summary>
        /// Session碼
        /// </summary>
        public double SessionID { get; set; }
        /// <summary>
        /// 使用者唯一碼
        /// </summary>
        public int UserUID { get; set; }

        /// <summary>
        /// 代理商唯一碼
        /// </summary>
        public int EntityUID { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 使用者密碼
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 使用者性別
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 是不是機器人
        /// </summary>
        public bool IsBot { get; set; }
        /// <summary>
        /// 機器人是否入帳
        /// </summary>
        public bool IsBotBill { get; set; }
        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string UserNickname { get; set; }
        /// <summary>
        /// 使用者餘額
        /// </summary>
        public double UserBalance { get; set; }
        /// <summary>
        /// 玩家餘額
        /// </summary>
        public double Balance 
        {
            get { return UserBalance; }
            set { UserBalance = value; }
        }
        /// <summary>
        /// 玩家總贏
        /// </summary>
        public double WinCredit { get; set; }
        /// <summary>
        /// 在哪個房間,只有Staus是2時有效
        /// </summary>
        public short roomIndex { get; set; }
        /// <summary>
        /// 在哪個座位,只有Staus是2時有效
        /// </summary>
        public short seatIndex { get; set; }
        /// <summary>
        /// 能量
        /// </summary>
        public int Energy { get; set; }
        /// <summary>
        /// 擁有星星總數
        /// </summary>
        public int Star { get; set; }

        /// <summary>
        /// 玩家押注
        /// </summary>
        public double[] Betinfo { get; set; }
        /// <summary>
        /// 房間閒置時間
        /// </summary>
        public int RoomIdleTime { get; set; }
        /// <summary>
        /// 大廳閒置時間
        /// </summary>
        public int LobbyIdleTime { get; set; }
        /// <summary>
        /// 中JP旗標
        /// </summary>
        public bool IsPlayJP { get; set; }
        /// <summary>
        /// 玩家遊戲玩完旗標
        /// </summary>
        public bool GameEnd { get; set; }

        /// <summary>
        /// 玩家遊戲後DB回傳餘額
        /// </summary>
        public double DBFinalBalance { get; set; }
        /// <summary>
        /// 押分階層
        /// </summary>
        public int BetLevel { get; set; }
        /// <summary>
        /// 押分單位
        /// </summary>
        public double BetUnit { get; set; }

        public UserInfoModel()
        {
            Betinfo = new double[1] {0};
        }

        public UserInfoModel(int BetInfoLength)
        {
            switch (BetInfoLength)
            {
                case 8:
                    Betinfo = new double[8];
                    break;
                case 9:
                    Betinfo = new double[9];
                    break;
                case 15:
                    Betinfo = new double[15];
                    break;
                case 20:
                    Betinfo = new double[20];
                    break;
                case 25:
                    Betinfo = new double[25];
                    break;
                case 30:
                    Betinfo = new double[30];
                    break;
                case 35:
                    Betinfo = new double[35];
                    break;
                case 40:
                    Betinfo = new double[40];
                    break;
                case 45:
                    Betinfo = new double[45];
                    break;
                case 50:
                    Betinfo = new double[50];
                    break;
                default:
                    Betinfo = new double[15];
                    break;
            }

            for (int i = 0; i < Betinfo.Length; i++)
            {
                Betinfo[i] = 0.0;
            }
        }

        /// <summary>
        /// 計算玩家主遊戲總押注
        /// </summary>
        public double CountMainGameTotalBet()
        {
            double totBet = 0;
            for (int i = 0; i < 12; i++)
            {
                totBet += Betinfo[i];
            }
            //MainGameTotalBet = totBet;
            return totBet;
        }

        /// <summary>計算玩家遊戲總押注(含莊閒和)</summary>
        public double CountTotalBet()
        {
            double totBet = 0;
            for (int i = 0; i < Betinfo.Length; i++)
            {
                totBet += Betinfo[i];
            }
            return totBet;
        }

        /// <summary>取得玩家總押注</summary>
        public double TotalBetInfo()
        {
            double totBet = 0;
            for (int i = 0; i < Betinfo.Length; i++)
            {
                totBet += Betinfo[i];
            }
            return totBet;
        }

        public void Resetinfo()
        {
            Array.Clear(Betinfo, 0, Betinfo.Length);
        }
    }
}
