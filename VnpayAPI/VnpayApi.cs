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
    public class VnpayIDInfo
    {

        public string ProjectID;

        public string SecretKey;

        public string ProductKey;

        public string MerchantCode;

        public string MerchantSecretKey;

        public VnpayIDInfo()
        {
            ProjectID = "";
            SecretKey = "";
            ProductKey = "";
            MerchantCode = "";
            MerchantSecretKey = "";
        }
    }

    public class TelcoIDInfo
    {

        public string ProjectID;

        public string SecretKey;

        public string Discount;

        public TelcoIDInfo()
        {
            ProjectID = "";
            SecretKey = "";
            Discount = "";
        }
    }

    public class VnpayDepositInfo
    {
        public int UserUID;

        public double Amount;

        public string Bank_Code;

        public string Bank_Account_Name;

        public string Bank_Account_Number;

        public string Client_IP_Address;

        public string PaymentType;

        public string Gateway;

        public string PinCode;

        public VnpayDepositInfo()
        {
            UserUID = 0;
            Amount = 0.00;
            Bank_Code = "";
            Bank_Account_Name = "";
            Bank_Account_Number = "";
            Client_IP_Address = "";
            PaymentType = "";
        }
    }

    public class VnpayPayoutInfo
    {
        public int UserUID;

        public double Amount;

        public string Bank_Code;

        public string Bank_Account_Name;

        public string Bank_Account_Number;

        public string Client_IP_Address;

        public string PaymentType;

        public string PhoneNumber;

        public VnpayPayoutInfo()
        {
            UserUID = 0;
            Amount = 0.00;
            Bank_Code = "";
            Bank_Account_Name = "";
            Bank_Account_Number = "";
            Client_IP_Address = "";
            PaymentType = "";
            PhoneNumber = "";
        }
    }

    public class VnpayResultCheckInfo
    {
        public int UserUID;

        public VnpayResultCheckInfo()
        {
            UserUID = 0;
        }
    }

    public class CheckBalanceParameter
    {
        public string project_id { get; set; }
        public string token { get; set; }
    }

    public class CheckTransactionParameter
    {
        public string project_id { get; set; }
        public string token { get; set; }
        public string transaction_type { get; set; }
        public string transaction_id { get; set; }
    }

    public class UserDepositParameter
    {
        public string callback_url { get; set; }
        public string client_transaction { get; set; }
        public string project_id { get; set; }
        public string token { get; set; }
        public string amount { get; set; }
        public string currency_code { get; set; }
        public string client_ip_address { get; set; }
    }

    public class PEUserDepositParameter
    {
        public string MerchantCode { get; set; }
        public string TransNum { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string PaymentDesc { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNum { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string MerchantRemark { get; set; }
        public string Signature { get; set; }
    }

    public class PEDepositCheckParameter
    {
        public string MerchantCode { get; set; }
        public string MerchantTransNum { get; set; }
        public string CheckString { get; set; }
    }


    public class UserPayoutParameter
    {
        public string callback_url { get; set; }
        public string project_id { get; set; }
        public string token { get; set; }
        public string bank_code { get; set; }
        public string bank_account_name { get; set; }
        public string bank_account_number { get; set; }
        public string amount { get; set; }
        public string currency_code { get; set; }
        public string client_transaction { get; set; }
    }

    public class APIResult
    {
        public string status { get; set; }
        public string ret_msg { get; set; }
        public Dictionary<string, string> data { get; set; }

        public APIResult()
        {
            status = "";
            ret_msg = "";
            data = new Dictionary<string, string>();
        }
    }

    public class VnpayApi
    {
        private string DomainName;

        public VnpayApi()
        {
            DomainName = "https://kimpays.shop";
        }

        private static string GetMD5_32(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < t.Length; i++)
            {
                stringBuilder.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString().ToUpper();
        }

        public static string GetVnpayToken(Dictionary<string, string> data, string SecretKey)
        {
            List<string> KeyList = new List<string>();

            string content = "";

            foreach (string s in data.Keys)
            {
                KeyList.Add(s);
            }

            KeyList.Sort((a, b) => a.CompareTo(b));

            for (int i = 0; i < KeyList.Count; i++)
            {
                if (KeyList[i] != "token")
                {
                    if (i != 0 && i != KeyList.Count)
                    {
                        content += "&";
                    }

                    string temp = WebUtility.UrlEncode(data[KeyList[i]]).Replace("+", "%20");

                    content += KeyList[i] + "=" + temp;
                }
            }

            content += SecretKey;

            string EncryptTxt = GetMD5_32(content);

            return EncryptTxt;
        }

        public static VnpayDepositInfo GetVnpayDepositInfo(string Msg)
        {
            VnpayDepositInfo Sendata = new VnpayDepositInfo();

            try
            {
                string[] data = Msg.Split(',');

                Sendata.PaymentType = data[0].Replace("PaymentType:", "");

                if (Sendata.PaymentType == "PayEssence")
                {
                    Sendata.UserUID = Int32.Parse(data[1].Replace("UserUID:", ""));
                    Sendata.Amount = double.Parse(data[2].Replace("Amount:", ""));
                    Sendata.Client_IP_Address = data[3].Replace("IP_Address:", "");
                }
                else if (Sendata.PaymentType == "TelcoPayment")
                {
                    Sendata.UserUID = Int32.Parse(data[1].Replace("UserUID:", ""));
                    Sendata.Gateway = data[2].Replace("Gateway:", "");
                    Sendata.PinCode = data[3].Replace("Number:", "");
                    Sendata.Client_IP_Address = data[4].Replace("IP_Address:", "");
                }
                else if (Sendata.PaymentType == "ThaiJustPay" || Sendata.PaymentType == "PaymentPro" || Sendata.PaymentType == "BitPayZ" ||
                    Sendata.PaymentType == "HeroPay")
                {
                    Sendata.UserUID = Int32.Parse(data[1].Replace("UserUID:", ""));
                    Sendata.Amount = double.Parse(data[2].Replace("Amount:", ""));
                    Sendata.Bank_Code = data[3].Replace("Bank_Code:", "");
                    Sendata.Bank_Account_Name = data[4].Replace("Bank_Account_Name:", "");
                    Sendata.Bank_Account_Number = data[5].Replace("Bank_Account_Number:", "");
                    Sendata.Client_IP_Address = data[6].Replace("IP_Address:", "");
                }
                else if (Sendata.PaymentType == "i1bayar" || Sendata.PaymentType == "BankTransfer")
                {
                    Sendata.UserUID = Int32.Parse(data[1].Replace("UserUID:", ""));
                    Sendata.Amount = double.Parse(data[2].Replace("Amount:", ""));
                    Sendata.Client_IP_Address = data[3].Replace("IP_Address:", "");
                }
            }
            catch (Exception ex)
            {
                Sendata = new VnpayDepositInfo();
                //MyConsole.WriteLine("VnpayDepositInfo transform failed")
            }

            return Sendata;
        }

        public static VnpayPayoutInfo GetVnpayPayoutInfo(string Msg)
        {
            VnpayPayoutInfo Sendata = new VnpayPayoutInfo();

            try
            {
                string[] data = Msg.Split(',');

                Sendata.PaymentType = data[0].Replace("PaymentType:", "");

                //if (Sendata.PaymentType == "PayEssence")
                //{
                    Sendata.UserUID = Int32.Parse(data[1].Replace("UserUID:", ""));
                    Sendata.Amount = double.Parse(data[2].Replace("Amount:", ""));
                    Sendata.Bank_Code = data[3].Replace("Bank_Code:", "");
                    Sendata.Bank_Account_Name = data[4].Replace("Bank_Account_Name:", "");
                    Sendata.Bank_Account_Number = data[5].Replace("Bank_Account_Number:", "");
                    Sendata.PhoneNumber = data[6].Replace("PhoneNumber:", "");
                    Sendata.Client_IP_Address = data[7].Replace("IP_Address:", "");
                //}
            }
            catch (Exception ex)
            {
                Sendata = new VnpayPayoutInfo();
                //MyConsole.WriteLine("VnpayDepositInfo transform failed")
            }

            return Sendata;
        }

        public static VnpayResultCheckInfo GetVnpayResultCheckInfo(string Msg)
        {
            VnpayResultCheckInfo Sendata = new VnpayResultCheckInfo();

            try
            {
                string[] data = Msg.Split(',');

                Sendata.UserUID = Int32.Parse(data[0].Replace("UserUID:", ""));
            }
            catch (Exception ex)
            {
                Sendata = new VnpayResultCheckInfo();
                //MyConsole.WriteLine("VnpayDepositInfo transform failed")
            }

            return Sendata;
        }

        public static async Task<APIResult> UserDeposit(string ClientTransaction, string ProjectID, decimal Amount, string CurrencyCode, string ClientIPAddress, string SecretKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            //PostContent.Add("callback_url", "https://api.vip-888.cc/api/v1/DepositCallBack");
            //PostContent.Add("callback_url", "https://api.fun-game.info/api/v1/DepositCallBack");
            PostContent.Add("callback_url", "https://api.96kim.club/api/v1/DepositCallBack");
            PostContent.Add("client_transaction", ClientTransaction);
            PostContent.Add("project_id", ProjectID);
            PostContent.Add("amount", Amount.ToString("f2"));
            PostContent.Add("currency_code", CurrencyCode);
            PostContent.Add("client_ip_address", ClientIPAddress);


            string Token = GetVnpayToken(PostContent, SecretKey);

            var userdepositParameter = new UserDepositParameter
            {
                callback_url = PostContent["callback_url"],
                client_transaction = PostContent["client_transaction"],
                project_id = PostContent["project_id"],
                token = Token,
                amount = PostContent["amount"],
                currency_code = PostContent["currency_code"],
                client_ip_address = PostContent["client_ip_address"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(userdepositParameter);

                        string ApiUrl = $"https://kimpays.shop/api/v2/deposit";

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

                                APIResult result = new APIResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.status = values["status"].ToString();

                                result.ret_msg = values["ret_msg"].ToString();

                                string Temp = values["data"].ToString();

                                var Info = JsonSerializer.Deserialize<Dictionary<string, Object>>(Temp);

                                Dictionary<string, string> data = new Dictionary<string, string>();

                                foreach (KeyValuePair<string, Object> v in Info)
                                {
                                    if (v.Value != null)
                                    {
                                        result.data.Add(v.Key, v.Value.ToString());
                                    }
                                    else
                                    {
                                        result.data.Add(v.Key, "");
                                    }
                                }

                                return result;
                            }
                            else
                            {
                                APIResult result = new APIResult();

                                result.status = "9998";

                                result.ret_msg = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            APIResult result = new APIResult();

                            result.status = "9998";

                            result.ret_msg = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        APIResult result = new APIResult();

                        result.status = "9999";

                        result.ret_msg = ex.Message;

                        return result;
                    }
                }
            }

        }

        public static async Task<APIResult> UserPayout(string ProjectID, string BankCode, string BankAccountName, string BankAccountNumber, decimal Amount, string CurrencyCode, string ClientTransaction, string SecretKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();
            
            //PostContent.Add("callback_url", "https://api.vip-888.cc/api/v1/PayoutCallBack");
            //PostContent.Add("callback_url", "https://api.fun-game.info/api/v1/PayoutCallBack");
            PostContent.Add("callback_url", "https://api.96kim.club/api/v1/PayoutCallBack");
            PostContent.Add("project_id", ProjectID);
            PostContent.Add("bank_code", BankCode);
            PostContent.Add("bank_account_name", BankAccountName);
            PostContent.Add("bank_account_number", BankAccountNumber);
            PostContent.Add("amount", Amount.ToString("f2"));
            PostContent.Add("currency_code", CurrencyCode);
            PostContent.Add("client_transaction", ClientTransaction);


            string Token = GetVnpayToken(PostContent, SecretKey);

            var userpayoutParameter = new UserPayoutParameter
            {
                callback_url = PostContent["callback_url"],
                project_id = PostContent["project_id"],
                token = Token,
                bank_code = PostContent["bank_code"],
                bank_account_name = PostContent["bank_account_name"],
                bank_account_number = PostContent["bank_account_number"],
                amount = PostContent["amount"],
                currency_code = PostContent["currency_code"],
                client_transaction = PostContent["client_transaction"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(userpayoutParameter);

                        string ApiUrl = $"https://kimpays.shop/api/v2/payout";

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

                                APIResult result = new APIResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.status = values["status"].ToString();

                                result.ret_msg = values["ret_msg"].ToString();

                                string Temp = values["data"].ToString();

                                var Info = JsonSerializer.Deserialize<Dictionary<string, Object>>(Temp);

                                Dictionary<string, string> data = new Dictionary<string, string>();

                                foreach (KeyValuePair<string, Object> v in Info)
                                {
                                    if (v.Value != null)
                                    {
                                        result.data.Add(v.Key, v.Value.ToString());
                                    }
                                    else
                                    {
                                        result.data.Add(v.Key, "");
                                    }
                                }

                                return result;
                            }
                            else
                            {
                                APIResult result = new APIResult();

                                result.status = "9998";

                                result.ret_msg = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            APIResult result = new APIResult();

                            result.status = "9998";

                            result.ret_msg = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        APIResult result = new APIResult();

                        result.status = "9999";

                        result.ret_msg = ex.Message;

                        return result;
                    }
                }
            }

        }

        public static async Task<PEPayoutResult> UserPEPayout(string ProjectID, string BankCode, string BankAccountName, string BankAccountNumber, double Amount, string CurrencyCode, string ClientTransaction, string SecretKey)
        {
            string orgvalue = "M2203124" + "da23c564-bb80-49ae-ab51-0412869e06c1" + "LGTPT00000500" + "1" + "7533967" + "1" + "110.00";
            string newvalue = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(orgvalue));
                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                newvalue = builder.ToString();
            }

            Dictionary<string, string> PostContent = new Dictionary<string, string>();
            
            PostContent.Add("MerchantCode", "M2203124");
            PostContent.Add("MerchantTransNum", "LGTPT00000500");
            PostContent.Add("CurrencyTypeId", "1");
            PostContent.Add("Amount", "110.00");
            PostContent.Add("ToBankId", "1");
            PostContent.Add("ToBankAccountName", "Joebar0001");
            PostContent.Add("ToBankAccountNum", "7533967");
            PostContent.Add("Signature", newvalue);

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        string ApiUrl = $"https://job1-demo.mxpay.asia/WebForm/Deposit_P2pPayout2_B2b.aspx";
                                           
                        HttpResponseMessage response = null;

                        var FullApiUrl = $"{ApiUrl}";

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        using (var content = new MultipartFormDataContent())
                        {
                            foreach (var keyValuePair in PostContent)
                            {
                                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                            }

                            response = await client.PostAsync(ApiUrl, content);
                        }

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();

                                PEPayoutResult result = new PEPayoutResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.PayoutId = values["PayoutId"].ToString();
                                result.Result = values["Result"].ToString();
                                result.MerchantCode = values["MerchantCode"].ToString();
                                result.TransNum = values["TransNum"].ToString();
                                result.CurrencyTypeId = values["CurrencyTypeId"].ToString();
                                result.ToBankId = values["ToBankId"].ToString();
                                result.ToBankAccountNum = values["ToBankAccountNum"].ToString();
                                result.Amount = values["Amount"].ToString();
                                result.Remarks = values["Remarks"].ToString();
                                result.CheckString2 = values["CheckString2"].ToString();

                                //Dictionary<string, string> data = new Dictionary<string, string>();

                                //foreach (KeyValuePair<string, Object> v in values)
                                //{
                                //    if (v.Value != null)
                                //    {
                                //        data.Add(v.Key, v.Value.ToString());
                                //    }
                                //    else
                                //    {
                                //        data.Add(v.Key, "");
                                //    }
                                //}

                                //string PayoutId = values["PayoutId"].ToString();

                                //result.status = values["status"].ToString();

                                //result.ret_msg = values["ret_msg"].ToString();

                                //string Temp = values["data"].ToString();

                                //var Info = JsonSerializer.Deserialize<Dictionary<string, Object>>(Temp);

                                //Dictionary<string, string> data = new Dictionary<string, string>();

                                //foreach (KeyValuePair<string, Object> v in Info)
                                //{
                                //    if (v.Value != null)
                                //    {
                                //        result.data.Add(v.Key, v.Value.ToString());
                                //    }
                                //    else
                                //    {
                                //        result.data.Add(v.Key, "");
                                //    }
                                //}

                                return result;
                            }
                            else
                            {
                                PEPayoutResult result = new PEPayoutResult();

                                result.Result = "9998";

                                result.Remarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            PEPayoutResult result = new PEPayoutResult();

                            result.Result = "9998";

                            result.Remarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        PEPayoutResult result = new PEPayoutResult();

                        result.Result = "9999";

                        result.Remarks = ex.Message;

                        return result;
                    }
                }
            }

        }

        public static async Task<APIResult> CheckDeposit(string ProjectID, string Transaction_ID, string SecretKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("project_id", ProjectID);
            PostContent.Add("transaction_type", "DEPOSIT");
            PostContent.Add("transaction_id", Transaction_ID);

            string Token = GetVnpayToken(PostContent, SecretKey);

            var checktransactionParameter = new CheckTransactionParameter
            {
                project_id = PostContent["project_id"],
                token = Token,
                transaction_type = PostContent["transaction_type"],
                transaction_id = PostContent["transaction_id"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(checktransactionParameter);

                        string ApiUrl = $"https://kimpays.shop/api/v2/check_transaction";

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

                                APIResult result = new APIResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.status = values["status"].ToString();

                                result.ret_msg = values["ret_msg"].ToString();

                                string Temp = values["data"].ToString();

                                var Info = JsonSerializer.Deserialize<Dictionary<string, Object>>(Temp);

                                Dictionary<string, string> data = new Dictionary<string, string>();

                                foreach (KeyValuePair<string, Object> v in Info)
                                {
                                    if (v.Value != null)
                                    {
                                        result.data.Add(v.Key, v.Value.ToString());
                                    }
                                    else
                                    {
                                        result.data.Add(v.Key, "");
                                    }
                                }

                                return result;
                            }
                            else
                            {
                                APIResult result = new APIResult();

                                result.status = "9998";

                                result.ret_msg = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            APIResult result = new APIResult();

                            result.status = "9998";

                            result.ret_msg = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        APIResult result = new APIResult();

                        result.status = "9999";

                        result.ret_msg = ex.Message;

                        return result;
                    }
                }
            }

        }

        public static async Task<APIResult> CheckPayout(string ProjectID, string Transaction_ID, string SecretKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("project_id", ProjectID);
            PostContent.Add("transaction_type", "PAYOUT");
            PostContent.Add("transaction_id", Transaction_ID);

            string Token = GetVnpayToken(PostContent, SecretKey);

            var checktransactionParameter = new CheckTransactionParameter
            {
                project_id = PostContent["project_id"],
                token = Token,
                transaction_type = PostContent["transaction_type"],
                transaction_id = PostContent["transaction_id"]
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(checktransactionParameter);

                        string ApiUrl = $"https://kimpays.shop/api/v2/check_transaction";

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

                                APIResult result = new APIResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.status = values["status"].ToString();

                                result.ret_msg = values["ret_msg"].ToString();

                                string Temp = values["data"].ToString();

                                var Info = JsonSerializer.Deserialize<Dictionary<string, Object>>(Temp);

                                Dictionary<string, string> data = new Dictionary<string, string>();

                                foreach (KeyValuePair<string, Object> v in Info)
                                {
                                    if (v.Value != null)
                                    {
                                        result.data.Add(v.Key, v.Value.ToString());
                                    }
                                    else
                                    {
                                        result.data.Add(v.Key, "");
                                    }
                                }

                                return result;
                            }
                            else
                            {
                                APIResult result = new APIResult();

                                result.status = "9998";

                                result.ret_msg = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            APIResult result = new APIResult();

                            result.status = "9998";

                            result.ret_msg = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        APIResult result = new APIResult();

                        result.status = "9999";

                        result.ret_msg = ex.Message;

                        return result;
                    }
                }
            }

        }

        public static async Task<APIResult> CheckBalance(string ProjectID, string SecretKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("project_id", ProjectID);

            string Token = GetVnpayToken(PostContent, SecretKey);

            var checkbalanceParameter = new CheckBalanceParameter
            {
                project_id = ProjectID,
                token = Token
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(checkbalanceParameter);

                        string ApiUrl = $"https://kimpays.shop/api/v2/check_balance";

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

                                APIResult result = new APIResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.status = values["status"].ToString();

                                result.ret_msg = values["ret_msg"].ToString();

                                string Temp = values["data"].ToString();

                                var Info = JsonSerializer.Deserialize<Dictionary<string, Object>>(Temp);

                                Dictionary<string, string> data = new Dictionary<string, string>();

                                foreach (KeyValuePair<string, Object> v in Info)
                                {
                                    result.data.Add(v.Key, v.Value.ToString());
                                }

                                return result;
                            }
                            else
                            {
                                APIResult result = new APIResult();

                                result.status = "9998";

                                result.ret_msg = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            APIResult result = new APIResult();

                            result.status = "9998";

                            result.ret_msg = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        APIResult result = new APIResult();

                        result.status = "9999";

                        result.ret_msg = ex.Message;

                        return result;
                    }
                }
            }

        }

        public static async Task<APIResult> PayoutBankList(string ProjectID, string SecretKey)
        {
            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("project_id", ProjectID);

            string Token = GetVnpayToken(PostContent, SecretKey);

            var checkbalanceParameter = new CheckBalanceParameter
            {
                project_id = ProjectID,
                token = Token
            };

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        var jsonString = JsonSerializer.Serialize(checkbalanceParameter);

                        string ApiUrl = $"https://kimpays.shop/api/v2/payout_bank/list";

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

                                APIResult result = new APIResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.status = values["status"].ToString();

                                result.ret_msg = values["ret_msg"].ToString();

                                string Temp = values["data"].ToString();

                                var Info = JsonSerializer.Deserialize<Dictionary<string, Object>>(Temp);

                                Dictionary<string, string> data = new Dictionary<string, string>();

                                foreach (KeyValuePair<string, Object> v in Info)
                                {
                                    result.data.Add(v.Key, v.Value.ToString());
                                }

                                return result;
                            }
                            else
                            {
                                APIResult result = new APIResult();

                                result.status = "9998";

                                result.ret_msg = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            APIResult result = new APIResult();

                            result.status = "9998";

                            result.ret_msg = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        APIResult result = new APIResult();

                        result.status = "9999";

                        result.ret_msg = ex.Message;

                        return result;
                    }
                }
            }

        }
    }

    public static class HttpClientProvider
    {
        // 使用靜態 readonly 字段確保只會建立一次 HttpClient 實例
        public static readonly HttpClient Client = new HttpClient();

        // 若有需要，也可在此設定預設請求參數、逾時時間等
         static HttpClientProvider()
         {
             Client.Timeout = TimeSpan.FromSeconds(15); //設定請求超時為 15 秒
         }
    }
}
