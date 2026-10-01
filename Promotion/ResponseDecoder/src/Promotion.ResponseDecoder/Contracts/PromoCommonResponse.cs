namespace Promotion.ResponseDecoder.Contracts;

/// <summary>DBCache 回覆的中立輸入模型。</summary>
public sealed record PromoCommonResponse(
    string Command,
    long UserUID,
    IReadOnlyDictionary<string, string>? Data,
    string? Message);
