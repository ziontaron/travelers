namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InventoryEvent")]
    public partial class InventoryEvent
    {
        [Key]
        public int InventoryEventKey { get; set; }

        [Required]
        [StringLength(50)]
        public string InventoryEventName { get; set; }

        [Required]
        public string InventoryEventDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime TerminationDate { get; set; }

        public bool Status { get; set; }
    }
}
