namespace UserService.Application.Commons.DTOs.Instructors
{

    public class InstructorFeedbackDTO
    {
        public string Username { get; set; }

        public string AvatarUrl { get; set; }

        public int Score { get; set; }

        public string Comment { get; set; }

        public DateTime FeedbackDate { get; set; }
    }

    public class InstructorPackageDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }

    public class InstructorDTO
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Avatar {  get; set; }

        public decimal UnitPrice { get; set; }

        public int ExperienceYear { get; set; }

        public int BookingCount { get; set; }

        public decimal AverageRating { get; set; }
    }

   
}
