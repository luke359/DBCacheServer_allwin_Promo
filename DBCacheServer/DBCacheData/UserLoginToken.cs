using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DBCacheServer
{
    public class UserLoginToken
    {
        public int TokenId { get; set; }
        public int UserUID { get; set; }

        public Guid Token { get; set; }   // EF Core 會自動對應
        public Guid GameToken { get; set; }   // EF Core 會自動對應

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            TokenId = Convert.ToInt32(datalist["TokenId"]);
            UserUID = Convert.ToInt32(datalist["UserUID"]);
            Guid.TryParse(datalist["Token"], out var Token);
            Guid.TryParse(datalist["GameToken"], out var GameToken);
        }
    }
}
