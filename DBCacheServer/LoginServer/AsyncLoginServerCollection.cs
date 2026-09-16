using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class AsyncLoginServerCollection
    {
        /// <summary>
        /// 登入伺服器列表
        /// </summary>
        public AsyncUserToken ConnectedLoginServer { get; set; }

        public AsyncLoginServerCollection()
        {
            ConnectedLoginServer = new AsyncUserToken();
        }

        /// <summary>
        /// 加入一個Login連線列表
        /// </summary>
        /// <param name="token"></param>
        public void AddConnectedLogin(AsyncUserToken token)
        {
            ConnectedLoginServer = token;
        }

        /// <summary>
        /// 移除一筆連線的AsyncUserToken
        /// </summary>
        /// <param name="token"></param>
        public void RemoveLogin()
        {
            if (ConnectedLoginServer != null)
                ConnectedLoginServer = new AsyncUserToken();
        }
    }
}
