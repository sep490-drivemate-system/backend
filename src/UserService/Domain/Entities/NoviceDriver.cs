using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class NoviceDriver : BaseEntites
    {
        public string DrivingLicense { get; set; }
        public bool AllowedBooking { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleTe { get; set; }
        public virtual InstructorApplication Application { get; set; } = new InstructorApplication();

    }
}
