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
                return Result<Guid>.Failure(ServiceError.NotFoundError($"{request.FromWalletId}"), "Source wallet not found");
            }

            // Checking enough money on the source wallet
            if (sourceWallet.Balance < request.TransactionInfo.Value)
            {
                return Result<Guid>.Failure(ServiceError.BadRequestError($"{request.TransactionInfo.Value}"), "Source wallet balance not enough");
            }
            

            if (request.TransactionInfo.UpdateBalance)
            {
                // Getting the destination wallet
                Domain.Entities.Wallet? destinationWallet = null;

                // Required an actual wallet to transfer money
                if (request.TransactionInfo.ToWallet == null)
                {
                    return Result<Guid>.Failure(ServiceError.BadRequestError($"Get null but expected a value"), "Missing wallet information");
                }
                
                destinationWallet = await unitOfWork.WalletRepository.GetByIdAsync(request.TransactionInfo.ToWallet);

                if (destinationWallet == null)
                {
                    return Result<Guid>.Failure(ServiceError.NotFoundError($"{request.TransactionInfo.ToWallet}"), "Destination wallet not found");
                }

                sourceWallet.Balance -= request.TransactionInfo.Value;
                destinationWallet.Balance += request.TransactionInfo.Value;
            }

            // Preparing the transaction
            Transaction newTransaction = new Transaction
            {
                FromWalletId = request.FromWalletId,
                ToWalletId = request.TransactionInfo.ToWallet,
                TransactionValue = request.TransactionInfo.Value,
                BookingId = request.TransactionInfo.BookingId,
                DrivingSessionId = request.TransactionInfo.SessionId,
                ReferenceCode = request.TransactionInfo.ReferenceCode,
                TransactionNote = request.TransactionInfo.TransactionNote,
                PaymentMethod = Domain.Enum.PaymentMethod.Wallet,
                Status = Domain.Enum.PaymentStatus.Completed,
            };

            newTransaction = await unitOfWork.TransactionRepository.CreateAsync(newTransaction);
            await unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(newTransaction.Id);
        }
    }
}
