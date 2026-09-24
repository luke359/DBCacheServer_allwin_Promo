using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;

namespace Promotion.Data.MySql;

public sealed class PromotionMySqlDataTransaction : IPromotionDataTransaction
{
    private readonly IPromotionV2Transaction tx;
    private readonly HashSet<long> lockedUsers = new();
    internal PromotionMySqlDataTransaction(IPromotionV2Transaction tx) => this.tx = tx;

    private IReadOnlyList<IReadOnlyDictionary<string, object?>> Select(Query query) => tx.Select(query);
    private static Condition Eq(string column, object value) => PromotionMySqlDataStore.Eq(column, value);
    private static Query Query(string table, IReadOnlyList<string> fields, IReadOnlyList<Condition>? where = null,
        IReadOnlyList<Sort>? order = null, int? limit = null, bool locked = false) =>
        PromotionMySqlDataStore.Query(table, fields, where, order, limit, null, locked);
    private static Dictionary<string, object?> Values(params (string Key, object? Value)[] entries) =>
        entries.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
    private InsertResult Insert(string table, Dictionary<string, object?> values)
    {
        PromotionTableMetadata.Validate(table, values.Keys);
        return tx.Insert(table, values);
    }
    private long Update(string table, Dictionary<string, object?> values, IReadOnlyList<Condition> where)
    {
        PromotionTableMetadata.Validate(table, values.Keys.Concat(where.Select(x => x.Column)));
        if (where.Count == 0) throw new ArgumentException("Promotion update requires conditions.");
        return tx.Update(table, values, where);
    }

    public BonusStatus GetOrCreateBonusStatusForUpdate(long userUid, DateTime operationTime)
    {
        var query = Query(PromotionTableMetadata.Status, PromotionTableMetadata.StatusFields,
            new[] { Eq("UserUID", userUid) }, locked: true);
        var current = PromotionMySqlDataStore.One(Select(query), PromotionRowMapper.Status);
        if (current is null)
        {
            try
            {
                Insert(PromotionTableMetadata.Status, Values(("UserUID", userUid), ("TaskState", (byte)BonusTaskState.None),
                    ("BonusTaskId", null), ("EligibilityEntryId", null), ("ActivityUID", null), ("BusinessDay", null),
                    ("LastFirstLoginBusinessDay", null), ("LastFirstDepositBusinessDay", null),
                    ("ActivitySnapshotJson", null), ("BonusAmount", 0m), ("RequiredWagerAmount", 0m),
                    ("CurrentWagerAmount", 0m), ("ClaimedAt", null), ("CreatedAt", operationTime), ("UpdatedAt", operationTime)));
            }
            catch (PromotionDataException ex) when (ex.Kind == PromotionDataErrorKind.DuplicateKey)
            { /* Another connection inserted the player row; lock and read it. */ }
            current = PromotionMySqlDataStore.One(Select(query), PromotionRowMapper.Status);
        }
        if (current is null) throw new PromotionDataException(PromotionDataErrorKind.DataCorruption, "Missing locked bonus status.");
        lockedUsers.Add(userUid);
        return current;
    }
    public PromotionActivity? GetActivity(long activityUid) => PromotionMySqlDataStore.One(
        Select(Query(PromotionTableMetadata.Activity, PromotionTableMetadata.ActivityFields, new[] { Eq("ActivityUID", activityUid) })),
        PromotionRowMapper.Activity);
    public IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(TriggerType triggerType, IReadOnlyCollection<long> allowedActivityUids)
    {
        if (allowedActivityUids.Count == 0) return Array.Empty<PromotionActivity>();
        var scope = allowedActivityUids.Distinct().ToArray();
        PromotionMySqlDataStore.CheckBatch(scope.Length);
        return Select(Query(PromotionTableMetadata.Activity, PromotionTableMetadata.ActivityFields,
            new[] { Eq("TriggerType", (byte)triggerType), new Condition("ActivityUID", Comparison.In, scope) },
            new[] { new Sort("ActivityUID") })).Select(PromotionRowMapper.Activity).ToArray();
    }
    public PromotionTriggerEvent? GetTriggerEvent(string eventId) => PromotionMySqlDataStore.One(
        Select(Query(PromotionTableMetadata.Event, PromotionTableMetadata.EventFields, new[] { Eq("EventId", eventId) })),
        PromotionRowMapper.Event);
    public IReadOnlyList<EligibilityEntry> GetEligibilityEntriesByEventId(string eventId) =>
        Select(Query(PromotionTableMetadata.Eligibility, PromotionTableMetadata.EligibilityFields,
            new[] { Eq("EventId", eventId) }, new[] { new Sort("ActivityUID"), new Sort("EligibilityEntryId") }))
            .Select(PromotionRowMapper.Eligibility).ToArray();
    public void InsertTriggerEvent(PromotionTriggerEvent value)
    {
        DomainInvariantValidator.ValidateTriggerEvent(value);
        Insert(PromotionTableMetadata.Event, Values(("EventId", value.EventId), ("UserUID", value.UserUID),
            ("TriggerType", (byte)value.TriggerType), ("EventTime", value.EventTime), ("BusinessDay", value.BusinessDay),
            ("EligibleDepositAmount", value.EligibleDepositAmount), ("CreatedAt", value.CreatedAt)));
    }
    public IReadOnlyList<EligibilityEntry> InsertEligibilityEntries(IReadOnlyList<NewEligibilityEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);
        var result = new List<EligibilityEntry>(entries.Count);
        foreach (var value in entries)
        {
            DomainInvariantValidator.ValidateEligibility(new EligibilityEntry(1, value.EventId, value.UserUID,
                value.ActivityUID, value.BusinessDay, value.TriggerType, value.EligibleDepositAmount,
                value.Status, value.ClaimedBonusTaskId, value.ExcludedByBonusTaskId, value.StatusChangedAt, value.CreatedAt));
            var inserted = Insert(PromotionTableMetadata.Eligibility, Values(("EventId", value.EventId),
                ("UserUID", value.UserUID), ("ActivityUID", value.ActivityUID), ("BusinessDay", value.BusinessDay),
                ("TriggerType", (byte)value.TriggerType), ("EligibleDepositAmount", value.EligibleDepositAmount),
                ("Status", (byte)value.Status), ("ClaimedBonusTaskId", value.ClaimedBonusTaskId),
                ("ExcludedByBonusTaskId", value.ExcludedByBonusTaskId), ("StatusChangedAt", value.StatusChangedAt),
                ("CreatedAt", value.CreatedAt)));
            if (inserted.LastInsertedId <= 0) throw new PromotionDataException(PromotionDataErrorKind.Unexpected, "Insert did not return an ID.");
            var mapped = new EligibilityEntry(inserted.LastInsertedId, value.EventId, value.UserUID, value.ActivityUID,
                value.BusinessDay, value.TriggerType, value.EligibleDepositAmount, value.Status, value.ClaimedBonusTaskId,
                value.ExcludedByBonusTaskId, value.StatusChangedAt, value.CreatedAt);
            DomainInvariantValidator.ValidateEligibility(mapped);
            result.Add(mapped);
        }
        return result;
    }
    public EligibilityEntry? GetEligibilityEntryForUpdate(long eligibilityEntryId) => PromotionMySqlDataStore.One(
        Select(Query(PromotionTableMetadata.Eligibility, PromotionTableMetadata.EligibilityFields,
            new[] { Eq("EligibilityEntryId", eligibilityEntryId) }, locked: true)), PromotionRowMapper.Eligibility);
    public int CountClaimedEligibility(long userUid, long activityUid, DateOnly businessDay)
    {
        var where = new[] { Eq("UserUID", userUid), Eq("ActivityUID", activityUid), Eq("BusinessDay", businessDay),
            Eq("Status", (byte)EligibilityStatus.Claimed) };
        PromotionTableMetadata.Validate(PromotionTableMetadata.Eligibility, where.Select(x => x.Column));
        return checked((int)tx.Count(PromotionTableMetadata.Eligibility, where));
    }
    public void MarkEligibilityClaimed(long eligibilityEntryId, string bonusTaskId, DateTime changedAt)
    {
        var affected = Update(PromotionTableMetadata.Eligibility,
            Values(("Status", (byte)EligibilityStatus.Claimed), ("ClaimedBonusTaskId", bonusTaskId), ("StatusChangedAt", changedAt)),
            new[] { Eq("EligibilityEntryId", eligibilityEntryId), Eq("Status", (byte)EligibilityStatus.Available) });
        if (affected != 1) throw new PromotionDataException(PromotionDataErrorKind.ConcurrencyConflict, "Eligibility state changed concurrently.");
    }
    public int ExcludeEligibilityEntries(long userUid, DateOnly businessDay, long claimedEligibilityEntryId,
        string bonusTaskId, bool excludeNonStackable, string? exclusiveGroup, DateTime changedAt)
    {
        var entries = Select(Query(PromotionTableMetadata.Eligibility, PromotionTableMetadata.EligibilityFields,
            new[] { Eq("UserUID", userUid), Eq("BusinessDay", businessDay), Eq("Status", (byte)EligibilityStatus.Available) },
            new[] { new Sort("EligibilityEntryId") }, locked: true)).Select(PromotionRowMapper.Eligibility);
        var count = 0;
        foreach (var entry in entries)
        {
            if (entry.EligibilityEntryId == claimedEligibilityEntryId) continue;
            var activity = GetActivity(entry.ActivityUID) ?? throw new PromotionDataException(PromotionDataErrorKind.DataCorruption, "Missing activity.");
            if (!(excludeNonStackable && activity.IsNonStackable ||
                  exclusiveGroup is not null && exclusiveGroup == activity.ExclusiveGroup)) continue;
            count += checked((int)Update(PromotionTableMetadata.Eligibility,
                Values(("Status", (byte)EligibilityStatus.Excluded), ("ExcludedByBonusTaskId", bonusTaskId),
                    ("StatusChangedAt", changedAt)),
                new[] { Eq("EligibilityEntryId", entry.EligibilityEntryId), Eq("Status", (byte)EligibilityStatus.Available) }));
        }
        return count;
    }
    public int ExpireAvailableEligibilityEntries(long userUid, DateOnly currentBusinessDay, DateTime changedAt)
    {
        var entries = Select(Query(PromotionTableMetadata.Eligibility, PromotionTableMetadata.EligibilityFields,
            new[] { Eq("UserUID", userUid), new Condition("BusinessDay", Comparison.LessThan, currentBusinessDay),
                Eq("Status", (byte)EligibilityStatus.Available) }, new[] { new Sort("EligibilityEntryId") }, locked: true))
            .Select(PromotionRowMapper.Eligibility);
        var count = 0;
        foreach (var entry in entries)
            count += checked((int)Update(PromotionTableMetadata.Eligibility,
                Values(("Status", (byte)EligibilityStatus.Expired), ("StatusChangedAt", changedAt)),
                new[] { Eq("EligibilityEntryId", entry.EligibilityEntryId), Eq("Status", (byte)EligibilityStatus.Available) }));
        return count;
    }
    public void SaveBonusStatus(BonusStatus value)
    {
        DomainInvariantValidator.ValidateBonusStatus(value.TaskState == BonusTaskState.Closing
            ? value with { TaskState = BonusTaskState.InProgress } : value);
        if (!lockedUsers.Contains(value.UserUID)) throw new InvalidOperationException("Bonus status must be locked first.");
        var affected = Update(PromotionTableMetadata.Status,
            Values(("TaskState", (byte)value.TaskState), ("BonusTaskId", value.BonusTaskId),
                ("EligibilityEntryId", value.EligibilityEntryId), ("ActivityUID", value.ActivityUID),
                ("BusinessDay", value.BusinessDay),
                ("LastFirstLoginBusinessDay", value.LastFirstLoginBusinessDay),
                ("LastFirstDepositBusinessDay", value.LastFirstDepositBusinessDay),
                ("ActivitySnapshotJson", value.ActivitySnapshotJson),
                ("BonusAmount", value.BonusAmount), ("RequiredWagerAmount", value.RequiredWagerAmount),
                ("CurrentWagerAmount", value.CurrentWagerAmount), ("ClaimedAt", value.ClaimedAt),
                ("CreatedAt", value.CreatedAt), ("UpdatedAt", value.UpdatedAt)), new[] { Eq("UserUID", value.UserUID) });
        if (affected == 1) return;
        if (affected == 0)
        {
            var reread = PromotionMySqlDataStore.One(Select(Query(PromotionTableMetadata.Status,
                PromotionTableMetadata.StatusFields, new[] { Eq("UserUID", value.UserUID) }, locked: true)), PromotionRowMapper.Status);
            if (reread == value) return;
        }
        throw new PromotionDataException(PromotionDataErrorKind.ConcurrencyConflict, "Bonus status changed concurrently.");
    }
    public BonusHistory? GetBonusHistoryByTaskId(string bonusTaskId) => PromotionMySqlDataStore.One(
        Select(Query(PromotionTableMetadata.History, PromotionTableMetadata.HistoryFields,
            new[] { Eq("BonusTaskId", bonusTaskId) })), PromotionRowMapper.History);
    public BonusHistory InsertBonusHistory(NewBonusHistory value)
    {
        DomainInvariantValidator.ValidateHistory(new BonusHistory(1, value.BonusTaskId, value.UserUID,
            value.EligibilityEntryId, value.ActivityUID, value.BusinessDay, value.ActivitySnapshotJson,
            value.BonusAmount, value.RequiredWagerAmount, value.CurrentWagerAmount, value.ConvertedAmount,
            value.CloseReason, value.ClaimedAt, value.ClosedAt, value.CreatedAt));
        try
        {
            var inserted = Insert(PromotionTableMetadata.History, Values(("BonusTaskId", value.BonusTaskId),
                ("UserUID", value.UserUID), ("EligibilityEntryId", value.EligibilityEntryId),
                ("ActivityUID", value.ActivityUID), ("BusinessDay", value.BusinessDay),
                ("ActivitySnapshotJson", value.ActivitySnapshotJson), ("BonusAmount", value.BonusAmount),
                ("RequiredWagerAmount", value.RequiredWagerAmount), ("CurrentWagerAmount", value.CurrentWagerAmount),
                ("ConvertedAmount", value.ConvertedAmount), ("CloseReason", (byte)value.CloseReason),
                ("ClaimedAt", value.ClaimedAt), ("ClosedAt", value.ClosedAt), ("CreatedAt", value.CreatedAt)));
            if (inserted.LastInsertedId <= 0) throw new PromotionDataException(PromotionDataErrorKind.Unexpected, "Insert did not return an ID.");
            var result = new BonusHistory(inserted.LastInsertedId, value.BonusTaskId, value.UserUID,
                value.EligibilityEntryId, value.ActivityUID, value.BusinessDay, value.ActivitySnapshotJson,
                value.BonusAmount, value.RequiredWagerAmount, value.CurrentWagerAmount, value.ConvertedAmount,
                value.CloseReason, value.ClaimedAt, value.ClosedAt, value.CreatedAt);
            DomainInvariantValidator.ValidateHistory(result);
            return result;
        }
        catch (PromotionDataException ex) when (ex.Kind == PromotionDataErrorKind.DuplicateKey)
        {
            var existing = GetBonusHistoryByTaskId(value.BonusTaskId);
            if (existing is not null && existing == new BonusHistory(existing.BonusHistoryId, value.BonusTaskId,
                value.UserUID, value.EligibilityEntryId, value.ActivityUID, value.BusinessDay,
                value.ActivitySnapshotJson, value.BonusAmount, value.RequiredWagerAmount, value.CurrentWagerAmount,
                value.ConvertedAmount, value.CloseReason, value.ClaimedAt, value.ClosedAt, value.CreatedAt)) return existing;
            throw;
        }
    }
}
