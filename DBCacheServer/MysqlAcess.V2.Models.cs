using System;
using System.Collections.Generic;
using System.Data;

namespace DBCacheServer
{
    public enum MysqlComparisonOperator : byte
    {
        Equal = 1, NotEqual = 2, LessThan = 3, LessThanOrEqual = 4,
        GreaterThan = 5, GreaterThanOrEqual = 6, In = 7, IsNull = 8, IsNotNull = 9
    }

    public sealed record MysqlCondition(string ColumnName, MysqlComparisonOperator Operator, object Value);

    public enum MysqlSortDirection : byte { Ascending = 1, Descending = 2 }
    public sealed record MysqlOrder(string ColumnName, MysqlSortDirection Direction);
    public enum MysqlLockMode : byte { None = 0, ForUpdate = 1 }

    public sealed record MysqlSelectCommand(
        string TableName, IReadOnlyList<string> Fields, IReadOnlyList<MysqlCondition> Where,
        IReadOnlyList<MysqlOrder> OrderBy, int? Limit, int? Offset,
        MysqlLockMode LockMode = MysqlLockMode.None);

    public enum MysqlScalarOperation : byte { Count = 1 }
    public sealed record MysqlScalarCommand(
        string TableName, MysqlScalarOperation Operation, string Field,
        IReadOnlyList<MysqlCondition> Where);

    public sealed record MysqlDeleteCommand(
        string TableName, IReadOnlyList<MysqlCondition> Where,
        IReadOnlyList<MysqlOrder> OrderBy, int? Limit);

    public sealed record MysqlParameterizedWriteResult(long AffectedRows, long LastInsertedId);

    public enum PromotionDataErrorKind : byte
    {
        Validation = 1, DuplicateKey = 2, Deadlock = 3, LockWaitTimeout = 4,
        Connection = 5, CommitOutcomeUnknown = 6, DataCorruption = 7, ConcurrencyConflict = 8,
        Unexpected = 255
    }

    public sealed class PromotionDataException : Exception
    {
        public PromotionDataErrorKind Kind { get; }
        public string ConstraintName { get; }

        public PromotionDataException(
            PromotionDataErrorKind kind, string message, Exception innerException = null,
            string constraintName = null)
            : base(message, innerException)
        {
            Kind = kind;
            ConstraintName = constraintName;
        }
    }
}
