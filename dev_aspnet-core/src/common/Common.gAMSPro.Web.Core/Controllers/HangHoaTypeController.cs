using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.HangHoaType;
using Common.gAMSPro.Intfs.HangHoaType.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class HangHoaTypeController : CoreAmsProControllerBase
    {
        readonly IHangHoaTypeAppService hangHoaTypeAppService;

        public HangHoaTypeController(IHangHoaTypeAppService hangHoaTypeAppService)
        {
            this.hangHoaTypeAppService = hangHoaTypeAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_HANGHOA_TYPE_ENTITY>> CM_HANGHOA_TYPE_Search([FromBody] CM_HANGHOA_TYPE_ENTITY input)
        {
            return await hangHoaTypeAppService.CM_HANGHOA_TYPE_Search(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_HANGHOA_TYPE_Ins([FromBody] CM_HANGHOA_TYPE_ENTITY input)
        {
            return await hangHoaTypeAppService.CM_HANGHOA_TYPE_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_HANGHOA_TYPE_Upd([FromBody] CM_HANGHOA_TYPE_ENTITY input)
        {
            return await hangHoaTypeAppService.CM_HANGHOA_TYPE_Upd(input);
        }

        [HttpGet]
        public async Task<CM_HANGHOA_TYPE_ENTITY> CM_HANGHOA_TYPE_ById(string id)
        {
            return await hangHoaTypeAppService.CM_HANGHOA_TYPE_ById(id);
        }

        [HttpGet]
        public async Task<List<CM_HANGHOA_TYPE_ENTITY>> CM_HANGHOA_TYPE_GetAll()
        {
            return await hangHoaTypeAppService.CM_HANGHOA_TYPE_GetAll();
        }

        [HttpDelete]
        public async Task<CommonResult> CM_HANGHOA_TYPE_Del(string id)
        {
            return await hangHoaTypeAppService.CM_HANGHOA_TYPE_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_HANGHOA_TYPE_App(string id, string currentUserName)
        {
            return await hangHoaTypeAppService.CM_HANGHOA_TYPE_App(id, currentUserName);
        }



    }
}
