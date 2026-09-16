using Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class MachineData
    {
        /// <summary>房間基礎號碼 (機台起始號碼從加1開始)</summary>
        public int RoomBaseNumber;
        /// <summary>機台總數 (注意:與開放台數不同)</summary>
        public int TotalCount;
        /// <summary>遊戲名稱</summary>
        //public GameServerCode GameServerCode;
        /// <summary>遊戲類型</summary>
        public GameTypeCode GameTypeCode;
        /// <summary>唯一碼</summary>
        //public int MachineUID { get; set; }
        /// <summary>機台名稱</summary>
        //public string MachineName;
        /// <summary>遊戲名稱</summary>
        //public string GameName { get; set; }
        /// <summary>遊戲類型</summary>
        //public string GameType { get; set; }

        /// <summary>取得機台起始號碼</summary>
        public int MachineStartNumber()
        {
            return RoomBaseNumber + 1;
        }
        /// <summary>取得機台結束號碼</summary>
        public int MachineStopNumber()
        {
            return RoomBaseNumber + TotalCount;
        }
        /// <summary>比對機台號碼是否在建置範圍</summary>
        public bool CompareMachineNo(int machineNo)
        {
            if (machineNo > RoomBaseNumber && machineNo <= (RoomBaseNumber + TotalCount))
                return true;
            return false;
        }
    }


}
