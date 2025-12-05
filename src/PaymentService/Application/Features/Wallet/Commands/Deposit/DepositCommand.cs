using MediatR;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Collections;

namespace PaymentService.Application.Features.Wallet.Commands.Deposit
{
    public class DepositCommand : IRequest<Result<decimal?>>
    {
        public IQueryCollection Data { get; set; }
    }
}
