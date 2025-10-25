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

        public async Task<bool> CheckWallet(Guid walletId, decimal amount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == walletId);

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    Id = walletId,
                    Balance = 0, 
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                return false; 
            }

            if (wallet.Balance >= amount)
                return true;

            return false;
        }

    }
}
