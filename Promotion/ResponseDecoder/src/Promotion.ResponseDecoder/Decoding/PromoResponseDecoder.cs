using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Promotion.ResponseDecoder.Contracts;
using Promotion.ResponseDecoder.Serialization;

namespace Promotion.ResponseDecoder.Decoding;

/// <summary>將 DBCache 的優惠 CommonInfoData 回覆安全解包為中立 DTO。</summary>
public sealed class PromoResponseDecoder : IPromoResponseDecoder
{
    public PromoDecodeResult Decode(PromoCommonResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        string command = response.Command ?? string.Empty;
        string requestId = GetDataValue(response.Data, "RequestId");
        string errorCode = GetDataValue(response.Data, "ErrorCode");
        string payloadJson = GetDataValue(response.Data, "Payload");
        bool isMock = string.Equals(GetDataValue(response.Data, "IsMock"), "true", StringComparison.OrdinalIgnoreCase);
        string message = response.Message ?? string.Empty;

        if (!PromoCommands.IsSupported(command))
        {
            return CreateResult(false, command, response.UserUID, requestId, isMock, false, string.Empty, message, null);
        }

        if (command == PromoCommands.ClaimUnlockResponse)
            return DecodeClaimUnlock(command, response.UserUID, requestId, isMock, errorCode, payloadJson, message);

        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            string resolvedErrorCode = string.IsNullOrWhiteSpace(errorCode) ? "MissingResponsePayload" : errorCode;
            return CreateResult(true, command, response.UserUID, requestId, isMock, false, resolvedErrorCode, message, null);
        }

        if (!TryDecodePayload(command, payloadJson, out PromoPayloadBase? payload))
        {
            return CreateResult(true, command, response.UserUID, requestId, isMock, false, "InvalidResponsePayload", message, null);
        }

        bool success = string.IsNullOrWhiteSpace(errorCode);
        return CreateResult(true, command, response.UserUID, requestId, isMock, success, errorCode, message, payload);
    }

    private static PromoDecodeResult DecodeClaimUnlock(string command, long userUid, string requestId, bool isMock,
        string errorCode, string payloadJson, string message)
    {
        if (!string.IsNullOrWhiteSpace(payloadJson))
        {
            return CreateResult(true, command, userUid, requestId, isMock, false, "InvalidResponsePayload", message, null);
        }

        string resolvedErrorCode = string.IsNullOrWhiteSpace(errorCode) ? "MissingResponsePayload" : errorCode;
        return CreateResult(true, command, userUid, requestId, isMock, false, resolvedErrorCode, message, null);
    }

    private static bool TryDecodePayload(string command, string payloadJson, out PromoPayloadBase? payload)
    {
        payload = null;
        try
        {
            using StringReader textReader = new(payloadJson);
            using JsonTextReader jsonReader = new(textReader)
            {
                DateParseHandling = DateParseHandling.None
            };
            JToken token = JToken.ReadFrom(jsonReader);
            if (token is not JObject payloadObject || !PromoPayloadContractValidator.IsValid(command, payloadObject))
                return false;

            payload = command switch
            {
                PromoCommands.GetActivitiesResponse or PromoCommands.GetPlayerOffersResponse =>
                    Deserialize<PromoGetPlayerOffersPayload>(payloadJson),
                PromoCommands.GetGamesResponse => Deserialize<PromoGetGamesPayload>(payloadJson),
                PromoCommands.ClaimResponse => Deserialize<PromoClaimPayload>(payloadJson),
                PromoCommands.GetTaskResponse => Deserialize<PromoGetTaskPayload>(payloadJson),
                PromoCommands.AbandonTaskResponse => Deserialize<PromoAbandonPayload>(payloadJson),
                PromoCommands.GetHistoryResponse => Deserialize<PromoGetHistoryPayload>(payloadJson),
                _ => null
            };

            return payload is not null;
        }
        catch (JsonException)
        {
            return false;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (OverflowException)
        {
            return false;
        }
    }

    private static T? Deserialize<T>(string payloadJson) where T : PromoPayloadBase =>
        JsonConvert.DeserializeObject<T>(payloadJson, PromoJsonSerializer.Settings);

    private static string GetDataValue(IReadOnlyDictionary<string, string>? data, string key)
    {
        if (data is null || !data.TryGetValue(key, out string? value) || value is null)
            return string.Empty;
        return value;
    }

    private static PromoDecodeResult CreateResult(bool isSupportedCommand, string command, long userUid,
        string requestId, bool isMock, bool success, string errorCode, string message, PromoPayloadBase? payload) =>
        new()
        {
            IsSupportedCommand = isSupportedCommand,
            Command = command,
            UserUID = userUid,
            RequestId = requestId,
            IsMock = isMock,
            Success = success,
            ErrorCode = errorCode,
            Message = message,
            Payload = payload
        };
}
