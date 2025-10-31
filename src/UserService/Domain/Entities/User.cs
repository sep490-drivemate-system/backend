using SharedLibrary.SharedKernel.Entities;
using SharedLibrary.SharedKernel.Enum;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class User : BaseEntites
    {
        // Propertíe
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public string HashedPassword { get; set; } 
        public string Avatar { get; set; } 
        public string PhoneNumber {  get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime UpdateAt { get; set; }
        public GenderType Gender { get; set; }
        public bool IsDelete { get; set; }
        public UserRole Role { get; set; }

        // Relationship navigation
        public virtual NoviceDriver? NoviceDriver { get; set; }
        public virtual Instructor? Instructor { get; set; } 
        public virtual ICollection<Address>? Addresses { get; set; }
        public virtual ICollection<ApplicationTracking>? InstructorApplications{ get; set; }


    }
}
