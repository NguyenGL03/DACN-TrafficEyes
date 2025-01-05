using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace gAMSPro.Web.Public.Views
{
    public abstract class gAMSProRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected gAMSProRazorPage()
        {
            LocalizationSourceName = gAMSProConsts.LocalizationSourceName;
        }
    }
}
