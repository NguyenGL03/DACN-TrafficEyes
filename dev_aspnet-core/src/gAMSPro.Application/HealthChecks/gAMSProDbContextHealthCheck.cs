using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using gAMSPro.EntityFrameworkCore;

namespace gAMSPro.HealthChecks
{
    public class gAMSProDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public gAMSProDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("gAMSProDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("gAMSProDbContext could not connect to database"));
        }
    }
}
