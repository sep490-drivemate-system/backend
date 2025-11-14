using MediatR;
using SharedLibrary.SharedKernel.Http.DTOs.Wallet;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Commands.UpdateWalletBalance
{
    public class UpdateWalletBalanceCommand: IRequest<Result<WalletBalanceDTO>>
    {
        public Guid UserId { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
