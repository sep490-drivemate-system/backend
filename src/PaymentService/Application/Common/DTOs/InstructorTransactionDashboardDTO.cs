namespace PaymentService.Application.Common.DTOs
{
    public class InstructorTransactionDashboardDTO
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal RevenueAfterDeduction { get; set; }
        public decimal TotalFundsWidthdrawled { get; set; }
    }
}
