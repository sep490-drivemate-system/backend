using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Data;

namespace PaymentService.Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(PaymentDbContext context) : base(context)
        {
        }

        public async Task<List<Transaction>> GetByBookingIdAsync(Guid bookingId)
        {
            return await _context.Transactions
                .Where(t => t.BookingId == bookingId && !t.IsDelete)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByWalletIdAsync(Guid walletId)
        {
            return await _context.Transactions
                .Where(t => t.FromWalletId == walletId && !t.IsDelete)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByStatusAsync(PaymentStatus status)
        {
            return await _context.Transactions
                .Where(t => t.Status == status && !t.IsDelete)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByPaymentMethodAsync(PaymentMethod paymentMethod)
        {
            return await _context.Transactions
                .Where(t => t.PaymentMethod == paymentMethod && !t.IsDelete)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByReferenceCodeAsync(string referenceCode)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.ReferenceCode == referenceCode && !t.IsDelete);
        }

        public async Task<bool> ExistsByBookingIdAndStatusAsync(Guid bookingId, PaymentStatus status)
        {
            return await _context.Transactions
                .AnyAsync(t => t.BookingId == bookingId && t.Status == status && !t.IsDelete);
        }
    }
}
