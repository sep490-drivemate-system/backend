using PaymentService.Infrastructure.Messaging.Models;

namespace PaymentService.Infrastructure.Messaging.Interfaces
{
    public interface IRabbitMQService
    {
        void StartWalletBalanceConsumer();

        void SendWalletBalanceResponse(WalletBalanceCheckResponse response, string replyTo, string correlationId);
        void PublishPaymentEvent<T>(T eventData, string routingKey) where T : class;
        Task PublishPaymentEventAsync<T>(T eventData, string routingKey) where T : class;
    }
}
