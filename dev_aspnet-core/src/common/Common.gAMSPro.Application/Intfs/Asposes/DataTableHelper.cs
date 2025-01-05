using gAMSPro.Helpers;
using GSOFTcore.gAMSPro.Report.Dto;
using System.Data;
using System.Reflection;

namespace Common.gAMSPro.Web.Controllers.Intfs.Asposes
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

        public static ReportTable ConvertDatatableToList(DataTable data)
        {
            ReportTable reportTable = new ReportTable();
            reportTable.TableName = data.TableName;

            var rows = data.Rows;
            //List<List<ReportNoteDto>> lstObj = new List<List<ReportNoteDto>>();
            int rowCount = data.Rows.Count;
            int colCount = data.Columns.Count;

            foreach (var item in data.Columns)
            {
                ReportColumn col = new ReportColumn();
                col.ColName = item.ToString();
                col.KeyName = ReportTemplateConst.OpenKey + data.TableName + "." + item.ToString() + ReportTemplateConst.CloseKey;

                reportTable.Columns.Add(col);
            }

            for (int i = 0; i < rowCount; i++)
            {
                var row = rows[i];
                List<object> rowParam = new List<object>();
                for (int j = 0; j < colCount; j++)
                {

                    //ReportNoteDto obj = new ReportNoteDto() { ReportNoteName = data.Columns[j].ToString(), Value = row[j].ToString(), ReportNoteCode = "«" + data.Columns[j].ToString() + "»" };
                    //rowParam.Add(obj);
                    if (row[j] != System.DBNull.Value)
                    {
                        rowParam.Add(row[j]);
                    }
                    else
                    {
                        rowParam.Add("");
                    }
                }
                ReportRow newRow = new ReportRow();
                newRow.Cells = rowParam;
                reportTable.Rows.Add(newRow);
            }

            return reportTable;
        }
        public static List<ReportTable> ConvertDatasetToList(DataSet data)
        {
            int i = 1;
            List<ReportTable> lstData = new List<ReportTable>();
            foreach (DataTable item in data.Tables)
            {
                item.TableName = "table" + i;
                lstData.Add(ConvertDatatableToList(item));
                i += 1;
            }
            return lstData;
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
