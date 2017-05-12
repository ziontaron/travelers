namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MOTagCount")]
    public partial class MOTagCount
    {
        [Key]        
        public int MOTagCountKey { get; set; }

        public int MOTagHeaderKey { get; set; }

        public int SeqNum { get; set; }

        [Required]
        [StringLength(50)]
        public string Component { get; set; }

        [Required]
        [StringLength(50)]
        public string CompDesc { get; set; }

        [Required]
        [StringLength(10)]
        public string UM { get; set; }

        public virtual MOTagHeader MOTagHeader { get; set; }
    }
}
