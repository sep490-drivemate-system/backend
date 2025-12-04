using SharedLibrary.SharedKernel.Enum;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserFilterDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? searchKey { get; set; } = null;
        public AccountStatus? Status { get; set; } = null;
        public UserRole? Role { get; set; } = null;
    }
}
