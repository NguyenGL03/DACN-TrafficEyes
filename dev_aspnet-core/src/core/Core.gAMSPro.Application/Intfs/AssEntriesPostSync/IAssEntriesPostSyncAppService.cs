using Abp.Application.Services;
using Core.gAMSPro.Intfs.AssEntriesPostSync.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.gAMSPro.Intfs.AssEntriesPostSync
{
    public interface IAssEntriesPostSyncAppService : IApplicationService
    {
        Task<List<ASS_ENTRIES_POST_SYNC_ENTITY>> ASS_ENTRIES_POST_SYNC_BY_TRN_ID(string id);
        Task<ASS_ENTRIES_POST_SYNC_ENTITY> PAY_ENTRIES_POST_SYNC_BY_TRN_ID(string id);
    }
}
