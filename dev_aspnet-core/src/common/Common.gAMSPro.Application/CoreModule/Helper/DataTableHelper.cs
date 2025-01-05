using gAMSPro.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Common.gAMSPro.CoreModule.Helper
{
    public static class DataTableHelper
    {
        public static List<ReportParameter> GetReportParameters(object obj)
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            PropertyInfo[] props = obj.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    parameters.Add(new ReportParameter
                    {
                        Name = props[i].Name,
                        Value = props[i].GetValue(obj, null)
                    });
                }
                catch
                {
                }
            }
            return parameters;
        }

        public static List<T> ToList<T>(this DataTable table) where T : class
        {
            List<T> result;
            try
            {
                List<T> list = new List<T>();
                EnumerableRowCollection<DataRow> collections = table.AsEnumerable();
                foreach (DataRow row in collections)
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        try
                        {
                            bool flag = row.Table.Columns.Contains(prop.Name);
                            if (flag)
                            {
                                bool flag2 = row[prop.Name] == DBNull.Value;
                                if (flag2)
                                {
                                    propertyInfo.SetValue(obj, null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(obj, row[prop.Name]);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    list.Add(obj);
                }
                result = list;
            }
            catch (Exception e2)
            {
                throw e2;
            }
            return result;
        }
    }
}