using Abp.Authorization;
using Common.gAMSPro.AppMenus.Dto;
using gAMSPro.Authorization.Permissions.Dto;
using System.Collections.Generic;
using System.Linq;

namespace gAMSPro.Authorization.Roles
{
    public class RoleCommon : IRoleCommon
    {
        private readonly IPermissionManager permissionManager;
        public RoleCommon(IPermissionManager permissionManager)
        {
            this.permissionManager = permissionManager;
        }
        public List<FlatPermissionDto> MergeMenuPermissionWithPermissionRole(List<FlatPermissionDto> permissionFlatDto, List<AppMenuDto> appMenu, List<string> actions)
        {
            var permission = permissionManager.GetAllPermissions();
            var root = GetRootNodeAction(permission.Where(x => x.Parent == null).ToArray(), actions);

            var appDist = appMenu.ToDictionary(x => x.Id);

            foreach (var item in appMenu)
            {
                var updatePermissionWithParent = permissionFlatDto.Where(x => x.Name == item.PermissionName).FirstOrDefault();
                if (updatePermissionWithParent != null)
                {
                    if (item.ParentId == null || !appDist.ContainsKey(item.ParentId))
                    {
                        updatePermissionWithParent.ParentName = root.Name;
                    }
                    else
                    {
                        var parent = appDist[item.ParentId];
                        updatePermissionWithParent.ParentName = parent.PermissionName;
                    }
                    updatePermissionWithParent.DisplayName = item.Display;
                    updatePermissionWithParent.IsRootAction = true;
                }
            }

            permissionFlatDto.Where(x => string.IsNullOrEmpty(x.ParentName) && x.Name == root.Name).FirstOrDefault().IsRootAction = true;
            return permissionFlatDto;
        }
        private Permission GetRootNodeAction(Permission[] root, List<string> actions)
        {
            foreach (var item in root)
            {
                var tmp = IsParentNode(actions, item, false);
                if (IsParentNode(actions, item, false) == true)
                {

                    return item;
                }
            }
            return null;
        }
        private bool IsParentNode(List<string> actions, Permission permissions, bool isParentNode)
        {
            if (actions.Exists(x => permissions.Name.Split('.')[permissions.Name.Split('.').Length - 1] == x))
            {
                isParentNode = true;
            }
            if (!isParentNode)
            {
                foreach (var item in permissions.Children)
                {
                    return IsParentNode(actions, item, isParentNode);
                }
            }

            return isParentNode;
        }

    }
}
