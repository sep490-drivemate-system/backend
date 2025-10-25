using BookingService.Infrastructure.Messaging.Models;

namespace BookingService.Infrastructure.Messaging.Interface
{
    public interface IRabbitMQService : IDisposable
    {
        Task<WalletBalanceCheckResponse?> CheckWalletBalanceAsync(Guid userId, decimal amout);
        void PublishBookingEvent<T>(T eventData, string routingKey) where T : class;

        Task PublishBookingEventAsync<T>(T eventData, string routingKey) where T : class;
    }
}
