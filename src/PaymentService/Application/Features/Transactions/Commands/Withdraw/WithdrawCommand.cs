using MediatR;
using PaymentService.Application.Common.DTOs;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.Withdraw
{
    public class WithdrawCommand : IRequest<Result<string?>>
    {
        public WithdrawStatus Status { get; set; }
        public Guid TransactionId { get; set; }
        public string TransactionNote { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public PaymentMethod  PaymentMethod { get; set; }
    }

    public enum WithdrawStatus
    {
        Approved,
        Rejected,
    }

}
