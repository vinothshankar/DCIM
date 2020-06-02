using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCIM.Data.Entities
{
    public partial class Server
    {
        [Key]
        public int Id { get; set; }
        public int RackId { get; set; }
        public int RackUnitId { get; set; }
        [Required]
        [Column("Server")]
        [StringLength(50)]
        public string Server1 { get; set; }
        [Required]
        [StringLength(4000)]
        public string ImagePath { get; set; }
        [Column("XCoordinate")]
        public int Xcoordinate { get; set; }
        [Column("YCoordinate")]
        public int Ycoordinate { get; set; }
        [Column("RectangleXCoordinate")]
        public int RectangleXcoordinate { get; set; }
        [Column("RectangleYCoordinate")]
        public int RectangleYcoordinate { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public int Source { get; set; }
        public int UnitSize { get; set; }
    }
}
