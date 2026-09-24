using Promotion.Core.Contracts;
using Promotion.Core.Domain;

namespace Promotion.Core.Data;

public interface IPromotionDataStore
{
    T ExecuteInTransaction<T>(Func<IPromotionDataTransaction, T> action);
    IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(TriggerType triggerType, IReadOnlyCollection<long> allowedActivityUids);
    PromotionActivity? GetActivity(long activityUid);
    IReadOnlyList<EligibilityEntry> GetAvailableEligibilityEntries(long userUid, DateOnly businessDay);
    BonusStatus? GetBonusStatus(long userUid);
    IReadOnlyList<BonusHistory> GetBonusHistory(long userUid, DateTime fromInclusive, DateTime toExclusive, int offset, int limit);
    IReadOnlyList<long> GetUsersWithExpiredEligibility(DateOnly currentBusinessDay, long afterUserUid, int batchSize);
    IReadOnlyList<long> GetUsersWithExpiredTasks(DateOnly currentBusinessDay, long afterUserUid, int batchSize);
    int DeleteExpiredBonusHistory(DateTime cutoffExclusive, int batchSize);
}

public interface IPromotionDataTransaction
{
    BonusStatus GetOrCreateBonusStatusForUpdate(long userUid, DateTime operationTime);
    PromotionActivity? GetActivity(long activityUid);
    IReadOnlyList<PromotionActivity> GetActivitiesByTriggerType(TriggerType triggerType, IReadOnlyCollection<long> allowedActivityUids);
    PromotionTriggerEvent? GetTriggerEvent(string eventId);
    IReadOnlyList<EligibilityEntry> GetEligibilityEntriesByEventId(string eventId);
    void InsertTriggerEvent(PromotionTriggerEvent triggerEvent);
    IReadOnlyList<EligibilityEntry> InsertEligibilityEntries(IReadOnlyList<NewEligibilityEntry> entries);
    EligibilityEntry? GetEligibilityEntryForUpdate(long eligibilityEntryId);
    int CountClaimedEligibility(long userUid, long activityUid, DateOnly businessDay);
    void MarkEligibilityClaimed(long eligibilityEntryId, string bonusTaskId, DateTime changedAt);
    int ExcludeEligibilityEntries(long userUid, DateOnly businessDay, long claimedEligibilityEntryId,
        string bonusTaskId, bool excludeNonStackable, string? exclusiveGroup, DateTime changedAt);
    int ExpireAvailableEligibilityEntries(long userUid, DateOnly currentBusinessDay, DateTime changedAt);
    void SaveBonusStatus(BonusStatus bonusStatus);
    BonusHistory? GetBonusHistoryByTaskId(string bonusTaskId);
    BonusHistory InsertBonusHistory(NewBonusHistory history);
}
