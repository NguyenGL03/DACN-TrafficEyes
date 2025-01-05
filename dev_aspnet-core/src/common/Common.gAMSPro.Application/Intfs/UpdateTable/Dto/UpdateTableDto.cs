using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.UpdateTable.Dto
{
    public class UpdateTableDto
    {
        public string REQ_ID { get; set; }
        public int UPDATE_COLUMN_KEY_ID { get; set; }
        public string UPDATE_VALUE {  get; set; }
        public string USER_LOGIN { get; set; }

        public string KEY{ get; set; }

        public string DISPLAY_VALUE{ get; set; }

        public int TYPE{get; set; }

        public long PARENT { get; set; }
        public long ID { get; set; }



    }
}
