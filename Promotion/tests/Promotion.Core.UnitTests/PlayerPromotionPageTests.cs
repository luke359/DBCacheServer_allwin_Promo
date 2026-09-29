using Promotion.Core;
using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;
using Xunit;

namespace Promotion.Core.UnitTests;

public sealed class PlayerPromotionPageTests
{
    [Fact]
    public void NoEligibilityStillReturnsVisibleActiveActivityWithoutClaimButton()
    {
        var result = Ready(new PageStore()).GetPlayerPromotionPage(Request(20001));

        var item = Assert.Single(result.Data!.Activities);
        Assert.False(item.CanClaim);
        Assert.False(item.ClaimButtonEnabled);
        Assert.Null(item.EligibilityEntryId);
    }

    [Fact]
    public void AvailableEligibilityWithoutTaskEnablesClaim()
    {
        var store = new PageStore { Entries = new[] { Entry(1, 20001) } };
        var result = Ready(store).GetPlayerPromotionPage(Request(20001));

        var item = Assert.Single(result.Data!.Activities);
        Assert.True(item.CanClaim);
        Assert.True(item.ClaimButtonEnabled);
        Assert.Equal(1, item.EligibilityEntryId);
    }

    [Fact]
    public void ActiveTaskOnlyMarksItsActivityAndDisablesOtherAvailableClaims()
    {
        var store = new PageStore
        {
            Entries = new[] { Entry(1, 20001) },
            Status = ActiveStatus(20002)
        };
        var result = Ready(store).GetPlayerPromotionPage(Request(20001, 20002));

        Assert.True(result.Data!.HasAnyActiveBonusTask);
        var claimable = Assert.Single(result.Data.Activities, item => item.ActivityUID == 20001);
        Assert.True(claimable.CanClaim);
        Assert.False(claimable.ClaimButtonEnabled);
        var active = Assert.Single(result.Data.Activities, item => item.ActivityUID == 20002);
        Assert.True(active.HasActiveTask);
        Assert.False(active.ClaimButtonEnabled);
        Assert.Equal(100m, active.ActiveTaskProgress!.CurrentWagerAmount);
        Assert.Equal(300m, active.ActiveTaskProgress.RequiredWagerAmount);
        Assert.Equal(200m, active.ActiveTaskProgress.RemainingWagerAmount);
    }

    [Fact]
    public void DuplicateScopeReturnsOneItemAndUsesLatestEligibility()
    {
        var store = new PageStore { Entries = new[] { Entry(1, 20001), Entry(2, 20001) } };
        var result = Ready(store).GetPlayerPromotionPage(Request(20001, 20001));

        var item = Assert.Single(result.Data!.Activities);
        Assert.Equal(2, item.EligibilityEntryId);
        Assert.Equal(new[] { 20001L }, store.RequestedActivityUids);
    }

    [Fact]
    public void EmptyScopeDoesNotReadActivitiesOrEligibility()
    {
        var store = new PageStore { Status = ActiveStatus(20001) };
        var result = Ready(store).GetPlayerPromotionPage(Request());

        Assert.Empty(result.Data!.Activities);
        Assert.Equal(0, store.ActivityReadCount);
        Assert.Equal(0, store.EligibilityReadCount);
    }

    [Fact]
    public void TodayCompletedHistoryIsReadOnlyWhenRequestedAndFilteredByBusinessDay()
    {
        var store = new PageStore { History = new[] { History(new DateOnly(2026, 9, 15)) } };
        var service = Ready(store);

        var withoutHistory = service.GetPlayerPromotionPage(Request(20001));
        Assert.Null(withoutHistory.Data!.TodayCompletedItems);
        Assert.Equal(0, store.HistoryReadCount);

        var withHistory = service.GetPlayerPromotionPage(Request(20001, includeTodayCompleted: true));
        Assert.Single(withHistory.Data!.TodayCompletedItems!);
        Assert.Equal(new DateOnly(2026, 9, 15), store.RequestedHistoryBusinessDay);
    }

    [Fact]
    public void ArchivedAndUnauthorizedActivitiesAreNotReturned()
    {
        var store = new PageStore
        {
            Activities = new[] { Activity(20001), Activity(20002) with { ActivityStatus = ActivityStatus.Archived }, Activity(20003) },
            Entries = new[] { Entry(1, 20003) }
        };
        var result = Ready(store).GetPlayerPromotionPage(Request(20001, 20002));

        Assert.Equal(new[] { 20001L }, result.Data!.Activities.Select(item => item.ActivityUID));
    }

    private static PromotionCoreService Ready(PageStore store)
    {
        var service = new PromotionCoreService(store, new FixedClock());
        Assert.Equal(PromotionResultKind.Succeeded, service.Initialize(new InitializeRequest(TimeSpan.Zero, _ => 0)).Kind);
        return service;
    }

    private static GetPlayerPromotionPageRequest Request(params long[] activityUids) =>
        new(10001, TestActivity.Now, activityUids, false, "page-test");
    private static GetPlayerPromotionPageRequest Request(long activityUid, bool includeTodayCompleted) =>
        new(10001, TestActivity.Now, new[] { activityUid }, includeTodayCompleted, "page-test");
    private static PromotionActivity Activity(long activityUid) => TestActivity.Create() with { ActivityUID = activityUid };
    private static EligibilityEntry Entry(long id, long activityUid) => TestActivity.Entry() with
    {
        EligibilityEntryId = id, ActivityUID = activityUid
    };
    private static BonusStatus ActiveStatus(long activityUid) => TestActivity.Active() with { ActivityUID = activityUid };
    private static BonusHistory History(DateOnly businessDay) => new(1, TestActivity.TaskId, 10001, 1, 20001,
        businessDay, ActivitySnapshotSerializer.Serialize(TestActivity.Snapshot()), 10m, 300m, 300m, 5m,
        CloseReason.WagerCompleted, TestActivity.Now, TestActivity.Now, TestActivity.Now);

    private sealed class FixedClock : ILocalClock { public DateTime GetNow() => TestActivity.Now; }

    private sealed class PageStore : IPromotionDataStore
    {
        public IReadOnlyList<PromotionActivity> Activities { get; set; } = new[] { Activity(20001), Activity(20002) };
        public IReadOnlyList<EligibilityEntry> Entries { get; set; } = Array.Empty<EligibilityEntry>();
        public IReadOnlyList<BonusHistory> History { get; set; } = Array.Empty<BonusHistory>();
        public BonusStatus? Status { get; set; }
        public int ActivityReadCount { get; private set; }
        public int EligibilityReadCount { get; private set; }
        public int HistoryReadCount { get; private set; }
        public IReadOnlyList<long> RequestedActivityUids { get; private set; } = Array.Empty<long>();
        public DateOnly? RequestedHistoryBusinessDay { get; private set; }

        public T ExecuteInTransaction<T>(Func<IPromotionDataTransaction, T> action) => throw new NotSupportedException();
        public IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(TriggerType triggerType,
            IReadOnlyCollection<long> allowedActivityUids) => throw new NotSupportedException();
        public IReadOnlyList<PromotionActivity> GetActivitiesByIds(IReadOnlyCollection<long> activityUids)
        {
            ActivityReadCount++;
            RequestedActivityUids = activityUids.ToArray();
            return Activities.Where(activity => activityUids.Contains(activity.ActivityUID)).ToArray();
        }
        public PromotionActivity? GetActivity(long activityUid) => throw new NotSupportedException();
        public IReadOnlyList<EligibilityEntry> GetAvailableEligibilityEntries(long userUid, DateOnly businessDay) =>
            throw new NotSupportedException();
        public IReadOnlyList<EligibilityEntry> GetAvailableEligibilityEntries(long userUid, DateOnly businessDay,
            IReadOnlyCollection<long> activityUids)
        {
            EligibilityReadCount++;
            return Entries.Where(entry => entry.UserUID == userUid && entry.BusinessDay == businessDay &&
                activityUids.Contains(entry.ActivityUID)).ToArray();
        }
        public BonusStatus? GetBonusStatus(long userUid) => Status?.UserUID == userUid ? Status : null;
        public IReadOnlyList<BonusHistory> GetBonusHistory(long userUid, DateTime fromInclusive, DateTime toExclusive,
            int offset, int limit) => throw new NotSupportedException();
        public IReadOnlyList<BonusHistory> GetBonusHistoryByBusinessDay(long userUid, DateOnly businessDay)
        {
            HistoryReadCount++;
            RequestedHistoryBusinessDay = businessDay;
            return History.Where(history => history.UserUID == userUid && history.BusinessDay == businessDay).ToArray();
        }
        public IReadOnlyList<long> GetUsersWithExpiredEligibility(DateOnly currentBusinessDay, long afterUserUid,
            int batchSize) => throw new NotSupportedException();
        public IReadOnlyList<long> GetUsersWithExpiredTasks(DateOnly currentBusinessDay, long afterUserUid,
            int batchSize) => throw new NotSupportedException();
        public int DeleteExpiredBonusHistory(DateTime cutoffExclusive, int batchSize) => throw new NotSupportedException();
    }
}
