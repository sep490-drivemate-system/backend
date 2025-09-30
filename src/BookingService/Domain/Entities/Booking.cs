using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Booking : BaseEntites
    {
        // Properties
        public Guid UserId { get; set; }
        public Guid InstructorId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }        
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<DrivingSession>? DrivingSessions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual Package Package { get; set; } = null!;
    }
}
