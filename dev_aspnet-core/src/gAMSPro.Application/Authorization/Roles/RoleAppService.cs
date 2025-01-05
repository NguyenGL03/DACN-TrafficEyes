using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Zero.Configuration;
using Common.gAMSPro.AppMenus;
using Common.gAMSPro.Core.Authorization;
using gAMSPro;
using gAMSPro.Authorization;
using gAMSPro.Authorization.Permissions.Dto;
using gAMSPro.Authorization.Roles;
using gAMSPro.Authorization.Roles.Dto;
using GSOFTcore.gAMSPro.Authorization.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GSOFTcore.gAMSPro.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// </summary>
    /// 
    public class RoleAppService : gAMSProAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly IAppMenuAppService _appMenuAppService;
        private readonly IRoleCommon _roleCommon;
        public RoleAppService(
            RoleManager roleManager,
            IRoleManagementConfig roleManagementConfig,
            IAppMenuAppService appMenuAppService,
            IRoleCommon roleCommon
          )
        {
            this._appMenuAppService = appMenuAppService;
            _roleManager = roleManager;
            _roleManagementConfig = roleManagementConfig;
            this._roleCommon = roleCommon;
        }

        public async Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input)
        {
            var query = _roleManager.Roles;

            if (!string.IsNullOrEmpty(input.Permission))
            {
                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                        r => r.GrantAllPermissionsByDefault &&
                             r.Side == AbpSession.MultiTenancySide
                    ).Select(r => r.RoleName).ToList();

                query = query.Where(r =>
                    r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted) ||
                    staticRoleNames.Contains(r.Name)
                );
            }
            if (!input.RoleName.IsNullOrWhiteSpace())
            {
                query = query.Where(x => x.DisplayName.ToLower().Contains(input.RoleName.ToLower()));
            }

            var roles = await query.OrderBy(x => x.DisplayName).ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        public GetRoleForEditOutput GetAllRole()
        { 
            var actions = typeof(gAMSProCorePermissions.Action)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(f => f.GetValue(null)).Select(x => x.ToString()).OrderBy(x => x.ToString()).ToList();

            var permissions = ObjectMapper.Map<List<FlatPermissionDto>>(PermissionManager.GetAllPermissions()).OrderBy(p => p.DisplayName).ToList();

            var permissionsAppMenu = _appMenuAppService.GetAllMenus();

            permissions = _roleCommon.MergeMenuPermissionWithPermissionRole(permissions, permissionsAppMenu, actions);

            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto = new RoleEditDto();

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = permissions,
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList(),
                Actions = actions,
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            var permissionsAppMenu = _appMenuAppService.GetAllMenus();
            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto;

            if (input.Id.HasValue)
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = ObjectMapper.Map<RoleEditDto>(role);
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList(),
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Role.Id.HasValue)
            {
                await UpdateRoleAsync(input);
            }
            else
            {
                await CreateRoleAsync(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Delete)]
        public async Task<string> DeleteRole(EntityDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            var users = await UserManager.GetUsersInRoleAsync(role.Name);
            if (users.Count > 0)
            {
                return "HasUserInRole";
            }
            foreach (var user in users)
            {
                CheckErrors(await UserManager.RemoveFromRoleAsync(user, role.Name));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
            return "";
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Edit)]
        protected virtual async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            Debug.Assert(input.Role.Id != null, "input.Role.Id should be set.");

            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);
            role.DisplayName = input.Role.DisplayName;
            role.IsDefault = input.Role.IsDefault;
            role.Desc = input.Role.Desc;
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        protected virtual async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = new Role(AbpSession.TenantId, input.Role.DisplayName) { IsDefault = input.Role.IsDefault, Desc = input.Role.Desc };

            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(grantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}
