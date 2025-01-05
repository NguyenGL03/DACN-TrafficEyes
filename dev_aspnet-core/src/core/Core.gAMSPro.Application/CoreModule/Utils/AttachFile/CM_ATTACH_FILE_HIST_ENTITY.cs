namespace Core.gAMSPro.Application.CoreModule.Utils.AttachFile
{
    public class CM_ATTACH_FILE_HIST_ENTITY
    {
        public int ID { get; set; }
        public string ATTACH_ID { get; set; }
        public string TYPE { get; set; }
        public string REF_ID { get; set; }
        public string FILE_NAME_OLD { get; set; }
        public string PATH_OLD { get; set; }
        public string FILE_NAME_NEW { get; set; }
        public string PATH_NEW { get; set; }
        public decimal? FILE_SIZE { get; set; }
        public string FILE_TYPE { get; set; }
        public string ATTACH_DT { get; set; }
        public string EMP_ID { get; set; }
        public string INDEX { get; set; }
        public string NOTES { get; set; }
        public string VERSION { get; set; }
        public string ACTION { get; set; }
    }
}
