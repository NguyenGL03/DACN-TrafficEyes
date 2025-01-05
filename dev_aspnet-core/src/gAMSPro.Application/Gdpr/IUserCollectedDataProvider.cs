using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using gAMSPro.Dto;

namespace gAMSPro.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
