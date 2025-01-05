using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.GoodsTypeReals.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using System.Threading.Tasks;

namespace Common.gAMSPro.GoodsTypeReals
{
    public interface IGoodsTypeRealAppService : IApplicationService
    {
        Task<PagedResultDto<CM_GOODSTYPE_REAL_ENTITY>> CM_GOODSTYPE_REAL_Search(CM_GOODSTYPE_REAL_ENTITY input);
        Task<FileDto> CM_GOODSTYPE_REAL_ToExcel(CM_GOODSTYPE_REAL_ENTITY input);
        Task<InsertResult> CM_GOODSTYPE_REAL_Ins(CM_GOODSTYPE_REAL_ENTITY input);
        Task<InsertResult> CM_GOODSTYPE_REAL_Upd(CM_GOODSTYPE_REAL_ENTITY input);
        Task<CommonResult> CM_GOODSTYPE_REAL_Del(string id);
        Task<CommonResult> CM_GOODSTYPE_REAL_App(string id, string currentUserName);
        Task<CM_GOODSTYPE_REAL_ENTITY> CM_GOODSTYPE_REAL_ById(string id);
    }
}
