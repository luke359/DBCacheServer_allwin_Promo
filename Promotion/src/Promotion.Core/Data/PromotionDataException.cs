namespace Promotion.Core.Data;

public enum PromotionDataErrorKind : byte
{
    Validation = 1, DuplicateKey = 2, Deadlock = 3, LockWaitTimeout = 4,
    Connection = 5, CommitOutcomeUnknown = 6, DataCorruption = 7, ConcurrencyConflict = 8,
    InvalidBonusTaskState = 9,
    Unexpected = 255
}

public sealed class PromotionDataException : Exception
{
    public PromotionDataErrorKind Kind { get; }
    public string? ConstraintName { get; }

    public PromotionDataException(PromotionDataErrorKind kind, string message,
        Exception? inner = null, string? constraintName = null) : base(message, inner)
    {
        Kind = kind;
        ConstraintName = constraintName;
    }
}
