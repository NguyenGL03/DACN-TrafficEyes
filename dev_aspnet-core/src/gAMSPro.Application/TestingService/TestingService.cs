using Abp.Authorization;
using Abp.Net.Mail.Smtp;
using Core.gAMSPro.Application;
using gAMSPro.TestingService.Dto;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace gAMSPro.Notifications
{
    [AbpAuthorize]
    public class TestingService : gAMSProCoreAppServiceBase, ITestingService
    {

        public TestingService()
        {
        }

        public async Task<SendEmailAndNotifyEntity> TestSendEmailAndNotify([FromBody] SendEmailAndNotifyEntity input)
        {
            await SendEmailAndNotify("", "", "TEST_SERVICE", "");
            return null;
        }
    }
}