using BusinessSpecificLogic.FS.Customer;
using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
