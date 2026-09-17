using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace VnpayAPI
{
    public class JILILoginResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url;

        public JILILoginResult()
        {
            ErrorCode = "";
            Message = "";
            Url = "";
        }
    }

    public class JILIResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public JILIResult()
        {
            ErrorCode = "";
            Message = "";
            Data = "";
        }
    }

    public class JILITransferData
    {
        public string TransactionId { get; set; }
        public double CoinBefore { get; set; }
        public double CoinAfter { get; set; }
        public double CurrencyBefore { get; set; }
        public double CurrencyAfter { get; set; }
        public int Status { get; set; }

        public JILITransferData()
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

    public class JILITransferResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public JILITransferData Data { get; set; }

        public JILITransferResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new JILITransferData();
        }
    }

    public class JILIMemberInfoData
    {
        public string Account { get; set; }
        public double Balance { get; set; }
        public int Status { get; set; }

        public JILIMemberInfoData()
        {
            Account = "";
            Balance = 0;
            Status = 0;
        }
    }

    public class JILIMemberInfoResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<JILIMemberInfoData> Data { get; set; }

        public JILIMemberInfoResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new List<JILIMemberInfoData>();
        }
    }

    public class JILIBetRecordData
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
        public long RoundIndex { get; set; }

        public JILIBetRecordData()
        {
            Account = "";
            WagersId = 0;
            GameId = 0;
            WagersTime = new DateTime(1984,12,11);
            BetAmount = 0;
            PayoffTime = new DateTime(1984, 12, 11);
            PayoffAmount = 0;
            Status = 0;
            SettlementTime = new DateTime(1984, 12, 11);
            GameCategoryId = 0;
            Type = 0;
            AgentId = "";
            Turnover = 0;
            RoundIndex = 0;
        }
    }

    public class JILIBetRecordPagination
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageLimit { get; set; }
        public int TotalNumber { get; set; }

        public JILIBetRecordPagination()
        {
            CurrentPage = 0;
            TotalPages = 0;
            PageLimit = 0;
            TotalNumber = 0;
        }
    }


    public class JILIBetRecordInfo
    {
        public List<JILIBetRecordData> Result { get; set; }
        public JILIBetRecordPagination Pagination { get; set; }

        public JILIBetRecordInfo()
        {
            Result = new List<JILIBetRecordData>();
            Pagination = new JILIBetRecordPagination();
        }
    }

    public class JILIBetRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public JILIBetRecordInfo Data { get; set; }

        public JILIBetRecordResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new JILIBetRecordInfo();
        }
    }

    public class JILIDetailUrlInfo
    {
        public string Url { get; set; }

        public JILIDetailUrlInfo()
        {
            Url = "";
        }
    }

    public class JILIDetailUrlResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public JILIDetailUrlInfo Data { get; set; }

        public JILIDetailUrlResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new JILIDetailUrlInfo();
        }
    }

    public class JILIActivityRecordData
    {
        public string Account { get; set; }
        public int GameId { get; set; }
        public long WagersId { get; set; }
        public decimal BetAmount { get; set; }
        public decimal PayoffAmount { get; set; }
        public DateTime WagersTime { get; set; }
        public string Currency { get; set; }
        public string ReferenceId { get; set; }

        public JILIActivityRecordData()
        {
            Account = "";
            GameId = 0;
            WagersId = 0;
            BetAmount = 0;
            PayoffAmount = 0;
            WagersTime = new DateTime(1984, 12, 11);
            Currency = "";
            ReferenceId = "";
        }
    }

    public class JILIActivityRecordInfo
    {
        public List<JILIActivityRecordData> Result { get; set; }
        public JILIBetRecordPagination Pagination { get; set; }

        public JILIActivityRecordInfo()
        {
            Result = new List<JILIActivityRecordData>();
            Pagination = new JILIBetRecordPagination();
        }
    }

    public class JILIActivityRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public JILIActivityRecordInfo Data { get; set; }

        public JILIActivityRecordResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new JILIActivityRecordInfo();
        }
    }

    public class JILI
    {
        static readonly string AgentID = "T_ALBBJL_YR";


        /// <summary>JILI轉帳幣比</summary>
        const int JILICurrency = 100;

        //Production：
        static readonly string AgentKey = "fe5e9d64ef6958cf";
        static readonly string ApiRoute = "https://wb-api-2.com/api1/";

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

        public static async Task<JILIResult> CreateMember(string Account)
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
                        //client.Timeout = TimeSpan.FromSeconds(ApiTimeout);

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

                                JILIResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                JILIResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            JILIResult result = new();
                            result.ErrorCode = "9998";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                JILIResult result = new();
                result.ErrorCode = "9998";
                result.Message = "post connection status error";
                return result;
            }
            catch (Exception ex)
            {
                JILIResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        public static async Task<JILILoginResult> Login(string User, string GameId, string Lang, string homeurl)
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
                        //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "Account=" + User + "&GameId=" + GameId + "&Lang=" + Lang + "&AgentId=" + AgentID;
                        PostContent.Add("Account", User);
                        PostContent.Add("GameId", GameId);
                        PostContent.Add("Lang", Lang);
                        if (homeurl != "")
                        {
                            PostContent.Add("HomeUrl", homeurl);
                        }
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

                                JILILoginResult result = new();

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

                                JILILoginResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            JILILoginResult result = new();
                            result.ErrorCode = "9998";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                JILILoginResult result = new();
                result.ErrorCode = "9998";
                result.Message = "post connection status error";
                return result;
            }
            catch (Exception ex)
            {
                JILILoginResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        public static async Task<JILILoginResult> GetGameList()
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string ApiUrl = $"{ApiRoute}GetGameList";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    using (HttpClient client2 = new HttpClient())
                    {
                        client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        string Params = "AgentId=" + AgentID;
                        PostContent.Add("AgentId", AgentID);
                        PostContent.Add("Key", GetKey(Params));

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = await client2.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                var values = JsonSerializer.Deserialize<JILIResult>(strResult);

                                JILILoginResult result = new();

                                

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                JILILoginResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            JILILoginResult result = new();
                            result.ErrorCode = "9998";
                            result.Message = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                JILILoginResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                JILILoginResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary>充值至JILI</summary>
        public static async Task<JILITransferResult> TransferInPlayerBalance(string player_name, double amount, string tref)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

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

                                JILITransferResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data= (values["Data"] == null) ? null : JsonSerializer.Deserialize<JILITransferData>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                //Console.WriteLine("TransferInPlayerBalance Response ERROR!!");

                                JILITransferResult result = new JILITransferResult();
                                result.ErrorCode = "9988";
                                result.Message = "post connection status error";
                                return result;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("TransferInPlayerBalance ERROR 無回應!");

                            JILITransferResult result = new JILITransferResult();
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

                JILITransferResult result = new JILITransferResult();
                result.ErrorCode = "9989";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary>從JILI轉出充</summary>
        public static async Task<JILITransferResult> TransferOutPlayerBalance(string player_name, double amount, string tref)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

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

                                JILITransferResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<JILITransferData>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                //Console.WriteLine("TransferInPlayerBalance Response ERROR!!");

                                JILITransferResult result = new JILITransferResult();
                                result.ErrorCode = "9988";
                                result.Message = "post connection status error";
                                return result;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("TransferInPlayerBalance ERROR 無回應!");

                            JILITransferResult result = new JILITransferResult();
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

                JILITransferResult result = new JILITransferResult();
                result.ErrorCode = "9989";
                result.Message = ex.Message;
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<JILIMemberInfoResult> CheckPlayerBalance(string player_name)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    //var jsonString = JsonSerializer.Serialize(CheckParameter);

                    string g = Guid.NewGuid().ToString();

                    //string ApiUrl = $"https://m.pgr-asni1z.com/external/Cash/v3/GetPlayerWallet?trace_id="+ g;
                    string ApiUrl = $"{ApiRoute}GetMemberInfo";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    //using (HttpClient client2 = new HttpClient())
                    {
                        //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        //string context = "";
                        //
                        //int i = 0;
                        //
                        //foreach (var keyValuePair in PostContent)
                        //{
                        //    if (i != 0) context = context + "&";
                        //
                        //    context = context + keyValuePair.Key + "=" + keyValuePair.Value;
                        //
                        //    i++;
                        //}
                        //request.Content = new StringContent(context);

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

                                JILIMemberInfoResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                //result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<List<JILIMemberInfoData>>(values["Data"].ToString());

                                if (values["Data"] == null)
                                {
                                    result.Data.Add(new JILIMemberInfoData()); //給個0值, 不要無資料返回 或 返回null
                                }
                                else
                                {
                                    result.Data = JsonSerializer.Deserialize<List<JILIMemberInfoData>>(values["Data"].ToString());

                                    if (JILICurrency > 1)
                                    {
                                        double jPoint = Math.Round(result.Data[0].Balance / JILICurrency, 8); //查詢為"遊戲幣", 故須轉為"貨幣"
                                        result.Data[0].Balance = jPoint;
                                    }
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                JILIMemberInfoResult result = new();

                                result.ErrorCode = "9998";
                                result.Message = "post connection status error";
                                result.Data.Add(new JILIMemberInfoData()); //給個0值, 不要無資料返回
                                return result;
                            }
                        }
                        else
                        {
                            JILIMemberInfoResult result = new();

                            result.ErrorCode = "9998";
                            result.Message = "post connection status error";
                            result.Data.Add(new JILIMemberInfoData()); //給個0值, 不要無資料返回
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                JILIMemberInfoResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                result.Data.Add(new JILIMemberInfoData()); //給個0值, 不要無資料返回
                return result;
            }
            catch (Exception ex)
            {
                JILIMemberInfoResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                result.Data.Add(new JILIMemberInfoData()); //給個0值, 不要無資料返回
                return result;
            }
        }

        public static async Task<JILIMemberInfoResult> CheckPlayerBalance(string player_name, HttpClient client2)
        {
            try
            {
                string g = Guid.NewGuid().ToString();
                string ApiUrl = $"{ApiRoute}GetMemberInfo";
                var FullApiUrl = $"{ApiUrl}";

                client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                Dictionary<string, string> PostContent = new Dictionary<string, string>();
                string Params = "Accounts=" + player_name + "&AgentId=" + AgentID;
                PostContent.Add("Accounts", player_name);
                PostContent.Add("AgentId", AgentID);
                PostContent.Add("Key", GetKey(Params));

                request.Content = new FormUrlEncodedContent(PostContent);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = await client2.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (response != null)
                {
                    if (response.IsSuccessStatusCode == true)
                    {
                        // 取得呼叫完成 API 後的回報內容
                        String strResult = await response.Content.ReadAsStringAsync();

                        var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                        JILIMemberInfoResult result = new();

                        result.ErrorCode = values["ErrorCode"].ToString();

                        result.Message = values["Message"].ToString();

                        result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<List<JILIMemberInfoData>>(values["Data"].ToString());

                        return result;
                    }
                    else
                    {
                        JILIMemberInfoResult result = new();
                        result.ErrorCode = "9998";
                        result.Message = "post connection status error";
                        return result;
                    }
                }
                else
                {
                    JILIMemberInfoResult result = new();
                    result.ErrorCode = "9998";
                    result.Message = "post connection status error";
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                JILIMemberInfoResult result = new();
                result.ErrorCode = "9998";
                result.Message = "post connection status error";
                return result;
            }
            catch (Exception ex)
            {
                JILIMemberInfoResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        public static async Task<JILIBetRecordResult> GetBetRecord(DateTime startT, DateTime endT, int pageN, int pageLimit)
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

                                JILIBetRecordResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<JILIBetRecordInfo>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                JILIBetRecordResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            JILIBetRecordResult result = new();

                            result.ErrorCode = "9998";

                            result.Message = "post connection status error";

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                JILIBetRecordResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                JILIBetRecordResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }

        public static async Task<JILIActivityRecordResult> GetActivityRecord(DateTime startT, DateTime endT, int pageN, int pageLimit)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiRoute}GetFreeSpinRecordByTime";

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

                                JILIActivityRecordResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<JILIActivityRecordInfo>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                JILIActivityRecordResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            JILIActivityRecordResult result = new();

                            result.ErrorCode = "9998";

                            result.Message = "post connection status error";

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                JILIActivityRecordResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                JILIActivityRecordResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }


        public static async Task<JILIDetailUrlResult> GetBetDetailUrl(long WagersId)
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

                                JILIDetailUrlResult result = new();

                                result.ErrorCode = values["ErrorCode"].ToString();

                                result.Message = values["Message"].ToString();

                                result.Data = (values["Data"] == null) ? null : JsonSerializer.Deserialize<JILIDetailUrlInfo>(values["Data"].ToString());

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                JILIDetailUrlResult result = new();

                                result.ErrorCode = "9998";

                                result.Message = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            JILIDetailUrlResult result = new();

                            result.ErrorCode = "9998";

                            result.Message = "post connection status error";

                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                JILIDetailUrlResult result = new();
                result.ErrorCode = "1";
                result.Message = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                JILIDetailUrlResult result = new();
                result.ErrorCode = "9999";
                result.Message = ex.Message;
                return result;
            }
        }
    }
}
