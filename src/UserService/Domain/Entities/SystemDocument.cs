using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class SystemDocument: BaseEntites
    {
        public string DisplayName { get; set; } // The display name
        public string Items { get; set; } // The items inside that document
        public string Category { get; set; } // Either users or cars documents
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
