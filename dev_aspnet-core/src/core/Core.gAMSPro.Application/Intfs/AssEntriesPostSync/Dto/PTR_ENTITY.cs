using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.gAMSPro.Intfs.AssEntriesPostSync.Dto
{
    public class PTR_ENTITY
    {
        public string acctId { get; set; }
        public string crDrFlag { get; set; }
        public string tranAmt { get; set; }
        public string tranCrncy { get; set; }
        public string oTrdt { get; set; }
        public string oTrId { get; set; }
        public string oSrn { get; set; }
    }
}
