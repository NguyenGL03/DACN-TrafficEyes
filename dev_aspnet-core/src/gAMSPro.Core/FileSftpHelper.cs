using Abp.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GSOFTcore.gAMSPro.Core
{
    public interface IFilesFtpHelper
    {
        Task<bool> UploadContent_Server1(string fileName, string content);
        Task<bool> UploadContent_Server2(string fileName, string content);
        Task<bool> UploadImage_Server3(string fileName, string folderName);
    }
    public class FilesFtpHelper : IFilesFtpHelper
    {
        public FilesFtpHelper(ISettingManager settingManager)
        {
            this.settingManager = settingManager;
        }

        ISettingManager settingManager;

        public void Download()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<bool> UploadContent_Server1(string fileName, string content)
        {
            var sftpPath = Directory.GetCurrentDirectory() + "\\FileSftpTemp\\";

            if(!Directory.Exists(sftpPath))
            {
                Directory.CreateDirectory(sftpPath);
            }

            sftpPath += Guid.NewGuid().ToString() + ".txt";

            File.WriteAllText(sftpPath, content);

            string ftpurl = await settingManager.GetSettingValueAsync("gAMSProCore.SFTPURL");
            string ftpusername = await settingManager.GetSettingValueAsync("gAMSProCore.SFTPUserName");
            string ftppassword = await settingManager.GetSettingValueAsync("gAMSProCore.SFTPPassword");
            string ftpfilepath = await settingManager.GetSettingValueAsync("gAMSProCore.SFTPSFilePath");
            string portNumber = await settingManager.GetSettingValueAsync("gAMSProCore.SFTPPortNumber");
			string fileTransferMode = await settingManager.GetSettingValueAsync("gAMSProCore.SFTPTransferMode");



			return UploadFile(sftpPath, fileName, ftpurl, ftpusername, ftppassword, ftpfilepath, portNumber, fileTransferMode);
        }

        public async Task<bool> UploadContent_Server2(string fileName, string content)
        {
            var sftpPath = Directory.GetCurrentDirectory() + "\\FileSftpTemp\\";

            if (!Directory.Exists(sftpPath))
            {
                Directory.CreateDirectory(sftpPath);
            }

            sftpPath += Guid.NewGuid().ToString() + ".txt";

            File.WriteAllText(sftpPath, content);

            string ftpurl = await settingManager.GetSettingValueAsync("gAMSProCore.SFTP2URL");
            string ftpusername = await settingManager.GetSettingValueAsync("gAMSProCore.SFTP2UserName");
            string ftppassword = await settingManager.GetSettingValueAsync("gAMSProCore.SFTP2Password");
            string ftpfilepath = await settingManager.GetSettingValueAsync("gAMSProCore.SFTP2SFilePath");
            string portNumber = await settingManager.GetSettingValueAsync("gAMSProCore.SFTP2PortNumber");
			string fileTransferMode = await settingManager.GetSettingValueAsync("gAMSProCore.SFTP2TransferMode");

			return UploadFile(sftpPath, fileName, ftpurl, ftpusername, ftppassword, ftpfilepath, portNumber, fileTransferMode);
        }

        public async Task<bool> UploadImage_Server3 (string fileName, string folderName)
        {
            var sftpPath = Directory.GetCurrentDirectory() + "\\IMAGE_IMPORT";

            if (!Directory.Exists(sftpPath))
            {
                Directory.CreateDirectory(sftpPath);
            }

            sftpPath += "\\" + fileName;

            //File.WriteAllText(sftpPath, content);
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var connectionStrings = (JObject)appsettingsjson["FTPServer"];

            string ftpurl = connectionStrings["FTPUrl3"].ToString();
            string ftpusername = connectionStrings["FTPUser3"].ToString();
            string ftppassword = connectionStrings["FTPPass3"].ToString();
            string ftpfilepath = connectionStrings["FTPFOLDER3"].ToString();
            string portNumber = connectionStrings["FTPPort3"].ToString();
            string fileTransferMode = connectionStrings["FTPFileTransport3"].ToString();

            return UploadFile(sftpPath, fileName, ftpurl, ftpusername, ftppassword, ftpfilepath, portNumber, fileTransferMode, folderName);
        }


        public bool UploadFile(string sourcefilepath, string nameOfFile, string ftpurl, string ftpusername,string ftppassword, string ftpfilepath, string portNumber, string fileTransferMode, string folderName = null)
        {
            bool success = false;
            
            return success;
        }
    }
}
