using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Runtime.Session;
using Common.gAMSPro.Models;
using gAMSPro.Authentication.TwoFactor;
using gAMSPro.Authorization.Delegation;
using gAMSPro.Authorization.Roles;
using gAMSPro.Authorization.Users;
using gAMSPro.Editions;
using gAMSPro.Features;
using gAMSPro.MultiTenancy.Payments;
using gAMSPro.Sessions.Dto;
using gAMSPro.UiCustomization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gAMSPro.Sessions
{
    public class SessionAppService : gAMSProAppServiceBase, ISessionAppService
    {
        private readonly IUiThemeCustomizerFactory _uiThemeCustomizerFactory;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserDelegationConfiguration _userDelegationConfiguration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly EditionManager _editionManager;
        private readonly RoleManager roleManager;
        private readonly ILocalizationContext _localizationContext;

        private readonly IRepository<User, long> userRepository;
        private readonly IRepository<CM_BRANCH, string> branchRepository;
        private readonly IRepository<CM_DEPARTMENT, string> depRepository;
        private readonly IRepository<UserRole, long> userRoleRepository;

        public SessionAppService(
            IUiThemeCustomizerFactory uiThemeCustomizerFactory,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserDelegationConfiguration userDelegationConfiguration,
            IUnitOfWorkManager unitOfWorkManager,
            EditionManager editionManager, ILocalizationContext localizationContext,
            RoleManager roleManager,
            IRepository<CM_BRANCH, string> branchRepository,
            IRepository<User, long> userRepository,
            IRepository<CM_DEPARTMENT, string> depRepository,
            IRepository<UserRole, long> userRoleRepository
        )
        {
            _uiThemeCustomizerFactory = uiThemeCustomizerFactory;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userDelegationConfiguration = userDelegationConfiguration;
            _unitOfWorkManager = unitOfWorkManager;
            _editionManager = editionManager;
            _localizationContext = localizationContext;
            this.roleManager = roleManager;
            this.userRoleRepository = userRoleRepository;
            this.userRepository = userRepository;
            this.branchRepository = branchRepository;
            this.depRepository = depRepository;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            return await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var output = new GetCurrentLoginInformationsOutput
                {
                    Application = new ApplicationInfoDto
                    {
                        Version = AppVersionHelper.Version,
                        ReleaseDate = AppVersionHelper.ReleaseDate,
                        Features = new Dictionary<string, bool>(),
                        Currency = gAMSProConsts.Currency,
                        CurrencySign = gAMSProConsts.CurrencySign,
                        AllowTenantsToChangeEmailSettings = gAMSProConsts.AllowTenantsToChangeEmailSettings,
                        UserDelegationIsEnabled = _userDelegationConfiguration.IsEnabled,
                        TwoFactorCodeExpireSeconds = TwoFactorCodeCacheItem.DefaultSlidingExpireTime.TotalSeconds
                    }
                };

                var uiCustomizer = await _uiThemeCustomizerFactory.GetCurrentUiCustomizer();
                output.Theme = await uiCustomizer.GetUiSettings();

                if (AbpSession.TenantId.HasValue)
                {
                    output.Tenant = await GetTenantLoginInfo(AbpSession.GetTenantId());
                }

                if (AbpSession.ImpersonatorTenantId.HasValue)
                {
                    output.ImpersonatorTenant = await GetTenantLoginInfo(AbpSession.ImpersonatorTenantId.Value);
                }

                if (AbpSession.UserId.HasValue)
                {
                    var userQuery = userRepository.GetAll().IgnoreQueryFilters();
                    var branchQuery = branchRepository.GetAll().IgnoreQueryFilters();
                    var depQuery = depRepository.GetAll().IgnoreQueryFilters();
                    var userRole = userRoleRepository.GetAll().IgnoreQueryFilters();
                    var roleQuery = roleManager.Roles;
                    var roleKeys = (from user in userQuery
                                    join userRoles in userRole on user.Id equals userRoles.UserId
                                    join roles in roleQuery on userRoles.RoleId equals roles.Id
                                    where user.Id == AbpSession.UserId
                                    select new
                                    {
                                        roles.DisplayName
                                    }
                        ).Distinct().ToList();

                    output.RoleKeys = roleKeys.Select(x => x.DisplayName.ToString()).ToList();

                    var result = (from user in userQuery
                                  join b in branchQuery on user.SubbrId equals b.Id into branches
                                  from b in branches.DefaultIfEmpty()
                                  join d in depQuery on user.DEP_ID equals d.Id into departments
                                  from d in departments.DefaultIfEmpty()
                                  where user.Id == AbpSession.UserId
                                  select new
                                  {
                                      user,
                                      b,
                                      d.DEP_NAME,
                                      d.DEP_CODE,  
                                      d.FATHER_ID
                                  }).FirstOrDefault();

                    CM_BRANCH branch = new CM_BRANCH();

                    if (result != null)
                    {
                        output.User = ObjectMapper.Map<UserLoginInfoDto>(result.user);
                        branch = result.b;

                        if (branch == null)
                        {
                            branch = new CM_BRANCH();
                        }
                    }

                    output.User.BranchName = branch.BRANCH_NAME;
                    output.User.BranchCode = branch.BRANCH_CODE;
                    output.User.TaxNo = branch.TAX_NO;
                    output.User.Branch = branch;
                    output.User.DEP_NAME = result.DEP_NAME;
                    output.User.DEP_CODE = result.DEP_CODE;
                    output.User.EmailAddress = result.user.Email;
				output.User.DEP_PARENT_ID = result.FATHER_ID;

                }

                if (AbpSession.ImpersonatorUserId.HasValue)
                {
                    output.ImpersonatorUser = ObjectMapper.Map<UserLoginInfoDto>(await GetImpersonatorUserAsync());
                }

                if (output.Tenant == null)
                {
                    return output;
                }

                if (output.Tenant.Edition != null)
                {
                    var lastPayment =
                        await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(output.Tenant.Id,
                            null, null);
                    if (lastPayment != null)
                    {
                        output.Tenant.Edition.IsHighestEdition = IsEditionHighest(output.Tenant.Edition.Id,
                            lastPayment.GetPaymentPeriodType());
                    }
                }

                output.Tenant.SubscriptionDateString = GetTenantSubscriptionDateString(output);
                output.Tenant.CreationTimeString = output.Tenant.CreationTime.ToString("d");

                return output;
            });
        }

        private async Task<TenantLoginInfoDto> GetTenantLoginInfo(int tenantId)
        {
            var tenant = await TenantManager.Tenants
                .Include(t => t.Edition)
                .FirstAsync(t => t.Id == AbpSession.GetTenantId());

            var tenantLoginInfo = ObjectMapper
                .Map<TenantLoginInfoDto>(tenant);

            if (!tenant.EditionId.HasValue)
            {
                return tenantLoginInfo;
            }

            var features = FeatureManager
                .GetAll()
                .Where(feature => (feature[FeatureMetadata.CustomFeatureKey] as FeatureMetadata)?.IsVisibleOnPricingTable ?? false);

            var featureDictionary = features.ToDictionary(feature => feature.Name, f => f);

            tenantLoginInfo.FeatureValues = (await _editionManager.GetFeatureValuesAsync(tenant.EditionId.Value))
                .Where(featureValue => featureDictionary.ContainsKey(featureValue.Name))
                .Select(fv => new NameValueDto(
                    featureDictionary[fv.Name].DisplayName.Localize(_localizationContext),
                    featureDictionary[fv.Name].GetValueText(fv.Value, _localizationContext))
                )
                .ToList();

            return tenantLoginInfo;
        }

        private bool IsEditionHighest(int editionId, PaymentPeriodType paymentPeriodType)
        {
            var topEdition = GetHighestEditionOrNullByPaymentPeriodType(paymentPeriodType);
            if (topEdition == null)
            {
                return false;
            }

            return editionId == topEdition.Id;
        }

        private SubscribableEdition GetHighestEditionOrNullByPaymentPeriodType(PaymentPeriodType paymentPeriodType)
        {
            var editions = TenantManager.EditionManager.Editions;
            if (editions == null || !editions.Any())
            {
                return null;
            }

            var query = editions.Cast<SubscribableEdition>();

            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Monthly:
                    query = query.OrderByDescending(e => e.MonthlyPrice ?? 0);
                    break;
                case PaymentPeriodType.Annual:
                    query = query.OrderByDescending(e => e.AnnualPrice ?? 0);
                    break;
            }

            return query.FirstOrDefault();
        }

        private string GetTenantSubscriptionDateString(GetCurrentLoginInformationsOutput output)
        {
            return output.Tenant.SubscriptionEndDateUtc == null
                ? L("Unlimited")
                : output.Tenant.SubscriptionEndDateUtc?.ToString("d");
        }

        public async Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken()
        {
            if (AbpSession.UserId <= 0)
            {
                throw new Exception(L("ThereIsNoLoggedInUser"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.SetSignInToken();
            return new UpdateUserSignInTokenOutput
            {
                SignInToken = user.SignInToken,
                EncodedUserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())),
                EncodedTenantId = user.TenantId.HasValue
                    ? Convert.ToBase64String(Encoding.UTF8.GetBytes(user.TenantId.Value.ToString()))
                    : ""
            };
        }

        protected virtual async Task<User> GetImpersonatorUserAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.ImpersonatorTenantId))
            {
                var user = await UserManager.FindByIdAsync(AbpSession.ImpersonatorUserId.ToString());
                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                return user;
            }
        }
    }
}
