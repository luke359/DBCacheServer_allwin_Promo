using Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class VersionData
    {
        /// <summary>唯一碼</summary>
        public int VersionUID;
        /// <summary>GameServerCode</summary>
        public int ServerCode;
        /// <summary>伺服器類型</summary>
        public string ServerType;
        /// <summary>版本號</summary>
        public string Version;
        /// <summary>iOS版本號</summary>
        public string IosVersion;
    }
}
