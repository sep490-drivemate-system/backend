using BookingService.Domain.Entities;

namespace BookingService.Application.Commons.DTOs.Feedbacks
{
    public class FeedbackCreationDTO
    {
        // Properties
        public int InstructorRating { get; set; }
        public string InstructorFeedback { get; set; }
        public int? CarRating { get; set; }
        public string? CarFeedback { get; set; }
        public Guid? CarId { get; set; }
        public Guid BookingId { get; set; }
        public Guid InstructorId { get; set; }
    }
}
