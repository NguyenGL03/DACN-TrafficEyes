using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.UserManager;
using Common.gAMSPro.Intfs.UserManager.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class UserManagerController : CoreAmsProControllerBase
    {
        private readonly IUserManagerAppService _UserManagerService;

        public UserManagerController(IUserManagerAppService userManagerAppService)
        {
            this._UserManagerService = userManagerAppService;
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> MD_USER_MANAGER_Ins([FromBody] MD_USER_MANAGER_ENTITY input)
        {
            return await _UserManagerService.MD_USER_MANAGER_Ins(input);

        }

        [HttpGet]
        public async Task<List<MD_USER_MANAGER_ENTITY>> MD_USER_MANAGER_GetByTLName(string tlname)
        {
            return await _UserManagerService.MD_USER_MANAGER_GetByTLName(tlname);
        }
    }
}
