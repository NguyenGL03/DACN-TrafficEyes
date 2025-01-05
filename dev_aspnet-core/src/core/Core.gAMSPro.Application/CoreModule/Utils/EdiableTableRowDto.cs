using System.Xml.Serialization;

namespace Core.gAMSPro.CoreModule.Utils
{
    public class EdiableTableRowDto
    {
        [XmlIgnore]
        public int No { get; set; }
        [XmlIgnore]
        public int Page { get; set; }
        [XmlIgnore]
        public bool? IsChecked { get; set; }
    }
}
