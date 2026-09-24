using Promotion.Core.Contracts;

namespace Promotion.Core.Domain;

public static class ActivityEligibilityEvaluator
{
    public static bool IsEligible(PromotionActivity activity, TriggerType triggerType,
        DateOnly businessDay, decimal? eligibleDepositAmount, IReadOnlyCollection<long> allowedActivityUids,
        int claimedCount)
    {
        if (activity.ActivityStatus != ActivityStatus.Active ||
            !allowedActivityUids.Contains(activity.ActivityUID) || activity.TriggerType != triggerType)
            return false;
        if (activity.StartDate is { } start && businessDay < start) return false;
        if (activity.EndDate is { } end && businessDay >= end) return false;
        var weekdayIndex = ((int)businessDay.DayOfWeek + 6) % 7;
        if (activity.WeekdayMask.Length != 7 || activity.WeekdayMask[weekdayIndex] != '1') return false;
        if (claimedCount >= activity.DailyClaimLimit) return false;
        return triggerType is TriggerType.Deposit or TriggerType.FirstDepositOfBusinessDay
            ? eligibleDepositAmount is { } amount && activity.MinimumDepositAmount is { } minimum && amount >= minimum
            : true;
    }
}
