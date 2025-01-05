using System;
using System.Collections.Generic;
using System.Text;

namespace Common.gAMSPro.Intfs.Dashboards.Dto
{
    public class DashboardParent<T>
    {
        public string USER_LOGIN { get; set; }
        public string BRANCH_ID { get; set; }
        public List<T> DASHBOARD_CHILDEN { get; set; }
        public string XML { get; set; }
        public DateTime? CREATE_DT { get; set; }
    }
}
