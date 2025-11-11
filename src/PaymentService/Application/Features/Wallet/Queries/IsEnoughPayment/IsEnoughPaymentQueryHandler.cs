using MediatR;
using PaymentService.Application.Interfaces;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.ServiceResult;

namespace PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment
{
    public class IsEnoughPaymentQueryHandler : IRequestHandler<IsEnoughPaymentQuery, PaymentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IsEnoughPaymentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentResponse> Handle(IsEnoughPaymentQuery request, CancellationToken cancellationToken)
        {
            var (isSuccess, message, currentBalance) = await _unitOfWork.WalletRepository.CheckAndDeductWallet(
                request.UserId, 
                request.Amount, 
                request.BookingId,
                request.DrivingSessionId
            );

            return new PaymentResponse
            {
                IsPayment = isSuccess,
                Message = message,
                CurrentBalance = currentBalance
            };
        }
    }
}
