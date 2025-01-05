using Abp.Application.Services;

namespace Core.gAMSPro.Intfs.ApiESB
{
    public interface IApiESBAppService : IApplicationService
    {
        Task<IDictionary<string, object>> AccountingSync(string id);
        Task<IDictionary<string, object>> AccountingPaymentSync(string id);
    }
}
