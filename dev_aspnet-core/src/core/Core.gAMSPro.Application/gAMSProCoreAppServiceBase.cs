using Abp.Application.Services;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Abp.Threading;
using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;
using Core.gAMSPro.Application.CoreModule.Utils.EmailSenders;
using Core.gAMSPro.Application.CoreModule.Utils.Notifications;
using Core.gAMSPro.Consts;
using Core.gAMSPro.CoreModule.Utils;
using Core.gAMSPro.Intfs.ApiESB;
using Core.gAMSPro.Intfs.WhiteCard;
using Core.gAMSPro.Utils;
using gAMSPro;
using gAMSPro.Authorization.Users;
using gAMSPro.Configuration;
using gAMSPro.Consts;
using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using gAMSPro.MultiTenancy;
using gAMSPro.ProcedureHelpers;
using gAMSPro.Storage;
using GSOFTcore.gAMSPro.Functions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace Core.gAMSPro.Application
{
    [AbpAuthorize]
    public abstract class gAMSProCoreAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }
        public UserManager UserManager { get; set; }
        private IEmailSender emailSender;
        protected IStoreProcedureProvider storeProcedureProvider;
        private IHttpContextAccessor httpContextAccessor;
        protected readonly IConfigurationRoot appConfiguration;
        protected IApiESBAppService _apiESBAppService;

        protected IPushCore pushCore;
        protected IWhiteCardAppService _whiteCardAppService;
        private readonly ITempFileCacheManager tempFileCacheManager;

        private readonly string _systemRootPath;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected gAMSProCoreAppServiceBase()
        {
            var env = IocManager.Instance.Resolve<IWebHostEnvironment>();
            _systemRootPath = env.ContentRootPath + "/wwwroot/";

            LocalizationSourceName = gAMSProConsts.LocalizationSourceName;
            storeProcedureProvider = IocManager.Instance.Resolve<IStoreProcedureProvider>();
            emailSender = IocManager.Instance.Resolve<IEmailSender>();
            httpContextAccessor = IocManager.Instance.Resolve<IHttpContextAccessor>();
            tempFileCacheManager = IocManager.Instance.Resolve<ITempFileCacheManager>();

            
            appConfiguration = env.GetAppConfiguration();
        }
        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }
        protected virtual void SetAuditForInsert(IAuditDto model)
        {
            model.CREATE_DT = DateTime.Now;
            model.AUTH_STATUS = ApproveStatusConsts.NotApprove;
            if (model.RECORD_STATUS == null)
            {
                model.RECORD_STATUS = RecordStatusConsts.Active;
            }
        }

        protected virtual void SetAuditForUpdate(IAuditDto model)
        {
            model.AUTH_STATUS = ApproveStatusConsts.NotApprove;
        }

        protected virtual async Task<bool> IsApproveFunct(string pageName, string functionId = null)
        {
            var item = (await storeProcedureProvider.GetDataFromStoredProcedure<TL_MENU_IsApproveFunct_Result>(CoreStoreProcedureConsts.TL_MENU_ISAPPROVEFUNCT, new
            {
                MENU_NAME_EL = pageName,
                FUNCTION_ID = functionId
            })).FirstOrDefault();

            if (item == null)
            {
                return false;
            }

            return item.ISAPPROVE_FUNC == IsApproveFunctConsts.IsApprove;
        }
        protected virtual User GetCurrentUser()
        {
            return AsyncHelper.RunSync(GetCurrentUserAsync);
        }
        protected string GetCurrentUserName()
        {
            return httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        protected DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
        public string GetClaimValue(string claimKey)
        {
            var claims = httpContextAccessor.HttpContext?.User.Identities.First().Claims;
            return claims.FirstOrDefault(x => x.Type == claimKey)?.Value;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }

        protected virtual Tenant GetCurrentTenant()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetById(AbpSession.GetTenantId());
            }
        }

        public async Task SendEmailAndNotify(string id, string? notificationMessage, string nfMessageType, string roleTifyType)
        {
            List<ROLE_NOTIFICATION_ENTITY> listEmail = await TR_ROLE_NOTIFI_ID(id, roleTifyType);
            EmailContent message = (await storeProcedureProvider.GetDataFromStoredProcedure<EmailContent>("NF_MESSAGE_GetContent", new { TYPE = nfMessageType, ID = id })).FirstOrDefault();

            string title = "";
            string messageContent = "";

            if (message != null && message.Title != null && message.MessageContent != null)
            {
                title = message.Title;
                messageContent = message.MessageContent;
            }

            foreach (var item in listEmail)
            {
                _ = Task.Run(() =>
                {
                    if (item.EMAIL == null || title == null || messageContent == null) return;
                    emailSender.Send(item.EMAIL, title, messageContent, true);
                });
            }
        }

        private async Task<List<ROLE_NOTIFICATION_ENTITY>> TR_ROLE_NOTIFI_ID(string poId, string type)
        {
            var lst = await storeProcedureProvider.GetDataFromStoredProcedure<ROLE_NOTIFICATION_ENTITY>("TR_ROLE_NOTIFI_ID", new
            {
                PO_ID = poId,
                TYPE = type
            });
            return lst;
        }
        protected async Task LogFile(string fileName, string action, decimal fileSize, string fileType, string oldPath = "", string notes = "")
        {
            var currentUser = await GetCurrentUserAsync();
            var fileLogEntity = new CM_ATTACH_FILE_LOG_ENTITY()
            {
                PATH = fileName,
                OLD_PATH = oldPath,
                ACTION = action,
                FILE_SIZE = fileSize,
                FILE_TYPE = fileType,
                USER_NAME = currentUser.UserName,
                NOTES = notes
            };
            await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CoreStoreProcedureConsts.CM_ATTACH_FILE_LOG_INS, fileLogEntity);
        }
        protected string[] SplitPathAndName(string filePath)
        {
            filePath = filePath.Replace("\\", "/");
            string[] res = new string[2];
            res[0] = filePath.Substring(0, filePath.LastIndexOf("/"));
            res[1] = filePath.Substring(filePath.LastIndexOf("/") + 1);
            return res;
        }

        protected string[] SplitFilenameAndEtx(string fileName)
        {
            string[] res = new string[2];
            res[0] = fileName.Substring(0, fileName.LastIndexOf("."));
            res[1] = fileName.Substring(fileName.LastIndexOf(".") + 1);
            return res;
        }

        protected string GetFileName(string filePath)
        {
            string fileName = null;
            if (filePath != null)
            {
                filePath = filePath.Replace("\\", "/");
                fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            }
            return fileName;
        }

        protected CM_ATTACH_FILE_ENTITY CreateAttachFileEntity(string attachId, string type, string refId, string filePath, string filePathOld = "", string index = "0", string notes = "")
        {
            var fileNameOld = GetFileName(filePathOld);

            if (!filePath.IsNullOrWhiteSpace() && filePath != "/")
            {
                var fi = new FileInfo(_systemRootPath + filePath);

                var filePathNew = fi.FullName.Replace("\\", "/");

                filePathNew = filePathNew.Substring(filePathNew.IndexOf("wwwroot/") + 8);
                filePathNew = filePathNew.Replace("/" + fi.Name, "");

                if (filePathOld.IsNullOrWhiteSpace())
                {
                    filePathOld = null;
                    fileNameOld = null;
                }
                else
                {
                    filePathOld = filePathOld.Replace("\\", "/");
                    filePathOld = filePathOld.Replace("/" + fileNameOld, "");
                }

                return new CM_ATTACH_FILE_ENTITY()
                {
                    ATTACH_ID = attachId,
                    TYPE = type,
                    REF_ID = refId,
                    FILE_NAME_OLD = fileNameOld,
                    FILE_NAME_NEW = fi.Name,
                    PATH_OLD = filePathOld,
                    PATH_NEW = filePathNew,
                    FILE_SIZE = fi.Exists ? fi.Length : 0,
                    FILE_TYPE = fi.Extension,
                    ATTACH_DT = DateTime.Now,
                    EMP_ID = GetCurrentUserName(),
                    INDEX = index,
                    NOTES = notes
                };
            }

            return new CM_ATTACH_FILE_ENTITY()
            {
                ATTACH_ID = attachId,
                TYPE = type,
                REF_ID = refId,
                FILE_NAME_OLD = fileNameOld,
                FILE_NAME_NEW = null,
                PATH_OLD = filePathOld,
                PATH_NEW = null,
                FILE_SIZE = null,
                FILE_TYPE = null,
                ATTACH_DT = DateTime.Now,
                EMP_ID = GetCurrentUserName(),
                INDEX = index,
                NOTES = notes
            };
        }
        protected List<string> GetFilesPhysics(CM_ATTACH_FILE_ENTITY attachFile)
        {
            if (attachFile == null)
            {
                return new List<string>();
            }
            var names = (attachFile?.FILE_NAME_NEW ?? "").Split("|");
            var paths = (attachFile?.PATH_NEW ?? "").Split("|");
            var m = Math.Min(names.Length, paths.Length);
            var result = new List<string>();
            for (var i = 0; i < m; i++)
            {
                result.Add(paths[i] + "/" + names[i]);
            }
            return result;
        }

        protected async Task<string> GetBase64StringAsync(string path)
        {
            try
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(path);
                return Convert.ToBase64String(imageArray);
            }
            catch (Exception e)
            {
                return "";
            }
        }


        protected async Task<InsertResult> CM_ATTACH_FILE_Ins_New(CM_ATTACH_FILE_ENTITY attachFile, List<CM_ATTACH_FILE_ENTITY> childs, string ids)
        {
            CM_ATTACH_FILE attachf = new CM_ATTACH_FILE();
            var fileNames = string.Join("|", GetFilesPhysics(attachFile));
            attachf.FILE_ATTACHMENT = fileNames;
            attachf.FILE_ATTACHMENT_OLD = attachFile.FILE_NAME_OLD;
            attachf.ATTACH_ID = attachFile.ATTACH_ID;
            attachf.TYPE = attachFile.TYPE;
            attachf.INDEX = attachFile.INDEX;

            List<CM_ATTACH_FILE> cs = new List<CM_ATTACH_FILE>();
            foreach (var child in childs)
            {
                CM_ATTACH_FILE cattachf = new CM_ATTACH_FILE();
                fileNames = string.Join("|", GetFilesPhysics(child));
                cattachf.FILE_ATTACHMENT = fileNames;
                cattachf.FILE_ATTACHMENT_OLD = child.FILE_NAME_OLD;
                cattachf.ATTACH_ID = child.ATTACH_ID;
                cattachf.TYPE = child.TYPE;
                cattachf.INDEX = child.INDEX;
                cs.Add(cattachf);
            }

            return await CM_ATTACH_FILE_Ins(attachf, cs, ids);
        }

        protected async Task<InsertResult> CM_ATTACH_FILE_Ins<T>(ICM_ATTACH_FILE attachFile, List<T> childs = null, string ids = null) where T : class, ICM_ATTACH_FILE
        {
            if (ids.IsNullOrWhiteSpace())
            {
                return null;
            }

            var idLists = new List<string>();
            idLists = ids.Split(",").ToList();

            var masterId = idLists.First();

            idLists = idLists.Skip(1).ToList();

            CM_ATTACH_FILE_ENTITY attachFileModel = new CM_ATTACH_FILE_ENTITY();

            attachFile.FILE_ATTACHMENT = string.Join("|", (attachFile.FILE_ATTACHMENT ?? "").Split("|").Select(x => x.Trim()));
            List<string> listFileAttachment = attachFile.FILE_ATTACHMENT != null ? attachFile.FILE_ATTACHMENT.Split('|').ToList() : new List<string>();
            if (listFileAttachment.Count == 0)
            {
                listFileAttachment.Add("");
            }
            List<string> listFileAttachmentOld = attachFile.FILE_ATTACHMENT_OLD != null ? attachFile.FILE_ATTACHMENT_OLD.Split('|').ToList() : new List<string>();
            if (listFileAttachmentOld.Count == 0)
            {
                listFileAttachmentOld.Add("");
            }
            var m = Math.Min(listFileAttachment.Count, listFileAttachmentOld.Count);
            for (var i = 0; i < m; i++)
            {
                var item = listFileAttachment[i];
                var NewAttachFileModel = CreateAttachFileEntity(attachFile.ATTACH_ID, attachFile.TYPE, masterId, item, listFileAttachmentOld[i]);
                attachFileModel = new CM_ATTACH_FILE_ENTITY()
                {
                    ATTACH_ID = NewAttachFileModel.ATTACH_ID,
                    TYPE = NewAttachFileModel.TYPE,
                    REF_ID = NewAttachFileModel.REF_ID,
                    FILE_NAME_OLD = attachFileModel.FILE_NAME_OLD + "|" + NewAttachFileModel.FILE_NAME_OLD,
                    FILE_NAME_NEW = attachFileModel.FILE_NAME_NEW + "|" + NewAttachFileModel.FILE_NAME_NEW,
                    PATH_OLD = attachFileModel.PATH_OLD + "|" + NewAttachFileModel.PATH_OLD,
                    PATH_NEW = attachFileModel.PATH_NEW + "|" + NewAttachFileModel.PATH_NEW,
                    FILE_SIZE = attachFileModel.FILE_SIZE + NewAttachFileModel.FILE_SIZE,
                    FILE_TYPE = attachFileModel.FILE_TYPE + "|" + NewAttachFileModel.FILE_TYPE,
                    ATTACH_DT = DateTime.Now,
                    EMP_ID = GetCurrentUserName(),
                    INDEX = attachFile.INDEX,
                    NOTES = NewAttachFileModel.NOTES
                };
            }


            if (childs != null && idLists.Count == childs.Count)
            {
                var fileChilds = new List<CM_ATTACH_FILE_ENTITY>();

                for (var i = 0; i < childs.Count; i++)
                {
                    CM_ATTACH_FILE_ENTITY fileDetail = new CM_ATTACH_FILE_ENTITY();
                    List<string> lstFile = childs[i].FILE_ATTACHMENT != null ? childs[i].FILE_ATTACHMENT.Split('|').ToList() : new List<string>();
                    List<string> lstFileOld = childs[i].FILE_ATTACHMENT_OLD != null ? childs[i].FILE_ATTACHMENT_OLD.Split('|').ToList() : new List<string>();

                    if (lstFile.Count == 0)
                    {
                        lstFile.Add("");
                    }

                    if (lstFileOld.Count == 0)
                    {
                        lstFileOld.Add("");
                    }

                    m = Math.Min(lstFile.Count, lstFileOld.Count);
                    for (var i2 = 0; i2 < m; i2++)
                    {
                        var item = lstFile[i2];
                        var fileOld = lstFileOld[i2];
                        var x = childs[i];

                        var att = CreateAttachFileEntity(attachFile.ATTACH_ID, x.TYPE, idLists[i], item, lstFileOld[i2]);
                        fileDetail = new CM_ATTACH_FILE_ENTITY()
                        {
                            ATTACH_ID = att.ATTACH_ID,
                            TYPE = att.TYPE,
                            REF_ID = att.REF_ID,
                            FILE_NAME_OLD = fileDetail.FILE_NAME_OLD + "|" + fileOld,
                            FILE_NAME_NEW = fileDetail.FILE_NAME_NEW + "|" + att.FILE_NAME_NEW,
                            PATH_OLD = fileDetail.PATH_OLD + "|" + att.PATH_OLD,
                            PATH_NEW = fileDetail.PATH_NEW + "|" + att.PATH_NEW,
                            FILE_SIZE = fileDetail.FILE_SIZE + att.FILE_SIZE,
                            FILE_TYPE = fileDetail.FILE_TYPE + "|" + att.FILE_TYPE,
                            ATTACH_DT = DateTime.Now,
                            EMP_ID = GetCurrentUserName(),
                            INDEX = childs[i].INDEX,
                            NOTES = att.NOTES
                        };
                    }
                    fileDetail.FILE_NAME_OLD = !fileDetail.FILE_NAME_OLD.IsNullOrEmpty() ? fileDetail.FILE_NAME_OLD.Substring(1, fileDetail.FILE_NAME_OLD.Length - 1) : "";
                    fileDetail.FILE_NAME_NEW = !fileDetail.FILE_NAME_NEW.IsNullOrEmpty() ? fileDetail.FILE_NAME_NEW.Substring(1, fileDetail.FILE_NAME_NEW.Length - 1) : "";
                    fileDetail.PATH_OLD = !fileDetail.PATH_OLD.IsNullOrEmpty() ? fileDetail.PATH_OLD.Substring(1, fileDetail.PATH_OLD.Length - 1) : "";
                    fileDetail.PATH_NEW = !fileDetail.PATH_NEW.IsNullOrEmpty() ? fileDetail.PATH_NEW.Substring(1, fileDetail.PATH_NEW.Length - 1) : "";
                    fileDetail.FILE_TYPE = !fileDetail.FILE_TYPE.IsNullOrEmpty() ? fileDetail.FILE_TYPE.Substring(1, fileDetail.FILE_TYPE.Length - 1) : "";

                    fileDetail.REF_MASTER = masterId;
                    fileChilds.Add(fileDetail);
                }

                attachFileModel.FILE_NAME_OLD = !attachFileModel.FILE_NAME_OLD.IsNullOrEmpty() ? attachFileModel.FILE_NAME_OLD.Substring(1, attachFileModel.FILE_NAME_OLD.Length - 1) : "";

                attachFileModel.FILE_NAME_NEW = !attachFileModel.FILE_NAME_NEW.IsNullOrEmpty() ? attachFileModel.FILE_NAME_NEW.Substring(1, attachFileModel.FILE_NAME_NEW.Length - 1) : "";

                attachFileModel.PATH_OLD = !attachFileModel.PATH_OLD.IsNullOrEmpty() ? attachFileModel.PATH_OLD.Substring(1, attachFileModel.PATH_OLD.Length - 1) : "";
                attachFileModel.PATH_NEW = !attachFileModel.PATH_NEW.IsNullOrEmpty() ? attachFileModel.PATH_NEW.Substring(1, attachFileModel.PATH_NEW.Length - 1) : "";
                attachFileModel.FILE_TYPE = !attachFileModel.FILE_TYPE.IsNullOrEmpty() ? attachFileModel.FILE_TYPE.Substring(1, attachFileModel.FILE_TYPE.Length - 1) : "";
                attachFileModel.REF_MASTER = masterId;

                attachFileModel.AttachDetail = global::Core.gAMSPro.CoreModule.Utils.XmlHelper.ToXmlFromList(fileChilds);
            }

            var result = (await storeProcedureProvider
            .GetDataFromStoredProcedure<InsertResult>(CoreStoreProcedureConsts.CM_ATTACH_FILE_INS, attachFileModel)).FirstOrDefault();
            return result;
        }

        protected async Task<InsertResult> CM_ATTACH_FILE_Upd_New(CM_ATTACH_FILE_ENTITY attachFile, List<CM_ATTACH_FILE_ENTITY> childs, string ids)
        {
            CM_ATTACH_FILE attachf = new CM_ATTACH_FILE();
            var fileNames = string.Join("|", GetFilesPhysics(attachFile));
            attachf.FILE_ATTACHMENT = fileNames;
            if (attachFile != null)
            {
                attachf.FILE_ATTACHMENT_OLD = attachFile.FILE_NAME_OLD;
                attachf.ATTACH_ID = attachFile.ATTACH_ID;
                attachf.INDEX = attachFile.INDEX;
                attachf.TYPE = attachFile.TYPE;
            }


            List<CM_ATTACH_FILE> cs = new List<CM_ATTACH_FILE>();
            foreach (var child in childs)
            {
                CM_ATTACH_FILE cattachf = new CM_ATTACH_FILE();
                fileNames = string.Join("|", GetFilesPhysics(child));
                cattachf.FILE_ATTACHMENT = fileNames;
                cattachf.FILE_ATTACHMENT_OLD = child.FILE_NAME_OLD;
                cattachf.ATTACH_ID = child.ATTACH_ID;
                cattachf.TYPE = child.TYPE;
                cattachf.INDEX = child.INDEX;
                cs.Add(cattachf);
            }

            return await CM_ATTACH_FILE_Upd(attachf, cs, ids);
        }

        protected async Task<InsertResult> CM_ATTACH_FILE_Upd<T>(ICM_ATTACH_FILE attachFile, List<T> childs = null, string ids = null) where T : class, ICM_ATTACH_FILE
        {
            if (ids.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (attachFile.ATTACH_ID.IsNullOrWhiteSpace())
            {
                return await CM_ATTACH_FILE_Ins(attachFile, childs, ids);
            }

            var idLists = new List<string>();
            idLists = ids.Split(",").ToList();

            var masterId = idLists.First();

            idLists = idLists.Skip(1).ToList();

            CM_ATTACH_FILE_ENTITY attachFileModel = CreateAttachFileEntity(attachFile.ATTACH_ID, attachFile.TYPE, masterId, null, null);
            attachFile.FILE_ATTACHMENT = string.Join("|", (attachFile.FILE_ATTACHMENT ?? "").Split("|").Select(x => x.Trim()));
            attachFile.FILE_ATTACHMENT_OLD = string.Join("|", (attachFile.FILE_ATTACHMENT_OLD ?? "").Split("|").Select(x => x.Trim()));
            List<string> listFileAttachment = attachFile.FILE_ATTACHMENT != null ? attachFile.FILE_ATTACHMENT.Split('|').ToList() : new List<string>();
            if (listFileAttachment.Count == 0)
            {
                listFileAttachment.Add("");
            }
            List<string> listFileAttachmentOld = attachFile.FILE_ATTACHMENT_OLD != null ? attachFile.FILE_ATTACHMENT_OLD.Split('|').ToList() : new List<string>();
            if (listFileAttachmentOld.Count == 0)
            {
                listFileAttachmentOld.Add("");
            }
            var m = Math.Min(listFileAttachment.Count, listFileAttachmentOld.Count);

            for (var i = 0; i < m; i++)
            {
                var item = listFileAttachment[i];
                var fileOld = listFileAttachmentOld[i];
                var NewAttachFileModel = CreateAttachFileEntity(attachFile.ATTACH_ID, attachFile.TYPE, masterId, item, listFileAttachmentOld[i]);
                attachFileModel = new CM_ATTACH_FILE_ENTITY()
                {
                    ATTACH_ID = NewAttachFileModel.ATTACH_ID,
                    TYPE = NewAttachFileModel.TYPE,
                    REF_ID = NewAttachFileModel.REF_ID,
                    FILE_NAME_OLD = attachFileModel.FILE_NAME_OLD + "|" + fileOld,
                    FILE_NAME_NEW = attachFileModel.FILE_NAME_NEW + "|" + NewAttachFileModel.FILE_NAME_NEW,
                    PATH_NEW = attachFileModel.PATH_NEW + "|" + NewAttachFileModel.PATH_NEW,
                    FILE_SIZE = attachFileModel.FILE_SIZE ?? 0 + NewAttachFileModel.FILE_SIZE ?? 0,
                    FILE_TYPE = attachFileModel.FILE_TYPE + "|" + NewAttachFileModel.FILE_TYPE,
                    ATTACH_DT = DateTime.Now,
                    EMP_ID = GetCurrentUserName(),
                    INDEX = attachFile.INDEX,
                    NOTES = NewAttachFileModel.NOTES
                };
            }
            if (childs != null && idLists.Count == childs.Count)
            {
                var fileChilds = new List<CM_ATTACH_FILE_ENTITY>();

                for (var i = 0; i < childs.Count; i++)
                {
                    CM_ATTACH_FILE_ENTITY fileDetail = new CM_ATTACH_FILE_ENTITY();

                    childs[i].FILE_ATTACHMENT = string.Join("|", (childs[i].FILE_ATTACHMENT ?? "").Split("|").Select(x => x.Trim()));
                    childs[i].FILE_ATTACHMENT_OLD = string.Join("|", (childs[i].FILE_ATTACHMENT_OLD ?? "").Split("|").Select(x => x.Trim()));

                    List<string> lstFile = childs[i].FILE_ATTACHMENT != null ? (childs[i].FILE_ATTACHMENT ?? "").Split('|').ToList() : new List<string>();
                    List<string> lstFileOld = childs[i].FILE_ATTACHMENT_OLD != null ? (childs[i].FILE_ATTACHMENT_OLD ?? "").Split('|').ToList() : new List<string>();

                    foreach (var item in lstFile)
                    {
                        var x = childs[i];

                        var att = CreateAttachFileEntity(x.ATTACH_ID, x.TYPE, idLists[i], item, item);
                        fileDetail = new CM_ATTACH_FILE_ENTITY()
                        {
                            ATTACH_ID = att.ATTACH_ID,
                            TYPE = att.TYPE,
                            REF_ID = att.REF_ID,
                            FILE_NAME_NEW = fileDetail.FILE_NAME_NEW + "|" + att.FILE_NAME_NEW,
                            PATH_NEW = fileDetail.PATH_NEW + "|" + att.PATH_NEW,
                            FILE_SIZE = fileDetail.FILE_SIZE + att.FILE_SIZE,
                            FILE_TYPE = fileDetail.FILE_TYPE + "|" + att.FILE_TYPE,
                            ATTACH_DT = DateTime.Now,
                            EMP_ID = GetCurrentUserName(),
                            INDEX = childs[i].INDEX,
                            NOTES = att.NOTES
                        };
                    }
                    foreach (var item in lstFileOld)
                    {
                        fileDetail.FILE_NAME_OLD = fileDetail.FILE_NAME_OLD + "|" + item;

                    }
                    fileDetail.FILE_NAME_OLD = !fileDetail.FILE_NAME_OLD.IsNullOrEmpty() ? fileDetail.FILE_NAME_OLD.Substring(1, fileDetail.FILE_NAME_OLD.Length - 1) : "";
                    fileDetail.FILE_NAME_NEW = !fileDetail.FILE_NAME_NEW.IsNullOrEmpty() ? fileDetail.FILE_NAME_NEW.Substring(1, fileDetail.FILE_NAME_NEW.Length - 1) : "";
                    fileDetail.PATH_OLD = !fileDetail.PATH_OLD.IsNullOrEmpty() ? fileDetail.PATH_OLD.Substring(1, fileDetail.PATH_OLD.Length - 1) : "";
                    fileDetail.PATH_NEW = !fileDetail.PATH_NEW.IsNullOrEmpty() ? fileDetail.PATH_NEW.Substring(1, fileDetail.PATH_NEW.Length - 1) : "";
                    fileDetail.FILE_TYPE = !fileDetail.FILE_TYPE.IsNullOrEmpty() ? fileDetail.FILE_TYPE.Substring(1, fileDetail.FILE_TYPE.Length - 1) : "";

                    fileDetail.REF_MASTER = masterId;
                    fileChilds.Add(fileDetail);
                }
                attachFileModel.FILE_NAME_OLD = !attachFileModel.FILE_NAME_OLD.IsNullOrEmpty() ? attachFileModel.FILE_NAME_OLD.Substring(1, attachFileModel.FILE_NAME_OLD.Length - 1) : "";
                attachFileModel.FILE_NAME_NEW = !attachFileModel.FILE_NAME_NEW.IsNullOrEmpty() ? attachFileModel.FILE_NAME_NEW.Substring(1, attachFileModel.FILE_NAME_NEW.Length - 1) : "";
                attachFileModel.PATH_OLD = !attachFileModel.PATH_OLD.IsNullOrEmpty() ? attachFileModel.PATH_OLD.Substring(1, attachFileModel.PATH_OLD.Length - 1) : "";
                attachFileModel.PATH_NEW = !attachFileModel.PATH_NEW.IsNullOrEmpty() ? attachFileModel.PATH_NEW.Substring(1, attachFileModel.PATH_NEW.Length - 1) : "";
                attachFileModel.FILE_TYPE = !attachFileModel.FILE_TYPE.IsNullOrEmpty() ? attachFileModel.FILE_TYPE.Substring(1, attachFileModel.FILE_TYPE.Length - 1) : "";

                attachFileModel.AttachDetail = global::Core.gAMSPro.CoreModule.Utils.XmlHelper.ToXmlFromList(fileChilds);
            }

            var result = (await storeProcedureProvider
            .GetDataFromStoredProcedure<InsertResult>(CoreStoreProcedureConsts.CM_ATTACH_FILE_UPD, attachFileModel)).FirstOrDefault();
            return result;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }

        protected void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }

        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }

        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }
        }

        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            tempFileCacheManager.SetFile(file.FileToken, excelPackage.GetAsByteArray());
        }

        public async Task<string> GetFunctionId(string permission)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<TL_MENU_GET_MENU_NAME_Result>(CoreStoreProcedureConsts.TL_MENU_GET_MENU_NAME, new
            {
                MENU_NAME_EL = permission
            })).FirstOrDefault()?.MENU_LINK;
        }
    }

}