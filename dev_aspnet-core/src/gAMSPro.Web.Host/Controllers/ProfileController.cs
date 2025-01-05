using Abp.AspNetCore.Mvc.Authorization;
using gAMSPro.Authorization.Users.Profile;
using gAMSPro.Graphics;
using gAMSPro.Storage;

namespace gAMSPro.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}