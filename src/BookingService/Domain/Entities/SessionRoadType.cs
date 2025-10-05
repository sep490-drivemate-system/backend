using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class SessionRoadType: BaseEntites
    {
        // Properties
        public Guid SessionId { get; set; }

        public Guid RoadTypeId { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational properties
        public RoadType RoadTypes { get; set; }

        public DrivingSession DrivingSessions {  get; set; }
    }
}
