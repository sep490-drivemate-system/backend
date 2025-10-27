using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class Booking: BaseEntites
    {
        // Properties
        public Guid PackageId { get; set; }
        public Guid DriverId { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // System properties

        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational properties
        public List<Feedback> Feedbacks { get; set; }

        public List<TimeRange> TimeRanges { get; set; }

        public List<DrivingSession> DrivingSessions { get; set; }
        public List<DrivingSkill> DrivingSkills{ get; set; }
        public List<RoadType> RoadTypes{ get; set; }
        public Package Package { get; set; }
    }
}
