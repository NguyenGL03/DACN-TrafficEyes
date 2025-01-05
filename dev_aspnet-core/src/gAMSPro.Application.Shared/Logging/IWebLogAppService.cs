using Abp.Application.Services;
using gAMSPro.Dto;
using gAMSPro.Logging.Dto;

namespace gAMSPro.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
