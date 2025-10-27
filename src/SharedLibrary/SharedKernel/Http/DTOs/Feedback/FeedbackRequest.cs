using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.DTOs.Feedback
{
    public class FeedbackRequest
    {
        public List<Guid> ListInstructor { get; set; }
    }
}
