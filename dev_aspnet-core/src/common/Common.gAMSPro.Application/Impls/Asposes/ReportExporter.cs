using Aspose.Cells;
using Aspose.Cells.Rendering;
using Aspose.Words;
using Aspose.Words.MailMerging;
using Common.gAMSPro.Intfs.Report;
using Core.gAMSPro.Intfs.UserInfo;
using gAMSPro.Configuration;
using gAMSPro.Dto;
using gAMSPro.Helpers;
using gAMSPro.ProcedureHelpers;
using GSOFTcore.gAMSPro.Report.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text.RegularExpressions;

using FileTypeConst = GSOFTcore.gAMSPro.Report.Dto.FileTypeConst;
using ReportInfo = GSOFTcore.gAMSPro.Report.Dto.ReportInfo;

namespace Common.gAMSPro.Intfs.Report
{
    public class ReportExporter : IReportExporter
    {
        private readonly string directionReport;
        private readonly string fileUploadQRPath;

        private readonly IStoreProcedureProvider _storeProcedureProvider;
        private readonly IConfigurationRoot appConfiguration;
        private readonly IUserInfoAppService _userInfoAppService;
        private readonly string _systemRootPath;

        public ReportExporter(IStoreProcedureProvider storeProc, IWebHostEnvironment env, IUserInfoAppService userInfoAppService)
        {
            this._userInfoAppService = userInfoAppService;
            this._storeProcedureProvider = storeProc;
            this.appConfiguration = env.GetAppConfiguration();
            this._userInfoAppService = userInfoAppService;
            _systemRootPath = appConfiguration["App:RootDirectory"];
            if (_systemRootPath.Length == 0)
            {
                _systemRootPath = env.ContentRootPath;
            }
            directionReport = _systemRootPath + this.appConfiguration["App:ReportDirectory"];
        }

        private async Task<DataSet> GetDataFromStoreToReport(string storeName, List<ReportParameter> parameters)
        {
            try
            {
                DataSet data = await _storeProcedureProvider.GetMultiDataFromStoredProcedure(storeName, parameters);
                for (int i = 0; i < data.Tables.Count; i++)
                {
                    data.Tables[i].TableName = "table" + i;
                }

                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<WorkbookDesigner> CreateExcelFileAndDesign(ReportInfo info)
        {
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            Workbook designer = new Workbook(directionReport + info.PathName);
            WorkbookDesigner designWord = new WorkbookDesigner(designer);

            foreach (var item in info.Values)
            {
                designWord.SetDataSource(item.Name, item.Value);
            }

            int TableCount = data.Tables.Count;

            for (int i = 0; i < TableCount; i++)
            {
                int rows = data.Tables[i].Rows.Count;
                int cols = data.Tables[i].Columns.Count;

                for (int row = 0; row < rows; row++)
                    for (int col = 0; col < cols; col++)
                    {
                        var obj = data.Tables[i].Rows[row][col];
                        if (obj.GetType() == typeof(string))
                        {
                            string _value = obj.ToString();
                            bool check = _value.StartsWith('=') ||
                                    _value.StartsWith('+') ||
                                    _value.StartsWith('-') ||
                                    _value.StartsWith('@');

                            if (check)
                            {
                                data.Tables[i].Rows[row][col] = _value.Insert(0, "'");
                            }
                        }
                    }

            }

            designWord.SetDataSource(data);

            designWord.Process(false);
            designWord.Workbook.FileName = info.PathName;
            designWord.Workbook.FileFormat = FileFormatType.Xlsx;
            designWord.Workbook.Settings.CalcMode = CalcModeType.Automatic;
            designWord.Workbook.Settings.RecalculateBeforeSave = true;
            designWord.Workbook.Settings.ReCalculateOnOpen = true;
            designWord.Workbook.Settings.CheckCustomNumberFormat = true;
            designWord.Workbook.Worksheets[0].AutoFitRows();
            return designWord;
        }

        private async Task<MemoryStream> CreateExcelFile(ReportInfo info)
        {
            var designWord = await CreateExcelFileAndDesign(info);

            if (info.ProcessMerge == true)
            {
                ProcessMergeCell(designWord.Workbook);
            }

            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            await _userInfoAppService.AddImageToExcel(designWord, data);

            if (info.HaveQR == true)
            {
                _userInfoAppService.AddQRCodeToExcel(info.FileName, designWord);
            }

            MemoryStream str = new MemoryStream();
            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Pdf:
                    designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Pdf);
                    //doc.Save(stream, Aspose.Words.SaveFormat.Pdf);
                    break;
                case FileTypeConst.Excel:
                    designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Xlsx);
                    //doc.Save(stream, Aspose.Words.SaveFormat.Excel);
                    break;
            }
            return str;

        }

        void ProcessMergeCell(Workbook wb)
        {
            string startMergeMarkup = "StartMerge.";
            string endMergeMarkup = "EndMerge.";
            string mergeRow = "Merge.";
            string textMerge = "";
            foreach (var ws in wb.Worksheets)
            {
                int rowBegin, rowEnd, colBegin, colEnd;
                if (ws.Cells.FirstCell == null)
                {
                    continue;
                }
                rowBegin = ws.Cells.FirstCell.Row;
                rowEnd = ws.Cells.LastCell.Row;
                colBegin = ws.Cells.FirstCell.Column;
                colEnd = ws.Cells.MaxColumn;

                //var CellSStyle = ws.Cells["A"].GetStyle();
                //CellSStyle.
                //ws.Cells["A"].SetStyle()

                for (int rowIndex = rowBegin; rowIndex <= rowEnd; rowIndex++)
                {
                    int colStartMerge = -1, colEndMerge = -1;
                    for (int colIndex = colBegin; colIndex <= colEnd; colIndex++)
                    {
                        var cell = ws.Cells.Rows[rowIndex][colIndex];
                        if (cell.Value != null && cell.Value.ToString().StartsWith(startMergeMarkup))
                        {
                            colStartMerge = colIndex;
                            cell.Value = cell.Value.ToString().Substring(startMergeMarkup.Length);
                            textMerge = cell.Value.ToString();
                        }

                        if (cell.Value != null && cell.Value.ToString().StartsWith(endMergeMarkup) && colStartMerge >= 0)
                        {
                            colEndMerge = colIndex;
                            cell.Value = cell.Value.ToString().Substring(endMergeMarkup.Length);
                            textMerge += cell.Value;
                            var style = ws.Cells.Rows[rowIndex][colStartMerge].GetStyle();
                            ws.Cells.Merge(rowIndex, colStartMerge, 1, colEndMerge - colStartMerge + 1);

                            ws.Cells.Rows[rowIndex][colStartMerge].Value = textMerge;
                            ws.Cells.Rows[rowIndex][colStartMerge].SetStyle(style);
                            textMerge = "";
                        }
                    }
                }
                //mergeRow
                for (int colIndex = colBegin; colIndex <= colEnd; colIndex++)
                {
                    int rowStartMerge = -1, rowEndMerge = -1;
                    for (int rowIndex = rowBegin; rowIndex <= rowEnd; rowIndex++)
                    {
                        var cell = ws.Cells.Rows[rowIndex][colIndex];
                        if (cell.Value != null && cell.Value.ToString().StartsWith(mergeRow))
                        {
                            if (rowStartMerge == -1)
                            {
                                rowStartMerge = rowIndex;
                                cell.Value = cell.Value.ToString().Substring(mergeRow.Length);
                                textMerge = cell.Value.ToString();

                            }
                            var nextCell = ws.Cells.Rows[rowIndex + 1][colIndex];
                            if (nextCell.Value == null || !nextCell.Value.ToString().StartsWith(mergeRow) || nextCell.Value.ToString().Substring(mergeRow.Length) != textMerge)
                            {
                                rowEndMerge = rowIndex;
                                var style = ws.Cells.Rows[rowStartMerge][colIndex].GetStyle();
                                ws.Cells.Merge(rowStartMerge, colIndex, rowEndMerge - rowStartMerge + 1, 1);
                                ws.Cells.Rows[rowStartMerge][colIndex].Value = textMerge;
                                ws.Cells.Rows[rowStartMerge][colIndex].SetStyle(style);
                                textMerge = "";
                                rowStartMerge = -1;
                                rowEndMerge = -1;
                            }
                        }
                    }
                }
            }
        }

        private async Task<MemoryStream> CreateWordFile(ReportInfo info)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            //data.Relations.Add(new DataRelation("CustomerType_Customer", data.Tables[1].Columns["Id"], data.Tables[0].Columns["CustomerTypeCode"], true));

            Document doc = new Document(directionReport + info.PathName);

            doc.MailMerge.CleanupParagraphsWithPunctuationMarks = true;
            doc.MailMerge.CleanupOptions = MailMergeCleanupOptions.RemoveUnusedFields | MailMergeCleanupOptions.RemoveUnusedRegions;
            doc.MailMerge.Execute(info.Values.Select(x => x.Name).ToArray(), info.Values.Select(x => x.Value).ToArray());
            doc.MailMerge.ExecuteWithRegions(data);

            if (info.HaveQR == true)
            {
                _userInfoAppService.AddQRCodeToWord(info.FileName, doc);
            }

            await _userInfoAppService.AddImageToWord(doc, data);
            //_userInfoAppService.AddQRCodeToWord(info.FileName, doc);
            MemoryStream stream = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Pdf:
                    doc.Save(stream, Aspose.Words.SaveFormat.Pdf);
                    break;
                case FileTypeConst.Word:
                    doc.Save(stream, Aspose.Words.SaveFormat.Docx);
                    break;
            }
            return stream;

        }

        private async Task<MemoryStream> CreateWordFileQR(ReportInfo info)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            var qty = info.Parameters.FirstOrDefault(item => item.Name == "NUMQR").Value;
            var tmp = getDataPrintTemp(data.Tables[0], qty.ToString());
            tmp.TableName = "Newrpt";
            var newData = new DataSet();
            newData.Tables.Add(tmp);
            //data.Relations.Add(new DataRelation("CustomerType_Customer", data.Tables[1].Columns["Id"], data.Tables[0].Columns["CustomerTypeCode"], true));


            Document doc = new Document(directionReport + info.PathName);

            doc.MailMerge.FieldMergingCallback = new HandleMergeImageField();

            doc.MailMerge.CleanupParagraphsWithPunctuationMarks = true;
            doc.MailMerge.CleanupOptions = MailMergeCleanupOptions.RemoveUnusedFields | MailMergeCleanupOptions.RemoveUnusedRegions | MailMergeCleanupOptions.RemoveEmptyParagraphs;

            doc.MailMerge.Execute(info.Values.Select(x => x.Name).ToArray(), info.Values.Select(x => x.Value).ToArray());


            doc.MailMerge.ExecuteWithRegions(newData);


            MemoryStream stream = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Pdf:
                    doc.Save(stream, Aspose.Words.SaveFormat.Pdf);
                    break;
                case FileTypeConst.Word:
                    doc.Save(stream, Aspose.Words.SaveFormat.Docx);
                    break;
            }


            return stream;

        }

        public async Task<MemoryStream> GetReportFile(ReportInfo info)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    str = await CreateExcelFile(info);
                    break;
                case FileTypeConst.Pdf:
                    if (info.PathName.Split('.')[info.PathName.Split('.').Length - 1] == "xlsx")
                    {
                        str = await CreateExcelFile(info);
                    }
                    else
                    {
                        str = await CreateWordFile(info);
                    }
                    break;
                case FileTypeConst.Word:
                    str = await CreateWordFile(info);
                    break;
            }

            return str;
        }

        public MemoryStream GetFile(FileDto file)
        {
            MemoryStream memoryStream = new MemoryStream();
            var path = "/download_files/temp/";
            string dest = _systemRootPath + path + file.FileName;
            if (File.Exists(dest))
            {
                using (FileStream fileStream = new FileStream(dest, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }

                // Delete the file after reading it into memory
                File.Delete(dest);

                // Optional: Reset the position of the MemoryStream to the beginning
                memoryStream.Position = 0;

            }
            return memoryStream;
        }

        public async Task<MemoryStream> GetReportFileQR(ReportInfo info)
        {
            MemoryStream str = new MemoryStream();
            str = await CreateWordFileQR(info);

            //Phucvh 24/10/22 Move In nhãn từ An Bình
            //MemoryStream str = new MemoryStream();
            //str = await CreateWordFileQR_VB(info);

            return str;
        }

        public MemoryStream GetReportFileFromHtml(ReportHtmlInfo info)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {

                case FileTypeConst.Pdf:
                    str = CreateWordFileFromHtml(info);
                    break;
                case FileTypeConst.Word:
                    str = CreateWordFileFromHtml(info);
                    break;
            }




            return str;
        }
        private MemoryStream CreateWordFileFromHtml(ReportHtmlInfo info)
        {




            Document doc = new Document();
            //switch (info.PageSize)
            //{
            //    case "A3":
            //        foreach (Section section in doc.Sections)
            //        {
            //            section.PageSetup.PaperSize = Aspose.Words.PaperSize.A3;
            //        }
            //        break;
            //    case "A4":
            //        foreach (Section section in doc.Sections)
            //        {
            //            section.PageSetup.PaperSize = Aspose.Words.PaperSize.A4;
            //        }
            //        break;
            //}


            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.InsertHtml(info.HTMLString);

            doc.UpdatePageLayout();


            MemoryStream stream = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Pdf:
                    doc.Save(stream, Aspose.Words.SaveFormat.Pdf);
                    break;
                case FileTypeConst.Word:
                    doc.Save(stream, Aspose.Words.SaveFormat.Docx);
                    break;
            }

            return stream;

        }

        //truongnv
        public async Task<MemoryStream> GetReportFileCustomFomart(ReportInfo info)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    str = await CreateExcelFileCustomFomart(info);
                    break;
                case FileTypeConst.Pdf:
                    str = await CreateWordFile(info);
                    break;
                case FileTypeConst.Word:
                    str = await CreateWordFile(info);
                    break;
            }




            return str;
        }
        //truongnv
        public async Task<MemoryStream> GetReportFile_BCKH_CustomFomart(ReportInfo info)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    str = await CreateExcelFile_BCKH_CustomFomart(info);
                    break;
                case FileTypeConst.Pdf:
                    str = await CreateWordFile(info);
                    break;
                case FileTypeConst.Word:
                    str = await CreateWordFile(info);
                    break;
            }
            return str;
        }
        //truongnv
        private async Task<MemoryStream> CreateExcelFile_BCKH_CustomFomart(ReportInfo info)
        {
            var designWord = await CreateExcelFileAndDesign(info);
            Custom_BCKH_Fomart(designWord.Workbook);
            MemoryStream str = new MemoryStream();
            designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Xlsx);
            return str;
        }
        //truopngnv
        private async Task<MemoryStream> CreateExcelFileCustomFomart(ReportInfo info)
        {
            var designWord = await CreateExcelFileAndDesign(info);


            CustomFomart(designWord.Workbook);


            MemoryStream str = new MemoryStream();
            designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Xlsx);

            return str;

        }
        //truongnv
        void CustomFomart(Workbook wb)
        {
            string startMergeMarkup = "StartMerge.";
            string endMergeMarkup = "EndMerge.";
            string textMerge = "";
            foreach (var ws in wb.Worksheets)
            {

                int rowBegin, rowEnd, colBegin, colEnd;
                if (ws.Cells.FirstCell == null)
                {
                    continue;
                }
                rowBegin = ws.Cells.FirstCell.Row + 2;
                rowEnd = ws.Cells.LastCell.Row;
                colBegin = ws.Cells.FirstCell.Column;
                colEnd = ws.Cells.LastCell.Column;


                for (int rowIndex = rowBegin; rowIndex <= rowEnd; rowIndex++)
                {
                    int colStartMerge = -1, colEndMerge = -1;
                    var cell = ws.Cells.Rows[rowIndex][colBegin];
                    var style = cell.GetStyle();
                    if (cell.Value != null)
                    {
                        if (cell.Value.ToString().All(char.IsDigit))
                        {
                            style.HorizontalAlignment = TextAlignmentType.Center;
                            cell.SetStyle(style);
                        }
                        else if (cell.Value.ToString().Length <= 2)
                        {
                            style.HorizontalAlignment = TextAlignmentType.Left;
                            style.Font.IsBold = true;
                            cell.SetStyle(style);
                        }
                        else
                        {
                            style.HorizontalAlignment = TextAlignmentType.Right;

                            cell.SetStyle(style);

                        }
                    }
                }
            }

        }
        //truongnv
        void Custom_BCKH_Fomart(Workbook wb)
        {
            string startMergeMarkup = "StartMerge.";
            string endMergeMarkup = "EndMerge.";
            string textMerge = "";
            foreach (var ws in wb.Worksheets)
            {

                int rowBegin, rowEnd, colBegin, colEnd;
                if (ws.Cells.FirstCell == null)
                {
                    continue;
                }
                rowBegin = ws.Cells.FirstCell.Row + 7;
                rowEnd = ws.Cells.LastCell.Row;
                colBegin = ws.Cells.FirstCell.Column;
                colEnd = ws.Cells.LastCell.Column;

                string temp = "";

                //XỬ lý cột 'MỤC'
                for (int rowIndex = rowBegin; rowIndex <= rowEnd; rowIndex++)
                {
                    var cell = ws.Cells.Rows[rowIndex][colBegin];

                    var style = cell.GetStyle();
                    if (cell.Value != null)
                    {
                        //Merge
                        if (temp == cell.Value.ToString())
                        {
                            cell.Value = "";
                        }
                        else
                        {
                            temp = cell.Value.ToString();
                        }

                        style.IsTextWrapped = false;
                        //A, B, C,A1, A2...
                        if ((cell.Value.ToString().Length == 1) || (cell.Value.ToString().Length > 1 && !cell.Value.ToString().Contains('.')))
                        {
                            var rowCells = ws.Cells.Rows[rowIndex][colBegin];
                            style.HorizontalAlignment = TextAlignmentType.Left;
                            rowCells.SetStyle(style);
                        }

                        //A1.01, A2.01...
                        if (cell.Value.ToString().Contains('.'))
                        {
                            var rowCells = ws.Cells.Rows[rowIndex][colBegin];
                            style.HorizontalAlignment = TextAlignmentType.Right;
                            rowCells.SetStyle(style);
                        }
                    }
                }

                for (int rowIndex = rowBegin; rowIndex <= rowEnd; rowIndex++)
                {
                    var cell = ws.Cells.Rows[rowIndex][colBegin + 1];
                    if (cell.Value != null)
                    {
                        //Merge
                        if (temp == cell.Value.ToString())
                        {
                            cell.Value = "";
                        }
                        else
                        {
                            temp = cell.Value.ToString();
                        }
                    }
                    cell = ws.Cells.Rows[rowIndex][colBegin + 2];
                    if (cell.Value != null)
                    {
                        if (string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            cell.Value = null;
                        }
                    }
                    cell = ws.Cells.Rows[rowIndex][colBegin + 3];
                    if (cell.Value != null)
                    {
                        if (string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            cell.Value = null;
                        }
                    }
                }
            }
        }

        public async Task<dynamic> CreateExcelFileAndDesignDynamicColumn(ReportInfo info)
        {
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            //data.Relations.Add(new DataRelation("CustomerType_Customer", data.Tables[1].Columns["Id"], data.Tables[0].Columns["CustomerTypeCode"], true));
            Workbook designer = new Workbook(directionReport + info.PathName);


            WorkbookDesigner designWord = new WorkbookDesigner(designer);

            foreach (var item in info.Values)
            {
                designWord.SetDataSource(item.Name, item.Value);
            }

            int TableCount = data.Tables.Count;

            for (int i = 0; i < TableCount; i++)
            {
                int rows = data.Tables[i].Rows.Count;
                int cols = data.Tables[i].Columns.Count;

                for (int row = 0; row < rows; row++)
                    for (int col = 0; col < cols; col++)
                    {
                        var obj = data.Tables[i].Rows[row][col];
                        if (obj.GetType() == typeof(string))
                        {
                            string _value = obj.ToString();
                            bool check = _value.StartsWith('=') ||
                                    _value.StartsWith('+') ||
                                    _value.StartsWith('-') ||
                                    _value.StartsWith('@');

                            if (check)
                            {
                                data.Tables[i].Rows[row][col] = _value.Insert(0, "'");
                            }
                        }
                    }

            }
            designWord.SetDataSource(data);

            designWord.Process(false);
            designWord.Workbook.FileName = info.PathName;
            designWord.Workbook.FileFormat = FileFormatType.Xlsx;
            designWord.Workbook.Settings.CalcMode = CalcModeType.Automatic;
            designWord.Workbook.Settings.RecalculateBeforeSave = true;
            designWord.Workbook.Settings.ReCalculateOnOpen = true;
            designWord.Workbook.Settings.CheckCustomNumberFormat = true;

            return new { designWord, data };
        }

        void DynamicColumnFomart(Workbook wb, DataSet data)
        {
            var _dataSheetSource = data.Tables[0];
            var _dynamicSource = data.Tables[1];
            foreach (var ws in wb.Worksheets)
            {
                int rowBegin, rowEnd, colBegin, colEnd;
                if (ws.Cells.FirstCell == null)
                {
                    continue;
                }

                rowBegin = ws.Cells.FirstCell.Row;
                rowEnd = ws.Cells.LastCell.Row;
                colBegin = ws.Cells.FirstCell.Column;
                colEnd = ws.Cells.MaxColumn;

                int dynamicCollOffset = -1, dynamicRowOffset = -1;
                string dynamicColName = "";

                //Tìm vị trí chèn
                for (int i = rowBegin; i < rowEnd; i++)
                {
                    for (int j = colBegin; j < colEnd; j++)
                    {
                        if (ws.Cells.Rows[i][j].Value != null)
                        {
                            string regex = @"<dynamic>(.*)<\/dynamic>";
                            Match match = Regex.Match(ws.Cells.Rows[i][j].Value.ToString(), regex);
                            if (match.Success)
                            {
                                dynamicRowOffset = i;
                                dynamicCollOffset = j;
                                dynamicColName = match.Groups[1].Value;
                                goto findDynamicPositionDone;
                            }
                        }
                    }
                }

            findDynamicPositionDone:;

                if (dynamicCollOffset == -1 || dynamicRowOffset == -1)
                {
                    return;
                }

                //Tìm số lượng cột chèn thêm
                int maxDynamicCollCount = 0;
                int maxRowInDynamicSouce = _dynamicSource.Rows.Count;
                for (int i = 0; i < maxRowInDynamicSouce; i++)
                {
                    int x = 0;
                    maxDynamicCollCount = Int32.TryParse(_dynamicSource.Rows[i][2].ToString(), out x) && x > maxDynamicCollCount
                        ? x : maxDynamicCollCount;
                }

                //Chèn thêm cột dựa vào số lượng cột đã tìm được
                for (int i = 0; i < maxDynamicCollCount; i++)
                {
                    ws.Cells.InsertColumn(dynamicCollOffset + i, true);
                    ws.Cells.Rows[dynamicRowOffset][dynamicCollOffset + i].Value = dynamicColName + " " + (i + 1);
                }
                ws.Cells.DeleteColumn(dynamicCollOffset + maxDynamicCollCount);

                //Chèn giá trị vào hàng thuộc mấy cột mới thêm
                int stt = 0;
                for (int i = dynamicRowOffset + 1; i < rowEnd; i++)
                {
                    stt++;
                    int phase = 0;
                    for (int j = dynamicCollOffset; j < maxDynamicCollCount + dynamicCollOffset; j++)
                    {
                        phase++;
                        for (int k = 0; k < _dynamicSource.Rows.Count; k++)
                        {
                            if (_dataSheetSource.Rows.Count > 0)
                            {
                                if (_dynamicSource.Rows[k][1].ToString() == _dataSheetSource.Rows[stt - 1][1].ToString() && _dynamicSource.Rows[k][2].ToString() == phase.ToString())
                                {
                                    ws.Cells.Rows[i][j].Value = _dynamicSource.Rows[k][0];
                                }
                            }
                        }
                    }
                }


                //
                int phaseSum = 0;
                for (int j = dynamicCollOffset; j < maxDynamicCollCount + dynamicCollOffset; j++)
                {
                    string topCellName = ws.Cells.Rows[dynamicRowOffset + 1][j].Name;
                    string botCellName = ws.Cells.Rows[dynamicRowOffset + _dataSheetSource.Rows.Count][j].Name;
                    var cell = ws.Cells.Rows[rowEnd][j];
                    cell.Formula = "=SUM(" + topCellName + ":" + botCellName + ")";
                    var style = cell.GetStyle();
                    style.ShrinkToFit = true;
                    cell.SetStyle(style);
                }


            }
        }

        private async Task<MemoryStream> CreateExcelFileDynamicColumn(ReportInfo info)
        {
            var fileAndDesign = await CreateExcelFileAndDesignDynamicColumn(info);
            DynamicColumnFomart(fileAndDesign.designWord.Workbook, fileAndDesign.data);
            MemoryStream str = new MemoryStream();
            fileAndDesign.designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Xlsx);
            return str;
        }

        public async Task<MemoryStream> GetReportFileDynamicColumn(ReportInfo info)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    str = await CreateExcelFileDynamicColumn(info);
                    break;
                case FileTypeConst.Pdf:
                    str = await CreateWordFile(info);
                    break;
                case FileTypeConst.Word:
                    str = await CreateWordFile(info);
                    break;
            }
            return str;
        }

        public async Task<dynamic> CreateExcelFileAndDesignPmGeneralExcel(ReportInfo info)
        {
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            //data.Relations.Add(new DataRelation("CustomerType_Customer", data.Tables[1].Columns["Id"], data.Tables[0].Columns["CustomerTypeCode"], true));
            Workbook designer = new Workbook(directionReport + info.PathName);


            WorkbookDesigner designWord = new WorkbookDesigner(designer);

            foreach (var item in info.Values)
            {
                designWord.SetDataSource(item.Name, item.Value);
            }

            int TableCount = data.Tables.Count;

            for (int i = 0; i < TableCount; i++)
            {
                int rows = data.Tables[i].Rows.Count;
                int cols = data.Tables[i].Columns.Count;

                for (int row = 0; row < rows; row++)
                    for (int col = 0; col < cols; col++)
                    {
                        var obj = data.Tables[i].Rows[row][col];
                        if (obj.GetType() == typeof(string))
                        {
                            string _value = obj.ToString();
                            bool check = _value.StartsWith('=') ||
                                    _value.StartsWith('+') ||
                                    _value.StartsWith('-') ||
                                    _value.StartsWith('@');

                            if (check)
                            {
                                data.Tables[i].Rows[row][col] = _value.Insert(0, "'");
                            }
                        }
                    }
            }
            designWord.SetDataSource(data);

            designWord.Process(false);
            designWord.Workbook.FileName = info.PathName;
            designWord.Workbook.FileFormat = FileFormatType.Xlsx;
            //designWord.Workbook.Settings.CalcMode = CalcModeType.Automatic;
            //designWord.Workbook.Settings.RecalculateBeforeSave = true;
            //designWord.Workbook.Settings.ReCalculateOnOpen = true;
            designWord.Workbook.Settings.CheckCustomNumberFormat = true;




            return new
            {
                designWord = designWord,
                data = data
            };
        }

        void PmGeneralExcelFomart(Workbook wb, DataSet data)
        {
            var _dataSheetSource = data.Tables[0];
            var _dynamicSource = data.Tables[1];
            foreach (var ws in wb.Worksheets)
            {
                int rowBegin, rowEnd, colBegin, colEnd;
                if (ws.Cells.FirstCell == null)
                {
                    continue;
                }

                rowBegin = ws.Cells.FirstCell.Row;
                rowEnd = ws.Cells.LastCell.Row;
                colBegin = ws.Cells.FirstCell.Column;
                colEnd = ws.Cells.MaxColumn;

                int dynamicCollOffset = -1, dynamicRowOffset = -1;

                //Tìm vị trí chèn
                for (int i = rowBegin; i < rowEnd; i++)
                {
                    for (int j = colBegin; j < colEnd; j++)
                    {
                        if (ws.Cells.Rows[i][j].Value != null)
                        {
                            string regex = @"%%PRINT_NAME";
                            Match match = Regex.Match(ws.Cells.Rows[i][j].Value.ToString(), regex);
                            if (match.Success)
                            {
                                dynamicRowOffset = i;
                                dynamicCollOffset = j;
                                goto findDynamicPositionDone;
                            }
                        }
                    }
                }

            findDynamicPositionDone:;

                if (dynamicCollOffset == -1 || dynamicRowOffset == -1)
                {
                    return;
                }

                //Tìm số lượng cột chèn thêm
                int maxDynamicCollCount = _dynamicSource.Rows.Count;
                int maxRowInDynamicSouce = _dynamicSource.Columns.Count;

                //Chèn thêm cột dựa vào số lượng cột đã tìm được
                for (int i = 0; i < maxDynamicCollCount - 1; i++)
                {
                    ws.Cells.InsertColumn(dynamicCollOffset + i, true);
                }

                //Điền tên ấn phẩm vào tên cột
                int index = 0;
                for (int i = dynamicCollOffset; i < maxDynamicCollCount + dynamicCollOffset; i++)
                {
                    ws.Cells.Rows[dynamicRowOffset][i].Value = _dynamicSource.Rows[index++][1];
                }

                int rowDyInd = 0, rowGrInd = 0;
                //Điền số lượng vào từng ấn phẩm1
                for (int i = dynamicCollOffset; i < maxDynamicCollCount + dynamicCollOffset; i++)
                {
                    rowDyInd = 0;
                    for (int j = dynamicRowOffset + 1; j <= dynamicRowOffset + _dataSheetSource.Rows.Count; j++)
                    {

                        if (
                            _dataSheetSource.Rows[rowDyInd][0].ToString() == _dynamicSource.Rows[rowGrInd][0].ToString()
                            &&
                            _dataSheetSource.Rows[rowDyInd][8].ToString() == _dynamicSource.Rows[rowGrInd][3].ToString()
                            )
                        {
                            ws.Cells.Rows[j][i].Value = _dynamicSource.Rows[rowGrInd][2];
                        }
                        else
                        {
                            ws.Cells.Rows[j][i].Value = 0;
                        }
                        rowDyInd++;
                    }
                    rowGrInd++;
                }

                int removedColCount = 0;
                //Gộp cột trùng
                PM_GopCotTrung(dynamicCollOffset, maxDynamicCollCount + dynamicCollOffset, dynamicRowOffset, dynamicRowOffset + _dataSheetSource.Rows.Count, ws, ref removedColCount);

                for (int j = dynamicCollOffset; j < maxDynamicCollCount + dynamicCollOffset - removedColCount; j++)
                {
                    string topCellName = ws.Cells.Rows[dynamicRowOffset + 1][j].Name;
                    string botCellName = ws.Cells.Rows[dynamicRowOffset + _dataSheetSource.Rows.Count][j].Name;
                    var cell = ws.Cells.Rows[rowEnd][j];
                    cell.Formula = "=SUM(" + topCellName + ":" + botCellName + ")";
                    var style = cell.GetStyle();
                    style.ShrinkToFit = true;
                    cell.SetStyle(style);
                }
            }
        }

        private void PM_GopCotTrung(int colStart, int colEnd, int rowStart, int rowEnd, Worksheet ws, ref int removedColCount)
        {
            if (colStart > colEnd - 1)
            {
                return;
            }
            //check trung cot
            if (ws.Cells.Rows[rowStart][colStart].Value.ToString() == ws.Cells.Rows[rowStart][colStart + 1].Value.ToString())
            {
                //gop cot
                for (int m = rowStart + 1; m < rowEnd; m++)
                {
                    long a = 0, b = 0;
                    Int64.TryParse(ws.Cells.Rows[m][colStart].Value.ToString(), out a);
                    Int64.TryParse(ws.Cells.Rows[m][colStart + 1].Value.ToString(), out b);
                    ws.Cells.Rows[m][colStart].Value = Math.Max(a, b);
                }
                ws.Cells.DeleteColumn(colStart + 1, true);
                removedColCount = removedColCount + 1;
                PM_GopCotTrung(colStart, colEnd - 1, rowStart, rowEnd, ws, ref removedColCount);
            }
            else
            {
                PM_GopCotTrung(colStart + 1, colEnd, rowStart, rowEnd, ws, ref removedColCount);
            }
        }

        private async Task<MemoryStream> CreateExcelFilePmGeneralExcel(ReportInfo info)
        {
            var fileAndDesign = await CreateExcelFileAndDesignPmGeneralExcel(info);
            PmGeneralExcelFomart(fileAndDesign.designWord.Workbook, fileAndDesign.data);
            MemoryStream str = new MemoryStream();
            fileAndDesign.designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Xlsx);
            return str;
        }

        public async Task<MemoryStream> GetReportFilePmGeneralExcel(ReportInfo info)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    str = await CreateExcelFilePmGeneralExcel(info);
                    break;
                case FileTypeConst.Pdf:
                    str = await CreateWordFile(info);
                    break;
                case FileTypeConst.Word:
                    str = await CreateWordFile(info);
                    break;
            }
            return str;
        }

        public async Task<MemoryStream> GetPivotReport(ReportInfo info)
        {
            var designWord = await CreatePivotExcelFile(info);
            MemoryStream str = new MemoryStream();
            designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Xlsx);
            return str;
        }

        public async Task<WorkbookDesigner> CreatePivotExcelFile(ReportInfo info)
        {
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            Workbook designer = new Workbook(directionReport + info.PathName);
            WorkbookDesigner designWord = new WorkbookDesigner(designer);
            foreach (var item in info.Values)
            {
                designWord.SetDataSource(item.Name, item.Value);
            }

            int TableCount = data.Tables.Count;
            for (int i = 0; i < TableCount; i++)
            {
                int rows = data.Tables[i].Rows.Count;
                int cols = data.Tables[i].Columns.Count;

                for (int row = 0; row < rows; row++)
                    for (int col = 0; col < cols; col++)
                    {
                        var obj = data.Tables[i].Rows[row][col];
                        if (obj.GetType() == typeof(string))
                        {
                            string _value = obj.ToString();
                            bool check = _value.StartsWith('=') ||
                                    _value.StartsWith('+') ||
                                    _value.StartsWith('-') ||
                                    _value.StartsWith('@');

                            if (check)
                            {
                                data.Tables[i].Rows[row][col] = _value.Insert(0, "'");
                            }
                        }
                    }

            }
            designWord.SetDataSource(data);

            designWord.Process(false);
            designWord.Workbook.FileName = info.PathName;
            designWord.Workbook.FileFormat = FileFormatType.Xlsx;
            //designWord.Workbook.Settings.C = CalcModeType.Automatic;
            //designWord.Workbook.Settings.RecalculateBeforeSave = true;
            //designWord.Workbook.Settings.ReCalculateOnOpen = true;
            designWord.Workbook.Settings.CheckCustomNumberFormat = true;

            InsertPivotData(designWord.Workbook, data);

            return designWord;
        }

        private void InsertPivotData(Workbook wb, DataSet data)
        {
            Worksheet ws = wb.Worksheets[0];
            if (ws.Cells.FirstCell == null)
            {
                return;
            }

            DataTable rowsForPivot = data.Tables[0];
            DataTable columnsForPivot = data.Tables[1];
            DataTable cellsForPivot = data.Tables[2];
            dynamic defaultCellValue = data.Tables[3].Rows[0][0];

            FindOptions opts = new FindOptions
            {
                LookInType = LookInType.Values,
                LookAtType = LookAtType.EntireContent
            };

            Cell dynamicRow = ws.Cells.Find("##DYNAMIC_ROW", null, opts);
            // insert new column after this column
            Cell dynamicCol = ws.Cells.Find("##DYNAMIC_COL", null, opts);

            int pivotRowsCount = rowsForPivot.Rows.Count;
            int pivotColsCount = columnsForPivot.Rows.Count;

            for (int i = 0; i < pivotColsCount; i++)
            {
                // Insert new column 
                var insColPosition = dynamicCol.Column + i + 1;
                ws.Cells.InsertColumn(insColPosition, true);

                // Copy cells style from template column to new column
                ws.Cells.CopyColumn(ws.Cells, dynamicCol.Column, insColPosition);

                // Insert column name 
                var colName = columnsForPivot.Rows[i][1];
                ws.Cells.Rows[dynamicCol.Row][insColPosition].Value = colName;

                // Insert cell value for new column 
                var colID = columnsForPivot.Rows[i][0].ToString();

                for (int j = 0; j < pivotRowsCount; j++)
                {
                    var rowIndex = dynamicRow.Row + j + 1;
                    var rowID = ws.Cells.Rows[rowIndex][insColPosition].Value;

                    var result = cellsForPivot.Rows.Cast<DataRow>().FirstOrDefault(
                        r => r.Field<dynamic>("ROW_ID").ToString() == rowID.ToString() && r.Field<dynamic>("COL_ID").ToString() == colID.ToString());

                    var cellValue = result != null ? result.Field<dynamic>("CELL_VALUE") : defaultCellValue;

                    ws.Cells.Rows[rowIndex][insColPosition].Value = cellValue;
                }
            }

            ws.Cells.DeleteColumn(dynamicCol.Column);
            ws.Cells.DeleteRow(dynamicRow.Row);
        }

        public async Task<MemoryStream> GetReportWordGroupFile(ReportInfo info, string groupId)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Pdf:
                    str = await CreateWordGroupFile(info, groupId);

                    break;
                case FileTypeConst.Word:
                    str = await CreateWordGroupFile(info, groupId);
                    break;
            }

            return str;
        }

        private async Task<MemoryStream> CreateWordGroupFile(ReportInfo info, string groupId)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);

            data.Relations.Add(new DataRelation("CustomerType_Customer", data.Tables[1].Columns[groupId], data.Tables[0].Columns[groupId], true));

            Document doc = new Document(directionReport + info.PathName);

            doc.MailMerge.CleanupParagraphsWithPunctuationMarks = true;
            doc.MailMerge.CleanupOptions = MailMergeCleanupOptions.RemoveUnusedFields | MailMergeCleanupOptions.RemoveUnusedRegions;
            doc.MailMerge.Execute(info.Values.Select(x => x.Name).ToArray(), info.Values.Select(x => x.Value).ToArray());
            doc.MailMerge.ExecuteWithRegions(data);

            MemoryStream stream = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Pdf:
                    doc.Save(stream, Aspose.Words.SaveFormat.Pdf);
                    break;
                case FileTypeConst.Word:
                    doc.Save(stream, Aspose.Words.SaveFormat.Docx);
                    break;
            }


            return stream;

        }

        private DataTable getDataPrintTemp(DataTable dtIn, string Qty)
        {
            int currentLenght = 0;
            int _Qty = int.Parse(Qty);
            DataTable dt = new DataTable();
            dt.Columns.Add("TSCD", typeof(string));
            dt.Columns.Add("MSTS", typeof(string));
            dt.Columns.Add("TGSD", typeof(string));
            dt.Columns.Add("DVSD", typeof(string));
            dt.Columns.Add("DEPT_CODE", typeof(string));
            dt.Columns.Add("SERIAL", typeof(string));
            dt.Columns.Add("NHOMTS", typeof(string));
            dt.Columns.Add("TENTS", typeof(string));
            dt.Columns.Add("GHICHU", typeof(string));
            dt.Columns.Add("DONVI_SD", typeof(string));
            dt.Columns.Add("PBSD", typeof(string));
            dt.Columns.Add("Lenght", typeof(int));
            for (int i = 0; i < dtIn.Rows.Count; i++)
            {
                for (int j = 0; j < _Qty; j++)
                {
                    dt.Rows.Add(dtIn.Rows[i]["TYPE_ID"].ToString(),
                    dtIn.Rows[i]["ASSET_CODE"].ToString(),
                    DateTime.Parse(dtIn.Rows[i]["BUY_DATE_KT"].ToString()).ToString("dd/MM/yyyy"),
                    dtIn.Rows[i]["DVSD"].ToString(),
                    dtIn.Rows[i]["DEPT_CODE"].ToString(),
                    dtIn.Rows[i]["ASSET_SERIAL_NO"].ToString(),
                    dtIn.Rows[i]["NHOM_TS"].ToString(),
                    dtIn.Rows[i]["ASSET_NAME"].ToString(),
                    dtIn.Rows[i]["NOTES"].ToString(),
                    dtIn.Rows[i]["BRANCH_NAME"].ToString(),
                    dtIn.Rows[i]["DEP_NAME"].ToString()
                    );
                    int length = dtIn.Rows[i]["ASSET_NAME"].ToString().Length + dtIn.Rows[i]["BRANCH_NAME"].ToString().Length + dtIn.Rows[i]["DEP_NAME"].ToString().Length + 48;
                    if (currentLenght < length)
                    {
                        currentLenght = length;
                    }
                }
            }

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("TSCD1", typeof(string));
            dt1.Columns.Add("TSCD2", typeof(string));
            dt1.Columns.Add("TSCD3", typeof(string));
            dt1.Columns.Add("MSTS1", typeof(string));
            dt1.Columns.Add("MSTS2", typeof(string));
            dt1.Columns.Add("MSTS3", typeof(string));
            dt1.Columns.Add("TGSD1", typeof(string));
            dt1.Columns.Add("TGSD2", typeof(string));
            dt1.Columns.Add("TGSD3", typeof(string));
            dt1.Columns.Add("DVSD1", typeof(string));
            dt1.Columns.Add("DVSD2", typeof(string));
            dt1.Columns.Add("DVSD3", typeof(string));
            dt1.Columns.Add("BarCode1", typeof(byte[]));
            dt1.Columns.Add("BarCode2", typeof(byte[]));
            dt1.Columns.Add("BarCode3", typeof(byte[]));
            dt1.Columns.Add("DEPT_CODE1", typeof(string));
            dt1.Columns.Add("DEPT_CODE2", typeof(string));
            dt1.Columns.Add("DEPT_CODE3", typeof(string));
            dt1.Columns.Add("SERIAL1", typeof(string));
            dt1.Columns.Add("SERIAL2", typeof(string));
            dt1.Columns.Add("SERIAL3", typeof(string));
            int rowcount = dt.Rows.Count;
            int row = rowcount / 3;
            string barcode1 = "";
            string barcode2 = "";
            string barcode3 = "";
            for (int i = 0; i < row; i++)
            {
                barcode1 = "<" + dt.Rows[i * 3][1] + ">"
                   + "<" + dt.Rows[i * 3][7] + ">"
                   + "<" + dt.Rows[i * 3][9] + ">";
                if (dt.Rows[i * 3][10].ToString() != "")
                    barcode1 = barcode1 + "< " + dt.Rows[i * 3][10] + ">";
                barcode1 = barcode1 + "<" + dt.Rows[i * 3][2] + ">";
                if (dt.Rows[i * 3][5].ToString() != "")
                    barcode1 = barcode1 + "<" + dt.Rows[i * 3][5] + ">";

                barcode2 = "<" + dt.Rows[i * 3 + 1][1] + ">"
                         + "<" + dt.Rows[i * 3 + 1][7] + ">"
                         + "<" + dt.Rows[i * 3 + 1][9] + ">";
                if (dt.Rows[i * 3 + 1][10].ToString() != "")
                    barcode2 = barcode2 + "<" + dt.Rows[i * 3 + 1][10] + ">";
                barcode2 = barcode2 + "<" + dt.Rows[i * 3 + 1][2] + ">";
                if (dt.Rows[i * 3 + 1][5].ToString() != "")

                    barcode3 = barcode3 + "<" + dt.Rows[i * 3 + 2][5] + ">";
                barcode3 = "<" + dt.Rows[i * 3 + 2][1] + ">"
                        + "<" + dt.Rows[i * 3 + 2][7] + ">"
                        + "<" + dt.Rows[i * 3 + 2][9] + ">";
                if (dt.Rows[i * 3 + 2][10].ToString() != "")
                    barcode3 = barcode3 + "<" + dt.Rows[i * 3 + 2][10] + ">";
                barcode3 = barcode3 + "<" + dt.Rows[i * 3 + 2][2] + ">";
                if (dt.Rows[i * 3 + 2][5].ToString() != "")
                    barcode3 = barcode3 + "<" + dt.Rows[i * 2 + 2][5] + ">";

                dt1.Rows.Add(
                    dt.Rows[i * 3][6],
                    dt.Rows[i * 3 + 1][6],
                    dt.Rows[i * 3 + 2][6],
                    dt.Rows[i * 3][1],
                    dt.Rows[i * 3 + 1][1],
                    dt.Rows[i * 3 + 2][1],
                    dt.Rows[i * 3][2],
                    dt.Rows[i * 3 + 1][2],
                    dt.Rows[i * 3 + 2][2],
                    dt.Rows[i * 3][3],
                    dt.Rows[i * 3 + 1][3],
                    dt.Rows[i * 3 + 2][3],
                    GenerateQrCode(barcode1, currentLenght),
                    GenerateQrCode(barcode2, currentLenght),
                    GenerateQrCode(barcode3, currentLenght),
                    dt.Rows[i * 3][4],
                    dt.Rows[i * 3 + 1][4],
                    dt.Rows[i * 3 + 2][4],
                    dt.Rows[i * 3][5],
                    dt.Rows[i * 3 + 1][5],
                    dt.Rows[i * 3 + 2][5]
                    );
            }
            if (row * 3 + 1 == rowcount)
            {
                barcode1 = "<" + dt.Rows[row * 3][1] + ">"
                    + "<" + dt.Rows[row * 3][7] + ">"
                    + "<" + dt.Rows[row * 3][9] + " >";
                if (dt.Rows[row * 3][10].ToString() != "")
                    barcode1 = barcode1 + "<" + dt.Rows[row * 3][10] + ">";
                barcode1 = barcode1 + "<" + dt.Rows[row * 3][2] + ">";
                if (dt.Rows[row * 3][5].ToString() != "")
                    barcode1 = barcode1 + "<" + dt.Rows[row * 2][5] + ">";
                dt1.Rows.Add(dt.Rows[row * 3][6], null, null,
                    dt.Rows[row * 3][1], null, null,
                    dt.Rows[row * 3][2], null, null,
                    dt.Rows[row * 3][3], null, null,
                     GenerateQrCode(barcode1, currentLenght),
                    null, null,
                    dt.Rows[row * 3][4],
                    null, null,
                      dt.Rows[row * 3][5],
                    null, null
                    );
            }
            if (row * 3 + 2 == rowcount)
            {
                barcode1 = "<" + dt.Rows[row * 3][1] + ">"
                    + "<" + dt.Rows[row * 3][7] + ">"
                    + "<" + dt.Rows[row * 3][9] + ">";
                if (dt.Rows[row * 3][10].ToString() != "")
                    barcode1 = barcode1 + "<" + dt.Rows[row * 3][10] + ">";
                barcode1 = barcode1 + "<" + dt.Rows[row * 3][2] + ">";
                if (dt.Rows[row * 3][5].ToString() != "")
                    barcode1 = barcode1 + "<" + dt.Rows[row * 2 + 1][5] + ">";

                barcode2 = "<" + dt.Rows[row * 3 + 1][1] + ">"
                         + "<" + dt.Rows[row * 3 + 1][7] + ">"
                         + "<" + dt.Rows[row * 3 + 1][9] + ">";
                if (dt.Rows[row * 3 + 1][10].ToString() != "")
                    barcode2 = barcode2 + "<" + dt.Rows[row * 3 + 1][10] + ">";
                barcode2 = barcode2 + "<" + dt.Rows[row * 3 + 1][2] + ">";

                if (dt.Rows[row * 3 + 1][5].ToString() != "")
                    barcode2 = barcode2 + "<" + dt.Rows[row * 2 + 1][5] + ">";
                dt1.Rows.Add(
                    dt.Rows[row * 3][6], dt.Rows[row * 3 + 1][6], null,
                    dt.Rows[row * 3][1], dt.Rows[row * 3 + 1][1], null,
                    dt.Rows[row * 3][2], dt.Rows[row * 3 + 1][2], null,
                    dt.Rows[row * 3][3], dt.Rows[row * 3 + 1][3], null,
                    GenerateQrCode(barcode1, currentLenght), GenerateQrCode(barcode2, currentLenght), null,
                    dt.Rows[row * 3][4], dt.Rows[row * 3 + 1][4], null,
                    dt.Rows[row * 3][5], dt.Rows[row * 3 + 1][5], null
                    );
            }
            return dt1;
        }

        private byte[] GenerateQrCode(string data, int maxStr)
        {
            int LenInsert = maxStr == 96 ? maxStr + 20 : maxStr;
            if (data.Length < LenInsert)
            {
                LenInsert = maxStr - data.Length;
                while (LenInsert > 0)
                {
                    data += " ";
                    LenInsert--;
                }
            }
            return null;
        }

        private byte[] ImageToByte2(Bitmap img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                stream.Close();
                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        #region 06-06-23 Xuất nhiều đơn vị trong file Move từ BVNT
        public async Task<MemoryStream> GetReportFileMultiSheet(ReportInfo info)
        {
            MemoryStream str = new MemoryStream();

            switch (info.TypeExport.ToLower())
            {
                case FileTypeConst.Excel:
                    str = await CreateExcelFileMultiSheet(info);
                    break;
                case FileTypeConst.Pdf:
                    if (info.PathName.Split('.')[info.PathName.Split('.').Length - 1] == "xlsx")
                    {
                        str = await CreateExcelFileMultiSheet(info);
                    }
                    else
                    {
                        str = await CreateWordFile(info);
                    }
                    break;
                case FileTypeConst.Word:
                    str = await CreateExcelFileMultiSheet(info);
                    break;
            }
            return str;
        }

        private async Task<MemoryStream> CreateExcelFileMultiSheet(ReportInfo info)
        {
            if (info.TypeExport.ToLower() == FileTypeConst.Excel)
            {
                var designWord = await CreateExcelFileMultiSheetAndDesignExcel(info);

                if (info.ProcessMerge == true)
                    ProcessMergeCell(designWord.Workbook);

                MemoryStream str = new MemoryStream();

                designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Xlsx);

                return str;
            }
            else if (info.TypeExport.ToLower() == FileTypeConst.Word)
            {
                var designWord = await CreateExcelFileMultiSheetAndDesignPdf(info);

                if (info.ProcessMerge == true)
                    ProcessMergeCell(designWord.Workbook);

                MemoryStream str = new MemoryStream();

                designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Html);

                Aspose.Words.LoadOptions loadOptions = new Aspose.Words.LoadOptions();
                loadOptions.LoadFormat = Aspose.Words.LoadFormat.Html;
                var document = new Aspose.Words.Document(str, loadOptions);

                str = new MemoryStream();

                document.Save(str, Aspose.Words.SaveFormat.Docx);

                return str;
            }
            else
            {
                var designWord = await CreateExcelFileMultiSheetAndDesignPdf(info);

                if (info.ProcessMerge == true)
                    ProcessMergeCell(designWord.Workbook);

                MemoryStream str = new MemoryStream();

                designWord.Workbook.Save(str, Aspose.Cells.SaveFormat.Pdf);

                return str;
            }
        }

        private async Task<WorkbookDesigner> CreateExcelFileMultiSheetAndDesignExcel(ReportInfo info)
        {
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            Workbook designer = new Workbook(directionReport + info.PathName);

            int TableCount = data.Tables.Count;
            WorkbookDesigner designWord = new WorkbookDesigner(designer);

            foreach (var item in info.Values)
            {
                designWord.SetDataSource(item.Name, item.Value);
            }

            //temp lưu giá trị branch
            string branch_temp = "";
            int index_sheet_temp = 0;
            int row_temp = 0;
            int stt_temp = 1;
            for (int i = 0; i < TableCount; i++)
            {
                int rows = data.Tables[i].Rows.Count;
                int cols = data.Tables[i].Columns.Count;

                for (int row = 0; row < rows; row++)
                {
                    // neu la table 0
                    if (i == 0)
                    {
                        // check branch co thay doi khong
                        if (branch_temp != data.Tables[0].Rows[row][17].ToString())
                        {
                            stt_temp = 1;
                            branch_temp = (string)data.Tables[0].Rows[row][17];

                            // add a new worksheet to the Excel object
                            int sheetIndex = designer.Worksheets.Add();
                            index_sheet_temp = sheetIndex;
                            Worksheet worksheetcopy = designer.Worksheets[index_sheet_temp];
                            worksheetcopy.Copy(designer.Worksheets[0]);
                            // dat ten cho sheet
                            worksheetcopy.Name = (string)data.Tables[0].Rows[row][19];

                            // gan cac o tinh tong
                            for (int l = 2; l < data.Tables[3].Columns.Count; l++)
                            {
                                worksheetcopy.Cells[Convert.ToString(data.Tables[4].Rows[0][l])]
                                    .PutValue(data.Tables[3].Rows[row_temp][l]);
                            }
                            //wraptext toan bo sheet
                            worksheetcopy.AutoFitRows(true);
                            // do rong khong doi=>column khong xuong dong
                            worksheetcopy.PageSetup.FitToPagesTall = 0;
                            worksheetcopy.PageSetup.FitToPagesWide = 1;
                            row_temp++;
                        }
                        else { stt_temp++; }

                        // Lấy worksheet mới tạo
                        Worksheet worksheet = designer.Worksheets[index_sheet_temp];

                        //worksheet.Cells.DeleteRange(8, 0, 8, 10, ShiftType.Up);
                        if (stt_temp == 1)
                        {
                            worksheet.Cells.InsertRow(info.Start_Row);
                            for (int h = 0; h < info.Header.Count; h++)
                            {
                                if (h == 0)
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + (info.Start_Row + 1).ToString()].PutValue(stt_temp);
                                else
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + (info.Start_Row + 1).ToString()].PutValue(data.Tables[0].Rows[row][h - 1]);
                            }

                            worksheet.Cells.DeleteRow(info.Start_Row - 1);
                        }
                        else
                        {
                            ImageOrPrintOptions printoption = new ImageOrPrintOptions();
                            printoption.PrintingPage = PrintingPageType.Default;
                            SheetRender sr = new SheetRender(worksheet, printoption);

                            string row_item = (info.Start_Row + stt_temp - 1).ToString();
                            worksheet.Cells.InsertRow(info.Start_Row - 2 + stt_temp);
                            for (int h = 0; h < info.Header.Count; h++)
                            {
                                if (h == 0)
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + row_item].PutValue(stt_temp);
                                else
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + row_item].PutValue(data.Tables[0].Rows[row][h - 1]);
                            }

                        }

                    }


                    for (int col = 0; col < cols; col++)
                    {
                        var obj = data.Tables[i].Rows[row][col];
                        if (obj.GetType() == typeof(string))
                        {
                            string _value = obj.ToString();
                            bool check = _value.StartsWith('=') ||
                                    _value.StartsWith('+') ||
                                    _value.StartsWith('-') ||
                                    _value.StartsWith('@');

                            if (check)
                            {
                                data.Tables[i].Rows[row][col] = _value.Insert(0, "'");
                            }
                        }
                    }
                }
            }

            designWord.SetDataSource(data);

            designWord.Process(false);
            designWord.Workbook.FileName = info.PathName;
            designWord.Workbook.FileFormat = FileFormatType.Xlsx;
            //designWord.Workbook.Settings.CalcMode = CalcModeType.Automatic;
            //designWord.Workbook.Settings.RecalculateBeforeSave = true;
            //designWord.Workbook.Settings.ReCalculateOnOpen = true;
            designWord.Workbook.Settings.CheckCustomNumberFormat = true;
            // xoa sheet dau tien
            designWord.Workbook.Worksheets.RemoveAt(0);

            return designWord;
        }

        private async Task<WorkbookDesigner> CreateExcelFileMultiSheetAndDesignPdf(ReportInfo info)
        {
            DataSet data = await GetDataFromStoreToReport(info.StoreName, info.Parameters);
            Workbook designer = new Workbook(directionReport + info.PathName);


            int TableCount = data.Tables.Count;

            WorkbookDesigner designWord = new WorkbookDesigner(designer);


            foreach (var item in info.Values)
            {
                designWord.SetDataSource(item.Name, item.Value);
            }

            //temp lưu giá trị branch
            string branch_temp = "";
            int index_sheet_temp = 0;
            int row_temp = 0;
            // danh stt cho tung dong trong 1 sheet
            int stt_temp = 1;
            // dem so trang trong 1 sheet
            int page_count = 1;

            for (int i = 0; i < TableCount; i++)
            {
                int rows = data.Tables[i].Rows.Count;
                int cols = data.Tables[i].Columns.Count;


                for (int row = 0; row < rows; row++)
                {
                    if (i == 0)
                    {
                        // check branch co thay doi khong
                        if (branch_temp != data.Tables[0].Rows[row][17].ToString())
                        {
                            stt_temp = 1;
                            page_count = 1;
                            branch_temp = (string)data.Tables[0].Rows[row][17];

                            // add a new worksheet to the Excel object
                            int sheetIndex = designer.Worksheets.Add();
                            index_sheet_temp = sheetIndex;
                            Worksheet worksheetcopy = designer.Worksheets[index_sheet_temp];
                            worksheetcopy.Copy(designer.Worksheets[0]);
                            // dat ten cho sheet
                            //worksheetcopy.Name = (string)data.Tables[0].Rows[row][19];

                            //gan cac truong tinh tong vao pdf
                            for (int l = 2; l < data.Tables[3].Columns.Count; l++)
                            {
                                worksheetcopy.Cells[Convert.ToString(data.Tables[4].Rows[0][l])]
                                    .PutValue(data.Tables[3].Rows[row_temp][l].ToString());
                            }
                            // do rong khong doi=>column khong xuong dong
                            worksheetcopy.PageSetup.FitToPagesTall = 0;
                            worksheetcopy.PageSetup.FitToPagesWide = 1;
                            row_temp++;
                        }
                        else { stt_temp++; }

                        // Lấy worksheet mới tạo
                        Worksheet worksheet = designer.Worksheets[index_sheet_temp];

                        if (stt_temp == 1)
                        {
                            worksheet.Cells.InsertRow(info.Start_Row);
                            for (int h = 0; h < info.Header.Count; h++)
                            {
                                if (h == 0)
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + (info.Start_Row + 1).ToString()].PutValue(stt_temp);
                                else
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + (info.Start_Row + 1).ToString()].PutValue(data.Tables[0].Rows[row][h - 1]);
                            }

                            worksheet.Cells.DeleteRow(info.Start_Row - 1);
                        }
                        else
                        {
                            ImageOrPrintOptions printoption = new ImageOrPrintOptions();
                            printoption.PrintingPage = PrintingPageType.Default;

                            string row_item = (info.Start_Row + stt_temp - 1).ToString();
                            worksheet.Cells.InsertRow(info.Start_Row - 2 + stt_temp);
                            for (int h = 0; h < info.Header.Count; h++)
                            {
                                if (h == 0)
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + row_item].PutValue(stt_temp);
                                else
                                    worksheet.Cells[Convert.ToChar(65 + h + info.Start_Column) + row_item].PutValue(data.Tables[0].Rows[row][h - 1]);
                            }

                        }
                        //worksheet.AutoFitRows();
                        // do rong cua row
                        if (data.Tables[4].Rows[0][1].ToString() == "noautofit" && data.Tables.Count > 4)
                        {
                            //06-06-23
                            //worksheet.Cells.SetRowHeight(info.Start_Row, 50);
                        }

                    }

                    for (int col = 0; col < cols; col++)
                    {
                        var obj = data.Tables[i].Rows[row][col];
                        if (obj.GetType() == typeof(string))
                        {
                            string _value = obj.ToString();
                            bool check = _value.StartsWith('=') ||
                                    _value.StartsWith('+') ||
                                    _value.StartsWith('-') ||
                                    _value.StartsWith('@');

                            if (check)
                            {
                                data.Tables[i].Rows[row][col] = _value.Insert(0, "'");
                            }
                        }
                    }
                }
            }

            designWord.SetDataSource(data);

            designWord.Process(false);
            designWord.Workbook.FileName = info.PathName;
            designWord.Workbook.FileFormat = FileFormatType.Xlsx;

            designWord.Workbook.Settings.CalcMode = CalcModeType.Automatic;
            designWord.Workbook.Settings.RecalculateBeforeSave = true;
            designWord.Workbook.Settings.ReCalculateOnOpen = true;
            designWord.Workbook.Settings.CheckCustomNumberFormat = true;
            // xoa sheet dau tien
            designWord.Workbook.Worksheets.RemoveAt(0);

            for (int d = 0; d < designWord.Workbook.Worksheets.Count; d++)
            {
                if (data.Tables[4].Rows[0][1].ToString() == "noautofit" && data.Tables.Count > 4) { }
                else
                    designWord.Workbook.Worksheets[d].AutoFitRows();
            }

            // them header cho table khi sang trang
            for (int i = 0; i < designWord.Workbook.Worksheets.Count; i++)
            {

                designWord.Workbook.Worksheets[i].AutoFitRows(true);


                if (info.orientaiton_page == "landscape")
                    designWord.Workbook.Worksheets[i].PageSetup.Orientation = PageOrientationType.Landscape;
                else
                    designWord.Workbook.Worksheets[i].PageSetup.Orientation = PageOrientationType.Portrait;

                int totalPage = 0;

                // lay tong so trang
                ImageOrPrintOptions printoption = new ImageOrPrintOptions();
                printoption.PrintingPage = PrintingPageType.Default;
                SheetRender sr = new SheetRender(designWord.Workbook.Worksheets[i], printoption);
                CellArea[] area = designWord.Workbook.Worksheets[i].GetPrintingPageBreaks(printoption);

                // gan totalPage = tong so trang
                totalPage = area.Length;

                //khoi tao dong dau
                int strow = 0;
                string strowString = "";
                string header = "";
                //bat dau moi sheet thi so trang se bat dau bang 1
                designWord.Workbook.Worksheets[i].PageSetup.FirstPageNumber = 1;

                if (area.Length > 1)
                {
                    //Get the first page rows. 
                    strow = area[1].StartRow;
                    strowString = area[1].StartRow.ToString();
                    header = (strow + 1).ToString();
                }

                // gan header cho table khi qua trang moi
                for (int k = 1; k < totalPage; k++)
                {
                    try
                    {
                        if (designWord.Workbook.Worksheets[i].Cells[Convert.ToChar(65 + info.Start_Column) + strowString].Value.GetType() == typeof(Int32)
                            || designWord.Workbook.Worksheets[i].Cells[Convert.ToChar(65 + info.Start_Column) + strowString].Value.GetType() == typeof(Double))
                        {
                            designWord.Workbook.Worksheets[i].Cells.InsertRow(strow);
                            for (int h = 0; h < info.Header.Count; h++)
                            {
                                designWord.Workbook.Worksheets[i].Cells[Convert.ToChar(65 + h + info.Start_Column) + header].PutValue(info.Header[h]);

                                // Setting the horizontal alignment of the text in the "A1" cell
                                Aspose.Cells.Style style = designWord.Workbook.Worksheets[i].Cells[Convert.ToChar(65 + h + info.Start_Column) + (info.Start_Row - 1).ToString()].GetStyle();
                                //style.HorizontalAlignment = TextAlignmentType.Center;
                                //// Create a Font object
                                //Aspose.Cells.Font font = style.Font;
                                //// Bold the text
                                //font.IsBold = true;
                                designWord.Workbook.Worksheets[i].Cells[Convert.ToChar(65 + h + info.Start_Column) + header].SetStyle(style);
                            }

                            // sau khi them header thi autofit lai
                            designWord.Workbook.Worksheets[i].AutoFitRows(strow, strow);

                            // gan lai tong so trang
                            ImageOrPrintOptions printoption_again = new ImageOrPrintOptions();
                            printoption_again.PrintingPage = PrintingPageType.Default;
                            CellArea[] area_again = designWord.Workbook.Worksheets[i].GetPrintingPageBreaks(printoption_again);

                            // header chua chuyen sang trang moi thi phai breakpage
                            if (area_again[k - 1].EndRow == strow)
                            {
                                designWord.Workbook.Worksheets[i].HorizontalPageBreaks.Add(Convert.ToChar(65 + info.Start_Column) + Convert.ToString(strow + 1));
                                area_again = designWord.Workbook.Worksheets[i].GetPrintingPageBreaks(printoption_again);
                            }

                            // gan totalPage = tong so trang
                            totalPage = area_again.Length;
                            strow = area_again[k + 1].StartRow;
                            strowString = area_again[k + 1].StartRow.ToString();
                            header = (strow + 1).ToString();

                        }
                    }
                    catch (Exception ex) { }

                }

                designWord.Workbook.Worksheets[i].PageSetup.SetFooter(1, "&P of " + totalPage);
            }

            return designWord;
        }
        #endregion
    }
    public class HandleMergeImageField : IFieldMergingCallback
    {

        void IFieldMergingCallback.FieldMerging(FieldMergingArgs e)
        {
            // Do nothing.
            if (e.FieldValue is bool)

            {
                DocumentBuilder builder = new DocumentBuilder(e.Document);

                // Move the “cursor” to the current merge field.

                builder.MoveToMergeField(e.FieldName);

                // It is nice to give names to check boxes. Lets generate a name such as MyField21 or so.

                string checkBoxName = string.Format("{0}{1}", e.FieldName, e.RecordIndex);

                // Insert a check box.

                builder.InsertCheckBox(checkBoxName, (bool)e.FieldValue, 0);

                // Nothing else to do for this field.

                return;

            }
        }
        ///
        /// This is called when mail merge engine encounters Image:XXX merge field in the document.

        /// You have a chance to return an Image object, file name or a stream that contains the image.

        ///
        void IFieldMergingCallback.ImageFieldMerging(ImageFieldMergingArgs e)
        {
            try
            {
                if (e.FieldValue != System.DBNull.Value)
                {
                    DocumentBuilder builder = new DocumentBuilder(e.Document);
                    builder.MoveToField(e.Field, true);

                    // Insert image and specify its size
                    builder.InsertImage((byte[])e.FieldValue, ConvertUtil.MillimeterToPoint(16), ConvertUtil.MillimeterToPoint(16));
                    e.Field.Remove();
                }
            }
            catch
            {

            }
        }
    }
}
