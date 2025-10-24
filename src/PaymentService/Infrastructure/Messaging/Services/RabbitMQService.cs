using PaymentService.Infrastructure.Messaging.Interfaces;
using PaymentService.Infrastructure.Messaging.Models;
using PaymentService.Infrastructure.Messaging.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using PaymentService.Application.Interfaces;

namespace PaymentService.Infrastructure.Messaging.Services
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly RabbitMQSettings _settings;
        private readonly ILogger<RabbitMQService> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private bool _disposed = false;
        private bool _isConnected = false;

        public RabbitMQService(
            IOptions<RabbitMQSettings> settings, 
            ILogger<RabbitMQService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _settings = settings.Value;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

            try
            {
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

                _logger.LogInformation("Attempting to connect to RabbitMQ at {HostName}:{Port} with user {UserName}", 
                    _settings.HostName, _settings.Port, _settings.UserName);

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                // Declare exchange
                _channel.ExchangeDeclare(_settings.PaymentExchange, ExchangeType.Direct, durable: true);

                // Declare queues
                _channel.QueueDeclare(_settings.WalletBalanceCheckQueue, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueDeclare(_settings.WalletBalanceResponseQueue, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueDeclare(_settings.PaymentProcessedQueue, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueDeclare(_settings.PaymentFailedQueue, durable: true, exclusive: false, autoDelete: false);

                // Bind queues to exchange
                _channel.QueueBind(_settings.WalletBalanceCheckQueue, _settings.PaymentExchange, _settings.WalletBalanceCheckRoutingKey);
                _channel.QueueBind(_settings.WalletBalanceResponseQueue, _settings.PaymentExchange, _settings.WalletBalanceResponseRoutingKey);
                _channel.QueueBind(_settings.PaymentProcessedQueue, _settings.PaymentExchange, _settings.PaymentProcessedRoutingKey);
                _channel.QueueBind(_settings.PaymentFailedQueue, _settings.PaymentExchange, _settings.PaymentFailedRoutingKey);

                _isConnected = true;
                _logger.LogInformation("PaymentService RabbitMQ initialized successfully");
            }
            catch (Exception ex)
            {
                _isConnected = false;
                _logger.LogError(ex, "Failed to initialize PaymentService RabbitMQ. Service will continue without RabbitMQ functionality.");
                // Don't throw - let service continue without RabbitMQ
            }
        }

        public void StartWalletBalanceConsumer()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(RabbitMQService));
            
            if (!_isConnected)
            {
                _logger.LogWarning("Cannot start wallet balance consumer - RabbitMQ not connected");
                return;
            }

            try
            {
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
                            _logger.LogDebug("Processing wallet balance check for user {UserId}, amount {Amount}", 
                                request.UserId, request.Amount);

                            // Process wallet balance check using wallet service
                            response = await ProcessWalletBalanceCheck(request);
                            response.CorrelationId = request.CorrelationId;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing wallet balance check request");
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

                _channel.BasicConsume(queue: _settings.WalletBalanceCheckQueue, autoAck: false, consumer: consumer);
                _logger.LogInformation("Started wallet balance consumer for queue {QueueName}", _settings.WalletBalanceCheckQueue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting up wallet balance consumer");
                throw;
            }
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

                _logger.LogDebug("Sent wallet balance response for correlation ID {CorrelationId}", correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending wallet balance response for correlation ID {CorrelationId}", correlationId);
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
                    exchange: _settings.PaymentExchange,
                    routingKey: routingKey,
                    basicProperties: props,
                    body: body);

                _logger.LogDebug("Published payment event {EventType} with routing key {RoutingKey}", 
                    typeof(T).Name, routingKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing payment event {EventType} with routing key {RoutingKey}", 
                    typeof(T).Name, routingKey);
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
                    _logger.LogInformation("Wallet not found for user {UserId}, creating new wallet", request.UserId);
                    
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
                _logger.LogError(ex, "Error processing wallet balance check for user {UserId}", request.UserId);
                
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
                    _logger.LogError(ex, "Error disposing PaymentService RabbitMQ");
                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }
}
