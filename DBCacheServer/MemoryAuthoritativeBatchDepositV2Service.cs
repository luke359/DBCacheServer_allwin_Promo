using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DBCacheServer
{
    internal sealed class BatchDepositV2PlayerSnapshot
    {
        internal int UserUid;
        internal string UserId;
        internal decimal Balance;
        internal string ManagerId;
        internal int EntityId;
        internal bool IsBlocked;
        internal int UserSituation;
        internal bool KeyInAward;
        internal bool KeyOutLimit;
        internal bool CreditRebateFg;
        internal bool CanRebateKeyOut;
    }

    internal sealed class MemoryAuthoritativeBatchDepositV2Service
    {
        private const int MaxBatchSize = 100;
        private static readonly TimeSpan PlayerLockTimeout = TimeSpan.FromSeconds(5);
        private readonly CacheManeger cache;
        private readonly MemoryAuthoritativeBatchDepositV2Repository repository;
        private readonly Func<int, bool> recallExternalWallet;

        internal MemoryAuthoritativeBatchDepositV2Service(CacheManeger cache, MysqlAcess mysql,
            Func<int, bool> recallExternalWallet = null)
        {
            this.cache = cache;
            repository = new MemoryAuthoritativeBatchDepositV2Repository(mysql);
            this.recallExternalWallet = recallExternalWallet;
        }

        internal BatchDepositV2Result Handle(BatchDepositV2Request request)
        {
            var elapsed = Stopwatch.StartNew();
            string validationError = ValidateRequest(request);
            string requestHash = CreateRequestHash(request);
            if (validationError != null) return LogValidationFailure(request, requestHash, validationError, elapsed.Elapsed);

            BatchDepositV2Result existing = repository.ReadExisting(request, requestHash);
            if (existing != null)
            {
                LogReplay(request, existing, elapsed.Elapsed);
                return existing;
            }

            string stage = "acquire_player_locks";
            try
            {
                BatchDepositV2OperationalLog.Write("info", "service", "processing_started", new
                {
                    request.BatchId,
                    request.ActorType,
                    request.ActorId,
                    request.EntityId,
                    request.IdempotencyKey,
                    PlayerUids = request.Details.Select(x => x.UserUID).ToArray(),
                    Stage = stage,
                    ElapsedMilliseconds = elapsed.ElapsedMilliseconds
                });

                using (cache.AcquireBatchDepositV2PlayerLocks(request.Details.Select(x => x.UserUID), PlayerLockTimeout))
                {
                    stage = "snapshot_players_before_recall";
                    Dictionary<int, BatchDepositV2PlayerSnapshot> players =
                        cache.SnapshotBatchDepositV2Players(request.Details.Select(x => x.UserUID));
                    BatchDepositV2OperationalLog.Write("info", "service", "player_locks_acquired", new
                    {
                        request.BatchId,
                        request.IdempotencyKey,
                        Stage = stage,
                        RequestedPlayerUids = request.Details.Select(x => x.UserUID).ToArray(),
                        Snapshots = players.Values.Select(x => new
                        {
                            x.UserUid,
                            x.EntityId,
                            x.Balance,
                            x.IsBlocked,
                            x.UserSituation
                        }).ToArray(),
                        ElapsedMilliseconds = elapsed.ElapsedMilliseconds
                    });

                    stage = "external_wallet_recall";
                    string transientFailure = PrepareWithdrawAll(request, players);
                    if (transientFailure != null)
                        return LogTransientFailure(request, requestHash, transientFailure, elapsed.Elapsed, stage);

                    stage = "acquire_persistence_gate";
                    if (!System.Threading.Monitor.TryEnter(BatchDepositV2PersistenceGate.SyncRoot, PlayerLockTimeout))
                        throw new TimeoutException("BatchDepositV2 persistence gate timeout.");
                    try
                    {
                        stage = "snapshot_players_before_persist";
                        players = cache.SnapshotBatchDepositV2Players(request.Details.Select(x => x.UserUID));
                        ApplyWithdrawAllAmounts(request, players);

                        stage = "load_balance_settings";
                        double playerBalanceLimit = 0;
                        double depositUnit = 0;
                        cache.GetPlayerBalanceLimit(ref playerBalanceLimit, ref depositUnit);
                        BatchDepositV2OperationalLog.Write("info", "service", "persistence_gate_acquired", new
                        {
                            request.BatchId,
                            request.IdempotencyKey,
                            Stage = stage,
                            PlayerBalanceLimit = playerBalanceLimit,
                            DepositUnit = depositUnit,
                            Details = request.Details.Select(x => new
                            {
                                x.UserUID,
                                x.RequestAmount,
                                OperationMode = x.OperationMode.ToString()
                            }).ToArray(),
                            ElapsedMilliseconds = elapsed.ElapsedMilliseconds
                        });

                        stage = "repository_execute";
                        bool committed;
                        BatchDepositV2Result result = repository.Execute(request, requestHash, players,
                            playerBalanceLimit, depositUnit, cache.GetCountryExtraBonusString(), out committed);
                        stage = "synchronize_memory";
                        if (committed)
                            cache.SynchronizeCommittedBatchDepositV2(result);
                        stage = "record_metrics";
                        BatchDepositV2Metrics.Record(result, committed);
                        LogCompletion(request, result, elapsed);
                        return result;
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(BatchDepositV2PersistenceGate.SyncRoot);
                    }
                }
            }
            catch (TimeoutException exception)
            {
                return LogTransientFailure(request, requestHash, "SystemBusy", elapsed.Elapsed, stage, exception);
            }
            catch (Exception exception)
            {
                return LogTransientFailure(request, requestHash, "SystemBusy", elapsed.Elapsed, stage, exception);
            }
        }

        internal BatchDepositV2QueryResult Query(BatchDepositV2QueryRequest request)
        {
            if (request == null || request.ActorType == BatchDepositV2ActorType.None ||
                string.IsNullOrWhiteSpace(request.ActorId) || string.IsNullOrWhiteSpace(request.IdempotencyKey) ||
                request.IdempotencyKey.Length > 128)
                return new BatchDepositV2QueryResult { Found = false, FailureCode = "InvalidRequest" };
            return repository.Query(request);
        }

        private string PrepareWithdrawAll(BatchDepositV2Request request,
            IReadOnlyDictionary<int, BatchDepositV2PlayerSnapshot> players)
        {
            if (request.ActorType != BatchDepositV2ActorType.ApiClient) return null;
            foreach (BatchDepositV2RequestDetail detail in request.Details)
            {
                if (detail.OperationMode != BatchDepositV2OperationMode.WithdrawAll) continue;
                if (!players.TryGetValue(detail.UserUID, out BatchDepositV2PlayerSnapshot player)) continue;
                if (player.UserSituation > 1 || player.IsBlocked) continue;
                if (recallExternalWallet == null || !recallExternalWallet(detail.UserUID))
                    return "ExternalWalletRecallFailed";
            }
            return null;
        }

        private static void ApplyWithdrawAllAmounts(BatchDepositV2Request request,
            IReadOnlyDictionary<int, BatchDepositV2PlayerSnapshot> players)
        {
            if (request.ActorType != BatchDepositV2ActorType.ApiClient) return;
            foreach (BatchDepositV2RequestDetail detail in request.Details)
                if (detail.OperationMode == BatchDepositV2OperationMode.WithdrawAll &&
                    players.TryGetValue(detail.UserUID, out BatchDepositV2PlayerSnapshot player))
                    detail.RequestAmount = player.Balance;
        }

        private static string ValidateRequest(BatchDepositV2Request request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.BatchId) ||
                string.IsNullOrWhiteSpace(request.IdempotencyKey) || string.IsNullOrWhiteSpace(request.ActorId) ||
                request.ActorType == BatchDepositV2ActorType.None)
                return "InvalidRequest";
            // ApiClient legacy packets did not populate PayerManagerId. The repository
            // resolves that value from the authoritative player snapshot during rollout.
            if (request.ActorType == BatchDepositV2ActorType.WebUser &&
                string.IsNullOrWhiteSpace(request.PayerManagerId))
                return "InvalidRequest";
            if (!Guid.TryParse(request.BatchId, out _) || request.IdempotencyKey.Length > 128 ||
                request.Details == null || request.Details.Count == 0)
                return "InvalidRequest";
            if (request.Details.Count > MaxBatchSize) return "BatchSizeLimitExceeded";
            if (request.ActorType == BatchDepositV2ActorType.WebUser)
            {
                if (request.Details.Any(x => x.OperationMode != BatchDepositV2OperationMode.Deposit &&
                    x.OperationMode != BatchDepositV2OperationMode.Withdraw))
                    return "InvalidOperationMode";
                if (request.Details.Any(x => x.RequestAmount <= 0)) return "InvalidAmount";
            }
            else if (request.ActorType == BatchDepositV2ActorType.ApiClient)
            {
                if (request.Details.Count != 1 || request.Details.Any(x =>
                    x.OperationMode != BatchDepositV2OperationMode.Deposit &&
                    x.OperationMode != BatchDepositV2OperationMode.WithdrawAll))
                    return "InvalidOperationMode";
                BatchDepositV2RequestDetail detail = request.Details[0];
                if ((detail.OperationMode == BatchDepositV2OperationMode.Deposit && detail.RequestAmount <= 0) ||
                    (detail.OperationMode == BatchDepositV2OperationMode.WithdrawAll && detail.RequestAmount != 0))
                    return "InvalidAmount";
            }
            return null;
        }

        private static string CreateRequestHash(BatchDepositV2Request request)
        {
            if (request == null) return string.Empty;
            var builder = new StringBuilder();
            builder.Append(request.ActorType).Append('|').Append(request.ActorId).Append('|')
                // API Payer is resolved from the authoritative player relationship and was
                // empty in legacy packets, so it is not part of the API idempotency payload.
                .Append(request.ActorType == BatchDepositV2ActorType.ApiClient
                    ? string.Empty
                    : request.PayerManagerId)
                .Append('|').Append(request.OperatorEntityId).Append('|')
                .Append(request.EntityId);
            if (request.Details != null)
                foreach (BatchDepositV2RequestDetail detail in request.Details.OrderBy(x => x.UserUID))
                {
                    builder.Append('|').Append(detail.UserUID).Append(':');
                    // Preserve the original logical hash representation so existing
                    // Deposit and WEB withdrawal idempotency keys still replay after rollout.
                    if (detail.OperationMode == BatchDepositV2OperationMode.WithdrawAll)
                    {
                        builder.Append("WithdrawAll");
                    }
                    else
                    {
                        decimal legacySignedAmount = detail.OperationMode == BatchDepositV2OperationMode.Withdraw
                            ? -detail.RequestAmount
                            : detail.RequestAmount;
                        builder.Append("Amount").Append(':').Append(legacySignedAmount.ToString("F4",
                            System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
            using (var sha256 = SHA256.Create())
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString())))
                    .Replace("-", string.Empty).ToLowerInvariant();
        }

        private static BatchDepositV2Result Failed(BatchDepositV2Request request, string hash, string code)
        {
            return new BatchDepositV2Result
            {
                BatchId = request?.BatchId,
                IdempotencyKey = request?.IdempotencyKey,
                RequestHash = hash,
                Status = BatchDepositV2Status.Failed,
                FailureCode = code,
                Details = new List<BatchDepositV2ResultDetail>()
            };
        }

        private static BatchDepositV2Result LogValidationFailure(BatchDepositV2Request request, string hash,
            string code, TimeSpan elapsed)
        {
            BatchDepositV2OperationalLog.Write("warning", "service", "validation_failed", new
            {
                BatchId = request?.BatchId,
                ActorType = request?.ActorType.ToString(),
                ActorId = request?.ActorId,
                EntityId = request?.EntityId,
                IdempotencyKey = request?.IdempotencyKey,
                Details = request?.Details?.Select(x => new
                {
                    x.UserUID,
                    x.RequestAmount,
                    OperationMode = x.OperationMode.ToString()
                }).ToArray(),
                ElapsedMilliseconds = (long)elapsed.TotalMilliseconds,
                Result = code
            });
            return Failed(request, hash, code);
        }

        private static void LogReplay(BatchDepositV2Request request, BatchDepositV2Result existing, TimeSpan elapsed)
        {
            BatchDepositV2OperationalLog.Write("info", "service", "idempotency_replay", new
            {
                request.BatchId,
                request.ActorType,
                request.ActorId,
                request.EntityId,
                request.IdempotencyKey,
                PlayerUids = request.Details.Select(x => x.UserUID).ToArray(),
                ElapsedMilliseconds = (long)elapsed.TotalMilliseconds,
                Result = existing.FailureCode ?? existing.Status.ToString()
            });
        }

        private static BatchDepositV2Result LogTransientFailure(BatchDepositV2Request request, string hash,
            string code, TimeSpan elapsed, string stage, Exception exception = null)
        {
            BatchDepositV2OperationalLog.Write("error", "service", "transient_failure", new
            {
                BatchId = request?.BatchId,
                ActorType = request?.ActorType.ToString(),
                ActorId = request?.ActorId,
                EntityId = request?.EntityId,
                IdempotencyKey = request?.IdempotencyKey,
                PlayerUids = request?.Details?.Select(x => x.UserUID).ToArray(),
                Stage = stage,
                ElapsedMilliseconds = (long)elapsed.TotalMilliseconds,
                ExceptionType = exception?.GetType().FullName,
                Error = exception?.Message,
                InnerError = exception?.InnerException?.Message,
                StackTrace = exception?.StackTrace,
                Result = code
            });
            return Failed(request, hash, code);
        }

        private static void LogCompletion(BatchDepositV2Request request, BatchDepositV2Result result, Stopwatch elapsed)
        {
            BatchDepositV2OperationalLog.Write("info", "service", "completed", new
            {
                request.BatchId,
                request.ActorType,
                request.ActorId,
                request.EntityId,
                request.IdempotencyKey,
                PlayerUids = request.Details.Select(x => x.UserUID).ToArray(),
                Stage = "database_and_memory_apply",
                ElapsedMilliseconds = elapsed.ElapsedMilliseconds,
                Result = result.FailureCode ?? result.Status.ToString()
            });
        }
    }
}
