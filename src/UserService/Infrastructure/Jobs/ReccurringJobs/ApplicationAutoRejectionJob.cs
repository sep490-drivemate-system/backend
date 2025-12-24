using SharedLibrary.Email;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enum;

namespace UserService.Application.Jobs.ReccurringJobs
{
    public class ApplicationAutoRejectionJob(ILogger<ApplicationAutoRejectionJob> logger, IUnitOfWork unitOfWork)
    {
        private readonly ILogger _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService _emailService; // This is currently not set up yet

        public async Task AutoRejectionJob()
        {
            var applications = await _unitOfWork.ApplicationRepository.GetAllAsync(filter: x => x.Status == ApplicationStatus.Pending || x.Status == ApplicationStatus.ReApplying);

            // Applications which expired when this task run:
            var expiring_applications = applications.Where(x => x.DatebeforeExpiry < DateOnly.FromDateTime(DateTime.Now));

            _logger.LogInformation($"[{DateTime.Now:hh:mm:ss tt  dd-MM-yyyy}] Found {expiring_applications.Count()} expired applications");

            foreach (var application in expiring_applications)
            {
                application.Status = ApplicationStatus.Rejected;

                await _unitOfWork.Repository<ApplicationTracking>().CreateAsync(new ApplicationTracking
                {
                    ApplicationId = application.Id,
                    Status = ApplicationStatus.Rejected,
                    Note = "Auto rejection"
                });
            }

            await _unitOfWork.CommitChangesAsync();
        }

        public async Task AutoBioExperienceIncrementalJob()
        {
            // Get all non-deleted instructor
            var instructors = await _unitOfWork.InstructorRepository.GetAllAsync(x => !x.IsDeleted);

            foreach (var instructor in instructors)
            {
                instructor.Experience += 1;
            }

            await _unitOfWork.CommitChangesAsync();
        }
    }
}
