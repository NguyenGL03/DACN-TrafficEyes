using System;
using System.Collections.Generic;
using System.Text;

namespace GSOFTcore.gAMSPro.Report.Dto
{
    public class ReportHtmlInfo
    {
        public ReportHtmlInfo()
        {
            TypeExport = "";
            HTMLString = "";
            FileName = "";
            PageInfo = "";
        }
        public string HTMLString { get; set; }
        public string TypeExport { get; set; }
        public string FileName { get; set; }


        public string PageInfo { get; set; }
    }
}
