using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Feedback: BaseEntites
    {
        // Properties
        public int InstructorRating { get; set; }
        public string InstructorFeedback { get; set; }
        public int CarRating { get; set; }
        public string CarFeedback { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Keys for relationships
        public Guid CarId { get; set; }
        public Guid BookingId { get; set; }
        public Guid InstructorId { get; set; } // Call User microservice to get data.
        public Guid NoviceDriverId { get; set; } // Call User microservice to get data.

        // Navigational properties
        public virtual Booking? Booking { get; set; }
        public virtual Car? Car { get; set; }
    }
}
