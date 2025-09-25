using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class DayOfWeek : BaseEntites
    {
        public string Name { get; set; } = string.Empty;
        public int DayNumber { get; set; }     
    }
}
