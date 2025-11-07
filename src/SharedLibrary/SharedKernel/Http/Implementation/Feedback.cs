using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using SharedLibrary.SharedKernel.Http.DTOs.Payment;
using SharedLibrary.SharedKernel.Http.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.Implementation
{
    public class Feedback : IFeedback
    {
        private readonly HttpService _httpService;
        private readonly IConfiguration _config;
        public Feedback(HttpService httpService,IConfiguration configuration)
        {
            _httpService = httpService;
             _config = configuration;
        }

        public async Task<NoviceDriverInfoFeedbackDTO> GetNoviceDriverInfor(Guid noviceDriverid)
        {

            string userServiceUrl = _config["USERSERVICE:URL"];
            string url = $"{userServiceUrl}/api//user/driver-feedback";
            var result = await _httpService.PostAsync<Guid, NoviceDriverInfoFeedbackDTO>(url, noviceDriverid);
            return result;
        }

        public async Task<FeedbackResponse> GetStatiticFeedback(List<Guid> listGuidInstructor)
        {
            var request = new FeedbackRequest
            {
                ListInstructor = listGuidInstructor
            };
            string bookingServiceUrl = _config["BOOKINGSERVICE:URL"];
            string url = $"{bookingServiceUrl}/api/feedback/list-instructor";
            var result = await _httpService.PostAsync<FeedbackRequest, FeedbackResponse>(url, request);
            return result;
        }
    }
}
