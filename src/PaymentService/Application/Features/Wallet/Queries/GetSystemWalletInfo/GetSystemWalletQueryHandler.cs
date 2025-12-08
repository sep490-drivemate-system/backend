using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Application.Interfaces;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Text.Json;

namespace PaymentService.Application.Features.Wallet.Queries.GetSystemWalletInfo
{
    public class GetSystemWalletQueryHandler : IRequestHandler<GetSystemWalletQuery, Result<WalletDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;

        public GetSystemWalletQueryHandler(IUnitOfWork uof, IHttpClientFactory hcf)
        {
            unitOfWork = uof;
            httpClientFactory = hcf;
        }

        public async Task<Result<WalletDto>> Handle(GetSystemWalletQuery request, CancellationToken cancellationToken)
        {
            // Call to User service to get data about admin

            try
            {
                var client = httpClientFactory.CreateClient("UserServiceClient");
                var responseMessage = await client.GetAsync($"api/users?Role={UserRole.Admin}");

                // Reading the message body to get the result
                var response = await responseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<PaginatedList<UserDetailDTO>>>();
                var admin_id = response?.Value?.PageContent.FirstOrDefault()?.UserId;

                if (admin_id == null)
                {
                    return Result<WalletDto>.Failure(ServiceError.ExternalServiceError("Admin account has not been set up"), "Can't find admin account");
                }

                var wallet = await unitOfWork.WalletRepository.GetByIdAsync(admin_id);

                if (wallet == null)
                {
                    return Result<WalletDto>.Failure(ServiceError.BadRequestError("Admin wallet has not been set up"), "Can't find admin wallet");
                }

                return Result<WalletDto>.Success(new WalletDto
                {
                    Id = wallet.Id,
                    Balance = wallet.Balance,
                    CreatedAt = wallet.CreatedAt,
                    UpdatedAt = wallet.UpdatedAt,
                });
            }
            catch (HttpRequestException ex)
            {
                return Result<WalletDto>.Failure(ServiceError.ExternalServiceError($"{ex.Message}"), "Can't call to external service");
            }
        }
    }
}
