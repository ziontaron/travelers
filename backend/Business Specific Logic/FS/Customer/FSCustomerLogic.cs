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

        public override CommonResponse GetAll()
        {
            CommonResponse response = new CommonResponse();
            IList<FSCustomer> entities;
            try
            {
                string sql = File.ReadAllText(HostingEnvironment.MapPath("~/bin/FS/Customer/Customers.sql"));
                var dataset = context.Database.SqlQuery<FSCustomer>(sql);
                entities = dataset.ToList();
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entities);
        }
    }
}
