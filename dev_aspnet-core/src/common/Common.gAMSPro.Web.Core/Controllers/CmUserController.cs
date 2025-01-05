using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.Users;
using Common.gAMSPro.Intfs.Users.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class CmUserController : CoreAmsProControllerBase
    {
        private readonly ICmUserAppService cmUserAppService;

        public CmUserController(ICmUserAppService cmUserAppService)
        {
            this.cmUserAppService = cmUserAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<TLUSER_GETBY_BRANCHID_ENTITY>> TLUSER_GETBY_BRANCHID([FromBody] TLUSER_GETBY_BRANCHID_ENTITY tlUserFilter)
        {
            return await cmUserAppService.TLUSER_GETBY_BRANCHID(tlUserFilter);
        }

    }
}
