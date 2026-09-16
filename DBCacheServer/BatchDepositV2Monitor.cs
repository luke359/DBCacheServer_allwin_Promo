using MySql.Data.MySqlClient;
using Protocol;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DBCacheServer
{
    internal static class BatchDepositV2Metrics
    {
        private static long requests;
        private static long succeeded;
        private static long failed;
        private static long idempotencyReplays;

        internal static void Record(BatchDepositV2Result result, bool committed)
        {
            Interlocked.Increment(ref requests);
            if (result.Status == BatchDepositV2Status.Succeeded)
            {
                Interlocked.Increment(ref succeeded);
                if (!committed) Interlocked.Increment(ref idempotencyReplays);
            }
            else
                Interlocked.Increment(ref failed);
        }

        internal static string Snapshot()
        {
            return "requests=" + Interlocked.Read(ref requests) +
                ", succeeded=" + Interlocked.Read(ref succeeded) +
                ", failed=" + Interlocked.Read(ref failed) +
                ", idempotencyReplays=" + Interlocked.Read(ref idempotencyReplays);
        }
    }

    // Emits a compact health line once a minute so gray-release alerts can consume
    // process metrics together with current database and in-memory cache state.
    internal sealed class BatchDepositV2Monitor
    {
        private readonly MysqlAcess mysql;
        private readonly CacheManeger cache;
        private readonly BatchDepositV2OperationsOptions options;

        internal BatchDepositV2Monitor(MysqlAcess mysql, CacheManeger cache)
            : this(mysql, cache, new BatchDepositV2OperationsOptions())
        {
        }

        internal BatchDepositV2Monitor(MysqlAcess mysql, CacheManeger cache, BatchDepositV2OperationsOptions options)
        {
            this.mysql = mysql;
            this.cache = cache;
            this.options = options;
        }

        internal void Run()
        {
            while (true)
            {
                try
                {
                    MonitorSnapshot snapshot = ReadSnapshot();
                    string severity =
                        snapshot.FailedDeliveries > 0 ||
                        snapshot.DbCacheBalanceDifferences > 0 ||
                        snapshot.DuplicateKeys > 0 ||
                        snapshot.OldestPendingSeconds >= options.PendingCriticalSeconds ? "critical" :
                        snapshot.HighRetryDeliveries > 0 ||
                        snapshot.OldestPendingSeconds >= options.PendingWarningSeconds ? "warning" : "info";

                    BatchDepositV2OperationalLog.Write(severity, "monitor", "health_snapshot", new
                    {
                        processMetrics = BatchDepositV2Metrics.Snapshot(),
                        snapshot.PendingDeliveries,
                        snapshot.OldestPendingSeconds,
                        snapshot.HighRetryDeliveries,
                        snapshot.FailedDeliveries,
                        snapshot.DbCacheBalanceDifferences,
                        snapshot.DuplicateKeys,
                        thresholds = new
                        {
                            options.PendingWarningSeconds,
                            options.PendingCriticalSeconds,
                            options.RetryWarningAttempts
                        }
                    });
                }
                catch (Exception exception)
                {
                    BatchDepositV2OperationalLog.Write("critical", "monitor", "monitor_failed",
                        new { error = exception.Message });
                }

                Thread.Sleep(TimeSpan.FromSeconds(options.MonitorIntervalSeconds));
            }
        }

        private MonitorSnapshot ReadSnapshot()
        {
            return mysql.ExecuteInReadCommitted(connection =>
            {
                var dbBalances = new Dictionary<int, decimal>();
                using (var command = new MySqlCommand(
                    "SELECT DISTINCT u.UserUID, u.UserBalance FROM BatchDepositDetailV2 d " +
                    "INNER JOIN BatchDepositV2 b ON b.BatchId = d.BatchId " +
                    "INNER JOIN Usertable u ON u.UserUID = d.UserUID " +
                    "WHERE b.CreatedAtUtc >= DATE_SUB(UTC_TIMESTAMP(6), INTERVAL 5 MINUTE)", connection))
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) dbBalances[reader.GetInt32(0)] = Convert.ToDecimal(reader.GetValue(1));

                return new MonitorSnapshot
                {
                    PendingDeliveries = ScalarInt(connection, "SELECT COUNT(*) FROM BatchDepositOutboxDeliveryV2 WHERE Status = 'Pending'"),
                    OldestPendingSeconds = ScalarInt(connection, "SELECT COALESCE(MAX(TIMESTAMPDIFF(SECOND, CreatedAtUtc, UTC_TIMESTAMP(6))), 0) FROM BatchDepositOutboxDeliveryV2 WHERE Status = 'Pending'"),
                    HighRetryDeliveries = ScalarInt(connection, "SELECT COUNT(*) FROM BatchDepositOutboxDeliveryV2 WHERE Status = 'Pending' AND AttemptCount >= " + options.RetryWarningAttempts),
                    FailedDeliveries = ScalarInt(connection, "SELECT COUNT(*) FROM BatchDepositOutboxDeliveryV2 WHERE Status = 'Failed'"),
                    DuplicateKeys = ScalarInt(connection, "SELECT COUNT(*) FROM (SELECT ActorType, ActorId, IdempotencyKey FROM BatchDepositV2 GROUP BY ActorType, ActorId, IdempotencyKey HAVING COUNT(*) > 1) duplicateKeys"),
                    DbCacheBalanceDifferences = cache.CountBatchDepositV2BalanceDifferences(dbBalances)
                };
            });
        }

        private static int ScalarInt(MySqlConnection connection, string sql)
        {
            using (var command = new MySqlCommand(sql, connection))
                return Convert.ToInt32(command.ExecuteScalar());
        }

        private sealed class MonitorSnapshot
        {
            internal int PendingDeliveries;
            internal int OldestPendingSeconds;
            internal int HighRetryDeliveries;
            internal int FailedDeliveries;
            internal int DuplicateKeys;
            internal int DbCacheBalanceDifferences;
        }
    }
}
