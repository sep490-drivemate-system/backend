using System;

namespace SharedLibrary.SharedKernel.Http.DTOs.Feedback
{
    public class InstructorStatisticDto
    {
        public Guid InstructorId { get; set; }
        
        public int BookingCount { get; set; }

        public decimal AverageRating { get; set; }
        public decimal PricePerHours { get; set; }
        
    }
}
