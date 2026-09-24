namespace Promotion.Data.MySql;

public enum Comparison : byte { Equal = 1, NotEqual = 2, LessThan = 3, LessThanOrEqual = 4, GreaterThan = 5, GreaterThanOrEqual = 6, In = 7, IsNull = 8, IsNotNull = 9 }
public sealed record Condition(string Column, Comparison Operator, object? Value);
public sealed record Sort(string Column, bool Descending = false);
public sealed record Query(string Table, IReadOnlyList<string> Fields, IReadOnlyList<Condition> Where,
    IReadOnlyList<Sort> Order, int? Limit = null, int? Offset = null, bool ForUpdate = false);
public sealed record DeleteQuery(string Table, IReadOnlyList<Condition> Where, IReadOnlyList<Sort> Order, int Limit);
public sealed record InsertResult(long AffectedRows, long LastInsertedId);

public interface IPromotionV2Gateway
{
    T Transaction<T>(Func<IPromotionV2Transaction, T> action);
    IReadOnlyList<IReadOnlyDictionary<string, object?>> Select(Query query);
    long Delete(DeleteQuery query);
}

public interface IPromotionV2Transaction
{
    IReadOnlyList<IReadOnlyDictionary<string, object?>> Select(Query query);
    InsertResult Insert(string table, IReadOnlyDictionary<string, object?> values);
    long Update(string table, IReadOnlyDictionary<string, object?> values, IReadOnlyList<Condition> where);
    long Count(string table, IReadOnlyList<Condition> where);
    long Delete(DeleteQuery query);
}
