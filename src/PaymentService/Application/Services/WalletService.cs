using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace PaymentService.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WalletService> _logger;

        public WalletService(IUnitOfWork unitOfWork, ILogger<WalletService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Wallet?> GetWalletByUserIdAsync(Guid userId)
        {
            try
            {
                return await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wallet for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Wallet> CreateWalletAsync(Guid userId)
        {
            try
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

                _logger.LogInformation("Created new wallet for user {UserId} with ID {WalletId}", userId, createdWallet.Id);
                
                return createdWallet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating wallet for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Wallet> UpdateWalletBalanceAsync(Guid userId, decimal amount)
        {
            try
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

                _logger.LogInformation("Updated wallet balance for user {UserId}. New balance: {Balance}", userId, wallet.Balance);
                
                return wallet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating wallet balance for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> HasSufficientBalanceAsync(Guid userId, decimal amount)
        {
            try
            {
                var wallet = await GetWalletByUserIdAsync(userId);
                return wallet != null && wallet.Balance >= amount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking sufficient balance for user {UserId}", userId);
                return false;
            }
        }
    }
}
