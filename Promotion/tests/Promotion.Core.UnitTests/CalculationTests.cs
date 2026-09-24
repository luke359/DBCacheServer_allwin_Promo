using Promotion.Core.Contracts;
using Promotion.Core.Domain;
using Xunit;

namespace Promotion.Core.UnitTests;

public sealed class CalculationTests
{
    [Fact]
    public void CAL_001_FixedBonusIsExact() => Assert.Equal(10m, PromotionAmountCalculator.Bonus(TestActivity.Snapshot(), null));

    [Fact]
    public void CAL_002_003_004_PercentageTruncatesAndCaps()
    {
        var snapshot = TestActivity.Snapshot() with { BonusType = BonusType.DepositPercentage };
        Assert.Equal(3.7036m, PromotionAmountCalculator.Bonus(snapshot, 12.3456m));
        Assert.Equal(100m, PromotionAmountCalculator.Bonus(snapshot, 1000m));
        Assert.Equal(3m, PromotionAmountCalculator.Bonus(snapshot with { MaxBonusAmount = 3 }, 10m));
    }

    [Fact]
    public void CAL_005_006_007_008_WagerAndRemaining()
    {
        Assert.Equal(111.1080m, PromotionAmountCalculator.RequiredWager(TestActivity.Snapshot(), 3.7036m, 12.3456m));
        Assert.Equal(3.3333m, PromotionAmountCalculator.EffectiveWager(10.0001m, 33.3333m));
        Assert.Equal(0m, PromotionAmountCalculator.RemainingWager(100m, 99.9999m + 0.0001m));
        Assert.Equal(0m, PromotionAmountCalculator.RemainingWager(100m, 101m));
    }

    [Fact]
    public void NewWagerModesAndUnlimitedBalanceConversion()
    {
        var snapshot = TestActivity.Snapshot();
        Assert.Equal(300m, PromotionAmountCalculator.RequiredWager(snapshot, 10m, 25m));
        Assert.Equal(1050m, PromotionAmountCalculator.RequiredWager(
            snapshot with { WagerCalculationType = WagerCalculationType.DepositAndBonus }, 10m, 25m));
        Assert.Equal(300m, PromotionAmountCalculator.RequiredWager(
            snapshot with { WagerCalculationType = WagerCalculationType.DepositAndBonus }, 10m, null));
        Assert.Equal(7.1234m, ConvertedAmountCalculator.Calculate(
            snapshot with { ConvertType = ConvertType.Balance, MaxBalanceConvertedAmount = null },
            _ => 7.12349, Context(snapshot)));
    }

    [Fact]
    public void CAL_009_010_011_012_FixedAndBalanceConversion()
    {
        var snapshot = TestActivity.Snapshot();
        Assert.Equal(50m, ConvertedAmountCalculator.Calculate(snapshot with { FixedConvertedAmount = 50 }, null, null));
        var context = Context(snapshot);
        Assert.Equal(7.1234m, ConvertedAmountCalculator.Calculate(
            snapshot with { ConvertType = ConvertType.Balance }, _ => 7.12349, context));
        Assert.Equal(5m, ConvertedAmountCalculator.Calculate(
            snapshot with { ConvertType = ConvertType.Balance, MaxBalanceConvertedAmount = 5 }, _ => 7.12349, context));
        Assert.Equal(0m, ConvertedAmountCalculator.Calculate(
            snapshot with { ConvertType = ConvertType.Balance }, _ => 0, context));
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(-1)]
    [InlineData(double.MaxValue)]
    public void CAL_013_InvalidFormulaResultFails(double value)
    {
        var snapshot = TestActivity.Snapshot() with { ConvertType = ConvertType.Balance };
        Assert.Throws<BalanceConvertFormulaException>(() => ConvertedAmountCalculator.Calculate(snapshot, _ => value, Context(snapshot)));
    }

    [Fact]
    public void CAL_013_FormulaExceptionFails()
    {
        var snapshot = TestActivity.Snapshot() with { ConvertType = ConvertType.Balance };
        Assert.Throws<BalanceConvertFormulaException>(() => ConvertedAmountCalculator.Calculate(snapshot,
            _ => throw new InvalidOperationException(), Context(snapshot)));
    }

    [Fact]
    public void CAL_014_SameFormulaInputProducesSameResult()
    {
        var snapshot = TestActivity.Snapshot() with { ConvertType = ConvertType.Balance };
        var context = Context(snapshot);
        BalanceConvertFormula formula = input => (double)(input.CurrentWagerAmount / 10m);
        Assert.Equal(ConvertedAmountCalculator.Calculate(snapshot, formula, context),
            ConvertedAmountCalculator.Calculate(snapshot, formula, context));
        Assert.Equal(100m, context.CurrentWagerAmount);
        Assert.Equal(0m, context.RemainingWagerAmount);
    }

    [Fact]
    public void CAL_015_LaterActivityChangeDoesNotChangeCapturedSnapshot()
    {
        var original = TestActivity.Create();
        var snapshot = ActivitySnapshotFactory.Capture(original, TestActivity.Now);
        var changed = original with { FixedBonusAmount = 99, WagerContributionRate = 25m };
        Assert.Equal(10m, PromotionAmountCalculator.Bonus(snapshot, null));
        Assert.Equal(100m, PromotionAmountCalculator.EffectiveWager(100m, snapshot.WagerContributionRate));
        Assert.Equal(99m, PromotionAmountCalculator.Bonus(ActivitySnapshotFactory.Capture(changed, TestActivity.Now), null));
    }

    private static BalanceConvertContext Context(ActivitySnapshotDto snapshot) => new(
        10001, TestActivity.TaskId, 1, 20001, new DateOnly(2026, 9, 15),
        10m, 100m, 100m, 0m, 10m, 10m, 0m, TestActivity.Now, snapshot);
}
