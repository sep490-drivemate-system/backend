using Hangfire;
using UserService.Application.Jobs.ReccurringJobs;

namespace UserService.Application.Jobs
{
    public static class RunningJobs
    {
        public static void AddRunningJobs()
        {
            RecurringJob.AddOrUpdate<ApplicationAutoRejectionJob>("auto-remove-unverified-applicants", x => x.AutoRejectionJob(), "1 0 * * *");
        }
    }
}
