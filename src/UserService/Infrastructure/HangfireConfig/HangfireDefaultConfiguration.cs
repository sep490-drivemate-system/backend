using Hangfire.Dashboard;

namespace UserService.Infrastructure.HangfireConfig
{
    public class HangfireDefaultConfiguration: IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // WARNING: This allows anonymous access to the Hangfire dashboard. 
            // It contains sensitive job information and management controls.
            return true;
        }
    }
}
