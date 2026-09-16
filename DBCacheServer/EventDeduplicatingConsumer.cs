using MySql.Data.MySqlClient;
using System;

namespace DBCacheServer
{
    // The handler's side effects and the event receipt are committed together.
    internal sealed class EventDeduplicatingConsumer
    {
        private readonly MysqlAcess mysql;

        internal EventDeduplicatingConsumer(MysqlAcess mysql)
        {
            this.mysql = mysql;
        }

        // Returns false when this consumer has already completed this event.
        internal bool Consume(string consumerName, OutboxEvent message, Action<MySqlConnection, MySqlTransaction, OutboxEvent> handler)
        {
            return mysql.ExecuteInTransaction((connection, transaction) =>
            {
                try
                {
                    using (var command = new MySqlCommand("INSERT INTO BatchDepositConsumedEventV2 (ConsumerName, EventId, ProcessedAtUtc) VALUES (@consumerName, @eventId, UTC_TIMESTAMP(6))", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@consumerName", consumerName);
                        command.Parameters.AddWithValue("@eventId", message.EventId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (MySqlException exception) when (exception.Number == 1062)
                {
                    return false;
                }

                handler(connection, transaction, message);
                return true;
            });
        }
    }
}
