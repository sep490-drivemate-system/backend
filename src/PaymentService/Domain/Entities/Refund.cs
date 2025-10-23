using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace PaymentService.Domain.Entities
{
    public class Refund : BaseEntites
    {
        // Properties
        public Guid PaymentId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public RefundStatus Status { get; set; }
        public string? RefundTransactionId { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ProcessedBy { get; set; }
        public bool IsDelete { get; set; }

        // Navigation relationship
        public virtual Transaction Payment { get; set; } = new Transaction();
    }
}
