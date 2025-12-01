using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Policy
{
    public class PolicyDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public PolicyType? Type { get; set; }
    }
}
