using MediatR;
using PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment;
using PaymentService.Application.Interfaces;

namespace PaymentService.Application.Features.Wallet.Queries.IsEnoughSessionPayment
{
    public class PaymentSessionQueryHandler : IRequestHandler<PaymentSessionQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentSessionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(PaymentSessionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.WalletRepository.CheckAndDeducSessiontWallet(
                request.UserId,
                request.InstructorId,
                request.Amount,
                request.BookingId,
                request.DrivingSessionId
            );
        }
    }

}