using SharedLibrary.SharedKernel.Enum;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Auth
{
    public class VerifyDTO
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class SignUpDTO
    {
       // public IFormFile? Avatar { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public UserRole UserRole { get; set; }

    }
}
