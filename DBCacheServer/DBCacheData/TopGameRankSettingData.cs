using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    public class TopGameRankSettingData
    {
        /// <summary>(單位:週)</summary>
        public int Top_Player { get; set; }
        /// <summary>(單位:次)</summary>
        public int Top_Game { get; set; }
        //public int Top_New { get; set; }
        //public int Top_Win { get; set; }
        //public int Top_Loss { get; set; }
        public DateTime ClearRankTime { get; set; }


        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist)
        {
            //datalist2 為各遊戲特有的設定
            Top_Player = Convert.ToInt32(datalist["Top_Player"]);
            Top_Game = Convert.ToInt32(datalist["Top_Game"]);
            //Top_New = Convert.ToInt32(datalist["Top_New"]);
            //Top_Win = Convert.ToInt32(datalist["Top_Win"]);
            //Top_Loss = Convert.ToInt32(datalist["Top_Loss"]);

            string date = datalist["ClearRankTime"];
            if (date == "")
            {
                ClearRankTime = DateTime.Now.AddDays(-(Top_Player * 7));
            }
            else
            {
                ClearRankTime = Convert.ToDateTime(datalist["ClearRankTime"]);
            }
        }

        /// <summary>獲取遊戲設定值</summary>
        //public Dictionary<string,string> GetSetting()
        //{
        //    Dictionary<string, string> data = new Dictionary<string, string>();
        //    data.Add("Top_Player", Top_Player.ToString());
        //    return data;
        //}
    }
}
