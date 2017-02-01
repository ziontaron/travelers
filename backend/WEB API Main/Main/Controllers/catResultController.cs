using BusinessSpecificLogic.EF;
using BusinessSpecificLogic.Logic;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/catResult")]
    public class catResultController : BaseController<cat_Result>
    {
        public catResultController(ICatResultLogic logic) : base(logic) { }
    }
}