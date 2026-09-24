using Promotion.Core.Contracts;

namespace Promotion.Core.Domain;

public static class PromotionAmountCalculator
{
    public static decimal Truncate4(decimal value) => decimal.Truncate(value * 10_000m) / 10_000m;

    public static decimal Bonus( ActivitySnapshotDto snapshot, decimal? eligibleDepositAmount) =>
        snapshot.BonusType switch
        {
            BonusType.FixedAmount => snapshot.FixedBonusAmount,
            BonusType.DepositPercentage when eligibleDepositAmount is > 0 =>
                Math.Min(Truncate4(eligibleDepositAmount.Value * snapshot.DepositPercentage / 100m), snapshot.MaxBonusAmount),
            _ => throw new ArgumentException("Invalid bonus calculation inputs.")
        };

    public static decimal RequiredWager(ActivitySnapshotDto snapshot, decimal bonusAmount,
        decimal? eligibleDepositAmount) => snapshot.WagerCalculationType switch
        {
            WagerCalculationType.BonusOnly => Truncate4(checked(bonusAmount * snapshot.WagerMultiplier)),
            WagerCalculationType.DepositAndBonus => Truncate4(checked(
                (bonusAmount + (eligibleDepositAmount ?? 0m)) * snapshot.WagerMultiplier)),
            _ => throw new ArgumentException("Invalid wager calculation type.", nameof(snapshot))
        };

    public static decimal EffectiveWager(decimal gameBetAmount, decimal contributionRate) =>
        Truncate4(checked(gameBetAmount * contributionRate / 100m));

    public static decimal RemainingWager(decimal required, decimal current) => Math.Max(required - current, 0m);
}
