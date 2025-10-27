using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.DTOs.Feedback
{
    public class FeedbackResponse
    {
        public Guid Id { get; set; }

        public int BookingCount { get; set; }

        public decimal AverageRating { get; set; }

        public decimal UnitPrice { get; set; }
    }

}
