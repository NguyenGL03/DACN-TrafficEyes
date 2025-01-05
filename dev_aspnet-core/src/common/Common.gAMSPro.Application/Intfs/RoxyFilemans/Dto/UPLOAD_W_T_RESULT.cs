using Core.gAMSPro.Application.CoreModule.Utils.AttachFile;

namespace Common.gAMSPro.Intfs.RoxyFilemans.Dto
{
    public class UPLOAD_W_T_RESULT
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public CM_ATTACH_FILE_ENTITY CM_ATTACH_FILE_ENTITY { get; set; }
    }
}
