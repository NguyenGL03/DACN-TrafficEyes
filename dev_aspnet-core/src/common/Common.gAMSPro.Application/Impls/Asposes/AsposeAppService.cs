using Abp.Application.Services;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.UI;
using Common.gAMSPro.Intfs.Report;
using Common.gAMSPro.Web.Controllers.Intfs.Asposes;
using gAMSPro.Configuration;
using gAMSPro.Dto;
using gAMSPro.Helper;
using gAMSPro.ProcedureHelpers;
using gAMSPro.Storage;
using GSOFTcore.gAMSPro.Report.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Common.gAMSPro.Intfs.AsposeSample
{
    [AbpAuthorize]
    public class AsposeAppService : ApplicationService, IAsposeAppService
    {
        private readonly IReportExporter _customReportFile;
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        private readonly IDetailLoggerHelper detailLoggerHelper;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly string _systemRootPath;
        private readonly IConfigurationRoot _appConfiguration;

        public AsposeAppService(
            IReportExporter _customReportFile,
            ITempFileCacheManager _tempFileCacheManager,
            IStoreProcedureProvider _storeProcedureProvider,
            IDetailLoggerHelper detailLoggerHelper,
            IWebHostEnvironment env
        )
        {
            this._tempFileCacheManager = _tempFileCacheManager;
            this._customReportFile = _customReportFile;
            this._storeProcedureProvider = _storeProcedureProvider;
            this.detailLoggerHelper = detailLoggerHelper;
            _appConfiguration = env.GetAppConfiguration();
            _systemRootPath = _appConfiguration["App:RootDirectory"];
            if (_systemRootPath.Length == 0)
            {
                _systemRootPath = env.ContentRootPath;
            }
        }

        public async Task<FileDto> GetReport(ReportInfo info)
        {
            try
            {
                if (info.PathName != "")
                {
                    info.PathName = info.PathName.Replace(@"/..", @"").Replace(@"..", @"/").Replace(@"//", @"/");
                }
                //var key = detailLoggerHelper.StartLog("/api/Aspose/GetReport");
                var reportByteArray = await _customReportFile.GetReportFile(info);
                FileDto file = new FileDto();

                var fileName = info.PathName.Substring(info.PathName.LastIndexOf("/") + 1);

                if (info.FileName != null)
                {
                    fileName = info.FileName;
                }
                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "-" + Guid.NewGuid().ToString();

                switch (info.TypeExport.ToLower())
                {
                    case FileTypeConst.Excel:
                        file = new FileDto(fileName + ".xlsx", MimeTypeNames.ApplicationVndMsExcel);
                        break;
                    case FileTypeConst.Pdf:
                        file = new FileDto(fileName + ".pdf", MimeTypeNames.ApplicationPdf);
                        break;
                    case FileTypeConst.Word:
                        file = new FileDto(fileName + ".docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                        break;
                }
                //
                var path = "/download_files/temp";
                path = _systemRootPath + path;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string dest = path + "/" + file.FileName;
                var fileInfo = new FileInfo(file.FileName);
                reportByteArray.Position = 0;
                using (FileStream saveFile = new FileStream(dest, FileMode.Create))
                {
                    reportByteArray.CopyTo(saveFile);
                }
                //_tempFileCacheManager.SetFile(file.FileToken, reportByteArray.ToArray());
                //detailLoggerHelper.EndLog(key);
                return file;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }
        public async Task<FileDto> GetReportHaveQR(ReportInfo info)
        {
            try
            {
                if (info.PathName != "")
                {
                    info.PathName = info.PathName.Replace(@"/..", @"").Replace(@"..", @"/").Replace(@"//", @"/");
                }
                var fileName = info.PathName.Substring(info.PathName.LastIndexOf("/") + 1);

                if (info.FileName != null)
                {
                    fileName = info.FileName;
                }
                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "-" + Guid.NewGuid().ToString();

                var key = detailLoggerHelper.StartLog("/api/Aspose/GetReport");

                var reportByteArray = await _customReportFile.GetReportFile(info);

                FileDto file = new FileDto();

                switch (info.TypeExport.ToLower())
                {
                    case FileTypeConst.Excel:
                        file = new FileDto(fileName + ".xlsx", MimeTypeNames.ApplicationVndMsExcel);
                        break;
                    case FileTypeConst.Pdf:
                        file = new FileDto(fileName + ".pdf", MimeTypeNames.ApplicationPdf);
                        break;
                    case FileTypeConst.Word:
                        file = new FileDto(fileName + ".docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                        break;
                }
                //
                var path = "/download_files/temp";
                path = _systemRootPath + path;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string dest = path + "/" + file.FileName;
                var fileInfo = new FileInfo(file.FileName);
                reportByteArray.Position = 0;
                using (FileStream saveFile = new FileStream(dest, FileMode.Create))
                {
                    reportByteArray.CopyTo(saveFile);
                }
                // LƯU FILE QR CODE TRÊN SERVER
                //var path1 = _systemRootPath + "/download_files/QR_CODE/";
                //string dest1 = path + file.FileName;

                //if (!Directory.Exists(path1))
                //{
                //    Directory.CreateDirectory(path1);
                //}

                //var fileInfo1 = new FileInfo(file.FileName);
                //reportByteArray.Position = 0;
                //using (FileStream saveFile1 = new FileStream(dest1, FileMode.Create))
                //{
                //    reportByteArray.CopyTo(saveFile1);
                //}

                //_tempFileCacheManager.SetFile(file.FileToken, reportByteArray.ToArray());
                detailLoggerHelper.EndLog(key);
                return file;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }

        public async Task<FileDto> GetReportWordGroupFile(ReportInfo info, string groupId)
        {
            try
            {
                var key = detailLoggerHelper.StartLog("/api/Aspose/GetReport");

                var reportByteArray = await _customReportFile.GetReportWordGroupFile(info, groupId);
                FileDto file = new FileDto();

                var fileName = info.PathName.Substring(info.PathName.LastIndexOf("/") + 1);

                if (info.FileName != null)
                {
                    fileName = info.FileName;
                }
                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "-" + Guid.NewGuid().ToString();

                switch (info.TypeExport.ToLower())
                {
                    case FileTypeConst.Excel:
                        file = new FileDto(fileName + ".xlsx", MimeTypeNames.ApplicationVndMsExcel);
                        break;
                    case FileTypeConst.Pdf:
                        file = new FileDto(fileName + ".pdf", MimeTypeNames.ApplicationPdf);
                        break;
                    case FileTypeConst.Word:
                        file = new FileDto(fileName + ".docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                        break;
                }

                _tempFileCacheManager.SetFile(file.FileToken, reportByteArray.ToArray());
                detailLoggerHelper.EndLog(key);
                return file;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }

        public FileDto GetReportFromHTML(ReportHtmlInfo info)
        {
            var reportByteArray = _customReportFile.GetReportFileFromHtml(info);
            FileDto file = new FileDto();
            var fileName = info.FileName + "-" + Guid.NewGuid().ToString();
            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    file = new FileDto(fileName + ".xlsx", MimeTypeNames.ApplicationVndMsExcel);
                    break;
                case FileTypeConst.Pdf:
                    file = new FileDto(fileName + ".pdf", MimeTypeNames.ApplicationPdf);
                    break;
                case FileTypeConst.Word:
                    file = new FileDto(fileName + ".docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                    break;
            }

            _tempFileCacheManager.SetFile(file.FileToken, reportByteArray.ToArray());
            return file;
        }
        //Tu begin
        public async Task<List<ReportTable>> GetDataFromStore(ReportInfo store)
        {
            try
            {
                var dataSet = await _storeProcedureProvider.GetMultiDataFromStoredProcedure(store.StoreName, store.Parameters);

                ReportTable valueTable = new ReportTable();

                ReportRow rows = new ReportRow();
                List<string> rowData = new List<string>();
                valueTable.TableName = "table0";

                foreach (var item in store.Values)
                {
                    ReportColumn col = new ReportColumn
                    {
                        ColName = item.Name,
                        KeyName = ReportTemplateConst.OpenKey + valueTable.TableName + "." + item.Name + ReportTemplateConst.CloseKey
                    };
                    valueTable.Columns.Add(col);

                    rows.Cells.Add(item.Value?.ToString());
                }
                valueTable.Rows.Add(rows);

                List<ReportTable> lstData = DataTableHelper.ConvertDatasetToList(dataSet);

                lstData.Insert(0, valueTable);



                //List<List<List<ReportNoteDto>>> lstData = new List<List<List<ReportNoteDto>>>();
                //foreach (DataTable item in dataSet.Tables)
                //{

                //    var tmp = DataTableHelper.ConvertDatatableToList(item);
                //    lstData.Add(tmp);
                //}


                return lstData;
            }
            catch (Exception e) { throw e; }
        }
        public async Task<FileDto> GetReportCustomFomart(ReportInfo info)
        {
            try
            {
                var reportByteArray = await _customReportFile.GetReportFileCustomFomart(info);
                return CreateFileDtoFromStream(info, reportByteArray);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }
        //truongnv
        public async Task<FileDto> GetReport_BCKH_CustomFomart(ReportInfo info)
        {
            try
            {
                var reportByteArray = await _customReportFile.GetReportFile_BCKH_CustomFomart(info);
                return CreateFileDtoFromStream(info, reportByteArray);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }
        private FileDto CreateFileDtoFromStream(ReportInfo info, System.IO.MemoryStream reportByteArray)
        {
            FileDto file = new FileDto();

            var fileName = info.PathName.Substring(info.PathName.LastIndexOf("/") + 1);

            if (info.FileName != null)
            {
                fileName = info.FileName;
            }
            fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "-" + Guid.NewGuid().ToString();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    file = new FileDto(fileName + ".xlsx", MimeTypeNames.ApplicationVndMsExcel);
                    break;
                case FileTypeConst.Pdf:
                    file = new FileDto(fileName + ".pdf", MimeTypeNames.ApplicationPdf);
                    break;
                case FileTypeConst.Word:
                    file = new FileDto(fileName + ".docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                    break;
            }
            _tempFileCacheManager.SetFile(file.FileToken, reportByteArray.ToArray());
            return file;
        }

        public async Task<FileDto> GetQRReport(ReportInfo info)
        {
            try
            {
                var key = detailLoggerHelper.StartLog("/api/Aspose/GetQRReport");

                var reportByteArray = await _customReportFile.GetReportFileQR(info);
                FileDto file = new FileDto();

                var fileName = info.PathName.Substring(info.PathName.LastIndexOf("/") + 1);

                if (info.FileName != null)
                {
                    fileName = info.FileName;
                }
                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "-" + Guid.NewGuid().ToString();

                switch (info.TypeExport.ToLower())
                {
                    case FileTypeConst.Excel:
                        file = new FileDto(fileName + ".xlsx", MimeTypeNames.ApplicationVndMsExcel);
                        break;
                    case FileTypeConst.Pdf:
                        file = new FileDto(fileName + ".pdf", MimeTypeNames.ApplicationPdf);
                        break;
                    case FileTypeConst.Word:
                        file = new FileDto(fileName + ".docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                        break;
                }

                _tempFileCacheManager.SetFile(file.FileToken, reportByteArray.ToArray());
                detailLoggerHelper.EndLog(key);
                return file;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }

        //06-06-23 Move từ BVNT
        public async Task<FileDto> GetReportMultiSheet(ReportInfo info)
        {
            try
            {
                var key = detailLoggerHelper.StartLog("/api/Aspose/GetReport");

                var reportByteArray = await _customReportFile.GetReportFileMultiSheet(info);
                FileDto file = new FileDto();

                var fileName = info.PathName.Substring(info.PathName.LastIndexOf("/") + 1);

                if (info.FileName != null)
                {
                    fileName = info.FileName;
                }
                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "-" + Guid.NewGuid().ToString();

                switch (info.TypeExport.ToLower())
                {
                    case FileTypeConst.Excel:
                        file = new FileDto(fileName + ".xlsx", MimeTypeNames.ApplicationVndMsExcel);
                        break;
                    case FileTypeConst.Pdf:
                        file = new FileDto(fileName + ".pdf", MimeTypeNames.ApplicationPdf);
                        break;
                    case FileTypeConst.Word:
                        file = new FileDto(fileName + ".docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                        break;
                }

                _tempFileCacheManager.SetFile(file.FileToken, reportByteArray.ToArray());
                detailLoggerHelper.EndLog(key);
                return file;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(message: e.Message);
            }
        }
        #region Private

        #endregion
        //Tu end
    }
}
