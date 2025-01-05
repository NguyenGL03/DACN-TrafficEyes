using Abp.Zero.EntityFrameworkCore;
using Common.gAMSPro.CoreModule.Models;
using Common.gAMSPro.Models;
using gAMSPro.Authorization.Delegation;
using gAMSPro.Authorization.Roles;
using gAMSPro.Authorization.Users;
using gAMSPro.Chat;
using gAMSPro.Editions;
using gAMSPro.ExtraProperties;
using gAMSPro.Friendships;
using gAMSPro.MultiTenancy;
using gAMSPro.MultiTenancy.Accounting;
using gAMSPro.MultiTenancy.Payments;
using gAMSPro.OpenIddict.Applications;
using gAMSPro.OpenIddict.Authorizations;
using gAMSPro.OpenIddict.Scopes;
using gAMSPro.OpenIddict.Tokens;
using gAMSPro.Storage;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace gAMSPro.EntityFrameworkCore
{
    public class gAMSProDbContext : AbpZeroDbContext<Tenant, Role, User, gAMSProDbContext>, IOpenIddictDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<OpenIddictApplication> Applications { get; }

        public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

        public virtual DbSet<OpenIddictScope> Scopes { get; }

        public virtual DbSet<OpenIddictToken> Tokens { get; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public virtual DbSet<TL_MENU> TL_MENUs { get; set; }

        public virtual DbSet<CM_BRANCH> CM_BRANCHes { get; set; }

        public virtual DbSet<CM_DEPARTMENT> CM_DEPARTMENTs { get; set; }




        #region Common.gAMSPro.Core

        public virtual DbSet<SYS_CODEMASTERS> CodeMasters { get; set; }
        public virtual DbSet<SYS_PREFIX> AppPrefixs { get; set; }
        public virtual DbSet<SysError> SysErrors { get; set; }
        public virtual DbSet<CM_ALLCODE> CM_ALLCODEs { get; set; }
        public virtual DbSet<CM_REGION> CM_REGIONs { get; set; }
        public virtual DbSet<CM_AUTH_STATUS> CM_AUTH_STATUSs { get; set; }
        public virtual DbSet<CON_MASTER> CON_MASTERs { get; set; }
        public virtual DbSet<CM_EMPLOYEE> CM_EMPLOYEEs { get; set; }
        public virtual DbSet<CM_EMPLOYEE_LOG> CM_EMPLOYEE_LOGs { get; set; }
        public virtual DbSet<CM_DEPT_GROUP> CM_DEPT_GROUPs { get; set; }
        public virtual DbSet<CM_SUPPLIERTYPE> CM_SUPPLIERTYPEs { get; set; }
        public virtual DbSet<CM_SUPPLIER> CM_SUPPLIERs { get; set; }
        public virtual DbSet<CM_GOODSTYPE> CM_GOODSTYPEs { get; set; }
        public virtual DbSet<CM_UNIT> CM_UNITs { get; set; }
        public virtual DbSet<CM_GOODS> CM_GOODSs { get; set; }
        public virtual DbSet<CM_DIVISION> CM_DIVISIONs { get; set; }
        public virtual DbSet<CM_INSU_COMPANY> CM_INSU_COMPANYs { get; set; }
        public virtual DbSet<CM_MODEL> CM_MODELs { get; set; }
        public virtual DbSet<CM_CAR_TYPE> CM_CAR_TYPEs { get; set; }
        public virtual DbSet<SYS_PARAMETERS> SYS_PARAMETERSs { get; set; }
        public virtual DbSet<CM_WORKFLOW> CM_WORKFLOWs { get; set; }
        public virtual DbSet<CM_WORKFLOW_HIST> CM_WORKFLOW_HISTs { get; set; }
        public virtual DbSet<CM_WORKFLOW_ASSIGN> CM_WORKFLOW_ASSIGNs { get; set; }
        public virtual DbSet<CM_WORKFLOW_DETAIL> CM_WORKFLOW_DETAILs { get; set; }
        public virtual DbSet<CM_REPORT_TEMPLATE> CM_REPORT_TEMPLATEs { get; set; }
        public virtual DbSet<CM_REPORT_TEMPLATE_DETAIL> CM_REPORT_TEMPLATE_DETAILs { get; set; }
        public virtual DbSet<CM_GOODSTYPE_REAL> CM_GOODSTYPE_REALs { get; set; }

        #endregion

        public gAMSProDbContext(DbContextOptions<gAMSProDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(b =>
            {
                b.Property(x => x.EmailAddress).IsRequired(false);
                b.Property(x => x.Name).IsRequired(false);
                b.Property(x => x.Surname).IsRequired(false);
                b.Property(x => x.AccessFailedCount).HasDefaultValue(0);
                b.Property(x => x.IsLockoutEnabled).HasDefaultValue(false);
                b.Property(x => x.IsPhoneNumberConfirmed).HasDefaultValue(false);
                b.Property(x => x.IsTwoFactorEnabled).HasDefaultValue(false);
                b.Property(x => x.IsEmailConfirmed).HasDefaultValue(false);
                b.Property(x => x.IsActive).HasDefaultValue(false);
                b.Property(x => x.NormalizedUserName).IsRequired(false);
                b.Property(x => x.NormalizedEmailAddress).IsRequired(false);
                b.Property(x => x.ShouldChangePasswordOnNextLogin).HasDefaultValue(false);
                b.Property(x => x.SendActivationEmail).HasDefaultValue(false);
            });

            modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

            modelBuilder.Entity<SubscriptionPayment>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigureOpenIddict();
        }
    }
}