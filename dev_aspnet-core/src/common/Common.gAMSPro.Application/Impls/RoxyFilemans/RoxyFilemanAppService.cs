using Abp.Authorization;
using Abp.Extensions;
using Abp.UI;
using Aspose.Words;
using Common.gAMSPro.Configuration;
using Common.gAMSPro.Intfs.RoxyFilemans;
using Common.gAMSPro.Intfs.RoxyFilemans.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;
using Core.gAMSPro.Consts;
using gAMSPro.Authorization.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Common.gAMSPro.Impls.RoxyFilemans
{
    [AbpAuthorize]
    public class RoxyFilemanAppService : gAMSProCoreAppServiceBase, IRoxyFilemanAppService
    {
        private readonly string _systemRootPath;
        private readonly string _systemRootPathFileman;
        private readonly string _tempPath;
        private readonly string _tempPathRelative;
        private string _filesRootPath;
        private string _filesRootVirtual;
        private Dictionary<string, string> _settings;
        private Dictionary<string, string> _lang = null;
        private UserManager userManager { get; set; }
        private readonly IConfigurationRoot _appConfiguration;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RoxyFilemanAppService(IWebHostEnvironment env,
            UserManager userManager)
        {
            _appConfiguration = env.GetAppConfiguration();
            _systemRootPathFileman = env.ContentRootPath;
            _systemRootPath = _appConfiguration["App:RootDirectory"];
            if (_systemRootPath.Length == 0)
            {
                _systemRootPath = env.ContentRootPath;
            }
            _tempPathRelative = _appConfiguration["App:FileUploadTempPath"];
            _tempPath = _systemRootPath + _appConfiguration["App:FileUploadTempPath"];
            _filesRootPath = "/wwwroot" + _appConfiguration["App:FileUploadPath"];
            _filesRootVirtual = _appConfiguration["App:FileUploadPath"];


            LoadSettings();
            this.userManager = userManager;
        }


        #region API Actions
        public async Task<StringResult> DIRLIST(string type, string functionFolder, ISession session)
        {
            try
            {
                var currentUserName = GetCurrentUserName();
                if (currentUserName == null)
                {
                    throw new Exception("Invalid files root directory. Check your configuration.");
                }

                var fileRoot = GetFilesRoot(session);
                DirectoryInfo d = new DirectoryInfo(fileRoot);
                if (!d.Exists)
                {
                    throw new Exception("Invalid files root directory. Check your configuration.");
                }

                string rootDir = fileRoot + "/" + currentUserName;
                d = new DirectoryInfo(rootDir);
                if (!d.Exists)
                {
                    Directory.CreateDirectory(rootDir);
                    d = new DirectoryInfo(rootDir);
                }

                if (!string.IsNullOrWhiteSpace(functionFolder))
                {
                    var dfp = rootDir + "/" + functionFolder;
                    var df = new DirectoryInfo(dfp);
                    if (!df.Exists)
                    {
                        Directory.CreateDirectory(dfp);
                    }
                }

                ArrayList dirs = ListDirs(d.FullName);
                dirs.Insert(0, d.FullName);
                string localPath = _systemRootPath;
                string result = "";
                for (int i = 0; i < dirs.Count; i++)
                {
                    string dir = (string)dirs[i];
                    result += (result != "" ? "," : "") + "{\"p\":\"" + MakeVirtualPath(dir.Replace(localPath, "").Replace("\\", "/")) + "\",\"f\":\"" + GetFiles(dir, type).Count.ToString() + "\",\"d\":\"" + Directory.GetDirectories(dir).Length.ToString() + "\"}";
                }
                return new StringResult() { SusscessCode = SuccessCode.Success, Result = "[" + result + "]" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> FILESLIST(string d, string type, ISession session)
        {
            try
            {
                var userName = GetCurrentUserName();

                string rootDir = _filesRootPath + "/" + userName;

                if (!d.StartsWith(_filesRootPath + "/" + userName) && !d.StartsWith(_filesRootVirtual + "/" + userName))
                {
                    throw new Exception("Access to " + d + " is denied");
                }
                d = MakePhysicalPath(d);
                await CheckPath(d, session);
                string fullPath = FixPath(d);
                List<string> files = GetFiles(fullPath, type);
                string result = "";
                for (int i = 0; i < files.Count; i++)
                {
                    FileInfo f = new FileInfo(files[i]);
                    int w = 0, h = 0;
                    // NO SUPPORT IN ASP.NET CORE! Per haps see https://github.com/CoreCompat/CoreCompat
                    //if (GetFileType(f.Extension) == "image")
                    //{
                    //    try
                    //    {
                    //        //FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read);
                    //        //Image img = Image.FromStream(fs);
                    //        //w = img.Width;
                    //        //h = img.Height;
                    //        //fs.Close();
                    //        //fs.Dispose();
                    //        //img.Dispose();
                    //    }
                    //    catch (Exception ex) { throw ex; }
                    //}
                    result += (result != "" ? "," : "") +
                        "{" +
                        "\"p\":\"" + MakeVirtualPath(d) + "/" + f.Name + "\"" +
                        ",\"t\":\"" + Math.Ceiling(LinuxTimestamp(f.LastWriteTime)).ToString() + "\"" +
                        ",\"s\":\"" + f.Length.ToString() + "\"" +
                        ",\"w\":\"" + w.ToString() + "\"" +
                        ",\"h\":\"" + h.ToString() + "\"" +
                        "}";
                }
                return new StringResult() { SusscessCode = SuccessCode.Success, Result = "[" + result + "]" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> COPYDIR(string d, string n, ISession session)
        {
            try
            {
                var userName = GetCurrentUserName();

                string rootDir = _filesRootPath + "/" + userName;

                if (!d.StartsWith("/wwwroot"))
                {
                    d = "/wwwroot" + d;
                }
                if (!n.StartsWith("/wwwroot"))
                {
                    n = "/wwwroot" + n;
                }

                if (!d.StartsWith(_filesRootPath + "/" + userName) && !d.StartsWith(_filesRootVirtual + "/" + userName))
                {
                    throw new Exception("Access to " + d + " is denied");
                }
                if (!n.StartsWith(_filesRootPath + "/" + userName) || !n.StartsWith(_filesRootPath + "/" + userName))
                {
                    throw new Exception("Access to " + n + " is denied");
                }

                d = MakePhysicalPath(d);
                n = MakePhysicalPath(n);
                await CheckPath(d, session);
                await CheckPath(n, session);
                DirectoryInfo dir = new DirectoryInfo(FixPath(d));
                DirectoryInfo newDir = new DirectoryInfo(FixPath(n + "/" + dir.Name));
                if (!dir.Exists)
                {
                    throw new Exception(LangRes("E_CopyDirInvalidPath"));
                }
                else if (newDir.Exists)
                {
                    throw new Exception(LangRes("E_DirAlreadyExists"));
                }
                else
                {
                    CopyDir(dir.FullName, newDir.FullName);
                    await LogFile(newDir.FullName, FileLogAction.CopyDir, dir.GetFiles().Select(x => x.Length).Sum(), "Folder", dir.FullName);
                }

                return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> COPYFILE(string f, string n, ISession session)
        {
            try
            {
                var currentUserName = GetCurrentUserName();

                f = MakePhysicalPath(f);
                await CheckPath(f, session);
                FileInfo file = new FileInfo(FixPath(f));
                n = FixPath(n);
                if (!file.Exists)
                {
                    throw new Exception(LangRes("E_CopyFileInvalisPath"));
                }
                else
                {
                    try
                    {
                        var oldFilePath = file.FullName;
                        var filePath = Path.Combine(n, MakeUniqueFilename(n, file.Name));
                        System.IO.File.Copy(oldFilePath, filePath);
                        await LogFile(filePath, FileLogAction.CopyFile, file.Length, file.Extension, oldFilePath);
                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception)
                    {
                        throw new Exception(LangRes("E_CopyFile"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> CREATEDIR(string d, string n, ISession session)
        {
            try
            {
                d = MakePhysicalPath(d);
                await CheckPath(d, session);
                d = FixPath(d);
                if (!Directory.Exists(d))
                {
                    throw new Exception(LangRes("E_CreateDirInvalidPath"));
                }
                else
                {
                    try
                    {
                        d = Path.Combine(d, n);
                        if (!Directory.Exists(d))
                        {
                            Directory.CreateDirectory(d);
                            await LogFile(d, FileLogAction.CopyFile, 0, "Folder");
                        }

                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception) { throw new Exception(LangRes("E_CreateDirFailed")); }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> DELETEDIR(string d, ISession session)
        {
            try
            {
                d = MakePhysicalPath(d);
                await CheckPath(d, session);
                d = FixPath(d);
                if (!Directory.Exists(d))
                {
                    throw new Exception(LangRes("E_DeleteDirInvalidPath"));
                }
                else if (d == GetFilesRoot(session))
                {
                    throw new Exception(LangRes("E_CannotDeleteRoot"));
                }
                else if (Directory.GetDirectories(d).Length > 0 || Directory.GetFiles(d).Length > 0)
                {
                    throw new Exception(LangRes("E_DeleteNonEmpty"));
                }
                else
                {
                    try
                    {
                        Directory.Delete(d);
                        await LogFile(d, FileLogAction.DeleteDir, 0, "Folder");
                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception) { throw new Exception(LangRes("E_CannotDeleteDir")); }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> DELETEFILE(string f, ISession session)
        {
            try
            {
                f = MakePhysicalPath(f);
                await CheckPath(f, session);
                f = FixPath(f);
                if (!System.IO.File.Exists(f))
                {
                    throw new Exception(LangRes("E_DeleteFileInvalidPath"));
                }
                else
                {
                    try
                    {
                        FileInfo file = new FileInfo(f);
                        await LogFile(f, FileLogAction.DeleteFile, file.Length, file.Extension);
                        System.IO.File.Delete(f);
                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception) { throw new Exception(LangRes("E_DeletеFile")); }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FileResultDto> DOWNLOAD(string f, ISession session)
        {
            try
            {
                f = MakePhysicalPath(f);
                //await CheckPath(f, session);
                FileInfo file = new FileInfo(FixPath(f));
                if (file.Exists)
                {
                    new FileExtensionContentTypeProvider().TryGetContentType(file.FullName, out string contentType);
                    //await LogFile(file.FullName, FileLogAction.DownloadFile, file.Length, file.Extension);
                    return new FileResultDto()
                    {
                        PhysicsPath = file.FullName,
                        ContentType = contentType ?? "application/octet-stream",
                        FileName = file.Name,
                        SusscessCode = SuccessCode.Success
                    };
                }
                else
                {
                    return new FileResultDto()
                    {
                        SusscessCode = SuccessCode.NotFound
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<FileResultDto> DOWNLOADDIR(string d)
        {
            try
            {
                d = MakePhysicalPath(d);
                d = FixPath(d);
                if (!Directory.Exists(d))
                {
                    throw new Exception(LangRes("E_CreateArchive"));
                }

                string dirName = new FileInfo(d).Name;
                string tmpZip = _tempPath + "/" + dirName + ".zip";
                if (System.IO.File.Exists(tmpZip))
                {
                    System.IO.File.Delete(tmpZip);
                }

                ZipFile.CreateFromDirectory(d, tmpZip, CompressionLevel.Fastest, true);

                var directoryInfo = new DirectoryInfo(d);
                await LogFile(dirName, FileLogAction.DownloadDir, directoryInfo.GetFiles().Sum(x => x.Length), "Folder");
                return new FileResultDto()
                {
                    PhysicsPath = tmpZip,
                    ContentType = "application/zip",
                    FileName = dirName + ".zip",
                    SusscessCode = SuccessCode.Success
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> MOVEDIR(string d, string n, ISession session)
        {
            try
            {
                d = MakePhysicalPath(d);
                n = MakePhysicalPath(n);

                if (!d.StartsWith("/wwwroot"))
                {
                    d = "/wwwroot" + d;
                }
                if (!n.StartsWith("/wwwroot"))
                {
                    n = "/wwwroot" + n;
                }

                await CheckPath(d, session);
                await CheckPath(n, session);
                DirectoryInfo source = new DirectoryInfo(FixPath(d));
                DirectoryInfo dest = new DirectoryInfo(FixPath(Path.Combine(n, source.Name)));
                if (dest.FullName.IndexOf(source.FullName) == 0)
                {
                    throw new Exception(LangRes("E_CannotMoveDirToChild"));
                }
                else if (!source.Exists)
                {
                    throw new Exception(LangRes("E_MoveDirInvalisPath"));
                }
                else if (dest.Exists)
                {
                    throw new Exception(LangRes("E_DirAlreadyExists"));
                }
                else
                {
                    try
                    {
                        await LogFile(dest.FullName, FileLogAction.MoveDir, source.GetFiles().Sum(x => x.Length), "Folder", source.FullName);
                        source.MoveTo(dest.FullName);
                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception) { throw new Exception(LangRes("E_MoveDir") + " \"" + d + "\""); }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> MOVEFILE(string f, string n, ISession session)
        {
            try
            {
                f = MakePhysicalPath(f);
                if (!n.StartsWith("/wwwroot"))
                {
                    n = "/wwwroot" + n;
                }
                n = MakePhysicalPath(n);
                await CheckPath(f, session);
                await CheckPath(n, session);
                FileInfo source = new FileInfo(FixPath(f));
                FileInfo dest = new FileInfo(FixPath(n));
                if (!source.Exists)
                {
                    throw new Exception(LangRes("E_MoveFileInvalisPath"));
                }
                else if (dest.Exists)
                {
                    throw new Exception(LangRes("E_MoveFileAlreadyExists"));
                }
                else if (!CanHandleFile(dest.Name))
                {
                    throw new Exception(LangRes("E_FileExtensionForbidden"));
                }
                else
                {
                    try
                    {
                        await LogFile(dest.FullName, FileLogAction.MoveFile, source.Length, source.Extension, source.FullName);
                        source.MoveTo(dest.FullName);
                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception) { throw new Exception(LangRes("E_MoveFile") + " \"" + f + "\""); }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> RENAMEDIR(string d, string n, ISession session)
        {
            try
            {
                d = MakePhysicalPath(d);
                await CheckPath(d, session);
                DirectoryInfo source = new DirectoryInfo(FixPath(d));
                DirectoryInfo dest = new DirectoryInfo(Path.Combine(source.Parent.FullName, n));
                if (source.FullName == GetFilesRoot(session))
                {
                    throw new Exception(LangRes("E_CannotRenameRoot"));
                }
                else if (!source.Exists)
                {
                    throw new Exception(LangRes("E_RenameDirInvalidPath"));
                }
                else if (dest.Exists)
                {
                    throw new Exception(LangRes("E_DirAlreadyExists"));
                }
                else
                {
                    try
                    {
                        await LogFile(dest.FullName, FileLogAction.RenameDir, source.GetFiles().Sum(x => x.Length), "Folder", source.FullName);
                        source.MoveTo(dest.FullName);
                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception) { throw new Exception(LangRes("E_RenameDir") + " \"" + d + "\""); }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StringResult> RENAMEFILE(string f, string n, ISession session)
        {
            try
            {
                f = MakePhysicalPath(f);
                await CheckPath(f, session);
                FileInfo source = new FileInfo(FixPath(f));
                FileInfo dest = new FileInfo(Path.Combine(source.Directory.FullName, n));
                if (!source.Exists)
                {
                    throw new Exception(LangRes("E_RenameFileInvalidPath"));
                }
                else if (!CanHandleFile(n))
                {
                    throw new Exception(LangRes("E_FileExtensionForbidden"));
                }
                else
                {
                    try
                    {
                        await LogFile(dest.FullName, FileLogAction.RenameFile, source.Length, source.Extension, source.FullName);
                        source.MoveTo(dest.FullName);
                        return new StringResult() { SusscessCode = SuccessCode.Success, Result = GetSuccessRes() };
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                return new StringResult() { SusscessCode = SuccessCode.Error, Result = GetErrorRes(ex.Message) };
            }
        }

        public async Task<StringResult> UPLOAD(string d, IFormFileCollection files, ISession session, bool isAjaxUpload)
        {
            Boolean check = false;
            if (checkFileImage(d) || checkFileOffice(d))
            {
                check = true;
            }
            foreach (var file in files)
            {
                if (checkFileImage(file.FileName) || checkFileOffice(file.FileName))
                {
                    check = true;
                }
            }

            if (!check)
            {
                return new StringResult() { SusscessCode = SuccessCode.Error, Result = "File denied." };
            }
            try
            {
                d = MakePhysicalPath(d);
                await CheckPath(d, session);
                d = FixPath(d);
                string res = GetSuccessRes();
                bool hasErrors = false;
                try
                {
                    foreach (Microsoft.AspNetCore.Http.IFormFile file in files)
                    {
                        if (CanHandleFile(file.FileName))
                        {
                            FileInfo f = new FileInfo(file.FileName);
                            string filesizeAttach = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.FileSizeAttach);

                            var error = await CheckFileLength(f.Length);
                            if (!error.IsNullOrWhiteSpace())
                            {
                                return new StringResult() { SusscessCode = SuccessCode.Error, Result = error };
                            }
                            string filename = MakeUniqueFilename(d, f.Name);
                            string dest = Path.Combine(d, filename);
                            using (FileStream saveFile = new FileStream(dest, FileMode.Create))
                            {
                                FileInfo fdInfo = new FileInfo(dest);
                                await LogFile(fdInfo.FullName, FileLogAction.UploadFile, fdInfo.Length, fdInfo.Extension);
                                file.CopyTo(saveFile);
                            }
                        }
                        else
                        {
                            hasErrors = true;
                            res = GetSuccessRes(LangRes("E_UploadNotAll"));
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
                        res = GetErrorRes(LangRes("E_UploadNotAll"));
                    }

                    return new StringResult() { SusscessCode = SuccessCode.Success, Result = res };
                }
                else
                {
                    return new StringResult() { SusscessCode = SuccessCode.Success, Result = "<script>parent.fileUploaded(" + res + ");</script>" };
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<UPLOAD_W_T_RESULT> UPLOAD_W_T(string d, string g, IFormFileCollection files, ISession session, bool isAjaxUpload)
        {
            // Vá lỗi Directory Traversal
            Boolean check = false;
            if (checkFileImage(d) || checkFileOffice(d))
            {
                check = true;
            }
            foreach (var file in files)
            {
                if (checkFileImage(file.FileName) || checkFileOffice(file.FileName)) check = true;
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
            else
            {
                try
                {
                    if (g.IsNullOrWhiteSpace())
                        g = Guid.NewGuid().ToString();

                    foreach (var file in files)
                    {
                        if (!file.FileName.IsNullOrEmpty() && (file.FileName.Contains("./") || (file.FileName.Contains(".."))))
                        {
                            throw new UserFriendlyException(message: "File denied.");
                        }
                    }

                    var dir = _tempPathRelative + "/" + GetCurrentUserName() + "/" + d;
                    d = dir;

                    d = MakePhysicalPath(d);
                    d = FixPath(d);

                    // CHECK FILE
                    var error = await CheckFileNumber(files.Count);
                    if (!error.IsNullOrWhiteSpace())
                    {
                        return new UPLOAD_W_T_RESULT()
                        {
                            Result = SuccessCode.Error,
                            Message = error, 
                        };
                    }

                    foreach (Microsoft.AspNetCore.Http.IFormFile file in files)
                    {
                        if (CanHandleFile(file.FileName))
                        {
                            error = await CheckFileLength(file.Length);
                            if (!error.IsNullOrWhiteSpace())
                            {
                                return new UPLOAD_W_T_RESULT()
                                {
                                    Result = SuccessCode.Error,
                                    Message = error,
                                };
                            }

                            error = await CheckFileNameLength(file.FileName);
                            if (!error.IsNullOrWhiteSpace())
                            {
                                return new UPLOAD_W_T_RESULT()
                                {
                                    Result = SuccessCode.Error,
                                    Message = error,
                                };
                            }
                        }
                    }


                    log.Info("Directory.Exists(d)=======:" + Directory.Exists(d));
                    if (!Directory.Exists(d))
                    {
                        Directory.CreateDirectory(d);
                    }
                    string res = GetSuccessRes();
                    bool hasErrors = false;
                    CM_ATTACH_FILE_ENTITY attachFile = new CM_ATTACH_FILE_ENTITY();

                    try
                    {
                        foreach (Microsoft.AspNetCore.Http.IFormFile file in files)
                        {
                            if (CanHandleFile(file.FileName))
                            {
                                FileInfo f = new FileInfo(file.FileName);
                                //string filesizeAttach = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.FileSizeAttach);
                                string filename = MakeUniqueFilenameGuid(f.Name);
                                string dest = Path.Combine(d, filename);
                                using (FileStream saveFile = new FileStream(dest, FileMode.Create))
                                {
                                    FileInfo fdInfo = new FileInfo(dest);
                                    await LogFile(fdInfo.FullName, FileLogAction.UploadFile, fdInfo.Length, fdInfo.Extension);
                                    file.CopyTo(saveFile);

                                    attachFile.PATH_NEW += "|" + dir;
                                    attachFile.FILE_NAME_NEW += "|" + fdInfo.Name;
                                    attachFile.FILE_NAME_OLD += "|" + f.Name;
                                }
                            }
                            else
                            {
                                hasErrors = true;
                                res = GetSuccessRes(LangRes("E_UploadNotAll"));
                            }
                        }

                        if (attachFile.PATH_NEW != null && attachFile.PATH_NEW.Length > 1)
                        {
                            attachFile.PATH_NEW = attachFile.PATH_NEW.Substring(1);
                        }
                        if (attachFile.FILE_NAME_NEW != null && attachFile.FILE_NAME_NEW.Length > 1)
                        {
                            attachFile.FILE_NAME_NEW = attachFile.FILE_NAME_NEW.Substring(1);
                        }
                        if (attachFile.FILE_NAME_OLD != null && attachFile.FILE_NAME_OLD.Length > 1)
                        {
                            attachFile.FILE_NAME_OLD = attachFile.FILE_NAME_OLD.Substring(1);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info("Directory.Exists(d)=======:2" + ex);
                        throw ex;

                    }
                    if (hasErrors)
                    {
                        var ress = GetErrorRes(LangRes("E_UploadNotAll"));
                        if (ress.Contains("Could not find a part of the path")) ress = "Could not find a part of the path.";
                        return new UPLOAD_W_T_RESULT()
                        {
                            Result = SuccessCode.Error,
                            Message = ress,
                            CM_ATTACH_FILE_ENTITY = null
                        };
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
                    var res = ex.Message;
                    log.Info("Directory.Exists(d)=======:3" + ex);
                    if (ex.Message.Contains("Could not find a part of the path")) res = "Could not find a part of the path.";
                    return new UPLOAD_W_T_RESULT()
                    {
                        Result = SuccessCode.Error,
                        Message = res,
                        CM_ATTACH_FILE_ENTITY = null
                    };
                }
            }
        }


        public async Task<string> UPLOAD_W_T_HTML(IFormFileCollection Formfiles)
        {
            Stream stream = new MemoryStream();
            Microsoft.AspNetCore.Http.IFormFile formfile = Formfiles[0];
            formfile.CopyTo(stream);
            var doc = new Document(stream);
            foreach (Section section in doc)
            {
                if (section.HeadersFooters.Count > 0)
                {
                    section.HeadersFooters.RemoveAt(0);
                }
                HeaderFooter footer;
                // Primary footer is the footer used for odd pages.
                footer = section.HeadersFooters[HeaderFooterType.FooterPrimary];
                if (footer != null)
                    footer.Remove();
            }
            doc.Save("wwwroot/ImportWordTemp/" + AbpSession.UserId + ".html", SaveFormat.Html);
            string line = "";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("wwwroot/ImportWordTemp/" + AbpSession.UserId + ".html");
                //Read the first line of text
                line = sr.ReadLine();
                //close the file
                sr.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            //delete file
            string[] files = Directory.GetFiles("wwwroot/ImportWordTemp/");
            foreach (string file in files)
            {
                File.Delete(file);
            }
            return line;
        }
        public async Task<string> CheckFileNumber(int fileCount)
        {
            int maxFileCount = -1;
            var filesizeAttach = await storeProcedureProvider
              .GetResultValueFromStore("ABP_SETTING_ByName", new
              {
                  p_NAME = SettingConsts.WebConsts.NumberOfFiles
              });
            int.TryParse(filesizeAttach["Value"].ToString(), out maxFileCount);
            if (maxFileCount != -1 && fileCount > maxFileCount)
            {
                return GetErrorRes(L("FileCountInvalid", maxFileCount));
            }
            return "";
        }

        public async Task<string> CheckFileLength(long fileLength)
        {
            int maxFilesize = -1;
            var filesizeAttach = await storeProcedureProvider
              .GetResultValueFromStore("ABP_SETTING_ByName", new
              {
                  p_NAME = SettingConsts.WebConsts.FileSizeAttach
              });
            int.TryParse(filesizeAttach["Value"].ToString(), out maxFilesize);
            if (maxFilesize != -1 && Math.Round((decimal)fileLength / (1024 * 1024), 0) > maxFilesize)
            {
                return GetErrorRes(L("MaxFileLengthInvalid", maxFilesize));
            }

            return "";
        }


        private bool CanHandleFileExtension(IFormFile file)
        {
            bool ret = true;
            Dictionary<string, byte[]> lstVali = new Dictionary<string, byte[]>();
            lstVali.Add(".DOC", new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 });//doc-xls-ppt-msg
            lstVali.Add(".XLS", new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 });//doc-xls-ppt-msg                
            lstVali.Add(".DOCX", new byte[] { 0x50, 0x4B, 0x03, 0x04 });//docx-zip-xlsx-aar-apk-pptx--...
            lstVali.Add(".XLSX", new byte[] { 0x50, 0x4B, 0x03, 0x04 });//docx-zip-xlsx-aar-apk-pptx--...
            lstVali.Add(".PDF", new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2d }); //pdf   
            FileInfo fileinfor = new FileInfo(file.FileName);
            byte[] tmpf = lstVali[fileinfor.Extension.ToUpper()];
            byte[] header = new byte[tmpf.Length];

            file.OpenReadStream().Read(header, 0, header.Length);



            if (!CompareArray(tmpf, header))
            {
                ret = false;
            }

            return ret;
        }
        private static bool CompareArray(byte[] tmpf, byte[] header)
        {
            return tmpf.SequenceEqual(header);
        }

        public async Task<string> CheckFileNameLength(string fileName)
        {
            int maxFilenameLength = -1;

            var filesizeAttach = await storeProcedureProvider
              .GetResultValueFromStore("ABP_SETTING_ByName", new
              {
                  p_NAME = SettingConsts.WebConsts.MaxFilenameLength
              });
            int.TryParse(filesizeAttach["Value"].ToString(), out maxFilenameLength);
            if (maxFilenameLength != -1 && fileName.Length > maxFilenameLength)
            {
                return GetErrorRes(L("FileNameLengthInvalid", maxFilenameLength));
            }

            return "";
        }
        #endregion

        #region Utilities
        private string MakeVirtualPath(string path)
        {
            //path = Path.GetFullPath(path);
            return !path.StartsWith(_filesRootPath) ? path : _filesRootVirtual + path.Substring(_filesRootPath.Length);
        }

        private string MakePhysicalPath(string path)
        {
            //path = Path.GetFullPath(path);
            return !path.StartsWith(_filesRootVirtual) ? path : _filesRootPath + path.Substring(_filesRootVirtual.Length);
        }

        private string GetFilesRoot(ISession session)
        {
            string ret = _filesRootPath;
            if (GetSetting("SESSION_PATH_KEY") != "" && session.GetString(GetSetting("SESSION_PATH_KEY")) != null)
            {
                ret = session.GetString(GetSetting("SESSION_PATH_KEY"));
            }

            ret = FixPath(ret);
            return ret;
        }

        private ArrayList ListDirs(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            ArrayList ret = new ArrayList();
            foreach (string dir in dirs)
            {
                ret.Add(dir);
                ret.AddRange(ListDirs(dir));
            }
            return ret;
        }

        private List<string> GetFiles(string path, string type)
        {
            List<string> ret = new List<string>();
            if (type == "#" || type == null)
            {
                type = "";
            }

            string[] files = Directory.GetFiles(path);
            foreach (string f in files)
            {
                if ((GetFileType(new FileInfo(f).Extension) == type) || (type == ""))
                {
                    ret.Add(f);
                }
            }
            return ret;
        }

        private string GetFileType(string ext)
        {
            string ret = "file";
            ext = ext.ToLower();
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
            {
                ret = "image";
            }
            else if (ext == ".swf" || ext == ".flv")
            {
                ret = "flash";
            }

            return ret;
        }

        private async Task CheckPath(string path, ISession session)
        {
            //TIENLEE 04-06-2022 hide path server
            if (FixPath(path).IndexOf(GetFilesRoot(session)) != 0)
            {
                throw new Exception("Access to invalid path is denied"); //Access to " + path + " is denied
            }

            var userName = GetCurrentUserName();

            string rootDir = _filesRootPath + "/" + userName;
            if (!path.StartsWith(_filesRootPath + "/" + userName) || !path.StartsWith(_filesRootPath + "/" + userName))
            {
                throw new Exception("Access to invalid path is denied");
            }

        }

        private string FixPath(string path)
        {
            path = path.TrimStart('~');
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            if (!path.StartsWith("/wwwroot"))
            {
                path = "/wwwroot" + path;
            }

            return _systemRootPath + path;
        }

        private double LinuxTimestamp(DateTime d)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();
            TimeSpan timeSpan = (d.ToLocalTime() - epoch);
            return timeSpan.TotalSeconds;
        }

        private string GetSetting(string name)
        {
            string ret = "";
            if (_settings.ContainsKey(name))
            {
                ret = _settings[name];
            }

            return ret;
        }

        private string GetErrorRes(string msg) { return GetResultStr("error", msg); }

        private string GetResultStr(string type, string msg)
        {
            return "{\"res\":\"" + type + "\",\"msg\":\"" + msg.Replace("\"", "\\\"") + "\"}";
        }

        private string LangRes(string name) { return _lang.ContainsKey(name) ? _lang[name] : name; }

        private string GetSuccessRes(string msg) { return GetResultStr("ok", msg); }

        private string GetSuccessRes() { return GetSuccessRes(""); }

        private void CopyDir(string path, string dest)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            foreach (string f in Directory.GetFiles(path))
            {
                FileInfo file = new FileInfo(f);
                if (!System.IO.File.Exists(Path.Combine(dest, file.Name)))
                {
                    System.IO.File.Copy(f, Path.Combine(dest, file.Name));
                }
            }
            foreach (string d in Directory.GetDirectories(path))
            {
                CopyDir(d, Path.Combine(dest, new DirectoryInfo(d).Name));
            }
        }

        private string MakeUniqueFilename(string dir, string filename)
        {
            string ret = filename;
            int i = 0;
            while (System.IO.File.Exists(Path.Combine(dir, ret)))
            {
                i++;
                ret = Path.GetFileNameWithoutExtension(filename) + " - Copy " + i.ToString() + Path.GetExtension(filename);
            }
            return ret;
        }

        private string MakeUniqueFilenameGuid(string filename)
        {
            int lastIndexOfDot = filename.LastIndexOf(".");
            if (lastIndexOfDot < 0)
            {
                return filename + Guid.NewGuid().ToString();
            }

            return filename.Substring(0, lastIndexOfDot) + "_" + Guid.NewGuid() + filename.Substring(lastIndexOfDot);
        }

        private bool CanHandleFile(string filename)
        {
            bool ret = false;
            FileInfo file = new FileInfo(filename);
            string ext = file.Extension.Replace(".", "").ToLower();
            string setting = GetSetting("FORBIDDEN_UPLOADS").Trim().ToLower();
            if (setting != "")
            {
                ArrayList tmp = new ArrayList();
                tmp.AddRange(Regex.Split(setting, "\\s+"));
                if (!tmp.Contains(ext))
                {
                    ret = true;
                }
            }
            setting = GetSetting("ALLOWED_UPLOADS").Trim().ToLower();
            if (setting != "")
            {
                ArrayList tmp = new ArrayList();
                tmp.AddRange(Regex.Split(setting, "\\s+"));
                if (!tmp.Contains(ext))
                {
                    ret = false;
                }
            }
            return ret;
        }



        private void LoadSettings()
        {
            try
            {
                _settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(_systemRootPathFileman + "/wwwroot/fileman/conf.json"));
                string langFile = _systemRootPathFileman + "/wwwroot/fileman/lang/" + GetSetting("LANG") + ".json";
                if (!System.IO.File.Exists(langFile))
                {
                    langFile = _systemRootPathFileman + "/wwwroot/fileman/lang/en.json";
                }

                _lang = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(langFile));
            }
            catch (Exception ex)
            {
                log.Error("LoadSettings()=======:" + ex);

                throw ex;
            }
        }
        #endregion
        private string slipUrl(string url)
        {
            return url.Replace(@"./", @"").Replace(@"..", @"").Replace(@"//", @"/");
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

        public async Task<UPLOAD_W_T_RESULT> UPLOAD_SYSTEM(string d, IFormFileCollection files, ISession session, bool isAjaxUpload)
        {
            Boolean check = false;
            if (checkFileImage(d) || checkFileOffice(d))
            {
                check = true;
            }
            foreach (var file in files)
            {
                if (checkFileImage(file.FileName) || checkFileOffice(file.FileName))
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
                return new UPLOAD_W_T_RESULT()
                {
                    Result = SuccessCode.Error,
                    Message = "File denied.",
                    CM_ATTACH_FILE_ENTITY = null
                };
            }

            try
            {

                string path = Directory.GetCurrentDirectory();
                string res = GetSuccessRes();
                bool hasErrors = false;
                //END
                foreach (var file in files)
                {
                    if (file.FileName.IsNullOrEmpty() || (file.FileName.Contains("./") || file.FileName.Contains("..")) && (checkFileOffice(file.FileName) == false || checkFileImage(file.FileName) == false))
                    {
                        return new UPLOAD_W_T_RESULT()
                        {
                            Result = SuccessCode.Error,
                            Message = "File denied.",
                            CM_ATTACH_FILE_ENTITY = null
                        };
                    }
                }

                string dir = Path.GetFullPath(Path.Combine(path, d));

                try
                {
                    foreach (Microsoft.AspNetCore.Http.IFormFile file in files)
                    {
                        if (CanHandleFile(file.FileName))
                        {
                            FileInfo f = new FileInfo(file.FileName);
                            string filesizeAttach = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.FileSizeAttach);
                            string filename = f.Name;

                            if (f.Name.Split(".")[f.Name.Split(".").Length - 1].ToLower() == "zip")
                            {
                                using (var zipArchive = new ZipArchive(file.OpenReadStream()))
                                {
                                    zipArchive.ExtractToDirectory(dir, true);
                                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                                    {
                                        string dest = Path.Combine(dir, entry.Name);
                                        FileInfo fdInfo = new FileInfo(dest);
                                        await LogFile(fdInfo.FullName, FileLogAction.UploadFile, fdInfo.Length, fdInfo.Extension);
                                    }
                                }
                            }
                            else
                            {
                                string dest = Path.Combine(dir, filename);
                                using (FileStream saveFile = new FileStream(dest, FileMode.Create))
                                {
                                    FileInfo fdInfo = new FileInfo(dest);
                                    await LogFile(fdInfo.FullName, FileLogAction.UploadFile, fdInfo.Length, fdInfo.Extension);
                                    file.CopyTo(saveFile);
                                }
                            }
                        }
                        else
                        {
                            hasErrors = true;
                            res = GetSuccessRes(LangRes("E_UploadNotAll"));
                            if (res.Contains("Could not find a part of the path")) res = "Could not find a part of the path.";
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Could not find a part of the path")) res = "Could not find a part of the path.";
                    else res = ex.Message;
                    return new UPLOAD_W_T_RESULT()
                    {
                        Result = SuccessCode.Error,
                        Message = res,
                        CM_ATTACH_FILE_ENTITY = null
                    };

                }
                if (isAjaxUpload)
                {
                    if (hasErrors)
                    {
                        res = GetErrorRes(LangRes("E_UploadNotAll"));
                        if (res.Contains("Could not find a part of the path")) res = "Could not find a part of the path.";
                        return new UPLOAD_W_T_RESULT()
                        {
                            Result = SuccessCode.Success,
                            Message = res,
                            CM_ATTACH_FILE_ENTITY = null
                        };
                    }
                }

                return new UPLOAD_W_T_RESULT()
                {
                    Result = SuccessCode.Success,
                    Message = "UpdateLoad File Success!",
                    CM_ATTACH_FILE_ENTITY = null
                };
            }
            catch (Exception ex)
            {
                var ress = GetErrorRes(LangRes("E_UploadNotAll"));
                if (ress.Contains("Could not find a part of the path")) ress = "Could not find a part of the path.";
                return new UPLOAD_W_T_RESULT()
                {
                    Result = SuccessCode.Error,
                    Message = ress,
                    CM_ATTACH_FILE_ENTITY = null
                };
            }
        }
    }
}