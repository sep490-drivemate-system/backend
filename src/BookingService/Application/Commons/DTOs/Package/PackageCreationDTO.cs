using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.Package
{
    public class PackageCreationDTO
    {
        [FromForm(Name = "name")]
        public string Name { get; set; }

        [FromForm(Name = "description")]
        public string Description { get; set; }

        [JsonIgnore]
        public Guid InstructorId { get; set; }

        [FromForm(Name = "duration")]
        public float Duration { get; set; } // Duration is calculated in hours

        [FromForm(Name = "road_types")]
        public Guid[] RoadTypes { get; set; }

        [FromForm(Name = "driving_skills")]
        public Guid[] DrivingSkills { get; set; }

        [FromForm(Name = "price")]
        public decimal Price { get; set; }

        [FromForm(Name = "is_rental_car")]
        public bool IsRentalCar { get; set; }

        [FromForm(Name = "car_ids")]
        public Guid[] PackageCars { get; set; }
    }
}
