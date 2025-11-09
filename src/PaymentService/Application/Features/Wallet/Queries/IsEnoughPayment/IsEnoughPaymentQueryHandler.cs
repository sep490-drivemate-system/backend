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
            bool isEnough = await _unitOfWork.WalletRepository.CheckWallet(request.UserId, request.Amount,request.BookingId);


            return new PaymentResponse
            {
                IsPayment = isEnough,
            };
        }
    }
}
