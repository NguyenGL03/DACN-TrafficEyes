using System;

namespace gAMSPro.ModelHelpers
{
    public interface IAuditDto
    {
        string RECORD_STATUS { get; set; }
        string MAKER_ID { get; set; }
        DateTime? CREATE_DT { get; set; }
        string AUTH_STATUS { get; set; }
        string CHECKER_ID { get; set; }
        DateTime? APPROVE_DT { get; set; }
    }
}
