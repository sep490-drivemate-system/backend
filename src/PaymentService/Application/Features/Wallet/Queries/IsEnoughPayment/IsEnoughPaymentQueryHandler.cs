using MediatR;
using PaymentService.Application.Interfaces;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment
{
    public class IsEnoughPaymentQueryHandler : IRequestHandler<IsEnoughPaymentQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IsEnoughPaymentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(IsEnoughPaymentQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.WalletRepository.CheckAndDeductWallet(
                request.UserId, 
                request.Amount, 
                request.BookingId,
                request.DrivingSessionId
            );
        }
    }
}
