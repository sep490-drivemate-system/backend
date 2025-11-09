using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Policy
{
    public class PolicyDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Detail { get; set; }
    }
}
