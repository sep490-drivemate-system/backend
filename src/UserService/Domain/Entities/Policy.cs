using SharedLibrary.SharedKernel.Entities;
using System.Text.Json.Serialization;

namespace UserService.Domain.Entities
{
    public class Policy: BaseEntites
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PolicyType { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
