using System;
using System.Text.Json.Serialization;

namespace SharedLibrary.SharedKernel.Http.DTOs.Package
{
    public class PackageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public Guid InstructorId { get; set; }
        public IList<DrivingSkill> DrivingSkills{ get; set; }
        public IList<RoadType> RoadTypes{ get; set; }
        public bool IsRentalCar { get; set; }


    }

    public class DrivingSkill
    {
        public string Name { get; set; }
    }
    public class RoadType
    {
        public string Name { get; set; }
    }

}
