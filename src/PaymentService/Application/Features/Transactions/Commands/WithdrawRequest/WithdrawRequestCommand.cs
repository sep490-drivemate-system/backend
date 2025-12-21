using MediatR;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Text.Json.Serialization;

namespace PaymentService.Application.Features.Transactions.Commands.WithdrawRequest
{
    public class WithdrawRequestCommand : IRequest<Result<bool>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionNote { get; set; }
    }
}
