using Reusable;

namespace BusinessSpecificLogic.FS.Customer
{
    public interface IFSCustomerLogic : IReadOnlyBaseLogic<FSCustomer> { }

    public class FSCustomerLogic : ReadOnlyBaseLogic<FSCustomer>, IFSCustomerLogic
    {
        public FSCustomerLogic(FSContext context, IReadOnlyRepository<FSCustomer> repository) : base(context, repository)
        {
        }
    }
}
