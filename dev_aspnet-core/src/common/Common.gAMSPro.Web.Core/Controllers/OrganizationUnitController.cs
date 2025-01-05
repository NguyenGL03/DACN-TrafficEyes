using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.OrganizationUnit;
using Common.gAMSPro.Intfs.OrganizationUnit.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class OrganizationUnitController : CoreAmsProControllerBase
    {

        readonly IOrganizationUnitAppService organizationUnitAppService;

        public OrganizationUnitController(IOrganizationUnitAppService organizationUnitAppService)
        {
            this.organizationUnitAppService = organizationUnitAppService;
        }
        [HttpPost]
        public Task<List<ORGANIZATION_UNIT_ENTITY>> ORGANIZATION_UNIT_Search()
        {
            return organizationUnitAppService.ORGANIZATION_UNIT_Search();
        }
        [HttpPost]
        public Task<CommonResult> ORGANIZATION_UNIT_Ins([FromBody] ORGANIZATION_UNIT_ENTITY input)
        {
            return organizationUnitAppService.ORGANIZATION_UNIT_Ins(input);
        }
        [HttpPost]
        public Task<CommonResult> ORGANIZATION_UNIT_Upd([FromBody] ORGANIZATION_UNIT_ENTITY input)
        {
            return organizationUnitAppService.ORGANIZATION_UNIT_Upd(input);
        }
        [HttpPost]
        public Task<CommonResult> ORGANIZATION_UNIT_Del(string id)
        {
            return organizationUnitAppService.ORGANIZATION_UNIT_Del(id);
        }
        [HttpPost]
        public Task<PagedResultDto<ORGANIZATION_UNIT_USER_ENTITY>> ORGANIZATION_UNIT_USER_Search([FromBody] ORGANIZATION_UNIT_USER_ENTITY input)
        {
            return organizationUnitAppService.ORGANIZATION_UNIT_USER_Search(input);
        }
        [HttpPost]
        public Task<CommonResult> ORGANIZATION_UNIT_Move(string id, string newParrentId)
        {
            return organizationUnitAppService.ORGANIZATION_UNIT_Move(id, newParrentId);
        }
    }
}
