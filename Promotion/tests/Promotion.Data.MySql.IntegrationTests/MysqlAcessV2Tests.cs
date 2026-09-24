using System.Text;
using System.Reflection;
using DBCacheServer;
using MySql.Data.MySqlClient;
using Xunit;

namespace Promotion.Data.MySql.IntegrationTests;

[CollectionDefinition("MysqlAcess V2", DisableParallelization = true)]
public sealed class MysqlAcessV2Collection { }

[Collection("MysqlAcess V2")]
public sealed class MysqlAcessV2Tests
{
    private static readonly Lazy<MysqlAcess> Mysql = new(InitializeMysql);

    [Fact]
    public async Task V2_TransactionCommitRollbackAndContextExpiry()
    {
        var connectionString = TestConnectionString();
        var table = "PromotionV2Test_" + Guid.NewGuid().ToString("N")[..12];
        using var setup = new MySqlConnection(connectionString);
        setup.Open();
        using (var create = new MySqlCommand(
            $"CREATE TABLE `{table}` (Id BIGINT PRIMARY KEY AUTO_INCREMENT, ValueText VARCHAR(80) NULL, UNIQUE KEY UX_ValueText (ValueText)) ENGINE=InnoDB", setup))
            create.ExecuteNonQuery();

        try
        {
            var mysql = Mysql.Value;
            Assert.Throws<ArgumentException>(() => mysql.SelectParameterizedV2(new MysqlSelectCommand(
                table, new[] { "Id" }, Array.Empty<MysqlCondition>(),
                Array.Empty<MysqlOrder>(), 1, null, MysqlLockMode.ForUpdate)));
            Assert.Throws<ArgumentException>(() => mysql.DeleteParameterizedV2(new MysqlDeleteCommand(
                table, Array.Empty<MysqlCondition>(), Array.Empty<MysqlOrder>(), 1)));

            MysqlTransactionContext escaped = null!;
            mysql.ExecuteParameterizedTransaction(context =>
            {
                escaped = context;
                Assert.Throws<InvalidOperationException>(() => mysql.ExecuteParameterizedTransaction(_ => 0));
                Assert.Throws<InvalidOperationException>(() => mysql.SelectParameterizedV2(new MysqlSelectCommand(
                    table, new[] { "Id" }, Array.Empty<MysqlCondition>(), Array.Empty<MysqlOrder>(), 1, null)));
                Assert.Throws<ArgumentException>(() => context.Insert(table,
                    new Dictionary<string, object> { ["ValueText"] = new object() }));
                var inserted = context.Insert(table,
                    new Dictionary<string, object> { ["ValueText"] = "committed" });
                Assert.Equal(1L, inserted.AffectedRows);
                Assert.True(inserted.LastInsertedId > 0);
                Assert.Equal(1L, Convert.ToInt64(context.ExecuteScalar(new MysqlScalarCommand(
                    table, MysqlScalarOperation.Count, null!, Array.Empty<MysqlCondition>()))));
                return 0;
            });

            const string attack = "x' OR 1=1 --";
            mysql.ExecuteParameterizedTransaction(context =>
            {
                context.Insert(table, new Dictionary<string, object> { ["ValueText"] = attack });
                return 0;
            });
            Assert.Single(mysql.SelectParameterizedV2(new MysqlSelectCommand(table, new[] { "Id" },
                new[] { new MysqlCondition("ValueText", MysqlComparisonOperator.Equal, attack) },
                Array.Empty<MysqlOrder>(), 1, null)));
            Assert.Equal(1L, mysql.DeleteParameterizedV2(new MysqlDeleteCommand(table,
                new[] { new MysqlCondition("ValueText", MysqlComparisonOperator.Equal, attack) },
                Array.Empty<MysqlOrder>(), 1)));

            using var barrier = new Barrier(2);
            var concurrent = Enumerable.Range(1, 2).Select(i => Task.Run(() =>
                mysql.ExecuteParameterizedTransaction(context =>
                {
                    Assert.True(barrier.SignalAndWait(TimeSpan.FromSeconds(10)),
                        "V2 transactions should enter concurrently on independent connections.");
                    context.Insert(table, new Dictionary<string, object> { ["ValueText"] = "parallel-" + i });
                    return 0;
                }))).ToArray();
            await Task.WhenAll(concurrent);
            Assert.Equal(2L, mysql.DeleteParameterizedV2(new MysqlDeleteCommand(table,
                new[] { new MysqlCondition("ValueText", MysqlComparisonOperator.In,
                    new[] { "parallel-1", "parallel-2" }) }, Array.Empty<MysqlOrder>(), 10)));
            Assert.Throws<InvalidOperationException>(() => escaped.ExecuteScalar(new MysqlScalarCommand(
                table, MysqlScalarOperation.Count, null!, Array.Empty<MysqlCondition>())));

            var expectedFailure = new InvalidOperationException("rollback sentinel");
            MysqlTransactionContext? rolledBackContext = null;
            var actualFailure = Assert.Throws<InvalidOperationException>(() => mysql.ExecuteParameterizedTransaction<int>(context =>
            {
                rolledBackContext = context;
                context.Insert(table, new Dictionary<string, object> { ["ValueText"] = "rolled-back" });
                throw expectedFailure;
            }));
            Assert.Same(expectedFailure, actualFailure);
            Assert.Throws<InvalidOperationException>(() => rolledBackContext!.Select(new MysqlSelectCommand(
                table, new[] { "Id" }, Array.Empty<MysqlCondition>(), Array.Empty<MysqlOrder>(), 1, null)));

            var duplicate = Assert.Throws<DBCacheServer.PromotionDataException>(() =>
                mysql.ExecuteParameterizedTransaction(context =>
                {
                    context.Insert(table, new Dictionary<string, object> { ["ValueText"] = "committed" });
                    return 0;
                }));
            Assert.Equal(DBCacheServer.PromotionDataErrorKind.DuplicateKey, duplicate.Kind);
            Assert.NotNull(duplicate.ConstraintName);

            var rows = mysql.SelectParameterizedV2(new MysqlSelectCommand(table,
                new[] { "Id", "ValueText" }, Array.Empty<MysqlCondition>(),
                Array.Empty<MysqlOrder>(), 10, null));
            Assert.Single(rows);
            Assert.Equal("committed", rows[0]["ValueText"]);

            mysql.ExecuteParameterizedTransaction(context =>
            {
                var locked = context.Select(new MysqlSelectCommand(table, new[] { "Id" },
                    new[] { new MysqlCondition("ValueText", MysqlComparisonOperator.Equal, "committed") },
                    Array.Empty<MysqlOrder>(), 1, null, MysqlLockMode.ForUpdate));
                Assert.Single(locked);
                Assert.Equal(1L, context.Delete(new MysqlDeleteCommand(table,
                    new[] { new MysqlCondition("Id", MysqlComparisonOperator.Equal, locked[0]["Id"]) },
                    Array.Empty<MysqlOrder>(), 1)));
                return 0;
            });
        }
        finally
        {
            using var drop = new MySqlCommand($"DROP TABLE IF EXISTS `{table}`", setup);
            drop.ExecuteNonQuery();
        }
    }

    [Fact]
    public void DB_009_015_NullAndTypedValuesRoundTripThroughRealMySql()
    {
        var table = "PromotionV2Typed_" + Guid.NewGuid().ToString("N")[..12];
        using var setup = new MySqlConnection(TestConnectionString());
        setup.Open();
        using (var create = new MySqlCommand(
            $"CREATE TABLE `{table}` (Id BIGINT PRIMARY KEY AUTO_INCREMENT, NullableText VARCHAR(80) NULL, " +
            "NullableNumber INT NULL, Amount DECIMAL(20,4) NOT NULL, EventDate DATE NOT NULL, " +
            "EventTime DATETIME(6) NOT NULL, GuidText CHAR(36) NOT NULL, Flag TINYINT(1) NOT NULL) ENGINE=InnoDB", setup))
            create.ExecuteNonQuery();
        try
        {
            var guid = Guid.Parse("AAAAAAAA-AAAA-4AAA-8AAA-AAAAAAAAAAAA");
            var date = new DateOnly(2026, 9, 16);
            var time = new DateTime(2026, 9, 16, 19, 0, 0).AddTicks(1_234_560);
            var inserted = Mysql.Value.ExecuteParameterizedTransaction(context => context.Insert(table,
                new Dictionary<string, object>
                {
                    ["NullableText"] = null!, ["NullableNumber"] = null!,
                    ["Amount"] = 123.4567m, ["EventDate"] = date,
                    ["EventTime"] = time, ["GuidText"] = guid, ["Flag"] = true
                }));
            Assert.True(inserted.LastInsertedId > 0);
            var rows = Mysql.Value.SelectParameterizedV2(new MysqlSelectCommand(table,
                new[] { "NullableText", "NullableNumber", "Amount", "EventDate", "EventTime", "GuidText", "Flag" },
                new[] { new MysqlCondition("Id", MysqlComparisonOperator.Equal, inserted.LastInsertedId) },
                Array.Empty<MysqlOrder>(), 1, null));
            var row = Assert.Single(rows);
            Assert.Null(row["NullableText"]);
            Assert.Null(row["NullableNumber"]);
            Assert.Equal(123.4567m, Assert.IsType<decimal>(row["Amount"]));
            Assert.Equal(date.ToDateTime(TimeOnly.MinValue), Assert.IsType<DateTime>(row["EventDate"]));
            Assert.Equal(time, Assert.IsType<DateTime>(row["EventTime"]));
            Assert.Equal(guid.ToString("D").ToLowerInvariant(), row["GuidText"]?.ToString());
            Assert.True(Convert.ToBoolean(row["Flag"]));
        }
        finally
        {
            using var drop = new MySqlCommand($"DROP TABLE IF EXISTS `{table}`", setup);
            drop.ExecuteNonQuery();
        }
    }

    [Fact]
    public void DB_018_020_TransactionVisibilityAndThreadOwnership()
    {
        var table = "PromotionV2Isolation_" + Guid.NewGuid().ToString("N")[..12];
        using var observer = new MySqlConnection(TestConnectionString());
        observer.Open();
        using (var create = new MySqlCommand(
            $"CREATE TABLE `{table}` (Id BIGINT PRIMARY KEY AUTO_INCREMENT, ValueText VARCHAR(80) NOT NULL) ENGINE=InnoDB", observer))
            create.ExecuteNonQuery();
        try
        {
            var mysql = Mysql.Value;
            mysql.ExecuteParameterizedTransaction(context =>
            {
                context.Insert(table, new Dictionary<string, object> { ["ValueText"] = "pending" });
                Assert.Equal(1L, Convert.ToInt64(context.ExecuteScalar(new MysqlScalarCommand(
                    table, MysqlScalarOperation.Count, null!, Array.Empty<MysqlCondition>()))));
                using (var count = new MySqlCommand($"SELECT COUNT(*) FROM `{table}`", observer))
                    Assert.Equal(0L, Convert.ToInt64(count.ExecuteScalar()));

                Exception? crossThread = null;
                var thread = new Thread(() =>
                {
                    try { context.Select(new MysqlSelectCommand(table, new[] { "Id" },
                        Array.Empty<MysqlCondition>(), Array.Empty<MysqlOrder>(), 1, null)); }
                    catch (Exception ex) { crossThread = ex; }
                });
                thread.Start();
                thread.Join();
                Assert.IsType<InvalidOperationException>(crossThread);

                Assert.Equal(1L, context.Update(table,
                    new Dictionary<string, object> { ["ValueText"] = "committed" },
                    new[] { new MysqlCondition("ValueText", MysqlComparisonOperator.Equal, "pending") }));
                return 0;
            });
            using var committed = new MySqlCommand($"SELECT COUNT(*) FROM `{table}` WHERE ValueText='committed'", observer);
            Assert.Equal(1L, Convert.ToInt64(committed.ExecuteScalar()));
        }
        finally
        {
            using var drop = new MySqlCommand($"DROP TABLE IF EXISTS `{table}`", observer);
            drop.ExecuteNonQuery();
        }
    }

    [Fact]
    public void DB_028_ConnectionLostAtCommitIsOutcomeUnknown()
    {
        var table = "PromotionV2Commit_" + Guid.NewGuid().ToString("N")[..12];
        using var observer = new MySqlConnection(TestConnectionString());
        observer.Open();
        using (var create = new MySqlCommand(
            $"CREATE TABLE `{table}` (Id BIGINT PRIMARY KEY AUTO_INCREMENT, ValueText VARCHAR(80) NOT NULL) ENGINE=InnoDB", observer))
            create.ExecuteNonQuery();
        try
        {
            var failure = Assert.Throws<DBCacheServer.PromotionDataException>(() =>
                Mysql.Value.ExecuteParameterizedTransaction(context =>
                {
                    context.Insert(table, new Dictionary<string, object> { ["ValueText"] = "pending" });
                    var field = typeof(MysqlTransactionContext).GetField("connection",
                        BindingFlags.Instance | BindingFlags.NonPublic)
                        ?? throw new InvalidOperationException("V2 test cannot inspect its own connection.");
                    var ownedConnection = (MySqlConnection)field.GetValue(context)!;
                    var transactionField = typeof(MysqlTransactionContext).GetField("transaction",
                        BindingFlags.Instance | BindingFlags.NonPublic)
                        ?? throw new InvalidOperationException("V2 test cannot inspect its own transaction.");
                    var ownedTransaction = (MySqlTransaction)transactionField.GetValue(context)!;
                    long connectionId;
                    using (var idCommand = new MySqlCommand("SELECT CONNECTION_ID()", ownedConnection,
                        ownedTransaction))
                        connectionId = Convert.ToInt64(idCommand.ExecuteScalar());
                    Assert.True(connectionId > 0);
                    using var killer = new MySqlConnection(TestConnectionString());
                    killer.Open();
                    using var kill = new MySqlCommand($"KILL CONNECTION {connectionId}", killer);
                    kill.ExecuteNonQuery();
                    return 0;
                }));
            Assert.Equal(DBCacheServer.PromotionDataErrorKind.CommitOutcomeUnknown, failure.Kind);
            using var count = new MySqlCommand($"SELECT COUNT(*) FROM `{table}`", observer);
            Assert.Equal(0L, Convert.ToInt64(count.ExecuteScalar()));
        }
        finally
        {
            using var drop = new MySqlCommand($"DROP TABLE IF EXISTS `{table}`", observer);
            drop.ExecuteNonQuery();
        }
    }

    [Fact]
    public void DB_028_RealLockWaitTimeoutIsClassified()
    {
        var table = "PromotionV2Lock_" + Guid.NewGuid().ToString("N")[..12];
        using var observer = new MySqlConnection(TestConnectionString());
        observer.Open();
        using (var create = new MySqlCommand(
            $"CREATE TABLE `{table}` (Id BIGINT PRIMARY KEY, ValueText VARCHAR(80) NOT NULL) ENGINE=InnoDB", observer))
            create.ExecuteNonQuery();
        try
        {
            using (var seed = new MySqlCommand($"INSERT INTO `{table}` (Id,ValueText) VALUES (1,'seed')", observer))
                seed.ExecuteNonQuery();
            using var lockTransaction = observer.BeginTransaction();
            try
            {
                using (var hold = new MySqlCommand($"UPDATE `{table}` SET ValueText='held' WHERE Id=1", observer,
                    lockTransaction)) hold.ExecuteNonQuery();
                var failure = Assert.Throws<DBCacheServer.PromotionDataException>(() =>
                    Mysql.Value.ExecuteParameterizedTransaction(context =>
                    {
                        var connection = (MySqlConnection)typeof(MysqlTransactionContext)
                            .GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic)!
                            .GetValue(context)!;
                        var transaction = (MySqlTransaction)typeof(MysqlTransactionContext)
                            .GetField("transaction", BindingFlags.Instance | BindingFlags.NonPublic)!
                            .GetValue(context)!;
                        using (var timeout = new MySqlCommand("SET SESSION innodb_lock_wait_timeout = 1",
                            connection, transaction)) timeout.ExecuteNonQuery();
                        context.Update(table, new Dictionary<string, object> { ["ValueText"] = "blocked" },
                            new[] { new MysqlCondition("Id", MysqlComparisonOperator.Equal, 1) });
                        return 0;
                    }));
                Assert.Equal(DBCacheServer.PromotionDataErrorKind.LockWaitTimeout, failure.Kind);
            }
            finally { lockTransaction.Rollback(); }
        }
        finally
        {
            using var drop = new MySqlCommand($"DROP TABLE IF EXISTS `{table}`", observer);
            drop.ExecuteNonQuery();
        }
    }

    private static MysqlAcess InitializeMysql()
    {
        var directory = Path.Combine(Path.GetTempPath(), "promotion-v2-" + Guid.NewGuid().ToString("N"));
        var path = Path.Combine(directory, "SqlConnection.txt");
        var prior = Environment.CurrentDirectory;
        var connection = new MySqlConnectionStringBuilder(TestConnectionString());
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

    private static string TestConnectionString()
    {
        var text = Environment.GetEnvironmentVariable("PROMOTION_TEST_MYSQL_CONNECTION_STRING")
            ?? throw new InvalidOperationException("PROMOTION_TEST_MYSQL_CONNECTION_STRING is required.");
        var builder = new MySqlConnectionStringBuilder(text);
        if (!builder.Database.StartsWith("promotion_test_", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("V2 tests require a promotion_test_ scratch database.");
        if (!string.Equals(builder.Server, "localhost", StringComparison.OrdinalIgnoreCase)
            && builder.Server != "127.0.0.1" && builder.Server != "::1")
            throw new InvalidOperationException("V2 tests require a loopback MySQL server.");
        if (builder.ConnectionString.Contains(':'))
            throw new InvalidOperationException("V2 test configuration format does not support ':' in the connection string.");
        return builder.ConnectionString;
    }
}
