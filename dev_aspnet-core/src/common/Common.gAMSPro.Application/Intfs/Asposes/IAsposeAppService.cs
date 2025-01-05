using Abp.Application.Services;
using gAMSPro.Dto;
using GSOFTcore.gAMSPro.Report.Dto;

namespace Common.gAMSPro.Intfs.AsposeSample
{
    public interface IAsposeAppService : IApplicationService
    {
        Task<FileDto> GetReport(ReportInfo info);
        Task<FileDto> GetReportHaveQR(ReportInfo info);
        FileDto GetReportFromHTML(ReportHtmlInfo info);
        Task<List<ReportTable>> GetDataFromStore(ReportInfo store);
        Task<FileDto> GetReportCustomFomart(ReportInfo info);
        Task<FileDto> GetReport_BCKH_CustomFomart(ReportInfo info);
        Task<FileDto> GetReportWordGroupFile(ReportInfo info, string groupId);
        Task<FileDto> GetQRReport(ReportInfo info);
        Task<FileDto> GetReportMultiSheet(ReportInfo info);

    }
}
