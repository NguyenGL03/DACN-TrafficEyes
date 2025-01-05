using Abp.Configuration;
using Common.gAMSPro.Configuration;
using Common.gAMSPro.Intfs.RoxyFilemans.Dto;
using Common.gAMSPro.Intfs.Utilities;
using gAMSPro.Dto;
using gAMSPro.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UltilityController : CoreAmsProControllerBase
    {
        readonly IUltilityAppService ultilityAppService;
        readonly ITempFileCacheManager tempFileCacheManager;
        private readonly string _systemRootPath;
        ISettingManager settingManager;
        //readonly IFilesFtpHelper filesFtpHelper;
        private readonly IConfigurationRoot _appConfiguration;

        public UltilityController(IWebHostEnvironment env, IUltilityAppService ultilityAppService, ITempFileCacheManager tempFileCacheManager, ISettingManager settingManager
            //, IFilesFtpHelper filesFtpHelper
            )
        {
            this.ultilityAppService = ultilityAppService;
            this.tempFileCacheManager = tempFileCacheManager;
            this.settingManager = settingManager;
           // this.filesFtpHelper = filesFtpHelper;
            this._appConfiguration = env.GetAppConfiguration();
            this._systemRootPath = _appConfiguration["App:RootDirectory"];
            if (_systemRootPath.Length == 0)
            {
                _systemRootPath = env.ContentRootPath;
            }
        }

        [HttpGet]
        public async Task<bool> IsApproveFunct(string functionId)
        {
            return await ultilityAppService.IsApproveFunction(functionId);
        }

        [HttpGet]
        public IActionResult GetProcedureContent(string procedureName)
        {
            return Content(ultilityAppService.GetProcedureContent(procedureName), "text/plain");
        }

        [HttpGet]
        public async Task<string> GetLogFile()
        {
            return System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/App_Data/Logs/Logs.txt");
        }


        [HttpGet]
        public FileDto DownloadFile(string path)
        {
            try
            {
                //Get File name
                string fileName = path.Substring(path.LastIndexOf('/') + 1, path.Length - path.LastIndexOf('/') - 1);
                //Get file name origin
                int lastIndexOfDot = fileName.LastIndexOf('.');
                int lastIndexOf_ = fileName.LastIndexOf("_");
                string fileNameOrigin = fileName.Substring(0, lastIndexOf_) + fileName.Substring(lastIndexOfDot);
                //Read file
                byte[] bytes = System.IO.File.ReadAllBytes(this._systemRootPath + "/wwwroot/" + path);

                //Get mime Type
                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);

                FileDto file = new FileDto(fileName, contentType ?? "application/octet-stream");
                var url = "/download_files/temp";
                url = _systemRootPath + url;
                if (!Directory.Exists(url))
                {
                    Directory.CreateDirectory(url);
                }
                string dest = url + "/" + fileNameOrigin;

                using (FileStream fileStream = new FileStream(dest, FileMode.Create))
                {
                    // Ghi mảng byte[] vào tệp tin
                    fileStream.Write(bytes, 0, bytes.Length);
                }

                tempFileCacheManager.SetFile(file.FileToken, bytes);
                return file;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [HttpPost]
        public bool DEL_F(string[] files)
        {
            return ultilityAppService.DEL_FILE(files);
        }


        [HttpPost]
        public async Task<UPLOAD_W_T_RESULT> UploadLogo(string d, string fileName, string g, string settingName)
        {
            var files = HttpContext.Request.Form.Files;
            var session = HttpContext.Session;

            return await ultilityAppService.UploadFile(d, fileName, g, files, session, true, settingName);
        }

        [HttpGet]
        public async Task UploadSFtp(string content, string _fileName)
        {
            //var sftpResult = await filesFtpHelper.UploadContent_Server1(_fileName, content);
        }

    }
}
