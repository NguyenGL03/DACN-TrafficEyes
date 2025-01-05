using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.gAMSPro.Application.Intfs.DynamicTable.Dto
{
    public class DYNAMIC_TABLE_MAP
    {
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }

        public string DATA_TYPE { get; set; }

        public string CHARACTER_MAXIMUM_LENGTH { get; set; }

        public string ACTION{ get; set; }= "none";

        public string ORIGINNAME {get; set; }
    }
    public class DYNAMIC_PROC_MAP
    {
        public string QueryData {get; set;}
    }
    public class DYNAMIC_PROC_ENTITY
    {
        public string ROUTINE_NAME { get; set; }

        public string ROUTINE_DEFINITION { get; set; }
    }
    public class DYNAMIC_TRIGGER_ENTITY
    {
        public string TRIGGER_NAME { get; set; }
    }
}