using Abp.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.AttachFiles;
using Common.gAMSPro.Intfs.AttachFiles.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;
using Core.gAMSPro.CoreModule.Utils;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Impls.AttachFiles
{
    [AbpAuthorize]
    public class AttachFileAppService : gAMSProCoreAppServiceBase, IAttachFileAppService
    {
        private readonly string directionImport;

        public AttachFileAppService()
        {
            directionImport = appConfiguration["App:ImportImageRootAddress"];
        }
        public async Task<List<CM_ATTACH_FILE_ENTITY>> CM_ATTACH_FILE_By_RefMaster(string refMaster)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_ATTACH_FILE_ENTITY>(CommonStoreProcedureConsts.CM_ATTACH_FILE_BY_REFMASTER, new { REF_MASTER = refMaster });
        }

        public async Task<InsertResult> CM_ATTACH_FILE_Ins(CM_ATTACH_FILE attachFile, List<CM_ATTACH_FILE> childs = null, string ids = null)
        {
            return await base.CM_ATTACH_FILE_Ins(attachFile, childs, ids);
        }

        public async Task<InsertResult> CM_ATTACH_FILE_Ins_New(CM_ATTACH_FILE_INPUT attachFileModel)
        {
            return await base.CM_ATTACH_FILE_Ins_New(attachFileModel.AttachFile, attachFileModel.Childs, attachFileModel.Ids);
        }

        public async Task<InsertResult> CM_ATTACH_FILE_Upd(CM_ATTACH_FILE attachFile, List<CM_ATTACH_FILE> childs = null, string ids = null)
        {
            return await base.CM_ATTACH_FILE_Upd(attachFile, childs, ids);
        }

        public async Task<InsertResult> CM_ATTACH_FILE_Upd_New(CM_ATTACH_FILE_INPUT attachFileModel)
        {
            return await base.CM_ATTACH_FILE_Upd_New(attachFileModel.AttachFile, attachFileModel.Childs, attachFileModel.Ids);
        }

        public async Task<List<CM_ATTACH_FILE_MODEL>> CM_ATTACH_FILE_By_RefId(string[] refIds)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_ATTACH_FILE_MODEL>(CommonStoreProcedureConsts.CM_ATTACH_FILE_BY_REFID, new
            {
                REF_ID = string.Join(",", refIds.Select(x => "'" + x + "'"))
            });
        }
        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_ByRefId(string refId)
        {
            //CommonStoreProcedureConsts.CM_IMAGE_ByRefId
            //CommonStoreProcedureConsts.CM_IMAGE_ByRefId
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_IMAGE_ENTITY>("CM_IMAGE_ByRefId", new { REF_ID = refId });
            if (result != null)
            {
                foreach (var item in result)
                {
                    //byte[] imageArray = System.IO.File.ReadAllBytes(directionImport + item.PATH);
                    //item.BASE64 = Convert.ToBase64String(imageArray);
                    item.BASE64 = await GetBase64StringAsync(directionImport + item.PATH);

                }

            }
            return result;
        }

        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_ByAssCode(string assCode)
        {
            //CommonStoreProcedureConsts.CM_IMAGE_ByRefId
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_IMAGE_ENTITY>("CM_IMAGE_ByAssCode", new { ASS_CODE = assCode });
            foreach (var item in result)
            {
                //byte[] imageArray = System.IO.File.ReadAllBytes(directionImport + item.PATH);
                //item.BASE64 = Convert.ToBase64String(imageArray);
                item.BASE64 = await GetBase64StringAsync(directionImport + item.PATH);

            }
            return result;
        }

        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_GetNearAsset(string assId, DateTime? INVENTORY_DT)
        {
            //CommonStoreProcedureConsts.CM_IMAGE_ByRefId
            //return await storeProcedureProvider.GetDataFromStoredProcedure<CM_IMAGE_ENTITY>("CM_IMAGE_GetNearAsset", new { ASSET_ID = assId, INVENTORY_DT = INVENTORY_DT });

            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_IMAGE_ENTITY>("CM_IMAGE_GetNearAsset", new { ASSET_ID = assId, INVENTORY_DT = INVENTORY_DT });
            if (result != null)
            {
                foreach (var item in result)
                {
                    item.BASE64 = await GetBase64StringAsync(directionImport + item.PATH);
                }

            }
            return result;
        }


        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_GetFirstAsset(string assId, DateTime? INVENTORY_DT)
        {
            //CommonStoreProcedureConsts.CM_IMAGE_ByRefId
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_IMAGE_ENTITY>("CM_IMAGE_GetFirstAsset", new { ASSET_ID = assId, INVENTORY_DT = INVENTORY_DT });
            if (result != null)
            {
                foreach (var item in result)
                {
                    item.BASE64 = await GetBase64StringAsync(directionImport + item.PATH);
                }

            }
            return result;
        }

        public async Task<InsertResult> CM_IMAGE_Ins(List<CM_IMAGE_ENTITY> images, string refId)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>("CM_IMAGE_Ins", new
                {
                    INVENTDT_ID = refId,
                    IMAGES = images.ToXmlFromList()
                })).FirstOrDefault();
            return result;
        }
        public async Task<InsertResult> CM_IMAGE_INVENTORY_Del(string refId)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>("CM_IMAGE_INVENTORY_Del", new
                {
                    INVENTDT_ID = refId
                })).FirstOrDefault();
            return result;
        }
    }
}
