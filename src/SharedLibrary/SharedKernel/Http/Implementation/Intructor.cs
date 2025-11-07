using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.Http.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Implementation
{
    public class Intructor : IIntructor
    {
        private readonly HttpService _httpService;
        private readonly IConfiguration _config;

        public Intructor(HttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _config = configuration;
        }

        public async Task<InstructorOverviewFeedbackResponse> GetInstructorOverviewFeedback(Guid instructorId)
        {
            string userServiceUrl = _config["BOOKINGSERVICE:URL"];
            string url = $"{userServiceUrl}/api/feedback/list-overview-instructo";
            var result = await _httpService.PostAsync<Guid, InstructorOverviewFeedbackResponse>(url, instructorId);
            return result;
        }
    }
}
