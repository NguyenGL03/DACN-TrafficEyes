namespace GSOFTcore.gAMSPro.Function.Dto
{
    public class REQ_INVENTORY_ENTITY
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string tranDate { get; set; }
    }

    public class REPONSE_INFO_INVENTORY_ENTITY
    {
        public string invTrdt { get; set; }
        public string invTrId { get; set; }
        public string invClss { get; set; }
        public string solIdFr { get; set; }
        public string invLocFr { get; set; }
        public string deptCode { get; set; }
        public string solIdTo { get; set; }
        public string invLocTo { get; set; }
        public string invQty { get; set; }
        public string srlNumFr { get; set; }
        public string srlNumTo { get; set; }
        public string invType { get; set; }
    }

    public class REPONSE_INVENTORY_IMPORT_ENTITY
    {
        public string trn_id { get; set; }
        public string material_code { get; set; }
        public string branch_code { get; set; }
        public string dep_code { get; set; }
        public string quantity { get; set; }
        public string serial_no_fr { get; set; }
        public string serial_no_to { get; set; }
    }

    public class REPONSE_INVENTORY_TRANFER_ENTITY
    {
        public string trn_id { get; set; }
        public string material_code { get; set; }

        public string branch_code_fr { get; set; }
        public string dep_code_fr { get; set; }
        public string branch_code_to { get; set; }
        public string dep_code_to { get; set; }
        public string quantity { get; set; }
        public string serial_no_fr { get; set; }
        public string serial_no_to { get; set; }
    }
}
