using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gAMSPro.MultiTenancy.HostDashboard.Dto;

namespace gAMSPro.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}