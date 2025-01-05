using System.Collections.Generic;

namespace Core.gAMSPro.Application.CoreModule.Utils.AttachFile
{
    public interface ICM_ATTACH_FILE
    {
        string ATTACH_ID { get; set; }
        string FILE_ATTACHMENT { get; set; }
        string FILE_ATTACHMENT_OLD { get; set; }
        string TYPE { get; }
        string INDEX { get; }
    }

    public class CM_ATTACH_FILE : ICM_ATTACH_FILE
    {
        public string ATTACH_ID { get; set; }
        public string FILE_ATTACHMENT { get; set; }
        public string FILE_ATTACHMENT_OLD { get; set; }
        public string TYPE { get; set; }
        public string INDEX { get; set; }
    }

    public class CM_ATTACH_FILE_INPUT
    {
        public CM_ATTACH_FILE_ENTITY AttachFile { get; set; }
        public List<CM_ATTACH_FILE_ENTITY> Childs { get; set; }
        public string Ids { get; set; }

        public string[] OldFiles { get; set; }
        public string[] NewFiles { get; set; }
    }
}
