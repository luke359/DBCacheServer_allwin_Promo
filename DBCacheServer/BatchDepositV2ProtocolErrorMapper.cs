using Protocol;

namespace DBCacheServer
{
    internal static class BatchDepositV2ProtocolErrorMapper
    {
        internal static ErrorCode Map(BatchDepositV2Request request, BatchDepositV2Result result)
        {
            if (result != null && result.Status == BatchDepositV2Status.Succeeded)
                return ErrorCode.Ok;

            // H5 API keeps its existing V2 error contract. Legacy Web operations expose
            // the same Protocol.ErrorCode categories as PlayerBalanceDeal.
            if (request == null || request.ActorType != BatchDepositV2ActorType.WebUser)
                return ErrorCode.CustomError;

            switch (result?.FailureCode)
            {
                case "OverDepositUnit":
                    return ErrorCode.InvalidOperation;
                case "PlayerBalanceLimitExceeded":
                    return ErrorCode.IllegalUser;
                case "KeyOutLimit":
                    return ErrorCode.InvalidMinStartRateLimit;
                default:
                    return ErrorCode.CustomError;
            }
        }
    }
}
