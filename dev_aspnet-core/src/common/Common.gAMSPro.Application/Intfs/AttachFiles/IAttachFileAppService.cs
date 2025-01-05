using Common.gAMSPro.Intfs.AttachFiles.Dto;
using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.AttachFiles
{
    public interface IAttachFileAppService
    {
        Task<List<CM_ATTACH_FILE_ENTITY>> CM_ATTACH_FILE_By_RefMaster(string refMaster);
        Task<List<CM_ATTACH_FILE_MODEL>> CM_ATTACH_FILE_By_RefId(string[] refIds);
        Task<InsertResult> CM_ATTACH_FILE_Ins(CM_ATTACH_FILE attachFile, List<CM_ATTACH_FILE> childs = null, string ids = null);
        Task<InsertResult> CM_ATTACH_FILE_Ins_New(CM_ATTACH_FILE_INPUT attachFileModel);
        Task<InsertResult> CM_ATTACH_FILE_Upd(CM_ATTACH_FILE attachFile, List<CM_ATTACH_FILE> childs = null, string ids = null);
        Task<InsertResult> CM_ATTACH_FILE_Upd_New(CM_ATTACH_FILE_INPUT attachFileModel);
        Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_ByRefId(string ref_id);
        Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_ByAssCode(string ass_code);
        Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_GetFirstAsset(string assId, DateTime? INVENTORY_DT);
        Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_GetNearAsset(string assId, DateTime? INVENTORY_DT);
        Task<InsertResult> CM_IMAGE_Ins(List<CM_IMAGE_ENTITY> images, string refId);
        Task<InsertResult> CM_IMAGE_INVENTORY_Del(string refId);
    }
}
