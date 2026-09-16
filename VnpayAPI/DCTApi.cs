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
    public class DCTLoginResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url;

        public DCTLoginResult()
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

    public class DCTMemberInfoData
    {
        /// <summary>交易玩家帳號</summary>
        public string Account { get; set; }
        /// <summary>交易額</summary>
        public double Amount { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public double Balance { get; set; }
        /// <summary>交易結果代號</summary>
        public int Status { get; set; }

        public DCTMemberInfoData()
        {
            Account = "";
            Amount = 0;
            Balance = 0;
            Status = 0;
        }
    }

    public class DCTMemberInfoResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<DCTMemberInfoData> Data { get; set; }

        public DCTMemberInfoResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
            if(errorCode == "0")
            {
                Data = new List<DCTMemberInfoData>();
            }
        }

        public DCTMemberInfoResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new List<DCTMemberInfoData>();
        }

        public void SetResult(List<DCTMemberInfoData> data)
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

    public class DCTBetInfo
    {
        public string brand_uid { get; set; }
        public string currency { get; set; }
        public string wager_type { get; set; }
        public decimal amount { get; set; }
        public decimal before_amount { get; set; }
        public decimal after_amount { get; set; }
        public int game_id { get; set; }
        public string game_name { get; set; }
        public string round_id { get; set; }
        public string wager_id { get; set; }
        public decimal jackpot_contribution { get; set; }
        public decimal jackpot_win { get; set; }
        public string description { get; set; }
        public string create_time { get; set; }
        public string game_result { get; set; }
        public decimal tip { get; set; }
        public bool is_endround { get; set; }


        public DCTBetInfo()
        {
            brand_uid = "";
            currency = "";
            wager_type = "";
            amount = 0;
            before_amount = 0;
            after_amount = 0;
            game_id = 0;
            game_name = "";
            round_id = "";
            wager_id = "";
            jackpot_contribution = 0;
            jackpot_win = 0;
            description = "";
            create_time = "";
            game_result = "";
            tip = 0;
            is_endround = true;
        }
    }

    public class DCTPage
    {
        public int current_page { get; set; }
        public int total_count { get; set; }

        public DCTPage()
        {
            current_page = 0;
            total_count = 0;
        }
    }

    public class DCTGetBetRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<DCTBetInfo> data { get; set; }
        public DCTPage page { get; set; }

        public DCTGetBetRecordResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public DCTGetBetRecordResult()
        {
            ErrorCode = "";
            Message = "";
            data = new List<DCTBetInfo>();
            page = new DCTPage();
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }

    class ApiLoginResponse
    {
        public DataUrl? data { get; set; }
        public int code { get; set; }
        public string? msg { get; set; }
    }
    class DataUrl
    {
        public string game_url { get; set; }
    }

    class ApiBalanceResponse
    {
        //public JsonNode? data { get; set; }
        public DataBalance? data { get; set; }
        public int code { get; set; }
        public string? msg { get; set; }
    }
    class DataBalance
    {
        public decimal balance { get; set; }
    }


    public class DCT
    {
        //正式
        static string DCTApiUrl = "https://gaming.dcgames.asia";
        static string DCTgetBetDataUrl = "https://ticket.dcgames.asia";
        static string DCTApiKey = "3E1D31BB544D458FB8BA54A3211474CE";

        //測試
        //static string DCTApiUrl = "https://gaming.stagedc.net";
        //static string DCTgetBetDataUrl = "https://ticket.stagedc.net";
        //static string DCTApiKey = "105410A106544B8E9A68F432268F7557";

        static string DCTBrandId = "T022173"; //ALIBABA
        static string DCTCurrency = "MYR"; //幣別
        static string DCTCountry = "MY";  //國別
        static string DCTChannel = "mobile"; //"pc" or "mobile"

        /// <summary>DCT轉帳幣比</summary>
        const int TransCurrency = 100; //MYRR 100(DCT):1(我)

        /// <summary>Debug 訊息</summary>
        static bool DebugFg = false;


        /// <summary>取得遊戲 url</summary>
        public static async Task<DCTLoginResult> Login(string User, string GameId, string Lang, string homeurl)
        {
            string md5Para = DCTBrandId + User + DCTApiKey;

            string md5Sign = Md5Sign(md5Para).ToUpper();

            //Console.WriteLine($"md5Sign = {md5Sign}");

            //Lang = "zh_hans"; //UNDONE: TEST ONLY

            var data = new
            {
                brand_id = DCTBrandId,
                sign = md5Sign,
                brand_uid = User,
                game_id = GameId,
                currency = DCTCurrency,
                language = Lang,
                channel = DCTChannel,
                country_code = DCTCountry,
                return_url = homeurl,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "/dct/loginGame";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            DCTLoginResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<ApiLoginResponse>(strResult);

                    if (values.code == 1000)
                    {
                        result.SetResult(values.data.game_url);
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"DCT {User} Login ERROR ! Code={values.code}, Msg={values.msg}");
                        result.SetError(values.code.ToString(), values.msg);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"DCT {User} Login API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"DCT {User} Login API 無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>取得遊戲 url</summary>
        public static async Task<DCTLoginResult> KickMember(string User)
        {
            string md5Para = DCTBrandId + User + DCTApiKey;

            string md5Sign = Md5Sign(md5Para).ToUpper();

            //Console.WriteLine($"md5Sign = {md5Sign}");

            //Lang = "zh_hans"; //UNDONE: TEST ONLY

            var data = new
            {
                brand_id = DCTBrandId,
                sign = md5Sign,
                brand_uid = User,
                currency = DCTCurrency,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "/dct/kickPlayer";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            DCTLoginResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<ApiLoginResponse>(strResult);

                    if (values.code == 1000)
                    {
                        result.SetResult("");
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"DCT {User} KickMember ERROR ! Code={values.code}, Msg={values.msg}");
                        result.SetError(values.code.ToString(), values.msg);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"DCT {User} KickMember API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"DCT {User} KickMember API 無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>查玩家餘額</summary>
        public static async Task<DCTMemberInfoResult> CheckPlayerBalance(string User)
        {
            string md5Para = DCTBrandId + User + DCTApiKey;
            string md5Sign = Md5Sign(md5Para).ToUpper(); //组成后需转为全大写

            var data = new
            {
                brand_id = DCTBrandId,
                sign = md5Sign,
                brand_uid = User,
                currency = DCTCurrency,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "/dct/getBalance";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            DCTMemberInfoResult result = new();

            if (strResult != null)
            {
                //Console.WriteLine($"DCT查詢餘額 API : {strResult}"); //UNDONE: Test Only

                try
                {
                    var values = JsonSerializer.Deserialize<ApiBalanceResponse>(strResult);

                    if (values.code == 1000)
                    {
                        DCTMemberInfoData info = new();
                        info.Account = User;
                        info.Balance = (double)values.data.balance;
                        info.Status = 0;

                        result.SetResult(new List<DCTMemberInfoData> { info });
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"DCT {User} 查詢餘額 ERROR ! Code={values.code}, Msg={values.msg}");
                        result.SetError(values.code.ToString(), values.msg);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"DCT {User} 查詢餘額API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"DCT {User} 查詢餘額API無回應!");
                result.SetError("9999", "No Response");
            }

            return result;
        }

        /// <summary>充值至DCT</summary>
        public static async Task<DCTMemberInfoResult> TransferInPlayerBalance(string User, double amount, string tRefId)
        {
            if (amount < 0)
            {
                return new DCTMemberInfoResult("9997", "Amount Error");
            }

            //DCT充值時, 會主動建立玩家帳號, 因此不須 CreateMember Api
            string md5Para = DCTBrandId + User + DCTApiKey;
            string md5Sign = Md5Sign(md5Para).ToUpper();

            decimal tamount = (decimal)amount;

            var data = new
            {
                brand_id = DCTBrandId,
                sign = md5Sign,
                brand_uid = User,
                amount = tamount,
                bill_no = tRefId,
                currency = DCTCurrency,
                country_code = DCTCountry,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "/dct/credit";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            DCTMemberInfoResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<ApiBalanceResponse>(strResult);

                    if (values.code == 1000)
                    {
                        DCTMemberInfoData info = new();
                        info.Account = User;
                        info.Amount = amount;
                        info.Balance = (double)values.data.balance; //After Blance: 充值後的DCT錢包餘額
                        info.Status = 0;

                        result.SetResult(new List<DCTMemberInfoData> { info });
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"DCT {User} 錢包充值 ERROR ! Code={values.code}, Msg={values.msg}");
                        result.SetError(values.code.ToString(), values.msg);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"DCT {User} 錢包充值API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"DCT {User} 錢包充值API無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>從DCT轉出</summary>
        public static async Task<DCTMemberInfoResult> TransferOutPlayerBalance(string User, double amount, string tRefId, string AgName = "")
        {
            if (amount < 0)
            {
                return new DCTMemberInfoResult("9997", "Amount Error");
            }

            if (amount == 0)
            {
                //0值 = 提領全部, 先查詢餘額
                var result2 = await CheckPlayerBalance(User);
                if (result2.ErrorCode != "0")
                {
                    return new DCTMemberInfoResult(result2.ErrorCode, result2.Message);
                }
                amount = result2.Data[0].Balance;
                if (DebugFg) Console.WriteLine($"DCT {User} 查餘額={amount}");

                if (amount == 0)
                {
                    return new DCTMemberInfoResult("0", "Amount Zero");
                }
            }

            decimal tamount = (decimal)amount;

            string md5Para = DCTBrandId + User + DCTApiKey;
            string md5Sign = Md5Sign(md5Para).ToUpper();

            var data = new
            {
                brand_id = DCTBrandId,
                sign = md5Sign,
                brand_uid = User,
                amount = tamount,
                bill_no = tRefId,
                currency = DCTCurrency,
                country_code = DCTCountry,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "/dct/withdraw";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            DCTMemberInfoResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<ApiBalanceResponse>(strResult);

                    if (values.code == 1000)
                    {
                        DCTMemberInfoData info = new();
                        info.Account = User;
                        info.Amount = amount;
                        info.Balance = (double)values.data.balance; //After Blance: 取回後的DCT錢包餘額
                        info.Status = 0;

                        result.SetResult(new List<DCTMemberInfoData> { info });
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"DCT {User} 錢包取回 ERROR ! Code={values.code}, Msg={values.msg}");
                        result.SetError(values.code.ToString(), values.msg);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"DCT {User} 錢包取回API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"DCT {User} 錢包取回API無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>取得遊戲 url</summary>
        public static async Task<DCTLoginResult> GameList(string brand)
        {
            string md5Para = DCTBrandId + DCTApiKey;

            string md5Sign = Md5Sign(md5Para).ToUpper();

            //Console.WriteLine($"md5Sign = {md5Sign}");

            //Lang = "zh_hans"; //UNDONE: TEST ONLY

            var data = new
            {
                brand_id = DCTBrandId,
                sign = md5Sign,
                provider = brand,
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "/dct/getGameList";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            DCTLoginResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<ApiLoginResponse>(strResult);

                    if (values.code == 1000)
                    {
                        result.SetResult(values.data.game_url);
                    }
                    else
                    {
                        if (DebugFg) Console.WriteLine($"DCT GameList ERROR ! Code={values.code}, Msg={values.msg}");
                        result.SetError(values.code.ToString(), values.msg);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"DCT GameList API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"DCT GameList API 無回應!");
                result.SetError("9999", "No Response");
            }
            return result;
        }

        /// <summary>取得遊戲 url</summary>
        public static async Task<DCTGetBetRecordResult> GetBetRecord(string start_time, string end_time, string provider, int pageIndex)
        {
            string md5Para = DCTBrandId + start_time + end_time + DCTApiKey;

            string md5Sign = Md5Sign(md5Para).ToUpper();

            var data = new
            {
                brand_id = DCTBrandId,
                sign = md5Sign,
                page = pageIndex,
                start_time = start_time,
                end_time = end_time,
                currency = "MYR",
                provider = provider
            };

            //依功能所需的参数转为 JSON String
            string paraStr = JsonSerializer.Serialize(data);

            string apiUrlReq = "/dct/getBetData";

            var strResult = await SendApiReq2(apiUrlReq, paraStr);

            DCTGetBetRecordResult result = new();

            if (strResult != null)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                    if (values != null)
                    {
                        result.ErrorCode = values["code"].ToString();

                        result.Message = values["msg"].ToString();

                        if (result.ErrorCode == "1000")
                        {
                            result.data = JsonSerializer.Deserialize<List<DCTBetInfo>>(values["data"].ToString());
                            result.page = JsonSerializer.Deserialize<DCTPage>(values["page"].ToString());
                        }
                        else if (result.ErrorCode == "5042")
                        {
                            
                        }
                        else
                        {
                            if (DebugFg) Console.WriteLine($"DCT  GetBetRecord ERROR ! Code={result.ErrorCode}, Msg={result.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"DCT GetBetRecord API 無回應!");
                        result.SetError("9999", "No Response");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"DCT GetBetRecord API JSON 解析失敗 : {ex.Message}");
                    result.SetError("9998", "Json Error");
                }
            }
            else
            {
                Console.WriteLine($"DCT GetBetRecord API 無回應!");
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

            string apiUrlReq = DCTApiUrl + apiurl;

            //Console.WriteLine($"ApiUrl=[{apiUrlReq}]"); //UNDONE: Test Only
            //Console.WriteLine($"Para=[{paraStr}]"); //UNDONE: Test Only

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrlReq);
                request.Content = new StringContent(paraStr, Encoding.UTF8, "application/json");

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
                Console.WriteLine("DCT SendHttp ERROR Exception : " + ex);
                return null;
            }
        }

        static async Task<string> SendApiReq2(string apiurl, string paraStr)
        {
            //HttpContent content = new StringContent(paraStr, Encoding.UTF8, "application/json");

            string apiUrlReq = DCTgetBetDataUrl + apiurl;

            //Console.WriteLine($"ApiUrl=[{apiUrlReq}]"); //UNDONE: Test Only
            //Console.WriteLine($"Para=[{paraStr}]"); //UNDONE: Test Only

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrlReq);
                request.Content = new StringContent(paraStr, Encoding.UTF8, "application/json");

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
                Console.WriteLine("DCT SendHttp ERROR Exception : " + ex);
                return null;
            }
        }


        static string Md5Sign(string querystring)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(querystring);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 转换为 16進制 字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                
                return sb.ToString();
            }
        }
    }
}
