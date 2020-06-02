using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCIM.Data.Entities
{
    public partial class SlotMaster
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string SlotName { get; set; }
        [StringLength(4000)]
        public string ImagePath { get; set; }
        public int RectangleXcoordinate { get; set; }
        public int RectangleYcoordinate { get; set; }
    }
}
