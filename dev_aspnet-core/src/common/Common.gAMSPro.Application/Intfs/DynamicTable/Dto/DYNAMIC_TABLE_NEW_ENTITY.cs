using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.gAMSPro.Application.Intfs.DynamicTable.Dto
{
    [XmlType("XmlData")]
    public class DYNAMIC_TABLE_NEW_ENTITY
    {
      
        public string TableName { get; set; }
    

        public string Columns { get; set; }

    }
    [XmlType("XmlData")]
    public class DYNAMIC_TABLE_UPDATE_ENTITY
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string Type { get; set; }

        public string ValueType { get; set; }

        public string Action { get; set; }

        public string OriginName { get; set; }
    }
}