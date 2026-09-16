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
    public class PEIDInfo
    {
        public string MerchantCode;

        public string MerchantSecretKey;

        public PEIDInfo()
        {
            MerchantCode = "";
            MerchantSecretKey = "";
        }
    }

    public class PEPayoutResult
    {
        public string PayoutId { get; set; }
        public string Result { get; set; }
        public string MerchantCode { get; set; }
        public string TransNum { get; set; }
        public string CurrencyTypeId { get; set; }
        public string ToBankId { get; set; }
        public string ToBankAccountNum { get; set; }
        public string Amount { get; set; }
        public string Remarks { get; set; }
        public string CheckString2 { get; set; }


        public PEPayoutResult()
        {
            PayoutId = "";
            Result = "";
            MerchantCode = "";
            TransNum = "";
            CurrencyTypeId = "";
            ToBankId = "";
            ToBankAccountNum = "";
            Amount = "";
            Remarks = "";
            CheckString2 = "";
        }
    }

    public class PEDepositStatusCheckResult
    {
        public string DepositId { get; set; }
        public string MerchantCode { get; set; }
        public string TransNum { get; set; }
        public string FromBank { get; set; }
        public string ToBank { get; set; }
        public string BankAccountNum { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string MerchantFeePercentage { get; set; }
        public string MerchantFee { get; set; }
        public string PaymentDesc { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNum { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string MerchantRemark { get; set; }
        public string Message { get; set; }
        public string CheckString { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }

        public PEDepositStatusCheckResult()
        {
            DepositId = "";
            MerchantCode = "";
            TransNum = "";
            FromBank = "";
            ToBank = "";
            BankAccountNum = "";
            Currency = "";
            Amount = "";
            MerchantFeePercentage = "";
            MerchantFee = "";
            PaymentDesc = "";
            FirstName = "";
            LastName = "";
            EmailAddress = "";
            PhoneNum = "";
            Address = "";
            City = "";
            State = "";
            Country = "";
            Postcode = "";
            MerchantRemark = "";
            Message = "";
            CheckString = "";
            Result = "";
            Remarks = "";
        }
    }

    public class PEPayoutStatusCheckResult
    {
        public string Result { get; set; }
        public string Remarks { get; set; }

        public PEPayoutStatusCheckResult()
        {
            Result = "";
            Remarks = "";
        }
    }

    public class PayEssenceApi
    {
        public static string SHA256Hash(string orgvalue)
        {
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

                string newvalue = builder.ToString();

                return newvalue;
            }
        }

        public static string PEUserDeposit(string ClientTransaction, string ProjectID, decimal Amount, string CurrencyCode, string CountryCode, string ClientIPAddress, string SecretKey, string PlayerID)
        {
            string orgvalue = ProjectID + SecretKey + ClientTransaction + CurrencyCode + Amount.ToString("f2");
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

            //string urlstr = "https://api-demo.mxpay.asia/WebForm/Deposit.aspx?" + "MerchantCode=" + ProjectID
            //   + "&TransNum=" + ClientTransaction + "&Currency=MYR" + "&Amount=" + Amount.ToString("f2")
            //   + "&PaymentDesc=" + "&FirstName=" + PlayerID + "&LastName="
            //   + "&EmailAddress=" + "&PhoneNum=" + "&Address="
            //   + "&City=" + "&State=" + "&Country=" + CountryCode + "&Postcode="
            //   + "&MerchantRemark=" + "&Signature=" + newvalue;

            string urlstr = "https://api.mxpay.asia/WebForm/Deposit.aspx?" + "MerchantCode=" + ProjectID
               + "&TransNum=" + ClientTransaction + "&Currency=MYR" + "&Amount=" + Amount.ToString("f2")
               + "&PaymentDesc=" + "&FirstName=" + PlayerID + "&LastName="
               + "&EmailAddress=" + "&PhoneNum=" + "&Address="
               + "&City=" + "&State=" + "&Country=" + CountryCode + "&Postcode="
               + "&MerchantRemark=" + "&Signature=" + newvalue;

            return urlstr;
        }

        public static async Task<PEPayoutResult> PEUserPayout(string ClientTransaction, string ProjectID, string Bank_Code, string Bank_Account_Name, string Bank_Account, decimal Amount, string CurrencyCode, string ClientIPAddress, string SecretKey)
        {
            string orgvalue = ProjectID + SecretKey + ClientTransaction + Bank_Code + Bank_Account + CurrencyCode + Amount.ToString("f2");
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

            PostContent.Add("MerchantCode", ProjectID);
            PostContent.Add("MerchantTransNum", ClientTransaction);
            PostContent.Add("CurrencyTypeId", CurrencyCode);
            PostContent.Add("Amount", Amount.ToString("f2"));
            PostContent.Add("ToBankId", Bank_Code);
            PostContent.Add("ToBankAccountName", Bank_Account_Name);
            PostContent.Add("ToBankAccountNum", Bank_Account);
            PostContent.Add("Signature", newvalue);

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        //string ApiUrl = $"https://job1-demo.mxpay.asia/WebForm/Deposit_P2pPayout2_B2b.aspx";

                        string ApiUrl = $"https://job1.mxpay.asia/WebForm/Deposit_P2pPayout2_B2b.aspx";

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

                                Console.WriteLine("PayoutId:" + result.PayoutId);
                                Console.WriteLine("Result:" + result.Result);
                                Console.WriteLine("MerchantCode:" + result.MerchantCode);
                                Console.WriteLine("CurrencyTypeId:" + result.CurrencyTypeId);
                                Console.WriteLine("ToBankId:" + result.ToBankId);
                                Console.WriteLine("ToBankAccountNum:" + result.ToBankAccountNum);
                                Console.WriteLine("Amount:" + result.Amount);
                                Console.WriteLine("Remarks:" + result.Remarks);
                                Console.WriteLine("CheckString2:" + result.CheckString2);

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


            //string urlstr = "https://job1-demo.mxpay.asia/WebForm/Deposit_P2pPayout2_B2b.aspx?" + "MerchantCode=" + ProjectID
            //   + "&MerchantTransNum=" + ClientTransaction + "&CurrencyTypeId=" + CurrencyCode + "&Amount="+ Amount.ToString("f2")
            //   + "&ToBankId=" + Bank_Code + "&ToBankAccountName=" + "&ToBankAccountNum=" + Bank_Account + "&Signature=" + newvalue;

            //return urlstr;
        }

        public static async Task<PEDepositStatusCheckResult> CheckDeposit(string ClientTransaction, string ProjectID, string SecretKey)
        {
            string orgvalue = ProjectID + SecretKey + ClientTransaction;

            string newvalue = SHA256Hash(orgvalue);

            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("MerchantCode", ProjectID);
            PostContent.Add("MerchantTransNum", ClientTransaction);
            PostContent.Add("CheckString", newvalue);

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        //string ApiUrl = $"https://job1-demo.mxpay.asia/WebForm/Deposit_StatusCheck_Json.aspx";

                        string ApiUrl = $"https://job1.mxpay.asia/WebForm/Deposit_StatusCheck_Json.aspx";

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
                                PEDepositStatusCheckResult result = new PEDepositStatusCheckResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.Result = values["Result"].ToString();

                                Console.WriteLine(values);

                                if (result.Result == "2002" || result.Result == "2101" || result.Result == "2102" ||
                                   result.Result == "2201" || result.Result == "2202" || result.Result == "2203")
                                {
                                    result.DepositId = values["DepositId"].ToString();
                                    result.MerchantCode = values["MerchantCode"].ToString();
                                    result.TransNum = values["TransNum"].ToString();
                                    result.FromBank = values["FromBank"].ToString();
                                    result.ToBank = values["ToBank"].ToString();
                                    result.BankAccountNum = values["BankAccountNum"].ToString();
                                    result.Currency = values["Currency"].ToString();
                                    result.Amount = values["Amount"].ToString();
                                    result.MerchantFeePercentage = values["MerchantFeePercentage"].ToString();
                                    result.MerchantFee = values["MerchantFee"].ToString();
                                    result.PaymentDesc = values["PaymentDesc"].ToString();
                                    result.FirstName = values["FirstName"].ToString();
                                    result.LastName = values["LastName"].ToString();
                                    result.EmailAddress = values["EmailAddress"].ToString();
                                    result.PhoneNum = values["PhoneNum"].ToString();
                                    result.Address = values["Address"].ToString();
                                    result.City = values["City"].ToString();
                                    result.State = values["State"].ToString();
                                    result.Country = values["Country"].ToString();
                                    result.Postcode = values["Postcode"].ToString();
                                    result.MerchantRemark = values["MerchantRemark"].ToString();
                                    result.Message = values["Message"].ToString();
                                    result.CheckString = values["CheckString"].ToString();
                                }

                                return result;
                            }
                            else
                            {
                                PEDepositStatusCheckResult result = new PEDepositStatusCheckResult();

                                result.Result = "9998";

                                result.Remarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            PEDepositStatusCheckResult result = new PEDepositStatusCheckResult();

                            result.Result = "9998";

                            result.Remarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {

                        PEDepositStatusCheckResult result = new PEDepositStatusCheckResult();

                        result.Result = "9999";

                        result.Remarks = ex.Message;

                        return result;
                    }
                }
            }
        }

        public static async Task<PEPayoutStatusCheckResult> CheckPayout(string PayoutId, string MerchantTransNum, string ProjectID, string SecretKey)
        {
            string orgvalue = ProjectID + SecretKey + MerchantTransNum;

            string newvalue = SHA256Hash(orgvalue);

            Dictionary<string, string> PostContent = new Dictionary<string, string>();

            PostContent.Add("PayoutId", PayoutId);
            PostContent.Add("MerchantCode", ProjectID);
            PostContent.Add("MerchantTransNum", MerchantTransNum);
            PostContent.Add("Signature", newvalue);

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        //string ApiUrl = $"https://job1-demo.mxpay.asia/WebForm/Deposit_P2pPayout2_Check_B2b.aspx";

                        string ApiUrl = $"https://job1.mxpay.asia/WebForm/Deposit_P2pPayout2_Check_B2b.aspx";

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
                                PEPayoutStatusCheckResult result = new PEPayoutStatusCheckResult();

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                result.Result = values["Result"].ToString();

                                return result;
                            }
                            else
                            {
                                PEPayoutStatusCheckResult result = new PEPayoutStatusCheckResult();

                                result.Result = "9998";

                                result.Remarks = "post connection status error";

                                return result;
                            }
                        }
                        else
                        {
                            PEPayoutStatusCheckResult result = new PEPayoutStatusCheckResult();

                            result.Result = "9998";

                            result.Remarks = "post connection status error";

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {

                        PEPayoutStatusCheckResult result = new PEPayoutStatusCheckResult();

                        result.Result = "9999";

                        result.Remarks = ex.Message;

                        return result;
                    }
                }
            }
        }

    }


}
