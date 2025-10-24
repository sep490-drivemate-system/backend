using BookingService.Application.Interfaces;
using BookingService.Infrastructure.Messaging.Models;

namespace BookingService.Infrastructure.Messaging.Services
{
    public interface IPaymentMessagingService
    {
        Task<WalletBalanceCheckResponse?> CheckWalletBalanceAsync(
            Guid userId, 
            decimal amount);
    }

    public class PaymentMessagingService : IPaymentMessagingService
    {
        private readonly IRabbitMQService _rabbitMQService;

        public PaymentMessagingService(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public async Task<WalletBalanceCheckResponse?> CheckWalletBalanceAsync(
            Guid userId, 
            decimal amount)
        {
            var request = new WalletBalanceCheckRequest
            {
                UserId = userId,
                Amount = amount,
            };

            return await _rabbitMQService.CheckWalletBalanceAsync(request);
        }
    }
}
