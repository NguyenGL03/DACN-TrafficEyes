using System.Threading.Tasks;
using gAMSPro.Authorization.Users;

namespace gAMSPro.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
