using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Application.Intfs.CarTypes;
using Common.gAMSPro.Application.Intfs.CarTypes.Dto;
using Common.gAMSPro.Web.Controllers;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Core.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class CarTypeController : CoreAmsProControllerBase
    {
        readonly ICarTypeAppService carTypeAppService;

        public CarTypeController(ICarTypeAppService carTypeAppService)
        {
            this.carTypeAppService = carTypeAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_CAR_TYPE_ENTITY>> CM_CAR_TYPE_Search([FromBody] CM_CAR_TYPE_ENTITY input)
        {
            return await carTypeAppService.CM_CAR_TYPE_Search(input);
        }

        [HttpPost]
        public async Task<FileDto> CM_CAR_TYPE_ToExcel([FromBody] CM_CAR_TYPE_ENTITY input)
        {
            return await carTypeAppService.CM_CAR_TYPE_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_CAR_TYPE_Ins([FromBody] CM_CAR_TYPE_ENTITY input)
        {
            return await carTypeAppService.CM_CAR_TYPE_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_CAR_TYPE_Upd([FromBody] CM_CAR_TYPE_ENTITY input)
        {
            return await carTypeAppService.CM_CAR_TYPE_Upd(input);
        }

        [HttpPost]
        public async Task<List<CM_CAR_TYPE_ENTITY>> CM_CAR_TYPE_List([FromBody] CM_CAR_TYPE_ENTITY input)
        {
            return await carTypeAppService.CM_CAR_TYPE_List(input);
        }

        [HttpGet]
        public async Task<CM_CAR_TYPE_ENTITY> CM_CAR_TYPE_ById(string id)
        {
            return await carTypeAppService.CM_CAR_TYPE_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_CAR_TYPE_Del(string id)
        {
            return await carTypeAppService.CM_CAR_TYPE_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_CAR_TYPE_App(string id, string currentUserName)
        {
            return await carTypeAppService.CM_CAR_TYPE_App(id, currentUserName);
        }
    }
}
