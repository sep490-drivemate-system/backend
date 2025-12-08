using MediatR;
using PaymentService.Application.Common.DTOs;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.GetSystemWalletInfo
{
    public class GetSystemWalletQuery: IRequest<Result<WalletDto>>
    {
    }
}
