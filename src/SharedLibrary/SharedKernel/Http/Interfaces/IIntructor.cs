using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Interfaces
{
    public interface IIntructor
    {
        Task<Dictionary<Guid, InstructorOverviewFeedbackResponse>> GetBatchInstructorOverviewFeedback(List<Guid> instructorIds);
    }
}
