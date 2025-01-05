using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using gAMSPro.Common.Dto;
using gAMSPro.Editions.Dto;

namespace gAMSPro.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}