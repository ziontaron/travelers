using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ICustomerLogic : IBaseLogic<Customer> { }

    public class CustomerLogic : BaseLogic<Customer>, ICustomerLogic
    {
        public CustomerLogic(DbContext context, IRepository<Customer> repository) : base(context, repository)
        {
        }
    }

}
