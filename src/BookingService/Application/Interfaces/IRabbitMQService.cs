using BookingService.Infrastructure.Messaging.Models;

namespace BookingService.Application.Interfaces
{
    public interface IRabbitMQService
    {
        Task<WalletBalanceCheckResponse?> CheckWalletBalanceAsync(
           WalletBalanceCheckRequest request,
           CancellationToken cancellationToken = default);

        void PublishBookingEvent<T>(T eventData, string routingKey) where T : class;

        Task PublishBookingEventAsync<T>(T eventData, string routingKey) where T : class;
    }
}
