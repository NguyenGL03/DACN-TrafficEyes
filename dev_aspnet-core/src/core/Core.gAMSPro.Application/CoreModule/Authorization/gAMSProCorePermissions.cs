namespace Common.gAMSPro.Core.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class gAMSProCorePermissions
    {
        public const string Pages = "gAMSProCorePermissions.Pages";
        public class Prefix
        {
            public const string Administration = "Pages.Administration";
            public const string Main = "Pages.Main";
        }
        public class Page
        {
            public const string Branch = "Branch";
            public const string Menu = "Menu";
            public const string Department = "Department";
            public const string Region = "Region";
            public const string DeptGroup = "DeptGroup";
            public const string SupplierType = "SupplierType";
            public const string Supplier = "Supplier";
            public const string GoodsType = "GoodsType";
            public const string GoodsTypeReal = "GoodsTypeReal";
            public const string Unit = "Unit";
            public const string Goods = "Goods";
            public const string Division = "Division";
            public const string Employee = "Employee";
            public const string InsuCompany = "InsuCompany";
            public const string Model = "Model";
            public const string CarType = "CarType";
            public const string AllCode = "AllCode";
            public const string SysParameters = "SysParameter";
            public const string Workflow = "Workflow";
            public const string ReportTemplate = "ReportTemplate";
            public const string TypeTemplate = "TypeTemplate";
            public const string TlUser = "TlUser";
            public const string AccountList = "AccountList";
            public const string AccountKT = "AccountKT";
            public const string SecurInfo = "SecurInfo";
            public const string CommonTerm = "CommonTerm";
            public const string DeptBlockMap = "DeptBlockMap";
            public const string PositionBlock = "PositionBlock";

            public const string HangHoa = "HangHoa";
            public const string HangHoaType = "HangHoaType";
            public const string HangHoaGroup = "HangHoaGroup";

            public const string RoleLimit = "RoleLimit";
            public const string SysGroupLimit = "SysGroupLimit";
            public const string SysGroupLimitDT = "SysGroupLimitDT"; 
            public const string SysNotifMaster = "SysNotifMaster";  
            public const string TlSYSRoleMapping = "TlSYSRoleMapping";
            public const string CMMappingChucDanhRole = "CMMappingChucDanhRole";
            public const string PlanType = "PlanType";
             
            public const string CarDevice = "CarDevice";
            public const string RequestCar = "RequestCar";
             
            public const string CallSoapServices = "CallSoapServices";
        }
        public class Action
        {
            public const string Create = "Create";
            public const string Edit = "Edit";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string View = "View";
            public const string Approve = "Approve";
            public const string Search = "Search";
            public const string Reject = "Reject";
        }
    }
}