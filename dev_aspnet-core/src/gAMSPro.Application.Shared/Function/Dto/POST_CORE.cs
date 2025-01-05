using System;
using System.Collections.Generic;
using System.Text;

namespace GSOFTcore.gAMSPro.Function.Dto
{
    public class ASS_ENTRIES_POST_CoreByRef
    {
        public string ET_ID { get; set; }
        public string CR_ACCT { get; set; }
        public string CR_BRN { get; set; }
        public string DR_ACCT { get; set; }
        public string DR_BRN { get; set; }
        public decimal? AMT { get; set; }
        public string CURRENCY { get; set; }
        public int EXCRATE { get; set; }
        public string MAKER_ID { get; set; }
        public string CHECKER_ID { get; set; }
        public string TRN_DESC { get; set; }
        public string DO_BRN { get; set; }
    }
    public class ASS_ENTRIES_POST_UpdRef
    {
        public string RESULT { get; set; }
        public string ERROR { get; set; }
    }

    public class MW_ENTRIES_POST_CoreByRef
    {
        public string ET_ID { get; set; }
        public string CR_ACCT { get; set; }
        public string CR_BRN { get; set; }
        public string DR_ACCT { get; set; }
        public string DR_BRN { get; set; }
        public string DR_DEP { get; set; }
        public decimal? AMT { get; set; }
        public string CURRENCY { get; set; }
        public int EXCRATE { get; set; }
        public string MAKER_ID { get; set; }
        public string CHECKER_ID { get; set; }
        public string TRN_DESC { get; set; }
        public string DO_BRN { get; set; }
        public string REF_ID { get; set; }
    }
    public class MW_ENTRIES_POST_UpdRef
    {
        public string RESULT { get; set; }
        public string ERROR { get; set; }
    }

    public class PAY_ENTRIES_POST_CoreByRef
    {
        public string ET_ID { get; set; }
        public string ENTRY_PAIR { get; set; }
        public string DRCR { get; set; }
        public string ACCT { get; set; }
        public decimal? AMT { get; set; }
        public string CURRENCY { get; set; }
        public decimal? EXC_RATE { get; set; }
        public string TRN_DESC { get; set; }
        public string MAKER_ID { get; set; }
        public string CHECKER_ID { get; set; }
        public string BRANCH_CODE { get; set; }
        public string DEP_CODE { get; set; }
        public string DO_BRN { get; set; }
    }
    public class PAY_ENTRIES_POST_UpdRef
    {
        public string RESULT { get; set; }
        public string ERROR { get; set; }
    }

    public class ASS_ENTRIES_POST_CoreByRef_Tool
    {
        public string ET_ID { get; set; }
        public string CR_ACCT { get; set; }
        public string CR_BRN { get; set; }
        public string DR_DEP { get; set; }
        public string DR_ACCT { get; set; }
        public string DR_BRN { get; set; }
        public decimal? AMT { get; set; }
        public string CURRENCY { get; set; }
        public int EXCRATE { get; set; }
        public string MAKER_ID { get; set; }
        public string CHECKER_ID { get; set; }
        public string TRN_DESC { get; set; }
        public string DO_BRN { get; set; }
    }

    public class CM_ACCOUNT
    {
        public int ID { get; set; }
        public string ACC_NO { get; set; }
        public string ACC_NAME { get; set; }
        public string TK_GL { get; set; }
        public string TK_GL_NAME { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string EDITOR_ID { get; set; }
        public DateTime? EDITOR_DT { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
    }
    public class SYS_PARAMETERS_ById
    {
        public decimal ID { get; set; }
        public string ParaKey { get; set; }
        public string ParaValue { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }


}
