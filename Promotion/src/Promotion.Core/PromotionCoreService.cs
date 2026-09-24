using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;
using Promotion.Core.Validation;

namespace Promotion.Core;

public interface IGuidProvider { Guid NewGuid(); }
public interface IRetryDelay { void Delay(TimeSpan duration); }
public interface IJitterSource { int NextMilliseconds(int exclusiveMaximum); }

public sealed class SystemGuidProvider : IGuidProvider { public Guid NewGuid() => Guid.NewGuid(); }
public sealed class SystemLocalClock : ILocalClock { public DateTime GetNow() => DateTime.Now; }
public sealed class ThreadRetryDelay : IRetryDelay { public void Delay(TimeSpan duration) => Thread.Sleep(duration); }
public sealed class RandomJitterSource : IJitterSource { public int NextMilliseconds(int exclusiveMaximum) => Random.Shared.Next(exclusiveMaximum); }

public sealed partial class PromotionCoreService : IPromotionCoreService
{
    private readonly IPromotionDataStore data;
    private readonly ILocalClock clock;
    private readonly IGuidProvider guids;
    private readonly IRetryDelay retryDelay;
    private readonly IJitterSource jitter;
    private readonly object initializeSync = new();
    private TimeSpan cutover;
    private BalanceConvertFormula? balanceFormula;
    private bool initialized;

    public PromotionCoreService(IPromotionDataStore data, ILocalClock? clock = null,
        IGuidProvider? guids = null, IRetryDelay? retryDelay = null, IJitterSource? jitter = null)
    {
        this.data = data ?? throw new ArgumentNullException(nameof(data));
        this.clock = clock ?? new SystemLocalClock();
        this.guids = guids ?? new SystemGuidProvider();
        this.retryDelay = retryDelay ?? new ThreadRetryDelay();
        this.jitter = jitter ?? new RandomJitterSource();
    }

    private string Correlation(string? supplied) => supplied ?? guids.NewGuid().ToString("D");
    private bool IsInitialized => Volatile.Read(ref initialized);
    private PromotionResult<T>? Check<T>(PromotionErrorCode validation, string correlation)
    {
        if (validation != PromotionErrorCode.None) return PromotionResultFactory.Failure<T>(correlation, validation);
        return IsInitialized ? null : PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.ServiceNotInitialized);
    }
    private DateOnly BusinessDay(DateTime time) => BusinessDayCalculator.Calculate(time, cutover);
    private DateTime OperationTime() => clock.GetNow();
    private static DateTime DatabaseTime(DateTime time) => time.AddTicks(-(time.Ticks % 10));
    private static IReadOnlyList<T> Freeze<T>(IEnumerable<T> values) => Array.AsReadOnly(values.ToArray());

    public PromotionResult<InitializeData> Initialize(InitializeRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        var error = PromotionRequestValidator.Validate(request);
        if (error != PromotionErrorCode.None) return PromotionResultFactory.Failure<InitializeData>(correlation, error);
        ArgumentNullException.ThrowIfNull(request);
        lock (initializeSync)
        {
            if (!initialized)
            {
                cutover = request.BusinessDayCutover;
                balanceFormula = request.BalanceConvertFormula;
                Volatile.Write(ref initialized, true);
                return PromotionResultFactory.Success(correlation, new InitializeData(cutover, 2, false));
            }
            if (cutover == request.BusinessDayCutover && ReferenceEquals(balanceFormula, request.BalanceConvertFormula))
                return PromotionResultFactory.Success(correlation, new InitializeData(cutover, 2, true));
        }
        return PromotionResultFactory.Failure<InitializeData>(correlation, PromotionErrorCode.InitializationConflict);
    }

    public PromotionResult<CreateEligibilityData> CreateEligibility(CreateEligibilityRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<CreateEligibilityData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        var now = OperationTime();
        var eventTime = DatabaseTime(request.EventTime);
        var day = BusinessDay(request.EventTime);
        return Transactional(correlation, tx =>
        {
            var status = tx.GetOrCreateBonusStatusForUpdate(request.UserUID, now);
            var existing = tx.GetTriggerEvent(request.EventId);
            if (existing is not null)
            {
                if (existing.UserUID != request.UserUID || existing.TriggerType != request.TriggerType ||
                    existing.EventTime != eventTime || existing.EligibleDepositAmount != request.EligibleDepositAmount)
                {
                    var details = new PromotionErrorDetailsDto(existing.EventId, existing.UserUID, existing.TriggerType,
                        existing.EventTime, existing.BusinessDay, null);
                    return PromotionResultFactory.Failure<CreateEligibilityData>(correlation, PromotionErrorCode.EventIdConflict, details);
                }
                return PromotionResultFactory.Success(correlation, new CreateEligibilityData(request.EventId,
                    request.UserUID, existing.BusinessDay, true,
                    Freeze(tx.GetEligibilityEntriesByEventId(request.EventId).Select(EntryDto))));
            }
            var isRepeatedDailyTrigger = request.TriggerType switch
            {
                TriggerType.FirstLoginOfBusinessDay => status.LastFirstLoginBusinessDay >= day,
                TriggerType.FirstDepositOfBusinessDay => status.LastFirstDepositBusinessDay >= day,
                _ => false
            };
            tx.InsertTriggerEvent(new PromotionTriggerEvent(request.EventId, request.UserUID,
                request.TriggerType, eventTime, day, request.EligibleDepositAmount, now));
            if (isRepeatedDailyTrigger)
                return PromotionResultFactory.Success(correlation, new CreateEligibilityData(request.EventId,
                    request.UserUID, day, false, Array.Empty<EligibilityEntryDto>()));
            if (request.TriggerType is TriggerType.FirstLoginOfBusinessDay or TriggerType.FirstDepositOfBusinessDay)
            {
                tx.SaveBonusStatus(status with
                {
                    LastFirstLoginBusinessDay = request.TriggerType == TriggerType.FirstLoginOfBusinessDay
                        ? day : status.LastFirstLoginBusinessDay,
                    LastFirstDepositBusinessDay = request.TriggerType == TriggerType.FirstDepositOfBusinessDay
                        ? day : status.LastFirstDepositBusinessDay,
                    UpdatedAt = now
                });
            }
            var activities = tx.GetActivitiesByTriggerType(request.TriggerType, request.AllowedActivityUIDs);
            var eligible = activities.Where(activity => ActivityEligibilityEvaluator.IsEligible(activity,
                request.TriggerType, day, request.EligibleDepositAmount, request.AllowedActivityUIDs,
                tx.CountClaimedEligibility(request.UserUID, activity.ActivityUID, day)))
                .OrderBy(activity => activity.ActivityUID).ToArray();
            var entries = tx.InsertEligibilityEntries(eligible.Select(activity => new NewEligibilityEntry(
                request.EventId, request.UserUID, activity.ActivityUID, day, request.TriggerType,
                request.EligibleDepositAmount, EligibilityStatus.Available, null, null, now, now)).ToArray());
            return PromotionResultFactory.Success(correlation, new CreateEligibilityData(request.EventId,
                request.UserUID, day, false, Freeze(entries.Select(EntryDto))));
        }, () => VerifyEligibility(request, correlation),
            duplicateRecovery: ex => ex.ConstraintName == "PromotionTriggerEvent.PRIMARY"
                ? VerifyEligibility(request, correlation) : null);
    }

    private PromotionResult<CreateEligibilityData>? VerifyEligibility(CreateEligibilityRequest request, string correlation)
    {
        var existing = data.ExecuteInTransaction<(PromotionTriggerEvent Event, IReadOnlyList<EligibilityEntry> Entries)?>(tx =>
        {
            var value = tx.GetTriggerEvent(request.EventId);
            return value is null ? null : (value, tx.GetEligibilityEntriesByEventId(request.EventId));
        });
        if (existing is null) return null;
        var (eventValue, entries) = existing.Value;
        if (eventValue.UserUID != request.UserUID || eventValue.TriggerType != request.TriggerType ||
            eventValue.EventTime != DatabaseTime(request.EventTime) || eventValue.EligibleDepositAmount != request.EligibleDepositAmount)
            return PromotionResultFactory.Failure<CreateEligibilityData>(correlation,
                PromotionErrorCode.EventIdConflict,
                new PromotionErrorDetailsDto(eventValue.EventId, eventValue.UserUID,
                    eventValue.TriggerType, eventValue.EventTime, eventValue.BusinessDay, null));
        return PromotionResultFactory.Success(correlation, new CreateEligibilityData(request.EventId, request.UserUID,
            eventValue.BusinessDay, true, Freeze(entries.Select(EntryDto))));
    }

    public PromotionResult<AvailablePromotionListData> GetAvailablePromotions(GetAvailablePromotionsRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<AvailablePromotionListData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        var day = BusinessDay(request.QueryTime);
        return Read(correlation, () =>
        {
            var status = data.GetBonusStatus(request.UserUID);
            var items = data.GetAvailableEligibilityEntries(request.UserUID, day).OrderBy(x => x.EligibilityEntryId)
                .Select(entry =>
                {
                    var activity = data.GetActivity(entry.ActivityUID) ?? throw new PromotionDataException(
                        PromotionDataErrorKind.DataCorruption, "Eligibility activity is missing.");
                    var snapshot = ActivitySnapshotFactory.Capture(activity, request.QueryTime);
                    var bonus = PromotionAmountCalculator.Bonus(snapshot, entry.EligibleDepositAmount);
                    return new AvailablePromotionDto(entry.EligibilityEntryId, entry.EventId, entry.ActivityUID,
                        entry.BusinessDay, entry.TriggerType, entry.EligibleDepositAmount, activity.ActivityInfo,
                        bonus, PromotionAmountCalculator.RequiredWager(snapshot, bonus, entry.EligibleDepositAmount),
                        activity.MaxBetAmount, activity.IsNonStackable, activity.ExclusiveGroup);
                }).ToArray();
            return new AvailablePromotionListData(request.UserUID, day,
                status?.TaskState == BonusTaskState.InProgress, status?.BonusTaskId, Freeze(items));
        });
    }

    public PromotionResult<GameServerListData> GetGameServerList(GetGameServerListRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<GameServerListData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        return Read(correlation, () =>
        {
            var activity = data.GetActivity(request.ActivityUID);
            return activity is null ? null : new GameServerListData(activity.ActivityUID, activity.GameServerList);
        }, PromotionErrorCode.ActivityNotFound);
    }

    public PromotionResult<BonusTaskStatusData> GetBonusTaskStatus(GetBonusTaskStatusRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        if (Check<BonusTaskStatusData>(PromotionRequestValidator.Validate(request), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        return Read(correlation, () =>
        {
            var status = data.GetBonusStatus(request.UserUID);
            if (status is null || status.TaskState == BonusTaskState.None)
                return new BonusTaskStatusData(request.UserUID, false, null);
            if (status.TaskState != BonusTaskState.InProgress)
                throw new InvalidOperationException("Invalid bonus task state.");
            var snapshot = ActivitySnapshotSerializer.Deserialize(status.ActivitySnapshotJson!);
            var dto = new ActiveBonusTaskDto(status.BonusTaskId!, status.EligibilityEntryId!.Value,
                status.ActivityUID!.Value, status.BusinessDay!.Value, status.BonusAmount,
                status.RequiredWagerAmount, status.CurrentWagerAmount,
                PromotionAmountCalculator.RemainingWager(status.RequiredWagerAmount, status.CurrentWagerAmount),
                snapshot.MaxBetAmount, status.ClaimedAt!.Value, snapshot);
            return new BonusTaskStatusData(request.UserUID, true, dto);
        });
    }

    public PromotionResult<BonusHistoryPageData> GetBonusHistory(GetBonusHistoryRequest request)
    {
        var correlation = Correlation(request?.CorrelationId);
        var now = OperationTime();
        if (Check<BonusHistoryPageData>(PromotionRequestValidator.Validate(request, now), correlation) is { } failure) return failure;
        ArgumentNullException.ThrowIfNull(request);
        return Read(correlation, () => new BonusHistoryPageData(request.UserUID, request.FromInclusive,
            request.ToExclusive, request.Offset, request.Limit,
            Freeze(data.GetBonusHistory(request.UserUID, request.FromInclusive, request.ToExclusive, request.Offset, request.Limit)
                .Select(history => new BonusHistoryDto(history.BonusHistoryId, history.BonusTaskId, history.UserUID,
                    history.EligibilityEntryId, history.ActivityUID, history.BusinessDay,
                    ActivitySnapshotSerializer.Deserialize(history.ActivitySnapshotJson), history.BonusAmount,
                    history.RequiredWagerAmount, history.CurrentWagerAmount, history.ConvertedAmount,
                    history.CloseReason, history.ClaimedAt, history.ClosedAt)))));
    }

    private static EligibilityEntryDto EntryDto(EligibilityEntry value) => new(value.EligibilityEntryId,
        value.EventId, value.UserUID, value.ActivityUID, value.BusinessDay, value.TriggerType,
        value.EligibleDepositAmount, value.Status, value.ClaimedBonusTaskId, value.ExcludedByBonusTaskId,
        value.StatusChangedAt, value.CreatedAt);

    private PromotionResult<T> Read<T>(string correlation, Func<T?> query, PromotionErrorCode? missing = null)
        where T : class
    {
        try
        {
            var value = query();
            return value is null ? PromotionResultFactory.Failure<T>(correlation, missing ?? PromotionErrorCode.UnexpectedError)
                : PromotionResultFactory.Success(correlation, value);
        }
        catch (Exception ex) { return FailureFrom<T>(correlation, ex); }
    }

    private PromotionResult<T> Transactional<T>(string correlation,
        Func<IPromotionDataTransaction, PromotionResult<T>> action,
        Func<PromotionResult<T>?>? verifyUnknown = null,
        Func<PromotionDataException, PromotionResult<T>?>? duplicateRecovery = null,
        string? retryDuplicateConstraint = null,
        string? retryDuplicateConstraint2 = null)
    {
        for (var attempt = 1; attempt <= 3; attempt++)
        {
            PromotionDataErrorKind? retryKind = null;
            try { return data.ExecuteInTransaction(action); }
            catch (PromotionDataException ex) when (ex.Kind == PromotionDataErrorKind.CommitOutcomeUnknown)
            {
                if (verifyUnknown is null) return PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.CommitOutcomeUnknown);
                try
                {
                    if (verifyUnknown() is { } verified) return verified;
                }
                catch { return PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.CommitOutcomeUnknown); }
                if (attempt == 3) return PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.DatabaseConnectionFailure);
                retryKind = PromotionDataErrorKind.Connection;
            }
            catch (PromotionDataException ex) when (ex.Kind == PromotionDataErrorKind.DuplicateKey)
            {
                if (attempt < 3 && ex.ConstraintName is not null &&
                    (ex.ConstraintName == retryDuplicateConstraint || ex.ConstraintName == retryDuplicateConstraint2))
                    retryKind = PromotionDataErrorKind.Deadlock;
                else if (duplicateRecovery is not null)
                {
                    try
                    {
                        if (duplicateRecovery(ex) is { } recovered) return recovered;
                    }
                    catch (Exception recoveryError) { return FailureFrom<T>(correlation, recoveryError); }
                    return FailureFrom<T>(correlation, ex);
                }
                else return FailureFrom<T>(correlation, ex);
            }
            catch (PromotionDataException ex) when (ex.Kind is PromotionDataErrorKind.Deadlock or
                PromotionDataErrorKind.LockWaitTimeout or PromotionDataErrorKind.Connection or PromotionDataErrorKind.ConcurrencyConflict)
            {
                if (attempt == 3) return FailureFrom<T>(correlation, ex);
                retryKind = ex.Kind;
            }
            catch (Exception ex) { return FailureFrom<T>(correlation, ex); }
            var connection = retryKind == PromotionDataErrorKind.Connection;
            var baseMilliseconds = connection ? (attempt == 1 ? 100 : 300) : (attempt == 1 ? 25 : 100);
            retryDelay.Delay(TimeSpan.FromMilliseconds(baseMilliseconds + jitter.NextMilliseconds(connection ? 101 : 26)));
        }
        return PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.UnexpectedError);
    }

    private static PromotionResult<T> FailureFrom<T>(string correlation, Exception ex)
    {
        if (ex is PromotionDataException dataError)
        {
            var code = dataError.Kind switch
            {
                PromotionDataErrorKind.Deadlock => PromotionErrorCode.DatabaseDeadlock,
                PromotionDataErrorKind.LockWaitTimeout => PromotionErrorCode.DatabaseLockWaitTimeout,
                PromotionDataErrorKind.Connection => PromotionErrorCode.DatabaseConnectionFailure,
                PromotionDataErrorKind.CommitOutcomeUnknown => PromotionErrorCode.CommitOutcomeUnknown,
                PromotionDataErrorKind.DataCorruption => PromotionErrorCode.DataCorruption,
                PromotionDataErrorKind.InvalidBonusTaskState => PromotionErrorCode.InvalidBonusTaskState,
                PromotionDataErrorKind.DuplicateKey or PromotionDataErrorKind.ConcurrencyConflict => PromotionErrorCode.DataIntegrityConflict,
                _ => PromotionErrorCode.UnexpectedError
            };
            return PromotionResultFactory.Failure<T>(correlation, code);
        }
        if (ex is BalanceConvertFormulaException)
            return PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.BalanceConvertFormulaFailed);
        if (ex is DomainInvariantException or UnsupportedSnapshotVersionException)
            return PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.DataCorruption);
        return PromotionResultFactory.Failure<T>(correlation, PromotionErrorCode.UnexpectedError);
    }
}
