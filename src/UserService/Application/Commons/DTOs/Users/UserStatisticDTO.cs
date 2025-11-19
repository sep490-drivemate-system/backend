using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace UserService.Application.Commons.DTOs.Users
{
    public class UserStatisticFilterDTO
    {
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public int Week { get; set; } = 0;
        public StatisticTimeType Type { get; set; }
    }

    public class UserStatisticDTO
    {
        // Filter option
        public StatisticTimeType Type { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }  

        // Currently in used
        public int TotalDriverCount { get; set; }
        public int TotalInstructorCount { get; set; }
        public int TotalInspectorCount { get; set; }
        public int TotalUserCount { get; set; }

        // New user data
        public int NewDriverCount { get; set; }
        public int NewInstructorCount { get; set; }
        public int NewUserCount { get; set; }

        // Deleted user data
        public int DeletedDriverCount { get; set; }
        public int DeletedInstructorCount { get; set; }
        public int DeletedUserCount { get; set; }

        // Percentage data
        public double NewDriverPercentage { get; set; }
        public double NewInstructorPercentage { get; set; }
        public double NewUserPercentage { get; set; }

        // Summarized Statistic
        public Dictionary<string, double> NoviceDriverStatistic { get; set; }
        public Dictionary<string, double> InstructorsStatistic { get; set; } 

        // Statistic summarized by role
        public Dictionary<string, int> UserRoleCount { get; set; }
        public Dictionary<string, double> UserRolePercentage { get; set; }

        // Statistic summarized by status
        public Dictionary<string, int> UserStatusCount { get; set; }
        public Dictionary<string, double> UserStatusPercentage { get; set; }
    }
}
