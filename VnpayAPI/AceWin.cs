using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace VnpayAPI
{
    public class AceWinLoginResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url;

        public AceWinLoginResult()
        {
            ErrorCode = "";
            Message = "";
            Url = "";
        }
    }

    public class AceWinResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public AceWinResult()
        {
            ErrorCode = "";
            Message = "";
            Data = "";
        }
    }

    public class AceWinTransferData
    {
        public string TransactionId { get; set; }
        public double CoinBefore { get; set; }
        public double CoinAfter { get; set; }
        public double CurrencyBefore { get; set; }
        public double CurrencyAfter { get; set; }
        public int Status { get; set; }

        public AceWinTransferData()
        {
            TransactionId = "";
            CoinBefore = 0;
            CoinAfter = 0;
            CoinBefore = 0;
            CurrencyBefore = 0;
            CurrencyAfter = 0;
            Status = 0;
        }
    }

    public class AceWinTransferResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public AceWinTransferData Data { get; set; }

        public AceWinTransferResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new AceWinTransferData();
        }
    }

    public class AceWinMemberInfoData
    {
        public string Account { get; set; }
        public double Balance { get; set; }
        public int Status { get; set; }

        public AceWinMemberInfoData()
        {
            Account = "";
            Balance = 0;
            Status = 0;
        }
    }

    public class AceWinMemberInfoResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<AceWinMemberInfoData> Data { get; set; }

        public AceWinMemberInfoResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new List<AceWinMemberInfoData>();
        }
    }

    public class AceWinBetRecordData
    {
        public string Account { get; set; }
        public long WagersId { get; set; }
        public int GameId { get; set; }
        public DateTime WagersTime { get; set; }
        public decimal BetAmount { get; set; }
        public DateTime PayoffTime { get; set; }
        public decimal PayoffAmount { get; set; }
        public int Status { get; set; }
        public DateTime SettlementTime { get; set; }
        public int GameCategoryId { get; set; }
        public int Type { get; set; }
        public string AgentId { get; set; }
        public decimal Turnover { get; set; }

        public AceWinBetRecordData()
        {
            Account = "";
            WagersId = 0;
            GameId = 0;
            WagersTime = new DateTime(1984, 12, 11);
            BetAmount = 0;
            PayoffTime = new DateTime(1984, 12, 11);
            PayoffAmount = 0;
            Status = 0;
            SettlementTime = new DateTime(1984, 12, 11);
            GameCategoryId = 0;
            Type = 0;
            AgentId = "";
            Turnover = 0;
        }
    }

    public class AceWinBetRecordPagination
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageLimit { get; set; }
        public int TotalNumber { get; set; }

        public AceWinBetRecordPagination()
        {
            CurrentPage = 0;
            TotalPages = 0;
            PageLimit = 0;
            TotalNumber = 0;
        }
    }


    public class AceWinBetRecordInfo
    {
        public List<AceWinBetRecordData> Result { get; set; }
        public AceWinBetRecordPagination Pagination { get; set; }

        public AceWinBetRecordInfo()
        {
            Result = new List<AceWinBetRecordData>();
            Pagination = new AceWinBetRecordPagination();
        }
    }

    public class AceWinBetRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public AceWinBetRecordInfo Data { get; set; }

        public AceWinBetRecordResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new AceWinBetRecordInfo();
        }
    }

    public class AceWinDetailUrlInfo
    {
        public string Url { get; set; }

        public AceWinDetailUrlInfo()
        {
            Url = "";
        }
    }

    public class AceWinDetailUrlResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public AceWinDetailUrlInfo Data { get; set; }

        public AceWinDetailUrlResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new AceWinDetailUrlInfo();
        }
    }

    public class AceWin
    {
        static readonly string AgentID = "albb_alibabamy_myr";

        //User Acceptance Test
        //static readonly string AgentKey = "128f0bc2ad9072bd631b1bea1de6a8ecd7a35fab";
        //static readonly string ApiRoute = "https://macross-platform-ag-stg.acewinplusfafafa.com/api1/";

        /// <summary>JILI轉帳幣比</summary>
        const int AceWinCurrency = 100;

        //Production：
        static readonly string AgentKey = "2a289aa2bee2856a9b304dce8bd21075d30591b2";
        static readonly string ApiRoute = "https://macross-platform-ag-prod.acewinplusfafafa.com/api1/";

        const double ApiTimeout = 15;  // 設定請求超時為 15 秒

        // 靜態 HttpClient 實例
        //private static readonly HttpClient _Client = new HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(ApiTimeout)
        //};

        // 從 HttpClientProvider 取得共用的 HttpClient 實例
        private static HttpClient _Client = HttpClientProvider.Client;


        public static string GetKeyG()
        {
            string UTC_4Txt = DateTime.UtcNow.AddHours(-4).ToString("yyMMd");

            string Temp = UTC_4Txt + AgentID + AgentKey;

            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(Temp);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            string KeyG = builder.ToString();

            return KeyG;
        }

        public static string GetKey(string Params)
        {
            string Temp = Params + GetKeyG();

            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.UTF8.GetBytes(Temp);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            string md5string = builder.ToString();

            string randomText1 = "LgtKey";

            string randomText2 = "123456";

            string Key = randomText1 + md5string + randomText2;

            return Key;
        }

        public static async Task<AceWinResult> CreateMember(string Account)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string ApiUrl = $"{ApiRoute}CreateMember";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    //using (HttpClient client2 = new HttpClient())
                    {
                        //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "Account=" + Account + "&AgentId=" + AgentID;
                        PostContent.Add("Account", Account);
                        PostContent.Add("AgentId", AgentID);
                        PostContent.Add("Key", GetKey(Params));

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = await _Client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                AceWinResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            AceWinResult result = new();
                            result.ErrorCode = "9998";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                AceWinResult result = new();
                result.ErrorCode = "9998";
                result.Message = "post connection status error";
                return result;
            }
            catch (Exception ex)
            {
                AceWinResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        public static async Task<AceWinLoginResult> Login(string User, string GameId, string Lang, string homeurl)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string ApiUrl = $"{ApiRoute}LoginWithoutRedirect";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    //using (HttpClient client2 = new HttpClient())
                    {
                        //client.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "Account=" + User + "&GameId=" + GameId + "&Lang=" + Lang + "&AgentId=" + AgentID;
                        PostContent.Add("Account", User);
                        PostContent.Add("GameId", GameId);
                        PostContent.Add("Lang", Lang);
                        //if (homeurl != "")
                        //{
                        //    PostContent.Add("HomeUrl", homeurl);
                        //}
                        PostContent.Add("AgentId", AgentID);
                        PostContent.Add("Key", GetKey(Params));

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = await _Client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinLoginResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                if (result.ErrorCode == "0")
                                {
                                    result.Url = values["Data"].ToString();
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                AceWinLoginResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            AceWinLoginResult result = new();
                            result.ErrorCode = "9998";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                AceWinLoginResult result = new();
                result.ErrorCode = "9998";
                result.Message = "post connection status error";
                return result;
            }
            catch (Exception ex)
            {
                AceWinLoginResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary>充值至JILI</summary>
        public static async Task<AceWinTransferResult> TransferInPlayerBalance(string player_name, double amount, string tref)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    //string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiRoute}ExchangeTransferByAgentId";

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    string Params = "Account=" + player_name + "&TransactionId=" + tref + "&Amount=" + amount.ToString() + "&TransferType=2" + "&AgentId=" + AgentID;
                    PostContent.Add("Account", player_name);
                    PostContent.Add("TransactionId", tref);
                    PostContent.Add("Amount", amount.ToString());
                    PostContent.Add("TransferType", "2");
                    PostContent.Add("AgentId", AgentID);
                    PostContent.Add("Key", GetKey(Params));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    //using (HttpClient client2 = new HttpClient())
                    {
                        //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpResponseMessage response = await _Client.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinTransferResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<AceWinTransferData>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                //Console.WriteLine("TransferInPlayerBalance Response ERROR!!");

                                AceWinTransferResult result = new AceWinTransferResult();
                                result.ErrorCode = "9988";
                                result.Message = "post connection status error";
                                return result;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("TransferInPlayerBalance ERROR 無回應!");

                            AceWinTransferResult result = new AceWinTransferResult();
                            result.ErrorCode = "9988";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TransferInPlayerBalance ERROR Exception : " + ex);

                AceWinTransferResult result = new AceWinTransferResult();
                result.ErrorCode = "9989";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary>從JILI轉出充</summary>
        public static async Task<AceWinTransferResult> TransferOutPlayerBalance(string player_name, double amount, string tref)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    //string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiRoute}ExchangeTransferByAgentId";

                    string trType = amount == 0 ? "1" : "3";  // 1: 全部轉出, 3: 指定金額轉出

                    string Params = "Account=" + player_name + "&TransactionId=" + tref + "&Amount=" + amount.ToString() + "&TransferType=" + trType + "&AgentId=" + AgentID;

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    PostContent.Add("Account", player_name);
                    PostContent.Add("TransactionId", tref);
                    PostContent.Add("Amount", amount.ToString());
                    PostContent.Add("TransferType", trType);
                    PostContent.Add("AgentId", AgentID);
                    PostContent.Add("Key", GetKey(Params));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    //using (HttpClient client2 = new HttpClient())
                    {
                        //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpResponseMessage response = await _Client.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinTransferResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<AceWinTransferData>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                //Console.WriteLine("TransferInPlayerBalance Response ERROR!!");

                                AceWinTransferResult result = new AceWinTransferResult();
                                result.ErrorCode = "9988";
                                result.Message = "post connection status error";
                                return result;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("TransferInPlayerBalance ERROR 無回應!");

                            AceWinTransferResult result = new AceWinTransferResult();
                            result.ErrorCode = "9988";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TransferInPlayerBalance ERROR Exception : " + ex);

                AceWinTransferResult result = new AceWinTransferResult();
                result.ErrorCode = "9989";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<AceWinMemberInfoResult> CheckPlayerBalance(string player_name)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string ApiUrl = $"{ApiRoute}GetMemberInfo";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    //using (HttpClient client2 = new HttpClient())
                    {
                        //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "Accounts=" + player_name + "&AgentId=" + AgentID;
                        PostContent.Add("Accounts", player_name);
                        PostContent.Add("AgentId", AgentID);
                        PostContent.Add("Key", GetKey(Params));

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = await _Client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinMemberInfoResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                if (values["Data"] == null)
                                {
                                    result.Data.Add(new AceWinMemberInfoData()); //給個0值, 不要無資料返回 或 返回null
                                }
                                else
                                {
                                    result.Data = JsonSerializer.Deserialize<List<AceWinMemberInfoData>>(values["Data"].ToString());
                                    //Console.WriteLine("AW.Data = " + values["Data"].ToString());
                                    //if (AceWinCurrency > 1)  //AceWin傳回的已經是"貨幣", 不須轉換
                                    //{
                                    //    double point = Math.Round(result.Data[0].Balance / AceWinCurrency, 8); //查詢為"遊戲幣", 故須轉為"貨幣"
                                    //    result.Data[0].Balance = point;
                                    //}
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                AceWinMemberInfoResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            AceWinMemberInfoResult result = new();

                            result.ErrorCode = "9998";

                            result.Message = "post connection status error";

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                AceWinMemberInfoResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                AceWinMemberInfoResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<AceWinResult> KickMember(string Account)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string ApiUrl = $"{ApiRoute}KickMember";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    //using (HttpClient client2 = new HttpClient())
                    {
                        //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "Account=" + Account + "&AgentId=" + AgentID;
                        PostContent.Add("Account", Account);
                        PostContent.Add("AgentId", AgentID);
                        PostContent.Add("Key", GetKey(Params));

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = await _Client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinResult result = new();
                                result.ErrorCode = values["ErrorCode"].ToString();
                                result.Message = values["Message"].ToString();
                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                AceWinResult result = new();
                                result.ErrorCode = "9998";
                                result.Message = "post connection status error";
                                return result;
                            }
                        }
                        else
                        {
                            AceWinResult result = new();
                            result.ErrorCode = "9998";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                AceWinResult result = new();
                result.ErrorCode = "9998";
                result.Message = "post connection status error";
                return result;
            }
            catch (Exception ex)
            {
                AceWinResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        public static async Task<AceWinBetRecordResult> GetBetRecord(DateTime startT, DateTime endT, int pageN, int pageLimit)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiRoute}GetBetRecordByTime";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    using (HttpClient client2 = new HttpClient())
                    {
                        client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "StartTime=" + startT.ToString("yyy-MM-ddTHH:mm:ss") + "&EndTime=" +
                            endT.ToString("yyy-MM-ddTHH:mm:ss") + "&Page=" + pageN.ToString() + "&PageLimit=" +
                            pageLimit.ToString() + "&AgentId=" + AgentID;
                        PostContent.Add("StartTime", startT.ToString("yyy-MM-ddTHH:mm:ss"));
                        PostContent.Add("EndTime", endT.ToString("yyy-MM-ddTHH:mm:ss"));
                        PostContent.Add("Page", pageN.ToString());
                        PostContent.Add("PageLimit", pageLimit.ToString());
                        PostContent.Add("AgentId", AgentID);
                        PostContent.Add("Key", GetKey(Params));

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = client2.Send(request);
                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinBetRecordResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<AceWinBetRecordInfo>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                AceWinBetRecordResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            AceWinBetRecordResult result = new();

                            result.ErrorCode = "9998";

                            result.Message = "post connection status error";

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                AceWinBetRecordResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                AceWinBetRecordResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        public static async Task<AceWinDetailUrlResult> GetBetDetailUrl(long WagersId,string GameId)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiRoute}GetGameDetailUrl";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    using (HttpClient client2 = new HttpClient())
                    {
                        client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "WagersId=" + WagersId.ToString() + "&AgentId=" + AgentID;
                        PostContent.Add("WagersId", WagersId.ToString());
                        PostContent.Add("GameId", GameId);
                        PostContent.Add("AgentId", AgentID);
                        PostContent.Add("Key", GetKey(Params));
                       

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = client2.Send(request);
                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                AceWinDetailUrlResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<AceWinDetailUrlInfo>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                AceWinDetailUrlResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            AceWinDetailUrlResult result = new();

                            result.ErrorCode = "9998";

                            result.Message = "post connection status error";

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                AceWinDetailUrlResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                AceWinDetailUrlResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }


        //public static async Task<AceWinDetailUrlResult> GetBetDetailUrl(long WagersId)
        //{
        //    try
        //    {
        //        using (HttpClientHandler handler = new HttpClientHandler())
        //        {
        //            string g = Guid.NewGuid().ToString();

        //            string ApiUrl = $"{ApiRoute}GetGameDetailUrl";

        //            HttpResponseMessage response = null;

        //            var FullApiUrl = $"{ApiUrl}";

        //            using (HttpClient client2 = new HttpClient())
        //            {
        //                client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
        //                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

        //                Dictionary<string, string> PostContent = new Dictionary<string, string>();
        //                string Params = "WagersId=" + WagersId.ToString() + "&AgentId=" + AgentID;
        //                PostContent.Add("WagersId", WagersId.ToString());
        //                PostContent.Add("GameId", "5274");
        //                PostContent.Add("AgentId", AgentID);
        //                PostContent.Add("Key", GetKey(Params));

        //                request.Content = new FormUrlEncodedContent(PostContent);
        //                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        //                response = client2.Send(request);
        //                response.EnsureSuccessStatusCode();
        //                string responseBody = await response.Content.ReadAsStringAsync();

        //                if (response != null)
        //                {
        //                    if (response.IsSuccessStatusCode == true)
        //                    {
        //                        // 取得呼叫完成 API 後的回報內容
        //                        String strResult = await response.Content.ReadAsStringAsync();
        //                        var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

        //                        AceWinDetailUrlResult result = new();

        //                        result.ErrorCode = values["ErrorCode"].ToString();

        //                        result.Message = values["Message"].ToString();

        //                        result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<AceWinDetailUrlInfo>(values["Data"].ToString());

        //                        return result;
        //                    }
        //                    else
        //                    {
        //                        String strResult = await response.Content.ReadAsStringAsync();

        //                        AceWinDetailUrlResult result = new();

        //                        result.ErrorCode = "9998";

        //                        result.Message = "post connection status error";

        //                        return result;
        //                    }
        //                }
        //                else
        //                {
        //                    AceWinDetailUrlResult result = new();

        //                    result.ErrorCode = "9998";

        //                    result.Message = "post connection status error";

        //                    return result;
        //                }
        //            }
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        AceWinDetailUrlResult result = new();
        //        result.ErrorCode = "1";
        //        result.Message = "player not found";
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        AceWinDetailUrlResult result = new();
        //        result.ErrorCode = "9999";
        //        result.Message = ex.Message;
        //        return result;
        //    }
        //}
    }
}
