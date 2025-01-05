using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using gAMSPro.MultiTenancy.Accounting.Dto;

namespace gAMSPro.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
