using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DBCacheServer
{
    internal static class MysqlCommandBuilderV2
    {
        private const int MaxLimit = 1000;

        internal static MySqlCommand Insert(MySqlConnection connection, MySqlTransaction transaction,
            string tableName, IReadOnlyDictionary<string, object> data)
        {
            if (data == null || data.Count == 0) throw new ArgumentException("Insert data is required.", nameof(data));
            var command = NewCommand(connection, transaction);
            try
            {
                var columns = new List<string>();
                var values = new List<string>();
                foreach (var item in data)
                {
                    columns.Add(Identifier(item.Key));
                    values.Add(AddValue(command, item.Value));
                }
                command.CommandText = "INSERT INTO " + Identifier(tableName) + " (" + string.Join(",", columns)
                    + ") VALUES (" + string.Join(",", values) + ")";
                return command;
            }
            catch { command.Dispose(); throw; }
        }

        internal static MySqlCommand Update(MySqlConnection connection, MySqlTransaction transaction,
            string tableName, IReadOnlyDictionary<string, object> data, IReadOnlyList<MysqlCondition> where)
        {
            if (data == null || data.Count == 0) throw new ArgumentException("Update data is required.", nameof(data));
            RequireWhere(where);
            var command = NewCommand(connection, transaction);
            try
            {
                var assignments = new List<string>();
                foreach (var item in data)
                    assignments.Add(Identifier(item.Key) + "=" + AddValue(command, item.Value));
                command.CommandText = "UPDATE " + Identifier(tableName) + " SET "
                    + string.Join(",", assignments) + " WHERE " + Conditions(command, where);
                return command;
            }
            catch { command.Dispose(); throw; }
        }

        internal static MySqlCommand Select(MySqlConnection connection, MySqlTransaction transaction,
            MysqlSelectCommand query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (transaction == null && query.LockMode != MysqlLockMode.None)
                throw new ArgumentException("FOR UPDATE requires a transaction.", nameof(query));
            if (query.LockMode != MysqlLockMode.None && query.LockMode != MysqlLockMode.ForUpdate)
                throw new ArgumentException("Unknown lock mode.", nameof(query));
            var command = NewCommand(connection, transaction);
            try
            {
                var fields = new List<string>();
                if (query.Fields != null)
                    foreach (var field in query.Fields) fields.Add(Identifier(field));
                var sql = new StringBuilder("SELECT ").Append(fields.Count == 0 ? "*" : string.Join(",", fields))
                    .Append(" FROM ").Append(Identifier(query.TableName));
                AppendWhere(sql, command, query.Where);
                AppendOrder(sql, query.OrderBy);
                AppendLimit(sql, command, query.Limit, query.Offset);
                if (query.LockMode == MysqlLockMode.ForUpdate) sql.Append(" FOR UPDATE");
                command.CommandText = sql.ToString();
                return command;
            }
            catch { command.Dispose(); throw; }
        }

        internal static MySqlCommand Scalar(MySqlConnection connection, MySqlTransaction transaction,
            MysqlScalarCommand query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (query.Operation != MysqlScalarOperation.Count)
                throw new ArgumentException("Unknown scalar operation.", nameof(query));
            var command = NewCommand(connection, transaction);
            try
            {
                var sql = new StringBuilder("SELECT COUNT(")
                    .Append(query.Field == null ? "*" : Identifier(query.Field)).Append(") FROM ")
                    .Append(Identifier(query.TableName));
                AppendWhere(sql, command, query.Where);
                command.CommandText = sql.ToString();
                return command;
            }
            catch { command.Dispose(); throw; }
        }

        internal static MySqlCommand Delete(MySqlConnection connection, MySqlTransaction transaction,
            MysqlDeleteCommand query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            RequireWhere(query.Where);
            var command = NewCommand(connection, transaction);
            try
            {
                var sql = new StringBuilder("DELETE FROM ").Append(Identifier(query.TableName));
                AppendWhere(sql, command, query.Where);
                AppendOrder(sql, query.OrderBy);
                AppendLimit(sql, command, query.Limit, null);
                command.CommandText = sql.ToString();
                return command;
            }
            catch { command.Dispose(); throw; }
        }

        private static MySqlCommand NewCommand(MySqlConnection connection, MySqlTransaction transaction)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;
            return command;
        }

        private static string Identifier(string name)
        {
            if (string.IsNullOrEmpty(name) || !(char.IsLetter(name[0]) && name[0] <= 127 || name[0] == '_'))
                throw new ArgumentException("Invalid SQL identifier.");
            foreach (var c in name)
                if (!(c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9' || c == '_'))
                    throw new ArgumentException("Invalid SQL identifier.");
            return "`" + name + "`";
        }

        private static void RequireWhere(IReadOnlyList<MysqlCondition> where)
        {
            if (where == null || where.Count == 0)
                throw new ArgumentException("A non-empty WHERE clause is required.", nameof(where));
        }

        private static void AppendWhere(StringBuilder sql, MySqlCommand command, IReadOnlyList<MysqlCondition> where)
        {
            if (where != null && where.Count > 0) sql.Append(" WHERE ").Append(Conditions(command, where));
        }

        private static string Conditions(MySqlCommand command, IReadOnlyList<MysqlCondition> where)
        {
            var parts = new List<string>();
            foreach (var item in where)
            {
                if (item == null) throw new ArgumentException("Condition cannot be null.");
                var field = Identifier(item.ColumnName);
                switch (item.Operator)
                {
                    case MysqlComparisonOperator.IsNull:
                    case MysqlComparisonOperator.IsNotNull:
                        if (item.Value != null) throw new ArgumentException("NULL operator must have a null value.");
                        parts.Add(field + (item.Operator == MysqlComparisonOperator.IsNull ? " IS NULL" : " IS NOT NULL"));
                        break;
                    case MysqlComparisonOperator.In:
                        if (item.Value is string || item.Value is not IEnumerable values ||
                            !item.Value.GetType().GetInterfaces().Any(i =>
                                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>)))
                            throw new ArgumentException("IN requires a non-empty collection.");
                        var names = new List<string>();
                        foreach (var value in values) names.Add(AddValue(command, value));
                        if (names.Count == 0) throw new ArgumentException("IN requires a non-empty collection.");
                        parts.Add(field + " IN (" + string.Join(",", names) + ")");
                        break;
                    default:
                        if (item.Value == null) throw new ArgumentException("Use IS NULL for null comparisons.");
                        var op = item.Operator switch
                        {
                            MysqlComparisonOperator.Equal => "=",
                            MysqlComparisonOperator.NotEqual => "<>",
                            MysqlComparisonOperator.LessThan => "<",
                            MysqlComparisonOperator.LessThanOrEqual => "<=",
                            MysqlComparisonOperator.GreaterThan => ">",
                            MysqlComparisonOperator.GreaterThanOrEqual => ">=",
                            _ => throw new ArgumentException("Unknown comparison operator.")
                        };
                        parts.Add(field + op + AddValue(command, item.Value));
                        break;
                }
            }
            return string.Join(" AND ", parts);
        }

        private static void AppendOrder(StringBuilder sql, IReadOnlyList<MysqlOrder> order)
        {
            if (order == null || order.Count == 0) return;
            var parts = new List<string>();
            foreach (var item in order)
            {
                if (item == null) throw new ArgumentException("Order cannot be null.");
                var direction = item.Direction switch
                {
                    MysqlSortDirection.Ascending => " ASC",
                    MysqlSortDirection.Descending => " DESC",
                    _ => throw new ArgumentException("Unknown sort direction.")
                };
                parts.Add(Identifier(item.ColumnName) + direction);
            }
            sql.Append(" ORDER BY ").Append(string.Join(",", parts));
        }

        private static void AppendLimit(StringBuilder sql, MySqlCommand command, int? limit, int? offset)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset.HasValue && !limit.HasValue) throw new ArgumentException("Offset requires Limit.");
            if (!limit.HasValue) return;
            if (limit <= 0 || limit > MaxLimit) throw new ArgumentOutOfRangeException(nameof(limit));
            sql.Append(" LIMIT ").Append(AddValue(command, limit.Value));
            if (offset.HasValue) sql.Append(" OFFSET ").Append(AddValue(command, offset.Value));
        }

        private static string AddValue(MySqlCommand command, object value)
        {
            var name = "@p" + command.Parameters.Count;
            var parameter = command.Parameters.Add(name, MySqlDbType.VarString);
            switch (value)
            {
                case null: parameter.Value = DBNull.Value; break;
                case string s: parameter.Value = s; break;
                case byte b: parameter.MySqlDbType = MySqlDbType.UByte; parameter.Value = b; break;
                case short s: parameter.MySqlDbType = MySqlDbType.Int16; parameter.Value = s; break;
                case int i: parameter.MySqlDbType = MySqlDbType.Int32; parameter.Value = i; break;
                case long l: parameter.MySqlDbType = MySqlDbType.Int64; parameter.Value = l; break;
                case bool b: parameter.MySqlDbType = MySqlDbType.Bit; parameter.Value = b; break;
                case decimal d: parameter.MySqlDbType = MySqlDbType.Decimal; parameter.Value = d; break;
                case double d: parameter.MySqlDbType = MySqlDbType.Double; parameter.Value = d; break;
                case DateTime dt: parameter.MySqlDbType = MySqlDbType.DateTime; parameter.Value = dt; break;
                case DateOnly date: parameter.MySqlDbType = MySqlDbType.Date; parameter.Value = date.ToDateTime(TimeOnly.MinValue); break;
                case Guid guid: parameter.Value = guid.ToString("D").ToLowerInvariant(); break;
                default: throw new ArgumentException("Unsupported V2 parameter type: " + value.GetType().Name);
            }
            return name;
        }
    }
}
