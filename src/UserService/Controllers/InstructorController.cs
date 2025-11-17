using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.AI.VnptEkyc;
using SharedLibrary.SharedKernel.ServiceResult;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Commons.DTOs.Instructors.Registration;
using UserService.Application.Interfaces;
using UserService.Domain.Enum;

namespace UserService.Controllers
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController(IInstructorUseCase usecase, IVnptEkycService ekycService) : ControllerBase
    {
        private readonly IInstructorUseCase _usecase = usecase;
        private readonly IVnptEkycService _ekycService = ekycService;

        [HttpGet]
        public async Task<IActionResult> GetInstructors([FromQuery] InstructorListFilterDTO filter)
        {
            var result = await _usecase.GetInstructors(filter);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorDetail([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorDetail(id);
            return result.ToActionResult();
        }

        [HttpGet("{id}/schedule")]
        public async Task<IActionResult> GetInstructorSchedule([FromRoute] Guid id)
        {
            var result = await _usecase.GetInstructorSchedule(id);
            return result.ToActionResult();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsAnInstructor([FromForm] RegistrationDTO registration)
        {
            var result = await _usecase.RegisterInstructor(registration);
            return result.ToActionResult();
        }

        [HttpGet("applicants")]
        public async Task<IActionResult> GetInstructorApplicantsWithStatus([FromQuery] ApplicationStatus status)
        {
            var result = await _usecase.GetAllInstructorApplicationsByStatus(status);
            return result.ToActionResult();
        }

        [HttpGet("{id}/applicants")]
        public async Task<IActionResult> GetInstructorApplicants(Guid id)
        {
            var result = await _usecase.GetInstructorApplication(id);
            return result.ToActionResult();
        }

        [HttpGet("applicants/{id}")]
        public async Task<IActionResult> GetInstructorApplicantWithId(Guid id)
        {
            var result = await _usecase.GetApplicationById(id);
            return result.ToActionResult();
        }

        [HttpPost("applicants/{id}/inspection")]
        public async Task<IActionResult> ModerateActionForApplication([FromRoute] Guid id, [FromQuery] string action, [FromBody] InstructorNoteDTO note)
        {
            var result = await _usecase.ModerateInstructorApplication(id, action, note.Note);
            return result.ToActionResult();
        }

        [HttpPatch("{id}/applicants")]
        public async Task<IActionResult> UpdatePartialInstructorApplicant([FromRoute] Guid id, [FromForm] RegistrationDTO registration)
        {
            var result = await _usecase.UpdateInstructorApplication(id, registration);
            return result.ToActionResult();
        }

        [HttpPost("cccd/verify")]
        public async Task<IActionResult> VerifyCitizenIdentification(
            [FromForm] IFormFile frontImage,
            [FromForm] IFormFile? backImage,
            CancellationToken cancellationToken = default)
        {
            if (frontImage is null || frontImage.Length == 0)
            {
                var error = ServiceError.BadRequestError("Ảnh mặt trước CCCD là bắt buộc.");
                return Result<FptAiEkycResponse>.Failure(error).ToActionResult();
            }

            var token = cancellationToken == default ? HttpContext.RequestAborted : cancellationToken;

            await using var frontStream = frontImage.OpenReadStream();
            await using var backStream = backImage is null ? null : backImage.OpenReadStream();

            try
            {
                var response = await _ekycService.AnalyzeDocumentAsync(
                    VnptDocumentType.CitizenIdentification,
                    frontStream,
                    frontImage.FileName,
                    backStream,
                    backImage?.FileName,
                    token);

                return Result<VnptEkycResponse>.Success(response, "VNPT đang xử lý yêu cầu xác thực.").ToActionResult();
            }
            catch (InvalidOperationException ex)
            {
                var error = ServiceError.InvalidStateError(ex.Message);
                return Result<VnptEkycResponse>.Failure(error, ex.Message).ToActionResult();
            }
            catch (HttpRequestException ex)
            {
                var error = ServiceError.ExternalServiceError(ex.Message);
                return Result<VnptEkycResponse>.Failure(error, "Không thể kết nối tới VNPT eKYC.").ToActionResult();
            }
        }
    }
}
