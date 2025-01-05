using System.Threading.Tasks;
using Abp.Application.Services;
using gAMSPro.Install.Dto;

namespace gAMSPro.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}