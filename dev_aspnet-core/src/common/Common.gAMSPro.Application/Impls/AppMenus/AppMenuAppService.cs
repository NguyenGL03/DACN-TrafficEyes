using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Common.gAMSPro.AppMenus.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Intfs.AsposeSample;
using Common.gAMSPro.Models;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Caching;
using gAMSPro.Consts;
using gAMSPro.Core.Authorization.Permissions;
using gAMSPro.Helper;
using GSOFTcore.gAMSPro.Authorization.Permissions;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Common.gAMSPro.AppMenus
{
    [AbpAuthorize]
    public class AppMenuAppService : gAMSProCoreAppServiceBase, IAppMenuAppService
    {
        private IRepository<TL_MENU> appMenuRepository;
        private IPermissionAppHelper permissionAppHelper;
        private ICachingAppService cachingAppService;
        private IDetailLoggerHelper detailLoggerHelper;


        public AppMenuAppService(
            IRepository<TL_MENU> appMenuRepository,
            IPermissionAppHelper permissionAppHelper,
            IAsposeAppService asposeAppService,
            ICachingAppService cachingAppService,
            IMemoryCache memoryCache,
            IDetailLoggerHelper detailLoggerHelper
        )
        {
            this.appMenuRepository = appMenuRepository;
            this.permissionAppHelper = permissionAppHelper;
            this.cachingAppService = cachingAppService;
            this.detailLoggerHelper = detailLoggerHelper;
        }

        #region Public Method

        public List<AppMenuDto> GetAllMenus()
        {
            var key = detailLoggerHelper.StartLog("api/AppMenu/GetAllMenus");

            var permissions = PermissionManager.GetAllPermissions().Select(x => x.Name).ToList();
            List<AppMenuDto> appMenus = appMenuRepository
                   .GetAll()
                   .Where(x => x.RECORD_STATUS == RecordStatusConsts.Active && x.AUTH_STATUS == ApproveStatusConsts.Approve)
                   .OrderBy(x => x.MENU_ORDER)
                   .Select(x => new AppMenuDto()
                   {
                       Id = x.MENU_ID,
                       Icon = x.MENU_ICON,
                       Name = x.MENU_NAME,
                       PermissionName = x.MENU_PERMISSION,
                       Route = x.MENU_LINK,
                       Display = x.MENU_NAME,
                       ParentId = x.MENU_PARENT
                   })
                   .ToList();


            appMenus = appMenus.Where(x => permissions.Contains(x.PermissionName)).ToList();
            detailLoggerHelper.EndLog(key);

            return appMenus;
        }

        public async Task<PagedResultDto<TL_MENU_ENTITY>> TL_MENU_Search_By_RoleID(TL_MENU_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<TL_MENU_ENTITY>(CommonStoreProcedureConsts.TL_MENU_SEARCH_By_RoleID, input);
        }
        public async Task<List<TL_MENU_ENTITY>> TL_MENU_Get_Menu_Parents()
        {
            var query = "SELECT A.* FROM TL_MENU A WHERE A.MENU_PARENT IS NULL";

            var result = await storeProcedureProvider.GetDataQuery<TL_MENU_ENTITY>(query);

            return result;
        }

        public async Task<PagedResultDto<TL_MENU_ENTITY>> TL_MENU_Search(TL_MENU_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<TL_MENU_ENTITY>(CommonStoreProcedureConsts.TL_MENU_SEARCH, input);
        }

        //[CoreAuthorize(gAMSProCorePermissions.Prefix.Administration, gAMSProCorePermissions.Page.Menu, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> TL_MENU_Ins(TL_MENU_ENTITY input)
        {
            input.MENU_PERMISSION = input.PREFIX + "." + input.MENU_NAME_EL;

            SetAuditForInsert(input);

            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.TL_MENU_INS, input)).FirstOrDefault();

            cachingAppService.ClearPermissionAndRole();

            return result;
        }

        //[CoreAuthorize(gAMSProCorePermissions.Prefix.Administration, gAMSProCorePermissions.Page.Menu, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> TL_MENU_Upd(TL_MENU_ENTITY input)
        {
            if (input.MENU_NAME_EL != gAMSProCorePermissions.Page.Menu)
            {
                RemovePermission(input.MENU_PERMISSION);
            }
            input.MENU_PERMISSION = input.PREFIX + "." + input.MENU_NAME_EL;

            SetAuditForUpdate(input);

            if (input.MENU_NAME_EL == gAMSProCorePermissions.Page.Menu)
            {
                input.AUTH_STATUS = ApproveStatusConsts.Approve;
            }

            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.TL_MENU_UPD, input)).FirstOrDefault();

            cachingAppService.ClearPermissionAndRole();

            return result;
        }

        public async Task<TL_MENU_ENTITY> TL_MENU_ById(int id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<TL_MENU_ENTITY>(CommonStoreProcedureConsts.TL_MENU_BYID, new
            {
                MENU_ID = id
            })).FirstOrDefault();

            return model;
        }

        //[CoreAuthorize(gAMSProCorePermissions.Prefix.Administration, gAMSProCorePermissions.Page.Menu, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> TL_MENU_App(int id, string currentUserName)
        {
            var item = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.TL_MENU_APP, new
                {
                    P_MENU_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
            cachingAppService.ClearPermissionAndRole();

            //await CreatePermission((await TL_MENU_ById(id)).MENU_NAME_EL);
            return item;
        }

        //[CoreAuthorize(gAMSProCorePermissions.Prefix.Administration, gAMSProCorePermissions.Page.Menu, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> TL_MENU_Del(int id)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.TL_MENU_DEL, new
                {
                    MENU_ID = id
                })).FirstOrDefault();

            cachingAppService.ClearPermissionAndRole();

            return result;

        }
        #endregion

        #region Private Method 
        private async Task CreatePermission(string menuName)
        {
            var permissionName = gAMSProCorePermissions.Prefix.Administration + "." + menuName;
            if (PermissionManager.GetPermissionOrNull(permissionName) == null)
            {
                var permission = await permissionAppHelper.CreatePermission(permissionName, menuName);
                var actions = typeof(gAMSProCorePermissions.Action)
                  .GetFields(BindingFlags.Public | BindingFlags.Static)
                  .Select(f => f.GetValue(null)).ToList();

                foreach (var action in actions)
                {
                    var permissionActionName = permissionName + "." + action;
                    var cPermission = permission.CreateChildPermission(permissionActionName, Lo(action.ToString()));
                    await permissionAppHelper.AddToDictPermission(cPermission);
                }
            }
        }

        private void RemovePermission(string menuPermission)
        {
            var permission = PermissionManager.GetPermissionOrNull(menuPermission);
            if (permission != null)
            {
                permissionAppHelper.RemovePermission(menuPermission);

                var actions = typeof(gAMSProCorePermissions.Action)
                  .GetFields(BindingFlags.Public | BindingFlags.Static)
                  .Select(f => f.GetValue(null)).ToList();

                foreach (var action in actions)
                {
                    var permissionActionName = menuPermission + "." + action;
                    var removeMethod = PermissionManager.GetType().GetMethod(PermissionConst.RemovePermission);
                    removeMethod.Invoke(PermissionManager, new object[] { permissionActionName });

                    permission.RemoveChildPermission(permissionActionName);
                }
            }
        }

        private static ILocalizableString Lo(string name)
        {
            return new LocalizableString(name, gAMSProConsts.LocalizationSourceName);
        }

        #endregion


    }
}
