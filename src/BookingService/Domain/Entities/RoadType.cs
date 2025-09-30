using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class RoadType : BaseEntites
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}
