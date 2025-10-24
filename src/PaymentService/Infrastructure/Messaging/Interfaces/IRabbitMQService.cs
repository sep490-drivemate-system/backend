using PaymentService.Infrastructure.Messaging.Models;

namespace PaymentService.Infrastructure.Messaging.Interfaces
{
    public interface IRabbitMQService
    {
        /// <summary>
        /// Setup consumer to handle wallet balance check requests
        /// </summary>
        void StartWalletBalanceConsumer();

        /// <summary>
        /// Send wallet balance check response back to BookingService
        /// </summary>
        void SendWalletBalanceResponse(WalletBalanceCheckResponse response, string replyTo, string correlationId);

        /// <summary>
        /// Publish payment events
        /// </summary>
        void PublishPaymentEvent<T>(T eventData, string routingKey) where T : class;

        /// <summary>
        /// Publish payment events asynchronously
        /// </summary>
        Task PublishPaymentEventAsync<T>(T eventData, string routingKey) where T : class;
    }
}
