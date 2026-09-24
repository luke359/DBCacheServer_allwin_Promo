using System.Globalization;
using System.Text.Json;
using Promotion.Core.Contracts;

namespace Promotion.Core.Domain;

public static class ActivitySnapshotSerializer
{
    private const string DateFormat = "yyyy-MM-dd";
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";
    private static readonly string[] V1Keys =
    {
        "SnapshotVersion", "ActivityUID", "ActivityInfo", "BonusType", "FixedBonusAmount",
        "MaxBonusAmount", "DepositPercentage", "MinimumDepositAmount", "WagerMultiplier",
        "MaxBetAmount", "WagerContributionRate", "ConvertType", "FixedConvertedAmount",
        "MaxBalanceConvertedAmount", "DailyClaimLimit", "StartDate", "EndDate", "WeekdayMask",
        "TriggerType", "IsNonStackable", "ExclusiveGroup", "CapturedAt"
    };
    private static readonly string[] V2Keys =
        V1Keys.Concat(new[] { "WagerCalculationType" }).ToArray();

    public static string Serialize(ActivitySnapshotDto snapshot)
    {
        Validate(snapshot);
        if (snapshot.SnapshotVersion == 1)
            return JsonSerializer.Serialize(new
            {
                snapshot.SnapshotVersion, snapshot.ActivityUID, snapshot.ActivityInfo,
                BonusType = (byte)snapshot.BonusType, snapshot.FixedBonusAmount, snapshot.MaxBonusAmount,
                snapshot.DepositPercentage, snapshot.MinimumDepositAmount, snapshot.WagerMultiplier,
                snapshot.MaxBetAmount, snapshot.WagerContributionRate, ConvertType = (byte)snapshot.ConvertType,
                snapshot.FixedConvertedAmount, snapshot.MaxBalanceConvertedAmount, snapshot.DailyClaimLimit,
                StartDate = snapshot.StartDate?.ToString(DateFormat, CultureInfo.InvariantCulture),
                EndDate = snapshot.EndDate?.ToString(DateFormat, CultureInfo.InvariantCulture),
                snapshot.WeekdayMask, TriggerType = (byte)snapshot.TriggerType, snapshot.IsNonStackable,
                snapshot.ExclusiveGroup,
                CapturedAt = snapshot.CapturedAt.ToString(DateTimeFormat, CultureInfo.InvariantCulture)
            });
        return JsonSerializer.Serialize(new
        {
            snapshot.SnapshotVersion, snapshot.ActivityUID, snapshot.ActivityInfo,
            BonusType = (byte)snapshot.BonusType, snapshot.FixedBonusAmount, snapshot.MaxBonusAmount,
            snapshot.DepositPercentage, snapshot.MinimumDepositAmount, snapshot.WagerMultiplier,
            WagerCalculationType = (byte)snapshot.WagerCalculationType,
            snapshot.MaxBetAmount, snapshot.WagerContributionRate, ConvertType = (byte)snapshot.ConvertType,
            snapshot.FixedConvertedAmount, snapshot.MaxBalanceConvertedAmount, snapshot.DailyClaimLimit,
            StartDate = snapshot.StartDate?.ToString(DateFormat, CultureInfo.InvariantCulture),
            EndDate = snapshot.EndDate?.ToString(DateFormat, CultureInfo.InvariantCulture),
            snapshot.WeekdayMask, TriggerType = (byte)snapshot.TriggerType, snapshot.IsNonStackable,
            snapshot.ExclusiveGroup,
            CapturedAt = snapshot.CapturedAt.ToString(DateTimeFormat, CultureInfo.InvariantCulture)
        });
    }

    public static ActivitySnapshotDto Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) throw new DomainInvariantException("Empty ActivitySnapshot.");
        try
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                throw new DomainInvariantException("Invalid ActivitySnapshot fields.");
            var version = root.GetProperty("SnapshotVersion").GetInt32();
            if (version is not (1 or 2)) throw new UnsupportedSnapshotVersionException(version);
            var keys = version == 1 ? V1Keys : V2Keys;
            if (root.EnumerateObject().Count() != keys.Length ||
                root.EnumerateObject().Select(p => p.Name).Distinct(StringComparer.Ordinal).Count() != keys.Length ||
                keys.Any(key => !root.TryGetProperty(key, out _)))
                throw new DomainInvariantException("Invalid ActivitySnapshot fields.");
            var snapshot = new ActivitySnapshotDto(
                version, Long(root, "ActivityUID"), NullableString(root, "ActivityInfo"),
                (BonusType)Byte(root, "BonusType"), Int(root, "FixedBonusAmount"),
                Int(root, "MaxBonusAmount"), Int(root, "DepositPercentage"),
                NullableInt(root, "MinimumDepositAmount"), Int(root, "WagerMultiplier"),
                version == 1 ? WagerCalculationType.BonusOnly :
                    (WagerCalculationType)Byte(root, "WagerCalculationType"),
                NullableInt(root, "MaxBetAmount"), Decimal(root, "WagerContributionRate"),
                (ConvertType)Byte(root, "ConvertType"), Int(root, "FixedConvertedAmount"),
                NullableInt(root, "MaxBalanceConvertedAmount"), Int(root, "DailyClaimLimit"),
                NullableDate(root, "StartDate"), NullableDate(root, "EndDate"),
                root.GetProperty("WeekdayMask").GetString()!, (TriggerType)Byte(root, "TriggerType"),
                root.GetProperty("IsNonStackable").GetBoolean(), NullableString(root, "ExclusiveGroup"),
                DateTime.ParseExact(root.GetProperty("CapturedAt").GetString()!, DateTimeFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None));
            Validate(snapshot);
            return snapshot;
        }
        catch (UnsupportedSnapshotVersionException) { throw; }
        catch (DomainInvariantException) { throw; }
        catch (Exception ex) when (ex is JsonException or FormatException or InvalidOperationException or OverflowException or ArgumentException or KeyNotFoundException)
        {
            throw new DomainInvariantException("Invalid ActivitySnapshot JSON.");
        }
    }

    public static void Validate(ActivitySnapshotDto snapshot)
    {
        if (snapshot.SnapshotVersion is not (1 or 2))
            throw new UnsupportedSnapshotVersionException(snapshot.SnapshotVersion);
        if (snapshot.SnapshotVersion == 1 &&
            (snapshot.WagerCalculationType != WagerCalculationType.BonusOnly ||
             snapshot.MaxBetAmount is null || snapshot.MaxBalanceConvertedAmount is null))
            throw new DomainInvariantException("Invalid version 1 ActivitySnapshot.");
        var activity = new PromotionActivity(snapshot.ActivityUID, snapshot.ActivityInfo,
            snapshot.BonusType, snapshot.FixedBonusAmount, snapshot.MaxBonusAmount,
            snapshot.DepositPercentage, snapshot.MinimumDepositAmount, snapshot.WagerMultiplier,
            snapshot.WagerCalculationType, snapshot.MaxBetAmount, null,
            snapshot.WagerContributionRate, snapshot.ConvertType,
            snapshot.FixedConvertedAmount, snapshot.MaxBalanceConvertedAmount, snapshot.DailyClaimLimit,
            snapshot.StartDate, snapshot.EndDate, snapshot.WeekdayMask, ActivityStatus.Active, snapshot.TriggerType,
            snapshot.IsNonStackable, snapshot.ExclusiveGroup, snapshot.CapturedAt, snapshot.CapturedAt);
        DomainInvariantValidator.ValidateActivity(activity);
    }

    private static int Int(JsonElement root, string name) => root.GetProperty(name).GetInt32();
    private static long Long(JsonElement root, string name) => root.GetProperty(name).GetInt64();
    private static byte Byte(JsonElement root, string name) => root.GetProperty(name).GetByte();
    private static decimal Decimal(JsonElement root, string name) => root.GetProperty(name).GetDecimal();
    private static int? NullableInt(JsonElement root, string name) => root.GetProperty(name).ValueKind == JsonValueKind.Null ? null : Int(root, name);
    private static string? NullableString(JsonElement root, string name) => root.GetProperty(name).ValueKind == JsonValueKind.Null ? null : root.GetProperty(name).GetString();
    private static DateOnly? NullableDate(JsonElement root, string name)
    {
        var value = NullableString(root, name);
        return value is null ? null : DateOnly.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
    }
}

public sealed class UnsupportedSnapshotVersionException : Exception
{
    public UnsupportedSnapshotVersionException(int version) : base($"Unsupported ActivitySnapshot version: {version}.") { }
}
