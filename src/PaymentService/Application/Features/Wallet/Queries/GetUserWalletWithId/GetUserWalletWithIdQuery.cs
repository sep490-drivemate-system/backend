using MediatR;
using PaymentService.Application.Common.DTOs;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.GetUserWalletWithId
{
    public class GetUserWalletWithIdQuery: IRequest<Result<WalletDto>>
    {
        public Guid UserId { get; set; }
    }
}
