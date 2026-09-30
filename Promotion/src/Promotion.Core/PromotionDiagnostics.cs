using Promotion.Core.Contracts;

namespace Promotion.Core;

/// <summary>提供核心服務的內部失敗診斷；實作不得將例外再次拋出。</summary>
public interface IPromotionDiagnostics
{
    void RecordFailure(string correlationId, PromotionErrorCode errorCode, Exception exception);
}

/// <summary>將僅供內部追查的例外詳細資訊輸出至主控台。</summary>
public sealed class ConsolePromotionDiagnostics : IPromotionDiagnostics
{
    public void RecordFailure(string correlationId, PromotionErrorCode errorCode, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Console.WriteLine(
            "Promotion Core internal failure: ErrorCode=" + errorCode +
            " CorrelationId=" + correlationId + Environment.NewLine + exception);
    }
}

internal sealed class NullPromotionDiagnostics : IPromotionDiagnostics
{
    public static readonly NullPromotionDiagnostics Instance = new();
    private NullPromotionDiagnostics() { }
    public void RecordFailure(string correlationId, PromotionErrorCode errorCode, Exception exception) { }
}
