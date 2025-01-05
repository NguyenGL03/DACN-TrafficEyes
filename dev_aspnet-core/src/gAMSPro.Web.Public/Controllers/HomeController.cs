using Microsoft.AspNetCore.Mvc;
using gAMSPro.Web.Controllers;

namespace gAMSPro.Web.Public.Controllers
{
    public class HomeController : gAMSProControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}