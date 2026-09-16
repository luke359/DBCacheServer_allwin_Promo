using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DBCacheServer
{
    // The broker implementation must return only after the broker has accepted the message.
    internal interface IOutboxMessageBroker
    {
        void Publish(OutboxEvent message);
    }

    internal sealed class OutboxEvent
    {
        internal long DeliveryId;
        internal long OutboxId;
        internal string EventId;
        internal string MessageType;
        internal string Payload;
        internal string TargetRoute;
    }

    // At-least-once publisher: a process crash after Publish and before MarkPublished is intentional;
    // the next run sends the same EventId again and the consumer suppresses the duplicate.
    internal sealed class BatchDepositOutboxPublisher
    {
        private const int BatchSize = 100;
        private readonly MysqlAcess mysql;
        private readonly IOutboxMessageBroker broker;
        private readonly BatchDepositV2OperationsOptions options;

        internal BatchDepositOutboxPublisher(MysqlAcess mysql, IOutboxMessageBroker broker)
            : this(mysql, broker, new BatchDepositV2OperationsOptions())
        {
        }

        internal BatchDepositOutboxPublisher(MysqlAcess mysql, IOutboxMessageBroker broker, BatchDepositV2OperationsOptions options)
        {
            this.mysql = mysql;
            this.broker = broker;
            this.options = options;
        }

        internal void Run()
        {
            while (true)
            {
                int published = PublishAvailable();
                Thread.Sleep(published == 0 ? 1000 : 10);
            }
        }

        internal int PublishAvailable()
        {
            int published = 0;
            foreach (OutboxEvent message in ReadPending())
            {
                try
                {
                    broker.Publish(message);
                    MarkPublished(message);
                    published++;
                    BatchDepositV2OperationalLog.Write("info", "publisher", "delivery_published",
                        new
                        {
                            message.EventId,
                            message.DeliveryId,
                            message.OutboxId,
                            route = message.TargetRoute
                        });
                }
                catch (Exception exception)
                {
                    RecordFailure(message, exception.Message);
                }
            }
            return published;
        }

        private List<OutboxEvent> ReadPending()
        {
            return mysql.ExecuteInReadCommitted(connection =>
            {
                var messages = new List<OutboxEvent>();
                using (var command = new MySqlCommand("SELECT d.DeliveryId, o.OutboxId, o.EventId, o.MessageType, o.Payload, d.TargetRoute " +
                    "FROM BatchDepositOutboxDeliveryV2 d INNER JOIN BatchDepositOutboxV2 o ON o.OutboxId = d.OutboxId " +
                    "WHERE d.Status = 'Pending' AND d.NextAttemptAtUtc <= UTC_TIMESTAMP(6) ORDER BY d.DeliveryId LIMIT " + BatchSize, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        messages.Add(new OutboxEvent
                        {
                            DeliveryId = reader.GetInt64(0), OutboxId = reader.GetInt64(1), EventId = Convert.ToString(reader.GetValue(2)),
                            MessageType = reader.GetString(3), Payload = reader.GetString(4), TargetRoute = reader.GetString(5)
                        });
                    }
                }
                return messages;
            });
        }

        private void MarkPublished(OutboxEvent message)
        {
            mysql.ExecuteInTransaction((connection, transaction) =>
            {
                using (var command = new MySqlCommand("UPDATE BatchDepositOutboxDeliveryV2 SET Status = 'Published', PublishedAtUtc = UTC_TIMESTAMP(6), LastError = NULL WHERE DeliveryId = @deliveryId AND Status = 'Pending'", connection, transaction))
                {
                    command.Parameters.AddWithValue("@deliveryId", message.DeliveryId);
                    command.ExecuteNonQuery();
                }
                using (var command = new MySqlCommand("UPDATE BatchDepositOutboxV2 SET Status = 'Published', DeliveredAtUtc = UTC_TIMESTAMP(6), LastError = NULL WHERE OutboxId = @outboxId AND NOT EXISTS (SELECT 1 FROM BatchDepositOutboxDeliveryV2 WHERE OutboxId = @outboxId AND Status <> 'Published')", connection, transaction))
                {
                    command.Parameters.AddWithValue("@outboxId", message.OutboxId);
                    command.ExecuteNonQuery();
                }
                return 0;
            });
        }

        private void RecordFailure(OutboxEvent message, string error)
        {
            string safeError = (error ?? string.Empty).Substring(0, Math.Min(2048, (error ?? string.Empty).Length));
            int attemptCount = 0;
            bool permanentlyFailed = false;
            bool recorded = mysql.ExecuteInTransaction((connection, transaction) =>
            {
                using (var command = new MySqlCommand("SELECT AttemptCount FROM BatchDepositOutboxDeliveryV2 WHERE DeliveryId = @deliveryId AND Status = 'Pending' FOR UPDATE", connection, transaction))
                {
                    command.Parameters.AddWithValue("@deliveryId", message.DeliveryId);
                    object current = command.ExecuteScalar();
                    if (current == null) return false;
                    attemptCount = Convert.ToInt32(current) + 1;
                }

                permanentlyFailed = attemptCount >= options.MaxPublishAttempts;
                using (var command = new MySqlCommand("UPDATE BatchDepositOutboxDeliveryV2 SET Status = @status, AttemptCount = @attemptCount, NextAttemptAtUtc = DATE_ADD(UTC_TIMESTAMP(6), INTERVAL LEAST(300, POW(2, LEAST(@attemptCount, 8))) SECOND), LastError = @error WHERE DeliveryId = @deliveryId", connection, transaction))
                {
                    command.Parameters.AddWithValue("@status", permanentlyFailed ? "Failed" : "Pending");
                    command.Parameters.AddWithValue("@attemptCount", attemptCount);
                    command.Parameters.AddWithValue("@error", safeError);
                    command.Parameters.AddWithValue("@deliveryId", message.DeliveryId);
                    command.ExecuteNonQuery();
                }

                using (var command = new MySqlCommand("UPDATE BatchDepositOutboxV2 SET Status = @status, AttemptCount = GREATEST(AttemptCount, @attemptCount), NextAttemptAtUtc = DATE_ADD(UTC_TIMESTAMP(6), INTERVAL LEAST(300, POW(2, LEAST(@attemptCount, 8))) SECOND), LastError = @error WHERE OutboxId = @outboxId", connection, transaction))
                {
                    command.Parameters.AddWithValue("@status", permanentlyFailed ? "Failed" : "Pending");
                    command.Parameters.AddWithValue("@attemptCount", attemptCount);
                    command.Parameters.AddWithValue("@error", safeError);
                    command.Parameters.AddWithValue("@outboxId", message.OutboxId);
                    command.ExecuteNonQuery();
                }

                return true;
            });

            if (!recorded) return;
            BatchDepositV2OperationalLog.Write(
                permanentlyFailed ? "critical" : "warning",
                "publisher",
                permanentlyFailed ? "delivery_failed_permanently" : "delivery_retry_scheduled",
                new
                {
                    message.EventId,
                    message.DeliveryId,
                    message.OutboxId,
                    route = message.TargetRoute,
                    attemptCount,
                    maxAttempts = options.MaxPublishAttempts,
                    error = safeError
                });
        }
    }
}
