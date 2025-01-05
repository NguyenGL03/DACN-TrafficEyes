using gAMSPro.Dto;

namespace Common.gAMSPro.Application.Intfs.DynamicPage.Dto
{
    public class DYNAMIC_PAGE_ENTITY: PagedAndSortedInputDto
    {
        public string PAGE_ID { get; set; }

        public string PAGE_NAME { get; set; }

        public string PAGE_URL { get; set; }

        public string RECORD_STATUS{ get; set; }

        public List<DYNAMIC_PAGE_INPUT_ENTITY> inputs { get; set; }
        public string inputsJson { get; set; }

    }
}