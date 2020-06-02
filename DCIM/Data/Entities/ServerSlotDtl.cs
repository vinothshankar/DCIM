using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCIM.Data.Entities
{
    public partial class ServerSlotDtl
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string SlotName { get; set; }
        public int? ServerId { get; set; }
        [Column("XCoordinate")]
        public int Xcoordinate { get; set; }
        [Column("YCoordinate")]
        public int Ycoordinate { get; set; }
        [Column("RectangleXCoordinate")]
        public int RectangleXcoordinate { get; set; }
        [Column("RectangleYCoordinate")]
        public int RectangleYcoordinate { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [StringLength(50)]
        public string ImagePath { get; set; }
    }
}
