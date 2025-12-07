using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserProfileUpdateDTO
    {
        public IFormFile? ProfileAvatar { get; set; }
        public string? Fullname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }
}
