using MediatR;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.CreateGenericTransaction
{
    public class CreateGenericTransactionCommand: IRequest<Result<Guid>>
    {
        public Guid FromWalletId { get; set; }
        public GenericTransactionInfo TransactionInfo { get; set; }
    }

    public class GenericTransactionInfo
    {
        public Guid? ToWallet { get; set; }
        public decimal Value { get; set; }
        public string? ReferenceCode { get; set; }
        public string? TransactionNote { get; set; } // Human readable note.
        public bool UpdateBalance { get; set; } // Actually update balance or just creating a transaction
    }
}
