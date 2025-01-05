using Common.gAMSPro.Intfs.AsposeSample;
using Common.gAMSPro.Web.Controllers;
using gAMSPro.Dto;
using GSOFTcore.gAMSPro.Report.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AsposeController : CoreAmsProControllerBase
    {
        private readonly IAsposeAppService asposeAppService;

        public AsposeController(IAsposeAppService asposeAppService)
        {
            this.asposeAppService = asposeAppService;
        }

        [HttpPost]
        public async Task<FileDto> GetReport([FromBody] ReportInfo info)
        { 
            return await asposeAppService.GetReport(info);
        }
        [HttpPost]
        public async Task<FileDto> GetReportHaveQR([FromBody] ReportInfo info)
        { 
            return await asposeAppService.GetReportHaveQR(info);
        }

        [HttpPost]
        public async Task<FileDto> GetReportWordGroupFile([FromBody] ReportInfo info, string groupId)
        {
            return await asposeAppService.GetReportWordGroupFile(info, groupId);
        }
        [HttpPost]
        public FileDto GetReportFromHTML([FromBody] ReportHtmlInfo info)
        {
            return asposeAppService.GetReportFromHTML(info);
        }

        [HttpPost]
        public async Task<List<ReportTable>> GetDataFromStore([FromBody] ReportInfo store)
        {
            return await asposeAppService.GetDataFromStore(store);
        }
        [HttpPost]
        public async Task<FileDto> GetReportCustomFomart([FromBody] ReportInfo info)
        {
            return await asposeAppService.GetReportCustomFomart(info);
        }

        [HttpPost]
        public async Task<FileDto> GetReport_BCKH_CustomFomart([FromBody] ReportInfo info)
        {
            return await asposeAppService.GetReport_BCKH_CustomFomart(info);
        }

        [HttpPost]
        public async Task<FileDto> GetReportQR([FromBody] ReportInfo info)
        {
            return await asposeAppService.GetQRReport(info);
        }

        [HttpPost]
        public async Task<FileDto> GetReportMultiSheet([FromBody] ReportInfo info)
        {
            return await asposeAppService.GetReportMultiSheet(info);
        }

    }
}
