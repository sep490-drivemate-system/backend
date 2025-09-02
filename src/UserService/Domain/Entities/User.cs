using SharedLibrary.SharedKernel.Entities;
using SharedLibrary.SharedKernel.Enum;

namespace UserService.Domain.Entities
{
    public class User : BaseEntites
    {
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public string HasedPassword { get; set; } 
        public string Avatar { get; set; } 
        public string PhoneNumber {  get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleTe { get; set; }
        public UserRole Role { get; set; } 
      
        //public virtual NoviceDriver NoviceDriver { get; set; } = new NoviceDriver();
        //public virtual Instructor Instructor { get; set; } = new Instructor();

        //public virtual RefreshToken RefreshToken { get; set; } = new RefreshToken();
        //public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();


    }
}
