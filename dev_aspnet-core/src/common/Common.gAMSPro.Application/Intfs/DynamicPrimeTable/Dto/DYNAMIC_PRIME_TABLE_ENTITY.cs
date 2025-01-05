using gAMSPro.Dto;

namespace Common.gAMSPro.Application.Intfs.DynamicPrimeTable.Dto
{
    public class DYNAMIC_PRIME_TABLE_ENTITY
    {
        public string tableName { get; set; }

        public string ScreenName { get; set; }

        public string ScreenId { get; set; }

        public string AUTH_STATUS { get; set; }

        public List<DYNAMIC_COLUMN_PRIME_TABLE> Columns { get; set; }

        public DYNAMIC_CONFIG_PRIME_TABLE Config { get; set; }
    }

    public class DYNAMIC_COLUMN_PRIME_TABLE
    {
        public string id { get; set; }
        public string tableName { get; set; }
        public string columnId { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string sortField { get; set; }
        public string width { get; set; }
        public string align { get; set; }
        public string inputType { get; set; }
        public string selectorModal { get; set; } 
        public string displayMember { get; set; } 
        public string valueMember { get; set; } 
    }
    public class DYNAMIC_CONFIG_PRIME_TABLE
    {
        public bool indexing { get; set; }

        public bool checkbox { get; set; }

        public bool isShowError { get; set; }

        public bool isShowButtonAdd { get; set; }

        public bool isShowButtonDelete { get; set; }

        public string termName { get; set; }
    }
    public class DYNAMIC_SCREEN_PRIME_TABLE : PagedAndSortedInputDto
    {
        public string tableName { get; set; }

        public string screenName { get; set; }

        public string screenId { get; set; }

        public string? AUTH_STATUS_NAME { get; set; }

        public string AUTH_STATUS { get; set; }
        public int? TotalCount { get; set; }
        public bool? IsChecked { get; set; }
    }
}