using gAMSPro.Dto;
using System;

namespace Common.gAMSPro.Intfs.Process.Dto
{
    public class PROCESS_ENTITY : PagedAndSortedInputDto
    {
        public int ID { get; set; }
        public string REQ_ID { get; set; }
        public string PROCESS_ID { get; set; }
        public string CHECKER_ID { get; set; }
        public string CHECKER_NAME { get; set; }
        public DateTime APPROVE_DT { get; set; }
        public string PROCESS_DESC { get; set; }
        public string NOTES { get; set; }
        public string TYPE_JOB { get; set; }
        public int? REF_ID { get; set; }
        public string TLNAME { get; set; }
    }
}
