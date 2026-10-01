using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Promotion.ResponseDecoder.Contracts;

namespace Promotion.ResponseDecoder.Decoding;

/// <summary>驗證目前已發布的必要欄位；未知欄位保持向前相容。</summary>
internal static class PromoPayloadContractValidator
{
    internal static bool IsValid(string command, JObject payload) => command switch
    {
        PromoCommands.GetActivitiesResponse => IsPlayerPromotionPage(payload, requireEmptyCompletedItems: true),
        PromoCommands.GetPlayerOffersResponse => IsPlayerPromotionPage(payload, requireEmptyCompletedItems: false),
        PromoCommands.GetGamesResponse => IsGetGames(payload),
        PromoCommands.ClaimResponse => IsClaim(payload),
        PromoCommands.GetTaskResponse => IsGetTask(payload),
        PromoCommands.AbandonTaskResponse => IsAbandon(payload),
        PromoCommands.GetHistoryResponse => IsGetHistory(payload),
        _ => false
    };

    private static bool IsPlayerPromotionPage(JObject value, bool requireEmptyCompletedItems)
    {
        if (!RequireString(value, "BusinessDay", requireContent: true) ||
            !TryGetBoolean(value, "HasActiveBonusTask", out bool hasActiveTask) ||
            !RequireString(value, "ActiveBonusTaskId") ||
            !RequireNullableObject(value, "ActiveTask", out JObject? activeTask) ||
            !RequireArray(value, "Activities", IsPlayerPromotionActivity, out _) ||
            !RequireArray(value, "TodayCompletedItems", IsBonusHistory, out JArray? completedItems))
            return false;

        if (hasActiveTask != (activeTask is not null))
            return false;
        if (activeTask is not null && !IsActiveBonusTask(activeTask))
            return false;
        return !requireEmptyCompletedItems || completedItems!.Count == 0;
    }

    private static bool IsPlayerPromotionActivity(JObject value)
    {
        if (!RequireInteger(value, "ActivityUID") ||
            !RequireObject(value, "Activity", out JObject? activity) ||
            !RequireNullableInteger(value, "EligibilityEntryId") ||
            !RequireBoolean(value, "CanClaim") ||
            !TryGetBoolean(value, "HasActiveTask", out bool hasActiveTask) ||
            !RequireBoolean(value, "ClaimButtonEnabled") ||
            !RequireNullableObject(value, "ActiveTaskProgress", out JObject? activeTaskProgress))
            return false;

        if (!IsActivityDisplay(activity!))
            return false;
        if (hasActiveTask != (activeTaskProgress is not null))
            return false;
        return activeTaskProgress is null || IsBonusTaskProgress(activeTaskProgress);
    }

    private static bool IsActivityDisplay(JObject value) =>
        RequireNullableString(value, "ActivityInfo") &&
        RequireInteger(value, "BonusType") &&
        RequireInteger(value, "FixedBonusAmount") &&
        RequireInteger(value, "MaxBonusAmount") &&
        RequireInteger(value, "DepositPercentage") &&
        RequireNullableInteger(value, "MinimumDepositAmount") &&
        RequireInteger(value, "WagerMultiplier") &&
        RequireNullableInteger(value, "MaxBetAmount") &&
        RequireInteger(value, "DailyClaimLimit");

    private static bool IsActiveBonusTask(JObject value)
    {
        return RequireString(value, "BonusTaskId", requireContent: true) &&
            RequireInteger(value, "EligibilityEntryId") &&
            RequireInteger(value, "ActivityUID") &&
            RequireString(value, "BusinessDay", requireContent: true) &&
            RequireNumber(value, "BonusAmount") &&
            RequireNumber(value, "RequiredWagerAmount") &&
            RequireNumber(value, "CurrentWagerAmount") &&
            RequireNumber(value, "RemainingWagerAmount") &&
            RequireNullableInteger(value, "MaxBetAmount") &&
            RequireString(value, "ClaimedAt", requireContent: true);
    }

    private static bool IsBonusTaskProgress(JObject value) =>
        RequireNumber(value, "CurrentWagerAmount") &&
        RequireNumber(value, "RequiredWagerAmount") &&
        RequireNumber(value, "RemainingWagerAmount");

    private static bool IsBonusHistory(JObject value)
    {
        return RequireInteger(value, "BonusHistoryId") &&
            RequireString(value, "BonusTaskId", requireContent: true) &&
            RequireInteger(value, "ActivityUID") &&
            RequireString(value, "BusinessDay", requireContent: true) &&
            RequireString(value, "ActivityInfo") &&
            RequireNumber(value, "BonusAmount") &&
            RequireNumber(value, "RequiredWagerAmount") &&
            RequireNumber(value, "CurrentWagerAmount") &&
            RequireNumber(value, "ConvertedAmount") &&
            RequireString(value, "CloseReason", requireContent: true) &&
            RequireString(value, "ClaimedAt", requireContent: true) &&
            RequireString(value, "ClosedAt", requireContent: true);
    }

    private static bool IsGetGames(JObject value) =>
        RequireInteger(value, "ActivityUID") && RequireString(value, "GameServerList");

    private static bool IsClaim(JObject value) =>
        RequireInteger(value, "EligibilityEntryId") &&
        RequireString(value, "BonusTaskId", requireContent: true) &&
        RequireInteger(value, "ActivityUID") &&
        RequireString(value, "BusinessDay", requireContent: true) &&
        RequireNumber(value, "BonusAmount") &&
        RequireNumber(value, "RequiredWagerAmount") &&
        RequireNumber(value, "CurrentWagerAmount") &&
        RequireNumber(value, "RemainingWagerAmount") &&
        RequireNullableInteger(value, "MaxBetAmount") &&
        RequireString(value, "ClaimedAt", requireContent: true) &&
        RequireBoolean(value, "IsReplay");

    private static bool IsGetTask(JObject value)
    {
        if (!TryGetBoolean(value, "HasActiveTask", out bool hasActiveTask) ||
            !RequireNullableObject(value, "Task", out JObject? task))
            return false;
        if (hasActiveTask != (task is not null))
            return false;
        return task is null || IsTask(task);
    }

    private static bool IsTask(JObject value) =>
        RequireString(value, "BonusTaskId", requireContent: true) &&
        RequireInteger(value, "EligibilityEntryId") &&
        RequireInteger(value, "ActivityUID") &&
        RequireNumber(value, "BonusAmount") &&
        RequireNumber(value, "RequiredWagerAmount") &&
        RequireNumber(value, "CurrentWagerAmount") &&
        RequireNumber(value, "RemainingWagerAmount") &&
        RequireString(value, "UnlockMode") &&
        RequireBoolean(value, "CanClaimUnlock") &&
        RequireNullableNumber(value, "EstimatedUnlockAmount") &&
        RequireNullableInteger(value, "MaxBetAmount") &&
        RequireString(value, "ClaimedAt", requireContent: true);

    private static bool IsAbandon(JObject value) =>
        RequireString(value, "BonusTaskId", requireContent: true) &&
        RequireString(value, "CloseReason", requireContent: true) &&
        RequireNumber(value, "FinalCurrentWagerAmount") &&
        RequireNumber(value, "ConvertedAmount") &&
        RequireString(value, "ClosedAt", requireContent: true) &&
        RequireBoolean(value, "AlreadyClosed");

    private static bool IsGetHistory(JObject value) =>
        RequireString(value, "FromInclusive", requireContent: true) &&
        RequireString(value, "ToExclusive", requireContent: true) &&
        RequireInteger(value, "Offset") &&
        RequireInteger(value, "Limit") &&
        RequireArray(value, "Items", IsHistoryItem, out _);

    private static bool IsHistoryItem(JObject value) =>
        RequireInteger(value, "BonusHistoryId") &&
        RequireString(value, "BonusTaskId", requireContent: true) &&
        RequireInteger(value, "ActivityUID") &&
        RequireString(value, "ActivityInfo") &&
        RequireString(value, "BusinessDay", requireContent: true) &&
        RequireNumber(value, "BonusAmount") &&
        RequireNumber(value, "RequiredWagerAmount") &&
        RequireNumber(value, "CurrentWagerAmount") &&
        RequireNumber(value, "ConvertedAmount") &&
        RequireString(value, "CloseReason", requireContent: true) &&
        RequireString(value, "ClaimedAt", requireContent: true) &&
        RequireString(value, "ClosedAt", requireContent: true);

    private static bool RequireObject(JObject value, string propertyName, out JObject? property)
    {
        property = null;
        if (!TryGet(value, propertyName, out JToken? token))
            return false;
        property = token as JObject;
        return property is not null;
    }

    private static bool RequireNullableObject(JObject value, string propertyName, out JObject? property)
    {
        property = null;
        return TryGet(value, propertyName, out JToken? token) &&
            (token.Type == JTokenType.Null || (property = token as JObject) is not null);
    }

    private static bool RequireArray(JObject value, string propertyName, Func<JObject, bool> validateItem,
        out JArray? property)
    {
        property = null;
        if (!TryGet(value, propertyName, out JToken? token) || token is not JArray array)
            return false;

        foreach (JToken item in array)
        {
            if (item is not JObject itemObject || !validateItem(itemObject))
                return false;
        }

        property = array;
        return true;
    }

    private static bool RequireString(JObject value, string propertyName, bool requireContent = false)
    {
        if (!TryGet(value, propertyName, out JToken? token) || token.Type != JTokenType.String)
            return false;
        return !requireContent || !string.IsNullOrWhiteSpace(token.Value<string>());
    }

    private static bool RequireNullableString(JObject value, string propertyName) =>
        TryGet(value, propertyName, out JToken? token) &&
        (token.Type == JTokenType.Null || token.Type == JTokenType.String);

    private static bool RequireBoolean(JObject value, string propertyName) =>
        TryGet(value, propertyName, out JToken? token) && token.Type == JTokenType.Boolean;

    private static bool TryGetBoolean(JObject value, string propertyName, out bool property)
    {
        property = false;
        if (!TryGet(value, propertyName, out JToken? token) || token.Type != JTokenType.Boolean)
            return false;
        property = token.Value<bool>();
        return true;
    }

    private static bool RequireInteger(JObject value, string propertyName) =>
        TryGet(value, propertyName, out JToken? token) && token.Type == JTokenType.Integer;

    private static bool RequireNullableInteger(JObject value, string propertyName) =>
        TryGet(value, propertyName, out JToken? token) &&
        (token.Type == JTokenType.Null || token.Type == JTokenType.Integer);

    private static bool RequireNumber(JObject value, string propertyName) =>
        TryGet(value, propertyName, out JToken? token) && IsNumber(token);

    private static bool RequireNullableNumber(JObject value, string propertyName) =>
        TryGet(value, propertyName, out JToken? token) &&
        (token.Type == JTokenType.Null || IsNumber(token));

    private static bool IsNumber(JToken token) => token.Type is JTokenType.Integer or JTokenType.Float;

    private static bool TryGet(JObject value, string propertyName, [NotNullWhen(true)] out JToken? property)
    {
        property = value.GetValue(propertyName, StringComparison.Ordinal);
        return property is not null;
    }
}
