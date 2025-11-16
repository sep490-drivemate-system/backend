using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Common.DTOs;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetDashboardStatistic
{
    public class GetDashboardStatisticQuery: IRequest<Result<PaymentStatisticDTO>>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public StatisticTimeType Type { get; set; }
    }
}
