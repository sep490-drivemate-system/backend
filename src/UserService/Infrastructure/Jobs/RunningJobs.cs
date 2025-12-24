using Hangfire;
using UserService.Application.Jobs.ReccurringJobs;

namespace UserService.Application.Jobs
{
    public class RunningJobs(IRecurringJobManager recurringJobManager)
    {
        private readonly IRecurringJobManager _recurringJobManager = recurringJobManager;

        public void AddRunningJobs()
        {
            _recurringJobManager.AddOrUpdate<ApplicationAutoRejectionJob>("auto-remove-unverified-applicants", x => x.AutoRejectionJob(), "1 0 * * *");
            _recurringJobManager.AddOrUpdate<ApplicationAutoRejectionJob>("auto-update-bio", x => x.AutoBioExperienceIncrementalJob(), "0 0 1 1 *");
        }
    }
}
