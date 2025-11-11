using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet?> GetByIdWithTransactionsAsync(Guid id);
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task<bool> UpdateBalanceAsync(Guid walletId, decimal newBalance);
        Task<List<Wallet>> GetWalletsWithBalanceGreaterThanAsync(decimal amount);

        Task<(bool IsSuccess, string Message, decimal CurrentBalance)> CheckAndDeductWallet(Guid userId, decimal amount, Guid bookingId, Guid? drivingSessionId = null);
    }
}
