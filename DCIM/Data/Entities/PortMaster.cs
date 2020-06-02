using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCIM.Data.Entities
{
    public partial class PortMaster
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string PortName { get; set; }
        [StringLength(4000)]
        public string ImagePath { get; set; }
    }
}
