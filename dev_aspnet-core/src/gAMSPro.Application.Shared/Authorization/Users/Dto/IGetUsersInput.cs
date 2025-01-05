using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace gAMSPro.Authorization.Users.Dto
{
    public interface IGetUsersInput : ISortedResultRequest
    {
        string Filter { get; set; }

        List<string> Permissions { get; set; }

        string Permission { get; set; }

        int? Role { get; set; }

        bool OnlyLockedUsers { get; set; }

        string UserName { get; set; }

        string AUTH_STATUS { get; set; }

        string SubbrId { get; set; }

        bool IndependentUnit { get; set; }

        string DEP_ID { get; set; }
    }
}
