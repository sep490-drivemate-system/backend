using MediatR;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace PaymentService.Application.Features.Transactions.Queries.GetWithdrawRequests
{
    public class GetWithdrawRequestsQueryHandler : IRequestHandler<GetWithdrawRequestsQuery, Result<PaginatedList<WithdrawRequestDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GetWithdrawRequestsQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Result<PaginatedList<WithdrawRequestDTO>>> Handle(GetWithdrawRequestsQuery request, CancellationToken cancellationToken)
        {
            string[]? dateParts = null;
            if (!string.IsNullOrEmpty(request.Filter.DateString))
            {
                dateParts = request.Filter.DateString.Split("/");
            }

            Expression<Func<Transaction, bool>> filterExpression = x =>
                x.ToWalletId == null
                && !x.IsDelete
                && (request.Filter.MinAmount == null || x.TransactionValue >= request.Filter.MinAmount.Value)
                && (request.Filter.MaxAmount == null || x.TransactionValue <= request.Filter.MaxAmount.Value)
                && (dateParts == null || (x.CreatedAt.Month == int.Parse(dateParts[0]) && x.CreatedAt.Year == int.Parse(dateParts[1])));

            // Get all withdraw requests with pagination
            var allWithdrawRequests = await _unitOfWork.TransactionRepository.GetAllAsync(
                filter: filterExpression,
                orderBy: x => x.OrderByDescending(y => y.CreatedAt));

            var paginatedRequests = PaginatedList<Transaction>.Create(
                allWithdrawRequests,
                request.Filter.PageNumber,
                request.Filter.PageSize);

            // Get unique user IDs from withdraw requests
            var userIds = paginatedRequests.PageContent
                .Where(x => x.FromWalletId.HasValue)
                .Select(x => x.FromWalletId.Value)
                .Distinct()
                .ToList();

            // Get user information from UserService
            var users = await GetUsersByIdsAsync(userIds);
            var userDict = users?.ToDictionary(u => u.UserId, u => u) 
                ?? new Dictionary<Guid, UserDetailDTO>();

            // Map to DTOs
            var mappedRequests = paginatedRequests.PageContent.Select(transaction =>
            {
                userDict.TryGetValue(transaction.FromWalletId ?? Guid.Empty, out var userInfo);

                return new WithdrawRequestDTO
                {
                    TransactionValue = transaction.TransactionValue,
                    TransactionNote = transaction.TransactionNote,
                    IsDelete = transaction.IsDelete,
                    FullName = userInfo?.FullName ?? "Unknown",
                    Email = userInfo?.Email ?? "Unknown",
                    RequestTime = transaction.CreatedAt
                };
            }).ToList();

            // Create paginated result
            var result = PaginatedList<WithdrawRequestDTO>.CreateFromPagedData(
                mappedRequests,
                paginatedRequests.CurrentPage,
                paginatedRequests.PageSize,
                paginatedRequests.TotalCount);

            return Result<PaginatedList<WithdrawRequestDTO>>.Success(result);
        }

        private async Task<List<UserDetailDTO>?> GetUsersByIdsAsync(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return null;
            }

            try
            {
                var userServiceUrl = _configuration["USERSERVICE:URL"] ?? "http://localhost:5100";
                var client = _httpClientFactory.CreateClient();
                var url = $"{userServiceUrl}/api/users/ids";

                var response = await client.PostAsJsonAsync(url, userIds);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<DefaultApiResponse<List<UserDetailDTO>>>();
                return result?.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return null;
            }
        }
    }
}
