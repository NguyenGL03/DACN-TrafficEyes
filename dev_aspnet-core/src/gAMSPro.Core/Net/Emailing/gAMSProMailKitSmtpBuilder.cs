using Abp.Localization;
using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using Abp.UI;
using Castle.Components.DictionaryAdapter.Xml;
using MailKit.Net.Smtp;

namespace gAMSPro.Net.Emailing
{
    public class gAMSProMailKitSmtpBuilder : DefaultMailKitSmtpBuilder
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly IEmailSettingsChecker _emailSettingsChecker;
        
        public gAMSProMailKitSmtpBuilder(
            ISmtpEmailSenderConfiguration smtpEmailSenderConfiguration,
            IAbpMailKitConfiguration abpMailKitConfiguration,
            ILocalizationManager localizationManager, 
            IEmailSettingsChecker emailSettingsChecker) : base(smtpEmailSenderConfiguration, abpMailKitConfiguration)
        {
            _localizationManager = localizationManager;
            _emailSettingsChecker = emailSettingsChecker;
        }

        protected override void ConfigureClient(SmtpClient client)
        {
            if (!_emailSettingsChecker.EmailSettingsValid())
            {
                throw new UserFriendlyException(L("SMTPSettingsNotProvidedWarningText"));
            }
            
            client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            base.ConfigureClient(client);
        }

        private string L(string name)
        {
            return _localizationManager.GetString(gAMSProConsts.LocalizationSourceName, name);
        }
    }
}