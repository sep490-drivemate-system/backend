using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs.Quizzes;
using Services;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;

namespace ResourceService.Controllers
{
    [Route("api/quizs")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IServiceProviders _services;
        private readonly IJwtService _jwtService;

        public QuizController(IServiceProviders serviceProviders, IJwtService jwtService)
        {
            _services = serviceProviders;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuiz([FromQuery] QuizFilterDTO filterDTO)
        {
            var result = await _services.QuizService.GetAllQuiz(filterDTO.Tag, filterDTO.Duration);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizDetail([FromRoute] Guid id)
        {
            var result = await _services.QuizService.GetQuizDetailWithId(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateOrUpdateDTO quiz)
        {
            var result = await _services.QuizService.CreateQuiz(quiz);
            return result.ToActionResult();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateQuiz([FromRoute] Guid id, [FromBody] QuizCreateOrUpdateDTO quiz)
        {
            var result = await _services.QuizService.UpdateQuiz(id, quiz);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz([FromRoute] Guid id)
        {
            var result = await _services.QuizService.DeleteQuiz(id);
            return result.ToActionResult();
        }

        [HttpPost("{id}/attempts")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> SubmitQuizAttempt([FromRoute] Guid id, [FromBody] QuizAttemptRequestDTO attemptRequest)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _services.QuizService.SubmitQuizAttempt(id, userId, attemptRequest);
            return result.ToActionResult();
        }
    }
}
