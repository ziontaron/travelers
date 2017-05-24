namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TravelerLine")]
    public partial class TravelerLine
    {
        [Key]
        public int TravelerLineKey { get; set; }

        public int TravelerHeaderKey { get; set; }

        public virtual TravelerHeader TravelerHeader { get; set; }
    }
}
