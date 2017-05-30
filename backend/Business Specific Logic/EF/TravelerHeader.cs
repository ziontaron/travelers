namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TravelerHeader")]
    public partial class TravelerHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TravelerHeader()
        {
            TravelerLines = new HashSet<TravelerLine>();
            sys_active = true;
            CreatedDate = DateTime.Now;
        }

        [Key]
        public int TravelerHeaderKey { get; set; }

        [Required]
        [StringLength(50)]
        public string PartNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string PartDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string TravelerNumber { get; set; }

        public DateTime? CreatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelerLine> TravelerLines { get; set; }
    }
}
