using Newtonsoft.Json;
using System;

namespace DBCacheServer
{
    internal sealed class BatchDepositV2OperationsOptions
    {
        internal int MaxPublishAttempts = ReadInt("BDV2_OUTBOX_MAX_ATTEMPTS", 8, 1, 100);
        internal int RetryWarningAttempts = ReadInt("BDV2_OUTBOX_RETRY_WARNING_ATTEMPTS", 3, 1, 100);
        internal int PendingWarningSeconds = ReadInt("BDV2_PENDING_WARNING_SECONDS", 300, 1, 86400);
        internal int PendingCriticalSeconds = ReadInt("BDV2_PENDING_CRITICAL_SECONDS", 900, 1, 86400);
        internal int MonitorIntervalSeconds = ReadInt("BDV2_MONITOR_INTERVAL_SECONDS", 60, 5, 3600);

        private static int ReadInt(string name, int fallback, int minimum, int maximum)
        {
            if (!int.TryParse(Environment.GetEnvironmentVariable(name), out int value))
                return fallback;
            return Math.Max(minimum, Math.Min(maximum, value));
        }
    }

    internal static class BatchDepositV2OperationalLog
    {
        internal static void Write(string severity, string component, string eventName, object detail)
        {
            string line = JsonConvert.SerializeObject(new
            {
                timestampUtc = DateTime.UtcNow,
                system = "BatchDepositV2",
                severity,
                component,
                eventName,
                detail
            });

            if (string.Equals(severity, "warning", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(severity, "critical", StringComparison.OrdinalIgnoreCase))
                Console.Error.WriteLine(line);
            else
                Console.WriteLine(line);
        }
    }
}
