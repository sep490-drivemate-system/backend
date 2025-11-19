using MediatR;
using PaymentService.Application.Common.DTOs;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetInstructorDashboardStatistic
{
    public class GetInstructorStatisticQuery: IRequest<Result<InstructorTransactionDashboardDTO>>
    {
        public Guid UserId { get; set; }
    }
}
