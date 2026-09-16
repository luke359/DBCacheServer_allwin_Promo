using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCacheServer
{
    class PGSlotData
    {
        /// <summary>PGSlot玩家令牌</summary>
        public string PGSlotPlayerSession = Guid.NewGuid().ToString();

        /// <summary>PGSlot玩家令牌</summary>
        public DateTime PGSlotTimestamp = DateTime.Now;
    }
}
