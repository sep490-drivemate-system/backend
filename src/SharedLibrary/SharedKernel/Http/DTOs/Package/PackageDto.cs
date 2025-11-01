using System;
using System.Text.Json.Serialization;

namespace SharedLibrary.SharedKernel.Http.DTOs.Package
{
    public class PackageDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("instructor_id")]
        public Guid InstructorId { get; set; }
        [JsonPropertyName("available_car_ids")]
        public List<Guid> CarIds { get; set; }
    }
}
