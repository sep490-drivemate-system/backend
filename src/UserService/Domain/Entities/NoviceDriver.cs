using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class NoviceDriver : BaseEntites
    {
        // Properties
        public string DrivingLicense { get; set; }
        public DateOnly DrivingLicenseExpirationDate { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Relationship navigation
        public virtual User? User {get; set; }
    }
}
