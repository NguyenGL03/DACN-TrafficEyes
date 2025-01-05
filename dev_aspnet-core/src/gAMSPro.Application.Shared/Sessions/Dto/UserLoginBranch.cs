using System;

namespace gAMSPro.Sessions.Dto
{
    public interface IUserLoginBranch
    {
        string ADDR { get; set; }
        DateTime? APPROVE_DT { get; set; }
        string AUTH_STATUS { get; set; }
        string BRANCH_CODE { get; set; }
        string BRANCH_NAME { get; set; }
        string BRANCH_TYPE { get; set; }
        string CHECKER_ID { get; set; }
        DateTime? CREATE_DT { get; set; }
        string DAO_CODE { get; set; }
        string DAO_NAME { get; set; }
        string FATHER_ID { get; set; }
        string Id { get; set; }
        string IS_POTENTIAL { get; set; }
        bool IsApprove { get; set; }
        string MAKER_ID { get; set; }
        string NOTES { get; set; }
        string PROVICE { get; set; }
        string RECORD_STATUS { get; set; }
        string REGION_ID { get; set; }
        string TAX_NO { get; set; }
        string TEL { get; set; }
    }
}
