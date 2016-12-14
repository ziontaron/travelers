namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CQAHeader")]
    public partial class CQAHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CQAHeader()
        {
            CQALines = new HashSet<CQALine>();
            sys_active = true;
        }

        [Key]
        public int CQAHeaderKey { get; set; }

        public int CustomerKey { get; set; }

        public int CQANumberKey { get; set; }

        public DateTime NotificationDate { get; set; }

        public int PartNumberKey { get; set; }

        public bool? ReoccurringIssue { get; set; }

        public int? ProductLineKey { get; set; }

        public int? ConcernTypeKey { get; set; }

        public string ConcertDescription { get; set; }

        public DateTime? FirstResponseDate { get; set; }

        public int? ResultKey { get; set; }

        public int? StatusKey { get; set; }

        [StringLength(100)]
        public string NumberConcernedParts { get; set; }

        public virtual cat_ConcernType cat_ConcernType { get; set; }

        public virtual cat_PartNumber cat_PartNumber { get; set; }

        public virtual cat_ProductLine cat_ProductLine { get; set; }

        public virtual cat_Result cat_Result { get; set; }

        public virtual cat_Status cat_Status { get; set; }

        public virtual CQANumber CQANumber { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CQALine> CQALines { get; set; }
    }
}
