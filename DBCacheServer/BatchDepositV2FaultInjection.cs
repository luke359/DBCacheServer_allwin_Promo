using System;

namespace DBCacheServer
{
    // This is intentionally opt-in: production behaviour is unchanged unless the exact
    // test-only environment variable is set by the isolated integration-test process.
    internal static class BatchDepositV2FaultInjection
    {
        internal static void ThrowIfEnabled(string point)
        {
            if (string.Equals(Environment.GetEnvironmentVariable("BATCH_DEPOSIT_V2_FAULT_POINT"), point, StringComparison.Ordinal))
                throw new InvalidOperationException("Injected BatchDepositV2 failure at " + point);
        }
    }
}
