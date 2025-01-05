using Common.gAMSPro.Process.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.gAMSPro.Intfs.Process.Dto
{
    public class CM_PROCESS_INS_UPD_ENTITY
    {
        public string PROCESS_KEY { get; set; }
        public List<CM_PROCESS_ENTITY> PROCESS_INS_UPD_ITEMS { get; set; }
        public List<CM_PROCESS_ENTITY> PROCESS_DEL_ITEMS { get; set; }
    }
}
