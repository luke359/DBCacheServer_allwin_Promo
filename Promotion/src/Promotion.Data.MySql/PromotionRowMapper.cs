using System.Globalization;
using Promotion.Core.Contracts;
using Promotion.Core.Data;
using Promotion.Core.Domain;

namespace Promotion.Data.MySql;

internal static class PromotionRowMapper
{
    private static object Required(IReadOnlyDictionary<string, object?> row, string key)
    {
        if (!row.TryGetValue(key, out var value) || value is null || value is DBNull)
            throw Corrupt();
        return value;
    }

    private static object? Optional(IReadOnlyDictionary<string, object?> row, string key)
    {
        if (!row.TryGetValue(key, out var value)) throw Corrupt();
        return value is DBNull ? null : value;
    }

    private static PromotionDataException Corrupt(Exception? inner = null) =>
        new(PromotionDataErrorKind.DataCorruption, "Invalid promotion database row.", inner);

    private static T Read<T>(IReadOnlyDictionary<string, object?> row, Func<T> read, Action<T> validate)
    {
        try
        {
            var value = read();
            validate(value);
            return value;
        }
        catch (PromotionDataException) { throw; }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException or InvalidCastException
            or DomainInvariantException or UnsupportedSnapshotVersionException or System.Text.Json.JsonException)
        { throw Corrupt(ex); }
    }

    private static long Long(object value) => Convert.ToInt64(value, CultureInfo.InvariantCulture);
    private static int Int(object value) => Convert.ToInt32(value, CultureInfo.InvariantCulture);
    private static decimal Decimal(object value) => value is decimal amount ? amount : throw Corrupt();
    private static string String(object value) => value switch
    {
        string text => text,
        Guid guid => guid.ToString("D"), // MySql.Data may materialize CHAR(36) as Guid.
        _ => throw Corrupt()
    };
    private static DateTime Time(object value) => value is DateTime date ? date : throw Corrupt();
    private static DateOnly Day(object value) => DateOnly.FromDateTime(Time(value));
    private static bool Bool(object value) => Int(value) switch { 0 => false, 1 => true, _ => throw Corrupt() };
    private static T EnumValue<T>(object value) where T : struct, Enum
    {
        var number = Convert.ToByte(value, CultureInfo.InvariantCulture);
        var result = (T)Enum.ToObject(typeof(T), number);
        return Enum.IsDefined(result) ? result : throw Corrupt();
    }
    private static string? TextOrNull(IReadOnlyDictionary<string, object?> row, string key) =>
        Optional(row, key) is { } value ? String(value) : null;
    private static int? IntOrNull(IReadOnlyDictionary<string, object?> row, string key) =>
        Optional(row, key) is { } value ? Int(value) : null;
    private static long? LongOrNull(IReadOnlyDictionary<string, object?> row, string key) =>
        Optional(row, key) is { } value ? Long(value) : null;
    private static decimal? DecimalOrNull(IReadOnlyDictionary<string, object?> row, string key) =>
        Optional(row, key) is { } value ? Decimal(value) : null;
    private static DateOnly? DayOrNull(IReadOnlyDictionary<string, object?> row, string key) =>
        Optional(row, key) is { } value ? Day(value) : null;
    private static DateTime? TimeOrNull(IReadOnlyDictionary<string, object?> row, string key) =>
        Optional(row, key) is { } value ? Time(value) : null;

    public static PromotionActivity Activity(IReadOnlyDictionary<string, object?> r) => Read(r, () => new PromotionActivity(
        Long(Required(r, "ActivityUID")), TextOrNull(r, "ActivityInfo"), EnumValue<BonusType>(Required(r, "BonusType")),
        Int(Required(r, "FixedBonusAmount")), Int(Required(r, "MaxBonusAmount")), Int(Required(r, "DepositPercentage")),
        IntOrNull(r, "MinimumDepositAmount"), Int(Required(r, "WagerMultiplier")),
        EnumValue<WagerCalculationType>(Required(r, "WagerCalculationType")), IntOrNull(r, "MaxBetAmount"),
        TextOrNull(r, "GameServerList"), Decimal(Required(r, "WagerContributionRate")),
        EnumValue<ConvertType>(Required(r, "ConvertType")), Int(Required(r, "FixedConvertedAmount")),
        IntOrNull(r, "MaxBalanceConvertedAmount"), Int(Required(r, "DailyClaimLimit")),
        DayOrNull(r, "StartDate"), DayOrNull(r, "EndDate"), String(Required(r, "WeekdayMask")),
        EnumValue<ActivityStatus>(Required(r, "ActivityStatus")), EnumValue<TriggerType>(Required(r, "TriggerType")),
        Bool(Required(r, "IsNonStackable")),
        TextOrNull(r, "ExclusiveGroup"), Time(Required(r, "CreatedAt")), Time(Required(r, "UpdatedAt"))),
        DomainInvariantValidator.ValidateActivity);

    public static PromotionTriggerEvent Event(IReadOnlyDictionary<string, object?> r) => Read(r, () => new PromotionTriggerEvent(
        String(Required(r, "EventId")), Long(Required(r, "UserUID")), EnumValue<TriggerType>(Required(r, "TriggerType")),
        Time(Required(r, "EventTime")), Day(Required(r, "BusinessDay")), DecimalOrNull(r, "EligibleDepositAmount"),
        Time(Required(r, "CreatedAt"))), DomainInvariantValidator.ValidateTriggerEvent);

    public static EligibilityEntry Eligibility(IReadOnlyDictionary<string, object?> r) => Read(r, () => new EligibilityEntry(
        Long(Required(r, "EligibilityEntryId")), String(Required(r, "EventId")), Long(Required(r, "UserUID")),
        Long(Required(r, "ActivityUID")), Day(Required(r, "BusinessDay")), EnumValue<TriggerType>(Required(r, "TriggerType")),
        DecimalOrNull(r, "EligibleDepositAmount"), EnumValue<EligibilityStatus>(Required(r, "Status")),
        TextOrNull(r, "ClaimedBonusTaskId"), TextOrNull(r, "ExcludedByBonusTaskId"),
        Time(Required(r, "StatusChangedAt")), Time(Required(r, "CreatedAt"))), DomainInvariantValidator.ValidateEligibility);

    public static BonusStatus Status(IReadOnlyDictionary<string, object?> r) => Read(r, () => new BonusStatus(
        Long(Required(r, "UserUID")), EnumValue<BonusTaskState>(Required(r, "TaskState")), TextOrNull(r, "BonusTaskId"),
        LongOrNull(r, "EligibilityEntryId"), LongOrNull(r, "ActivityUID"), DayOrNull(r, "BusinessDay"),
        DayOrNull(r, "LastFirstLoginBusinessDay"), DayOrNull(r, "LastFirstDepositBusinessDay"),
        TextOrNull(r, "ActivitySnapshotJson"), Decimal(Required(r, "BonusAmount")),
        Decimal(Required(r, "RequiredWagerAmount")), Decimal(Required(r, "CurrentWagerAmount")),
        TimeOrNull(r, "ClaimedAt"), Time(Required(r, "CreatedAt")), Time(Required(r, "UpdatedAt"))),
        value =>
        {
            if ((value.TaskState is BonusTaskState.InProgress or BonusTaskState.Closing) &&
                !string.IsNullOrWhiteSpace(value.ActivitySnapshotJson))
                ActivitySnapshotSerializer.Deserialize(value.ActivitySnapshotJson);
            try { DomainInvariantValidator.ValidateBonusStatus(value); }
            catch (DomainInvariantException ex)
            {
                throw new PromotionDataException(PromotionDataErrorKind.InvalidBonusTaskState,
                    "Invalid committed bonus task state.", ex);
            }
        });

    public static BonusHistory History(IReadOnlyDictionary<string, object?> r) => Read(r, () => new BonusHistory(
        Long(Required(r, "BonusHistoryId")), String(Required(r, "BonusTaskId")), Long(Required(r, "UserUID")),
        Long(Required(r, "EligibilityEntryId")), Long(Required(r, "ActivityUID")), Day(Required(r, "BusinessDay")),
        String(Required(r, "ActivitySnapshotJson")), Decimal(Required(r, "BonusAmount")),
        Decimal(Required(r, "RequiredWagerAmount")), Decimal(Required(r, "CurrentWagerAmount")),
        Decimal(Required(r, "ConvertedAmount")), EnumValue<CloseReason>(Required(r, "CloseReason")),
        Time(Required(r, "ClaimedAt")), Time(Required(r, "ClosedAt")), Time(Required(r, "CreatedAt"))),
        DomainInvariantValidator.ValidateHistory);
}
