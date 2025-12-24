using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.Http.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Implementation
{
    public class Payment : IPayment
    {
        private readonly HttpService _httpService;
        private readonly IConfiguration _config;
        public Payment(HttpService httpService,IConfiguration configuration)
        {
            _httpService = httpService;
            _config = configuration;
        }

        public async Task<bool> CheckWalletBooking(Guid userId, decimal amount, Guid bookingId, Guid? drivingSessionId = null)
        {
            var paymentRequest = new PaymentRequest
            {
                UserId = userId,
                Amount = amount,
                BookingId = bookingId,
                DrivingSessionId = drivingSessionId
            };
            string userServiceUrl = _config["PAYMENTSERVICE:URL"];
            string url = $"{userServiceUrl}/api/wallet/payment-booking";
            var result = await _httpService.PostAsync<PaymentRequest, bool>(url, paymentRequest);
            return result;
        }

        public async Task<bool> CheckWalletSession(Guid userId, Guid instructorId,decimal amount, Guid bookingId, Guid? drivingSessionId = null)
        {
            var paymentRequest = new PaymentRequest
            {
                UserId = userId,
                Amount = amount,
                BookingId = bookingId,
                DrivingSessionId = drivingSessionId,
                InstructorId = instructorId

            };
            string userServiceUrl = _config["PAYMENTSERVICE:URL"];
            string url = $"{userServiceUrl}/api/wallet/payment-session";
            var result = await _httpService.PostAsync<PaymentRequest, bool>(url, paymentRequest);
            return result;
        }
    }
}
