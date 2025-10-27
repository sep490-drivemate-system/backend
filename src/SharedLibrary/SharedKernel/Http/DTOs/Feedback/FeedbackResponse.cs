using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.DTOs.Feedback
{
    public class FeedbackResponse
    {
        public List<InstructorStatisticDto> InstructorStatistics { get; set; } = new List<InstructorStatisticDto>();
    }

}
