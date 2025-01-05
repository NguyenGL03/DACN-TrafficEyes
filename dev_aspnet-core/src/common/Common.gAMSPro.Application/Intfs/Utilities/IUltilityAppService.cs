using Abp.Application.Services;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.RoxyFilemans.Dto;
using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;
using gAMSPro.Authorization.Users.Profile.Dto;
using Microsoft.AspNetCore.Http;

namespace Common.gAMSPro.Intfs.Utilities
{
    public interface IUltilityAppService : IApplicationService
    {
        Task<bool> IsApproveFunction(string functionId);
        string GetProcedureContent(string procedureName);
        CM_ATTACH_FILE_INPUT MoveTmpFile(CM_ATTACH_FILE_INPUT input);
        bool Delete_g_path(string g);
        bool DEL_FILE(string[] files);
        Task<UPLOAD_W_T_RESULT> UploadFile(string d, string fileName, string g, IFormFileCollection files, ISession session, bool isAjaxUpload, string settingName);
        [DisableValidation]
        Task UploadLogo(UpdateLogoPictureInput input);
    }
}
