using System;
using System.Text.Json.Serialization;

namespace SharedLibrary.SharedKernel.Http.DTOs.Package
{
    public class PackageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public decimal Price { get; set; }
        public Guid InstructorId { get; set; }
        public IList<string> DrivingSkills{ get; set; }
        public IList<string> RoadTypes{ get; set; }
        public bool IsRentalCar { get; set; }


    }

   

}
