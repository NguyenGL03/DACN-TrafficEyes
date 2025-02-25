﻿using System.Threading.Tasks;
using Abp.Application.Services;
using gAMSPro.MultiTenancy.Payments.PayPal.Dto;

namespace gAMSPro.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
