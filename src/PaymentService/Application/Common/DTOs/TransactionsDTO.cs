using PaymentService.Domain.Enum;

namespace PaymentService.Application.Common.DTOs
{
    public class TransactionsDTO
    {
        public Guid BookingId { get; set; }
        public Guid? DrivingSessionId { get; set; }
        public decimal TransactionValue { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionNote { get; set; } = string.Empty;
        public string? ReferenceCode { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public Guid? ToWalletId { get; set; }
        public Guid? FromWalletId { get; set; }
        public bool IsDelete { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
