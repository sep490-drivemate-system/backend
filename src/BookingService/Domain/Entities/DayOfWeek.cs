namespace BookingService.Domain.Entities
{
    public class DayOfWeek
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DayNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
