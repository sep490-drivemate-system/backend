using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace BookingService.Application.Commons.DTOs.Booking
{
    public class BookingStatisticFilterDTO
    {
        [FromQuery(Name = "year")]
        public int Year { get; set; } = 0;

        [FromQuery(Name = "month")]
        public int Month { get; set; } = 0;

        [FromQuery(Name = "week")] 
        public int Week {  get; set; } = 0;

        [FromQuery(Name = "type")] 
        public StatisticTimeType Type { get; set; } = 0;
    }

    public class BookingStatisticDTO
    {
        #region Statistic Setting
        [JsonPropertyName("type")]
        public StatisticTimeType Type { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("week")]
        public int Week { get; set; }
        #endregion

        #region System wide (not influenced by time filter)
        [JsonPropertyName("total_package_count")]
        public int TotalPackageCount { get; set; }

        [JsonPropertyName("total_car_count")]
        public int TotalCarCount { get; set; }

        [JsonPropertyName("total_booking_count")]
        public int TotalBookingCount { get; set; }

        [JsonPropertyName("total_session_count")]
        public int TotalSessionCount { get; set; }
        #endregion

        #region Booking Statistic
        [JsonPropertyName("booking_by_status_count")]
        public Dictionary<string, int> BookingByStatusCount { get; set; }

        [JsonPropertyName("booking_by_status_percentage")]
        public Dictionary<string, double> BookingStatusPercentage { get; set; }

        [JsonPropertyName("total_cancelation_count")]
        public int TotalCancelationCount { get; set; }
        #endregion

        #region Session Statistic
        [JsonPropertyName("session_by_status_count")]
        public Dictionary<string, int> SessionByStatusCount { get; set; }

        [JsonPropertyName("session_by_status_percentage")]
        public Dictionary<string, double> SessionStatusPercentage { get; set; }

        [JsonPropertyName("session_cancelation_count")]
        public Dictionary<string, int> SessionCancelationCount { get; set; }

        [JsonPropertyName("session_cancelation_percentage")]
        public Dictionary<string, double> SessionCancelationPercentage { get; set; }
        #endregion

        #region Charts
        [JsonPropertyName("booking_count_by_day")]
        public Dictionary<string, int> BookingByDay { get; set; }

        [JsonPropertyName("session_average_time")]
        public Dictionary<string, double> SessionTimeByDay { get; set; }
        #endregion

        [JsonPropertyName("top_packages")]
        public IEnumerable<TopPackage> TopPackages { get; set; }

        [JsonPropertyName("top_car")]
        public IEnumerable<TopCar> TopCars { get; set; }
    }

    public class TopPackage
    {
        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        [JsonPropertyName("book_count")]
        public int PackageBookCount { get; set; }

        [JsonPropertyName("average_rating")]
        public double AverageRating { get; set; }
    }

    public class TopCar
    {
        [JsonPropertyName("car")]
        public string CarName { get; set; }

        [JsonPropertyName("book_count")]
        public int CarBookCount { get; set; }

        [JsonPropertyName("average_rating")]
        public double AverageRating { get; set; }
    }
}
