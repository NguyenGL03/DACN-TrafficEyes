using gAMSPro.Dto;

namespace Common.gAMSPro.AllCodes.Dto
{
    /// <summary>
    /// <see cref="Models.CM_ALLCODE"/>
    /// </summary>
    public class CM_ALLCODE_ENTITY : PagedAndSortedInputDto
    {
        public int? Id { get; set; }
        public string? CDNAME { get; set; }
        public string? CDVAL { get; set; }
        public string? CONTENT { get; set; }
        public string? CDTYPE { get; set; }
        public int? LSTODR { get; set; }
        public int? TotalCount { get; set; }
    }
}
