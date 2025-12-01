using SharedLibrary.SharedKernel.Entities;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class Instructor : BaseEntites
    {
        // properties
        public string Bio {  get; set; }
        public int Experience { get; set; } // Instructor experience will be calculated in year
        public InstructorStatus Status { get; set; }

        // System properties
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        // realationship
        public virtual User? User { get; set; }
        public virtual InstructorApplication? InstructorApplication { get; set; }
        public virtual ICollection<PersonalSchedule>? InstructorSchedules { get; set; } 
    }
}
