using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCacheServer
{
    // RABBITMQ_URI is intentionally environment based so the same binary can run on AWS Linux
    // without storing credentials in source or app.config.
    internal sealed class RabbitMqOutboxMessageBroker : IOutboxMessageBroker, IDisposable
    {
        internal const string ExchangeName = "user-info.direct";
        internal const string AdjustmentIdHeaderName = "x-bdv2-adjustment-id";
        private readonly IConnection connection;
        private readonly IModel channel;

        internal RabbitMqOutboxMessageBroker(string connectionUri)
        {
            var factory = new ConnectionFactory { Uri = new Uri(connectionUri), DispatchConsumersAsync = true };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true, autoDelete: false);
            channel.ConfirmSelect();
        }

        internal static bool TryCreate(out RabbitMqOutboxMessageBroker broker)
        {
            broker = null;
            string uri = Environment.GetEnvironmentVariable("RABBITMQ_URI");
            if (string.IsNullOrWhiteSpace(uri))
            {
                BatchDepositV2OperationalLog.Write("critical", "publisher", "publisher_disabled",
                    new { error = "RABBITMQ_URI is not configured." });
                return false;
            }
            try
            {
                broker = new RabbitMqOutboxMessageBroker(uri);
                return true;
            }
            catch (Exception exception)
            {
                // Preserve the committed outbox rows; operators can restore the broker and
                // restart DBCACHE without losing a financial notification.
                broker?.Dispose();
                broker = null;
                BatchDepositV2OperationalLog.Write("critical", "publisher", "publisher_start_failed",
                    new { error = exception.Message });
                return false;
            }
        }

        public void Publish(OutboxEvent message)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = message.EventId;
            properties.Type = message.MessageType;
            properties.Headers = CreateHeaders(message.OutboxId);
            channel.BasicPublish(ExchangeName, message.TargetRoute, mandatory: true, basicProperties: properties,
                body: Encoding.UTF8.GetBytes(message.Payload));
            if (!channel.WaitForConfirms(TimeSpan.FromSeconds(10)))
                throw new InvalidOperationException("RabbitMQ publisher confirm was not received.");
        }

        internal static IDictionary<string, object> CreateHeaders(long outboxId)
        {
            return new Dictionary<string, object>
            {
                [AdjustmentIdHeaderName] = outboxId
            };
        }

        public void Dispose()
        {
            channel?.Dispose();
            connection?.Dispose();
        }
    }
}
