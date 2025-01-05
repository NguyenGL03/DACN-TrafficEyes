using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.gAMSPro.Intfs.WhiteCard
{
    public interface IWhiteCardAppService : IApplicationService
    {
        Task<bool> WhiteTaxMaterialSync(string id);
        Task<bool> WhiteCardInSync(string id);
        Task<bool> WhiteCardRefundSync(string id);
    }
}
