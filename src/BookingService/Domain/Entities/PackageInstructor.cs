namespace BookingService.Domain.Entities
{
    public class PackageInstructor
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public Guid InstructorId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Package Package { get; set; } = null!;
    }
}
