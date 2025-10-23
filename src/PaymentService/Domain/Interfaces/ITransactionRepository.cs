using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;

namespace PaymentService.Domain.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetByBookingIdAsync(Guid bookingId);
        Task<List<Transaction>> GetByWalletIdAsync(Guid walletId);
        Task<List<Transaction>> GetByStatusAsync(PaymentStatus status);
        Task<List<Transaction>> GetByPaymentMethodAsync(PaymentMethod paymentMethod);
        Task<Transaction?> GetByReferenceCodeAsync(string referenceCode);
        Task<bool> ExistsByBookingIdAndStatusAsync(Guid bookingId, PaymentStatus status);
    }
}
