using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Sms
{
    public interface ISmsService
    {
        Task<bool> SendVerificationCodeAsync(string phoneNumber, string code);
        Task<SmsResponse> SendSmsAsync(string phoneNumber, string message);
    }

    public class SmsResponse
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int? TransactionId { get; set; }
        public decimal? TotalPrice { get; set; }
        public List<string> InvalidPhones { get; set; } = new List<string>();
    }
}
