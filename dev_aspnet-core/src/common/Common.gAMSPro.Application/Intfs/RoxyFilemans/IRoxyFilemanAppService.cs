using Common.gAMSPro.Intfs.RoxyFilemans.Dto;
using Microsoft.AspNetCore.Http;

namespace Common.gAMSPro.Intfs.RoxyFilemans
{
    public interface IRoxyFilemanAppService
    {
        Task<StringResult> COPYDIR(string d, string n, ISession session);
        Task<StringResult> COPYFILE(string f, string n, ISession session);
        Task<StringResult> CREATEDIR(string d, string n, ISession session);
        Task<StringResult> DELETEDIR(string d, ISession session);
        Task<StringResult> DELETEFILE(string f, ISession session);
        Task<StringResult> DIRLIST(string type, string functionFolder, ISession session);
        Task<FileResultDto> DOWNLOAD(string f, ISession session);
        Task<FileResultDto> DOWNLOADDIR(string d);
        Task<StringResult> FILESLIST(string d, string type, ISession session);
        Task<StringResult> MOVEDIR(string d, string n, ISession session);
        Task<StringResult> MOVEFILE(string f, string n, ISession session);
        Task<StringResult> RENAMEDIR(string d, string n, ISession session);
        Task<StringResult> RENAMEFILE(string f, string n, ISession session);
        Task<StringResult> UPLOAD(string d, IFormFileCollection files, ISession session, bool isAjaxUpload);
        Task<UPLOAD_W_T_RESULT> UPLOAD_W_T(string d, string g, IFormFileCollection files, ISession session, bool isAjaxUpload);
        Task<string> UPLOAD_W_T_HTML(IFormFileCollection files);
        Task<UPLOAD_W_T_RESULT> UPLOAD_SYSTEM(string d, IFormFileCollection files, ISession session, bool isAjaxUpload);
    }
}
