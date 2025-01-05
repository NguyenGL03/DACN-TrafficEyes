using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.Images.Dto
{
    public class Image_ENTITY:PagedAndSortedInputDto
    {
        public int id { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }
        public string CapturedDate { get; set; }
        public int Cars { get; set; }
        public int Motorcycles { get; set; }

    }
}
