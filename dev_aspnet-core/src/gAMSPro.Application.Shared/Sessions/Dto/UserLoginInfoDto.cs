using Abp.Application.Services.Dto;
using gAMSPro.Authorization.Users.Profile.Dto;

namespace gAMSPro.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePictureId { get; set; }

        public string TaxNo { get; set; }

        public string SubbrId { get; set; }

        public string RoleName { get; set; }

        public string BranchName { get; set; }

        public string DEP_ID { get; set; }

        public string DEP_PARENT_ID { get; set; }

        public string Roles { get; set; }

        public string DEP_NAME { get; set; }

        public string DEP_CODE { get; set; }

        public string SECUR_CODE { get; set; }

        public string BranchCode { get; set; }

        public GetProfilePictureOutput ProfilePicture { get; set; }

        public IUserLoginBranch Branch { get; set; }
    }
}
