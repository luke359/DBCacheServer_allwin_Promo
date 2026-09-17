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
using System.Reflection.PortableExecutable;
using System.Net;
using System.Runtime.InteropServices;
using System.Drawing;

namespace VnpayAPI
{
    public class FastSpinNormResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public void SetResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public class FastSpinLoginResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url;

        public FastSpinLoginResult()
        {
            ErrorCode = "";
            Message = "";
            Url = "";
        }

        public void SetResult(string url)
        {
            ErrorCode = "0";
            Message = "Success";
            Url = url;
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class FastSpinMemberInfoData
    {
        /// <summary>交易玩家帳號</summary>
        public string Account { get; set; }
        /// <summary>交易額</summary>
        public double Amount { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public double Balance { get; set; }
        /// <summary>錢包遊戲幣餘額</summary>
        public double Score { get; set; }
        /// <summary>FastSpin產生的交易序號</summary>
        public string TransactionId { get; set; }
        /// <summary>交易結果代號</summary>
        public int Status { get; set; }

        public FastSpinMemberInfoData()
        {
            Account = "";
            TransactionId = "";
            Amount = 0;
            Balance = 0;
            Status = 0;
        }
    }

    public class FastSpinMemberInfoResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<FastSpinMemberInfoData>? Data { get; set; }

        public FastSpinMemberInfoResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
            if(errorCode == "0")
            {
                Data = new List<FastSpinMemberInfoData>();
            }
        }

        public FastSpinMemberInfoResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new List<FastSpinMemberInfoData>();
        }

        public void SetResult(List<FastSpinMemberInfoData> data)
        {
            ErrorCode = "0";
            Message = "Success";
            Data = data;
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    /// <summary>玩家帳號資訊</summary>
    public class FastSpinAccInfo
    {
        /// <summary>玩家帳號</summary>
        public string acctId { get; set; }
        /// <summary>玩家名稱(暱稱)</summary>
        public string userName { get; set; }
        /// <summary>貨幣代號</summary>
        public string currency { get; set; }
        /// <summary>錢包餘額</summary>
        public decimal balance { get; set; }

        public FastSpinAccInfo()
        {
            acctId = "";
            userName = "";
            currency = "";
            balance = 0;
        }
    }

    public class FastSpinBetInfo
    {
        /// <summary>下注单号</summary>
        public long ticketId { get; set; }
        /// <summary>玩家名稱(暱稱)</summary>
        public string acctId { get; set; }
        /// <summary>貨幣代號</summary>
        public string ticketTime { get; set; }
        /// <summary>貨幣代號</summary>
        public string categoryId { get; set; }
        /// <summary>貨幣代號</summary>
        public string gameCode { get; set; }
        /// <summary>貨幣代號</summary>
        public string currency { get; set; }
        /// <summary>錢包餘額</summary>
        public decimal betAmount { get; set; }
        /// <summary>貨幣代號</summary>
        public string result { get; set; }
        /// <summary>錢包餘額</summary>
        public decimal winLoss { get; set; }
        /// <summary>錢包餘額</summary>
        public decimal jackpotAmount { get; set; }
        /// <summary>貨幣代號</summary>
        public string betIp { get; set; }
        /// <summary>錢包餘額</summary>
        public long luckyDrawId { get; set; }
        /// <summary>錢包餘額</summary>
        public int roundId { get; set; }
        /// <summary>錢包餘額</summary>
        public int sequence { get; set; }
        /// <summary>貨幣代號</summary>
        public string channel { get; set; }
        /// <summary>錢包餘額</summary>
        public decimal Balance { get; set; }
        /// <summary>錢包餘額</summary>
        public decimal jpWin { get; set; }
        /// <summary>貨幣代號</summary>
        public string referenceId { get; set; }
        /// <summary>貨幣代號</summary>
        public string gameFeature { get; set; }

        public FastSpinBetInfo()
        {
            ticketId = 0;
            acctId = "";
            ticketTime = "";
            categoryId = "";
            gameCode = "";
            currency = "";
            betAmount = 0;
            result = "";
            winLoss = 0;
            jackpotAmount = 0;
            betIp = "";
            luckyDrawId = 0;
            roundId = 0;
            sequence = 0;
            channel = "";
            Balance = 0;
            jpWin = 0;
            referenceId = "";
            gameFeature = "";
        }
    }

    public class FastSpinGetBetRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public int pageCount { get; set; }
        public int resultCount { get; set; }
        public List<FastSpinBetInfo> list { get; set; }

        public FastSpinGetBetRecordResult()
        {
            ErrorCode = "";
            Message = "";
            pageCount = 0;
            resultCount = 0;
            list = new List<FastSpinBetInfo>();
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class FastSpin
    {
        //正式
        static string FSApiUrl = "https://api.com/api";
        static string MerchantCode = "AL8"; //商號ID
        static string SecurityKey = "ALINjNlyKGkRq";
        static string FSCurrency = "MY2"; //幣別 1:100

        /// <summary>FastSpin轉帳幣比</summary>
        const int TransCurrency = 100; //100:1

        /// <summary>Debug 訊息</summary>
        static bool DebugFg = false;


        /// <summary>取得遊戲 url</summary>
        public static async Task<FastSpinLoginResult> Login(string User, string GameId, string Lang, string homeurl, string token)
        {
            //en_US
            //zh_CN 简中
            //th_TH 泰文
            //id_ID 印尼文
            //vi_VN 越南文

            var aInfo = new
            {
                acctId = User,
                currency = FSCurrency,
            };
            var data = new
            {
                acctInfo = aInfo,
                merchantCode = MerchantCode,
                //serialNo = guid, //經測試不用也行, 但不保證以後FS會不會改
                token = token,
                acctIp = "8.8.8.8",
                game = GameId,
                language = Lang,
                exitUrl = homeurl,
                mobile = true,
                fullScreen = true,
                //fun = true, //試玩
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "getAuthorize";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            try
            {
                var values = JsonSerializer.Deserialize<ApiLoginResponse>(strResult);

                FastSpinLoginResult result = new();

                if (values.code == 0 && values.gameUrl != null)
                {
                    result.SetResult(values.gameUrl);
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"FastSpin {User} Login ERROR ! Code={values.code}, Msg={values.msg}");
                    result.SetError(values.code.ToString(), values.msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FastSpin {User} Login API異常: {ex.Message}");
                return new FastSpinLoginResult { ErrorCode = "9998", Message = "API Exception" };
            }
        }

        /// <summary>強制登出玩家</summary>
        public static async Task<FastSpinNormResult> KickMember(string User)
        {
            var data = new
            {
                acctId = User,
                merchantCode = MerchantCode,
                //serialNo = serNo, //經測試不用也行, 但不保證以後FS會不會改
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "kickAcct";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            try
            {
                var values = JsonSerializer.Deserialize<ApiNormResponse>(strResult);

                FastSpinNormResult result = new();

                if (values.code == 0)
                {
                    result.SetResult("0", "Success");
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"FastSpin {User} KickMember ERROR ! Code={values.code}, Msg={values.msg}");
                    result.SetResult(values.code.ToString(), values.msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FastSpin {User} KickMember API異常: {ex.Message}");
                return new FastSpinNormResult { ErrorCode = "9998", Message = "API Exception" };
            }
        }

        /// <summary>查玩家餘額</summary>
        public static async Task<FastSpinMemberInfoResult> CheckPlayerBalance(string User)
        {
            var data = new
            {
                acctId = User,
                pageIndex = 0,
                merchantCode = MerchantCode,
                //serialNo = serNo, //經測試不用也行, 但不保證以後FS會不會改
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "getAcctInfo";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            try
            {
                var values = JsonSerializer.Deserialize<QueryPlayerResponse>(strResult);

                FastSpinMemberInfoResult result = new();

                if (values.code == 0)
                {
                    if (values.list != null && values.list.Count > 0)
                    {
                        FastSpinMemberInfoData info = new();
                        info.Account = User;
                        info.Score = (double)values.list[0].balance;
                        info.Status = 0;
                        info.Balance = Math.Round(info.Score / TransCurrency, 4);

                        result.SetResult(new List<FastSpinMemberInfoData> { info });
                    }
                    else //values.list為零=沒有玩家資料
                    {
                        if (DebugFg) Console.WriteLine($"FastSpin {User} 查詢餘額 ERROR !");
                        result.SetError("50100", "No Player Data"); //模擬FastSpin的錯誤代碼 (50100, 账号不存在, Acct Not Found)
                    }
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"FastSpin {User} 查詢餘額 ERROR !  Code={values.code}, Msg={values.msg}");
                    result.SetError(values.code.ToString(), values.msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FastSpin {User} 查詢餘額API異常: {ex.Message}");
                return new FastSpinMemberInfoResult("9998", "API Exception");
            }
        }

        /// <summary>充值至FastSpin</summary>
        public static async Task<FastSpinMemberInfoResult> TransferInPlayerBalance(string User, double amount, string tRefId)
        {
            if (amount <= 0)
            {
                return new FastSpinMemberInfoResult("9997", "Amount Error");
            }

            decimal tamount = (decimal)Math.Round(amount * TransCurrency, 4);

            var data = new
            {
                acctId = User,
                currency = FSCurrency,
                amount = tamount,
                merchantCode = MerchantCode,
                serialNo = tRefId,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "deposit";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            FastSpinMemberInfoResult result = new();

            try
            {
                var values = JsonSerializer.Deserialize<FSTransferResponse>(strResult);

                if (values.code == 0)
                {
                    FastSpinMemberInfoData info = new();
                    info.Account = User;
                    info.Amount = amount;
                    info.Balance = (double)values.afterBalance; //After Blance: 充值後的FS錢包餘額
                    info.TransactionId = values.transactionId;
                    info.Status = 0;

                    result.SetResult(new List<FastSpinMemberInfoData> { info });
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"FastSpin {User} 充值 ERROR ! Code={values.code}, Msg={values.msg}");
                    result.SetError(values.code.ToString(), values.msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FastSpin {User} 錢包充值API異常: {ex.Message}");
                return new FastSpinMemberInfoResult("9998", "API Exception");
            }
        }

        /// <summary>從FastSpin轉出</summary>
        public static async Task<FastSpinMemberInfoResult> TransferOutPlayerBalance(string User, double amount, string tRefId)
        {
            if (amount < 0)
            {
                return new FastSpinMemberInfoResult("9997", "Amount Error");
            }

            decimal tamount = 0; //遊戲幣

            if (amount > 0)
            {
                tamount = (decimal)Math.Round(amount * TransCurrency, 4);
            }
            else if (amount == 0)
            {
                //0值 = 提領全部, 先查詢餘額
                var result2 = await CheckPlayerBalance(User);
                if (result2.ErrorCode != "0")
                {
                    return new FastSpinMemberInfoResult(result2.ErrorCode, result2.Message);
                }
                amount = result2.Data[0].Balance;
                if (DebugFg) Console.WriteLine($"FastSpin {User} 查餘額={amount}");

                if (amount == 0)
                {
                    return new FastSpinMemberInfoResult("0", "Amount Zero");
                }

                tamount = (decimal)result2.Data[0].Score;
            }

            var data = new
            {
                acctId = User,
                currency = FSCurrency,
                amount = tamount,
                merchantCode = MerchantCode,
                serialNo = tRefId,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "withdraw";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            try
            {
                var values = JsonSerializer.Deserialize<FSTransferResponse>(strResult);

                FastSpinMemberInfoResult result = new();

                if (values.code == 0)
                {
                    FastSpinMemberInfoData info = new();
                    info.Account = User;
                    info.Amount = amount;
                    info.Balance = (double)values.afterBalance; //After Blance: 充值後的FS錢包餘額
                    info.TransactionId = values.transactionId;
                    info.Status = 0;

                    result.SetResult(new List<FastSpinMemberInfoData> { info });
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"FastSpin {User} 錢包取回 ERROR ! Code={values.code}, Msg={values.msg}");
                    result.SetError(values.code.ToString(), values.msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FastSpin {User} 錢包取回API異常: {ex.Message}");
                return new FastSpinMemberInfoResult("9998", "API Exception");
            }
        }

        /// <summary>取得押注紀錄</summary>
        public static async Task<FastSpinGetBetRecordResult> GetBetRecord(string beginDate, string endDate, int pageIndex)
        {
            var data = new
            {
                beginDate = beginDate,
                endDate = endDate,
                pageIndex = pageIndex,
                merchantCode = MerchantCode,
                //serialNo = guid, //經測試不用也行, 但不保證以後FS會不會改
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "getBetHistory";

            try
            {
                var strResult = await SendApiReq(apiUrlReq, paraStr);

                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                FastSpinGetBetRecordResult result = new();

                result.ErrorCode = values["code"].ToString();

                result.Message = values["msg"].ToString();

                if (result.ErrorCode == "0")
                {
                    result.pageCount = Convert.ToInt32(values["pageCount"].ToString());

                    result.resultCount = Convert.ToInt32(values["resultCount"].ToString());

                    result.list = JsonSerializer.Deserialize<List<FastSpinBetInfo>>(values["list"].ToString());
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"FastSpin GetBetRecord Login ERROR ! Code={result.ErrorCode}, Msg={result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FastSpin GetBetRecord API異常: {ex.Message}");
                return new FastSpinGetBetRecordResult { ErrorCode = "9998", Message = "API Exception" };
            }
        }

        /// <summary>取得押注紀錄</summary>
        public static async Task<FastSpinGetBetRecordResult> GetActivityRecord(string beginDate, string endDate, int promotionId, string promotionCode, int pageIndex)
        {
            var data = new
            {
                beginDate = beginDate,
                endDate = endDate,
                pageIndex = pageIndex,
                merchantCode = MerchantCode,
                promotionId = promotionId,
                promotionCode = promotionCode,
                //serialNo = guid, //經測試不用也行, 但不保證以後FS會不會改
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "getPromotionHistory";

            try
            {
                var strResult = await SendApiReq(apiUrlReq, paraStr);

                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                FastSpinGetBetRecordResult result = new();

                result.ErrorCode = values["code"].ToString();

                result.Message = values["msg"].ToString();

                if (result.ErrorCode == "0")
                {
                    result.pageCount = Convert.ToInt32(values["pageCount"].ToString());

                    result.resultCount = Convert.ToInt32(values["resultCount"].ToString());

                    result.list = JsonSerializer.Deserialize<List<FastSpinBetInfo>>(values["list"].ToString());
                }
                else
                {
                    if (DebugFg) Console.WriteLine($"FastSpin GetBetRecord Login ERROR ! Code={result.ErrorCode}, Msg={result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FastSpin GetBetRecord API異常: {ex.Message}");
                return new FastSpinGetBetRecordResult { ErrorCode = "9998", Message = "API Exception" };
            }
        }


        // 靜態 HttpClient 實例
        //private static readonly HttpClient _Client = new HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(15)
        //};
        // 從 HttpClientProvider 取得共用的 HttpClient 實例
        private static HttpClient _Client = HttpClientProvider.Client;

        static async Task<string> SendApiReq(string api, string paraStr)
        {
            string hash = ComputeHash(paraStr);

            //Console.WriteLine($"Para=[{paraStr}]"); //UNDONE: Test Only
            //Console.WriteLine($"Hash=[{hash}]"); //UNDONE: Test Only

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, FSApiUrl);
                request.Content = new StringContent(paraStr, Encoding.UTF8, "application/json");
                //加入自訂 Header
                request.Headers.Add("API", api);
                request.Headers.Add("Digest", hash);
                request.Headers.Add("DataType", "JSON");

                HttpResponseMessage response = await _Client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                //Console.WriteLine($"{api} ResponseBody={responseBody}"); //UNDONE: Test Only

                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FastSpin SendHttp ERROR Exception : " + ex);
                return null;
            }
        }


        class ApiNormResponse
        {
            public int code { get; set; } //交易結果代號  
            public string? msg { get; set; } //訊息
            public string? serialNo { get; set; }
            public string? merchantCode { get; set; }
        }

        class ApiLoginResponse
        {
            public int code { get; set; } //交易結果代號  
            public string? msg { get; set; } //訊息
            public string? serialNo { get; set; }
            public string? merchantCode { get; set; }
            public string? token { get; set; } //我方產生的token
            public string? gameUrl { get; set; }
        }

        class FSTransferResponse
        {
            public int code { get; set; } //交易結果代號  
            public string? msg { get; set; } //訊息
            public string? serialNo { get; set; }
            public string? merchantCode { get; set; }
            public string? transactionId { get; set; } //交易ID
            public decimal afterBalance { get; set; } //交易後餘額
        }

        class QueryPlayerResponse
        {
            public int code { get; set; } //交易結果代號  
            public string? msg { get; set; } //訊息
            public string? serialNo { get; set; }
            public string? merchantCode { get; set; }
            public int resultCount { get; set; } //总共的记录数  
            public int pageCount { get; set; } //总共的页数  

            public List<FastSpinAccInfo> list { get; set; }
        }

        private static readonly HashAlgorithm DigestProvider = MD5.Create();
        private static readonly Encoding Charset = Encoding.UTF8;

        static string ComputeHash(string data)
        {
            //byte[] body = Encoding.UTF8.GetBytes(data);
            byte[] body = Charset.GetBytes(data);
            byte[] secretKey = Charset.GetBytes(SecurityKey);

            int bodySize = body.Length, keySize = secretKey.Length;
            byte[] buffer = new byte[bodySize + keySize];

            Array.Copy(body, 0, buffer, 0, bodySize);
            Array.Copy(secretKey, 0, buffer, bodySize, keySize);

            byte[] hashBytes = DigestProvider.ComputeHash(buffer);

            string hash = Convert.ToHexString(hashBytes);

            return hash;
        }
    }
}
