using UserService.Domain.Enum;

namespace UserService.Application.Commons.DTOs.Instructors
{
    public class InstructorDetailDTO 
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }

        public int ExperienceYear { get; set; }

        public int BookingCount { get; set; }

        public decimal AverageRating { get; set; }
        public string Bio { get; set; }

        public GenderType Gender { get; set; }

        public DateOnly Birthdate { get; set; }

        public OverViewPackage OverViewPackage { get; set; }

        public List<CarPackage> CarPackages { get; set; }
    }
    public class OverViewPackage
    {
        public string Instructor {  get; set; }
        public string InstructorAndCar { get; set; }
    }
    public class CarPackage
    {
        public Guid Id{ get; set; }
        public string Thumnail{ get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
