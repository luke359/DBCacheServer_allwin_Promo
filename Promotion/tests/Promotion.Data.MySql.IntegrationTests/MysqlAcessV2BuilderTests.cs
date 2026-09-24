using System.Data;
using DBCacheServer;
using MySql.Data.MySqlClient;
using Xunit;

namespace Promotion.Data.MySql.IntegrationTests;

public sealed class MysqlAcessV2BuilderTests
{
    [Fact]
    public void DB_009_013_ValuesIncludingNullAreParameters()
    {
        using var connection = new MySqlConnection();
        var attack = "x' OR 1=1 --";
        using var select = MysqlCommandBuilderV2.Select(connection, null, new MysqlSelectCommand(
            "PromotionActivity", new[] { "ActivityUID" },
            new[] { new MysqlCondition("ActivityInfo", MysqlComparisonOperator.Equal, attack) },
            new[] { new MysqlOrder("ActivityUID", MysqlSortDirection.Descending) }, 10, 2));
        Assert.DoesNotContain(attack, select.CommandText, StringComparison.Ordinal);
        Assert.Contains("`ActivityUID` DESC", select.CommandText, StringComparison.Ordinal);
        Assert.Contains("LIMIT @p1 OFFSET @p2", select.CommandText, StringComparison.Ordinal);
        Assert.Equal(attack, select.Parameters[0].Value);
        Assert.Equal(10, select.Parameters[1].Value);
        Assert.Equal(2, select.Parameters[2].Value);

        using var insert = MysqlCommandBuilderV2.Insert(connection, null, "PromotionActivity",
            new Dictionary<string, object> { ["ActivityInfo"] = null! });
        Assert.Equal(DBNull.Value, insert.Parameters[0].Value);
    }

    [Fact]
    public void DB_014_015_016_017_InvalidQueryShapesAreRejected()
    {
        using var connection = new MySqlConnection();
        var emptyWhere = Array.Empty<MysqlCondition>();
        var noOrder = Array.Empty<MysqlOrder>();
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity;DROP", new[] { "ActivityUID" }, emptyWhere, noOrder, 1, null)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID`" }, emptyWhere, noOrder, 1, null)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" }, emptyWhere,
                new[] { new MysqlOrder("ActivityUID --", MysqlSortDirection.Ascending) }, 1, null)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" }, emptyWhere, noOrder,
                1, null, MysqlLockMode.ForUpdate)));
        Assert.Throws<ArgumentOutOfRangeException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" }, emptyWhere, noOrder, 0, null)));
        Assert.Throws<ArgumentOutOfRangeException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" }, emptyWhere, noOrder, 1001, null)));
        Assert.Throws<ArgumentOutOfRangeException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" }, emptyWhere, noOrder, 1, -1)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" }, emptyWhere, noOrder, null, 1)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Update(connection, null,
            "PromotionActivity", new Dictionary<string, object> { ["ActivityInfo"] = "x" }, emptyWhere));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Delete(connection, null,
            new MysqlDeleteCommand("PromotionActivity", emptyWhere, noOrder, 1)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" },
                new[] { new MysqlCondition("ActivityUID", MysqlComparisonOperator.In, Array.Empty<int>()) },
                noOrder, 1, null)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" },
                new[] { new MysqlCondition("ActivityUID", MysqlComparisonOperator.In,
                    Enumerable.Range(1, 2).Where(x => x > 0)) }, noOrder, 1, null)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" },
                new[] { new MysqlCondition("ActivityUID", MysqlComparisonOperator.Equal, null) },
                noOrder, 1, null)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" },
                new[] { new MysqlCondition("ActivityUID", MysqlComparisonOperator.IsNull, 1) },
                noOrder, 1, null)));
        Assert.Throws<ArgumentException>(() => MysqlCommandBuilderV2.Select(connection, null,
            new MysqlSelectCommand("PromotionActivity", new[] { "ActivityUID" },
                new[] { new MysqlCondition("ActivityUID", MysqlComparisonOperator.Equal, new object()) },
                noOrder, 1, null)));
    }

    [Fact]
    public void DB_015_TypedValuesKeepDatabaseTypes()
    {
        using var connection = new MySqlConnection();
        var guid = Guid.Parse("11111111-1111-4111-8111-111111111111");
        var date = new DateOnly(2026, 9, 16);
        using var command = MysqlCommandBuilderV2.Insert(connection, null, "PromotionActivity",
            new Dictionary<string, object>
            {
                ["ByteValue"] = (byte)1, ["ShortValue"] = (short)2,
                ["IntValue"] = 3, ["LongValue"] = 4L, ["BoolValue"] = true,
                ["DecimalValue"] = 1.2345m, ["DoubleValue"] = 2.5d,
                ["DateValue"] = date, ["TimeValue"] = new DateTime(2026, 9, 16, 12, 0, 0),
                ["GuidValue"] = guid
            });
        Assert.Equal(MySqlDbType.Decimal, command.Parameters[5].MySqlDbType);
        Assert.Equal(1.2345m, command.Parameters[5].Value);
        Assert.Equal(MySqlDbType.Date, command.Parameters[7].MySqlDbType);
        Assert.Equal(date.ToDateTime(TimeOnly.MinValue), command.Parameters[7].Value);
        Assert.Equal(guid.ToString("D"), command.Parameters[9].Value);
    }

    [Theory]
    [InlineData(1062, DBCacheServer.PromotionDataErrorKind.DuplicateKey)]
    [InlineData(1213, DBCacheServer.PromotionDataErrorKind.Deadlock)]
    [InlineData(1205, DBCacheServer.PromotionDataErrorKind.LockWaitTimeout)]
    [InlineData(2006, DBCacheServer.PromotionDataErrorKind.Connection)]
    [InlineData(1146, DBCacheServer.PromotionDataErrorKind.Unexpected)]
    public void DB_028_MySqlNumberHasStableKind(int number, DBCacheServer.PromotionDataErrorKind expected) =>
        Assert.Equal(expected, MysqlV2Errors.KindForNumber(number));

    [Fact]
    public void DB_028_CommitFailureIsAlwaysOutcomeUnknown()
    {
        var original = new IOException("commit connection lost");
        var classified = MysqlV2Errors.CommitUnknown(original);
        Assert.Equal(DBCacheServer.PromotionDataErrorKind.CommitOutcomeUnknown, classified.Kind);
        Assert.Same(original, classified.InnerException);
        Assert.DoesNotContain("connection lost", classified.Message, StringComparison.Ordinal);
    }
}
