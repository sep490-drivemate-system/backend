using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Auth;

namespace UserService.Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<Result<string>> VerifyEmail(string email);
        Task<Result<string>> VerifyPhone(string phone);
        Task<Result<SignUpRespondDTO>> SignUp(SignUpDTO signUpDTO);
    }
}
