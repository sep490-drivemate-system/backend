using SharedLibrary.SharedKernel.Entities;

namespace PaymentService.Domain.Entities
{
    public class Wallet : BaseEntites
    {
        // Properties
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDelete { get; set; }

        // Relationship navigation
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
