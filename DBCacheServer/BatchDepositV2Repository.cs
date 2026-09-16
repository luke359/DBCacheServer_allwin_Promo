using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Protocol;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DBCacheServer
{
    [Obsolete("Database-authoritative implementation retained only for legacy test compatibility.")]
    internal sealed class BatchDepositV2Repository
    {
        private readonly MysqlAcess mysql;

        internal BatchDepositV2Repository(MysqlAcess mysql)
        {
            this.mysql = mysql;
        }

        internal BatchDepositV2Result Execute(BatchDepositV2Request request, string requestHash,
            double playerBalanceLimit, double depositUnit, CountrySettingData bonusSetting, out bool committed)
        {
            bool newSuccess = false;
            BatchDepositV2Result result;
            try
            {
                result = mysql.ExecuteInTransaction((connection, transaction) =>
                {
                    BatchDepositV2Result existing = ReadExisting(connection, transaction, request, requestHash);
                    if (existing != null)
                        return existing;

                    var details = request.Details.OrderBy(x => x.UserUID).ToList();
                    var players = ReadPlayersForUpdate(connection, transaction, details.Select(x => x.UserUID));
                    var manager = ReadManagerForUpdate(connection, transaction, request.PayerManagerId);
                    var actor = ReadActorForUpdate(connection, transaction, request.ActorId);
                    int operatorEntityId = actor?.EntityId ?? 0;
                    var validationFailure = Validate(connection, transaction, request, details, players, manager, actor,
                        playerBalanceLimit, depositUnit, bonusSetting);
                    if (validationFailure != null && !IsFinancialLimitFailure(validationFailure))
                        return PersistFailure(connection, transaction, request, requestHash, validationFailure,
                            operatorEntityId);

                    decimal totalRequestAmount = details.Sum(x => x.RequestAmount);
                    decimal signedTotal = details.Sum(GetSignedAmount);
                    bool unlimited = ReadBalanceUnlimited(connection, transaction, request.PayerManagerId);
                    if (!unlimited && signedTotal > 0 && manager.Balance < signedTotal)
                        return PersistFailure(connection, transaction, request, requestHash,
                            "InsufficientManagerBalance", operatorEntityId);
                    if (validationFailure != null)
                        return PersistFailure(connection, transaction, request, requestHash, validationFailure,
                            operatorEntityId);

                    var result = NewResult(request, requestHash, BatchDepositV2Status.Succeeded, null);
                    InsertBatch(connection, transaction, request, requestHash, "Processing", totalRequestAmount, 0m, null,
                        operatorEntityId);
                    decimal totalBonus = 0m;
                    int sequence = 1;
                    foreach (var detail in details)
                    {
                        PlayerRow player = players[detail.UserUID];
                        decimal signedAmount = GetSignedAmount(detail);
                        decimal extraBonus = detail.OperationMode == BatchDepositV2OperationMode.Deposit
                            ? CalculateBonus(player, detail.RequestAmount, bonusSetting) : 0m;
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

                    Execute(connection, transaction, "UPDATE AspNetUsers SET Balance = Balance - @amount WHERE Id = @id",
                        ("@amount", signedTotal), ("@id", request.PayerManagerId));
                    UpdateBatchSuccess(connection, transaction, request.BatchId, totalBonus, result);
                    newSuccess = true;
                    return result;
                });
            }
            catch (MySqlException exception) when (exception.Number == 1062)
            {
                result = mysql.ExecuteInTransaction((connection, transaction) =>
                {
                    BatchDepositV2Result existing = ReadExisting(connection, transaction, request, requestHash);
                    if (existing == null) throw exception;
                    return existing;
                });
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
                using (var command = new MySqlCommand(
                    "SELECT BatchId, IdempotencyKey, RequestHash, Status, FailureCode, ResultPayload " +
                    "FROM BatchDepositV2 WHERE ActorType = @actorType AND ActorId = @actorId " +
                    "AND IdempotencyKey = @idempotencyKey", connection))
                {
                    command.Parameters.AddWithValue("@actorType", request.ActorType.ToString());
                    command.Parameters.AddWithValue("@actorId", request.ActorId);
                    command.Parameters.AddWithValue("@idempotencyKey", request.IdempotencyKey);
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
                }
                if (result.Status == BatchDepositV2Status.Succeeded)
                    result.Details = ReadResultDetails(connection, result.BatchId);
                return new BatchDepositV2QueryResult { Found = true, Result = result };
            });
        }

        private static List<BatchDepositV2ResultDetail> ReadResultDetails(MySqlConnection connection, string batchId)
        {
            return ReadResultDetails(connection, null, batchId);
        }

        private static List<BatchDepositV2ResultDetail> ReadResultDetails(MySqlConnection connection,
            MySqlTransaction transaction, string batchId)
        {
            var details = new List<BatchDepositV2ResultDetail>();
            using (var command = new MySqlCommand(
                "SELECT DetailSequence, UserUID, RequestAmount, OperationMode, ExtraBonus, BeforeBalance, AfterBalance, " +
                "SessionId, TradeRecordId, Status, FailureCode FROM BatchDepositDetailV2 " +
                "WHERE BatchId = @batchId ORDER BY DetailSequence", connection, transaction))
            {
                command.Parameters.AddWithValue("@batchId", batchId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
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
                    }
                }
            }
            return details;
        }

        private static BatchDepositV2Result ReadExisting(MySqlConnection connection, MySqlTransaction transaction,
            BatchDepositV2Request request, string requestHash)
        {
            BatchDepositV2Result result;
            using (var command = Command(connection, transaction,
                "SELECT RequestHash, ResultPayload, Status, FailureCode, BatchId FROM BatchDepositV2 " +
                "WHERE ActorType = @actorType AND ActorId = @actorId AND IdempotencyKey = @idempotencyKey FOR UPDATE",
                ("@actorType", request.ActorType.ToString()), ("@actorId", request.ActorId), ("@idempotencyKey", request.IdempotencyKey)))
            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                if (!string.Equals(reader.GetString(0), requestHash, StringComparison.Ordinal))
                    return NewResult(request, requestHash, BatchDepositV2Status.Failed, "IdempotencyKeyPayloadMismatch");
                if (!reader.IsDBNull(1))
                    result = JsonConvert.DeserializeObject<BatchDepositV2Result>(reader.GetString(1));
                else
                    result = NewResult(request, requestHash, ParseStatus(reader.GetString(2)),
                        reader.IsDBNull(3) ? null : reader.GetString(3));
            }
            if (result.Status == BatchDepositV2Status.Succeeded)
                result.Details = ReadResultDetails(connection, transaction, result.BatchId);
            return result;
        }

        private static Dictionary<int, PlayerRow> ReadPlayersForUpdate(MySqlConnection connection, MySqlTransaction transaction, IEnumerable<int> userUids)
        {
            var ids = userUids.OrderBy(x => x).ToList();
            var parameters = ids.Select((id, index) => "@user" + index).ToList();
            using (var command = Command(connection, transaction,
                "SELECT UserUID, UserID, UserBalance, AspNetUserId, EntityId, blockFlag, Usersituation, KeyInAward, KeyOutLimit, CreditRebateFg " +
                "FROM Usertable WHERE UserUID IN (" + string.Join(",", parameters) + ") ORDER BY UserUID FOR UPDATE"))
            {
                for (int i = 0; i < ids.Count; i++) command.Parameters.AddWithValue(parameters[i], ids[i]);
                using (var reader = command.ExecuteReader())
                {
                    var rows = new Dictionary<int, PlayerRow>();
                    while (reader.Read())
                    {
                        rows.Add(reader.GetInt32(0), new PlayerRow
                        {
                            UserUid = reader.GetInt32(0), UserId = reader.GetString(1), Balance = reader.GetDecimal(2),
                            ManagerId = reader.GetString(3), EntityId = reader.GetInt32(4), IsBlocked = reader.GetBoolean(5),
                            UserSituation = reader.GetInt32(6), KeyInAward = reader.GetBoolean(7),
                            KeyOutLimit = reader.GetBoolean(8), CreditRebateFg = reader.GetBoolean(9)
                        });
                    }
                    return rows;
                }
            }
        }

        private static ManagerRow ReadManagerForUpdate(MySqlConnection connection, MySqlTransaction transaction, string managerId)
        {
            using (var command = Command(connection, transaction, "SELECT Balance, ParentID FROM AspNetUsers WHERE Id = @id FOR UPDATE", ("@id", managerId)))
            using (var reader = command.ExecuteReader())
                return reader.Read() ? new ManagerRow { Balance = reader.GetDecimal(0), EntityId = reader.GetInt32(1) } : null;
        }

        private static ActorRow ReadActorForUpdate(MySqlConnection connection, MySqlTransaction transaction, string actorId)
        {
            using (var command = Command(connection, transaction,
                "SELECT actor.freeze, actor.ParentID, permission.Player_Deposit, permission.Player_Withdrawal " +
                "FROM AspNetUsers actor LEFT JOIN permissiontable permission ON permission.PermissionUID = actor.Id " +
                "WHERE actor.Id = @id FOR UPDATE", ("@id", actorId)))
            using (var reader = command.ExecuteReader())
                return reader.Read()
                    ? new ActorRow
                    {
                        IsFrozen = reader.GetInt32(0) == 1,
                        EntityId = reader.GetInt32(1),
                        CanDeposit = !reader.IsDBNull(2) && reader.GetInt32(2) == 1,
                        CanWithdraw = !reader.IsDBNull(3) && reader.GetInt32(3) == 1
                    }
                    : null;
        }

        private static bool ReadBalanceUnlimited(MySqlConnection connection, MySqlTransaction transaction, string managerId)
        {
            using (var command = Command(connection, transaction,
                "SELECT BalanceUnlimited FROM permissiontable WHERE PermissionUID = @id FOR UPDATE", ("@id", managerId)))
            using (var reader = command.ExecuteReader())
                return reader.Read() && reader.GetBoolean(0);
        }

        private static string Validate(MySqlConnection connection, MySqlTransaction transaction, BatchDepositV2Request request,
            List<BatchDepositV2RequestDetail> details, Dictionary<int, PlayerRow> players, ManagerRow manager, ActorRow actor,
            double playerBalanceLimit, double depositUnit, CountrySettingData bonusSetting)
        {
            if (actor == null) return "ActorNotFound";
            if (actor.IsFrozen) return "ActorFrozen";
            if (manager == null) return "PayerManagerNotFound";
            if (!CanManageEntity(connection, transaction, actor.EntityId, manager.EntityId)) return "PayerOutOfScope";
            if (details.Count == 0 || details.Select(x => x.UserUID).Distinct().Count() != details.Count) return "InvalidDetails";
            if (details.Any(x => x.OperationMode == BatchDepositV2OperationMode.Deposit) &&
                !actor.CanDeposit) return "NoDepositPermission";
            if (details.Any(x => x.OperationMode == BatchDepositV2OperationMode.Withdraw) &&
                !actor.CanWithdraw) return "NoWithdrawalPermission";
            decimal maxPlayerBalance = Convert.ToDecimal(playerBalanceLimit);
            decimal maxDepositUnit = Convert.ToDecimal(depositUnit);
            string financialFailure = null;
            foreach (var detail in details)
            {
                if (detail.OperationMode != BatchDepositV2OperationMode.Deposit &&
                    detail.OperationMode != BatchDepositV2OperationMode.Withdraw)
                    return "InvalidOperationMode";
                if (detail.RequestAmount <= 0 ||
                    Math.Round(detail.RequestAmount, Program.AccuracyDigitBal) != detail.RequestAmount)
                    return "InvalidAmount";
                if (!players.TryGetValue(detail.UserUID, out PlayerRow player)) return "PlayerNotFound";
                if (!CanManageEntity(connection, transaction, actor.EntityId, player.EntityId)) return "PlayerOutOfScope";
                if (player.IsBlocked) return "PlayerUnavailable";
                decimal amount = detail.RequestAmount;
                if (detail.OperationMode == BatchDepositV2OperationMode.Withdraw)
                {
                    // This must live in DBCACHE as well as H5 so a direct protocol call cannot
                    // bypass the business rule that a player in a game cannot be withdrawn from.
                    if (player.UserSituation > 1) return "PlayerInGameWithdrawalNotAllowed";
                    if (player.Balance - amount < 0)
                        financialFailure = "InsufficientPlayerBalance";
                    else if (player.KeyOutLimit)
                        financialFailure = "KeyOutLimit";
                }
                else
                {
                    decimal bonus = CalculateBonus(player, amount, bonusSetting);
                    decimal creditedAmount = amount + bonus;
                    if (creditedAmount > maxDepositUnit)
                        financialFailure = "OverDepositUnit";
                    else if (player.Balance + creditedAmount > maxPlayerBalance)
                        financialFailure = "PlayerBalanceLimitExceeded";
                }
                if (financialFailure != null) break;
            }
            return financialFailure;
        }

        private static bool IsFinancialLimitFailure(string failure)
        {
            return failure == "InsufficientPlayerBalance" || failure == "OverDepositUnit" ||
                failure == "PlayerBalanceLimitExceeded" || failure == "KeyOutLimit";
        }

        private static bool CanManageEntity(MySqlConnection connection, MySqlTransaction transaction, int actorEntityId, int targetEntityId)
        {
            var visited = new HashSet<int>();
            while (targetEntityId > 1 && visited.Add(targetEntityId))
            {
                if (targetEntityId == actorEntityId) return true;
                using (var command = Command(connection, transaction,
                    "SELECT parentID FROM Entity WHERE id = @id", ("@id", targetEntityId)))
                {
                    object parentId = command.ExecuteScalar();
                    if (parentId == null || parentId == DBNull.Value) return false;
                    targetEntityId = Convert.ToInt32(parentId);
                }
            }
            return targetEntityId == actorEntityId;
        }

        private static decimal CalculateBonus(PlayerRow player, decimal amount, CountrySettingData bonusSetting)
        {
            if (bonusSetting == null || !bonusSetting.ExtraBonusFg || player.KeyInAward ||
                amount < Convert.ToDecimal(bonusSetting.ExtraBonusUnitKeyIn) || player.Balance > Convert.ToDecimal(bonusSetting.ExtraBonusThreshold))
                return 0m;
            return Math.Round(Convert.ToDecimal(bonusSetting.ExtraBonusValue), Program.AccuracyDigit);
        }

        private static decimal GetSignedAmount(BatchDepositV2RequestDetail detail)
        {
            return detail.OperationMode == BatchDepositV2OperationMode.Deposit
                ? detail.RequestAmount
                : -detail.RequestAmount;
        }

        private static BatchDepositV2Result PersistFailure(MySqlConnection connection, MySqlTransaction transaction,
            BatchDepositV2Request request, string requestHash, string failureCode, int operatorEntityId)
        {
            var result = NewResult(request, requestHash, BatchDepositV2Status.Failed, failureCode);
            InsertBatch(connection, transaction, request, requestHash, "Failed", 0m, 0m, result, operatorEntityId);
            return result;
        }

        private static BatchDepositV2Result NewResult(BatchDepositV2Request request, string hash, BatchDepositV2Status status, string failureCode)
        {
            return new BatchDepositV2Result { BatchId = request.BatchId, IdempotencyKey = request.IdempotencyKey, RequestHash = hash,
                Status = status, FailureCode = failureCode, Details = new List<BatchDepositV2ResultDetail>() };
        }

        private static void InsertBatch(MySqlConnection c, MySqlTransaction t, BatchDepositV2Request request, string hash, string status,
            decimal total, decimal totalBonus, BatchDepositV2Result result, int operatorEntityId)
        {
            Execute(c, t, "INSERT INTO BatchDepositV2 (BatchId, ActorType, ActorId, IdempotencyKey, RequestHash, PayerManagerId, OperatorEntityId, EntityId, Status, FailureCode, DetailCount, TotalRequestAmount, TotalExtraBonus, RequestIp, RequestPayload, ResultPayload, CreatedAtUtc, CompletedAtUtc) " +
                "VALUES (@batchId, @actorType, @actorId, @idempotencyKey, @hash, @payerManagerId, @operatorEntityId, @entityId, @status, @failureCode, @detailCount, @total, @totalBonus, @requestIp, @requestPayload, @resultPayload, UTC_TIMESTAMP(6), IF(@status = 'Failed', UTC_TIMESTAMP(6), NULL))",
                ("@batchId", request.BatchId), ("@actorType", request.ActorType.ToString()), ("@actorId", request.ActorId), ("@idempotencyKey", request.IdempotencyKey),
                ("@hash", hash), ("@payerManagerId", request.PayerManagerId), ("@operatorEntityId", operatorEntityId), ("@entityId", operatorEntityId),
                ("@status", status), ("@failureCode", result == null ? null : result.FailureCode), ("@detailCount", request.Details.Count), ("@total", total),
                ("@totalBonus", totalBonus), ("@requestIp", request.RequestIp), ("@requestPayload", JsonConvert.SerializeObject(request)),
                ("@resultPayload", result == null ? null : JsonConvert.SerializeObject(result)));
        }

        private static int UpdatePlayerAndInsertTradeRecord(MySqlConnection c, MySqlTransaction t, BatchDepositV2Request request,
            int operatorEntityId, PlayerRow player,
            decimal amount, decimal extraBonus, decimal afterBalance, double sessionId)
        {
            bool isDeposit = amount > 0;
            Execute(c, t, "UPDATE Usertable SET UserBalance = @balance, SessionID = IF(@isDeposit = 1, @sessionId, SessionID), KeyInAward = IF(@extraBonus > 0, 1, KeyInAward), KeyOutLimit = IF(@extraBonus > 0, 1, KeyOutLimit) WHERE UserUID = @userUid",
                ("@balance", afterBalance), ("@sessionId", sessionId), ("@isDeposit", isDeposit), ("@extraBonus", extraBonus), ("@userUid", player.UserUid));
            Execute(c, t, "INSERT INTO MangerToUserTradeRecord (TradeType, OperationType, TransactionType, Status, OperatorId, UserUID, UserID, OperatorEntityId, EntityID, BeforeBalance, Amount, ExtraBonus, IP, StatusValue, SearchIndex, TimeStamp) " +
                "VALUES ('PlayerBalanceDeal', @operationType, @transactionType, @status, @operatorId, @userUid, @userId, @operatorEntityId, @entityId, @beforeBalance, @amount, @extraBonus, @ip, '2', @searchIndex, @timeStamp)",
                ("@operatorId", request.PayerManagerId), ("@userUid", player.UserUid), ("@userId", player.UserId), ("@operatorEntityId", operatorEntityId),
                ("@entityId", player.EntityId), ("@beforeBalance", player.Balance), ("@amount", amount), ("@extraBonus", extraBonus), ("@ip", request.RequestIp),
                ("@operationType", isDeposit ? "Deposit" : "Withdrawal"), ("@transactionType", isDeposit ? "1" : "2"), ("@status", isDeposit ? "Approved" : "Completed"),
                ("@searchIndex", Program.GetLogSearchIndex(DateTime.Now, player.UserUid)), ("@timeStamp", Program.GetLogTimeStamp(DateTime.Now)));
            using (var command = Command(c, t, "SELECT LAST_INSERT_ID()")) return Convert.ToInt32(command.ExecuteScalar());
        }

        private static void InsertDetail(MySqlConnection c, MySqlTransaction t, string batchId, int sequence,
            PlayerRow player, BatchDepositV2OperationMode operationMode, decimal requestAmount,
            decimal extraBonus, decimal afterBalance, double sessionId, int tradeRecordId)
        {
            Execute(c, t, "INSERT INTO BatchDepositDetailV2 (BatchId, DetailSequence, UserUID, UserId, EntityId, OperationMode, RequestAmount, ExtraBonus, BeforeBalance, AfterBalance, SessionId, TradeRecordId, Status, CreatedAtUtc) VALUES (@batchId, @sequence, @userUid, @userId, @entityId, @operationMode, @amount, @extraBonus, @beforeBalance, @afterBalance, @sessionId, @tradeRecordId, 'Succeeded', UTC_TIMESTAMP(6))",
                ("@batchId", batchId), ("@sequence", sequence), ("@userUid", player.UserUid), ("@userId", player.UserId),
                ("@entityId", player.EntityId), ("@operationMode", operationMode.ToString()), ("@amount", requestAmount),
                ("@extraBonus", extraBonus), ("@beforeBalance", player.Balance), ("@afterBalance", afterBalance), ("@sessionId", sessionId), ("@tradeRecordId", tradeRecordId));
        }

        private static void InsertOutbox(MySqlConnection c, MySqlTransaction t, string batchId, int sequence, PlayerRow player, decimal amount, double sessionId)
        {
            string eventId = Guid.NewGuid().ToString();
            var targetRoutes = ResolveTargetRoutes(player.UserSituation).ToList();
            string status = targetRoutes.Count == 0 ? "Published" : "Pending";
            Execute(c, t, "INSERT INTO BatchDepositOutboxV2 (EventId, BatchId, DetailSequence, UserUID, MessageType, Payload, Status, AttemptCount, NextAttemptAtUtc, DeliveredAtUtc, CreatedAtUtc) VALUES (@eventId, @batchId, @sequence, @userUid, 'UserBalanceDelta', @payload, @status, 0, UTC_TIMESTAMP(6), IF(@status = 'Published', UTC_TIMESTAMP(6), NULL), UTC_TIMESTAMP(6))",
                ("@eventId", eventId), ("@batchId", batchId), ("@sequence", sequence), ("@userUid", player.UserUid), ("@status", status), ("@payload", JsonConvert.SerializeObject(new { EventId = eventId, EventType = "user.balance.delta.v1", UserUID = player.UserUid, Delta = amount, SessionId = sessionId, BatchId = batchId, DetailSequence = sequence, Operation = amount > 0 ? "Deposit" : "Withdrawal" })));
            if (targetRoutes.Count > 0)
            {
                long outboxId;
                using (var command = Command(c, t, "SELECT LAST_INSERT_ID()")) outboxId = Convert.ToInt64(command.ExecuteScalar());
                foreach (string targetRoute in targetRoutes)
                {
                    InsertDelivery(c, t, outboxId, targetRoute);
                }
            }
            BatchDepositV2OperationalLog.Write("info", "repository", "outbox_route_decision",
                new
                {
                    EventId = eventId,
                    BatchId = batchId,
                    DetailSequence = sequence,
                    Player = player.UserId,
                    player.UserUid,
                    player.UserSituation,
                    GameServerCode = Enum.IsDefined(typeof(GameServerCode), player.UserSituation)
                        ? ((GameServerCode)player.UserSituation).ToString()
                        : "Unknown",
                    Amount = amount,
                    OutboxStatus = status,
                    TargetRoutes = targetRoutes
                });
        }

        private static IEnumerable<string> ResolveGameServerRoutes(int gameServerCode)
        {
            GameServerCode gameServer = (GameServerCode)gameServerCode;
            yield return "user-info.game." + gameServer.ToString().ToLowerInvariant();
        }

        private static IEnumerable<string> ResolveTargetRoutes(int gameServerCode)
        {
            if (!Enum.IsDefined(typeof(GameServerCode), gameServerCode) ||
                (GameServerCode)gameServerCode == GameServerCode.None)
                yield break;

            yield return "user-info.login";

            foreach (string gameRoute in ResolveGameServerRoutes(gameServerCode))
                yield return gameRoute;
        }

        private static void InsertDelivery(MySqlConnection c, MySqlTransaction t, long outboxId, string targetRoute)
        {
            Execute(c, t, "INSERT INTO BatchDepositOutboxDeliveryV2 (OutboxId, TargetRoute, Status, AttemptCount, NextAttemptAtUtc, CreatedAtUtc) VALUES (@outboxId, @targetRoute, 'Pending', 0, UTC_TIMESTAMP(6), UTC_TIMESTAMP(6))",
                ("@outboxId", outboxId), ("@targetRoute", targetRoute));
        }

        private static void UpdateBatchSuccess(MySqlConnection c, MySqlTransaction t, string batchId, decimal totalBonus, BatchDepositV2Result result)
        {
            Execute(c, t, "UPDATE BatchDepositV2 SET Status = 'Succeeded', TotalExtraBonus = @totalBonus, ResultPayload = @resultPayload, CompletedAtUtc = UTC_TIMESTAMP(6) WHERE BatchId = @batchId",
                ("@totalBonus", totalBonus), ("@resultPayload", JsonConvert.SerializeObject(result)), ("@batchId", batchId));
        }

        private static BatchDepositV2Status ParseStatus(string status) => Enum.TryParse(status, true, out BatchDepositV2Status parsed) ? parsed : BatchDepositV2Status.Failed;
        private static BatchDepositV2OperationMode ParseOperationMode(string operationMode) =>
            Enum.TryParse(operationMode, true, out BatchDepositV2OperationMode parsed)
                ? parsed : BatchDepositV2OperationMode.Deposit;
        private static MySqlCommand Command(MySqlConnection c, MySqlTransaction t, string sql, params (string, object)[] values)
        {
            var command = new MySqlCommand(sql, c, t);
            foreach (var value in values) command.Parameters.AddWithValue(value.Item1, value.Item2 ?? DBNull.Value);
            return command;
        }
        private static void Execute(MySqlConnection c, MySqlTransaction t, string sql, params (string, object)[] values)
        {
            using (var command = Command(c, t, sql, values)) command.ExecuteNonQuery();
        }

        private sealed class ManagerRow { internal decimal Balance; internal int EntityId; }
        private sealed class ActorRow
        {
            internal bool IsFrozen; internal int EntityId; internal bool CanDeposit; internal bool CanWithdraw;
        }
        private sealed class PlayerRow
        {
            internal int UserUid; internal string UserId; internal decimal Balance; internal string ManagerId; internal int EntityId;
            internal bool IsBlocked; internal int UserSituation; internal bool KeyInAward; internal bool KeyOutLimit; internal bool CreditRebateFg;
        }
    }
}
