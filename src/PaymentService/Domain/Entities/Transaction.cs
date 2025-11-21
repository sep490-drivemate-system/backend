using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Domain.Entities
{
    public class Transaction : BaseEntites
    {
        // Properties 
        public Guid BookingId { get; set; }
        public Guid? DrivingSessionId { get; set; }
        public decimal TransactionValue { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string? ReferenceCode { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public Guid? ToWalletId { get; set; }
        public bool IsDelete { get; set; }
        // Key for navigation 
        public Guid FromWalletId {  get; set; }

        // Navigation relationship
        public virtual Wallet? Wallet {  get; set; }

    }


}
