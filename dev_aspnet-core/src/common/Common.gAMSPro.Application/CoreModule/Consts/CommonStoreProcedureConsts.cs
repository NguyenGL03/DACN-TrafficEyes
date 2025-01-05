namespace Common.gAMSPro.Consts
{
    public static class CommonStoreProcedureConsts
    {
        #region Stored procedure - Cài đặt layout trang
        public const string DYNAMIC_PAGE_Ins = "DYNAMIC_PAGE_Ins";
        public const string DYNAMIC_PAGE_Upd = "DYNAMIC_PAGE_Upd";
        public const string DYNAMIC_PAGE_Search = "DYNAMIC_PAGE_Search";
        public const string DYNAMIC_PAGE_ById = "DYNAMIC_PAGE_ById";
        public const string DYNAMIC_PAGE_Del = "DYNAMIC_PAGE_Del";
        public const string DYNAMIC_PAGE_INPUT_Search = "DYNAMIC_PAGE_INPUT_Search";
        #endregion

        #region Stored procedure - Cài đặt prime table
        public const string DYNAMIC_PRIME_TABLE_Del = "DYNAMIC_PRIME_TABLE_Del";
        public const string DYNAMIC_PRIME_TABLE_SendAppr = "DYNAMIC_PRIME_TABLE_SendAppr";
        public const string DYNAMIC_PRIME_TABLE_Search = "DYNAMIC_PRIME_TABLE_Search";
        public const string DYNAMIC_PRIME_TABLE_UPDATE = "DYNAMIC_PRIME_TABLE_UPDATE";
        public const string DYNAMIC_PRIME_TABLE_GetAllData = "DYNAMIC_PRIME_TABLE_GetAllData";
        public const string DYNAMIC_PRIME_TABLE_GetTableName = "DYNAMIC_PRIME_TABLE_GetTableName";
        public const string DYNAMIC_PRIME_TABLE_GetScreenName = "DYNAMIC_PRIME_TABLE_GetScreenName";
        public const string DYNAMIC_PRIME_TABLE_CREATE = "DYNAMIC_PRIME_TABLE_CREATE";
        #endregion
        public const string Image_Insert = "Image_Insert";
        public const string Image_Update = "Image_Update";
        public const string Image_Delete = "Image_Delete";
        public const string Image_Search_By_Location = "Image_Search_By_Location";
        public const string Image_Search_By_Location_And_Date_Range = "Image_Search_By_Location_And_Date_Range";
        public const string Image_Search_By_Location_And_Date = "Image_Search_By_Location_And_Date";

        public const string DYNAMIC_TRIGGER_GetName = "DYNAMIC_TRIGGER_GetName";
        public const string DYNAMIC_PROC_GetProcCode = "DYNAMIC_PROC_GetProcCode";
        public const string DYNAMIC_PROC_GetName = "DYNAMIC_PROC_GetName";
        public const string EXECUTE_QUERY = "EXECUTE_QUERY";
        public const string DYNAMIC_TABLE_NEW_UPDATE = "DYNAMIC_TABLE_NEW_UPDATE";
        public const string DYNAMIC_TABLE_NEW_CREATE = "DYNAMIC_TABLE_NEW_CREATE";
        public const string DYNAMIC_TABLE_GETCOLNAME = "DYNAMIC_TABLE_GetColName";
        public const string DYNAMIC_TABLE_GETTABLENAME = "DYNAMIC_TABLE_GetTableName";
        public const string UPDATE_TABLE_UDP = "SYS_UPDATE_TABLE_CONFIG_Upd";
        public const string UPDATE_TABLE_INS = "SYS_UPDATE_TABLE_CONFIG_Ins";
        public const string UPDATE_TABLE_GETLIST = "UPDATE_TABLE_GetList";
        public const string UPDATE_TABLE_BYLEVEL = "UPDATE_TABLE_BYLEVEL";
        public const string UPDATE_TABLE = "SYS_UPDATE_TABLE";
        public const string SYS_UPDATE_DYNAMIC_TABLE_SEARCH = "SYS_UPDATE_DYNAMIC_TABLE_Search";
        public const string ORGANIZATION_UNIT_Search = "ORGANIZATION_UNIT_Search";
        public const string ORGANIZATION_UNIT_Ins = "ORGANIZATION_UNIT_Ins";
        public const string ORGANIZATION_UNIT_Upd = "ORGANIZATION_UNIT_Upd";
        public const string ORGANIZATION_UNIT_Del = "ORGANIZATION_UNIT_Del";
        public const string ORGANIZATION_UNIT_Move = "ORGANIZATION_UNIT_Move";
        public const string ORGANIZATION_UNIT_USER_Search = "ORGANIZATION_UNIT_USER_Search";

        public const string CM_APPROVE_GROUP_BYID = "CM_APPROVE_GROUP_ById";
        public const string CM_APPROVE_GROUP_SEARCH = "CM_APPROVE_GROUP_Search";
        public const string CM_APPROVE_GROUP_INS = "CM_APPROVE_GROUP_INS";
        public const string CM_APPROVE_GROUP_UPD = "CM_APPROVE_GROUP_Upd";
        public const string CM_APPROVE_GROUP_DEL = "CM_APPROVE_GROUP_Del";

        public const string CM_APPROVE_GROUP_SENDAPPR = "CM_APPROVE_GROUP_SendAppr";


        public const string CM_ACCOUNT_PAY_INS = "CM_ACCOUNT_PAY_Ins";
        public const string CM_ACCOUNT_PAY_UPD = "CM_ACCOUNT_PAY_Upd";
        public const string CM_ACCOUNT_PAY_DEL = "CM_ACCOUNT_PAY_DEL";
        public const string CM_ACCOUNT_PAY_SEARCH = "CM_ACCOUNT_PAY_Search";
        public const string CM_ACCOUNT_Search = "CM_ACCOUNT_Search";
        public const string CM_ACCOUNT_PAY_BYID = "CM_ACCOUNT_PAY_BYID";
        public const string CM_ACCOUNT_PAY_APP = "CM_ACCOUNT_PAY_Appr";
        public const string CM_ACCOUNT_PAY_LIST = "CM_ACCOUNT_PAY_List";
        public const string CM_ACCOUNT_BYID = "CM_ACCOUNT_BYID";
        public const string CM_ACCOUNT_Upd = "CM_ACCOUNT_Upd";
        public const string CM_ACCOUNT_PAY_Appr = "CM_ACCOUNT_PAY_Appr";

        public const string CM_BRANCH_INS = "CM_BRANCH_Ins";
        public const string CM_BRANCH_UPD = "CM_BRANCH_Upd";
        public const string CM_BRANCH_APP = "CM_BRANCH_App";
        public const string CM_BRANCH_LEV = "CM_BRANCH_Lev";
        public const string CM_BRANCH_SEARCH = "CM_BRANCH_Search";
        public const string CM_BRANCH_DEL = "CM_BRANCH_Del";
        public const string CM_BRANCH_BYID = "CM_BRANCH_ById";
        public const string CM_BRANCH_COMBOBOX = "CM_BRANCH_Combobox";
        public const string CM_BRANCH_USER_Combobox = "CM_BRANCH_USER_Combobox";
        public const string CM_BRANCH_BYBRANCHTYPE = "CM_BRANCH_ByBranchType";
        public const string CM_BRANCH_GETFATHERLIST = "CM_BRANCH_GetFatherList";
        public const string CM_BRANCH_GETALLCHILD = "CM_BRANCH_GETALLCHILD";
        public const string CM_BRANCH_BYCODE = "CM_BRANCH_ByCode";
        public const string CM_BRANCH_GET_ALL = "CM_BRANCH_GET_ALL";

        public const string CM_DEPARTMENT_INS = "CM_DEPARTMENT_Ins";
        public const string CM_DEPARTMENT_SEARCH = "CM_DEPARTMENT_Search";
        public const string CM_DEPARTMENT_HS_List = "CM_DEPARTMENT_HS_List";
        public const string CM_DEPARTMENT_COSTCENTER_Search = "CM_DEPARTMENT_COSTCENTER_Search";
        public const string CM_DEPARTMENT_UPD = "CM_DEPARTMENT_Upd";
        public const string CM_DEPARTMENT_APP = "CM_DEPARTMENT_App";
        public const string CM_DEPARTMENT_DEL = "CM_DEPARTMENT_Del";
        public const string CM_DEPARTMENT_BYID = "CM_DEPARTMENT_ById";
        public const string CM_DEPARTMENT_COMBOBOX = "CM_DEPARTMENT_Combobox";

        public const string CM_ALLCODE_GETBYCDNAME = "CM_ALLCODE_GetByCDNAME";

        public const string CM_ATTACH_FILE_BY_REFMASTER = "CM_ATTACH_FILE_By_RefMaster";
        public const string CM_ATTACH_FILE_BY_REFID = "CM_ATTACH_FILE_By_RefId";
        public const string CM_ATTACH_FILE_REQ_Search = "CM_ATTACH_FILE_REQ_Search";
        public const string CM_ATTACH_FILE_Ins_Xml_v2 = "CM_ATTACH_FILE_Ins_Xml_v2";

        public const string CM_REGION_INS = "CM_REGION_Ins";
        public const string CM_REGION_UPD = "CM_REGION_Upd";
        public const string CM_REGION_APP = "CM_REGION_App";
        public const string CM_REGION_DEL = "CM_REGION_Del";
        public const string CM_REGION_BYID = "CM_REGION_ById";
        public const string CM_REGION_LNS = "CM_REGION_Lns";
        public const string CM_REGIONS_SEARCH = "CM_REGIONS_Search";

        public const string TL_MENU_INS = "TL_MENU_Ins";
        public const string TL_MENU_UPD = "TL_MENU_Upd";
        public const string TL_MENU_APP = "TL_MENU_App";
        public const string TL_MENU_DEL = "TL_MENU_Del";
        public const string TL_MENU_BYID = "TL_MENU_ById";
        public const string TL_MENU_SEARCH = "TL_MENU_Search";
        public const string TL_MENU_SEARCH_By_RoleID = "TL_MENU_Search_By_RoleID";

        public const string CM_DEPT_GROUP_INS = "CM_DEPT_GROUP_Ins";
        public const string CM_DEPT_GROUP_UPD = "CM_DEPT_GROUP_Upd";
        public const string CM_DEPT_GROUP_APP = "CM_DEPT_GROUP_App";
        public const string CM_DEPT_GROUP_DEL = "CM_DEPT_GROUP_Del";
        public const string CM_DEPT_GROUP_BYID = "CM_DEPT_GROUP_ById";
        public const string CM_DEPT_GROUP_SEARCH = "CM_DEPT_GROUP_Search";

        public const string CM_SUPPLIERTYPE_INS = "CM_SUPPLIERTYPE_Ins";
        public const string CM_SUPPLIERTYPE_UPD = "CM_SUPPLIERTYPE_Upd";
        public const string CM_SUPPLIERTYPE_APP = "CM_SUPPLIERTYPE_App";
        public const string CM_SUPPLIERTYPE_SEARCH = "CM_SUPPLIERTYPE_Search";
        public const string CM_SUPPLIERTYPE_DEL = "CM_SUPPLIERTYPE_Del";
        public const string CM_SUPPLIERTYPE_BYID = "CM_SUPPLIERTYPE_ById";

        public const string CM_SUPPLIER_INS = "CM_SUPPLIER_Ins";
        public const string CM_SUPPLIER_UPD = "CM_SUPPLIER_Upd";
        public const string CM_SUPPLIER_APP = "CM_SUPPLIER_App";
        public const string CM_SUPPLIER_DEL = "CM_SUPPLIER_Del";
        public const string CM_SUPPLIER_BYID = "CM_SUPPLIER_ById";
        public const string CM_SUPPLIER_SEARCH = "CM_SUPPLIER_Search";

        public const string CM_GOODSTYPE_INS = "CM_GOODSTYPE_Ins";
        public const string CM_GOODSTYPE_UPD = "CM_GOODSTYPE_Upd";
        public const string CM_GOODSTYPE_APP = "CM_GOODSTYPE_App";
        public const string CM_GOODSTYPE_DEL = "CM_GOODSTYPE_Del";
        public const string CM_GOODSTYPE_BYID = "CM_GOODSTYPE_ById";
        public const string CM_GOODSTYPE_SEARCH = "CM_GOODSTYPE_Search";
        public const string CM_GOODSTYPE_LIST = "CM_GOODSTYPE_LIST";
        public const string CM_GOODSTYPE_KH_SEARCH = "CM_GOODSTYPE_KH_Search";

        public const string CM_LIST_DOCUMENT_INS = "CM_LIST_DOCUMENT_Ins";
        public const string CM_LIST_DOCUMENT_UPD = "CM_LIST_DOCUMENT_Upd";
        public const string CM_LIST_DOCUMENT_DEL = "CM_LIST_DOCUMENT_Del";
        public const string CM_LIST_DOCUMENT_BYID = "CM_LIST_DOCUMENT_ById";
        public const string CM_LIST_DOCUMENT_SEARCH = "CM_LIST_DOCUMENT_Search";
        public const string CM_LIST_DOCUMENT_APP = "CM_LIST_DOCUMENT_App";
        public const string CM_LIST_DOCUMENT_COMBOBOX = "CM_LIST_DOCUMENT_Combobox";

        public const string CM_GOODSTYPE_REAL_INS = "CM_GOODSTYPE_REAL_Ins";
        public const string CM_GOODSTYPE_REAL_UPD = "CM_GOODSTYPE_REAL_Upd";
        public const string CM_GOODSTYPE_REAL_APPR = "CM_GOODSTYPE_REAL_Appr";
        public const string CM_GOODSTYPE_REAL_DEL = "CM_GOODSTYPE_REAL_Del";
        public const string CM_GOODSTYPE_REAL_BYID = "CM_GOODSTYPE_REAL_ById";
        public const string CM_GOODSTYPE_REAL_APP = "CM_GOODSTYPE_REAL_App";
        public const string CM_GOODSTYPE_REAL_SEARCH = "CM_GOODSTYPE_REAL_SEARCH";

        public const string TL_USER_INS = "TL_USER_Ins";
        public const string TL_USER_UPD = "TL_USER_Upd";
        public const string TL_USER_APP = "TL_USER_App";
        public const string TL_USER_DEL = "TL_USER_Del";
        public const string TL_USER_BYID = "TL_USER_ById";
        public const string TL_USER_SEARCH = "TL_USER_SEARCH";
        public const string TL_USER_GET_List = "TL_USER_GET_List";
        public const string TL_USER_By_DEPARTMENT = "TL_USER_By_DEPARTMENT";
        public const string TLUSER_MANAGER_SEARCH = "TLUSER_MANAGER_SEARCH";

        public const string CM_UNIT_INS = "CM_UNIT_Ins";
        public const string CM_UNIT_UPD = "CM_UNIT_Upd";
        public const string CM_UNIT_APP = "CM_UNIT_App";
        public const string CM_UNIT_DEL = "CM_UNIT_Del";
        public const string CM_UNIT_BYID = "CM_UNIT_ById";
        public const string CM_UNIT_SEARCH = "CM_UNIT_Search";
        public const string CM_UNIT_LIST = "CM_UNIT_LIST";

        public const string CM_GOODS_INS = "CM_GOODS_Ins";
        public const string CM_GOODS_UPD = "CM_GOODS_Upd";
        public const string CM_GOODS_APP = "CM_GOODS_App";
        public const string CM_GOODS_DEL = "CM_GOODS_Del";
        public const string CM_GOODS_BYID = "CM_GOODS_ById";
        public const string CM_GOODS_SEARCH = "CM_GOODS_Search";
        public const string CM_GOODS_SearchByLevel1 = "CM_GOODS_SearchByLevel1";

        public const string CM_DIVISION_INS = "CM_DIVISION_Ins";
        public const string CM_DIVISION_UPD = "CM_DIVISION_Upd";
        public const string CM_DIVISION_APP = "CM_DIVISION_App";
        public const string CM_DIVISION_DEL = "CM_DIVISION_Del";
        public const string CM_DIVISION_BYID = "CM_DIVISION_ById";
        public const string CM_DIVISION_SEARCH = "CM_DIVISION_SEARCH";
        public const string CM_DIVISION_GETALLCHILD = "CM_DIVISION_GETALLCHILD";

        public const string CM_EMPLOYEE_INS_MUL = "CM_EMPLOYEE_Ins_Mul";
        public const string CM_EMPLOYEE_UPD_MUL = "CM_EMPLOYEE_Upd_Mul";
        public const string CM_EMPLOYEE_DEL_MUL = "CM_EMPLOYEE_Del_Mul";

        public const string CM_EMPLOYEE_INS = "CM_EMPLOYEE_Ins";
        public const string CM_EMPLOYEE_UPD = "CM_EMPLOYEE_Upd";
        public const string CM_EMPLOYEE_APP = "CM_EMPLOYEE_App";
        public const string CM_EMPLOYEE_DEL = "CM_EMPLOYEE_Del";
        public const string CM_EMPLOYEE_EXIST_IN_TLUSER_MODAL = "CM_EMPLOYEE_EXIST_IN_TLUSER_MODAL";
        public const string CM_EMPLOYEE_LOG_ByUserName = "CM_EMPLOYEE_LOG_ByUserName";
        public const string CM_EMPLOYEE_BYID = "CM_EMPLOYEE_ById";
        public const string CM_EMPLOYEE_SEARCH = "CM_EMPLOYEE_Search";
        public const string CM_EMPLOYEE_SYNC = "CM_EMPLOYEE_Sync";
        public const string CM_ORGANIZATIONAL_SYNC = "CM_ORGANIZATIONAL_Sync";


        public const string CM_INSU_COMPANY_INS = "INSU_COMPANY_Ins";
        public const string CM_INSU_COMPANY_UPD = "INSU_COMPANY_Upd";
        public const string CM_INSU_COMPANY_APP = "INSU_COMPANY_App";
        public const string CM_INSU_COMPANY_DEL = "INSU_COMPANY_Del";
        public const string CM_INSU_COMPANY_SEARCH = "INSU_COMPANY_Search";
        public const string CM_INSU_COMPANY_BYID = "INSU_COMPANY_ById";

        public const string CM_MODEL_INS = "CM_MODEL_Ins";
        public const string CM_MODEL_UPD = "CM_MODEL_Upd";
        public const string CM_MODEL_APP = "CM_MODEL_App";
        public const string CM_MODEL_DEL = "CM_MODEL_Del";
        public const string CM_MODEL_BYID = "CM_MODEL_ById";
        public const string CM_MODEL_SEARCH = "CM_MODEL_Search";

        public const string CM_CAR_TYPE_INS = "CAR_TYPE_Ins";
        public const string CM_CAR_TYPE_LIST = "CAR_TYPE_List";
        public const string CM_CAR_TYPE_UPD = "CAR_TYPE_Upd";
        public const string CM_CAR_TYPE_APP = "CAR_TYPE_App";
        public const string CM_CAR_TYPE_DEL = "CAR_TYPE_Del";
        public const string CM_CAR_TYPE_BYID = "CAR_TYPE_ById";

        public const string CM_ALLCODE_INS = "CM_ALLCODE_Ins";
        public const string CM_ALLCODE_UPD = "CM_ALLCODE_Upd";
        public const string CM_ALLCODE_APP = "CM_ALLCODE_App";
        public const string CM_ALLCODE_DEL = "CM_ALLCODE_Del";
        public const string CM_ALLCODE_BYID = "CM_ALLCODE_ById";
        public const string CM_ALLCODE_SEARCH = "CM_ALLCODE_Search";
        //BAODNQ 1/7/2022
        public const string CM_ALLCODE_BYID_V2 = "CM_ALLCODE_ById_v2";

        public const string CM_WORKFLOW_INS = "CM_WORKFLOW_Ins";
        public const string CM_WORKFLOW_UPD = "CM_WORKFLOW_Upd";
        public const string CM_WORKFLOW_APP = "CM_WORKFLOW_App";
        public const string CM_WORKFLOW_DEL = "CM_WORKFLOW_Del";
        public const string CM_WORKFLOW_TRANSACTION_Reject = "CM_WORKFLOW_TRANSACTION_Reject";
        public const string CM_WORKFLOW_BYID = "CM_WORKFLOW_ById";
        public const string CM_WORKFLOW_ASSIGN_BYID = "CM_WORKFLOW_ASSIGN_ById";
        public const string CM_WORKFLOW_REJECT_DETAIL_LIST = "CM_WORKFLOW_REJECT_DETAIL_List";

        public const string SYS_PARAMETERS_INS = "SYS_PARAMETERS_Ins";
        public const string SYS_PARAMETERS_UPD = "SYS_PARAMETERS_Upd";
        public const string SYS_PARAMETERS_APP = "SYS_PARAMETERS_App";
        public const string SYS_PARAMETERS_SEARCH = "SYS_PARAMETERS_Search";
        public const string SYS_PARAMETERS_DEL = "SYS_PARAMETERS_Del";
        public const string SYS_PARAMETERS_BYID = "SYS_PARAMETERS_ById";
        public const string SYS_PARAMETERS_BYPARAKEY = "SYS_PARAMETERS_ByParaKey";

        public const string CM_REPORT_TEMPLATE_INS = "CM_REPORT_TEMPLATE_Ins";
        public const string CM_REPORT_TEMPLATE_UPD = "CM_REPORT_TEMPLATE_Upd";
        public const string CM_REPORT_TEMPLATE_APP = "CM_REPORT_TEMPLATE_App";
        public const string CM_REPORT_TEMPLATE_DEL = "CM_REPORT_TEMPLATE_Del";
        public const string CM_REPORT_TEMPLATE_BYID = "CM_REPORT_TEMPLATE_ById";
        public const string CM_REPORT_TEMPLATE_DETAIL_BYTEMPLATEID = "CM_REPORT_TEMPLATE_DETAIL_ByTemplateId";
        public const string CM_REPORT_TEMPLATE_DETAIL_DEFAULT_BYTEMPLATEID = "CM_REPORT_TEMPLATE_DETAIL_DEFAULT_ByTemplateId";
        public const string CM_REPORT_TEMPLATE_BYCODE = "CM_REPORT_TEMPLATE_ByCode";
        public const string CM_REPORT_TEMPLATE_DETAIL_UPD = "CM_REPORT_TEMPLATE_DETAIL_Upd";
        public const string CM_REPORT_TEMPLATE_DETAIL_BYID = "CM_REPORT_TEMPLATE_DETAIL_ById";

        public const string TLUSER_GETBY_BRANCHID = "TLUSER_GETBY_BRANCHID";

        public const string CM_TERM_SEARCH = "CM_TERM_SEARCH";
        // 28-04-2021
        public const string CM_TERM_INS = "CM_TERM_INS";
        public const string CM_TERM_UPD = "CM_TERM_UPD";
        public const string CM_TERM_APPR = "CM_TERM_APPR";
        public const string CM_TERM_DEL = "CM_TERM_DEL";
        public const string CM_TERM_BYID = "CM_TERM_BYID";
        public const string CM_TERM_SEARCHPARAM = "CM_TERM_SEARCHPARAM";

        public const string CM_LOCATION_ALLDATA = "CM_LOCATION_AllData";

        public const string CM_MAPPING_CHUCDANH_ROLE_Upd = "CM_MAPPING_CHUCDANH_ROLE_Upd";
        public const string CM_MAPPING_CHUCDANH_ROLE_Search = "CM_MAPPING_CHUCDANH_ROLE_Search";
        public const string CM_MAPPING_CHUCDANH_ROLE_Ins = "CM_MAPPING_CHUCDANH_ROLE_Ins";
        public const string CM_MAPPING_CHUCDANH_ROLE_Del = "CM_MAPPING_CHUCDANH_ROLE_Del";
        public const string CM_MAPPING_CHUCDANH_ROLE_ById = "CM_MAPPING_CHUCDANH_ROLE_ById";

        public const string TL_SYS_ROLE_MAPPING_Upd = "TL_SYS_ROLE_MAPPING_Upd";
        public const string TL_SYS_ROLE_MAPPING_Search = "TL_SYS_ROLE_MAPPING_Search";
        public const string TL_SYS_ROLE_MAPPING_Ins = "TL_SYS_ROLE_MAPPING_Ins";
        public const string TL_SYS_ROLE_MAPPING_Del = "TL_SYS_ROLE_MAPPING_Del";
        public const string TL_SYS_ROLE_MAPPING_Sync = "TL_SYS_ROLE_MAPPING_Sync";
        public const string TL_SYS_GET_USER_BY_ROLE = "TL_SYS_GET_USER_BY_ROLE";

        // Duy 
        public const string TL_SYSROLE_LIMIT_Appr = "TL_SYSROLE_LIMIT_Appr";
        public const string TL_SYSROLE_LIMIT_ByID = "TL_SYSROLE_LIMIT_ByID";
        public const string TL_SYSROLE_LIMIT_Search = "TL_SYSROLE_LIMIT_Search";
        public const string TL_SYSROLE_LIMIT_Del = "TL_SYSROLE_LIMIT_Del";
        public const string TL_SYSROLE_LIMIT_Ins = "TL_SYSROLE_LIMIT_Ins";
        public const string TL_SYSROLE_LIMIT_Upd = "TL_SYSROLE_LIMIT_Upd";
        public const string TL_SYSROLE_LIMIT_Auto = "TL_SYSROLE_LIMIT_Auto";
        public const string TL_SYSROLE_List = "TL_SYSROLE_List";
        public const string CM_DVDM_Search = "CM_DVDM_Search";

        public const string CM_REJECT_LOG_SEARCH = "CM_REJECT_LOG_Search";
        public const string CM_REJECT_LOG_INS = "CM_REJECT_LOG_Ins";
        public const string CM_REJECT_LOG_ByID = "CM_REJECT_LOG_ByID";
        public const string CM_REJECT_LOG_Hist = "CM_REJECT_LOG_Hist";
        // GiaNT 02/11/2021
        public const string PL_PROCESS_REJECT_SEARCH = "PL_PROCESS_REJECT_Search";
        public const string CM_REJECT_LOG_ByType_Ins = "CM_REJECT_LOG_ByType_Ins";

        public const string CM_KHOI_APP = "CM_KHOI_APP";
        public const string CM_KHOI_BYID = "CM_KHOI_ById";
        public const string CM_KHOI_DEL = "CM_KHOI_Del";
        public const string CM_KHOI_INS = "CM_KHOI_Ins";
        public const string CM_KHOI_SEARCH = "CM_KHOI_Search";
        public const string CM_KHOI_UPD = "CM_KHOI_Upd";
        public const string CM_KHOI_LST = "CM_KHOI_Lst";
        public const string CM_KHOI_DT_LST = "CM_KHOI_DT_Lst";
        public const string CM_KHOI_DT_SEARCH = "CM_KHOI_DT_Search";

        // GiaNT
        public const string CM_HANGHOA_INS = "CM_HANGHOA_Ins";
        public const string CM_HANGHOA_UPD = "CM_HANGHOA_Upd";
        public const string CM_HANGHOA_APP = "CM_HANGHOA_App";
        public const string CM_HANGHOA_DEL = "CM_HANGHOA_Del";
        public const string CM_HANGHOA_BYID = "CM_HANGHOA_ById";
        public const string CM_HANGHOA_SEARCH = "CM_HANGHOA_Search";

        public const string CM_HANGHOA_TYPE_SEARCH = "CM_HANGHOA_TYPE_Search";
        public const string CM_HANGHOA_TYPE_INS = "CM_HANGHOA_TYPE_Ins";
        public const string CM_HANGHOA_TYPE_UPD = "CM_HANGHOA_TYPE_Upd";
        public const string CM_HANGHOA_TYPE_APP = "CM_HANGHOA_TYPE_App";
        public const string CM_HANGHOA_TYPE_DEL = "CM_HANGHOA_TYPE_Del";
        public const string CM_HANGHOA_TYPE_BYID = "CM_HANGHOA_TYPE_ById";

        public const string CM_HANGHOA_GROUP_SEARCH = "CM_HANGHOA_GROUP_Search";
        public const string CM_HANGHOA_GROUP_INS = "CM_HANGHOA_GROUP_Ins";
        public const string CM_HANGHOA_GROUP_UPD = "CM_HANGHOA_GROUP_Upd";
        public const string CM_HANGHOA_GROUP_APP = "CM_HANGHOA_GROUP_App";
        public const string CM_HANGHOA_GROUP_DEL = "CM_HANGHOA_GROUP_Del";
        public const string CM_HANGHOA_GROUP_BYID = "CM_HANGHOA_GROUP_ById";


        // GiaNT 16/06/2021 
        public const string SYS_GROUP_LIMIT_INS = "SYS_GROUP_LIMIT_Ins";

        // GiaNT 23/06/2021
        public const string SYS_GROUP_LIMIT_DT_UPD = "SYS_GROUP_LIMIT_DT_Upd";
        public const string SYS_GROUP_LIMIT_DT_DEL = "SYS_GROUP_LIMIT_DT_Del";
        public const string SYS_GROUP_LIMIT_DT_INS = "SYS_GROUP_LIMIT_DT_Ins";
        public const string SYS_GROUP_LIMIT_DT_BYID = "SYS_GROUP_LIMIT_DT_ById";



        public const string SYS_GROUP_LIMIT_UPD = "SYS_GROUP_LIMIT_Upd";
        public const string SYS_GROUP_LIMIT_APP = "SYS_GROUP_LIMIT_App";
        public const string SYS_GROUP_LIMIT_DEL = "SYS_GROUP_LIMIT_Del";
        public const string SYS_GROUP_LIMIT_BYID = "SYS_GROUP_LIMIT_ById";

        // GiaNT 23/06/2021
        public const string SYS_GROUP_LIMIT_SEARCH = "SYS_GROUP_LIMIT_Search";
        public const string SYS_GROUP_LIMIT_DT_SEARCH = "SYS_GROUP_LIMIT_DT_Search";

        // Duy 17-6-2021
        public const string CM_DEPT_BLOCKMAP_Search = "CM_DEPT_BLOCKMAP_Search";
        public const string CM_DEPT_BLOCKMAP_Ins = "CM_DEPT_BLOCKMAP_Ins";
        public const string CM_DEPT_BLOCKMAP_Upd = "CM_DEPT_BLOCKMAP_Upd";
        public const string CM_DEPT_BLOCKMAP_Del = "CM_DEPT_BLOCKMAP_Del";
        public const string CM_DEPT_BLOCKMAP_App = "CM_DEPT_BLOCKMAP_App";
        public const string CM_DEPT_BLOCKMAP_ById = "CM_DEPT_BLOCKMAP_ById";
        // Phong 15-6-2022 Bổ sung lưới tham chiếu danh sách người quản lý
        public const string CM_DEPT_BLOCKMAP_MANAGER_ById = "CM_DEPT_BLOCKMAP_MANAGER_ById";

        public const string CM_POSITION_BLOCK_Search = "CM_POSITION_BLOCK_Search";
        public const string CM_POSITION_BLOCK_Ins = "CM_POSITION_BLOCK_Ins";
        public const string CM_POSITION_BLOCK_Upd = "CM_POSITION_BLOCK_Upd";
        public const string CM_POSITION_BLOCK_Del = "CM_POSITION_BLOCK_Del";
        public const string CM_POSITION_BLOCK_App = "CM_POSITION_BLOCK_App";
        public const string CM_POSITION_BLOCK_ById = "CM_POSITION_BLOCK_ById";



        // GiaNT 18-06-2021
        public const string SYS_NOTIF_MASTER_INS = "SYS_NOTIF_MASTER_Ins";
        public const string SYS_NOTIF_MASTER_UPD = "SYS_NOTIF_MASTER_Upd";
        public const string SYS_NOTIF_MASTER_APP = "SYS_NOTIF_MASTER_App";
        public const string SYS_NOTIF_MASTER_DEL = "SYS_NOTIF_MASTER_Del";
        public const string SYS_NOTIF_MASTER_BYID = "SYS_NOTIF_MASTER_ById";
        public const string SYS_NOTIF_MASTER_SEARCH = "SYS_NOTIF_MASTER_Search";


        // GiaNT 14/07/2021
        public const string CM_PLAN_TYPE_SEARCH = "CM_PLAN_TYPE_Search";

        public const string CM_PLAN_TYPE_LIST = "CM_PLAN_TYPE_List";

        public const string CM_PLAN_TYPE_App = "CM_PLAN_TYPE_App";
        public const string CM_PLAN_TYPE_ById = "CM_PLAN_TYPE_ById";
        public const string CM_PLAN_TYPE_DEL = "CM_PLAN_TYPE_DEL";
        public const string CM_PLAN_TYPE_Ins = "CM_PLAN_TYPE_Ins";
        public const string CM_PLAN_TYPE_Upd = "CM_PLAN_TYPE_Upd";

        public const string CM_DEVICE_INS = "CM_DEVICE_Ins";
        public const string CM_DEVICE_UPD = "CM_DEVICE_Upd";
        public const string CM_DEVICE_APPR = "CM_DEVICE_Appr";
        public const string CM_DEVICE_DEL = "CM_DEVICE_Del";
        public const string CM_DEVICE_BYID = "CM_DEVICE_ById";
        public const string CM_DEVICE_SEARCH = "CM_DEVICE_Search";
        public const string CM_DEVICE_IMPORT = "CM_DEVICE_Import";
        public const string CM_BRANCH_DEP_SEARCH = "CM_BRANCH_DEP_Search";
        public const string CM_BRANCH_DEP_SEARCH_v2 = "CM_BRANCH_DEP_Search_v2";
        public const string CM_BRANCH_DEP_ById = "CM_BRANCH_DEP_ById";

        // doanptt 28/12/2021
        public const string CM_DEPARTMENT_BYCODE = "CM_DEPARTMENT_ByCode";
        public const string CM_BRANCH_ByCode = "CM_BRANCH_ByCode";
        public const string TR_REQ_PAYMENT_LOG_LIST_SEARCH = "TR_REQ_PAYMENT_LOG_LIST_Search";
        public const string TR_REQ_PAY_INVOICE_LOG_Search = "TR_REQ_PAY_INVOICE_LOG_Search";
        public const string TR_REJECT_PROCESS_Search = "TR_REJECT_PROCESS_Search";

        // TienLee 05/03/22
        public const string DB_STATUS_ASSET_VALUE_BAR = "DB_STATUS_ASSET_VALUE_BAR";
        public const string DB_STATUS_ASSET_QUANTITY_BAR = "DB_STATUS_ASSET_QUANTITY_BAR";
        public const string DB_STATUS_ASSET_STATUS_PIE = "DB_STATUS_ASSET_STATUS_PIE";
        public const string DB_STATUS_ASSET_QUANTITY_PIE = "DB_STATUS_ASSET_QUANTITY_PIE";
        public const string DB_AMORT_ASSET_PIE = "DB_AMORT_ASSET_PIE";
        public const string DB_VALUE_ASSET_PIE = "DB_VALUE_ASSET_PIE";
        public const string DB_BUDGET_BAR = "DB_BUDGET_BAR";
        public const string DB_BUDGET_PIE = "DB_BUDGET_PIE";
        public const string DB_BUY_VALUE_PLAN_PIE = "DB_BUY_VALUE_PLAN_PIE";
        public const string DB_BUY_VALUE_BAR = "DB_BUY_VALUE_BAR";
        //luatndv 31/01/2023
        public const string CM_REQUEST_TEMPLATE_INS = "CM_REQUEST_TEMPLATE_Ins";
        public const string CM_REQUEST_TEMPLATE_UPD = "CM_REQUEST_TEMPLATE_Upd";
        public const string CM_REQUEST_TEMPLATE_APP = "CM_REQUEST_TEMPLATE_App";
        public const string CM_REQUEST_TEMPLATE_DEL = "CM_REQUEST_TEMPLATE_Del";
        public const string CM_REQUEST_TEMPLATE_BYID = "CM_REQUEST_TEMPLATE_ById";
        public const string CM_REQUEST_TEMPLATE_DETAIL_BYTEMPLATEID = "CM_REQUEST_TEMPLATE_DETAIL_ByTemplateId";
        public const string CM_REQUEST_TEMPLATE_BYCODE = "CM_REQUEST_TEMPLATE_ByCode";
        public const string CM_REQUEST_TEMPLATE_DETAIL_UPD = "CM_REQUEST_TEMPLATE_DETAIL_Upd";
        public const string CM_REQUEST_TEMPLATE_DETAIL_BYID = "CM_REQUEST_TEMPLATE_DETAIL_ById";
        public const string CM_REQUEST_TEMPLATE_Open_Template = "CM_REQUEST_TEMPLATE_Open_Template";
        public const string CM_REQUEST_TEMPLATE_Close_Template = "CM_REQUEST_TEMPLATE_Close_Template";



        public const string CM_TYPE_TEMPLATE_INS = "CM_TYPE_TEMPLATE_Ins";
        public const string CM_TYPE_TEMPLATE_UPD = "CM_TYPE_TEMPLATE_Upd";
        public const string CM_TYPE_TEMPLATE_APP = "CM_TYPE_TEMPLATE_App";
        public const string CM_TYPE_TEMPLATE_DEL = "CM_TYPE_TEMPLATE_Del";
        public const string CM_TYPE_TEMPLATE_BYID = "CM_TYPE_TEMPLATE_ById";
        public const string CM_TYPE_TEMPLATE_DETAIL_BYTEMPLATEID = "CM_TYPE_TEMPLATE_DETAIL_ByTemplateId";
        public const string CM_TYPE_TEMPLATE_BYCODE = "CM_TYPE_TEMPLATE_ByCode";
        public const string CM_TYPE_TEMPLATE_DETAIL_UPD = "CM_TYPE_TEMPLATE_DETAIL_Upd";
        public const string CM_TYPE_TEMPLATE_DETAIL_BYID = "CM_TYPE_TEMPLATE_DETAIL_ById";
        //Luatndv 24/09/2022
        public const string CM_PROCESS_ByProcess = "CM_PROCESS_ByProcess";
        public const string CM_PROCESS_GetStatusByProcess = "CM_PROCESS_GetStatusByProcess";

        public const string CM_PROCESS_Search = "CM_PROCESS_Search";
        public const string CM_PROCESS_Current_Search = "CM_PROCESS_Current_Search";

        public const string CM_PROCESS_DT_Create = "CM_PROCESS_DT_Create";
        public const string CM_PROCESS_DT_Insert_Is_Done = "CM_PROCESS_DT_Is_Done_Create";
        public const string CM_PROCESS_DT_Reject = "CM_PROCESS_DT_Reject";
        public const string CM_PROCESS_Reject = "CM_PROCESS_Reject";
        //khanhnhd 
        public const string CM_PROCESS_CONCENTER_Search = "CM_PROCESS_CONCENTER_Search";

        #region Store Procedure - Quản lý quy trình
        public const string CM_PROCESS_LIST_Search = "CM_PROCESS_LIST_Search";
        public const string CM_PROCESS_LIST_Ins = "CM_PROCESS_LIST_Ins";
        public const string CM_PROCESS_LIST_Upd = "CM_PROCESS_LIST_Upd"; 
        public const string CM_PROCESS_LIST_Del = "CM_PROCESS_LIST_Del";
        public const string CM_PROCESS_LIST_ById = "CM_PROCESS_LIST_ById";
        #endregion Store Procedure - Quản lý quy trình

        public const string CM_PROCESS_InsertOrUpdate = "CM_PROCESS_InsertOrUpdate";
        public const string CM_PROCESS_GetHiddenField = "CM_PROCESS_GetHiddenField";


        public const string CM_TITLE_SEARCH = "CM_TITLE_SEARCH";

        public const string PL_PROCESS_CURRENT_SEARCH = "PL_PROCESS_CURRENT_Search";
        public const string PL_PROCESS_SEARCH = "PL_PROCESS_SEARCH";
        //
        public const string MD_USER_MANAGER_Ins = "MD_USER_MANAGER_Ins";
        public const string MD_USER_MANAGER_GetByTLName = "MD_USER_MANAGER_GetByTLName";
        public const string MD_USER_MANAGER_Upd_AUTHORITYUSER = "MD_USER_MANAGER_Upd_AUTHORITYUSER";
        public const string MD_USER_MANAGER_Del_AUTHORITYUSER = "MD_USER_MANAGER_Del_AUTHORITYUSER";

        //06/12/2024 Nguyen
        public const string DB_GetVehicleData = "DB_GetVehicleData";
        public const string VehicleCategory_Del = "VehicleCategory_Del";
        public const string VehicleCategory_GetAll = "VehicleCategory_GetAll";
        public const string VehicleCategory_Ins = "VehicleCategory_Ins";
        public const string VehicleCategory_Upd = "VehicleCategory_Upd";

        public const string VehicleDashboard_Summary = "VehicleDashboard_Summary";
        public const string VehicleDetection_CategoryRatio = "VehicleDetection_CategoryRatio";
        public const string VehicleDetection_Del = "VehicleDetection_Del";
        public const string VehicleDetection_Ins = "VehicleDetection_Ins";
        public const string VehicleDetection_Upd = "VehicleDetection_Upd";
        public const string VehicleDetection_View_ByRegion = "VehicleDetection_View_ByRegion";
        public const string VehicleDetection_Ratio_ByRegion = "VehicleDetection_Ratio_ByRegion";
        public const string VehicleDetection_PeakYear_ByRegion = "VehicleDetection_PeakYear_ByRegion";

        public const string VehicleStatistic_OfRegion_AvgByYear = "VehicleStatistic_OfRegion_AvgByYear";
        public const string VehicleStatistics_Top8 = "VehicleStatistics_Top8";
        public const string VehicleStatisticsByYear_Top8 = "VehicleStatisticsByYear_Top8";
        public const string VehicleStatisticsByYear_Upd = "VehicleStatisticsByYear_Upd";

    }

}
