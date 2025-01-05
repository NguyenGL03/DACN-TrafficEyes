using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.OrganizationUnit.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.OrganizationUnit
{
    public interface IOrganizationUnitAppService : IApplicationService
    {
        Task<List<ORGANIZATION_UNIT_ENTITY>> ORGANIZATION_UNIT_Search();
        Task<CommonResult> ORGANIZATION_UNIT_Upd(ORGANIZATION_UNIT_ENTITY input);
        Task<CommonResult> ORGANIZATION_UNIT_Ins(ORGANIZATION_UNIT_ENTITY input);
        Task<CommonResult> ORGANIZATION_UNIT_Del(string id);
        Task<CommonResult> ORGANIZATION_UNIT_Move(string id, string newParentId);
        Task<PagedResultDto<ORGANIZATION_UNIT_USER_ENTITY>> ORGANIZATION_UNIT_USER_Search(ORGANIZATION_UNIT_USER_ENTITY input);
    }
}
