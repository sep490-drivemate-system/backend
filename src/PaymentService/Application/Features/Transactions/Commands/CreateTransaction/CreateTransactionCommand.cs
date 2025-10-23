using MediatR;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<Result<Transaction>>
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? ReferenceCode { get; set; }
        public Guid FromWalletId { get; set; }
        public Guid? ToWalletId { get; set; }
    }
}
