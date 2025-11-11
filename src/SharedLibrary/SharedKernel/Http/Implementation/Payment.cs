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

        public async Task<PaymentResponse> CheckWalletBooking(Guid userId, decimal amount, Guid bookingId, Guid? drivingSessionId = null)
        {
            var paymentRequest = new PaymentRequest
            {
                UserId = userId,
                Amount = amount,
                BookingId = bookingId,
                DrivingSessionId = drivingSessionId
            };
            string userServiceUrl = _config["PAYMENTSERVICE:URL"];
            string url = $"{userServiceUrl}/api/wallet/check-payment";
            var result = await _httpService.PostAsync<PaymentRequest, PaymentResponse>(url, paymentRequest);
            return result;
        }
    }
}
