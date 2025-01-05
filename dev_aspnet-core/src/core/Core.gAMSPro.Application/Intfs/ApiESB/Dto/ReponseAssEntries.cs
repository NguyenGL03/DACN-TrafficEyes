using System.Xml.Serialization;

namespace Core.gAMSPro.Intfs.ApiESB.Dto
{
    [XmlType("XmlData")]
    public class Transaction
    {
        public string Branch { get; set; }
        public string SBV { get; set; }
        public string GLSH { get; set; }
        public string SchemeCd { get; set; }
        public string AccountNo { get; set; }
        public string CCY { get; set; }
        public string DrCr { get; set; }
        public string Amount { get; set; }
    }
    public class ReponseAssEntries
    {
        public string Status { get; set; }
        public string Tranid { get; set; }
        public string Trandate { get; set; }
        public string EntryDate { get; set; }
        public string ValueDate { get; set; }
        public string InitSolID { get; set; }
        public string TotalAmount { get; set; }
        public string SRN { get; set; }
        public List<Transaction> TRANS { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public string? XmlData { get; set; }
        public string? Trn_id { get; set; }
    }
}
