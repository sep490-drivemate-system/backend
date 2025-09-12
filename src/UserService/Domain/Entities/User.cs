using SharedLibrary.SharedKernel.Entities;
using SharedLibrary.SharedKernel.Enum;

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
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }
        public UserRole Role { get; set; }

        // Relationship navigation
        public virtual NoviceDriver? NoviceDriver { get; set; }
        public virtual Instructor? Instructor { get; set; } 
        public virtual RefreshToken? RefreshToken { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; }
        public virtual ICollection<InstructorApplication>? InstructorApplications{ get; set; }


    }
}
