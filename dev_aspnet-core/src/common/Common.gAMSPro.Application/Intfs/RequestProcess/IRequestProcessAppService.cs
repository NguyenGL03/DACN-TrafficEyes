using Common.gAMSPro.Intfs.Process.Dto;
using Common.gAMSPro.Intfs.RequestProcess.Dto;
using Common.gAMSPro.Process.Dto;

namespace Common.gAMSPro.Intfs.RequestProcess
{
    public interface IRequestProcessAppService
    {
       
        Task<List<REQUEST_PROCESS_ENTITY>> PROCESS_CURRENT_SEARCH(string reQ_ID, string type, string userLogin);
        Task<List<PROCESS_ENTITY>> PROCESS_SEARCH(string reQ_ID, string type, string userLogin);
    }
}
