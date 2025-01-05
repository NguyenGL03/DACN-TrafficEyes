using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Branchs.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.gAMSPro.Branchs
{
    /// <summary>
    /// <see cref="BranchAppService"/>
    /// </summary>
    public interface IBranchAppService : IApplicationService
    {
        Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_Search(CM_BRANCH_ENTITY input);
        Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_Dep_Search(CM_BRANCH_ENTITY input);
        Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_Dep_Search_v2(CM_BRANCH_ENTITY input);
        Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_GET_ALL(CM_BRANCH_ENTITY input);
        Task<FileDto> CM_BRANCH_ToExcel(CM_BRANCH_ENTITY input);
        Task<InsertResult> CM_BRANCH_Ins(CM_BRANCH_ENTITY input);
        Task<InsertResult> CM_BRANCH_Upd(CM_BRANCH_ENTITY input);
        Task<CommonResult> CM_BRANCH_Del(string id);
        Task<CommonResult> CM_BRANCH_App(string id, string currentUserName);
        Task<CM_BRANCH_ENTITY> CM_BRANCH_ById(string id);
        Task<CM_BRANCH_ENTITY> CM_BRANCH_ByCode(string id);
        Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_GetAllChild(string parentId);
        Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_Combobox();
        Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_USER_Combobox();
        Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_USER_Combobox_2();
        Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_GetFatherList(string regionId, string branchType);
        Task<CM_BRANCH_LEV_ENTITY> CM_BRANCH_Lev(string branchId);

        Task<CM_BRANCH_ENTITY> CM_BRANCH_Dep_ById(string branchId,string depId);
       

    }
}
