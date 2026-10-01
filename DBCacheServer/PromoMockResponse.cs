using Protocol;
using System;
using System.Globalization;
using System.Text.Json;

namespace DBCacheServer
{
    /// <summary>只供 CLIENT 通訊測試；不變更優惠資格、任務或錢包。</summary>
    internal static class PromoMockResponse
    {
        internal static bool Enabled =>
            string.Equals(Environment.GetEnvironmentVariable("PROMO_FAKE_RESPONSES"), "true", StringComparison.OrdinalIgnoreCase);

        internal static string CreatePayload(CommonInfoData request)
        {
            DateTime now = DateTime.Now;
            string businessDay = (now.TimeOfDay < TimeSpan.FromHours(8) ? now.Date.AddDays(-1) : now.Date)
                .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string time = now.ToString("o", CultureInfo.InvariantCulture);
            long activityUid = GetLong(request, "ActivityUID", 1001);
            long entryId = GetLong(request, "EligibilityEntryId", 2001);
            string bonusTaskId = GetString(request, "BonusTaskId", "MOCK-TASK-2001");

            switch (request.Command)
            {
                case "PromoGetActivitiesRequest":
                case "PromoGetPlayerOffersRequest":
                    return CreatePlayerPromotionPagePayload(request, businessDay, time,
                        request.Command == "PromoGetPlayerOffersRequest");
                case "PromoGetGamesRequest":
                    return JsonSerializer.Serialize(new { ActivityUID = activityUid, GameServerList = "Wukong" });
                case "PromoClaimRequest":
                    return JsonSerializer.Serialize(new
                    {
                        EligibilityEntryId = entryId, BonusTaskId = "MOCK-TASK-" + entryId,
                        ActivityUID = 1001L, BusinessDay = businessDay, BonusAmount = 10m,
                        RequiredWagerAmount = 300m, CurrentWagerAmount = 0m,
                        RemainingWagerAmount = 300m, MaxBetAmount = (int?)5,
                        ClaimedAt = time, IsReplay = false
                    });
                case "PromoGetTaskRequest":
                    return JsonSerializer.Serialize(new
                    {
                        HasActiveTask = true,
                        Task = new
                        {
                            BonusTaskId = "MOCK-TASK-2002", EligibilityEntryId = 2002L,
                            ActivityUID = 1002L, BonusAmount = 30m, RequiredWagerAmount = 600m,
                            CurrentWagerAmount = 720m, RemainingWagerAmount = 0m,
                            UnlockMode = "Manual", CanClaimUnlock = true,
                            EstimatedUnlockAmount = (decimal?)18m,
                            MaxBetAmount = (int?)5, ClaimedAt = time
                        }
                    });
                case "PromoClaimUnlockRequest":
                    return JsonSerializer.Serialize(new
                    {
                        BonusTaskId = bonusTaskId, FinalCurrentWagerAmount = 720m,
                        ConvertedAmount = 18m, ClosedAt = time, IsReplay = false,
                        WalletStatus = "NotExecuted"
                    });
                case "PromoAbandonTaskRequest":
                    return JsonSerializer.Serialize(new
                    {
                        BonusTaskId = bonusTaskId, CloseReason = "PlayerAbandoned",
                        FinalCurrentWagerAmount = 120m, ConvertedAmount = 0m,
                        ClosedAt = time, AlreadyClosed = false
                    });
                case "PromoGetHistoryRequest":
                    return JsonSerializer.Serialize(new
                    {
                        FromInclusive = GetString(request, "FromInclusive", now.AddDays(-30).ToString("o", CultureInfo.InvariantCulture)),
                        ToExclusive = GetString(request, "ToExclusive", time),
                        Offset = GetInt(request, "Offset", 0), Limit = GetInt(request, "Limit", 20),
                        Items = new[]
                        {
                            new
                            {
                                BonusHistoryId = 3001L, BonusTaskId = "MOCK-TASK-3001",
                                ActivityUID = 1001L, ActivityInfo = "測試免費優惠",
                                BusinessDay = businessDay, BonusAmount = 10m,
                                RequiredWagerAmount = 300m, CurrentWagerAmount = 300m,
                                ConvertedAmount = 5m, CloseReason = "WagerCompleted",
                                ClaimedAt = time, ClosedAt = time
                            }
                        }
                    });
                default:
                    return "{}";
            }
        }

        private static string GetString(CommonInfoData request, string key, string fallback) =>
            request.Data != null && request.Data.TryGetValue(key, out string value) ? value : fallback;

        private static long GetLong(CommonInfoData request, string key, long fallback) =>
            long.TryParse(GetString(request, key, ""), NumberStyles.Integer, CultureInfo.InvariantCulture, out long value)
                ? value : fallback;

        private static int GetInt(CommonInfoData request, string key, int fallback) =>
            int.TryParse(GetString(request, key, ""), NumberStyles.Integer, CultureInfo.InvariantCulture, out int value)
                ? value : fallback;

        private static string CreatePlayerPromotionPagePayload(CommonInfoData request, string businessDay,
            string time, bool includeTodayCompleted)
        {
            object freeActivity = CreateActivityDisplay("測試免費優惠", 1, 10, 0, 0, null, 30, 5, 1);
            object depositActivity = CreateActivityDisplay("測試儲值優惠", 2, 0, 50, 30, 100, 20, 5, 1);

            return JsonSerializer.Serialize(new
            {
                BusinessDay = businessDay,
                HasActiveBonusTask = true,
                ActiveBonusTaskId = "MOCK-TASK-2002",
                ActiveTask = new
                {
                    BonusTaskId = "MOCK-TASK-2002", EligibilityEntryId = 2002L, ActivityUID = 1002L,
                    BusinessDay = businessDay, BonusAmount = 30m, RequiredWagerAmount = 600m,
                    CurrentWagerAmount = 120m, RemainingWagerAmount = 480m, MaxBetAmount = (int?)5,
                    ClaimedAt = time
                },
                Activities = new object[]
                {
                    new
                    {
                        ActivityUID = 1001L, Activity = freeActivity, EligibilityEntryId = (long?)2001L,
                        CanClaim = true, HasActiveTask = false, ClaimButtonEnabled = false,
                        ActiveTaskProgress = (object)null
                    },
                    new
                    {
                        ActivityUID = 1002L, Activity = depositActivity, EligibilityEntryId = (long?)null,
                        CanClaim = false, HasActiveTask = true, ClaimButtonEnabled = false,
                        ActiveTaskProgress = new
                        {
                            CurrentWagerAmount = 120m, RequiredWagerAmount = 600m, RemainingWagerAmount = 480m
                        }
                    }
                },
                TodayCompletedItems = includeTodayCompleted
                    ? new object[]
                    {
                        new
                        {
                            BonusHistoryId = 3001L, BonusTaskId = "MOCK-TASK-3001", ActivityUID = 1001L,
                            ActivityInfo = "測試免費優惠", BusinessDay = businessDay, BonusAmount = 10m,
                            RequiredWagerAmount = 300m, CurrentWagerAmount = 300m, ConvertedAmount = 5m,
                            CloseReason = "WagerCompleted",
                            ClaimedAt = time, ClosedAt = time
                        }
                    }
                    : Array.Empty<object>()
            });
        }

        private static object CreateActivityDisplay(string activityInfo, int bonusType,
            int fixedBonusAmount, int maxBonusAmount, int depositPercentage, int? minimumDepositAmount,
            int wagerMultiplier, int? maxBetAmount, int dailyClaimLimit) => new
            {
                ActivityInfo = activityInfo, BonusType = bonusType,
                FixedBonusAmount = fixedBonusAmount, MaxBonusAmount = maxBonusAmount,
                DepositPercentage = depositPercentage, MinimumDepositAmount = minimumDepositAmount,
                WagerMultiplier = wagerMultiplier, MaxBetAmount = maxBetAmount, DailyClaimLimit = dailyClaimLimit
            };
    }
}
