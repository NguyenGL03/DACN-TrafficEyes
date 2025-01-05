using Common.gAMSPro.Intfs.RoxyFilemans;
using Common.gAMSPro.Intfs.RoxyFilemans.Dto;
using gAMSPro.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GSOFTcore.gAMSPro.Web.Controllers
{
    public class RoxyFilemanController : gAMSProControllerBase
    {
        private readonly IRoxyFilemanAppService roxyFilemanAppService;

        public RoxyFilemanController(IRoxyFilemanAppService roxyFilemanAppService)
        {
            this.roxyFilemanAppService = roxyFilemanAppService;
        }

        [AllowAnonymous, Produces("text/plain"), ActionName("")]
        public string Get() { return "RoxyFileman - access to API requires Authorisation"; }

        #region API Actions
        [HttpPost]
        public async Task<IActionResult> DIRLIST(string type, string functionFolder)
        {
            var result = await roxyFilemanAppService.DIRLIST(type, functionFolder, HttpContext.Session);
            if (result.SusscessCode == SuccessCode.Success)
            {
                return Content(result.Result, "application/json");
            }
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> FILESLIST(string d, string type)
        {
            var result = await roxyFilemanAppService.FILESLIST(d, type, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> COPYDIR(string d, string n)
        {
            var result = await roxyFilemanAppService.COPYDIR(d, n, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> COPYFILE(string f, string n)
        {
            var result = await roxyFilemanAppService.COPYFILE(f, n, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> CREATEDIR(string d, string n)
        {
            var result = await roxyFilemanAppService.CREATEDIR(d, n, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> DELETEDIR(string d)
        {
            var result = await roxyFilemanAppService.DELETEDIR(d, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> DELETEFILE(string f)
        {
            var result = await roxyFilemanAppService.DELETEFILE(f, HttpContext.Session);
            return Content(result.Result);
        }

        public async Task<ActionResult> DOWNLOAD(string f)
        {
            var result = await roxyFilemanAppService.DOWNLOAD(f, HttpContext.Session);
            switch (result.SusscessCode)
            {
                case SuccessCode.Success:
                    return PhysicalFile(result.PhysicsPath, result.ContentType, result.FileName);
                case SuccessCode.NotFound:
                    return NotFound();
            }
            return Json(result.Result);
        }

        public async Task<ActionResult> DOWNLOADDIR(string d)
        {
            var result = await roxyFilemanAppService.DOWNLOADDIR(d);
            switch (result.Result)
            {
                case SuccessCode.Success:
                    return PhysicalFile(result.PhysicsPath, result.ContentType, result.FileName);
                case SuccessCode.NotFound:
                    return NotFound();
            }
            return Json(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> MOVEDIR(string d, string n)
        {
            var result = await roxyFilemanAppService.MOVEDIR(d, n, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> MOVEFILE(string f, string n)
        {
            var result = await roxyFilemanAppService.MOVEFILE(f, n, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> RENAMEDIR(string d, string n)
        {
            var result = await roxyFilemanAppService.RENAMEDIR(d, n, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> RENAMEFILE(string f, string n)
        {
            var result = await roxyFilemanAppService.RENAMEFILE(f, n, HttpContext.Session);
            return Content(result.Result);
        }

        [HttpPost]
        public async Task<string> UPLOAD(string d)
        {
            var files = HttpContext.Request.Form.Files;
            var session = HttpContext.Session;
            try
            {
                var result = await roxyFilemanAppService.UPLOAD(d, files, session, IsAjaxUpload());
                return result.Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<UPLOAD_W_T_RESULT> UPLOAD_W_T(string d, string g)
        {
            var files = HttpContext.Request.Form.Files;
            var session = HttpContext.Session;

            var result = await roxyFilemanAppService.UPLOAD_W_T(d, g, files, session, IsAjaxUpload());
            return result;

        }
        [HttpPost]
        public async Task<string> UPLOAD_W_T_HTML(string d, string g)
        {
            var files = HttpContext.Request.Form.Files;
            var result = await roxyFilemanAppService.UPLOAD_W_T_HTML(files);
            return result;

        }

        [HttpPost]
        public async Task<UPLOAD_W_T_RESULT> UPLOAD_SYSTEM(string d)
        {
            var files = HttpContext.Request.Form.Files;
            var session = HttpContext.Session;

            var result = await roxyFilemanAppService.UPLOAD_SYSTEM(d, files, session, IsAjaxUpload());
            return result;
        }

        #endregion

        private bool IsAjaxUpload()
        {
            return (!string.IsNullOrEmpty(HttpContext.Request.Query["method"]) && HttpContext.Request.Query["method"].ToString() == "ajax");
        }
    }
}