using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VnpayAPI;
using System.Security.Cryptography;
using System.IO;

using System.Security.Principal;
using System.Xml.Linq;
using System.Text.Json.Nodes;

namespace VnpayAPI
{
    public class JDBResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public JDBResult()
        {
            ErrorCode = "";
            Message = "";
            Data = "";
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class JDBLoginResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url;

        public JDBLoginResult()
        {
            ErrorCode = "";
            Message = "";
            Url = "";
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class JDBTransferData
    {
        /// <summary>玩家总余额</summary>
        public double UserBalance { get; set; }
        /// <summary>代理现金余额</summary>
        public double AgentCashBalance { get; set; }
        /// <summary>提领现金</summary>
        public double TransAmount { get; set; }
        /// <summary>JDB交易号码</summary>
        public long PId { get; set; }
        /// <summary>交易日期</summary>
        public string PayDate { get; set; }
        /// <summary>本機交易序号</summary>
        public string TransactionId { get; set; }

        public JDBTransferData()
        {
            UserBalance = 0;
            AgentCashBalance = 0;
            TransAmount = 0;
            PId = 0;
            PayDate = "";
            TransactionId = "";
        }
    }

    public class JDBTransferResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public JDBTransferData Data { get; set; }

        public JDBTransferResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new JDBTransferData();
        }
        public JDBTransferResult(string errCode, string errMsg)
        {
            SetError(errCode, errMsg);
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class JDBMemberInfoData
    {
        public string Account { get; set; }
        public double Balance { get; set; }
        public int Status { get; set; }

        public JDBMemberInfoData()
        {
            Account = "";
            Balance = 0;
            Status = 0;
        }
    }

    public class JDBMemberInfoResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<JDBMemberInfoData> Data { get; set; }

        public JDBMemberInfoResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new List<JDBMemberInfoData>();
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    /// <summary>一般Api回應</summary>
    class JDBApiResponse
    {
        public string status { get; set; }
        public string? data { get; set; }
        public string? err_text { get; set; }
    }
    /// <summary>轉帳Api回應</summary>
    class JDBTransferResp
    {
        public string status { get; set; }
        public double? userBalance { get; set; }
        public double? agentCashBalance { get; set; }
        public double? amount { get; set; }
        public string? serialNo { get; set; }
        public long? pid { get; set; }
        public string? payDate { get; set; }
        public string? err_text { get; set; }
    }

    public class JDBSoltBetRecord
    {
        public string? historyId { get; set; }
        public string? playerId { get; set; }
        public int gType { get; set; }
        public int mtype { get; set; }
        public string? gameDate{ get; set; }
        public double bet { get; set; }
        public double win { get; set; }
        public double total { get; set; }
        public string? currency { get; set; }
        public double jackpot { get; set; }
        public double jackpotContribute { get; set; }
        public double denom { get; set; }
        public string? lastModifyTime { get; set; }
        public string? playerIp { get; set; }
        public string? clientType { get; set; }
        public int hasFreegame { get; set; }
        public int systemTakeWin { get; set; }
        public string? beforeBalance { get; set; }
        public string? afterBalance { get; set; }
    }


    /// <summary>轉帳Api回應</summary>
    public class JDBSoltBetResult
    {
        public string? status { get; set; }
        public List<JDBSoltBetRecord> data { get; set; }
        public string? err_text { get; set; }

        public JDBSoltBetResult()
        {
            status = "";
            data = new List<JDBSoltBetRecord>();
            err_text = "";
        }
    }

    /// <summary>轉帳Api回應</summary>
    public class JDBSoltBetResult2
    {
        public string? status { get; set; }
        public List<JDBSoltBetRecord> data { get; set; }

        public JDBSoltBetResult2()
        {
            status = "";
            data = new List<JDBSoltBetRecord>();
        }
    }

    public class JDB
    {
        //正式
        static string JDBApiUrl = "http://api.jdb1688.net/apiRequest.do";
        static string dcName = "JKJD"; //客户域名
        static string key_aes = "af12a73f434b43c3";
        static string iv_aes = "2e7e1835b596d5f0";

        //正式RM
        //static string AgentName = ""; //代理帳號()

        //正式RMC
        static string AgentName = "alibaba88"; //代理帳號

        ////測試
        //static string JDBApiUrl = "https://api.jdb711.com/apiRequest.do";
        //static string dcName = "JKJD"; //客户域名
        //static string key_aes = "8880dc6955191877";
        //static string iv_aes = "5513493b694856f6";

        ////測試RM
        //static string AgentName = "jkjdmyrag"; //代理帳號

        //測試RMC
        //static string AgentName = "jkjdrmcag"; //代理帳號

        /// <summary>JDB轉帳幣比</summary>
        const int JDBCurrency = 100; //MYRR 100(JDB):1(我)

        /// <summary>Debug 訊息</summary>
        static bool DebugFg = false;

        enum Command
        {
            /// <summary>取得游戏连结</summary>
            GetGameLaunchURL = 11,
            /// <summary>创建玩家</summary>
            CreatePlayer = 12,
            /// <summary>提款/存款</summary>
            WithdrawDeposit = 19,
            /// <summary>查询玩家数据 (可查餘額)</summary>
            PlayerInformation = 15,
            /// <summary>踢出玩家</summary>
            KickOutPlayer = 17,
            /// <summary>查询玩家是否在游戏中 (可查餘額)</summary>
            QueryInGamePlayer = 52,

            /// <summary>创建管理层帐号</summary>
            CreateManagementAccounts = 13,
            /// <summary>玩家状态管理</summary>
            PlayerStatusManagement = 14,
            /// <summary>踢出玩家</summary>
            KickOutDownlineUsers = 58, //会对 parent 的所有下线进行动作

            /// <summary>查询历史游戏详细交易信息</summary>
            GetBetRecord = 64,
            GetActivityRecord = 62,
        }

        /// <summary></summary>
        public static async Task<JDBResult> CreateMember(string User, string AgName = "")
        {
            if(AgName == "") AgName = AgentName;

            var data = new
            {
                action = (int)Command.CreatePlayer,
                ts = Get_ts_Time(),
                parent = AgName, //代理账号
                uid = User,
                name = User,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBResult result = new();

            if (response != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<JDBApiResponse>(response);

                    if (values.status == "0000" || values.status == "7602") //創成或已存在
                    {
                        if (DebugFg) Console.WriteLine($"回報 JDB API 內容: 創建玩家 {User} 成功");
                        result.ErrorCode = "0";
                        result.Message = "Success";
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"回報 JDB API 內容: 創建玩家 {User} 失敗: ErrorCode:{values.status}, Message:{values.err_text}");
                        result.SetError(values.status, values.err_text);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JDB 創建玩家 {User} API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Result Error");
                }
            }
            else
            {
                Console.WriteLine($"JDB 創建玩家 {User} API無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary></summary>
        public static async Task<JDBResult> KickMember(string User, string AgName = "")
        {
            if (AgName == "") AgName = AgentName;

            var data = new
            {
                action = (int)Command.KickOutPlayer,
                ts = Get_ts_Time(),
                parent = AgName, //代理账号
                uid = User,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBResult result = new();

            if (response != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<JDBApiResponse>(response);

                    if (values.status == "0000")
                    {
                        if (DebugFg) Console.WriteLine($"回報 JDB API 內容: 踢出玩家 {User} 成功");
                        result.ErrorCode = "0";
                        result.Message = "Success";
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"回報 JDB API 內容: 踢出玩家 {User} 失敗: ErrorCode:{values.status}, Message:{values.err_text}");
                        result.SetError(values.status, values.err_text);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JDB 踢出玩家 {User} API JASON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Result Error");
                }
            }
            else
            {
                Console.WriteLine($"JDB 踢出玩家 {User} API無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary></summary>
        public static async Task<JDBLoginResult> GameList(string Lang)
        {
            var data = new
            {
                //action = 47, //試玩 (只有JDB自己的遊戲可以試玩)
                action = 49, //真玩
                ts = Get_ts_Time(),

                parent = AgentName,
                lang = Lang,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBLoginResult result = new();

            if (response != null)
            {
                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(response);

                if (values != null)
                {
                    string status = values["status"].ToString();

                    if (status == "0000")
                    {
                        //if (DebugFg) Console.WriteLine($"回報 JDB API 內容: {User} 登入成功 URL= {values["path"].ToString()}");

                        result.ErrorCode = "0";
                        result.Message = "Success";
                        result.Url = values["path"].ToString();
                    }
                    else
                    {
                        //if (DebugFg) Console.WriteLine($"回報 JDB API 內容: {User} 登入失敗: ErrorCode:{status}, Message:{values["err_text"].ToString()}");
                        result.SetError(status, values["err_text"].ToString());
                    }
                }
                else
                {
                    if (DebugFg) Console.WriteLine("JDB {User} 登入ERROR!");
                    result.SetError("9998", "No Result");
                }
            }
            else
            {
                if (DebugFg) Console.WriteLine("JDB {User} 登入無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary></summary>
        public static async Task<JDBLoginResult> Login(string User, string GameType, string GameId, string Lang, string homeurl)
        {
            var data = new
            {
                //action = 47, //試玩 (只有JDB自己的遊戲可以試玩)
                action = (int)Command.GetGameLaunchURL, //真玩
                ts = Get_ts_Time(),

                uid = User,
                lang = Lang,
                gType = GameType, //游戏类型
                mType = GameId, //机台代號

                windowMode = 2, //不使用 JDB 游戏大厅  ,1=使用 JDB 游戏大厅
                lobbyURL = homeurl,
                isShowDollarSign = false,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBLoginResult result = new();

            if (response != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(response);

                    string status = values["status"].ToString();

                    if (status == "0000")
                    {
                        if (DebugFg) Console.WriteLine($"回報 JDB API 內容: {User} 登入成功 URL= {values["path"].ToString()}");

                        result.ErrorCode = "0";
                        result.Message = "Success";
                        result.Url = values["path"].ToString();
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"回報 JDB API 內容: {User} 登入失敗: ErrorCode:{status}, Message:{values["err_text"].ToString()}");
                        result.SetError(status, values["err_text"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JDB {User} 登入API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Result Error");
                }
            }
            else
            {
                Console.WriteLine($"JDB {User} 登入API無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary></summary>
        public static async Task<JDBMemberInfoResult> CheckPlayerBalance(string User, string AgName = "")
        {
            if (AgName == "") AgName = AgentName;

            var data = new
            {
                //action = (int)Command.QueryInGamePlayer,
                action = (int)Command.PlayerInformation,
                ts = Get_ts_Time(),
                parent = AgName, //代理账号
                uid = User,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBMemberInfoResult result = new();
            if (response != null)
            {
                try
                {
                    //Console.WriteLine("CheckPlayerBalance Response : " + response);

                    //var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(response);
                    //var values = JsonSerializer.Deserialize<JDBTransferResp>(response);
                    //if (values != null)
                    //{

                    JsonNode jsonNode = JsonNode.Parse(response);

                    string errorCode = jsonNode["status"].GetValue<string>();

                    //if (values.status == "0000")
                    if (errorCode == "0000")
                    {
                        result.ErrorCode = "0";
                        result.Message = "Success";

                        //JsonNode jsonNode = JsonNode.Parse(response);
                        JsonArray dataArray = jsonNode["data"].AsArray();
                        string uid = dataArray[0]["uid"].GetValue<string>(); //使用這個玩家名稱
                        string username = dataArray[0]["username"].GetValue<string>();
                        double balance = (double)(dataArray[0]["balance"].GetValue<decimal>() / JDBCurrency);
                        string currency = dataArray[0]["currency"].GetValue<string>();
                        int locked = dataArray[0]["locked"].GetValue<int>();

                        JDBMemberInfoData info = new();
                        info.Account = User;
                        info.Balance = balance;
                        info.Status = 0;
                        result.Data = new List<JDBMemberInfoData> { info };

                        if (DebugFg) Console.WriteLine($"JDB {User} 查詢餘額 ST={errorCode}, BAL={info.Balance}-{currency}, UID={uid}");
                    }
                    else
                    {
                        string msg = jsonNode["err_text"].GetValue<string>();
                        if (DebugFg) Console.WriteLine($"JDB {User} 查詢餘額 ERROR ! : ST={errorCode}, MSG={msg}");
                        result.SetError(errorCode, msg);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JDB {User} 查詢餘額 API JSON 解析失敗 : " + ex);
                    result.SetError("9998", "Result Error");
                }
            }
            else
            {
                Console.WriteLine($"JDB {User} 查詢餘額 API 無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>充值至JDB</summary>
        public static async Task<JDBTransferResult> TransferInPlayerBalance(string User, double amount, string tRefId, string AgName = "")
        {
            if (amount <= 0)
            {
                return new JDBTransferResult("0", "Amount Error");
            }

            if (AgName == "") AgName = AgentName;

            double tamount = Math.Round(amount * JDBCurrency, 4);

            var data = new
            {
                action = (int)Command.WithdrawDeposit,
                ts = Get_ts_Time(),
                parent = AgName,
                uid = User,
                serialNo = tRefId,
                amount = tamount,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBTransferResult result = new();
            if (response != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<JDBTransferResp>(response);

                    if (values.status == "0000")
                    {
                        result.ErrorCode = "0";
                        result.Message = "Success";

                        double ramount = (values.amount ?? 0) / JDBCurrency; //交易金額

                        result.Data.UserBalance = values.userBalance ?? 0; //玩家总余额
                        result.Data.AgentCashBalance = values.agentCashBalance ?? 0; //代理现金余额
                        result.Data.TransAmount = ramount; //交易金額
                        result.Data.PId = values.pid ?? 0; //JDB交易序号
                        result.Data.PayDate = values.payDate ?? ""; //交易日期
                        result.Data.TransactionId = values.serialNo; //本機交易序号

                        if (DebugFg) Console.WriteLine($"JDB  {User} 充值成功 : {values.userBalance},{values.agentCashBalance},{values.amount},{values.serialNo},{values.pid},{values.payDate}");
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"JDB {User} 充值 ERROR !");
                        result.SetError(values.status, values.err_text);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JDB {User} 充值 API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"JDB {User} 充值 API 無回應1!");
                result.SetError("9999", "No Response");
            }

            return result;
        }

        /// <summary>從JDB轉出</summary>
        public static async Task<JDBTransferResult> TransferOutPlayerBalance(string User, double amount, string tRefId, string AgName = "")
        {
            if (amount < 0)
            {
                return new JDBTransferResult("0", "Amount Error");
            }

            if (AgName == "") AgName = AgentName;

            //if (amount == 0)
            //{
            //    //0值 = 提領全部, 先查詢餘額
            //    var result2 = await CheckPlayerBalance(User, AgName);
            //    if (result2.ErrorCode != "0")
            //    {
            //        return new JDBTransferResult(result2.ErrorCode, result2.Message);
            //    }
            //    amount = result2.Data[0].Balance;
            //    if (DebugFg) Console.WriteLine($"JDB {User} 查餘額={amount}");
            //
            //    if (amount == 0)
            //    {
            //        return new JDBTransferResult("0", "Amount Zero");
            //    }
            //}

            string isAll;
            double transAmount;

            if (amount == 0)
            {
                //0值 = 提領全部
                isAll = "1";
                transAmount = 0;
            }
            else
            {
                //非0值 = 提領指定金額
                isAll = "0";
                transAmount = -amount; //轉成負值 = 提領
            }

            var data = new
            {
                action = (int)Command.WithdrawDeposit,
                ts = Get_ts_Time(),
                parent = AgName,
                uid = User,
                serialNo = tRefId,
                allCashOutFlag = isAll,
                amount = transAmount,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBTransferResult result = new();
            if (response != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<JDBTransferResp>(response);
                    if (values.status == "0000")
                    {
                        result.ErrorCode = "0";
                        result.Message = "Success";

                        double ramount = (values.amount ?? 0) / JDBCurrency; //交易金額

                        result.Data.UserBalance = values.userBalance ?? 0; //玩家总余额
                        result.Data.AgentCashBalance = values.agentCashBalance ?? 0; //代理现金余额
                        result.Data.TransAmount = ramount; //交易金額
                        result.Data.PId = values.pid ?? 0; //JDB交易序号
                        result.Data.PayDate = values.payDate ?? ""; //交易日期
                        result.Data.TransactionId = values.serialNo; //本機交易序号

                        result.Data.TransAmount = Math.Abs(result.Data.TransAmount); //負值轉正

                        if (DebugFg) Console.WriteLine($"JDB  {User} 錢包取回成功 : {values.userBalance},{values.agentCashBalance},{result.Data.TransAmount},{values.serialNo},{values.pid},{values.payDate}");
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"JDB {User} 錢包取回 ERROR !");
                        result.SetError(values.status, values.err_text);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JDB {User} 錢包取回 API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"JDB {User} 錢包取回 API 無回應1!");
                result.SetError("9999", "No Response");
            }

            return result;
        }

        /// <summary>從JDB轉出</summary>
        public static async Task<JDBSoltBetResult> GetBetRecord(DateTime startT, DateTime endT, int[] platformtype, string AgName = "")
        {
            if (AgName == "") AgName = AgentName;

            var data = new
            {
                action = (int)Command.GetBetRecord,
                ts = Get_ts_Time(),
                parent = AgName,
                starttime = startT.ToString("dd-MM-yyyy HH:mm:ss"),
                endtime = endT.ToString("dd-MM-yyyy HH:mm:ss"),
                gTypes = platformtype,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq(paraStr);

            JDBSoltBetResult result = new();

            if (response != null)
            {
                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(response);

                if (values != null)
                {
                    result.status = (values["status"] == null) ? "9999" : values["status"].ToString();

                    if (result.status == "0000")
                    {
                        JDBSoltBetResult2 rrr = JsonSerializer.Deserialize<JDBSoltBetResult2>(response);

                        result.data = rrr.data;
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"JDB GetBetRecord ERROR !");
                        result.err_text = (values["err_text"] == null) ? "UnKnown" : values["err_text"].ToString();
                    }
                }
            }
            else
            {
                if (DebugFg) Console.WriteLine($"JDB GetBetRecord ERROR 無回應1!");

                result.status = "9999";

                result.err_text = "No Response";
            }

            return result;
        }

        /// <summary>從JDB轉出</summary>
        public static async Task<JDBSoltBetResult> GetActivityRecord(DateTime startT, DateTime endT, string AgName = "")
        {
            if (AgName == "") AgName = AgentName;

            var data = new
            {
                action = (int)Command.GetActivityRecord,
                ts = Get_ts_Time(),
                parent = AgName,
                startDate = startT.ToString("dd-MM-yyyy"),
                endDate = endT.ToString("dd-MM-yyyy"),
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            var response = await SendApiReq2(paraStr);

            JDBSoltBetResult result = new();

            if (response != null)
            {
                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(response);

                if (values != null)
                {
                    result.status = (values["status"] == null) ? "9999" : values["status"].ToString();

                    if (result.status == "0000")
                    {
                        JDBSoltBetResult2 rrr = JsonSerializer.Deserialize<JDBSoltBetResult2>(response);

                        result.data = rrr.data;
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"JDB GetBetRecord ERROR !");
                        result.err_text = (values["err_text"] == null) ? "UnKnown" : values["err_text"].ToString();
                    }
                }
            }
            else
            {
                if (DebugFg) Console.WriteLine($"JDB GetBetRecord ERROR 無回應1!");

                result.status = "9999";

                result.err_text = "No Response";
            }

            return result;
        }


        // 靜態 HttpClient 實例
        //private static readonly HttpClient _Client = new HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(ApiTimeout)
        //};
        // 從 HttpClientProvider 取得共用的 HttpClient 實例
        private static HttpClient _Client = HttpClientProvider.Client;

        //static async Task<Dictionary<string, Object>> SendApiReq(string paraStr)
        static async Task<string> SendApiReq(string paraStr)
        {
            //将此 JSON String 先利用 AES-CBC-128 bit 进行加密，再用 Base64 URL 编码
            string aesPara = JDBencrypt(paraStr);

            //再用 Base64 URL 编码
            string xString = System.Web.HttpUtility.UrlEncode(aesPara);

            //製作 API Request
            string apiUrlReq = $"{JDBApiUrl}?dc={dcName}&x={xString}";
            //Console.WriteLine($"ApiUrl={apiUrlReq}"); //UNDONE: Test Only

            try
            {
                Dictionary<string, string> PostContent = new Dictionary<string, string>();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrlReq);
                request.Content = new FormUrlEncodedContent(PostContent);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                //using (HttpClient client2 = new HttpClient())
                //{
                HttpResponseMessage response = await _Client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;

                //if (response != null)
                //{
                //    if (response.IsSuccessStatusCode == true)
                //    {
                //        // 取得呼叫完成 API 後的回報內容
                //        String strResult = await response.Content.ReadAsStringAsync();
                //        var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);
                //        return values;
                //    }
                //    else
                //    {
                //        Console.WriteLine("JDB SendHttp Response ERROR!!");
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("JDB SendHttp ERROR 無回應!");
                //}
                ////}
                //return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("JDB SendHttp ERROR Exception : " + ex);
                return null;
            }
        }

        //static async Task<Dictionary<string, Object>> SendApiReq(string paraStr)
        static async Task<string> SendApiReq2(string paraStr)
        {
            //将此 JSON String 先利用 AES-CBC-128 bit 进行加密，再用 Base64 URL 编码
            string aesPara = JDBencrypt(paraStr);

            //再用 Base64 URL 编码
            string xString = System.Web.HttpUtility.UrlEncode(aesPara);

            //製作 API Request
            string apiUrlReq = $"{JDBApiUrl}?dc={dcName}&x={xString}";
            //Console.WriteLine($"ApiUrl={apiUrlReq}"); //UNDONE: Test Only

            try
            {
                Dictionary<string, string> PostContent = new Dictionary<string, string>();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrlReq);
                request.Content = new FormUrlEncodedContent(PostContent);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                //using (HttpClient client2 = new HttpClient())
                //{
                HttpResponseMessage response = _Client.Send(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;

                //if (response != null)
                //{
                //    if (response.IsSuccessStatusCode == true)
                //    {
                //        // 取得呼叫完成 API 後的回報內容
                //        String strResult = await response.Content.ReadAsStringAsync();
                //        var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);
                //        return values;
                //    }
                //    else
                //    {
                //        Console.WriteLine("JDB SendHttp Response ERROR!!");
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("JDB SendHttp ERROR 無回應!");
                //}
                ////}
                //return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("JDB SendHttp ERROR Exception : " + ex);
                return null;
            }
        }


        /// <summary>取得 ts 時間</summary>
        static long Get_ts_Time()
        {
            return DateTime.Now.Ticks;
        }

        /// <summary>AES Encrypt</summary>
        static string JDBencrypt(string data)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(data);
                aes.Key = Encoding.UTF8.GetBytes(key_aes);
                aes.IV = Encoding.UTF8.GetBytes(iv_aes);

                using (var encrypter = aes.CreateEncryptor())
                {
                    encrypted = encrypter.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                }
            }
            var URL_Safe_AESEncrypt_String = Convert.ToBase64String(encrypted)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return URL_Safe_AESEncrypt_String;
        }
    }
}
