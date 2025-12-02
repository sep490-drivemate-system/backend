using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;

namespace BookingService.Application.Commons.DTOs.Booking
{

    public class InstructorStatisticFilterDTO
    {
        [FromQuery(Name = "year")]
        public int Year { get; set; } = 0;

        [FromQuery(Name = "month")]
        public int Month { get; set; } = 0;

        [FromQuery(Name = "week")]
        public int Week { get; set; } = 0;

        [FromQuery(Name = "type")]
        public StatisticTimeType Type { get; set; } = 0;
    }

    public class InstructorStatisticDTO
    {
        public int TotalPackageCount { get; set; }
        public int TotalCarCount { get; set; }
        public int TotalUpcomingSesionCount {  get; set; }
        public IEnumerable<RecentPackagePurchasesDTO> RecentPurchases { get; set; }
        public Dictionary<string, int> TotalSessionByStatusCount { get; set; }
        public Dictionary<string, Dictionary<string, int>> TotalSessionByday {  get; set; } // Based on "from and to"
        public IEnumerable<TopPersonalPackage> TopPersonalPackages { get; set; }
        public IEnumerable<TopPersonalCar> TopPersonalCars { get; set; }
    }

    public class RecentPackagePurchasesDTO
    {
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public string PackageName { get; set; }
        public DateTime BoughtTime { get; set; }

        // Navigational ids
        public Guid NoviceDriverUserId { get; set; } // User Id
        public Guid PackageId { get; set; }
    }
    public class TopPersonalPackage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BookCount { get; set; }
        public double Percentage { get; set; }
    }
    public class TopPersonalCar
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BookCount { get; set; }
        public double Percentage { get; set; }
    }
}
