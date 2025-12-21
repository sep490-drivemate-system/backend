using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<Transaction>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Transaction>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            //// Check if payment already exists for this booking
            //var existingPayment = await _unitOfWork.TransactionRepository
            //    .ExistsByBookingIdAndStatusAsync(request.BookingId, PaymentStatus.Pending) ||
            //    await _unitOfWork.TransactionRepository
            //    .ExistsByBookingIdAndStatusAsync(request.BookingId, PaymentStatus.Completed);


            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                BookingId = request.BookingId,
                TransactionValue = request.Amount,
                PaymentMethod = request.PaymentMethod,
                Status = PaymentStatus.Pending,
                ReferenceCode = request.ReferenceCode ?? Guid.NewGuid().ToString(),
                FromWalletId = request.FromWalletId == Guid.Empty ? null : request.FromWalletId,
                ToWalletId = request.ToWalletId == Guid.Empty ? null : request.ToWalletId,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            var createdTransaction = await _unitOfWork.TransactionRepository.CreateAsync(transaction);
            return Result<Transaction>.Success(createdTransaction);
        }
    }
}
