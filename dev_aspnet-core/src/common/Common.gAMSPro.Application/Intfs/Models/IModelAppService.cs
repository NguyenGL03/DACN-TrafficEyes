using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Application.Intfs.Models.Dto;
using Core.gAMSPro.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.gAMSPro.Application.Intfs.Models
{
    public interface IModelAppService: IApplicationService
    {
        Task<PagedResultDto<CM_MODEL_ENTITY>> CM_MODEL_Search(CM_MODEL_ENTITY input);
        Task<InsertResult> CM_MODEL_Ins(CM_MODEL_ENTITY input);
        Task<InsertResult> CM_MODEL_Upd(CM_MODEL_ENTITY input);
        Task<CommonResult> CM_MODEL_Del(string id);
        Task<CommonResult> CM_MODEL_App(string id, string currentUserName);
        Task<CM_MODEL_ENTITY> CM_MODEL_ById(string id);
    }
}
