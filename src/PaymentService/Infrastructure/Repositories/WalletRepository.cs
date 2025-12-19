using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Data;

namespace PaymentService.Infrastructure.Repositories
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(PaymentDbContext context) : base(context)
        {
        }

        public async Task<Wallet?> GetByIdWithTransactionsAsync(Guid id)
        {
            return await _context.Wallets
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDelete);
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.Id == userId && !w.IsDelete);
        }

        public async Task<bool> UpdateBalanceAsync(Guid walletId, decimal newBalance)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet == null || wallet.IsDelete)
            {
                return false;
            }

            wallet.Balance = newBalance;
            wallet.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Wallet>> GetWalletsWithBalanceGreaterThanAsync(decimal amount)
        {
            return await _context.Wallets
                .Where(w => w.Balance > amount && !w.IsDelete)
                .OrderByDescending(w => w.Balance)
                .ToListAsync();
        }

        public async Task<bool> CheckAndDeductBookingWallet(
            Guid userId, decimal amount, Guid bookingId, Guid? drivingSessionId = null)
        {
            try
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == userId && !w.IsDelete);
                
                if (wallet == null)
                {
                    return false;
                }

                wallet.Balance -= amount;
                wallet.UpdatedAt = DateTime.Now;

                var transaction = new Transaction
                {
                    FromWalletId = wallet.Id,
                    ToWalletId = null, 
                    TransactionValue = amount,
                    PaymentMethod = Domain.Enum.PaymentMethod.Wallet,
                    Status = Domain.Enum.PaymentStatus.Processing,
                    BookingId = bookingId,
                    DrivingSessionId = drivingSessionId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckAndDeductBookingWallet: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CheckAndDeducSessiontWallet(Guid userId, decimal amount, Guid bookingId, Guid? drivingSessionId = null)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == userId && !w.IsDelete);
            
            if (wallet == null)
            {
                return false;
            }

            wallet.Balance -= amount;
            wallet.UpdatedAt = DateTime.UtcNow;

            var transaction = new Transaction
            {
                FromWalletId = wallet.Id,
                ToWalletId = null,
                TransactionValue = amount,
                PaymentMethod = Domain.Enum.PaymentMethod.Wallet,
                Status = Domain.Enum.PaymentStatus.Completed,
                BookingId = bookingId,
                DrivingSessionId = drivingSessionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return true;
        }
    }
    
}
