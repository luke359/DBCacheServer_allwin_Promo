using Promotion.Core.Contracts;

namespace Promotion.Core;

public static class PromotionResultFactory
{
    public static PromotionResult<T> Success<T>(string correlationId, T data) =>
        new(PromotionResultKind.Succeeded, PromotionErrorCode.None, false, correlationId,
            data ?? throw new ArgumentNullException(nameof(data)), null);

    public static PromotionResult<T> Partial<T>(string correlationId, T data) =>
        new(PromotionResultKind.PartialSucceeded, PromotionErrorCode.MaintenancePartiallySucceeded,
            false, correlationId, data ?? throw new ArgumentNullException(nameof(data)), null);

    public static PromotionResult<T> Failure<T>(string correlationId, PromotionErrorCode code,
        PromotionErrorDetailsDto? details = null)
    {
        if (code is PromotionErrorCode.None or PromotionErrorCode.MaintenancePartiallySucceeded)
            throw new ArgumentOutOfRangeException(nameof(code));
        var kind = code switch
        {
            PromotionErrorCode.DatabaseDeadlock or PromotionErrorCode.DatabaseLockWaitTimeout or
                PromotionErrorCode.DatabaseConnectionFailure or PromotionErrorCode.BalanceConvertFormulaFailed
                => PromotionResultKind.RetryableFailure,
            PromotionErrorCode.CommitOutcomeUnknown => PromotionResultKind.OutcomeUnknown,
            PromotionErrorCode.EventIdConflict or PromotionErrorCode.EligibilityNotFound or
                PromotionErrorCode.EligibilityPlayerMismatch or PromotionErrorCode.EligibilityExpired or
                PromotionErrorCode.EligibilityExcluded or PromotionErrorCode.EligibilityAlreadyClaimed or
                PromotionErrorCode.ActiveBonusTaskExists or PromotionErrorCode.DailyClaimLimitReached or
                PromotionErrorCode.BonusTaskNotFound or PromotionErrorCode.BonusTaskMismatch or
                PromotionErrorCode.BonusWalletNotDepleted or PromotionErrorCode.WagerRequirementNotMet
                => PromotionResultKind.Rejected,
            _ => PromotionResultKind.PermanentFailure
        };
        return new(kind, code, kind == PromotionResultKind.RetryableFailure, correlationId, default, details);
    }
}
