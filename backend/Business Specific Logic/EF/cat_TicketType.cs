namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_TicketType
    {
        [Key]
        public int TicketTypeKey { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketType { get; set; }

        [Required]
        [StringLength(100)]
        public string TicketTypeDescirption { get; set; }
    }
}
