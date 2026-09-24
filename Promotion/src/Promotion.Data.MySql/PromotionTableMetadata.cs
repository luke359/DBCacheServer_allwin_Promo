namespace Promotion.Data.MySql;

public static class PromotionTableMetadata
{
    public const string Activity = "PromotionActivity";
    public const string Event = "PromotionTriggerEvent";
    public const string Eligibility = "EligibilityEntry";
    public const string Status = "PromotionBonusStatus";
    public const string History = "PromotionBonusHistory";

    public static readonly IReadOnlyList<string> ActivityFields = Columns(
        "ActivityUID ActivityInfo BonusType FixedBonusAmount MaxBonusAmount DepositPercentage MinimumDepositAmount WagerMultiplier WagerCalculationType MaxBetAmount GameServerList WagerContributionRate ConvertType FixedConvertedAmount MaxBalanceConvertedAmount DailyClaimLimit StartDate EndDate WeekdayMask ActivityStatus TriggerType IsNonStackable ExclusiveGroup CreatedAt UpdatedAt");
    public static readonly IReadOnlyList<string> EventFields = Columns(
        "EventId UserUID TriggerType EventTime BusinessDay EligibleDepositAmount CreatedAt");
    public static readonly IReadOnlyList<string> EligibilityFields = Columns(
        "EligibilityEntryId EventId UserUID ActivityUID BusinessDay TriggerType EligibleDepositAmount Status ClaimedBonusTaskId ExcludedByBonusTaskId StatusChangedAt CreatedAt");
    public static readonly IReadOnlyList<string> StatusFields = Columns(
        "UserUID TaskState BonusTaskId EligibilityEntryId ActivityUID BusinessDay LastFirstLoginBusinessDay LastFirstDepositBusinessDay ActivitySnapshotJson BonusAmount RequiredWagerAmount CurrentWagerAmount ClaimedAt CreatedAt UpdatedAt");
    public static readonly IReadOnlyList<string> HistoryFields = Columns(
        "BonusHistoryId BonusTaskId UserUID EligibilityEntryId ActivityUID BusinessDay ActivitySnapshotJson BonusAmount RequiredWagerAmount CurrentWagerAmount ConvertedAmount CloseReason ClaimedAt ClosedAt CreatedAt");

    private static readonly IReadOnlyDictionary<string, string[]> Constraints = new Dictionary<string, string[]>(StringComparer.Ordinal)
    {
        [Activity] = new[] { "PRIMARY" },
        [Event] = new[] { "PRIMARY" },
        [Eligibility] = new[] { "PRIMARY", "UK_EligibilityEntry_EventId_ActivityUID", "UK_EligibilityEntry_ClaimedBonusTaskId" },
        [Status] = new[] { "PRIMARY", "UK_PromotionBonusStatus_BonusTaskId" },
        [History] = new[] { "PRIMARY", "UK_PromotionBonusHistory_BonusTaskId" }
    };

    public static string? KnownConstraint(string? table, string? providerName)
    {
        if (table is null || providerName is null || !Constraints.TryGetValue(table, out var allowed)) return null;
        var name = providerName.StartsWith(table + ".", StringComparison.OrdinalIgnoreCase)
            ? providerName[(table.Length + 1)..] : providerName;
        return allowed.Contains(name, StringComparer.OrdinalIgnoreCase) ? table + "." + name : null;
    }

    private static IReadOnlyList<string> Columns(string value) => Array.AsReadOnly(value.Split(' '));

    public static void Validate(string table, IEnumerable<string> fields)
    {
        var allowed = table switch
        {
            Activity => ActivityFields, Event => EventFields, Eligibility => EligibilityFields,
            Status => StatusFields, History => HistoryFields,
            _ => throw new ArgumentException("Unknown promotion table.", nameof(table))
        };
        foreach (var field in fields)
            if (!allowed.Contains(field, StringComparer.Ordinal))
                throw new ArgumentException("Unknown promotion column.", nameof(fields));
    }

    public static void Validate(Query query)
    {
        Validate(query.Table, query.Fields.Concat(query.Where.Select(x => x.Column)).Concat(query.Order.Select(x => x.Column)));
        if (query.Fields.Count == 0 || query.Limit is <= 0 or > 1000 || query.Offset < 0)
            throw new ArgumentException("Invalid promotion query.", nameof(query));
    }

    public static void Validate(DeleteQuery query)
    {
        Validate(query.Table, query.Where.Select(x => x.Column).Concat(query.Order.Select(x => x.Column)));
        if (query.Where.Count == 0 || query.Limit is < 1 or > 1000)
            throw new ArgumentException("Invalid promotion delete.", nameof(query));
    }
}
