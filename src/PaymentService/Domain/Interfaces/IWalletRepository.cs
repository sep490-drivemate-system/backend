using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet?> GetByIdWithTransactionsAsync(Guid id);
        Task<bool> UpdateBalanceAsync(Guid walletId, decimal newBalance);
        Task<List<Wallet>> GetWalletsWithBalanceGreaterThanAsync(decimal amount);
    }
}
