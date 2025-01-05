using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CON_MASTER")]
    public class CON_MASTER
    {
        [Key]
        [MaxLength(15)]
        public string CONSTRUCT_ID { get; set; }
        [MaxLength(15)]
        public string CONSTRUCT_CODE { get; set; }
        [MaxLength(100)]
        public string CONSTRUCT_NAME { get; set; }
        [MaxLength(15)]
        public string PLAN_ID { get; set; }
        [MaxLength(15)]
        public string DIVI_ID { get; set; }
        [MaxLength(200)]
        public string CONSTRUCT_ADDR { get; set; }
        public decimal? LENGTH { get; set; }
        public decimal? WIDTH { get; set; }
        public decimal? CONSTRUCT_AREA { get; set; }
        public int? FLOORS { get; set; }
        public decimal? FLOORS_AREA { get; set; }
        [MaxLength(4)]
        public string YEAR_EXE { get; set; }
        [MaxLength(4)]
        public string CONSTRUCT_TYPE { get; set; }
        public decimal? TOTAL_AMT { get; set; }
        public decimal? COST_ESTIMATE { get; set; }
        public decimal? COST_EXE { get; set; }
        public decimal? COST_INCURRED { get; set; }
        [MaxLength(15)]
        public string CONST_PURPOSE { get; set; }
        [MaxLength(2000)]
        public string DESCRIPTION { get; set; }
        public DateTime? START_DT { get; set; }
        public DateTime? END_DT { get; set; }
        public DateTime? DATE_EXE { get; set; }
        public DateTime? COMPLETION_DT { get; set; }
        [MaxLength(1)]
        public string IS_FINISH { get; set; }
        public decimal? CONSTRUCT_PROGRESS { get; set; }
        [MaxLength(1)]
        public string RECORD_STATUS { get; set; }
        [MaxLength(15)]
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        [MaxLength(1)]
        public string AUTH_STATUS { get; set; }
        [MaxLength(15)]
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        [MaxLength(15)]
        public string CONST_ID { get; set; }
        [MaxLength(15)]
        public string REQUEST_ID { get; set; }
        [MaxLength(15)]
        public string BRANCH_ID { get; set; }
        [MaxLength(200)]
        public string STREET { get; set; }
        [MaxLength(15)]
        public string LOCATION { get; set; }
        [MaxLength(1000)]
        public string SCALE { get; set; }
    }
}
