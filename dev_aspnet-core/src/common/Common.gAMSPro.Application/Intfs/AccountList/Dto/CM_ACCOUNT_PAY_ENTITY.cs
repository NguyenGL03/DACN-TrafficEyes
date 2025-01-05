using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.AccountList.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_ACCOUNT_PAY"/>
    /// </summary>
    public class CM_ACCOUNT_PAY_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string ID { get; set; }
        public string ACC_NO { get; set; }
        public string ACC_NUM { set; get; }
        public string ACC_TYPE { get; set; }
        public string ACC_NAME { get; set; }
        public string REF_ID { get; set; }
        public string TK_GL { get; set; }
        public string TK_GL_NAME { get; set; }
        public string CHECKER_ID { get; set; }
        public string MAKER_ID { get; set; }
        public string EDITOR_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public DateTime? EDITOR_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        // doanptt 260322
        public string ACCNO { get; set; }
        public string ACCNAME { set; get; }
        //doanptt 280322
        public string XMP_LIST_ACCOUNT { set; get; }
        public List<CM_ACCOUNT_ENTITY> LIST_ACCOUNT { set; get; }              // XmlData 
        public int? Top { get; set; }
        public int? TotalCount { get; set; }
        public bool? IsChecked { get; set; }
        public string RECORD_STATUS { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string MAKER_ID_NAME { get; set; } 
        //sonnq
        public string ADV_TYPE { get; set; }
        public string ADV_TYPE_NAME { get; set; }
        public string ACC_TYPE_NAME { get; set; }
    }
}
