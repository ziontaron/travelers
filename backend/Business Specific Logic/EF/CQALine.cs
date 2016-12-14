namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CQALine")]
    public partial class CQALine
    {
        [Key]
        public int CQALinelKey { get; set; }

        public int CQAHeaderKey { get; set; }

        [Required]
        public string OngoingActivities { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        [StringLength(100)]
        public string Champion { get; set; }

        public virtual CQAHeader CQAHeader { get; set; }
    }
}
