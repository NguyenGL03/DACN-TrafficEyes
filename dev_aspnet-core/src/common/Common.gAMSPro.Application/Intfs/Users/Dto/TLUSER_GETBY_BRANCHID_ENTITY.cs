using gAMSPro.Dto;

namespace Common.gAMSPro.Intfs.Users.Dto
{
    public class TLUSER_GETBY_BRANCHID_ENTITY : PagedAndSortedInputDto
    {
        public string TLID { get; set; }
        public string TLNANME { get; set; }
        public string Password { get; set; }
        public string TLFullName { get; set; }
        public string TLSUBBRID { get; set; }
        public string BRANCH_NAME { get; set; }
        public string BRANCH_TYPE { get; set; }
        public string BRANCH_ID { get; set; }
        public string LEVEL { get; set; }
        public string RoleName { get; set; }
        public string EmailAddress { get; set; }
        public string Roles { get; set; }
        public int? ROLE_ID { get; set; }
        public string EMAIL { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string AUTH_STATUS { get; set; }
        public string MARKER_ID { get; set; }
        public string AUTH_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string ISAPPROVE { get; set; }
        public DateTime? Birthday { get; set; }
        public string ISFIRSTTIME { get; set; }
        public string SECUR_CODE { get; set; }
        public int? TotalCount { get; set; }
    }
}
