using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;

namespace PaymentService.Domain.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet?> GetByIdWithTransactionsAsync(Guid id);
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task<bool> UpdateBalanceAsync(Guid walletId, decimal newBalance);
        Task<List<Wallet>> GetWalletsWithBalanceGreaterThanAsync(decimal amount);

        Task<bool> CheckAndDeductWallet(Guid userId, decimal amount, Guid bookingId, Guid? drivingSessionId = null);
    }
}
