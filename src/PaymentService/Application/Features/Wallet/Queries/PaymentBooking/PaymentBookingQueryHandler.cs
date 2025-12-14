using MediatR;
using PaymentService.Application.Interfaces;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment
{
    public class PaymentBookingQueryHandler : IRequestHandler<PaymentBookingQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentBookingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(PaymentBookingQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.WalletRepository.CheckAndDeductBookingWallet(
                request.UserId, 
                request.Amount, 
                request.BookingId,
                request.DrivingSessionId
            );
        }
    }
}
