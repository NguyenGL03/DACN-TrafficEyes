using gAMSPro.Helpers;

namespace GSOFTcore.gAMSPro.Report.Dto
{
    public class ReportInfo
    {
        public ReportInfo()
        {
            TypeExport = "";
            StoreName = "";
            PathName = null;
            Parameters = new List<ReportParameter>();
            Values = new List<ReportParameter>(); 
            Header = new List<string>();
            Start_Column = 0;
            Start_Row = 0;
            addHeader = false;
            HaveQR = false;
            orientaiton_page = "portrait";
        }
        public bool? HaveQR { get; set; }
        public string StoreName { get; set; }
        public string TypeExport { get; set; }
        public string PathName { get; set; }
        public string? FileName { get; set; }
        public bool? ProcessMerge { get; set; }
        public List<ReportParameter> Parameters { get; set; }
        public List<ReportParameter> Values { get; set; }
        public string? pageName { get; set; }
        public string? groupId { get; set; }
        public string? TypePrint { get; set; }
        //06/06/23
        public List<string> Header { get; set; }
        public int Start_Column { get; set; }
        public int Start_Row { get; set; }
        public bool? addHeader { get; set; }
        public string orientaiton_page { get; set; }
    }
}
