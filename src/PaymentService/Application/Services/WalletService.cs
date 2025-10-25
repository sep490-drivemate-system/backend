using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace PaymentService.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Wallet?> GetWalletByUserIdAsync(Guid userId)
        {

            return await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);

        }

        public async Task<Wallet> CreateWalletAsync(Guid userId)
        {

            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsDelete = false
            };

            var createdWallet = await _unitOfWork.WalletRepository.CreateAsync(wallet);
            await _unitOfWork.SaveChangesAsync();


            return createdWallet;

        }

        public async Task<Wallet> UpdateWalletBalanceAsync(Guid userId, decimal amount)
        {

            var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
            {
                throw new InvalidOperationException($"Wallet not found for user {userId}");
            }

            wallet.Balance += amount;
            wallet.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.WalletRepository.UpdateAsync(wallet);
            await _unitOfWork.SaveChangesAsync();


            return wallet;

        }

        public async Task<bool> HasSufficientBalanceAsync(Guid userId, decimal amount)
        {

            var wallet = await GetWalletByUserIdAsync(userId);
            return wallet != null && wallet.Balance >= amount;

        }
    }
}
