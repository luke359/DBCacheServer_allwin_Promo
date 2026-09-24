using Promotion.Core.Contracts;

namespace Promotion.Core.Domain;

public static class ActivitySnapshotFactory
{
    public static ActivitySnapshotDto Capture(PromotionActivity activity, DateTime capturedAt)
    {
        DomainInvariantValidator.ValidateActivity(activity);
        if (capturedAt.Kind == DateTimeKind.Utc) throw new ArgumentException("Local time required.", nameof(capturedAt));
        return new ActivitySnapshotDto(2, activity.ActivityUID, activity.ActivityInfo, activity.BonusType,
            activity.FixedBonusAmount, activity.MaxBonusAmount, activity.DepositPercentage,
            activity.MinimumDepositAmount, activity.WagerMultiplier, activity.WagerCalculationType,
            activity.MaxBetAmount,
            activity.WagerContributionRate, activity.ConvertType, activity.FixedConvertedAmount,
            activity.MaxBalanceConvertedAmount, activity.DailyClaimLimit, activity.StartDate,
            activity.EndDate, activity.WeekdayMask, activity.TriggerType, activity.IsNonStackable,
            activity.ExclusiveGroup, capturedAt);
    }
}
