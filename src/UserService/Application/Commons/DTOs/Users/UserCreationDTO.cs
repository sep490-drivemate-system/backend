using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserCreationDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("fullname")]
        public string Fullname { get; set; }
        [JsonPropertyName("phone")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("gender")]
        public GenderType Gender { get; set; }
        [JsonPropertyName("dob")]
        public DateOnly DateOfBirth { get; set; }
        [JsonPropertyName("role")]
        public UserRole Role { get; set; }
    }
}
