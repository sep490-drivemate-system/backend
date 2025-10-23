using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.UpdateTransactionStatus
{
    public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand, Result<Transaction>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTransactionStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Transaction>> Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(request.TransactionId);
            
            

            transaction.Status = request.Status;
            transaction.ReferenceCode = request.ReferenceCode ?? transaction.ReferenceCode;
            transaction.UpdatedAt = DateTime.UtcNow;

            var updatedTransaction = await _unitOfWork.TransactionRepository.Update(transaction);
            return Result<Transaction>.Success(updatedTransaction);
        }
    }
}
