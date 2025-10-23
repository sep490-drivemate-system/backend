using MediatR;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetTransactionById
{
    public class GetTransactionByIdQuery : IRequest<Result<Transaction>>
    {
        public Guid TransactionId { get; set; }

        public GetTransactionByIdQuery(Guid transactionId)
        {
            TransactionId = transactionId;
        }

        public GetTransactionByIdQuery() { }
    }
}
