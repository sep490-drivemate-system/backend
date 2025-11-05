using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.NoviceDriver
{
    public class UserAddressDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("address")]
        public string AddressString { get; set; }

        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        [JsonPropertyName("longtitude")]
        public float Longitude { get; set; }
    }
}
