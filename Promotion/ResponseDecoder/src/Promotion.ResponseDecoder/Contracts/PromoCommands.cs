namespace Promotion.ResponseDecoder.Contracts;

/// <summary>目前正式支援的 DBCache 優惠回覆 Command。</summary>
public static class PromoCommands
{
    public const string GetActivitiesResponse = "PromoGetActivitiesResponse";
    public const string GetPlayerOffersResponse = "PromoGetPlayerOffersResponse";
    public const string GetGamesResponse = "PromoGetGamesResponse";
    public const string ClaimResponse = "PromoClaimResponse";
    public const string ClaimUnlockResponse = "PromoClaimUnlockResponse";
    public const string GetTaskResponse = "PromoGetTaskResponse";
    public const string AbandonTaskResponse = "PromoAbandonTaskResponse";
    public const string GetHistoryResponse = "PromoGetHistoryResponse";

    public static bool IsSupported(string? command) => command is
        GetActivitiesResponse or GetPlayerOffersResponse or GetGamesResponse or ClaimResponse or
        ClaimUnlockResponse or GetTaskResponse or AbandonTaskResponse or GetHistoryResponse;
}
