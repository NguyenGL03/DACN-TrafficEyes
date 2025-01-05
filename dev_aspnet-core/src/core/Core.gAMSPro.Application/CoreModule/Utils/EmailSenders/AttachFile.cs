namespace Core.gAMSPro.Application.CoreModule.Utils.EmailSenders
{
    public class AttachFile
    {
        public AttachFile()
        {
            names = new List<string>();
            dataAttachs = new List<MemoryStream>();
        }
        public List<string> names { get; set; }
        public bool isMulti { get; set; }
        public List<MemoryStream> dataAttachs { get; set; }
    }
}
