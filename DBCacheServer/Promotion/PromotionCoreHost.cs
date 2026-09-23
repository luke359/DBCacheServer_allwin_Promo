using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Promotion.Core;
using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Data.MySql;

namespace DBCacheServer
{
    /// <summary>優惠活動核心服務的 DBCache Host 組合根。單一程序內使用 Singleton(單例)。</summary>
    internal static class PromotionCoreHost
    {
        private static readonly object Sync = new object();
        private static readonly BalanceConvertFormula BalanceFormula = ConvertBalance;

        private static IPromotionV2Gateway gateway;
        private static IPromotionCoreService service;
        private static bool ready;
        private static TimeSpan businessDayCutover;
        private static Thread maintenanceThread;
        private static DateOnly? lastCompletedBusinessDay;
        private static DateTime? pendingExecutionTime;
        private const int MaintenanceBatchSize = 200;

        public static bool IsReady => ready;
        public static IPromotionCoreService Service => service;

        /// <summary>開機建構核心服務並呼叫 Initialize。失敗時維持 NotReady，不中斷 DBCache 開機。</summary>
        public static void BuildAndInitialize(TimeSpan businessDayCutover)
        {
            lock (Sync)
            {
                if (ready)
                    return;

                try
                {
                    MysqlAcess mysql = MysqlAcess.GetInstance();
                    IPromotionV2Gateway builtGateway = new PromotionV2Gateway(mysql);
                    IPromotionDataStore dataStore = new PromotionMySqlDataStore(builtGateway);
                    IPromotionCoreService core = new PromotionCoreService(dataStore);

                    PromotionResult<InitializeData> initializeResult = core.Initialize(new InitializeRequest(
                        BusinessDayCutover: businessDayCutover,
                        BalanceConvertFormula: BalanceFormula,
                        CorrelationId: "promotion-host-startup"));

                    if (initializeResult.Kind != PromotionResultKind.Succeeded)
                    {
                        MyConsole.WriteLine(
                            "Promotion Core 初始化失敗：" + initializeResult.ErrorCode
                            + " CorrelationId=" + initializeResult.CorrelationId);
                        ready = false;
                        gateway = null;
                        service = null;
                        return;
                    }

                    gateway = builtGateway;
                    service = core;
                    PromotionCoreHost.businessDayCutover = businessDayCutover;
                    ready = true;
                    StartMaintenanceThread();
                }
                catch (Exception ex)
                {
                    ready = false;
                    gateway = null;
                    service = null;
                    MyConsole.WriteLine("Promotion Core 建構失敗：" + ex.Message);
                }
            }
        }

        /// <summary>目前回傳開機傳入的切換時間。Country 方法完成後改由呼叫端傳入，此處不再改規則。</summary>
        private static TimeSpan GetBusinessDayCutover()
        {
            return businessDayCutover;
        }

        private static void StartMaintenanceThread()
        {
            if (maintenanceThread != null && maintenanceThread.IsAlive)
                return;

            maintenanceThread = new Thread(MaintenanceLoop)
            {
                IsBackground = true,
                Name = "PromotionDailyMaintenance"
            };
            maintenanceThread.Start();
        }

        private static void MaintenanceLoop()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                    TryRunDailyMaintenance();
                }
                catch (Exception ex)
                {
                    MyConsole.WriteLine("Promotion 每日維護迴圈例外：" + ex.Message);
                }
            }
        }

        /// <summary>活動日切換後執行每日維護。同一輪重試沿用原 ExecutionTime；錢包指令先回傳結果，後續再處理。</summary>
        public static PromotionResult<DailyMaintenanceData> TryRunDailyMaintenance()
        {
            if (!ready || service == null)
                return null;

            TimeSpan cutover = GetBusinessDayCutover();
            DateTime now = DateTime.Now;
            if (now.TimeOfDay < cutover)
                return null;

            DateOnly currentBusinessDay = DateOnly.FromDateTime(now - cutover);
            DateTime executionTime;
            lock (Sync)
            {
                if (lastCompletedBusinessDay.HasValue && lastCompletedBusinessDay.Value == currentBusinessDay)
                    return null;
                if (!pendingExecutionTime.HasValue)
                    pendingExecutionTime = now;
                executionTime = pendingExecutionTime.Value;
            }

            try
            {
                PromotionResult<DailyMaintenanceData> result = service.RunDailyMaintenance(new DailyMaintenanceRequest(
                    ExecutionTime: executionTime,
                    BatchSize: MaintenanceBatchSize,
                    CorrelationId: "promotion-daily-" + currentBusinessDay.ToString("yyyyMMdd")));

                if (result.Kind == PromotionResultKind.Succeeded)
                {
                    lock (Sync)
                    {
                        lastCompletedBusinessDay = result.Data != null
                            ? result.Data.CurrentBusinessDay
                            : currentBusinessDay;
                        pendingExecutionTime = null;
                    }
                    MyConsole.WriteLine(
                        "Promotion 每日維護完成：BusinessDay=" + currentBusinessDay
                        + " CorrelationId=" + result.CorrelationId);
                }
                else
                {
                    MyConsole.WriteLine(
                        "Promotion 每日維護未完成：Kind=" + result.Kind
                        + " ErrorCode=" + result.ErrorCode
                        + " CorrelationId=" + result.CorrelationId);
                }

                return result;
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 每日維護例外：" + ex.Message);
                return null;
            }
        }

        /// <summary>玩家註冊成功後建立註冊觸發資格。未就緒、無允許活動或呼叫失敗時不影響建帳。</summary>
        public static void TryCreateRegistrationEligibility(int userUid, string activityUidList)
        {
            if (!ready || service == null || userUid <= 0)
                return;

            try
            {
                IReadOnlyList<long> allowed = GetAllowedActivityUids(activityUidList);
                if (allowed.Count == 0)
                    return;

                DateTime eventTime = DateTime.Now;
                PromotionResult<CreateEligibilityData> result = service.CreateEligibility(new CreateEligibilityRequest(
                    EventId: "reg-" + userUid.ToString(),
                    UserUID: userUid,
                    TriggerType: TriggerType.Registration,
                    EventTime: eventTime,
                    EligibleDepositAmount: null,
                    AllowedActivityUIDs: allowed));

                if (result.Kind != PromotionResultKind.Succeeded)
                {
                    MyConsole.WriteLine(
                        "Promotion 註冊資格建立失敗：UserUID=" + userUid
                        + " Kind=" + result.Kind
                        + " ErrorCode=" + result.ErrorCode
                        + " CorrelationId=" + result.CorrelationId);
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 註冊資格建立例外：UserUID=" + userUid + " " + ex.Message);
            }
        }

        /// <summary>玩家登入成功後送出每日首登候選。是否為當日首次由核心依 EventTime 所屬營業日判斷。</summary>
        public static void TryCreateFirstLoginEligibility(int userUid, string activityUidList)
        {
            if (!ready || service == null || userUid <= 0)
                return;

            try
            {
                IReadOnlyList<long> allowed = GetAllowedActivityUids(activityUidList);
                DateTime eventTime = DateTime.Now;
                string eventId = "login-" + userUid.ToString() + "-" + Guid.NewGuid().ToString("N");
                TryCreateEligibility(
                    userUid, eventId, TriggerType.FirstLoginOfBusinessDay, eventTime, null, allowed);
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 登入資格建立例外：UserUID=" + userUid + " " + ex.Message);
            }
        }

        /// <summary>登入成功後建立免費活動資格。同一營業日沿用 free-{UserUID}-{yyyyMMdd}；已有事件則略過。</summary>
        public static void TryCreateFreeEligibility(int userUid, string activityUidList)
        {
            if (!ready || service == null || gateway == null || userUid <= 0)
                return;

            try
            {
                IReadOnlyList<long> allowed = GetAllowedActivityUids(activityUidList);
                if (allowed.Count == 0)
                    return;

                DateTime eventTime = DateTime.Now;
                DateOnly businessDay = DateOnly.FromDateTime(eventTime - GetBusinessDayCutover());
                string eventId = "free-" + userUid.ToString() + "-" + businessDay.ToString("yyyyMMdd");
                if (TriggerEventExists(eventId))
                    return;

                TryCreateEligibility(userUid, eventId, TriggerType.Free, eventTime, null, allowed);
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 免費活動資格建立例外：UserUID=" + userUid + " " + ex.Message);
            }
        }

        /// <summary>玩家儲值成功後建立一般儲值資格，並另送每日首儲候選。是否為當日首次由核心判斷。</summary>
        public static void TryCreateDepositEligibility(
            int userUid, string activityUidList, string eventId, decimal depositAmount)
        {
            if (!ready || service == null || userUid <= 0 || depositAmount <= 0 || string.IsNullOrWhiteSpace(eventId))
                return;

            try
            {
                IReadOnlyList<long> allowed = GetAllowedActivityUids(activityUidList);
                DateTime eventTime = DateTime.Now;
                string depositEventId = eventId.Trim();
                TryCreateEligibility(
                    userUid, depositEventId, TriggerType.Deposit, eventTime, depositAmount, allowed);
                TryCreateEligibility(
                    userUid, "f" + depositEventId, TriggerType.FirstDepositOfBusinessDay,
                    eventTime, depositAmount, allowed);
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 儲值資格建立例外：UserUID=" + userUid + " " + ex.Message);
            }
        }

        private static void TryCreateEligibility(
            int userUid, string eventId, TriggerType triggerType, DateTime eventTime,
            decimal? eligibleDepositAmount, IReadOnlyList<long> allowed)
        {
            PromotionResult<CreateEligibilityData> result = service.CreateEligibility(new CreateEligibilityRequest(
                EventId: eventId,
                UserUID: userUid,
                TriggerType: triggerType,
                EventTime: eventTime,
                EligibleDepositAmount: eligibleDepositAmount,
                AllowedActivityUIDs: allowed));

            if (result.Kind != PromotionResultKind.Succeeded)
            {
                MyConsole.WriteLine(
                    "Promotion 資格建立失敗：UserUID=" + userUid
                    + " TriggerType=" + triggerType
                    + " EventId=" + eventId
                    + " Kind=" + result.Kind
                    + " ErrorCode=" + result.ErrorCode
                    + " CorrelationId=" + result.CorrelationId);
            }
        }

        /// <summary>一局最終結算後累計流水。未呼叫核心時回傳 null；呼叫後回傳核心結果供後續處理錢包指令。</summary>
        public static PromotionResult<AccumulateWagerData> TryAccumulateWager(
            int userUid, double gameBetAmount, double gameWinAmount, double settledBonusWalletBalance)
        {
            if (!ready || service == null || userUid <= 0)
                return null;

            try
            {
                if (!TryToPromotionAmount(gameBetAmount, out decimal bet) ||
                    !TryToPromotionAmount(gameWinAmount, out decimal win) ||
                    !TryToPromotionAmount(settledBonusWalletBalance, out decimal bonusBalance))
                    return null;

                PromotionResult<BonusTaskStatusData> status = service.GetBonusTaskStatus(
                    new GetBonusTaskStatusRequest(userUid));
                if (status.Kind != PromotionResultKind.Succeeded ||
                    status.Data == null ||
                    !status.Data.HasActiveTask ||
                    status.Data.Task == null)
                    return null;

                PromotionResult<AccumulateWagerData> result = service.AccumulateWager(new AccumulateWagerRequest(
                    UserUID: userUid,
                    ExpectedBonusTaskId: status.Data.Task.BonusTaskId,
                    GameBetAmount: bet,
                    GameWinAmount: win,
                    GameTime: DateTime.Now,
                    SettledBonusWalletBalance: bonusBalance));

                if (result.Kind != PromotionResultKind.Succeeded)
                {
                    MyConsole.WriteLine(
                        "Promotion 累計流水失敗：UserUID=" + userUid
                        + " Kind=" + result.Kind
                        + " ErrorCode=" + result.ErrorCode
                        + " CorrelationId=" + result.CorrelationId);
                }

                return result;
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion 累計流水例外：UserUID=" + userUid + " " + ex.Message);
                return null;
            }
        }

        private static bool TryToPromotionAmount(double value, out decimal amount)
        {
            amount = 0;
            if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
                return false;
            try
            {
                amount = decimal.Truncate(checked((decimal)value) * 10000m) / 10000m;
                return amount <= 9999999999999999.9999m;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        private static bool TriggerEventExists(string eventId)
        {
            Query query = new Query(
                PromotionTableMetadata.Event,
                new[] { "EventId" },
                new[] { new Condition("EventId", Comparison.Equal, eventId) },
                Array.Empty<Sort>(),
                1);
            return gateway.Select(query).Count > 0;
        }

        /// <summary>
        /// 解析逗號分隔的 ActivityUID 名單。null、空字串或僅空白視為無優惠。
        /// 任一非空白片段無法轉成 long 時記錄錯誤，整份名單視為無優惠可用。
        /// </summary>
        private static IReadOnlyList<long> GetAllowedActivityUids(string activityUidList)
        {
            if (string.IsNullOrWhiteSpace(activityUidList))
                return Array.Empty<long>();

            string[] parts = activityUidList.Split(',');
            List<long> ids = new List<long>(parts.Length);
            for (int i = 0; i < parts.Length; i++)
            {
                string token = parts[i].Trim();
                if (token.Length == 0)
                    continue;

                if (!long.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out long id))
                {
                    MyConsole.WriteLine(
                        "Promotion 活動名單解析失敗，視為無優惠可用：token=" + token
                        + " ActivityUIDList=" + activityUidList);
                    return Array.Empty<long>();
                }

                ids.Add(id);
            }

            return ids;
        }

        /// <summary>暫定公式：回傳結算後紅利錢包餘額。正式規則確定後再整段替換。</summary>
        private static double ConvertBalance(BalanceConvertContext context)
        {
            return (double)context.SettledBonusWalletBalance;
        }
    }
}
