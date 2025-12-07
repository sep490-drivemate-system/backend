using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.CreateGenericTransaction
{
    public class CreateGenericTransactionCommandHandler : IRequestHandler<CreateGenericTransactionCommand, Result<Guid>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateGenericTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateGenericTransactionCommand request, CancellationToken cancellationToken)
        {
            // Getting the source wallet
            var sourceWallet = await unitOfWork.WalletRepository.GetByIdAsync(request.FromWalletId);

            if (sourceWallet == null)
            {
                return Result<Guid>.Failure(ServiceError.NotFoundError($"{request.FromWalletId}"), "Wallet not found");
            }

            // Getting the destination wallet
            Domain.Entities.Wallet? destinationWallet = null;

            if (request.TransactionInfo.ToWallet != null)
            {
                destinationWallet = await unitOfWork.WalletRepository.GetByIdAsync(request.TransactionInfo.ToWallet);
            }
            
            if (request.TransactionInfo.UpdateBalance && destinationWallet == null)
            {
                return Result<>
            }

            // Preparing the transaction
            Transaction newTransaction = new Transaction
            {
                FromWalletId = request.FromWalletId,
                ToWalletId = request.TransactionInfo.ToWallet,
            }
        }
    }
}
