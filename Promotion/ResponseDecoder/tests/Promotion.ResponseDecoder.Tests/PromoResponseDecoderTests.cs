using Promotion.ResponseDecoder.Contracts;
using Promotion.ResponseDecoder.Decoding;
using Xunit;

namespace Promotion.ResponseDecoder.Tests;

public sealed class PromoResponseDecoderTests
{
    private readonly PromoResponseDecoder decoder = new();

    [Theory]
    [InlineData(PromoCommands.GetActivitiesResponse, "get-activities.json")]
    [InlineData(PromoCommands.GetPlayerOffersResponse, "get-player-offers.json")]
    public void Decode_PlayerPromotionPagePayload_ReturnsTypedPayload(string command, string fixtureName)
    {
        PromoDecodeResult result = Decode(command, LoadFixture(fixtureName));

        Assert.True(result.IsSupportedCommand);
        Assert.True(result.Success);
        PromoGetPlayerOffersPayload payload = Assert.IsType<PromoGetPlayerOffersPayload>(result.Payload);
        if (command == PromoCommands.GetPlayerOffersResponse)
        {
            PromoPlayerPromotionActivityPayload activity = Assert.Single(payload.Activities);
            Assert.Equal(1001, activity.ActivityUID);
            Assert.Equal("Welcome bonus", activity.Activity.ActivityInfo);
        }
    }

    [Theory]
    [InlineData(PromoCommands.GetGamesResponse, "get-games.json", typeof(PromoGetGamesPayload))]
    [InlineData(PromoCommands.ClaimResponse, "claim.json", typeof(PromoClaimPayload))]
    [InlineData(PromoCommands.GetTaskResponse, "get-task.json", typeof(PromoGetTaskPayload))]
    [InlineData(PromoCommands.AbandonTaskResponse, "abandon.json", typeof(PromoAbandonPayload))]
    [InlineData(PromoCommands.GetHistoryResponse, "get-history.json", typeof(PromoGetHistoryPayload))]
    public void Decode_FormalPayload_ReturnsExpectedType(string command, string fixtureName, Type expectedPayloadType)
    {
        PromoDecodeResult result = Decode(command, LoadFixture(fixtureName));

        Assert.True(result.Success);
        Assert.NotNull(result.Payload);
        Assert.Equal(expectedPayloadType, result.Payload.GetType());
    }

    [Fact]
    public void Decode_ClaimUnlockNotImplementedWithoutPayload_ReturnsFailure()
    {
        PromoDecodeResult result = Decode(PromoCommands.ClaimUnlockResponse, null, "NotImplemented");

        Assert.True(result.IsSupportedCommand);
        Assert.False(result.Success);
        Assert.Equal("NotImplemented", result.ErrorCode);
        Assert.Null(result.Payload);
    }

    [Fact]
    public void Decode_ErrorAndValidPayload_PreservesPayloadAndError()
    {
        PromoDecodeResult result = Decode(PromoCommands.ClaimResponse, LoadFixture("claim.json"), "WalletInstructionFailed");

        Assert.False(result.Success);
        Assert.Equal("WalletInstructionFailed", result.ErrorCode);
        Assert.IsType<PromoClaimPayload>(result.Payload);
    }

    [Fact]
    public void Decode_ValidPayloadWithoutError_UsesResponseMetadata()
    {
        PromoDecodeResult result = decoder.Decode(new PromoCommonResponse(
            PromoCommands.GetGamesResponse,
            42,
            new Dictionary<string, string>
            {
                ["Payload"] = LoadFixture("get-games.json"),
                ["RequestId"] = "request-42",
                ["IsMock"] = "TrUe"
            },
            "test message"));

        Assert.True(result.Success);
        Assert.Equal(42, result.UserUID);
        Assert.Equal("request-42", result.RequestId);
        Assert.True(result.IsMock);
        Assert.Equal("test message", result.Message);
    }

    [Fact]
    public void Decode_MissingPayloadWithoutError_ReturnsMissingResponsePayload()
    {
        PromoDecodeResult result = Decode(PromoCommands.GetGamesResponse, null);

        Assert.False(result.Success);
        Assert.Equal("MissingResponsePayload", result.ErrorCode);
    }

    [Fact]
    public void Decode_InvalidJson_ReturnsInvalidResponsePayload()
    {
        PromoDecodeResult result = Decode(PromoCommands.GetHistoryResponse, "{bad}");

        Assert.False(result.Success);
        Assert.Equal("InvalidResponsePayload", result.ErrorCode);
        Assert.Null(result.Payload);
    }

    [Fact]
    public void Decode_MissingRequiredProperty_ReturnsInvalidResponsePayload()
    {
        PromoDecodeResult result = Decode(PromoCommands.GetGamesResponse, "{\"ActivityUID\":1001}");

        Assert.False(result.Success);
        Assert.Equal("InvalidResponsePayload", result.ErrorCode);
    }

    [Fact]
    public void Decode_TypeMismatchedProperty_ReturnsInvalidResponsePayload()
    {
        PromoDecodeResult result = Decode(PromoCommands.GetGamesResponse, "{\"ActivityUID\":1001,\"GameServerList\":1}");

        Assert.False(result.Success);
        Assert.Equal("InvalidResponsePayload", result.ErrorCode);
    }

    [Fact]
    public void Decode_UnknownProperty_IsForwardCompatible()
    {
        const string payload = "{\"ActivityUID\":1001,\"GameServerList\":\"Wukong\",\"FutureField\":true}";

        PromoDecodeResult result = Decode(PromoCommands.GetGamesResponse, payload);

        Assert.True(result.Success);
        Assert.IsType<PromoGetGamesPayload>(result.Payload);
    }

    [Fact]
    public void Decode_NullData_DoesNotThrow()
    {
        PromoDecodeResult result = decoder.Decode(new PromoCommonResponse(PromoCommands.GetTaskResponse, 42, null, "message"));

        Assert.False(result.Success);
        Assert.Equal("MissingResponsePayload", result.ErrorCode);
    }

    [Fact]
    public void Decode_EmptyDataAndNullPayloadValue_DoNotThrow()
    {
        PromoDecodeResult emptyDataResult = Decode(PromoCommands.GetTaskResponse, null);
        PromoDecodeResult nullPayloadResult = decoder.Decode(new PromoCommonResponse(
            PromoCommands.GetTaskResponse,
            42,
            new Dictionary<string, string> { ["Payload"] = null! },
            "message"));

        Assert.Equal("MissingResponsePayload", emptyDataResult.ErrorCode);
        Assert.Equal("MissingResponsePayload", nullPayloadResult.ErrorCode);
    }

    [Fact]
    public void Decode_UnknownCommand_ReturnsUnsupportedResult()
    {
        PromoDecodeResult result = Decode("PromoFutureResponse", "{}");

        Assert.False(result.IsSupportedCommand);
        Assert.False(result.Success);
        Assert.Null(result.Payload);
    }

    private PromoDecodeResult Decode(string command, string? payload, string? errorCode = null)
    {
        Dictionary<string, string> data = new();
        if (payload is not null)
            data["Payload"] = payload;
        if (errorCode is not null)
            data["ErrorCode"] = errorCode;

        return decoder.Decode(new PromoCommonResponse(command, 42, data, "message"));
    }

    private static string LoadFixture(string fixtureName) =>
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", fixtureName));
}
