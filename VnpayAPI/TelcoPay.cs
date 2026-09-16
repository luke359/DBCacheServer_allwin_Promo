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

namespace VnpayAPI
{
    public class TelcoLoginRequest
    {
        public string PartnerID { get; set; }
        public string SecretKey { get; set; }
    }

    public class TelcoLoginResult
    {
        public string UserID { get; set; }
        public string LoginID { get; set; }
        public string PartnerID { get; set; }
        public string SecretKey { get; set; }
        public string AgentID { get; set; }
        public string Token { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }

        public TelcoLoginResult()
        {
            UserID = "";
            LoginID = "";
            PartnerID = "";
            SecretKey = "";
            AgentID = "";
            Token = "";
            Result = "";
            Remarks = "";
        }
    }

    public class TelcoSendPinRequest
    {
        public string Reference { get; set; }
        public string GatewayOperator { get; set; }
        public string Pin { get; set; }
        public string Remark { get; set; }
        public string UserID { get; set; }
        public string Token { get; set; }
    }

    public class TelcoSendPinResult
    {
        public string TransactionKey { get; set; }
        public string Reference { get; set; }
        public string GatewayOperator { get; set; }
        public string Pin { get; set; }
        public string Result { get; set; }
        public string Balance { get; set; }
        public string VoucherFaceValue { get; set; }
        public string CreateDate { get; set; }
        public string SubmitPinDate { get; set; }
        public string ResultDate { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string Complete { get; set; }
        public string UserID { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public string StatusType { get; set; }
        public string LGTResult { get; set; }
        public string LGTRemarks { get; set; }

        public TelcoSendPinResult()
        {
            TransactionKey = "";
            Reference = "";
            GatewayOperator = "";
            Pin = "";
            Result = "";
            Balance = "";
            VoucherFaceValue = "";
            CreateDate = "";
            SubmitPinDate = "";
            ResultDate = "";
            Status = "";
            Remark = "";
            Complete = "";
            UserID = "";
            Token = "";
            Message = "";
            StatusType = "";
            LGTResult = "";
            LGTRemarks = "";
        }
    }

    public class TelcoGetResultRequest
    {
        public string TransactionKey { get; set; }
    }

    public class TelcoPay
    {
        public static async Task<TelcoLoginResult> TelcoLogin(string ProjectID, string SecretKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("PartnerID", ProjectID);
            PostContent.Add("SecretKey", SecretKey);

            var userdepositParameter = new TelcoLoginRequest
            {
                PartnerID = PostContent["PartnerID"],
                SecretKey = PostContent["SecretKey"],
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(userdepositParameter);

                        string ApiUrl = $"https://api.telcopayment.com/api/login";

                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}";

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
                                TelcoLoginResult result = new TelcoLoginResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.UserID = values["userID"].ToString();
                                result.LoginID = values["loginID"].ToString();
                                result.PartnerID = values["partnerID"].ToString();
                                result.SecretKey = values["secretKey"].ToString();
                                result.AgentID = values["agentID"].ToString();
                                result.Token = values["token"].ToString();

                                result.Result = "10001";

                                return result;
                            }
                            else
                            {
                                TelcoLoginResult result = new TelcoLoginResult();

                                result.Result = "9998";

                                result.Remarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            TelcoLoginResult result = new TelcoLoginResult();

                            result.Result = "9998";

                            result.Remarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        TelcoLoginResult result = new TelcoLoginResult();

                        result.Result = "9999";

                        result.Remarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<TelcoSendPinResult> TelcoSendPin(string ProjectID, string SecretKey, string TransNumber, string Gateway, string PinCode, string Remark)
        {
            var task = TelcoLogin(ProjectID, SecretKey);

            TelcoLoginResult telcologinresult = task.Result;

            if (telcologinresult.Result == "10001" && telcologinresult.Token != "")
            {
                Dictionary<string, string> PostContent = new Dictionary<string, string>();

                PostContent.Add("Reference", TransNumber);
                PostContent.Add("GatewayOperator", Gateway);
                PostContent.Add("Pin", PinCode);
                PostContent.Add("Remark", Remark);
                PostContent.Add("UserID", telcologinresult.UserID);
                PostContent.Add("Token", telcologinresult.Token);

                var userdepositParameter = new TelcoSendPinRequest
                {
                    Reference = PostContent["Reference"],
                    GatewayOperator = PostContent["GatewayOperator"],
                    Pin = PostContent["Pin"],
                    Remark = PostContent["Remark"],
                    UserID = PostContent["UserID"],
                    Token = PostContent["Token"]
                };

                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    using (HttpClient client = new HttpClient(handler))
                    {
                        try
                        {
                            var jsonString = JsonSerializer.Serialize(userdepositParameter);

                            string ApiUrl = $"https://api.telcopayment.com/api/transaction";

                            HttpResponseMessage response = null;

                            var FullApiUrl = $"{ApiUrl}";

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
                                    TelcoSendPinResult result = new TelcoSendPinResult();

                                    var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                    result.TransactionKey = (values["transactionKey"] == null) ? "" : values["transactionKey"].ToString();
                                    result.Reference = (values["reference"] == null) ? "" : values["reference"].ToString();
                                    result.GatewayOperator = (values["gatewayOperator"] == null) ? "" : values["gatewayOperator"].ToString();
                                    result.Pin = (values["pin"] == null) ? "" : values["pin"].ToString();
                                    result.Result = (values["result"] == null) ? "" : values["result"].ToString();
                                    result.Balance = (values["balance"] == null) ? "" : values["balance"].ToString();
                                    result.VoucherFaceValue = (values["voucherFaceValue"] == null) ? "" : values["voucherFaceValue"].ToString();
                                    result.CreateDate = (values["createDate"] == null) ? "" : values["createDate"].ToString();
                                    result.SubmitPinDate = (values["submitPinDate"] == null) ? "" : values["submitPinDate"].ToString();
                                    result.ResultDate = (values["resultDate"] == null) ? "" : values["resultDate"].ToString();
                                    result.Status = (values["status"] == null) ? "" : values["status"].ToString();
                                    result.Remark = (values["remark"] == null) ? "" : values["remark"].ToString();
                                    result.Complete = (values["complete"] == null) ? "" : values["complete"].ToString();
                                    result.UserID = (values["userID"] == null) ? "" : values["userID"].ToString();
                                    result.Token = (values["token"] == null) ? "" : values["token"].ToString();
                                    result.Message = (values["message"] == null) ? "" : values["message"].ToString();
                                    result.StatusType = (values["statusType"] == null) ? "" : values["statusType"].ToString();

                                    if (values["status"].ToString() == "Success")
                                    {
                                        result.LGTResult = "10001";
                                    }
                                    else if (values["status"].ToString() == "Unknown")
                                    {
                                        if (values["complete"].ToString() == "False")
                                        {
                                            result.LGTResult = "10002";
                                        }
                                    }
                                    else if (values["status"].ToString() == "Failed")
                                    {
                                        result.LGTResult = "9996";

                                        result.LGTRemarks = "send pin error";
                                    }

                                    return result;
                                }
                                else
                                {
                                    TelcoSendPinResult result = new TelcoSendPinResult();

                                    result.LGTResult = "9998";

                                    result.LGTRemarks = "post connection status error";

                                    return result;
                                }
                            }
                            else
                            {
                                TelcoSendPinResult result = new TelcoSendPinResult();

                                result.LGTResult = "9998";

                                result.LGTRemarks = "post connection status error";

                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            TelcoSendPinResult result = new TelcoSendPinResult();

                            result.LGTResult = "9999";

                            result.LGTRemarks = ex.Message;

                            return result;
                        }
                    }
                }
            }
            else
            {
                TelcoSendPinResult result = new TelcoSendPinResult();

                result.LGTResult = "9997";

                result.LGTRemarks = "telco login error";

                return result;
            }
        }

        public static async Task<TelcoSendPinResult> TelcoGetResult(string TransactionKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("TransactionKey", TransactionKey);

            var userdepositParameter = new TelcoGetResultRequest
            {
                TransactionKey = PostContent["TransactionKey"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(userdepositParameter);

                        string ApiUrl = $"https://api.telcopayment.com/api/transaction/"+"{"+TransactionKey+"}";

                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}";

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
                                TelcoSendPinResult result = new TelcoSendPinResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.TransactionKey = (values["transactionKey"] == null) ? "" : values["transactionKey"].ToString();
                                result.Reference = (values["reference"] == null) ? "" : values["reference"].ToString();
                                result.GatewayOperator = (values["gatewayOperator"] == null) ? "" : values["gatewayOperator"].ToString();
                                result.Pin = (values["pin"] == null) ? "" : values["pin"].ToString();
                                result.Result = (values["result"] == null) ? "" : values["result"].ToString();
                                result.Balance = (values["balance"] == null) ? "" : values["balance"].ToString();
                                result.VoucherFaceValue = (values["voucherFaceValue"] == null) ? "" : values["voucherFaceValue"].ToString();
                                result.CreateDate = (values["createDate"] == null) ? "" : values["createDate"].ToString();
                                result.SubmitPinDate = (values["submitPinDate"] == null) ? "" : values["submitPinDate"].ToString();
                                result.ResultDate = (values["resultDate"] == null) ? "" : values["resultDate"].ToString();
                                result.Status = (values["status"] == null) ? "" : values["status"].ToString();
                                result.Remark = (values["remark"] == null) ? "" : values["remark"].ToString();
                                result.Complete = (values["complete"] == null) ? "" : values["complete"].ToString();
                                result.UserID = (values["userID"] == null) ? "" : values["userID"].ToString();
                                result.Token = (values["token"] == null) ? "" : values["token"].ToString();
                                result.Message = (values["message"] == null) ? "" : values["message"].ToString();
                                result.StatusType = (values["statusType"] == null) ? "" : values["statusType"].ToString();

                                if (values["status"].ToString() == "Success")
                                {
                                    result.LGTResult = "10001";
                                }
                                else if (values["status"].ToString() == "Unknown")
                                {
                                    if (values["complete"].ToString() == "False")
                                    {
                                        result.LGTResult = "10002";
                                    }
                                }
                                else if (values["status"].ToString() == "Failed")
                                {
                                    result.LGTResult = "9996";

                                    result.LGTRemarks = "send pin error";
                                }

                                return result;
                            }
                            else
                            {
                                TelcoSendPinResult result = new TelcoSendPinResult();

                                result.LGTResult = "9998";

                                result.LGTRemarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            TelcoSendPinResult result = new TelcoSendPinResult();

                            result.LGTResult = "9998";

                            result.LGTRemarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        TelcoSendPinResult result = new TelcoSendPinResult();

                        result.LGTResult = "9999";

                        result.LGTRemarks = ex.Message;

                        return result;
                    }
                }
            }
        }

    }
}
