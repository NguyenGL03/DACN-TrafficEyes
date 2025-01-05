﻿using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_GOODSTYPE_REAL")]
    public class CM_GOODSTYPE_REAL : Entity<string>
    {
        [Column("GD_RETYPE_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string GD_RETYPE_CODE { get; set; }
        public string TYPE_NAME { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }
}
