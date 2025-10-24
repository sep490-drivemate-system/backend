using BookingService.Application.Interfaces;
using BookingService.Infrastructure.Messaging.Models;
using BookingService.Infrastructure.Messaging.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace BookingService.Infrastructure.Messaging.Services
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly RabbitMQSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests;
        private readonly string _replyQueueName;
        private bool _disposed = false;

        public RabbitMQService(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQService> logger)
        {
            _settings = settings.Value;
            _pendingRequests = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(30),
                SocketReadTimeout = TimeSpan.FromSeconds(30),
                SocketWriteTimeout = TimeSpan.FromSeconds(30)
            };

            // Enable SSL/TLS if using port 5671
            if (_settings.Port == 5671)
            {
                factory.Ssl.Enabled = true;
                factory.Ssl.ServerName = _settings.HostName;
                factory.Ssl.Version = System.Security.Authentication.SslProtocols.Tls12;
            }

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare exchange
            _channel.ExchangeDeclare(_settings.BookingExchange, ExchangeType.Direct, durable: true);

            // Declare queues
            _channel.QueueDeclare(_settings.WalletBalanceCheckQueue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(_settings.WalletBalanceResponseQueue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(_settings.BookingCreatedQueue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(_settings.BookingUpdatedQueue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(_settings.BookingCancelledQueue, durable: true, exclusive: false, autoDelete: false);

            // Bind queues to exchange
            _channel.QueueBind(_settings.WalletBalanceCheckQueue, _settings.BookingExchange, _settings.WalletBalanceCheckRoutingKey);
            _channel.QueueBind(_settings.WalletBalanceResponseQueue, _settings.BookingExchange, _settings.WalletBalanceResponseRoutingKey);
            _channel.QueueBind(_settings.BookingCreatedQueue, _settings.BookingExchange, _settings.BookingCreatedRoutingKey);
            _channel.QueueBind(_settings.BookingUpdatedQueue, _settings.BookingExchange, _settings.BookingUpdatedRoutingKey);
            _channel.QueueBind(_settings.BookingCancelledQueue, _settings.BookingExchange, _settings.BookingCancelledRoutingKey);

            // Create reply queue for RPC pattern
            _replyQueueName = _channel.QueueDeclare().QueueName;

            // Setup consumer for reply queue
            SetupReplyConsumer();


        }

        public async Task<WalletBalanceCheckResponse?> CheckWalletBalanceAsync(
            WalletBalanceCheckRequest request,
            CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));


            var correlationId = Guid.NewGuid().ToString();
            request.CorrelationId = correlationId;

            var tcs = new TaskCompletionSource<string>();
            _pendingRequests[correlationId] = tcs;

            try
            {
                var props = _channel.CreateBasicProperties();
                props.CorrelationId = correlationId;
                props.ReplyTo = _replyQueueName;
                props.Persistent = true;
                props.MessageId = request.MessageId;
                props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                var message = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(
                    exchange: _settings.BookingExchange,
                    routingKey: _settings.WalletBalanceCheckRoutingKey,
                    basicProperties: props,
                    body: body);


                // Wait for response with timeout (30 seconds)
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

                var responseJson = await tcs.Task.WaitAsync(combinedCts.Token);
                var response = JsonConvert.DeserializeObject<WalletBalanceCheckResponse>(responseJson);



                return response;
            }
            catch (OperationCanceledException)
            {
                _pendingRequests.TryRemove(correlationId, out _);


                return new WalletBalanceCheckResponse
                {
                    IsSuccess = false,
                    HasSufficientBalance = false,
                    Message = "Request timeout",
                    UserId = request.UserId,
                    CorrelationId = correlationId
                };
            }
            catch (Exception ex)
            {
                _pendingRequests.TryRemove(correlationId, out _);
                throw;
            }
        }

        public void PublishBookingEvent<T>(T eventData, string routingKey) where T : class
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));


            var json = JsonConvert.SerializeObject(eventData);
            var body = Encoding.UTF8.GetBytes(json);

            var props = _channel.CreateBasicProperties();
            props.Persistent = true;
            props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: _settings.BookingExchange,
                routingKey: routingKey,
                basicProperties: props,
                body: body);


        }

        public async Task PublishBookingEventAsync<T>(T eventData, string routingKey) where T : class
        {
            await Task.Run(() => PublishBookingEvent(eventData, routingKey));
        }

        private void SetupReplyConsumer()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var correlationId = ea.BasicProperties.CorrelationId;
                if (!string.IsNullOrEmpty(correlationId) && _pendingRequests.TryRemove(correlationId, out var tcs))
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    tcs.SetResult(response);
                }
            };

            _channel.BasicConsume(consumer: consumer, queue: _replyQueueName, autoAck: true);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _channel?.Close();
                _connection?.Close();
                _channel?.Dispose();
                _connection?.Dispose();

                _disposed = true;

            }
        }
    }
}
