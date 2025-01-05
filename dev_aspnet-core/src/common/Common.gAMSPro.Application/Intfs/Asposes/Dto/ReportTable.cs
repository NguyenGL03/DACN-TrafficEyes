using System;
using System.Collections.Generic;
using System.Text;

namespace GSOFTcore.gAMSPro.Report.Dto
{
    public class ReportTable
    {
        public ReportTable()
        {
            Columns = new List<ReportColumn>();
            Rows = new List<ReportRow>();
        }
        public string TableName { get; set; }
        public List<ReportColumn> Columns { get; set; }
        public List<ReportRow> Rows { get; set; }
    }
}
