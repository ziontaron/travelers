namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CQANumber")]
    public partial class CQANumber
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CQANumber()
        {
            CQAHeaders = new HashSet<CQAHeader>();
        }

        [Key]
        public int CQANumberKey { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(11)]
        public string GeneratedNumber { get; set; }

        [StringLength(50)]
        public string Revision { get; set; }

        public int? RevisionFrom { get; set; }

        public int? DuplicatedFrom { get; set; }

        public int Sequence { get; set; }

        public string TaskDescriptionRevisionReason { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CQAHeader> CQAHeaders { get; set; }
    }
}
