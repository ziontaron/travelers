using BusinessSpecificLogic.FS.Customer;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/FSCustomer")]
    public class FSCustomerController : ReadOnlyBaseController<FSCustomer>
    {
        public FSCustomerController(IFSCustomerLogic logic) : base(logic) { }
    }
}