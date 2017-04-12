using Reusable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using System.Web.Hosting;

namespace BusinessSpecificLogic.FS.Customer
{
    public interface IFSCustomerLogic : IReadOnlyBaseLogic<FSCustomer> { }

    public class FSCustomerLogic : ReadOnlyBaseLogic<FSCustomer>, IFSCustomerLogic
    {
        public FSCustomerLogic(DbContext context, IReadOnlyRepository<FSCustomer> repository) : base(context, repository)
        {
        }
    }
}
