using System.Threading.Tasks;
using gAMSPro.Sessions.Dto;

namespace gAMSPro.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
