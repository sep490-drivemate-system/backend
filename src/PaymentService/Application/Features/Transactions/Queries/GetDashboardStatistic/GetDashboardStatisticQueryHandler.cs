using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetDashboardStatistic
{
    public class GetDashboardStatisticQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDashboardStatisticQuery, Result<PaymentStatisticDTO>>
    {
        private IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<PaymentStatisticDTO>> Handle(GetDashboardStatisticQuery request, CancellationToken cancellationToken)
        {
            Func<Transaction,bool> queryFilter;
            IEnumerable<Transaction> filteredTransactions;
            
            List<Transaction> transactionList = await _unitOfWork.TransactionRepository.GetAllAsync();
            
            switch (request.Type)
            {
                case StatisticTimeType.Yearly:
                    queryFilter = x => x.CreatedAt.Year == request.Year;
                    break;
                case StatisticTimeType.Monthly:
                    queryFilter = x => x.CreatedAt.Year == request.Year && x.CreatedAt.Month == request.Month;
                    break;
                case StatisticTimeType.Weekly:
                    queryFilter = x => x.CreatedAt.Year == request.Year && x.CreatedAt.Month == request.Month && ((x.CreatedAt.Day - 1) / 7) + 1 == request.Week;
                    break;
                default:
                    return Result<PaymentStatisticDTO>.Failure(ServiceError.InvalidStateError($"{request.Type}"));
            }

            filteredTransactions = transactionList.Where(queryFilter);

            PaymentStatisticDTO paymentStatistic = new PaymentStatisticDTO();

            paymentStatistic.TotalPaymentForInstructor = 0; // How to get this data ?
            paymentStatistic.TotalEarning = filteredTransactions.Where(x => !x.IsDelete && x.Status == Domain.Enum.PaymentStatus.Completed).Sum(x => x.TransactionValue);
            paymentStatistic.TotalProfit = paymentStatistic.TotalEarning * 0.15m; // Is this the right formula ?
            paymentStatistic.TotalHolding = filteredTransactions.Where(x => x.Status == PaymentStatus.Processing).Sum(x => x.TransactionValue); // Is this right ?

            // Need another way to put 2 switch clause into one
            switch (request.Type)
            {
                case StatisticTimeType.Yearly:
                    paymentStatistic.EarningByDay = filteredTransactions.GroupBy(x => x.CreatedAt.Month.ToString()).ToDictionary(u => u.Key, u => u.Sum(u => u.TransactionValue));
                    paymentStatistic.ProfitByDay = filteredTransactions.GroupBy(x => x.CreatedAt.Month.ToString()).ToDictionary(u => u.Key, u => u.Sum(u => u.TransactionValue));
                    break;
                case StatisticTimeType.Monthly:
                    paymentStatistic.EarningByDay = filteredTransactions.GroupBy(x => x.CreatedAt.Day.ToString()).ToDictionary(u => u.Key, u => u.Sum(u => u.TransactionValue));
                    paymentStatistic.ProfitByDay = filteredTransactions.GroupBy(x => x.CreatedAt.Day.ToString()).ToDictionary(u => u.Key, u => u.Sum(u => u.TransactionValue));
                    break;
                case StatisticTimeType.Weekly:
                    paymentStatistic.EarningByDay = filteredTransactions.GroupBy(x => x.CreatedAt.DayOfWeek.ToString()).ToDictionary(u => u.Key, u => u.Sum(u => u.TransactionValue));
                    paymentStatistic.ProfitByDay = filteredTransactions.GroupBy(x => x.CreatedAt.DayOfWeek.ToString()).ToDictionary(u => u.Key, u => u.Sum(u => u.TransactionValue));
                    break;
                default:
                    return Result<PaymentStatisticDTO>.Failure(ServiceError.InvalidStateError($"{request.Type}"));
            }

            return Result<PaymentStatisticDTO>.Success(paymentStatistic);
        }
    }
}
