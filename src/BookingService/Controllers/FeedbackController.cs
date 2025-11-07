using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Application.Interfaces;
using SharedLibrary.SharedKernel.ServiceResult;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("instructor/{id}")]
        public async Task<IActionResult> GetInstructorFeedback(Guid id)
        {
            var result = await _useCase.GetInstructorFeedback(id);
            return Ok(result);
        }

        //[HttpGet("instructor/{id}")]
        //public async Task<IActionResult> GetInstructorFeedback(Guid id)
        //{
        //    var result = await _useCase.GetInstructorFeedback(request);
        //    return Ok(result);
        //}
    }
}
