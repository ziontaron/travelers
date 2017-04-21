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

        [NotMapped]
        public FSCustomer Customer { get; set; }

        [NotMapped]
        public FSItem FSItem { get; set; }
    }
}
