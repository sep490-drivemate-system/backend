using Microsoft.AspNetCore.Mvc;
using SharedLibrary.SharedKernel.Enum;
using System.Text.Json.Serialization;

namespace PaymentService.Application.Common.DTOs
{
    public class PaymentStatisticFilterDTO
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

    public class PaymentStatisticDTO
    {
        [JsonPropertyName("profit")]
        public decimal TotalProfit { get; set; }

        [JsonPropertyName("instructors_payment")]
        public decimal TotalPaymentForInstructor { get; set; }

        [JsonPropertyName("earning")]
        public decimal TotalEarning { get; set; }

        [JsonPropertyName("holding")]
        public decimal TotalHolding { get; set; }

        [JsonPropertyName("profit_graph")]
        public Dictionary<string, decimal> ProfitByDay { get; set; }

        [JsonPropertyName("earning_graph")]
        public Dictionary<string, decimal> EarningByDay { get; set; }
    }
}

