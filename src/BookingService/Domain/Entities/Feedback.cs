using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Feedback: BaseEntites
    {
        // Properties
        public Guid UserId { get; set; }

        public Guid BookingId { get; set; }

        public int InstructorRating { get; set; }

        public string InstructorFeedback { get; set; }

        public int CarRating { get; set; }

        public string CarFeedback { get; set; }

        // Systematic properties
        
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational properties
        public Booking Bookings { get; set; }
    }
}
