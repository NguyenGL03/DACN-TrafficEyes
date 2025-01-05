using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.HangHoa;
using Common.gAMSPro.Intfs.HangHoa.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class HangHoaController : CoreAmsProControllerBase
    {
        readonly IHangHoaAppService hangHoaAppService;

        public HangHoaController(IHangHoaAppService hangHoaAppService)
        {
            this.hangHoaAppService = hangHoaAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_HANGHOA_ENTITY>> CM_HANGHOA_Search([FromBody] CM_HANGHOA_ENTITY input)
        {
            return await hangHoaAppService.CM_HANGHOA_Search(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_HANGHOA_Ins([FromBody] CM_HANGHOA_ENTITY input)
        {
            return await hangHoaAppService.CM_HANGHOA_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_HANGHOA_Upd([FromBody] CM_HANGHOA_ENTITY input)
        {
            return await hangHoaAppService.CM_HANGHOA_Upd(input);
        }

        [HttpGet]
        public async Task<CM_HANGHOA_ENTITY> CM_HANGHOA_ById(string id)
        {
            return await hangHoaAppService.CM_HANGHOA_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_HANGHOA_Del(string id, string userLogin)
        {
            return await hangHoaAppService.CM_HANGHOA_Del(id, userLogin);
        }

        [HttpPost]
        public async Task<CommonResult> CM_HANGHOA_App(string id, string currentUserName)
        {
            return await hangHoaAppService.CM_HANGHOA_App(id, currentUserName);
        }
    }
}
