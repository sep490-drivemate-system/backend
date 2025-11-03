using SharedLibrary.SharedKernel.Entities;
using SharedLibrary.SharedKernel.Enum;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class User : BaseEntites
    {
        // Propertíe
        public string Username { get; set; } 
        public string Email { get; set; } 
        public string HashedPassword { get; set; } 
        public string Avatar { get; set; } 
        public string PhoneNumber {  get; set; }
        public DateOnly DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public UserRole Role { get; set; }
        public AccountStatus AccountStatus { get; set; }
        
        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Keys for relationships
        public Guid MaxLicenseLevel { get; set; }

        // Relationship navigation
        public virtual NoviceDriver? NoviceDriver { get; set; }
        public virtual Instructor? Instructor { get; set; } 
        public virtual LicenseCategory? LicenseCategory { get; set; }
    }
}
