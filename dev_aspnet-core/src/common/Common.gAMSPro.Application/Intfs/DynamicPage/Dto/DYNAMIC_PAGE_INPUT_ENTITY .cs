namespace Common.gAMSPro.Application.Intfs.DynamicPage.Dto
{
    public class DYNAMIC_PAGE_INPUT_ENTITY
    {
        public string INPUT_ID { get; set; }
        public string PAGE_ID { get; set; }
        public string LABEL { get; set; }
        public string FIELD_NAME { get; set; }
        public string INPUT_NAME { get; set; }
        public string INPUT_MODEL { get; set; }
        public string INPUT_TYPE { get; set; }
        public string DEFAULT_VALUE { get; set; }
        public string IS_REQUIRED { get; set; }
        public string IS_DISABLED { get; set; }
        public string IS_EDITABLE { get; set; }
        public string GRID_WIDTH { get; set; }
        public string CUSTOM_WIDTH { get; set; }
        public string POSITION { get; set; }
        public List<DYNAMIC_PAGE_INPUT_DESIGN_ENTITY> designs { get; set; }
    }

    public class DYNAMIC_PAGE_INPUT_DESIGN_ENTITY
    {
        public string ID { get; set; }
        public string InputId { get; set; }
        public string InputKey { get; set; }
        public string InputValue { get; set; }
    }
}