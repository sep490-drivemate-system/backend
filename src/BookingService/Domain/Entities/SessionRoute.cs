using SharedLibrary.SharedKernel.Entities;

namespace BookingService.Domain.Entities
{
    public class SessionRoute: BaseEntites
    {
        // Properties
        public Guid SessionId { get; set; }

        public string TextInstruction { get; set; }

        public string StreetName { get; set; }

        public decimal LatitudeStart { get; set; }

        public decimal LongitudeStart { get; set; }

        //public decimal LatitudeEnd { get; set; }

        //public decimal LongtitudeEnd { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigational properties
        public DrivingSession DrivingSessions { get; set; }
    }
}
