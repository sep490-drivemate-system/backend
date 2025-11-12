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
                .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDelete);
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

        public async Task<(bool IsSuccess, string Message, decimal CurrentBalance)> CheckAndDeductWallet(
            Guid userId, decimal amount, Guid bookingId, Guid? drivingSessionId = null)
        {
            // Find wallet by UserId
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == userId && !w.IsDelete);

            // If wallet doesn't exist, create new wallet with 0 balance
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    Id = userId,
                    UserId = userId,
                    Balance = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                return (false, $"Số dư không đủ. Số dư hiện tại: {wallet.Balance:N0} VNĐ, cần: {amount:N0} VNĐ", wallet.Balance);
            }

            // Check if balance is sufficient
            if (wallet.Balance < amount)
            {
                return (false, $"Số dư không đủ. Số dư hiện tại: {wallet.Balance:N0} VNĐ, cần: {amount:N0} VNĐ", wallet.Balance);
            }

            // Deduct amount from wallet
            wallet.Balance -= amount;
            wallet.CreatedAt = DateTime.UtcNow;
            wallet.UpdatedAt = DateTime.UtcNow;


            // Create transaction record
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                FromWalletId = wallet.Id,
                ToWalletId = null, 
                TransactionValue = amount,
                PaymentMethod = Domain.Enum.PaymentMethod.Wallet,
                Status = Domain.Enum.PaymentStatus.Completed,
                BookingId = bookingId,
                DrivingSessionId = drivingSessionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                


            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return (true, $"Thanh toán thành công. Số dư còn lại: {wallet.Balance:N0} VNĐ", wallet.Balance);
        }

    }
}
