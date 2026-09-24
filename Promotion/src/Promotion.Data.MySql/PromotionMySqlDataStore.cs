using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;

namespace Promotion.Data.MySql;

public sealed class PromotionMySqlDataStore : IPromotionDataStore
{
    private readonly IPromotionV2Gateway gateway;
    public PromotionMySqlDataStore(IPromotionV2Gateway gateway) => this.gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    internal static Condition Eq(string column, object value) => new(column, Comparison.Equal, value);
    internal static Query Query(string table, IReadOnlyList<string> fields, IReadOnlyList<Condition>? where = null,
        IReadOnlyList<Sort>? order = null, int? limit = null, int? offset = null, bool forUpdate = false)
    {
        var query = new Query(table, fields, where ?? Array.Empty<Condition>(), order ?? Array.Empty<Sort>(), limit, offset, forUpdate);
        PromotionTableMetadata.Validate(query);
        return query;
    }
    private IReadOnlyList<IReadOnlyDictionary<string, object?>> Select(Query query) => gateway.Select(query);
    internal static T? One<T>(IReadOnlyList<IReadOnlyDictionary<string, object?>> rows, Func<IReadOnlyDictionary<string, object?>, T> map)
        where T : class => rows.Count switch { 0 => null, 1 => map(rows[0]), _ => throw new PromotionDataException(PromotionDataErrorKind.DataCorruption, "Non-unique promotion row.") };

    public T ExecuteInTransaction<T>(Func<IPromotionDataTransaction, T> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return gateway.Transaction(tx => action(new PromotionMySqlDataTransaction(tx)));
    }
    public IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(TriggerType triggerType, IReadOnlyCollection<long> allowedActivityUids)
    {
        if (allowedActivityUids.Count == 0) return Array.Empty<PromotionActivity>();
        var scope = allowedActivityUids.Distinct().ToArray();
        if (scope.Length > 1000) throw new ArgumentOutOfRangeException(nameof(allowedActivityUids));
        return Select(Query(PromotionTableMetadata.Activity, PromotionTableMetadata.ActivityFields,
            new[] { Eq("TriggerType", (byte)triggerType), new Condition("ActivityUID", Comparison.In, scope) },
            new[] { new Sort("ActivityUID") })).Select(PromotionRowMapper.Activity).ToArray();
    }
    public PromotionActivity? GetActivity(long activityUid) => One(Select(Query(PromotionTableMetadata.Activity,
        PromotionTableMetadata.ActivityFields, new[] { Eq("ActivityUID", activityUid) })), PromotionRowMapper.Activity);
    public IReadOnlyList<EligibilityEntry> GetAvailableEligibilityEntries(long userUid, DateOnly businessDay) =>
        Select(Query(PromotionTableMetadata.Eligibility, PromotionTableMetadata.EligibilityFields,
            new[] { Eq("UserUID", userUid), Eq("BusinessDay", businessDay), Eq("Status", (byte)EligibilityStatus.Available) },
            new[] { new Sort("ActivityUID"), new Sort("EligibilityEntryId") })).Select(PromotionRowMapper.Eligibility).ToArray();
    public BonusStatus? GetBonusStatus(long userUid) => One(Select(Query(PromotionTableMetadata.Status,
        PromotionTableMetadata.StatusFields, new[] { Eq("UserUID", userUid) })), PromotionRowMapper.Status);
    public IReadOnlyList<BonusHistory> GetBonusHistory(long userUid, DateTime fromInclusive, DateTime toExclusive, int offset, int limit)
    {
        CheckBatch(limit);
        return Select(Query(PromotionTableMetadata.History, PromotionTableMetadata.HistoryFields,
            new[] { Eq("UserUID", userUid), new Condition("ClosedAt", Comparison.GreaterThanOrEqual, fromInclusive),
                new Condition("ClosedAt", Comparison.LessThan, toExclusive) },
            new[] { new Sort("ClosedAt", true), new Sort("BonusHistoryId", true) }, limit, offset))
            .Select(PromotionRowMapper.History).ToArray();
    }
    public IReadOnlyList<long> GetUsersWithExpiredEligibility(DateOnly currentBusinessDay, long afterUserUid, int batchSize)
    {
        CheckBatch(batchSize);
        // Eligibility may contain several rows per player. Page by distinct UserUID in bounded chunks.
        var result = new SortedSet<long>();
        var cursor = afterUserUid;
        while (result.Count < batchSize)
        {
            var rows = Select(Query(PromotionTableMetadata.Eligibility, new[] { "UserUID" },
                new[] { Eq("Status", (byte)EligibilityStatus.Available),
                    new Condition("BusinessDay", Comparison.LessThan, currentBusinessDay),
                    new Condition("UserUID", Comparison.GreaterThan, cursor) },
                new[] { new Sort("UserUID") }, 1000));
            if (rows.Count == 0) break;
            foreach (var row in rows)
            {
                result.Add(PositiveUserUid(row));
            }
            cursor = PositiveUserUid(rows[^1]);
            if (rows.Count < 1000) break;
        }
        return result.Take(batchSize).ToArray();
    }
    public IReadOnlyList<long> GetUsersWithExpiredTasks(DateOnly currentBusinessDay, long afterUserUid, int batchSize)
    {
        CheckBatch(batchSize);
        return Select(Query(PromotionTableMetadata.Status, new[] { "UserUID" },
            new[] { Eq("TaskState", (byte)BonusTaskState.InProgress),
                new Condition("BusinessDay", Comparison.LessThan, currentBusinessDay),
                new Condition("UserUID", Comparison.GreaterThan, afterUserUid) },
            new[] { new Sort("UserUID") }, batchSize))
            .Select(row => PositiveUserUid(row)).ToArray();
    }
    public int DeleteExpiredBonusHistory(DateTime cutoffExclusive, int batchSize)
    {
        CheckBatch(batchSize);
        var delete = new DeleteQuery(PromotionTableMetadata.History,
            new[] { new Condition("ClosedAt", Comparison.LessThan, cutoffExclusive) },
            new[] { new Sort("BonusHistoryId") }, batchSize);
        PromotionTableMetadata.Validate(delete);
        return checked((int)gateway.Delete(delete));
    }
    internal static void CheckBatch(int size)
    {
        if (size is < 1 or > 1000) throw new ArgumentOutOfRangeException(nameof(size));
    }
    private static long PositiveUserUid(IReadOnlyDictionary<string, object?> row)
    {
        if (!row.TryGetValue("UserUID", out var raw) || raw is null)
            throw new PromotionDataException(PromotionDataErrorKind.DataCorruption, "Missing user key.");
        try
        {
            var value = Convert.ToInt64(raw);
            return value > 0 ? value : throw new PromotionDataException(PromotionDataErrorKind.DataCorruption, "Invalid user key.");
        }
        catch (Exception ex) when (ex is FormatException or OverflowException or InvalidCastException)
        {
            throw new PromotionDataException(PromotionDataErrorKind.DataCorruption, "Invalid user key.", ex);
        }
    }
}
