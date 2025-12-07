using MediatR;
using PaymentService.Application.Common.DTOs.Transaction;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetUserTransactions
{
    public class GetUserTransactionQuery: IRequest<Result<PaginatedList<PersonalTransactionViewDTO>>>
    {
        public Guid UserId { get; set; } // Wallet id
        public TransactionFilter Filter { get; set; }
    }

    public class TransactionFilter
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? DateString { get; set; } = null;
        public decimal? Amount { get; set; } = 0; // Default value = 0; 
        public PaymentStatus? Status { get; set; } = 0; // get all transaction
    }
}
