using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserProfileUpdateDTO
    {
        public string? Username { get; set; }
        public GenderType? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
