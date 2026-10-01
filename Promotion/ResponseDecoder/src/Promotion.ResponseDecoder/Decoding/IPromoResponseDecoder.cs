using Promotion.ResponseDecoder.Contracts;

namespace Promotion.ResponseDecoder.Decoding;

public interface IPromoResponseDecoder
{
    PromoDecodeResult Decode(PromoCommonResponse response);
}
