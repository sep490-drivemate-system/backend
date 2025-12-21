using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetAllTransactions
{
    public class GetAllTransactionsQuery : IRequest<Result<PaginatedList<TransactionsDTO>>>
    {
        public GetAllTransactionsFilter Filter { get; set; } = new();
    }

    public class GetAllTransactionsFilter : PaginationFilter
    {
        public PaymentStatus? Status { get; set; } = null; 
    }
}

