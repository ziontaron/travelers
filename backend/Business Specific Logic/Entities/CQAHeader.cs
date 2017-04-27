using BusinessSpecificLogic.FS.Customer;
using Reusable;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.EF
{
    public partial class CQAHeader : BaseDocument
    {
        public override int id
        {
            get
            {
                return CQAHeaderKey;
            }
        }

        public string CustomerNumber { get; set; }

        [NotMapped]
        public FSCustomer Customer { get; set; }

        [NotMapped]
        public FSItem FSItem { get; set; }

        [NotMapped]
        public string FSItem_PartValue { get; set; }

        [NotMapped]
        public string FSItem_ProductLine { get; set; }

        [NotMapped]
        public string ConcernValue { get; set; }

        [NotMapped]
        public string ResultValue { get; set; }

        [NotMapped]
        public string StatusValue { get; set; }

        [NotMapped]
        public string CustomerValue { get; set; }

        [NotMapped]
        public string CQANumberValue { get; set; }

    }
}
