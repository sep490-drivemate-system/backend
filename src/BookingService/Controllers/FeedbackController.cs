using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;

namespace BookingService.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackUseCase _useCase;

        public FeedbackController(IFeedbackUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("list-overview-instructor")]
        public async Task<ActionResult<FeedbackResponse>> GetStatisticFeedback([FromBody] Guid instructorId)
        {
            var result = await _useCase.GetStatitic(instructorId);
            return Ok(result);
        }

        [HttpPost("batch-statistics")]
        public async Task<ActionResult<Dictionary<Guid, InstructorOverviewFeedbackResponse>>> GetBatchStatistics([FromBody] List<Guid> instructorIds)
        {

            var result = await _useCase.GetBatchStatistics(instructorIds);
            return Ok(result);
        }

        //[HttpGet("instructor/{id}")]
        //public async Task<IActionResult> GetInstructorFeedback(Guid id)
        //{
        //    var result = await _useCase.GetInstructorFeedback(id);
        //    return Ok(result);
        //}

        //[HttpGet("instructor/{id}")]
        //public async Task<IActionResult> GetInstructorFeedback(Guid id)
        //{
        //    var result = await _useCase.GetInstructorFeedback(request);
        //    return Ok(result);
        //}
    }
}
