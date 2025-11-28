using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Auth;

namespace UserService.Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<Result<string>> Verify(VerifyDTO verifyDTO);
      //  Task<Result<string>> VerifyPhone(string phone);
        Task<Result<SignUpRespondDTO>> SignUp(SignUpDTO signUpDTO);
        Task<Result<SignInRespondDTO>> SignIn(SignInDTO signInDTO);
    }
}
