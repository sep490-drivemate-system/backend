using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class RescheduleRequest: BaseEntites
    {
        // Properties
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note {  get; set; }
        public RequestSide Side { get; set; }

        // Keys for relationships
        public Guid SessionId { get; set; }
        
        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigational properties
        public virtual DrivingSession? DrivingSessions { get; set; }
    }
}
