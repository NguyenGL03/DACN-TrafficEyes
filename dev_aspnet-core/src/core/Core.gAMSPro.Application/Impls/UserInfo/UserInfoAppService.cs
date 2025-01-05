using Abp.Extensions;
using Aspose.Cells;
using Aspose.Cells.Drawing;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Replacing;
using Core.gAMSPro.Intfs.UserInfo;
using Core.gAMSPro.Intfs.UserInfo.Dto;
using gAMSPro.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Net.Codecrete.QrCodeGenerator;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using CellPageSetup = Aspose.Cells.PageSetup;
using CellSaveFormat = Aspose.Cells.SaveFormat;
using CellStyle = Aspose.Cells.Style;
using Task = System.Threading.Tasks.Task;
using WordSaveFormat = Aspose.Words.SaveFormat;

namespace Core.gAMSPro.Impls.UserInfo
{
    public class UserInfoAppService : IUserInfoAppService
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly string _fileRootPathImage;
        private readonly string _rootPath;
        private readonly string _tempReportFolder;
        private readonly string _replacingFolder;
        private readonly string _replacedFolder;

        public UserInfoAppService(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _fileRootPathImage = _appConfiguration["App:FileUploadTempPath"];
            _replacingFolder = _appConfiguration["App:ReplacingTempPath"];
            _replacedFolder = _appConfiguration["App:ReplacedTempPath"];
            _rootPath = env.WebRootPath;
            _tempReportFolder = _rootPath + _fileRootPathImage + "\\" + "tempReport";
        }
        public void SaveImageFromBase64(string base64, string folderName)
        {
            try
            {
                var path = _rootPath + "\\" + _fileRootPathImage + "\\" + "UserInfoImage" + "\\" + folderName;
                string fileName = folderName + "-signature";
                string fullPathFile = path + "\\" + fileName + ".png";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // Delete old file
                var files = Directory.GetFiles(path);
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }

                using (var imageFile = new FileStream(fullPathFile, FileMode.Create))
                {
                    var bytes = Convert.FromBase64String(base64);
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserInfoResponseEntity> GetUserInfo(string userName)
        {
            string userNameCheck = _appConfiguration["UserInfo:userNameCheck"];
            string passwordCheck = _appConfiguration["UserInfo:passwordCheck"];
            string url = $"{_appConfiguration["UserInfo:url"]}?userNameCheck={userNameCheck}&passWordCheck={passwordCheck}&username={userName}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                UserInfoResponseEntity contentParsed = JsonConvert.DeserializeObject<UserInfoResponseEntity>(content);
                if (response.IsSuccessStatusCode && contentParsed.status == 200 && !contentParsed.userInfor[0].signImage.IsNullOrEmpty())
                {
                    var userInfos = contentParsed.userInfor;

                    foreach (var item in userInfos)
                    {
                        SaveImageFromBase64(item.signImage, userName);
                    }
                }
                return contentParsed;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                client.Dispose();
            }

        }

        public async Task<string> GetImageFromUserInfo(string userName)
        {
            try
            {
                if (userName.IsNullOrEmpty()) return null;
                var path = _rootPath + "\\" + _fileRootPathImage + "\\" + "UserInfoImage" + "\\" + userName;
                if (!Directory.Exists(path))
                    await GetUserInfo(userName);

                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path)
                    .Select(file => Path.GetFullPath(file)).ToArray();
                    if (files.Length > 0) return files[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddImageToExcel(WorkbookDesigner designWord, DataSet data)
        {
            try
            {
                WorksheetCollection sheetCollection = designWord.Workbook.Worksheets;
                foreach (var sheet in sheetCollection)
                {
                    for (int i = 0; i < data.Tables.Count; i++)
                    {
                        if (data.Tables[i].Columns.Count > 1 && data.Tables[i].Columns[1].ColumnName == "SIGN_IMG")
                        {
                            int rowCount = data.Tables[i].Rows.Count;
                            for (int j = 0; j < rowCount; j++)
                            {
                                if (!String.IsNullOrEmpty(data.Tables[i].Rows[j]["SIGN_IMG"].ToString()))
                                {
                                    string userName = data.Tables[i].Rows[j].Field<string>("SIGN_IMG");
                                    string content = data.Tables[i].Rows[j].Field<string>("CONTENT");
                                    // Instantiate FindOptions Object
                                    FindOptions findOptions = new FindOptions();
                                    findOptions.LookInType = LookInType.Values;
                                    findOptions.LookAtType = LookAtType.Contains;

                                    // Finding the cell containing the specified text
                                    Cell cell = sheet.Cells.Find(userName, null, findOptions);
                                    if (cell != null)
                                    {
                                        string imagePath = await GetImageFromUserInfo(userName);

                                        if (!imagePath.IsNullOrEmpty())
                                        {
                                            // read file image
                                            FileStream fs = File.OpenRead(imagePath);
                                            byte[] array = new byte[fs.Length];
                                            fs.Read(array, 0, array.Length);
                                            MemoryStream stream = new MemoryStream(array);

                                            CellStyle style = cell.GetStyle();
                                            style.VerticalAlignment = TextAlignmentType.Bottom;
                                            style.HorizontalAlignment = TextAlignmentType.Center;

                                            if (cell.IsMerged)
                                            {
                                                // set style
                                                var mergedRange = cell.GetMergedRange();
                                                StyleFlag flag = new StyleFlag();
                                                flag.Alignments = flag.VerticalAlignment = flag.HorizontalAlignment = true;
                                                mergedRange.ApplyStyle(style, flag);

                                                int index = sheet.Pictures.Add(
                                                    mergedRange.FirstRow,
                                                    mergedRange.FirstColumn,
                                                    mergedRange.FirstRow + mergedRange.RowCount,
                                                    mergedRange.FirstColumn + mergedRange.ColumnCount,
                                                    stream
                                                    );

                                                Picture pic = sheet.Pictures[index];
                                                pic.Height -= 70;
                                                pic.Placement = PlacementType.MoveAndSize;
                                            }
                                            else
                                            {
                                                // set style
                                                cell.SetStyle(style);

                                                // specify posision
                                                int imgRow = cell.Row;
                                                int imgColumn = cell.Column;

                                                int index = sheet.Pictures.Add(imgRow, imgColumn, stream);
                                                Picture pic = sheet.Pictures[index];
                                                pic.Placement = PlacementType.MoveAndSize;
                                            }
                                        }
                                        //sheet.Replace(userName, content);
                                        cell.PutValue(content);
                                    }
                                }
                            }
                        }
                    }
                }
                designWord.Process(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddImageToWord(Document doc, DataSet data)
        {
            try
            {
                for (int i = 0; i < data.Tables.Count; i++)
                {
                    if (data.Tables[i].Columns.Count > 1 && data.Tables[i].Columns[1].ColumnName == "SIGN_IMG")
                    {
                        int rowCount = data.Tables[i].Rows.Count;
                        for (int j = 0; j < rowCount; j++)
                        {
                            string userName = data.Tables[i].Rows[j].Field<string>("SIGN_IMG");
                            string image = await GetImageFromUserInfo(userName);

                            if (!image.IsNullOrEmpty())
                            {
                                FileStream fs = File.OpenRead(image);
                                byte[] array = new byte[fs.Length];
                                fs.Read(array, 0, array.Length);
                                MemoryStream stream = new MemoryStream(array);
                                FindReplaceOptions options = new FindReplaceOptions();
                                options.ReplacingCallback = new ReplaceWithImageEvaluator(stream, userName);
                                doc.Range.Replace(userName, userName, options);
                            }
                            else doc.Range.Replace(userName, "", new FindReplaceOptions());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddQRCodeToWord(string fileName, Document doc)
        {
            if (!Directory.Exists(_tempReportFolder))
                Directory.CreateDirectory(_tempReportFolder);

            if (fileName == null || fileName.IsNullOrEmpty())
                fileName = "download-file";

            string guid = Guid.NewGuid().ToString();
            string path = _tempReportFolder + "\\" + fileName + "-" + guid + ".pdf";
            // Chỉnh đường dẫn từ QR
            path.Replace("/", "\\");

            string qrPath = path.Replace(this._replacedFolder, this._replacingFolder);

            DocumentBuilder builder = new DocumentBuilder(doc);
            var data = GenerateQrCode(qrPath);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            MemoryStream stream = new MemoryStream(bytes);
            builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary);
            builder.InsertImage(stream,
                RelativeHorizontalPosition.Margin,
                410,
                RelativeVerticalPosition.BottomMargin,
                100,
                85,
                85,
                WrapType.Inline);
            doc.Save(path, WordSaveFormat.Pdf);
        }

        public void AddQRCodeToExcel(string fileName, WorkbookDesigner designWord)
        {
            if (!Directory.Exists(_tempReportFolder))
                Directory.CreateDirectory(_tempReportFolder);

            if (fileName == null || fileName.IsNullOrEmpty())
                fileName = "download-file";

            string guid = Guid.NewGuid().ToString();
            string path = _tempReportFolder + "\\" + fileName + "-" + guid + ".pdf";
            path = path.Replace("/", "\\");
            // Chỉnh đường dẫn từ QR
            string qrPath = path.Replace(this._replacedFolder, this._replacingFolder).Replace("\\", "/"); ;

            var qr = QrCode.EncodeText(qrPath, QrCode.Ecc.Medium);
            var data = qr.ToBmpBitmap(4, 2);
            string QRFolderPath = _rootPath + "\\" + _fileRootPathImage + "\\" + "QRTemp";

            if (!Directory.Exists(QRFolderPath)) Directory.CreateDirectory(QRFolderPath);
            string QRguid = Guid.NewGuid().ToString();
            string pathQR = QRFolderPath + $"\\{QRguid}-tempQR.bmp";

            File.WriteAllBytes(pathQR, data);

            byte[] pngData = File.ReadAllBytes(pathQR);

            WorksheetCollection sheetCollection = designWord.Workbook.Worksheets;

            foreach (var sheet in sheetCollection)
            {
                CellPageSetup pageSetup = sheet.PageSetup;
                Picture pic = pageSetup.SetHeaderPicture(2, pngData);
                pic.ToFrontOrBack(1);
                pageSetup.SetHeader(2, "&G");
                //pageSetup.HeaderMargin = -3;
            }
            designWord.Workbook.Save(path, CellSaveFormat.Pdf);
            //File.Delete(pathQR);
        }

        private static string GenerateQrCode(string data)
        {
            var qr = QrCode.EncodeText(data, QrCode.Ecc.Medium);
            return qr.ToSvgString(4);
        }
    }

}


