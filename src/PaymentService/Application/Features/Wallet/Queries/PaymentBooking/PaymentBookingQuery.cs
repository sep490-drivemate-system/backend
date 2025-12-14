using MediatR;

namespace PaymentService.Application.Features.Wallet.Queries.IsEnoughPayment
{
    public class PaymentBookingQuery : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public Guid BookingId { get; set; }
        public Guid? DrivingSessionId { get; set; }
    }
}
