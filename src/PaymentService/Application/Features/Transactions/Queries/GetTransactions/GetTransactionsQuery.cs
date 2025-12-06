using MediatR;
using PaymentService.Application.Common.DTOs;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionsQuery : IRequest<Result<List<TransactionsDTO>>>
    {
        public Guid WalletId { get; set; }
    }
}
