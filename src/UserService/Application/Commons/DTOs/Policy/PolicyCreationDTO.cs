using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Policy
{
    public class PolicyCreationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
