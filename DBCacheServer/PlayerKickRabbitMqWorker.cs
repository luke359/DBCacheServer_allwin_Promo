using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using PlayerKick.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DBCacheServer;

internal sealed record PlayerKickPlayerSnapshot(int UserUID, int EntityId, string UserID, int UserSituation);

internal sealed class PlayerKickRabbitMqWorker : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly Func<int, PlayerKickPlayerSnapshot?> _getPlayer;
    private readonly Func<PlayerKickCommandV1, bool> _dispatch;
    private readonly ConcurrentDictionary<int, PendingKick> _pending = new();
    private readonly CancellationTokenSource _stopping = new();
    private IConnection? _connection;
    private IModel? _channel;

    private sealed class PendingKick
    {
        internal string CommandId = string.Empty;
        internal TaskCompletionSource<bool> Offline =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    internal PlayerKickRabbitMqWorker(
        Func<int, PlayerKickPlayerSnapshot?> getPlayer,
        Func<PlayerKickCommandV1, bool> dispatch)
    {
        _getPlayer = getPlayer;
        _dispatch = dispatch;
    }

    internal void Run()
    {
        while (!_stopping.IsCancellationRequested)
        {
            try
            {
                ConnectAndConsume();
                while (!_stopping.IsCancellationRequested && _connection is { IsOpen: true })
                    Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PlayerKickMQ] connection/consumer error: " + ex.Message);
            }
            finally
            {
                DisposeConnection();
            }

            if (!_stopping.IsCancellationRequested) Thread.Sleep(5000);
        }
    }

    internal void NotifyUserSituationChanged(int userUid, int userSituation)
    {
        if (userSituation == 0 && _pending.TryGetValue(userUid, out PendingKick? pending))
            pending.Offline.TrySetResult(true);
    }

    private void ConnectAndConsume()
    {
        string? uri = Environment.GetEnvironmentVariable("RABBITMQ_URI");
        if (string.IsNullOrWhiteSpace(uri))
            throw new InvalidOperationException("RABBITMQ_URI is not configured.");
        var factory = new ConnectionFactory
        {
            Uri = new Uri(uri),
            DispatchConsumersAsync = true,
            AutomaticRecoveryEnabled = true,
            TopologyRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
        };
        _connection = factory.CreateConnection("dbcache-player-kick");
        _channel = _connection.CreateModel();
        DeclareTopology(_channel);
        _channel.ConfirmSelect();
        _channel.BasicQos(0, 1, false);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnCommandAsync;
        _channel.BasicConsume(PlayerKickContract.DbCacheQueueName, false, consumer);
        Console.WriteLine("[PlayerKickMQ] consumer connected.");
    }

    private static void DeclareTopology(IModel channel)
    {
        channel.ExchangeDeclare(PlayerKickContract.ExchangeName, ExchangeType.Direct, true, false);
        channel.ExchangeDeclare(PlayerKickContract.DeadLetterExchangeName, ExchangeType.Fanout, true, false);
        var queueArgs = new Dictionary<string, object>
        {
            ["x-dead-letter-exchange"] = PlayerKickContract.DeadLetterExchangeName
        };
        channel.QueueDeclare(PlayerKickContract.DbCacheQueueName, true, false, false, queueArgs);
        channel.QueueDeclare(PlayerKickContract.H5ApiQueueName, true, false, false, queueArgs);
        channel.QueueDeclare(PlayerKickContract.DeadLetterQueueName, true, false, false);
        channel.QueueBind(PlayerKickContract.DbCacheQueueName, PlayerKickContract.ExchangeName,
            PlayerKickContract.RequestedRoutingKey);
        channel.QueueBind(PlayerKickContract.H5ApiQueueName, PlayerKickContract.ExchangeName,
            PlayerKickContract.CompletedRoutingKey);
        channel.QueueBind(PlayerKickContract.H5ApiQueueName, PlayerKickContract.ExchangeName,
            PlayerKickContract.FailedRoutingKey);
        channel.QueueBind(PlayerKickContract.DeadLetterQueueName, PlayerKickContract.DeadLetterExchangeName, string.Empty);
    }

    private async Task OnCommandAsync(object sender, BasicDeliverEventArgs args)
    {
        PlayerKickCommandV1? command = null;
        try
        {
            command = JsonSerializer.Deserialize<PlayerKickCommandV1>(args.Body.Span, JsonOptions)
                ?? throw new JsonException("Empty player kick command.");
            Validate(command);

            PlayerKickResultV1 result = await ExecuteAsync(command);
            PublishResult(result);
            _channel?.BasicAck(args.DeliveryTag, false);
        }
        catch (JsonException ex)
        {
            Console.WriteLine("[PlayerKickMQ] invalid command dead-lettered: " + ex.Message);
            _channel?.BasicReject(args.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[PlayerKickMQ] transient command failure: " + ex.Message);
            RetryOrDeadLetter(args);
        }
        finally
        {
            if (command != null && _pending.TryGetValue(command.UserUID, out PendingKick? pending) &&
                pending.CommandId == command.CommandId)
                _pending.TryRemove(new KeyValuePair<int, PendingKick>(command.UserUID, pending));
        }
    }

    private static void Validate(PlayerKickCommandV1 command)
    {
        if (command.ContractVersion != PlayerKickContract.Version || command.UserUID <= 0 ||
            command.EntityId <= 0 || string.IsNullOrWhiteSpace(command.AgentId) ||
            string.IsNullOrWhiteSpace(command.Account) || string.IsNullOrWhiteSpace(command.CommandId))
            throw new JsonException("Invalid player kick command contract.");
    }

    private async Task<PlayerKickResultV1> ExecuteAsync(PlayerKickCommandV1 command)
    {
        if (DateTime.UtcNow >= command.ExpiresAtUtc)
            return Failed(command, "CommandExpired");

        PlayerKickPlayerSnapshot? player = _getPlayer(command.UserUID);
        string expectedUserId = command.AgentId + "_" + command.Account;
        if (player == null || player.EntityId != command.EntityId ||
            !string.Equals(player.UserID, expectedUserId, StringComparison.Ordinal))
            return Failed(command, "PlayerOwnershipMismatch");

        if (player.UserSituation == 0)
            return Completed(command);

        var pending = new PendingKick { CommandId = command.CommandId };
        if (!_pending.TryAdd(command.UserUID, pending))
            return Failed(command, "PlayerKickAlreadyProcessing");

        if (!_dispatch(command))
            return Failed(command, "TargetServerUnavailable");

        DateTime executionDeadline = DateTime.UtcNow.AddSeconds(PlayerKickContract.DbCacheProcessingTimeoutSeconds);
        if (executionDeadline > command.ExpiresAtUtc) executionDeadline = command.ExpiresAtUtc;
        TimeSpan remaining = executionDeadline - DateTime.UtcNow;
        if (remaining <= TimeSpan.Zero) return Failed(command, "CommandExpired");

        Task completed = await Task.WhenAny(pending.Offline.Task, Task.Delay(remaining));
        return completed == pending.Offline.Task && pending.Offline.Task.Result
            ? Completed(command)
            : Failed(command, "PlayerOfflineTimeout");
    }

    private void PublishResult(PlayerKickResultV1 result)
    {
        if (_channel is not { IsOpen: true }) throw new InvalidOperationException("RabbitMQ channel is closed.");
        byte[] body = JsonSerializer.SerializeToUtf8Bytes(result, JsonOptions);
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";
        properties.Type = nameof(PlayerKickResultV1);
        properties.MessageId = result.CommandId;
        string routingKey = result.Status == PlayerKickStatus.Completed
            ? PlayerKickContract.CompletedRoutingKey
            : PlayerKickContract.FailedRoutingKey;
        _channel.BasicPublish(PlayerKickContract.ExchangeName, routingKey, true, properties, body);
        if (!_channel.WaitForConfirms(TimeSpan.FromSeconds(2)))
            throw new TimeoutException("Player kick result publisher confirm timed out.");
    }

    private void RetryOrDeadLetter(BasicDeliverEventArgs args)
    {
        if (_channel is not { IsOpen: true }) return;
        int retry = 0;
        if (args.BasicProperties?.Headers != null &&
            args.BasicProperties.Headers.TryGetValue("x-player-kick-retry", out object? raw))
        {
            if (raw is byte[] bytes) int.TryParse(System.Text.Encoding.UTF8.GetString(bytes), out retry);
            else int.TryParse(raw?.ToString(), out retry);
        }
        if (retry >= 3)
        {
            _channel.BasicReject(args.DeliveryTag, false);
            return;
        }
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = args.BasicProperties?.ContentType ?? "application/json";
        properties.Type = args.BasicProperties?.Type;
        properties.MessageId = args.BasicProperties?.MessageId;
        properties.Expiration = args.BasicProperties?.Expiration;
        properties.Headers = new Dictionary<string, object>
        {
            ["x-player-kick-retry"] = System.Text.Encoding.UTF8.GetBytes((retry + 1).ToString())
        };
        _channel.BasicPublish(PlayerKickContract.ExchangeName, PlayerKickContract.RequestedRoutingKey,
            true, properties, args.Body);
        if (_channel.WaitForConfirms(TimeSpan.FromSeconds(2)))
            _channel.BasicAck(args.DeliveryTag, false);
        else
            _channel.BasicNack(args.DeliveryTag, false, true);
    }

    private static PlayerKickResultV1 Completed(PlayerKickCommandV1 command) => new()
    {
        CommandId = command.CommandId,
        UserUID = command.UserUID,
        Status = PlayerKickStatus.Completed,
        CompletedAtUtc = DateTime.UtcNow
    };

    private static PlayerKickResultV1 Failed(PlayerKickCommandV1 command, string code) => new()
    {
        CommandId = command.CommandId,
        UserUID = command.UserUID,
        Status = PlayerKickStatus.Failed,
        FailureCode = code,
        CompletedAtUtc = DateTime.UtcNow
    };

    private void DisposeConnection()
    {
        _channel?.Dispose();
        _channel = null;
        _connection?.Dispose();
        _connection = null;
    }

    public void Dispose()
    {
        _stopping.Cancel();
        DisposeConnection();
        _stopping.Dispose();
    }
}
