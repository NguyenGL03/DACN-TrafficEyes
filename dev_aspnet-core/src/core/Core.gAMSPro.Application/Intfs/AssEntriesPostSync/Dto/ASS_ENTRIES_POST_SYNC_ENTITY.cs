using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.gAMSPro.Intfs.AssEntriesPostSync.Dto
{
    public class ASS_ENTRIES_POST_SYNC_ENTITY
    {
        public string tranRef { get; set; }
        public string makerId { get; set; }
        public string checkerId { get; set; }
        public string tranParticular { get; set; }
        public string solId { get; set; }
        public List<PTR_ENTITY> pTR { get; set; }
    }
}
