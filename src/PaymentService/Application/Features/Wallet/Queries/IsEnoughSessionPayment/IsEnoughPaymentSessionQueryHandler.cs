using MediatR;
using PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment;
using PaymentService.Application.Interfaces;

namespace PaymentService.Application.Features.Wallet.Queries.IsEnoughSessionPayment
{
    public class IsEnoughPaymentSessionQueryHandler : IRequestHandler<IsEnoughPaymentSessionQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IsEnoughPaymentSessionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(IsEnoughPaymentSessionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.WalletRepository.CheckAndDeducSessiontWallet(
                request.UserId,
                request.Amount,
                request.BookingId,
                request.DrivingSessionId
            );
        }
    }

}