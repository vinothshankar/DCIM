using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCIM.Data.Entities
{
    public partial class RackUnit
    {
        [Key]
        public int Id { get; set; }
        public int RackId { get; set; }
        public int FromUnit { get; set; }
        public int ToUnit { get; set; }
        [Column("XCoordinate")]
        public int Xcoordinate { get; set; }
        [Column("YCoordinate")]
        public int Ycoordinate { get; set; }
        [Column("RectangleXCoordinate")]
        public int RectangleXcoordinate { get; set; }
        [Column("RectangleYCoordinate")]
        public int RectangleYcoordinate { get; set; }
    }
}
