using Microsoft.Extensions.Configuration;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Http.Interfaces;

namespace SharedLibrary.SharedKernel.Http.Implementation
{
    public class User : IUser
    {
        private readonly HttpService _httpService;
        private readonly IConfiguration _config;

        public User(HttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _config = configuration;
        }

        public async Task<Dictionary<Guid, InstructorBasicInfoDTO>> GetBatchInstructorInfo(List<Guid> instructorIds)
        {
            string userServiceUrl = _config["USERSERVICE:URL"];
            string url = $"{userServiceUrl}/api/users/batch-instructor-info";
            
            var result = await _httpService.PostAsync<List<Guid>, Dictionary<Guid, InstructorBasicInfoDTO>>(url, instructorIds);
            
            return result ?? new Dictionary<Guid, InstructorBasicInfoDTO>();
        }

        public async Task<Dictionary<Guid, NoviceDriverBasicInfoDTO>> GetBatchNoviceDriverInfo(List<Guid> noviceDriverIds)
        {
            string userServiceUrl = _config["USERSERVICE:URL"];
            string url = $"{userServiceUrl}/api/users/batch-novice-driver-info";
            
            var result = await _httpService.PostAsync<List<Guid>, Dictionary<Guid, NoviceDriverBasicInfoDTO>>(url, noviceDriverIds);
            
            return result ?? new Dictionary<Guid, NoviceDriverBasicInfoDTO>();
        }
    }
}
