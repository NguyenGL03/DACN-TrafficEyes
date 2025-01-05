using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.TlUsers.Dto
{
    public class TL_USER_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? TLID { get; set; }
        public string? TLNANME { get; set; }
        public string? Password { get; set; }
        public string? TLFullName { get; set; }
        public string? TLSUBBRID { get; set; }
        public string? AUTHORITY_USER { get; set; }
        public string? BRANCH_ID { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? BRANCH_TYPE { get; set; }
        public string? ROLE_ID { get; set; }
        public string? RoleName { get; set; }
        public string? ROLES { get; set; }
        public string? EMAIL { get; set; }
        public string? LEVEL { get; set; }
        public string? ADDRESS { get; set; }
        public string? PHONE { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? MARKER_ID { get; set; }
        public string? AUTH_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? ISAPPROVE { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ISFIRSTTIME { get; set; }
        public string? SECUR_CODE { get; set; }
        public int AccessFailedCount { get; set; }
        public string? AuthenticationSource { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public long? CreatorUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsLockoutEnabled { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public string? Name { get; set; }
        public string? NormalizedEmailAddress { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? PasswordResetCode { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? ProfilePictureId { get; set; }
        public string? SecurityStamp { get; set; }
        public bool ShouldChangePasswordOnNextLogin { get; set; }
        public string? Surname { get; set; }
        public int? TenantId { get; set; }
        public string? SignInToken { get; set; }
        public DateTime? SignInTokenExpireTimeUtc { get; set; }
        public string? GoogleAuthenticatorKey { get; set; }
        public bool SendActivationEmail { get; set; }
        public long ID { get; set; }
        public string? DEP_ID { get; set; }
        public DateTime CreationTime { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? BRANCH_CODE { get; set; }
        public string? KHU_VUC { get; set; }
        public string? CHI_NHANH { get; set; }
        public string? PGD { get; set; }
        public string? TAX_NO { get; set; }
        public string[]? AssignedRoleNames { get; set; }
        public bool? SetRandomPassword { get; set; }
        public string? PasswordRepeat { get; set; }
        public int? TotalCount { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? CHECKER_ID { get; set; }
        public string? POS_NAME { get; set; }
        public string? EMP_CODE { get; set; }
        public string? DEP_NAME { get; set; }
        public string? DEP_CODE { get; set; }
        public string? ACC_NUM { get; set; }
        public string? ACC_TYPE { get; set; }
        public string? BR_FULL_NAME { get; set; }
        public string? DP_FULL_NAME { get; set; }
        public string? ACC_NAME { get; set; }
        public string? KHOI_ID { get; set; }
        public string? KHOI_CODE { get; set; }
        public string? KHOI_NAME { get; set; }
        public string? USER_LOGIN { get; set; }
        public string? EMP_ID { get; set; }
    }
}
