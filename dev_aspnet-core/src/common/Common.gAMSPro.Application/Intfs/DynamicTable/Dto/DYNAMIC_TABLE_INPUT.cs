using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.gAMSPro.Application.Intfs.DynamicTable.Dto
{
    public class DYNAMIC_TABLE_INPUT
    {
        //   [XmlIgnore]
        // public string XmlData { get; set; }
        public List<DYNAMIC_TABLE_NEW_ENTITY> Entities { get; set; }

    }

    public class DYNAMIC_TABLE_UPDATE_INPUT
    {

        // [XmlIgnore]
        // public string XmlData { get; set; }
        public List<DYNAMIC_TABLE_UPDATE_ENTITY> Entities { get; set; }
    }
}