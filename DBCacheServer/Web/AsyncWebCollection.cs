using System;
using System.Collections.Generic;
using System.Text;
using Protocol;

namespace DBCacheServer
{
    public class AsyncWebCollection
    {
        /// <summary>
        /// 網頁伺服器列表
        /// </summary>
        public AsyncUserToken ConnectedWeb { get; set; }

        public AsyncWebCollection()
        {
            ConnectedWeb = new AsyncUserToken();
        }

        /// <summary>
        /// 加入一個Web連線列表
        /// </summary>
        /// <param name="token"></param>
        public void AddConnectedWeb(AsyncUserToken token)
        {
            ConnectedWeb = token;
        }

        /// <summary>
        /// 移除一筆連線的AsyncUserToken
        /// </summary>
        /// <param name="token"></param>
        public void RemoveWeb()
        {
            if (ConnectedWeb != null)
                ConnectedWeb = new AsyncUserToken();
        }
    }
}
