using System.Text;
using DBCacheServer;
using MySql.Data.MySqlClient;
using Promotion.Core.Contracts;
using Promotion.Core.Domain;
using Promotion.Core;
using Promotion.Data.MySql;
using Xunit;

namespace Promotion.Data.MySql.IntegrationTests;

[Collection("MysqlAcess V2")]
public sealed class PromotionAdapterTests
{
    [Fact]
    public async Task P6_ConcurrentDifferentDailyFirstEventsCreateOneEligibility()
    {
        var connectionString = TestConnectionString();
        var now = new DateTime(2026, 9, 17, 12, 0, 0);
        var day = DateOnly.FromDateTime(now);
        var userUid = 930_000_000_000L + Random.Shared.Next(1, 100_000_000);
        var eventIds = new[]
        {
            "daily-first-a-" + Guid.NewGuid().ToString("N"),
            "daily-first-b-" + Guid.NewGuid().ToString("N")
        };
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            using (var command = new MySqlCommand(@"INSERT INTO PromotionActivity
                (ActivityUID, ActivityInfo, BonusType, FixedBonusAmount, MaxBonusAmount, DepositPercentage,
                 MinimumDepositAmount, WagerMultiplier, WagerCalculationType, MaxBetAmount, GameServerList,
                 WagerContributionRate, ConvertType, FixedConvertedAmount, MaxBalanceConvertedAmount,
                 DailyClaimLimit, StartDate, EndDate, WeekdayMask, TriggerType, IsNonStackable, ExclusiveGroup,
                 CreatedAt, UpdatedAt)
                VALUES (@id, 'daily-first', 1, 10, 100, 100, NULL, 2, 1, NULL, NULL,
                        100.0000, 1, 10, NULL, 1, NULL, NULL, '1111111', 2, 0, NULL, @now, @now)", connection))
            {
                command.Parameters.AddWithValue("@id", userUid);
                command.Parameters.AddWithValue("@now", now);
                command.ExecuteNonQuery();
            }
            var services = Enumerable.Range(0, 2).Select(_ =>
            {
                var service = new PromotionCoreService(new PromotionMySqlDataStore(
                    new PromotionV2Gateway(InitializeMysql(connectionString))), new FixedClock(now));
                service.Initialize(new InitializeRequest(TimeSpan.Zero, _ => 0));
                return service;
            }).ToArray();
            using var barrier = new Barrier(2);
            var results = await Task.WhenAll(services.Select((service, index) => Task.Run(() =>
            {
                Assert.True(barrier.SignalAndWait(TimeSpan.FromSeconds(10)));
                return service.CreateEligibility(new CreateEligibilityRequest(eventIds[index], userUid,
                    TriggerType.FirstLoginOfBusinessDay, now.AddSeconds(index), null, new[] { userUid }));
            })));

            Assert.All(results, result =>
            {
                Assert.Equal(PromotionResultKind.Succeeded, result.Kind);
                Assert.False(result.Data!.IsReplay);
            });
            Assert.Equal(1, results.Sum(result => result.Data!.Entries.Count));
            Assert.Equal(2L, Scalar(connection,
                "SELECT COUNT(*) FROM PromotionTriggerEvent WHERE UserUID = @user", userUid));
            Assert.Equal(1L, Scalar(connection,
                "SELECT COUNT(*) FROM EligibilityEntry WHERE UserUID = @user", userUid));
            var store = new PromotionMySqlDataStore(new PromotionV2Gateway(InitializeMysql(connectionString)));
            Assert.Equal(day, store.GetBonusStatus(userUid)!.LastFirstLoginBusinessDay);
        }
        finally
        {
            foreach (var table in new[] { "PromotionBonusHistory", "PromotionBonusStatus", "EligibilityEntry", "PromotionTriggerEvent" })
            {
                using var cleanup = new MySqlCommand($"DELETE FROM {table} WHERE UserUID = @user", connection);
                cleanup.Parameters.AddWithValue("@user", userUid);
                cleanup.ExecuteNonQuery();
            }
            using var activityCleanup = new MySqlCommand("DELETE FROM PromotionActivity WHERE ActivityUID = @id", connection);
            activityCleanup.Parameters.AddWithValue("@id", userUid);
            activityCleanup.ExecuteNonQuery();
        }
    }

    [Fact]
    public async Task P6_ConcurrentSameEventAndClaimCreateOneTask()
    {
        var connectionString = TestConnectionString();
        var now = new DateTime(2026, 9, 17, 12, 0, 0);
        var userUid = 920_000_000_000L + Random.Shared.Next(1, 100_000_000);
        var eventId = "p6-concurrent-" + Guid.NewGuid().ToString("N");
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            using (var command = new MySqlCommand(@"INSERT INTO PromotionActivity
                (ActivityUID, ActivityInfo, BonusType, FixedBonusAmount, MaxBonusAmount, DepositPercentage,
                 MinimumDepositAmount, WagerMultiplier, WagerCalculationType, MaxBetAmount, GameServerList,
                 WagerContributionRate, ConvertType, FixedConvertedAmount, MaxBalanceConvertedAmount,
                 DailyClaimLimit, StartDate, EndDate, WeekdayMask, TriggerType, IsNonStackable, ExclusiveGroup,
                 CreatedAt, UpdatedAt)
                VALUES (@id, 'concurrent', 1, 10, 100, 100, NULL, 2, 1, NULL, NULL,
                        100.0000, 1, 10, NULL, 1, NULL, NULL, '1111111', 5, 0, NULL, @now, @now)", connection))
            {
                command.Parameters.AddWithValue("@id", userUid);
                command.Parameters.AddWithValue("@now", now);
                command.ExecuteNonQuery();
            }
            var mysql = InitializeMysql(connectionString);
            var services = Enumerable.Range(0, 2).Select(_ =>
            {
                var service = new PromotionCoreService(new PromotionMySqlDataStore(new PromotionV2Gateway(mysql)),
                    new FixedClock(now));
                service.Initialize(new InitializeRequest(TimeSpan.Zero, _ => 0));
                return service;
            }).ToArray();
            using var eventBarrier = new Barrier(2);
            var eventResults = await Task.WhenAll(services.Select(service => Task.Run(() =>
            {
                Assert.True(eventBarrier.SignalAndWait(TimeSpan.FromSeconds(10)));
                return service.CreateEligibility(new CreateEligibilityRequest(eventId, userUid,
                    TriggerType.Free, now, null, new[] { userUid }));
            })));
            Assert.All(eventResults, result => Assert.Equal(PromotionResultKind.Succeeded, result.Kind));
            var entryId = Assert.Single(eventResults[0].Data!.Entries).EligibilityEntryId;
            Assert.Equal(entryId, Assert.Single(eventResults[1].Data!.Entries).EligibilityEntryId);
            Assert.Single(eventResults, x => !x.Data!.IsReplay);
            using var claimBarrier = new Barrier(2);
            var claims = await Task.WhenAll(services.Select(service => Task.Run(() =>
            {
                Assert.True(claimBarrier.SignalAndWait(TimeSpan.FromSeconds(10)));
                return service.ClaimPromotion(new ClaimPromotionRequest(userUid, entryId));
            })));
            Assert.All(claims, result => Assert.Equal(PromotionResultKind.Succeeded, result.Kind));
            Assert.Equal(claims[0].Data!.BonusTaskId, claims[1].Data!.BonusTaskId);
            Assert.Single(claims, x => !x.Data!.IsReplay);
        }
        finally
        {
            foreach (var table in new[] { "PromotionBonusHistory", "PromotionBonusStatus", "EligibilityEntry", "PromotionTriggerEvent" })
            {
                using var cleanup = new MySqlCommand($"DELETE FROM {table} WHERE UserUID = @user", connection);
                cleanup.Parameters.AddWithValue("@user", userUid);
                cleanup.ExecuteNonQuery();
            }
            using var activityCleanup = new MySqlCommand("DELETE FROM PromotionActivity WHERE ActivityUID = @id", connection);
            activityCleanup.Parameters.AddWithValue("@id", userUid);
            activityCleanup.ExecuteNonQuery();
        }
    }

    [Fact]
    public void P6_CoreWorkflow_ClaimCloseReplayAndClosingClassification()
    {
        var connectionString = TestConnectionString();
        var now = new DateTime(2026, 9, 17, 12, 0, 0);
        var userUid = 910_000_000_000L + Random.Shared.Next(1, 100_000_000);
        var activityUid = userUid;
        var eventId = "p6-" + Guid.NewGuid().ToString("N");
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            using (var command = new MySqlCommand(@"INSERT INTO PromotionActivity
                (ActivityUID, ActivityInfo, BonusType, FixedBonusAmount, MaxBonusAmount, DepositPercentage,
                 MinimumDepositAmount, WagerMultiplier, WagerCalculationType, MaxBetAmount, GameServerList,
                 WagerContributionRate, ConvertType, FixedConvertedAmount, MaxBalanceConvertedAmount,
                 DailyClaimLimit, StartDate, EndDate, WeekdayMask, TriggerType, IsNonStackable, ExclusiveGroup,
                 CreatedAt, UpdatedAt)
                VALUES (@id, 'P6', 1, 10, 100, 100, NULL, 2, 1, NULL, 'g1',
                        100.0000, 1, 10, NULL, 1, NULL, NULL, '1111111', 5, 0, NULL, @now, @now)", connection))
            {
                command.Parameters.AddWithValue("@id", activityUid);
                command.Parameters.AddWithValue("@now", now);
                command.ExecuteNonQuery();
            }
            var store = new PromotionMySqlDataStore(new PromotionV2Gateway(InitializeMysql(connectionString)));
            var clock = new FixedClock(now);
            var service = new PromotionCoreService(store, clock);
            Assert.Equal(PromotionResultKind.Succeeded, service.Initialize(new InitializeRequest(TimeSpan.Zero,
                _ => throw new InvalidOperationException("Injected formula failure."))).Kind);
            var created = service.CreateEligibility(new CreateEligibilityRequest(eventId, userUid,
                TriggerType.Free, now.AddTicks(7), null, new[] { activityUid }));
            Assert.Equal(PromotionResultKind.Succeeded, created.Kind);
            var entry = Assert.Single(created.Data!.Entries);
            var exposedEntries = Assert.IsAssignableFrom<IList<EligibilityEntryDto>>(created.Data.Entries);
            Assert.Throws<NotSupportedException>(() => exposedEntries[0] = entry);
            Assert.True(service.CreateEligibility(new CreateEligibilityRequest(eventId, userUid,
                TriggerType.Free, now.AddTicks(9), null, new[] { activityUid })).Data!.IsReplay);
            var eventConflict = service.CreateEligibility(new CreateEligibilityRequest(eventId, userUid,
                TriggerType.Free, now.AddSeconds(1), null, new[] { activityUid }));
            Assert.Equal(PromotionErrorCode.EventIdConflict, eventConflict.ErrorCode);
            Assert.Equal(eventId, eventConflict.ErrorDetails!.ExistingEventId);
            Assert.Equal(PromotionErrorCode.EligibilityPlayerMismatch,
                service.ClaimPromotion(new ClaimPromotionRequest(userUid + 1, entry.EligibilityEntryId)).ErrorCode);
            Assert.Single(service.GetAvailablePromotions(new GetAvailablePromotionsRequest(userUid, now)).Data!.Items);
            Assert.Equal("g1", service.GetGameServerList(new GetGameServerListRequest(activityUid)).Data!.GameServerList);
            var claimed = service.ClaimPromotion(new ClaimPromotionRequest(userUid, entry.EligibilityEntryId));
            Assert.Equal(PromotionResultKind.Succeeded, claimed.Kind);
            var taskId = claimed.Data!.BonusTaskId;
            var competingUser = userUid + 2;
            var competitorEvent = eventId + "-competitor";
            var competitor = service.CreateEligibility(new CreateEligibilityRequest(competitorEvent,
                competingUser, TriggerType.Free, now, null, new[] { activityUid }));
            var replacementId = Guid.NewGuid().ToString("D");
            var collision = Assert.Throws<Promotion.Core.Data.PromotionDataException>(() =>
                store.ExecuteInTransaction(tx =>
                {
                    tx.GetOrCreateBonusStatusForUpdate(competingUser, now);
                    tx.MarkEligibilityClaimed(Assert.Single(competitor.Data!.Entries).EligibilityEntryId,
                        taskId, now);
                    return 0;
                }));
            Assert.Equal("EligibilityEntry.UK_EligibilityEntry_ClaimedBonusTaskId", collision.ConstraintName);
            var guidProvider = new SequenceGuidProvider(Guid.Parse(taskId), Guid.Parse(replacementId));
            var competingService = new PromotionCoreService(store, clock, guidProvider);
            competingService.Initialize(new InitializeRequest(TimeSpan.Zero, _ => 0, "competing-init"));
            var competingClaim = competingService.ClaimPromotion(new ClaimPromotionRequest(competingUser,
                Assert.Single(competitor.Data!.Entries).EligibilityEntryId, "competing-claim"));
            Assert.True(competingClaim.Kind == PromotionResultKind.Succeeded,
                $"{competingClaim.ErrorCode}; Guid calls: {guidProvider.Calls}");
            Assert.Equal(replacementId, competingClaim.Data!.BonusTaskId);
            Assert.Equal(2, guidProvider.Calls);
            Assert.Equal(taskId, service.ClaimPromotion(new ClaimPromotionRequest(userUid, entry.EligibilityEntryId)).Data!.BonusTaskId);
            Assert.True(service.GetBonusTaskStatus(new GetBonusTaskStatusRequest(userUid)).Data!.HasActiveTask);
            using (var setClosing = new MySqlCommand("UPDATE PromotionBonusStatus SET TaskState = 2 WHERE UserUID = @user", connection))
            {
                setClosing.Parameters.AddWithValue("@user", userUid);
                setClosing.ExecuteNonQuery();
            }
            Assert.Equal(PromotionErrorCode.InvalidBonusTaskState,
                service.GetBonusTaskStatus(new GetBonusTaskStatusRequest(userUid)).ErrorCode);
            using (var restore = new MySqlCommand("UPDATE PromotionBonusStatus SET TaskState = 1 WHERE UserUID = @user", connection))
            {
                restore.Parameters.AddWithValue("@user", userUid);
                restore.ExecuteNonQuery();
            }
            var closed = service.AbandonBonusTask(new AbandonBonusTaskRequest(userUid, taskId, now));
            Assert.Equal(PromotionResultKind.Succeeded, closed.Kind);
            Assert.Single(closed.Data!.WalletInstructions);
            Assert.True(service.AbandonBonusTask(new AbandonBonusTaskRequest(userUid, taskId, now)).Data!.AlreadyClosed);
            Assert.Equal(PromotionErrorCode.BonusTaskNotFound,
                service.AbandonBonusTask(new AbandonBonusTaskRequest(userUid + 1, taskId, now)).ErrorCode);
            Assert.False(service.GetBonusTaskStatus(new GetBonusTaskStatusRequest(userUid)).Data!.HasActiveTask);
            clock.Now = now.AddDays(1);
            var next = service.CreateEligibility(new CreateEligibilityRequest(eventId + "-next", userUid,
                TriggerType.Free, clock.Now, null, new[] { activityUid }));
            Assert.Equal(PromotionResultKind.Succeeded, next.Kind);
            var newTask = service.ClaimPromotion(new ClaimPromotionRequest(userUid,
                Assert.Single(next.Data!.Entries).EligibilityEntryId));
            Assert.Equal(PromotionResultKind.Succeeded, newTask.Kind);
            var newTaskId = newTask.Data!.BonusTaskId;
            Assert.NotEqual(taskId, newTaskId);
            Assert.Equal(BonusTaskOutcome.AlreadyClosed, service.AccumulateWager(new AccumulateWagerRequest(
                userUid, taskId, 1, 0, clock.Now, 1)).Data!.Outcome);
            Assert.True(service.AbandonBonusTask(new AbandonBonusTaskRequest(userUid, taskId, clock.Now)).Data!.AlreadyClosed);
            Assert.Equal(newTaskId, service.GetBonusTaskStatus(new GetBonusTaskStatusRequest(userUid)).Data!.Task!.BonusTaskId);
            var historyPage = service.GetBonusHistory(new GetBonusHistoryRequest(userUid, now.AddDays(-1),
                now.AddSeconds(1), 0, 10));
            Assert.True(historyPage.Kind == PromotionResultKind.Succeeded, historyPage.ErrorCode.ToString());
            Assert.Single(historyPage.Data!.Items);
            var wager = service.AccumulateWager(new AccumulateWagerRequest(userUid, newTaskId,
                5, 0, clock.Now, 10));
            Assert.Equal(BonusTaskOutcome.InProgress, wager.Data!.Outcome);
            Assert.Equal(5m, wager.Data.CurrentWagerAmount);
            Assert.Equal(PromotionErrorCode.WagerRequirementNotMet,
                service.CloseWagerCompletedBonusTask(new CloseWagerCompletedBonusTaskRequest(
                    userUid, newTaskId, 10, 5, 0, clock.Now)).ErrorCode);
            Assert.Equal(PromotionErrorCode.BonusTaskMismatch,
                service.AccumulateWager(new AccumulateWagerRequest(userUid,
                    Guid.NewGuid().ToString("D"), 1, 0, clock.Now, 10)).ErrorCode);
            Assert.Equal(PromotionErrorCode.BonusWalletNotDepleted,
                service.CloseDepletedBonusTask(new CloseDepletedBonusTaskRequest(userUid,
                    newTaskId, 1, clock.Now)).ErrorCode);
            var depleted = service.CloseDepletedBonusTask(new CloseDepletedBonusTaskRequest(
                userUid, newTaskId, 0, clock.Now));
            Assert.Equal(PromotionResultKind.Succeeded, depleted.Kind);
            Assert.Equal(CloseReason.BonusDepleted, depleted.Data!.CloseReason);
            Assert.Empty(depleted.Data.WalletInstructions);
            Assert.True(service.CloseDepletedBonusTask(new CloseDepletedBonusTaskRequest(
                userUid, newTaskId, 0, clock.Now)).Data!.AlreadyClosed);

            clock.Now = now.AddDays(2);
            var third = service.CreateEligibility(new CreateEligibilityRequest(eventId + "-third", userUid,
                TriggerType.Free, clock.Now, null, new[] { activityUid }));
            var thirdClaim = service.ClaimPromotion(new ClaimPromotionRequest(userUid,
                Assert.Single(third.Data!.Entries).EligibilityEntryId));
            Assert.Equal(PromotionResultKind.Succeeded, thirdClaim.Kind);
            var maintenance = service.RunDailyMaintenance(new DailyMaintenanceRequest(now.AddDays(3), 20));
            Assert.Equal(PromotionResultKind.Succeeded, maintenance.Kind);
            Assert.Equal(2, maintenance.Data!.ClosedExpiredTaskCount);
            Assert.All(maintenance.Data.WalletInstructions,
                instruction => Assert.Equal(WalletAction.ClearBonusWallet, instruction.Action));
            Assert.False(service.GetBonusTaskStatus(new GetBonusTaskStatusRequest(userUid)).Data!.HasActiveTask);
            Assert.Equal(0, service.RunDailyMaintenance(new DailyMaintenanceRequest(now.AddDays(3), 20))
                .Data!.ClosedExpiredTaskCount);

            clock.Now = now.AddDays(3);
            var fourth = service.CreateEligibility(new CreateEligibilityRequest(eventId + "-fourth", userUid,
                TriggerType.Free, clock.Now, null, new[] { activityUid }));
            var fourthClaim = service.ClaimPromotion(new ClaimPromotionRequest(userUid,
                Assert.Single(fourth.Data!.Entries).EligibilityEntryId));
            Assert.Equal(PromotionResultKind.Succeeded, fourthClaim.Kind);
            var completed = service.AccumulateWager(new AccumulateWagerRequest(userUid,
                fourthClaim.Data!.BonusTaskId, 20, 0, clock.Now, 10));
            Assert.Equal(BonusTaskOutcome.ReadyToClose, completed.Data!.Outcome);
            Assert.Null(completed.Data.CloseReason);
            Assert.Empty(completed.Data.WalletInstructions);
            Assert.True(service.GetBonusTaskStatus(new GetBonusTaskStatusRequest(userUid)).Data!.HasActiveTask);
            var completedClose = service.CloseWagerCompletedBonusTask(new CloseWagerCompletedBonusTaskRequest(
                userUid, fourthClaim.Data.BonusTaskId, 10, 20, 0, clock.Now));
            Assert.Equal(CloseReason.WagerCompleted, completedClose.Data!.CloseReason);
            Assert.Equal(10m, completedClose.Data.ConvertedAmount);
            Assert.Equal(new[] { WalletAction.ClearBonusWallet, WalletAction.CreditMainWallet },
                completedClose.Data.WalletInstructions.Select(x => x.Action));
            Assert.True(service.CloseWagerCompletedBonusTask(new CloseWagerCompletedBonusTaskRequest(
                userUid, fourthClaim.Data.BonusTaskId, 10, 20, 0, clock.Now)).Data!.AlreadyClosed);

            using (var convertType = new MySqlCommand(
                "UPDATE PromotionActivity SET ConvertType = 2 WHERE ActivityUID = @id", connection))
            {
                convertType.Parameters.AddWithValue("@id", activityUid);
                convertType.ExecuteNonQuery();
            }
            clock.Now = now.AddDays(4);
            var fifth = service.CreateEligibility(new CreateEligibilityRequest(eventId + "-fifth", userUid,
                TriggerType.Free, clock.Now, null, new[] { activityUid }));
            var fifthClaim = service.ClaimPromotion(new ClaimPromotionRequest(userUid,
                Assert.Single(fifth.Data!.Entries).EligibilityEntryId));
            Assert.Equal(PromotionResultKind.Succeeded, fifthClaim.Kind);
            var readyToClose = service.AccumulateWager(new AccumulateWagerRequest(userUid,
                fifthClaim.Data!.BonusTaskId, 20, 0, clock.Now, 10));
            Assert.Equal(BonusTaskOutcome.ReadyToClose, readyToClose.Data!.Outcome);
            var failedClose = service.CloseWagerCompletedBonusTask(new CloseWagerCompletedBonusTaskRequest(
                userUid, fifthClaim.Data.BonusTaskId, 10, 20, 0, clock.Now));
            Assert.Equal(PromotionErrorCode.BalanceConvertFormulaFailed, failedClose.ErrorCode);
            Assert.Equal(20m, service.GetBonusTaskStatus(new GetBonusTaskStatusRequest(userUid)).Data!.Task!.CurrentWagerAmount);
        }
        finally
        {
            foreach (var table in new[] { "PromotionBonusHistory", "PromotionBonusStatus", "EligibilityEntry", "PromotionTriggerEvent" })
            {
                using var cleanup = new MySqlCommand($"DELETE FROM {table} WHERE UserUID IN (@user, @other, @competing)", connection);
                cleanup.Parameters.AddWithValue("@user", userUid);
                cleanup.Parameters.AddWithValue("@other", userUid + 1);
                cleanup.Parameters.AddWithValue("@competing", userUid + 2);
                cleanup.ExecuteNonQuery();
            }
            using var activityCleanup = new MySqlCommand("DELETE FROM PromotionActivity WHERE ActivityUID = @id", connection);
            activityCleanup.Parameters.AddWithValue("@id", activityUid);
            activityCleanup.ExecuteNonQuery();
        }
    }

    private sealed class FixedClock : ILocalClock
    {
        public DateTime Now { get; set; }
        public FixedClock(DateTime now) => Now = now;
        public DateTime GetNow() => Now;
    }

    private sealed class SequenceGuidProvider : IGuidProvider
    {
        private readonly Queue<Guid> values;
        public int Calls { get; private set; }
        public SequenceGuidProvider(params Guid[] values) => this.values = new(values);
        public Guid NewGuid()
        {
            Calls++;
            return values.Dequeue();
        }
    }

    [Fact]
    public void DB_014_RejectsUnknownTableAndColumn()
    {
        Assert.Throws<ArgumentException>(() => PromotionTableMetadata.Validate("Other", new[] { "UserUID" }));
        Assert.Throws<ArgumentException>(() => PromotionTableMetadata.Validate(PromotionTableMetadata.Status, new[] { "Injected" }));
        Assert.Throws<ArgumentException>(() => PromotionTableMetadata.Validate(new Query(PromotionTableMetadata.Status,
            Array.Empty<string>(), Array.Empty<Condition>(), Array.Empty<Sort>())));
        Assert.Equal("PromotionTriggerEvent.PRIMARY", PromotionTableMetadata.KnownConstraint(
            PromotionTableMetadata.Event, "PRIMARY"));
        Assert.Null(PromotionTableMetadata.KnownConstraint(PromotionTableMetadata.Event, "unknown_constraint"));
    }

    [Fact]
    public void DB_026_027_KeysetAndHistoryCutoffAreBounded()
    {
        var gateway = new RecordingGateway();
        var store = new PromotionMySqlDataStore(gateway);
        var day = new DateOnly(2026, 9, 17);
        Assert.Equal(1000, store.GetUsersWithExpiredTasks(day, 0, 1000).Count);
        Assert.Equal(1001L, store.GetUsersWithExpiredTasks(day, 1000, 1000).Single());
        var cutoff = new DateTime(2026, 6, 1);
        Assert.Equal(7, store.DeleteExpiredBonusHistory(cutoff, 50));
        var delete = Assert.IsType<DeleteQuery>(gateway.LastDelete);
        Assert.Equal(PromotionTableMetadata.History, delete.Table);
        Assert.Equal(Comparison.LessThan, Assert.Single(delete.Where).Operator);
        Assert.Equal(cutoff, delete.Where[0].Value);
        Assert.Equal("BonusHistoryId", Assert.Single(delete.Order).Column);
        Assert.Equal(50, delete.Limit);
    }

    private sealed class RecordingGateway : IPromotionV2Gateway
    {
        public DeleteQuery? LastDelete { get; private set; }
        public T Transaction<T>(Func<IPromotionV2Transaction, T> action) => throw new NotSupportedException();
        public IReadOnlyList<IReadOnlyDictionary<string, object?>> Select(Query query)
        {
            Assert.Equal(PromotionTableMetadata.Status, query.Table);
            Assert.Equal("UserUID", Assert.Single(query.Fields));
            var cursor = Convert.ToInt64(query.Where.Single(x => x.Column == "UserUID").Value);
            return Enumerable.Range(1, 1001).Select(x => (long)x).Where(x => x > cursor)
                .Take(query.Limit!.Value).Select(x => (IReadOnlyDictionary<string, object?>)
                    new Dictionary<string, object?> { ["UserUID"] = x }).ToArray();
        }
        public long Delete(DeleteQuery query)
        {
            LastDelete = query;
            return 7;
        }
    }

    [Fact]
    public void DB_025_ZeroAffectedUpdateMustRereadLockedStatus()
    {
        var unchanged = new ZeroUpdateGateway();
        var store = new PromotionMySqlDataStore(unchanged);
        store.ExecuteInTransaction(tx =>
        {
            var value = tx.GetOrCreateBonusStatusForUpdate(42, new DateTime(2026, 9, 17));
            tx.SaveBonusStatus(value);
            Assert.Equal(2, unchanged.SelectCount);
            Assert.Equal(Promotion.Core.Data.PromotionDataErrorKind.ConcurrencyConflict,
                Assert.Throws<Promotion.Core.Data.PromotionDataException>(() =>
                    tx.SaveBonusStatus(value with { UpdatedAt = value.UpdatedAt.AddSeconds(1) })).Kind);
            return 0;
        });
    }

    private sealed class ZeroUpdateGateway : IPromotionV2Gateway, IPromotionV2Transaction
    {
        public int SelectCount { get; private set; }
        public T Transaction<T>(Func<IPromotionV2Transaction, T> action) => action(this);
        public IReadOnlyList<IReadOnlyDictionary<string, object?>> Select(Query query)
        {
            Assert.True(query.ForUpdate);
            SelectCount++;
            var now = new DateTime(2026, 9, 17);
            return new[] { (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
            {
                ["UserUID"] = 42L, ["TaskState"] = (byte)0, ["BonusTaskId"] = null,
                ["EligibilityEntryId"] = null, ["ActivityUID"] = null, ["BusinessDay"] = null,
                ["LastFirstLoginBusinessDay"] = null, ["LastFirstDepositBusinessDay"] = null,
                ["ActivitySnapshotJson"] = null, ["BonusAmount"] = 0m, ["RequiredWagerAmount"] = 0m,
                ["CurrentWagerAmount"] = 0m, ["ClaimedAt"] = null, ["CreatedAt"] = now, ["UpdatedAt"] = now
            } };
        }
        public long Update(string table, IReadOnlyDictionary<string, object?> values, IReadOnlyList<Condition> where) => 0;
        public InsertResult Insert(string table, IReadOnlyDictionary<string, object?> values) => throw new NotSupportedException();
        public long Count(string table, IReadOnlyList<Condition> where) => throw new NotSupportedException();
        public long Delete(DeleteQuery query) => throw new NotSupportedException();
    }

    [Fact]
    public async Task DB_021_TwoConnectionsCreateOneLockedBonusStatus()
    {
        var connectionString = TestConnectionString();
        var store = new PromotionMySqlDataStore(new PromotionV2Gateway(InitializeMysql(connectionString)));
        var userUid = 800_000_000_000L + Random.Shared.Next(1, 100_000_000);
        using var barrier = new Barrier(2);
        var now = new DateTime(2026, 9, 17, 12, 0, 0);
        try
        {
            var workers = Enumerable.Range(0, 2).Select(_ => Task.Run(() => store.ExecuteInTransaction(tx =>
            {
                Assert.True(barrier.SignalAndWait(TimeSpan.FromSeconds(10)));
                return tx.GetOrCreateBonusStatusForUpdate(userUid, now);
            }))).ToArray();
            var rows = await Task.WhenAll(workers);
            Assert.All(rows, row => Assert.Equal(userUid, row.UserUID));
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var count = new MySqlCommand("SELECT COUNT(*) FROM PromotionBonusStatus WHERE UserUID = @user", connection);
            count.Parameters.AddWithValue("@user", userUid);
            Assert.Equal(1L, Convert.ToInt64(count.ExecuteScalar()));
        }
        finally
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var cleanup = new MySqlCommand("DELETE FROM PromotionBonusStatus WHERE UserUID = @user", connection);
            cleanup.Parameters.AddWithValue("@user", userUid);
            cleanup.ExecuteNonQuery();
        }
    }

    [Fact]
    public void DB_010_011_012_023_AdapterRoundTripOnRealMysql()
    {
        var connectionString = TestConnectionString();
        var mysql = InitializeMysql(connectionString);
        var store = new PromotionMySqlDataStore(new PromotionV2Gateway(mysql));
        var suffix = Guid.NewGuid().ToString("N");
        var userUid = 900_000_000_000L + Random.Shared.Next(1, 100_000_000);
        var activityUid = userUid;
        var eventId = "adapter-" + suffix;
        var day = new DateOnly(2026, 9, 17);
        var now = new DateTime(2026, 9, 17, 12, 34, 56, 123, DateTimeKind.Unspecified).AddTicks(4560);
        using var setup = new MySqlConnection(connectionString);
        setup.Open();
        try
        {
            using (var command = new MySqlCommand(@"INSERT INTO PromotionActivity
                (ActivityUID, ActivityInfo, BonusType, FixedBonusAmount, MaxBonusAmount, DepositPercentage,
                 MinimumDepositAmount, WagerMultiplier, WagerCalculationType, MaxBetAmount, GameServerList,
                 WagerContributionRate, ConvertType, FixedConvertedAmount, MaxBalanceConvertedAmount,
                 DailyClaimLimit, StartDate, EndDate, WeekdayMask, TriggerType, IsNonStackable, ExclusiveGroup,
                 CreatedAt, UpdatedAt)
                VALUES (@id, NULL, 1, 10, 100, 100, NULL, 2, 1, NULL, 'g1,g2',
                        100.0000, 1, 10, NULL, 1, NULL, NULL, '1111111', 5, 0, NULL, @now, @now)", setup))
            {
                command.Parameters.AddWithValue("@id", activityUid);
                command.Parameters.AddWithValue("@now", now);
                command.ExecuteNonQuery();
            }
            using (var copy = new MySqlCommand(@"INSERT INTO PromotionActivity
                SELECT @copyId, ActivityInfo, BonusType, FixedBonusAmount, MaxBonusAmount, DepositPercentage,
                    MinimumDepositAmount, WagerMultiplier, WagerCalculationType, MaxBetAmount, GameServerList,
                    WagerContributionRate, ConvertType, FixedConvertedAmount, MaxBalanceConvertedAmount,
                    DailyClaimLimit, StartDate, EndDate, WeekdayMask, ActivityStatus, TriggerType, 1, ExclusiveGroup,
                    CreatedAt, UpdatedAt FROM PromotionActivity WHERE ActivityUID = @sourceId", setup))
            {
                copy.Parameters.AddWithValue("@copyId", activityUid + 1);
                copy.Parameters.AddWithValue("@sourceId", activityUid);
                copy.ExecuteNonQuery();
            }
            using (var groupCopy = new MySqlCommand(@"INSERT INTO PromotionActivity
                SELECT @copyId, ActivityInfo, BonusType, FixedBonusAmount, MaxBonusAmount, DepositPercentage,
                    MinimumDepositAmount, WagerMultiplier, WagerCalculationType, MaxBetAmount, GameServerList,
                    WagerContributionRate, ConvertType, FixedConvertedAmount, MaxBalanceConvertedAmount,
                    DailyClaimLimit, StartDate, EndDate, WeekdayMask, ActivityStatus, TriggerType, 0, 'group-a',
                    CreatedAt, UpdatedAt FROM PromotionActivity WHERE ActivityUID = @sourceId", setup))
            {
                groupCopy.Parameters.AddWithValue("@copyId", activityUid + 2);
                groupCopy.Parameters.AddWithValue("@sourceId", activityUid);
                groupCopy.ExecuteNonQuery();
            }
            var activity = store.GetActivity(activityUid)!;
            Assert.Equal("g1,g2", activity.GameServerList);
            Assert.Null(activity.MaxBetAmount);
            Assert.Null(activity.MaxBalanceConvertedAmount);
            Assert.Equal(100m, activity.WagerContributionRate);
            var extraColumn = "P5Test_" + suffix[..8];
            using (var addColumn = new MySqlCommand($"ALTER TABLE PromotionActivity ADD COLUMN `{extraColumn}` INT NULL", setup))
                addColumn.ExecuteNonQuery();
            try { Assert.Equal(activity, store.GetActivity(activityUid)); }
            finally
            {
                using var dropColumn = new MySqlCommand($"ALTER TABLE PromotionActivity DROP COLUMN `{extraColumn}`", setup);
                dropColumn.ExecuteNonQuery();
            }

            var taskId = Guid.NewGuid().ToString("D");
            var entry = store.ExecuteInTransaction(tx =>
            {
                var status = tx.GetOrCreateBonusStatusForUpdate(userUid, now);
                Assert.Equal(BonusTaskState.None, status.TaskState);
                tx.InsertTriggerEvent(new PromotionTriggerEvent(eventId, userUid, TriggerType.Free, now, day, null, now));
                var inserted = tx.InsertEligibilityEntries(new[] { new NewEligibilityEntry(eventId, userUid,
                    activityUid, day, TriggerType.Free, null, EligibilityStatus.Available, null, null, now, now) });
                Assert.Single(inserted);
                Assert.True(inserted[0].EligibilityEntryId > 0);
                Assert.Equal(0, tx.CountClaimedEligibility(userUid, activityUid, day));
                tx.MarkEligibilityClaimed(inserted[0].EligibilityEntryId, taskId, now);
                Assert.Equal(1, tx.CountClaimedEligibility(userUid, activityUid, day));
                Assert.Equal(Promotion.Core.Data.PromotionDataErrorKind.ConcurrencyConflict,
                    Assert.Throws<Promotion.Core.Data.PromotionDataException>(() =>
                        tx.MarkEligibilityClaimed(inserted[0].EligibilityEntryId, Guid.NewGuid().ToString("D"), now)).Kind);
                tx.SaveBonusStatus(status with
                {
                    LastFirstLoginBusinessDay = day,
                    LastFirstDepositBusinessDay = day.AddDays(-1)
                });
                return inserted[0];
            });
            Assert.Equal(taskId, store.ExecuteInTransaction(tx => tx.GetEligibilityEntryForUpdate(entry.EligibilityEntryId))!.ClaimedBonusTaskId);
            Assert.Equal(eventId, store.ExecuteInTransaction(tx => tx.GetTriggerEvent(eventId))!.EventId);
            var duplicateEvent = Assert.Throws<Promotion.Core.Data.PromotionDataException>(() =>
                store.ExecuteInTransaction(tx =>
                {
                    tx.InsertTriggerEvent(new PromotionTriggerEvent(eventId, userUid, TriggerType.Free, now, day, null, now));
                    return 0;
                }));
            Assert.Equal(Promotion.Core.Data.PromotionDataErrorKind.DuplicateKey, duplicateEvent.Kind);
            Assert.Equal("PromotionTriggerEvent.PRIMARY", duplicateEvent.ConstraintName);
            var savedDailyTriggerDays = store.GetBonusStatus(userUid)!;
            Assert.Equal(BonusTaskState.None, savedDailyTriggerDays.TaskState);
            Assert.Equal(day, savedDailyTriggerDays.LastFirstLoginBusinessDay);
            Assert.Equal(day.AddDays(-1), savedDailyTriggerDays.LastFirstDepositBusinessDay);
            var snapshot = ActivitySnapshotSerializer.Serialize(ActivitySnapshotFactory.Capture(activity, now));
            var history = store.ExecuteInTransaction(tx =>
            {
                var locked = tx.GetOrCreateBonusStatusForUpdate(userUid, now);
                tx.SaveBonusStatus(locked with
                {
                    TaskState = BonusTaskState.InProgress, BonusTaskId = taskId,
                    EligibilityEntryId = entry.EligibilityEntryId, ActivityUID = activityUid,
                    BusinessDay = day, ActivitySnapshotJson = snapshot, BonusAmount = 10m,
                    RequiredWagerAmount = 20m, CurrentWagerAmount = 5m, ClaimedAt = now
                });
                return tx.InsertBonusHistory(new NewBonusHistory(taskId, userUid, entry.EligibilityEntryId,
                    activityUid, day, snapshot, 10m, 20m, 20m, 10m, CloseReason.WagerCompleted, now, now, now));
            });
            Assert.True(history.BonusHistoryId > 0);
            Assert.Equal(snapshot, store.GetBonusStatus(userUid)!.ActivitySnapshotJson);
            Assert.Equal(history, Assert.Single(store.GetBonusHistory(userUid, now.AddDays(-1), now.AddDays(1), 0, 10)));
            using (var older = new MySqlCommand("SELECT COUNT(*) FROM PromotionBonusHistory WHERE ClosedAt < @time", setup))
            {
                older.Parameters.AddWithValue("@time", now);
                Assert.Equal(0L, Convert.ToInt64(older.ExecuteScalar()));
            }
            Assert.Equal(0, store.DeleteExpiredBonusHistory(now, 1));
            Assert.Equal(1, store.DeleteExpiredBonusHistory(now.AddTicks(10), 1));
            Assert.Null(store.ExecuteInTransaction(tx => tx.GetBonusHistoryByTaskId(taskId)));
            var secondEvent = "other-" + suffix;
            store.ExecuteInTransaction(tx =>
            {
                tx.InsertTriggerEvent(new PromotionTriggerEvent(secondEvent, userUid, TriggerType.Free, now, day, null, now));
                tx.InsertEligibilityEntries(new[] { new NewEligibilityEntry(secondEvent, userUid, activityUid + 1,
                    day, TriggerType.Free, null, EligibilityStatus.Available, null, null, now, now) });
                Assert.Equal(0, tx.ExcludeEligibilityEntries(userUid, day, entry.EligibilityEntryId,
                    taskId, false, null, now));
                Assert.Equal(1, tx.ExcludeEligibilityEntries(userUid, day, entry.EligibilityEntryId,
                    taskId, true, null, now));
                return 0;
            });
            var groupEvent = "group-" + suffix;
            store.ExecuteInTransaction(tx =>
            {
                tx.InsertTriggerEvent(new PromotionTriggerEvent(groupEvent, userUid, TriggerType.Free, now, day, null, now));
                tx.InsertEligibilityEntries(new[] { new NewEligibilityEntry(groupEvent, userUid, activityUid + 2,
                    day, TriggerType.Free, null, EligibilityStatus.Available, null, null, now, now) });
                Assert.Equal(1, tx.ExcludeEligibilityEntries(userUid, day, entry.EligibilityEntryId,
                    taskId, false, "group-a", now));
                return 0;
            });
            Assert.Contains(userUid, store.GetUsersWithExpiredTasks(day.AddDays(1), userUid - 1, 10));
            var oldEvent = "old-" + suffix;
            store.ExecuteInTransaction(tx =>
            {
                tx.InsertTriggerEvent(new PromotionTriggerEvent(oldEvent, userUid, TriggerType.Free,
                    now.AddDays(-1), day.AddDays(-1), null, now.AddDays(-1)));
                tx.InsertEligibilityEntries(new[] { new NewEligibilityEntry(oldEvent, userUid, activityUid,
                    day.AddDays(-1), TriggerType.Free, null, EligibilityStatus.Available, null, null,
                    now.AddDays(-1), now.AddDays(-1)) });
                return 0;
            });
            foreach (var extraUser in new[] { userUid + 10, userUid + 20 })
            {
                var extraEvent = "extra-" + extraUser + "-" + suffix;
                store.ExecuteInTransaction(tx =>
                {
                    tx.InsertTriggerEvent(new PromotionTriggerEvent(extraEvent, extraUser, TriggerType.Free,
                        now.AddDays(-1), day.AddDays(-1), null, now.AddDays(-1)));
                    tx.InsertEligibilityEntries(new[] { new NewEligibilityEntry(extraEvent, extraUser, activityUid,
                        day.AddDays(-1), TriggerType.Free, null, EligibilityStatus.Available, null, null,
                        now.AddDays(-1), now.AddDays(-1)) });
                    return 0;
                });
            }
            Assert.Contains(userUid, store.GetUsersWithExpiredEligibility(day, userUid - 1, 10));
            Assert.Equal(new[] { userUid, userUid + 10 },
                store.GetUsersWithExpiredEligibility(day, userUid - 1, 2));
            Assert.Equal(new[] { userUid + 20 },
                store.GetUsersWithExpiredEligibility(day, userUid + 10, 2));
            Assert.Equal(1, store.ExecuteInTransaction(tx => tx.ExpireAvailableEligibilityEntries(userUid, day, now)));
            Assert.DoesNotContain(userUid, store.GetUsersWithExpiredEligibility(day, userUid - 1, 10));
            using (var corruptStatus = new MySqlCommand("UPDATE PromotionActivity SET ActivityStatus = 255 WHERE ActivityUID = @id", setup))
            {
                corruptStatus.Parameters.AddWithValue("@id", activityUid);
                corruptStatus.ExecuteNonQuery();
            }
            Assert.Equal(Promotion.Core.Data.PromotionDataErrorKind.DataCorruption,
                Assert.Throws<Promotion.Core.Data.PromotionDataException>(() => store.GetActivity(activityUid)).Kind);
            using (var restoreStatus = new MySqlCommand("UPDATE PromotionActivity SET ActivityStatus = 1 WHERE ActivityUID = @id", setup))
            {
                restoreStatus.Parameters.AddWithValue("@id", activityUid);
                restoreStatus.ExecuteNonQuery();
            }
            using (var corrupt = new MySqlCommand("UPDATE PromotionActivity SET BonusType = 255 WHERE ActivityUID = @id", setup))
            {
                corrupt.Parameters.AddWithValue("@id", activityUid);
                corrupt.ExecuteNonQuery();
            }
            Assert.Equal(Promotion.Core.Data.PromotionDataErrorKind.DataCorruption,
                Assert.Throws<Promotion.Core.Data.PromotionDataException>(() => store.GetActivity(activityUid)).Kind);
            using (var corruptSnapshot = new MySqlCommand(
                "UPDATE PromotionBonusStatus SET ActivitySnapshotJson = 'invalid-json' WHERE UserUID = @user", setup))
            {
                corruptSnapshot.Parameters.AddWithValue("@user", userUid);
                corruptSnapshot.ExecuteNonQuery();
            }
            Assert.Equal(Promotion.Core.Data.PromotionDataErrorKind.DataCorruption,
                Assert.Throws<Promotion.Core.Data.PromotionDataException>(() => store.GetBonusStatus(userUid)).Kind);
        }
        finally
        {
            foreach (var statement in new[]
            {
                "DELETE FROM PromotionBonusHistory WHERE UserUID = @user",
                "DELETE FROM PromotionBonusStatus WHERE UserUID = @user",
                "DELETE FROM EligibilityEntry WHERE UserUID IN (@user, @plus10, @plus20)",
                "DELETE FROM PromotionTriggerEvent WHERE UserUID IN (@user, @plus10, @plus20)",
                "DELETE FROM PromotionActivity WHERE ActivityUID IN (@user, @other, @third)"
            })
            {
                using var cleanup = new MySqlCommand(statement, setup);
                cleanup.Parameters.AddWithValue("@user", userUid);
                cleanup.Parameters.AddWithValue("@other", userUid + 1);
                cleanup.Parameters.AddWithValue("@third", userUid + 2);
                cleanup.Parameters.AddWithValue("@plus10", userUid + 10);
                cleanup.Parameters.AddWithValue("@plus20", userUid + 20);
                cleanup.ExecuteNonQuery();
            }
        }
    }

    private static MysqlAcess InitializeMysql(string connectionString)
    {
        var directory = Path.Combine(Path.GetTempPath(), "promotion-adapter-" + Guid.NewGuid().ToString("N"));
        var path = Path.Combine(directory, "SqlConnection.txt");
        var prior = Environment.CurrentDirectory;
        var connection = new MySqlConnectionStringBuilder(connectionString);
        try
        {
            Directory.CreateDirectory(directory);
            File.WriteAllLines(path, new[]
            {
                "DBIP:" + connection.Server,
                "DBID:" + connection.UserID,
                "DBPWD:" + connection.Password,
                "DBDataBase:" + connection.Database
            }, new UTF8Encoding(false));
            Environment.CurrentDirectory = directory;
            return MysqlAcess.GetInstance();
        }
        finally
        {
            Environment.CurrentDirectory = prior;
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
        }
    }

    private static long Scalar(MySqlConnection connection, string sql, long userUid)
    {
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@user", userUid);
        return Convert.ToInt64(command.ExecuteScalar());
    }

    private static string TestConnectionString()
    {
        var text = Environment.GetEnvironmentVariable("PROMOTION_TEST_MYSQL_CONNECTION_STRING")
            ?? throw new InvalidOperationException("PROMOTION_TEST_MYSQL_CONNECTION_STRING is required.");
        var builder = new MySqlConnectionStringBuilder(text);
        if (!builder.Database.StartsWith("promotion_test_", StringComparison.OrdinalIgnoreCase) ||
            builder.Server is not ("localhost" or "127.0.0.1" or "::1") || builder.ConnectionString.Contains(':'))
            throw new InvalidOperationException("A loopback promotion_test_ database is required.");
        return builder.ConnectionString;
    }
}
