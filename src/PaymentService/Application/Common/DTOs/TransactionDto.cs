using PaymentService.Domain.Enum;

namespace PaymentService.Application.Common.DTOs
{
    public class CreateTransactionDto
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? ReferenceCode { get; set; }
        public Guid FromWalletId { get; set; }
        public Guid? ToWalletId { get; set; }
    }

    public class UpdateTransactionStatusDto
    {
        public PaymentStatus Status { get; set; }
        public string? ReferenceCode { get; set; }
    }
}
