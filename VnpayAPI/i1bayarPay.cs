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

namespace VnpayAPI
{
    public class i1bayarRequest
    {
        public string ShopName { get; set; }
        public string ProductName { get; set; }
        public string SenderId { get; set; }
        public string Amount { get; set; }
        public string RedirectUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string Param { get; set; }
    }

    public class i1bayarDepositResult
    {
        public string Amount { get; set; }
        public string BillId { get; set; }
        public string BillUrl { get; set; }
        public string BuyerId { get; set; }
        public string Date { get; set; }
        public string Paid { get; set; }
        public string Param { get; set; }
        public string ProductName { get; set; }
        public string ShopName { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public i1bayarDepositResult()
        {
            Amount      = "";
            BillId      = "";
            BillUrl     = "";
            BuyerId     = "";
            Date        = "";
            Paid        = "";
            Param       = "";
            ProductName = "";
            ShopName    = "";
            LGTResult   = "";
            LGTRemarks  = "";
        }
    }

    public class i1bayarPay
    {
        public static async Task<i1bayarDepositResult> i1bayarDeposit(string PaymentKey, string PaymentPass, string Player, string Agent, string TransNumber, string Amount)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("ShopName", Agent);
            PostContent.Add("ProductName", "Deposit");
            PostContent.Add("SenderId", Player);
            PostContent.Add("Amount", Amount);
            PostContent.Add("RedirectUrl", "");
            PostContent.Add("CallbackUrl", "https://callback.win888.work/api/v1/i1bayarDepositCallback");
            PostContent.Add("Param", TransNumber);

            var i1bayarDepositParameter = new i1bayarRequest
            {
                ShopName = PostContent["ShopName"],
                ProductName = PostContent["ProductName"],
                SenderId = PostContent["SenderId"],
                Amount = PostContent["Amount"],
                RedirectUrl = PostContent["RedirectUrl"],
                CallbackUrl = PostContent["CallbackUrl"],
                Param = PostContent["Param"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {

                try
                {
                    var jsonString = JsonSerializer.Serialize(i1bayarDepositParameter);

                    string ApiUrl = $"https://i1bayar.org/WCF/Bills.svc/Create";

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    HttpClient client2 = new HttpClient();

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

                    string authorization = PaymentKey + ":" + PaymentPass;

                    request.Headers.TryAddWithoutValidation("Authorization", authorization);

                    string context = "";

                    int i = 0;

                    foreach (var keyValuePair in PostContent)
                    {
                        if (i != 0) context = context + "&";

                        context = context + keyValuePair.Key + "=" + keyValuePair.Value;

                        i++;
                    }

                    request.Content = new StringContent(context);

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
                            i1bayarDepositResult result = new i1bayarDepositResult();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            result.Amount = values["Amount"].ToString();
                            result.BillId = values["BillId"].ToString();
                            result.BillUrl = values["BillUrl"].ToString();
                            result.BuyerId = values["BuyerId"].ToString();
                            result.Date = values["Date"].ToString();
                            result.Paid = values["Paid"].ToString();
                            result.Param = values["Param"].ToString();
                            result.ProductName = values["ProductName"].ToString();
                            result.ShopName = values["ShopName"].ToString();

                            if (result.BillId != null)
                            {
                                if (result.BillId != "")
                                {
                                    result.LGTResult = "10002";
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
                            String strResult = await response.Content.ReadAsStringAsync();

                            i1bayarDepositResult result = new i1bayarDepositResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    else
                    {
                        String strResult = await response.Content.ReadAsStringAsync();

                        i1bayarDepositResult result = new i1bayarDepositResult();

                        result.LGTResult = "9998";

                        result.LGTRemarks = "post connection status error";

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    i1bayarDepositResult result = new i1bayarDepositResult();

                    result.LGTResult = "9999";

                    result.LGTRemarks = ex.Message;

                    return result;
                }
            }
        }

        public static async Task<i1bayarDepositResult> Checki1bayarDeposit(string ClientTransaction, string PaymentKey, string PaymentPass)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                try
                {
                    string ApiUrl = $"https://i1bayar.org/WCF/Bills.svc/Get/" + ClientTransaction;

                    HttpResponseMessage response = null;

                    var FullApiUrl = $"{ApiUrl}";

                    HttpClient client = new HttpClient();

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);

                    string authorization = PaymentKey + ":" + PaymentPass;

                    request.Headers.TryAddWithoutValidation("Authorization", authorization);

                    response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            String strResult = await response.Content.ReadAsStringAsync();
                            i1bayarDepositResult result = new i1bayarDepositResult();

                            var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                            result.Amount = values["Amount"].ToString();
                            result.BillId = values["BillId"].ToString();
                            result.BillUrl = values["BillUrl"].ToString();
                            result.BuyerId = values["BuyerId"].ToString();
                            result.Date = values["Date"].ToString();
                            result.Paid = values["Paid"].ToString();
                            result.Param = values["Param"].ToString();
                            result.ProductName = values["ProductName"].ToString();
                            result.ShopName = values["ShopName"].ToString();

                            return result;
                        }
                        else
                        {
                            i1bayarDepositResult result = new i1bayarDepositResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    else
                    {
                        i1bayarDepositResult result = new i1bayarDepositResult();

                        result.LGTResult = "9998";

                        result.LGTRemarks = "post connection status error";

                        return result;
                    }
                }
                catch (Exception ex)
                {

                    i1bayarDepositResult result = new i1bayarDepositResult();

                    result.LGTResult = "9999";

                    result.LGTRemarks = ex.Message;

                    return result;
                }
            }
        }
    }
}
