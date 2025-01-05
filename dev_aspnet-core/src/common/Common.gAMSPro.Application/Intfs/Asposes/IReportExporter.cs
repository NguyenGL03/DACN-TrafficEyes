using Abp.Dependency;
using Aspose.Cells;
using gAMSPro.Dto;
using GSOFTcore.gAMSPro.Report.Dto;

namespace Common.gAMSPro.Intfs.Report
{
    public interface IReportExporter : ITransientDependency
    {
        Task<MemoryStream> GetReportFile(ReportInfo info);
        MemoryStream GetReportFileFromHtml(ReportHtmlInfo info);
        Task<WorkbookDesigner> CreateExcelFileAndDesign(ReportInfo info);
        Task<MemoryStream> GetReportFileCustomFomart(ReportInfo info);
        Task<MemoryStream> GetReportFile_BCKH_CustomFomart(ReportInfo info);
        Task<MemoryStream> GetReportWordGroupFile(ReportInfo info, string groupId);
        Task<MemoryStream> GetReportFileQR(ReportInfo info);
        Task<MemoryStream> GetReportFileMultiSheet(ReportInfo info);
        MemoryStream GetFile(FileDto file);
    }
}
