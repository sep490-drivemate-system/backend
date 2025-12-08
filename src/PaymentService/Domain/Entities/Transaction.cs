using PaymentService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Domain.Entities
{
    public class Transaction : BaseEntites
    {
        // Properties
        public decimal TransactionValue { get; set; }
        public string? ReferenceCode { get; set; } = string.Empty;
        public string? TransactionNote { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PaymentStatus Status { get; set; }
        public bool IsDelete { get; set; }
        
        // Foreign Keys
        public Guid? ToWalletId { get; set; }
        public Guid? FromWalletId {  get; set; }

        // System specific entities
        public Guid? BookingId { get; set; }
        public Guid? DrivingSessionId { get; set; }

        // Navigation relationship
        public virtual Wallet? Wallet {  get; set; } // The source wallet navigational property

    }


}
