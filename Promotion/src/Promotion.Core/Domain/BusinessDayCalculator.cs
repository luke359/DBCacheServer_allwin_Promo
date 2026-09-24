namespace Promotion.Core.Domain;

public static class BusinessDayCalculator
{
    public static bool IsValidCutover(TimeSpan cutover) =>
        cutover >= TimeSpan.Zero && cutover < TimeSpan.FromDays(1) && cutover.Ticks % TimeSpan.TicksPerSecond == 0;

    public static DateOnly Calculate(DateTime localTime, TimeSpan cutover)
    {
        if (!IsValidCutover(cutover)) throw new ArgumentOutOfRangeException(nameof(cutover));
        if (localTime.Kind == DateTimeKind.Utc) throw new ArgumentException("Local time required.", nameof(localTime));
        return DateOnly.FromDateTime(localTime - cutover);
    }
}
