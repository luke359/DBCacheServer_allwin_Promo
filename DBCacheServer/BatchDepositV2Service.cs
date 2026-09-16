using Protocol;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DBCacheServer
{
    [Obsolete("Database-authoritative implementation. Runtime handlers must use MemoryAuthoritativeBatchDepositV2Service.")]
    internal sealed class BatchDepositV2Service
    {
        private readonly CacheManeger cache;
        private readonly BatchDepositV2Repository repository;

        internal BatchDepositV2Service(CacheManeger cache, MysqlAcess mysql)
        {
            this.cache = cache;
            repository = new BatchDepositV2Repository(mysql);
        }

        internal BatchDepositV2Result Handle(BatchDepositV2Request request)
        {
            string validationError = ValidateRequest(request);
            string requestHash = CreateRequestHash(request);
            if (validationError != null)
                return new BatchDepositV2Result
                {
                    BatchId = request?.BatchId,
                    IdempotencyKey = request?.IdempotencyKey,
                    RequestHash = requestHash,
                    Status = BatchDepositV2Status.Failed,
                    FailureCode = validationError,
                    Details = new System.Collections.Generic.List<BatchDepositV2ResultDetail>()
                };

            double playerBalanceLimit = 0;
            double depositUnit = 0;
            cache.GetPlayerBalanceLimit(ref playerBalanceLimit, ref depositUnit);
            bool committed;
            BatchDepositV2Result result = repository.Execute(request, requestHash, playerBalanceLimit, depositUnit,
                cache.GetCountryExtraBonusString(), out committed);
            if (committed)
                cache.SynchronizeCommittedBatchDepositV2(result);
            BatchDepositV2Metrics.Record(result, committed);
            return result;
        }

        internal BatchDepositV2QueryResult Query(BatchDepositV2QueryRequest request)
        {
            if (request == null || request.ActorType == BatchDepositV2ActorType.None ||
                string.IsNullOrWhiteSpace(request.ActorId) || string.IsNullOrWhiteSpace(request.IdempotencyKey) ||
                request.IdempotencyKey.Length > 128)
                return new BatchDepositV2QueryResult { Found = false, FailureCode = "InvalidRequest" };
            return repository.Query(request);
        }

        private static string ValidateRequest(BatchDepositV2Request request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.BatchId) || string.IsNullOrWhiteSpace(request.IdempotencyKey) ||
                string.IsNullOrWhiteSpace(request.ActorId) || string.IsNullOrWhiteSpace(request.PayerManagerId) || request.ActorType == BatchDepositV2ActorType.None)
                return "InvalidRequest";
            if (!Guid.TryParse(request.BatchId, out _) || request.IdempotencyKey.Length > 128 || request.Details == null || request.Details.Count == 0)
                return "InvalidRequest";
            return null;
        }

        private static string CreateRequestHash(BatchDepositV2Request request)
        {
            if (request == null) return string.Empty;
            var builder = new StringBuilder();
            builder.Append(request.ActorType).Append('|').Append(request.ActorId).Append('|').Append(request.PayerManagerId).Append('|')
                .Append(request.OperatorEntityId).Append('|').Append(request.EntityId).Append('|').Append(request.RequestIp);
            if (request.Details != null)
                foreach (var detail in request.Details.OrderBy(x => x.UserUID))
                    builder.Append('|').Append(detail.UserUID).Append(':').Append(Math.Round(detail.RequestAmount, Program.AccuracyDigitBal).ToString("F4", System.Globalization.CultureInfo.InvariantCulture));
            using (var sha256 = SHA256.Create())
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
