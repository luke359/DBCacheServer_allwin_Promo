using Google.Protobuf;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace DBCacheServer
{
    /// <summary>APP平台 (以Client前端代號為準)</summary>
    public enum LgtPlatformxxx
    {
        None = 0,
    }

    public class LandPlatformData
    {
        /// <summary>平台商UID</summary>
        public int Id { get; private set; }
        /// <summary>平台商名稱</summary>
        public string PlatformName { get; private set; } = "";
        /// <summary>使用幣或芬模式(0.顯示幣，1.顯示分)</summary>
        public int UnitDisplayMode { get; private set; }


        /// <summary>從DB取出資料</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            try
            {
                Id = Convert.ToInt32(datalist["Id"]);
                PlatformName = datalist["PlatformName"];
                UnitDisplayMode = Convert.ToInt32(datalist["UnitDisplayMode"]);
                //Console.WriteLine($"  網頁平台商資料: Id={Id}, PlatformName={PlatformName}, UnitDisplayMode={UnitDisplayMode}");
            }
            catch
            {
                Console.WriteLine("! 網頁平台商資料錯誤 !");
            }
        }
    }
}
