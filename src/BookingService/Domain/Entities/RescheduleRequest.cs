using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class RescheduleRequest: BaseEntites
    {
        // Properties
        public DateOnly Date {  get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
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
