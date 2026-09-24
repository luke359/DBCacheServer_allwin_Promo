using MySql.Data.MySqlClient;
using Xunit;

namespace Promotion.Data.MySql.IntegrationTests;

[Collection("MysqlAcess V2")]
public sealed class V001SchemaTests
{
    private const string TaskId = "11111111-1111-4111-8111-111111111111";
    private const string OtherTaskId = "22222222-2222-4222-8222-222222222222";

    [Fact]
    public void DB_001_002_FiveTablesAndMetadataAreExact()
    {
        using var connection = OpenTestConnection();
        Assert.Equal(5L, Scalar(connection, null,
            "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME IN ('PromotionActivity','PromotionTriggerEvent','EligibilityEntry','PromotionBonusStatus','PromotionBonusHistory')"));
        Assert.Equal(74L, Scalar(connection, null,
            "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME IN ('PromotionActivity','PromotionTriggerEvent','EligibilityEntry','PromotionBonusStatus','PromotionBonusHistory')"));
        Assert.Equal(0L, Scalar(connection, null,
            "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME IN ('PromotionActivity','PromotionTriggerEvent','EligibilityEntry','PromotionBonusStatus','PromotionBonusHistory') " +
            "AND (ENGINE <> 'InnoDB' OR TABLE_COLLATION <> 'utf8mb4_bin')"));
        Assert.Equal(6L, Scalar(connection, null,
            "SELECT COUNT(*) FROM information_schema.KEY_COLUMN_USAGE WHERE CONSTRAINT_SCHEMA=DATABASE() " +
            "AND REFERENCED_TABLE_NAME IS NOT NULL"));
    }

    [Fact]
    public void DB_V002_NewColumnsAndNullableLimitsArePresent()
    {
        using var connection = OpenTestConnection();
        Assert.Equal(1L, Scalar(connection, null,
            "SELECT COUNT(*) FROM PromotionSchemaMigration WHERE Version='V002' AND Status='SUCCEEDED'"));
        Assert.Equal(2L, Scalar(connection, null,
            "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME='PromotionActivity' AND COLUMN_NAME IN ('WagerCalculationType','GameServerList')"));
        Assert.Equal(2L, Scalar(connection, null,
            "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME='PromotionActivity' AND COLUMN_NAME IN ('MaxBetAmount','MaxBalanceConvertedAmount') " +
            "AND IS_NULLABLE='YES'"));
        Assert.Equal(500L, Scalar(connection, null,
            "SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME='PromotionActivity' AND COLUMN_NAME='GameServerList'"));
    }

    [Fact]
    public void DB_V004_DailyFirstTriggerDaysAreNullableDates()
    {
        using var connection = OpenTestConnection();
        Assert.Equal(1L, Scalar(connection, null,
            "SELECT COUNT(*) FROM PromotionSchemaMigration WHERE Version='V004' AND Status='SUCCEEDED'"));
        Assert.Equal(2L, Scalar(connection, null,
            "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME='PromotionBonusStatus' " +
            "AND COLUMN_NAME IN ('LastFirstLoginBusinessDay','LastFirstDepositBusinessDay') " +
            "AND DATA_TYPE='date' AND IS_NULLABLE='YES'"));
    }

    [Fact]
    public void DB_V005_ActivityStatusDefaultsToActiveAndSupportsArchived()
    {
        using var connection = OpenTestConnection();
        using var transaction = connection.BeginTransaction();
        Assert.Equal(1L, Scalar(connection, transaction,
            "SELECT COUNT(*) FROM PromotionSchemaMigration WHERE Version='V005' AND Status='SUCCEEDED'"));
        Assert.Equal(1L, Scalar(connection, transaction,
            "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() " +
            "AND TABLE_NAME='PromotionActivity' AND COLUMN_NAME='ActivityStatus' " +
            "AND COLUMN_TYPE='tinyint unsigned' AND IS_NULLABLE='NO' AND COLUMN_DEFAULT='1'"));
        InsertActivity(connection, transaction);
        Assert.Equal(1L, Scalar(connection, transaction,
            "SELECT ActivityStatus FROM PromotionActivity WHERE ActivityUID=20001"));
        Execute(connection, transaction,
            "UPDATE PromotionActivity SET ActivityStatus=2 WHERE ActivityUID=20001");
        Assert.Equal(2L, Scalar(connection, transaction,
            "SELECT ActivityStatus FROM PromotionActivity WHERE ActivityUID=20001"));
    }

    [Fact]
    public void DB_003_004_EventIdIsCaseSensitiveAndPrimaryKeyIsUnique()
    {
        using var connection = OpenTestConnection();
        using var transaction = connection.BeginTransaction();
        InsertEvent(connection, transaction, "evt-A");
        InsertEvent(connection, transaction, "evt-a");
        Assert.Equal(2L, Scalar(connection, transaction, "SELECT COUNT(*) FROM PromotionTriggerEvent"));
        var duplicate = Assert.Throws<MySqlException>(() => InsertEvent(connection, transaction, "evt-A"));
        Assert.Equal(1062, duplicate.Number);
    }

    [Fact]
    public void DB_003_BonusTaskIdAndExclusiveGroupAreCaseSensitive()
    {
        using var connection = OpenTestConnection();
        using var transaction = connection.BeginTransaction();
        InsertActivity(connection, transaction, 20001, "GroupA");
        InsertActivity(connection, transaction, 20002, "groupa");
        Assert.Equal(1L, Scalar(connection, transaction,
            "SELECT COUNT(*) FROM PromotionActivity WHERE ExclusiveGroup = 'GroupA'"));
        InsertEvent(connection, transaction, "evt-case");
        var entryId = InsertEligibility(connection, transaction, "evt-case", null);
        var lower = "aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa";
        var upper = lower.ToUpperInvariant();
        InsertStatus(connection, transaction, 10001, lower, entryId);
        InsertStatus(connection, transaction, 10002, upper, entryId);
        InsertHistory(connection, transaction, lower, entryId);
        InsertHistory(connection, transaction, upper, entryId);
        Assert.Equal(2L, Scalar(connection, transaction, "SELECT COUNT(*) FROM PromotionBonusStatus"));
        Assert.Equal(2L, Scalar(connection, transaction, "SELECT COUNT(*) FROM PromotionBonusHistory"));
    }

    [Fact]
    public void DB_005_EligibilityIsUniquePerEventAndActivity()
    {
        using var connection = OpenTestConnection();
        using var transaction = connection.BeginTransaction();
        InsertActivity(connection, transaction);
        InsertEvent(connection, transaction, "evt-1");
        InsertEvent(connection, transaction, "evt-2");
        InsertEligibility(connection, transaction, "evt-1", null);
        var duplicate = Assert.Throws<MySqlException>(() => InsertEligibility(connection, transaction, "evt-1", null));
        Assert.Equal(1062, duplicate.Number);
        InsertEligibility(connection, transaction, "evt-2", null);
        Assert.Equal(2L, Scalar(connection, transaction, "SELECT COUNT(*) FROM EligibilityEntry"));
    }

    [Fact]
    public void DB_006_NullClaimedTaskIdIsAllowedButRepeatedValueIsNot()
    {
        using var connection = OpenTestConnection();
        using var transaction = connection.BeginTransaction();
        InsertActivity(connection, transaction);
        for (var i = 1; i <= 4; i++) InsertEvent(connection, transaction, $"evt-{i}");
        InsertEligibility(connection, transaction, "evt-1", null);
        InsertEligibility(connection, transaction, "evt-2", null);
        InsertEligibility(connection, transaction, "evt-3", TaskId);
        var duplicate = Assert.Throws<MySqlException>(() => InsertEligibility(connection, transaction, "evt-4", TaskId));
        Assert.Equal(1062, duplicate.Number);
        Assert.Equal(3L, Scalar(connection, transaction, "SELECT COUNT(*) FROM EligibilityEntry"));
    }

    [Fact]
    public void DB_007_BonusStatusAndHistoryTaskIdsAreUnique()
    {
        using var connection = OpenTestConnection();
        using var transaction = connection.BeginTransaction();
        InsertActivity(connection, transaction);
        InsertEvent(connection, transaction, "evt-1");
        var entryId = InsertEligibility(connection, transaction, "evt-1", null);
        InsertStatus(connection, transaction, 10001, TaskId, entryId);
        var statusDuplicate = Assert.Throws<MySqlException>(() => InsertStatus(connection, transaction, 10002, TaskId, entryId));
        Assert.Equal(1062, statusDuplicate.Number);
        InsertHistory(connection, transaction, TaskId, entryId);
        var historyDuplicate = Assert.Throws<MySqlException>(() => InsertHistory(connection, transaction, TaskId, entryId));
        Assert.Equal(1062, historyDuplicate.Number);
    }

    [Fact]
    public void DB_008_InvalidForeignKeysAreRejectedWithoutRows()
    {
        using var connection = OpenTestConnection();
        using var transaction = connection.BeginTransaction();
        var missingEvent = Assert.Throws<MySqlException>(() => InsertEligibility(connection, transaction, "missing-event", null));
        Assert.Equal(1452, missingEvent.Number);
        InsertEvent(connection, transaction, "evt-missing-activity");
        var missingActivity = Assert.Throws<MySqlException>(() => InsertEligibility(connection, transaction, "evt-missing-activity", null));
        Assert.Equal(1452, missingActivity.Number);
        InsertActivity(connection, transaction);
        var missingEntry = Assert.Throws<MySqlException>(() => InsertStatus(connection, transaction, 10001, OtherTaskId, 99999));
        Assert.Equal(1452, missingEntry.Number);
        Assert.Equal(0L, Scalar(connection, transaction, "SELECT COUNT(*) FROM EligibilityEntry"));
        Assert.Equal(0L, Scalar(connection, transaction, "SELECT COUNT(*) FROM PromotionBonusStatus"));
    }

    private static MySqlConnection OpenTestConnection()
    {
        var text = Environment.GetEnvironmentVariable("PROMOTION_TEST_MYSQL_CONNECTION_STRING")
            ?? throw new InvalidOperationException("PROMOTION_TEST_MYSQL_CONNECTION_STRING is required.");
        var builder = new MySqlConnectionStringBuilder(text);
        if (!builder.Database.StartsWith("promotion_test_", StringComparison.Ordinal) ||
            builder.Server is not ("localhost" or "127.0.0.1" or "::1"))
            throw new InvalidOperationException("Integration tests require a loopback promotion_test_ database.");
        var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();
        return connection;
    }

    private static long Scalar(MySqlConnection connection, MySqlTransaction? transaction, string sql)
    {
        using var command = new MySqlCommand(sql, connection, transaction);
        return Convert.ToInt64(command.ExecuteScalar());
    }

    private static void Execute(MySqlConnection connection, MySqlTransaction transaction, string sql,
        params (string Name, object? Value)[] parameters)
    {
        using var command = new MySqlCommand(sql, connection, transaction);
        foreach (var (name, value) in parameters) command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        command.ExecuteNonQuery();
    }

    private static void InsertActivity(MySqlConnection connection, MySqlTransaction transaction,
        long activityUid = 20001, string? exclusiveGroup = null) =>
        Execute(connection, transaction, @"
            INSERT INTO PromotionActivity (ActivityUID, ActivityInfo, BonusType, FixedBonusAmount,
                MaxBonusAmount, DepositPercentage, MinimumDepositAmount, WagerMultiplier, MaxBetAmount,
                WagerContributionRate, ConvertType, FixedConvertedAmount, MaxBalanceConvertedAmount,
                DailyClaimLimit, StartDate, EndDate, WeekdayMask, TriggerType, IsNonStackable,
                ExclusiveGroup, CreatedAt, UpdatedAt)
            VALUES (@activityUid, NULL, 1, 10, 100, 30, 10, 30, 5, 100.0000, 1, 5, 100,
                1, '2026-09-01', '2026-10-01', '1111111', 4, 0, @exclusiveGroup,
                '2026-09-15 10:00:00.123456', '2026-09-15 10:00:00.123456')
            ", ("@activityUid", activityUid), ("@exclusiveGroup", exclusiveGroup));

    private static void InsertEvent(MySqlConnection connection, MySqlTransaction transaction, string id) =>
        Execute(connection, transaction, @"
            INSERT INTO PromotionTriggerEvent (EventId, UserUID, TriggerType, EventTime,
                BusinessDay, EligibleDepositAmount, CreatedAt)
            VALUES (@eventId, 10001, 4, '2026-09-15 10:00:00.123456', '2026-09-15',
                10.0000, '2026-09-15 10:00:00.123456')
            ", ("@eventId", id));

    private static long InsertEligibility(MySqlConnection connection, MySqlTransaction transaction,
        string eventId, string? claimedTaskId)
    {
        using var command = new MySqlCommand(@"
            INSERT INTO EligibilityEntry (EventId, UserUID, ActivityUID, BusinessDay,
                TriggerType, EligibleDepositAmount, Status, ClaimedBonusTaskId,
                ExcludedByBonusTaskId, StatusChangedAt, CreatedAt)
            VALUES (@eventId, 10001, 20001, '2026-09-15', 4, 10.0000, @status, @taskId,
                NULL, '2026-09-15 10:00:00.123456', '2026-09-15 10:00:00.123456')
            ", connection, transaction);
        command.Parameters.AddWithValue("@eventId", eventId);
        command.Parameters.AddWithValue("@status", claimedTaskId is null ? 1 : 2);
        command.Parameters.AddWithValue("@taskId", claimedTaskId is null ? DBNull.Value : claimedTaskId);
        command.ExecuteNonQuery();
        return command.LastInsertedId;
    }

    private static void InsertStatus(MySqlConnection connection, MySqlTransaction transaction,
        long userUid, string taskId, long entryId) =>
        Execute(connection, transaction, @"
            INSERT INTO PromotionBonusStatus (UserUID, TaskState, BonusTaskId, EligibilityEntryId, ActivityUID,
                BusinessDay, ActivitySnapshotJson, BonusAmount, RequiredWagerAmount,
                CurrentWagerAmount, ClaimedAt, CreatedAt, UpdatedAt)
            VALUES (@userUid, 1, @taskId, @entryId, 20001, '2026-09-15', '{}',
                10, 300, 0, '2026-09-15 10:00:00.123456',
                '2026-09-15 10:00:00.123456', '2026-09-15 10:00:00.123456')
            ", ("@userUid", userUid), ("@taskId", taskId), ("@entryId", entryId));

    private static void InsertHistory(MySqlConnection connection, MySqlTransaction transaction,
        string taskId, long entryId) =>
        Execute(connection, transaction, @"
            INSERT INTO PromotionBonusHistory (BonusTaskId, UserUID, EligibilityEntryId, ActivityUID,
                BusinessDay, ActivitySnapshotJson, BonusAmount, RequiredWagerAmount,
                CurrentWagerAmount, ConvertedAmount, CloseReason, ClaimedAt, ClosedAt, CreatedAt)
            VALUES (@taskId, 10001, @entryId, 20001, '2026-09-15', '{}', 10, 300, 300,
                5, 1, '2026-09-15 10:00:00.123456', '2026-09-15 10:01:00.123456',
                '2026-09-15 10:01:00.123456')
            ", ("@taskId", taskId), ("@entryId", entryId));
}
