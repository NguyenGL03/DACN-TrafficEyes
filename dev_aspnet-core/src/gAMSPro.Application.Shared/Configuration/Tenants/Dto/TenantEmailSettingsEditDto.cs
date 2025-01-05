using Abp.Auditing;
using gAMSPro.Configuration.Dto;

namespace gAMSPro.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}