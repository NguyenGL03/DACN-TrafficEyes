using Core.gAMSPro.CoreModule.Utils;
using System;
using System.Xml.Serialization;

namespace PlanMaster.gAMSPro.Intfs.PlanRequestDoc.Dto
{
    [XmlType("ListREQ")]
    public class PL_REQUEST_PROCESS_CHILD : EdiableTableRowDto
    {
        public int ID { get; set; }
        public string REQ_ID { get; set; }
        public string REF_ID { get; set; }
        public string PROCESS_ID { get; set; }

        public string TLNAME { get; set; }
        public string LEVEL_JOB { get; set; }
        public string TYPE_JOB { get; set; }
        public string STATUS_JOB { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? TRANFER_DT { get; set; }
    }
}
