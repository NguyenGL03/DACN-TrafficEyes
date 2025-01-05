using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gAMSPro.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    [Table("TL_USER")]
    public class User : AbpUser<User>
    {
        [Column("TLID")]
        [MaxLength(15)]
        public string TlId { get; set; }

        [Column("TLNANME")]
        [MaxLength(15)]
        public override string UserName { get { return base.UserName; } set { base.UserName = value; } }

        [Column("Name")]
        [MaxLength(200)]
        public string Fullname { get; set; }

        [Column("TLFullName")]
        public override string Name { get => base.Name; set => base.Name = value; }

        [Column("TLSUBBRID")]
        [MaxLength(15)]
        public string SubbrId { get; set; }

        [Column("BRANCH_NAME")]
        [MaxLength(200)]
        public string BranchName { get; set; }

        [Column("BRANCH_TYPE")]
        [MaxLength(5)]
        public string BranchType { get; set; }

        [Column("RoleName")]
        [MaxLength(20)]
        public string RoleName { get; set; }

        [Column("EMAIL")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Column("ADDRESS")]
        [MaxLength(100)]
        public string Address { get; set; }

        [Column("PHONE")]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Column("AUTH_STATUS")]
        [MaxLength(1)]
        public string AuthStatus { get; set; }

        [Column("MARKER_ID")]
        [MaxLength(12)]
        public string MarkerId { get; set; }

        [Column("AUTH_ID")]
        [MaxLength(12)]
        public string AuthId { get; set; }

        [Column("APPROVE_DT")]
        public DateTime? ApproveDt { get; set; }

        [Column("ISAPPROVE")]
        [MaxLength(1)]
        public string IsApprove { get; set; }

        [Column("Birthday")]
        public DateTime? Birthday { get; set; }

        [Column("ISFIRSTTIME")]
        [MaxLength(1)]
        public string IsFirstTime { get; set; }

        [Column("SECUR_CODE")]
        [MaxLength(50)]
        public string SECUR_CODE { get; set; }

        public string DEP_ID { get; set; }

        public string EMP_ID { get; set; }

        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public string SignInToken { get; set; }

        public string GoogleAuthenticatorKey { get; set; }

        public string RecoveryCode { get; set; }

        public bool SendActivationEmail { get; set; }

        public List<UserOrganizationUnit> OrganizationUnits { get; set; }

        //Can add application specific user properties here

        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="name">Name of admin user</param>
        /// <param name="surname">Surname of admin user</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string name = null, string surname = null)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>(),
                OrganizationUnits = new List<UserOrganizationUnit>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }
        public void Lock()
        {
            AccessFailedCount = 0;
            IsActive = false;
            LockoutEndDateUtc = null;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }

        [NotMapped]
        public override DateTime? LastModificationTime { get => base.LastModificationTime; set => base.LastModificationTime = value; }

        [NotMapped]
        public override DateTime? DeletionTime { get => base.DeletionTime; set => base.DeletionTime = value; }
    }
}