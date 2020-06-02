using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCIM.Models
{
    public class ConnectionList
    {
        public int SPortId { get; set; }
        public string SPort { get; set; }
        public int SSlotId { get; set; }
        public  string SSlot { get; set; }
        public int SServerId { get; set; }
        public string SServer { get; set; }
        public int SRackId { get; set; }
        public string SRack { get; set; }

        public int DPortId { get; set; }
        public string DPort { get; set; }
        public int DSlotId { get; set; }
        public string DSlot { get; set; }
        public int DServerId { get; set; }
        public string DServer { get; set; }
        public int DRackId { get; set; }
        public string DRack { get; set; }

    }
}
