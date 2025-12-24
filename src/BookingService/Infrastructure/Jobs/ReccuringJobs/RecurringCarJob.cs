using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using SharedLibrary.Email;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Infrastructure.Jobs.ReccuringJobs
{
    public class RecurringCarJob(ILogger<RecurringCarJob> logger, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IEmailService emailService)
    {
        private readonly ILogger _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService _emailService = emailService;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task AutoSendInsuranceExpiryEmail()
        {
            // Filter for expired insurance (this expression use local time)
            Expression<Func<Car, bool>> filter = x => !x.IsDeleted 
                && x.Status == Domain.Enum.CarStatus.Approved 
                && x.InsuranceEndTime <= DateOnly.FromDateTime(DateTime.Now);

            // Get cars with expired insurance
            var cars = await _unitOfWork.CarRepository.GetAllAsync(filter: filter, disable_tracking: true);
            _logger.LogInformation($"[{DateTime.Now:hh:mm:ss tt  dd-MM-yyyy}] Found {cars.Count()} cars with expired insurance");

            // Getting owner information
            IEnumerable<UserDetailDTO> users_info;

            try
            {
                var userServiceHttpClient = _httpClientFactory.CreateClient("UserServiceClient");
                var http_message = await userServiceHttpClient.PostAsJsonAsync("api/users/ids", cars.Select(x => x.InstructorId).Distinct());
                http_message.EnsureSuccessStatusCode();

                var result = await http_message.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();
                users_info = result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now:hh:mm:ss tt  dd-MM-yyyy}] Failed to run ({ex.GetType()}):\n{ex.Message}\n{ex.StackTrace}");
                return;
            }

            foreach (var car in cars)
            {
                var user = users_info.FirstOrDefault(x => x.UserId == car.InstructorId);

                // Skip through null users (users might be deleted)
                if (user == null) continue;

                Dictionary<string, string> terms = new Dictionary<string, string>
                {
                    {"fullname", user.FullName},
                    {"car_name", car.Name },
                    {"car_plate", car.LicensePlate },
                    {"expiry_date", $"{car.InsuranceEndTime:dd-MM-yyyy}" }
                };

                // Non-awaiting for the email sending.
               _emailService.SendingEmail(user.Email, terms, "[DriveMate] Cảnh báo hết hạn giấy tờ xe (bảo hiểm xe)", EmailType.ExpiredInsurance);
            }
        }
    }
}
