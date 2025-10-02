using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class SessionLog: BaseEntites
    {
        // Properties
        public Guid SessionId { get; set; }

        public string StreetName { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Heading { get; set; }

        public decimal Speed { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational properties
        public DrivingSession DrivingSessions { get; set; }
    }
}
