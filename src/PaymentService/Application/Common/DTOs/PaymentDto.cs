using PaymentService.Domain.Enum;

namespace PaymentService.Application.Common.DTOs
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public decimal TransactionValue { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid FromWalletId { get; set; }
        public Guid? ToWalletId { get; set; }
    }

    public class CreatePaymentDto
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Description { get; set; }
    }

    public class UpdatePaymentStatusDto
    {
        public Guid PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentGatewayResponse { get; set; }
        public string? FailureReason { get; set; }
    }
}
