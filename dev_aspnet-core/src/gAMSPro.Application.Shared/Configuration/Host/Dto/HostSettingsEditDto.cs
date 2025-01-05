using gAMSPro.Configuration.Dto; 
using System.ComponentModel.DataAnnotations;

namespace gAMSPro.Configuration.Host.Dto
{
    public class HostSettingsEditDto
    {
        [Required]
        public GeneralSettingsEditDto General { get; set; }

        [Required]
        public HostUserManagementSettingsEditDto UserManagement { get; set; }

        [Required]
        public EmailSettingsEditDto Email { get; set; }

        [Required]
        public TenantManagementSettingsEditDto TenantManagement { get; set; }

        [Required]
        public SecuritySettingsEditDto Security { get; set; }

        public HostBillingSettingsEditDto Billing { get; set; }

        public OtherSettingsEditDto OtherSettings { get; set; }

        public CommonSettingEditDto CommonSettingEditDto { get; set; }

        public ExternalLoginProviderSettingsEditDto ExternalLoginProviderSettings { get; set; }

        public LogoCompanySettingDto LogoCompanySettingDto { get; set; }

        public CommonThemeSettingsDto CommonThemeSettings { get; set; }

        public HostSettingsEditDto()
        {
            ExternalLoginProviderSettings = new ExternalLoginProviderSettingsEditDto();
        }
    }
}