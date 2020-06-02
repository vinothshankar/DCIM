using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCIM.Data.Entities
{
    public partial class Connections
    {
        [Key]
        public int Id { get; set; }
        public int SourceRackId { get; set; }
        public int SourceServerId { get; set; }
        public int SourceSlotId { get; set; }
        public int SourcePortId { get; set; }
        public int DestinationRackId { get; set; }
        public int DestinationServerId { get; set; }
        public int DestinationSlotId { get; set; }
        public int DestinationPortId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
    }
}
