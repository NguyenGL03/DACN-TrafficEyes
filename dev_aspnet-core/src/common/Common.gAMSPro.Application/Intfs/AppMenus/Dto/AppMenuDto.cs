using Abp.Application.Services.Dto;

namespace Common.gAMSPro.AppMenus.Dto
{
    public class AppMenuDto : EntityDto<string>
    {
        public string MenuId => Id.ToString();
        public string Name { get; set; }
        public string PermissionName { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public string Display { get; set; }
        public AppMenuDto[] Items { get; set; }
        public string ParentId { get; set; }
        public AppMenuDto Parent { get; set; }
        public bool External { get; set; }
        public bool RequiresAuthentication { get; set; }
        public string FeatureDependency { get; set; }
        public string[] Parameters { get; set; }
    }
}
