using Promotion.Core.Contracts;
using Promotion.Core.Domain;

namespace Promotion.Core.Validation;

public static class PromotionRequestValidator
{
    private const decimal MaxStoredAmount = 9_999_999_999_999_999.9999m;

    public static PromotionErrorCode Validate(InitializeRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        var correlation = Correlation(request.CorrelationId);
        if (correlation != PromotionErrorCode.None) return correlation;
        if (!BusinessDayCalculator.IsValidCutover(request.BusinessDayCutover)) return PromotionErrorCode.InvalidDateTime;
        return request.BalanceConvertFormula is null ? PromotionErrorCode.InvalidIdentifier : PromotionErrorCode.None;
    }

    public static PromotionErrorCode Validate(CreateEligibilityRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        if (!ValidEventId(request.EventId) || Correlation(request.CorrelationId) != PromotionErrorCode.None)
            return PromotionErrorCode.InvalidIdentifier;
        if (!Enum.IsDefined(request.TriggerType)) return PromotionErrorCode.InvalidEnumValue;
        if (!Local(request.EventTime)) return PromotionErrorCode.InvalidDateTime;
        if (request.AllowedActivityUIDs is null || request.AllowedActivityUIDs.Any(id => id <= 0))
            return PromotionErrorCode.InvalidActivityScope;
        if (request.TriggerType is TriggerType.Deposit or TriggerType.FirstDepositOfBusinessDay)
            return request.EligibleDepositAmount is { } amount && ValidAmount(amount) && amount > 0
                ? PromotionErrorCode.None : PromotionErrorCode.InvalidEligibleDepositAmount;
        return request.EligibleDepositAmount is null ? PromotionErrorCode.None : PromotionErrorCode.InvalidEligibleDepositAmount;
    }

    public static PromotionErrorCode Validate(GetAvailablePromotionsRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        if (Correlation(request.CorrelationId) != PromotionErrorCode.None) return PromotionErrorCode.InvalidIdentifier;
        return Local(request.QueryTime) ? PromotionErrorCode.None : PromotionErrorCode.InvalidDateTime;
    }

    public static PromotionErrorCode Validate(GetGameServerListRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        return request.ActivityUID > 0 && Correlation(request.CorrelationId) == PromotionErrorCode.None
            ? PromotionErrorCode.None : PromotionErrorCode.InvalidIdentifier;
    }

    public static PromotionErrorCode Validate(ClaimPromotionRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        return request.EligibilityEntryId > 0 && Correlation(request.CorrelationId) == PromotionErrorCode.None
            ? PromotionErrorCode.None : PromotionErrorCode.InvalidIdentifier;
    }

    public static PromotionErrorCode Validate(GetBonusTaskStatusRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        return Correlation(request.CorrelationId);
    }

    public static PromotionErrorCode Validate(AccumulateWagerRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        if (!DomainInvariantValidator.IsGuid(request.ExpectedBonusTaskId) || Correlation(request.CorrelationId) != PromotionErrorCode.None)
            return PromotionErrorCode.InvalidIdentifier;
        if (!Local(request.GameTime)) return PromotionErrorCode.InvalidDateTime;
        return ValidAmount(request.GameBetAmount) && ValidAmount(request.GameWinAmount) &&
            ValidAmount(request.SettledBonusWalletBalance) ? PromotionErrorCode.None : PromotionErrorCode.InvalidAmount;
    }

    public static PromotionErrorCode Validate(CloseWagerCompletedBonusTaskRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        if (!DomainInvariantValidator.IsGuid(request.ExpectedBonusTaskId) || Correlation(request.CorrelationId) != PromotionErrorCode.None)
            return PromotionErrorCode.InvalidIdentifier;
        if (!Local(request.GameTime)) return PromotionErrorCode.InvalidDateTime;
        return ValidAmount(request.SettledBonusWalletBalance) && ValidAmount(request.GameBetAmount) &&
            ValidAmount(request.GameWinAmount) ? PromotionErrorCode.None : PromotionErrorCode.InvalidAmount;
    }

    public static PromotionErrorCode Validate(CloseDepletedBonusTaskRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        if (!DomainInvariantValidator.IsGuid(request.ExpectedBonusTaskId) || Correlation(request.CorrelationId) != PromotionErrorCode.None)
            return PromotionErrorCode.InvalidIdentifier;
        if (!Local(request.NotificationTime)) return PromotionErrorCode.InvalidDateTime;
        return ValidAmount(request.SettledBonusWalletBalance) ? PromotionErrorCode.None : PromotionErrorCode.InvalidAmount;
    }

    public static PromotionErrorCode Validate(AbandonBonusTaskRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        if (!DomainInvariantValidator.IsGuid(request.ExpectedBonusTaskId) || Correlation(request.CorrelationId) != PromotionErrorCode.None)
            return PromotionErrorCode.InvalidIdentifier;
        return Local(request.RequestedAt) ? PromotionErrorCode.None : PromotionErrorCode.InvalidDateTime;
    }

    public static PromotionErrorCode Validate(DailyMaintenanceRequest? request)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (Correlation(request.CorrelationId) != PromotionErrorCode.None) return PromotionErrorCode.InvalidIdentifier;
        if (!Local(request.ExecutionTime)) return PromotionErrorCode.InvalidDateTime;
        return request.BatchSize is >= 1 and <= 1000 ? PromotionErrorCode.None : PromotionErrorCode.InvalidPaginationOrBatchSize;
    }

    public static PromotionErrorCode Validate(GetBonusHistoryRequest? request, DateTime operationTime)
    {
        if (request is null) return PromotionErrorCode.RequestNull;
        if (request.UserUID <= 0) return PromotionErrorCode.InvalidUserUID;
        if (Correlation(request.CorrelationId) != PromotionErrorCode.None) return PromotionErrorCode.InvalidIdentifier;
        if (!Local(request.FromInclusive) || !Local(request.ToExclusive)) return PromotionErrorCode.InvalidDateTime;
        if (request.Offset < 0 || request.Limit is < 1 or > 200) return PromotionErrorCode.InvalidPaginationOrBatchSize;
        return request.FromInclusive < request.ToExclusive &&
            request.ToExclusive - request.FromInclusive <= TimeSpan.FromDays(90) &&
            request.ToExclusive <= operationTime ? PromotionErrorCode.None : PromotionErrorCode.InvalidHistoryRange;
    }

    public static bool ValidAmount(decimal value) =>
        value >= 0 && value <= MaxStoredAmount && value == decimal.Truncate(value * 10_000m) / 10_000m;

    private static bool Local(DateTime value) => value.Kind is DateTimeKind.Local or DateTimeKind.Unspecified;
    private static bool ValidEventId(string? value) => value is { Length: >= 1 and <= 128 } && value == value.Trim();
    private static PromotionErrorCode Correlation(string? value) =>
        value is null || value.Length is >= 1 and <= 128 ? PromotionErrorCode.None : PromotionErrorCode.InvalidIdentifier;
}
