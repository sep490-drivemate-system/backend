using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.NoviceDriver
{
    public class UserAddressDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("address")]
        public string AddressString { get; set; }
    }
}
