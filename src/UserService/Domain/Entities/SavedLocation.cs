using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class SavedLocation : BaseEntites
    {
        // Properties
        public string DisplayName {  get; set; }
        public float LocationLatitude { get; set; }
        public float LocationLongtitude { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Key for relationship
        public Guid UserId { get; set; }

        // Relationship navigation
        public virtual User? User { get; set; }
    }
}
