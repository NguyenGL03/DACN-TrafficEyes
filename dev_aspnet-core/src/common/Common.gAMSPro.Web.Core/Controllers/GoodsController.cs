using Abp.Application.Services.Dto;
using Common.gAMSPro.Goodss;
using Common.gAMSPro.Goodss.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GoodsController : CoreAmsProControllerBase
    {
        readonly IGoodsAppService goodsAppService;

        public GoodsController(IGoodsAppService goodsAppService)
        {
            this.goodsAppService = goodsAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_GOODS_ENTITY>> CM_GOODS_Search([FromBody]CM_GOODS_ENTITY input)
        {
            return await goodsAppService.CM_GOODS_Search(input);
        }

        [HttpPost]
        public async Task<FileDto> CM_GOODS_ToExcel([FromBody]CM_GOODS_ENTITY input)
        {
            return await goodsAppService.CM_GOODS_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_GOODS_Ins([FromBody]CM_GOODS_ENTITY input)
        {
            return await goodsAppService.CM_GOODS_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_GOODS_Upd([FromBody]CM_GOODS_ENTITY input)
        {
            return await goodsAppService.CM_GOODS_Upd(input);
        }

        [HttpGet]
        public async Task<CM_GOODS_ENTITY> CM_GOODS_ById(string id)
        {
            return await goodsAppService.CM_GOODS_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_GOODS_Del(string id)
        {
            return await goodsAppService.CM_GOODS_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_GOODS_App(string id, string currentUserName)
        {
            return await goodsAppService.CM_GOODS_App(id, currentUserName);
        }
    }
}
