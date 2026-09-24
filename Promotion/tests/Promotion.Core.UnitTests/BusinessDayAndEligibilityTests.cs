using Promotion.Core.Contracts;
using Promotion.Core.Domain;
using Xunit;

namespace Promotion.Core.UnitTests;

public sealed class BusinessDayAndEligibilityTests
{
    [Fact]
    public void BD_001_BeforeCutoverBelongsToPreviousBusinessDay() =>
        Assert.Equal(new DateOnly(2026, 9, 14), BusinessDayCalculator.Calculate(
            new DateTime(2026, 9, 15, 7, 59, 59).AddTicks(9_999_990), TimeSpan.FromHours(8)));

    [Fact]
    public void BD_002_AtCutoverBelongsToCurrentBusinessDay() =>
        Assert.Equal(new DateOnly(2026, 9, 15), BusinessDayCalculator.Calculate(
            new DateTime(2026, 9, 15, 8, 0, 0), TimeSpan.FromHours(8)));

    [Fact]
    public void BD_003_CutoverBounds()
    {
        Assert.True(BusinessDayCalculator.IsValidCutover(TimeSpan.Zero));
        Assert.True(BusinessDayCalculator.IsValidCutover(new TimeSpan(23, 59, 59)));
        Assert.False(BusinessDayCalculator.IsValidCutover(TimeSpan.FromDays(1)));
        Assert.False(BusinessDayCalculator.IsValidCutover(TimeSpan.FromTicks(1)));
    }

    [Fact]
    public void BD_004_005_StartInclusiveEndExclusive()
    {
        var activity = TestActivity.Create();
        Assert.True(Eligible(activity, new DateOnly(2026, 9, 1)));
        Assert.False(Eligible(activity, new DateOnly(2026, 10, 1)));
    }

    [Fact]
    public void BD_006_NullDateBoundsDoNotRestrict()
    {
        var activity = TestActivity.Create() with { StartDate = null, EndDate = null };
        Assert.True(Eligible(activity, new DateOnly(2026, 1, 1)));
    }

    [Fact]
    public void BD_007_008_WeekdayMaskMondayToSundayAndDisabled()
    {
        var monday = TestActivity.Create() with { WeekdayMask = "1000000" };
        Assert.True(Eligible(monday, new DateOnly(2026, 9, 14)));
        Assert.False(Eligible(monday, new DateOnly(2026, 9, 15)));
        Assert.False(Eligible(monday with { WeekdayMask = "0000000" }, new DateOnly(2026, 9, 14)));
    }

    [Fact]
    public void ArchivedActivityDoesNotCreateNewEligibility()
    {
        var archived = TestActivity.Create() with { ActivityStatus = ActivityStatus.Archived };
        Assert.False(Eligible(archived, new DateOnly(2026, 9, 15)));
    }

    [Theory]
    [InlineData(2026, 9, 14, 0)]
    [InlineData(2026, 9, 15, 1)]
    [InlineData(2026, 9, 16, 2)]
    [InlineData(2026, 9, 17, 3)]
    [InlineData(2026, 9, 18, 4)]
    [InlineData(2026, 9, 19, 5)]
    [InlineData(2026, 9, 20, 6)]
    public void BD_007_EachWeekdayMapsToItsMaskPosition(int year, int month, int day, int position)
    {
        var mask = new string('0', position) + "1" + new string('0', 6 - position);
        var activity = TestActivity.Create() with { WeekdayMask = mask };
        Assert.True(Eligible(activity, new DateOnly(year, month, day)));
    }

    [Fact]
    public void BD_009_010_011_012_TriggerDepositScopeAndClaimLimit()
    {
        var activity = TestActivity.Create();
        var day = new DateOnly(2026, 9, 15);
        Assert.False(ActivityEligibilityEvaluator.IsEligible(activity, TriggerType.Free, day, null, new[] { 20001L }, 0));
        Assert.False(ActivityEligibilityEvaluator.IsEligible(activity, TriggerType.Deposit, day, 9.9999m, new[] { 20001L }, 0));
        Assert.True(ActivityEligibilityEvaluator.IsEligible(activity, TriggerType.Deposit, day, 10m, new[] { 20001L }, 0));
        Assert.False(ActivityEligibilityEvaluator.IsEligible(activity, TriggerType.Deposit, day, 10m, Array.Empty<long>(), 0));
        Assert.False(ActivityEligibilityEvaluator.IsEligible(activity, TriggerType.Deposit, day, 10m, new[] { 20001L }, 1));
    }

    private static bool Eligible(PromotionActivity activity, DateOnly day) =>
        ActivityEligibilityEvaluator.IsEligible(activity, TriggerType.Deposit, day, 10m, new[] { 20001L }, 0);
}
