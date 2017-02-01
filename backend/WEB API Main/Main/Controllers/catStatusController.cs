using BusinessSpecificLogic.EF;
using BusinessSpecificLogic.Logic;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/catStatus")]
    public class catStatusController : BaseController<cat_Status>
    {
        public catStatusController(ICatStatusLogic logic) : base(logic) { }
    }
}