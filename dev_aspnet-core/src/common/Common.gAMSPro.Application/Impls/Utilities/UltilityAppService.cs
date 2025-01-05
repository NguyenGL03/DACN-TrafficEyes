using Abp.Authorization;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.UI;
using Common.gAMSPro.Configuration;
using Common.gAMSPro.Intfs.RoxyFilemans.Dto;
using Common.gAMSPro.Intfs.Utilities;
using Core.gAMSPro.Application;
using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;
using gAMSPro.Authorization.Users.Profile.Dto;
using gAMSPro.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Drawing;

namespace Common.gAMSPro.Impls.Utilities
{
    [AbpAuthorize]
    [DisableValidation]
    public class UltilityAppService : gAMSProCoreAppServiceBase, IUltilityAppService
    {
        private readonly string _tmpFilePath;
        private readonly string _filePath;
        private readonly string _filePathGd2;
        private readonly string _systemRootPath;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IBinaryObjectManager binaryObjectManager;
        private readonly TempFileCacheManager tempFileCacheManager;
        public UltilityAppService(IWebHostEnvironment env,
            IBinaryObjectManager binaryObjectManager,
            TempFileCacheManager tempFileCacheManager)
        {
            _appConfiguration = env.GetAppConfiguration();
            _tmpFilePath = _appConfiguration["App:FileUploadTempPath"];
            _filePath = _appConfiguration["App:FileUploadPath"];
            _filePathGd2 = "/Upload";
            _systemRootPath = _appConfiguration["App:RootDirectory"];
            if (_systemRootPath.Length == 0)
            {
                _systemRootPath = env.ContentRootPath;
            }
            this.tempFileCacheManager = tempFileCacheManager;
            this.binaryObjectManager = binaryObjectManager;
        }
        public async Task<bool> IsApproveFunction(string functionId)
        {
            return await base.IsApproveFunct(null, functionId);
        }

        public string GetProcedureContent(string procedureName)
        {
            return storeProcedureProvider.GetProcedureContent(procedureName);
        }

        string GetPhysicPath(string filePath)
        {
            if (!filePath.StartsWith(_tmpFilePath))
            {
                filePath = _tmpFilePath + filePath;
            }
            return _systemRootPath + "/wwwroot" + _tmpFilePath + filePath.Substring(_tmpFilePath.Length);
        }

        string MoveFile(string tmpFilePath, string userName)
        {
            var oldFile = GetPhysicPath(tmpFilePath);
            if (!tmpFilePath.StartsWith(_tmpFilePath + "/" + userName) && !File.Exists(oldFile))
            {
                return null;
            }
            var fileInfo = new FileInfo(oldFile);
            var destination = _systemRootPath + "/wwwroot" + tmpFilePath.Replace(_tmpFilePath, _filePath);
            destination = destination.Replace("/", "\\");

            var destinationPathAndName = SplitPathAndName(destination);
            var folderDestination = destinationPathAndName[0];

            if (File.Exists(destination))
            {
                var fileName = destinationPathAndName[1];
                var fileNameAndEtx = SplitFilenameAndEtx(fileName);

                var name = fileNameAndEtx[0];

                var indexOf_ = name.LastIndexOf("_");
                fileName = name.Substring(0, indexOf_) + "_" + Guid.NewGuid() + "." + fileNameAndEtx[1];

                destination = folderDestination + "/" + fileName;
            }

            if (!Directory.Exists(folderDestination))
            {
                Directory.CreateDirectory(folderDestination);
            }
            fileInfo.CopyTo(destination, true);
            return destination.Replace(_systemRootPath + "/wwwroot", "");
        }


        public CM_ATTACH_FILE_INPUT MoveTmpFile(CM_ATTACH_FILE_INPUT input)
        {
            // delete file old
            if (input.OldFiles != null && input.OldFiles.Length > 0)
            {
                var filesNeedDel = input.OldFiles.Where(x => !input.NewFiles.Contains(x) && !input.NewFiles.Contains(x.Replace(_filePath, _tmpFilePath)));
                foreach (var file in filesNeedDel)
                {
                    var filePath = _systemRootPath + "/wwwroot/" + file;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            var result = new CM_ATTACH_FILE_INPUT();
            result.Ids = input.Ids;

            var physicsPath = new List<string>();
            var userName = GetCurrentUserName();

            List<string> filePhysics = new List<string>();

            // move master
            if (input.AttachFile != null)
            {
                result.AttachFile = new CM_ATTACH_FILE_ENTITY();
                result.AttachFile.TYPE = input.AttachFile.TYPE;
                result.AttachFile.ATTACH_ID = input.AttachFile.ATTACH_ID;
                result.AttachFile.ATTACH_DT = input.AttachFile.ATTACH_DT;
                result.AttachFile.INDEX = input.AttachFile.INDEX;
                filePhysics = GetFilesPhysics(input.AttachFile);
                result.AttachFile.FILE_NAME_OLD = input.AttachFile.FILE_NAME_OLD;
                foreach (var file in filePhysics)
                {
                    string newFile = "";
                    if (file.StartsWith(_filePath) || ("/" + file).StartsWith(_filePath) || file.StartsWith(_filePathGd2) || ("/" + file).StartsWith(_filePathGd2))
                    {
                        newFile = _systemRootPath + "/wwwroot" + file;
                    }
                    else
                    {
                        newFile = MoveFile(file, userName);
                    }
                    if (newFile.IsNullOrWhiteSpace())
                    {
                        continue;
                    }
                    var pathAndName = SplitPathAndName(newFile);
                    result.AttachFile.PATH_NEW += "|" + pathAndName[0].Substring((_systemRootPath + "/wwwroot").Length);
                    result.AttachFile.FILE_NAME_NEW += "|" + pathAndName[1];
                }
                if (result.AttachFile.PATH_NEW != null && result.AttachFile.PATH_NEW.Length > 1)
                {
                    result.AttachFile.PATH_NEW = result.AttachFile.PATH_NEW.Substring(1);
                }
                if (result.AttachFile.FILE_NAME_NEW != null && result.AttachFile.FILE_NAME_NEW.Length > 1)
                {
                    result.AttachFile.FILE_NAME_NEW = result.AttachFile.FILE_NAME_NEW.Substring(1);
                }
            }

            if (input.Childs != null)
            {
                result.Childs = new List<CM_ATTACH_FILE_ENTITY>();
                foreach (var detail in input.Childs)
                {
                    if (detail == null)
                    {
                        continue;
                    }

                    filePhysics = GetFilesPhysics(detail);
                    detail.PATH_NEW = "";
                    detail.FILE_NAME_NEW = "";
                    foreach (var file in filePhysics)
                    {
                        string newFile = "";
                        if (file.StartsWith(_filePath) || ("/" + file).StartsWith(_filePath) || file.StartsWith(_filePathGd2) || ("/" + file).StartsWith(_filePathGd2))
                        {
                            newFile = _systemRootPath + "/wwwroot" + file;
                        }
                        else
                        {
                            newFile = MoveFile(file, userName);
                        }
                        if (newFile.IsNullOrWhiteSpace())
                        {
                            continue;
                        }
                        var pathAndName = SplitPathAndName(newFile);
                        detail.PATH_NEW += "|" + pathAndName[0].Substring((_systemRootPath + "/wwwroot").Length);
                        detail.FILE_NAME_NEW += "|" + pathAndName[1];
                    }

                    if (detail.PATH_NEW != null && detail.PATH_NEW.Length > 1)
                    {
                        detail.PATH_NEW = detail.PATH_NEW.Substring(1);
                    }
                    if (detail.FILE_NAME_NEW != null && detail.FILE_NAME_NEW.Length > 1)
                    {
                        detail.FILE_NAME_NEW = detail.FILE_NAME_NEW.Substring(1);
                    }
                    result.Childs.Add(detail);
                }
            }

            return result;
        }

        private bool checkFileImage(string s)
        {
            s = s.ToUpper();
            string[] spearator = { ",", " " };
            String[] strlist = _appConfiguration["HostFix:AllowImages"].Split(spearator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var c in strlist)
            {
                if (s.Contains(c))
                    return true;
            }

            return false;
        }
        private bool checkFileOffice(string s)
        {
            s = s.ToUpper();
            string[] spearator = { ",", " " };
            String[] strlist = _appConfiguration["HostFix:AllowFiles"].Split(spearator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var c in strlist)
            {
                if (s.Contains(c))
                    return true;
            }

            return false;
        }

        private string slipUrl(string url)
        {
            return url.Replace(@"./", @"").Replace(@"..", @"").Replace(@"//", @"/");
        }
        public async Task<UPLOAD_W_T_RESULT> UploadFile(string d, string fileName, string g, IFormFileCollection files, ISession session, bool isAjaxUpload, string settingName)
        {
            if (d != "/logo")
            {
                return new UPLOAD_W_T_RESULT()
                {
                    Result = SuccessCode.Error,
                    Message = "File denied.",
                    CM_ATTACH_FILE_ENTITY = null
                };
            }
            Boolean check = false;
            if (checkFileImage(d) || checkFileOffice(d))
            {
                check = true;
            }
            foreach (var file in files)
            {
                if (checkFileImage(file.FileName) || checkFileOffice(file.FileName))
                {
                    if (checkFileImage(fileName) || checkFileOffice(fileName))
                    {
                        check = true;
                    }
                }
                else
                {
                    check = false;
                    break;
                }
            }
            if (!check)
            {
                return new UPLOAD_W_T_RESULT()
                {
                    Result = SuccessCode.Error,
                    Message = "File denied.",
                    CM_ATTACH_FILE_ENTITY = null
                };
            }
            try
            {
                if (g.IsNullOrWhiteSpace())
                {
                    g = Guid.NewGuid().ToString();
                }
                if (!d.IsNullOrEmpty())
                {
                    d = slipUrl(d);
                }
                if (!fileName.IsNullOrEmpty())
                {
                    fileName = slipUrl(fileName);
                }
                var dir = _systemRootPath + "/wwwroot" + d;
                d = dir;


                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
                bool hasErrors = false;
                CM_ATTACH_FILE_ENTITY attachFile = new CM_ATTACH_FILE_ENTITY();

                string dest = "";
                try
                {
                    foreach (Microsoft.AspNetCore.Http.IFormFile file in files)
                    {
                        FileInfo f = new FileInfo(file.FileName);
                        dest = Path.Combine(d, fileName);

                        var i = dest.IndexOf("?v=");
                        if (i >= 0)
                        {
                            dest = dest.Substring(0, i);
                        }

                        using (FileStream saveFile = new FileStream(dest, FileMode.Create))
                        {
                            FileInfo fdInfo = new FileInfo(dest);
                            //await LogFile(fdInfo.FullName, FileLogAction.UploadFile, fdInfo.Length, fdInfo.Extension);
                            file.CopyTo(saveFile);
                        }

                        var value = await SettingManager.GetSettingValueAsync(settingName);
                        string nameFile = "";
                        if (value != null)
                        {
                            i = value.IndexOf("?v=");
                            if (i >= 0)
                            {
                                value = value.Substring(0, i).Replace(@"/..", @"").Replace(@"..", @"/").Replace(@"//", @"/");
                            }

                            if (settingName == "gAMSProCore.WebLogo") nameFile = "web_logo";
                            else if(settingName == "gAMSProCore.SmallWebLogo") nameFile = "small_web_logo";
                            else nameFile = "web_logo_login";

                            value = "/logo/" + nameFile + "." + files[0].ContentType.Substring(6, files[0].ContentType.Length - 6) + "?v=" + Guid.NewGuid().ToString();
                            await SettingManager.ChangeSettingForApplicationAsync(settingName, value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (isAjaxUpload)
                {
                    if (hasErrors)
                    {
                        return new UPLOAD_W_T_RESULT()
                        {
                            Result = SuccessCode.Error,
                            Message = "Error",
                            CM_ATTACH_FILE_ENTITY = attachFile
                        };
                    }
                }

                return new UPLOAD_W_T_RESULT()
                {
                    Result = SuccessCode.Success,
                    Message = "UpdateLoad File Success!",
                    CM_ATTACH_FILE_ENTITY = attachFile
                };
            }
            catch (Exception ex)
            {
                return new UPLOAD_W_T_RESULT()
                {
                    Result = SuccessCode.Error,
                    Message = ex.Message,
                    CM_ATTACH_FILE_ENTITY = null
                };
            }
        }
        [DisableValidation]
        public async Task UploadLogo(UpdateLogoPictureInput input)
        {
            byte[] byteArray;

            var imageBytes = tempFileCacheManager.GetFile(input.FileToken);

            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                using (var stream = new MemoryStream())
                {
                    bmpImage.Save(stream, bmpImage.RawFormat);
                    byteArray = stream.ToArray();
                }
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await binaryObjectManager.SaveAsync(storedFile);
        }

        public bool Delete_g_path(string g)
        {
            if (!g.IsNullOrEmpty())
            {
                g = slipUrl(g);
            }
            var d = _systemRootPath + "/wwwroot" + _tmpFilePath + "/" + g;
            if (Directory.Exists(d))
            {
                Directory.Delete(d, true);
            }
            return true;
        }

        public bool DEL_FILE(string[] files)
        {

            return true;
        }
    }
}
