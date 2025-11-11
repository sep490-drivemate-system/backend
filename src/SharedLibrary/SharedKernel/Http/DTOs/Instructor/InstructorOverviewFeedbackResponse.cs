using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.DTOs.Instructor
{
    public class InstructorOverviewFeedbackResponse
    {
        public decimal AverageRating { get; set; }
        public int BookingCount { get; set; }
        public int PackageCount {  get; set; }
    }
}
