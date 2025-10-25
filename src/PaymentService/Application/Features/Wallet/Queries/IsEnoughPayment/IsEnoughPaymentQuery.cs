using MediatR;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment
{
    public class IsEnoughPaymentQuery : IRequest<PaymentResponse>
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
