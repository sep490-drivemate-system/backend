using Microsoft.AspNetCore.Mvc;
using ResourceService.Application.Commons.DTOs.QA;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Domain.Enums;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using Sprache;


namespace ResourceService.Controllers
{
    [Route("api/qa")]
    [ApiController]
    public class QAController(IQAService qAService,IJwtService jwtService) : ControllerBase
    {
        private readonly IQAService _qaService =  qAService;
        private readonly IJwtService _jwtService =  jwtService;

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionDTO createQuestionDTO)
        {
            var authorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _qaService.CreateQuestion(createQuestionDTO,authorId);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions([FromQuery] FilterQADTO filterQADTO)
        {
            var result = await _qaService.GetQuestions(filterQADTO);
            return result.ToActionResult();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] QaStatus status)
        {
            var result = await _qaService.UpdateQuestionStatus(id, status);
            return result.ToActionResult();
        }

        [HttpPost("answers")]
        public async Task<IActionResult> AnswerQuestion([FromBody] AnswerQuestionDTO answerQuestionDTO)
        {
            var authorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _qaService.AnswerQuestion(answerQuestionDTO, authorId);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var result = await _qaService.DeleteQuestion(id);
            return result.ToActionResult();
        }
    }
}
