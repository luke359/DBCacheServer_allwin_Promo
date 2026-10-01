namespace Promotion.ResponseDecoder.Contracts;

/// <summary>優惠回覆解包後、供 Game Server 轉接層使用的結果。</summary>
public sealed class PromoDecodeResult
{
    public bool IsSupportedCommand { get; init; }
    public string Command { get; init; } = string.Empty;
    public long UserUID { get; init; }
    public string RequestId { get; init; } = string.Empty;
    public bool IsMock { get; init; }
    public bool Success { get; init; }
    public string ErrorCode { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public PromoPayloadBase? Payload { get; init; }
}
