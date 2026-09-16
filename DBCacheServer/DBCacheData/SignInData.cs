using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class SignInData
    {
        /// <summary>唯一碼</summary>
        public int UID { get; set; }
        /// <summary>玩家UID</summary>
        public int UserUID { get; set; }
        /// <summary>簽到記錄</summary>
        public string SingInLog { get; set; }
        /// <summary>時間戳記</summary>
        public DateTime TimeStamp { get; set; }
    }
}
