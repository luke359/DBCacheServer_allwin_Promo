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
using System.Diagnostics.Metrics;
using System.Security.Principal;

namespace VnpayAPI
{
    public class FCApiResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public FCApiResult()
        {
            ErrorCode = "";
            Message = "";
            Data = "";
        }
    }

    public class FCLogin
    {
        public string MemberAccount { get; set; }
        public int GameID { get; set; }
        public bool LoginGameHall { get; set; }

        public FCLogin()
        {
            MemberAccount = "JafBar1996";
            GameID = 22065;
            LoginGameHall = false;
        }
    }

    public class FCLoginResult
    {
        public string Result { get; set; }
        public string Url { get; set; }

        public FCLoginResult()
        {
            Result = "";
            Url = "";
        }
    }

    public class FCFreeRun
    {
        public int GameID { get; set; }

        public FCFreeRun()
        {
            GameID = 22065;
        }
    }

    /// <summary>FC玩家餘額查詢結果</summary>
    public class FCMemberInfoResult
    {
        public int Status { get; set; } //Result 状态代码
        public int OnlineType { get; set; } //OnlineType 在线信息(游戏编号
        public double Balance { get; set; } //Points 玩家餘額
    }

    /// <summary>FC轉帳結果</summary>
    public class FCTransferResult
    {
        /// <summary>轉帳結果</summary>
        public int Status { get; set; }
        /// <summary>FC產生的 結果代號</summary>
        public int Result { get; set; }
        /// <summary>轉帳金額</summary>
        public double Amount { get; set; } //Points
        /// <summary>FC轉帳後玩家餘額</summary>
        public double BalanceAfter { get; set; } //AfterPoint
        /// <summary>FC產生的交易識別碼</summary>
        public string TransactionId { get; set; } //BankID
        /// <summary>結果</summary>
        public string Message { get; set; } //BankID
    }

    /// <summary>FC轉帳結果</summary>
    public class FCHistoryRecordData
    {
        /// <summary>游戏记录编号(唯一码)</summary>
        public string recordID { get; set; }
        /// <summary>玩家账号</summary>
        public string account { get; set; }
        /// <summary>游戏编号</summary>
        public int gameID { get; set; }
        /// <summary>游戏类型</summary>
        public int gametype { get; set; }
        /// <summary>下注点数</summary>
        public double bet { get; set; }
        /// <summary>净输赢点数</summary>
        public double winlose { get; set; }
        /// <summary>赢分点数</summary>
        public double prize { get; set; }
        /// <summary>退还金额 (除 Lucky 9 游戏，其余游戏 refund = win)</summary>
        public double refund { get; set; }
        /// <summary>有效投注 (除 Lucky9 游戏，其余游戏 Bet = validBet)</summary>
        public double validBet { get; set; }
        /// <summary>抽水金额 (除 Lucky9 游戏，其余游戏为 0)</summary>
        public double commission { get; set; }
        /// <summary>彩金模式</summary>
        public int jpmode { get; set; }
        /// <summary>游戏内彩金模式</summary>
        public int inGameJpmode { get; set; }
        /// <summary>彩金点数</summary>
        public double jppoints { get; set; }
        /// <summary>彩金抽水(支持到小数第六位)</summary>
        public double jptax { get; set; }

        /// <summary>游戏内彩金贡献</summary>
        public double inGameJptax { get; set; }

        /// <summary>游戏内中奖彩金</summary>
        public double inGameJppoints { get; set; }

        /// <summary>下注前点数</summary>
        public double before { get; set; }

        /// <summary>下注后点数</summary>
        public double after { get; set; }

        /// <summary>下注时间</summary>
        public string bdate { get; set; }

        /// <summary>是否购买免费游戏</summary>
        public bool isBuyFeature { get; set; }

        /// <summary>用于确认此注单是否有获得免费游戏或其他红利游戏</summary>
        public int gameMode { get; set; }

        public FCHistoryRecordData()
        {
            recordID = "";
            account = "";
            gameID = 0;
            gametype = 0;
            bet = 0.00d;
            winlose = 0.00d;
            prize = 0.00d;
            refund = 0.00d;
            validBet = 0.00d;
            commission = 0.00d;
            jpmode = 0;
            inGameJpmode = 0;
            jppoints = 0.00d;
            jptax = 0.000000d;
            inGameJptax = 0.00d;
            inGameJppoints = 0.00d;
            before = 0.00d;
            after = 0.00d;
            bdate = "1970-01-01 00:00:00";
            isBuyFeature = false;
            gameMode = 0;
        }
    }

    /// <summary>FC轉帳結果</summary>
    public class FCHistoryRecordResult
    {
        /// <summary>FC產生的 結果代號</summary>
        public int Result { get; set; }
       
        public List<FCHistoryRecordData> Records { get; set; }

        public FCHistoryRecordResult()
        {
            Result = 9999;
            Records = new List<FCHistoryRecordData>();
        }
    }

    /// <summary>FC轉帳結果</summary>
    public class FCActivityRecordData
    {
        /// <summary>下注点数</summary>
        public double bet { get; set; }
        /// <summary>赢分点数</summary>
        public double prize { get; set; }
        /// <summary>提领金额-当withdrawalAmount有值时，bet与prize皆为0</summary>
        public double withdrawalAmount { get; set; }
        /// <summary>净输赢</summary>
        public double winlose { get; set; }
        /// <summary>奖励前点数</summary>
        public double before { get; set; }
        /// <summary>奖励后点数</summary>
        public double after { get; set; }
        /// <summary>游戏局号</summary>
        public string recordID { get; set; }
        /// <summary>会员帐号</summary>
        public string account { get; set; }
        /// <summary>游戏编号</summary>
        public int gameID { get; set; }
        /// <summary>游戏类型代码</summary>
        public int gametype { get; set; }
        /// <summary>创建时间</summary>
        public string bdate { get; set; }
        /// <summary>活动编号</summary>
        public string eventId { get; set; }
        

        public FCActivityRecordData()
        {
            bet = 0;
            prize = 0;
            withdrawalAmount = 0;
            winlose = 0;
            before = 0;
            after = 0;
            recordID = "";
            account = "";
            gameID = 0;
            gametype = 0;
            bdate = "1970-01-01 00:00:00";
            eventId = "";
        }
    }

    /// <summary>FC轉帳結果</summary>
    public class FCActivityRecordResult
    {
        /// <summary>FC產生的 結果代號</summary>
        public int Result { get; set; }

        public List<FCActivityRecordData> Records { get; set; }

        public FCActivityRecordResult()
        {
            Result = 9999;
            Records = new List<FCActivityRecordData>();
        }
    }

    /// <summary>FC 語文代號</summary>
    //public enum FCLeng
    //{
    //    英文 = 1,
    //    簡體中文 = 2,
    //    越南文 = 3,
    //    泰文 = 4,
    //    印尼文 = 5,
    //    緬甸文 = 6,
    //    日文 = 7,
    //    韓文 = 8,
    //    葡萄牙文 = 9,
    //    西班牙文 = 10,
    //    孟加拉文 = 14,
    //}

    public class FC
    {
        //正式
        static string FCApiBaseUrl = "http://ap1.fcg1688.net";
        static string FCAgentKey = "5fhdhUrbPjatX3Da";
        //測試
        //static string FCApiBaseUrl = "https://api.fcg666.net"; //測試
        //static string FCAgentKey = "y1UEsHzwJi8rc68P"; //測試

        static string FCAgentCode = "ALBBM";

        /// <summary>FC轉帳幣比</summary>
        const int FCCurrency = 100; //MYRR 100(FC):1(我)

        //Login 游戏登入測試  如该玩家在FC无帐号，会自动创建帐号
        public static FCLoginResult Login(string User, int GameId, string Lang, string homeurl)
        {
            string MemberAccount = User;
            int GameID = GameId;
            int LanguageID = 1;

            switch (Lang)
            {
                case "en-US": LanguageID = 1; break;
                case "zh-CN": LanguageID = 2; break;
                case "vi-VN": LanguageID = 3; break;
                case "th-TH": LanguageID = 4; break;
                default: LanguageID = 1; break;
            }

            //製作 Login 所需參數
            var data = new
            {
                MemberAccount,
                GameID,
                LanguageID,
                HomeUrl = homeurl,
                LoginGameHall = false,
            };

            string paraStr = JsonSerializer.Serialize(data);

            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);

            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/Login";

            var task2 = SendApiReq(aesPara, md5Para, ApiUrl);

            FCLoginResult result = new();

            try
            {
                if (task2 != null)
                {
                    Dictionary<string, Object> value = task2.Result;

                    if (value != null)
                    {
                        result.Result = value["Result"].ToString();

                        result.Url = (value.ContainsKey("Url")) ? value["Url"].ToString() : "";
                    }
                    else
                    {
                        result.Result = "400";
                    }
                }
                else
                {
                    result.Result = "400";
                }
            }
            catch (Exception ex)
            {
                result.Result = "400";
            }

            return result;
        }

        /// <summary>檢查玩家餘額</summary>
        public static async Task<FCMemberInfoResult> CheckPlayerBalance(string player_name)
        {
            //製作 檢查玩家 所需參數
            var data = new
            {
                MemberAccount = player_name
            };
            string paraStr = JsonSerializer.Serialize(data);
            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);
            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/SearchMember";

            var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

            FCMemberInfoResult result = new();

            try
            {
                if (task2 != null)
                {
                    Dictionary<string, Object> value = task2;

                    if (value != null)
                    {
                        result.Status = Convert.ToInt32(value["Result"].ToString());
                        if (result.Status == 0)
                        {
                            //FCCurrency
                            double fcPoint = Convert.ToDouble(value["Points"].ToString());
                            result.Balance = Math.Round(fcPoint / FCCurrency, 4);
                            result.OnlineType = Convert.ToInt32(value["OnlineType"].ToString());
                        }
                    }
                    else
                    {
                        result.Status = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = 3;
            }
            return result;
        }

        /// <summary>充值至FC</summary>
        public static async Task<FCTransferResult> TransferInPlayerBalance(string player_name, double amount, string tref)
        {
            double myPoint = Math.Round(amount * FCCurrency, 4);

            //製作 轉帳 所需參數
            var data = new
            {
                MemberAccount = player_name,
                TrsID = tref,
                Points = myPoint
            };
            string paraStr = JsonSerializer.Serialize(data);
            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);
            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/SetPoints";

            FCTransferResult result = new();

            try
            {
                var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

                if (task2 != null)
                {
                    //Dictionary<string, Object> value = task2.Result;
                    Dictionary<string, Object> value = task2;

                    //foreach (KeyValuePair<string, Object> info in value) { Console.WriteLine($"Key={info.Key}, Value={info.Value}"); } //UNDONE: Debug Show

                    if (value != null)
                    {
                        result.Result = Convert.ToInt32(value["Result"].ToString());
                        if (result.Result == 0)
                        {
                            double fcPoint = Convert.ToDouble(value["Points"].ToString());
                            double fcAfter = Convert.ToDouble(value["AfterPoint"].ToString());

                            result.Status = 0; //轉帳成功
                            result.Amount = Math.Round(fcPoint / FCCurrency, 4);
                            result.BalanceAfter = Math.Round(fcAfter / FCCurrency, 4);
                            result.TransactionId = value["BankID"].ToString();
                            result.Message = "Ok";
                        }
                    }
                    else
                    {
                        result.Status = 2;
                        result.Result = 2;
                        result.Message = "No Data";
                    }
                }
                else
                {
                    result.Status = 3;
                    result.Result = 3;
                    result.Message = "No Response";
                }
            }
            catch (Exception ex)
            {
                result.Status = 4;
                result.Message = "Exception";
            }

            return result;
        }
        /// <summary>從FC轉回</summary>
        public static async Task<FCTransferResult> TransferOutPlayerBalance(string player_name, string tref)
        {
            //製作 轉帳 所需參數
            var data = new
            {
                MemberAccount = player_name,
                TrsID = tref,
                AllOut = 1  //全部轉出
            };
            string paraStr = JsonSerializer.Serialize(data);
            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);
            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/SetPoints";

            FCTransferResult result = new();

            try
            {
                var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

                if (task2 != null)
                {
                    //Dictionary<string, Object> value = task2.Result;
                    Dictionary<string, Object> value = task2;

                    //foreach (KeyValuePair<string, Object> info in value) { Console.WriteLine($"Key={info.Key}, Value={info.Value}"); } //UNDONE: Debug Show

                    if (value != null)
                    {
                        result.Result = Convert.ToInt32(value["Result"].ToString());
                        if (result.Result == 0)
                        {
                            double fcPoint = Math.Abs(Convert.ToDouble(value["Points"].ToString())); //轉為正值(FC轉回會傳回負值表示提款)
                            double fcAfter = Convert.ToDouble(value["AfterPoint"].ToString());

                            result.Status = 0; //轉帳成功
                            result.Amount = Math.Round(fcPoint / FCCurrency, 4);
                            result.BalanceAfter = Math.Round(fcAfter / FCCurrency, 4);
                            result.TransactionId = value["BankID"].ToString();
                            result.Message = "Ok";
                        }
                        else if (result.Result == 200)
                        {
                            result.Status = 0; //轉帳成功
                            result.Amount = 0;
                            result.BalanceAfter = 0;
                            result.Message = "No Points";
                            //Console.WriteLine($"從FC轉回 點數為 0");
                        }
                    }
                    else
                    {
                        result.Status = 2;
                        result.Message = "No Data";
                    }
                }
                else
                {
                    result.Message = "No Response";
                    result.Status = 3;
                }
            }
            catch (Exception ex)
            {
                result.Message = "Exception";
                result.Status = 4;
            }
            return result;
        }


        /// <summary>檢查玩家餘額</summary>
        public static async Task<FCHistoryRecordResult> GetHistoryRecordList(string startT, string endT)
        {
            //製作 檢查玩家 所需參數
            var data = new
            {
                StartDate = startT,
                EndDate = endT
            };
            string paraStr = JsonSerializer.Serialize(data);
            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);
            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/GetHistoryRecordList";

            var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

            FCHistoryRecordResult result = new();

            try
            {
                if (task2 != null)
                {
                    Dictionary<string, Object> value = task2;

                    if (value != null)
                    {
                        result.Result = Convert.ToInt32(value["Result"].ToString());

                        result.Records = (value["Records"] == null) ? null : JsonSerializer.Deserialize<List<FCHistoryRecordData>>(value["Records"].ToString());
                    }
                    else
                    {
                        result.Result = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Result = 3;
            }
            return result;
        }

        /// <summary>檢查玩家餘額</summary>
        public static async Task<FCHistoryRecordResult> GetfundInOut(string startT, string endT)
        {
            //製作 檢查玩家 所需參數
            var data = new
            {
                StartDateTime = startT,
                EndDateTime = endT,
                MemberAccount = "255702830"
            };
            string paraStr = JsonSerializer.Serialize(data);
            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);
            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/GetBillList";

            var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

            FCHistoryRecordResult result = new();

            try
            {
                if (task2 != null)
                {
                    Dictionary<string, Object> value = task2;

                    if (value != null)
                    {
                        result.Result = Convert.ToInt32(value["Result"].ToString());

                        result.Records = (value["Records"] == null) ? null : JsonSerializer.Deserialize<List<FCHistoryRecordData>>(value["Records"].ToString());
                    }
                    else
                    {
                        result.Result = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Result = 3;
            }
            return result;
        }

        /// <summary>檢查玩家餘額</summary>
        public static async Task<FCActivityRecordResult> GetActivityRecordList(string startT, string endT)
        {
            //製作 檢查玩家 所需參數
            var data = new
            {
                StartDate = startT,
                EndDate = endT
            };
            string paraStr = JsonSerializer.Serialize(data);
            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);
            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/GetActivityRecordList";

            var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

            FCActivityRecordResult result = new();

            try
            {
                if (task2 != null)
                {
                    Dictionary<string, Object> value = task2;

                    if (value != null)
                    {
                        result.Result = Convert.ToInt32(value["Result"].ToString());

                        result.Records = (value["Records"] == null) ? null : JsonSerializer.Deserialize<List<FCActivityRecordData>>(value["Records"].ToString());
                    }
                    else
                    {
                        result.Result = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Result = 3;
            }
            return result;
        }

        /// <summary>檢查玩家餘額</summary>
        public static async Task<FCHistoryRecordResult> CreateFreeSpin(string startT, string endT, string FreeSpinName, List<int> GameIDList, double minWithdraw, double maxWithdraw, int freeSpinTimes, string joinOption, int maxMemberCount,string ZoneID, double bet)
        {
            //製作 檢查玩家 所需參數
            var data = new
            {
                StartDate = startT,
                EndDate = endT,
                EventName = FreeSpinName,
                GameIDList = GameIDList.ToArray(),
                MinWithdraw = minWithdraw,
                MaxWithdraw = maxWithdraw,
                FreeSpinTimes = freeSpinTimes,
                JoinOption = joinOption,
                MaxMemberCount = maxMemberCount,
                ZoneID = ZoneID,
                bet = bet
            };
            string paraStr = JsonSerializer.Serialize(data);
            //AES參數加密
            string aesPara = AesPara(paraStr, FCAgentKey);
            //MD5參數加密
            string md5Para = Md5Sign(paraStr);

            string ApiUrl = $"{FCApiBaseUrl}/CreateFreeSpin";

            var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

            FCHistoryRecordResult result = new();

            try
            {
                if (task2 != null)
                {
                    Dictionary<string, Object> value = task2;

                    if (value != null)
                    {
                        result.Result = Convert.ToInt32(value["Result"].ToString());

                        result.Records = (value["Records"] == null) ? null : JsonSerializer.Deserialize<List<FCHistoryRecordData>>(value["Records"].ToString());
                    }
                    else
                    {
                        result.Result = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Result = 3;
            }
            return result;
        }

        /// <summary>檢查玩家餘額</summary>
        //public static async Task<FCHistoryRecordResult> GetHistoryRecordList(string player_name, int LanguageID, string HomeUrl,bool JackpotStatus)
        //{
        //    //製作 檢查玩家 所需參數
        //    var data = new
        //    {
        //        StartDate = startT,
        //        EndDate = endT
        //    };
        //    string paraStr = JsonSerializer.Serialize(data);
        //    //AES參數加密
        //    string aesPara = AesPara(paraStr, FCAgentKey);
        //    //MD5參數加密
        //    string md5Para = Md5Sign(paraStr);

        //    string ApiUrl = $"{FCApiBaseUrl}/GetHistoryRecordList";

        //    var task2 = await SendApiReq(aesPara, md5Para, ApiUrl);

        //    FCHistoryRecordResult result = new();

        //    try
        //    {
        //        if (task2 != null)
        //        {
        //            Dictionary<string, Object> value = task2;

        //            if (value != null)
        //            {
        //                result.Result = Convert.ToInt32(value["Result"].ToString());

        //                result.Records = (value["Records"] == null) ? null : JsonSerializer.Deserialize<List<FCHistoryRecordData>>(value["Records"].ToString());
        //            }
        //            else
        //            {
        //                result.Result = 2;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Result = 3;
        //    }
        //    return result;
        //}



        /// <summary>送出Api請求</summary>
        public static async Task<Dictionary<string, Object>> SendApiReq(string aesParams, string md5Sign, string ApiUrl)
        {
            try
            {
                //using (HttpClientHandler handler = new HttpClientHandler())
                {
                    using (HttpClient client2 = new HttpClient())
                    {
                        //HttpClient client2 = HttpClientManager.Instance;
                        client2.Timeout = TimeSpan.FromSeconds(15);

                        Dictionary<string, string> PostContent = new Dictionary<string, string>();
                        PostContent.Add("AgentCode", FCAgentCode);
                        PostContent.Add("Currency", "MYRR");
                        PostContent.Add("Params", aesParams); //使用AES加密
                        PostContent.Add("Sign", md5Sign); //使用MD5加密

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

                                var values = JsonSerializer.Deserialize<Dictionary<string, Object>>(strResult);

                                return values;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>製作FC參數 "Params"</summary>
        public static string AesPara(string jsonStr, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(jsonStr);
                        }

                        byte[] encrypted = msEncrypt.ToArray();

                        return Convert.ToBase64String(encrypted);
                    }
                }
            }
        }

        /// <summary>製作FC參數 "Sign"</summary>
        public static string Md5Sign(string jsonStr)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(jsonStr);

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
