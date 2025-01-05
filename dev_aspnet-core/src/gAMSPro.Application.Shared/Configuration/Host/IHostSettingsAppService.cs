using System.Threading.Tasks;
using Abp.Application.Services;
using gAMSPro.Configuration.Host.Dto;

namespace gAMSPro.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
