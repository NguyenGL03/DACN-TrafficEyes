using System;

namespace Core.gAMSPro.Application.CoreModule.Utils.Notifications
{
    public class ROLE_NOTIFICATION_ENTITY
    {
        public int USER_ID { get; set; }
        public string NOTIFY_ID { get; set; }
        public string TL_NAME { get; set; }
        public DateTime? EDITOR_DT { get; set; }
        public string EDITOR_ID { get; set; }
        public string NOTES { get; set; }
        public string EMAIL { get; set; }
    }
}
