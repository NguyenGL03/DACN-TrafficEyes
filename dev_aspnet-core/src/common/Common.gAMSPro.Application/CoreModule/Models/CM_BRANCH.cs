using Abp.Domain.Entities;
using gAMSPro.Consts;
using gAMSPro.Sessions.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_BRANCH")]
    public class CM_BRANCH : Entity<string>, IUserLoginBranch
    {
        [Column("BRANCH_ID")]
        [MaxLength(15)]
        public override string Id { get => base.Id; set => base.Id = value; }

        [MaxLength(15)]
        public string FATHER_ID { get; set; }

        [MaxLength(1)]
        public string IS_POTENTIAL { get; set; }

        [MaxLength(10)]
        public string BRANCH_CODE { get; set; }

        [MaxLength(200)]
        public string BRANCH_NAME { get; set; }

        [MaxLength(40)]
        public string DAO_CODE { get; set; }

        [MaxLength(500)]
        public string DAO_NAME { get; set; }

        [MaxLength(15)]
        public string REGION_ID { get; set; }

        [MaxLength(5)]
        public string BRANCH_TYPE { get; set; }

        [MaxLength(200)]
        public string ADDR { get; set; }

        [MaxLength(500)]
        public string PROVICE { get; set; }

        [MaxLength(20)]
        public string TEL { get; set; }

        [MaxLength(50)]
        public string TAX_NO { get; set; }

        [MaxLength(1000)]
        public string NOTES { get; set; }

        [MaxLength(1)]
        public string RECORD_STATUS { get; set; }

        [MaxLength(15)]
        public string MAKER_ID { get; set; }

        public DateTime? CREATE_DT { get; set; }

        [MaxLength(50)]
        public string AUTH_STATUS { get; set; }

        [MaxLength(15)]
        public string CHECKER_ID { get; set; }

        public DateTime? APPROVE_DT { get; set; }

        [NotMapped]
        public bool IsApprove
        {
            get => AUTH_STATUS == ApproveStatusConsts.Approve;
            set => AUTH_STATUS = value ? ApproveStatusConsts.Approve : ApproveStatusConsts.NotApprove;
        }

    }
}
