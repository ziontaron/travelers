using BusinessSpecificLogic.FS.Customer;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/FSItem")]
    public class FSItemController : ReadOnlyBaseController<FSItem>
    {
        public FSItemController(IFSItemLogic logic) : base(logic) { }
    }
}