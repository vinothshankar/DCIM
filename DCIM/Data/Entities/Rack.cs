using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCIM.Data.Entities
{
    public partial class Rack
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("Rack")]
        [StringLength(50)]
        public string Rack1 { get; set; }
        [Required]
        [StringLength(50)]
        public string ImagePath { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public int Source { get; set; }
    }
}
