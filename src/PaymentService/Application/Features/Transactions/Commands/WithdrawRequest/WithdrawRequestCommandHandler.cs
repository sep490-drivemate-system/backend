using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.WithdrawRequest
{
    public class WithdrawRequestCommandHandler : IRequestHandler<WithdrawRequestCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public WithdrawRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(WithdrawRequestCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                Status = PaymentStatus.Pending,
                FromWalletId = request.UserId,
                TransactionValue = request.Amount,
                TransactionNote = request.TransactionNote,
                ToWalletId = null,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            var createdTransaction = await _unitOfWork.TransactionRepository.CreateAsync(transaction);
            return Result<bool>.Success(true);
        }
    }
}