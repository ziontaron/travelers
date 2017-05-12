namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MOTagHeader")]
    public partial class MOTagHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MOTagHeader()
        {
            MOTagCounts = new HashSet<MOTagCount>();
        }

        [Key]
        public int MOTagHeaderKey { get; set; }

        public int TicketKey { get; set; }

        [StringLength(50)]
        public string Planner { get; set; }

        [StringLength(50)]
        public string MO { get; set; }

        [StringLength(10)]
        public string MO_Ln { get; set; }

        [StringLength(3)]
        public string MO_Status { get; set; }

        public int? Order_Qty { get; set; }

        public int? QtyRecv { get; set; }

        [StringLength(3)]
        public string LineType { get; set; }

        public int? QtyWip { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MOTagCount> MOTagCounts { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
