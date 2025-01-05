using System.Xml.Serialization;

namespace Common.gAMSPro.AccountList.Dto
{
    [XmlType("XmlData")]
    public class CM_ACCOUNT_ENTITY
    {
        public string ACC_NO { get; set; }
        public string ACC_NAME { set; get; }
        public string ACCT { set; get; }

    }
}
