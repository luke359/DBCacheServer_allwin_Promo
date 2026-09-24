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

        /// <summary>
        /// 代理商允許的 ActivityUID 名單，逗號分隔。
        /// PromoGetActivitiesRequest 待核心提供活動目錄查詢後使用；目前不讀核心表。
        /// </summary>
        public string ActivityUidList;
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
        private const int ClaimedHistoryPageSize = 200;

        /// <summary>
        /// 處理八個優惠 CLIENT Command。不修改錢包。
        /// PromoGetActivitiesRequest、PromoClaimUnlockRequest 尚未有對應核心方法，回 NotImplemented。
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
                        return ActivitiesNotImplemented(responseCommand);
                    case "PromoGetPlayerOffersRequest":
                        return GetPlayerOffers(command, responseCommand);
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
        /// 需要核心新增唯讀查詢，例如 GetActivities(AllowedActivityUIDs, QueryTime)。
        /// 回傳本活動日、且落在 ActivityUidList 內的有效活動，不過濾玩家是否已觸發、可領或已領。
        /// 每筆至少要有 ActivityUID、ActivityInfo、TriggerType、BonusType、FixedBonusAmount、
        /// DepositPercentage、MaxBonusAmount、MinimumDepositAmount、WagerMultiplier、DailyClaimLimit。
        /// Host 不直接讀 PromotionActivity。
        /// </summary>
        private static PromoClientCommandResult ActivitiesNotImplemented(string responseCommand)
        {
            return Fail(responseCommand, "NotImplemented", "活動目錄查詢尚未由核心提供");
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

        private static PromoClientCommandResult GetPlayerOffers(PromoClientCommand command, string responseCommand)
        {
            if (!TryReady(responseCommand, out PromoClientCommandResult notReady))
                return notReady;

            DateTime queryTime = DateTime.Now;
            PromotionResult<AvailablePromotionListData> available = service.GetAvailablePromotions(
                new GetAvailablePromotionsRequest(command.UserUID, queryTime));
            if (!Succeeded(available, responseCommand, out PromoClientCommandResult availableError))
                return availableError;

            PromotionResult<BonusTaskStatusData> status = service.GetBonusTaskStatus(
                new GetBonusTaskStatusRequest(command.UserUID));
            if (!Succeeded(status, responseCommand, out PromoClientCommandResult statusError))
                return statusError;

            AvailablePromotionListData data = available.Data;
            List<PromoClaimedOfferPayload> claimed = new List<PromoClaimedOfferPayload>();
            string activeTaskId = "";
            if (status.Data != null && status.Data.HasActiveTask && status.Data.Task != null &&
                status.Data.Task.BusinessDay == data.BusinessDay)
            {
                ActiveBonusTaskDto task = status.Data.Task;
                activeTaskId = task.BonusTaskId ?? "";
                claimed.Add(new PromoClaimedOfferPayload
                {
                    EligibilityEntryId = task.EligibilityEntryId,
                    BonusTaskId = activeTaskId,
                    ActivityUID = task.ActivityUID,
                    ActivityInfo = task.ActivitySnapshot != null ? task.ActivitySnapshot.ActivityInfo ?? "" : "",
                    BonusAmount = task.BonusAmount,
                    ClaimedAt = FormatTime(task.ClaimedAt),
                    TaskState = "Active",
                    CloseReason = ""
                });
            }

            PromoClientCommandResult historyError = AppendClosedOffers(command.UserUID, data.BusinessDay, claimed, responseCommand);
            if (historyError != null)
                return historyError;

            return Ok(responseCommand, new PromoGetPlayerOffersPayload
            {
                BusinessDay = data.BusinessDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                AvailableItems = MapAvailable(data.Items),
                ClaimedItems = claimed,
                HasActiveBonusTask = data.HasActiveBonusTask,
                ActiveBonusTaskId = data.ActiveBonusTaskId ?? ""
            });
        }

        /// <summary>補上本活動日已結案、且不是目前進行中任務的紀錄。</summary>
        private static PromoClientCommandResult AppendClosedOffers(
            int userUid, DateOnly businessDay, List<PromoClaimedOfferPayload> claimed, string responseCommand)
        {
            DateTime from = businessDay.ToDateTime(TimeOnly.FromTimeSpan(businessDayCutover));
            DateTime to = DateTime.Now;
            if (from >= to)
                return null;

            PromotionResult<BonusHistoryPageData> history = service.GetBonusHistory(new GetBonusHistoryRequest(
                userUid, from, to, 0, ClaimedHistoryPageSize));
            if (!Succeeded(history, responseCommand, out PromoClientCommandResult error))
                return error;

            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < claimed.Count; i++)
                seen.Add(claimed[i].BonusTaskId ?? "");

            IReadOnlyList<BonusHistoryDto> items = history.Data.Items;
            for (int i = 0; i < items.Count; i++)
            {
                BonusHistoryDto item = items[i];
                if (item.BusinessDay != businessDay || seen.Contains(item.BonusTaskId ?? ""))
                    continue;
                claimed.Add(new PromoClaimedOfferPayload
                {
                    EligibilityEntryId = item.EligibilityEntryId,
                    BonusTaskId = item.BonusTaskId ?? "",
                    ActivityUID = item.ActivityUID,
                    ActivityInfo = item.ActivitySnapshot != null ? item.ActivitySnapshot.ActivityInfo ?? "" : "",
                    BonusAmount = item.BonusAmount,
                    ClaimedAt = FormatTime(item.ClaimedAt),
                    TaskState = "Closed",
                    CloseReason = item.CloseReason.ToString()
                });
            }

            return null;
        }

        private static List<PromoAvailableOfferPayload> MapAvailable(IReadOnlyList<AvailablePromotionDto> items)
        {
            List<PromoAvailableOfferPayload> list = new List<PromoAvailableOfferPayload>();
            if (items == null)
                return list;
            for (int i = 0; i < items.Count; i++)
            {
                AvailablePromotionDto item = items[i];
                list.Add(new PromoAvailableOfferPayload
                {
                    EligibilityEntryId = item.EligibilityEntryId,
                    ActivityUID = item.ActivityUID,
                    EventId = item.EventId ?? "",
                    ActivityInfo = item.ActivityInfo ?? "",
                    TriggerType = item.TriggerType.ToString(),
                    EligibleDepositAmount = item.EligibleDepositAmount,
                    EstimatedBonusAmount = item.EstimatedBonusAmount,
                    EstimatedRequiredWagerAmount = item.EstimatedRequiredWagerAmount,
                    MaxBetAmount = item.MaxBetAmount
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

    internal sealed class PromoAvailableOfferPayload
    {
        public long EligibilityEntryId;
        public long ActivityUID;
        public string EventId = "";
        public string ActivityInfo = "";
        public string TriggerType = "";
        public decimal? EligibleDepositAmount;
        public decimal EstimatedBonusAmount;
        public decimal EstimatedRequiredWagerAmount;
        public int? MaxBetAmount;
    }

    internal sealed class PromoClaimedOfferPayload
    {
        public long EligibilityEntryId;
        public string BonusTaskId = "";
        public long ActivityUID;
        public string ActivityInfo = "";
        public decimal BonusAmount;
        public string ClaimedAt = "";
        public string TaskState = "";
        public string CloseReason = "";
    }

    internal sealed class PromoGetPlayerOffersPayload
    {
        public string BusinessDay = "";
        public List<PromoAvailableOfferPayload> AvailableItems = new List<PromoAvailableOfferPayload>();
        public List<PromoClaimedOfferPayload> ClaimedItems = new List<PromoClaimedOfferPayload>();
        public bool HasActiveBonusTask;
        public string ActiveBonusTaskId = "";
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
