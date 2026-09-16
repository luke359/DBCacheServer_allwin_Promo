using Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class AsyncGameServerCollection
    {
        /// <summary>
        /// 遊戲伺服器列表
        /// </summary>
        public List< AsyncUserToken> ConnectedGameServer { get; set; }

        //public Dictionary<string, AsyncUserToken> RemoteToken { get; set; }

        public AsyncGameServerCollection()
        {
            ConnectedGameServer = new List<AsyncUserToken>();
            //RemoteToken = new Dictionary<string, AsyncUserToken>();
        }

        /// <summary>
        /// 加入一個GameServer連線列表
        /// </summary>
        /// <param name="token"></param>
        public void AddConnectedGameServer(AsyncUserToken token)
        {
            lock (ConnectedGameServer)
            {
                if (ConnectedGameServer.Contains(token))
                {
                    ConnectedGameServer.Remove(token);
                }
                ConnectedGameServer.Add(token);
            }
            //RemoteToken.Add(token.Remote.ToString(), token);
        }

        /// <summary>
        /// 移除一筆連線的AsyncUserToken
        /// </summary>
        /// <param name="token"></param>
        public void RemoveClient(AsyncUserToken token)
        {
            lock (ConnectedGameServer)
            {
                if (ConnectedGameServer.Contains(token))
                    ConnectedGameServer.Remove(token);
            }

            //if (RemoteToken.ContainsValue(token))
            //    RemoteToken.Remove(token.Remote.ToString());
        }
   
        /// <summary>
        /// 獲取列表中的遊戲伺服器
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AsyncUserToken GetGameToken(AsyncUserToken token)
        {
            lock (ConnectedGameServer)
            {
                foreach (var temp in ConnectedGameServer)
                {
                    if (temp.Socket == token.Socket)
                    {
                        return temp;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 由GameServerCode獲取遊戲伺服器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AsyncUserToken GetGameTokenbyGameType(GameServerCode type)
        {
            lock (ConnectedGameServer)
            {
                foreach (var temp in ConnectedGameServer)
                {
                    if (temp.gameType == type)
                    {
                        return temp;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 清除GameServer列表
        /// </summary>
        public void ClearGameServer()
        {
            lock (ConnectedGameServer)
            {
                ConnectedGameServer.Clear();
            }
        }
    }
}
