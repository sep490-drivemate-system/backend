using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Threading.Tasks;
using UserService.Application.Commons.DTOs.Auth;
using UserService.Application.Interfaces;

namespace UserService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUseCase _usecase;
        public AuthController(IAuthUseCase usecase, ICloudinaryServiceProvider cloudinaryServiceProvider)
        {
            _usecase = usecase;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInDTO signInDTO)
        {
            var result = await _usecase.SignIn(signInDTO);
            return result.ToActionResult();
        }
        [HttpPost("verify-phone")]
        public async Task<IActionResult> VerifyPhone([FromBody] string phoneNumber)
        {
            var result = await _usecase.VerifyPhone(phoneNumber);
            return result.ToActionResult();
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailDTO emailDTO)
        {
            var result = await _usecase.VerifyEmail(emailDTO.Email);
            return result.ToActionResult();
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            var result = await _usecase.SignUp(signUpDTO);
            return result.ToActionResult();
        }
        [HttpPost("signin-google")]
        public IActionResult SignInGoogle()
        {
            return Ok();
        }
    }
}
