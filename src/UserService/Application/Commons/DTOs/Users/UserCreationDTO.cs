using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserCreationDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public UserRole Role { get; set; }
    }
}
