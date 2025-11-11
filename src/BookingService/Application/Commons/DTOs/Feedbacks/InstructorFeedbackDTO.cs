namespace BookingService.Application.Commons.DTOs.Feedbacks
{
    public class InstructorFeedbackDTO
    {
        public string Name { get; set; }
        public string Avatar {  get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

    }
}
