using Abp.Application.Services;
using gAMSPro.TestingService.Dto;
using System.Threading.Tasks;

namespace gAMSPro.Notifications
{
    public interface ITestingService : IApplicationService
    {
        Task<SendEmailAndNotifyEntity> TestSendEmailAndNotify(SendEmailAndNotifyEntity input);
    }
}
