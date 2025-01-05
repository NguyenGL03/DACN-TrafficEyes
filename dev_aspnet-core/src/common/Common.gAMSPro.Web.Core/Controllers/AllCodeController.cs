using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.AllCodes;
using Common.gAMSPro.AllCodes.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{

    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class AllCodeController : CoreAmsProControllerBase
    {
        readonly IAllCodeAppService allCodeAppService;

        public AllCodeController(IAllCodeAppService allCodeAppService)
        {
            this.allCodeAppService = allCodeAppService;
        }

        [HttpGet]
        public async Task<List<CM_ALLCODE_ENTITY>> CM_ALLCODE_GetByCDNAME(string cdName, string cdType)
        {
            return await allCodeAppService.CM_ALLCODE_GetByCDNAME(cdName, cdType);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_ALLCODE_ENTITY>> CM_ALLCODE_Search([FromBody] CM_ALLCODE_ENTITY input)
        {
            return await allCodeAppService.CM_ALLCODE_Search(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_ALLCODE_Ins([FromBody] CM_ALLCODE_ENTITY input)
        {
            return await allCodeAppService.CM_ALLCODE_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_ALLCODE_Upd([FromBody] CM_ALLCODE_ENTITY input)
        {
            return await allCodeAppService.CM_ALLCODE_Upd(input);
        }

        [HttpGet]
        public async Task<CM_ALLCODE_ENTITY> CM_ALLCODE_ById(string cdName, string cdType)
        {
            return await allCodeAppService.CM_ALLCODE_ById(cdName, cdType);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_ALLCODE_Del(int id)
        {
            return await allCodeAppService.CM_ALLCODE_Del(id);
        }

        //BAODNQ 1/7/2022
        [HttpGet]
        public async Task<CM_ALLCODE_ENTITY> CM_ALLCODE_ById_v2(string cdType, string cdName, string cdVal)
        {
            return await allCodeAppService.CM_ALLCODE_ById_v2(cdType, cdName, cdVal);
        }
    }
}
