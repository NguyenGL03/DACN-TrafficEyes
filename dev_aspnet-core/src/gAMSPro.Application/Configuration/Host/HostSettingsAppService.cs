using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Json;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using Core.gAMSPro.Consts;
using gAMSPro.Authentication;
using gAMSPro.Authorization;
using gAMSPro.Configuration.Dto;
using gAMSPro.Configuration.Host.Dto;
using gAMSPro.Editions;
using gAMSPro.Security;
using gAMSPro.Timing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace gAMSPro.Configuration.Host
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Settings)]
    public class HostSettingsAppService : SettingsAppServiceBase, IHostSettingsAppService
    {
        public IExternalLoginOptionsCacheManager ExternalLoginOptionsCacheManager { get; set; }

        private readonly EditionManager _editionManager;
        private readonly ITimeZoneService _timeZoneService;
        readonly ISettingDefinitionManager _settingDefinitionManager;

        public HostSettingsAppService(
            IEmailSender emailSender,
            EditionManager editionManager,
            ITimeZoneService timeZoneService,
            ISettingDefinitionManager settingDefinitionManager,
            IAppConfigurationAccessor configurationAccessor) : base(emailSender, configurationAccessor)
        {
            ExternalLoginOptionsCacheManager = NullExternalLoginOptionsCacheManager.Instance;

            _editionManager = editionManager;
            _timeZoneService = timeZoneService;
            _settingDefinitionManager = settingDefinitionManager;
        }

        #region Get Settings

        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            return new HostSettingsEditDto
            {
                CommonSettingEditDto = await GetCommonSettingEditAsync(),
                CommonThemeSettings = await GetCommonThemeSettingEditAsync(),
                General = await GetGeneralSettingsAsync(),
                TenantManagement = await GetTenantManagementSettingsAsync(),
                UserManagement = await GetUserManagementAsync(),
                Email = await GetEmailSettingsAsync(),
                Security = await GetSecuritySettingsAsync(),
                Billing = await GetBillingSettingsAsync(),
                LogoCompanySettingDto = await GetLogoCompanySettingEditAsync(),
                OtherSettings = await GetOtherSettingsAsync(),
                ExternalLoginProviderSettings = await GetExternalLoginProviderSettings()
            };
        }

        private async Task<CommonSettingEditDto> GetCommonSettingEditAsync()
        {
            return new CommonSettingEditDto
            {
                DefaultRecordsCountPerPage = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.DefaultRecordsCountPerPage),
                LanguageComboboxEnable = await SettingManager.GetSettingValueAsync<bool>(SettingConsts.WebConsts.LanguageComboboxEnable),
                PredefinedRecordsCountPerPage = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.PredefinedRecordsCountPerPage),
                PhoneNumberRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.PhoneNumberRegexValidation),
                DateTimeFormatClient = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.DateTimeFormatClient),
                DatePickerDisplayFormat = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.DatePickerDisplayFormat),
                DatePickerValueFormat = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.DatePickerValueFormat),
                EmailRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.EmailRegexValidation),
                CoreNoteRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.CoreNoteRegexValidation),
                CodeNumberRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.CodeNumberRegexValidation),
                NumberPlateRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.NumberPlateRegexValidation),
                MaxQuantityNumber = await SettingManager.GetSettingValueAsync<int>(SettingConsts.WebConsts.MaxQuantityNumber),
                TaxNoRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.TaxNoRegexValidation),
                FullNameRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.FullNameRegexValidation),
                MaxLenghtRegexValidation = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.MaxLenghtRegexValidation),
                TimeShowSuccessMessage = await SettingManager.GetSettingValueAsync<int>(SettingConsts.WebConsts.TimeShowSuccessMessage),
                TimeShowWarningMessage = await SettingManager.GetSettingValueAsync<int>(SettingConsts.WebConsts.TimeShowWarningMessage),
                TimeShowErrorMessage = await SettingManager.GetSettingValueAsync<int>(SettingConsts.WebConsts.TimeShowErrorMessage),
                FileSizeAttach = await SettingManager.GetSettingValueAsync<int>(SettingConsts.WebConsts.FileSizeAttach),
                FileExtensionAttach = await SettingManager.GetSettingValueAsync(SettingConsts.WebConsts.FileExtensionAttach)
            };
        }

        private async Task<GeneralSettingsEditDto> GetGeneralSettingsAsync()
        {
            var timezone = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            var settings = new GeneralSettingsEditDto
            {
                Timezone = timezone,
                TimezoneForComparison = timezone
            };

            var defaultTimeZoneId =
                await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        private async Task<TenantManagementSettingsEditDto> GetTenantManagementSettingsAsync()
        {
            var settings = new TenantManagementSettingsEditDto
            {
                AllowSelfRegistration =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.AllowSelfRegistration),
                IsNewRegisteredTenantActiveByDefault =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement
                        .IsNewRegisteredTenantActiveByDefault),
                UseCaptchaOnRegistration =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement
                        .UseCaptchaOnRegistration),
            };

            var defaultEditionId =
                await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.DefaultEdition);
            if (!string.IsNullOrEmpty(defaultEditionId) &&
                (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionId)) != null))
            {
                settings.DefaultEditionId = Convert.ToInt32(defaultEditionId);
            }

            return settings;
        }

        private async Task<HostUserManagementSettingsEditDto> GetUserManagementAsync()
        {
            return new HostUserManagementSettingsEditDto
            {
                IsEmailConfirmationRequiredForLogin =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .IsEmailConfirmationRequiredForLogin),
                SmsVerificationEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SmsVerificationEnabled),
                IsCookieConsentEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsCookieConsentEnabled),
                IsQuickThemeSelectEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(
                        AppSettings.UserManagement.IsQuickThemeSelectEnabled),
                UseCaptchaOnLogin =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.UseCaptchaOnLogin),
                AllowUsingGravatarProfilePicture =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement
                        .AllowUsingGravatarProfilePicture),
                SessionTimeOutSettings = new SessionTimeOutSettingsEditDto
                {
                    IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement
                        .SessionTimeOut.IsEnabled),
                    TimeOutSecond =
                        await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.SessionTimeOut
                            .TimeOutSecond),
                    ShowTimeOutNotificationSecond =
                        await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.SessionTimeOut
                            .ShowTimeOutNotificationSecond),
                    ShowLockScreenWhenTimedOut =
                        await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SessionTimeOut
                            .ShowLockScreenWhenTimedOut)
                },
                UserPasswordSettings = new UserPasswordSettingsEditDto()
                {
                    EnableCheckingLastXPasswordWhenPasswordChange = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.Password.EnableCheckingLastXPasswordWhenPasswordChange),
                    CheckingLastXPasswordCount = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.Password.CheckingLastXPasswordCount),
                    EnablePasswordExpiration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.Password.EnablePasswordExpiration),
                    PasswordExpirationDayCount = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.Password.PasswordExpirationDayCount),
                    PasswordResetCodeExpirationHours = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.Password.PasswordResetCodeExpirationHours),
                }
            };
        }

        private async Task<EmailSettingsEditDto> GetEmailSettingsAsync()
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsEditDto
            {
                DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName =
                    await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials =
                    await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }

        private async Task<SecuritySettingsEditDto> GetSecuritySettingsAsync()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireDigit),
                RequireLowercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireUppercase),
                RequiredLength =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity
                        .RequiredLength)
            };

            var defaultPasswordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit)
                    .DefaultValue),
                RequireLowercase = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase)
                    .DefaultValue),
                RequireNonAlphanumeric = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric)
                    .DefaultValue),
                RequireUppercase = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase)
                    .DefaultValue),
                RequiredLength = Convert.ToInt32(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
                    .DefaultValue)
            };

            return new SecuritySettingsEditDto
            {
                UseDefaultPasswordComplexitySettings =
                    passwordComplexitySetting.Equals(defaultPasswordComplexitySetting),
                PasswordComplexity = passwordComplexitySetting,
                DefaultPasswordComplexity = defaultPasswordComplexitySetting,
                UserLockOut = await GetUserLockOutSettingsAsync(),
                TwoFactorLogin = await GetTwoFactorLoginSettingsAsync(),
                AllowOneConcurrentLoginPerUser = await GetOneConcurrentLoginPerUserSetting()
            };
        }

        private async Task<HostBillingSettingsEditDto> GetBillingSettingsAsync()
        {
            return new HostBillingSettingsEditDto
            {
                LegalName = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingLegalName),
                Address = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingAddress)
            };
        }

        private async Task<OtherSettingsEditDto> GetOtherSettingsAsync()
        {
            return new OtherSettingsEditDto()
            {
                IsQuickThemeSelectEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(
                        AppSettings.UserManagement.IsQuickThemeSelectEnabled)
            };
        }

        private async Task<UserLockOutSettingsEditDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut
                        .MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut
                        .DefaultAccountLockoutSeconds)
            };
        }

        private async Task<TwoFactorLoginSettingsEditDto> GetTwoFactorLoginSettingsAsync()
        {
            var twoFactorLoginSettingsEditDto = new TwoFactorLoginSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .TwoFactorLogin.IsEnabled),
                IsEmailProviderEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                        .IsEmailProviderEnabled),
                IsSmsProviderEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                        .IsSmsProviderEnabled),
                IsRememberBrowserEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                        .IsRememberBrowserEnabled),
                IsGoogleAuthenticatorEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.TwoFactorLogin
                        .IsGoogleAuthenticatorEnabled)
            };
            return twoFactorLoginSettingsEditDto;
        }

        private async Task<bool> GetOneConcurrentLoginPerUserSetting()
        {
            return await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement
                .AllowOneConcurrentLoginPerUser);
        }

        private async Task<ExternalLoginProviderSettingsEditDto> GetExternalLoginProviderSettings()
        {
            var facebookSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host
                    .Facebook);
            var googleSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host.Google);
            var twitterSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host.Twitter);
            var microsoftSettings =
                await SettingManager.GetSettingValueForApplicationAsync(
                    AppSettings.ExternalLoginProvider.Host.Microsoft);

            var openIdConnectSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host
                    .OpenIdConnect);
            var openIdConnectMapperClaims =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider
                    .OpenIdConnectMappedClaims);

            var wsFederationSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host
                    .WsFederation);
            var wsFederationMapperClaims =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider
                    .WsFederationMappedClaims);

            return new ExternalLoginProviderSettingsEditDto
            {
                Facebook = facebookSettings.IsNullOrWhiteSpace()
                    ? new FacebookExternalLoginProviderSettings()
                    : facebookSettings.FromJsonString<FacebookExternalLoginProviderSettings>(),
                Google = googleSettings.IsNullOrWhiteSpace()
                    ? new GoogleExternalLoginProviderSettings()
                    : googleSettings.FromJsonString<GoogleExternalLoginProviderSettings>(),
                Twitter = twitterSettings.IsNullOrWhiteSpace()
                    ? new TwitterExternalLoginProviderSettings()
                    : twitterSettings.FromJsonString<TwitterExternalLoginProviderSettings>(),
                Microsoft = microsoftSettings.IsNullOrWhiteSpace()
                    ? new MicrosoftExternalLoginProviderSettings()
                    : microsoftSettings.FromJsonString<MicrosoftExternalLoginProviderSettings>(),

                OpenIdConnect = openIdConnectSettings.IsNullOrWhiteSpace()
                    ? new OpenIdConnectExternalLoginProviderSettings()
                    : openIdConnectSettings.FromJsonString<OpenIdConnectExternalLoginProviderSettings>(),
                OpenIdConnectClaimsMapping = openIdConnectMapperClaims.IsNullOrWhiteSpace()
                    ? new List<JsonClaimMapDto>()
                    : openIdConnectMapperClaims.FromJsonString<List<JsonClaimMapDto>>(),

                WsFederation = wsFederationSettings.IsNullOrWhiteSpace()
                    ? new WsFederationExternalLoginProviderSettings()
                    : wsFederationSettings.FromJsonString<WsFederationExternalLoginProviderSettings>(),
                WsFederationClaimsMapping = wsFederationMapperClaims.IsNullOrWhiteSpace()
                    ? new List<JsonClaimMapDto>()
                    : wsFederationMapperClaims.FromJsonString<List<JsonClaimMapDto>>()
            };
        }

        private async Task<LogoCompanySettingDto> GetLogoCompanySettingEditAsync()
        {
            return new LogoCompanySettingDto
            {
                LoginLogo = await SettingManager.GetSettingValueAsync(SettingConsts.WebLogoConsts.LogoLogin),
                WebLogo = await SettingManager.GetSettingValueAsync(SettingConsts.WebLogoConsts.WebLogo),
                SmallWebLogo = await SettingManager.GetSettingValueAsync(SettingConsts.WebLogoConsts.SmallWebLogo),
            };
        }

        private async Task<CommonThemeSettingsDto> GetCommonThemeSettingEditAsync()
        {
            return new CommonThemeSettingsDto
            {
                IsApplySetting = await SettingManager.GetSettingValueAsync<bool>(SettingConsts.ThemeConsts.IsApplySetting),
                LogoLoginWidth = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.LogoLoginWidth),
                LogoLoginHeight = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.LogoLoginHeight),
                LogoLoginBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.LogoLoginBackground),
                DarkLogoLoginBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkLogoLoginBackground),
                WebLogoWidth = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.WebLogoWidth),
                WebLogoHeight = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.WebLogoHeight),
                WebLogoBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.WebLogoBackground),
                DarkWebLogoBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkWebLogoBackground),
                SmallWebLogoWidth = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SmallWebLogoWidth),
                SmallWebLogoHeight = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SmallWebLogoHeight),
                SmallWebLogoBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SmallWebLogoBackground),
                DarkSmallWebLogoBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkSmallWebLogoBackground),
                PrimaryColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.PrimaryColor),
                DarkPrimaryColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkPrimaryColor),
                DangerColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DangerColor),
                DarkDangerColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkDangerColor),
                WarningColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.WarningColor),
                DarkWarningColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkWarningColor),
                SuccessColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SuccessColor),
                DarkSuccessColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkSuccessColor),
                CardBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.CardBackground),
                DarkCardBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkCardBackground),
                WebBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.WebBackground),
                DarkWebBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkWebBackground),
                HeaderBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.HeaderBackground),
                HeaderColorText = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.HeaderColorText),
                DarkHeaderBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkHeaderBackground),
                DarkHeaderColorText = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkHeaderColorText),
                SidebarBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SidebarBackground),
                SidebarColorText = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SidebarColorText),
                SidebarColorTextActive = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SidebarColorTextActive),
                DarkSidebarBackground = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkSidebarBackground),
                DarkSidebarColorText = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkSidebarColorText),
                DarkSidebarColorTextActive = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkSidebarColorTextActive),
                BorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.BorderColor),
                SideBarChildBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SideBarChildBgColor),
                TextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TextColor),
                DarkBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkBorderColor),
                DarkSideBarChildBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkSideBarChildBgColor),
                DarkTextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTextColor),

                HeaderBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.HeaderBorderColor),
                HeaderLogoBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.HeaderLogoBgColor),
                HeaderLogoBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.HeaderLogoBorderColor),
                HeaderWrapperBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.HeaderWrapperBgColor),
                HeaderWrapperBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.HeaderWrapperBorderColor),
                DarkHeaderBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkHeaderBorderColor),
                DarkHeaderLogoBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkHeaderLogoBgColor),
                DarkHeaderLogoBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkHeaderLogoBorderColor),
                DarkHeaderWrapperBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkHeaderWrapperBgColor),
                DarkHeaderWrapperBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkHeaderWrapperBorderColor),
                Theme = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.Theme),
                Scrollbar = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.Scrollbar),
                DarkScrollbar = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkScrollbar),
                ScrollbarBg = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.ScrollbarBg),
                DarkscrollbarBg = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkscrollbarBg),
                SvgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.SvgColor),
                DarkSvgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkSvgColor),
                TableHeadBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TableHeadBgColor),
                TableBodyOddBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TableBodyOddBgColor),
                TableBodyEvenBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TableBodyEvenBgColor),
                TableMainColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TableMainColor),
                TableBodyTextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TableBodyTextColor),
                TableBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TableBorderColor),
                InputLabelColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.InputLabelColor),
                InputBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.InputBorderColor),
                InputTextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.InputTextColor),
                InputIconColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.InputIconColor),
                DarkTableHeadBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTableHeadBgColor),
                DarkTableBodyOddBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTableBodyOddBgColor),
                DarkTableBodyEvenBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTableBodyEvenBgColor),
                DarkTableMainColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTableMainColor),
                DarkTableBodyTextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTableBodyTextColor),
                DarkTableBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTableBorderColor),
                DarkInputLabelColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkInputLabelColor),
                DarkInputBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkInputBorderColor),
                DarkInputTextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkInputTextColor),
                DarkInputIconColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkInputIconColor),
                TableHighlightColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.TableHighlightColor),
                DarkTableHighlightColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkTableHighlightColor),
                InputBgcolor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.InputBgcolor),
                DarkInputBgcolor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkInputBgcolor),
                BtnToolbarBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.BtnToolbarBgColor),
                BtnToolbarTextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.BtnToolbarTextColor),
                BtnToolbarBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.BtnToolbarBorderColor),
                DarkBtnToolbarBgColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkBtnToolbarBgColor),
                DarkBtnToolbarTextColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkBtnToolbarTextColor),
                DarkBtnToolbarBorderColor = await SettingManager.GetSettingValueAsync(SettingConsts.ThemeConsts.DarkBtnToolbarBorderColor),
            };
        }

        #endregion

        #region Update Settings

        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await UpdateGeneralSettingsAsync(input.General);
            await UpdateTenantManagementAsync(input.TenantManagement);
            await UpdateUserManagementSettingsAsync(input.UserManagement);
            await UpdateSecuritySettingsAsync(input.Security);
            await UpdateEmailSettingsAsync(input.Email);
            await UpdateBillingSettingsAsync(input.Billing);
            await UpdateOtherSettingsAsync(input.OtherSettings);
            await UpdateExternalLoginSettingsAsync(input.ExternalLoginProviderSettings);
            await UpdateCommonSettingEdit(input.CommonSettingEditDto);
            await UpdateCommonThemeSettingEdit(input.CommonThemeSettings);
        }

        private async Task UpdateOtherSettingsAsync(OtherSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.IsQuickThemeSelectEnabled,
                input.IsQuickThemeSelectEnabled.ToString().ToLowerInvariant()
            );
        }

        private async Task UpdateBillingSettingsAsync(HostBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingLegalName,
                input.LegalName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingAddress,
                input.Address);
        }

        private async Task UpdateGeneralSettingsAsync(GeneralSettingsEditDto settings)
        {
            if (Clock.SupportsMultipleTimezone)
            {
                if (settings.Timezone.IsNullOrEmpty())
                {
                    var defaultValue =
                        await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone,
                        settings.Timezone);
                }
            }
        }

        private async Task UpdateTenantManagementAsync(TenantManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.AllowSelfRegistration,
                settings.AllowSelfRegistration.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault,
                settings.IsNewRegisteredTenantActiveByDefault.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.UseCaptchaOnRegistration,
                settings.UseCaptchaOnRegistration.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.DefaultEdition,
                settings.DefaultEditionId?.ToString() ?? ""
            );
        }

        private async Task UpdateUserManagementSettingsAsync(HostUserManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SmsVerificationEnabled,
                settings.SmsVerificationEnabled.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.IsCookieConsentEnabled,
                settings.IsCookieConsentEnabled.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.UseCaptchaOnLogin,
                settings.UseCaptchaOnLogin.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.AllowUsingGravatarProfilePicture,
                settings.AllowUsingGravatarProfilePicture.ToString().ToLowerInvariant()
            );

            await UpdateUserManagementSessionTimeOutSettingsAsync(settings.SessionTimeOutSettings);
            await UpdateUserManagementPasswordSettingsAsync(settings.UserPasswordSettings);
        }

        private async Task UpdateUserManagementPasswordSettingsAsync(UserPasswordSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.EnableCheckingLastXPasswordWhenPasswordChange,
                settings.EnableCheckingLastXPasswordWhenPasswordChange.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.CheckingLastXPasswordCount,
                settings.CheckingLastXPasswordCount.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.EnablePasswordExpiration,
                settings.EnablePasswordExpiration.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.PasswordExpirationDayCount,
                settings.PasswordExpirationDayCount.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.PasswordResetCodeExpirationHours,
                settings.PasswordResetCodeExpirationHours.ToString()
            );
        }

        private async Task UpdateUserManagementSessionTimeOutSettingsAsync(SessionTimeOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.IsEnabled,
                settings.IsEnabled.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.TimeOutSecond,
                settings.TimeOutSecond.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.ShowTimeOutNotificationSecond,
                settings.ShowTimeOutNotificationSecond.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.ShowLockScreenWhenTimedOut,
                settings.ShowLockScreenWhenTimedOut.ToString()
            );
        }

        private async Task UpdateSecuritySettingsAsync(SecuritySettingsEditDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await UpdatePasswordComplexitySettingsAsync(settings.DefaultPasswordComplexity);
            }
            else
            {
                if (settings.PasswordComplexity.RequiredLength < settings.PasswordComplexity.AllowedMinimumLength)
                {
                    throw new UserFriendlyException(L("AllowedMinimumLength", settings.PasswordComplexity.AllowedMinimumLength));
                }

                await UpdatePasswordComplexitySettingsAsync(settings.PasswordComplexity);
            }

            await UpdateUserLockOutSettingsAsync(settings.UserLockOut);
            await UpdateTwoFactorLoginSettingsAsync(settings.TwoFactorLogin);
            await UpdateOneConcurrentLoginPerUserSettingAsync(settings.AllowOneConcurrentLoginPerUser);
        }

        private async Task UpdatePasswordComplexitySettingsAsync(PasswordComplexitySetting settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit,
                settings.RequireDigit.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase,
                settings.RequireLowercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric,
                settings.RequireNonAlphanumeric.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase,
                settings.RequireUppercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength,
                settings.RequiredLength.ToString()
            );
        }

        private async Task UpdateUserLockOutSettingsAsync(UserLockOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled,
                settings.IsEnabled.ToString().ToLowerInvariant()
            );

            if (settings.DefaultAccountLockoutSeconds.HasValue)
            {
                await SettingManager.ChangeSettingForApplicationAsync(
                    AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds,
                    settings.DefaultAccountLockoutSeconds.ToString()
                );
            }

            if (settings.MaxFailedAccessAttemptsBeforeLockout.HasValue)
            {
                await SettingManager.ChangeSettingForApplicationAsync(
                    AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout,
                    settings.MaxFailedAccessAttemptsBeforeLockout.ToString()
                );
            }
        }

        private async Task UpdateTwoFactorLoginSettingsAsync(TwoFactorLoginSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled,
                settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled,
                settings.IsEmailProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled,
                settings.IsSmsProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled,
                settings.IsGoogleAuthenticatorEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled,
                settings.IsRememberBrowserEnabled.ToString().ToLowerInvariant());
        }

        private async Task UpdateEmailSettingsAsync(EmailSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress,
                settings.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName,
                settings.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, settings.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port,
                settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName,
                settings.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password,
                SimpleStringCipher.Instance.Encrypt(settings.SmtpPassword));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl,
                settings.SmtpEnableSsl.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials,
                settings.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        }

        private async Task UpdateOneConcurrentLoginPerUserSettingAsync(bool allowOneConcurrentLoginPerUser)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.AllowOneConcurrentLoginPerUser, allowOneConcurrentLoginPerUser.ToString());
        }

        private async Task UpdateExternalLoginSettingsAsync(ExternalLoginProviderSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Facebook,
                input.Facebook == null || !input.Facebook.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Facebook)
                        .DefaultValue
                    : input.Facebook.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Google,
                input.Google == null || !input.Google.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Google)
                        .DefaultValue
                    : input.Google.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Twitter,
                input.Twitter == null || !input.Twitter.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Twitter)
                        .DefaultValue
                    : input.Twitter.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Microsoft,
                input.Microsoft == null || !input.Microsoft.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Microsoft)
                        .DefaultValue
                    : input.Microsoft.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.OpenIdConnect,
                input.OpenIdConnect == null || !input.OpenIdConnect.IsValid()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.OpenIdConnect).DefaultValue
                    : input.OpenIdConnect.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.OpenIdConnectMappedClaims,
                input.OpenIdConnectClaimsMapping.IsNullOrEmpty()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.OpenIdConnectMappedClaims).DefaultValue
                    : input.OpenIdConnectClaimsMapping.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.WsFederation,
                input.WsFederation == null || !input.WsFederation.IsValid()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.WsFederation).DefaultValue
                    : input.WsFederation.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.WsFederationMappedClaims,
                input.WsFederationClaimsMapping.IsNullOrEmpty()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.WsFederationMappedClaims).DefaultValue
                    : input.WsFederationClaimsMapping.ToJsonString()
            );

            ExternalLoginOptionsCacheManager.ClearCache();
        }

        private async Task UpdateCommonSettingEdit(CommonSettingEditDto input)
        {
            if (!input.DefaultRecordsCountPerPage.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.DefaultRecordsCountPerPage, input.DefaultRecordsCountPerPage);
            }
            if (!input.PredefinedRecordsCountPerPage.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.PredefinedRecordsCountPerPage, input.PredefinedRecordsCountPerPage);
            }
            if (!input.PhoneNumberRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.PhoneNumberRegexValidation, input.PhoneNumberRegexValidation);
            }
            if (!input.DateTimeFormatClient.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.DateTimeFormatClient, input.DateTimeFormatClient);
            }
            if (!input.DatePickerDisplayFormat.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.DatePickerDisplayFormat, input.DatePickerDisplayFormat);
            }
            if (!input.DatePickerValueFormat.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.DatePickerValueFormat, input.DatePickerValueFormat);
            }
            // Bo sung Regex Email & Regex ma code & Taxno & FullName
            if (!input.EmailRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.EmailRegexValidation, input.EmailRegexValidation);
            }
            // Bo sung CoreNoteRegexValidation Dien giai hach toan
            if (!input.CoreNoteRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.CoreNoteRegexValidation, input.CoreNoteRegexValidation);
            }
            // Ma code
            if (!input.CodeNumberRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.CodeNumberRegexValidation, input.CodeNumberRegexValidation);
            }
            // So xe
            if (!input.NumberPlateRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.NumberPlateRegexValidation, input.NumberPlateRegexValidation);
            }
            // So luong toi da
            if (input.MaxQuantityNumber != null)
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.MaxQuantityNumber, input.MaxQuantityNumber.Value.ToString());
            }
            // Ma so thue
            if (!input.TaxNoRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.TaxNoRegexValidation, input.TaxNoRegexValidation);
            }
            // Full Name
            if (!input.FullNameRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.FullNameRegexValidation, input.FullNameRegexValidation);
            }
            // Chieu dai ma code
            if (!input.MaxLenghtRegexValidation.IsNullOrWhiteSpace())
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.MaxLenghtRegexValidation, input.MaxLenghtRegexValidation);
            }
            if (input.LanguageComboboxEnable != null)
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.LanguageComboboxEnable, input.LanguageComboboxEnable.Value.ToString());
            }
            if (input.TimeShowSuccessMessage != null)
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.TimeShowSuccessMessage, input.TimeShowSuccessMessage.Value.ToString());
            }
            if (input.TimeShowWarningMessage != null)
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.TimeShowWarningMessage, input.TimeShowWarningMessage.Value.ToString());
            }
            if (input.TimeShowErrorMessage != null)
            {
                await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.WebConsts.TimeShowErrorMessage, input.TimeShowErrorMessage.Value.ToString());
            }
        }

        private async Task UpdateCommonThemeSettingEdit(CommonThemeSettingsDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.IsApplySetting, Convert.ToString(input.IsApplySetting));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.LogoLoginWidth, Convert.ToString(input.LogoLoginWidth));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.LogoLoginHeight, Convert.ToString(input.LogoLoginHeight));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.LogoLoginBackground, Convert.ToString(input.LogoLoginBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkLogoLoginBackground, Convert.ToString(input.DarkLogoLoginBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.WebLogoWidth, Convert.ToString(input.WebLogoWidth));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.WebLogoHeight, Convert.ToString(input.WebLogoHeight));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.WebLogoBackground, Convert.ToString(input.WebLogoBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkWebLogoBackground, Convert.ToString(input.DarkWebLogoBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SmallWebLogoWidth, Convert.ToString(input.SmallWebLogoWidth));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SmallWebLogoHeight, Convert.ToString(input.SmallWebLogoHeight));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SmallWebLogoBackground, Convert.ToString(input.SmallWebLogoBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkSmallWebLogoBackground, Convert.ToString(input.DarkSmallWebLogoBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.PrimaryColor, Convert.ToString(input.PrimaryColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkPrimaryColor, Convert.ToString(input.DarkPrimaryColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DangerColor, Convert.ToString(input.DangerColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkDangerColor, Convert.ToString(input.DarkDangerColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.WarningColor, Convert.ToString(input.WarningColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkWarningColor, Convert.ToString(input.DarkWarningColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SuccessColor, Convert.ToString(input.SuccessColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkSuccessColor, Convert.ToString(input.DarkSuccessColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.CardBackground, Convert.ToString(input.CardBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkCardBackground, Convert.ToString(input.DarkCardBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.WebBackground, Convert.ToString(input.WebBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkWebBackground, Convert.ToString(input.DarkWebBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.HeaderBackground, Convert.ToString(input.HeaderBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.HeaderColorText, Convert.ToString(input.HeaderColorText));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkHeaderBackground, Convert.ToString(input.DarkHeaderBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkHeaderColorText, Convert.ToString(input.DarkHeaderColorText));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SidebarBackground, Convert.ToString(input.SidebarBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SidebarColorText, Convert.ToString(input.SidebarColorText));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SidebarColorTextActive, Convert.ToString(input.SidebarColorTextActive));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkSidebarBackground, Convert.ToString(input.DarkSidebarBackground));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkSidebarColorText, Convert.ToString(input.DarkSidebarColorText));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkSidebarColorTextActive, Convert.ToString(input.DarkSidebarColorTextActive));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.BorderColor, Convert.ToString(input.BorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SideBarChildBgColor, Convert.ToString(input.SideBarChildBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TextColor, Convert.ToString(input.TextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkBorderColor, Convert.ToString(input.DarkBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkSideBarChildBgColor, Convert.ToString(input.DarkSideBarChildBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTextColor, Convert.ToString(input.DarkTextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.HeaderBorderColor, Convert.ToString(input.HeaderBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.HeaderLogoBgColor, Convert.ToString(input.HeaderLogoBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.HeaderLogoBorderColor, Convert.ToString(input.HeaderLogoBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.HeaderWrapperBgColor, Convert.ToString(input.HeaderWrapperBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.HeaderWrapperBorderColor, Convert.ToString(input.HeaderWrapperBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkHeaderBorderColor, Convert.ToString(input.DarkHeaderBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkHeaderLogoBgColor, Convert.ToString(input.DarkHeaderLogoBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkHeaderLogoBorderColor, Convert.ToString(input.DarkHeaderLogoBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkHeaderWrapperBgColor, Convert.ToString(input.DarkHeaderWrapperBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkHeaderWrapperBorderColor, Convert.ToString(input.DarkHeaderWrapperBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.Theme, Convert.ToString(input.Theme));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.Scrollbar, Convert.ToString(input.Scrollbar));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkScrollbar, Convert.ToString(input.DarkScrollbar));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.ScrollbarBg, Convert.ToString(input.ScrollbarBg));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkscrollbarBg, Convert.ToString(input.DarkscrollbarBg));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.SvgColor, Convert.ToString(input.SvgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkSvgColor, Convert.ToString(input.DarkSvgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TableHeadBgColor, Convert.ToString(input.TableHeadBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TableBodyOddBgColor, Convert.ToString(input.TableBodyOddBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TableBodyEvenBgColor, Convert.ToString(input.TableBodyEvenBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TableMainColor, Convert.ToString(input.TableMainColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TableBodyTextColor, Convert.ToString(input.TableBodyTextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TableBorderColor, Convert.ToString(input.TableBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.InputLabelColor, Convert.ToString(input.InputLabelColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.InputBorderColor, Convert.ToString(input.InputBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.InputTextColor, Convert.ToString(input.InputTextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.InputIconColor, Convert.ToString(input.InputIconColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTableHeadBgColor, Convert.ToString(input.DarkTableHeadBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTableBodyOddBgColor, Convert.ToString(input.DarkTableBodyOddBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTableBodyEvenBgColor, Convert.ToString(input.DarkTableBodyEvenBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTableMainColor, Convert.ToString(input.DarkTableMainColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTableBodyTextColor, Convert.ToString(input.DarkTableBodyTextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTableBorderColor, Convert.ToString(input.DarkTableBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkInputLabelColor, Convert.ToString(input.DarkInputLabelColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkInputBorderColor, Convert.ToString(input.DarkInputBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkInputTextColor, Convert.ToString(input.DarkInputTextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkInputIconColor, Convert.ToString(input.DarkInputIconColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.TableHighlightColor, Convert.ToString(input.TableHighlightColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkTableHighlightColor, Convert.ToString(input.DarkTableHighlightColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.InputBgcolor, Convert.ToString(input.InputBgcolor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkInputBgcolor, Convert.ToString(input.DarkInputBgcolor));

            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.BtnToolbarBgColor, Convert.ToString(input.BtnToolbarBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.BtnToolbarTextColor, Convert.ToString(input.BtnToolbarTextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.BtnToolbarBorderColor, Convert.ToString(input.BtnToolbarBorderColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkBtnToolbarBgColor, Convert.ToString(input.DarkBtnToolbarBgColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkBtnToolbarTextColor, Convert.ToString(input.DarkBtnToolbarTextColor));
            await SettingManager.ChangeSettingForApplicationAsync(SettingConsts.ThemeConsts.DarkBtnToolbarBorderColor, Convert.ToString(input.DarkBtnToolbarBorderColor));
        }
        #endregion
    }
}
