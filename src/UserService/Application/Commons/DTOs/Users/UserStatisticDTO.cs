using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserStatisticFilterDTO
    {
        [FromQuery(Name = "year")]
        public int Year { get; set; } = 0;

        [FromQuery(Name = "month")]
        public int Month { get; set; } = 0;

        [FromQuery(Name = "week")]
        public int Week { get; set; } = 0;

        [FromQuery(Name = "type")]
        public StatisticTimeType Type { get; set; }
    }

    public class UserStatisticDTO
    {
        // Filter option
        [JsonPropertyName("type")]
        public StatisticTimeType Type { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("week")]
        public int Week { get; set; }  

        // Currently in used
        [JsonPropertyName("total_novice_driver_count")]
        public int TotalDriverCount { get; set; }

        [JsonPropertyName("total_instructor_count")]
        public int TotalInstructorCount { get; set; }

        public int TotalInspectorCount { get; set; }

        [JsonPropertyName("total_user_count")]
        public int TotalUserCount { get; set; }

        // New user data
        [JsonPropertyName("new_novice_driver_count")]
        public int NewDriverCount { get; set; }

        [JsonPropertyName("new_instructor_count")]
        public int NewInstructorCount { get; set; }

        [JsonPropertyName("new_user_count")]
        public int NewUserCount { get; set; }

        // Deleted user data
        [JsonPropertyName("deleted_novice_driver_count")]
        public int DeletedDriverCount { get; set; }

        [JsonPropertyName("deleted_instructor_count")]
        public int DeletedInstructorCount { get; set; }

        [JsonPropertyName("deleted_user_count")]
        public int DeletedUserCount { get; set; }

        // Percentage data
        [JsonPropertyName("new_driver_percentage")]
        public double NewDriverPercentage { get; set; }

        [JsonPropertyName("new_isntructor_percentage")]
        public double NewInstructorPercentage { get; set; }

        [JsonPropertyName("new_user_percentage")]
        public double NewUserPercentage { get; set; }

        // Summarized Statistic

        [JsonPropertyName("novice_driver")]
        public Dictionary<string, double> NoviceDriverStatistic { get; set; }

        [JsonPropertyName("instructors")]
        public Dictionary<string, double> InstructorsStatistic { get; set; } 

        // Statistic summarized by role
        [JsonPropertyName("user_role_count")]
        public Dictionary<string, int> UserRoleCount { get; set; }

        [JsonPropertyName("user_role_percentage")]
        public Dictionary<string, double> UserRolePercentage { get; set; }

        // Statistic summarized by status
        [JsonPropertyName("user_status_count")]
        public Dictionary<string, int> UserStatusCount { get; set; }

        [JsonPropertyName("user_status_percentage")]
        public Dictionary<string, double> UserStatusPercentage { get; set; }
    }
}
