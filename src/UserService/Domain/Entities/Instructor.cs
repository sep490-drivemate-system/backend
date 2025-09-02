using SharedLibrary.SharedKernel.Entities;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class Instructor : BaseEntites
    {
        public string Bio {  get; set; }
        public int Experence { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleTe { get; set; }
        public  InstructorStatus Status { get; set; }
    }
}
