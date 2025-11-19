namespace BookingService.Application.Commons.DTOs.Booking
{

    public class InstructorStatisticFilterDTO
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }

        public InstructorStatisticFilterDTO()
        {
            int CurrentDayOffset = (int)DateTime.Today.DayOfWeek - (int) DayOfWeek.Monday;

            if (CurrentDayOffset < 0)
            {
                CurrentDayOffset += 7;
            }

            DateTime previousMonday = DateTime.Today.AddDays(-CurrentDayOffset - 7);
            DateTime nextSunday = DateTime.Today.AddDays((7 - (int)DateTime.Today.DayOfWeek) % 7 + 7);
            
            this.From = DateOnly.FromDateTime(previousMonday);
            this.To = DateOnly.FromDateTime(nextSunday);
        }
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
