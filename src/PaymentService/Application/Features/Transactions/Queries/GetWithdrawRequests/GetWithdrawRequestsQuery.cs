using MediatR;
using PaymentService.Application.Common.DTOs;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetWithdrawRequests
{
    public class GetWithdrawRequestsQuery : IRequest<Result<PaginatedList<WithdrawRequestDTO>>>
    {
        public WithdrawRequestFilterDTO Filter { get; set; } = new();
    }

    public class WithdrawRequestFilterDTO : PaginationFilter
    {
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? DateString { get; set; } 
    }
}
