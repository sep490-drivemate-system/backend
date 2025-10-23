using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Transactions.Queries.GetTransactionsByBookingId
{
    public class GetTransactionsByBookingIdQueryHandler : IRequestHandler<GetTransactionsByBookingIdQuery, Result<List<Transaction>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionsByBookingIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Transaction>>> Handle(GetTransactionsByBookingIdQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetByBookingIdAsync(request.BookingId);
            return Result<List<Transaction>>.Success(transactions);
        }
    }
}
