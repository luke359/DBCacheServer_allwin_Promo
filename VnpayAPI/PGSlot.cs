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

namespace VnpayAPI
{
    public class PGGetPlayerWalletRequest
    {
        public string operator_token { get; set; }
        public string secret_key { get; set; }
        public string player_name { get; set; }
    }

    public class PGBonusData
    {
        public string key { get; set; }
        public string bonusId { get; set; }
        public string bonusName { get; set; }
        public string transactionId { get; set; }
        public string bonusParentType { get; set; }
        public string gameIds { get; set; }
        public string balanceAmount { get; set; }
        public string bonusRatioAmount { get; set; }
        public string minimumConversionAmount { get; set; }
        public string maximumConversionAmount { get; set; }
        public string status { get; set; }
        public string createdDate { get; set; }
        public string expiredDate { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public string isSuppressDiscard { get; set; }

        public PGBonusData()
        {
            key = "";
            bonusId = "";
            bonusName = "";
            transactionId = "";
            bonusParentType = "";
            gameIds = "";
            balanceAmount = "";
            bonusRatioAmount = "";
            minimumConversionAmount = "";
            maximumConversionAmount = "";
            status = "";
            createdDate = "";
            expiredDate = "";
            createdBy = "";
            updatedBy = "";
            isSuppressDiscard = "";
        }
    }

    public class PGFreeGameData
    {
        public string key { get; set; }
        public string freeGameId { get; set; }
        public string transactionId { get; set; }
        public string gameIds { get; set; }
        public string gameIdLock { get; set; }
        public string gameCount { get; set; }
        public string totalGame { get; set; }
        public string balanceAmount { get; set; }
        public string minimumConversionAmount { get; set; }
        public string maximumConversionAmount { get; set; }
        public string multiplier { get; set; }
        public string coinSize { get; set; }
        public string createdDate { get; set; }
        public string expiredDate { get; set; }
        public string status { get; set; }
        public string conversionType { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public string isSupressDiscard { get; set; }

        public PGFreeGameData()
        {
            key = "";
            freeGameId = "";
            transactionId = "";
            gameIds = "";
            gameIdLock = "";
            gameCount = "";
            totalGame = "";
            balanceAmount = "";
            minimumConversionAmount = "";
            maximumConversionAmount = "";
            multiplier = "";
            coinSize = "";
            createdDate= "";
            expiredDate = "";
            status = "";
            conversionType = "";
            createdBy = "";
            updatedBy = "";
            isSupressDiscard = "";
        }
    }

    public class PGCashWallet
    {
        public string key { get; set; }
        public string cashId { get; set; }
        public string cashBalance { get; set; }

        public PGCashWallet()
        {
            key = "";
            cashId = "";
            cashBalance = "";
        }
    }

    public class PGBonusWallet
    {
        public string totalBonusBalance { get; set; }

        public PGBonusData bonuses { get; set; }

        public PGBonusWallet()
        {
            totalBonusBalance = "";
            bonuses = new PGBonusData();
        }
    }

    public class PGFreeGameWallet
    {
        public string freeGameBalance { get; set; }

        public PGFreeGameData freeGames { get; set; }

        public PGFreeGameWallet()
        {
            freeGameBalance = "";
            freeGames = new PGFreeGameData();
        }
    }

    public class PGData
    {
        public string currencyCode { get; set; }
        public decimal? totalBalance { get; set; }
        /// <summary>現金金額</summary>
        public decimal? cashBalance { get; set; } //現金值 只有此欄位值可以轉出, 其他都是試玩或外送, 只能在PGS遊戲內使用
        public decimal? totalBonusBalance { get; set; }
        public decimal? freeGameBalance { get; set; }

        //public PGBonusData bonuses { get; set; }
        //public PGFreeGameData freeGames { get; set; }
        //public PGCashWallet cashWallet { get; set; }
        //public PGBonusWallet bonusWallet { get; set; }
        //public PGFreeGameWallet freeGameWallet { get; set; }


        //public PGData()
        //{
        //    currencyCode = "";
        //    totalBalance = "";
        //    cashBalance = "";
        //    totalBonusBalance = "";
        //    freeGameBalance = "";
        //    bonuses = new PGBonusData();
        //    freeGames = new PGFreeGameData();
        //    cashWallet = new PGCashWallet();
        //    bonusWallet = new PGBonusWallet();
        //    freeGameWallet = new PGFreeGameWallet();
        //}
    }


    public class PGGetPlayerWalletResult
    {
        public PGData data { get; set; }
        public string error { get; set; }

        public PGGetPlayerWalletResult()
        {
            data = new PGData();
            error = "";
        }
    }


    public class PGTransferPlayerWallet
    {
        public TransferData data { get; set; }
        public ErrorData error { get; set; }

        //public PGTransferPlayerWallet()
        //{
        //    data = new TransferData();
        //    error = new ErrorData();
        //}
    }

    /// <summary>充值轉值 Api請求</summary>
    public class PGTransferWalletRequest
    {
        public string operator_token { get; set; }
        public string secret_key { get; set; }
        public string player_name { get; set; }
        /// <summary>充轉金額</summary>
        public string amount { get; set; }
        /// <summary>交易憑證</summary>
        public string transfer_reference { get; set; }
        /// <summary>幣種</summary>
        public string currency { get; set; }
    }

    /// <summary>充值轉值 Api回應</summary>
    public class TransferData
    {
        /// <summary>充轉序號</summary>
        public string transactionId { get; set; }
        /// <summary>充轉後玩家金額</summary>
        public decimal? balanceAmount { get; set; }
        /// <summary>充轉前玩家金額</summary>
        public decimal? balanceAmountBefore { get; set; }
        /// <summary>充轉金額</summary>
        public decimal? amount { get; set; }
    }

    public class ErrorData
    {
        public int? code { get; set; }
        public string msg { get; set; }
    }

    /// <summary>Api請求的回應結果</summary>
    public class PGSlotResult
    {
        /// <summary>轉帳金額</summary>
        public double Amount { get; set; }
        /// <summary>轉帳前玩家餘額</summary>
        public double BalanceBefore { get; set; }
        /// <summary>轉帳後玩家餘額</summary>
        public double BalanceAfter { get; set; }
        /// <summary>PGS產生的交易識別碼</summary>
        public string TransactionId { get; set; }

        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }
    }

    public class PGBetRecordInfo
    {
        /// <summary>子投注的唯一标识</summary>
        public long betId { get; set; }
        /// <summary>母单投注的唯一标识</summary>
        public long parentBetId { get; set; }
        /// <summary>玩家账号</summary>
        public string playerName { get; set; }
        /// <summary>玩家的币种</summary>
        public string currency { get; set; }
        /// <summary>游戏独特代码</summary>
        public int gameId { get; set; }
        /// <summary>投注记录的平台</summary>
        public int platform { get; set; }
        /// <summary>投注记录的类别</summary>
        public int betType { get; set; }
        /// <summary>交易的类别</summary>
        public int transactionType { get; set; }
        /// <summary>玩家的投注额</summary>
        public decimal betAmount { get; set; }
        /// <summary>玩家的派彩金额</summary>
        public decimal winAmount { get; set; }

        /// <summary>玩家的派彩金额</summary>
        public decimal jackpotRtpContributionAmount { get; set; }
        /// <summary>玩家的派彩金额</summary>
        public decimal jackpotContributionAmount { get; set; }
        /// <summary>玩家的派彩金额</summary>
        public decimal jackpotWinAmount { get; set; }
        /// <summary>玩家的派彩金额</summary>
        public decimal balanceBefore { get; set; }
        /// <summary>玩家的派彩金额</summary>
        public decimal balanceAfter { get; set; }
        /// <summary>玩家的派彩金额</summary>
        public int handsStatus { get; set; }
        /// <summary>数据的更新时间</summary>
        public long rowVersion { get; set; }
        /// <summary>当前投注的开始时间</summary>
        public long betTime { get; set; }
        /// <summary>当前投注的开始时间</summary>
        public long betEndTime { get; set; }
        /// <summary>指示旋转类型</summary>
        public bool isFeatureBuy { get; set; }

        public PGBetRecordInfo()
        {
            betId = 0;
            parentBetId = 0;
            playerName = "";
            currency = "";
            gameId = 0;
            platform = 0;
            betType = 0;
            transactionType = 0;
            betAmount = 0;
            winAmount = 0;
            jackpotRtpContributionAmount = 0;
            jackpotContributionAmount = 0;
            jackpotWinAmount = 0;
            balanceBefore = 0;
            balanceAfter = 0;
            handsStatus = 0;
            rowVersion = 0;
            betTime = 0;
            betEndTime = 0;
            isFeatureBuy = false;
        }
    }

    public class PGBetRecordResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string serialNo { get; set; }
        public string error { get; set; }

        public List<PGBetRecordInfo> data { get; set; }

        public PGBetRecordResult()
        {
            code = 0;
            msg = "";
            serialNo = "";
            data = new List<PGBetRecordInfo>();
            error = "";
        }
    }

    public class PGSlot
    {
        static readonly string OperatorToken = "fe-aa37-9900-9dea4ac4";
        static readonly string SecretKey = "b31-467c-8b88-b6d8";

        static readonly string ApiCashRoute = "https://api.pgs-bo.com/";
        static readonly string ApiBrtRoute = "https://api.pgs-bo.com/";
        const double ApiTimeout = 15;  // 設定請求超時為 15 秒

        /// <summary></summary>
        public static async Task<PGSlotResult> CheckPlayerBalance(string player_name)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    //var jsonString = JsonSerializer.Serialize(CheckParameter);

                    string g = Guid.NewGuid().ToString();

                    //string ApiUrl = $"https://m.pgr-asni1z.com/external/Cash/v3/GetPlayerWallet?trace_id="+ g;
                    string ApiUrl = $"{ApiCashRoute}GetPlayerWallet?trace_id=" + g;

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    using (HttpClient client2 = new HttpClient())
                    {
                        client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
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
                        PostContent.Add("operator_token", OperatorToken);
                        PostContent.Add("secret_key", SecretKey);
                        PostContent.Add("player_name", player_name);

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = await client2.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                var values = JsonSerializer.Deserialize<PGGetPlayerWalletResult>(strResult);

                                PGSlotResult result = new();
                                result.Amount = DecimalToDouble(values.data.cashBalance);

                                result.LGTResult = "0";
                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                PGSlotResult result = new();

                                result.LGTResult = "9998";

                                result.LGTRemarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            PGSlotResult result = new();
                            result.LGTResult = "9998";
                            result.LGTRemarks = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                PGSlotResult result = new();
                result.Amount = 0;
                result.LGTResult = "1";
                result.LGTRemarks = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                PGSlotResult result = new();
                result.LGTResult = "9999";
                result.LGTRemarks = ex.Message;
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<PGSlotResult> CheckPlayerBalance2(string player_name, HttpClient client2)
        {
            try
            {
                string g = Guid.NewGuid().ToString();
                string ApiUrl = $"{ApiCashRoute}GetPlayerWallet?trace_id=" + g;
                var FullApiUrl = $"{ApiUrl}";

                client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                Dictionary<string, string> PostContent = new Dictionary<string, string>();
                PostContent.Add("operator_token", OperatorToken);
                PostContent.Add("secret_key", SecretKey);
                PostContent.Add("player_name", player_name);

                request.Content = new FormUrlEncodedContent(PostContent);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = await client2.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response != null)
                {
                    if (response.IsSuccessStatusCode == true)
                    {
                        // 取得呼叫完成 API 後的回報內容
                        String strResult = await response.Content.ReadAsStringAsync();
                        var values = JsonSerializer.Deserialize<PGGetPlayerWalletResult>(strResult);

                        if (values != null)
                        {
                            PGSlotResult result = new();
                            result.Amount = DecimalToDouble(values.data.cashBalance);
                            result.LGTResult = "0";
                            return result;
                        }
                    }

                    PGSlotResult eresult = new();
                    eresult.LGTResult = "9998";
                    eresult.LGTRemarks = "post connection status error";
                    return eresult;
                }
                else
                {
                    PGSlotResult result = new();
                    result.LGTResult = "9998";
                    result.LGTRemarks = "post connection status error";
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                PGSlotResult result = new();
                result.Amount = 0;
                result.LGTResult = "1";
                result.LGTRemarks = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>充值至PGS</summary>
        public static async Task<PGSlotResult> TransferInPlayerBalance(string player_name, double amount, string tref, string currency)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiCashRoute}TransferIn?trace_id=" + g;

                    Dictionary<string, string> PostContent = new Dictionary<string, string>();
                    PostContent.Add("operator_token", OperatorToken);
                    PostContent.Add("secret_key", SecretKey);
                    PostContent.Add("player_name", player_name);
                    PostContent.Add("amount", amount.ToString());
                    PostContent.Add("transfer_reference", tref);
                    PostContent.Add("currency", currency);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
                    request.Content = new FormUrlEncodedContent(PostContent);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    using (HttpClient client2 = new HttpClient())
                    {
                        client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpResponseMessage response = await client2.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                PGSlotResult result = new();
                                var values = JsonSerializer.Deserialize<PGTransferPlayerWallet>(strResult);

                                if (values != null)
                                {
                                    if (values.data != null)
                                    {
                                        result.Amount = DecimalToDouble(values.data.amount);
                                        result.BalanceBefore = DecimalToDouble(values.data.balanceAmountBefore);
                                        result.BalanceAfter = DecimalToDouble(values.data.balanceAmount);
                                        result.TransactionId = values.data.transactionId;
                                        result.LGTResult = "0";
                                    }
                                    else if (values.error != null)
                                    {
                                        result.Amount = 0;
                                        result.LGTResult = "1";
                                        Console.WriteLine($"TransferOutPlayerBalance ERROR msg:[Code={values.error.code}, Msg={values.error.msg}]");
                                    }
                                }
                                else
                                {
                                    result.LGTResult = "9985";
                                    result.LGTRemarks = "TransferIn Player Balance Error";
                                }

                                return result;
                            }
                            else
                            {
                                //Console.WriteLine("TransferInPlayerBalance Response ERROR!!");

                                PGSlotResult result = new();
                                result.LGTResult = "9988";
                                result.LGTRemarks = "post connection status error";
                                return result;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("TransferInPlayerBalance ERROR 無回應!");

                            PGSlotResult result = new();
                            result.LGTResult = "9988";
                            result.LGTRemarks = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TransferInPlayerBalance ERROR Exception : " + ex);

                PGSlotResult result = new();
                result.LGTResult = "9989";
                result.LGTRemarks = ex.Message;
                return result;
            }
        }

        /// <summary>從PGS轉出</summary>
        public static async Task<PGSlotResult> TransferOutPlayerBalance(string player_name, string tref, string currency)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiCashRoute}TransferOut?trace_id=" + g;

                    using (HttpClient client2 = new HttpClient())
                    {
                        //HttpClient client2 = HttpClientManager.Instance;
                        client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);

                        //先檢查玩家餘額
                        var chkRes = await PGSlot.CheckPlayerBalance2(player_name, client2);
                        //PGSlotResult chkRes = task.Result;

                        if (chkRes.LGTResult != "0")
                        {
                            return chkRes;
                        }

                        if (chkRes.Amount <= 0)
                        {
                            return chkRes;
                        }

                        //開始取回玩家餘額
                        double amount = chkRes.Amount;

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        PostContent.Add("operator_token", OperatorToken);
                        PostContent.Add("secret_key", SecretKey);
                        PostContent.Add("player_name", player_name);
                        PostContent.Add("amount", amount.ToString());
                        PostContent.Add("transfer_reference", tref);
                        PostContent.Add("currency", currency);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                        HttpResponseMessage response = await client2.SendAsync(request);

                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                //Console.WriteLine($"ApiRawData:[ {strResult} ]");
                                PGSlotResult result = new();
                                var values = JsonSerializer.Deserialize<PGTransferPlayerWallet>(strResult);

                                if (values != null)
                                {
                                    if (values.data != null)
                                    {
                                        result.Amount = DecimalToDouble(values.data.amount);
                                        result.BalanceBefore = DecimalToDouble(values.data.balanceAmountBefore);
                                        result.BalanceAfter = DecimalToDouble(values.data.balanceAmount);
                                        result.TransactionId = values.data.transactionId;
                                        result.LGTResult = "0";
                                    }
                                    else if (values.error != null)
                                    {
                                        result.Amount = 0;
                                        result.LGTResult = "1";
                                        //Console.WriteLine($"TransferOutPlayerBalance ERROR msg:[Code={values.error.code}, Msg={values.error.msg}]");
                                    }
                                }
                                else
                                {
                                    result.LGTResult = "9975";
                                    result.LGTRemarks = "TransferOut Player Balance Error";
                                }

                                return result;
                            }
                            else
                            {
                                //Console.WriteLine("TransferOutPlayerBalance Response ERROR!!");

                                PGSlotResult result = new();
                                result.LGTResult = "9977";
                                result.LGTRemarks = "post connection response error";
                                return result;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("TransferOutPlayerBalance ERROR 無回應!");

                            PGSlotResult result = new();
                            result.LGTResult = "9978";
                            result.LGTRemarks = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("TransferOutPlayerBalance ERROR Exception : " + ex);

                PGSlotResult result = new();
                result.LGTResult = "9979";
                result.LGTRemarks = ex.Message;
                return result;
            }
        }

        /// <summary></summary>
        public static async Task<PGBetRecordResult> GetHistory(long timestamp)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    string g = Guid.NewGuid().ToString();

                    string ApiUrl = $"{ApiBrtRoute}GetHistory?trace_id=" + g;

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    using (HttpClient client2 = new HttpClient())
                    {
                        client2.Timeout = TimeSpan.FromSeconds(ApiTimeout);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        PostContent.Add("operator_token", OperatorToken);
                        PostContent.Add("secret_key", SecretKey);
                        PostContent.Add("count", "5000");
                        PostContent.Add("bet_type", "1");
                        PostContent.Add("row_version", timestamp.ToString());
                        PostContent.Add("hands_status", "0");

                        request.Content = new FormUrlEncodedContent(PostContent);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        response = client2.Send(request);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                PGBetRecordResult result = new();

                                if (values.ContainsKey("code"))
                                {
                                    result.code = (values["code"] == null) ? 0 : Int32.Parse(values["code"].ToString());
                                }

                                if (values.ContainsKey("msg"))
                                {
                                    result.msg = (values["msg"] == null) ? "" : values["msg"].ToString();
                                }

                                if (values.ContainsKey("serialNo"))
                                {
                                    result.serialNo = (values["serialNo"] == null) ? "" : values["serialNo"].ToString();
                                }

                                if (values.ContainsKey("data"))
                                {
                                    result.data = (values["data"] == null) ? null : JsonSerializer.Deserialize<List<PGBetRecordInfo>>(values["data"].ToString());
                                }

                                if (values.ContainsKey("error"))
                                {
                                    result.error = (values["error"] == null) ? "" : values["error"].ToString();
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();

                                PGBetRecordResult result = new();

                                result.code = Int32.Parse("9998");

                                result.msg = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            PGBetRecordResult result = new();
                            result.code = Int32.Parse("9998");
                            result.msg = "post connection status error";
                            return result;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                PGBetRecordResult result = new();
                result.code = Int32.Parse("1");
                result.msg = "player not found";
                return result;
            }
            catch (Exception ex)
            {
                PGBetRecordResult result = new();
                result.code = Int32.Parse("9999");
                result.msg = ex.Message;
                return result;
            }
        }


        /// <summary></summary>
        public static Double DecimalToDouble(decimal? dval)
        {
            return (Double)(dval ?? 0);
        }
    }
}
