using System;
using System.Xml.Serialization;

namespace Core.gAMSPro.Application.CoreModule.Utils.AttachFile
{
    [XmlType("AttachDetail")]
    public class CM_ATTACH_FILE_ENTITY
    {
        public int ID { get; set; }
        public string ATTACH_ID { get; set; }
        public string TYPE { get; set; }
        public string REF_ID { get; set; }
        public string FILE_NAME_OLD { get; set; }
        public string PATH_OLD { get; set; }
        public string FILE_NAME_NEW { get; set; }
        public string PATH_NEW { get; set; }
        public decimal? FILE_SIZE { get; set; }
        public string FILE_TYPE { get; set; }
        public DateTime? ATTACH_DT { get; set; }
        public string EMP_ID { get; set; }
        public string INDEX { get; set; }
        public string NOTES { get; set; }
        public string VERSION { get; set; }
        public string ACTION { get; set; }
        public string STATUS { get; set; }
        public string CurrentFileItem { get; set; }
        public string REF_MASTER { get; set; }
        public string EMP_NAME { get; set; }
        public string TYPE_REQ { get; set; }
        public string TLFullName { get; set; }
        public bool? IS_VIEW { get; set; }
        //xml
        public string AttachDetail { get; set; }
    }
}
