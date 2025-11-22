using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.RoadTypes
{
    public class RoadTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
