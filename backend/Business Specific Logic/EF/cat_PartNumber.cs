namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_PartNumber
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_PartNumber()
        {
            CQAHeaders = new HashSet<CQAHeader>();
        }

        [Key]
        public int PartNumberKey { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(200)]
        public string PartDescription { get; set; }

        public int? ProductLineKey { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CQAHeader> CQAHeaders { get; set; }
    }
}
