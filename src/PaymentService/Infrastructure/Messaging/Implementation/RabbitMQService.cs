using PaymentService.Infrastructure.Messaging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Messaging.Interfaces;

namespace PaymentService.Infrastructure.Messaging.Implementation
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _paymentExchange;
        private readonly string _walletBalanceCheckQueue;
        private readonly string _walletBalanceResponseQueue;
        private readonly string _paymentProcessedQueue;
        private readonly string _paymentFailedQueue;
        private readonly string _walletBalanceCheckRoutingKey;
        private readonly string _walletBalanceResponseRoutingKey;
        private readonly string _paymentProcessedRoutingKey;
        private readonly string _paymentFailedRoutingKey;
        private bool _disposed = false;
        private bool _isConnected = false;

        public RabbitMQService(
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            try
            {
                // Read from configuration
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


                // Read exchange and queue settings from configuration
                var paymentExchange = configuration["RabbitMQ:PaymentExchange"];
                var walletBalanceCheckQueue = configuration["RabbitMQ:WalletBalanceCheckQueue"];
                var walletBalanceResponseQueue = configuration["RabbitMQ:WalletBalanceResponseQueue"];
                var paymentProcessedQueue = configuration["RabbitMQ:PaymentProcessedQueue"];
                var paymentFailedQueue = configuration["RabbitMQ:PaymentFailedQueue"];

                var walletBalanceCheckRoutingKey = configuration["RabbitMQ:WalletBalanceCheckRoutingKey"];
                var walletBalanceResponseRoutingKey = configuration["RabbitMQ:WalletBalanceResponseRoutingKey"];
                var paymentProcessedRoutingKey = configuration["RabbitMQ:PaymentProcessedRoutingKey"];
                var paymentFailedRoutingKey = configuration["RabbitMQ:PaymentFailedRoutingKey"];

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                // Declare exchange
                _channel.ExchangeDeclare(paymentExchange, ExchangeType.Direct, durable: true);

                // Declare queues
                _channel.QueueDeclare(walletBalanceCheckQueue, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueDeclare(walletBalanceResponseQueue, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueDeclare(paymentProcessedQueue, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueDeclare(paymentFailedQueue, durable: true, exclusive: false, autoDelete: false);

                // Bind queues to exchange
                _channel.QueueBind(walletBalanceCheckQueue, paymentExchange, walletBalanceCheckRoutingKey);
                _channel.QueueBind(walletBalanceResponseQueue, paymentExchange, walletBalanceResponseRoutingKey);
                _channel.QueueBind(paymentProcessedQueue, paymentExchange, paymentProcessedRoutingKey);
                _channel.QueueBind(paymentFailedQueue, paymentExchange, paymentFailedRoutingKey);

                // Store the values in readonly fields
                _paymentExchange = paymentExchange;
                _walletBalanceCheckQueue = walletBalanceCheckQueue;
                _walletBalanceResponseQueue = walletBalanceResponseQueue;
                _paymentProcessedQueue = paymentProcessedQueue;
                _paymentFailedQueue = paymentFailedQueue;
                _walletBalanceCheckRoutingKey = walletBalanceCheckRoutingKey;
                _walletBalanceResponseRoutingKey = walletBalanceResponseRoutingKey;
                _paymentProcessedRoutingKey = paymentProcessedRoutingKey;
                _paymentFailedRoutingKey = paymentFailedRoutingKey;

                _isConnected = true;
            }
            catch (Exception ex)
            {
                _isConnected = false;
            }
        }

        public void StartWalletBalanceConsumer()
        {

            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));

            if (!_isConnected)
            {
                return;
            }

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                WalletBalanceCheckResponse? response = null;
                try
                {
                    var body = ea.Body.ToArray();
                    var requestJson = Encoding.UTF8.GetString(body);
                    var request = JsonConvert.DeserializeObject<WalletBalanceCheckRequest>(requestJson);
                    if (request != null)
                    {
                        // request.UserId, request.Amount);
                        // Process wallet balance check using wallet service
                        Console.WriteLine("téttttttt");
                        response = await ProcessWalletBalanceCheck(request);
                        response.CorrelationId = request.CorrelationId;
                    }
                }
                catch (Exception ex)
                {
                    response = new WalletBalanceCheckResponse
                    {
                        IsSuccess = false,
                        HasSufficientBalance = false,
                        Message = "Internal error occurred while checking wallet balance",
                        UserId = Guid.Empty
                    };
                }
                finally
                {
                    // Send response back
                    if (response != null && !string.IsNullOrEmpty(ea.BasicProperties.ReplyTo))
                    {
                        SendWalletBalanceResponse(response, ea.BasicProperties.ReplyTo, ea.BasicProperties.CorrelationId);
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(queue: _walletBalanceCheckQueue, autoAck: false, consumer: consumer);

        }

        public void SendWalletBalanceResponse(WalletBalanceCheckResponse response, string replyTo, string correlationId)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));

            try
            {
                var responseJson = JsonConvert.SerializeObject(response);
                var responseBody = Encoding.UTF8.GetBytes(responseJson);

                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = correlationId;
                replyProps.Persistent = true;

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: replyTo,
                    basicProperties: replyProps,
                    body: responseBody);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void PublishPaymentEvent<T>(T eventData, string routingKey) where T : class
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));

            try
            {
                var json = JsonConvert.SerializeObject(eventData);
                var body = Encoding.UTF8.GetBytes(json);

                var props = _channel.CreateBasicProperties();
                props.Persistent = true;
                props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                _channel.BasicPublish(
                    exchange: _paymentExchange,
                    routingKey: routingKey,
                    basicProperties: props,
                    body: body);


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task PublishPaymentEventAsync<T>(T eventData, string routingKey) where T : class
        {
            await Task.Run(() => PublishPaymentEvent(eventData, routingKey));
        }

        private async Task<WalletBalanceCheckResponse> ProcessWalletBalanceCheck(WalletBalanceCheckRequest request)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();

                // Check if wallet exists for user
                var wallet = await walletService.GetWalletByUserIdAsync(request.UserId);

                if (wallet == null)
                {
                    // Create new wallet with 0 balance


                    wallet = await walletService.CreateWalletAsync(request.UserId);

                    return new WalletBalanceCheckResponse
                    {
                        IsSuccess = true,
                        HasSufficientBalance = false,
                        CurrentBalance = 0,
                        RequestedAmount = request.Amount,
                        UserId = request.UserId,
                        Message = "Wallet created with 0 balance. Please top up your wallet."
                    };
                }

                // Compare balance with requested amount
                bool hasSufficientBalance = wallet.Balance >= request.Amount;

                return new WalletBalanceCheckResponse
                {
                    IsSuccess = true,
                    HasSufficientBalance = hasSufficientBalance,
                    CurrentBalance = wallet.Balance,
                    RequestedAmount = request.Amount,
                    UserId = request.UserId,
                    Message = hasSufficientBalance
                        ? "Sufficient balance available"
                        : $"Insufficient balance. Current: {wallet.Balance:C}, Required: {request.Amount:C}"
                };
            }
            catch (Exception ex)
            {


                return new WalletBalanceCheckResponse
                {
                    IsSuccess = false,
                    HasSufficientBalance = false,
                    CurrentBalance = 0,
                    RequestedAmount = request.Amount,
                    UserId = request.UserId,
                    Message = "Error occurred while checking wallet balance"
                };
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    _channel?.Close();
                    _connection?.Close();
                    _channel?.Dispose();
                    _connection?.Dispose();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }
}
