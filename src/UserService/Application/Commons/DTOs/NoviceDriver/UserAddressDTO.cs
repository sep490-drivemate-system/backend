using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.NoviceDriver
{
    public class UserAddressDTO
    {
        public Guid Id { get; set; }
        public string AddressString { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
