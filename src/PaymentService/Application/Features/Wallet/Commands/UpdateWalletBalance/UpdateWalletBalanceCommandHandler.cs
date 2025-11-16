using MediatR;
using PaymentService.Application.Interfaces;
using SharedLibrary.SharedKernel.Http.DTOs.Wallet;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance
{
    public class UpdateWalletBalanceCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<UpdateWalletBalanceCommand, Result<WalletBalanceDTO>>
    {
        private IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<WalletBalanceDTO>> Handle(UpdateWalletBalanceCommand request, CancellationToken cancellationToken)
        {
            var userWallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(request.UserId);

            if (userWallet == null)
            {
                return Result<WalletBalanceDTO>.Failure(ServiceError.NotFoundError($"{request.UserId}"), "Failure");
            }

            userWallet.Balance += request.BalanceAmount;

            await _unitOfWork.WalletRepository.UpdateAsync(userWallet);
            await _unitOfWork.SaveChangesAsync();

            return Result<WalletBalanceDTO>.Success(new WalletBalanceDTO
            {
                UserId = request.UserId,
                Balance = userWallet.Balance,
            });
        }
    }
}
