using Abp.AspNetCore.Mvc.Controllers;
using Aspose.Words.MailMerging;
using Core.gAMSPro.Intfs.UserInfo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserInfoController: AbpController
    {
        private readonly IUserInfoAppService _userInfoAppService;
        public UserInfoController(IUserInfoAppService userInfoAppService)
        {
            _userInfoAppService = userInfoAppService;
        }
        [HttpPost]
        public async Task<IActionResult> GetUserInfo(string userName)
        {
            var result = await _userInfoAppService.GetUserInfo(userName);
            return Ok(result);
        }

        [HttpPost]
        public  IActionResult GetImageFromUserInfo(string userName)
        {
            var result = _userInfoAppService.GetImageFromUserInfo(userName);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult SaveImageFromBase64([FromBody] string base64)
        {
           _userInfoAppService.SaveImageFromBase64(base64, "testttttttt");
            return Ok();
        }

        [HttpPost]
        public IActionResult GetImages([FromBody] string userName)
        {
            var res = _userInfoAppService.GetImageFromUserInfo(userName);
            return Ok(res);
        }
    }
}
