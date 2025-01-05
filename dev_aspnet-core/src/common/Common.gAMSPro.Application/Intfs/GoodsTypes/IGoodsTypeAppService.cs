using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.GoodsTypes.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.GoodsTypes
{
    public interface IGoodsTypeAppService : IApplicationService
    {
        Task<PagedResultDto<CM_GOODSTYPE_ENTITY>> CM_GOODSTYPE_Search(CM_GOODSTYPE_ENTITY input);
        Task<InsertResult> CM_GOODSTYPE_Ins(CM_GOODSTYPE_ENTITY input);
        Task<InsertResult> CM_GOODSTYPE_Upd(CM_GOODSTYPE_ENTITY input);
        Task<List<CM_GOODSTYPE_ENTITY>> CM_GOODSTYPE_List();
        Task<CommonResult> CM_GOODSTYPE_Del(string id);
        Task<CommonResult> CM_GOODSTYPE_App(string id, string currentUserName);
        Task<CM_GOODSTYPE_ENTITY> CM_GOODSTYPE_ById(string id);
    }
}
