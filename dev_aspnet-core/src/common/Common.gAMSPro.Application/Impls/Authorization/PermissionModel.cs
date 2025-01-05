using Abp.Authorization;

namespace Common.gAMSPro.Authorization
{
    public class PermissionModel
    {
        public string MENU_PERMISSION { get; set; }
        public Permission Permission { get; set; }
    }
}
