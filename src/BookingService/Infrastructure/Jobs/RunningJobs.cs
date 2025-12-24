using Hangfire;

namespace BookingService.Infrastructure.Jobs
{
    public class RunningJobs(IRecurringJobManager recurringJobManager)
    {
        private readonly IRecurringJobManager _recurringJobManager = recurringJobManager;

        public void AddRunningJobs()
        {
            _recurringJobManager.AddOrUpdate<ReccuringJobs.RecurringCarJob>("auto-remove-unverified-applicants", x => x.AutoSendInsuranceExpiryEmail(), "0 0 * * *");
        }
    }
}
