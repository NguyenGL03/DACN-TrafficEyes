using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common.gAMSPro.Intfs.AttachFiles.Dto
{
    [XmlType("ImageDetail")]
    public class CM_IMAGE_ENTITY
    {
        public string IMAGE_ID { get; set; }

        public string FILE_NAME { get; set; }
        public string PATH { get; set; }

        [XmlIgnore]
        public string REF_ID { get; set; }
        [XmlIgnore]
        public string RECORD_STATUS { get; set; }
        [XmlIgnore]

        public string AUTH_STATUS { get; set; }
        [XmlIgnore]

        public string MAKER_ID { get; set; }
        [XmlIgnore]

        public DateTime? CREATE_DT { get; set; }
        [XmlIgnore]

        public string CHECKER_ID { get; set; }
        [XmlIgnore]

        public DateTime? APPROVE_DT { get; set; }
        [XmlIgnore]
        public string BASE64 { get; set; }
        [XmlIgnore]
        public string IMAGE_NAME { get; set; }
    }
}
