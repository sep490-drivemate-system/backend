using MediatR;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.UpdateTransactionStatus
{
    public class UpdateTransactionStatusCommand : IRequest<Result<Transaction>>
    {
        public Guid TransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public string? ReferenceCode { get; set; }
    }
}
