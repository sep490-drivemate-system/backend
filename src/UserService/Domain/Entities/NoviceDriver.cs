using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class NoviceDriver : BaseEntites
    {
        // Properties
        public string DrivingLicenseImageUrl { get; set; }
        public bool AllowedBooking { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }


        // Relationship navigation
        public virtual User? User {get; set; }

    }
}
