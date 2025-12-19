using BookingService.Domain.Enum;
using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.Cars.Get
{
    public class CarPackageDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CarDTO
    {
        public Guid Id { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public int SeatCounts { get; set; }
        public string VehicleType { get; set; }
        public DrivingLicenseTier LicenseTier { get; set; }
        [JsonPropertyName("brand")]
        public string ManufacturerName { get; set; }
        public Guid ManufacturerId { get; set; }
        [JsonPropertyName("fuel")]
        public string FuelType { get; set; }
        [JsonPropertyName("booking_count")]
        public int BookingCount { get; set; }
        [JsonPropertyName("average_rating")]
        public double AverageRating { get; set; }
        public CarStatus Status { get; set; }
    }

    public class CarDetailDTO : CarDTO
    {
        [JsonPropertyName("license_plate")]
        public string LicensePlate { get; set; }
        [JsonPropertyName("dsescription")]
        public string Detail { get; set; }
        [JsonPropertyName("images")]
        public IEnumerable<string> Images { get; set; }
        [JsonPropertyName("instructor_id")]
        public Guid OwnerId { get; set; }
        [JsonPropertyName("documents_string")]
        public string DocumentsRawString { get; set; }
        [JsonPropertyName("insurance_document")]
        public CarDocument? Insurance { get; set; }
        [JsonPropertyName("registration_document")]
        public CarDocument? Registration { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        public CarStatus StatusEnum { get; set; }
    }

    public class CarDocument
    {
        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }
        public string? DocumentType { get; set; }
    }


    public class CarInstructorDetailDTO
    {
        public Guid Id { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public int SeatCounts { get; set; }
        public string VehicleType { get; set; }
        public DrivingLicenseTier LicenseTier { get; set; }
    }
}
