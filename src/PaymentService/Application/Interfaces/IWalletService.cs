using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces
{
    public interface IWalletService
    {
        /// <summary>
        /// Get wallet by user ID
        /// </summary>
        Task<Wallet?> GetWalletByUserIdAsync(Guid userId);

        /// <summary>
        /// Create new wallet for user with 0 balance
        /// </summary>
        Task<Wallet> CreateWalletAsync(Guid userId);

        /// <summary>
        /// Update wallet balance
        /// </summary>
        Task<Wallet> UpdateWalletBalanceAsync(Guid userId, decimal amount);

        /// <summary>
        /// Check if user has sufficient balance
        /// </summary>
        Task<bool> HasSufficientBalanceAsync(Guid userId, decimal amount);
    }
}
