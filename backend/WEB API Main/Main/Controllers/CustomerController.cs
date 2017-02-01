using BusinessSpecificLogic.EF;
using BusinessSpecificLogic.Logic;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : BaseController<Customer>
    {
        public CustomerController(ICustomerLogic logic) : base(logic) { }
    }
}