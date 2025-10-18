namespace UserService.Application.Commons.DTOs.Cars
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

        public int SeatCounts { get; set; }
        
        public string VehicleType { get; set; }

        public string FuelType { get; set; }

        public decimal UnitPrice { get; set; }

        public string PreferredLocation { get; set; }

        public int BookingCount { get; set; }

        public decimal AverageRating { get; set; }
    }

    public class CarDetailDTO: CarDTO
    {
        public string LicensePlate { get; set; }

        public string Detail { get; set; }
       
        public string ManufacturerName { get; set; }

        public List<string> Images { get; set; }
        
        public List<CarPackageDTO> CarPackages { get; set; }

        public Guid OwnerId { get; set; }
    }
}
