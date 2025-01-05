using Abp.AspNetZeroCore.Net;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.UI;
using Common.gAMSPro.Configuration;
using Common.gAMSPro.Intfs.AttachFiles;
using Common.gAMSPro.Intfs.AttachFiles.Dto;
using Common.gAMSPro.Intfs.Utilities;
using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using gAMSPro.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class AttachFileController : CoreAmsProControllerBase
    {
        IAttachFileAppService attachFileAppService;
        readonly IUltilityAppService ultilityAppService;
        //private readonly IFilesFtpHelper filesFtpHelper;
        private readonly IConfigurationRoot appConfiguration;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private readonly string directionImport;

        public AttachFileController(
            ITempFileCacheManager _tempFileCacheManager,
            IAttachFileAppService attachFileAppService,
            IWebHostEnvironment env,
            IUltilityAppService ultilityAppService
            //IFilesFtpHelper filesFtpHelper
            )
        {
            this.attachFileAppService = attachFileAppService;
            this.ultilityAppService = ultilityAppService;
            //this.filesFtpHelper = filesFtpHelper;
            appConfiguration = env.GetAppConfiguration();
            directionImport = appConfiguration["App:ImportImageRootAddress"];
            this._tempFileCacheManager = _tempFileCacheManager;

        }

        [HttpPost]
        public async Task<List<CM_ATTACH_FILE_ENTITY>> CM_ATTACH_FILE_By_RefMaster(string refMaster)
        {
            return await attachFileAppService.CM_ATTACH_FILE_By_RefMaster(refMaster);
        }

        [HttpPost]
        public async Task<List<CM_ATTACH_FILE_MODEL>> CM_ATTACH_FILE_By_RefId(string[] refIds)
        {
            return await attachFileAppService.CM_ATTACH_FILE_By_RefId(refIds);
        }

        [HttpPost]
        public async Task<InsertResult> CM_ATTACH_FILE_Ins([FromBody] CM_ATTACH_FILE_INPUT attachFileModel)
        {
            return await attachFileAppService.CM_ATTACH_FILE_Ins_New(attachFileModel);
        }

        [HttpPost]
        public async Task<InsertResult> CM_ATTACH_FILE_Upd([FromBody] CM_ATTACH_FILE_INPUT attachFileModel)
        {
            return await attachFileAppService.CM_ATTACH_FILE_Upd_New(attachFileModel);
        }

        [HttpPost]
        public CM_ATTACH_FILE_ENTITY TestFile()
        {
            return null;
        }

        [HttpPost]
        public CM_ATTACH_FILE_INPUT MoveTmpFile([FromBody] CM_ATTACH_FILE_INPUT input)
        {
            return ultilityAppService.MoveTmpFile(input);
        }

        [HttpPost]
        public IActionResult Delete_g_path(string g)
        {
            if (!ultilityAppService.Delete_g_path(g))
            {
                return Forbid();
            }
            return Ok();
        }

        private bool checkFileImage(string s)
        {
            s = s.ToUpper();
            if (s.Contains(".JPG") || s.Contains(".JPEG") || s.Contains(".JPE") || s.Contains(".BMP") || s.Contains(".GIF") || s.Contains(".PNG") || s.Contains(".JS") || s.Contains(".HTML"))
                return true;
            return false;
        }
        private bool checkFileOffice(string s)
        {
            s = s.ToUpper();
            if (s.Contains(".DO") || s.Contains(".PD") || s.Contains(".PPT") || s.Contains("XLS") || s.Contains(".JS") || s.Contains(".HTML"))
                return true;
            return false;
        }
        [HttpPost]
        public async Task<InsertResult> UploadImageFiles([FromBody] List<CM_IMAGE_ENTITY> images, string refId, string folderName)
        {
            try
            {
                foreach (var file in images)
                {
                    if (!file.PATH.IsNullOrEmpty())
                    {
                        file.PATH = slipUrl(file.PATH);
                    }
                }

                if (!Directory.Exists(directionImport + "\\" + folderName))
                {
                    Directory.CreateDirectory(directionImport + "\\" + folderName);
                }
                images.ForEach(async image =>
                {
                    if (image.PATH == null && image.BASE64 != null)
                    {

                        using (var imageFile = new FileStream(directionImport + "\\" + folderName + "\\" + image.FILE_NAME, FileMode.Create))
                        {
                            image.BASE64 = image.BASE64.Substring(image.BASE64.IndexOf(",") + 1);

                            var bytes = Convert.FromBase64String(image.BASE64);

                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            image.PATH = "\\" + folderName + "\\" + image.FILE_NAME;
                        }


                        //await filesFtpHelper.UploadImage_Server3(image.FILE_NAME, folderName);
                    }
                });
                return await attachFileAppService.CM_IMAGE_Ins(images, refId);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }

        }
        [HttpPost]
        public async Task<InsertResult> UploadImageFilesInventory([FromBody] List<CM_IMAGE_ENTITY> images, string refId, string folderName)
        {
            try
            {
                //string directImage = "./wwwroot/IMAGE_IMPORT/";
                //if (!Directory.Exists("./wwwroot/IMAGE_IMPORT"))
                //{
                //    System.IO.Directory.CreateDirectory("./wwwroot/IMAGE_IMPORT");

                //}
                //if (!Directory.Exists("./wwwroot/IMAGE_IMPORT/" + folderName))
                //{
                //    System.IO.Directory.CreateDirectory("./wwwroot/IMAGE_IMPORT/" + folderName);

                //}
                //if (!Directory.Exists("./wwwroot/IMAGE_IMPORT"))
                //{
                //    Directory.CreateDirectory("./wwwroot/IMAGE_IMPORT");
                //}
                // TIENLEE 04-06-2022 Vá lỗi Directory Traversal
                Boolean check = false;
                foreach (var file in images)
                {
                    if (checkFileImage(file.FILE_NAME))
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        break;
                    }
                }
                if (!check)
                {
                    return await attachFileAppService.CM_IMAGE_INVENTORY_Del(refId);
                }
                else
                {
                    foreach (var file in images)
                    {
                        if (!file.PATH.IsNullOrEmpty())
                        {
                            //file.PATH = "/" + file.PATH.Replace(@"/", @"").Replace(@".", @"");
                            file.PATH = slipUrl(file.PATH);
                        }
                    }

                    if (!Directory.Exists(directionImport + "\\" + folderName))
                    {
                        Directory.CreateDirectory(directionImport + "\\" + folderName);
                    }
                    images.ForEach(async image =>
                    {
                        if (image.PATH == null && image.BASE64 != null)
                        {

                            using (var imageFile = new FileStream(directionImport + "\\" + folderName + "\\" + image.FILE_NAME, FileMode.Create))
                            {
                                image.BASE64 = image.BASE64.Substring(image.BASE64.IndexOf(",") + 1);

                                var bytes = Convert.FromBase64String(image.BASE64);

                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                                image.PATH = "\\" + folderName + "\\" + image.FILE_NAME;
                            }


                            //await filesFtpHelper.UploadImage_Server3(image.FILE_NAME, folderName);
                        }
                    });
                    return await attachFileAppService.CM_IMAGE_Ins(images, refId);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }

        }


        [HttpGet]
        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_ByRefId(string ref_id)
        {
            return await attachFileAppService.CM_IMAGE_ByRefId(ref_id);
        }

        [HttpGet]
        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_ByAssCode(string ass_code)
        {
            return await attachFileAppService.CM_IMAGE_ByAssCode(ass_code);
        }

        [HttpGet]
        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_GetNearAsset(string assId, DateTime? INVENTORY_DT)
        {
            return await attachFileAppService.CM_IMAGE_GetNearAsset(assId, INVENTORY_DT);
        }

        [HttpGet]
        public async Task<List<CM_IMAGE_ENTITY>> CM_IMAGE_GetFirstAsset(string assId, DateTime? INVENTORY_DT)
        {
            return await attachFileAppService.CM_IMAGE_GetFirstAsset(assId, INVENTORY_DT);
        }
        private string slipUrl(string url)
        {
            return url.Replace(@"./", @"").Replace(@"..", @"").Replace(@"//", @"/");
        }

        [HttpGet]
        public FileDto GetImage(string image_path, string image_name)
        {
            if (!image_path.IsNullOrEmpty())
            {
                image_path = slipUrl(image_path);
            }
            try
            {
                var reportByteArray = System.IO.File.ReadAllBytes(image_path);
                FileDto file = new FileDto();

                var fileName = image_path.Substring(image_path.LastIndexOf("/") + 1);


                file = new FileDto(string.IsNullOrEmpty(image_name) ? fileName : image_name, MimeTypeNames.ImageJpeg);
                _tempFileCacheManager.SetFile(file.FileToken, reportByteArray);

                return file;

            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }
        private byte[] GetByteFromUrl(string path)
        {
            // TIENLEE 28-06-2022 Vá lỗi Directory Traversal
            if (!path.IsNullOrEmpty())
            {
                //path = "/" + path.Replace(@"/..", @"").Replace(@".", @"").Replace(@"..", @"/").Replace(@"//", @"/");
                path = slipUrl(path);
            }
            try
            {
                using (var webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(path);
                    return imageBytes;
                }

            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }
    }
}
