using System;
using System.Collections.Generic;
using System.Text;

namespace GSOFTcore.gAMSPro.Report.Dto
{
    public class ReportRow
    {
        public ReportRow()
        {
            Cells = new List<object>();
        }
        public List<object> Cells { get; set; }
    }
}
