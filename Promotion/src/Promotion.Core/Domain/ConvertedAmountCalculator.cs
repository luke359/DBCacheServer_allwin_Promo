using Promotion.Core.Contracts;

namespace Promotion.Core.Domain;

public sealed class BalanceConvertFormulaException : Exception
{
    public BalanceConvertFormulaException(Exception? inner = null) : base("Balance conversion formula failed.", inner) { }
}

public static class ConvertedAmountCalculator
{
    public static decimal Calculate(ActivitySnapshotDto snapshot, BalanceConvertFormula? formula,
        BalanceConvertContext? context)
    {
        if (snapshot.ConvertType == ConvertType.Fixed) return snapshot.FixedConvertedAmount;
        if (snapshot.ConvertType != ConvertType.Balance || formula is null || context is null)
            throw new ArgumentException("Invalid conversion inputs.");
        try
        {
            var result = formula(context);
            if (double.IsNaN(result) || double.IsInfinity(result) || result < 0)
                throw new BalanceConvertFormulaException();
            var converted = PromotionAmountCalculator.Truncate4(checked((decimal)result));
            return snapshot.MaxBalanceConvertedAmount is { } maximum
                ? Math.Min(converted, maximum) : converted;
        }
        catch (BalanceConvertFormulaException) { throw; }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            throw new BalanceConvertFormulaException(ex);
        }
    }
}
