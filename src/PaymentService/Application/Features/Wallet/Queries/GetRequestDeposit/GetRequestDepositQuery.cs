using MediatR;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Text.Json.Serialization;

namespace PaymentService.Application.Features.Wallet.Queries.GetRequestDeposit
{
    public class GetRequestDepositQuery : IRequest<Result<string>>
    {
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ClientPlatform Platform { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
