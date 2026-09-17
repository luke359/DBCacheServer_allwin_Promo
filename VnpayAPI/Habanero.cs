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

namespace VnpayAPI
{
    class ApiNormResponse
    {
        public bool Success { get; set; } //是否成功
        public string? Message { get; set; } //訊息
    }
    public class HABANormResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public void SetResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    class HABALoginResponse
    {
        public bool Authenticated { get; set; } //是否已認證
        public string? PlayerId { get; set; } //玩家ID
        public string? BrandId { get; set; } //品牌ID
        public string? BrandName { get; set; } //品牌名稱
        public string? Token { get; set; } //Token
        public decimal RealBalance { get; set; } //實際餘額
        public string? CurrencyCode { get; set; } //幣別
        public bool PlayerCreated { get; set; } //是否已建立玩家帳號

        public bool HasBonus { get; set; } //是否有獎金
        public decimal BonusBalance { get; set; } //獎金餘額
        public int BonusSpins { get; set; }
        public int BonusGameId { get; set; }
        public decimal BonusPercentage { get; set; }
        public decimal BonusWagerRemaining { get; set; }
        public string? CurrencySymbol { get; set; }
        public decimal PointBalance { get; set; }
        public string? Message { get; set; }
    }

    public class HABALoginResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Token;

        public HABALoginResult()
        {
            ErrorCode = "";
            Message = "";
            Token = "";
        }

        public void SetResult(string token)
        {
            ErrorCode = "0";
            Message = "Success";
            Token = token;
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class HABAMemberInfoData
    {
        /// <summary>交易玩家帳號</summary>
        public string Account { get; set; }
        /// <summary>交易額</summary>
        public double Amount { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public double Balance { get; set; }
        /// <summary>HABA產生的交易序號</summary>
        public string TransactionId { get; set; }
        /// <summary>交易結果代號</summary>
        public int Status { get; set; }

        public HABAMemberInfoData()
        {
            Account = "";
            TransactionId = "";
            Amount = 0;
            Balance = 0;
            Status = 0;
        }
    }

    public class HABAMemberInfoResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<HABAMemberInfoData>? Data { get; set; }

        public HABAMemberInfoResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public HABAMemberInfoResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new List<HABAMemberInfoData>();
        }

        public void SetResult(List<HABAMemberInfoData> data)
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

    class HABATransferResponse
    {
        public bool Success { get; set; } //是否成功
        public decimal Amount { get; set; } //交易額
        public decimal RealBalance { get; set; } //交易後錢包餘額
        public string? CurrencyCode { get; set; } //幣別
        public string? TransactionId { get; set; } //交易ID
        public string? Message { get; set; } //訊息
        public string? CurrencySymbol { get; set; } //幣別符號
    }

    class QueryPlayerResponse
    {
        public bool Found { get; set; }
        public string? PlayerId { get; set; } //玩家ID
        public string? BrandId { get; set; } //品牌ID
        public string? BrandName { get; set; } //品牌名稱
        public decimal? RealBalance { get; set; } //錢包餘額
        public string? CurrencyCode { get; set; } //幣別
        public string? CurrencySymbol { get; set; } //幣別符號
        /// <summary>玩家未Login遊戲(或被KickOut)時, 此值為null</summary>
        public string? Token { get; set; } //Token
        public bool? HasBonus { get; set; } //是否有獎金
        public decimal? BonusBalance { get; set; } //獎金餘額
        public int? BonusSpins { get; set; }
        public int? BonusGameId { get; set; }
        public decimal? BonusPercentage { get; set; }
        public decimal? BonusWagerRemaining { get; set; }
    }

    public class HABBetInfo
    {
        /// <summary>交易玩家帳號</summary>
        public string PlayerId { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string BrandId { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string Username { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string BrandGameId { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string GameKeyName { get; set; }
        /// <summary>交易玩家帳號</summary>
        public int GameTypeId { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string DtStarted { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string DtCompleted { get; set; }
        /// <summary>交易額</summary>
        public long FriendlyGameInstanceId { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string GameInstanceId { get; set; }
        /// <summary>交易玩家帳號</summary>
        public int GameStateId { get; set; }
        /// <summary>交易額</summary>
        public Decimal Stake { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public Decimal Payout { get; set; }
        /// <summary>交易額</summary>
        public Decimal JackpotWin { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public Decimal JackpotContribution { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string CurrencyCode { get; set; }
        /// <summary>交易玩家帳號</summary>
        public int ChannelTypeId { get; set; }
        /// <summary>交易額</summary>
        public Decimal BalanceAfter { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public Decimal BonusStake { get; set; }
        /// <summary>交易額</summary>
        public Decimal BonusPayout { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public Decimal BonusToReal { get; set; }
        /// <summary>交易玩家帳號</summary>
        public string BonusCoupon { get; set; }
        /// <summary>交易玩家帳號</summary>
        public int FeatureCount { get; set; }
        /// <summary>交易玩家帳號</summary>
        public int BuyFeatureId { get; set; }

        public HABBetInfo()
        {
            PlayerId = "";
            BrandId = "";
            Username = "";
            BrandGameId = "";
            GameKeyName = "";
            GameTypeId = 0;
            DtStarted = "";
            DtCompleted = "";
            FriendlyGameInstanceId = 0;
            GameInstanceId = "";
            GameStateId = 0;
            Stake = 0;
            Payout = 0;
            JackpotWin = 0;
            JackpotContribution = 0;
            CurrencyCode = "";
            ChannelTypeId = 0;
            BalanceAfter = 0;
            BonusStake = 0;
            BonusPayout = 0;
            BonusToReal = 0;
            BonusCoupon = "";
            FeatureCount = 0;
            BuyFeatureId = 0;
        }
    }

    public class HABAGetBetRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<HABBetInfo> resultlist { get; set; }

        public HABAGetBetRecordResult()
        {
            ErrorCode = "";
            Message = "";
            resultlist = new List<HABBetInfo>();
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class HABA
    {
        //正式
        static string HABAWebApiUrl = "https://ws-b.com/jsonapi";
        static string HABALaunchApiUrl = "https://app-b.com/play";
        static string HABAHistoryApiUrl = "https://app-b.com/games/history";

        static string HABABrandId = "84608d0e-224856e463";
        static string HABAApiKey = "75B5491F5CBC93";
        static string HABACurrency = "MYR"; //幣別


        /// <summary>HABA轉帳幣比</summary>
        const int TransCurrency = 100; //MYRR 100(HABA):1(我)

        /// <summary>Debug 訊息</summary>
        static bool DebugFg = false;


        /// <summary>取得遊戲 Launch Token</summary>
        public static async Task<HABALoginResult> LoginGetToken(string User, string UserPass)
        {
            //string userAgent = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36\\";

            var data = new
            {
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
                Username = User,
                password = UserPass,
                currencycode = HABACurrency,
                //useragent = userAgent,
            };

            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/loginorcreateplayer";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABALoginResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<HABALoginResponse>(strResult);

                    if (values.Authenticated)
                    {
                        result.SetResult(values.Token);
                    }
                    else
                    {
                        if (values.Message != null)
                        {
                            Console.WriteLine($"HABA {User} Login ERROR ! [{values.Message}]");
                            result.SetError("9996", values.Message);
                        }
                        else
                        {
                            if (DebugFg) Console.WriteLine($"HABA {User} Login ERROR !");
                            result.SetError("9998", "User Login Error");
                        }
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA {User} Login API JSON 解析失敗: {ex.Message}");
                    result.SetError("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA {User} Login API 無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>取得遊戲 url</summary>
        public static string LaunchGame(string token, string gameKeyname, string lang, string homeurl = "")
        {
            //string apiUrlReq = $"{HABATransferApiUrl}?brandid={HABABrandId}&keyname={keyname}&token={token}&mode=fun&locale={lang}&lobbyurl={homeurl}";

            string GameDomain = GetGameDomain().Result;

            string apiUrlReq = GameDomain + "?brandid=" + HABABrandId + "&keyname=" + gameKeyname + "&token=" + token +
                                "&locale=" + lang +
                                "&mode=Real";
                                //"&mode=fun";

            if (homeurl != "")
            {
                apiUrlReq += "&lobbyurl=" + homeurl;
            }

            //Console.WriteLine($"HABA Launch URL : {apiUrlReq}"); //UNDONE: Test Only

            return apiUrlReq;
        }

        /// <summary>變更玩家密碼</summary>
        public static async Task<HABANormResult> UpdatePlayerPassword(string User, string UserPass)
        {
            var data = new
            {
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
                Username = User,
                NewPassword = UserPass,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/UpdatePlayerPassword";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABANormResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<ApiNormResponse>(strResult);

                    if (values.Success)
                    {
                        result.SetResult("0", "Success");
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"HABA {User} 變更密碼 ERROR ! Msg={values.Message}");
                        result.SetResult("9998", values.Message);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA {User} 變更密碼API JSON 解析失敗: {ex.Message}");
                    result.SetResult("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA {User} 變更密碼API無回應!");
                result.SetResult("9999", "No Response");
            }
            return result;
        }

        /// <summary>強制登出玩家</summary>
        public static async Task<HABANormResult> KickMember(string User, string UserPass)
        {
            var data = new
            {
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
                Username = User,
                password = UserPass,
                currencycode = HABACurrency,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/logoutplayer";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABANormResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<ApiNormResponse>(strResult);

                    if (values.Success)
                    {
                        result.SetResult("0", "Success");
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"HABA {User} KickMember ERROR ! Msg={values.Message}");
                        result.SetResult("9998", values.Message);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA {User} KickMember API JSON 解析失敗: {ex.Message}");
                    result.SetResult("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA {User} KickMember API 無回應!");
                result.SetResult("9999", "No Response");
            }
            return result;
        }

        /// <summary>查玩家餘額</summary>
        public static async Task<HABAMemberInfoResult> CheckPlayerBalance(string User, string UserPass)
        {
            var data = new
            {
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
                Username = User,
                password = UserPass,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/QueryPlayer";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABAMemberInfoResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<QueryPlayerResponse>(strResult);

                    if (values.Found)
                    {
                        HABAMemberInfoData info = new();
                        info.Account = User;
                        info.Balance = (double)values.RealBalance;
                        info.Status = 0;

                        result.SetResult(new List<HABAMemberInfoData> { info });
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"HABA {User} 查詢餘額 ERROR !");
                        result.SetError("9998", "Player Not Found");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA {User} 查詢餘額API JSON 解析失敗: {ex.Message}");
                    result.SetError("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA {User} 查詢餘額API無回應!");
                result.SetError("9999", "No Response");
            }

            return result;
        }

        /// <summary>充值至HABA</summary>
        public static async Task<HABAMemberInfoResult> TransferInPlayerBalance(string User, string UserPass, double amount, string tRefId)
        {
            if (amount <= 0)
            {
                return new HABAMemberInfoResult("9997", "Amount Error");
            }

            decimal tamount = (decimal)amount;

            var data = new
            {
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
                Username = User,
                Password = UserPass,
                CurrencyCode = HABACurrency,
                Amount = amount,
                RequestId = tRefId
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/DepositPlayerMoney";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABAMemberInfoResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<HABATransferResponse>(strResult);

                    if (values.Success)
                    {
                        HABAMemberInfoData info = new();
                        info.Account = User;
                        info.Amount = amount;
                        info.Balance = (double)values.RealBalance; //After Blance: 充值後的HABA錢包餘額
                        info.TransactionId = values.TransactionId;
                        info.Status = 0;

                        result.SetResult(new List<HABAMemberInfoData> { info });
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"HABA {User} 充值 ERROR ! Msg={values.Message}");
                        result.SetError("9998", values.Message);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA {User} 充值API JSON 解析失敗: {ex.Message}");
                    result.SetError("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA {User} 充值API無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>從HABA轉出</summary>
        public static async Task<HABAMemberInfoResult> TransferOutPlayerBalance(string User, string UserPass, double amount, string tRefId)
        {
            if (amount < 0)
            {
                return new HABAMemberInfoResult("9997", "Amount Error");
            }

            bool isAll;
            decimal tamount;

            if (amount == 0)
            {
                //0值 = 提領全部
                isAll = true;
                tamount = 0;
            }
            else
            {
                //非0值 = 提領指定金額
                isAll = false;
                tamount = -(decimal)amount;
            }

            var data = new
            {
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
                Username = User,
                Password = UserPass,
                CurrencyCode = HABACurrency,
                Amount = tamount,
                WithdrawAll = isAll,
                RequestId = tRefId
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/WithdrawPlayerMoney";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABAMemberInfoResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<HABATransferResponse>(strResult);

                    if (values.Success)
                    {
                        HABAMemberInfoData info = new();
                        info.Account = User;
                        info.Amount = Math.Abs((double)values.Amount); //交易額負轉正
                        info.Balance = (double)values.RealBalance; //After Blance: 取回後的HABA錢包餘額
                        info.TransactionId = values.TransactionId;
                        info.Status = 0;

                        result.SetResult(new List<HABAMemberInfoData> { info });
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"HABA {User} 錢包取回 ERROR ! Msg={values.Message}");
                        result.SetError("9998", values.Message);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA {User} 錢包取回API JSON 解析失敗: {ex.Message}");
                    result.SetError("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA {User} 錢包取回API無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>取得遊戲 Launch Token</summary>
        public static async Task<HABAGetBetRecordResult> GetBetRecord(string startT, string endT)
        {
            //string userAgent = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36\\";

            var data = new
            {
                //BrandId = "1234",
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
                DtStartUTC = startT,
                DtEndUTC = endT,
            };

            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/getbrandcompletedgameresultsv2";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABAGetBetRecordResult result = new();

            if (strResult != null)
            {
                try
                {
                    result.resultlist = JsonSerializer.Deserialize<List<HABBetInfo>>(strResult);

                    result.SetError("0", "success");

                    //if (values.Authenticated)
                    //{
                    //    result.SetResult(values.Token);
                    //}
                    //else
                    //{
                    //    if (DebugFg) Console.WriteLine($"HABA GetBetRecord ERROR !");
                    //    result.SetError("9998", "User GetBetRecord Error");
                    //}
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA GetBetRecord API JSON 解析失敗: {ex.Message}");
                    result.SetError("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA GetBetRecord API 無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }


        /// <summary>取得遊戲 Launch Token</summary>
        public static async Task<string> GetGameDomain()
        {
            //string userAgent = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36\\";

            var data = new
            {
                //BrandId = "1234",
                BrandId = HABABrandId,
                Country = "MY",
            };

            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/GetGameDomain";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABAGetBetRecordResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                    if (values["Success"].ToString() == "True")
                    {
                        return values["LaunchURL"].ToString()+"play";
                    }
                    else
                    {
                        return HABALaunchApiUrl;
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA GetBetRecord API JSON 解析失敗: {ex.Message}");
                    return HABALaunchApiUrl;
                }
            }
            else
            {
                Console.WriteLine($"HABA GetBetRecord API 無回應!");
                return HABALaunchApiUrl;
            }
            return HABALaunchApiUrl;
        }

        /// <summary>取得遊戲 Launch Token</summary>
        public static async Task<HABAGetBetRecordResult> GetGameList()
        {
            //string userAgent = "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36\\";

            var data = new
            {
                //BrandId = "1234",
                BrandId = HABABrandId,
                APIKey = HABAApiKey,
            };

            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = HABAWebApiUrl + "/GetGames";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            HABAGetBetRecordResult result = new();

            if (strResult != null)
            {
                try
                {
                    result.resultlist = JsonSerializer.Deserialize<List<HABBetInfo>>(strResult);

                    result.SetError("0", "success");

                    //if (values.Authenticated)
                    //{
                    //    result.SetResult(values.Token);
                    //}
                    //else
                    //{
                    //    if (DebugFg) Console.WriteLine($"HABA GetBetRecord ERROR !");
                    //    result.SetError("9998", "User GetBetRecord Error");
                    //}
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"HABA GetBetRecord API JSON 解析失敗: {ex.Message}");
                    result.SetError("9997", "JSON Error");
                }
            }
            else
            {
                Console.WriteLine($"HABA GetBetRecord API 無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }


        // 靜態 HttpClient 實例
        //private static readonly HttpClient _Client = new HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(15)
        //};
        // 從 HttpClientProvider 取得共用的 HttpClient 實例
        private static HttpClient _Client = HttpClientProvider.Client;

        //static async Task<Dictionary<string, Object>> SendApiReq(string paraStr)
        static async Task<string> SendApiReq(string apiurl, string paraStr)
        {
            //HttpContent content = new StringContent(paraStr, Encoding.UTF8, "application/json");

            //Console.WriteLine($"HABA ApiUrl=[{apiurl}]"); //UNDONE: Test Only
            //Console.WriteLine($"Para=[{paraStr}]"); //UNDONE: Test Only

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiurl);
                request.Content = new StringContent(paraStr, Encoding.UTF8, "application/json");

                //using (HttpClient client2 = new HttpClient())
                //{
                HttpResponseMessage response = _Client.Send(request);
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode == true)
                {
                    // 取得 HTTP 錯誤碼 (例如: 404, 500 等)
                    int statusCode = (int)response.StatusCode;
                    HttpStatusCode httpStatusCode = response.StatusCode;
                    Console.WriteLine($"HTTP 錯誤碼: {statusCode} ({httpStatusCode})");

                    if (response.Headers.TryGetValues("X-HABA-ErrorCode", out var errorCodeValues))
                    {
                        if (errorCodeValues.Any()) // 確保集合不為空
                        {
                            string errorCode = errorCodeValues.FirstOrDefault();
                            Console.WriteLine($"X-HABA-ErrorCode: {errorCode}");
                        }
                    }

                    if (response.Headers.TryGetValues("X-HABA-ErrorName", out var errorNameValues))
                    {
                        if (errorNameValues.Any()) // 確保集合不為空
                        {
                            string errorCode = errorNameValues.FirstOrDefault();
                            Console.WriteLine($"X-HABA-ErrorName: {errorCode}");
                        }
                    }

                    if (response.Headers.TryGetValues("X-HABA-ErrorDetail", out var errorDetailValues))
                    {
                        if (errorDetailValues.Any()) // 確保集合不為空
                        {
                            string errorCode = errorDetailValues.FirstOrDefault();
                            Console.WriteLine($"X-HABA-ErrorDetail: {errorCode}");
                        }
                    }
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine("HABA SendHttp ERROR Exception : " + ex);
                return null;
            }
        }
    }
}
