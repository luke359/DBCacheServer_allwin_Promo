using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UserSessionReward.Core.Enums;
using UserSessionReward.Core.Models;
using ProbabilityLib;

namespace DBCacheServer
{
    public class UserData
    {
        #region 玩家主要資料
        /// <summary>唯一碼</summary>
        public int UserUID { get; private set; }
        /// <summary>代理商ID</summary>
        public int EntityId { get; set; }
        /// <summary>管理者ID</summary>
        public string AspNetUserId { get; set; }
        /// <summary>玩家ID</summary>
        public string UserID { get; set; }
        /// <summary>玩家密碼</summary>
        public string UserPwd { get; set; }
        /// <summary>玩家暱稱</summary>
        public string Nickname { get; set; }
        /// <summary>玩家性別</summary>
        public int Sex { get; set; }
        /// <summary>玩家餘額</summary>
        public double UserBalance { get; set; }
        /// <summary>玩家電郵</summary>
        public string Email { get; set; }
        /// <summary>玩家電話</summary>
        public string PhoneNum { get; set; }
        /// <summary>玩家凍結旗標</summary>
        public bool blockFlag { get; set; }
        /// <summary>玩家所在位置 (1=大廳 , 2=Xiyou , 3=HighWay....)</summary>
        public int Usersituation { get; set; }
        /// <summary>能量條</summary>
        public int EnergyBar { get; set; }
        /// <summary>能量Coin值</summary>
        public double Energy { get; set; }
        /// <summary>星星總數</summary>
        public int Star { get; set; }
        /// <summary>Session ID</summary>
        public double SessionID { get; set; }
        /// <summary>第一次押分時間</summary>
        public int FirstBetTime { get; set; }
        /// <summary>是否為機器人</summary>
        public bool IsBot { get; set; }
        /// <summary>強控機率</summary>
        public bool CalFg { get; set; }
        /// <summary>4大JP出牌 開關</summary>
        public bool BigJPFg { get; set; }
        /// <summary>共用大水庫出牌 開關</summary>
        public bool BigWaterFg { get; set; }
        /// <summary>海王8 如來神掌累積值(此資訊紀錄在 UserBankTable)</summary>
        public double BuddhasPalm { get; set; }
        /// <summary>玩家最後總押分</summary>
        public double LasTotBet { get; private set; } = 12.34;
        #endregion


        #region 玩家IP模式機率 #251112
        /// <summary>WEB設定玩家 Debug Show</summary>
        bool IpSetDbFg = true; //WEB設定玩家 Debug用

        /// <summary>玩家第2錢包</summary>
        public double Balance2 { get; private set; }  //玩家促銷禮金 PromoBonus AdBonus 
        /// <summary>總押</summary>
        public decimal PlayInAcc { get; private set; } //由IpInfo更新, Server未改新機率模式者, 此值不會變
        /// <summary>總得</summary>
        public decimal PlayOutAcc { get; private set; } //由IpInfo更新
        /// <summary>總玩局數</summary>
        public int TotalPlayTimes { get; private set; } //由IpInfo更新
        /// <summary>總開</summary>
        public decimal KeyInAcc { get; private set; }
        /// <summary>總洗</summary>
        public decimal KeyOutAcc { get; private set; }
        /// <summary>H5 總開 (轉回本地)</summary>
        public decimal H5InAcc { get; private set; }
        /// <summary>H5 總洗 (本地轉出)</summary>
        public decimal H5OutAcc { get; private set; }
        /// <summary>送錢總計</summary>
        public decimal ExBonusAcc { get; private set; }
        /// <summary>個人累積水庫</summary>
        public double WaterColl { get; private set; }
        /// <summary>玩家紅包待放水庫</summary>
        public double RedBonusPool { get; private set; }
        /// <summary>待放水水庫</summary>
        public double WaterInAcc { get; private set; }
        /// <summary>待抽水水庫</summary>
        public double WaterOutAcc { get; private set; }
        /// <summary>放水水庫紀錄</summary>
        public double WaterInRec { get; private set; }
        /// <summary>抽水水庫紀錄</summary>
        public double WaterOutRec { get; private set; }
        /// <summary>玩家紅包放水紀錄</summary>
        public double RedBonusRec { get; private set; }
        /// <summary>後台設置機率用Data (RTP,抽放水段數,紅包幣值選中率)</summary>
        public string ParaData { get; private set; }
        /// <summary>後台設置機率用Data有變更 旗號</summary>
        bool paraDataChangeFg = false;

        /// <summary>機率用Data</summary>
        string CalcData;
        /// <summary>機率用Data 獨立買</summary>
        string CalcDataIndep;
        /// <summary>機率用Set參數</summary>
        string CalcSet;
        /// <summary>機率參數有變更 旗號</summary>
        bool calaSetChangeFg = false;
        /// <summary>平均押分紀錄</summary>
        string BetAverage;
        /// <summary>中獎統計</summary>
        string ResultRec;

        #region 洗分限制
        /// <summary>日總洗分 [倍率] 限制(倍) (0表示無限制)</summary>
        public int PayOutLimitDayMx { get; private set; }
        /// <summary>日總洗分 限制(幣) (0表示無限制)</summary>
        public int PayOutLimitDay { get; private set; }
        /// <summary>週總洗分 限制(幣) (0表示無限制)</summary>
        public int PayOutLimitWeek { get; private set; }
        /// <summary>月總洗分 限制(幣) (0表示無限制)</summary>
        public int PayOutLimitMonth { get; private set; }

        /// <summary>日總開分 紀錄 (計算開分倍率用)</summary>
        public double KeyInRecDay { get; private set; }
        /// <summary>日總洗分 紀錄 (正值=玩家贏錢)</summary>
        public double PayOutRecDay { get; private set; }
        /// <summary>週總洗分 紀錄 (正值=玩家贏錢)</summary>
        public double PayOutRecWeek { get; private set; }
        /// <summary>月總洗分 紀錄 (正值=玩家贏錢)</summary>
        public double PayOutRecMonth { get; private set; }

        /// <summary>日總洗分[倍率] Over旗號</summary>
        public bool PayOutOvFgDayMx { get; private set; } = false;
        /// <summary>日總洗分 Over旗號</summary>
        public bool PayOutOvFgDay { get; private set; } = false;
        /// <summary>週總洗分 Over旗號</summary>
        public bool PayOutOvFgWeek { get; private set; } = false;
        /// <summary>月總洗分 Over旗號</summary>
        public bool PayOutOvFgMonth { get; private set; } = false;
        #endregion 

        /// <summary>放水遊戲選擇 (空白代表無限制)</summary>
        List<GameServerCode> WaterInServer;
        /// <summary>紅包放水遊戲選擇 (空白代表無限制)</summary>
        List<GameServerCode> RedBonusServer;

        /// <summary>讀取DB 玩家IP模式 機率資料</summary>
        void IPGetDBData(Dictionary<string, string> datalist)
        {
            if (datalist.ContainsKey("CalcData"))
            {
                Balance2 = Convert.ToDouble(datalist[nameof(Balance2)]);
                PlayInAcc = Convert.ToDecimal(datalist[nameof(PlayInAcc)]);
                PlayOutAcc = Convert.ToDecimal(datalist[nameof(PlayOutAcc)]);
                TotalPlayTimes = Convert.ToInt32(datalist[nameof(TotalPlayTimes)]);
                KeyInAcc = Convert.ToDecimal(datalist[nameof(KeyInAcc)]);
                KeyOutAcc = Convert.ToDecimal(datalist[nameof(KeyOutAcc)]);
                H5InAcc = Convert.ToDecimal(datalist[nameof(H5InAcc)]);
                H5OutAcc = Convert.ToDecimal(datalist[nameof(H5OutAcc)]);
                ExBonusAcc = Convert.ToDecimal(datalist[nameof(ExBonusAcc)]);
                WaterColl = Convert.ToDouble(datalist[nameof(WaterColl)]);
                RedBonusPool = Convert.ToDouble(datalist[nameof(RedBonusPool)]);
                WaterInAcc = Convert.ToDouble(datalist[nameof(WaterInAcc)]);
                WaterOutAcc = Convert.ToDouble(datalist[nameof(WaterOutAcc)]);
                WaterInRec = Convert.ToDouble(datalist[nameof(WaterInRec)]);
                WaterOutRec = Convert.ToDouble(datalist[nameof(WaterOutRec)]);
                RedBonusRec = Convert.ToDouble(datalist[nameof(RedBonusRec)]);

                SetCalcData(datalist[nameof(CalcData)]);
                SetCalcDataIndep(datalist[nameof(CalcDataIndep)]);
                SetCalcSet(datalist[nameof(CalcSet)]);
                //ParaData = CheckParaData(datalist[nameof(ParaData)]); //提至外層另外讀取, 因為若有問題需額外處理
                BetAverage = datalist[nameof(BetAverage)];
                ResultRec = datalist[nameof(ResultRec)];

                PayOutLimitDay = Convert.ToInt32(datalist[nameof(PayOutLimitDay)]);
                PayOutLimitWeek = Convert.ToInt32(datalist[nameof(PayOutLimitWeek)]);
                PayOutLimitMonth = Convert.ToInt32(datalist[nameof(PayOutLimitMonth)]);
                PayOutLimitDayMx = Convert.ToInt32(datalist[nameof(PayOutLimitDayMx)]);

                KeyInRecDay = Convert.ToDouble(datalist[nameof(KeyInRecDay)]);
                PayOutRecDay = Convert.ToDouble(datalist[nameof(PayOutRecDay)]);
                PayOutRecWeek = Convert.ToDouble(datalist[nameof(PayOutRecWeek)]);
                PayOutRecMonth = Convert.ToDouble(datalist[nameof(PayOutRecMonth)]);

                WaterInServer = CacheManeger.DecodeServerList(datalist[nameof(WaterInServer)], $"[{UserID}]個人放水");
                RedBonusServer = CacheManeger.DecodeServerList(datalist[nameof(RedBonusServer)], $"[{UserID}]個人紅包放水");

                CheckPayOutLimit();

                SetRewardCacheData(datalist);
            }
        }

        /// <summary>更新玩家ParaData</summary>
        public void SetParaData(string paraData)
        {
            ParaData = paraData;
            paraDataChangeFg = true;
            //MyConsole.WriteLine($"    重設玩家[{UserID}]遊戲參數值[{ParaData}]");
        }
        /// <summary>更新玩家CalcSet</summary>
        public void SetCalcSet(string calaSet)
        {
            CalcSet = calaSet;
            calaSetChangeFg = true;
            //MyConsole.WriteLine($"    重設玩家[{UserID}]機率參數值[{CalcSet}]");
        }
        /// <summary>更新玩家CalcData</summary>
        public void SetCalcData(string calaData)
        {
            CalcData = calaData;
            //MyConsole.WriteLine($"    重設玩家[{UserID}]機率資料值[{CalcSet}]");
        }
        /// <summary>更新玩家CalcDataIndep</summary>
        public void SetCalcDataIndep(string calaDataIndep)
        {
            CalcDataIndep = calaDataIndep;
            //MyConsole.WriteLine($"    重設玩家[{UserID}]機率資料值[{CalcSetIndep}]");
        }


        /// <summary>檢查ParaData</summary>
        //string CheckParaData(string paraData)
        //{
        //    CacheManeger.IpParaDataInfo ipParaDataInfo = new CacheManeger.IpParaDataInfo(paraData);
        //    return ipParaDataInfo.CombineData();
        //}

        /// <summary>後台修改 玩家參數 處理程序 (不處理ParaData參數)</summary>
        public void IPGetWebSet(CacheManeger.WebIpSetPara spara)
        {
            //抽放水
            if (spara.WaterIn > 0)
            {
                //放水幣值
                WaterInAcc += spara.WaterIn;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:放水[{spara.WaterIn}]幣, 總可放[{WaterInAcc}]");
            }
            if (spara.WaterOut > 0)
            {
                //抽水幣值
                WaterOutAcc += spara.WaterOut;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:抽水[{spara.WaterOut}]幣, 總待抽[{WaterOutAcc}]");
            }

            if (spara.RedWaterIn > 0)
            {
                //红包放水幣值
                RedBonusPool += spara.RedWaterIn;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:红包放水[{spara.RedWaterIn}]幣, 總可放[{RedBonusPool}]");
            }
            if (spara.RedWaterOut > 0)
            {
                //红包抽水幣值
                WaterOutAcc += spara.RedWaterOut; //红包抽水也放到一般抽水庫
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:红包抽水[{spara.RedWaterOut}]幣, 總待抽[{WaterOutAcc}]");
            }

            if (spara.WaterInClear)
            {
                spara.WaterIn = -WaterInAcc;
                WaterInAcc = 0;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:清除放水水庫[{spara.WaterIn}]幣");
            }
            if (spara.WaterOutClear)
            {
                spara.WaterOut = -WaterOutAcc;
                WaterOutAcc = 0;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:清除抽水水庫[{spara.WaterOut}]幣");
            }
            if (spara.RedWaterInClear)
            {
                spara.RedWaterIn = -RedBonusPool;
                RedBonusPool = 0;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:清除红包放水水庫[{spara.RedWaterIn}]幣");
            }

            //抽放水遊戲選擇
            if (spara.WaterInServer != "0") //"0"=不變
            {
                //放水遊戲
                WaterInServer = CacheManeger.DecodeServerList(spara.WaterInServer, $"[{UserID}]個人放水");
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:放水遊戲[{spara.WaterInServer}]");
            }
            if (spara.RedWaterInServer != "0") //"0"=不變
            {
                //红包放水游戏
                RedBonusServer = CacheManeger.DecodeServerList(spara.RedWaterInServer, $"[{UserID}]個人紅包放水");
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:红包放水遊戲[{spara.RedWaterInServer}]");
            }

            //洗分限制
            if (spara.PayOutLimitDay >= 0)
            {
                //日總洗分 限制
                PayOutLimitDay = spara.PayOutLimitDay;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:日總洗分 限制[{PayOutLimitDay}]");
            }
            if (spara.PayOutLimitWeek >= 0)
            {
                //週總洗分 限制
                PayOutLimitWeek = spara.PayOutLimitWeek;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:週總洗分 限制[{PayOutLimitWeek}]");
            }
            if (spara.PayOutLimitMonth >= 0)
            {
                //月總洗分 限制
                PayOutLimitMonth = spara.PayOutLimitMonth;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:月總洗分 限制[{PayOutLimitMonth}]");
            }
            if (spara.PayOutLimitDayMx >= 0)
            {
                //日總洗分[倍率] 限制
                PayOutLimitDayMx = spara.PayOutLimitDayMx;
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:日總洗分[倍率] 限制[{PayOutLimitDayMx}]");
            }

            //控制開關
            if (spara.BigWaterFg >= 0)
            {
                //大水庫出牌 開關
                BigWaterFg = (spara.BigWaterFg == 1);
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:大水庫出牌 開關[{BigWaterFg}]");
            }
            if (spara.BigJPFg >= 0)
            {
                //4大JP出牌 開關
                BigJPFg = (spara.BigJPFg == 1);
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:4大JP出牌 開關[{BigJPFg}]");
            }
            if (spara.CalFg >= 0)
            {
                //強制控制 開關
                CalFg = (spara.CalFg == 1);
                if (IpSetDbFg) MyConsole.WriteLine($"    {UserID}:強制控制 開關[{CalFg}]");
            }
        }

        /// <summary>清除玩家個人 機率帳紀錄</summary>
        public void ClearIPRecord()
        {
            CalcData = "";
            CalcDataIndep = "";
            BetAverage = "";
            ResultRec = "";
            CalcSet = "";

            PlayInAcc = 0;
            PlayOutAcc = 0;
            TotalPlayTimes = 0;
            KeyInAcc = 0;
            KeyOutAcc = 0;
            H5InAcc = 0;
            H5OutAcc = 0;
            ExBonusAcc = 0;
            WaterColl = 0;
            RedBonusPool = 0;
            WaterInAcc = 0;
            WaterOutAcc = 0;
            WaterInRec = 0;
            WaterOutRec = 0;
            RedBonusRec = 0;
            KeyInRecDay = 0;
            PayOutRecDay = 0;
            PayOutRecWeek = 0;
            PayOutRecMonth = 0;
        }

        /// <summary>取得玩家IP模式機率資料 (玩家進入GameServer時)</summary>
        public Dictionary<string, string> GetIpInfoEnterGame(GameServerCode gameServer)
        {
            Dictionary<string, string> updata = new();
            updata.Add(nameof(Balance2), PrecisionAdj(Balance2).ToString());
            updata.Add(nameof(PlayInAcc), PlayInAcc.ToString());
            updata.Add(nameof(PlayOutAcc), PlayOutAcc.ToString());
            updata.Add(nameof(TotalPlayTimes), TotalPlayTimes.ToString());
            updata.Add(nameof(KeyInAcc), KeyInAcc.ToString());
            updata.Add(nameof(KeyOutAcc), KeyOutAcc.ToString());
            updata.Add(nameof(H5InAcc), H5InAcc.ToString());
            updata.Add(nameof(H5OutAcc), H5OutAcc.ToString());
            //updata.Add(nameof(ExBonusAcc), PrecisionAdj(ExBonusAcc).ToString());
            updata.Add(nameof(RedBonusRec), PrecisionAdj(RedBonusRec).ToString());
            updata.Add(nameof(WaterColl), PrecisionAdj(WaterColl).ToString());
            updata.Add(nameof(CalcData), CalcData);
            updata.Add(nameof(CalcDataIndep), CalcDataIndep);
            updata.Add(nameof(BetAverage), BetAverage);
            updata.Add(nameof(ResultRec), ResultRec);
            updata.Add(nameof(CalcSet), CalcSet);
            calaSetChangeFg = false;

            //待抽放水庫 (WEB設置)
            updata.Add(nameof(WaterOutAcc), PrecisionAdj(WaterOutAcc).ToString());
            updata.Add(nameof(WaterInAcc), GetWaterInAcc(gameServer).ToString());
            updata.Add(nameof(RedBonusPool), GetRedBonusPool(gameServer).ToString());

            //洗分限制
            updata.Add(nameof(PayOutLimitDay), PayOutLimitDay.ToString());
            updata.Add(nameof(PayOutLimitWeek), PayOutLimitWeek.ToString());
            updata.Add(nameof(PayOutLimitMonth), PayOutLimitMonth.ToString());
            updata.Add(nameof(PayOutLimitDayMx), PayOutLimitDayMx.ToString());

            updata.Add("KOvDayMx", BoolToStr(PayOutOvFgDayMx));
            updata.Add("KOvDay", BoolToStr(PayOutOvFgDay));
            updata.Add("KOvWeek", BoolToStr(PayOutOvFgWeek));
            updata.Add("KOvMonth", BoolToStr(PayOutOvFgMonth));

            //機率參數 (WEB設置)
            updata.Add(nameof(ParaData), ParaData);
            updata.Add(nameof(RewardCacheData), RewardCacheData); //#260821
            updata.Add(nameof(ExtraInfo), ExtraInfo); //#260821

            paraDataChangeFg = false;

            return updata;
        }
        /// <summary>取得玩家IP模式機率資料 (回傳遊戲結果驗證時)</summary>
        public Dictionary<string, string> GetIpInfoGameResult(GameServerCode gameServer)
        {
            Dictionary<string, string> updata = new();
            updata.Add(nameof(UserUID), UserUID.ToString());

            //由DBCache維護
            updata.Add(nameof(Balance2), PrecisionAdj(Balance2).ToString());
            updata.Add(nameof(PlayInAcc), PlayInAcc.ToString());
            updata.Add(nameof(PlayOutAcc), PlayOutAcc.ToString());
            updata.Add(nameof(TotalPlayTimes), TotalPlayTimes.ToString());
            updata.Add(nameof(KeyInAcc), KeyInAcc.ToString());
            updata.Add(nameof(KeyOutAcc), KeyOutAcc.ToString());
            updata.Add(nameof(RedBonusRec), PrecisionAdj(RedBonusRec).ToString());
            //updata.Add(nameof(H5InAcc), PrecisionAdj(H5InAcc).ToString());
            //updata.Add(nameof(H5OutAcc), PrecisionAdj(H5OutAcc).ToString());
            //updata.Add(nameof(ExBonusAcc), PrecisionAdj(ExBonusAcc).ToString());

            //待抽放水庫 (WEB設置)
            updata.Add(nameof(WaterOutAcc), PrecisionAdj(WaterOutAcc).ToString());
            updata.Add(nameof(WaterInAcc), GetWaterInAcc(gameServer).ToString());
            updata.Add(nameof(RedBonusPool), GetRedBonusPool(gameServer).ToString());

            //洗分限制
            updata.Add(nameof(PayOutLimitDay), PayOutLimitDay.ToString());
            updata.Add(nameof(PayOutLimitWeek), PayOutLimitWeek.ToString());
            updata.Add(nameof(PayOutLimitMonth), PayOutLimitMonth.ToString());
            updata.Add(nameof(PayOutLimitDayMx), PayOutLimitDayMx.ToString());

            //機率參數 (WEB設置)
            if (paraDataChangeFg)
            {
                paraDataChangeFg = false;
                updata.Add(nameof(ParaData), ParaData);
            }

            if (calaSetChangeFg)
            {
                calaSetChangeFg = false;
                updata.Add(nameof(CalcSet), CalcSet);
            }

            if (IsNewReRewardCacheData)
            {
                updata.Add(nameof(RewardCacheData), RewardCacheData); //#260821
            }
            //由遊戲端維護 : 以下值是由遊戲端上傳更新, 因此不需再回傳給遊戲端
            //updata.Add(nameof(WaterColl), WaterColl.ToString());
            //updata.Add(nameof(CalcData), CalcData);
            return updata;
        }

        /// <summary>計算玩家IP機率資料</summary>
        public void CountIpInfo(double totBet, double totWin, int playTimes, double waterColl, double waterInTake, double waterOutTake, string calcData, string calcDataIndep, string betAverage, string resultRec)
        {
            //統計總押得
            PlayInAcc += (decimal)totBet;
            PlayOutAcc += (decimal)totWin;
            TotalPlayTimes += playTimes;

            WaterColl = waterColl;
            if (calcData != "") CalcData = calcData;
            if (calcDataIndep != "") CalcDataIndep = calcDataIndep;
            if (betAverage != "") BetAverage = betAverage;
            if (resultRec != "") ResultRec = resultRec;

            if (waterInTake > 0)
            {
                WaterInAcc -= waterInTake;
                WaterInRec += waterInTake;
            }

            WaterOutAcc -= waterOutTake;
            if (waterOutTake > 0) WaterOutRec += waterOutTake;
        }
        /// <summary>計算玩家取用紅包水庫</summary>
        public void TakeRedBonusPool(double redBonusTake)
        {
            if (redBonusTake > 0)
            {
                RedBonusPool -= redBonusTake;
                RedBonusRec += redBonusTake;
            }
        }

        /// <summary>玩家開洗統計</summary>
        public void KeyInOutRec(double dealAmount)
        {
            if (dealAmount > 0)
            {
                //開分
                double keyin = dealAmount;
                KeyInAcc = KeyInAcc + (decimal)keyin;

                //限制紀錄
                //CheckPayOutLimit(); //開分 記入前 判斷

                KeyInRecDay = PrecisionAdj(KeyInRecDay + keyin);
                PayOutRecDay = PrecisionAdj(PayOutRecDay - keyin);
                PayOutRecWeek = PrecisionAdj(PayOutRecWeek - keyin);
                PayOutRecMonth = PrecisionAdj(PayOutRecMonth - keyin);

                CheckPayOutLimit(); //開分 記入後 判斷
            }
            else if (dealAmount < 0)
            {
                //洗分
                double keyout = -dealAmount;
                KeyOutAcc = KeyOutAcc + (decimal)keyout;

                //限制紀錄
                PayOutRecDay = PrecisionAdj(PayOutRecDay + keyout);
                PayOutRecWeek = PrecisionAdj(PayOutRecWeek + keyout);
                PayOutRecMonth = PrecisionAdj(PayOutRecMonth + keyout);

                CheckPayOutLimit(); //洗分 記入後 判斷
            }
        }

        /// <summary>玩家 H5轉回本地 統計</summary>
        public void H5InRec(double dealAmount)
        {
            if (dealAmount > 0)
            {
                H5InAcc += (decimal)dealAmount;
            }
        }
        /// <summary>玩家 H5本地轉出 統計</summary>
        public void H5OutRec(double dealAmount)
        {
            if (dealAmount > 0)
            {
                H5OutAcc += (decimal)dealAmount;
            }
        }

        /// <summary>玩家開洗歸零 每日</summary>
        public bool ResetPayOutRecDay()
        {
            if (PayOutRecDay != 0 || KeyInRecDay != 0)
            {
                PayOutRecDay = 0;
                KeyInRecDay = 0;
                PayOutOvFgDay = false;
                PayOutOvFgDayMx = false;
                return true;
            }
            return false;
        }
        /// <summary>玩家開洗歸零 每週</summary>
        public bool ResetPayOutRecWeek()
        {
            if (PayOutRecWeek != 0)
            {
                PayOutRecWeek = 0;
                PayOutOvFgWeek = false;
                return true;
            }
            return false;
        }
        /// <summary>玩家開洗歸零 每月</summary>
        public bool ResetPayOutRecMonth()
        {
            if (PayOutRecMonth != 0)
            {
                PayOutRecMonth = 0;
                PayOutOvFgMonth = false;
                return true;
            }
            return false;
        }

        /// <summary>玩家每日開洗歸零DB資料</summary>
        static public Dictionary<string, string> GetResetPayOutRecDayData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>{
                { nameof(PayOutRecDay), "0" },
                { nameof(KeyInRecDay), "0" }
            };
            return updata;
        }
        /// <summary>玩家每日開洗歸零DB條件</summary>
        static public string GetResetPayOutRecDayWhere()
        {
            return $"{nameof(PayOutRecDay)}<>0 OR {nameof(KeyInRecDay)}<>0";
        }
        /// <summary>玩家每週開洗歸零DB資料</summary>
        static public Dictionary<string, string> GetResetPayOutRecWeekData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>{
                { nameof(PayOutRecWeek), "0" }
            };
            return updata;
        }
        /// <summary>玩家每週開洗歸零DB條件</summary>
        static public string GetResetPayOutRecWeekWhere()
        {
            return $"{nameof(PayOutRecWeek)}<>0";
        }
        /// <summary>玩家每月開洗歸零DB資料</summary>
        static public Dictionary<string, string> GetResetPayOutRecMonthData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>{
                { nameof(PayOutRecMonth), "0" }
            };
            return updata;
        }
        /// <summary>玩家每月開洗歸零DB條件</summary>
        static public string GetResetPayOutRecMonthWhere()
        {
            return $"{nameof(PayOutRecMonth)}<>0";
        }

        /// <summary>玩家開洗Over判斷</summary>
        void CheckPayOutLimit()
        {
            PayOutOvFgDayMx = false;
            if (PayOutLimitDayMx > 0)
            {
                double payOutDayMx = PrecisionAdj(KeyInRecDay * PayOutLimitDayMx); //日總開分 * 限制倍率
                if (payOutDayMx < PayOutRecDay)
                {
                    PayOutOvFgDayMx = true;
                }
            }

            if (PayOutLimitDay > 0 && PayOutLimitDay < PayOutRecDay)
            {
                PayOutOvFgDay = true;
            }
            else
            {
                PayOutOvFgDay = false;
            }

            if (PayOutLimitWeek > 0 && PayOutLimitWeek < PayOutRecWeek)
            {
                PayOutOvFgWeek = true;
            }
            else
            {
                PayOutOvFgWeek = false;
            }

            if (PayOutLimitMonth > 0 && PayOutLimitMonth < PayOutRecMonth)
            {
                PayOutOvFgMonth = true;
            }
            else
            {
                PayOutOvFgMonth = false;
            }
        }

        /// <summary>依據Server取得放水水庫值</summary>
        public double GetWaterInAcc(GameServerCode server)
        {
            //放水有遊戲的限制
            if (CheckWaterInServer(server))
            {
                return PrecisionAdj(WaterInAcc);
            }
            return 0;
        }
        /// <summary>依據Server取得紅包放水水庫值</summary>
        public double GetRedBonusPool(GameServerCode server)
        {
            //放水有遊戲的限制
            if (CheckRedBonusServer(server))
            {
                return PrecisionAdj(RedBonusPool);
            }
            return 0;
        }
        /// <summary>檢查Server是否可使用 放水水庫</summary>
        public bool CheckWaterInServer(GameServerCode server)
        {
            if (WaterInServer != null && WaterInServer.Count > 0)
            {
                if (WaterInServer.Contains(server) == false)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>檢查Server是否可使用 紅包放水水庫</summary>
        public bool CheckRedBonusServer(GameServerCode server)
        {
            if (RedBonusServer != null && RedBonusServer.Count > 0)
            {
                if (RedBonusServer.Contains(server) == false)
                {
                    return false;
                }
            }
            return true;
        }


        #region 每日遊玩遊戲列表
        /// <summary>每日遊玩遊戲列表 (每日歸零)</summary>
        List<GameServerCode> DayPlayGames;
        /// <summary>獨立買 每日遊玩遊戲列表 (每日歸零)</summary>
        List<GameServerCode> IndepDayPlayGames;
        /// <summary>加買 每日遊玩遊戲列表 (每日歸零)</summary>
        List<GameServerCode> ExBetDayPlayGames;

        private readonly object DayPlayGamesLock = new();
        /// <summary>重置每日遊玩遊戲列表</summary>
        public void ResetDayPlayGames()
        {
            lock (DayPlayGamesLock)
            {
                DayPlayGames?.Clear();
                IndepDayPlayGames?.Clear();
                ExBetDayPlayGames?.Clear();
            }
        }

        /// <summary>解出 玩家每日遊玩遊戲列表</summary>
        public void GetDBDataDayPlayGames(string normGamesStr, string indepGamesStr, string exBetGamesStr)
        {
            lock (DayPlayGamesLock)
            {
                if (!string.IsNullOrEmpty(normGamesStr))
                {
                    string[] gameServerIds = normGamesStr.Split(',');
                    DayPlayGames = new List<GameServerCode>();
                    foreach (string idStr in gameServerIds)
                    {
                        if (int.TryParse(idStr, out int serverId))
                        {
                            DayPlayGames.Add((GameServerCode)serverId);
                            //if (IpSetDbFg) Console.WriteLine($"玩家[{UserID}]取得遊戲伺服器: {(GameServerCode)serverId}");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(indepGamesStr))
                {
                    string[] gameServerIds = indepGamesStr.Split(',');
                    IndepDayPlayGames = new List<GameServerCode>();
                    foreach (string idStr in gameServerIds)
                    {
                        if (int.TryParse(idStr, out int serverId))
                        {
                            IndepDayPlayGames.Add((GameServerCode)serverId);
                            //Console.WriteLine($"玩家[{UserID}]取得獨立買遊戲伺服器: {(GameServerCode)serverId}");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(exBetGamesStr))
                {
                    string[] gameServerIds = exBetGamesStr.Split(',');
                    ExBetDayPlayGames = new List<GameServerCode>();
                    foreach (string idStr in gameServerIds)
                    {
                        if (int.TryParse(idStr, out int serverId))
                        {
                            ExBetDayPlayGames.Add((GameServerCode)serverId);
                        }
                    }
                }
            }
        }

        /// <summary>檢查 遊玩遊戲列表 今日是否已玩過</summary>
        /// <returns>true=未玩過(並登錄為已玩過), false=已玩過</returns>
        public bool CheckDayPlayGame(GameServerCode gameServer)
        {
            lock (DayPlayGamesLock)
            {
                if (DayPlayGames == null) { DayPlayGames = new(); }
                if (DayPlayGames.Contains(gameServer))
                {
                    return false; //已玩過
                }
                DayPlayGames.Add(gameServer);
                return true;
            }
        }
        /// <summary>檢查 獨立買遊玩遊戲列表 今日是否已玩過</summary>
        /// <returns>true=未玩過(並登錄為已玩過), false=已玩過</returns>
        public bool CheckIndepDayPlayGame(GameServerCode gameServer)
        {
            lock (DayPlayGamesLock)
            {
                if (IndepDayPlayGames == null) { IndepDayPlayGames = new(); }
                if (IndepDayPlayGames.Contains(gameServer))
                {
                    return false; //已玩過
                }
                IndepDayPlayGames.Add(gameServer);
                return true;
            }
        }
        /// <summary>檢查 加買遊玩遊戲列表 今日是否已玩過</summary>x
        /// <returns>true=未玩過(並登錄為已玩過), false=已玩過</returns>
        public bool CheckExBetDayPlayGame(GameServerCode gameServer)
        {
            lock (DayPlayGamesLock)
            {
                if (ExBetDayPlayGames == null) { ExBetDayPlayGames = new(); }
                if (ExBetDayPlayGames.Contains(gameServer))
                {
                    return false; //已玩過
                }
                ExBetDayPlayGames.Add(gameServer);
                return true;
            }
        }

        /// <summary>將DayPlayGames轉成儲存字串</summary>
        public string GetDayPlayGamesStr()
        {
            lock (DayPlayGamesLock)
            {
                if (DayPlayGames != null && DayPlayGames.Count > 0)
                {
                    List<int> serverIds = DayPlayGames.ConvertAll(gs => (int)gs);
                    return string.Join(",", serverIds);
                }
            }
            return "";
        }
        /// <summary>將IndepDayPlayGames轉成儲存字串</summary>
        public string GetIndepDayPlayGamesStr()
        {
            lock (DayPlayGamesLock)
            {
                if (IndepDayPlayGames != null && IndepDayPlayGames.Count > 0)
                {
                    List<int> serverIds = IndepDayPlayGames.ConvertAll(gs => (int)gs);
                    return string.Join(",", serverIds);
                }
            }
            return "";
        }
        /// <summary>將ExbetDayPlayGames轉成儲存字串</summary>
        public string GetExBetDayPlayGamesStr()
        {
            lock (DayPlayGamesLock)
            {
                if (ExBetDayPlayGames != null && ExBetDayPlayGames.Count > 0)
                {
                    //先將 List<GameServerCode> 轉成 List<int>, 再轉成 用逗號串接的字串
                    List<int> serverIds = ExBetDayPlayGames.ConvertAll(gs => (int)gs);
                    // 再用逗號串接成字串
                    return string.Join(",", serverIds);
                }
            }
            return "";
        }
        #endregion

        #endregion (玩家IP模式機率)


        #region 玩家玩家上升額度 #260821
        bool rewardCacheDebug = true;
        /// <summary>玩家玩家上升額度資訊</summary>
        public RewardCacheDataModel rewardCacheDataModel { get; private set; } = new RewardCacheDataModel();
        /// <summary>要傳給玩家的上升額度資訊字串</summary>
        private string RewardCacheData { get; set; }
        /// <summary>Web 設定字串</summary>
        public string RewardWebSetting { get; private set; }
        /// <summary>主紀錄後取得的流水號 (0代表目前無有效紀錄)</summary>
        public int RewardRecordId { get; private set; } = 0;
        /// <summary>新資訊旗號 (非GameServer變更)</summary>
        public bool IsNewReRewardCacheData { get; private set; } = true;
        /// <summary></summary>
        private string ExtraInfo { get; set; }
        /// <summary>功能狀態</summary>
        //public RewardEndStatus RewardEndStatus { get; private set; }

        /// <summary>設定玩家RewardCacheData/RewardWebSetting資料 (Login時Load)</summary>
        public void SetRewardCacheData(Dictionary<string, string> datalist)
        {
            if (datalist.ContainsKey(nameof(RewardCacheData)))
            {
                //#260821
                RewardWebSetting = datalist[nameof(RewardWebSetting)];
                RewardCacheData = datalist[nameof(RewardCacheData)];
                ResetRewardRecordId();
                //MyConsole.WriteLine($"    設定玩家[{UserUID}]RewardCache資料");
            }
            else
            {
                if (rewardCacheDebug) Console.WriteLine($"    玩家[{UserUID}]無RewardCache資料");
            }
        }

        /// <summary>開機重設</summary>
        private void ResetRewardRecordId()
        {
            IsNewReRewardCacheData = true;
            if (RewardCacheData == null || RewardCacheData == "")
            {
                RewardRecordId = 0;
                return;
            }

            string[] data = RewardCacheData.Split(",");
            if (data.Length >= 3)
            {
                RewardRecordId = int.Parse(data[0]);
                //if (RewardRecordId > 0)
                {
                    SetRewardCacheDataModel(RewardRecordId, data);
                }
                if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]重設上升額度資訊: Id={RewardRecordId}");
            }
        }

        /// <summary>Check玩家上升額度資訊紀錄</summary>
        public void CheckInProgressSession(UserSessionRewardRecord session)
        {
            if (rewardCacheDebug) MyConsole.WriteLine($"    檢查玩家[{UserID}]Session");

            if (RewardCacheData != null && RewardCacheData != "")
            {
                string[] rcdata = RewardCacheData.Split(",");
                if (rcdata.Length >= 3)
                {
                    if (int.TryParse(rcdata[0], out int recid))
                    {
                        if (recid == session.RewardRecordId)
                        {
                            RewardRecordId = (int)session.RewardRecordId;
                            IsNewReRewardCacheData = true;
                            SetExtraInfo(session.ExtraInfo);
                            if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]Session 正確");
                            return;
                        }

                        if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]Session 與紀錄不符, 重建");
                    }
                    else
                    {
                        if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]Session 紀錄錯誤A, 重建");
                    }
                }
                else
                {
                    if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]Session 紀錄錯誤B, 重建");
                }
            }
            else
            {
                if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]Session 無紀錄, 重建");
            }

            RewardRecordId = (int)session.RewardRecordId;

            rewardCacheDataModel = new();
            rewardCacheDataModel.RewardRecordId = session.RewardRecordId;
            rewardCacheDataModel.RewardEndStatus = session.RewardEndStatus;
            rewardCacheDataModel.TargetBalance = session.TargetBalance;

            IsNewReRewardCacheData = true;

            (RewardCacheData, ExtraInfo) = ProbCal.InitRewardCacheData(rewardCacheDataModel.RewardRecordId, (int)rewardCacheDataModel.RewardEndStatus, rewardCacheDataModel.TargetBalance);
            rewardCacheDataModel.ExtraInfo = ExtraInfo;
            //RewardCacheData = $"{session.RewardRecordId},{(int)session.RewardEndStatus},{session.TargetBalance}";
            if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]重建Session上升額度資訊: {RewardCacheData}");
            return;
        }

        /// <summary>建立新的玩家上升額度資訊</summary>
        public void CreateNewRewardCacheData(RewardCacheDataModel newData, double openAmount)
        {
            rewardCacheDataModel = new();
            rewardCacheDataModel.RewardRecordId = newData.RewardRecordId;
            rewardCacheDataModel.RewardEndStatus = newData.RewardEndStatus;
            rewardCacheDataModel.TargetBalance = newData.TargetBalance;

            IsNewReRewardCacheData = true;

            (RewardCacheData, ExtraInfo) = ProbCal.InitRewardCacheData(rewardCacheDataModel.RewardRecordId, (int)rewardCacheDataModel.RewardEndStatus, rewardCacheDataModel.TargetBalance);
            rewardCacheDataModel.ExtraInfo = ExtraInfo;
            //RewardCacheData = $"{newData.RewardRecordId},{(int)newData.RewardEndStatus},{newData.TargetBalance}";
            if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]建立新的上升額度資訊: {RewardCacheData}");
        }

        /// <summary>結束玩家上升額度資訊</summary>
        public void EndRewardCacheData()
        {
            RewardRecordId = 0;
            rewardCacheDataModel.RewardRecordId = 0;
            rewardCacheDataModel.RewardEndStatus = RewardEndStatus.PlayerEarlyClose;
            RewardCacheData = "";
            if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]結束上升額度資訊");
        }

        /// <summary>結束玩家功能狀態資訊</summary>
        public void EndRewardEndStatus(RewardEndStatus status)
        {
            //RewardRecordId = 0;
            //rewardCacheDataModel.RewardRecordId = 0;
            rewardCacheDataModel.RewardEndStatus = status;
            //RewardCacheData = "";
            //MyConsole.WriteLine($"    玩家[{UserID}]結束上升額度資訊");
        }

        /// <summary>以 Web 強制關閉上升額度功能；Session 紀錄保持進行中，統計以資料庫快照為準以免覆寫。</summary>
        public void ApplyWebForceClose(UserSessionRewardRecord session)
        {
            RewardRecordId = (int)session.RewardRecordId;
            rewardCacheDataModel.RewardRecordId = session.RewardRecordId;
            rewardCacheDataModel.TargetBalance = session.TargetBalance;
            rewardCacheDataModel.TotalGameCount = session.TotalGameCount;
            rewardCacheDataModel.TotalBet = session.TotalBet;
            rewardCacheDataModel.MaxBet = session.MaxBet;
            rewardCacheDataModel.MinBet = session.MinBet;
            rewardCacheDataModel.MaxBalance = session.MaxBalance;
            rewardCacheDataModel.MinBalance = session.MinBalance;
            rewardCacheDataModel.ExtraInfo = ExtraInfo;
            rewardCacheDataModel.RewardEndStatus = RewardEndStatus.WebForceClose;

            SetRewardCacheDataStatus(RewardEndStatus.WebForceClose);
            IsNewReRewardCacheData = true;

            if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]Web強制關閉上升額度功能: {RewardCacheData}");
        }

        private void SetRewardCacheDataStatus(RewardEndStatus status)
        {
            if (string.IsNullOrEmpty(RewardCacheData))
            {
                return;
            }

            string[] data = RewardCacheData.Split(',');
            if (data.Length < 3)
            {
                return;
            }

            data[1] = ((int)status).ToString();
            RewardCacheData = string.Join(",", data);
        }

        private void SetExtraInfo(string extraInfo)
        {
            ExtraInfo = extraInfo;
            rewardCacheDataModel.ExtraInfo = extraInfo;
        }

        /// <summary>更新玩家上升額度資訊</summary>
        public void UpdateRewardCacheData(string rewardCacheData, string extraInfo, GameServerCode server)
        {
            if (string.IsNullOrEmpty(rewardCacheData))
            {
                if (rewardCacheDebug && server == GameServerCode.Olympus1000Plus) MyConsole.WriteLine($"    玩家[{UserID}]沒有更新上升額度資訊");
                return;
            }

            try
            {
                string[] data = rewardCacheData.Split(',');
                if (data.Length < 3)
                {
                    if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]更新上升額度資訊失敗, 資料格式錯誤: {rewardCacheData}");
                    return;
                }

                int rewardRecordId = int.Parse(data[0]);
                RewardEndStatus rewardEndStatus = (RewardEndStatus)int.Parse(data[1]);

                if (rewardRecordId <= 0 || rewardCacheDataModel == null)
                {
                    RewardCacheData = rewardCacheData; //只紀錄玩家身上的上升額度資訊 

                    if (string.IsNullOrEmpty(extraInfo) == false)
                    {
                        SetExtraInfo(extraInfo);
                    }
                    //沒有有效的上升額度紀錄表, 不須回寫
                    //MyConsole.WriteLine($"    玩家[{UserID}]沒有有效的上升額度紀錄表");
                    return;
                }

                if (IsNewReRewardCacheData) //有新的上升額度紀錄表
                {
                    if (rewardCacheDataModel.RewardRecordId != rewardRecordId) //Serever回傳的上升額度紀錄表流水號, 不是目前的流水號
                    {
                        if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]上升額度 資訊已過時, 不可覆寫!");
                        return;
                    }

                    if(rewardCacheDataModel.RewardEndStatus != RewardEndStatus.InProgress && rewardCacheDataModel.RewardEndStatus != rewardEndStatus)
                    {
                        if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]上升額度 功能狀態已過時, 不可覆寫!");
                        return;
                    }

                    IsNewReRewardCacheData = false; //Server已經收到新的上升額度資訊
                }

                if (rewardCacheDataModel.RewardRecordId <= 0)
                {
                    if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]上升額度紀錄Session已結束, 不可覆寫!");
                    return;
                }

                RewardCacheData = rewardCacheData;

                if (string.IsNullOrEmpty(extraInfo) == false)
                {
                    SetExtraInfo(extraInfo);
                }

                if (rewardCacheDataModel.RewardEndStatus == RewardEndStatus.InProgress)
                {
                    if(rewardEndStatus != RewardEndStatus.InProgress)
                    {
                        if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]上升額度 功能狀態已結束, 不可覆寫!");
                    }
                }

                SetRewardCacheDataModel(rewardRecordId, data);
            }
            catch (Exception ex)
            {
                if (rewardCacheDebug) Console.WriteLine($"    玩家[{UserID}]更新上升額度資訊Exception: {ex}");
            }
        }

        void SetRewardCacheDataModel(int rewardRecordId, string[] data)
        {
            try
            {
                rewardCacheDataModel.RewardRecordId = rewardRecordId;
                rewardCacheDataModel.RewardEndStatus = (RewardEndStatus)int.Parse(data[1]);
                rewardCacheDataModel.TargetBalance = double.Parse(data[2]);

                if (data.Length < 9)
                {
                    if (rewardCacheDebug) MyConsole.WriteLine($"    玩家[{UserID}]更新 rewardCacheDataModel, 沒有遊戲詳細紀錄資料");
                    return;
                }

                rewardCacheDataModel.TotalGameCount = int.Parse(data[3]);
                rewardCacheDataModel.TotalBet = double.Parse(data[4]);
                rewardCacheDataModel.MaxBet = double.Parse(data[5]);
                rewardCacheDataModel.MinBet = double.Parse(data[6]);
                rewardCacheDataModel.MaxBalance = double.Parse(data[7]);
                rewardCacheDataModel.MinBalance = double.Parse(data[8]);
            }
            catch (Exception ex)
            {
                if (rewardCacheDebug) Console.WriteLine($"    玩家[{UserID}]更新 SetRewardCacheDataModel Exception: {ex}");
            }
        }
        #endregion


        #region H5 變數
        /// <summary>玩家H5錢包最後所在</summary>
        public Walletlocation Wallet { get; set; } = Walletlocation.none;
        /// <summary>PGS錢包待取回旗號</summary>
        public bool PGS_Ret { get; set; } = true; //設為true是為了重啟時掃一次玩家錢包位置
        /// <summary>JILI錢包待取回旗號</summary>
        public bool JILI_Ret { get; set; } = true;
        /// <summary>FC錢包待取回旗號</summary>
        public bool FC_Ret { get; set; } = true;
        /// <summary>AceWin錢包待取回旗號</summary>
        public bool AceWin_Ret { get; set; } = true;
        /// <summary>JDB錢包待取回旗號</summary>
        public bool JDB_Ret { get; set; } = true;
        /// <summary>DCT錢包待取回旗號</summary>
        public bool DCT_Ret { get; set; } = true;
        /// <summary>HABA錢包待取回旗號</summary>
        public bool HABA_Ret { get; set; } = true;
        /// <summary>FastSpin錢包待取回旗號</summary>
        public bool FastSpin_Ret { get; set; } = true;
        /// <summary>Spade錢包待取回旗號</summary>
        public bool Spade_Ret { get; set; } = true;
        /// <summary>PlayTech錢包待取回旗號</summary>
        public bool PlayTech_Ret { get; set; } = true;
        /// <summary>CP錢包待取回旗號</summary>
        public bool CP_Ret { get; set; } = true;

        //轉錢無帳號時, 會自動建立玩家帳號的外部商, 不須設判斷旗號
        /// <summary>JILI帳號已建立</summary>
        public bool JILI_Member { get; set; } = false;
        /// <summary>AceWin帳號已建立</summary>
        public bool AceWin_Member { get; set; } = false;
        /// <summary>JDB帳號已建立</summary>
        public bool JDB_Member { get; set; } = false;
        /// <summary>PlayTech帳號已建立</summary>
        public bool PlayTech_Member { get; set; } = false;
        #endregion


        #region 營業模式 開分外送 介紹者獎勵功能
        //營業模式 開分外送 功能 #20240807
        /// <summary>目前執行的營業模式</summary>
        public BusinessModeTableData.EBusinessMode BusinessMode;
        /// <summary>營業模式狀態 (這是尚未定案的欄位)</summary>
        //public BusinessModeTableData.EBusinessAct BusinessAct; //使用KeyOutLimit替代
        /// <summary>營業模式子項目</summary>
        public int BusinessSubMode;
        /// <summary>營業模式每日執行次數</summary>
        public int BusinessRunCount;
        /// <summary>營業模式玩家狀態 (0=新註冊, 1=新玩家) (這是尚未定案的欄位)</summary>
        //public int BusinessPlayerMode;
        /// <summary>每日首充狀態 (0=尚未首充, 1=已首充) (這是尚未定案的欄位)</summary>
        //public int FirstKeyInOfDay;


        //進分獎勵功能
        /// <summary>進分獎勵旗標(true=已送) (營業模式共用: true=已首充, false=尚未首充)</summary>
        public bool KeyInAward;
        /// <summary>出分限制(true=已限制不可出分) (營業模式共用: true=模式執行中, false=模式已解除)</summary>
        public bool KeyOutLimit; //
        /// <summary>玩家押分累積 (營業模式共用: 當前模式總押分)</summary>
        public decimal PlayerPlayTotal;
        /// <summary>登入平台</summary>
        public LgtPlatform LoginPlatform;

        //介紹者獎勵功能
        /// <summary>介紹者Uid</summary>
        public int IntroducerUID;
        /// <summary>獎勵狀態(0未啟動 1獎勵中 2已完成)</summary>
        public int RewardStatus;
        /// <summary>玩家總營利</summary>
        public double UserTotalProfit;
        /// <summary>玩家總押分</summary>
        public decimal PlayerTotalBet;
        /// <summary>總累計獎勵金</summary>
        public double AccTotalReward;
        /// <summary>暫記介紹獎勵金收入</summary>
        public double ReceivingRewardBonus;
        #endregion


        public UserData() { }
        public UserData(int userUID, int entityId)
        {
            UserUID = userUID;
            EntityId = entityId;
        }


        /// <summary>從DB資料更新 各遊戲的玩家個人水庫</summary>
        public void SetDBExtraData(Dictionary<string, string> datalist)
        {
            BuddhasPalm = Convert.ToDouble(datalist["BuddhasPalm"]);
            //MyConsole.WriteLine("獲取玩家[" + UserID + "]各遊戲的個人水庫 [BuddhasPalm =" + BuddhasPalm + "]"); //UNDONE: 取得 玩家額外資訊
        }

        /// <summary>GameServer取得玩家額外資訊</summary>
        public string GetExtraDataInfo(GameServerCode gameServer)
        {
            //資料格式 Key:Data
            //每組資料以";"分隔
            StringBuilder extData = new StringBuilder();

            //玩家強控機率, 出牌開關
            extData.Append($"CalFg:{BoolToStr(CalFg)}");
            extData.Append($";BigJPFg:{BoolToStr(BigJPFg)}"); //字首記得加分號";"
            extData.Append($";BigWaterFg:{BoolToStr(BigWaterFg)}");
            extData.Append($";LasTotBet:{PrecisionAdj(LasTotBet).ToString()}");


            //海王8 如來神掌 玩家累積值
            if (gameServer == GameServerCode.OceanKing8)
            {
                extData.Append(";BuddhasPalm:"); //字首記得加分號";"
                extData.Append(BuddhasPalm.ToString("f2"));
                //extData.Append(";GS"); //字首記得加分號";"
                //extData.Append(((int)GameServerCode.OceanKing8).ToString());
                //extData.Append(":");
            }

            string DataTxt = extData.ToString();

            //MyConsole.WriteLine("取得玩家[" + UserID + "]額外資訊:" + DataTxt); //UNDONE: Show 玩家額外資訊
            return DataTxt;
        }

        /// <summary>從DB資料更新內存</summary>
        public void GetDBData(Dictionary<string, string> datalist, int coinPerStar, out bool upfg)
        {
            UserUID = Convert.ToInt32(datalist["UserUID"]);
            EntityId = Convert.ToInt32(datalist["EntityId"]);
            AspNetUserId = datalist["AspNetUserId"];
            UserID = datalist["UserID"];
            UserPwd = datalist["UserPwd"];
            Nickname = datalist["Nickname"];
            Email = datalist["Email"];
            PhoneNum = datalist["PhoneNum"];
            Sex = Convert.ToInt32(datalist["Sex"]);
            UserBalance = Convert.ToDouble(datalist["UserBalance"]);
            blockFlag = Convert.ToBoolean(datalist["blockFlag"]);
            Usersituation = Convert.ToInt32(datalist["Usersituation"]);
            IsBot = Convert.ToBoolean(datalist["IsBot"]);
            SessionID = Convert.ToDouble(datalist["SessionID"]);
            FirstBetTime = Convert.ToInt32(datalist["FirstBetTime"]);
            CalFg = Convert.ToBoolean(datalist["CalFg"]);
            BigJPFg = Convert.ToBoolean(datalist["BigJPFg"]);
            BigWaterFg = Convert.ToBoolean(datalist["BigWaterFg"]);
            LasTotBet = PrecisionAdj(Convert.ToDouble(datalist["BetRec"]));

            IPGetDBData(datalist); //玩家IP模式機率  #251112

            FRBGetDBData(datalist); //紅包炒場

            //#UNDONE: 營業模式 開分外送 UserData DB讀入 #20240807
            BusinessMode = (BusinessModeTableData.EBusinessMode)Convert.ToInt32(datalist["BusinessMode"]);
            BusinessSubMode = Convert.ToInt32(datalist["BusinessSubMode"]);
            BusinessRunCount = Convert.ToInt32(datalist["BusinessRunCount"]);

            if (datalist.ContainsKey("KeyInAward"))
            {
                //#20231128
                KeyInAward = Convert.ToBoolean(datalist["KeyInAward"]);
                KeyOutLimit = Convert.ToBoolean(datalist["KeyOutLimit"]);
                PlayerPlayTotal = Convert.ToDecimal(datalist["PlayerPlayTotal"]);
            }

            if (datalist.ContainsKey("IntroducerUID"))
            {
                //#20231225
                IntroducerUID = Convert.ToInt32(datalist["IntroducerUID"]);
                RewardStatus = Convert.ToInt32(datalist["RewardStatus"]);
                UserTotalProfit = Convert.ToDouble(datalist["UserTotalProfit"]);
                PlayerTotalBet = Convert.ToDecimal(datalist["PlayerTotalBet"]);
                AccTotalReward = Convert.ToDouble(datalist["AccTotalReward"]);
                ReceivingRewardBonus = Convert.ToDouble(datalist["ReceivingRewardBonus"]);
            }


            Energy = Convert.ToDouble(datalist["Energy"]);
            Star = Convert.ToInt32(datalist["Star"]);
            EnergyToEnergyBar(0, coinPerStar);

            //double energy = Convert.ToDouble(datalist["Energy"]);
            //int star = Convert.ToInt32(datalist["Star"]);
            //int starold = star;
            //EnergyBar = EnergyToEnergyBar(ref energy, ref star, coinPerStar);
            //Energy = energy;
            //Star = star;

            //if (Star != starold || Usersituation != 0) //如果星星數有變 or 有玩家的狀態不為未登入
            //{
            //    Usersituation = 0;
            //    upfg = true; //資料有變, 須寫回DB
            //}
            //else
            {
                upfg = false;
            }
        }

        /// <summary>紀錄玩家最後總押分</summary>
        public void CheckUserLastBet(double lastBet)
        {
            if(lastBet < 0) { return; }

            double tempTotBet = PrecisionAdj(lastBet);
            if (LasTotBet != tempTotBet)
            {
                LasTotBet = tempTotBet;
            }
        }

        /// <summary>依據內存製作更新字典(全部資料更新)</summary>
        //public Dictionary<string, string> GetUpdateData()
        //{
        //    Dictionary<string, string> updata = new Dictionary<string, string>();
        //    updata.Add("UserPwd", "'" + UserPwd + "'");
        //    updata.Add("Nickname", "'" + Nickname + "'");
        //    updata.Add("Sex", Sex.ToString());
        //    updata.Add("UserBalance", UserBalance.ToString());
        //    updata.Add("Email", "'" + Email + "'");
        //    updata.Add("PhoneNum", "'" + PhoneNum + "'");
        //    updata.Add("blockFlag", blockFlag == true ? "1" : "0");
        //    updata.Add("Usersituation", Usersituation.ToString());
        //    updata.Add("Energy", Energy.ToString("f2"));
        //    updata.Add("Star", Star.ToString());
        //    updata.Add("SessionID", SessionID.ToString());
        //    updata.Add("FirstBetTime", FirstBetTime.ToString());
        //    //由網頁設定, 這邊不須更新
        //    //updata.Add("CalFg", CalFg == true ? "1" : "0");
        //
        //    //#UNDONE: 營業模式 開分外送 UserData DB寫出 #20240807
        //
        //
        //    //進分獎勵
        //    updata.Add("KeyInAward", KeyInAward.ToString());
        //    updata.Add("KeyOutLimit", KeyOutLimit.ToString());
        //    updata.Add("PlayerPlayTotal", PlayerPlayTotal.ToString());
        //
        //    //介紹者獎勵功能
        //    updata.Add("RewardStatus", RewardStatus.ToString());
        //    updata.Add("UserTotalProfit", UserTotalProfit.ToString());
        //    updata.Add("PlayerTotalBet", PlayerTotalBet.ToString());
        //    updata.Add("AccTotalReward", AccTotalReward.ToString());
        //    updata.Add("ReceivingRewardBonus", ReceivingRewardBonus.ToString());
        //
        //    //紅包炒場
        //    FRBAddUpdateData(ref updata);
        //
        //    //返水 #250807
        //    //updata.Add("ValidBets", ValidBets.ToString());
        //    //updata.Add("CountRebate", CountRebate.ToString());
        //    //updata.Add("Rebate", Rebate.ToString());
        //
        //    return updata;
        //}

        /// <summary>依據內存製作 玩家金錢 更新字典(遊戲時 經常變動的資料更新)(使用userdataUpdateList更新) (每局遊戲紀錄)</summary>
        public Dictionary<string, string> GetPlayUpdateData()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();
            updata.Add(nameof(UserBalance), UserBalance.ToString("0.####"));
            updata.Add(nameof(Energy), Energy.ToString("0.####"));
            updata.Add(nameof(Star), Star.ToString());
            updata.Add(nameof(Usersituation), Usersituation.ToString());

            updata.Add("BetRec", "'" + LasTotBet.ToString("0.####") + "'");

            //#UNDONE: 營業模式 開分外送 UserData 寫回DB #20240807


            //進分獎勵
            updata.Add(nameof(PlayerPlayTotal), PlayerPlayTotal.ToString("0.####"));

            //介紹者獎勵功能
            updata.Add(nameof(UserTotalProfit), UserTotalProfit.ToString("0.####"));
            updata.Add(nameof(PlayerTotalBet), PlayerTotalBet.ToString("0.####"));

            //返水 #250807
            updata.Add(nameof(ValidBets), ValidBets.ToString("0.####"));
            updata.Add(nameof(CountRebate), CountRebate.ToString("0.####"));
            updata.Add(nameof(Rebate), Rebate.ToString("0.####"));

            //UNDONE: #251112 玩家IP模式機率資料
            updata.Add(nameof(Balance2), Balance2.ToString("0.####"));
            updata.Add(nameof(PlayInAcc), PlayInAcc.ToString("0.####"));
            updata.Add(nameof(PlayOutAcc), PlayOutAcc.ToString("0.####"));
            updata.Add(nameof(TotalPlayTimes), TotalPlayTimes.ToString());
            //updata.Add(nameof(KeyInAcc), KeyInAcc.ToString("0.####"));
            //updata.Add(nameof(KeyOutAcc), KeyOutAcc.ToString("0.####"));
            updata.Add(nameof(ExBonusAcc), ExBonusAcc.ToString("0.####"));
            updata.Add(nameof(WaterColl), WaterColl.ToString("0.####"));
            updata.Add(nameof(RedBonusPool), RedBonusPool.ToString("0.####"));
            updata.Add(nameof(WaterInAcc), WaterInAcc.ToString("0.####"));
            updata.Add(nameof(WaterOutAcc), WaterOutAcc.ToString("0.####"));
            updata.Add(nameof(WaterInRec), WaterInRec.ToString("0.####"));
            updata.Add(nameof(WaterOutRec), WaterOutRec.ToString("0.####"));
            updata.Add(nameof(RedBonusRec), RedBonusRec.ToString("0.####"));
            updata.Add(nameof(CalcData), "'" + CalcData + "'");
            updata.Add(nameof(CalcDataIndep), "'" + CalcDataIndep + "'");
            updata.Add(nameof(BetAverage), "'" + BetAverage + "'");
            updata.Add(nameof(ResultRec), "'" + ResultRec + "'");
            //updata.Add(nameof(ParaData), "'" + ParaData + "'");
            updata.Add(nameof(RewardCacheData), "'" + RewardCacheData + "'");

            return updata;
        }

        /// <summary>依據內存製作 玩家開洗紀錄 更新字典 (開洗時記錄)</summary>
        public Dictionary<string, string> GetPlayUpdateDataRec()
        {
            Dictionary<string, string> updata = new Dictionary<string, string>();

            updata.Add(nameof(KeyInAcc), KeyInAcc.ToString("0.####"));
            updata.Add(nameof(KeyOutAcc), KeyOutAcc.ToString("0.####"));

            updata.Add(nameof(H5InAcc), H5InAcc.ToString("0.####"));
            updata.Add(nameof(H5OutAcc), H5OutAcc.ToString("0.####"));

            updata.Add(nameof(KeyInRecDay), KeyInRecDay.ToString("0.####"));
            updata.Add(nameof(PayOutRecDay), PayOutRecDay.ToString("0.####"));
            updata.Add(nameof(PayOutRecWeek), PayOutRecWeek.ToString("0.####"));
            updata.Add(nameof(PayOutRecMonth), PayOutRecMonth.ToString("0.####"));

            return updata;
        }

        /// <summary>Double值精度調整(小數6位)</summary>
        public static double PrecisionAdj(double dVal)
        {
            dVal = Math.Round(dVal, 6);
            return dVal;
        }

        /// <summary>能量條轉換星星計算</summary>
        public int EnergyToEnergyBar(double addEnergy, int coinPerStar)
        {
            //MARKLU: 能量條轉換星星計算
            //int energyBar = 0;
            int totGrade = coinPerStar;  //設定總級數
            double gradePerCoin = 1d / (100d / (double)totGrade);    //每單位級數/幣

            Energy += addEnergy;

            if (Energy >= totGrade)
            {
                while (true)
                {
                    Energy -= totGrade;
                    Star += 1;
                    if (Energy < totGrade) break;
                }
            }

            if (Energy >= gradePerCoin)
            {
                EnergyBar = (int)Math.Floor(Energy / gradePerCoin);
            }
            else
            {
                EnergyBar = 0;
            }

            return EnergyBar;
        }

        /// <summary>製作H5待取回錢包列表</summary>
        public List<Walletlocation> GetAllWalletCheckList()
        {
            List<Walletlocation> list = new List<Walletlocation>();
            if (PGS_Ret) list.Add(Walletlocation.PGS);
            if (JILI_Ret) list.Add(Walletlocation.JILI);
            if (FC_Ret) list.Add(Walletlocation.FC);
            if (AceWin_Ret) list.Add(Walletlocation.AceWin);
            if (JDB_Ret) list.Add(Walletlocation.JDB);
            if (DCT_Ret) list.Add(Walletlocation.DCT);
            if (HABA_Ret) list.Add(Walletlocation.HABA);
            if (FastSpin_Ret) list.Add(Walletlocation.FastSpin);
            if (Spade_Ret) list.Add(Walletlocation.Spade);
            if (PlayTech_Ret) list.Add(Walletlocation.PlayTech);
            if (CP_Ret) list.Add(Walletlocation.CP);
            return list;
        }
        /// <summary>將指定H5錢包 設為已取回</summary>
        public void SetWalletBack(Walletlocation wall)
        {
            if (wall == Walletlocation.PGS) { PGS_Ret = false; }
            else if (wall == Walletlocation.JILI) { JILI_Ret = false; }
            else if (wall == Walletlocation.FC) { FC_Ret = false; }
            else if (wall == Walletlocation.AceWin) { AceWin_Ret = false; }
            else if (wall == Walletlocation.JDB) { JDB_Ret = false; }
            else if (wall == Walletlocation.DCT) { DCT_Ret = false; }
            else if (wall == Walletlocation.HABA) { HABA_Ret = false; }
            else if (wall == Walletlocation.FastSpin) { FastSpin_Ret = false; }
            else if (wall == Walletlocation.Spade) { Spade_Ret = false; }
            else if (wall == Walletlocation.PlayTech) { PlayTech_Ret = false; }
            else if (wall == Walletlocation.CP) { CP_Ret = false; }
        }
        /// <summary>檢查是否有H5錢包須取回</summary>
        public bool CheckExtGameRet(List<Walletlocation> externGameList)
        {
            if (PGS_Ret && !externGameList.Contains(Walletlocation.PGS))
            {
                PGS_Ret = false; //PGS未啟用, 不須取回
            }
            if (JILI_Ret && !externGameList.Contains(Walletlocation.JILI))
            {
                JILI_Ret = false;
            }
            if (FC_Ret && !externGameList.Contains(Walletlocation.FC))
            {
                FC_Ret = false;
            }
            if (AceWin_Ret && !externGameList.Contains(Walletlocation.AceWin))
            {
                AceWin_Ret = false;
            }
            if (JDB_Ret && !externGameList.Contains(Walletlocation.JDB))
            {
                JDB_Ret = false;
            }
            if (DCT_Ret && !externGameList.Contains(Walletlocation.DCT))
            {
                DCT_Ret = false;
            }
            if (HABA_Ret && !externGameList.Contains(Walletlocation.HABA))
            {
                HABA_Ret = false;
            }
            if (FastSpin_Ret && !externGameList.Contains(Walletlocation.FastSpin))
            {
                FastSpin_Ret = false;
            }
            if (Spade_Ret && !externGameList.Contains(Walletlocation.Spade))
            {
                Spade_Ret = false;
            }
            if (PlayTech_Ret && !externGameList.Contains(Walletlocation.PlayTech))
            {
                PlayTech_Ret = false;
            }
            if (CP_Ret && !externGameList.Contains(Walletlocation.CP))
            {
                CP_Ret = false;
            }

            if (PGS_Ret || JILI_Ret || FC_Ret || AceWin_Ret || JDB_Ret || DCT_Ret || HABA_Ret || FastSpin_Ret || Spade_Ret || PlayTech_Ret || CP_Ret)
            {
                return true;
            }
            return false;
        }

        /// <summary>變更玩家狀態</summary>
        public void ChangeSituation(int status)
        {
            Usersituation = status;

            if (status == 0) //玩家狀態為未登入
            {

            }
        }

        /// <summary>更新玩家密碼</summary>
        public void ChangePassword(string newPwd)
        {
            UserPwd = newPwd;

            //密碼變更後須做的動作
            PlayTech_Member = false;
        }

        #region 紅包炒場 //#250211
        /// <summary>Debug開關</summary>
        const bool FRBDbuFg = false; //_UNDONE: 紅包炒場 DEBUG

        /// <summary>玩家 紅包炒場 啟用開關</summary>
        public bool FeverRedBonusFg;
        /// <summary>玩家 紅包炒場 鎖住不出 (每日重置)</summary>
        public bool FRBLockFg = false;
        /// <summary>紅包炒場 前次押分紀錄</summary>
        public double FRBBetSave = 0;
        /// <summary>紅包炒場 總得分 (每日重置, 換機台重置)</summary>
        public double FeverRedBonusTotalWin = 0; //玩家紅包炒場總得須每日歸零, 其程序併入 "營業模式每日重置" BusinessModeOverDayProc() 裡面
        /// <summary>紅包炒場 總得次數</summary>
        public int FRBTakeCounts = 0;

        /// <summary>玩家紅包炒場 可取用旗號</summary>
        bool FRBActFg = false;

        /// <summary>紅包炒場 未中局數</summary>
        //public int FRBCalCnt = 0;
        /// <summary>紅包炒場 機率模式 (0=正常, 1=逼牌, 2=暫停不出)</summary>
        //public int FRBCalMode = 0;
        /// <summary>紅包炒場 模式計數器</summary>
        //public int FRBCalModeCnt = 0;

        /// <summary>玩家 目前玩的遊戲種類</summary>
        public GameTypeCode PlayGameType;
        /// <summary>玩家 目前玩的遊戲</summary>
        public GameServerCode PlayServerCode = GameServerCode.None; //Initial
        /// <summary>玩家 目前玩的機台</summary>
        public int PlayMachine = 0;

        /// <summary>玩家 連續玩開始的時間</summary>
        public DateTime StartPlayTime;
        /// <summary>玩家 前一次玩的時間</summary>
        public DateTime LastPlayTime;
        /// <summary>玩家 下一次可出紅包的時間</summary>
        public DateTime NextBonusTime;
        /// <summary>玩家 累計玩的局數</summary>
        public int PlayCount;
        /// <summary>紅包下次要出的時間</summary>
        //public DateTime FRBNextOutTime;

        //public Stopwatch Stopwatch1;

        /// <summary>紅包炒場從DB得取資料</summary>
        public void FRBGetDBData(Dictionary<string, string> datalist)
        {
            FeverRedBonusFg = true; //玩家紅包炒場 預設開啟

            //if (datalist.ContainsKey("FeverRedBonusTotalWin"))
            FeverRedBonusTotalWin = Convert.ToDouble(datalist["FeverRedBonusTotalWin"]);
        }
        /// <summary>更新字典 加入紅包炒場資料</summary>
        public void FRBAddUpdateData(ref Dictionary<string, string> updata)
        {
            updata.Add("FeverRedBonusTotalWin", FeverRedBonusTotalWin.ToString());
        }

        /// <summary>關閉玩家 紅包炒場功能</summary>
        public void OffFRBBonus()
        {
            FeverRedBonusFg = false;
        }

        /// <summary>設定玩家 停止出紅包炒場, 連續玩秒數清零</summary>
        void StopFRBBonus(DateTime now)
        {
            LastPlayTime = now;
            StartPlayTime = now; //連續玩秒數清零
            StopFRBBonus();
        }
        /// <summary>設定玩家 停止出紅包炒場</summary>
        void StopFRBBonus()
        {
            FRBActFg = false;
        }
        /// <summary>設定玩家 開始出紅包炒場</summary>
        void StartFRBBonus()
        {
            FRBActFg = true;
        }

        int DbuStatSave = 0;
        /// <summary>Debug Show Message</summary>
        bool FRBDbShow(int stat, string msg, bool force = false)
        {
            if (!FRBDbuFg) return false;

            if (DbuStatSave != stat || force)
            {
                DbuStatSave = stat;
                MyConsole.WriteLine(msg);
                return true;
            }
            return false;
        }
        int DbuStatSave2 = 0;
        /// <summary>Debug Show Message</summary>
        bool FRBDbShow2(int stat, string msg, bool force = false)
        {
            if (!FRBDbuFg) return false;

            if (DbuStatSave2 != stat || force)
            {
                DbuStatSave2 = stat;
                MyConsole.WriteLine(msg);
                return true;
            }
            return false;
        }


        /// <summary>玩家換Server時 要處理的工作</summary>
        public bool ChangeGameServer(GameServerCode nowServer, GameTypeCode nowType, int nowMachine)
        {
            PlayMachine = nowMachine;

            if (nowMachine == 0) //由 VerificationData 呼叫
            {
                FRBBetSave = 0;
                FRBLockFg = false;
                FRBDbShow(3, $"紅包炒場T1: 玩家[{UserID}]炒場解鎖", true);
            }

            if (PlayServerCode != nowServer) //玩家換Server
            {
                FRBDbShow(1, $"紅包炒場T1: 玩家[{UserID}]換Server:[{nowServer}-{nowType}],  舊:[{PlayServerCode}-{PlayGameType}]", true);

                if (PlayServerCode == GameServerCode.None || PlayGameType != nowType) //玩家換遊戲種類
                {
                    //開始新的遊戲計時
                    PlayServerCode = nowServer;
                    PlayGameType = nowType; //保存遊戲種類
                    PlayCount = 0; //換遊戲，遊戲總次數清零
                    NextBonusTime = DateTime.Now;
                    StopFRBBonus(NextBonusTime); //換遊戲，連續玩秒數清零
                    FRBDbShow(2, $"紅包炒場T1: 玩家[{UserID}]換遊戲種類 清零", true);
                    return true;
                }

                PlayServerCode = nowServer;
            }
            return false;
        }

        /// <summary>設定玩家紅包炒場計時器 (InsertUserGamedata2 呼叫)</summary>
        public void SetPlayerFRBTimerProc(GameServerCode nowServer, GameTypeCode nowType, int nowMachine, EntityData entity, double bet, int betCnt)
        {
            //換遊戲種類判斷 (出與不出 都要判斷)
            if (ChangeGameServer(nowServer, nowType, nowMachine))
            {
                return;
            }
            else //玩家還在同一遊戲種類 (因為Server沒換)
            {

            }

            DateTime now = DateTime.Now;

            if (bet > 0) PlayCount += betCnt; //不論押分大小, 只要有玩, 遊戲次數加總

            if (CheckPlayTimeAndBetError(nowType, entity, bet, now))
            {
                return; //押分和停機時間未過
            }
            CheckTimeBetFg = false; //設為未檢查 (下局判斷用)

            if (FRBLockFg)
            {
                return;
            }

            if (!FRBActFg) //目前為 不出狀態
            {
                //遊戲總次數 門檻判斷 (不出時, 才需要判斷)
                if (PlayCount < entity.GetFeverRedBonusGamesLimit(nowType))
                {
                    StartPlayTime = now; //遊戲總次數未達到門檻 連續玩秒數清零
                    //StopFRBBonus();
                    return;
                }

                //連續玩 時間判斷 (不出時, 才需要判斷)
                //TimeSpan elapsed = now - StartPlayTime;
                int startElapsed = (int)Math.Truncate((now - StartPlayTime).TotalSeconds); //玩家 開始連續玩後經過秒數
                int playTimesLimit = entity.GetFeverRedBonusPlayTimesLimit(PlayGameType);
                if (startElapsed > playTimesLimit) //超過連續玩時間限制
                {
                    //LastPlayTime = now;
                    //NextBonusTime = now; //下局立馬可出
                    FRBDbShow(5, $"紅包炒場T2: 玩家[{UserID}]達標 可開始出  上次={StartPlayTime:hh:mm:ss}, 現在={now:hh:mm:ss}, 經過秒數={startElapsed}, 時限={playTimesLimit}秒", true);
                    StartFRBBonus();
                }
            }
            return;
        }

        bool CheckTimeBetFg = false;
        /// <summary>檢查押分和停機時間 (驗證押分時呼叫)</summary>
        public bool CheckPlayTimeAndBetError(GameTypeCode gameType, EntityData entity, double bet, DateTime now)
        {
            //押分驗證時, 且有要求紅包時 才會呼叫
            if (CheckTimeBetFg)
            {
                return false;
            }

            CheckTimeBetFg = true; //設為已檢查過

            if (FRBLockFg)
            {
                LastPlayTime = now;
                return true;
            }


            double nbet;
            if (bet > 0)
            {
                nbet = bet;
                FRBBetSave = bet; //保存前次押分
            }
            else
            {
                nbet = FRBBetSave; //取前次押分
            }
            double betLimit = entity.GetFeverRedBonusMinBetLimit(gameType);
            if (nbet < betLimit) //押分不足
            {
                FRBDbShow(6, $"紅包炒場T0: 玩家[{UserID}]押分不足 清零, {bet}<{betLimit}");
                StopFRBBonus(now); //押分低於門檻值 連續玩秒數清零
                return true;
            }


            int lastElapsed = (int)Math.Truncate((now - LastPlayTime).TotalSeconds); //玩家前一次玩後經過秒數
            int stopTimesLimit = entity.GetFeverRedBonusStopTimesLimit(PlayGameType);
            if (lastElapsed > stopTimesLimit) //超過停機時間限制
            {
                FRBDbShow(7, $"紅包炒場T0: 玩家[{UserID}]停機 清零  上次={LastPlayTime:hh:mm:ss}, 現在={now:hh:mm:ss}, 經過秒數={lastElapsed}, 時限={stopTimesLimit}秒", true);
                StopFRBBonus(now); //連續玩秒數清零
                return true;
            }

            LastPlayTime = now;
            return false;
        }

        /// <summary>玩家可出紅包炒場 (驗證押分時呼叫)</summary>
        public int IsFRBOut(GameTypeCode gameType, EntityData entity, int machine, double bet, DateTime now)
        {
            //押分驗證時, 且有要求紅包時 才會呼叫
            if (FRBLockFg)
            {
                return 1;
            }

            if (!FeverRedBonusFg)
            {
                FRBDbShow2(5, $"紅包炒場T1: 玩家[{UserID}]禁制 不出");
                return 1;
            }

            if (CalFg)
            {
                FRBDbShow2(6, $"紅包炒場T1: 玩家[{UserID}]強控 不出");
                return 1;
            }

            if (PlayMachine != machine)
            {
                NextBonusTime = NextBonusTime.AddSeconds(30);
                FRBDbShow2(7, $"紅包炒場T1: 玩家[{UserID}]機號不同 不出, NB時間={NextBonusTime.ToLongTimeString()} PlayMachine={PlayMachine}, machine={machine}", true);
                return 2;
            }

            if (!FRBActFg)
            {
                FRBDbShow2(8, $"紅包炒場T1: 玩家[{UserID}]未達取用標準 不出");
                return 1;
            }

            double getLimit = entity.PlayerDaysFeverRedBonusLimit;
            if (FeverRedBonusTotalWin >= getLimit)
            {
                FRBDbShow2(9, $"紅包炒場T1: 玩家[{UserID}]紅包1日限制 不出並上鎖");
                FRBLockFg = true; //不用再出
                return 1;
            }

            if (now < NextBonusTime) //now 早於 NextBonusTime
            {
                FRBDbShow2(10, $"紅包炒場T1: 玩家[{UserID}]發放率時間未到 不出");
                return 1;
            }

            DbuStatSave2 = 0;
            return 0; //可以出
        }

        #endregion


        #region 返水 //#250807
        const bool RebateDbuFg = false; //UNDONE: 返水 DEBUG
        /// <summary>玩家的Credit為返水(受返水設定限制)</summary>
        public bool CreditRebateFg { get; private set; }
        /// <summary>玩家的有效押分</summary>
        public double ValidBets { get; private set; }
        /// <summary>玩家的 當期 返水值</summary>
        public double CountRebate { get; private set; }
        /// <summary>玩家的 可領 返水值</summary>
        public double Rebate { get; private set; }
        /// <summary>玩家結算廣播旗號</summary>
        public int RebateSettlementBDCnt { get; private set; }
        /// <summary>廣播序號</summary>
        int RebateSeq;

        /// <summary>從DB取得返水設定值</summary>
        public void RebateGetDBData(Dictionary<string, string> datalist, EntityData entity)
        {
            ValidBets = 0;
            CountRebate = 0;
            Rebate = 0;

            if (!datalist.ContainsKey("ValidBets"))
            {
                if (RebateDbuFg) MyConsole.WriteLine($"    玩家[{UserID}] 取得 有效押分和返水累積失敗, 沒有ValidBets欄位");
                return;
            }
            if (entity == null)
            {
                if (RebateDbuFg) MyConsole.WriteLine($"    玩家[{UserID}] 取得 有效押分和返水累積失敗, EntityData為空");
                return;
            }
            //if (entity.RebateFg == false)
            //if (entity.RebateValid == false && entity.CheckRebateClaimDate(DateTime.Now))
            //{
            //    //if (RebateDbuFg) MyConsole.WriteLine($"    玩家[{UserID}] 取得 有效押分和返水累積失敗, 代理商返水 功能無效");
            //    return;
            //}

            CreditRebateFg = datalist["CreditRebateFg"] == "1";
            ValidBets = Math.Round(Convert.ToDouble(datalist["ValidBets"]), 4);
            CountRebate = Math.Round(Convert.ToDouble(datalist["CountRebate"]), 4);
            Rebate = Math.Round(Convert.ToDouble(datalist["Rebate"]), 4);

            if (RebateDbuFg && (ValidBets > 0 || Rebate > 0)) MyConsole.WriteLine($"玩家[{UserID}] 取得 有效押分[{ValidBets}] 當期返水[{CountRebate}] 可領返水[{Rebate}]");
        }

        /// <summary>計算 即時 返水值</summary>
        public void CountImmedRebate(double pers)
        {
            //使用本次押分計算 (Rebate累計)
            //double nowRebate = nowbet * (pers / 100);
            //Rebate += nowRebate;

            //使用ValidBets計算 (ValidBets累計)
            //Rebate = ValidBets * (pers / 100);

            //使用CountRebate計算
            CountCountRebate(pers);
        }
        /// <summary>累計有效押分</summary>
        public double CountValidBets(double nowbet)
        {
            ValidBets += nowbet;
            return Math.Round(ValidBets, 4);
        }
        /// <summary>計算當期返水值</summary>
        public void CountCountRebate(double pers)
        {
            CountRebate = ValidBets * (pers / 100);
        }
        /// <summary>當期返水結算</summary>
        public bool RebateSettlement()
        {
            bool settfg = false;
            if (CountRebate > 0)
            {
                Rebate += CountRebate;
                RebateSettlementBDCnt = 1;
                settfg = true;
            }
            CountRebate = 0;
            ValidBets = 0;
            return settfg;
        }
        /// <summary>提取當期返水值</summary>
        public double GetRebate(bool creditRebateFg)
        {
            double rebate = Rebate;
            Rebate = 0; //清除可領返水
            //CreditRebateFg = creditRebateFg;
            CreditRebateFg = false; //8/22改為->返水一律不限制
            return rebate;
        }
        /// <summary>重新檢查Credit受限旗號</summary>
        public bool RecheckCreditRebateFg()
        {
            if (CreditRebateFg)
            {
                if (UserBalance <= 0)
                {
                    CreditRebateFg = false; //Credit=0時, 解除限制
                }
            }
            return CreditRebateFg;
        }

        /// <summary>玩家的返水錢包有錢</summary>
        public bool HasRebateWallet()
        {
            if (Rebate > 0 || ValidBets > 0)
                return true;

            return false;
        }

        /// <summary>清除玩家的有效押分和當期返水</summary>
        public void ClearValidBets()
        {
            //#250807 清除玩家的有效投注量和當期返水
            ValidBets = 0;
            CountRebate = 0;
            if (RebateDbuFg) Console.WriteLine($"清除玩家[{UserID}]的有效押分");
        }
        /// <summary>清除玩家的可領返水累積</summary>
        public void ClearRebat()
        {
            Rebate = 0;
            if (RebateDbuFg) Console.WriteLine($"清除玩家[{UserID}]的可領返水累積");
        }

        /// <summary>歸零返水廣播序號</summary>
        public void ResetRebateSeq()
        {
            RebateSeq = 0;
        }
        /// <summary>取得返水廣播序號</summary>
        public string GetRebateSeq()
        {
            RebateSeq++;
            return RebateSeq.ToString();
        }
        #endregion

        /// <summary>布林值轉字串</summary>
        string BoolToStr(bool fg)
        {
            if (fg) return "1";
            else return "0";
        }

        //public int EnergyToEnergyBar(ref double energy, ref int star, int coinPerStar)
        //{
        //    //MARKLU: 能量條轉換星星計算
        //    int energyBar = 0;
        //    int totGrade = coinPerStar;  //設定總級數
        //    double gradePerCoin = 1d / (100d / (double)totGrade);    //每單位級數/幣
        //    
        //    if (energy >= totGrade)
        //    {
        //        while (true)
        //        {
        //            energy -= totGrade;
        //            star += 1;
        //            if (energy < totGrade) break;
        //        }
        //    }
        //
        //    if (energy >= gradePerCoin)
        //    {
        //        energyBar = (int)Math.Floor(energy / gradePerCoin);
        //    }
        //
        //    return energyBar;
        //}

        //public bool GetmemberID(string memberID)
        //{
        //    if (UserID.ToLower() == memberID.ToLower())
        //        return true;
        //    else
        //        return false;
        //}
        //
        //public bool GetmemberPW(string memberPW)
        //{
        //    if (UserPwd.ToLower() == memberPW.ToLower())
        //        return true;
        //    else
        //        return false;
        //}

        /// <summary>
        /// 獲取User資料
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="memberPW"></param>
        /// <returns></returns>
        //public bool GetUserID(string memberID, string memberPW, out Dictionary<string, string> data)
        //{
        //    data = null;
        //    if(GetmemberID(memberID)) // && GetmemberPW(memberPW))
        //    {
        //        if (GetmemberPW(memberPW))
        //        {
        //            data = new Dictionary<string, string>();
        //            data.Add("UserUID", UserUID.ToString());
        //            data.Add("UserID", UserID);
        //            data.Add("Nickname", Nickname);
        //            data.Add("UserBalance", UserBalance.ToString());
        //            data.Add("blockFlag", blockFlag.ToString());
        //            data.Add("Sex", Sex.ToString());
        //            data.Add("IsBot", IsBot.ToString());
        //            data.Add("Energy", EnergyBar.ToString());
        //            data.Add("Star", Star.ToString());
        //            data.Add("SessionID", SessionID.ToString());
        //        }
        //        return true; //有找到玩家
        //    }
        //    return false;
        //}
    }
}
