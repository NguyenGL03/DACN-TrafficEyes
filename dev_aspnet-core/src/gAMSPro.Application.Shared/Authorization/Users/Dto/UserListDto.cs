using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace gAMSPro.Authorization.Users.Dto
{
    public class UserListDto : EntityDto<long>, IPassivable, IHasCreationTime
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string SubbrName { get; set; }

        public string DeptName { get; set; }

        public string PhoneNumber { get; set; }

        public string AuthStatusName { get; set; }

        public string AuthStatus { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public List<UserListRoleDto> Roles { get; set; }

        public bool IsActive { get; set; }

        public string MarkerId { get; set; }

        public DateTime CreationTime { get; set; }

        public bool? IsChecked { get; set; }

        public string EMP_CODE { get; set; }

        public string MA_CHUC_DANH { get; set; }

        public string TEN_CHUC_DANH { get; set; }
    }
}