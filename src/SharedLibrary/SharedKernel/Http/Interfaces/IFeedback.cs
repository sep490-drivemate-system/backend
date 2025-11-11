using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Interfaces
{
    public interface IFeedback
    {
       public Task<FeedbackResponse> GetStatiticFeedback(List<Guid> listGuidInstructor);
       public Task<NoviceDriverInfoFeedbackDTO> GetNoviceDriverInfor(Guid noviceDriverid);
    }
}
