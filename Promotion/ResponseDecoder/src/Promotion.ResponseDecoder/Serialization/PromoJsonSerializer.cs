using Newtonsoft.Json;

namespace Promotion.ResponseDecoder.Serialization;

internal static class PromoJsonSerializer
{
    internal static readonly JsonSerializerSettings Settings = new()
    {
        DateParseHandling = DateParseHandling.None,
        FloatParseHandling = FloatParseHandling.Decimal,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };
}
