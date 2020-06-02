using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCIM.Models
{
    public class DCIMModel
    {
        public int DRack { get; set; }
        public int DServer { get; set; }
        public int DPort { get; set; }
        public int DSlot { get; set; }
        public int SRack { get; set; }
        public int SServer { get; set; }
        public int SPort { get; set; }
        public int SSlot { get; set; }
        public string PortName { get; set; }


    }
    public class RackServerMap
    {
        //Rack Server Map { Server Id}
        public int Server { get; set; }
        public int Rack { get; set; }
        public int RackUnit { get; set; }
    }


    public class DestinationIds
    {
        public int DRackId { get; set; }
        public int DServerId { get; set; }
        public int DPortId { get; set; }
        public int DSlotId { get; set; }
    }

    public partial class ImageFile
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("locations")]
        public Pixel[] Locations { get; set; }
    }

    public partial class Pixel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("x")]
        public int X_Axis { get; set; }

        [JsonProperty("y")]
        public int Y_Axis { get; set; }

        [JsonProperty("xp")]
        public int X_Rect { get; set; }

        [JsonProperty("yp")]
        public int Y_Rect { get; set; }
    }
}
