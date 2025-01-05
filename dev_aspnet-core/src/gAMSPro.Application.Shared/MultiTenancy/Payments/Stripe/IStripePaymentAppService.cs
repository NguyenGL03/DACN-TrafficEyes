using System.Threading.Tasks;
using Abp.Application.Services;
using gAMSPro.MultiTenancy.Payments.Dto;
using gAMSPro.MultiTenancy.Payments.Stripe.Dto;

namespace gAMSPro.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();
        
        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}