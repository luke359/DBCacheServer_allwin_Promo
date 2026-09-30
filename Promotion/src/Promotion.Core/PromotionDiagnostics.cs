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

/// <summary>可由 Host 在執行期間安全地開關的主控台診斷器。</summary>
public sealed class SwitchablePromotionDiagnostics : IPromotionDiagnostics
{
    private int enabled;

    public SwitchablePromotionDiagnostics(bool enabled = true) =>
        this.enabled = enabled ? 1 : 0;

    public bool Enabled => Volatile.Read(ref enabled) == 1;

    public void SetEnabled(bool value) =>
        Volatile.Write(ref enabled, value ? 1 : 0);

    public void RecordFailure(string correlationId, PromotionErrorCode errorCode, Exception exception)
    {
        if (!Enabled) return;
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
