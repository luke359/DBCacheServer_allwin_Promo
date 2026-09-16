using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class LobbyBonusData
    {
        /// <summary>唯一碼</summary>
        public int BonusUID { get; set; }
        /**<summary>玩家UID</summary>*/
        public int UserUID { get; set; }
        /**<summary>彩金金額</summary>*/
        public double BonusAmount { get; set; }
        /**<summary>領獎結果結果</summary>*/
        public int Status { get; set; } //0未領 1已領
    }
}
