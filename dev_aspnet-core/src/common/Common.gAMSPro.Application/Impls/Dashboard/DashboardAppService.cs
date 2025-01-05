using Abp.Authorization;
using Common.gAMSPro.Application.Intfs.Dashboards;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.Dashboards.Dto;
using Core.gAMSPro.Application;
using System;
using Core.gAMSPro.CoreModule.Utils;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.gAMSPro.Application.Intfs.Dashboards.Dto;
using gAMSPro.ProcedureHelpers;

namespace Common.gAMSPro.Application.Impls.Dashboard
{
    [AbpAuthorize]
    public class DashboardAppService: gAMSProCoreAppServiceBase, IDashboardAppService
    {
        #region DASHBOARD 01
        public async Task<Dictionary<string, object>> DB_STATUS_ASSET_VALUE_BAR(DashboardParent<DB_STATUS_ASSET_BAR> input)
        {
            try
            {
                //input.XML = XmlHelper.ToXmlFromList(input.DASHBOARD_CHILDEN);
                input.CREATE_DT = DateTime.Now;

                DataSet result = await storeProcedureProvider.GetMultiSelect(CommonStoreProcedureConsts.DB_STATUS_ASSET_VALUE_BAR, new
                {
                    USER_LOGIN = input.USER_LOGIN,
                    TYPE_ID = input.DASHBOARD_CHILDEN[0].TYPE_ID,
                    GROUP_ID = input.DASHBOARD_CHILDEN[0].GROUP_ID,
                    BRANCH_ID = input.BRANCH_ID,
                    FROM_DATE = input.DASHBOARD_CHILDEN[0].FROM_DATE,
                    TO_DATE = input.DASHBOARD_CHILDEN[0].TO_DATE,
                    USE_DATE_KT_CHECK = input.DASHBOARD_CHILDEN[0].USE_DATE_KT_CHECK,
                    AMORT_DATE_CHECK = input.DASHBOARD_CHILDEN[0].AMORT_DATE_CHECK,
                    FILTER = input.DASHBOARD_CHILDEN[0].FILTER
                });

                // only one array
                List<object> listItem1 = new List<object>();
                List<object> listItem2 = new List<object>();
                if (result.Tables[0].Rows.Count > 0)
                {
                    int i = 0;

                    foreach (DataRow item in result.Tables[0].Rows)
                    {

                        Object temp = new
                        {
                            YEAR = item.ItemArray[i].ToString(),
                            BUY_PRICE = item.ItemArray[++i]
                        };

                        listItem1.Add(temp);
                        i = 0;
                    }

                    i = 0;

                    foreach (DataRow item in result.Tables[1].Rows)
                    {

                        Object temp = new
                        {
                            BUY_DATE_KT = item.ItemArray[i].ToString(),
                            BUY_PRICE = item.ItemArray[++i].ToString(),
                            YEAR = item.ItemArray[++i].ToString()
                        };

                        listItem2.Add(temp);
                        i = 0;
                    }
                }

                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "Result", "0" },
                    { "Table1", listItem1 },
                    { "Table2", listItem2 }
                };
                return values;
            }
            catch (Exception e)
            {
                Dictionary<string, object> error = new Dictionary<string, object>();

                error.Add("Result", "-1");
                error.Add("ErrorDesc", e.Message);

                return error;
            }
        }
        public async Task<Dictionary<string, object>> DB_STATUS_ASSET_QUANTITY_BAR(DashboardParent<DB_STATUS_ASSET_BAR> input)
        {
            try
            {
                input.XML = XmlHelper.ToXmlFromList(input.DASHBOARD_CHILDEN);
                input.CREATE_DT = DateTime.Now;

                DataSet result = await storeProcedureProvider.GetMultiSelect(CommonStoreProcedureConsts.DB_STATUS_ASSET_QUANTITY_BAR, new
                {
                    USER_LOGIN = input.USER_LOGIN,
                    TYPE_ID = input.DASHBOARD_CHILDEN[0].TYPE_ID,
                    GROUP_ID = input.DASHBOARD_CHILDEN[0].GROUP_ID,
                    BRANCH_ID = input.BRANCH_ID,
                    FROM_DATE = input.DASHBOARD_CHILDEN[0].FROM_DATE,
                    TO_DATE = input.DASHBOARD_CHILDEN[0].TO_DATE,
                    USE_DATE_KT_CHECK = input.DASHBOARD_CHILDEN[0].USE_DATE_KT_CHECK,
                    AMORT_DATE_CHECK = input.DASHBOARD_CHILDEN[0].AMORT_DATE_CHECK,
                    FILTER = input.DASHBOARD_CHILDEN[0].FILTER
                });

                // only one array
                List<object> listItem1 = new List<object>();
                List<object> listItem2 = new List<object>();
                int i = 0;
                if (result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in result.Tables[0].Rows)
                    {

                        Object temp = new
                        {
                            YEAR = item.ItemArray[i].ToString(),
                            TOTAL_COUNT = item.ItemArray[++i]
                        };

                        listItem1.Add(temp);
                        i = 0;
                    }

                    i = 0;

                    foreach (DataRow item in result.Tables[1].Rows)
                    {

                        Object temp = new
                        {
                            BUY_DATE_KT = item.ItemArray[i].ToString(),
                            TOTAL_COUNT = item.ItemArray[++i].ToString(),
                            YEAR = item.ItemArray[++i].ToString()
                        };

                        listItem2.Add(temp);
                        i = 0;
                    }

                }

                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "Result", "0" },
                    { "Table1", listItem1 },
                    { "Table2", listItem2 }
                };
                return values;
            }
            catch (Exception e)
            {
                Dictionary<string, object> error = new Dictionary<string, object>();

                error.Add("Result", "-1");
                error.Add("ErrorDesc", e.Message);

                return error;
            }
        }
        public async Task<List<DB_STATUS_ASSET_PIE>> DB_STATUS_ASSET_STATUS_PIE(DashboardParent<DB_STATUS_ASSET_PIE> input)
        {
            var user = GetCurrentUser();
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_STATUS_ASSET_PIE>(CommonStoreProcedureConsts.DB_STATUS_ASSET_STATUS_PIE, new
            {
                BRANCH_ID = input.BRANCH_ID,
                YEAR = input.DASHBOARD_CHILDEN[0].YEAR,
                USER_LOGIN = user.UserName,

            });
        }
        public async Task<List<DB_STATUS_ASSET_PIE>> DB_STATUS_ASSET_QUANTITY_PIE(DashboardParent<DB_STATUS_ASSET_PIE> input)
        {
            var user = GetCurrentUser();
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_STATUS_ASSET_PIE>(CommonStoreProcedureConsts.DB_STATUS_ASSET_QUANTITY_PIE, new
            {
                BRANCH_ID = input.BRANCH_ID,
                YEAR = input.DASHBOARD_CHILDEN[0].YEAR,
                USER_LOGIN = user.UserName,
            });
        }
        #endregion

        #region DASHBOARD 02
        public async Task<List<DB_ASSET_PIE>> DB_AMORT_ASSET_PIE(DashboardParent<DB_ASSET_PIE> input)
        {
            var ok = await storeProcedureProvider.GetDataFromStoredProcedure<DB_ASSET_PIE>(CommonStoreProcedureConsts.DB_AMORT_ASSET_PIE, new
            {
                USER_LOGIN = input.USER_LOGIN,
                BRANCH_ID = input.BRANCH_ID,
                YEAR = input.DASHBOARD_CHILDEN[0].YEAR,
                TYPE_ID = input.DASHBOARD_CHILDEN[0].TYPE_ID,
                GROUP_ID = input.DASHBOARD_CHILDEN[0].GROUP_ID
            });
            return ok;
        }
        public async Task<List<DB_ASSET_PIE>> DB_VALUE_ASSET_PIE(DashboardParent<DB_ASSET_PIE> input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_ASSET_PIE>(CommonStoreProcedureConsts.DB_VALUE_ASSET_PIE, new
            {
                USER_LOGIN = input.USER_LOGIN,
                BRANCH_ID = input.BRANCH_ID,
                YEAR = input.DASHBOARD_CHILDEN[0].YEAR,
                TYPE_ID = input.DASHBOARD_CHILDEN[0].TYPE_ID,
                GROUP_ID = input.DASHBOARD_CHILDEN[0].GROUP_ID
            });
        }
        #endregion

        #region DASHBOARD 03
        public async Task<Dictionary<string, object>> DB_BUDGET_BAR(DashboardParent<DB_BUGET> input)
        {
            try
            {
                //input.XML = XmlHelper.ToXmlFromList(input.DASHBOARD_CHILDEN);
                input.CREATE_DT = DateTime.Now;

                DataSet result = await storeProcedureProvider.GetMultiSelect(CommonStoreProcedureConsts.DB_BUDGET_BAR, new
                {
                    USER_LOGIN = input.USER_LOGIN,
                    YEAR = input.DASHBOARD_CHILDEN[0].YEAR,
                    DATE = input.DASHBOARD_CHILDEN[0].DATE,
                    GD_ID = input.DASHBOARD_CHILDEN[0].GD_ID,
                    PLAN_TYPE_ID = input.DASHBOARD_CHILDEN[0].PLAN_TYPE_ID,
                    GD_TYPE_ID = input.DASHBOARD_CHILDEN[0].GD_TYPE_ID,
                    BRANCH_ID = input.BRANCH_ID,
                    FILTER = input.DASHBOARD_CHILDEN[0].FILTER
                });

                // only one array
                List<object> listItem1 = new List<object>();
                List<object> listItem2 = new List<object>();
                if (result.Tables[0].Rows.Count > 0)
                {
                    int i = 0;

                    foreach (DataRow item in result.Tables[0].Rows)
                    {

                        Object temp = new
                        {
                            YEAR = item.ItemArray[i].ToString(),
                            PLAN = item.ItemArray[++i]
                        };

                        listItem1.Add(temp);
                        i = 0;
                    }

                    i = 0;

                    foreach (DataRow item in result.Tables[1].Rows)
                    {

                        Object temp = new
                        {
                            YEAR = item.ItemArray[i].ToString(),
                            MONTH = item.ItemArray[++i].ToString(),
                            MADE = item.ItemArray[++i].ToString(),
                            DOING = item.ItemArray[++i].ToString(),
                            RESIDUAL = item.ItemArray[++i].ToString(),
                        };

                        listItem2.Add(temp);
                        i = 0;
                    }
                }

                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "Result", "0" },
                    { "Table1", listItem1 },
                    { "Table2", listItem2 }
                };
                return values;
            }
            catch (Exception e)
            {
                Dictionary<string, object> error = new Dictionary<string, object>();

                error.Add("Result", "-1");
                error.Add("ErrorDesc", e.Message);

                return error;
            }
        }
        public async Task<List<DB_BUGET>> DB_BUDGET_PIE(DashboardParent<DB_BUGET> input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_BUGET>(CommonStoreProcedureConsts.DB_BUDGET_PIE, new
            {
                USER_LOGIN = input.USER_LOGIN,
                YEAR = input.DASHBOARD_CHILDEN[0].YEAR,
                DATE = input.DASHBOARD_CHILDEN[0].DATE,
                GD_ID = input.DASHBOARD_CHILDEN[0].GD_ID,
                PLAN_TYPE_ID = input.DASHBOARD_CHILDEN[0].PLAN_TYPE_ID,
                GD_TYPE_ID = input.DASHBOARD_CHILDEN[0].GD_TYPE_ID,
                BRANCH_ID = input.BRANCH_ID,
                FILTER = input.DASHBOARD_CHILDEN[0].FILTER
            });
        }
        #endregion

        #region DASHBOARD 04
        public async Task<Dictionary<string, object>> DB_BUY_VALUE_BAR(DashboardParent<DB_BUY_VALUE> input)
        {
            try
            {
                //input.XML = XmlHelper.ToXmlFromList(input.DASHBOARD_CHILDEN);
                input.CREATE_DT = DateTime.Now;

                DataSet result = await storeProcedureProvider.GetMultiSelect(CommonStoreProcedureConsts.DB_BUY_VALUE_BAR, new
                {
                    USER_LOGIN = input.USER_LOGIN,
                    FILTER = input.DASHBOARD_CHILDEN[0].FILTER,
                    FROM_YEAR = input.DASHBOARD_CHILDEN[0].FROM_YEAR,
                    TO_YEAR = input.DASHBOARD_CHILDEN[0].TO_YEAR
                });

                // only one array
                int i = 0;
                List<object> listItem1 = new List<object>();
                foreach (DataRow item in result.Tables[0].Rows)
                {

                    Object temp = new
                    {
                        YEAR = item.ItemArray[i].ToString(),
                        SUMARY_BUY_RECEIVE = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_MADE = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_DOING = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_REAL = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_SAVE = item.ItemArray[++i].ToString(),
                    };

                    listItem1.Add(temp);
                    i = 0;
                }

                i = 0;
                List<object> listItem2 = new List<object>();
                foreach (DataRow item in result.Tables[1].Rows)
                {

                    Object temp = new
                    {
                        YEAR = item.ItemArray[i].ToString(),
                        MONTH = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_RECEIVE = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_MADE = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_DOING = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_REAL = item.ItemArray[++i].ToString(),
                        SUMARY_BUY_SAVE = item.ItemArray[++i].ToString(),
                    };

                    listItem2.Add(temp);
                    i = 0;
                }

                Dictionary<string, object> values = new Dictionary<string, object>()
                {
                    { "Result", "0" },
                    { "Table1", listItem1 },
                    { "Table2", listItem2 }
                };
                return values;
            }
            catch (Exception e)
            {
                Dictionary<string, object> error = new Dictionary<string, object>();

                error.Add("Result", "-1");
                error.Add("ErrorDesc", e.Message);

                return error;
            }
        }
        public async Task<List<DB_BUY_VALUE>> DB_BUY_VALUE_PLAN_PIE(DashboardParent<DB_BUY_VALUE> input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_BUY_VALUE>(CommonStoreProcedureConsts.DB_BUY_VALUE_PLAN_PIE, new
            {
                BRANCH_ID = input.BRANCH_ID,
                USER_LOGIN = input.USER_LOGIN,
                FROM_YEAR = input.DASHBOARD_CHILDEN[0].FROM_YEAR,
                TO_YEAR = input.DASHBOARD_CHILDEN[0].TO_YEAR
            }); ;
        }
        #endregion

        #region Nguyen
        public async Task<List<DB_VEHICLE_ENTITY>> GetVehicleDataAsync(string yearInput)
        {
            //if (yearInput == null)
            //{
            //    yearInput = "2024";
            //}
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_VEHICLE_ENTITY>(CommonStoreProcedureConsts.DB_GetVehicleData, new
            {
                Year = yearInput
            });
            
        }

        public async Task<List<DB_VEHICLE_ENTITY>> VehicleStatisticsByYear_Top8(string yearInput)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_VEHICLE_ENTITY>(CommonStoreProcedureConsts.VehicleStatisticsByYear_Top8, new
            {
                StatisticYear = yearInput
            });

        }

        public async Task<List<DB_VEHICLE_ENTITY>> VehicleStatistics_Top8()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_VEHICLE_ENTITY>(CommonStoreProcedureConsts.VehicleStatistics_Top8, null);

        }
        public async Task<List<DB_VEHICLE_ENTITY>> VehicleDashboard_Summary()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_VEHICLE_ENTITY>(CommonStoreProcedureConsts.VehicleDashboard_Summary, null);

        }
        public async Task<List<DB_VEHICLE_ENTITY>> VehicleDetection_CategoryRatio()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DB_VEHICLE_ENTITY>(CommonStoreProcedureConsts.VehicleDetection_CategoryRatio, null);

        }


        #endregion

    }
}
