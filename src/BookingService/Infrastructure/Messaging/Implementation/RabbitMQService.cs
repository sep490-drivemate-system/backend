using BookingService.Infrastructure.Messaging.Interface;
using BookingService.Infrastructure.Messaging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace BookingService.Infrastructure.Messaging.Implementation
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests;
        private readonly string _replyQueueName;

        private readonly string _bookingExchange;
        private readonly string _walletBalanceCheckQueue;
        private readonly string _walletBalanceResponseQueue;
        private readonly string _bookingCreatedQueue;
        private readonly string _bookingUpdatedQueue;
        private readonly string _bookingCancelledQueue;

        private readonly string _walletBalanceCheckRoutingKey;
        private readonly string _walletBalanceResponseRoutingKey;
        private readonly string _bookingCreatedRoutingKey;
        private readonly string _bookingUpdatedRoutingKey;
        private readonly string _bookingCancelledRoutingKey;

        private bool _disposed = false;

        public RabbitMQService(IConfiguration configuration)
        {
            _pendingRequests = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

            var hostName = configuration["RabbitMQ:HostName"];
            var port = int.Parse(configuration["RabbitMQ:Port"]);
            var userName = configuration["RabbitMQ:UserName"];
            var password = configuration["RabbitMQ:Password"];
            var virtualHost = configuration["RabbitMQ:VirtualHost"];

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(30),
                SocketReadTimeout = TimeSpan.FromSeconds(30),
                SocketWriteTimeout = TimeSpan.FromSeconds(30),
                Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = "collie.lmq.cloudamqp.com"
                }
            };

            Console.WriteLine("=== RabbitMQ Configuration ===");
            Console.WriteLine($"Host: {hostName}");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"User: {userName}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Virtual Host: {virtualHost}");
            Console.WriteLine("==============================");
            if (port == 5671)
            {
                factory.Ssl.Enabled = true;
                factory.Ssl.ServerName = hostName;
                factory.Ssl.Version = System.Security.Authentication.SslProtocols.Tls12;
            }

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _bookingExchange = configuration["RabbitMQ:BookingExchange"];
            _walletBalanceCheckQueue = configuration["RabbitMQ:WalletBalanceCheckQueue"];
            _walletBalanceResponseQueue = configuration["RabbitMQ:WalletBalanceResponseQueue"];
            _bookingCreatedQueue = configuration["RabbitMQ:BookingCreatedQueue"];
            _bookingUpdatedQueue = configuration["RabbitMQ:BookingUpdatedQueue"];
            _bookingCancelledQueue = configuration["RabbitMQ:BookingCancelledQueue"];

            _walletBalanceCheckRoutingKey = configuration["RabbitMQ:WalletBalanceCheckRoutingKey"];
            _walletBalanceResponseRoutingKey = configuration["RabbitMQ:WalletBalanceResponseRoutingKey"];
            _bookingCreatedRoutingKey = configuration["RabbitMQ:BookingCreatedRoutingKey"];
            _bookingUpdatedRoutingKey = configuration["RabbitMQ:BookingUpdatedRoutingKey"];
            _bookingCancelledRoutingKey = configuration["RabbitMQ:BookingCancelledRoutingKey"];

            // Declare exchange
            _channel.ExchangeDeclare(_bookingExchange, ExchangeType.Direct, durable: true);

            // Declare queues
            DeclareQueue(_walletBalanceCheckQueue, _walletBalanceCheckRoutingKey);
            DeclareQueue(_walletBalanceResponseQueue, _walletBalanceResponseRoutingKey);
            DeclareQueue(_bookingCreatedQueue, _bookingCreatedRoutingKey);
            DeclareQueue(_bookingUpdatedQueue, _bookingUpdatedRoutingKey);
            DeclareQueue(_bookingCancelledQueue, _bookingCancelledRoutingKey);

            // Create reply queue for RPC pattern
            _replyQueueName = _channel.QueueDeclare().QueueName;

            SetupReplyConsumer();


        }

        private void DeclareQueue(string queueName, string routingKey)
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: queueName, exchange: _bookingExchange, routingKey: routingKey);
        }

        private void SetupReplyConsumer()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var correlationId = ea.BasicProperties.CorrelationId;
                if (!string.IsNullOrEmpty(correlationId) && _pendingRequests.TryRemove(correlationId, out var tcs))
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    tcs.SetResult(response);
                }
                await Task.Yield();
            };

            _channel.BasicConsume(queue: _replyQueueName, autoAck: true, consumer: consumer);
        }

        public async Task<WalletBalanceCheckResponse?> CheckWalletBalanceAsync(Guid userId, decimal amount)
        {

            var request = new WalletBalanceCheckRequest
            {
                UserId = userId,
                Amount = amount
            };
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));

            var correlationId = Guid.NewGuid().ToString();

            var tcs = new TaskCompletionSource<string>();
            _pendingRequests[correlationId] = tcs;

            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = _replyQueueName;
            props.Persistent = true;
            props.ContentType = "application/json";

            var message = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: _bookingExchange,
                routingKey: _walletBalanceCheckRoutingKey,
                basicProperties: props,
                body: body
            );


            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            try
            {
                var responseJson = await tcs.Task.WaitAsync(timeoutCts.Token);
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
        }

        public void PublishBookingEvent<T>(T eventData, string routingKey) where T : class
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));

            var message = JsonConvert.SerializeObject(eventData);
            var body = Encoding.UTF8.GetBytes(message);

            var props = _channel.CreateBasicProperties();
            props.Persistent = true;
            props.ContentType = "application/json";
            props.MessageId = Guid.NewGuid().ToString();
            props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: _bookingExchange,
                routingKey: routingKey,
                basicProperties: props,
                body: body
            );


        }

        public Task PublishBookingEventAsync<T>(T eventData, string routingKey) where T : class
        {
            return Task.Run(() => PublishBookingEvent(eventData, routingKey));
        }

        public void RegisterConsumer(string queueName, Func<string, Task> onMessageReceived)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                try
                {
                    await onMessageReceived(message);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
           
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
