namespace Core.gAMSPro.Application.CoreModule.Utils.EmailSenders
{
    public class EmailInfo
    {
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BCCEmail { get; set; }
        public string Subj { get; set; }
        public string Message { get; set; }
        public MemoryStream dataAttach { get; set; }
        public string nameAttach { get; set; }
        public bool isAttach { get; set; }
        public AttachFile dataMultiAttachs { get; set; }
    }
}
