using Microsoft.Extensions.Logging;
using PaymentService.Infrastructure.Messaging.Interfaces;
using PaymentService.Infrastructure.Messaging.Models;

namespace PaymentService.Infrastructure.Messaging.Services
{
    /// <summary>
    /// Mock RabbitMQ service for development/testing when RabbitMQ is not available
    /// </summary>
    public class MockRabbitMQService : IRabbitMQService
    {
        private readonly ILogger<MockRabbitMQService> _logger;

        public MockRabbitMQService(ILogger<MockRabbitMQService> logger)
        {
            _logger = logger;
        }

        public void StartWalletBalanceConsumer()
        {
            _logger.LogInformation("Mock RabbitMQ: Wallet balance consumer started (no-op)");
        }

        public void SendWalletBalanceResponse(WalletBalanceCheckResponse response, string replyTo, string correlationId)
        {
            _logger.LogInformation("Mock RabbitMQ: Would send wallet balance response for correlation {CorrelationId}", correlationId);
        }

        public void PublishPaymentEvent<T>(T eventData, string routingKey) where T : class
        {
            _logger.LogInformation("Mock RabbitMQ: Would publish event {EventType} with routing key {RoutingKey}", 
                typeof(T).Name, routingKey);
        }

        public async Task PublishPaymentEventAsync<T>(T eventData, string routingKey) where T : class
        {
            await Task.Run(() => PublishPaymentEvent(eventData, routingKey));
        }
    }
}
