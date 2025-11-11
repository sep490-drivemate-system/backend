using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Instructors.Car
{
    public class InstructorCar
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("fuel_type")]
        public string FuelType { get; set; }

        [JsonPropertyName("seat_count")]
        public int SeatCount { get; set; }

        [JsonPropertyName("license_tier")]
        public DrivingLicenseTier LicenseTier { get; set; }
    }
}
