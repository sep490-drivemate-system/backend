using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.GetUserWalletWithId
{
    public class GetUserWalletWithIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserWalletWithIdQuery, Result<WalletDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<WalletDto>> Handle(GetUserWalletWithIdQuery request, CancellationToken cancellationToken)
        {
            var user_wallet = await unitOfWork.WalletRepository.GetByUserIdAsync(request.UserId);

            if (user_wallet == null)
            {
                return Result<WalletDto>.Failure(ServiceError.NotFoundError($"{request.UserId}"), "Failed");
            }

            return Result<WalletDto>.Success(new WalletDto { Id = user_wallet.Id, Balance = user_wallet.Balance });
        }
    }
}
