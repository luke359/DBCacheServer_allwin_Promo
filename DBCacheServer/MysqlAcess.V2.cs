using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;

namespace DBCacheServer
{
    public partial class MysqlAcess
    {
        private static readonly AsyncLocal<bool> v2TransactionActive = new AsyncLocal<bool>();

        public T ExecuteParameterizedTransaction<T>(
            Func<MysqlTransactionContext, T> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (v2TransactionActive.Value) throw new InvalidOperationException("Nested V2 transactions are not allowed.");
            if (isolationLevel != IsolationLevel.ReadCommitted && isolationLevel != IsolationLevel.RepeatableRead
                && isolationLevel != IsolationLevel.Serializable)
                throw new ArgumentException("Unsupported isolation level.", nameof(isolationLevel));

            v2TransactionActive.Value = true;
            try
            {
                using var connection = OpenV2Connection();
                using var transaction = connection.BeginTransaction(isolationLevel);
                var context = new MysqlTransactionContext(connection, transaction);
                T result;
                try
                {
                    result = action(context);
                }
                catch
                {
                    context.Invalidate();
                    TryRollback(transaction);
                    throw;
                }

                context.Invalidate();
                try
                {
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    TryRollback(transaction);
                    throw MysqlV2Errors.CommitUnknown(ex);
                }
            }
            catch (MySqlException ex)
            {
                throw MysqlV2Errors.Classify(ex);
            }
            finally
            {
                v2TransactionActive.Value = false;
            }
        }

        public IReadOnlyList<IReadOnlyDictionary<string, object>> SelectParameterizedV2(MysqlSelectCommand query)
        {
            EnsureNotInsideV2Transaction();
            try
            {
                using var connection = new MySqlConnection(connstr);
                using var command = MysqlCommandBuilderV2.Select(connection, null, query);
                connection.Open();
                return MysqlTransactionContext.ReadRows(command);
            }
            catch (MySqlException ex) { throw MysqlV2Errors.Classify(ex); }
        }

        public long DeleteParameterizedV2(MysqlDeleteCommand query)
        {
            EnsureNotInsideV2Transaction();
            try
            {
                using var connection = new MySqlConnection(connstr);
                using var command = MysqlCommandBuilderV2.Delete(connection, null, query);
                connection.Open();
                return command.ExecuteNonQuery();
            }
            catch (MySqlException ex) { throw MysqlV2Errors.Classify(ex); }
        }

        private MySqlConnection OpenV2Connection()
        {
            // V2 never shares the legacy connection or its replaceable Mutex.
            // The provider may use connection pooling, but each operation owns its connection lifetime.
            var connection = new MySqlConnection(connstr);
            try
            {
                connection.Open();
                return connection;
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }

        private static void EnsureNotInsideV2Transaction()
        {
            if (v2TransactionActive.Value)
                throw new InvalidOperationException("Non-transaction V2 API cannot run inside a V2 transaction.");
        }

        private static void TryRollback(MySqlTransaction transaction)
        {
            try { transaction.Rollback(); }
            catch { /* Preserve the original action or commit failure. */ }
        }
    }

    public sealed class MysqlTransactionContext
    {
        private readonly MySqlConnection connection;
        private readonly MySqlTransaction transaction;
        private readonly int ownerThreadId;
        private bool active = true;

        internal MysqlTransactionContext(MySqlConnection connection, MySqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
            ownerThreadId = Environment.CurrentManagedThreadId;
        }

        internal void Invalidate() { active = false; }

        private void EnsureActive()
        {
            if (!active || Environment.CurrentManagedThreadId != ownerThreadId)
                throw new InvalidOperationException("The transaction context is no longer valid on this thread.");
        }

        public MysqlParameterizedWriteResult Insert(string tableName, IReadOnlyDictionary<string, object> data)
        {
            EnsureActive();
            try
            {
                using var command = MysqlCommandBuilderV2.Insert(connection, transaction, tableName, data);
                var count = command.ExecuteNonQuery();
                return new MysqlParameterizedWriteResult(count, command.LastInsertedId);
            }
            catch (MySqlException ex) { throw MysqlV2Errors.Classify(ex); }
        }

        public long Update(string tableName, IReadOnlyDictionary<string, object> data,
            IReadOnlyList<MysqlCondition> where)
        {
            EnsureActive();
            try
            {
                using var command = MysqlCommandBuilderV2.Update(connection, transaction, tableName, data, where);
                return command.ExecuteNonQuery();
            }
            catch (MySqlException ex) { throw MysqlV2Errors.Classify(ex); }
        }

        public IReadOnlyList<IReadOnlyDictionary<string, object>> Select(MysqlSelectCommand query)
        {
            EnsureActive();
            try
            {
                using var command = MysqlCommandBuilderV2.Select(connection, transaction, query);
                return ReadRows(command);
            }
            catch (MySqlException ex) { throw MysqlV2Errors.Classify(ex); }
        }

        public object ExecuteScalar(MysqlScalarCommand query)
        {
            EnsureActive();
            try
            {
                using var command = MysqlCommandBuilderV2.Scalar(connection, transaction, query);
                var value = command.ExecuteScalar();
                return value == DBNull.Value ? null : value;
            }
            catch (MySqlException ex) { throw MysqlV2Errors.Classify(ex); }
        }

        public long Delete(MysqlDeleteCommand query)
        {
            EnsureActive();
            try
            {
                using var command = MysqlCommandBuilderV2.Delete(connection, transaction, query);
                return command.ExecuteNonQuery();
            }
            catch (MySqlException ex) { throw MysqlV2Errors.Classify(ex); }
        }

        internal static IReadOnlyList<IReadOnlyDictionary<string, object>> ReadRows(MySqlCommand command)
        {
            var rows = new List<IReadOnlyDictionary<string, object>>();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>(reader.FieldCount, StringComparer.OrdinalIgnoreCase);
                for (var i = 0; i < reader.FieldCount; i++)
                    row.Add(reader.GetName(i), reader.IsDBNull(i) ? null : reader.GetValue(i));
                rows.Add(row);
            }
            return rows;
        }
    }

    internal static class MysqlV2Errors
    {
        internal static PromotionDataException CommitUnknown(Exception ex) =>
            new PromotionDataException(PromotionDataErrorKind.CommitOutcomeUnknown,
                "The transaction commit outcome is unknown; verify by business identifier.", ex);

        internal static PromotionDataException Classify(MySqlException ex)
        {
            var kind = KindForNumber(ex.Number);
            // Do not expose provider messages: duplicate-key messages can contain submitted values.
            return new PromotionDataException(kind, "MySQL V2 operation failed (error " + ex.Number + ").", ex,
                kind == PromotionDataErrorKind.DuplicateKey ? ConstraintFromDuplicateMessage(ex.Message) : null);
        }

        internal static PromotionDataErrorKind KindForNumber(int number) => number switch
            {
                1062 => PromotionDataErrorKind.DuplicateKey,
                1213 => PromotionDataErrorKind.Deadlock,
                1205 => PromotionDataErrorKind.LockWaitTimeout,
                1042 or 1043 or 2002 or 2003 or 2006 or 2013 => PromotionDataErrorKind.Connection,
                _ => PromotionDataErrorKind.Unexpected
            };

        private static string ConstraintFromDuplicateMessage(string message)
        {
            const string marker = "for key '";
            var start = message.LastIndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (start < 0) return null;
            start += marker.Length;
            var end = message.IndexOf('\'', start);
            if (end <= start) return null;
            var name = message.Substring(start, end - start);
            foreach (var c in name)
                if (!(c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9' || c == '_' || c == '.'))
                    return null;
            return name;
        }
    }
}
