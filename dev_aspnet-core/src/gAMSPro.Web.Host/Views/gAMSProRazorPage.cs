using Abp.AspNetCore.Mvc.Views;

namespace gAMSPro.Web.Views
{
    public abstract class gAMSProRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected gAMSProRazorPage()
        {
            LocalizationSourceName = gAMSProConsts.LocalizationSourceName;
        }
    }
}
