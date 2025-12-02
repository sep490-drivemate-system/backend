using System.Text.Json.Serialization;
using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Policy
{
    public class PolicyCreationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public PolicyType Type { get; set; }
    }
}
