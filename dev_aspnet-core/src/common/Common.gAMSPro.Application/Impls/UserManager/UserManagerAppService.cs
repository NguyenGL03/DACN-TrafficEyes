using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.CoreModule.Consts;
using Common.gAMSPro.Intfs.UserManager;
using Common.gAMSPro.Intfs.UserManager.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.CoreModule.Utils;

namespace documentMaster.gAMSPro.Impls.UserManager
{
    public class UserManagerAppService : gAMSProCoreAppServiceBase, IUserManagerAppService
    {
        public async Task<List<MD_USER_MANAGER_ENTITY>> MD_USER_MANAGER_GetByTLName(string tlname)
        {
            List<MD_USER_MANAGER_ENTITY> lstUserManager = await storeProcedureProvider.GetDataFromStoredProcedure<MD_USER_MANAGER_ENTITY>(CommonStoreProcedureConsts.MD_USER_MANAGER_GetByTLName, new
            {
                P_TLNAME = tlname
            });
            return lstUserManager;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, PermissionPageConsts.UserManager, gAMSProCorePermissions.Action.Create)]
        public async Task<IDictionary<string, object>> MD_USER_MANAGER_Ins(MD_USER_MANAGER_ENTITY input)
        {
            input.XmlData = XmlHelper.ToXmlFromList(input.BranchManager);
            input.XmlDataRegion = XmlHelper.ToXmlFromList(input.RegionManager);
            if (input.XmlDataRegion == "") input.XmlDataRegion = null;
            if (input.XmlData == "") input.XmlData = null;
            var result = await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.MD_USER_MANAGER_Ins, input);

            return result;
        }
    }
}
