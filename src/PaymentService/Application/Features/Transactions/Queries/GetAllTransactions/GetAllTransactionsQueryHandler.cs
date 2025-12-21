using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Linq.Expressions;

namespace PaymentService.Application.Features.Transactions.Queries.GetAllTransactions
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, Result<PaginatedList<TransactionsDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GetAllTransactionsQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Result<PaginatedList<TransactionsDTO>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Transaction, bool>> filterExpression = x =>
                !x.IsDelete
                && (request.Filter.Status == null || x.Status == request.Filter.Status.Value);

            var transactions = await _unitOfWork.TransactionRepository.GetAllAsync(
                filter: filterExpression,
                orderBy: x => x.OrderByDescending(t => t.CreatedAt)
            );

            var paginatedTransactions = PaginatedList<Transaction>.Create(
                transactions,
                request.Filter.PageNumber,
                request.Filter.PageSize
            );

            var userIds = paginatedTransactions.PageContent
                .Where(x => x.FromWalletId.HasValue || x.ToWalletId.HasValue)
                .SelectMany(x => new[] { x.FromWalletId, x.ToWalletId })
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var users = await GetUsersByIdsAsync(userIds);
            var userDict = users?.ToDictionary(u => u.UserId, u => u) 
                ?? new Dictionary<Guid, UserDetailDTO>();

            var transactionsDTO = paginatedTransactions.PageContent.Select(transaction =>
            {
                var dto = _mapper.Map<TransactionsDTO>(transaction);
                
                if (transaction.FromWalletId.HasValue && userDict.TryGetValue(transaction.FromWalletId.Value, out var userInfo))
                {
                    dto.FullName = userInfo.FullName;
                    dto.Email = userInfo.Email;
                }
                
                return dto;
            }).ToList();

            var result = PaginatedList<TransactionsDTO>.CreateFromPagedData(
                transactionsDTO,
                paginatedTransactions.CurrentPage,
                paginatedTransactions.PageSize,
                paginatedTransactions.TotalCount);

            return Result<PaginatedList<TransactionsDTO>>.Success(result);
        }

        private async Task<List<UserDetailDTO>?> GetUsersByIdsAsync(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return null;
            }

            try
            {
                var userServiceUrl = _configuration["USERSERVICE:URL"];
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

