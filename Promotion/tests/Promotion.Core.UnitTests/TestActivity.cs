using Promotion.Core.Contracts;
using Promotion.Core.Domain;

namespace Promotion.Core.UnitTests;

internal static class TestActivity
{
    public static readonly DateTime Now = new DateTime(2026, 9, 15, 10, 0, 0, DateTimeKind.Unspecified).AddTicks(1_234_560);
    public const string TaskId = "11111111-1111-4111-8111-111111111111";

    public static PromotionActivity Create() => new(
        20001, "活動", BonusType.FixedAmount, 10, 100, 30, 10, 30,
        WagerCalculationType.BonusOnly, 5, null, 100m, ConvertType.Fixed, 5, 100, 1,
        new DateOnly(2026, 9, 1), new DateOnly(2026, 10, 1), "1111111",
        ActivityStatus.Active, TriggerType.Deposit, false, null, Now, Now);

    public static ActivitySnapshotDto Snapshot() => ActivitySnapshotFactory.Capture(Create(), Now);

    public static EligibilityEntry Entry() => new(1, "evt-DOM-001", 10001, 20001,
        new DateOnly(2026, 9, 15), TriggerType.Deposit, 10m, EligibilityStatus.Available,
        null, null, Now, Now);

    public static BonusStatus Active() => new(10001, BonusTaskState.InProgress, TaskId, 1, 20001,
        new DateOnly(2026, 9, 15), new DateOnly(2026, 9, 15), new DateOnly(2026, 9, 14),
        ActivitySnapshotSerializer.Serialize(Snapshot()),
        10m, 300m, 100m, Now, Now, Now);
}
