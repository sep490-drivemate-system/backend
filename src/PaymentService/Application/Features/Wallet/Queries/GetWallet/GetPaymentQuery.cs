using MediatR;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.GetWallet
{
    public class GetPaymentQuery : IRequest<Result<decimal>>
    {
        public Guid WalletId { get; set; }
    }
}
