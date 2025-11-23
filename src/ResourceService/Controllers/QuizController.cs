using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Services.DTOs.Quizzes;
using Services;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;

namespace ResourceService.Controllers
{
    [Route("api/quizs")]
    [ApiController]
    public class QuizController(IServiceProviders serviceProviders): ControllerBase
    {
        private readonly IServiceProviders _services = serviceProviders;

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
    }
}
