using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace PaymentService.Application.Features.Transactions.Queries.GetInstructorDashboardStatistic
{
    public class GetInstructorStatisticQueryHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory) : IRequestHandler<GetInstructorStatisticQuery, Result<InstructorTransactionDashboardDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<Result<InstructorTransactionDashboardDTO>> Handle(GetInstructorStatisticQuery request, CancellationToken cancellationToken)
        {
            // Check for user identity

            var userServiceClient = _httpClientFactory.CreateClient("UserServiceClient");

            var userServiceCallResult = await userServiceClient.PostAsJsonAsync("api/users/ids", new Guid[] { request.UserId });

            if (!userServiceCallResult.IsSuccessStatusCode)
            {
                return Result<InstructorTransactionDashboardDTO>.Failure(ServiceError.ServiceUnavailableError($"User Service"), "User service failure");
            }

            var users = await userServiceCallResult.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

            if (users.Value.Count() == 0 || users.Value.First().Role != UserRole.Instructor)
            {
                return Result<InstructorTransactionDashboardDTO>.Failure(ServiceError.BadRequestError($"{request.UserId}"), "Not Found");
            }

            var instructorWallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(request.UserId);

            Expression<Func<Transaction, bool>> transaction_filter = x => !x.IsDelete && x.ToWalletId == instructorWallet.Id;
            IEnumerable<Transaction> instructor_transaction = await _unitOfWork.TransactionRepository.GetAllAsync(filter: transaction_filter, null);

            return Result<InstructorTransactionDashboardDTO>.Success(new InstructorTransactionDashboardDTO
            {
                TotalRevenue = instructor_transaction.Where(x => x.Status == Domain.Enum.PaymentStatus.Completed).Sum(x => x.TransactionValue),
                RevenueAfterDeduction = instructor_transaction.Where(x => x.Status == Domain.Enum.PaymentStatus.Completed).Sum(x => x.TransactionValue) * 0.15m,
                TotalDeduction = instructor_transaction.Where(x => x.Status == Domain.Enum.PaymentStatus.Completed).Sum(x => x.TransactionValue) * 0.75m,
                TotalFundsWidthdrawled = 0 // Does not available for current state.
            });
        }
    }
}