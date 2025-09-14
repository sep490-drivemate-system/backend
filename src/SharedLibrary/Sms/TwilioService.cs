using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Net;
using System.IO;

namespace SharedLibrary.Sms
{
    public class SpeedSmsService : ISmsService
    {
        private readonly SpeedSmsSettings _speedSmsSettings;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SpeedSmsService(IOptions<SpeedSmsSettings> speedSmsSettings, HttpClient httpClient, IConfiguration configuration)
        {
            _speedSmsSettings = speedSmsSettings.Value;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> SendVerificationCodeAsync(string phoneNumber, string code)
        {
            try
            {
                var message = string.Format(_speedSmsSettings.MessageTemplate, code);
                var response = await SendSmsAsync(phoneNumber, message);
                return response.IsSuccess;
            }
            catch
            {
                return false;
            }
        }

        public async Task<SmsResponse> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                GetSpeedSmsSettings();
                
                // Normalize phone number (remove +84 and replace with 0 if needed)
                var normalizedPhone = NormalizePhoneNumber(phoneNumber);

                var url = $"{_speedSmsSettings.ApiUrl}/sms/send";

                // Basic Auth
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_speedSmsSettings.AccessToken}:x"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

                var payload = new
                {
                    to = new[] { phoneNumber },
                    content = message,
                    type = _speedSmsSettings.SmsType,
                    sender = _speedSmsSettings.SmsType == 2 ? "" : _speedSmsSettings.Sender
                };

                var json = JsonSerializer.Serialize(payload);
                var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<SpeedSmsApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new SmsResponse
                {
                    IsSuccess = apiResponse?.Status == "success",
                    Status = apiResponse?.Status ?? "unknown",
                    Code = apiResponse?.Code ?? "unknown",
                    Message = apiResponse?.Message ?? string.Empty,
                    TransactionId = apiResponse?.Data?.TranId,
                    TotalPrice = apiResponse?.Data?.TotalPrice,
                    InvalidPhones = apiResponse?.InvalidPhone ?? new List<string>()
                };
            }
            catch (HttpRequestException httpEx)
            {
                return new SmsResponse
                {
                    IsSuccess = false,
                    Status = "error",
                    Code = "HttpError",
                    Message = $"HTTP Error: {httpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                return new SmsResponse
                {
                    IsSuccess = false,
                    Status = "error",
                    Code = "500",
                    Message = ex.Message
                };
            }
        }

        private string NormalizePhoneNumber(string phoneNumber)
        {
            // Remove all non-digit characters
            var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
            
            // If starts with 84, replace with 0
            if (digits.StartsWith("84"))
            {
                return "0" + digits.Substring(2);
            }
            
            // If doesn't start with 0, add 0
            if (!digits.StartsWith("0"))
            {
                return "0" + digits;
            }
            
            return digits;
        }

        private IConfigurationSection GetSpeedSmsSettings()
        {
            return _configuration.GetSection("SpeedSMS");
        }
    }

    // Internal classes for API response deserialization
    internal class SpeedSmsApiResponse
    {
        public string Status { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public SpeedSmsData? Data { get; set; }
        public List<string>? InvalidPhone { get; set; }
    }

    internal class SpeedSmsData
    {
        public int TranId { get; set; }
        public int TotalSMS { get; set; }
        public decimal TotalPrice { get; set; }
        public List<string> InvalidPhone { get; set; } = new List<string>();
    }
}
