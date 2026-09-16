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
using System.Reflection.PortableExecutable;
using System.Net;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;


namespace VnpayAPI
{
    public class PlayTechNormResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public void SetResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public class CreatePlayerResult
    {
        public string Password { get; set; } = "";
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public void SetResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public class PlayTechMemberInfoData
    {
        /// <summary>交易玩家帳號</summary>
        public string Account { get; set; }
        /// <summary>交易額</summary>
        public double Amount { get; set; }
        /// <summary>交易後錢包餘額</summary>
        public double Balance { get; set; }
        /// <summary>錢包遊戲幣餘額</summary>
        public double Score { get; set; }
        /// <summary>PlayTech產生的交易序號</summary>
        public string TransactionId { get; set; }
        /// <summary>交易結果代號</summary>
        public int Status { get; set; }

        public PlayTechMemberInfoData()
        {
            Account = "";
            TransactionId = "";
            Amount = 0;
            Balance = 0;
            Status = 0;
        }
    }

    public class PlayTechMemberInfoResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<PlayTechMemberInfoData>? Data { get; set; }

        public PlayTechMemberInfoResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
            if(errorCode == "0")
            {
                Data = new List<PlayTechMemberInfoData> { new PlayTechMemberInfoData() };
            }
        }

        public PlayTechMemberInfoResult()
        {
            ErrorCode = "";
            Message = "";
            Data = new List<PlayTechMemberInfoData>();
        }

        public void SetResult(List<PlayTechMemberInfoData> data)
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

    public class PlayTechBetInfo
    {
        public string PLAYERNAME { get; set; }
        public string WINDOWCODE { get; set; }
        public string GAMEID { get; set; }
        public string GAMECODE { get; set; }
        public string GAMETYPE { get; set; }
        public string GAMENAME { get; set; }
        public string SESSIONID { get; set; }
        public string CURRENCYCODE { get; set; }
        public string BET { get; set; }
        public string WIN { get; set; }
        public string PROGRESSIVEBET { get; set; }
        public string PROGRESSIVEWIN { get; set; }
        public string BALANCE { get; set; }
        public string CURRENTBET { get; set; }
        public string GAMEDATE { get; set; }
        public string RNUM { get; set; }
        public string LIVENETWORK { get; set; }

        public PlayTechBetInfo()
        {
            PLAYERNAME = "";
            WINDOWCODE = "";
            GAMEID = "";
            GAMECODE = "";
            GAMETYPE = "";
            GAMENAME = "";
            SESSIONID = "";
            CURRENCYCODE = "";
            BET = "";
            WIN = "";
            PROGRESSIVEBET = "";
            PROGRESSIVEWIN = "";
            BALANCE = "";
            CURRENTBET = "";
            GAMEDATE = "";
            RNUM = "";
            LIVENETWORK = "";
        }
    }

    public class PlayTechPage
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int itemsPerPage { get; set; }
        public int totalCount { get; set; }

        public PlayTechPage()
        {
            currentPage = 0;
            totalPages = 0;
            itemsPerPage = 0;
            totalCount = 0;
        }
    }

    public class PlayTechGetBetRecordResult
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<PlayTechBetInfo> result { get; set; }
        public PlayTechPage pagination { get; set; }

        public PlayTechGetBetRecordResult(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public PlayTechGetBetRecordResult()
        {
            ErrorCode = "";
            Message = "";
            result = new List<PlayTechBetInfo>();
            pagination = new PlayTechPage();
        }

        public void SetError(string errCode, string errMsg)
        {
            ErrorCode = errCode;
            Message = errMsg;
        }
    }


    public class PlayTech
    {
        //正式環境
        //static string PTPassWord = "MKBIetVLmD0lpUUl"; //2025
        //static string SSLCertPath = "AGDRA_PROD.p12"; //2025
        static string PTPassWord = "jsWdeS76dKqB8uid"; //2026 Api憑證
        static string SSLCertPath = "ADGRA-PROD-20270105.p12"; //2026 Api憑證
        static string SecurityKey = "aa36e4b43b6b7fbe8c4d88ca3cbe15ba9496cbc2a934a6662abdb78bdd94605187aa2d67d0b798603c037fb6a36da49e8ba361af3d7033b9f9d1082f49296a4b";
        static string PTCurrency = "MYR"; //幣別 1:1
        static string MerchantCode = "ALIBABAMYR"; //商號ID

        //測試環境
        //static string PTPassWord = "A6ZCfYoycq5tDa5p";
        //static string SSLCertPath = "AGDRA_UAT.p12";
        //static string SecurityKey = "215e5fb462e94a2b43497c0eeb5818f55a7f7ca30d08590db1745579fbf2c35f73083b548abe24a51a317326ee8fccb6d0c52819d6fe4e0be72d4703bc2f70a4";
        //static string PTCurrency = "CNY"; //幣別
        //static string MerchantCode = "ALI88"; //商號ID

        //其他不變參數
        static string PTApiUrl = "https://kioskpublicapi.88shared.com/";
        static string Cloudlocation = "88shared";
        static string MobileHub = "nptgp";
        static string Virtualdatabase = "agdragon";
        static int SystemID = 77;


        /// <summary>PlayTech轉帳幣比</summary>
        const int TransCurrency = 1; //100:1

        /// <summary>Debug 訊息</summary>
        static bool DebugFg = false;


        /// <summary>創建玩家</summary>
        public static async Task<CreatePlayerResult> CreateMember(string User, string password)
        {
            string apiUrlReq = PTApiUrl + "player/create/playername/" + User + "/currency/" + PTCurrency;

            string paraStr = "playername=" + User + "&password=" + password;

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            CreatePlayerResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(values["result"].ToString() ?? "");

                    if (jresult.Count > 0)
                    {
                        //Console.WriteLine($"PlayTech {jresult["playername"].ToString()} password[{jresult["password"].ToString()}]");

                        result.Password = jresult["password"].ToString() ?? "";
                        result.SetResult("0", "Success");
                        return result;
                    }
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    if (errcode == "6") //玩家帳號已存在
                    {
                        result.SetResult("6", "Player Already Exist");
                    }
                    else
                    {
                        Console.WriteLine($"PlayTech {User} 創建玩家API ERROR ! Code={errcode}, Msg={errmsg}");
                        result.SetResult(errcode, errmsg);
                    }
                    return result;
                }

                Console.WriteLine($"PlayTech {User} 創建玩家API回應 ERROR!");
                result.SetResult("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech {User} 創建玩家API無回應! Exception: {ex}");
                result.SetResult("9999", "Response Error");
            }

            return result;
        }

        /// <summary>變更玩家Password</summary>
        public static async Task<PlayTechNormResult> ChangePlayerPassword(string User, string password)
        {
            string apiUrlReq = PTApiUrl + "player/changepassword/playername/" + User;

            string paraStr = "playername=" + User + "&password=" + password;

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechNormResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(values["result"].ToString() ?? "");

                    if (jresult.Count > 0)
                    {
                        //Console.WriteLine($"PlayTech {User} 變更密碼成功");

                        string msg = jresult["password"].ToString() ?? "";
                        result.SetResult("0", msg);
                        return result;
                    }
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech {User} 登出玩家API ERROR ! Code={errcode}, Msg={errmsg}");
                    result.SetResult(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech {User} 登出玩家API回應 ERROR!");
                result.SetResult("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech {User} 登出玩家API無回應! Exception: {ex}");
                result.SetResult("9999", "Response Error");
            }

            return result;
        }

        /// <summary>強制登出玩家</summary>
        public static async Task<PlayTechNormResult> KickMember(string User)
        {
            string apiUrlReq = PTApiUrl + "player/logout/playername/" + User;

            string paraStr = "playername=" + User;

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechNormResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(values["result"].ToString() ?? "");

                    if (jresult.Count > 0)
                    {
                        //Console.WriteLine($"PlayTech {User} 已成功 Logout");

                        string msg = jresult["result"].ToString() ?? "";
                        result.SetResult("0", msg);
                        return result;
                    }
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    if (errcode != "5") //錯誤代碼5: 玩家不存在
                    {
                        Console.WriteLine($"PlayTech {User} 登出玩家API ERROR ! Code={errcode}, Msg={errmsg}");
                    }
                    result.SetResult(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech {User} 登出玩家API回應 ERROR!");
                result.SetResult("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech {User} 登出玩家API無回應! Exception: {ex}");
                result.SetResult("9999", "Response Error");
            }

            return result;
        }

        /// <summary>查玩家餘額</summary>
        public static async Task<PlayTechMemberInfoResult> CheckPlayerBalance(string User)
        {
            string apiUrlReq = PTApiUrl + "player/balance/playername/" + User;
            //string apiUrlReq = PTApiUrl + "player/info";

            string paraStr = "playername=" + User;

            var strResult = await SendApiReq(apiUrlReq, paraStr);
            
            PlayTechMemberInfoResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(values["result"].ToString() ?? "");

                    if (jresult.Count > 0)
                    {
                        PlayTechMemberInfoData info = new();
                        info.Account = User;
                        info.Score = Convert.ToDouble(jresult["balance"].ToString());
                        info.Balance = info.Score; //Math.Round(info.Score / TransCurrency, 4);
                        info.Status = 0;
                        result.SetResult(new List<PlayTechMemberInfoData> { info });

                        //Console.WriteLine($"CurrentBet={jresult["current_bet"].ToString()}");
                        //Console.WriteLine($"BonusBalance={jresult["bonusbalance"].ToString()}");
                        //Console.WriteLine($"RcBalance={jresult["rc_balance"].ToString()}");
                        //Console.WriteLine($"CurrencyCode={jresult["currencycode"].ToString()}");
                        return result;
                    }
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    if (errcode != "5") //錯誤代碼5: 玩家不存在
                    {
                        Console.WriteLine($"PlayTech {User} 查詢餘額 ERROR ! Code={errcode}, Msg={errmsg}");
                    }
                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech {User} 查詢餘額API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech {User} 查詢餘額API Exception: {ex}");
                result.SetError("9999", "Response Error");
            }

            return result;
        }

        /// <summary>查玩家資訊</summary>
        public static async Task<PlayTechMemberInfoResult> GetPlayerInfo(string User)
        {
            string apiUrlReq = PTApiUrl + "player/info/playername/" + User;
            //string apiUrlReq = PTApiUrl + "player/info";

            string paraStr = "playername=" + User;

            var strResult = await SendApiReq(apiUrlReq, paraStr);
            
            PlayTechMemberInfoResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(values["result"].ToString() ?? "");

                    if (jresult.Count > 0)
                    {
                        PlayTechMemberInfoData info = new();
                        info.Account = User;
                        info.Score = Convert.ToDouble(jresult["BALANCE"].ToString());
                        info.Balance = info.Score; //Math.Round(info.Score / TransCurrency, 4);
                        info.Status = 0;
                        result.SetResult(new List<PlayTechMemberInfoData> { info });

                        //Console.WriteLine($"PLAYERNAME={jresult["PLAYERNAME"].ToString()}");
                        //Console.WriteLine($"ENTITYNAME={jresult["ENTITYNAME"].ToString()}");
                        //Console.WriteLine($"KIOSKADMINNAME={jresult["KIOSKADMINNAME"].ToString()}");
                        //Console.WriteLine($"CURRENCY={jresult["CURRENCY"].ToString()}");
                        //Console.WriteLine($"PASSWORD={jresult["PASSWORD"].ToString()}");
                        return result;
                    }
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech {User} 查詢資訊 ERROR ! Code={errcode}, Msg={errmsg}");

                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech {User} 查詢資訊API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech {User} 查詢資訊API Exception: {ex}");
                result.SetError("9999", "Response Error");
            }

            return result;
        }

        /// <summary>充值至PlayTech</summary>
        public static async Task<PlayTechMemberInfoResult> TransferInPlayerBalance(string User, double amount, string tRefId)
        {
            if (amount <= 0)
            {
                return new PlayTechMemberInfoResult("9997", "Amount Error");
            }

            //double tamount = Math.Round(amount * TransCurrency, 2); //PlayTech只接受小數2位
            double tamount = Math.Round(amount, 2); //PlayTech只接受小數2位

            string apiUrlReq = PTApiUrl + "player/deposit/playername/" + User + "/amount/" + tamount.ToString();

            string paraStr = "playername=" + User + "&amount=" + tamount.ToString() + "&externaltranid=" + tRefId;
            //Console.WriteLine("dataStream=" + paraStr);

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechMemberInfoResult result = new();

            try
            {
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    //var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(values["result"].ToString() ?? "");
                    PTTransferResponse jresult = new(values["result"]);

                    PlayTechMemberInfoData info = new();
                    info.Account = User;
                    info.Amount = jresult.Amount;
                    info.Balance = jresult.AfterBalance; //此金額??
                    info.TransactionId = jresult.TransactionId; //PT交易Id
                    info.Status = 0;
                    
                    result.SetResult(new List<PlayTechMemberInfoData> { info });
                    return result;
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech {User} 充值 ERROR ! Code={errcode}, Msg={errmsg}");

                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech {User} 充值API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech {User} 錢包充值API Exception: {ex}");
                result.SetError("9999", "Response Error");

            }
            return result;
        }

        /// <summary>從PlayTech轉出</summary>
        public static async Task<PlayTechMemberInfoResult> TransferOutPlayerBalance(string User, double amount, string tRefId)
        {
            if (amount < 0)
            {
                return new PlayTechMemberInfoResult("9997", "Amount Error");
            }

            decimal tamount = 0; //遊戲幣

            if (amount > 0)
            {
                tamount = (decimal)Math.Round(amount * TransCurrency, 4);
            }
            else if (amount == 0)
            {
                //0值 = 提領全部, 先查詢餘額
                var result2 = await CheckPlayerBalance(User);
                if (result2.ErrorCode != "0")
                {
                    return new PlayTechMemberInfoResult(result2.ErrorCode, result2.Message);
                }
                amount = result2.Data[0].Balance;
                if (DebugFg) Console.WriteLine($"PlayTech {User} 查餘額={amount}");
            
                if (amount == 0)
                {
                    return new PlayTechMemberInfoResult("0", "Amount Zero");
                }
            
                tamount = (decimal)result2.Data[0].Score;
            }

            string apiUrlReq = PTApiUrl + "player/withdraw/playername/" + User; // + "/amount/" + tamount.ToString() + "/externaltranid/" + tRefId;
            //Console.WriteLine("apiUrlReq=" + apiUrlReq);
            string paraStr = "playername=" + User + "&amount=" + tamount.ToString() + "&externaltranid=" + tRefId;

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechMemberInfoResult result = new();

            try
            {
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    //var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(values["result"].ToString() ?? "");
                    PTTransferResponse jresult = new(values["result"]);

                    PlayTechMemberInfoData info = new();
                    info.Account = User;
                    info.Amount = jresult.Amount;
                    info.Balance = jresult.AfterBalance;
                    info.TransactionId = jresult.TransactionId; //PT交易Id
                    info.Status = 0;

                    result.SetResult(new List<PlayTechMemberInfoData> { info });
                    return result;
                }
                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech {User} 錢包取回 ERROR ! Code={errcode}, Msg={errmsg}");

                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech {User} 錢包取回API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech {User} 錢包取回API Exception: {ex}");
                result.SetError("9999", "Response Error");
            }
            return result;
        }

        /// <summary>取得遊戲 url</summary>
        public static async Task<PlayTechGetBetRecordResult> GetBetRecord(string start_time, string end_time, int pageIndex)
        {
            string apiUrlReq = PTApiUrl + "customreport/getdata/reportname/PlayerGames";

            string paraStr = "startdate=" + start_time + "&enddate=" + end_time + "&frozen=all" + "&page=" + pageIndex.ToString() + "&perPage=500";

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechGetBetRecordResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    result.ErrorCode = "0";
                    result.Message = "success";
                    result.result = JsonSerializer.Deserialize<List<PlayTechBetInfo>>(values["result"].ToString());
                    result.pagination = JsonSerializer.Deserialize<PlayTechPage>(values["pagination"].ToString());
                    return result;
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech GetBetRecord ERROR ! Code={errcode}, Msg={errmsg}");

                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech GetBetRecord API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech GetBetRecord API Exception: {ex}");
                result.SetError("9999", "Response Error");
            }

            return result;
        }

        /// <summary>取得遊戲 url</summary>
        public static async Task<PlayTechGetBetRecordResult> GetGoldenChipGames(int bonusid)
        {
            string apiUrlReq = PTApiUrl + "goldenchip/bonusgames/";

            string paraStr = "bonusid=" + bonusid.ToString();

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechGetBetRecordResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    result.ErrorCode = "0";
                    result.Message = "success";
                    result.result = JsonSerializer.Deserialize<List<PlayTechBetInfo>>(values["result"].ToString());
                    result.pagination = JsonSerializer.Deserialize<PlayTechPage>(values["pagination"].ToString());
                    return result;
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech GetBetRecord ERROR ! Code={errcode}, Msg={errmsg}");

                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech GetBetRecord API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech GetBetRecord API Exception: {ex}");
                result.SetError("9999", "Response Error");
            }

            return result;
        }

        public static async Task<PlayTechGetBetRecordResult> GetPlayerActiveBonuses(string playerid)
        {
            string apiUrlReq = PTApiUrl + "player/getactivebonuses/";

            string paraStr = "playername=" + playerid;

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechGetBetRecordResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    result.ErrorCode = "0";
                    result.Message = "success";
                    result.result = JsonSerializer.Deserialize<List<PlayTechBetInfo>>(values["result"].ToString());
                    result.pagination = JsonSerializer.Deserialize<PlayTechPage>(values["pagination"].ToString());
                    return result;
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech GetBetRecord ERROR ! Code={errcode}, Msg={errmsg}");

                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech GetBetRecord API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech GetBetRecord API Exception: {ex}");
                result.SetError("9999", "Response Error");
            }

            return result;
        }

        public static async Task<PlayTechGetBetRecordResult> GivePlayerGoldenChip(string playerid,int amount, int chipscount, string externalbonusid, int bonusid)
        {
            string apiUrlReq = PTApiUrl + "goldenchip/giveplayer/";

            string paraStr = "playername=" + playerid + "&adminname=ALIBABAINTCNY" + "&amount="+ amount.ToString() + 
                             "&chipscount=" + chipscount.ToString() + "&externalbonusid=" + externalbonusid + "&bonusid=" + bonusid;

            var strResult = await SendApiReq(apiUrlReq, paraStr);

            PlayTechGetBetRecordResult result = new();

            try
            {
                //var values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(strResult);
                var values = JsonSerializer.Deserialize<Dictionary<string, object>>(strResult);

                if (values.ContainsKey("result"))
                {
                    result.ErrorCode = "0";
                    result.Message = "success";
                    result.result = JsonSerializer.Deserialize<List<PlayTechBetInfo>>(values["result"].ToString());
                    result.pagination = JsonSerializer.Deserialize<PlayTechPage>(values["pagination"].ToString());
                    return result;
                }

                if (values.ContainsKey("error"))
                {
                    string errmsg = values["error"].ToString() ?? "";
                    string errcode = values["errorcode"].ToString() ?? "";

                    Console.WriteLine($"PlayTech GetBetRecord ERROR ! Code={errcode}, Msg={errmsg}");

                    result.SetError(errcode, errmsg);
                    return result;
                }

                Console.WriteLine($"PlayTech GetBetRecord API回應 ERROR!");
                result.SetError("9998", "Response Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PlayTech GetBetRecord API Exception: {ex}");
                result.SetError("9999", "Response Error");
            }

            return result;
        }



        // 靜態 HttpClient 實例
        //private static readonly HttpClient _Client = new HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(15)
        //};
        // 從 HttpClientProvider 取得共用的 HttpClient 實例
        //private static HttpClient _Client = HttpClientProvider.Client;

        static async Task<string> SendApiReq(string api, string paraStr = "")
        {
            byte[] dataStream = Encoding.UTF8.GetBytes(paraStr);

            //Console.WriteLine($"api=[{api}]"); //UNDONE: Test Only
            //Console.WriteLine($"Para=[{paraStr}]"); //UNDONE: Test Only

            try
            {
                // 建立 HttpClientHandler 以處理憑證和其他相關設定
                var handler = new HttpClientHandler
                {
                    ClientCertificates = { new X509Certificate2(SSLCertPath, PTPassWord, X509KeyStorageFlags.MachineKeySet) },
                    ServerCertificateCustomValidationCallback = CertificateValidationCallBack //?這行沒加好像也可以
                };

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, api)
                    {
                        //Content = new ByteArrayContent(dataStream)
                        Content = new StringContent(paraStr, Encoding.UTF8, "application/x-www-form-urlencoded")
                    };

                    // 設定 Headers
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
                    request.Headers.TryAddWithoutValidation("Cache-Control", "max-age=0");
                    request.Headers.TryAddWithoutValidation("Keep-Alive", "timeout=5, max=100");
                    request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "es-ES,es;q=0.8");
                    request.Headers.TryAddWithoutValidation("Pragma", "");
                    request.Headers.TryAddWithoutValidation("X_ENTITY_KEY", SecurityKey);

                    // 發送請求並處理回應
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    //Console.WriteLine("Response Data: " + responseBody);

                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PlayTech SendHttp ERROR Exception : " + ex);
                return null;
            }
        }

        //static async Task<string> _SendApiReq(string rURL, string paraStr = "")
        //{
        //    byte[] dataStream = Encoding.UTF8.GetBytes(paraStr);
        //
        //    //Console.WriteLine($"api=[{api}]"); //UNDONE: Test Only
        //    //Console.WriteLine($"Para=[{paraStr}]"); //UNDONE: Test Only
        //
        //    try
        //    {
        //        HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(rURL);
        //        HttpWebResponse Response = null;
        //
        //        Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        //        Request.Headers.Add("Cache-Control", "max-age=0");
        //        Request.KeepAlive = true;
        //        Request.Headers.Add("Keep-Alive", "timeout=5, max=100");
        //        Request.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
        //        Request.Headers.Add("Accept-Language", "es-ES,es;q=0.8");
        //        Request.Headers.Add("Pragma", "");
        //        Request.Headers.Add("X_ENTITY_KEY", SecurityKey);
        //        Request.Method = "POST";
        //        Request.ContentType = "application/x-www-form-urlencoded";
        //        Request.ClientCertificates.Add(new X509Certificate2(SSLCertPath, PTPassWord, X509KeyStorageFlags.MachineKeySet));
        //
        //        ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
        //        Request.ContentLength = dataStream.Length;
        //        Stream newStream = Request.GetRequestStream();
        //        // Send the data.
        //        newStream.Write(dataStream, 0, dataStream.Length);
        //        newStream.Close();
        //        Response = (HttpWebResponse)Request.GetResponse();
        //        StreamReader reader = new StreamReader(Response.GetResponseStream());
        //        String retData = reader.ReadToEnd();
        //
        //        Console.WriteLine("");
        //        Console.WriteLine("Response Data: " + retData);
        //        Console.WriteLine("");
        //
        //        return retData;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("PlayTech SendHttp ERROR Exception : " + ex);
        //        return null;
        //    }
        //}


        /// <summary>PlayTech的憑證驗證Callback</summary>
        private static bool CertificateValidationCallBack(object sender,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) && (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }

        class PTTransferResponse
        {
            /// <summary>本次交易金額</summary>
            public double Amount { get; set; }
            /// <summary>交易後PT玩家餘額</summary>
            public double AfterBalance { get; set; }
            /// <summary>PT交易Id</summary>
            public string TransactionId { get; set; }

            public PTTransferResponse(object value)
            {
                //只取需要的欄位
                var jresult = JsonSerializer.Deserialize<Dictionary<string, object>>(value.ToString() ?? "");
                if (jresult != null && jresult.Count > 0)
                {
                    Amount = Convert.ToDouble(jresult["amount"].ToString());
                    AfterBalance = Convert.ToDouble(jresult["currentplayerbalance"].ToString()); //After Blance: 充值後的PT錢包餘額
                    TransactionId = jresult["ptinternaltransactionid"].ToString() ?? "";  //PT交易Id
                }
                else
                {
                    Amount = 0;
                    AfterBalance = 0;
                    TransactionId = "";
                }
            }
        }
    }
}
