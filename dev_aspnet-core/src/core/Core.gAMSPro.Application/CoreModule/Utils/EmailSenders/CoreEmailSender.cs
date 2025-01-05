using Abp.Configuration;
using Abp.Dependency;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.UI;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Core.gAMSPro.Application.CoreModule.Utils.EmailSenders
{
    public class CoreEmailSender : IEmailSender
    {
        public static bool SendEmail(EmailInfo EmailInfo)
        {
            try
            {
                #region comment

                #endregion
                var settingManager = IocManager.Instance.IocContainer.Resolve<ISettingManager>();

                string smtpAddress = settingManager.GetSettingValue(EmailSettingNames.Smtp.Host);

                int portNumber = settingManager.GetSettingValue<int>(EmailSettingNames.Smtp.Port);

                bool enableSSL = settingManager.GetSettingValue<bool>(EmailSettingNames.Smtp.EnableSsl);


                //string emailFrom = "no_reply@gsoft.com.vn";
                //string password = "Ggroup0000))))";
                string emailFrom = settingManager.GetSettingValue(EmailSettingNames.Smtp.UserName);
                string password = settingManager.GetSettingValue(EmailSettingNames.Smtp.Password);
                password = SimpleStringCipher.Instance.Decrypt(password);

                string displayName = settingManager.GetSettingValue(EmailSettingNames.DefaultFromDisplayName);

                string subject = EmailInfo.Subj;
                string body = EmailInfo.Message;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom, displayName);
                    if (!string.IsNullOrEmpty(EmailInfo.ToEmail))
                    {
                        mail.To.Add(EmailInfo.ToEmail);
                    }
                    if (!string.IsNullOrEmpty(EmailInfo.CcEmail))
                    {
                        mail.CC.Add(EmailInfo.CcEmail);
                    }
                    if (!string.IsNullOrEmpty(EmailInfo.BCCEmail))
                    {
                        mail.CC.Add(EmailInfo.BCCEmail);
                    }
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    //media type that is respective of the data attach file
                    int i = 0;
                    if (EmailInfo.isAttach)
                        mail.Attachments.Add(new Attachment(EmailInfo.dataAttach, EmailInfo.nameAttach, "text/plain"));
                    if (EmailInfo.dataMultiAttachs != null)
                    {
                        if (EmailInfo.dataMultiAttachs.isMulti)
                        {
                            foreach (MemoryStream item in EmailInfo.dataMultiAttachs.dataAttachs)
                            {
                                mail.Attachments.Add(new Attachment(item, EmailInfo.dataMultiAttachs.names[i], "text/plain"));
                                i++;
                            }
                        }
                    }
                    new UserFriendlyException("smtpAddress " + smtpAddress);
                    new UserFriendlyException("portNumber " + portNumber.ToString());
                    new UserFriendlyException("emailFrom " + emailFrom);
                    new UserFriendlyException("mail " + mail.ToString());
                    new UserFriendlyException("password " + password);

                    // Can set to false, if you are sending pure text.
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
                return false;
            }
        }
        public void Send(string to, string subject, string body, bool isBodyHtml = true)
        {
            EmailInfo emailInfo = new EmailInfo()
            {
                ToEmail = to,
                Subj = subject,
                Message = body
            };

            SendEmail(emailInfo);
        }

        public void Send(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            EmailInfo emailInfo = new EmailInfo()
            {
                ToEmail = to,
                Subj = subject,
                Message = body
            };

            SendEmail(emailInfo);
        }

        public void Send(MailMessage mail, bool normalize = true)
        {
            foreach (var mailInfo in mail.To)
            {
                var emailInfo = new EmailInfo()
                {
                    ToEmail = mailInfo.Address,
                    Subj = mail.Subject,
                    Message = mail.Body
                };
                SendEmail(emailInfo);
            }
        }

        public async Task SendAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            EmailInfo emailInfo = new EmailInfo()
            {
                ToEmail = to,
                Subj = subject,
                Message = body
            };

            SendEmail(emailInfo);
            await Task.Delay(0);
        }

        public async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            EmailInfo emailInfo = new EmailInfo()
            {
                ToEmail = to,
                Subj = subject,
                Message = body
            };

            SendEmail(emailInfo);
            await Task.Delay(0);
        }

        public async Task SendAsync(MailMessage mail, bool normalize = true)
        {
            foreach (var mailInfo in mail.To)
            {
                var emailInfo = new EmailInfo()
                {
                    ToEmail = mailInfo.Address,
                    Subj = mail.Subject,
                    Message = mail.Body
                };
                SendEmail(emailInfo);
            }

            await Task.Delay(0);
        }
    }
}
