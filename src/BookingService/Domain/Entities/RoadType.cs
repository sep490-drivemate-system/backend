using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class RoadType: BaseEntites
    {
        // Properties
        public string Name { get; set; }

        // System property

        public string LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational Properties
        public List<SessionRoadType> SessionRoadTypes { get; set; }
    }
}
