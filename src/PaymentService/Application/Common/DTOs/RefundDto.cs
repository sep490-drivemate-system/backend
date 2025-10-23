using PaymentService.Domain.Enum;

namespace PaymentService.Application.Common.DTOs
{
    public class RefundDto
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public RefundStatus Status { get; set; }
        public string? RefundTransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ProcessedBy { get; set; }
    }

    public class CreateRefundDto
    {
        public Guid PaymentId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class ProcessRefundDto
    {
        public Guid RefundId { get; set; }
        public RefundStatus Status { get; set; }
        public string? RefundTransactionId { get; set; }
        public string? ProcessedBy { get; set; }
    }
}
