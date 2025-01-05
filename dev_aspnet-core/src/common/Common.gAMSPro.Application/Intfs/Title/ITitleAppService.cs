using Common.gAMSPro.Intfs.Title.Dto;

namespace Common.gAMSPro.Intfs.Title
{
    public interface ITitleAppService
    {
        Task<List<CM_TITLE_ENTITY>> CM_TITLE_SEARCH(string titleType);
    }
}
