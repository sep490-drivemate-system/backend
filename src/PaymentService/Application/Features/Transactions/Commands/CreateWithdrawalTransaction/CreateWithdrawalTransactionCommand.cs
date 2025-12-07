using MediatR;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.CreateWithdrawalTransaction
{
    public class CreateWithdrawalTransactionCommand: IRequest<Result<Guid>>
    {
        public Guid UserId { get; set; }
        public WithdrawalRequest Request { get; set; }
    }

    public class WithdrawalRequest 
    { 
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public decimal Value { get; set; }
    }
}
