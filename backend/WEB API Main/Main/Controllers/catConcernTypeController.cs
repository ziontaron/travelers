using BusinessSpecificLogic.EF;
using BusinessSpecificLogic.Logic;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/catConcernType")]
    public class catConcernTypeController : BaseController<cat_ConcernType>
    {
        public catConcernTypeController(ICatConcernTypeLogic logic) : base(logic) { }
    }
}