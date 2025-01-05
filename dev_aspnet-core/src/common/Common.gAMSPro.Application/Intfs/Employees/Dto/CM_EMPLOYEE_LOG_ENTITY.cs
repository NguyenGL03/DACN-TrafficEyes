using System;
using System.Collections.Generic;
using System.Text;

namespace Common.gAMSPro.Intfs.Employees.Dto
{
    public class CM_EMPLOYEE_LOG_ENTITY
    {
        public int ID { get; set; }
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_PHONE { get; set; }
        public string BRANCH_CODE { get; set; }
        public string DEP_CODE { get; set; }
        public string USER_DOMAIN { get; set; }
        public string POS_CODE { get; set; }
        public string POS_NAME { get; set; }
        public DateTime? CREATE_DT { get; set; }

    }
}
