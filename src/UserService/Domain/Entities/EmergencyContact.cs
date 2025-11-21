using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class EmergencyContact: BaseEntites
    {
        public string SavedName { get; set; }
        public string ContactNumber { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign key properties
        public Guid UserId { get; set; }

        // Navigational properties
        public User User { get; set; }
    }
}
