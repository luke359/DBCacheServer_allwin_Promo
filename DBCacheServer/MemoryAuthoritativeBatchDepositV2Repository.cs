using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Protocol;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DBCacheServer
{
    internal sealed class MemoryAuthoritativeBatchDepositV2Repository
    {
        private const int CommandTimeoutSeconds = 15;
        private readonly MysqlAcess mysql;

        internal MemoryAuthoritativeBatchDepositV2Repository(MysqlAcess mysql) { this.mysql = mysql; }

        internal BatchDepositV2Result ReadExisting(BatchDepositV2Request request, string requestHash)
        {
            return mysql.ExecuteInReadCommitted(connection =>
            {
                BatchDepositV2Result result;
                using (var command = Command(connection, null,
                    "SELECT RequestHash, ResultPayload, Status, FailureCode, BatchId FROM BatchDepositV2 " +
                    "WHERE ActorType = @actorType AND ActorId = @actorId AND IdempotencyKey = @idempotencyKey",
                    ("@actorType", request.ActorType.ToString()), ("@actorId", request.ActorId),
                    ("@idempotencyKey", request.IdempotencyKey)))
                using (var reader = command.ExecuteReader())
                    result = ReadExistingResult(reader, request, requestHash);
                return RefreshSuccessfulDetails(connection, null, result);
            });
        }

        internal BatchDepositV2Result Execute(BatchDepositV2Request request, string requestHash,
            IReadOnlyDictionary<int, BatchDepositV2PlayerSnapshot> players, double playerBalanceLimit,
            double depositUnit, CountrySettingData bonusSetting, out bool committed)
        {
            bool newSuccess = false;
            BatchDepositV2Result result;
            try
            {
                result = mysql.ExecuteInTransaction((connection, transaction) =>
                {
                    BatchDepositV2Result existing = ReadExistingForUpdate(connection, transaction, request, requestHash);
                    if (existing != null) return existing;

                    List<BatchDepositV2RequestDetail> details = request.Details.OrderBy(x => x.UserUID).ToList();
                    LockPlayerRows(connection, transaction, details.Select(x => x.UserUID));
                    if (request.ActorType == BatchDepositV2ActorType.ApiClient &&
                        string.IsNullOrWhiteSpace(request.PayerManagerId))
                    {
                        List<string> playerManagerIds = details
                            .Where(x => players.ContainsKey(x.UserUID))
                            .Select(x => players[x.UserUID].ManagerId)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Distinct(StringComparer.Ordinal)
                            .ToList();
                        if (playerManagerIds.Count == 1)
                            request.PayerManagerId = playerManagerIds[0];
                    }
                    ManagerRow manager = ReadManagerForUpdate(connection, transaction, request.PayerManagerId);
                    ActorRow actor = null;
                    if (request.ActorType == BatchDepositV2ActorType.WebUser)
                    {
                        actor = ReadActorForUpdate(connection, transaction, request.ActorId);
                    }
                    int operatorEntityId = request.ActorType == BatchDepositV2ActorType.ApiClient
                        ? ReadMerchantEntityId(connection, transaction, request.ActorId)
                        : actor?.EntityId ?? 0;

                    string failure = Validate(connection, transaction, request, details, players, manager, actor,
                        operatorEntityId, playerBalanceLimit, depositUnit, bonusSetting);
                    if (failure != null && !IsWebFinancialLimitFailure(request, failure))
                        return PersistFailure(connection, transaction, request, requestHash, failure, operatorEntityId);

                    decimal totalRequestAmount = details.Sum(x => x.RequestAmount);
                    decimal signedTotal = details.Sum(GetSignedAmount);
                    if (request.ActorType == BatchDepositV2ActorType.WebUser)
                    {
                        bool unlimited = ReadBalanceUnlimited(connection, transaction, request.PayerManagerId);
                        if (!unlimited && signedTotal > 0 && manager.Balance < signedTotal)
                            return PersistFailure(connection, transaction, request, requestHash,
                                "InsufficientManagerBalance", operatorEntityId);
                    }
                    if (failure != null)
                        return PersistFailure(connection, transaction, request, requestHash, failure, operatorEntityId);

                    var result = NewResult(request, requestHash, BatchDepositV2Status.Succeeded, null);
                    InsertBatch(connection, transaction, request, requestHash, "Processing", totalRequestAmount, 0m, null,
                        operatorEntityId);
                    decimal totalBonus = 0m;
                    int sequence = 1;
                    foreach (BatchDepositV2RequestDetail detail in details)
                    {
                        BatchDepositV2PlayerSnapshot player = players[detail.UserUID];
                        decimal signedAmount = GetSignedAmount(detail);
                        decimal extraBonus = request.ActorType == BatchDepositV2ActorType.WebUser &&
                            detail.OperationMode == BatchDepositV2OperationMode.Deposit
                            ? CalculateBonus(player, detail.RequestAmount, bonusSetting)
                            : 0m;
                        decimal afterBalance = player.Balance + signedAmount + extraBonus;
                        double sessionId = detail.OperationMode == BatchDepositV2OperationMode.Deposit
                            ? Message.GetSessionID() : 0;
                        int tradeRecordId = UpdatePlayerAndInsertTradeRecord(connection, transaction, request,
                            operatorEntityId, player, signedAmount, extraBonus, afterBalance, sessionId);
                        InsertDetail(connection, transaction, request.BatchId, sequence, player, detail.OperationMode,
                            detail.RequestAmount, extraBonus, afterBalance, sessionId, tradeRecordId);
                        InsertOutbox(connection, transaction, request.BatchId, sequence, player,
                            signedAmount + extraBonus, sessionId);
                        BatchDepositV2FaultInjection.ThrowIfEnabled("AfterOutboxInsert");
                        result.Details.Add(new BatchDepositV2ResultDetail
                        {
                            DetailSequence = sequence,
                            UserUID = player.UserUid,
                            RequestAmount = detail.RequestAmount,
                            OperationMode = detail.OperationMode,
                            ExtraBonus = extraBonus,
                            BeforeBalance = player.Balance,
                            AfterBalance = afterBalance,
                            SessionId = sessionId,
                            TradeRecordId = tradeRecordId,
                            Status = BatchDepositV2Status.Succeeded
                        });
                        totalBonus += extraBonus;
                        sequence++;
                    }

                    if (request.ActorType == BatchDepositV2ActorType.WebUser)
                        Execute(connection, transaction,
                            "UPDATE AspNetUsers SET Balance = Balance - @amount WHERE Id = @id",
                            ("@amount", signedTotal), ("@id", request.PayerManagerId));
                    UpdateBatchSuccess(connection, transaction, request.BatchId, totalBonus, result);
                    newSuccess = true;
                    return result;
                });
            }
            catch (MySqlException exception) when (exception.Number == 1062)
            {
                result = ReadExisting(request, requestHash);
                if (result == null) throw;
            }
            committed = newSuccess;
            return result;
        }

        internal BatchDepositV2QueryResult Query(BatchDepositV2QueryRequest request)
        {
            return mysql.ExecuteInReadCommitted(connection =>
            {
                BatchDepositV2Result result;
                bool needsDetails;
                using (var command = Command(connection, null,
                    "SELECT BatchId, IdempotencyKey, RequestHash, Status, FailureCode, ResultPayload " +
                    "FROM BatchDepositV2 WHERE ActorType = @actorType AND ActorId = @actorId " +
                    "AND IdempotencyKey = @idempotencyKey",
                    ("@actorType", request.ActorType.ToString()), ("@actorId", request.ActorId),
                    ("@idempotencyKey", request.IdempotencyKey)))
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return new BatchDepositV2QueryResult { Found = false };
                    string payload = reader.IsDBNull(5) ? null : reader.GetString(5);
                    needsDetails = string.IsNullOrWhiteSpace(payload);
                    result = needsDetails
                        ? new BatchDepositV2Result
                        {
                            BatchId = reader.GetString(0),
                            IdempotencyKey = reader.GetString(1),
                            RequestHash = reader.GetString(2),
                            Status = ParseStatus(reader.GetString(3)),
                            FailureCode = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Details = new List<BatchDepositV2ResultDetail>()
                        }
                        : JsonConvert.DeserializeObject<BatchDepositV2Result>(payload);
                }
                if (result.Status == BatchDepositV2Status.Succeeded)
                    result.Details = ReadResultDetails(connection, null, result.BatchId);
                return new BatchDepositV2QueryResult { Found = true, Result = result };
            });
        }

        private static string Validate(MySqlConnection connection, MySqlTransaction transaction,
            BatchDepositV2Request request, List<BatchDepositV2RequestDetail> details,
            IReadOnlyDictionary<int, BatchDepositV2PlayerSnapshot> players, ManagerRow manager, ActorRow actor,
            int operatorEntityId, double playerBalanceLimit, double depositUnit, CountrySettingData bonusSetting)
        {
            if (details.Count == 0 || details.Select(x => x.UserUID).Distinct().Count() != details.Count)
                return "InvalidDetails";
            if (request.ActorType == BatchDepositV2ActorType.ApiClient)
            {
                if (operatorEntityId <= 0) return "MerchantScopeMismatch";
                if (manager == null) return "PayerManagerNotFound";
                if (manager.EntityId != operatorEntityId) return "PayerOutOfScope";
            }
            else
            {
                if (actor == null) return "ActorNotFound";
                if (actor.IsFrozen) return "ActorFrozen";
                if (manager == null) return "PayerManagerNotFound";
                if (!CanManageEntity(connection, transaction, actor.EntityId, manager.EntityId))
                    return "PayerOutOfScope";
                if (details.Any(x => x.OperationMode == BatchDepositV2OperationMode.Deposit) &&
                    !actor.CanDeposit) return "NoDepositPermission";
                if (details.Any(x => x.OperationMode == BatchDepositV2OperationMode.Withdraw) &&
                    !actor.CanWithdraw) return "NoWithdrawalPermission";
            }

            decimal maxPlayerBalance = Convert.ToDecimal(playerBalanceLimit);
            decimal maxDepositUnit = Convert.ToDecimal(depositUnit);
            string financialFailure = null;
            foreach (BatchDepositV2RequestDetail detail in details)
            {
                if (!players.TryGetValue(detail.UserUID, out BatchDepositV2PlayerSnapshot player))
                    return "PlayerNotFound";
                if (player.IsBlocked) return "PlayerUnavailable";
                if (request.ActorType == BatchDepositV2ActorType.ApiClient)
                {
                    if (player.EntityId != operatorEntityId) return "PlayerOutOfScope";
                    if (detail.OperationMode == BatchDepositV2OperationMode.WithdrawAll)
                    {
                        if (player.UserSituation > 1) return "PlayerInGameWithdrawalNotAllowed";
                        if (player.Balance == 0) return "ZeroPlayerBalance";
                        if (detail.RequestAmount != player.Balance) return "InvalidAmount";
                    }
                    else if (detail.OperationMode != BatchDepositV2OperationMode.Deposit)
                    {
                        return "InvalidOperationMode";
                    }
                    else if (detail.RequestAmount <= 0 ||
                        decimal.Round(detail.RequestAmount, Program.AccuracyDigitBal) != detail.RequestAmount)
                        return "InvalidAmount";
                    continue;
                }

                if (!CanManageEntity(connection, transaction, actor.EntityId, player.EntityId))
                    return "PlayerOutOfScope";
                if ((detail.OperationMode != BatchDepositV2OperationMode.Deposit &&
                    detail.OperationMode != BatchDepositV2OperationMode.Withdraw))
                    return "InvalidOperationMode";
                if (detail.RequestAmount <= 0 ||
                    decimal.Round(detail.RequestAmount, Program.AccuracyDigitBal) != detail.RequestAmount)
                    return "InvalidAmount";
                if (detail.OperationMode == BatchDepositV2OperationMode.Withdraw)
                {
                    if (player.UserSituation > 1) return "PlayerInGameWithdrawalNotAllowed";
                    bool keyOutRestricted = player.KeyOutLimit ||
                        (player.CreditRebateFg && !player.CanRebateKeyOut);
                    bool insufficientPlayerBalance = player.Balance - detail.RequestAmount < 0;
                    if (insufficientPlayerBalance)
                        financialFailure = "InsufficientPlayerBalance";
                    else if (keyOutRestricted)
                        financialFailure = "KeyOutLimit";
                }
                else
                {
                    decimal bonus = CalculateBonus(player, detail.RequestAmount, bonusSetting);
                    decimal creditedAmount = detail.RequestAmount + bonus;
                    bool overDepositUnit = creditedAmount > maxDepositUnit;
                    bool overPlayerBalanceLimit = player.Balance + creditedAmount > maxPlayerBalance;
                    if (overDepositUnit)
                        financialFailure = "OverDepositUnit";
                    else if (overPlayerBalanceLimit)
                        financialFailure = "PlayerBalanceLimitExceeded";
                }
                if (financialFailure != null) break;
            }
            return financialFailure;
        }

        private static bool IsWebFinancialLimitFailure(BatchDepositV2Request request, string failure)
        {
            if (request.ActorType != BatchDepositV2ActorType.WebUser) return false;
            return failure == "InsufficientPlayerBalance" || failure == "OverDepositUnit" ||
                failure == "PlayerBalanceLimitExceeded" || failure == "KeyOutLimit";
        }

        private static int ReadMerchantEntityId(MySqlConnection connection, MySqlTransaction transaction,
            string actorId)
        {
            using (var command = Command(connection, transaction,
                "SELECT h.id FROM H5AgentTable h INNER JOIN Entity e ON e.id = h.id " +
                "WHERE h.AgentId = @agentId AND h.Enabled = 1 AND e.Name = h.AgentId LIMIT 1",
                ("@agentId", actorId)))
            {
                object entityId = command.ExecuteScalar();
                return entityId == null || entityId == DBNull.Value ? 0 : Convert.ToInt32(entityId);
            }
        }

        private static void LockPlayerRows(MySqlConnection connection, MySqlTransaction transaction,
            IEnumerable<int> userUids)
        {
            List<int> ids = userUids.OrderBy(x => x).ToList();
            List<string> parameters = ids.Select((_, index) => "@user" + index).ToList();
            using (var command = Command(connection, transaction,
                "SELECT UserUID FROM Usertable WHERE UserUID IN (" + string.Join(",", parameters) +
                ") ORDER BY UserUID FOR UPDATE"))
            {
                for (int i = 0; i < ids.Count; i++) command.Parameters.AddWithValue(parameters[i], ids[i]);
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) { }
            }
        }

        private static ManagerRow ReadManagerForUpdate(MySqlConnection c, MySqlTransaction t, string managerId)
        {
            using (var command = Command(c, t, "SELECT Balance, ParentID FROM AspNetUsers WHERE Id = @id FOR UPDATE",
                ("@id", managerId)))
            using (var reader = command.ExecuteReader())
                return reader.Read() ? new ManagerRow { Balance = reader.GetDecimal(0), EntityId = reader.GetInt32(1) } : null;
        }

        private static ActorRow ReadActorForUpdate(MySqlConnection c, MySqlTransaction t, string actorId)
        {
            using (var command = Command(c, t,
                "SELECT actor.freeze, actor.ParentID, permission.Player_Deposit, permission.Player_Withdrawal " +
                "FROM AspNetUsers actor LEFT JOIN permissiontable permission ON permission.PermissionUID = actor.Id " +
                "WHERE actor.Id = @id FOR UPDATE", ("@id", actorId)))
            using (var reader = command.ExecuteReader())
                return reader.Read() ? new ActorRow
                {
                    IsFrozen = reader.GetInt32(0) == 1,
                    EntityId = reader.GetInt32(1),
                    CanDeposit = !reader.IsDBNull(2) && reader.GetInt32(2) == 1,
                    CanWithdraw = !reader.IsDBNull(3) && reader.GetInt32(3) == 1
                } : null;
        }

        private static bool ReadBalanceUnlimited(MySqlConnection c, MySqlTransaction t, string managerId)
        {
            using (var command = Command(c, t,
                "SELECT BalanceUnlimited FROM permissiontable WHERE PermissionUID = @id FOR UPDATE",
                ("@id", managerId)))
            using (var reader = command.ExecuteReader())
                return reader.Read() && reader.GetBoolean(0);
        }

        private static bool CanManageEntity(MySqlConnection c, MySqlTransaction t, int actorEntityId, int targetEntityId)
        {
            var visited = new HashSet<int>();
            while (targetEntityId > 1 && visited.Add(targetEntityId))
            {
                if (targetEntityId == actorEntityId) return true;
                using (var command = Command(c, t, "SELECT parentID FROM Entity WHERE id = @id",
                    ("@id", targetEntityId)))
                {
                    object parentId = command.ExecuteScalar();
                    if (parentId == null || parentId == DBNull.Value) return false;
                    targetEntityId = Convert.ToInt32(parentId);
                }
            }
            return targetEntityId == actorEntityId;
        }

        private static decimal CalculateBonus(BatchDepositV2PlayerSnapshot player, decimal amount,
            CountrySettingData bonusSetting)
        {
            if (bonusSetting == null || !bonusSetting.ExtraBonusFg || player.KeyInAward ||
                amount < Convert.ToDecimal(bonusSetting.ExtraBonusUnitKeyIn) ||
                player.Balance > Convert.ToDecimal(bonusSetting.ExtraBonusThreshold))
                return 0m;
            return Math.Round(Convert.ToDecimal(bonusSetting.ExtraBonusValue), Program.AccuracyDigit);
        }

        private static decimal GetSignedAmount(BatchDepositV2RequestDetail detail)
        {
            return detail.OperationMode == BatchDepositV2OperationMode.Deposit
                ? detail.RequestAmount
                : -detail.RequestAmount;
        }

        private static BatchDepositV2Result ReadExistingForUpdate(MySqlConnection c, MySqlTransaction t,
            BatchDepositV2Request request, string requestHash)
        {
            BatchDepositV2Result result;
            using (var command = Command(c, t,
                "SELECT RequestHash, ResultPayload, Status, FailureCode, BatchId FROM BatchDepositV2 " +
                "WHERE ActorType = @actorType AND ActorId = @actorId AND IdempotencyKey = @idempotencyKey FOR UPDATE",
                ("@actorType", request.ActorType.ToString()), ("@actorId", request.ActorId),
                ("@idempotencyKey", request.IdempotencyKey)))
            using (var reader = command.ExecuteReader())
                result = ReadExistingResult(reader, request, requestHash);
            return RefreshSuccessfulDetails(c, t, result);
        }

        private static BatchDepositV2Result RefreshSuccessfulDetails(MySqlConnection connection,
            MySqlTransaction transaction, BatchDepositV2Result result)
        {
            if (result == null || result.Status != BatchDepositV2Status.Succeeded) return result;
            List<BatchDepositV2ResultDetail> details = ReadResultDetails(connection, transaction, result.BatchId);
            if (details.Count > 0) result.Details = details;
            return result;
        }

        private static BatchDepositV2Result ReadExistingResult(MySqlDataReader reader,
            BatchDepositV2Request request, string requestHash)
        {
            if (!reader.Read()) return null;
            if (!string.Equals(reader.GetString(0), requestHash, StringComparison.Ordinal))
                return NewResult(request, requestHash, BatchDepositV2Status.Failed,
                    "IdempotencyKeyPayloadMismatch");
            if (!reader.IsDBNull(1))
                return JsonConvert.DeserializeObject<BatchDepositV2Result>(reader.GetString(1));
            return NewResult(request, requestHash, ParseStatus(reader.GetString(2)),
                reader.IsDBNull(3) ? null : reader.GetString(3));
        }

        private static BatchDepositV2Result PersistFailure(MySqlConnection c, MySqlTransaction t,
            BatchDepositV2Request request, string hash, string failureCode, int operatorEntityId)
        {
            BatchDepositV2Result result = NewResult(request, hash, BatchDepositV2Status.Failed, failureCode);
            InsertBatch(c, t, request, hash, "Failed", 0m, 0m, result, operatorEntityId);
            return result;
        }

        private static BatchDepositV2Result NewResult(BatchDepositV2Request request, string hash,
            BatchDepositV2Status status, string failureCode)
        {
            return new BatchDepositV2Result
            {
                BatchId = request.BatchId,
                IdempotencyKey = request.IdempotencyKey,
                RequestHash = hash,
                Status = status,
                FailureCode = failureCode,
                Details = new List<BatchDepositV2ResultDetail>()
            };
        }

        private static void InsertBatch(MySqlConnection c, MySqlTransaction t, BatchDepositV2Request request,
            string hash, string status, decimal total, decimal totalBonus, BatchDepositV2Result result,
            int operatorEntityId)
        {
            Execute(c, t, "INSERT INTO BatchDepositV2 (BatchId, ActorType, ActorId, IdempotencyKey, RequestHash, PayerManagerId, OperatorEntityId, EntityId, Status, FailureCode, DetailCount, TotalRequestAmount, TotalExtraBonus, RequestIp, RequestPayload, ResultPayload, CreatedAtUtc, CompletedAtUtc) " +
                "VALUES (@batchId, @actorType, @actorId, @idempotencyKey, @hash, @payerManagerId, @operatorEntityId, @entityId, @status, @failureCode, @detailCount, @total, @totalBonus, @requestIp, @requestPayload, @resultPayload, UTC_TIMESTAMP(6), IF(@status = 'Failed', UTC_TIMESTAMP(6), NULL))",
                ("@batchId", request.BatchId), ("@actorType", request.ActorType.ToString()),
                ("@actorId", request.ActorId), ("@idempotencyKey", request.IdempotencyKey), ("@hash", hash),
                ("@payerManagerId", request.PayerManagerId), ("@operatorEntityId", operatorEntityId),
                ("@entityId", operatorEntityId), ("@status", status),
                ("@failureCode", result == null ? null : result.FailureCode),
                ("@detailCount", request.Details.Count), ("@total", total), ("@totalBonus", totalBonus),
                ("@requestIp", request.RequestIp), ("@requestPayload", JsonConvert.SerializeObject(request)),
                ("@resultPayload", result == null ? null : JsonConvert.SerializeObject(result)));
        }

        private static int UpdatePlayerAndInsertTradeRecord(MySqlConnection c, MySqlTransaction t,
            BatchDepositV2Request request, int operatorEntityId, BatchDepositV2PlayerSnapshot player, decimal amount,
            decimal extraBonus, decimal afterBalance, double sessionId)
        {
            bool isDeposit = amount > 0;
            bool api = request.ActorType == BatchDepositV2ActorType.ApiClient;
            Execute(c, t,
                "UPDATE Usertable SET UserBalance = @balance, SessionID = IF(@isDeposit = 1, @sessionId, SessionID), " +
                "KeyInAward = IF(@applyBonus = 1, 1, KeyInAward), KeyOutLimit = IF(@applyBonus = 1, 1, KeyOutLimit) " +
                "WHERE UserUID = @userUid",
                ("@balance", afterBalance), ("@sessionId", sessionId), ("@isDeposit", isDeposit),
                ("@applyBonus", extraBonus > 0), ("@userUid", player.UserUid));
            Execute(c, t,
                "INSERT INTO MangerToUserTradeRecord (TradeType, OperationType, TransactionType, Status, OperatorId, APIManagerId, UserUID, UserID, OperatorEntityId, EntityID, BeforeBalance, Amount, ExtraBonus, IP, StatusValue, SearchIndex, TimeStamp) " +
                "VALUES ('PlayerBalanceDeal', @operationType, @transactionType, @status, @operatorId, @apiManagerId, @userUid, @userId, @operatorEntityId, @entityId, @beforeBalance, @amount, @extraBonus, @ip, '2', @searchIndex, @timeStamp)",
                ("@operatorId", request.PayerManagerId), ("@apiManagerId", api ? request.PayerManagerId : null),
                ("@userUid", player.UserUid), ("@userId", player.UserId),
                ("@operatorEntityId", operatorEntityId), ("@entityId", player.EntityId),
                ("@beforeBalance", player.Balance), ("@amount", amount), ("@extraBonus", extraBonus),
                ("@ip", request.RequestIp), ("@operationType", isDeposit ? "Deposit" : "Withdrawal"),
                ("@transactionType", isDeposit ? "1" : "2"),
                ("@status", isDeposit ? "Approved" : "Completed"),
                ("@searchIndex", Program.GetLogSearchIndex(DateTime.Now, player.UserUid)),
                ("@timeStamp", Program.GetLogTimeStamp(DateTime.Now)));
            using (var command = Command(c, t, "SELECT LAST_INSERT_ID()"))
                return Convert.ToInt32(command.ExecuteScalar());
        }

        private static void InsertDetail(MySqlConnection c, MySqlTransaction t, string batchId, int sequence,
            BatchDepositV2PlayerSnapshot player, BatchDepositV2OperationMode operationMode,
            decimal requestAmount, decimal extraBonus, decimal afterBalance, double sessionId, int tradeRecordId)
        {
            Execute(c, t, "INSERT INTO BatchDepositDetailV2 (BatchId, DetailSequence, UserUID, UserId, EntityId, OperationMode, RequestAmount, ExtraBonus, BeforeBalance, AfterBalance, SessionId, TradeRecordId, Status, CreatedAtUtc) VALUES (@batchId, @sequence, @userUid, @userId, @entityId, @operationMode, @amount, @extraBonus, @beforeBalance, @afterBalance, @sessionId, @tradeRecordId, 'Succeeded', UTC_TIMESTAMP(6))",
                ("@batchId", batchId), ("@sequence", sequence), ("@userUid", player.UserUid),
                ("@userId", player.UserId), ("@entityId", player.EntityId),
                ("@operationMode", operationMode.ToString()), ("@amount", requestAmount), ("@extraBonus", extraBonus),
                ("@beforeBalance", player.Balance), ("@afterBalance", afterBalance),
                ("@sessionId", sessionId), ("@tradeRecordId", tradeRecordId));
        }

        private static void InsertOutbox(MySqlConnection c, MySqlTransaction t, string batchId, int sequence,
            BatchDepositV2PlayerSnapshot player, decimal amount, double sessionId)
        {
            string eventId = Guid.NewGuid().ToString();
            List<string> routes = ResolveTargetRoutes(player.UserSituation).ToList();
            string status = routes.Count == 0 ? "Published" : "Pending";
            Execute(c, t, "INSERT INTO BatchDepositOutboxV2 (EventId, BatchId, DetailSequence, UserUID, MessageType, Payload, Status, AttemptCount, NextAttemptAtUtc, DeliveredAtUtc, CreatedAtUtc) VALUES (@eventId, @batchId, @sequence, @userUid, 'UserBalanceDelta', @payload, @status, 0, UTC_TIMESTAMP(6), IF(@status = 'Published', UTC_TIMESTAMP(6), NULL), UTC_TIMESTAMP(6))",
                ("@eventId", eventId), ("@batchId", batchId), ("@sequence", sequence), ("@userUid", player.UserUid), ("@status", status),
                ("@payload", JsonConvert.SerializeObject(new
                {
                    EventId = eventId,
                    EventType = "user.balance.delta.v1",
                    UserUID = player.UserUid,
                    Delta = amount,
                    SessionId = sessionId,
                    BatchId = batchId,
                    DetailSequence = sequence,
                    Operation = amount > 0 ? "Deposit" : "Withdrawal"
                })));
            if (routes.Count > 0)
            {
                long outboxId;
                using (var command = Command(c, t, "SELECT LAST_INSERT_ID()"))
                    outboxId = Convert.ToInt64(command.ExecuteScalar());
                foreach (string route in routes)
                    Execute(c, t, "INSERT INTO BatchDepositOutboxDeliveryV2 (OutboxId, TargetRoute, Status, AttemptCount, NextAttemptAtUtc, CreatedAtUtc) VALUES (@outboxId, @targetRoute, 'Pending', 0, UTC_TIMESTAMP(6), UTC_TIMESTAMP(6))",
                        ("@outboxId", outboxId), ("@targetRoute", route));
            }
        }

        private static IEnumerable<string> ResolveTargetRoutes(int gameServerCode)
        {
            if (!Enum.IsDefined(typeof(GameServerCode), gameServerCode) ||
                (GameServerCode)gameServerCode == GameServerCode.None)
                yield break;

            yield return "user-info.login";
            yield return "user-info.game." + ((GameServerCode)gameServerCode).ToString().ToLowerInvariant();
        }

        private static void UpdateBatchSuccess(MySqlConnection c, MySqlTransaction t, string batchId,
            decimal totalBonus, BatchDepositV2Result result)
        {
            Execute(c, t,
                "UPDATE BatchDepositV2 SET Status = 'Succeeded', TotalExtraBonus = @totalBonus, ResultPayload = @resultPayload, CompletedAtUtc = UTC_TIMESTAMP(6) WHERE BatchId = @batchId",
                ("@totalBonus", totalBonus), ("@resultPayload", JsonConvert.SerializeObject(result)),
                ("@batchId", batchId));
        }

        private static List<BatchDepositV2ResultDetail> ReadResultDetails(MySqlConnection connection,
            MySqlTransaction transaction, string batchId)
        {
            var details = new List<BatchDepositV2ResultDetail>();
            using (var command = Command(connection, transaction,
                "SELECT DetailSequence, UserUID, RequestAmount, OperationMode, ExtraBonus, BeforeBalance, AfterBalance, SessionId, TradeRecordId, Status, FailureCode FROM BatchDepositDetailV2 WHERE BatchId = @batchId ORDER BY DetailSequence",
                ("@batchId", batchId)))
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    details.Add(new BatchDepositV2ResultDetail
                    {
                        DetailSequence = reader.GetInt32(0),
                        UserUID = reader.GetInt32(1),
                        RequestAmount = reader.GetDecimal(2),
                        OperationMode = ParseOperationMode(reader.GetString(3)),
                        ExtraBonus = reader.GetDecimal(4),
                        BeforeBalance = reader.GetDecimal(5),
                        AfterBalance = reader.GetDecimal(6),
                        SessionId = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                        TradeRecordId = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        Status = ParseStatus(reader.GetString(9)),
                        FailureCode = reader.IsDBNull(10) ? null : reader.GetString(10)
                    });
            return details;
        }

        private static BatchDepositV2Status ParseStatus(string status)
        {
            return Enum.TryParse(status, true, out BatchDepositV2Status parsed)
                ? parsed : BatchDepositV2Status.Failed;
        }

        private static BatchDepositV2OperationMode ParseOperationMode(string operationMode)
        {
            return Enum.TryParse(operationMode, true, out BatchDepositV2OperationMode parsed)
                ? parsed : BatchDepositV2OperationMode.Deposit;
        }

        private static MySqlCommand Command(MySqlConnection c, MySqlTransaction t, string sql,
            params (string, object)[] values)
        {
            var command = new MySqlCommand(sql, c, t) { CommandTimeout = CommandTimeoutSeconds };
            foreach (var value in values)
                command.Parameters.AddWithValue(value.Item1, value.Item2 ?? DBNull.Value);
            return command;
        }

        private static void Execute(MySqlConnection c, MySqlTransaction t, string sql,
            params (string, object)[] values)
        {
            using (var command = Command(c, t, sql, values)) command.ExecuteNonQuery();
        }

        private sealed class ManagerRow { internal decimal Balance; internal int EntityId; }
        private sealed class ActorRow
        {
            internal bool IsFrozen;
            internal int EntityId;
            internal bool CanDeposit;
            internal bool CanWithdraw;
        }
    }
}
