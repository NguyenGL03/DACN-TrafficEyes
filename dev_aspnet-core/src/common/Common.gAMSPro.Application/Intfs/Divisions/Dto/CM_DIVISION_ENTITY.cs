﻿using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System;

namespace Common.gAMSPro.Divisions.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_DIVISION"/>
    /// </summary>
    public class CM_DIVISION_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? DIV_ID { get; set; }
        public string? DIV_CODE { get; set; }
        public string? DIV_NAME { get; set; }
        public string? ADDR { get; set; }
        public string? BRANCH_ID { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? BRANCH_CODE { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? BRANCH_LOGIN { get; set; }
        public bool? INDEPENDENT_UNIT { get; set; }
        public int? TotalCount { get; set; }
    }
}
