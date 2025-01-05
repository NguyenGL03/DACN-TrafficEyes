using System.Threading.Tasks;
using Abp.Webhooks;

namespace gAMSPro.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
