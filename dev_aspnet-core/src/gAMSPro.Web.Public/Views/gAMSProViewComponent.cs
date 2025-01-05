using Abp.AspNetCore.Mvc.ViewComponents;

namespace gAMSPro.Web.Public.Views
{
    public abstract class gAMSProViewComponent : AbpViewComponent
    {
        protected gAMSProViewComponent()
        {
            LocalizationSourceName = gAMSProConsts.LocalizationSourceName;
        }
    }
}