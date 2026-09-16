using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Protocol;

namespace DBCacheServer
{
    public class SocketManager
    {
        private List<string> IPList = new List<string>();

        public AsyncLoginServerCollection AsyncLoginServer;

        public AsyncGameServerCollection AsyncGameServers;

        public AsyncWebCollection AsyncWebServers;

        public AsyncWebCollection AsyncWebAPIServers;

        public AsyncWebCollection AsyncPaymentServers;

        /// <summary>
        /// 最大連接數
        /// </summary>
        private int m_maxConnectNum;

        /// <summary>
        /// 最大接收字節數
        /// </summary>
        private int m_revBufferSize;

        BufferManager m_bufferManager;

        BufferPool m_bufferPool;

        const int opsToAlloc = 2;

        /// <summary>
        /// 監聽Socket
        /// </summary>
        Socket listenSocket;

        SocketEventPool m_pool;
    
        Semaphore m_maxNumberAcceptedClients;

        string TypeName = "";

        #region 定義委託
        /// <summary>
        /// 登入伺服器端連接數量變化時觸發
        /// </summary>
        /// <param name="num">當前增加客戶的個數(退出時為負數,加入時為正數1)</param>
        /// <param name="token">增加用戶的訊息</param>
        public delegate void OnLoginNumberChange(int num, AsyncUserToken token);

        /// <summary>
        /// 遊戲伺服器端連接數量變化時觸發
        /// </summary>
        /// <param name="num"></param>
        /// <param name="token"></param>
        public delegate void OnGameServerNumberChange(int num, AsyncUserToken token);

        /// <summary>
        /// Web伺服器端連接數量變化時觸發
        /// </summary>
        /// <param name="num"></param>
        /// <param name="token"></param>
        public delegate void OnWebServerNumberChange(int num, AsyncUserToken token);

        /// <summary>
        /// WebAPI伺服器端連接數量變化時觸發
        /// </summary>
        /// <param name="num"></param>
        /// <param name="token"></param>
        public delegate void OnWebAPIServerNumberChange(int num, AsyncUserToken token);

        /// <summary>
        /// Payment伺服器端連接數量變化時觸發
        /// </summary>
        /// <param name="num"></param>
        /// <param name="token"></param>
        public delegate void OnPaymentServerNumberChange(int num, AsyncUserToken token);

        /// <summary>
        /// 接收到客戶端的數據
        /// </summary>
        /// <param name="token">客戶端</param>
        /// <param name="">客戶端數據</param>
        public delegate void OnReceiveData(AsyncUserToken token, byte[] buff);
        #endregion

        #region 定義事件
        /// <summary>
        /// 登入伺服器端連接數量變化事件
        /// </summary>
        public event OnLoginNumberChange LoginNumberChange;

        /// <summary>
        /// Web伺服器連接數量變化事件
        /// </summary>
        public event OnGameServerNumberChange GameServerNumberChange;


        /// <summary>
        /// 遊戲伺服器連接數量變化事件
        /// </summary>
        public event OnWebServerNumberChange WebServerNumberChange;

        /// <summary>
        /// WebAPI伺服器連接數量變化事件
        /// </summary>
        public event OnWebAPIServerNumberChange WebAPIServerNumberChange;

        /// <summary>
        /// Payment伺服器連接數量變化事件
        /// </summary>
        public event OnPaymentServerNumberChange PaymentServerNumberChange;

        /// <summary>
        /// 接收到客戶端的數據事件
        /// </summary>
        public event OnReceiveData ReceiveClientData;
        #endregion

        #region 定義屬性
        public AsyncUserToken LoginServer
        {
            get
            {
                if (AsyncLoginServer.ConnectedLoginServer != null)
                {
                    return AsyncLoginServer.ConnectedLoginServer;
                }
                return null;
            }
        }

        public List<AsyncUserToken> GameServerList
        {
            get
            {
                if (AsyncGameServers.ConnectedGameServer != null)
                {
                    return AsyncGameServers.ConnectedGameServer;
                }
                return null;
            }
        }

        public AsyncUserToken WebServer
        {
            get
            {
                if (AsyncWebServers.ConnectedWeb != null)
                {
                    return AsyncWebServers.ConnectedWeb;
                }
                return null;
            }
        }

        public AsyncUserToken WebAPIServer
        {
            get
            {
                if (AsyncWebAPIServers.ConnectedWeb != null)
                {
                    return AsyncWebAPIServers.ConnectedWeb;
                }
                return null;
            }
        }

        public AsyncUserToken PaymentServer
        {
            get
            {
                if (AsyncPaymentServers.ConnectedWeb != null)
                {
                    return AsyncPaymentServers.ConnectedWeb;
                }
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="numConnections">最大連接數</param>
        /// <param name="receiveBufferSize">緩衝區大小</param>
        public SocketManager(int numConnections, int receiveBufferSize)
        {
            m_maxConnectNum = numConnections;
            m_revBufferSize = receiveBufferSize;
            // allocate buffers such that the maximum number of sockets can have one outstanding read and   
            //write posted to the socket simultaneously    
            //m_bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToAlloc, receiveBufferSize);
            m_bufferPool = new BufferPool(numConnections, receiveBufferSize);
            m_pool = new SocketEventPool(numConnections);
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(string type)
        {
            ReadTxt();

            TypeName = type;

            switch (type)
            {
                case "Login":
                    AsyncLoginServer = new AsyncLoginServerCollection();
                    break;
                case "GameServer":
                    AsyncGameServers = new AsyncGameServerCollection();
                    break;
                case "Web":
                    AsyncWebServers = new AsyncWebCollection();
                    break;
                case "WebAPI":
                    AsyncWebAPIServers = new AsyncWebCollection();
                    break;
                case "Payment":
                    AsyncPaymentServers = new AsyncWebCollection();
                    break;
            }

            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds   
            // against memory fragmentation  
            //m_bufferManager.InitBuffer();

            // preallocate pool of SocketAsyncEventArgs objects  
            for (int i = 0; i < m_maxConnectNum; i++)
            {
                m_pool.Push(CreateSocketEventArgs());
            }
        }

        private SocketAsyncEventArgs CreateSocketEventArgs()
        {
            SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
            eventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            eventArgs.UserToken = new AsyncUserToken();

            byte[] buffer = m_bufferPool.Rent();
            eventArgs.SetBuffer(buffer, 0, buffer.Length);

            return eventArgs;
        }

        public bool Start(IPEndPoint localPoint)
        {
            try
            {
                listenSocket = new Socket(localPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listenSocket.Bind(localPoint);
                // start the server with a listen backlog of 100 connections  
                listenSocket.Listen(m_maxConnectNum);
                // post accepts on the listening socket  
                StartAccept(null);

                if(localPoint.ToString() == "0.0.0.0:40256")
                {
                    AsyncLoginServer.RemoveLogin();
                    MyConsole.WriteLine("DBCacgeServer Listen LoginServer Online......", Console.ForegroundColor = ConsoleColor.Green);
                }
                else if(localPoint.ToString() == "0.0.0.0:40257")
                {
                    AsyncGameServers.ClearGameServer();
                    MyConsole.WriteLine("DBCacgeServer Listen GameServer Online......", Console.ForegroundColor = ConsoleColor.Green);
                }
                else if(localPoint.ToString() == "0.0.0.0:40258")
                {
                    AsyncWebServers.RemoveWeb();
                    MyConsole.WriteLine("DBCacgeServer Listen WebServer Online......", Console.ForegroundColor = ConsoleColor.Green);
                }
                else if (localPoint.ToString() == "0.0.0.0:40259")
                {
                    AsyncWebAPIServers.RemoveWeb();
                    MyConsole.WriteLine("DBCacgeServer Listen WebAPIServer Online......", Console.ForegroundColor = ConsoleColor.Green);
                }
                else if (localPoint.ToString() == "0.0.0.0:40260")
                {
                    AsyncPaymentServers.RemoveWeb();
                    MyConsole.WriteLine("DBCacgeServer Listen PaymentServer Online......", Console.ForegroundColor = ConsoleColor.Green);
                }
                MyConsole.ResetColor();
                return true;
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine(ex.Message);
                MyConsole.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public void Stop()
        {
            //登入伺服器部分
            try
            {
                AsyncLoginServer.ConnectedLoginServer.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
            //遊戲伺服器部分
            foreach (AsyncUserToken token in AsyncGameServers.ConnectedGameServer)
            {
                try
                {
                    token.Socket.Shutdown(SocketShutdown.Both);
                    token.ClearSendMsg();
                }
                catch (Exception) { }
            }
            //web伺服器部分
            try
            {
                AsyncWebServers.ConnectedWeb.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            //webAPI伺服器部分
            try
            {
                AsyncWebAPIServers.ConnectedWeb.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            //Payment伺服器部分
            try
            {
                AsyncPaymentServers.ConnectedWeb.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            //自己的伺服器的部分
            try
            {
                listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            listenSocket.Close();

            //登入伺服器部分
            AsyncLoginServer.RemoveLogin();

            //web伺服器部分
            AsyncWebServers.RemoveWeb();

            //webAPI伺服器部分
            AsyncWebAPIServers.RemoveWeb();

            //Payment伺服器部分
            AsyncPaymentServers.RemoveWeb();

            //遊戲伺服器部分
            lock (AsyncGameServers.ConnectedGameServer)
            {
                AsyncGameServers.ClearGameServer();
            }

            /* LukeModify 2020/10/05   ServerNumberChange第二個參數不可為null, 拿掉以下程式碼看看...
            if (LoginNumberChange != null)
                LoginNumberChange(-1, null);

            if (GameServerNumberChange != null)
            {
                int g_count = AsyncGameServers.ConnectedGameServer.Count;
                GameServerNumberChange(-g_count, null);
            }

            if (WebServerNumberChange != null)
                WebServerNumberChange(-1, null);*/
        }

        /// <summary>
        /// 踢使用者
        /// </summary>
        /// <param name="token"></param>
        public void CloseClient(AsyncUserToken token)
        {
            try
            {
                if (token.Socket != null)
                {
                    token.Socket.Shutdown(SocketShutdown.Both);
                    token.ClearSendMsg();
                }
            }
            catch (Exception) { }

            //if (token.Socket != null)
            //{
            //    token.Socket.Close();
            //}
        }

        // Begins an operation to accept a connection request from the client   
        //  
        // <param name="acceptEventArg">The context object to use when issuing   
        // the accept operation on the server's listening socket</param>  
        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused  
                acceptEventArg.AcceptSocket = null;
            }

  

            m_maxNumberAcceptedClients.WaitOne();

            if (!listenSocket.AcceptAsync(acceptEventArg))
            {
                ProcessAccept(acceptEventArg);
            }
        }

        // This method is the callback method associated with Socket.AcceptAsync   
        // operations and is invoked when an accept operation is complete  
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                ProcessAccept(e);
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Accept client {0} error, message: {1}", e.AcceptSocket, ex.Message);
                MyConsole.WriteLine(ex.StackTrace);
            }
        }

        private  void ProcessAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket s = e.AcceptSocket;
                if (s.Connected)
                {
                    try
                    {
                        // Get the socket for the accepted client connection and put it into the   
                        //ReadEventArg object user token 
                        SocketAsyncEventArgs readEventArgs = m_pool.Pop();

                        // 每條連線都配一個全新的 token。
                        // 舊 token 會留在關閉流程裡當作冪等旗標(CloseFlag), 所以不能在 CloseClientSocket 裡換掉。
                        AsyncUserToken userToken = new AsyncUserToken();
                        readEventArgs.UserToken = userToken;

                        userToken.Socket = e.AcceptSocket;
                        userToken.ConnectTime = DateTime.Now;
                        userToken.Remote = e.AcceptSocket.RemoteEndPoint;

                        userToken.IPAddress = ((IPEndPoint)(e.AcceptSocket.RemoteEndPoint)).Address;

                        // 先取好本地端位址, 後面一律用這個區域變數判斷,
                        // 避免在 ReceiveAsync 投出後還去 deref userToken.Socket (那時可能已被另一條執行緒設為 null)
                        string localEndPoint = e.AcceptSocket.LocalEndPoint.ToString();

                        #region 登入伺服器
                        if (localEndPoint == IPList[0])
                        {
                            lock (AsyncLoginServer.ConnectedLoginServer)
                            {
                                AsyncLoginServer.AddConnectedLogin(userToken);

                                if (LoginNumberChange != null)
                                    LoginNumberChange(1, userToken);
                            }

                            Program.SendLockGameData();
                        }
                        #endregion

                        #region 遊戲伺服器
                        if (localEndPoint == IPList[2])
                        {
                            lock (AsyncGameServers.ConnectedGameServer)
                            {
                                AsyncGameServers.AddConnectedGameServer(userToken);
                            }

                            if (GameServerNumberChange != null)
                                GameServerNumberChange(1, userToken);
                        }
                        #endregion

                        #region web伺服器
                        if (localEndPoint == IPList[4])
                        {
                            lock (AsyncWebServers.ConnectedWeb)
                            {
                                AsyncWebServers.AddConnectedWeb(userToken);

                                if (WebServerNumberChange != null)
                                    WebServerNumberChange(1, userToken);
                            }
                        }
                        #endregion

                        #region webAPI伺服器
                        if (localEndPoint == IPList[6])
                        {
                            lock (AsyncWebAPIServers.ConnectedWeb)
                            {
                                AsyncWebAPIServers.AddConnectedWeb(userToken);

                                if (WebAPIServerNumberChange != null)
                                    WebAPIServerNumberChange(1, userToken);
                            }
                        }
                        #endregion

                        #region Payment伺服器
                        if (localEndPoint == IPList[8])
                        {
                            lock (AsyncPaymentServers.ConnectedWeb)
                            {
                                AsyncPaymentServers.AddConnectedWeb(userToken);

                                if (PaymentServerNumberChange != null)
                                    PaymentServerNumberChange(1, userToken);
                            }
                        }
                        #endregion

                        // DB 未載入完成(DBCache.Init() 失敗)時直接拒絕這條連線, 不投出 receive。
                        // 這裡不能用 CloseClient: 它只做 Shutdown, 前提是「已經有 pending 的 receive」
                        // 會以 0 byte 完成並觸發清理; accept 階段還沒投出 receive, 前提不成立。
                        // 直接呼叫 CloseClientSocket 才能確定性地歸還 SocketAsyncEventArgs 與號誌。
                        if (!Program.IsDBLoadOver)
                        {
                            CloseClientSocket(readEventArgs);

                            if (e.SocketError == SocketError.OperationAborted) return;
                            StartAccept(e);
                            return;
                        }

                        bool startReceive;

                        try
                        {
                            startReceive = e.AcceptSocket.ReceiveAsync(readEventArgs);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ReceiveAsync exception: " + ex.Message);
                            Console.WriteLine(ex.StackTrace);
                            CloseClientSocket(readEventArgs);

                            if (e.SocketError == SocketError.OperationAborted) return;
                            StartAccept(e);
                            return;
                        }

                        // ============================================================
                        // 從這裡開始 readEventArgs / userToken 的所有權已經轉移給 IOCP,
                        // 連線隨時可能在另一條執行緒被關閉、token.Socket 被設為 null、
                        // SocketAsyncEventArgs 被歸還物件池並交給下一條連線使用。
                        // 因此以下只能使用區域變數(startReceive / localEndPoint), 嚴禁再存取這兩個物件。
                        // ============================================================
                        if (!startReceive)
                        {
                            // 同步完成時 Completed 事件不會觸發, 必須由本執行緒接手處理
                            ProcessReceive(readEventArgs);
                        }

                        if (localEndPoint != IPList[6])
                        {
                            MyConsole.WriteLine("startReceive:" + startReceive.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MyConsole.WriteLine(ex.Message + "\r\n" + ex.StackTrace, Console.ForegroundColor = ConsoleColor.Red);
                    }
                }
            }

            // Accept the next connection request  
            if (e.SocketError == SocketError.OperationAborted) return;

            StartAccept(e);
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            AsyncUserToken userToken = e.UserToken as AsyncUserToken;
            try
            {
                lock(userToken)
                {
                    // determine which type of operation just completed and call the associated handler  
                    switch (e.LastOperation)
                    {
                        case SocketAsyncOperation.Receive:
                            ProcessReceive(e);
                            break;
                        case SocketAsyncOperation.Send:
                            ProcessSend(e);
                            break;
                        default:
                            throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                    }
                }
            }
            catch(Exception ex)
            {
                MyConsole.WriteLine("IO_Completed {0} error, message: {1}", userToken.Socket, ex.Message);
                MyConsole.WriteLine(ex.StackTrace);
            }
        }

        // This method is invoked when an asynchronous receive operation completes.   
        // If the remote host closed the connection, then the socket is closed.    
        // If data was received then the data is echoed back to the client.  
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                do
                {
                    if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                    {
                        AsyncUserToken token = (AsyncUserToken)e.UserToken;

                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
                        lock (token.Buffer)
                        {
                            token.Buffer.AddRange(data);
                        }

                        while (true)
                        {
                            byte[] bufferArray;
                            int packageLen;

                            lock (token.Buffer)
                            {
                                if (token.Buffer.Count < 4)
                                    break;

                                bufferArray = token.Buffer.ToArray();
                                packageLen = BitConverter.ToInt32(bufferArray, 0);

                                if (packageLen <= 0 || packageLen > 10_000_000)
                                {
                                    MyConsole.WriteLine($"Invalid packet length: {packageLen}", ConsoleColor.Yellow);
                                    CloseClientSocket(e);
                                    return;
                                }

                                if (token.Buffer.Count < packageLen + 4)
                                    break;

                                byte[] rev = token.Buffer.GetRange(4, packageLen).ToArray();
                                token.Buffer.RemoveRange(0, packageLen + 4);
                                ReceiveClientData?.Invoke(token, rev);
                            }
                        }

                    }
                    else
                    {
                        MyConsole.WriteLine("BytesTransferred: " + e.BytesTransferred);
                        MyConsole.WriteLine("SocketError: " + e.SocketError.ToString());

                        // 這幾行只是診斷用, socket 可能已被關閉/釋放。
                        // 絕不能讓它拋例外, 否則會跳過下面的 CloseClientSocket, 造成 SocketAsyncEventArgs 永遠不回池子。
                        try
                        {
                            AsyncUserToken closeToken = e.UserToken as AsyncUserToken;
                            if (closeToken != null && closeToken.Socket != null)
                            {
                                MyConsole.WriteLine("availableBytes : " + closeToken.Socket.Available);
                            }
                        }
                        catch { }

                        CloseClientSocket(e);
                        return;
                    }

                } while (!((AsyncUserToken)e.UserToken).ReceiveMessage(e)); // Repeat if ReceiveAsync returns false

            }
            catch (SocketException ex)
            {
                MyConsole.WriteLine("Socket error: " + ex.Message + "\r\n" + ex.StackTrace, ConsoleColor.Red);
                CloseClientSocket(e);
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine(ex.Message + "\r\n" + ex.StackTrace, ConsoleColor.Red);
            }

            //try
            //{
            //    // check if the remote host closed the connection  
            //    if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            //    {
            //        AsyncUserToken token = (AsyncUserToken)e.UserToken;

            //        //讀取數據
            //        byte[] data = new byte[e.BytesTransferred];
            //        Array.Copy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
            //        lock (token.Buffer)
            //        {
            //            token.Buffer.AddRange(data);
            //        }
            //        //用do-while循环
            //        //如果当客户发送大数据流的时候,e.BytesTransferred的大小就会比客户端发送过来的要小,  
            //        //需要分多次接收.所以收到包的时候,先判断包头的大小.够一个完整的包再处理.  
            //        //如果客户短时间内发送多个小数据包时, 服务器可能会一次性把他们全收了.  
            //        //这样如果没有一个循环来控制,那么只会处理第一个包,  
            //        //剩下的包全部留在token.Buffer中了,只有等下一个数据包过来后,才会放出一个来.
            //        do
            //        {
            //            //判斷包的長度
            //            byte[] lenBytes = token.Buffer.GetRange(0, 4).ToArray();

            //            int packageLen = BitConverter.ToInt32(lenBytes, 0);

            //            if (packageLen <= 0 || packageLen > 10_000_000) // 防止異常封包
            //            {
            //                MyConsole.WriteLine($"Invalid packet length: {packageLen}", Console.ForegroundColor = ConsoleColor.Yellow);
            //                CloseClientSocket(e);
            //                return;
            //            }

            //            if (packageLen > token.Buffer.Count - 4)
            //            {
            //                //長度不夠時,讓程序繼續接收
            //                break;
            //            }

            //            //包夠長時,則提取出來,交給後面的程序去處理
            //            byte[] rev = token.Buffer.GetRange(4, packageLen).ToArray();
            //            //從數據池中移除這組數據
            //            lock (token.Buffer)
            //            {
            //                token.Buffer.RemoveRange(0, packageLen + 4);
            //            }
            //            //將數據包交給後台處理
            //            if (ReceiveClientData != null)
            //                ReceiveClientData(token, rev);
            //            //这里API处理完后,并没有返回结果,当然结果是要返回的,却不是在这里, 这里的代码只管接收.  
            //            //若要返回结果,可在API处理中调用此类对象的SendMessage方法,统一打包发送.不要被微软的示例给迷惑了.  
            //        } while (token.Buffer.Count > 4);

            //        //繼續接收
            //        if (!token.ReceiveMessage(e))
            //            this.ProcessReceive(e);
            //    }
            //    else
            //    {
            //        CloseClientSocket(e);
            //    }
            //}
            //catch (SocketException ex)
            //{
            //    MyConsole.WriteLine("Socket error: " + ex.Message + "\r\n" + ex.StackTrace, Console.ForegroundColor = ConsoleColor.Red);
            //    CloseClientSocket(e);
            //}
            //catch (Exception ex)
            //{
            //    MyConsole.WriteLine(ex.Message + "\r\n" + ex.StackTrace, Console.ForegroundColor = ConsoleColor.Red);
            //}

        }

        // This method is invoked when an asynchronous send operation completes.    
        // The method issues another receive on the socket to read any additional   
        // data sent from the client  
        //  
        // <param name="e"></param>  
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // done echoing data back to the client  
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                // read the next block of data send from the client  
                bool willRaiseEvent = token.ReceiveMessage(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            // 冪等保護: 原子性地取得「由我負責歸還這個 SocketAsyncEventArgs」的資格。
            // 只要有任何一條路徑重複呼叫(或兩條執行緒同時呼叫), 第二次會在這裡直接返回。
            // 少了這道保護, 號誌會被多 Release 一次、同一個實例會被重複放回物件池,
            // 造成兩條連線共用同一個實例, 第二條在 ReceiveAsync 時就會拋
            // "An asynchronous socket operation is already in progress using this SocketAsyncEventArgs instance."
            if (!m_pool.TryBeginRelease(e))
            {
                MyConsole.WriteLine(TypeName + " CloseClientSocket: 偵測到重複關閉, 已忽略");
                return;
            }

            AsyncUserToken token = e.UserToken as AsyncUserToken;

            // 全程使用這個區域變數, 不再重複 deref token.Socket (它隨時可能被設為 null)
            Socket socket = token != null ? token.Socket : null;

            string localEndPoint;
            try
            {
                localEndPoint = socket != null ? socket.LocalEndPoint.ToString() : string.Empty;
            }
            catch
            {
                localEndPoint = string.Empty;
            }

            try
            {
                #region 登入伺服器部分
                if (localEndPoint == IPList[0] || localEndPoint == IPList[1])
                {
                    //是登入伺服器
                    lock (AsyncLoginServer.ConnectedLoginServer)
                    {
                        AsyncLoginServer.RemoveLogin();
                    }
                    //如果有事件,調用發送登入伺服器端數量變化通知
                    if (LoginNumberChange != null)
                        LoginNumberChange(-1, token);
                }
                #endregion

                #region 遊戲伺服器部分
                if (localEndPoint == IPList[2] || localEndPoint == IPList[3])
                {
                    lock (AsyncGameServers.ConnectedGameServer) { AsyncGameServers.RemoveClient(token); }
                    //如果有事件,調用發送小遊戲伺服器端數量變化通知
                    if (GameServerNumberChange != null)
                        GameServerNumberChange(-1, token);
                    return;
                }
                #endregion

                #region web伺服器部分
                if (localEndPoint == IPList[4] || localEndPoint == IPList[5])
                {
                    //是網頁伺服器
                    lock (AsyncWebServers.ConnectedWeb)
                    {
                        AsyncWebServers.RemoveWeb();
                    }
                    //如果有事件,調用發送網頁伺服器端數量變化通知
                    if (WebServerNumberChange != null)
                        WebServerNumberChange(-1, token);
                }
                #endregion

                #region webAPI伺服器部分
                if (localEndPoint == IPList[6] || localEndPoint == IPList[7])
                {
                    //是網頁伺服器
                    lock (AsyncWebAPIServers.ConnectedWeb)
                    {
                        AsyncWebAPIServers.RemoveWeb();
                    }
                    //如果有事件,調用發送網頁伺服器端數量變化通知
                    if (WebAPIServerNumberChange != null)
                        WebAPIServerNumberChange(-1, token);
                }
                #endregion

                #region Payment伺服器部分
                if (localEndPoint == IPList[8] || localEndPoint == IPList[9])
                {
                    //是網頁伺服器
                    lock (AsyncPaymentServers.ConnectedWeb)
                    {
                        AsyncPaymentServers.RemoveWeb();
                    }
                    //如果有事件,調用發送網頁伺服器端數量變化通知
                    if (PaymentServerNumberChange != null)
                        PaymentServerNumberChange(-1, token);
                }
                #endregion

                // close the socket associated with the client

                if (socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                }

                if (token != null)
                {
                    token.ClearSendMsg();
                }
            }
            catch (Exception ex)
            {
                // 注意: 這裡不可以再去讀 token.Socket.RemoteEndPoint,
                // socket 可能已經被關閉/釋放, 二次拋出的例外會逃出本方法並讓上層再呼叫一次 CloseClientSocket。
                MyConsole.WriteLine("CloseClientSocket Disconnect client [" + localEndPoint + "] error, message: " + ex.Message);
            }
            finally
            {
                // Free the SocketAsyncEventArg so they can be reused by another client

                if (e.Buffer != null)
                {
                    Array.Clear(e.Buffer, e.Offset, e.Count);
                }

                // 先確實關閉 socket, 再把 SocketAsyncEventArgs 歸還池子,
                // 避免下一條連線取走這個實例時舊 socket 還活著。
                if (socket != null)
                {
                    try { socket.Close(); } catch { }
                }

                if (token != null)
                {
                    token.Socket = null;
                }

                m_pool.EndRelease(e);

                // 號誌最後才釋放: 先歸還池子再放行 StartAccept, 否則等待中的 accept 可能 Pop 到空池。
                int available = m_maxNumberAcceptedClients.Release();

                MyConsole.WriteLine(TypeName + " maxNumber : " + available.ToString());
            }
        }

        public List<string> GetCheckIPList()
        {
            return IPList;
        }

        /// <summary>
        /// 對數據進行打包後再發送
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public void SendMessage(AsyncUserToken token, byte[] message)
        {
            if (token == null || token.Socket == null)
                return;

            try
            {
                //對要發送的訊息,制定協定,頭四字節指定包的大小,方便客戶端接收
                byte[] buff = new byte[message.Length + 4];
                byte[] len = BitConverter.GetBytes(message.Length);
                Array.Copy(len, buff, 4);
                Array.Copy(message, 0, buff, 4, message.Length);
                //token.Socket.Send(buff);  //这句也可以发送, 可根据自己的需要来选择  
                //新建异步发送对象, 发送消息  
                SocketAsyncEventArgs sendArg = new SocketAsyncEventArgs();
                sendArg.UserToken = token;
                sendArg.SetBuffer(buff, 0, buff.Length);
                sendArg.Completed += new EventHandler<SocketAsyncEventArgs>(token.SendCallback);
                token.BeginSend(sendArg);

                // 打印傳送封包狀況
                //if(token.Socket.LocalEndPoint.ToString() == "61.219.179.64:3333")
                //{
                //    Console.WriteLine("LoginServerSendMessage to [Client:{0}]", token.IPAddress, Console.ForegroundColor = ConsoleColor.Green);
                //}
                //else
                //{
                //    Console.WriteLine("LoginServerSendMessage to [GameServer:{0}]", token.IPAddress, Console.ForegroundColor = ConsoleColor.Green);
                //}
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("SendMessage - Error:" + ex.Message, Console.ForegroundColor = ConsoleColor.Red);
            }
        }

        /// <summary>
        /// 讀取外部檔
        /// </summary>
        private void ReadTxt()
        {
            try
            {
                using (StreamReader sr = new StreamReader("IPCollect.txt"))
                {
                    String text;

                    while ((text = sr.ReadLine()) != null)
                    {
                        string[] words = text.Split(':');
                        IPList.Add(words[1] + ":" + words[2]);
                    }
                }
            }
            catch (Exception e)
            {
                MyConsole.WriteLine("The file could not be read:");
                MyConsole.WriteLine(e.Message);
                MyConsole.WriteLine(e.StackTrace);
            }
        }

        public int SocketEventPoolCount()
        {
            return m_pool.Count;
        }
    }
}
