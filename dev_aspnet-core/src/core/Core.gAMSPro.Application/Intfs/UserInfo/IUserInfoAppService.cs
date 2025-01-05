using Abp.Application.Services;
using Aspose.Cells;
using Aspose.Words;
using Core.gAMSPro.Intfs.UserInfo.Dto;
using System.Data;
using System.Threading.Tasks;

namespace Core.gAMSPro.Intfs.UserInfo
{
    public interface IUserInfoAppService : IApplicationService
    {
        Task<UserInfoResponseEntity> GetUserInfo(string userName);
        public Task<string> GetImageFromUserInfo(string userName);
        public Task AddImageToExcel(WorkbookDesigner designWord, DataSet data);
        public Task AddImageToWord(Document doc, DataSet data);
        public void SaveImageFromBase64(string base64, string folderName);
        public void AddQRCodeToWord(string path, Document doc);
        public void AddQRCodeToExcel(string path, WorkbookDesigner designWord);
    }
}
