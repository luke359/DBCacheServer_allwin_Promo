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
using System.Net.Sockets;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Xml.Linq;


namespace VnpayAPI
{
    public class CPLoginResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url;

        public CPLoginResult()
        {
            ErrorCode = "";
            Message = "";
            Url = "";
        }

        public CPLoginResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public class CPResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public CPResult()
        {
            ErrorCode = "";
            Message = "";
            Data = "";
        }

        public CPResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public class CPTransferResult
    {
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
        public CPMemberInfoData Data { get; set; }

        public CPTransferResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new CPMemberInfoData();
        }

        public CPTransferResult(string errCode, string errMsg)
        {
            SetError(errCode, errMsg);
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class CPMemberInfoData
    {
        public string Account { get; set; }
        public double Amount { get; set; }

        public CPMemberInfoData()
        {
            Account = "";
            Amount = 0;
        }
    }

    public class CPextend
    {
        public string act_id { get; set; }
        public int act_type { get; set; }
        public string free_bet_amount { get; set; }
        public int is_free { get; set; }
        public int jackpot { get; set; }
        public string invite_code { get; set; }

        public CPextend()
        {
            act_id = "";
            act_type = 0;
            free_bet_amount = "";
            is_free = 0;
            jackpot = 0;
            invite_code = "";
        }
    }

    public class CPBetInfo
    {
        public long id { get; set; }
        public string uid { get; set; }
        public string sub_uid { get; set; }
        public string bet_id { get; set; }
        public string transaction_id { get; set; }
        public string round_id { get; set; }
        public int currency { get; set; }
        public string game_id { get; set; }
        public string bet_amount { get; set; }
        public string win_amount { get; set; }
        public string transfer_amount { get; set; }
        public int settlement_status { get; set; }
        public int is_settled { get; set; }
        public string parent_bet_id { get; set; }
        public string bet_time { get; set; }
        public string settle_time { get; set; }
        public int is_free { get; set; }
        //public string extend { get; set; }

        public CPBetInfo()
        {
            id = 0;
            uid = "";
            sub_uid = "";
            bet_id = "";
            transaction_id = "";
            round_id = "";
            currency = 0;
            game_id = "";
            bet_amount = "";
            win_amount = "";
            transfer_amount = "";
            settlement_status = 0;
            is_settled = 0;
            parent_bet_id = "";
            bet_time = "";
            settle_time = "";
            is_free = 0;
            //extend = "";

        }
    }

    public class CPBetData
    {
        public int start_time { get; set; }
        public int end_time { get; set; }
        public List<CPBetInfo> list { get; set; }

        public CPBetData()
        {
            start_time = 0;
            end_time = 0;
            list = new List<CPBetInfo>();
        }
    }

    public class CPGetBetRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public CPBetData data { get; set; }

        public CPGetBetRecordResult()
        {
            ErrorCode = "";
            Message = "";
            data = new CPBetData();
        }

        public CPGetBetRecordResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    public class CP
    {
        //正式
        static readonly string AppID = "bwg6zfpng";
        static readonly string SecretKey = "cYst5WKDYlsvcr";
        static readonly string ApiRoute = "https://egc-api.api/"; //{api_domain}/api/

        /// <summary>CP轉帳幣比</summary>
        const int CPCurrency = 100;

        static bool DebugFg = false;

        // 靜態 HttpClient 實例
        //private static readonly HttpClient _Client = new HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(15)
        //};
        // 從 HttpClientProvider 取得共用的 HttpClient 實例
        private static HttpClient _Client = HttpClientProvider.Client;

        /// <summary>創建玩家</summary>
        public static async Task<CPResult> CreateMember(string User)
        {
            try
            {
                string ApiUrl = $"{ApiRoute}login"; //登录创角

                HttpResponseMessage response = null;

                //using (HttpClient client2 = new HttpClient())
                {
                    //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string time = GetTimeStamp();

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    //Params須按照参数名ASCII字典序排序
                    string Params = "appid=" + AppID +
                                    "&game_key=hog" +
                                    "&sub_uid=" + User +
                                    "&time=" + time;

                    PostContent.Add("appid", AppID);
                    PostContent.Add("game_key", "hog");
                    PostContent.Add("sub_uid", User);
                    PostContent.Add("time", time);

                    PostContent.Add("token", GetToken(Params));

                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    response = await _Client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            CPResult result = new()
                            {
                                ErrorCode = values["code"].ToString(),
                                Message = values["msg"].ToString()
                            };

                            //if (result.ErrorCode == "0")

                            return result;
                        }
                        else
                        {
                            CPResult result = new("9996", "post connection status error");
                            return result;
                        }
                    }
                    else
                    {
                        CPResult result = new("9997", "post connection status error");
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                CPResult result = new("9998", "post connection status error");
                return result;
            }
            catch (Exception ex)
            {
                CPResult result = new("9999", ex.Message);
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<CPResult> KickMember(string User)
        {
            try
            {
                string ApiUrl = $"{ApiRoute}offline"; //踢出在线玩家

                HttpResponseMessage response = null;

                //using (HttpClient client2 = new HttpClient())
                {
                    //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string time = GetTimeStamp();

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    //Params須按照参数名ASCII字典序排序
                    string Params = "appid=" + AppID +
                                    "&game_key=hog" +
                                    "&sub_uid=" + User +
                                    "&time=" + time;

                    PostContent.Add("appid", AppID);
                    PostContent.Add("game_key", "hog");
                    PostContent.Add("sub_uid", User);
                    PostContent.Add("time", time);

                    PostContent.Add("token", GetToken(Params));

                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    response = await _Client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            CPResult result = new()
                            {
                                ErrorCode = values["code"].ToString(),
                                Message = values["msg"].ToString()
                            };

                            //if (result.ErrorCode == "0")

                            return result;
                        }
                        else
                        {
                            CPResult result = new("9996", "post connection status error");
                            return result;
                        }
                    }
                    else
                    {
                        CPResult result = new("9997", "post connection status error");
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                CPResult result = new("9998", "post connection status error");
                return result;
            }
            catch (Exception ex)
            {
                CPResult result = new("9999", ex.Message);
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<CPLoginResult> GetGameUrl(string User, string GameId, string Lang)
        {
            try
            {
                string ApiUrl = $"{ApiRoute}get_game_url"; //获取进游戏url

                HttpResponseMessage response = null;

                //using (HttpClient client2 = new HttpClient())
                {
                    //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string time = GetTimeStamp();

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    //Params須按照参数名ASCII字典序排序
                    string Params = "appid=" + AppID +
                                    "&game_id=" + GameId +
                                    "&game_key=hog" +
                                    "&lang=" + Lang +
                                    "&sub_uid=" + User +
                                    "&time=" + time;

                    PostContent.Add("appid", AppID);
                    PostContent.Add("game_key", "hog");
                    PostContent.Add("sub_uid", User);
                    PostContent.Add("time", time);
                    PostContent.Add("game_id", GameId);
                    PostContent.Add("lang", Lang);

                    PostContent.Add("token", GetToken(Params));

                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    response = await _Client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            CPLoginResult result = new()
                            {
                                ErrorCode = values["code"].ToString(),
                                Message = values["msg"].ToString()
                            };

                            if (result.ErrorCode == "0")
                            {
                                result.Url = values["data"].ToString();
                            }

                            return result;
                        }
                        else
                        {
                            CPLoginResult result = new("9996", "post connection status error");
                            return result;
                        }
                    }
                    else
                    {
                        CPLoginResult result = new("9997", "post connection status error");
                        return result;
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                CPLoginResult result = new("9998", "post connection status error");
                return result;
            }
            catch (Exception ex)
            {
                CPLoginResult result = new("9999", ex.Message);
                return result;
            }
        }

        /// <summary>充值至CP</summary>
        public static async Task<CPTransferResult> TransferInPlayerBalance(string User, double amount, string tref)
        {
            try
            {
                string ApiUrl = $"{ApiRoute}gold_up";

                HttpResponseMessage response = null;

                //using (HttpClient client2 = new HttpClient())
                {
                    //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string time = GetTimeStamp();

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    //Params須按照参数名ASCII字典序排序
                    string Params = "appid=" + AppID +
                                    "&game_key=hog" +
                                    "&gold=" + amount +   //Double(10,2)
                                    "&order_no=" + tref +
                                    "&sub_uid=" + User +
                                    "&time=" + time;

                    PostContent.Add("appid", AppID);
                    PostContent.Add("game_key", "hog");
                    PostContent.Add("time", time);
                    PostContent.Add("sub_uid", User);
                    PostContent.Add("gold", amount.ToString());
                    PostContent.Add("order_no", tref);

                    PostContent.Add("token", GetToken(Params));

                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    response = await _Client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            CPTransferResult result = new()
                            {
                                ErrorCode = values["code"].ToString(),
                                Message = values["msg"].ToString()
                            };

                            if (result.ErrorCode == "0")
                            {
                                result.Data.Account = User;
                                result.Data.Amount = amount; //上分金额
                                result.Message = "Success";
                            }

                            return result;
                        }
                        else
                        {
                            CPTransferResult result = new("9996", "post connection status error");
                            return result;
                        }
                    }
                    else
                    {
                        CPTransferResult result = new("9997", "post connection status error");
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                CPTransferResult result = new("9998", "post connection status error");
                return result;
            }
            catch (Exception ex)
            {
                CPTransferResult result = new("9999", ex.Message);
                return result;
            }
        }

        /// <summary>從CP轉出充</summary>
        public static async Task<CPTransferResult> TransferOutPlayerBalance(string User, double amount, string tref)
        {
            if (amount < 0)
            {
                return new CPTransferResult("0", "Amount Error");
            }

            if (amount == 0)
            {
                //0值 = 提領全部, 先查詢餘額
                var result2 = await CheckPlayerBalance(User);
                if (result2.ErrorCode != "0")
                {
                    return new CPTransferResult(result2.ErrorCode, result2.Message);
                }
                amount = result2.Data.Amount;
                if (DebugFg) Console.WriteLine($"CP {User} 查餘額={amount}");

                if (amount == 0)
                {
                    return new CPTransferResult("0", "Amount Zero");
                }
            }

            try
            {
                string ApiUrl = $"{ApiRoute}gold_down";

                HttpResponseMessage response = null;

                //using (HttpClient client2 = new HttpClient())
                {
                    //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string time = GetTimeStamp();

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    //Params須按照参数名ASCII字典序排序
                    string Params = "appid=" + AppID +
                                    "&game_key=hog" +
                                    "&gold=" + amount +   //Double(10,2)
                                    "&order_no=" + tref +
                                    "&sub_uid=" + User +
                                    "&time=" + time;

                    PostContent.Add("appid", AppID);
                    PostContent.Add("game_key", "hog");
                    PostContent.Add("time", time);
                    PostContent.Add("sub_uid", User);
                    PostContent.Add("gold", amount.ToString());
                    PostContent.Add("order_no", tref);

                    PostContent.Add("token", GetToken(Params));

                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    response = await _Client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            CPTransferResult result = new()
                            {
                                ErrorCode = values["code"].ToString(),
                                Message = values["msg"].ToString()
                            };

                            if (result.ErrorCode == "0")
                            {
                                var jele = (JsonElement)values["data"];
                                result.Data.Account = jele.GetProperty("sub_uid").GetString();
                                result.Data.Amount = double.Parse(jele.GetProperty("gold").GetString()); //下分金额
                                result.Message = "Success";
                            }

                            return result;
                        }
                        else
                        {
                            CPTransferResult result = new("9996", "post connection status error");
                            return result;
                        }
                    }
                    else
                    {
                        CPTransferResult result = new("9997", "post connection status error");
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                CPTransferResult result = new("9998", "post connection status error");
                return result;
            }
            catch (Exception ex)
            {
                CPTransferResult result = new("9999", ex.Message);
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<CPTransferResult> CheckPlayerBalance(string User)
        {
            try
            {
                string ApiUrl = $"{ApiRoute}user_gold";

                HttpResponseMessage response = null;

                //using (HttpClient client2 = new HttpClient())
                {
                    //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string time = GetTimeStamp();

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    //Params須按照参数名ASCII字典序排序
                    string Params = "appid=" + AppID +
                                    "&game_key=hog" +
                                    "&sub_uid=" + User +
                                    "&time=" + time;

                    PostContent.Add("appid", AppID);
                    PostContent.Add("game_key", "hog");
                    PostContent.Add("sub_uid", User);
                    PostContent.Add("time", time);

                    PostContent.Add("token", GetToken(Params));

                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    response = await _Client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            CPTransferResult result = new()
                            {
                                ErrorCode = values["code"].ToString(),
                                Message = values["msg"].ToString()
                            };

                            if (result.ErrorCode == "0")
                            {
                                var jele = (JsonElement)values["data"];
                                result.Data.Account = jele.GetProperty("sub_uid").GetString();
                                result.Data.Amount = double.Parse(jele.GetProperty("gold").GetString());
                            }
                            //else if (result.ErrorCode == "-1")
                            //{
                            //    result.ErrorCode = "0"; //玩家帳號不存在, 視為"查詢成功"
                            //}

                            return result;
                        }
                        else
                        {
                            CPTransferResult result = new("9996", "post connection status error");
                            return result;
                        }
                    }
                    else
                    {
                        CPTransferResult result = new("9997", "post connection status error");
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                CPTransferResult result = new("9998", "post connection status error");
                return result;
            }
            catch (Exception ex)
            {
                CPTransferResult result = new("9999", ex.Message);
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<CPGetBetRecordResult> GetBetRecord(string start_time, string end_time)
        {
            try
            {
                string ApiUrl = $"{ApiRoute}get_order_log";

                HttpResponseMessage response = null;

                //using (HttpClient client2 = new HttpClient())
                {
                    //client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string time = GetTimeStamp();

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    //Params須按照参数名ASCII字典序排序
                    string Params = "appid=" + AppID +
                                    "&end_time=" + end_time +
                                    "&game_key=hog" +
                                    "&query_type=2" +
                                    "&start_time=" + start_time +
                                    "&time="+ time;

                    PostContent.Add("appid", AppID);
                    PostContent.Add("game_key", "hog");
                    PostContent.Add("start_time", start_time);
                    PostContent.Add("end_time", end_time);
                    PostContent.Add("query_type", "2");
                    PostContent.Add("time", time);
                    PostContent.Add("token", GetToken(Params));

                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    response = await _Client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            // 取得呼叫完成 API 後的回報內容
                            String strResult = await response.Content.ReadAsStringAsync();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            CPGetBetRecordResult result = new()
                            {
                                ErrorCode = values["code"].ToString(),
                                Message = values["msg"].ToString()
                            };

                            if (result.ErrorCode == "0")
                            {
                                result.data = JsonSerializer.Deserialize<CPBetData>(values["data"].ToString());
                            }
 
                            return result;
                        }
                        else
                        {
                            CPGetBetRecordResult result = new("9996", "post connection status error");
                            return result;
                        }
                    }
                    else
                    {
                        CPGetBetRecordResult result = new("9997", "post connection status error");
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                CPGetBetRecordResult result = new("9998", "post connection status error");
                return result;
            }
            catch (Exception ex)
            {
                CPGetBetRecordResult result = new("9999", ex.Message);
                return result;
            }
        }

        /// <summary>取得時間戳</summary>
        static string GetTimeStamp()
        {
            DateTimeOffset nowUtc = DateTimeOffset.UtcNow; // 取得目前 UTC 時間
            DateTimeOffset epochStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero); // Unix Epoch 的起始時間
            string unixTimestamp = ((long)(nowUtc - epochStart).TotalSeconds).ToString();
            return unixTimestamp;
        }

        static string GetTimeStamp(string t)
        {
            DateTime temp = Convert.ToDateTime(t);
            DateTimeOffset nowUtc = DateTime.SpecifyKind(temp, DateTimeKind.Utc); // 取得目前 UTC 時間
            DateTimeOffset epochStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero); // Unix Epoch 的起始時間
            string unixTimestamp = ((long)(nowUtc - epochStart).TotalSeconds).ToString();
            return unixTimestamp;
        }

        /// <summary>CP Api Token計算</summary>
        static string GetToken(string Params)
        {
            //加入 SecretKey 後再計算
            Params = Params + "&secret=" + SecretKey;

            // 1. 計算輸入字串的 MD5 哈希值
            string md5Hash = CalculateMD5(Params);

            // 2. 計算 MD5 哈希值的 SHA1 哈希值
            string sha1Hash = CalculateSHA1(md5Hash).ToUpper();

            //Console.WriteLine("token=" + sha1Hash);

            return sha1Hash;
        }

        static string CalculateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 转换为 16進制 字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" 格式化為兩位十六進制小寫字母
                }
                return sb.ToString();
            }
        }

        static string CalculateSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                // 转换为 16進制 字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" 格式化為兩位十六進制小寫字母
                }
                return sb.ToString();
            }
        }
    }
}
