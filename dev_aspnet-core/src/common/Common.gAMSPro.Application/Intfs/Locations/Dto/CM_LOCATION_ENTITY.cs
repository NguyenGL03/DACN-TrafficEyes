namespace Common.gAMSPro.Intfs.Locations.Dto
{
    public class CM_DISTRICT
    {
        public string DIS_ID { get; set; }
        public string DIS_CODE { get; set; }
        public string PRO_ID { get; set; }
        public string DIS_NAME { get; set; }
        public string DIS_TYPE { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATEDATE { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKED_ID { get; set; }
        public DateTime? DATE_APPROVE { get; set; }
    }

    public class CM_WARD
    {
        public string WAR_ID { get; set; }
        public string WAR_CODE { get; set; }
        public string DIS_ID { get; set; }
        public string WAR_NAME { get; set; }
        public string WAR_TYPE { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATEDATE { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKED_ID { get; set; }
        public DateTime? DATE_APPROVE { get; set; }
    }

    public class CM_NATION
    {
        public string NAT_ID { get; set; }
        public string NAT_CODE { get; set; }
        public string NAT_NAME { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATEDATE { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKED_ID { get; set; }
        public DateTime? DATE_APPROVE { get; set; }
    }

    public class CM_PROVINCE
    {
        public string PRO_ID { get; set; }
        public string PRO_CODE { get; set; }
        public string NAT_ID { get; set; }
        public string PRO_NAME { get; set; }
        public string PRO_TYPE { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATEDATE { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKED_ID { get; set; }
        public DateTime? DATE_APPROVE { get; set; }
    }

    public class CM_LOCATION_ENTITY
    {
        public List<CM_DISTRICT> DISTRICTs { get; set; }
        public List<CM_WARD> WARDs { get; set; }
        public List<CM_NATION> NATIONs { get; set; }
        public List<CM_PROVINCE> PROVINCEs { get; set; }
    }
}
