using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Promotion.Core.Contracts;

namespace DBCacheServer
{
    /// <summary>CLIENT 優惠 Command 進入 Host 的輸入。UserUID 取自 CommonInfoData，不採信 Data 內的玩家 ID。</summary>
    internal sealed class PromoClientCommand
    {
        public string Command;
        public int UserUID;
        public IReadOnlyDictionary<string, string> Data;

    }

    /// <summary>Host 回給 DBCache 掛點的結果。掛點負責填 CommonInfoData，並自行保存、執行 WalletInstructions。</summary>
    internal sealed class PromoClientCommandResult
    {
        public string ResponseCommand = "";
        public string Payload;
        public string ErrorCode = "";
        public string Message = "";
        public IReadOnlyList<WalletInstructionDto> WalletInstructions = Array.Empty<WalletInstructionDto>();
    }

    internal static partial class PromotionCoreHost
    {
        /// <summary>
        /// 處理八個優惠 CLIENT Command。不修改錢包。
        /// 活動範圍一律由 Host 依玩家目前所屬代理商取得，不採信 CLIENT 輸入。
        /// </summary>
        public static PromoClientCommandResult HandleClientCommand(PromoClientCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.Command))
                return Fail("", "InvalidIdentifier", "Request 無效");

            string responseCommand = ToResponseCommand(command.Command);
            if (responseCommand == null)
                return Fail("", "InvalidIdentifier", "未知的優惠 Command");

            try
            {
                switch (command.Command)
                {
                    case "PromoGetActivitiesRequest":
                        return GetPlayerPromotionPage(command, responseCommand, false);
                    case "PromoGetPlayerOffersRequest":
                        return GetPlayerPromotionPage(command, responseCommand, true);
                    case "PromoGetGamesRequest":
                        return GetGames(command, responseCommand);
                    case "PromoClaimRequest":
                        return Claim(command, responseCommand);
                    case "PromoGetTaskRequest":
                        return GetTask(command, responseCommand);
                    case "PromoClaimUnlockRequest":
                        return UnlockNotImplemented(responseCommand);
                    case "PromoAbandonTaskRequest":
                        return Abandon(command, responseCommand);
                    case "PromoGetHistoryRequest":
                        return GetHistory(command, responseCommand);
                    default:
                        return Fail(responseCommand, "InvalidIdentifier", "未知的優惠 Command");
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("Promotion CLIENT Command 例外：Command=" + command.Command + " " + ex.Message);
                return Fail(responseCommand, "UnexpectedError", "UnexpectedError");
            }
        }

        /// <summary>
        /// 手動解鎖尚未由核心實作。固定金額仍由達標後的 CloseWagerCompletedBonusTask 自動結案。
        /// 不要把 ConvertType.Balance 當成手動解鎖。
        /// 完成後 PromoGetTaskRequest 才能填 UnlockMode、CanClaimUnlock、EstimatedUnlockAmount。
        /// </summary>
        private static PromoClientCommandResult UnlockNotImplemented(string responseCommand)
        {
            return Fail(responseCommand, "NotImplemented", "手動解鎖尚未由核心提供");
        }

        private static PromoClientCommandResult GetPlayerPromotionPage(PromoClientCommand command,
            string responseCommand, bool includeTodayCompleted)
        {
            if (!TryReady(responseCommand, out PromoClientCommandResult notReady))
                return notReady;

            IReadOnlyList<long> visibleActivityUids = GetVisibleActivityUids(command.UserUID);
            PromotionResult<GetPlayerPromotionPageData> result = service.GetPlayerPromotionPage(
                new GetPlayerPromotionPageRequest(command.UserUID, DateTime.Now, visibleActivityUids,
                    includeTodayCompleted));
            if (!Succeeded(result, responseCommand, out PromoClientCommandResult error))
                return error;

            return Ok(responseCommand, new PromoGetPlayerOffersPayload
            {
                BusinessDay = result.Data.BusinessDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                HasActiveBonusTask = result.Data.HasAnyActiveBonusTask,
                ActiveBonusTaskId = result.Data.ActiveTask?.BonusTaskId ?? "",
                ActiveTask = MapActiveTask(result.Data.ActiveTask),
                Activities = MapActivities(result.Data.Activities),
                TodayCompletedItems = MapTodayCompletedItems(result.Data.TodayCompletedItems)
            });
        }

        private static IReadOnlyList<long> GetVisibleActivityUids(int userUid)
        {
            return GetAllowedActivityUids(Program.GetPromotionActivityUidListForUser(userUid));
        }

        private static List<PromoPlayerPromotionActivityPayload> MapActivities(
            IReadOnlyList<PlayerPromotionActivityDto> activities)
        {
            List<PromoPlayerPromotionActivityPayload> list = new List<PromoPlayerPromotionActivityPayload>();
            if (activities == null)
                return list;
            for (int i = 0; i < activities.Count; i++)
            {
                PlayerPromotionActivityDto item = activities[i];
                list.Add(new PromoPlayerPromotionActivityPayload
                {
                    ActivityUID = item.ActivityUID,
                    Activity = MapActivity(item.Activity),
                    EligibilityEntryId = item.EligibilityEntryId,
                    CanClaim = item.CanClaim,
                    HasActiveTask = item.HasActiveTask,
                    ClaimButtonEnabled = item.ClaimButtonEnabled,
                    ActiveTaskProgress = item.ActiveTaskProgress
                });
            }
            return list;
        }

        private static PromoActivityDisplayPayload MapActivity(ActivitySnapshotDto activity)
        {
            return new PromoActivityDisplayPayload
            {
                ActivityInfo = activity.ActivityInfo ?? "",
                BonusType = (int)activity.BonusType,
                FixedBonusAmount = activity.FixedBonusAmount,
                MaxBonusAmount = activity.MaxBonusAmount,
                DepositPercentage = activity.DepositPercentage,
                MinimumDepositAmount = activity.MinimumDepositAmount,
                WagerMultiplier = activity.WagerMultiplier,
                MaxBetAmount = activity.MaxBetAmount,
                DailyClaimLimit = activity.DailyClaimLimit
            };
        }

        private static PromoActiveBonusTaskPayload MapActiveTask(ActiveBonusTaskDto task)
        {
            if (task == null)
                return null;
            return new PromoActiveBonusTaskPayload
            {
                BonusTaskId = task.BonusTaskId ?? "",
                EligibilityEntryId = task.EligibilityEntryId,
                ActivityUID = task.ActivityUID,
                BusinessDay = task.BusinessDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                BonusAmount = task.BonusAmount,
                RequiredWagerAmount = task.RequiredWagerAmount,
                CurrentWagerAmount = task.CurrentWagerAmount,
                RemainingWagerAmount = task.RemainingWagerAmount,
                MaxBetAmount = task.MaxBetAmount,
                ClaimedAt = FormatTime(task.ClaimedAt)
            };
        }

        private static List<PromoTodayCompletedItemPayload> MapTodayCompletedItems(
            IReadOnlyList<BonusHistoryDto> items)
        {
            List<PromoTodayCompletedItemPayload> list = new List<PromoTodayCompletedItemPayload>();
            if (items == null)
                return list;
            for (int i = 0; i < items.Count; i++)
            {
                BonusHistoryDto item = items[i];
                list.Add(new PromoTodayCompletedItemPayload
                {
                    BonusHistoryId = item.BonusHistoryId,
                    BonusTaskId = item.BonusTaskId ?? "",
                    ActivityUID = item.ActivityUID,
                    ActivityInfo = item.ActivitySnapshot?.ActivityInfo ?? "",
                    BusinessDay = item.BusinessDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    BonusAmount = item.BonusAmount,
                    RequiredWagerAmount = item.RequiredWagerAmount,
                    CurrentWagerAmount = item.CurrentWagerAmount,
                    ConvertedAmount = item.ConvertedAmount,
                    CloseReason = item.CloseReason.ToString(),
                    ClaimedAt = FormatTime(item.ClaimedAt),
                    ClosedAt = FormatTime(item.ClosedAt)
                });
            }
            return list;
        }

        private static PromoClientCommandResult GetGames(PromoClientCommand command, string responseCommand)
        {
            if (!TryReady(responseCommand, out PromoClientCommandResult notReady))
                return notReady;
            if (!TryGetInt64(command, "ActivityUID", out long activityUid))
                return Fail(responseCommand, "InvalidIdentifier", "ActivityUID 無效");

            PromotionResult<GameServerListData> result = service.GetGameServerList(
                new GetGameServerListRequest(activityUid));
            if (!Succeeded(result, responseCommand, out PromoClientCommandResult error))
                return error;

            return Ok(responseCommand, new PromoGetGamesPayload
            {
                ActivityUID = result.Data.ActivityUID,
                GameServerList = result.Data.GameServerList ?? ""
            });
        }

        private static PromoClientCommandResult Claim(PromoClientCommand command, string responseCommand)
        {
            if (!TryReady(responseCommand, out PromoClientCommandResult notReady))
                return notReady;
            if (!TryGetInt64(command, "EligibilityEntryId", out long entryId))
                return Fail(responseCommand, "InvalidIdentifier", "EligibilityEntryId 無效");

            PromotionResult<ClaimPromotionData> result = service.ClaimPromotion(
                new ClaimPromotionRequest(command.UserUID, entryId));
            if (!Succeeded(result, responseCommand, out PromoClientCommandResult error))
                return error;

            ClaimPromotionData data = result.Data;
            return Ok(responseCommand, new PromoClaimPayload
            {
                EligibilityEntryId = data.EligibilityEntryId,
                BonusTaskId = data.BonusTaskId ?? "",
                ActivityUID = data.ActivityUID,
                BusinessDay = data.BusinessDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                BonusAmount = data.BonusAmount,
                RequiredWagerAmount = data.RequiredWagerAmount,
                CurrentWagerAmount = data.CurrentWagerAmount,
                RemainingWagerAmount = data.RemainingWagerAmount,
                MaxBetAmount = data.MaxBetAmount,
                ClaimedAt = FormatTime(data.ClaimedAt),
                IsReplay = data.IsReplay
            }, data.WalletInstructions);
        }

        private static PromoClientCommandResult GetTask(PromoClientCommand command, string responseCommand)
        {
            if (!TryReady(responseCommand, out PromoClientCommandResult notReady))
                return notReady;

            PromotionResult<BonusTaskStatusData> result = service.GetBonusTaskStatus(
                new GetBonusTaskStatusRequest(command.UserUID));
            if (!Succeeded(result, responseCommand, out PromoClientCommandResult error))
                return error;

            PromoGetTaskPayload payload = new PromoGetTaskPayload();
            if (result.Data == null || !result.Data.HasActiveTask || result.Data.Task == null)
                return Ok(responseCommand, payload);

            ActiveBonusTaskDto task = result.Data.Task;
            payload.HasActiveTask = true;
            payload.Task = new PromoTaskPayload
            {
                BonusTaskId = task.BonusTaskId ?? "",
                EligibilityEntryId = task.EligibilityEntryId,
                ActivityUID = task.ActivityUID,
                BonusAmount = task.BonusAmount,
                RequiredWagerAmount = task.RequiredWagerAmount,
                CurrentWagerAmount = task.CurrentWagerAmount,
                RemainingWagerAmount = task.RemainingWagerAmount,
                UnlockMode = "",
                CanClaimUnlock = false,
                EstimatedUnlockAmount = null,
                MaxBetAmount = task.MaxBetAmount,
                ClaimedAt = FormatTime(task.ClaimedAt)
            };
            return Ok(responseCommand, payload);
        }

        private static PromoClientCommandResult Abandon(PromoClientCommand command, string responseCommand)
        {
            if (!TryReady(responseCommand, out PromoClientCommandResult notReady))
                return notReady;
            if (!TryGetText(command, "BonusTaskId", out string bonusTaskId))
                return Fail(responseCommand, "InvalidIdentifier", "BonusTaskId 無效");

            PromotionResult<CloseBonusTaskData> result = service.AbandonBonusTask(new AbandonBonusTaskRequest(
                command.UserUID, bonusTaskId, DateTime.Now));
            if (!Succeeded(result, responseCommand, out PromoClientCommandResult error))
                return error;

            CloseBonusTaskData data = result.Data;
            return Ok(responseCommand, new PromoAbandonPayload
            {
                BonusTaskId = data.BonusTaskId ?? "",
                CloseReason = data.CloseReason.ToString(),
                FinalCurrentWagerAmount = data.FinalCurrentWagerAmount,
                ConvertedAmount = data.ConvertedAmount,
                ClosedAt = FormatTime(data.ClosedAt),
                AlreadyClosed = data.AlreadyClosed
            }, data.WalletInstructions);
        }

        private static PromoClientCommandResult GetHistory(PromoClientCommand command, string responseCommand)
        {
            if (!TryReady(responseCommand, out PromoClientCommandResult notReady))
                return notReady;
            if (!TryGetTime(command, "FromInclusive", out DateTime from) ||
                !TryGetTime(command, "ToExclusive", out DateTime to))
                return Fail(responseCommand, "InvalidDateTime", "歷史查詢時間無效");
            if (!TryGetInt32(command, "Offset", out int offset) || !TryGetInt32(command, "Limit", out int limit))
                return Fail(responseCommand, "InvalidPaginationOrBatchSize", "歷史查詢分頁無效");

            PromotionResult<BonusHistoryPageData> result = service.GetBonusHistory(new GetBonusHistoryRequest(
                command.UserUID, from, to, offset, limit));
            if (!Succeeded(result, responseCommand, out PromoClientCommandResult error))
                return error;

            BonusHistoryPageData data = result.Data;
            List<PromoHistoryItemPayload> items = new List<PromoHistoryItemPayload>();
            if (data.Items != null)
            {
                for (int i = 0; i < data.Items.Count; i++)
                {
                    BonusHistoryDto item = data.Items[i];
                    items.Add(new PromoHistoryItemPayload
                    {
                        BonusHistoryId = item.BonusHistoryId,
                        BonusTaskId = item.BonusTaskId ?? "",
                        ActivityUID = item.ActivityUID,
                        ActivityInfo = item.ActivitySnapshot != null ? item.ActivitySnapshot.ActivityInfo ?? "" : "",
                        BusinessDay = item.BusinessDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        BonusAmount = item.BonusAmount,
                        RequiredWagerAmount = item.RequiredWagerAmount,
                        CurrentWagerAmount = item.CurrentWagerAmount,
                        ConvertedAmount = item.ConvertedAmount,
                        CloseReason = item.CloseReason.ToString(),
                        ClaimedAt = FormatTime(item.ClaimedAt),
                        ClosedAt = FormatTime(item.ClosedAt)
                    });
                }
            }

            return Ok(responseCommand, new PromoGetHistoryPayload
            {
                FromInclusive = FormatTime(data.FromInclusive),
                ToExclusive = FormatTime(data.ToExclusive),
                Offset = data.Offset,
                Limit = data.Limit,
                Items = items
            });
        }

        private static bool TryReady(string responseCommand, out PromoClientCommandResult error)
        {
            if (ready && service != null)
            {
                error = null;
                return true;
            }
            error = Fail(responseCommand, "ServiceNotInitialized", "優惠核心尚未就緒");
            return false;
        }

        private static bool Succeeded<T>(PromotionResult<T> result, string responseCommand, out PromoClientCommandResult error)
        {
            if (result != null && result.Kind == PromotionResultKind.Succeeded && result.Data != null)
            {
                error = null;
                return true;
            }
            string code = result == null ? "UnexpectedError" : result.ErrorCode.ToString();
            error = Fail(responseCommand, code, code);
            return false;
        }

        private static bool TryGetText(PromoClientCommand command, string key, out string value)
        {
            value = null;
            if (command.Data == null || !command.Data.TryGetValue(key, out string raw) || string.IsNullOrWhiteSpace(raw))
                return false;
            value = raw.Trim();
            return true;
        }

        private static bool TryGetInt64(PromoClientCommand command, string key, out long value)
        {
            value = 0;
            return TryGetText(command, key, out string text) &&
                long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value) &&
                value > 0;
        }

        private static bool TryGetInt32(PromoClientCommand command, string key, out int value)
        {
            value = 0;
            return TryGetText(command, key, out string text) &&
                int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryGetTime(PromoClientCommand command, string key, out DateTime value)
        {
            value = default;
            if (!TryGetText(command, key, out string text))
                return false;
            if (!DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out value))
                return false;
            if (value.Kind == DateTimeKind.Utc)
                value = value.ToLocalTime();
            return value.Kind == DateTimeKind.Local || value.Kind == DateTimeKind.Unspecified;
        }

        private static string FormatTime(DateTime value)
        {
            DateTime local = value.Kind == DateTimeKind.Utc ? value.ToLocalTime() : value;
            return local.ToString("yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture);
        }

        private static string ToResponseCommand(string command)
        {
            switch (command)
            {
                case "PromoGetActivitiesRequest": return "PromoGetActivitiesResponse";
                case "PromoGetPlayerOffersRequest": return "PromoGetPlayerOffersResponse";
                case "PromoGetGamesRequest": return "PromoGetGamesResponse";
                case "PromoClaimRequest": return "PromoClaimResponse";
                case "PromoGetTaskRequest": return "PromoGetTaskResponse";
                case "PromoClaimUnlockRequest": return "PromoClaimUnlockResponse";
                case "PromoAbandonTaskRequest": return "PromoAbandonTaskResponse";
                case "PromoGetHistoryRequest": return "PromoGetHistoryResponse";
                default: return null;
            }
        }

        private static PromoClientCommandResult Ok(string responseCommand, object payload, IReadOnlyList<WalletInstructionDto> instructions = null)
        {
            return new PromoClientCommandResult
            {
                ResponseCommand = responseCommand,
                Payload = JsonConvert.SerializeObject(payload),
                WalletInstructions = instructions ?? Array.Empty<WalletInstructionDto>()
            };
        }

        private static PromoClientCommandResult Fail(string responseCommand, string errorCode, string message)
        {
            return new PromoClientCommandResult
            {
                ResponseCommand = responseCommand ?? "",
                ErrorCode = errorCode ?? "",
                Message = message ?? ""
            };
        }
    }

    internal sealed class PromoGetPlayerOffersPayload
    {
        public string BusinessDay = "";
        public bool HasActiveBonusTask;
        public string ActiveBonusTaskId = "";
        public PromoActiveBonusTaskPayload ActiveTask;
        public List<PromoPlayerPromotionActivityPayload> Activities = new List<PromoPlayerPromotionActivityPayload>();
        public List<PromoTodayCompletedItemPayload> TodayCompletedItems = new List<PromoTodayCompletedItemPayload>();
    }

    internal sealed class PromoActivityDisplayPayload
    {
        public string ActivityInfo = "";
        public int BonusType;
        public int FixedBonusAmount;
        public int MaxBonusAmount;
        public int DepositPercentage;
        public int? MinimumDepositAmount;
        public int WagerMultiplier;
        public int? MaxBetAmount;
        public int DailyClaimLimit;
    }

    internal sealed class PromoPlayerPromotionActivityPayload
    {
        public long ActivityUID;
        public PromoActivityDisplayPayload Activity;
        public long? EligibilityEntryId;
        public bool CanClaim;
        public bool HasActiveTask;
        public bool ClaimButtonEnabled;
        public BonusTaskProgressDto ActiveTaskProgress;
    }

    internal sealed class PromoActiveBonusTaskPayload
    {
        public string BonusTaskId = "";
        public long EligibilityEntryId;
        public long ActivityUID;
        public string BusinessDay = "";
        public decimal BonusAmount;
        public decimal RequiredWagerAmount;
        public decimal CurrentWagerAmount;
        public decimal RemainingWagerAmount;
        public int? MaxBetAmount;
        public string ClaimedAt = "";
    }

    internal sealed class PromoTodayCompletedItemPayload
    {
        public long BonusHistoryId;
        public string BonusTaskId = "";
        public long ActivityUID;
        public string ActivityInfo = "";
        public string BusinessDay = "";
        public decimal BonusAmount;
        public decimal RequiredWagerAmount;
        public decimal CurrentWagerAmount;
        public decimal ConvertedAmount;
        public string CloseReason = "";
        public string ClaimedAt = "";
        public string ClosedAt = "";
    }

    internal sealed class PromoGetGamesPayload
    {
        public long ActivityUID;
        public string GameServerList = "";
    }

    internal sealed class PromoClaimPayload
    {
        public long EligibilityEntryId;
        public string BonusTaskId = "";
        public long ActivityUID;
        public string BusinessDay = "";
        public decimal BonusAmount;
        public decimal RequiredWagerAmount;
        public decimal CurrentWagerAmount;
        public decimal RemainingWagerAmount;
        public int? MaxBetAmount;
        public string ClaimedAt = "";
        public bool IsReplay;
    }

    internal sealed class PromoTaskPayload
    {
        public string BonusTaskId = "";
        public long EligibilityEntryId;
        public long ActivityUID;
        public decimal BonusAmount;
        public decimal RequiredWagerAmount;
        public decimal CurrentWagerAmount;
        public decimal RemainingWagerAmount;
        public string UnlockMode = "";
        public bool CanClaimUnlock;
        public decimal? EstimatedUnlockAmount;
        public int? MaxBetAmount;
        public string ClaimedAt = "";
    }

    internal sealed class PromoGetTaskPayload
    {
        public bool HasActiveTask;
        public PromoTaskPayload Task;
    }

    internal sealed class PromoAbandonPayload
    {
        public string BonusTaskId = "";
        public string CloseReason = "";
        public decimal FinalCurrentWagerAmount;
        public decimal ConvertedAmount;
        public string ClosedAt = "";
        public bool AlreadyClosed;
    }

    internal sealed class PromoHistoryItemPayload
    {
        public long BonusHistoryId;
        public string BonusTaskId = "";
        public long ActivityUID;
        public string ActivityInfo = "";
        public string BusinessDay = "";
        public decimal BonusAmount;
        public decimal RequiredWagerAmount;
        public decimal CurrentWagerAmount;
        public decimal ConvertedAmount;
        public string CloseReason = "";
        public string ClaimedAt = "";
        public string ClosedAt = "";
    }

    internal sealed class PromoGetHistoryPayload
    {
        public string FromInclusive = "";
        public string ToExclusive = "";
        public int Offset;
        public int Limit;
        public List<PromoHistoryItemPayload> Items = new List<PromoHistoryItemPayload>();
    }
}
