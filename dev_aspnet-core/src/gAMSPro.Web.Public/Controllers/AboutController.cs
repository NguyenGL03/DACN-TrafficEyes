using Microsoft.AspNetCore.Mvc;
using gAMSPro.Web.Controllers;

namespace gAMSPro.Web.Public.Controllers
{
    public class AboutController : gAMSProControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}