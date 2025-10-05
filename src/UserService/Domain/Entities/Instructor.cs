using SharedLibrary.SharedKernel.Entities;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class Instructor : BaseEntites
    {
        // properties
        public string Bio {  get; set; } = string.Empty;
        public int Experience { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }
        public  InstructorStatus Status { get; set; }

        // realationship
        public virtual User? User { get; set; }
        public virtual InstructorApplication? InstructorApplication { get; set; }
        public virtual ICollection<ScheduleUnavailability>? ScheduleAvailabilities { get; set; } 
        public virtual ICollection<Car>? Cars { get; set; } 

    }
}
