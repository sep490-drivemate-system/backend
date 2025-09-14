using BookingService.Domain.Enum;

namespace BookingService.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public TransactionStatus Status { get; set; }
        public string TransactionCode { get; set; } = string.Empty;
        public string PaymentGatewayResponse { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Booking Booking { get; set; } = null!;
    }
}
