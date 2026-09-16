using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Numerics;
using System.Threading.Channels;
using System.Xml;

namespace VnpayAPI
{
    public class EEDepositRequest
    {
        public string acc { get; set; }
        public string referrer_acc { get; set; }
        public string ukey { get; set; }
        public double amount { get; set; }
        public string channel { get; set; }
        public string remark { get; set; }
        public string playerName { get; set; }
        public string playerAccountNumber { get; set; }
        public string bankcode { get; set; }
    }

    public class EEDepositResult
    {
        public string paymentUrl { get; set; }
        public string success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string teansid { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public EEDepositResult()
        {
            paymentUrl = "";
            success = "";
            code = "";
            message = "";
            teansid = "";
            LGTResult = "";
            LGTRemarks = "";
        }
    }

    public class EECheckDepositStatusResult
    {
        public string paymentStatus { get; set; }
        public string paymentDate { get; set; }
        public string referenceNumber { get; set; }
        public string success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public EECheckDepositStatusResult()
        {
            paymentStatus = "";
            paymentDate = "";
            referenceNumber = "";
            success = "";
            code = "";
            message = "";
            LGTResult = "";
            LGTRemarks = "";
        }
    }

    public class EEWithdrawRequest
    {
        public string acc { get; set; }
        public string referrer_acc { get; set; }
        public string ukey { get; set; }
        public double amount { get; set; }
        public double fee { get; set; }
        public string channel { get; set; }
        public string customerBankCode { get; set; }
        public string customerBankHolderName { get; set; }
        public string customerBankAccount { get; set; }
        public string customerMobilePhone { get; set; }
        public string city { get; set; }
        public string province { get; set; }
    }

    public class EEBankTransferWithdrawRequest
    {
        public string acc { get; set; }
        public string referrer_acc { get; set; }
        public string ukey { get; set; }
        public double amount { get; set; }
        public double fee { get; set; }
        public string customerBankName { get; set; }
        public string customerBankHolderName { get; set; }
        public string customerBankAccount { get; set; }
        public string customerMobilePhone { get; set; }
    }

    public class EEWithdrawResult
    {
        public string success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string teansid { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public EEWithdrawResult()
        {
            success = "";
            code = "";
            message = "";
            teansid = "";
            LGTResult = "";
            LGTRemarks = "";
        }
    }

    public class EECheckWithdrawStatusResult
    {
        public string payoutStatus { get; set; }
        public string payoutDate { get; set; }
        public string success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public EECheckWithdrawStatusResult()
        {
            payoutStatus = "";
            payoutDate = "";
            success = "";
            code = "";
            message = "";
            LGTResult = "";
            LGTRemarks = "";
        }
    }

    public class EEBankListRequest
    {
        public string channel { get; set; }
    }

    public class EEBankListResult
    {
        public string success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string Result { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public EEBankListResult()
        {
            success = "";
            code = "";
            message = "";
            Result = "";
            LGTResult = "";
            LGTRemarks = "";
        }
    }

    public class EEGetwayResult
    {
        public string success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public List<string> Result { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public EEGetwayResult()
        {
            success = "";
            code = "";
            message = "";
            Result = new List<string>();
            LGTResult = "";
            LGTRemarks = "";
        }
    }

    public class EEGetwayList
    {
        public List<string> name { get; set; }

        public EEGetwayList()
        {
            name = new List<string>();
        }
    }

    public class PaymentData
    {
        public string paymentStatus { get; set; }
        public string paymentDate { get; set; }
        public string referenceNumber { get; set; }
        public string uniqueId { get; set; }

        public PaymentData()
        {
            paymentStatus = "";
            paymentDate = "";
            referenceNumber = "";
            uniqueId = "";
        }
    }

    public class StatusResult
    {
        public string success { get; set; }
        public string code { get; set; }
        public string message { get; set; }

        public StatusResult()
        {
            success = "";
            code = "";
            message = "";
        }
    }

    public class EECheckDepositStatusResult2
    {
        public PaymentData data { get; set; }

        public StatusResult result { get; set; }

        public EECheckDepositStatusResult2()
        {
            data = new PaymentData();
            result = new StatusResult();
        }
    }

    public class ToolData
    {
        public string name { get; set; }
        public string supportToolType { get; set; }
        public string link { get; set; }

        public ToolData()
        {
            name = "";
            supportToolType = "";
            link = "";
        }
    }

    public class EEStatusResult
    {
        public bool success { get; set; }
        public string code { get; set; }
        public string message { get; set; }

        public EEStatusResult()
        {
            success = false;
            code = "";
            message = "";
        }
    }

    public class EESupportToolResult
    {
        public List<ToolData> data { get; set; }

        public EEStatusResult result { get; set; }

        public EESupportToolResult()
        {
            data = new List<ToolData>();
            result = new EEStatusResult();
        }
    }

    public class EEPay
    {
        public static int testcnt = 0;

        static string ApiUrl = $"https://vgth-api.gamemasterpay.com/api/";

        //static string ApiUrl = $"https://vgth-apistaging.gamemasterpay.com/api/";

        public static async Task<EEDepositResult> EEDeposit(string Player, string Agent, string TransNumber, string Amount, string BankCode, string BankName, string BankAccount, string Remark, string channel)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("acc", Player);
            PostContent.Add("referrer_acc", Agent);
            PostContent.Add("ukey", TransNumber);
            PostContent.Add("amount", Amount);
            PostContent.Add("channel", channel);
            PostContent.Add("remark", Remark);
            PostContent.Add("playerName", BankName);
            PostContent.Add("playerAccountNumber", BankAccount);
            PostContent.Add("bankcode", BankCode);

            var eeDepositParameter = new EEDepositRequest
            {
                acc = PostContent["acc"],
                referrer_acc = PostContent["referrer_acc"],
                ukey = PostContent["ukey"],
                amount = double.Parse(PostContent["amount"]),
                channel = PostContent["channel"],
                remark = PostContent["remark"],
                playerName = PostContent["playerName"],
                playerAccountNumber = PostContent["playerAccountNumber"],
                bankcode = PostContent["bankcode"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(eeDepositParameter);

                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}transactions/payments";

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.PostAsync(FullApiUrl, Content);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EEDepositResult result = new EEDepositResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.paymentUrl = (data.GetProperty("paymentUrl").ToString() == null) ? "" : data.GetProperty("paymentUrl").ToString();
                                result.teansid = (data.GetProperty("uniqueId").ToString() == null) ? "" : data.GetProperty("uniqueId").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    if (result.code == "100099")
                                    {
                                        result.LGTResult = "100099";

                                        result.LGTRemarks = result.message;
                                    }
                                    else
                                    {
                                        result.LGTResult = "9996";

                                        //result.LGTRemarks = "deposit error";
                                        result.LGTRemarks = "Unexpected Error, Please Contact Support";
                                    }
                                }

                                return result;
                            }
                            else
                            {
                                EEDepositResult result = new EEDepositResult();

                                result.LGTResult = "9998";

                                //result.LGTRemarks = "post connection status error";
                                result.LGTRemarks = "Unexpected Error, Please Contact Support";

                                return result;
                            }
                        }
                        else
                        {
                            EEDepositResult result = new EEDepositResult();

                            result.LGTResult = "9998";

                            //result.LGTRemarks = "post connection status error";
                            result.LGTRemarks = "Unexpected Error, Please Contact Support";
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EEDepositResult result = new EEDepositResult();

                        result.LGTResult = "9999";

                        //result.LGTRemarks = ex.Message;
                        result.LGTRemarks = "Unexpected Error, Please Contact Support";

                        return result;
                    }
                }
            }
        }

        public static async Task<EECheckDepositStatusResult> EECheckDepositStatus(string uniqueId, string channel)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}transactions/payments/" + uniqueId;

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        string jsonString = "";

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.GetAsync(FullApiUrl);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.paymentStatus = (data.GetProperty("paymentStatus").ToString() == null) ? "" : data.GetProperty("paymentStatus").ToString();
                                result.paymentDate = (data.GetProperty("paymentDate").ToString() == null) ? "" : data.GetProperty("paymentDate").ToString();
                                result.referenceNumber = (data.GetProperty("referenceNumber").ToString() == null) ? "" : data.GetProperty("referenceNumber").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();


                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "deposit error";
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement resultvalue = (JsonElement)values["result"];

                                if (values["data"] != null)
                                {
                                    JsonElement data = (JsonElement)values["data"];

                                    result.paymentStatus = (data.GetProperty("paymentStatus").ToString() == null) ? "" : data.GetProperty("paymentStatus").ToString();
                                    result.paymentDate = (data.GetProperty("paymentDate").ToString() == null) ? "" : data.GetProperty("paymentDate").ToString();
                                    result.referenceNumber = (data.GetProperty("referenceNumber").ToString() == null) ? "" : data.GetProperty("referenceNumber").ToString();
                                }
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.message == "New Request")
                                {
                                    result.LGTResult = "10002";

                                    result.paymentStatus = "Initial";
                                }
                                else if (result.message == "expired")
                                {
                                    result.LGTResult = "10002";

                                    result.paymentStatus = "Expired";
                                }
                                else
                                {
                                    result.LGTResult = "9998";

                                    result.LGTRemarks = "post connection status error";
                                }


                                return result;
                            }
                        }
                        else
                        {
                            EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<EEWithdrawResult> EEWithdraw(string Player, string Agent, string TransNumber, string Amount, string BankCode, string BankName, string BankAccount, string Phone, string channel)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("acc", Player);
            PostContent.Add("referrer_acc", Agent);
            PostContent.Add("ukey", TransNumber);
            PostContent.Add("amount", Amount);
            PostContent.Add("fee", "0.00");
            PostContent.Add("channel", channel);
            PostContent.Add("customerBankCode", BankCode);
            PostContent.Add("customerBankHolderName", BankName);
            PostContent.Add("customerBankAccount", BankAccount);
            if (Phone == "")
            {
                PostContent.Add("customerMobilePhone", "7533967");
            }
            else
            {
                PostContent.Add("customerMobilePhone", Phone);
            }

            if (channel == "heropay")
            {
                PostContent.Add("city", "bangkok");
                PostContent.Add("province", "bangkok");
            }
            else
            {
                PostContent.Add("city", "");
                PostContent.Add("province", "");
            }

            var eeWithdrawParameter = new EEWithdrawRequest
            {
                acc = PostContent["acc"],
                referrer_acc = PostContent["referrer_acc"],
                ukey = PostContent["ukey"],
                amount = double.Parse(PostContent["amount"]),
                fee = double.Parse(PostContent["fee"]),
                channel = PostContent["channel"],
                customerBankCode = PostContent["customerBankCode"],
                customerBankHolderName = PostContent["customerBankHolderName"],
                customerBankAccount = PostContent["customerBankAccount"],
                customerMobilePhone = PostContent["customerMobilePhone"],
                city = PostContent["city"],
                province = PostContent["province"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(eeWithdrawParameter);

                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}transactions/withdraws";

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.PostAsync(FullApiUrl, Content);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EEWithdrawResult result = new EEWithdrawResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);
                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.teansid = (data.GetProperty("uniqueId").ToString() == null) ? "" : data.GetProperty("uniqueId").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    if (result.code == "100099")
                                    {
                                        result.LGTResult = "100099";

                                        result.LGTRemarks = result.message;
                                    }
                                    else
                                    {
                                        result.LGTResult = "9996";

                                        //result.LGTRemarks = "withdraw error";
                                        result.LGTRemarks = "Unexpected Error, Please Contact Support";
                                    }
                                    
                                }

                                return result;
                            }
                            else
                            {
                                EEWithdrawResult result = new EEWithdrawResult();

                                result.LGTResult = "9998";

                                //result.LGTRemarks = "post connection status error";
                                result.LGTRemarks = "Unexpected Error, Please Contact Support";
                                return result;
                            }
                        }
                        else
                        {
                            EEWithdrawResult result = new EEWithdrawResult();

                            result.LGTResult = "9998";

                            //result.LGTRemarks = "post connection status error";
                            result.LGTRemarks = "Unexpected Error, Please Contact Support";
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EEWithdrawResult result = new EEWithdrawResult();

                        result.LGTResult = "9999";

                        //result.LGTRemarks = ex.Message;
                        result.LGTRemarks = "Unexpected Error, Please Contact Support";
                        return result;
                    }
                }
            }
        }

        public static async Task<EECheckWithdrawStatusResult> EECheckWithdrawStatus(string uniqueId, string channel)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}transactions/withdraws/" + uniqueId;

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        string jsonString = "";

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.GetAsync(FullApiUrl);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.payoutStatus = (data.GetProperty("payoutStatus").ToString() == null) ? "" : data.GetProperty("payoutStatus").ToString();
                                result.payoutDate = (data.GetProperty("payoutDate").ToString() == null) ? "" : data.GetProperty("payoutDate").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();


                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "deposit error";
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement resultvalue = (JsonElement)values["result"];

                                if (values["data"] != null)
                                {
                                    JsonElement data = (JsonElement)values["data"];

                                    result.payoutStatus = (data.GetProperty("payoutStatus").ToString() == null) ? "" : data.GetProperty("payoutStatus").ToString();
                                    result.payoutDate = (data.GetProperty("payoutDate").ToString() == null) ? "" : data.GetProperty("payoutDate").ToString();
                                }
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.message == "New Request")
                                {
                                    result.LGTResult = "10002";

                                    result.payoutStatus = "Initial";
                                }
                                else if (result.message == "expired")
                                {
                                    result.LGTResult = "10002";

                                    result.payoutStatus = "Expired";
                                }
                                else
                                {
                                    result.LGTResult = "9998";

                                    result.LGTRemarks = "post connection status error";
                                }


                                return result;
                            }
                        }
                        else
                        {
                            EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<EEDepositResult> EEBankTransferDeposit(string Player, string Agent, string TransNumber, string Amount)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("acc", Player);
            PostContent.Add("referrer_acc", Agent);
            PostContent.Add("ukey", TransNumber);
            PostContent.Add("amount", Amount);

            var eeDepositParameter = new EEDepositRequest
            {
                acc = PostContent["acc"],
                referrer_acc = PostContent["referrer_acc"],
                ukey = PostContent["ukey"],
                amount = double.Parse(PostContent["amount"])
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(eeDepositParameter);

                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}bank-transfer/deposit";

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.PostAsync(FullApiUrl, Content);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EEDepositResult result = new EEDepositResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.paymentUrl = (data.GetProperty("paymentUrl").ToString() == null) ? "" : data.GetProperty("paymentUrl").ToString();
                                result.teansid = (data.GetProperty("uniqueId").ToString() == null) ? "" : data.GetProperty("uniqueId").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    if (result.code == "100099")
                                    {
                                        result.LGTResult = "100099";

                                        result.LGTRemarks = result.message;
                                    }
                                    else
                                    {
                                        result.LGTResult = "9996";

                                        //result.LGTRemarks = "deposit error";
                                        result.LGTRemarks = "Unexpected Error, Please Contact Support";
                                    }
                                }

                                return result;
                            }
                            else
                            {
                                EEDepositResult result = new EEDepositResult();

                                result.LGTResult = "9998";

                                //result.LGTRemarks = "post connection status error";
                                result.LGTRemarks = "Unexpected Error, Please Contact Support";

                                return result;
                            }
                        }
                        else
                        {
                            EEDepositResult result = new EEDepositResult();

                            result.LGTResult = "9998";

                            //result.LGTRemarks = "post connection status error";
                            result.LGTRemarks = "Unexpected Error, Please Contact Support";
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EEDepositResult result = new EEDepositResult();

                        result.LGTResult = "9999";

                        //result.LGTRemarks = ex.Message;
                        result.LGTRemarks = "Unexpected Error, Please Contact Support";
                        return result;
                    }
                }
            }
        }

        public static async Task<EECheckDepositStatusResult> EECheckBankTransferDepositStatus(string uniqueId)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}bank-transfer/deposit/" + uniqueId;

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        string jsonString = "";

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.GetAsync(FullApiUrl);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.paymentStatus = (data.GetProperty("paymentStatus").ToString() == null) ? "" : data.GetProperty("paymentStatus").ToString();
                                result.paymentDate = (data.GetProperty("paymentDate").ToString() == null) ? "" : data.GetProperty("paymentDate").ToString();
                                result.referenceNumber = (data.GetProperty("referenceNumber").ToString() == null) ? "" : data.GetProperty("referenceNumber").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();


                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "deposit error";
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement resultvalue = (JsonElement)values["result"];

                                if (values["data"] != null)
                                {
                                    JsonElement data = (JsonElement)values["data"];

                                    result.paymentStatus = (data.GetProperty("paymentStatus").ToString() == null) ? "" : data.GetProperty("paymentStatus").ToString();
                                    result.paymentDate = (data.GetProperty("paymentDate").ToString() == null) ? "" : data.GetProperty("paymentDate").ToString();
                                    result.referenceNumber = (data.GetProperty("referenceNumber").ToString() == null) ? "" : data.GetProperty("referenceNumber").ToString();
                                }
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.message == "New Request")
                                {
                                    result.LGTResult = "10002";

                                    result.paymentStatus = "Initial";
                                }
                                else if (result.message == "expired")
                                {
                                    result.LGTResult = "10002";

                                    result.paymentStatus = "Expired";
                                }
                                else
                                {
                                    result.LGTResult = "9998";

                                    result.LGTRemarks = "post connection status error";
                                }


                                return result;
                            }
                        }
                        else
                        {
                            EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EECheckDepositStatusResult result = new EECheckDepositStatusResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<EEWithdrawResult> EEBankTransferWithdraw(string Player, string Agent, string TransNumber, string Amount, string BankCode, string BankName, string BankAccount, string Phone)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("acc", Player);
            PostContent.Add("referrer_acc", Agent);
            PostContent.Add("ukey", TransNumber);
            PostContent.Add("amount", Amount);
            PostContent.Add("fee", "0.00");
            PostContent.Add("customerBankName", BankCode);
            PostContent.Add("customerBankHolderName", BankName);
            PostContent.Add("customerBankAccount", BankAccount);
            if (Phone == "")
            {
                PostContent.Add("customerMobilePhone", "7533967");
            }
            else
            {
                PostContent.Add("customerMobilePhone", Phone);
            }

            var eeWithdrawParameter = new EEBankTransferWithdrawRequest
            {
                acc = PostContent["acc"],
                referrer_acc = PostContent["referrer_acc"],
                ukey = PostContent["ukey"],
                amount = double.Parse(PostContent["amount"]),
                fee = double.Parse(PostContent["fee"]),
                customerBankName = PostContent["customerBankName"],
                customerBankHolderName = PostContent["customerBankHolderName"],
                customerBankAccount = PostContent["customerBankAccount"],
                customerMobilePhone = PostContent["customerMobilePhone"],
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(eeWithdrawParameter);

                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}bank-transfer/withdraw";

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.PostAsync(FullApiUrl, Content);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EEWithdrawResult result = new EEWithdrawResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);
                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.teansid = (data.GetProperty("uniqueId").ToString() == null) ? "" : data.GetProperty("uniqueId").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    if (result.code == "100099")
                                    {
                                        result.LGTResult = "100099";

                                        result.LGTRemarks = result.message;
                                    }
                                    else
                                    {
                                        result.LGTResult = "9996";

                                        //result.LGTRemarks = "withdraw error";
                                        result.LGTRemarks = "Unexpected Error, Please Contact Support";
                                    }

                                }

                                return result;
                            }
                            else
                            {
                                EEWithdrawResult result = new EEWithdrawResult();

                                String strResult = await response.Content.ReadAsStringAsync();

                                result.LGTResult = "9998";

                                //result.LGTRemarks = "post connection status error";
                                result.LGTRemarks = "Unexpected Error, Please Contact Support";
                                return result;
                            }
                        }
                        else
                        {
                            EEWithdrawResult result = new EEWithdrawResult();

                            result.LGTResult = "9998";

                            //result.LGTRemarks = "post connection status error";
                            result.LGTRemarks = "Unexpected Error, Please Contact Support";
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EEWithdrawResult result = new EEWithdrawResult();

                        result.LGTResult = "9999";

                        //result.LGTRemarks = ex.Message;
                        result.LGTRemarks = "Unexpected Error, Please Contact Support";
                        return result;
                    }
                }
            }
        }

        public static async Task<EECheckWithdrawStatusResult> EECheckBankTransferWithdrawStatus(string uniqueId)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}bank-transfer/withdraw/" + uniqueId;

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        string jsonString = "";

                        using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        {
                            response = await client.GetAsync(FullApiUrl);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement data = (JsonElement)values["data"];
                                JsonElement resultvalue = (JsonElement)values["result"];
                                result.payoutStatus = (data.GetProperty("payoutStatus").ToString() == null) ? "" : data.GetProperty("payoutStatus").ToString();
                                result.payoutDate = (data.GetProperty("payoutDate").ToString() == null) ? "" : data.GetProperty("payoutDate").ToString();
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();


                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";
                                }
                                else
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "deposit error";
                                }

                                return result;
                            }
                            else
                            {
                                String strResult = await response.Content.ReadAsStringAsync();
                                EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement resultvalue = (JsonElement)values["result"];

                                if (values["data"] != null)
                                {
                                    JsonElement data = (JsonElement)values["data"];

                                    result.payoutStatus = (data.GetProperty("payoutStatus").ToString() == null) ? "" : data.GetProperty("payoutStatus").ToString();
                                    result.payoutDate = (data.GetProperty("payoutDate").ToString() == null) ? "" : data.GetProperty("payoutDate").ToString();
                                }
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();

                                if (result.message == "New Request")
                                {
                                    result.LGTResult = "10002";

                                    result.payoutStatus = "Initial";
                                }
                                else if (result.message == "expired")
                                {
                                    result.LGTResult = "10002";

                                    result.payoutStatus = "Expired";
                                }
                                else
                                {
                                    result.LGTResult = "9998";

                                    result.LGTRemarks = "post connection status error";
                                }


                                return result;
                            }
                        }
                        else
                        {
                            EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EECheckWithdrawStatusResult result = new EECheckWithdrawStatusResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<EEBankListResult> EEBankList(string channel)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("channel", channel);

            var eeBankListParameter = new EEBankListRequest
            {
                channel = PostContent["channel"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(eeBankListParameter);

                        //HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}banks";

                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        //using (var Content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
                        //{
                        //    response = await client.PostAsync(FullApiUrl, Content);
                        //}

                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri(FullApiUrl),
                            Content = new StringContent(jsonString, Encoding.UTF8, "application/json"),
                        };

                        var response = client.Send(request);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                EEBankListResult result = new EEBankListResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);
                             
                                JsonElement resultvalue = (JsonElement)values["result"];
                                
                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();


                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";

                                    string Temp = values["data"].ToString();

                                    result.Result = Temp;
                                }
                                else
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "deposit error";
                                }

                                return result;
                            }
                            else
                            {
                                EEBankListResult result = new EEBankListResult();

                                result.LGTResult = "9998";

                                result.LGTRemarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            EEBankListResult result = new EEBankListResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EEBankListResult result = new EEBankListResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<EEGetwayResult> EEGetPaymentGetway()
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var FullApiUrl = $"{ApiUrl}payment-gateways";

                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri(FullApiUrl),
                            Content = new StringContent("", Encoding.UTF8, "application/json"),
                        };

                        var response = client.Send(request);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                EEGetwayResult result = new EEGetwayResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement resultvalue = (JsonElement)values["result"];

                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();


                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";

                                    string Temp = values["data"].ToString();

                                    var info = JsonSerializer.Deserialize<List<JsonElement>>(Temp);

                                    List<string> TempList = new List<string>();

                                    for (int i = 0; i < info.Count; i++)
                                    {
                                        TempList.Add(info[i].GetProperty("name").ToString());
                                    }

                                    for (int i = 0; i < TempList.Count; i++)
                                    {
                                        result.Result.Add(TempList[i]);
                                    }
                                }
                                else
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "deposit error";
                                }

                                return result;
                            }
                            else
                            {
                                EEGetwayResult result = new EEGetwayResult();

                                result.LGTResult = "9998";

                                result.LGTRemarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            EEGetwayResult result = new EEGetwayResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EEGetwayResult result = new EEGetwayResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<EEGetwayResult> EEGetWithdrawalGetway()
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}payment-gateways/withdrawal";

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        response = await client.GetAsync(FullApiUrl);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                EEGetwayResult result = new EEGetwayResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                JsonElement resultvalue = (JsonElement)values["result"];

                                result.success = (resultvalue.GetProperty("success").ToString() == null) ? "" : resultvalue.GetProperty("success").ToString();
                                result.code = (resultvalue.GetProperty("code").ToString() == null) ? "" : resultvalue.GetProperty("code").ToString();
                                result.message = (resultvalue.GetProperty("message").ToString() == null) ? "" : resultvalue.GetProperty("message").ToString();


                                if (result.success == "True")
                                {
                                    result.LGTResult = "10002";

                                    string Temp = values["data"].ToString();

                                    var info = JsonSerializer.Deserialize<List<JsonElement>>(Temp);

                                    List<string> TempList = new List<string>();

                                    for (int i = 0; i < info.Count; i++)
                                    {
                                        TempList.Add(info[i].GetProperty("name").ToString());
                                    }

                                    for (int i = 0; i < TempList.Count; i++)
                                    {
                                        result.Result.Add(TempList[i]);
                                    }
                                }
                                else
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "deposit error";
                                }

                                return result;
                            }
                            else
                            {
                                EEGetwayResult result = new EEGetwayResult();

                                result.LGTResult = "9998";

                                result.LGTRemarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            EEGetwayResult result = new EEGetwayResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EEGetwayResult result = new EEGetwayResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<EESupportToolResult> EEGetSupportToolList()
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}support-tool";

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        response = await client.GetAsync(FullApiUrl);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                var result = JsonSerializer.Deserialize<EESupportToolResult>(strResult);

                                return result;
                            }
                            else
                            {
                                EESupportToolResult result = new EESupportToolResult();

                                return result;
                            }
                        }
                        else
                        {
                            EESupportToolResult result = new EESupportToolResult();

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        EESupportToolResult result = new EESupportToolResult();

                        return result;
                    }
                }
            }
        }
    }
}
