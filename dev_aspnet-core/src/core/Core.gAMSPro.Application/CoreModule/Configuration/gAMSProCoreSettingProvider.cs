using Abp.Configuration;
using Core.gAMSPro.Consts;
using Core.gAMSPro.CoreModule.Consts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Core.gAMSPro.Configuration
{
    public class gAMSProCoreSettingProvider : SettingProvider
    {
        private readonly IConfigurationRoot _appConfiguration;

        public gAMSProCoreSettingProvider(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return GetGAMSProCoreSettings()
                .Union(GetUserLoginSettings())
                .Union(GetLogoCompanySettings())
                .Union(GetCommonThemeSettings())
                .Union(GetSFtpSettings());
        }

        private IEnumerable<SettingDefinition> GetGAMSProCoreSettings()
        {
            return new[] {
                new SettingDefinition(SettingConsts.WebConsts.DefaultRecordsCountPerPage, "10", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.IsResponsive, "True", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.PredefinedRecordsCountPerPage, "[5, 10, 25, 50, 100, 250, 500, 1000]", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.ResizableColumns, "True", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.PhoneNumberRegexValidation, @"^[0-9\-\+]{9,15}$", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.DateTimeFormatClient, @"YYYY-MM-DDTHH:mm:ss+07:00", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.DatePickerDisplayFormat, @"d/m/Y", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.DatePickerValueFormat, @"Y/m/d", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.LanguageComboboxEnable, "True", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.TimeShowSuccessMessage, "3000", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.TimeShowWarningMessage, "3000", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.TimeShowErrorMessage, "0", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.EmailRegexValidation, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.CodeNumberRegexValidation, @"", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.NumberPlateRegexValidation, @"^[a-zA-Z0-9\s-.]{1,}$", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.TaxNoRegexValidation, @"^[0-9a-zA-Z-,( ),(,),-,.]{0,34}$", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.FullNameRegexValidation, @"^[0-9a-zA-Z-,( ),(,),-,.]{0,34}$", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.MaxLenghtRegexValidation, "15", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.SearchMenuVisible, "True", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.MaxQuantityNumber, "1000000", scopes: SettingScopes.Application, isVisibleToClients: true),
                // khai bao them 2 regex kich thuoc file & phan mo rong cua file
                new SettingDefinition(SettingConsts.WebConsts.FileSizeAttach, "2", scopes: SettingScopes.Application, isVisibleToClients: false),
                new SettingDefinition(SettingConsts.WebConsts.FileExtensionAttach, "txt,doc,docx,xls,xlsx,pdf,jpg,png,rar,zip", scopes: SettingScopes.Application, isVisibleToClients: false),
                new SettingDefinition(SettingConsts.WebConsts.DateReportDisplayFormat, "DD/MM/YYYY", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebConsts.CoreNoteRegexValidation, @"^[A-Za-z0-9.\-,\s]{1,}$", scopes: SettingScopes.Application, isVisibleToClients: true),
            };
        }

        private IEnumerable<SettingDefinition> GetUserLoginSettings()
        {
            return new[] {
                new SettingDefinition(SettingConsts.UserLoginConsts.LoginMethod, LoginMethodConsts.Normal, scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.UserLoginConsts.LdapServerName, "ldapserver", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.UserLoginConsts.AdfsWtrealm, "Wtrealm", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.UserLoginConsts.AdfsMetadataAddress, "MetadataAddress", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.UserLoginConsts.EmailActivationEnable, "True", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.UserLoginConsts.FogotPasswordEnable, "True", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.UserLoginConsts.LdapPortNumber, "389", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.UserLoginConsts.LdapDomainName, "GGROUP", scopes: SettingScopes.Application, isVisibleToClients: true)
            };
        }

        private IEnumerable<SettingDefinition> GetSFtpSettings()
        {
            return new[] {
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTPURL, "sftp://192.168.1.230", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTPUserName, "RnVafNyAHQVBtuSfYdDoBBaTym9pJJFg", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTPPassword, "WyLkvlljeOaJsaVJvkAA7w==", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTPTransferMode, "Ascii", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTPPortNumber, "22", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTPSFilePath, "TSdWDovBF48=", scopes: SettingScopes.Application, isVisibleToClients: true),

                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP2URL, "sftp://192.168.1.230", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP2UserName, "RnVafNyAHQVBtuSfYdDoBBaTym9pJJFg", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP2Password, "WyLkvlljeOaJsaVJvkAA7w==", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP2TransferMode, "Ascii", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP2PortNumber, "22", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP2SFilePath, "TSdWDovBF48=", scopes: SettingScopes.Application, isVisibleToClients: true),

                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP3URL, "sftp://192.168.1.230", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP3UserName, "RnVafNyAHQVBtuSfYdDoBBaTym9pJJFg", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP3Password, "WyLkvlljeOaJsaVJvkAA7w==", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP3TransferMode, "Ascii", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP3PortNumber, "22", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.SFTPFileConsts.SFTP3SFilePath, "TSdWDovBF48=", scopes: SettingScopes.Application, isVisibleToClients: true),
            };
        }

        private IEnumerable<SettingDefinition> GetLogoCompanySettings()
        {
            return new[] {
                new SettingDefinition(SettingConsts.WebLogoConsts.WebLogo, "/logo/web_logo.png", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebLogoConsts.LogoLogin, "/logo/web_logo_login.png", scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(SettingConsts.WebLogoConsts.SmallWebLogo, "/logo/small_web_logo.png", scopes: SettingScopes.Application, isVisibleToClients: true),
            };
        }

        private IEnumerable<SettingDefinition> GetCommonThemeSettings()
        {
            return new[] {
                new SettingDefinition(SettingConsts.ThemeConsts.IsApplySetting, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.LogoLoginWidth, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.LogoLoginHeight, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.LogoLoginBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkLogoLoginBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.WebLogoWidth, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.WebLogoHeight, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.WebLogoBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkWebLogoBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SmallWebLogoWidth, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SmallWebLogoHeight, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SmallWebLogoBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkSmallWebLogoBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.PrimaryColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkPrimaryColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DangerColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkDangerColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.WarningColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkWarningColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SuccessColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkSuccessColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.BorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SideBarChildBgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TextColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkBorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkSideBarChildBgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTextColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.CardBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkCardBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.WebBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkWebBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.HeaderBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.HeaderColorText, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkHeaderBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkHeaderColorText, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SidebarBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SidebarColorText, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SidebarColorTextActive, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkSidebarBackground, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkSidebarColorText, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkSidebarColorTextActive, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.HeaderBorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.HeaderLogoBgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.HeaderLogoBorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.HeaderWrapperBgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.HeaderWrapperBorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkHeaderBorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkHeaderLogoBgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkHeaderLogoBorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkHeaderWrapperBgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkHeaderWrapperBorderColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.SvgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkSvgColor, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.Theme, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.Scrollbar, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkScrollbar, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.ScrollbarBg, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkscrollbarBg, null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TableHeadBgColor          ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TableBodyOddBgColor       ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TableBodyEvenBgColor      ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TableMainColor            ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TableBodyTextColor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TableBorderColor          ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.InputLabelColor           ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.InputBorderColor          ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.InputTextColor            ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.InputIconColor            ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTableHeadBgColor      ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTableBodyOddBgColor   ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTableBodyEvenBgColor  ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTableMainColor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTableBodyTextColor    ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTableBorderColor      ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkInputLabelColor       ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkInputBorderColor      ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkInputTextColor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkInputIconColor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.TableHighlightColor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkTableHighlightColor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.InputBgcolor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkInputBgcolor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.BtnToolbarBgColor          ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.BtnToolbarTextColor        ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.BtnToolbarBorderColor      ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkBtnToolbarBgColor      ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkBtnToolbarTextColor    ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
                new SettingDefinition(SettingConsts.ThemeConsts.DarkBtnToolbarBorderColor  ,null, scopes: SettingScopes.Application,  isVisibleToClients:true),
            };
        }
    }
}
