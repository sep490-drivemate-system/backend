using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserAddressDTO
    {
        public string AddressString { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
