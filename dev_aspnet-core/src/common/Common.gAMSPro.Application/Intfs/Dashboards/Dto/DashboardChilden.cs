using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common.gAMSPro.Intfs.Dashboards.Dto
{
    [XmlType("XmlData")]

    // Dashboard 01
    public class DB_STATUS_ASSET_BAR
    {
        public string TYPE_ID { get; set; }
        public string GROUP_ID { get; set; }
        public string FROM_MONTH { get; set; }
        public string TO_MONTH { get; set; }
        public string FROM_YEAR { get; set; }
        public string TO_YEAR { get; set; }
        public string FILTER { get; set; }
        public string AMORT_DATE_CHECK { get; set; }
        public string USE_DATE_KT_CHECK { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
    }
    public class DB_STATUS_ASSET_PIE
    {
        //input
        public string YEAR { get; set; }

        //output
        public string AMORT_STATUS { get; set; }
        public float TOTAL_COUNT { get; set; }

        public string TYPE_ID { get; set; }
    }

    // Dashboard 02
    public class DB_ASSET_PIE
    {
        // input
        public string TYPE_ID { get; set; }
        public string GROUP_ID { get; set; }
        public string BRANCH_ID { get; set; }
        public string YEAR { get; set; }

        //output
        public string AMORT_STATUS { get; set; }
        public float TOTAL_COUNT { get; set; }

        public string AMORT_AMT { get; set; }
        public string SUMARY_GTCL { get; set; }
        public string SUMARY_GTKH { get; set; }
    }
    
    // Dashboard 03
    public class DB_BUGET
    {
        public string YEAR { get; set; }
        public DateTime? DATE { get; set; }
        public string GD_ID { get; set; }
        public string PLAN_TYPE_ID { get; set; }
        public string GD_TYPE_ID { get; set; }
        public string FILTER { get; set; }

        public string MONTH { get; set; }
        public string PLAN { get; set; }
        public string MADE { get; set; }
        public string DOING { get; set; }
        public string RESIDUAL { get; set; }
    }

    // Dashboard 04
    public class DB_BUY_VALUE
    {
        public string FROM_YEAR { get; set; }
        public string TO_YEAR { get; set; }
        public string FILTER { get; set; }

        //ouptut
        public string SUMARY_HANDLING { get; set; }
        public string SUMARY_COMPLETED { get; set; }

        public string PLAN { get; set; }
        public string MADE { get; set; }
        public string DOING { get; set; }
        public string RESIDUAL { get; set; }
    }
}
