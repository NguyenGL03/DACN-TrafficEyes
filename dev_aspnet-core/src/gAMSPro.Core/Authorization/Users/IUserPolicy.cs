﻿using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace gAMSPro.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
