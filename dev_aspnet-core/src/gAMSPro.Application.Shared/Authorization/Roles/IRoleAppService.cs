using Abp.Application.Services;
using Abp.Application.Services.Dto;
using gAMSPro.Authorization.Roles.Dto;
using System.Threading.Tasks;

namespace GSOFTcore.gAMSPro.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// </summary>
    public interface IRoleAppService : IApplicationService
    {
        Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input);

        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input);

        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);

        Task<string> DeleteRole(EntityDto input);
    }
}