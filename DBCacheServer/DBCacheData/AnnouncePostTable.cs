using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;


namespace DBCacheServer
{
    public class AnnouncePostTable
    {
        //static bool DebugFg = true; //UNDONE: 訊息語言包 顯示Debug訊息

        /// <summary>公告 啟用開關</summary>
        public bool Action { get; private set; }
        /// <summary>原本舊的中大獎訊息 啟用開關</summary>
        public bool BigWinBroadcastFg { get; private set; }
        /// <summary>循環模式 : 設定每種公告顯示的順序(使用AnnouncePost.Type)</summary>
        public string TypeSequence { get; private set; }
        /// <summary>排行榜 玩家選擇模式 (0=不含假出, 1=全部)</summary>
        public int RankingGetMode { get; private set; } = 0;

        /// <summary>廣播序號</summary>
        public int BroadCastSerial = 0;

        /// <summary>公告種類列表</summary>
        public List<AnnoPostInfo> AnnoPostList;

        /// <summary>排行榜玩家及贏分</summary>
        List<RankingBox> RankingList;

        /// <summary>Client所需公告全部資訊字串</summary>
        string AnnPostString = "";

        /// <summary>取得Client所需公告全部資訊</summary>
        public string GetAnnPostString()
        {
            if (AnnPostString == "")
            {
                MyConsole.WriteLine("重製公告全部資訊");
                GenerAnnPostString();
            }

            return AnnPostString;
        }
        /// <summary>製作Client所需公告全部資訊</summary>
        public void GenerAnnPostString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("#PS#");  //公告設定 及 公告種類 分隔符號
            sb.Append(Action ? "1" : "0"); //公告啟用
            sb.Append(";");
            sb.Append(BigWinBroadcastFg ? "1" : "0"); //中大獎訊息
            sb.Append(";");
            sb.Append(TypeSequence); //公告種類顯示順序
            sb.Append("#PS#");

            var typeList = TypeSequence.Split(',')
                                        .Where(s => int.TryParse(s, out _))
                                        .Select(int.Parse)
                                        .Distinct().ToList();

            foreach (AnnoPostInfo info in AnnoPostList)
            {
                if (typeList.Contains(info.Type) || info.Type == (int)AnnoPType.歡迎詞 || info.Type == (int)AnnoPType.登入公告) //只發循環模式列表裡有的Type
                {
                    sb.Append(info.TransOutString());
                }
            }
            //Console.WriteLine($"====== 取得Client所需公告全部資訊: {sb.ToString()}");
            //return sb.ToString();

            //AnnoPostInfo annoPost = GetAnnoPost((int)AnnoPType.維修公告);
            //if (annoPost != null)
            //{
            //    annoPost.LangList
            //}

            AnnPostString = sb.ToString();
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> commSetting, List<Dictionary<string, string>> datalist)
        {
            if (commSetting == null || commSetting.Count == 0 || datalist == null || datalist.Count == 0)
            {
                BigWinBroadcastFg = true; //使用舊版的中大獎訊息
                Action = false;
                return;
            }

            //初始化公告設定
            Action = StringToBool(commSetting["Action"]);
            BigWinBroadcastFg = StringToBool(commSetting["BigWinBroadcastFg"]);
            TypeSequence = commSetting["TypeSequence"];

            RankingGetMode = int.Parse(commSetting["RankingGetMode"]);

            MyConsole.WriteLine($"公告 循環模式 : [{TypeSequence}]");

            //初始化公告種類列表
            AnnoPostList = new List<AnnoPostInfo>();

            foreach (Dictionary<string, string> data in datalist)
            {
                MyConsole.WriteLine($"    公告總類 : [{data["Type"]}]{data["Note"]}");
                //建立並載入公告設定
                AnnoPostInfo annoPostInfo = new AnnoPostInfo(data);

                //載入訊息語言包
                CheckLang(ref annoPostInfo, data, "English", AnnoPostLang.English);
                CheckLang(ref annoPostInfo, data, "ChineseCH", AnnoPostLang.Chinese_Simple);
                CheckLang(ref annoPostInfo, data, "ChineseTW", AnnoPostLang.Chinese_Traditional);
                CheckLang(ref annoPostInfo, data, "Vietnamese", AnnoPostLang.Vietnamese);
                CheckLang(ref annoPostInfo, data, "Thailand", AnnoPostLang.Thailand);
                CheckLang(ref annoPostInfo, data, "Burmese", AnnoPostLang.Burmese);
                CheckLang(ref annoPostInfo, data, "Indonesian", AnnoPostLang.Indonesian);
                CheckLang(ref annoPostInfo, data, "Bengali", AnnoPostLang.Bengali);

                AnnoPostList.Add(annoPostInfo);

                //if (false) //以下為Debug用
                //{
                //    string dstr = annoPostInfo.DeadLineFg == false ? "" : $", 有效期限 : {annoPostInfo.Deadline}";
                //    Console.WriteLine($"公告總類 : {data["Note"]} 顯示秒數={annoPostInfo.DisplayDuration} 顯示模式={annoPostInfo.DisplayMode}{dstr}");
                //    if (annoPostInfo.LangList.Count > 0)
                //    {
                //        for (int i = 0; i < annoPostInfo.LangList.Count; i++)
                //        {
                //            MyConsole.WriteLine($"{annoPostInfo.LangList[i].LangEm}:");
                //            foreach (string msg in annoPostInfo.LangList[i].Msg)
                //            {
                //                MyConsole.WriteLine($"    {msg}");
                //            }
                //        }
                //    }
                //    Console.WriteLine("------------------------------");
                //}
            }

            BroadCastSerial = Math.Abs((int)DateTime.Now.Ticks);
            //BroadCastSerial = DateTime.Now.Millisecond;
            //MyConsole.WriteLine($"重設SN={BroadCastSerial}");
        }
        /// <summary>取得指定語言訊息</summary>
        void CheckLang(ref AnnoPostInfo annoPostInfo, Dictionary<string, string> data, string langData, AnnoPostLang lang)
        {
            if (data.ContainsKey(langData) && data[langData] != null && data[langData] != "")
            {
                string utf8String = ConvertToUTF8(data[langData]);
                AnnPostLang annPostLang = new AnnPostLang(lang, utf8String);
                annoPostInfo.LangList.Add(annPostLang);
            }
        }

        /// <summary>重新設定排行榜</summary>
        public void SetRanking(List<KeyValuePair<string, string>> rankList)
        {
            RankingList = new();

            //MyConsole.WriteLine($"重新設定排行榜:{rankList.Count}");

            if (rankList.Count > 0)
            {
                int takeCnt = 3; //取3名
                if (takeCnt > rankList.Count) takeCnt = rankList.Count;

                for (int i = 0; i < takeCnt; i++)
                {
                    RankingList.Add(new RankingBox(rankList[i].Key, rankList[i].Value));
                }
            }

            AnnoPostInfo annoPost = GetAnnoPost((int)AnnoPType.排行榜);

            if (annoPost != null)
            {
                if (annoPost.LangList.Count > 0)
                {
                    foreach (AnnPostLang lang in annoPost.LangList)
                    {
                        if (lang.Msg.Count > 0)
                        {
                            for (int x = 0; x < lang.Msg.Count; x++)
                            {
                                //MyConsole.WriteLine($"重設排行榜1={lang.Msg[x]}");
                                lang.Msg[x] = ChangeRankPlayer(0, lang.Msg[x]);
                                lang.Msg[x] = ChangeRankPlayer(1, lang.Msg[x]);
                                lang.Msg[x] = ChangeRankPlayer(2, lang.Msg[x]);
                                //MyConsole.WriteLine($"重設排行榜2={lang.Msg[x]}");
                            }

                            lang.Msg.RemoveAll(x => string.IsNullOrWhiteSpace(x)); //移除空白行

                            //for (int x = 0; x < lang.Msg.Count; x++)
                            //{
                            //    MyConsole.WriteLine($"最終排行榜={lang.Msg[x]}");
                            //}
                        }
                    }
                }
            }
        }

        /// <summary>排行榜置換玩家字串</summary>
        string ChangeRankPlayer(int x, string oriStr)
        {
            if (x < RankingList.Count) //x=名次 (0~4)
            {
                string name = RankingList[x].Name;
                string win = RankingList[x].Win.ToString();

                if (x == 0)
                {
                    //MyConsole.WriteLine($"1置換1={newStr}");
                    string newStr = oriStr.Replace(JpPlay1, name).Replace(JpWin1, win);
                    //MyConsole.WriteLine($"1置換2={newStr}");
                    return newStr;
                }

                if (x == 1)
                {
                    string newStr = oriStr.Replace(JpPlay2, name).Replace(JpWin2, win);
                    return newStr;
                }

                if (x == 2)
                {
                    string newStr = oriStr.Replace(JpPlay3, name).Replace(JpWin3, win);
                    return newStr;
                }

                if (x == 3)
                {
                    string newStr = oriStr.Replace(JpPlay4, name).Replace(JpWin4, win);
                    return newStr;
                }

                if (x == 4)
                {
                    string newStr = oriStr.Replace(JpPlay5, name).Replace(JpWin5, win);
                    return newStr;
                }
            }
            else
            {
                if (x == 0)
                {
                    if (oriStr.Contains(JpPlay1) || oriStr.Contains(JpWin1))
                    {
                        //MyConsole.WriteLine($"1置換3={oriStr}");
                        return ""; //此名次無人
                    }
                }
                else if (x == 1)
                {
                    if (oriStr.Contains(JpPlay2) || oriStr.Contains(JpWin2))
                    {
                        return ""; //此名次無人
                    }
                }
                else if (x == 2)
                {
                    if (oriStr.Contains(JpPlay3) || oriStr.Contains(JpWin3))
                    {
                        return ""; //此名次無人
                    }
                }
                else if (x == 3)
                {
                    if (oriStr.Contains(JpPlay4) || oriStr.Contains(JpWin4))
                    {
                        return ""; //此名次無人
                    }
                }
                else if (x == 4)
                {
                    if (oriStr.Contains(JpPlay5) || oriStr.Contains(JpWin5))
                    {
                        return ""; //此名次無人
                    }
                }
            }
            //MyConsole.WriteLine($"不置換 = {x}, {oriStr}, {RankingList.Count}");
            return oriStr;
        }

        /// <summary>取得公告種類</summary>
        AnnoPostInfo GetAnnoPost(int type)
        {
            foreach (AnnoPostInfo info in AnnoPostList)
            {
                if (info.Type == type)
                {
                    return info;
                }
            }
            return null;
        }

        /// <summary>轉為UTF8字串</summary>
        string ConvertToUTF8(string input)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(input);
            return Encoding.UTF8.GetString(utf8Bytes);
        }

        /// <summary></summary>
        bool StringToBool(string str)
        {
            if (str == "1" || str == "True") { return true; }
            return false;
        }

        const string JpPlay1 = "@PLAY1";
        const string JpWin1 = "@WIN1";
        const string JpPlay2 = "@PLAY2";
        const string JpWin2 = "@WIN2";
        const string JpPlay3 = "@PLAY3";
        const string JpWin3 = "@WIN3";
        const string JpPlay4 = "@PLAY4";
        const string JpWin4 = "@WIN4";
        const string JpPlay5 = "@PLAY5";
        const string JpWin5 = "@WIN5";
    }

    /// <summary>排行榜</summary>
    class RankingBox
    {
        public string Name;
        public string Win;

        public RankingBox(string name, string win)
        {
            Name = name;
            Win = win;
        }   
    }

    /// <summary>訊息語言代號</summary>
    public enum AnnoPostLang
    {
        None,
        /// <summary>英語</summary>
        English,
        /// <summary>中文簡體</summary>
        Chinese_Simple,
        /// <summary>中文繁體</summary>
        Chinese_Traditional,
        /// <summary>越南</summary>
        Vietnamese,
        /// <summary>泰語</summary>
        Thailand,
        /// <summary>緬甸</summary>
        Burmese,
        /// <summary>印尼</summary>
        Indonesian,
        /// <summary>孟加拉</summary>
        Bengali,
    }

    enum AnnoPType
    {
        歡迎詞 = 1,
        登入公告 = 2,
        維修公告 = 3,
        緊急維修公告 = 4,
        排行榜 = 5,
        遊戲廣告 = 6,
        輪播廣告 = 7
    }

    /// <summary>訊息語言包</summary>
    public class AnnPostLang
    {
        bool DebugFg = true; //UNDONE: 訊息語言包 顯示Debug訊息

        /// <summary>語言代號</summary>
        public AnnoPostLang LangEm { get; private set; }
        /// <summary>語言名稱</summary>
        //public string Lang { get; private set; }
        /// <summary>公告訊息內容</summary>
        public List<string> Msg { get; private set; }

        public AnnPostLang()
        {
            //Lang = "";
            LangEm = AnnoPostLang.None;
            Msg = null;
        }
        public AnnPostLang(AnnoPostLang eLang, string msg)
        {
            LangEm = eLang;
            Msg = new List<string>(msg.Split('\n', StringSplitOptions.RemoveEmptyEntries)); //以換行符號分割
        }

        /// <summary>Client端 解訊息語言包</summary>
        public bool GetAnnPostLang(string strArray)
        {
            try
            {
                //string[] strArray2 = strArray.Split(',');
                //Lang = strArray.Split('\n')[0];
                LangEm = (AnnoPostLang)int.Parse(strArray.Split('\n', StringSplitOptions.RemoveEmptyEntries)[0]);
                Msg = new List<string>(strArray.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1));

                //Console.WriteLine($"訊息語言包:語言={Lang}");
                if (Msg.Count > 0)
                {
                    //string result = str.Replace("\r", "");

                    //Msg.ForEach(s => s.Replace("\r", ""));

                    //Console.WriteLine($"訊息語言包{LangEm}:訊息數量={Msg.Count}");
                    for (int i = 0; i < Msg.Count; i++)
                    {
                        //Msg[i].Replace("\"", "\\\"");
                        Msg[i] = Msg[i].Replace("\r", ""); //去除換行符號
                        Msg[i] = Msg[i].Replace("\n", ""); //去除換行符號

                        //string escapedStr = Msg[i].Replace("\n", "\\n").Replace("\t", "\\t").Replace("\r", "\\r");
                        //Console.WriteLine($"訊息語言包:訊息{(i+1)}={escapedStr}");
                    }
                    return true;
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"訊息語言包:錯誤! 訊息數量為0");
                }
            }
            catch (Exception ex)
            {
                if (DebugFg) Console.WriteLine($"訊息語言包 解譯錯誤 : {ex.Message}");
            }
            return false;
        }
    }

    //#250325 公告訊息
    /// <summary>公告種類</summary>
    public class AnnoPostInfo
    {
        //bool DebugFg = true; //UNDONE: 訊息語言包 顯示Debug訊息

        /// <summary>公告 啟用狀態, 由DBCache自行控制</summary>
        public bool Status;

        /// <summary>公告總類代號 (代號相同時, 取ID較大者)</summary>
        public int Type { get; private set; }
        /// <summary>每行訊息 顯示秒數</summary>
        public double DisplayDuration { get; private set; }
        /// <summary>顯示模式 (0=全部顯示, 1=每次隨機抽1行顯示)</summary>
        public int DisplayMode { get; private set; }

        /// <summary>有效期限 啟用開關 (未啟用=無期限)</summary>
        public bool DeadLineFg;
        /// <summary>有效期限, 超過期限停止顯示</summary>
        public DateTime? Deadline;

        /// <summary>訊息語言包列表</summary>
        public List<AnnPostLang> LangList { get; private set; }

        public AnnoPostInfo()
        {
            Status = true;
            Type = -1; //負值表示無效的總類
            DisplayDuration = 0;
            DisplayMode = 0;
            DeadLineFg = false;
            LangList = new List<AnnPostLang>();
        }
        public AnnoPostInfo(Dictionary<string, string> data)
        {
            Status = true;
            Type = int.Parse(data["Type"]);
            DisplayDuration = double.Parse(data["DisplayDuration"]);
            DisplayMode = int.Parse(data["DisplayMode"]);

            if (data["Deadline"] != null && data["Deadline"] != "")
            {
                //Console.WriteLine($"Deadline = {data["Deadline"]}");
                DeadLineFg = true;
                Deadline = Convert.ToDateTime(data["Deadline"]);
            }

            LangList = new List<AnnPostLang>();
        }

        /// <summary>Server用轉出字串</summary>
        public void TransOutOneLang(AnnPostLang lang, StringBuilder sb)
        {
            sb.Append((int)lang.LangEm + "\n");
            foreach (string msg in lang.Msg)
            {
                sb.Append(msg + "\n");
            }
            sb.Append("\f");
        }
        /// <summary>Server用轉出字串</summary>
        public string TransOutString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Type + ";"); //公告總類
            sb.Append(DisplayDuration + ";"); //秒數
            sb.Append(DisplayMode + "\f"); //顯示模式

            //if (Type == (int)AnnoPType.歡迎詞)
            //{
            //    int idx = 0;
            //    foreach (AnnPostLang lang in LangList)
            //    {
            //        sb.Append((int)lang.LangEm + "\n");
            //        sb.Append(lang.Msg[idx] + "\n");
            //        sb.Append("\f");
            //    }
            //}
            //else
            {
                foreach (AnnPostLang lang in LangList)
                {
                    sb.Append((int)lang.LangEm + "\n");
                    foreach (string msg in lang.Msg)
                    {
                        sb.Append(msg + "\n");
                    }
                    sb.Append("\f");
                }
            }

            sb.Append("#AP#"); //公告種類分隔符號

            return sb.ToString();
        }

        /// <summary>Client用字串解譯</summary>
        public bool GetAnnoPostPara(string strArray)
        {
            try
            {
                string[] strArray3 = strArray.Split(';', StringSplitOptions.RemoveEmptyEntries);
                Type = int.Parse(strArray3[0]);
                DisplayDuration = int.Parse(strArray3[1]);
                DisplayMode = int.Parse(strArray3[2]);

                //Console.WriteLine($"公告資訊 總類={Type}, 秒數={DisplayDuration}, 模式={DisplayMode}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            MyConsole.WriteLine($"公告資訊 字串解譯錯誤");
            return false;
        }
    }
}
