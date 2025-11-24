using BookingService.Application.Commons.DTOs.Feedbacks;
using BookingService.Application.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Implementation;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using SharedLibrary.SharedKernel.ServiceResult;

namespace BookingService.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackUseCase _useCase;
        private readonly IJwtService _jwtService;

        public FeedbackController(IFeedbackUseCase useCase, IJwtService jwtService)
        {
            _useCase = useCase;
            _jwtService = jwtService;
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

        [HttpPost]
        public async Task<IActionResult> CreateFeeback(FeedbackCreationDTO feedbackCreationDTO)
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _useCase.SaveFeedback(feedbackCreationDTO,driverId);
            return result.ToActionResult();
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
