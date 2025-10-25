using SharedLibrary.SharedKernel.Enum;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Auth
{
    public class UserClaimTokenDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}
