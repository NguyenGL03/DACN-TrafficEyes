using System.Threading.Tasks;
using Abp.Domain.Uow;

namespace gAMSPro.OpenIddict
{
    public interface IOpenIddictDbConcurrencyExceptionHandler
    {
        Task HandleAsync(AbpDbConcurrencyException exception);
    }
}