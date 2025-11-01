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
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        [JsonPropertyName("name")]
        public string ModelName { get; set; }
        [JsonPropertyName("price")]
        public decimal UnitPrice { get; set; }
        [JsonPropertyName("seats")]
        public int SeatCounts { get; set; }
        [JsonPropertyName("car_type")]
        public string VehicleType { get; set; }
        [JsonPropertyName("brand")]
        public string ManufacturerName { get; set; }
        [JsonPropertyName("fuel")]
        public string FuelType { get; set; }
        [JsonPropertyName("booking_count")]
        public int BookingCount { get; set; }
        [JsonPropertyName("average_rating")]
        public decimal AverageRating { get; set; }
    }

    public class CarDetailDTO : CarDTO
    {
        [JsonPropertyName("license_plate")]
        public string LicensePlate { get; set; }
        [JsonPropertyName("dsescription")]
        public string Detail { get; set; }
        [JsonPropertyName("images")]
        public List<string> Images { get; set; }
        [JsonPropertyName("instructor_id")]
        public Guid OwnerId { get; set; }
        [JsonPropertyName("insurance_document")]
        public CarDocument Insurance { get; set; }
        [JsonPropertyName("registration")]
        public CarDocument InsuranceDocument { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class CarDocument
    {
        [JsonPropertyName("front_image")]
        public string? FrontImageUrl { get; set; }
        [JsonPropertyName("back_image")]
        public string? BackImageUrl { get; set; }
        [JsonPropertyName("expiration_date")]
        public DateOnly ExpirationDate { get; set; }
    }
}
