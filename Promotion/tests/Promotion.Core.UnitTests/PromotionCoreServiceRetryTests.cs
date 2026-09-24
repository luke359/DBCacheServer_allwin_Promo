using Promotion.Core;
using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;
using Xunit;

namespace Promotion.Core.UnitTests;

public sealed class PromotionCoreServiceRetryTests
{
    private static readonly DateTime Now = TestActivity.Now;

    [Theory]
    [InlineData(PromotionDataErrorKind.Deadlock, 25, 100)]
    [InlineData(PromotionDataErrorKind.LockWaitTimeout, 25, 100)]
    [InlineData(PromotionDataErrorKind.Connection, 100, 300)]
    public void RetriableTransactionRunsThreeFreshAttempts(PromotionDataErrorKind kind, int firstMs, int secondMs)
    {
        var store = new ScriptedStore(kind, kind);
        var delay = new RecordingDelay();
        var service = Ready(store, delay);
        var result = service.CreateEligibility(Request());
        Assert.Equal(PromotionResultKind.Succeeded, result.Kind);
        Assert.Equal(3, store.Attempts);
        Assert.Equal(new[] { firstMs, secondMs }, delay.Delays.Select(x => (int)x.TotalMilliseconds));
        Assert.NotNull(store.Event);
    }

    [Theory]
    [InlineData(PromotionDataErrorKind.Deadlock, PromotionErrorCode.DatabaseDeadlock)]
    [InlineData(PromotionDataErrorKind.LockWaitTimeout, PromotionErrorCode.DatabaseLockWaitTimeout)]
    [InlineData(PromotionDataErrorKind.Connection, PromotionErrorCode.DatabaseConnectionFailure)]
    public void RetryExhaustionReturnsLastStableError(PromotionDataErrorKind kind, PromotionErrorCode code)
    {
        var store = new ScriptedStore(kind, kind, kind);
        var result = Ready(store, new RecordingDelay()).CreateEligibility(Request());
        Assert.Equal(code, result.ErrorCode);
        Assert.True(result.IsRetryable);
        Assert.Equal(3, store.Attempts);
    }

    [Fact]
    public void CommitUnknownWithExistingEventRecoversWithoutRepeatingAction()
    {
        var store = new ScriptedStore(PromotionDataErrorKind.CommitOutcomeUnknown);
        store.Event = new PromotionTriggerEvent("retry-event", 10001, TriggerType.Free, Now,
            new DateOnly(2026, 9, 15), null, Now);
        var result = Ready(store, new RecordingDelay()).CreateEligibility(Request());
        Assert.Equal(PromotionResultKind.Succeeded, result.Kind);
        Assert.True(result.Data!.IsReplay);
        Assert.Equal(2, store.Attempts); // first call plus read-only verification
    }

    [Fact]
    public void CommitUnknownWithoutEventRetriesWithConnectionBackoff()
    {
        var store = new ScriptedStore(PromotionDataErrorKind.CommitOutcomeUnknown);
        var delay = new RecordingDelay();
        var result = Ready(store, delay).CreateEligibility(Request());
        Assert.Equal(PromotionResultKind.Succeeded, result.Kind);
        Assert.False(result.Data!.IsReplay);
        Assert.Equal(3, store.Attempts); // failed commit, verifier, new action
        Assert.Equal(100, delay.Delays.Single().TotalMilliseconds);
    }

    [Fact]
    public void EventPrimaryKeyRaceReturnsThePersistedEvent()
    {
        var store = new ScriptedStore(PromotionDataErrorKind.DuplicateKey)
        {
            ConstraintName = "PromotionTriggerEvent.PRIMARY",
            Event = new PromotionTriggerEvent("retry-event", 10001, TriggerType.Free, Now,
                new DateOnly(2026, 9, 15), null, Now)
        };
        var result = Ready(store, new RecordingDelay()).CreateEligibility(Request());
        Assert.Equal(PromotionResultKind.Succeeded, result.Kind);
        Assert.True(result.Data!.IsReplay);
        Assert.Equal(2, store.Attempts);
    }

    [Fact]
    public void EventPrimaryKeyRaceWithDifferentFactsReturnsConflictDetails()
    {
        var store = new ScriptedStore(PromotionDataErrorKind.DuplicateKey)
        {
            ConstraintName = "PromotionTriggerEvent.PRIMARY",
            Event = new PromotionTriggerEvent("retry-event", 10002, TriggerType.Free, Now,
                new DateOnly(2026, 9, 15), null, Now)
        };
        var result = Ready(store, new RecordingDelay()).CreateEligibility(Request());
        Assert.Equal(PromotionErrorCode.EventIdConflict, result.ErrorCode);
        Assert.Equal(10002, result.ErrorDetails!.ExistingUserUID);
    }

    [Fact]
    public void DailyFirstTriggersAreConsumedOncePerBusinessDayAndReplayRemainsIdempotent()
    {
        var store = new ScriptedStore();
        var service = Ready(store, new RecordingDelay());
        var dayOne = new DateTime(2026, 9, 15, 10, 0, 0);

        var firstLogin = service.CreateEligibility(new CreateEligibilityRequest("login-1", 10001,
            TriggerType.FirstLoginOfBusinessDay, dayOne, null, Array.Empty<long>()));
        var repeatedLogin = service.CreateEligibility(new CreateEligibilityRequest("login-2", 10001,
            TriggerType.FirstLoginOfBusinessDay, dayOne.AddHours(1), null, Array.Empty<long>()));
        var replay = service.CreateEligibility(new CreateEligibilityRequest("login-2", 10001,
            TriggerType.FirstLoginOfBusinessDay, dayOne.AddHours(1), null, Array.Empty<long>()));
        var firstDeposit = service.CreateEligibility(new CreateEligibilityRequest("deposit-1", 10001,
            TriggerType.FirstDepositOfBusinessDay, dayOne.AddHours(2), 1m, Array.Empty<long>()));
        var nextDayLogin = service.CreateEligibility(new CreateEligibilityRequest("login-3", 10001,
            TriggerType.FirstLoginOfBusinessDay, dayOne.AddDays(1), null, Array.Empty<long>()));
        var delayedOldLogin = service.CreateEligibility(new CreateEligibilityRequest("login-old", 10001,
            TriggerType.FirstLoginOfBusinessDay, dayOne.AddHours(3), null, Array.Empty<long>()));

        Assert.Equal(PromotionResultKind.Succeeded, firstLogin.Kind);
        Assert.Equal(PromotionResultKind.Succeeded, repeatedLogin.Kind);
        Assert.Empty(repeatedLogin.Data!.Entries);
        Assert.True(replay.Data!.IsReplay);
        Assert.Equal(PromotionResultKind.Succeeded, firstDeposit.Kind);
        Assert.Equal(PromotionResultKind.Succeeded, nextDayLogin.Kind);
        Assert.Empty(delayedOldLogin.Data!.Entries);
        Assert.Equal(3, store.ActivityQueryCount);
        Assert.Equal(new DateOnly(2026, 9, 16), store.TaskStatus!.LastFirstLoginBusinessDay);
        Assert.Equal(new DateOnly(2026, 9, 15), store.TaskStatus.LastFirstDepositBusinessDay);
    }

    [Fact]
    public void CommitUnknownOnGameRoundStopsImmediately()
    {
        var store = new ScriptedStore(PromotionDataErrorKind.CommitOutcomeUnknown);
        var result = Ready(store, new RecordingDelay()).AccumulateWager(new AccumulateWagerRequest(
            10001, TestActivity.TaskId, 1, 0, Now, 1));
        Assert.Equal(PromotionResultKind.OutcomeUnknown, result.Kind);
        Assert.Equal(PromotionErrorCode.CommitOutcomeUnknown, result.ErrorCode);
        Assert.False(result.IsRetryable);
        Assert.Equal(1, store.Attempts);
    }

    [Fact]
    public void ReachingWagerKeepsTaskOpenUntilHostClosesIt()
    {
        var store = new ScriptedStore { TaskStatus = TestActivity.Active() };
        var service = Ready(store, new RecordingDelay());

        var accumulated = service.AccumulateWager(new AccumulateWagerRequest(
            10001, TestActivity.TaskId, 200, 0, Now, 10));

        Assert.Equal(BonusTaskOutcome.ReadyToClose, accumulated.Data!.Outcome);
        Assert.Empty(accumulated.Data.WalletInstructions);
        Assert.Null(store.History);
        Assert.Equal(BonusTaskState.InProgress, store.TaskStatus!.TaskState);
        Assert.Equal(300m, store.TaskStatus.CurrentWagerAmount);

        var closed = service.CloseWagerCompletedBonusTask(new CloseWagerCompletedBonusTaskRequest(
            10001, TestActivity.TaskId, 10, 200, 0, Now));

        Assert.Equal(PromotionResultKind.Succeeded, closed.Kind);
        Assert.Equal(CloseReason.WagerCompleted, closed.Data!.CloseReason);
        Assert.Equal(5m, closed.Data.ConvertedAmount);
        Assert.Equal(BonusTaskState.None, store.TaskStatus.TaskState);
        Assert.NotNull(store.History);
    }

    [Fact]
    public void HostCannotCloseWagerBeforeRequirementIsMet()
    {
        var store = new ScriptedStore { TaskStatus = TestActivity.Active() };
        var result = Ready(store, new RecordingDelay()).CloseWagerCompletedBonusTask(
            new CloseWagerCompletedBonusTaskRequest(10001, TestActivity.TaskId, 10, 1, 0, Now));

        Assert.Equal(PromotionResultKind.Rejected, result.Kind);
        Assert.Equal(PromotionErrorCode.WagerRequirementNotMet, result.ErrorCode);
        Assert.Null(store.History);
        Assert.Equal(BonusTaskState.InProgress, store.TaskStatus!.TaskState);
    }

    [Fact]
    public void MaintenanceIsolatesOnePlayersFailureAndReportsPartialSuccess()
    {
        var store = new ScriptedStore
        {
            ExpiredUsers = new long[] { 10001, 10002 },
            FailUserUid = 10001
        };
        var result = Ready(store, new RecordingDelay()).RunDailyMaintenance(new DailyMaintenanceRequest(
            Now.AddDays(1), 2));
        Assert.Equal(PromotionResultKind.PartialSucceeded, result.Kind);
        Assert.Equal(1, result.Data!.ExpiredEligibilityCount);
        var failure = Assert.Single(result.Data.Failures);
        Assert.Equal(10001, failure.UserUID);
        Assert.Equal(MaintenanceStage.ExpireEligibility, failure.Stage);
        Assert.Equal(PromotionErrorCode.DatabaseDeadlock, failure.ErrorCode);
    }

    [Fact]
    public void MaintenanceUnknownCommitReconstructsHistoryAndWalletInstruction()
    {
        var store = new ScriptedStore
        {
            TaskStatus = TestActivity.Active(),
            PostActionUnknownOnce = true
        };
        var result = Ready(store, new RecordingDelay()).RunDailyMaintenance(new DailyMaintenanceRequest(
            Now.AddDays(1), 2));
        Assert.Equal(PromotionResultKind.Succeeded, result.Kind);
        Assert.Equal(1, result.Data!.ClosedExpiredTaskCount);
        Assert.Equal(WalletAction.ClearBonusWallet, Assert.Single(result.Data.WalletInstructions).Action);
        Assert.Empty(result.Data.Failures);
        Assert.Equal(BonusTaskState.None, store.TaskStatus!.TaskState);
    }

    [Fact]
    public void AbandonUnknownCommitReplaysTheCommittedHistory()
    {
        var store = new ScriptedStore
        {
            TaskStatus = TestActivity.Active(),
            PostActionUnknownOnce = true
        };
        var result = Ready(store, new RecordingDelay()).AbandonBonusTask(new AbandonBonusTaskRequest(
            10001, TestActivity.TaskId, Now));
        Assert.Equal(PromotionResultKind.Succeeded, result.Kind);
        Assert.True(result.Data!.AlreadyClosed);
        Assert.Equal(CloseReason.PlayerAbandoned, result.Data.CloseReason);
        Assert.Equal(WalletAction.ClearBonusWallet, Assert.Single(result.Data.WalletInstructions).Action);
        Assert.Equal(BonusTaskState.None, store.TaskStatus!.TaskState);
    }

    private static CreateEligibilityRequest Request() => new("retry-event", 10001,
        TriggerType.Free, Now, null, Array.Empty<long>(), "same-correlation");

    private static PromotionCoreService Ready(ScriptedStore store, RecordingDelay delay)
    {
        var service = new PromotionCoreService(store, new FixedClock(), null, delay, new ZeroJitter());
        Assert.Equal(PromotionResultKind.Succeeded, service.Initialize(new InitializeRequest(TimeSpan.Zero, _ => 0)).Kind);
        return service;
    }

    private sealed class FixedClock : ILocalClock { public DateTime GetNow() => Now; }
    private sealed class ZeroJitter : IJitterSource { public int NextMilliseconds(int _) => 0; }
    private sealed class RecordingDelay : IRetryDelay
    {
        public List<TimeSpan> Delays { get; } = new();
        public void Delay(TimeSpan duration) => Delays.Add(duration);
    }

    private sealed class ScriptedStore : IPromotionDataStore
    {
        private readonly Queue<PromotionDataErrorKind> failures;
        public int Attempts { get; private set; }
        public PromotionTriggerEvent? Event { get; set; }
        public string? ConstraintName { get; set; }
        public IReadOnlyList<long> ExpiredUsers { get; set; } = Array.Empty<long>();
        public long? FailUserUid { get; set; }
        public BonusStatus? TaskStatus { get; set; }
        public BonusHistory? History { get; set; }
        public bool PostActionUnknownOnce { get; set; }
        public int ActivityQueryCount { get; set; }
        public ScriptedStore(params PromotionDataErrorKind[] failures) => this.failures = new(failures);
        public T ExecuteInTransaction<T>(Func<IPromotionDataTransaction, T> action)
        {
            Attempts++;
            if (failures.TryDequeue(out var kind)) throw new PromotionDataException(kind, "injected",
                constraintName: ConstraintName);
            var result = action(new Transaction(this));
            if (PostActionUnknownOnce)
            {
                PostActionUnknownOnce = false;
                throw new PromotionDataException(PromotionDataErrorKind.CommitOutcomeUnknown, "injected after commit");
            }
            return result;
        }
        public IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(TriggerType type, IReadOnlyCollection<long> allowed) => Array.Empty<PromotionActivity>();
        public PromotionActivity? GetActivity(long uid) => null;
        public IReadOnlyList<EligibilityEntry> GetAvailableEligibilityEntries(long uid, DateOnly day) => Array.Empty<EligibilityEntry>();
        public BonusStatus? GetBonusStatus(long uid) => TaskStatus?.UserUID == uid ? TaskStatus : null;
        public IReadOnlyList<BonusHistory> GetBonusHistory(long uid, DateTime from, DateTime to, int offset, int limit) => Array.Empty<BonusHistory>();
        public IReadOnlyList<long> GetUsersWithExpiredEligibility(DateOnly day, long cursor, int batch) =>
            ExpiredUsers.Where(x => x > cursor).Take(batch).ToArray();
        public IReadOnlyList<long> GetUsersWithExpiredTasks(DateOnly day, long cursor, int batch) =>
            TaskStatus is { TaskState: BonusTaskState.InProgress } status &&
            status.BusinessDay < day && status.UserUID > cursor
                ? new[] { status.UserUID } : Array.Empty<long>();
        public int DeleteExpiredBonusHistory(DateTime cutoff, int batch) => 0;

        private sealed class Transaction : IPromotionDataTransaction
        {
            private readonly ScriptedStore store;
            public Transaction(ScriptedStore store) => this.store = store;
            public BonusStatus GetOrCreateBonusStatusForUpdate(long uid, DateTime time) =>
                store.TaskStatus?.UserUID == uid ? store.TaskStatus :
                new BonusStatus(uid, BonusTaskState.None, null, null, null, null, null, null, null, 0, 0, 0, null, time, time);
            public PromotionActivity? GetActivity(long uid) => null;
            public IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(TriggerType type, IReadOnlyCollection<long> allowed)
            {
                store.ActivityQueryCount++;
                return Array.Empty<PromotionActivity>();
            }
            public PromotionTriggerEvent? GetTriggerEvent(string id) =>
                store.Event?.EventId == id ? store.Event : null;
            public IReadOnlyList<EligibilityEntry> GetEligibilityEntriesByEventId(string id) => Array.Empty<EligibilityEntry>();
            public void InsertTriggerEvent(PromotionTriggerEvent value) => store.Event = value;
            public IReadOnlyList<EligibilityEntry> InsertEligibilityEntries(IReadOnlyList<NewEligibilityEntry> values) => Array.Empty<EligibilityEntry>();
            public EligibilityEntry? GetEligibilityEntryForUpdate(long id) => null;
            public int CountClaimedEligibility(long uid, long activity, DateOnly day) => 0;
            public void MarkEligibilityClaimed(long id, string taskId, DateTime time) => throw new NotSupportedException();
            public int ExcludeEligibilityEntries(long uid, DateOnly day, long id, string taskId, bool nonStackable, string? group, DateTime time) => 0;
            public int ExpireAvailableEligibilityEntries(long uid, DateOnly day, DateTime time)
            {
                if (uid == store.FailUserUid)
                    throw new PromotionDataException(PromotionDataErrorKind.Deadlock, "injected");
                return 1;
            }
            public void SaveBonusStatus(BonusStatus value) => store.TaskStatus = value;
            public BonusHistory? GetBonusHistoryByTaskId(string taskId) =>
                store.History?.BonusTaskId == taskId ? store.History : null;
            public BonusHistory InsertBonusHistory(NewBonusHistory value)
            {
                store.History = new BonusHistory(1, value.BonusTaskId, value.UserUID,
                    value.EligibilityEntryId, value.ActivityUID, value.BusinessDay,
                    value.ActivitySnapshotJson, value.BonusAmount, value.RequiredWagerAmount,
                    value.CurrentWagerAmount, value.ConvertedAmount, value.CloseReason,
                    value.ClaimedAt, value.ClosedAt, value.CreatedAt);
                return store.History;
            }
        }
    }
}
