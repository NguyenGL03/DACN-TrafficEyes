using Microsoft.AspNetCore.Antiforgery;

namespace gAMSPro.Web.Controllers
{
    public class AntiForgeryController : gAMSProControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
